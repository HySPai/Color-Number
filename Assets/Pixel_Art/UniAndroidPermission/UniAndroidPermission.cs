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

public class UniAndroidPermission : MonoBehaviour
{
	const string PackageName = "net.sanukin.PermissionManager";

	static Action onAllowCallback;
	static Action onDenyCallback;
	static Action onDenyAndNeverAskAgainCallback;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	public static bool IsPermitted(AndroidPermission permission)
	{
#if !UNITY_EDITOR && UNITY_ANDROID
        using (var permissionManager = new AndroidJavaClass(PackageName))
        {
            return permissionManager.CallStatic<bool>("hasPermission", GetPermittionStr(permission));
        }
#else
		return true;
#endif
	}

	public static void RequestPermission(AndroidPermission permission, Action onAllow = null, Action onDeny = null, Action onDenyAndNeverAskAgain = null)
	{
#if !UNITY_EDITOR && UNITY_ANDROID
        using (var permissionManager = new AndroidJavaClass(PackageName))
        {
            permissionManager.CallStatic("requestPermission", GetPermittionStr(permission));
            onAllowCallback = onAllow;
            onDenyCallback = onDeny;
            onDenyAndNeverAskAgainCallback = onDenyAndNeverAskAgain;
        }
#else
		Debug.LogWarning("UniAndroidPermission works only Androud Devices.");
		if (onDeny != null)
		{
			onDeny();
		}
#endif
	}

	private static string GetPermittionStr(AndroidPermission permittion)
	{
		return "android.permission." + permittion.ToString();
	}

	private void OnAllow()
	{
		if (onAllowCallback != null)
		{
			onAllowCallback();
		}
		ResetAllCallBacks();
	}

	private void OnDeny()
	{
		if (onDenyCallback != null)
		{
			onDenyCallback();
		}
		ResetAllCallBacks();
	}

	private void OnDenyAndNeverAskAgain()
	{
		if (onDenyAndNeverAskAgainCallback != null)
		{
			onDenyAndNeverAskAgainCallback();
		}
		ResetAllCallBacks();
	}

	private void ResetAllCallBacks()
	{
		onAllowCallback = null;
		onDenyCallback = null;
		onDenyAndNeverAskAgainCallback = null;
	}
}

// Protection level: dangerous permissions 2015/11/25
// http://developer.android.com/intl/ja/reference/android/Manifest.permission.html
public enum AndroidPermission
{
	ACCESS_COARSE_LOCATION,
	ACCESS_FINE_LOCATION,
	ADD_VOICEMAIL,
	BODY_SENSORS,
	CALL_PHONE,
	CAMERA,
	GET_ACCOUNTS,
	PROCESS_OUTGOING_CALLS,
	READ_CALENDAR,
	READ_CALL_LOG,
	READ_CONTACTS,
	READ_EXTERNAL_STORAGE,
	READ_PHONE_STATE,
	READ_SMS,
	RECEIVE_MMS,
	RECEIVE_SMS,
	RECEIVE_WAP_PUSH,
	RECORD_AUDIO,
	SEND_SMS,
	USE_SIP,
	WRITE_CALENDAR,
	WRITE_CALL_LOG,
	WRITE_CONTACTS,
	WRITE_EXTERNAL_STORAGE
}
