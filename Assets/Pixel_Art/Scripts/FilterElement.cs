/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System;
using UnityEngine;
using UnityEngine.UI;

public class FilterElement : MonoBehaviour
{
	public Action<FilterElement> OnClick;

	[SerializeField]
	private LayoutElement m_layoutElement;

	[SerializeField]
	private RawImage m_image;

	[SerializeField]
	private RawImage m_dark;

	public int FilterIndex { get; private set; }

	public void Init(FilterInfo info)
	{
		this.m_image.texture = info.Texture;
		this.FilterIndex = info.FilterIndex;
	}

	public void InitEmpty(Texture2D tex)
	{
		this.m_image.texture = tex;
		this.m_image.material = null;
		this.FilterIndex = 0;
	}

	public void Click()
	{
		this.OnClick.SafeInvoke(this);
	}

	public void Scale(float scale, float maxScale)
	{
		float a = scale / maxScale;
		a = Mathf.Min(a, 1f);
		this.m_layoutElement.preferredWidth = 150f * a;
		this.m_layoutElement.minWidth = 150f * a;
		((RectTransform)base.transform).sizeDelta = 150f * new Vector2(a, a);
		this.m_dark.SetAlpha(1f - a);
	}
}


