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
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : BaseWindow
{
	private bool restoredAnything;

	[SerializeField]
	private Popup m_popup;

	[SerializeField]
	private GameObject m_vibroRightPart;

	[SerializeField]
	private GameObject m_vibroWrongPart;

	[SerializeField]
	private Switcher m_vibroRightSwitcher;

	[SerializeField]
	private Switcher m_vibroWrongSwitcher;

	[SerializeField]
	private Switcher m_soundsSwitcher;

	[SerializeField]
	private Switcher m_highlightCellsSwitcher;

	[SerializeField]
	private Switcher m_loupeSwitcher;

	[SerializeField]
	private GameObject m_removeAdsButton;

	[SerializeField]
	private Text m_groupText;

	protected override string WindowName
	{
		get
		{
			return "mainMenu_moreTab";
		}
	}

	private void OnGUI()
	{
	}

	private void Awake()
	{
		this.m_vibroRightSwitcher.UpdateState(AppData.VibroRightEnabled, true);
		this.m_vibroWrongSwitcher.UpdateState(AppData.VibroWrongEnabled, true);
		this.m_soundsSwitcher.UpdateState(AppData.SoundsEnabled, true);
		this.m_highlightCellsSwitcher.UpdateState(AppData.BulbMode, true);
		this.m_loupeSwitcher.UpdateState(AppData.LoupeEnabled, true);
		this.m_vibroRightPart.SetActive(VibroWrapper.IsVibroAvailable());
		this.m_vibroWrongPart.SetActive(VibroWrapper.IsVibroAvailable());
		IAPWrapper instance = IAPWrapper.Instance;
		instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Combine(instance.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandlerRemoveAds));
		this.OnPurchaseHandlerRemoveAds(false, SubscriptionType.remove_ads);
	}

	public void VibroRightButtonClick()
	{
		AppData.VibroRightEnabled = !AppData.VibroRightEnabled;
		this.m_vibroRightSwitcher.UpdateState(AppData.VibroRightEnabled, false);
		AudioManager.Instance.PlayClick();
	}

	public void VibroWrongButtonClick()
	{
		AppData.VibroWrongEnabled = !AppData.VibroWrongEnabled;
		this.m_vibroWrongSwitcher.UpdateState(AppData.VibroWrongEnabled, false);
		AudioManager.Instance.PlayClick();
	}

	public void SoundButtonClick()
	{
		AppData.SoundsEnabled = !AppData.SoundsEnabled;
		this.m_soundsSwitcher.UpdateState(AppData.SoundsEnabled, false);
		AudioManager.Instance.PlayClick();
		INPluginWrapper.Instance.UpdateSoundState();

		if (BackgroundMusic.musicMgr != null)
		{
			BackgroundMusic.musicMgr.RefreshSettings();
		}
		if (AudioManager.Instance != null)
		{
			AudioManager.Instance.RefreshSettings();
		}
	}

	public void HighlightCellsClick()
	{
		AppData.BulbMode = !AppData.BulbMode;
		this.m_highlightCellsSwitcher.UpdateState(AppData.BulbMode, false);
		AudioManager.Instance.PlayClick();
	}

	public void LoupeClick()
	{
		AppData.LoupeEnabled = !AppData.LoupeEnabled;
		this.m_loupeSwitcher.UpdateState(AppData.LoupeEnabled, false);
		AudioManager.Instance.PlayClick();
	}

	public void TutorialButtonClick()
	{
		TutorialWindow tutorialWindow = WindowManager.Instance.OpenTutorial();
		tutorialWindow.Init("settings", "2D");
		AudioManager.Instance.PlayClick();
	}

	public void Tutorial3DButtonClick()
	{
		TutorialWindow tutorialWindow = WindowManager.Instance.OpenTutorial3D();
		tutorialWindow.Init("settings", "3D");
		AudioManager.Instance.PlayClick();
	}

	public void RateUsButtonClick()
	{
		AnalyticsManager.Instance.RateUsClicked();
		RateUsTool.OpenRateUs();
		AudioManager.Instance.PlayClick();
	}

	public void HelpButtonClick()
	{
		AudioManager.Instance.PlayClick();
	}

	public void SupportButtonClick()
	{
		AudioManager.Instance.PlayClick();
	}

	public void ShareAppButtonClick()
	{
		ShareWrapper.ShareApp();
	}

	public void TermsButtonClick()
	{
		Application.OpenURL(AppPathsConfig.TermsUrl);
	}

	public void PrivacyButtonClick()
	{
		Application.OpenURL(AppPathsConfig.PrivacyUrl);
	}

	public void MoreAppsButtonClick()
	{
		AnalyticsManager.Instance.MoreAppsClicked();
		Application.OpenURL("https://play.google.com/store/apps/dev?id=6817085628932553524");
	}

	public void RemoveAdsClick()
	{
		IAPWrapper.Instance.BuyProduct(SubscriptionType.remove_ads, "settings", null);
	}

	private void OnPurchaseHandlerRemoveAds(bool res, SubscriptionType subscrType)
	{
		switch (INPluginWrapper.Instance.GetAbTestGroup())
		{
			case ABTestGroup.None:
			case ABTestGroup.RewardedNo_ContentEasy:
			case ABTestGroup.RewardedNo_ContentMedium_Old:
			case ABTestGroup.RewardedNo_ContentMedium_Revealed:
				this.m_removeAdsButton.SetActive(!IAPWrapper.Instance.NoAds);
				break;
			default:
				this.m_removeAdsButton.SetActive(false);
				break;
		}
	}

	public void RestoreButtonClick()
	{
		IAPWrapper instance = IAPWrapper.Instance;
		instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Combine(instance.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
		base.StartCoroutine(this.WaitForRestored());
		IAPWrapper.Instance.RestorePurchases(null);
		AnalyticsManager.Instance.RestoreButtonClicked();
		AudioManager.Instance.PlayClick();
	}

	private void OnPurchaseHandler(bool res, SubscriptionType type)
	{
		this.restoredAnything = true;
		IAPWrapper instance = IAPWrapper.Instance;
		instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Remove(instance.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
	}
	public void ChangeGroupButtonClick()
	{
		ABTestGroup abTestGroup = INPluginWrapper.Instance.GetAbTestGroup();
		abTestGroup++;
		if (abTestGroup > ABTestGroup.RewardedNo_ContentMedium_Revealed)
		{
			abTestGroup = ABTestGroup.None;
		}
		INPluginWrapper.Instance.SetDefaultAbTestGroup(abTestGroup);
		this.UpdateGroupText();
		this.OnPurchaseHandlerRemoveAds(false, SubscriptionType.remove_ads);
	}

	private void UpdateGroupText()
	{
		this.m_groupText.text = INPluginWrapper.Instance.GetAbTestGroup().ToString();
	}

	public void CloseButtonClick()
	{
		WindowManager.Instance.CloseMe(this);
		AnalyticsManager.Instance.BackButtonClicked();
		AudioManager.Instance.PlayClick();
	}

	private void OnEnable()
	{
		this.UpdateGroupText();
	}

	private void OnDestroy()
	{
		IAPWrapper instance = IAPWrapper.Instance;
		instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Remove(instance.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandlerRemoveAds));
	}
	private IEnumerator WaitForRestored()
	{
		var timer = 2f;
		while (true)
		{
			if (timer > 0f)
			{
				timer -= Time.deltaTime;
				yield return null;
				continue;
			}
			IAPWrapper instance = IAPWrapper.Instance;
			instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Remove(instance.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
			if (this.restoredAnything)
			{
				if (IAPWrapper.Instance.Subscribed)
				{
					AnalyticsManager.Instance.PurchasesRestored();
					this.m_popup.Show(LocalizationManager.Instance.GetString("restored1"));
				}
				else
				{
					this.m_popup.Show(LocalizationManager.Instance.GetString("restored2"));
				}
			}
			else
			{
				this.m_popup.Show(LocalizationManager.Instance.GetString("restored0"));
			}
			yield break;
		}
	}
}
