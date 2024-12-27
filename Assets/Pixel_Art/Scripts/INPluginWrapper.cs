/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using GoogleMobileAds.Api;
using System;
using System.Collections.Generic;
using UnityEngine;

public class INPluginWrapper : MonoBehaviour
{ 
	static Dictionary<string, int> f__switch_0024map1;
	private static INPluginWrapper s_instance;

	public Action OnVideoAvailabilityChanged;

	private Action<bool> m_rewardedCallback;

	private bool m_inited;

	private BannerView bannerView;
	private InterstitialAd interstitial;
	private NativeExpressAdView nativeExpressAdView;
	private RewardBasedVideoAd rewardBasedVideo;

	//private bool m_isVideoAvailable;

	public static INPluginWrapper Instance
	{
		get
		{
			if ((UnityEngine.Object)INPluginWrapper.s_instance == (UnityEngine.Object)null)
			{
				GameObject gameObject = new GameObject("AdsManager");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				INPluginWrapper.s_instance = gameObject.AddComponent<INPluginWrapper>();
			}
			return INPluginWrapper.s_instance;
		}
	}

	private void Awake()
	{
		INPluginWrapper.s_instance = this;
		DontDestroyOnLoad(gameObject);
	}
	// Returns an ad request with custom ad targeting.
	private AdRequest CreateAdRequest()
	{
		return new AdRequest.Builder()
			//.AddTestDevice(AdRequest.TestDeviceSimulator)
			//.AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
			//.AddKeyword("game")
			//.SetGender(Gender.Male)
			//.SetBirthday(new DateTime(1985, 1, 1))
			//.TagForChildDirectedTreatment(false)
			//.AddExtra("color_bg", "9B30FF")
			.Build();
	}

	private void RequestBanner()
	{
		// Clean up banner ad before creating a new one.
		if (this.bannerView != null)
		{
			this.bannerView.Destroy();
		}

		// Create a 320x50 banner at the top of the screen.
		this.bannerView = new BannerView(AdsWrapper.Instance.unitBanner, AdSize.SmartBanner, AdPosition.Top);

		// Register for ad events.
		this.bannerView.OnAdLoaded += this.HandleAdLoaded;
		this.bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
		this.bannerView.OnAdOpening += this.HandleAdOpened;
		this.bannerView.OnAdClosed += this.HandleAdClosed;
		//this.bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;

		// Load a banner ad.
		this.bannerView.LoadAd(this.CreateAdRequest());
	}

	private void RequestInterstitial()
	{
		// Clean up interstitial ad before creating a new one.
		if (this.interstitial != null)
		{
			this.interstitial.Destroy();
		}

		// Create an interstitial.
		this.interstitial = new InterstitialAd(AdsWrapper.Instance.unitInterstitial);

		// Register for ad events.
		this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
		this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
		this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
		this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
		//this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;

		// Load an interstitial ad.
		this.interstitial.LoadAd(this.CreateAdRequest());
	}

	private void RequestNativeExpressAdView()
	{
		// Clean up native express ad before creating a new one.
		if (this.nativeExpressAdView != null)
		{
			this.nativeExpressAdView.Destroy();
		}

		// Create a 320x150 native express ad at the top of the screen.
		this.nativeExpressAdView = new NativeExpressAdView(
			AdsWrapper.Instance.unitNativeAdvanced,
			new AdSize(320, 150),
			AdPosition.Top);

		// Register for ad events.
		//this.nativeExpressAdView.OnAdLoaded += this.HandleNativeExpressAdLoaded;
		//this.nativeExpressAdView.OnAdFailedToLoad += this.HandleNativeExpresseAdFailedToLoad;
		//this.nativeExpressAdView.OnAdOpening += this.HandleNativeExpressAdOpened;
		//this.nativeExpressAdView.OnAdClosed += this.HandleNativeExpressAdClosed;
		//this.nativeExpressAdView.OnAdLeavingApplication += this.HandleNativeExpressAdLeftApplication;

		// Load a native express ad.
		this.nativeExpressAdView.LoadAd(this.CreateAdRequest());
	}

