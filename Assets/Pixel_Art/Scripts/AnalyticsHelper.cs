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
using UnityEngine;

public static class AnalyticsHelper
{ 
	public static bool IsTestDevice()
	{
		 
		return false;
	}

	public static string GetPlatform()
	{
		if (AnalyticsHelper.RuntimePlatformIs("OSXEditor"))
		{
			return "MAC_CLIENT";
		}
		if (AnalyticsHelper.RuntimePlatformIs("OSXPlayer"))
		{
			return "MAC_CLIENT";
		}
		if (AnalyticsHelper.RuntimePlatformIs("WindowsPlayer"))
		{
			return "PC_CLIENT";
		}
		if (AnalyticsHelper.RuntimePlatformIs("OSXWebPlayer"))
		{
			return "WEB";
		}
		if (AnalyticsHelper.RuntimePlatformIs("OSXDashboardPlayer"))
		{
			return "MAC_CLIENT";
		}
		if (AnalyticsHelper.RuntimePlatformIs("WindowsWebPlayer"))
		{
			return "WEB";
		}
		if (AnalyticsHelper.RuntimePlatformIs("WindowsEditor"))
		{
			return "PC_CLIENT";
		}
		if (AnalyticsHelper.RuntimePlatformIs("IPhonePlayer"))
		{
			string deviceModel = SystemInfo.deviceModel;
			if (deviceModel.Contains("iPad"))
			{
				return "IOS_TABLET";
			}
			return "IOS_MOBILE";
		}
		if (AnalyticsHelper.RuntimePlatformIs("PS3"))
		{
			return "PS3";
		}
		if (AnalyticsHelper.RuntimePlatformIs("XBOX360"))
		{
			return "XBOX360";
		}
		if (AnalyticsHelper.RuntimePlatformIs("Android"))
		{
			return (!AnalyticsHelper.IsTablet()) ? "ANDROID_MOBILE" : "ANDROID_TABLET";
		}
		if (AnalyticsHelper.RuntimePlatformIs("NaCL"))
		{
			return "WEB";
		}
		if (AnalyticsHelper.RuntimePlatformIs("LinuxPlayer"))
		{
			return "PC_CLIENT";
		}
		if (AnalyticsHelper.RuntimePlatformIs("WebGLPlayer"))
		{
			return "WEB";
		}
		if (AnalyticsHelper.RuntimePlatformIs("FlashPlayer"))
		{
			return "WEB";
		}
		if (!AnalyticsHelper.RuntimePlatformIs("MetroPlayerX86") && !AnalyticsHelper.RuntimePlatformIs("MetroPlayerX64") && !AnalyticsHelper.RuntimePlatformIs("MetroPlayerARM") && !AnalyticsHelper.RuntimePlatformIs("WSAPlayerX86") && !AnalyticsHelper.RuntimePlatformIs("WSAPlayerX64") && !AnalyticsHelper.RuntimePlatformIs("WSAPlayerARM"))
		{
			if (AnalyticsHelper.RuntimePlatformIs("WP8Player"))
			{
				return (!AnalyticsHelper.IsTablet()) ? "WINDOWS_MOBILE" : "WINDOWS_TABLET";
			}
			if (!AnalyticsHelper.RuntimePlatformIs("BB10Player") && !AnalyticsHelper.RuntimePlatformIs("BlackBerryPlayer"))
			{
				if (AnalyticsHelper.RuntimePlatformIs("TizenPlayer"))
				{
					return (!AnalyticsHelper.IsTablet()) ? "ANDROID_MOBILE" : "ANDROID_TABLET";
				}
				if (AnalyticsHelper.RuntimePlatformIs("PSP2"))
				{
					return "PSVITA";
				}
				if (AnalyticsHelper.RuntimePlatformIs("PS4"))
				{
					return "PS4";
				}
				if (AnalyticsHelper.RuntimePlatformIs("PSMPlayer"))
				{
					return "WEB";
				}
				if (AnalyticsHelper.RuntimePlatformIs("XboxOne"))
				{
					return "XBOXONE";
				}
				if (AnalyticsHelper.RuntimePlatformIs("SamsungTVPlayer"))
				{
					return "ANDROID_CONSOLE";
				}
				return "UNKNOWN";
			}
			return (!AnalyticsHelper.IsTablet()) ? "BLACKBERRY_MOBILE" : "BLACKBERRY_TABLET";
		}
		if (SystemInfo.deviceType == DeviceType.Handheld)
		{
			return (!AnalyticsHelper.IsTablet()) ? "WINDOWS_MOBILE" : "WINDOWS_TABLET";
		}
		return "PC_CLIENT";
	}

	private static bool RuntimePlatformIs(string platformName)
	{
		return Enum.IsDefined(typeof(RuntimePlatform), platformName) && Application.platform.ToString() == platformName;
	}

	private static float ScreenSizeInches()
	{
		float num = (float)Screen.width / Screen.dpi;
		float num2 = (float)Screen.height / Screen.dpi;
		return (float)Math.Sqrt((double)(num * num + num2 * num2));
	}

	private static bool IsTablet()
	{
		return AnalyticsHelper.ScreenSizeInches() > 6f;
	}
}


