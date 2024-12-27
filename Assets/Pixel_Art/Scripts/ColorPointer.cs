/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using UnityEngine;
using UnityEngine.UI;

public class ColorPointer : MonoBehaviour
{
	private ColorImage m_colorImage;

	[SerializeField]
	private Image m_image;

	[SerializeField]
	private GameObject m_iphoneXAngle;

	public void UpdateColor(ColorImage colorImage, SpecialColorPosition pos)
	{
		if (this.m_colorImage != null)
		{
			this.m_colorImage.Unselect();
		}
		this.m_colorImage = colorImage;
		this.m_colorImage.Select();
		base.transform.SetParent(colorImage.transform);
		((RectTransform)base.transform).sizeDelta = Vector2.zero;
		((RectTransform)base.transform).anchoredPosition = Vector2.zero;
		this.m_iphoneXAngle.SetActive(false);
	}
}


