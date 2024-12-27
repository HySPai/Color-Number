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
using System.Linq;
using UnityEngine;

public class ColorsPage : MonoBehaviour
{
	public Action<ColorImage, SpecialColorPosition> OnColorClick;

	private int m_rowWidth = -1;

	private int m_pageIndex = -1;

	[SerializeField]
	private ColorImage m_colorPrefab;

	private List<ColorImage> m_colorImages;

	[SerializeField]
	private RectTransform m_firstRow;

	[SerializeField]
	private RectTransform m_secondRow;

	public void Init(List<Color> colors, int startIndex, int rowWidth, float colorSize, int pageIndex)
	{
		this.m_colorImages = new List<ColorImage>();
		this.m_rowWidth = rowWidth;
		this.m_pageIndex = pageIndex;
		bool flag = false;
		for (int i = 0; i < colors.Count; i++)
		{
			ColorImage colorImage = UnityEngine.Object.Instantiate(this.m_colorPrefab);
			this.m_colorImages.Add(colorImage);
			colorImage.Init(colors[i], startIndex + i + 1);
			ColorImage colorImage2 = colorImage;
			colorImage2.OnClick = (Action<ColorImage>)Delegate.Combine(colorImage2.OnClick, new Action<ColorImage>(this.OnColorClickedHandler));
			if (i < rowWidth)
			{
				colorImage.transform.SetParent(this.m_firstRow);
			}
			else
			{
				colorImage.transform.SetParent(this.m_secondRow);
				flag = true;
			}
			colorImage.transform.localScale = Vector3.one;
			((RectTransform)colorImage.transform).sizeDelta = new Vector2(colorSize, colorSize);
			((RectTransform)colorImage.transform).anchoredPosition = new Vector2((float)(i % rowWidth) * colorSize, 0f);
			colorImage.gameObject.SetActive(true);
		}
		((RectTransform)base.transform).sizeDelta = new Vector2((float)rowWidth * colorSize, colorSize * (float)((!flag) ? 1 : 2));
		((RectTransform)this.m_firstRow.transform).sizeDelta = new Vector2((float)rowWidth * colorSize, colorSize);
		((RectTransform)this.m_secondRow.transform).sizeDelta = new Vector2((float)rowWidth * colorSize, (!flag) ? 0f : colorSize);
	}

	public void SelectFirstColor()
	{
		if (this.m_colorImages.Count <= this.m_rowWidth)
		{
			this.OnColorClick(this.m_colorImages[0], SpecialColorPosition.BottomLeft);
		}
		else
		{
			this.OnColorClick(this.m_colorImages[0], SpecialColorPosition.None);
		}
	}

	public void DisableColor(Color color)
	{
		ColorImage colorImage = this.m_colorImages.FirstOrDefault((ColorImage a) => a.Color == color);
		if (colorImage != null)
		{
			colorImage.Disable();
		}
	}

	private void OnColorClickedHandler(ColorImage colorImage)
	{
		int num = this.m_colorImages.IndexOf(colorImage);
		if (this.m_colorImages.Count <= this.m_rowWidth)
		{
			if (num == 0 && this.m_pageIndex == 0)
			{
				this.OnColorClick(colorImage, SpecialColorPosition.BottomLeft);
			}
			else if (num == this.m_rowWidth - 1 && this.m_pageIndex == 0)
			{
				this.OnColorClick(colorImage, SpecialColorPosition.BottomRight);
			}
			else
			{
				this.OnColorClick(colorImage, SpecialColorPosition.None);
			}
		}
		else if (num == this.m_rowWidth)
		{
			this.OnColorClick(colorImage, SpecialColorPosition.BottomLeft);
		}
		else if (num == 2 * this.m_rowWidth - 1)
		{
			this.OnColorClick(colorImage, SpecialColorPosition.BottomRight);
		}
		else
		{
			this.OnColorClick(colorImage, SpecialColorPosition.None);
		}
	}
}


