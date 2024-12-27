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
using UnityEngine.UI;

public class IapPopup : BaseWindow
{
	private Action<bool> FinishHandler;

	private ImageInfo m_imageInfo;

	[SerializeField]
	private GameObject m_subscrPart;

	[SerializeField]
	private GameObject m_subscrVideoPart;

	[SerializeField]
	private GameObject mozaic;

	[SerializeField]
	private GameObject mozaic2;

	[SerializeField]
	private RawImage m_image1;

	[SerializeField]
	private RawImage m_image2;

	[SerializeField]
	private Image m_picImage1;

	[SerializeField]
	private Image m_picImage2;

	[SerializeField]
	private Sprite m_defaultTex;

	[SerializeField]
	private Sprite m_bombTex;

	[SerializeField]
	private Sprite m_lassoTex;

	[SerializeField]
	private Text m_changableText;

	private ImagePreview m_imagePreview;
	private string m_place = string.Empty;

	protected override string WindowName
	{
		get
		{
			return "abtestWindow";
		}
	}

	private void Awake()
	{
		RawImage image = this.m_image1;
		Material material = new Material(Shader.Find("Custom/GreyTextureShader"));
		this.m_image2.material = material;
		image.material = material;
		IAPWrapper instance = IAPWrapper.Instance;
		instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Combine(instance.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
	}

	public void Init(ImageInfo imageInfo, ImagePreview imagePreview)
	{
		this.m_place = "image";
		this.m_imagePreview = imagePreview;

		AnalyticsManager.Instance.ABTestWindowOpened(this.m_place);
		this.m_picImage1.gameObject.SetActive(false);
		this.m_picImage2.gameObject.SetActive(false);
		this.m_image1.gameObject.SetActive(true);
		this.m_image2.gameObject.SetActive(true);
		this.m_image1.texture = null;
		this.m_image2.texture = null;
		DataManager.Instance.GetImageAsset(imageInfo, delegate (bool res, Texture2D tex)
		{
			this.m_image1.texture = tex;
			this.m_image2.texture = tex;
		});
		this.m_imageInfo = imageInfo;
		this.SwitchAbTestGroup(INPluginWrapper.Instance.GetAbTestGroup());
		this.FinishHandler = null;
		this.m_changableText.text = LocalizationManager.Instance.GetString("get_this_image");
		this.CheckVisibleImage();
	}

	public void Init(Action<bool> handler, ABTestGroup abTestGroup, AbTestWindowMode mode)
	{
		this.CheckVisibleImage();
		this.m_imageInfo = null;
		this.FinishHandler = handler;
		this.SwitchAbTestGroup(abTestGroup);
		Sprite sprite = this.m_defaultTex;
		switch (mode)
		{
			case AbTestWindowMode.Bomb:
				sprite = this.m_bombTex;
				this.m_place = "grenade";
				this.m_changableText.text = LocalizationManager.Instance.GetString("get_bomb");
				break;
			case AbTestWindowMode.Lasso:
				sprite = this.m_lassoTex;
				this.m_place = "wand";
				this.m_changableText.text = LocalizationManager.Instance.GetString("get_wand");
				break;
			case AbTestWindowMode.Photo:
				sprite = this.m_defaultTex;
				this.m_place = "photo";
				this.m_changableText.text = LocalizationManager.Instance.GetString("create_art");
				break;
		}
		AnalyticsManager.Instance.ABTestWindowOpened(this.m_place);
		this.m_picImage1.gameObject.SetActive(true);
		this.m_picImage2.gameObject.SetActive(true);
		this.m_image1.gameObject.SetActive(false);
		this.m_image2.gameObject.SetActive(false);
		this.m_picImage1.sprite = sprite;
		this.m_picImage2.sprite = sprite;
	}

	private void CheckVisibleImage()
	{
		if (this.m_imageInfo != null)
		{
			if (IAPWrapper.Instance.NoAds || IAPWrapper.Instance.Subscribed)
			{
				this.mozaic.SetActive(false);
			}
			else if (INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.None || INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.RewardedNo_ContentMedium_Old)
			{
				this.mozaic.SetActive(this.m_imageInfo.AccessStatus != 0 && !IAPWrapper.Instance.Subscribed && !AppData.UnlockedImages.Contains(this.m_imageInfo.Id));
			}
			else if (INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.RewardedNo_ContentMedium_Revealed)
			{
				this.mozaic.SetActive(false);
			}
			else
			{
				bool active = this.m_imageInfo.CustomAccessStatus != 0 && !IAPWrapper.Instance.Subscribed && !AppData.UnlockedImages.Contains(this.m_imageInfo.Id);
				this.mozaic.SetActive(active);
			}
		}
		else
		{
			this.mozaic.SetActive(false);
		}
	}

	private void SwitchAbTestGroup(ABTestGroup abTestGroup)
	{
		switch (abTestGroup)
		{
			case ABTestGroup.RewardedNo_ContentMedium:
			case ABTestGroup.RewardedNo_ContentHard:
			case ABTestGroup.RewardedNo_ContentEasy:
				this.m_subscrPart.SetActive(true);
				this.m_subscrVideoPart.SetActive(false);
				break;
			default:
				if (AdsWrapper.Instance.IsVideoAvailable() && m_imagePreview != null && m_imagePreview.AdsAvailable)
				{
					this.m_subscrPart.SetActive(false);
					this.m_subscrVideoPart.SetActive(true);
				}
				else
				{
					this.m_subscrPart.SetActive(true);
					this.m_subscrVideoPart.SetActive(false);
				}
				break;
		}
	}

	public void BuyButtonClick()
	{
		ABTestGroup abTestGroup = INPluginWrapper.Instance.GetAbTestGroup();
		//if (abTestGroup == ABTestGroup.RewardedNo_ContentMedium_Old || abTestGroup == ABTestGroup.None)
		{
			NewInappsWindow newInappsWindow = WindowManager.Instance.OpenInappsWindow();
			newInappsWindow.Init(this.m_place, null);
			WindowManager.Instance.CloseMe(this);
		}
		//else
		//{
		//	TrialInappsWindow trialInappsWindow = WindowManager.Instance.OpenTrialInappsWindow();
		//	trialInappsWindow.Init(this.m_place, null, false, false);
		//	WindowManager.Instance.CloseMe(this);
		//}
		AnalyticsManager.Instance.ABTestWindowAction("subscribe", this.m_place);
	}

	public void WatchButtonClick()
	{
		AdsWrapper.Instance.ShowVideo(this.m_place, delegate (bool res)
		{
			if (res)
			{
				if (this.m_imageInfo != null)
				{
					List<string> unlockedImages = AppData.UnlockedImages;
					unlockedImages.Add(this.m_imageInfo.Id);
					AppData.UnlockedImages = unlockedImages;
					MainManager.Instance.StartWorkbook(this.m_imageInfo, ImageOpenType.New, MainMenuPage.Library, null);
				}
				else
				{
					this.FinishHandler.SafeInvoke(true);
				}
			}
		});
		AnalyticsManager.Instance.ABTestWindowAction("view_rewarded", this.m_place);
	}

	public void CloseButtonClick()
	{
		WindowManager.Instance.CloseMe(this);
		AnalyticsManager.Instance.ABTestWindowAction("close", this.m_place);
	}

	private void OnPurchaseHandler(bool res, SubscriptionType type)
	{
		if (base.gameObject.activeSelf)
		{
			WindowManager.Instance.CloseMe(this);
		}
	}

	private void OnDestroy()
	{
		IAPWrapper instance = IAPWrapper.Instance;
		instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Remove(instance.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
	}
}