	private void RequestRewardBasedVideo()
	{
		this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), AdsWrapper.Instance.unitRewardedVideo);
	}

	private void ShowInterstitial()
	{
		if (this.interstitial.IsLoaded())
		{
			this.interstitial.Show();
		}
		else
		{
			MonoBehaviour.print("Interstitial is not ready yet");
		}
	}

	private void ShowRewardBasedVideo()
	{
#if UNITY_EDITOR
		Debug.Log("Reward based video in Editor");
		HandleRewardBasedVideoRewarded(this, new Reward());
#else
		if (this.rewardBasedVideo.IsLoaded())
		{
			this.rewardBasedVideo.Show();
		}
		else
		{
			MonoBehaviour.print("Reward based video ad is not ready yet");
		}
#endif
	}

	public int BannersCount { get; private set; }

	public int BannerClicksCount { get; private set; }

	public int IntersCount { get; private set; }

	public int InterClicksCount { get; private set; }

	public void HandleAdLoaded(object sender, EventArgs args)
	{
		this.Log("UnityOnBannerLoaded");
	}

	public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		this.Log("UnityOnBannerFailed");
	}

	public void HandleAdOpened(object sender, EventArgs args)
	{
		this.BannersCount++;
		this.Log("UnityOnBannerImpression");
	}

	public void OnBannerClick(string par)
	{
		this.BannerClicksCount++;
		this.Log("UnityOnBannerClick");
	}

	public void HandleAdClosed(object sender, EventArgs args)
	{
		this.Log("UnityOnBannerPos");
	}

	public void HandleInterstitialClosed(object sender, EventArgs args)
	{
		this.Log("UnityOnInterDidDisappear");
	}

	public void OnInterWillDisappear(string par)
	{
		this.Log("UnityOnInterWillDisappear");
	}

	public void OnInterWillAppear(string par)
	{
		this.Log("UnityOnInterWillAppear");
	}

	public void HandleInterstitialLoaded(object sender, EventArgs args)
	{
		this.Log("UnityOnInterLoaded");
	}

	public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		this.Log("UnityOnInterFailed");
	}
	public System.Collections.IEnumerator ThisWillBeExecutedOnTheMainThread()
	{
		AppData.IntersCount++;
		UnityEngine.Debug.Log("AppData.IntersCount: " + AppData.IntersCount);
		if (AppData.IntersCount == 16 && (DateTime.UtcNow - AppData.FirstLaunch).TotalDays <= 7.0)
		{
			AnalyticsManager.Instance.TrackFirstWeekIntersLimit();
		}
		yield return null;
	}
	public void HandleInterstitialOpened(object sender, EventArgs args)
	{
		this.IntersCount++;
		this.Log("UnityOnInterDidAppear");
		UnityMainThreadDispatcher.Instance().Enqueue(ThisWillBeExecutedOnTheMainThread()); 
	}

	public void OnInterClick(string par)
	{
		this.InterClicksCount++;
		this.Log("UnityOnInterClick");
	}

	public void OnInterExpire(string par)
	{
		this.Log("UnityOnInterExpire");
	}

	public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
	{
		this.Log("UnityOnRewardedLoad");
		this.OnVideoAvailabilityChanged.SafeInvoke();
	}

	public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
	{
		this.Log("UnityOnRewardedUpdate");
		this.OnVideoAvailabilityChanged.SafeInvoke();
	}

	public void HandleRewardBasedVideoFailedToLoad(object sender, EventArgs args)
	{
		this.Log("UnityOnRewardedUpdate");
		this.OnVideoAvailabilityChanged.SafeInvoke();
	}

	public void HandleRewardBasedVideoRewarded(object sender, Reward args)
	{
		this.IntersCount++;
		this.Log("UnityOnGetReward");
		if (m_rewardedCallback != null)
			this.m_rewardedCallback.SafeInvoke(true);

		if (AdsWrapper.Instance != null)
			AdsWrapper.Instance.RequestVideo();
	}

	public void OnRewardedClick(string par)
	{
		this.InterClicksCount++;
		this.Log("UnityOnRewardedClick");
	}

	public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
	{
		this.Log("UnityOnRewardedDidAppear");
		this.OnVideoAvailabilityChanged.SafeInvoke();
	}

	public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
	{
		this.Log("UnityOnRewardedWillDisappear");
	}

	public void OnRewardedDidExpire(string par)
	{
		this.Log("UnityOnRewardedDidExpire");
		this.OnVideoAvailabilityChanged.SafeInvoke();
	}

	private void Log(string s)
	{
	}

	public void Init()
	{
		IAPWrapper instance = IAPWrapper.Instance;
		instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Combine(instance.OnPurchase, (Action<bool, SubscriptionType>)delegate
		{
			this.EnableInterCaching();
		});

		//MobileAds.SetiOSAppPauseOnBackground(true);

		// Initialize the Google Mobile Ads SDK.
		MobileAds.Initialize(AdsWrapper.Instance.appId);

		// Get singleton reward based video ad reference.
		this.rewardBasedVideo = RewardBasedVideoAd.Instance;

		// RewardBasedVideoAd is a singleton, so handlers should only be registered once.
		this.rewardBasedVideo.OnAdLoaded -= this.HandleRewardBasedVideoLoaded;
		this.rewardBasedVideo.OnAdFailedToLoad -= this.HandleRewardBasedVideoFailedToLoad;
		this.rewardBasedVideo.OnAdOpening -= this.HandleRewardBasedVideoOpened;
		this.rewardBasedVideo.OnAdStarted -= this.HandleRewardBasedVideoStarted;
		this.rewardBasedVideo.OnAdRewarded -= this.HandleRewardBasedVideoRewarded;
		this.rewardBasedVideo.OnAdClosed -= this.HandleRewardBasedVideoClosed;

		this.rewardBasedVideo.OnAdLoaded += this.HandleRewardBasedVideoLoaded;
		this.rewardBasedVideo.OnAdFailedToLoad += this.HandleRewardBasedVideoFailedToLoad;
		this.rewardBasedVideo.OnAdOpening += this.HandleRewardBasedVideoOpened;
		this.rewardBasedVideo.OnAdStarted += this.HandleRewardBasedVideoStarted;
		this.rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
		this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;

		RequestInterstitial();
		//this.rewardBasedVideo.OnAdLeavingApplication += this.HandleRewardBasedVideoLeftApplication;
	}

	private void EnableInterCaching()
	{
	}

	public void EnableLogging(bool value)
	{
	}

	public void ShowBanner(string place)
	{
		RequestBanner();
	}

	public void HideBanner(string place)
	{
		if (this.bannerView != null)
		{
			this.bannerView.Hide();
		}
	}

	public void DisableAds()
	{
	}

	public int CheckPlacement(string s, int filter)
	{
		return -1;
	}

	public bool ShowInter(string s, int filter)
	{
		this.ShowInterstitial();
		return true;
	}

	public void ShowInterOrRate(string s, int filter)
	{
		this.ShowInterstitial();
	}

	public void ShowRate(string placement)
	{
	}

	public void StartRewardedCaching()
	{
		RequestRewardBasedVideo();
	}

	public bool IsRewardedAvailable()
	{
#if UNITY_EDITOR
		return true;
#else
		return this.rewardBasedVideo.IsLoaded();
#endif
	}

	public void ShowRewardedVideo(string place, Action<bool> handler)
	{
		if (!rewardBasedVideo.IsLoaded())
		{
			MessagePanel.ShowMessage("Ad is not ready. Please try Premium pack to continue.");
			return;
		}
		Debug.Log("ShowRewardedVideo");
		m_rewardedCallback = handler;
		ShowRewardBasedVideo();
	}

	public BannerPosition GetBannerPosition()
	{
		return BannerPosition.Top;
		//#if UNITY_EDITOR
		//#elif UNITY_ANDROID
		//        if (this.m_wrapper != null)
		//        {
		//            switch (((AndroidJavaObject)this.m_wrapper).CallStatic<string>("GetBannerStrategy", new object[0]))
		//            {
		//                case "top_all":
		//                case "top_game_sc":
		//                    return BannerPosition.Top;
		//                case "bottom_all":
		//                case "bottom_game_sc":
		//                    return BannerPosition.Bottom;
		//                default:
		//                    return BannerPosition.Undefined;
		//            }
		//        }
		//#endif
		//        return BannerPosition.Undefined;
	}

	public BannerStrategy GetBannerStrategy()
	{
		return BannerStrategy.Undefined;
		//#if UNITY_EDITOR
		//#elif UNITY_ANDROID
		//        if (this.m_wrapper != null)
		//        {
		//            switch (((AndroidJavaObject)this.m_wrapper).CallStatic<string>("GetBannerStrategy", new object[0]))
		//            {
		//                case "top_all":
		//                case "bottom_all":
		//                    return BannerStrategy.AllScreens;
		//                case "top_game_sc":
		//                case "bottom_game_sc":
		//                    return BannerStrategy.OnlyGame;
		//                default:
		//                    return BannerStrategy.Undefined;
		//            }
		//        }
		//#endif
		//		return BannerStrategy.Undefined;
	}

	public string GetBannerStrategyStr()
	{
		return "bottom";
		//#if UNITY_EDITOR
		//		return string.Empty;
		//#elif UNITY_ANDROID
		//        return (this.m_wrapper != null) ? ((AndroidJavaObject)this.m_wrapper).CallStatic<string>("GetBannerStrategy", new object[0]) : string.Empty;
		//#endif
	}

	public void SetActiveScreen(string screen)
	{
	}

	public bool IsAdShown()
	{
		return false;
	}

	public ABTestGroup GetAbTestGroup()
	{
		return ABTestGroup.RewardedYes_ContentMedium_New;
		 
	}

	public string GetAbTestGroupStr()
	{
		//#if UNITY_EDITOR
		return string.Empty;
		//#elif UNITY_ANDROID
		//        return ((AndroidJavaObject)this.m_wrapper).CallStatic<string>("EbGetAbTestGroup", new object[0]);
		//#endif
	}

	public void SetDefaultAbTestGroup(ABTestGroup group)
	{
		//#if UNITY_EDITOR
		//#elif UNITY_ANDROID
		//        this.m_wrapper.CallStatic("EbSetAbTestGroup", "iap_ab_testing_out");
		//#endif
	}

	public void SetDefaultBannerStrategy()
	{
		//#if UNITY_EDITOR
		//#elif UNITY_ANDROID
		//        this.m_wrapper.CallStatic("EbSetBannerStrategy", "top_game_sc");
		//#endif
	}

	public int GetInterTimeout()
	{
		//#if UNITY_EDITOR
		return 10;
		//#elif UNITY_ANDROID
		//        return ((AndroidJavaObject)this.m_wrapper).CallStatic<int>("EbGetInterTimeout", new object[0]);
		//#endif
	}

	public void UpdateSoundState()
	{
		//#if UNITY_EDITOR
		//#elif UNITY_ANDROID
		//        this.m_wrapper.CallStatic("EbAdsSetSoundMuted", !AppData.SoundsEnabled);
		//#endif
	}

	private string GroupToString(ABTestGroup group)
	{
		switch (group)
		{
			case ABTestGroup.None:
				return "iap_ab_testing_out";
			case ABTestGroup.RewardedNo_ContentEasy:
				return "rewarded_no_content_easy";
			case ABTestGroup.RewardedNo_ContentHard:
				return "rewarded_no_content_hard";
			case ABTestGroup.RewardedNo_ContentMedium:
				return "rewarded_no_content_medium";
			case ABTestGroup.RewardedNo_ContentMedium_Old:
				return "rewarded_no_content_medium_old";
			case ABTestGroup.RewardedYes_ContentHard:
				return "rewarded_yes_content_hard";
			case ABTestGroup.RewardedYes_ContentMedium_New:
				return "rewarded_yes_content_medium_new";
			case ABTestGroup.Rewarded_yes_content_medium_no1screen:
				return "rewarded_yes_content_medium_no1screen";
			case ABTestGroup.Rewarded_yes_content_hard_no1screen:
				return "rewarded_yes_content_hard_no1screen";
			default:
				return "iap_ab_testing_out";
		}
	}

	public void ResetCounters()
	{
		this.BannersCount = 0;
		this.BannerClicksCount = 0;
		this.IntersCount = 0;
		this.InterClicksCount = 0;
	}
}


