/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System;
public class DebugLogger
{
	public static bool IsDevelopmentBuild = true;

	public static void Log(object message)
	{
		if (DebugLogger.IsDevelopmentBuild)
		{
			UnityEngine.Debug.Log(message);
		}
	}

	public static void Log(object message, UnityEngine.Object obj)
	{
		if (DebugLogger.IsDevelopmentBuild)
		{
			UnityEngine.Debug.Log(message, obj);
		}
	}

	public static void LogWarning(object message)
	{
		if (DebugLogger.IsDevelopmentBuild)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}

	public static void LogWarning(object message, UnityEngine.Object obj)
	{
		if (DebugLogger.IsDevelopmentBuild)
		{
			UnityEngine.Debug.LogWarning(message, obj);
		}
	}

	public static void LogError(object message)
	{
		if (DebugLogger.IsDevelopmentBuild)
		{
			UnityEngine.Debug.LogError(message);
		}
	}

	public static void LogError(object message, UnityEngine.Object obj)
	{
		if (DebugLogger.IsDevelopmentBuild)
		{
			UnityEngine.Debug.LogError(message, obj);
		}
	}

	public static void LogException(Exception exception)
	{
		if (DebugLogger.IsDevelopmentBuild)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	public static void LogException(Exception exception, UnityEngine.Object obj)
	{
		if (DebugLogger.IsDevelopmentBuild)
		{
			UnityEngine.Debug.LogException(exception, obj);
		}
	}
}
