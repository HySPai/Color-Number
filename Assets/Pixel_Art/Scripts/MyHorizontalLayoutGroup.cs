/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System.Collections.Generic;
using UnityEngine;

public class MyHorizontalLayoutGroup : MonoBehaviour
{
	private float m_fullLength;

	private void Start()
	{
		this.Reinit();
	}

	private void Update()
	{
		if (this.m_fullLength != ((RectTransform)base.transform).rect.width)
		{
			this.Reinit();
		}
	}

	private void Reinit()
	{
		List<RectTransform> list = new List<RectTransform>();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			RectTransform rectTransform = (RectTransform)base.transform.GetChild(i);
			if (rectTransform.gameObject.activeSelf)
			{
				list.Add(rectTransform);
			}
		}
		this.m_fullLength = ((RectTransform)base.transform).rect.width;
		float num = this.m_fullLength / (float)(list.Count + 1);
		for (int j = 0; j < list.Count; j++)
		{
			list[j].anchoredPosition = new Vector2(num * (float)(j + 1), 0f);
		}
	}
}


