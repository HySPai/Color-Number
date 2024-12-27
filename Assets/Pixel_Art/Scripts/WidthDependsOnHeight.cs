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

public class WidthDependsOnHeight : MonoBehaviour
{
	[SerializeField]
	private string m_expression;

	private RectTransform m_rectTransform;

	private float m_height = -1f;

	private void Start()
	{
		this.m_rectTransform = (RectTransform)base.transform;
		this.UpdateWidth();
	}

	private void Update()
	{
		if (this.m_rectTransform.rect.height != this.m_height)
		{
			this.UpdateWidth();
			base.enabled = false;
		}
	}

	private void UpdateWidth()
	{
		this.m_height = this.m_rectTransform.rect.height;
		float height = this.m_height;
		Vector2 sizeDelta = this.m_rectTransform.sizeDelta;
		sizeDelta.x = height;
		this.m_rectTransform.sizeDelta = sizeDelta;
		LayoutElement component = base.GetComponent<LayoutElement>();
		if (component != null)
		{
			LayoutElement layoutElement = component;
			component.preferredWidth = height; float num3 = layoutElement.minWidth = (component.preferredWidth);
		}
	}
}


