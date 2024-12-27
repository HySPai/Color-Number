/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PositScrollRect : ScrollRect
{
	public float Speed = 5000f;

	public float DistForScale = 100f;

	public float Scale = 1f;

	public bool DynamicSize;

	public Action OnDragStart;

	public Action<int> OnSelectIndex;

	public Action<int> OnDragSelectIndex;

	public Action<int, float, float> OnScaleElement;

	public Action<int> OnSlideToIndex;

	public Action OnSwipe;

	public Action OnNavigationButtonClick;

	private RectTransform m_rectTransform;

	private List<RectTransform> m_list;

	private int m_currentElement;

	private HorizontalOrVerticalLayoutGroup m_contentGroup;

	private bool m_drag;

	private int m_pointerId = -1;

	private Canvas m_canvas;

	private float m_screenWidth;

	private float m_rectWidth;

	private float m_rectHeight;

	private float m_startDragPos;

	public bool Moving
	{
		get
		{
			return this.m_drag;
		}
	}

	public bool Inverted { get; set; }

	public bool NextAvailable
	{
		get
		{
			return this.m_currentElement < this.m_list.Count - 1;
		}
	}

	public bool PrevAvailable
	{
		get
		{
			return this.m_currentElement > 0;
		}
	}

	public void Reinit(float screenWidth, bool withoutPosCorrection = false)
	{
		this.m_list = new List<RectTransform>();
		this.m_drag = false;
		int childCount = base.content.childCount;
		for (int i = 0; i < childCount; i++)
		{
			RectTransform rectTransform = base.content.GetChild(i) as RectTransform;
			if (rectTransform.gameObject.activeSelf)
			{
				this.m_list.Add(rectTransform);
			}
		}
		this.m_contentGroup = ((Component)base.content).GetComponent<HorizontalOrVerticalLayoutGroup>();
		this.m_rectTransform = (base.transform as RectTransform);
		this.m_canvas = this.FindParentCanvas();
		if (base.horizontal)
		{
			this.m_screenWidth = (this.m_canvas.transform as RectTransform).rect.width;
		}
		else if (base.vertical)
		{
			this.m_screenWidth = (this.m_canvas.transform as RectTransform).rect.height;
		}
		this.m_rectWidth = this.m_rectTransform.rect.width;
		this.m_rectHeight = this.m_rectTransform.rect.height;
		this.SetContentSize();
		if (!withoutPosCorrection)
		{
			RectTransform content = base.content;
			Vector2 anchoredPosition = content.anchoredPosition;
			Vector2 anchoredPosition2 = base.content.anchoredPosition;
			content.anchoredPosition = anchoredPosition + new Vector2(0f - anchoredPosition2.x, 0f);
		}
		this.UpdateImageIndex();
		this.UpdateScale(true);
	}

	private Canvas FindParentCanvas()
	{
		Transform transform = this.m_rectTransform; 
		while (true)
		{
			transform = transform.parent;
			if (transform != null)
			{
				Canvas component = ((Component)transform).GetComponent<Canvas>();
				if (!(component != null))
				{
					continue;
				}
				this.m_canvas = component;
			}
			break;
		}
		return this.m_canvas; 
	}

	public void SetContentSize()
	{
		if (base.horizontal)
		{
			float num = 0f;
			foreach (RectTransform item in this.m_list)
			{
				num += item.rect.width;
			}
			num += (float)(this.m_list.Count - 1) * this.m_contentGroup.spacing;
			RectTransform content = base.content;
			float x = num;
			Vector2 sizeDelta = base.content.sizeDelta;
			content.sizeDelta = new Vector2(x, sizeDelta.y);
		}
		else if (base.vertical)
		{
			float num2 = 0f;
			foreach (RectTransform item2 in this.m_list)
			{
				num2 += item2.rect.height;
			}
			num2 += (float)(this.m_list.Count - 1) * this.m_contentGroup.spacing;
			RectTransform content2 = base.content;
			Vector2 sizeDelta2 = base.content.sizeDelta;
			content2.sizeDelta = new Vector2(sizeDelta2.x, num2);
		}
		if (base.horizontal)
		{
			base.enabled = true;
		}
		else if (base.vertical)
		{
			base.enabled = true;
		}
	}

	public void PrevButtonClick(bool force = false)
	{
		if (this.m_drag && !force)
		{
			return;
		}
		if (base.horizontal)
		{
			if (this.m_currentElement > 0)
			{
				base.StartCoroutine(this.MoveCoroutine(this.m_list[this.m_currentElement - 1].rect.width + this.m_contentGroup.spacing));
				this.OnDragStart.SafeInvoke();
			}
		}
		else if (base.vertical && this.m_currentElement > 0)
		{
			base.StartCoroutine(this.MoveCoroutine(0f - (this.m_list[this.m_currentElement - 1].rect.height + this.m_contentGroup.spacing)));
			this.OnDragStart.SafeInvoke();
		}
		this.OnNavigationButtonClick.SafeInvoke();
	}

	public void NextButtonClick(bool force = false)
	{
		if (this.m_drag && !force)
		{
			return;
		}
		if (base.horizontal)
		{
			Vector2 anchoredPosition = base.content.anchoredPosition;
			if (anchoredPosition.x - this.m_rectTransform.rect.width - 5f > (0f - this.m_list[this.m_currentElement].rect.width) * (float)this.m_list.Count - this.m_contentGroup.spacing * (float)(this.m_list.Count - 1))
			{
				base.StartCoroutine(this.MoveCoroutine(0f - (this.m_list[this.m_currentElement].rect.width + this.m_contentGroup.spacing)));
				this.OnDragStart.SafeInvoke();
			}
		}
		else if (base.vertical)
		{
			Vector2 anchoredPosition2 = base.content.anchoredPosition;
			if (0f - anchoredPosition2.y - this.m_rectHeight - 5f > (0f - this.m_list[this.m_currentElement].rect.height) * (float)this.m_list.Count - this.m_contentGroup.spacing * (float)(this.m_list.Count - 1))
			{
				base.StartCoroutine(this.MoveCoroutine(this.m_list[this.m_currentElement].rect.height + this.m_contentGroup.spacing));
				this.OnDragStart.SafeInvoke();
			}
		}
		this.OnNavigationButtonClick.SafeInvoke();
	}

	public override void OnBeginDrag(PointerEventData data)
	{
		if (!this.m_drag)
		{
			if (base.horizontal)
			{
				Vector3 localPosition = base.content.localPosition;
				this.m_startDragPos = localPosition.x;
				Vector2 vector = data.delta / (float)Camera.main.pixelWidth * this.m_screenWidth;
				if (this.Inverted)
				{
					vector.x *= -1f;
				}
				RectTransform content = base.content;
				content.localPosition += new Vector3(vector.x, 0f, 0f);
				this.m_drag = true;
			}
			else if (base.vertical)
			{
				Vector3 localPosition2 = base.content.localPosition;
				this.m_startDragPos = localPosition2.y;
				Vector2 vector2 = data.delta / (float)Camera.main.pixelWidth * this.m_screenWidth;
				if (this.Inverted)
				{
					vector2.y *= -1f;
				}
				RectTransform content2 = base.content;
				content2.localPosition += new Vector3(0f, vector2.y, 0f);
				this.m_drag = true;
			}
			this.m_pointerId = data.pointerId;
			this.OnDragStart.SafeInvoke();
		}
	}

	public override void OnDrag(PointerEventData data)
	{
		if (data.pointerId == this.m_pointerId)
		{
			if (base.horizontal)
			{
				Vector2 vector = data.delta / (float)Camera.main.pixelWidth * this.m_screenWidth;
				if (this.Inverted)
				{
					vector.x *= -1f;
				}
				RectTransform content = base.content;
				content.localPosition += new Vector3(vector.x, 0f, 0f);
				this.UpdateScale(false);
			}
			else if (base.vertical)
			{
				Vector2 vector2 = data.delta / (float)Camera.main.pixelWidth * this.m_screenWidth;
				if (this.Inverted)
				{
					vector2.y *= -1f;
				}
				RectTransform content2 = base.content;
				content2.localPosition += new Vector3(0f, vector2.y, 0f);
				this.UpdateScale(false);
			}
		}
	}

	public override void OnEndDrag(PointerEventData data)
	{
		if (data.pointerId == this.m_pointerId)
		{
			if (base.horizontal)
			{
				if (this.DynamicSize)
				{
					this.MoveToIndex(this.m_currentElement);
					return;
				}
				Vector2 vector = data.delta / (float)Camera.main.pixelWidth * this.m_screenWidth;
				if (this.Inverted)
				{
					vector.x *= -1f;
				}
				RectTransform content = base.content;
				content.localPosition += new Vector3(vector.x, 0f, 0f);
				Vector3 localPosition = base.content.localPosition;
				float num = localPosition.x - this.m_startDragPos;
				if (num > 0f)
				{
					if (num < 100f)
					{
						base.StartCoroutine(this.MoveCoroutine(0f - num));
					}
					else
					{
						Vector2 anchoredPosition = base.content.anchoredPosition;
						if (anchoredPosition.x > 0f)
						{
							Vector2 anchoredPosition2 = base.content.anchoredPosition;
							base.StartCoroutine(this.MoveCoroutine(0f - anchoredPosition2.x));
						}
						else
						{
							int b = Mathf.RoundToInt(num / (this.m_list[this.m_currentElement].rect.width + this.m_contentGroup.spacing));
							base.StartCoroutine(this.MoveCoroutine((this.m_list[0].rect.width + this.m_contentGroup.spacing) * (float)Mathf.Max(1, b) - num));
						}
					}
				}
				else if (num > -100f)
				{
					base.StartCoroutine(this.MoveCoroutine(0f - num));
				}
				else
				{
					Vector2 anchoredPosition3 = base.content.anchoredPosition;
					if (anchoredPosition3.x - this.m_rectTransform.rect.width < (0f - this.m_list[0].rect.width) * (float)this.m_list.Count - this.m_contentGroup.spacing * (float)(this.m_list.Count - 1))
					{
						float num2 = (0f - (this.m_list[0].rect.width + this.m_contentGroup.spacing)) * (float)(this.m_list.Count - 1);
						Vector2 anchoredPosition4 = base.content.anchoredPosition;
						base.StartCoroutine(this.MoveCoroutine(num2 - anchoredPosition4.x));
					}
					else
					{
						int b2 = Mathf.RoundToInt((0f - num) / (this.m_list[this.m_currentElement].rect.width + this.m_contentGroup.spacing));
						base.StartCoroutine(this.MoveCoroutine((0f - (this.m_list[this.m_currentElement].rect.width + this.m_contentGroup.spacing)) * (float)Mathf.Max(1, b2) - num));
					}
				}
			}
			else if (base.vertical)
			{
				Vector2 vector2 = data.delta / (float)Camera.main.pixelWidth * this.m_screenWidth;
				RectTransform content2 = base.content;
				content2.localPosition += new Vector3(0f, vector2.y, 0f);
				Vector3 localPosition2 = base.content.localPosition;
				float num3 = localPosition2.y - this.m_startDragPos;
				if (num3 > 0f)
				{
					if (num3 < 100f)
					{
						base.StartCoroutine(this.MoveCoroutine(0f - num3));
					}
					else
					{
						Vector2 anchoredPosition5 = base.content.anchoredPosition;
						if (anchoredPosition5.y + this.m_rectHeight > this.m_list[this.m_currentElement].rect.height * (float)this.m_list.Count + this.m_contentGroup.spacing * (float)(this.m_list.Count - 1))
						{
							float num4 = base.content.rect.height - this.m_rectHeight;
							Vector2 anchoredPosition6 = base.content.anchoredPosition;
							base.StartCoroutine(this.MoveCoroutine(num4 - anchoredPosition6.y));
						}
						else
						{
							base.StartCoroutine(this.MoveCoroutine(this.m_list[this.m_currentElement].rect.height + this.m_contentGroup.spacing - num3));
						}
					}
				}
				else if (num3 > -100f)
				{
					base.StartCoroutine(this.MoveCoroutine(0f - num3));
				}
				else
				{
					Vector2 anchoredPosition7 = base.content.anchoredPosition;
					if (anchoredPosition7.y - this.m_rectHeight < 0f)
					{
						Vector2 anchoredPosition8 = base.content.anchoredPosition;
						base.StartCoroutine(this.MoveCoroutine(0f - anchoredPosition8.y));
					}
					else
					{
						base.StartCoroutine(this.MoveCoroutine(0f - (this.m_list[this.m_currentElement].rect.height + this.m_contentGroup.spacing) - num3));
					}
				}
			}
			this.OnSwipe.SafeInvoke();
		}
	}

	public void SetIndex(int index)
	{
		if (this.m_list.Count > index)
		{
			if (this.DynamicSize)
			{
				this.MoveToIndex(index);
			}
			else
			{
				if (base.horizontal)
				{
					RectTransform content = base.content;
					float x = (0f - (this.m_list[0].rect.width + this.m_contentGroup.spacing)) * (float)index;
					Vector2 anchoredPosition = base.content.anchoredPosition;
					content.anchoredPosition = new Vector2(x, anchoredPosition.y);
				}
				else if (base.vertical)
				{
					RectTransform content2 = base.content;
					Vector2 anchoredPosition2 = base.content.anchoredPosition;
					content2.anchoredPosition = new Vector2(anchoredPosition2.x, (this.m_list[0].rect.height + this.m_contentGroup.spacing) * (float)index);
				}
				this.UpdateImageIndex();
				this.UpdateScale(true);
			}
		}
	}

	public void MoveToIndex(int index)
	{
		if (this.m_list.Count > index)
		{
			if (base.horizontal)
			{
				base.StartCoroutine(this.MoveToIndexCoroutine(index));
			}
			else if (!base.vertical)
			{ 
            }
            this.UpdateScale(true);
		}
		return; 
	}

	private void UpdateImageIndex()
	{
		if (this.m_list.Count == 0)
		{
			this.m_currentElement = -1;
		}
		else if (base.horizontal)
		{
			if (!this.DynamicSize)
			{
				Vector2 anchoredPosition = base.content.anchoredPosition;
				this.m_currentElement = Mathf.RoundToInt((0f - anchoredPosition.x) / (this.m_list[0].rect.width + this.m_contentGroup.spacing));
			}
			else
			{
				float num = (0f - this.m_contentGroup.spacing) / 2f + (float)this.m_contentGroup.padding.left;
				for (int i = 0; i < this.m_list.Count; i++)
				{
					num += this.m_list[i].rect.width + this.m_contentGroup.spacing;
					Vector2 anchoredPosition2 = base.content.anchoredPosition;
					if (anchoredPosition2.x > 0f - num)
					{
						this.m_currentElement = i;
						break;
					}
				}
				this.m_currentElement = Mathf.Min(this.m_currentElement, this.m_list.Count - 1);
			}
		}
		else if (base.vertical)
		{
			Vector2 anchoredPosition3 = base.content.anchoredPosition;
			this.m_currentElement = Mathf.RoundToInt(anchoredPosition3.y / (this.m_list[0].rect.height + this.m_contentGroup.spacing));
		}
		if (!this.m_drag)
		{
			this.OnSelectIndex.SafeInvoke(this.m_currentElement);
		}
		else
		{
			this.OnDragSelectIndex.SafeInvoke(this.m_currentElement);
		}
	}

	private void UpdateScale(bool all)
	{
		if (this.m_list != null)
		{
			if (this.Scale > 1f)
			{
				float num = 0f;
				for (int i = 0; i < this.m_list.Count; i++)
				{
					float num2 = 0f;
					if (base.horizontal)
					{
						if (i > 0)
						{
							num += this.m_list[i - 1].rect.width + this.m_contentGroup.spacing;
						}
						Vector2 anchoredPosition = base.content.anchoredPosition;
						num2 = Mathf.Abs(anchoredPosition.x + num);
					}
					else if (base.vertical)
					{
						num = this.m_list[i].rect.height * (float)i + this.m_contentGroup.spacing * (float)i;
						Vector2 anchoredPosition2 = base.content.anchoredPosition;
						num2 = Mathf.Abs(anchoredPosition2.y - num);
					}
					if (all || num2 < this.DistForScale + this.m_list[i].rect.width + this.m_contentGroup.spacing)
					{
						float par = (!(num2 < this.DistForScale)) ? 1f : Mathf.Lerp(this.Scale, 1f, num2 / this.DistForScale);
						this.OnScaleElement.SafeInvoke(i, par, this.Scale);
					}
				}
			}
			this.UpdateImageIndex();
		}
	}

	public void PublicSetDirty()
	{
		base.SetDirty();
	}

	protected override void OnEnable()
	{
		this.UpdateScale(true);
		base.OnEnable();
	}
	private IEnumerator MoveCoroutine(float delta)
	{ 
		this.m_drag = true;
		var deltaMoreThan0 = delta >= 0f;

		while (true)
		{
			var nextMove = Time.deltaTime * this.Speed;
			nextMove = Mathf.Min(nextMove, Mathf.Abs(delta));
			nextMove *= (float)((!(delta < 0f)) ? 1 : (-1));
			if (this.horizontal)
			{ 
				this.content.localPosition += new Vector3(nextMove, 0f, 0f);
			}
			else if (this.vertical)
			{ 
				this.content.localPosition += new Vector3(0f, nextMove, 0f);
			}
			delta -= nextMove;
			if (deltaMoreThan0)
			{
				if (!(delta <= 0f))
				{
					this.UpdateScale(false);
					yield return null;
					continue; 
				}
			}
			else if (!(delta >= 0f))
			{
				this.UpdateScale(false);
				yield return null;
				continue;
			}

			yield return null;

			this.m_drag = false;
			this.UpdateImageIndex();
			this.SetIndex(this.m_currentElement);
			this.UpdateScale(false);

			yield break;
		}
	}

	private IEnumerator MoveToIndexCoroutine(int index)
	{
		this.m_drag = true;
		while(true)
		{
			var resPos = 0f;
			for (int i = 0; i < index; i++)
			{
				resPos -= this.m_list[i].rect.width + this.m_contentGroup.spacing;
			}  
			var delta = resPos - this.content.anchoredPosition.x;
			var deltaTime = Mathf.Min(0.05f, Time.deltaTime);
			var newDelta = Mathf.Min(Mathf.Abs(delta), Mathf.Abs(this.Speed * deltaTime));
			if (delta < 0f)
			{
				newDelta = 0f - newDelta;
			} 
			this.content.localPosition += new Vector3(newDelta, 0f, 0f);
			this.UpdateScale(false);
			if (!(Mathf.Abs(newDelta) < 1f))
			{
				yield return null;
				continue;
			}
			this.m_drag = false;

			yield break;
		}
	}
}
