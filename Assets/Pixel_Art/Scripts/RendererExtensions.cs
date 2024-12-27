/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;

public static class RendererExtensions
{
	private static int CountCornersVisibleFrom(this RectTransform rectTransform, Camera camera)
	{
		Rect rect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		Vector3[] array = new Vector3[4];
		rectTransform.GetWorldCorners(array);
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			Vector3 point = camera.WorldToScreenPoint(array[i]);
			if (rect.Contains(point))
			{
				num++;
			}
		}
		return num;
	}

	public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera)
	{
		return rectTransform.CountCornersVisibleFrom(camera) == 4;
	}

	public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera)
	{
		return rectTransform.CountCornersVisibleFrom(camera) > 0;
	}
}


