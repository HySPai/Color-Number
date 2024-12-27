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

public class UniAndroidQueryPermission : MonoBehaviour
{
	public static Queue<AndroidPermission> permissionQueue = new Queue<AndroidPermission>();

	public static Action allPermissionRequested; 

	public static void RequestPermissions(List<AndroidPermission> listPermission)
	{
		foreach (AndroidPermission item in listPermission)
		{
			UniAndroidQueryPermission.permissionQueue.Enqueue(item);
		}
		UniAndroidQueryPermission.allPermissionRequested = delegate
		{
		};
		UniAndroidQueryPermission.RequestPermission();
	}

	private static void RequestPermission()
	{
		if (UniAndroidQueryPermission.permissionQueue.Count > 0)
		{
			AndroidPermission permission = UniAndroidQueryPermission.permissionQueue.Dequeue();
			UniAndroidPermission.RequestPermission(permission, new Action(UniAndroidQueryPermission.RequestPermission), new Action(UniAndroidQueryPermission.RequestPermission));
		}
		else
		{
			UniAndroidQueryPermission.allPermissionRequested();
		}
	}
}


