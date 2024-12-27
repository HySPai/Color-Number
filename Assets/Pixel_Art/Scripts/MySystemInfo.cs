/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;

public class MySystemInfo
{
	private static bool? s_IsTablet;

	public static bool IsTablet
	{
		get
		{
			bool? nullable = MySystemInfo.s_IsTablet;
			if (!nullable.HasValue)
			{
				MySystemInfo.s_IsTablet = (MySystemInfo.DeviceDiagonalSizeInInches() > 6.5f);
			}
			bool? nullable2 = MySystemInfo.s_IsTablet;
			return nullable2.Value;
		}
	}

	private static float DeviceDiagonalSizeInInches()
	{
		float f = (float)Screen.width / Screen.dpi;
		float f2 = (float)Screen.height / Screen.dpi;
		return Mathf.Sqrt(Mathf.Pow(f, 2f) + Mathf.Pow(f2, 2f));
	}
}


