/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System;
using UnityEngine;

public class SystemToolsWrapper
{
	private static AndroidJavaObject systemTool;

	public static void Minimize()
	{
		AndroidJavaObject s = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		s.Call<bool>("moveTaskToBack", new object[1] {
			true
		});
	}

	public static bool CheckWritePermission()
	{
		return UniAndroidPermission.IsPermitted(AndroidPermission.WRITE_EXTERNAL_STORAGE);
	}

	public static void RequestWritePermission()
	{
		UniAndroidPermission.RequestPermission(AndroidPermission.WRITE_EXTERNAL_STORAGE);

	}

	public static void TestCrash()
	{
		//Crashlytics.ThrowNonFatal();
	}

	public static string GetUid()
	{
		return SystemInfo.deviceUniqueIdentifier;
	}

	public static string GetInstanceId()
	{
		return SystemInfo.deviceUniqueIdentifier;
	}

	public static string GetAndroidId()
	{
		return SystemInfo.deviceUniqueIdentifier;
	}

	public static DateTime GetInstallDate()
	{
		return DateTime.Now;
	}

	public static string GetLocale()
	{
		return "en-US";
	}
}


