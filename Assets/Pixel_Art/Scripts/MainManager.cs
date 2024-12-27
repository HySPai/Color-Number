/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using Assets.Scripts.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
	public string oneSignalAppId = "fbe964d0-9eee-4cf6-845d-174245eec26a";
	public string oneSignaliOSAppId = "d88013d2-c394-40ee-8036-d55175dc3f4b";
	public string googleProjectId = "718810063125";

	private bool first;

	private float m_secondsInGame;

	private EventSystem m_currentEventSystemInternal;

	private SavedWorksList m_savedWorksList;

	[SerializeField]
	private FreeImageSaver m_freeImageSaver;

	[SerializeField]
	private FilterManager m_filterManager;

	public TextAsset localization;

	public static MainManager Instance { get; private set; }

	public SavedWorksList SavedWorksList
	{
		get
		{
			return this.m_savedWorksList;
		}
	}

	public FreeImageSaver FreeImageSaver
	{
		get
		{
			return this.m_freeImageSaver;
		}
	}

	public FilterManager FilterManager
	{
		get
		{
			return this.m_filterManager;
		}
	}

	private EventSystem m_currentEventSystem
	{
		get
		{
			if ((UnityEngine.Object)this.m_currentEventSystemInternal == (UnityEngine.Object)null)
			{
				this.m_currentEventSystemInternal = EventSystem.current;
			}
			return this.m_currentEventSystemInternal;
		}
	}

	private void Awake()
	{
		MainManager.Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		base.StartCoroutine(this.InitializeCoroutine());
		this.m_secondsInGame = (float)AppData.SecondsInGame;
	}

	public void StartLibrary(MainMenuPage page = MainMenuPage.Library)
	{
		base.StartCoroutine(this.StartLibraryCoroutine(page));
	}

	public void StartWorkbook(ImageInfo imageInfo, ImageOpenType imageOpenType, MainMenuPage page, Action badHandler = null)
	{
		MainMenu.LastPage = page;
		MainMenu.ImageId = imageInfo.Id;
		MainMenu.WorkId = null;
		base.StartCoroutine(this.StartWorkbookCoroutine(imageInfo, imageOpenType, null, badHandler));
	}

	public void StartWorkbook(ISavedWorkData savedWorkData, ImageOpenType imageOpenType, Action badHandler = null)
	{
		base.StartCoroutine(this.StartWorkbookCoroutine(savedWorkData.ImageInfo, imageOpenType, savedWorkData, badHandler));
	}

	public void EnableEventSystem()
	{
		this.m_currentEventSystem.enabled = true;
	}

	public void DisableEventSystem()
	{
		this.m_currentEventSystem.enabled = false;
	}

	public void OnLowMemoryMessage(string message)
	{
		UnityEngine.Debug.Log("OnLowMemoryMessageReceived: " + message);
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	public void OnOpenWithUrl(string s)
	{
		Dictionary<string, string> dict = DeepLinkWrapper.Parse(s);
		if (dict != null && dict.ContainsKey("host") && dict["host"] == "main")
		{
			if ((UnityEngine.Object)IAPWrapper.Instance != (UnityEngine.Object)null && dict.ContainsKey("inapp"))
			{
				string inapp = dict["inapp"];
				UnityEngine.Debug.Log("try buy");
				IAPWrapper.Instance.BuyProductStoreSpecific(inapp, "start", delegate (bool result, SubscriptionType type, PurchaseFailureReason reason)
				{
					UnityEngine.Debug.Log(result + ": " + type);
					if (result && dict.ContainsKey("adid"))
					{
						string text = dict["adid"];
						UnityEngine.Debug.Log(text + " ... " + inapp);
						AnalyticsManager.Instance.PurchaseFromAd(inapp, text);
					}
				});
			}
			if (!dict.ContainsKey("from_image_local"))
			{
				return;
			}
		}
	}

	private void Update()
	{
		this.m_secondsInGame += Time.deltaTime;
		int num = 9768;
		if (this.m_secondsInGame >= (float)num && (DateTime.UtcNow - AppData.FirstLaunch).TotalDays <= 7.0 && !AppData.WeekSecondsTracked)
		{
			AnalyticsManager.Instance.TrackFirstWeekSecondsLimit();
			AppData.WeekSecondsTracked = true;
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		try
		{
			if (pauseStatus)
			{
				if (this.m_secondsInGame > (float)AppData.SecondsInGame)
				{
					AppData.SecondsInGame = (int)this.m_secondsInGame;
				}
				UnityEngine.Debug.Log("pause_AppData.SecondsInGame: " + AppData.SecondsInGame);
				AppData.EndSessionTime = DateTime.Now;
				if (SceneManager.GetActiveScene().name != "2DScene")
				{
					this.SavedWorksList.Save();
				}
				this.DisableEventSystem();
			}
			else
			{
				UnityEngine.Debug.Log("unpause_AppData.SecondsInGame: " + AppData.SecondsInGame);
				this.m_secondsInGame = (float)AppData.SecondsInGame;
				if ((DateTime.Now - AppData.EndSessionTime).TotalSeconds > 10.0)
				{
					AppData.CheckSessionEvents();
					DataManager.Instance.GetImages(true, delegate (ImagesInfo imagesInfo)
					{
						if (WindowManager.Instance != null)
						{
							MainMenu mainMenu = WindowManager.Instance.GetMainMenu();
							if ((UnityEngine.Object)mainMenu != (UnityEngine.Object)null)
							{
								mainMenu.UpdateLibrary(imagesInfo);
							}
						}
					});
				}
				this.EnableEventSystem();
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}
	private IEnumerator InitializeCoroutine()
	{
		DeepLinkWrapper.Init();
		AppPathsConfig.Init();
		ShareWrapper.Init();

		yield return null;
		AppData.Init();
#if FACEBOOK
        FacebookWrapper.Instance.Init(null);
#endif
		LocalNotificationWrapper.Instance.Init();
		SystemToolsWrapper.GetUid();
		yield return null;

		DataManager.Instance.Init();
		this.m_savedWorksList = new SavedWorksList();
		this.m_savedWorksList.Init();
		LocalizationManager.Instance.Init(this.localization.text);
		AdsWrapper.Instance.Init();
		float timer = 3f;

		while (INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.Undefined)
		{
			timer -= Time.deltaTime;
			if (timer < 0f)
			{
				UnityEngine.Debug.Log("NoResponse. SetDefaultGroup");
				INPluginWrapper.Instance.SetDefaultAbTestGroup(ABTestGroup.None);
			}
			yield return null;
		}
		UnityEngine.Debug.Log("ABTestGroup: " + INPluginWrapper.Instance.GetAbTestGroup());
		if (INPluginWrapper.Instance.GetBannerStrategy() == BannerStrategy.Undefined)
		{
			INPluginWrapper.Instance.SetDefaultBannerStrategy();
		}
		AppData.CheckSessionEvents();
		this.first = true;
		this.StartLibrary(MainMenuPage.Library);
	} 

	private IEnumerator LoadMenuSceneCoroutine()
	{
		yield return SceneManager.LoadSceneAsync("LibraryScene");
	} 

	private IEnumerator StartLibraryCoroutine(MainMenuPage page)
	{
		if (!SceneManager.GetActiveScene().name.Contains("LibraryScene"))
		{
			yield return this.StartCoroutine(this.LoadMenuSceneCoroutine()); 
		}

		yield return null; 

		var mainMenu = WindowManager.Instance.OpenMainMenu();
		mainMenu.Init(page);
		bool needTrialWindow = false; 
		if (this.first && !IAPWrapper.Instance.NoAds)
		{
			needTrialWindow = (AppData.SessionId == 1);
			if (!needTrialWindow && AppData.UpdateToNew && !AppData.SubCampaignWasShown)
			{ 
				needTrialWindow = true; 
			}
		}
		if (needTrialWindow)
		{
			this.first = false; 
			if (INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.RewardedYes_ContentHard 
				|| INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.RewardedYes_ContentMedium_New)
			{
				var trialInappsWindow = WindowManager.Instance.OpenInappsWindow();
				trialInappsWindow.Init("first_launch", null);
			}
			while (WindowManager.Instance.OpenedWindowsCount > 1)
			{
				yield return null;
			}
			yield return new WaitForSeconds(1f);

            try
            {
#if UNITY_ANDROID
			OneSignalWrapper.Init(oneSignalAppId, googleProjectId);
#else
                OneSignalWrapper.Init(oneSignaliOSAppId, googleProjectId);
#endif
            }
            catch(Exception ex)
            {
                Debug.LogError(ex);
            }
		}
	}

	private IEnumerator StartWorkbookCoroutine(ImageInfo imageInfo, ImageOpenType imageOpenType, ISavedWorkData savedWorkData = null, Action badHandler = null)
	{
		if (imageInfo.Is3D)
		{
			DataManager.Instance.GetImageAsset3D(imageInfo, delegate (bool res, CashImage3D vox)
			{
				if (res)
				{
					NavigationService.Navigate(new GameNavigationArgs(imageInfo, vox, savedWorkData as SavedWorkData3D, imageOpenType), true);
				}
				else
				{
					DialogToolWrapper.ShowNoInternetDialog();
					badHandler.SafeInvoke();
				}
			}); 
		}
		else
		{
			yield return SceneManager.LoadSceneAsync("2DScene");

			while (NewWorkbookManager.Instance == null)
			{
				yield return null;
			}
			Resources.UnloadUnusedAssets();
			NewWorkbookManager.Instance.Init(imageInfo, imageOpenType, savedWorkData);
		}
	}
}
