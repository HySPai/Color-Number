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
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShareWindow : BaseWindow
{
	//[SerializeField]
	//private GameObject m_watermark;

	[SerializeField]
	private RawImage m_image;

	[SerializeField]
	private Popup m_popup;

	[SerializeField]
	private GameObject m_okButton;

	[SerializeField]
	protected RawImage m_filterImage;

	[SerializeField]
	private Button m_facebookButton;

	[SerializeField]
	private ButtonEffectComponent m_facebookButtonEventTrigger;

	[SerializeField]
	private List<Image> m_facebookButtonImages;

	[SerializeField]
	private Button m_instButton;

	[SerializeField]
	private ButtonEffectComponent m_instButtonEventTrigger;

	[SerializeField]
	private List<Image> m_instButtonImages;

	[SerializeField]
	private GameObject m_playButton;

	[SerializeField]
	private Text m_title;

	private ISavedWorkData m_savedWorkData;

	private bool m_waitTimeForRateUs;

	private string m_videoPath;

	private bool m_isColoring;

	private bool m_completed;
	private Vector2 _imageSize; 

	protected override string WindowName
	{
		get
		{
			return "shareWindow";
		}
	}

	protected void Awake()
	{
		this.m_image.material = new Material(Shader.Find("Custom/GreyTextureShader"));
		this.m_filterImage.material = new Material(Shader.Find("Custom/TilingShader"));
		IAPWrapper instance = IAPWrapper.Instance;
		instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Combine(instance.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
		base.OnOpen = (Action)Delegate.Combine(base.OnOpen, new Action(this.OnOpenHandler));
	}

	public void Init(ISavedWorkData savedWorkData)
	{
		this.m_savedWorkData = savedWorkData;
		this.m_waitTimeForRateUs = false;
		this.ReinitImage(true);
		this.UpdateSocialButtons(); 
		this.m_title.text = LocalizationManager.Instance.GetString("share_picture");
	}

	public void InitFromLibrary(string saveId)
	{
		this.m_waitTimeForRateUs = false;
		this.m_popup.Init();
		this.m_savedWorkData = MainManager.Instance.SavedWorksList.LoadById(saveId);
		base.gameObject.SetActive(true);
		this.ReinitImage(false);
		this.UpdateSocialButtons(); 
		this.m_okButton.SetActive(false);
		this.m_title.text = LocalizationManager.Instance.GetString("share_picture");
	}

	private void OnOpenHandler()
	{
		this.m_isColoring = false;
		this.m_completed = true; 
		this.PlayButtonClick();
	}

	private void ReinitImage(bool withRect)
	{
		Texture2D resTex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		resTex.filterMode = FilterMode.Point;
		resTex.LoadImage(this.m_savedWorkData.Preview);
		this.m_image.material.SetTexture("_ResTex", resTex);
		this.m_image.enabled = true;
		this.m_filterImage.enabled = false;
		if (this.m_savedWorkData.ImageInfo.Url != null)
		{
			DataManager.Instance.GetImageAsset(this.m_savedWorkData.ImageInfo, delegate (bool res, Texture2D tex)
			{
				this.m_image.texture = tex;
				if (this.m_savedWorkData.Completed)
				{
					this.m_image.material.SetInt("_HideMainTex", 1);
				}
				Texture2D filter2 = MainManager.Instance.FilterManager.GetFilter(this.m_savedWorkData.FilterId);
				if (filter2 != null)
				{
					this.m_filterImage.material.mainTextureScale = new Vector2((float)resTex.width, (float)resTex.height);
					this.m_filterImage.texture = filter2;
					this.m_filterImage.enabled = true;

					_imageSize = new Vector2(this.m_image.texture.width, this.m_image.texture.height);
					this.m_filterImage.material.SetFloat("RepeatX", _imageSize.x);
					this.m_filterImage.material.SetFloat("RepeatY", _imageSize.y);
				}
				else
				{
					this.m_filterImage.enabled = false;
				}
				if (withRect)
				{
					this.m_image.uvRect = this.m_savedWorkData.UvRect1;
					this.m_filterImage.uvRect = this.m_image.uvRect;
				}
			});
		}
		else
		{
			DataManager.Instance.GetPhotoAsset(this.m_savedWorkData.ImageInfo.Id, delegate (bool res, Texture2D tex)
			{
				this.m_image.texture = tex;
				if (this.m_savedWorkData.Completed)
				{
					this.m_image.material.SetInt("_HideMainTex", 1);
				}
				Texture2D filter = MainManager.Instance.FilterManager.GetFilter(this.m_savedWorkData.FilterId);
				if (filter != null)
				{
					this.m_filterImage.material.mainTextureScale = new Vector2((float)resTex.width, (float)resTex.height);
					this.m_filterImage.texture = filter;
					this.m_filterImage.enabled = true;

					_imageSize = new Vector2(this.m_image.texture.width, this.m_image.texture.height);
					this.m_filterImage.material.SetFloat("RepeatX", _imageSize.x);
					this.m_filterImage.material.SetFloat("RepeatY", _imageSize.y);
				}
				else
				{
					this.m_filterImage.enabled = false;
				}
				if (withRect)
				{
					this.m_image.uvRect = this.m_savedWorkData.UvRect1;
					this.m_filterImage.uvRect = this.m_image.uvRect;
				}
			});
		}
	}

	private void UpdateSocialButtons()
	{
		bool flag = ShareWrapper.CanFbShare();
		this.m_facebookButton.interactable = flag;
		foreach (Image facebookButtonImage in this.m_facebookButtonImages)
		{
			if (facebookButtonImage != null)
			{
				Color color = facebookButtonImage.color;
				color.a = ((!flag) ? 0.5f : 1f);
				facebookButtonImage.color = color;
			}
		}
		flag = ShareWrapper.CanInstShare();
		this.m_instButton.interactable = flag;
		foreach (Image instButtonImage in this.m_instButtonImages)
		{
			if (instButtonImage != null)
			{
				Color color2 = instButtonImage.color;
				color2.a = ((!flag) ? 0.5f : 1f);
				instButtonImage.color = color2;
			}
		}
	}

	public void DeleteWatermark()
	{
		AnalyticsManager.Instance.DeleteWatermarkAttempt();
		switch (INPluginWrapper.Instance.GetAbTestGroup())
		{
			case ABTestGroup.RewardedYes_ContentMedium_New:
			case ABTestGroup.RewardedYes_ContentHard:
			case ABTestGroup.Rewarded_yes_content_medium_no1screen:
			case ABTestGroup.Rewarded_yes_content_hard_no1screen:
				{
					NewInappsWindow newInappsWindow = WindowManager.Instance.OpenInappsWindow();
					newInappsWindow.Init("watermark", null);
					break;
				}
			default:
				IAPWrapper.Instance.BuyProduct(SubscriptionType.remove_ads, "watermark", null);
				break;
		}
	}

	private void OnPurchaseHandler(bool res, SubscriptionType subscription)
	{ 
	} 

	public void SaveButtonClick()
	{
		if (!this.m_completed)
		{ 
			this.ShowMessage("Please wait...");
		}
		else
		{ 
			string filePath = AppPathsConfig.SavedImagesPath + this.m_savedWorkData.Id + ".png";
			MainManager.Instance.FreeImageSaver.GetFilteredImage(this.m_savedWorkData, delegate (byte[] bytes)
			{
				File.WriteAllBytes(filePath, bytes);
				this.SaveIntoGallery(filePath);
			}, this.m_savedWorkData.ImageInfo.Is3D); 
			AudioManager.Instance.PlayClick();
		}
	} 

	private void SaveIntoGallery(string filePath)
	{
		Debug.Log("Save into gallery: " + filePath);
		ShareWrapper.SaveIntoGallery(filePath, this.m_savedWorkData.Id, string.Empty, delegate (bool res)
		{
			this.ShowMessage((!res) ? "error" : "saved");
			if (res)
			{
				AnalyticsManager.Instance.SaveSuccess();
				base.StartCoroutine(this.ShowRateUsIfNeedCoroutine(RateUsReason.SaveInGallery));
			}
		});
	}

	public void ShareButtonClick()
	{
		if (!this.m_completed)
		{ 
			this.ShowMessage("Please wait...");
		}
		else
		{
			AnalyticsManager.Instance.ShareButtonClicked(); 
			string filePath = AppPathsConfig.SavedImagesPath + this.m_savedWorkData.Id + ".png";
			MainManager.Instance.FreeImageSaver.GetFilteredImage(this.m_savedWorkData, delegate (byte[] bytes)
			{
				File.WriteAllBytes(filePath, bytes);
				ShareWrapper.ShareImage("Share image..", "Subject", LocalizationManager.Instance.GetString("share_image_text"), filePath);
			}, this.m_savedWorkData.ImageInfo.Is3D); 
			AudioManager.Instance.PlayClick();
		}
	}

	public void FacebookButtonClick()
	{
		if (!this.m_completed)
		{ 
			this.ShowMessage("Please wait...");
		}
		else
		{
			AnalyticsManager.Instance.ShareViaFbButtonClicked(); 
			string filePath = AppPathsConfig.SavedImagesPath + this.m_savedWorkData.Id + ".png";
			MainManager.Instance.FreeImageSaver.GetFilteredImage(this.m_savedWorkData, delegate (byte[] bytes)
			{
				File.WriteAllBytes(filePath, bytes);
				ShareWrapper.ShareImageFb("Share image..", "Subject", LocalizationManager.Instance.GetString("share_image_text"), filePath);
			}, this.m_savedWorkData.ImageInfo.Is3D); 
			AudioManager.Instance.PlayClick();
		}
	}

	public void InstagramButtonClick()
	{
		if (!this.m_completed)
		{ 
			this.ShowMessage("Please wait...");
		}
		else
		{
			AnalyticsManager.Instance.ShareViaInstagramButtonClicked(); 
			string filePath = AppPathsConfig.SavedImagesPath + this.m_savedWorkData.Id + ".png";
			UnityEngine.Debug.Log(filePath);
			MainManager.Instance.FreeImageSaver.GetFilteredImage(this.m_savedWorkData, delegate (byte[] bytes)
			{
				File.WriteAllBytes(filePath, bytes);
				ShareWrapper.ShareImageInstagram("Share image..", "Subject", LocalizationManager.Instance.GetString("share_image_text"), filePath);
			}, this.m_savedWorkData.ImageInfo.Is3D); 
			AudioManager.Instance.PlayClick();
		}
	} 

	public void PlayButtonClick()
	{
	}


	private void ShowMessage(string text)
	{
		this.m_popup.gameObject.SetActive(true);
		this.m_popup.Show(LocalizationManager.Instance.GetString(text));
	}

	public void ReturnToMenu()
	{
		AudioManager.Instance.PlayClick();
		if (SceneManager.GetActiveScene().name == "LibraryScene")
		{
			WindowManager.Instance.CloseMe(this);
		}
		else
		{
			if (SceneManager.GetActiveScene().name == "2DScene")
			{
				NewWorkbookManager.Instance.SaveWork(false);
			}
			MainManager.Instance.StartLibrary(MainMenu.LastPage);
		}
	}

	public void CloseButtonClick()
	{
		WindowManager.Instance.CloseMe(this);
		AnalyticsManager.Instance.BackButtonClicked();
		AudioManager.Instance.PlayClick();
	}

	private void OnDestroy()
	{
		IAPWrapper instance = IAPWrapper.Instance;
		instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Remove(instance.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
	} 

	private IEnumerator ShowRateUsIfNeedCoroutine(RateUsReason reason)
	{
		this.m_waitTimeForRateUs = true;
		while (this.m_popup.isActiveAndEnabled)
		{
			yield return null;
		}
		yield return new WaitForSeconds(1f);

		if (this.m_waitTimeForRateUs)
		{
			this.m_waitTimeForRateUs = false;
			if (RateUsWindow.NeedShow(reason))
			{
				DialogToolWrapper.ShowRateUsDialog(null);
			}
		}
	}
}
