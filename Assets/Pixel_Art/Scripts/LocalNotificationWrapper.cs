/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

internal class LocalNotificationWrapper : MonoBehaviour
{
	private static LocalNotificationWrapper s_instance;

	private bool notificationsCreated;

	private List<int> m_currentNotificationsIds = new List<int>();

	public static LocalNotificationWrapper Instance
	{
		get
		{
			if (LocalNotificationWrapper.s_instance == null)
			{
				GameObject gameObject = new GameObject("LocalNotificationWrapper");
				LocalNotificationWrapper.s_instance = gameObject.AddComponent<LocalNotificationWrapper>();
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			return LocalNotificationWrapper.s_instance;
		}
	}

	public void Init()
	{
		this.CancelAll();
	}

	public void Create(int id, DateTime dt, string title, string message, bool repeat = false, IDictionary dict = null)
	{
		this.Create(id, dt, (dt - DateTime.Now).TotalSeconds, title, message, repeat, dict);
	}

	public void Create(int id, double seconds, string title, string message, bool repeat = false, IDictionary dict = null)
	{
		DateTime dt = DateTime.Now + new TimeSpan((long)seconds * 10000000);
		this.Create(id, dt, seconds, title, message, repeat, dict);
	}

	public void Create(int id, DateTime dt, double seconds, string title, string message, bool repeat = false, IDictionary dict = null)
	{
		if (repeat)
		{
			LocalNotification.SendRepeatingNotification(id, TimeSpan.FromSeconds(seconds), TimeSpan.FromDays(1), title, message, Color.white);
		}
		else
		{
			LocalNotification.SendNotification(id, TimeSpan.FromSeconds(seconds), title, message, Color.white);
		} 
	}

	public void CancelAll()
	{
		if (this.m_currentNotificationsIds.Count == 0)
		{
			List<int> localNotifications = AppData.LocalNotifications;
			if (localNotifications != null)
			{
				this.m_currentNotificationsIds = localNotifications;
			}
			AppData.LocalNotifications = null;
		}
		foreach (int currentNotificationsId in this.m_currentNotificationsIds)
		{
			LocalNotification.CancelNotification(currentNotificationsId); 
		}
		this.m_currentNotificationsIds.Clear();
	}

	public void OnApplicationPause(bool val)
	{
		if (!val)
		{
			this.CancelAll();
			this.notificationsCreated = false;
		}
		else
		{
			this.ApplicationQuitHandler();
			this.notificationsCreated = true;
		}
	}

	private void ApplicationQuitHandler()
	{
		if (!this.notificationsCreated)
		{
			try
			{
				System.Random random = new System.Random();
				int num = 0;
				if (!IAPWrapper.Instance.Subscribed)
				{
					DateTime dateTime = DateTime.Now;
					dateTime += new TimeSpan(3, 0, 0);
					if (dateTime.Hour >= 9 && dateTime.Hour < 21)
					{
						num = random.Next(0, 2147483647);
						this.m_currentNotificationsIds.Add(num);
						this.Create(num, dateTime, "Pixel Art", LocalizationManager.Instance.GetString("ntfy_title") + '\n' + LocalizationManager.Instance.GetString("ntfy_body"), false, null);
					}
				}
				AppData.LocalNotifications = this.m_currentNotificationsIds;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(ex.Message);
			}
		}
	}
}


