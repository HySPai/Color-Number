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

internal class MultiTouchModule : MonoBehaviour
{
	private void Update()
	{
		List<Touch> list = new List<Touch>();
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			list.Add(touch);
		}
		List<Touch> list2 = new List<Touch>();
		foreach (Touch item in list)
		{
			if (item.fingerId < 2)
			{
				list2.Add(item);
			}
		}
		list2.Sort((Touch a, Touch b) => a.fingerId.CompareTo(b.fingerId));
		if (list2.Count != 0 && list2.Count > 1)
		{
			Vector2 vector = list2[0].position - list2[0].deltaPosition - (list2[1].position - list2[1].deltaPosition);
			float num = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);
			Vector2 vector2 = list2[0].position - list2[1].position;
			float num2 = Mathf.Sqrt(vector2.x * vector2.x + vector2.y * vector2.y);
		}
	}
}


