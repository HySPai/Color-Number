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

public static class AppData
{
	private static string s_uidKey = "uid"; 

	private static string s_appRatedKey = "app_rated";

	private static string s_sessionIdKey = "session_id";

	private static string s_oneSignalIdKey = "onesignal_id";

	private static string s_rateUsShowsKey = "last_rateus_shows";

	private static string s_localNotificationsKey = "local_notifs";

	private static string s_purhasesKey = "purchases";

	private static string s_freeImagesCounterKey = "free_images_counter";

	private static string s_videoImagesKey = "video_images";

	private static string s_vibroKey = "vibro";

	private static string s_vibroRightKey = "vibro_right";

	private static string s_vibroWrongKey = "vibro_wrong";

	public static string s_soundsKey = "sounds";

	private static string s_tutorialKey = "tutorial";

	private static string s_tutorialKey3D = "tutorial3D";

	private static string s_lassoCountKey = "lassos";

	private static string s_bombCountKey = "bombs";

	private static string s_prevHostKey = "prev_host";

	private static string s_isOldUserKey = "is_old_user";

	private static string s_bulbModeKey = "bulb_mode";

	private static string s_loupeKey = "loupe";

	private static string s_noAdsKey = "no_ads";

	private static string s_specPageOpenedKey = "spec_page";

	private static string s_unlockedImages = "unlocked_images";

	private static string s_pictureDoneKey = "picture_done";

	private static string s_pictureDoneCountKey = "picture_done_count";

	private static string s_launchNextDayKey = "launch_next_day";

	private static string s_launchNext2DaysKey = "launch_next2_days";

	private static string s_endSessionTime = "end_session_time";

	private static string s_lastDayTracked = "session_tracked";

	private static string s_freePhotos = "free_photos";

	private static string s_firstLaunch = "first_launch";

	private static string s_intersCount = "inters_count";

	private static string s_secondsInGame = "seconds_count";

	private static string s_weekSecondsTracked = "week_sec_tracked";

	private static string s_subCampaign = "sub_campaign";

	private static string s_updateToNew = "udpate_to_new";

	public static bool Inited { get; private set; }

	public static string Uid
	{
		get
		{
			string text = PlayerPrefsWrapper.GetString(AppData.s_uidKey);
			if (string.IsNullOrEmpty(text))
			{
				text = Guid.NewGuid().ToString();
				PlayerPrefsWrapper.SetString(AppData.s_uidKey, text);
			}
			return text;
		}
	} 

