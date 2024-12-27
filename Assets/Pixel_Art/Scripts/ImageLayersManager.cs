/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageLayersManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	public enum TapState
	{
		None,
		Down,
		Drag,
		LongTap,
		Multitouch,
		BrushMode
	}

	[SerializeField]
	private Canvas m_canvas;

	[SerializeField]
	private Loupe m_loupe;

	private float m_longTapTime = 0.7f;

	private Vector3 m_tapPosition = Vector3.zero;

	private Color m_longTapPipetColor = Color.white;

	private DateTime m_downTime = default(DateTime);

	private Camera m_mainCamera;

	private List<Vector2> m_clickedPixels;

	private Guid m_clickId;

	private bool m_brushDrag;

	public static ImageLayersManager Instance { get; private set; }

	public TapState CurrentTapState { get; private set; }

	public bool Hovered { get; private set; }

	public bool Drag
	{
		get
		{
			return this.CurrentTapState == TapState.Drag || this.CurrentTapState == TapState.Multitouch;
		}
	}

	private void Start()
	{
		ImageLayersManager.Instance = this;
		this.m_mainCamera = Camera.main;
		this.m_clickedPixels = new List<Vector2>();
	}

	private void Update()
	{
		switch (this.CurrentTapState)
		{
			case TapState.LongTap:
				break;
			case TapState.Down:
				if (!this.CheckMultiTouch())
				{
					this.CheckLongTap();
				}
				break;
			case TapState.Drag:
				this.CheckMultiTouch();
				break;
			case TapState.Multitouch:
				if (Input.touches.Length == 0)
				{
					this.CurrentTapState = TapState.None;
				}
				break;
			case TapState.BrushMode:
				if (!this.CheckMultiTouch() && !this.m_brushDrag)
				{
					this.CheckLongTap();
				}
				break;
			case TapState.None:
				this.CheckMultiTouch();
				break;
		}
	}

	private bool CheckMultiTouch()
	{
		if (Input.touches.Length > 1)
		{
			this.CurrentTapState = TapState.Multitouch;
			return true;
		}
		return false;
	}

	private void CheckLongTap()
	{
		bool flag = false;
		if ((DateTime.Now - this.m_downTime).TotalSeconds > (double)this.m_longTapTime)
		{
			if (WorkbookModel.Instance.ColorizationModeModel.CurrentSpaceType != 0 && this.m_brushDrag)
			{
				return;
			}
			WorkbookModel.Instance.ColorizationModeModel.CurrentSpaceType = ColorizationModeModel.BrushType.Plural;
			flag = true;
			this.CurrentTapState = TapState.BrushMode;
			this.m_loupe.SwitchOnOff(true);
		}
	}

	public void OnPointerDown(PointerEventData data)
	{
		if (!this.CheckMultiTouch())
		{
			this.m_clickedPixels.Clear();
			this.m_clickId = Guid.NewGuid();
			this.CurrentTapState = TapState.Down;
			this.m_downTime = DateTime.Now;
			this.m_tapPosition = Input.mousePosition;
			if (WorkbookModel.Instance.ColorizationModeModel.CurrentSpaceType == ColorizationModeModel.BrushType.Plural)
			{
				this.CurrentTapState = TapState.BrushMode;
				this.m_brushDrag = false;
			}
		}
	}

	public void OnPointerUp(PointerEventData data)
	{
		switch (this.CurrentTapState)
		{
			case TapState.Down:
				this.ClickLayer();
				break;
			case TapState.LongTap:
				WorkbookModel.Instance.ColorizationModeModel.CurrentSpaceType = ColorizationModeModel.BrushType.Singular;
				break;
			case TapState.BrushMode:
				this.ClickLayer();
				WorkbookModel.Instance.ColorizationModeModel.CurrentSpaceType = ColorizationModeModel.BrushType.Singular;
				this.m_loupe.SwitchOnOff(false);
				break;
		}
		this.CurrentTapState = TapState.None;
	}

	public void OnDrag(PointerEventData data)
	{
		switch (this.CurrentTapState)
		{
			case TapState.None:
				break;
			case TapState.Drag:
				break;
			case TapState.LongTap:
				break;
			case TapState.Multitouch:
				break;
			case TapState.Down:
				this.CurrentTapState = TapState.Drag;
				break;
			case TapState.BrushMode:
				{
					this.m_brushDrag = true;
					Vector2 vector = data.position - data.delta;
					float[] obj = new float[3] {
					1f,
					0f,
					0f
				};
					Vector2 delta = data.delta;
					obj[1] = Mathf.Abs(delta.x);
					Vector2 delta2 = data.delta;
					obj[2] = Mathf.Abs(delta2.y);
					float num = Mathf.Max(obj);
					Vector2 a = data.delta / num;
					Vector2 a2 = vector;
					for (int i = 1; (float)i < num; i++)
					{
						this.ClickLayer(a2 + (float)i * a);
					}
					this.ClickLayer(data.position);
					break;
				}
		}
	}

	private void ClickLayer()
	{
		this.ClickLayer(Input.mousePosition);
	}

	private void ClickLayer(Vector2 point)
	{
		Ray ray = this.m_mainCamera.ScreenPointToRay(point);
		Vector2 zero = Vector2.zero;
		if (this.CheckLayers(ray, out zero))
		{
			WorkbookModel.Instance.TutorialModel.ImageClick();
			this.m_clickedPixels.Add(zero);
		}
	}

	public bool CheckLayers(Ray ray, out Vector2 pixelPos)
	{
		RaycastHit[] array = Physics.RaycastAll(ray);
		if (array != null && array.Length > 0)
		{
			Texture mainTexture = array[0].collider.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;
			Vector3 point = array[0].point;
			float x = point.x;
			Vector3 lossyScale = array[0].collider.transform.lossyScale;
			point.x = x / lossyScale.x;
			float y = point.y;
			Vector3 lossyScale2 = array[0].collider.transform.lossyScale;
			point.y = y / lossyScale2.y;
			point.x = (point.x + 1f) / 2f * (float)mainTexture.width;
			point.y = (point.y + 1f) / 2f * (float)mainTexture.height;
			pixelPos = point;
			return true;
		}
		pixelPos = Vector2.zero;
		return false;
	}

	private bool GetWorldPos(Ray ray, out Vector2 worldPos)
	{
		RaycastHit[] array = Physics.RaycastAll(ray);
		if (array != null && array.Length > 0)
		{
			Texture mainTexture = array[0].collider.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;
			worldPos = array[0].point;
			return true;
		}
		worldPos = Vector2.zero;
		return false;
	}

	public void OnPointerEnter(PointerEventData data)
	{
		this.Hovered = true;
	}

	public void OnPointerExit(PointerEventData data)
	{
		this.Hovered = false;
	}
}


