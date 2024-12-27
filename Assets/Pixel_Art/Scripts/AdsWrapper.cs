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
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdsWrapper : MonoBehaviour
{ 
	public string unitBanner = "ca-app-pub-8407856226410057/2457029536";
	public string unitInterstitial = "ca-app-pub-8407856226410057/6688806637";
	public string unitRewardedVideo = "ca-app-pub-8407856226410057/4432456187";
	public string unitNativeAdvanced = "ca-app-pub-8407856226410057/5687071370";

	#if UNITY_ANDROID
	public string appId = "ca-app-pub-8407856226410057~6779417928";
	#elif UNITY_IPHONE
	public string appId = "ca-app-pub-3940256099942544~1458002511";
	#else
	public string appId = "unexpected_platform";
	#endif
	
	public Action OnBannerEnableChanged;

	public Action OnVideoAvailabilityChanged;

	private float m_interstitialDelay;

	private bool m_bannerAvailable;

	private int m_bannerCounter;

	private bool m_bannerShowed;

	private bool s_isWorkbook;

	public static AdsWrapper Instance { get; private set; }

	public bool BannerEnabled { get; private set; }

	public BannerPosition BannerPosition
	{
		get
		{
			return INPluginWrapper.Instance.GetBannerPosition();
		}
	}

	public BannerStrategy BannerStrategy
	{
		get
		{
			return INPluginWrapper.Instance.GetBannerStrategy();
		}
	}

	public int BannersCount
	{
		get
		{
			return INPluginWrapper.Instance.BannersCount;
		}
	}

	public int BannerClicksCount
	{
		get
		{
			return INPluginWrapper.Instance.BannerClicksCount;
		}
	}

	public int IntersCount
	{
		get
		{
			return INPluginWrapper.Instance.IntersCount;
		}
	}

	public int InterClicksCount
	{
		get
		{
			return INPluginWrapper.Instance.InterClicksCount;
		}
	}

	private void Awake()
	{
		AdsWrapper.Instance = this;
	}

	public void Init()
	{
		IAPWrapper instance = IAPWrapper.Instance;
		instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Combine(instance.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
		INPluginWrapper.Instance.Init();
		INPluginWrapper instance2 = INPluginWrapper.Instance;
		instance2.OnVideoAvailabilityChanged = (Action)Delegate.Combine(instance2.OnVideoAvailabilityChanged, (Action)delegate
		{
			this.OnVideoAvailabilityChanged.SafeInvoke();
		});
		base.StartCoroutine(this.InterstitialDelayCounterCoroutine());
		this.OnPurchaseHandler(false, SubscriptionType.one_week_subscription);
		if (!IAPWrapper.Instance.NoAds)
		{
			INPluginWrapper.Instance.StartRewardedCaching();
		}
	}

	private IEnumerator InterstitialDelayCounterCoroutine()
	{
		this.m_interstitialDelay -= Time.deltaTime;
		yield return null;
	}

	public void OpenedWindowsCountChangedHandler()
	{
		this.UpdateBannerState();
	}

	private void OnPurchaseHandler(bool res, SubscriptionType type)
	{
		this.UpdateBannerState();
		if (IAPWrapper.Instance.NoAds)
		{
			INPluginWrapper.Instance.DisableAds();
		}
	}

	public void ShowBanner(string place)
	{
		if (!IAPWrapper.Instance.Subscribed)
		{
			INPluginWrapper.Instance.ShowBanner(place);
		}
	}

	private void HideBanner(string place)
	{
		INPluginWrapper.Instance.HideBanner(place);
	}

	private IEnumerator UpdateBannerStateCoroutine()
	{
		yield return new WaitForSeconds(1f);
		this.UpdateBannerState();
		yield return new WaitForSeconds(5f);
		this.UpdateBannerStateT();
	}

	public void UpdateBannerState()
	{
		bool flag = SceneManager.GetActiveScene().name == "2DScene" || SceneManager.GetActiveScene().name.Contains("3DScene");
		switch (this.BannerStrategy)
		{
			case BannerStrategy.Undefined:
				return;
			case BannerStrategy.AllScreens:
				this.BannerEnabled = (!IAPWrapper.Instance.NoAds && WindowManager.Instance != null && WindowManager.Instance.BannerEnabled);
				break;
			case BannerStrategy.OnlyGame:
				this.BannerEnabled = (!IAPWrapper.Instance.NoAds && WindowManager.Instance != null && WindowManager.Instance.BannerEnabled && flag);
				break;
		}
		this.OnBannerEnableChanged.SafeInvoke();
		UnityEngine.Debug.Log("ShowBanner: " + this.BannerEnabled);
		if (this.BannerEnabled)
		{
			this.ShowBanner((!flag) ? "menuScreen" : "gameScreen");
		}
		else
		{
			this.HideBanner((!flag) ? "menuScreen" : "gameScreen");
		}
	}

	public void UpdateBannerStateT()
	{
		bool flag = SceneManager.GetActiveScene().name == "2DScene" || SceneManager.GetActiveScene().name.Contains("3DScene");
		switch (this.BannerStrategy)
		{
			case BannerStrategy.Undefined:
				return;
			case BannerStrategy.AllScreens:
				this.BannerEnabled = (!IAPWrapper.Instance.NoAds && WindowManager.Instance != null && WindowManager.Instance.BannerEnabled);
				break;
			case BannerStrategy.OnlyGame:
				this.BannerEnabled = (!IAPWrapper.Instance.NoAds && WindowManager.Instance != null && WindowManager.Instance.BannerEnabled && flag);
				break;
		}
		this.OnBannerEnableChanged.SafeInvoke();
		if (this.BannerEnabled)
		{
			this.ShowBanner((!flag) ? "menuScreen" : "gameScreen");
		}
	}

	public bool ShowInter(string placement)
	{
		if (!IAPWrapper.Instance.NoAds && this.m_interstitialDelay <= 0f)
		{
			return INPluginWrapper.Instance.ShowInter(placement, 0);
		}
		return false;
	}

	public void ShowInterOrRate(string placement)
	{
		if (!IAPWrapper.Instance.NoAds)
		{
			if (this.m_interstitialDelay <= 0f)
			{
				INPluginWrapper.Instance.ShowInterOrRate(placement, 0);
			}
		}
		else
		{
			INPluginWrapper.Instance.ShowRate(placement);
		}
	}

	public void RequestVideo()
	{
		if (!IAPWrapper.Instance.NoAds)
		{
			INPluginWrapper.Instance.StartRewardedCaching();
		}
	}

	public bool IsVideoAvailable()
	{
		return INPluginWrapper.Instance.IsRewardedAvailable();
	}

	public void ShowVideo(string place, Action<bool> handler)
	{
		INPluginWrapper.Instance.ShowRewardedVideo(place, handler);
		if (!(place == "image") && !(place == "photo"))
		{
			return;
		}
		this.m_interstitialDelay = (float)INPluginWrapper.Instance.GetInterTimeout();
		UnityEngine.Debug.Log("m_interstitialDelay: " + this.m_interstitialDelay);
	}

	public void OnWorkbookUnload()
	{
		this.HideBanner((SceneManager.GetActiveScene().name != "2DScene" && !SceneManager.GetActiveScene().name.Contains("3DScene")) ? "menuScreen" : "gameScreen");
	}

	private void OnLevelWasLoaded(int level)
	{
		base.StartCoroutine(this.UpdateBannerStateCoroutine());
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			this.UpdateBannerState();
		}
	}

	public void ResetCounters()
	{
		INPluginWrapper.Instance.ResetCounters();
	}
}
