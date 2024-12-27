/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomVerticalLayoutGroup : MonoBehaviour
{
	private List<RectTransform> m_childs;

	private float m_rectHeight;

	[ContextMenu("ForceUpdate")]
	private void ContextMenuForceUpdate()
	{
		this.ForceUpdate();
	}

	private void Start()
	{
		this.m_childs = new List<RectTransform>();
		this.m_rectHeight = (base.transform as RectTransform).rect.height;
		this.Process();
	}

	private void Update()
	{
		if ((base.transform as RectTransform).rect.height != this.m_rectHeight)
		{
			this.m_rectHeight = (base.transform as RectTransform).rect.height;
			this.Process();
		}
	}

	public void ForceUpdate()
	{
		this.Process();
	}

	private void Process()
	{
		this.m_childs.Clear();
		CustomVerticalLayoutGroupElement[] componentsInChildren = ((Component)base.transform).GetComponentsInChildren<CustomVerticalLayoutGroupElement>();
		if (componentsInChildren.Length > 0)
		{
			componentsInChildren = (from a in componentsInChildren
									orderby a.SiblingIndex
									select a).ToArray();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				RectTransform rectTransform = componentsInChildren[i].transform as RectTransform;
				if (rectTransform.gameObject.activeSelf)
				{
					this.m_childs.Add(rectTransform);
				}
			}
		}
		else
		{
			for (int j = 0; j < base.transform.childCount; j++)
			{
				RectTransform rectTransform2 = base.transform.GetChild(j) as RectTransform;
				if (rectTransform2.gameObject.activeSelf)
				{
					this.m_childs.Add(rectTransform2);
				}
			}
		}
		float num = this.m_childs.Sum((RectTransform a) => a.rect.height);
		float num2 = this.m_rectHeight - num;
		float num3 = num2 / (float)this.m_childs.Count;
		float num4 = 0f;
		for (int k = 0; k < this.m_childs.Count; k++)
		{
			this.m_childs[k].anchoredPosition = new Vector2(0f, (0f - num3) * ((float)k + 0.5f) - num4 - this.m_childs[k].rect.height / 2f);
			num4 += this.m_childs[k].rect.height;
		}
	}
}


