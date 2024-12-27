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

public class DeepLinkWrapper
{
	public static string DeepLink { get; private set; }

	public static void Init()
	{
	}

	public static Dictionary<string, string> Parse(string par)
	{
		UnityEngine.Debug.Log(par);
		if (!string.IsNullOrEmpty(par))
		{
			Uri uri = new Uri(par);
			if (uri.Host == "main")
			{
				string text = uri.Query.Replace("?", string.Empty);
				string[] array = text.Split('&');
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("host", uri.Host);
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					string[] array3 = text2.Split('=');
					dictionary.Add(array3[0], array3[1]);
				}
				return dictionary;
			}
		}
		return null;
	}
}


