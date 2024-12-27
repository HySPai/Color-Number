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

public static class SystemExtends
{
	private static Random rng = new Random();

	public static void SafeInvoke(this Action action)
	{
		if (action != null)
		{
			action();
		}
	}

	public static void SafeInvoke<T>(this Action<T> action, T par)
	{
		if (action != null)
		{
			action(par);
		}
	}

	public static void SafeInvoke<T1, T2>(this Action<T1, T2> action, T1 par1, T2 par2)
	{
		if (action != null)
		{
			action(par1, par2);
		}
	}

	public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> action, T1 par1, T2 par2, T3 par3)
	{
		if (action != null)
		{
			action(par1, par2, par3);
		}
	}

	public static void SafeInvoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 par1, T2 par2, T3 par3, T4 par4)
	{
		if (action != null)
		{
			action(par1, par2, par3, par4);
		}
	}

	public static void Shuffle<T>(this IList<T> list)
	{
		int num = ((ICollection<T>)list).Count;
		while (num > 1)
		{
			num--;
			int index = SystemExtends.rng.Next(num + 1);
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}
}


