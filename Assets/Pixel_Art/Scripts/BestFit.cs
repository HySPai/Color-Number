/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestFit : MonoBehaviour
{
	private float timer = 1f;

	[SerializeField]
	private int m_minSize = 16;

	[SerializeField]
	private int m_maxSize = 16;

	[SerializeField]
	private bool m_enableAfterRefit;

	private RectTransform m_transform;

	private Text m_text;

	private string m_textText;

	private bool m_textEnabled = true;

	private Rect m_rect = default(Rect);

	private Canvas m_parentCanvas;

	private bool m_waitAndRefit;

	public int MinSize
	{
		get
		{
			return this.m_minSize;
		}
		set
		{
			this.m_minSize = value;
		}
	}

	public int MaxSize
	{
		get
		{
			return this.m_maxSize;
		}
		set
		{
			this.m_maxSize = value;
		}
	}

	private void Start()
	{
		if (this.m_text == null)
		{
			this.m_text = base.GetComponent<Text>();
			this.m_transform = (base.transform as RectTransform);
			base.StartCoroutine(this.WaitCanvasCoroutine());
		}
	}

	private IEnumerator WaitCanvasCoroutine()
	{
		if (this.m_parentCanvas == null)
		{
			Transform tr = this.m_transform;
			while (((Component)tr).GetComponent<Canvas>() == null)
			{
				tr = tr.parent;
				if (tr == null)
				{
					break;
				}
			}
			if (tr != null)
			{
				this.m_parentCanvas = ((Component)tr).GetComponent<Canvas>();
			}
			yield return null;
		}
	}

	private void Update()
	{
		if (this.m_parentCanvas != null && !this.m_parentCanvas.enabled)
		{
			return;
		}
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			base.enabled = false;
		}
		if (this.m_textText != this.m_text.text || this.m_waitAndRefit)
		{
			this.m_textEnabled = this.m_text.enabled;
			this.m_text.enabled = false;
			this.Refit();
		}
		if (!(this.m_transform.rect != this.m_rect))
		{
			return;
		}
		Vector2 size = this.m_rect.size;
		float x = size.x;
		Vector2 size2 = this.m_transform.rect.size;
		if (!(x > size2.x))
		{
			Vector2 size3 = this.m_rect.size;
			float y = size3.y;
			Vector2 size4 = this.m_transform.rect.size;
			if (y > size4.y)
			{
                this.m_rect = this.m_transform.rect;
                return;
			}
			this.m_textEnabled = this.m_text.enabled;
			this.m_text.enabled = false;
			this.Refit();
			return;
		} 
		this.m_rect = this.m_transform.rect;
	}

	public void Refit()
	{
		if (this.m_text == null)
		{
			this.Start();
		}
		this.m_textText = this.m_text.text;
		if (this.m_text.font.dynamic)
		{
			this.m_text.resizeTextForBestFit = true;
			this.m_text.resizeTextMinSize = this.m_minSize;
			this.m_text.resizeTextMaxSize = this.m_maxSize;
			this.m_text.enabled = this.m_textEnabled;
		}
		else
		{
			this.m_text.fontSize = this.MinSize;
			this.m_rect = this.m_transform.rect;
			if (this.m_parentCanvas != null && !this.m_parentCanvas.enabled)
			{
				this.m_waitAndRefit = true;
			}
			else
			{
				this.UpdateThis();
			}
		}
	}

	private void UpdateThis()
	{
		if (this.m_textText.Length == 0)
		{
			this.m_text.enabled = this.m_textEnabled;
		}
		else
		{
			TextGenerator textGenerator = new TextGenerator();
			TextGenerationSettings generationSettings = this.m_text.GetGenerationSettings(this.m_transform.rect.size);
			textGenerator.Populate(this.m_textText, generationSettings);
			IList<UICharInfo> characters = textGenerator.characters;
			string[] array = this.m_textText.Split(' ');
			float baseSpaceLength = 0f;
			if (array.Length > 1 && characters.Count > 0)
			{
				UICharInfo uICharInfo = characters[array[0].Length];
				baseSpaceLength = uICharInfo.charWidth;
			}
			float[] array2 = new float[array.Length];
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = 0f;
				for (int j = 0; j < array[i].Length; j++)
				{
					try
					{
						UICharInfo uICharInfo2 = characters[num++];
						float charWidth = uICharInfo2.charWidth;
						array2[i] += charWidth;
					}
					catch
					{
					}
				}
				num++;
			}
			if (this.CheckSize(array2, this.m_maxSize, baseSpaceLength))
			{
				this.SetSize(this.m_maxSize);
			}
			else
			{
				this.CheckRange(array2, this.m_minSize, this.m_maxSize, baseSpaceLength);
			}
		}
	}

	private void CheckRange(float[] wordLengths, int minSize, int maxSize, float baseSpaceLength)
	{
		int num = (minSize + maxSize) / 2;
		if (!this.CheckSize(wordLengths, num, baseSpaceLength))
		{
			if (minSize == num)
			{
				this.SetSize(num);
			}
			else
			{
				this.CheckRange(wordLengths, minSize, num, baseSpaceLength);
			}
		}
		else
		{
			float num2 = (float)((num + maxSize) / 2);
			if (maxSize == num || num2 == (float)num)
			{
				this.SetSize(num);
			}
			else
			{
				this.CheckRange(wordLengths, num, maxSize, baseSpaceLength);
			}
		}
	}

	private bool CheckSize(float[] wordLengths, int targetSize, float baseSpaceLength)
	{
		float num = (float)targetSize / (float)this.m_text.font.fontSize;
		float[] array = new float[wordLengths.Length];
		for (int i = 0; i < wordLengths.Length; i++)
		{
			array[i] = wordLengths[i] * num;
		}
		float num2 = baseSpaceLength * num;
		int num3 = Mathf.CeilToInt((float)this.m_text.font.lineHeight * num);
		if ((float)num3 >= this.m_transform.rect.height)
		{
			return false;
		}
		List<float> list = new List<float>();
		list.Add(0f);
		int num4 = 0;
		int num5 = 0;
		while (num5 < array.Length)
		{
			if (list[num4] + num2 + array[num5] < this.m_transform.rect.width)
			{
				List<float> list2;
				int index;
				list2 = list; index = num4; (list2)[index] = list2[index] + (num2 + array[num5]);
				num5++;
				continue;
			}
			if (!((float)((list.Count + 1) * num3) < this.m_transform.rect.height))
			{
				return false;
			}
			list.Add(0f);
			num4++;
		}
		return true;
	}

	private void SetSize(int targetSize)
	{
		this.m_text.fontSize = targetSize;
		if (this.m_enableAfterRefit)
		{
			this.m_text.enabled = true;
		}
		else
		{
			this.m_text.enabled = this.m_textEnabled;
		}
	}
}
