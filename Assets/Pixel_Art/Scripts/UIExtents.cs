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

public static class UIExtents
{
	public static void SetAlpha(this MaskableGraphic graphic, float alpha)
	{
		Color color = graphic.color;
		color.a = alpha;
		graphic.color = color;
	}

	public static bool IsDark(this Color color)
	{
		double num = 1.0 - (0.299 * (double)color.r + 0.587 * (double)color.g + 0.114 * (double)color.b);
		return num >= 0.5;
	}
}