	public static bool AppRated
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_appRatedKey);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_appRatedKey, value);
		}
	}

	public static int SessionId
	{
		get
		{
			return PlayerPrefsWrapper.GetInt(AppData.s_sessionIdKey);
		}
		private set
		{
			PlayerPrefsWrapper.SetInt(AppData.s_sessionIdKey, value);
		}
	}

	public static string OneSignalId
	{
		get
		{
			return PlayerPrefsWrapper.GetString(AppData.s_oneSignalIdKey);
		}
		set
		{
			PlayerPrefsWrapper.SetString(AppData.s_oneSignalIdKey, value);
		}
	}

	public static List<DateTime> LastRateUsShows
	{
		get
		{
			List<string> stringList = PlayerPrefsWrapper.GetStringList(AppData.s_rateUsShowsKey);
			List<DateTime> list = new List<DateTime>();
			foreach (string item in stringList)
			{
				list.Add(DateTime.Parse(item));
			}
			return list;
		}
		private set
		{
			if (value != null)
			{
				List<string> list = new List<string>();
				foreach (DateTime item in value)
				{
					list.Add(item.ToString());
				}
				PlayerPrefsWrapper.SetStringList(AppData.s_rateUsShowsKey, list);
			}
		}
	}

	public static List<int> LocalNotifications
	{
		get
		{
			List<string> stringList = PlayerPrefsWrapper.GetStringList(AppData.s_localNotificationsKey);
			List<int> list = new List<int>();
			foreach (string item in stringList)
			{
				list.Add(int.Parse(item));
			}
			return list;
		}
		set
		{
			if (value != null)
			{
				List<string> list = new List<string>();
				foreach (int item in value)
				{
					list.Add(item.ToString());
				}
				PlayerPrefsWrapper.SetStringList(AppData.s_localNotificationsKey, list);
			}
		}
	}

	public static List<string> Purchases
	{
		get
		{
			return PlayerPrefsWrapper.GetStringList(AppData.s_purhasesKey);
		}
		set
		{
			PlayerPrefsWrapper.SetStringList(AppData.s_purhasesKey, value);
		}
	}

	public static int FreeImagesCounter
	{
		get
		{
			return PlayerPrefsWrapper.GetInt(AppData.s_freeImagesCounterKey);
		}
		set
		{
			PlayerPrefsWrapper.SetInt(AppData.s_freeImagesCounterKey, value);
		}
	}

	public static List<string> VideoImages
	{
		get
		{
			return PlayerPrefsWrapper.GetStringList(AppData.s_videoImagesKey);
		}
		set
		{
			PlayerPrefsWrapper.SetStringList(AppData.s_videoImagesKey, value);
		}
	}

	private static bool VibroEnabled
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_vibroKey);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_vibroKey, value);
		}
	}

	public static bool VibroRightEnabled
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_vibroRightKey);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_vibroRightKey, value);
		}
	}

	public static bool VibroWrongEnabled
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_vibroWrongKey);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_vibroWrongKey, value);
		}
	}

	public static bool SoundsEnabled
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_soundsKey);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_soundsKey, value);
		}
	}

	public static bool TutorialCompleted
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_tutorialKey);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_tutorialKey, value);
		}
	}

	//public static bool TutorialCompleted3D
	//{
	//	get
	//	{
	//		return PlayerPrefsWrapper.GetBool(AppData.s_tutorialKey3D);
	//	}
	//	set
	//	{
	//		PlayerPrefsWrapper.SetBool(AppData.s_tutorialKey3D, value);
	//	}
	//}

	public static int LassoCount
	{
		get
		{
			return PlayerPrefsWrapper.GetInt(AppData.s_lassoCountKey);
		}
		set
		{
			PlayerPrefsWrapper.SetInt(AppData.s_lassoCountKey, value);
		}
	}

	public static int BombCount
	{
		get
		{
			return PlayerPrefsWrapper.GetInt(AppData.s_bombCountKey);
		}
		set
		{
			PlayerPrefsWrapper.SetInt(AppData.s_bombCountKey, value);
		}
	} 

	public static bool IsOldUser
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_isOldUserKey);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_isOldUserKey, value);
		}
	}

	public static bool BulbMode
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_bulbModeKey);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_bulbModeKey, value);
		}
	}

	public static bool LoupeEnabled
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_loupeKey);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_loupeKey, value);
		}
	}

	public static bool NoAds
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_noAdsKey);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_noAdsKey, value);
		}
	}

	public static bool SpecPageOpened
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_specPageOpenedKey);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_specPageOpenedKey, value);
		}
	}

	public static List<string> UnlockedImages
	{
		get
		{
			return PlayerPrefsWrapper.GetStringList(AppData.s_unlockedImages);
		}
		set
		{
			PlayerPrefsWrapper.SetStringList(AppData.s_unlockedImages, value);
		}
	}

	public static bool PictureCompleted
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_pictureDoneKey);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_pictureDoneKey, value);
		}
	}

	public static int PictureCompletedCount
	{
		get
		{
			return PlayerPrefsWrapper.GetInt(AppData.s_pictureDoneCountKey);
		}
		set
		{
			PlayerPrefsWrapper.SetInt(AppData.s_pictureDoneCountKey, value);
		}
	}

	public static bool LaunchNextDay
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_launchNextDayKey);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_launchNextDayKey, value);
		}
	}

	public static bool LaunchNext2Days
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_launchNext2DaysKey);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_launchNext2DaysKey, value);
		}
	}

	public static DateTime EndSessionTime
	{
		get
		{
			string str = PlayerPrefsWrapper.GetString(AppData.s_endSessionTime);
			if (!string.IsNullOrEmpty(str))
			{
				return DateTime.Parse(str);
			}
			return DateTime.MinValue;
		}
		set
		{
			PlayerPrefsWrapper.SetString(AppData.s_endSessionTime, value.ToString());
		}
	}

	public static DateTime LastDayTracked
	{
		get
		{
			string @string = PlayerPrefsWrapper.GetString(AppData.s_lastDayTracked);
			if (!string.IsNullOrEmpty(@string))
			{
				return DateTime.Parse(@string);
			}
			return DateTime.MinValue;
		}
		set
		{
			PlayerPrefsWrapper.SetString(AppData.s_lastDayTracked, value.ToString());
		}
	}

	public static int FreePhotos
	{
		get
		{
			return 2;
		}
		set
		{
			PlayerPrefsWrapper.SetInt(AppData.s_freePhotos, value);
		}
	}

	public static DateTime FirstLaunch
	{
		get
		{
			string @string = PlayerPrefsWrapper.GetString(AppData.s_firstLaunch);
			if (!string.IsNullOrEmpty(@string))
			{
				return DateTime.Parse(@string);
			}
			return DateTime.MinValue;
		}
		set
		{
			PlayerPrefsWrapper.SetString(AppData.s_firstLaunch, value.ToString());
		}
	}

	public static int IntersCount
	{
		get
		{
			return PlayerPrefsWrapper.GetInt(AppData.s_intersCount);
		}
		set
		{
			PlayerPrefsWrapper.SetInt(AppData.s_intersCount, value);
		}
	}

	public static int SecondsInGame
	{
		get
		{
			return PlayerPrefsWrapper.GetInt(AppData.s_secondsInGame);
		}
		set
		{
			PlayerPrefsWrapper.SetInt(AppData.s_secondsInGame, value);
		}
	}

	public static bool WeekSecondsTracked
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_weekSecondsTracked);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_weekSecondsTracked, value);
		}
	}

	public static bool SubCampaignWasShown
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_subCampaign);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_subCampaign, value);
		}
	}

	public static bool UpdateToNew
	{
		get
		{
			return PlayerPrefsWrapper.GetBool(AppData.s_updateToNew);
		}
		set
		{
			PlayerPrefsWrapper.SetBool(AppData.s_updateToNew, value);
		}
	}

	public static void Init()
	{
		AppData.Inited = true;
		string uid = AppData.Uid; 
		bool isOldUser = false;
		if (!PlayerPrefsWrapper.Exists(AppData.s_isOldUserKey))
		{
			AppData.IsOldUser = (AppData.SessionId > 0);
			UnityEngine.Debug.Log("IsOldUser: " + AppData.IsOldUser);
			isOldUser = true;
		}
		AppData.SessionId++;
		if (!PlayerPrefsWrapper.Exists(AppData.s_vibroRightKey))
		{
			if (isOldUser)
			{
				if (AppData.IsOldUser)
				{
					AppData.VibroRightEnabled = AppData.VibroEnabled;
					AppData.VibroWrongEnabled = false;
				}
				else
				{
					AppData.VibroRightEnabled = false;
					AppData.VibroWrongEnabled = true;
				}
			}
			else
			{
				AppData.VibroRightEnabled = false;
				AppData.VibroWrongEnabled = AppData.VibroEnabled;
			}
		}
		if (!PlayerPrefsWrapper.Exists(AppData.s_soundsKey))
		{
			AppData.SoundsEnabled = true;
		}
		if (!PlayerPrefsWrapper.Exists(AppData.s_lassoCountKey))
		{
			AppData.LassoCount = 5;
		}
		if (!PlayerPrefsWrapper.Exists(AppData.s_bombCountKey))
		{
			AppData.BombCount = 5;
		}
		if (!PlayerPrefsWrapper.Exists(AppData.s_bulbModeKey))
		{
			AppData.BulbMode = true;
		}
		if (!PlayerPrefsWrapper.Exists(AppData.s_loupeKey))
		{
			AppData.LoupeEnabled = false;
		}
		if (AppData.SessionId == 1)
		{
			AppData.FirstLaunch = DateTime.UtcNow;
			AppData.SecondsInGame = 0;
		}
		if (!PlayerPrefsWrapper.Exists(AppData.s_freePhotos))
		{
			AppData.FreePhotos = 2;
		}
		if (!PlayerPrefsWrapper.Exists(AppData.s_updateToNew))
		{
			AppData.UpdateToNew = (AppData.SessionId > 1);
		}
		AppData.CheckSessionEvents();
	}

	public static void CheckSessionEvents()
	{
		if (!PlayerPrefsWrapper.Exists(AppData.s_launchNextDayKey))
		{
			// AnalyticsManager.Instance.Test();
			try
			{
				DateTime installDate = SystemToolsWrapper.GetInstallDate();
				DateTime today = DateTime.Today;
				TimeSpan t = today - installDate;
				if (t == new TimeSpan(1, 0, 0, 0))
				{
					AnalyticsManager.Instance.LaunchNextDay();
					AnalyticsManager.Instance.LaunchNext2Days();
					AppData.LaunchNextDay = true;
					AppData.LaunchNext2Days = true;
				}
				else if (t == new TimeSpan(2, 0, 0, 0))
				{
					AnalyticsManager.Instance.LaunchNext2Days();
					AppData.LaunchNextDay = true;
					AppData.LaunchNext2Days = true;
				}
				if (t > new TimeSpan(2, 0, 0, 0))
				{
					AppData.LaunchNextDay = true;
					AppData.LaunchNext2Days = true;
				}
			}
			catch
			{
			}
		}
		if (PlayerPrefsWrapper.Exists(AppData.s_lastDayTracked) && !(AppData.LastDayTracked < DateTime.Today))
		{
			return;
		}
		if (INPluginWrapper.Instance.GetAbTestGroup() != 0 && INPluginWrapper.Instance.GetBannerStrategy() != BannerStrategy.Undefined)
		{
			AnalyticsManager.Instance.TrackFirstSessionOfTheDay();
			AppData.LastDayTracked = DateTime.Today;
		}
	}

	public static void AddNewRateUsView()
	{
		List<DateTime> list = AppData.LastRateUsShows;
		if (list == null)
		{
			list = new List<DateTime>();
		}
		list.Add(DateTime.Now);
		while (list.Count > 2)
		{
			list.RemoveAt(0);
		}
		AppData.LastRateUsShows = list;
	}
}


