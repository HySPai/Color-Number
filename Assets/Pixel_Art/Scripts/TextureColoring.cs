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
using System.Linq;
using UnityEngine;

public class TextureColoring
{
	private static int MaxColorCount = 64;

	private static IList<Color> simpleColorPallete;

	public static bool CheckToNeedConverColor(Texture2D texture, int maxColorCount)
	{
		TextureColoring.MaxColorCount = maxColorCount;
		Color32[] pixels = texture.GetPixels32();
		List<Color32> source = pixels.Distinct().ToList();
		if (source.Count() > TextureColoring.MaxColorCount - 1)
		{
			DebugLogger.Log("CheckToNeedConverColor = true " + source.Count());
			return true;
		}
		DebugLogger.Log("CheckToNeedConverColor = false " + source.Count());
		return false;
	}

	public static bool CompareColor(Color32 c1, Color32 c2)
	{
		if ((int)Math.Sqrt(Math.Pow((double)(c1.r - c2.r), 2.0) + Math.Pow((double)(c1.g - c2.g), 2.0) + Math.Pow((double)(c1.b - c2.b), 2.0)) == 0)
		{
			return true;
		}
		return false;
	}

	public static bool isNeedConvertColor(Texture2D texture)
	{
		IList<Color> list = new List<Color>();
		for (int i = 0; i < texture.width; i++)
		{
			for (int j = 0; j < texture.height; j++)
			{
			}
		}
		return true;
	}
}


