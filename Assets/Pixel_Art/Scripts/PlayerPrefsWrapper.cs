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
using System.Text;
using UnityEngine;

public class PlayerPrefsWrapper
{
	private static string s_listSeparator = "\n";

	public static bool Exists(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	public static string GetString(string key)
	{
		return PlayerPrefs.GetString(key);
	}

	public static void SetString(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
	}

	public static bool GetBool(string key)
	{
		return PlayerPrefs.GetString(key) == "1";
	}

	public static void SetBool(string key, bool value)
	{
		PlayerPrefs.SetString(key, (!value) ? "0" : "1");
	}

	public static int GetInt(string key)
	{
		return PlayerPrefs.GetInt(key);
	}

	public static void SetInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
	}

	public static List<string> GetStringList(string key)
	{
		string @string = PlayerPrefsWrapper.GetString(key);
		if (string.IsNullOrEmpty(@string))
		{
			return new List<string>();
		}
		return @string.Split(new string[1] {
			PlayerPrefsWrapper.s_listSeparator
		}, StringSplitOptions.RemoveEmptyEntries).ToList();
	}

	public static void SetStringList(string key, List<string> list)
	{
		if (list != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string item in list)
			{
				stringBuilder.Append(item).Append(PlayerPrefsWrapper.s_listSeparator);
			}
			PlayerPrefsWrapper.SetString(key, stringBuilder.ToString());
		}
	}

	public static void Clear()
	{
		PlayerPrefs.DeleteAll();
	}
}


