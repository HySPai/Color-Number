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
using UnityEngine.UI;

public class PhotoPreview : MonoBehaviour
{
	public Action<PhotoInfo, PhotoPreview> OnClick;

	private PhotoInfo m_photoInfo;

	private bool m_loadIcon;

	private bool m_loadedIcon;

	private bool m_materialInited;

	[SerializeField]
	private RawImage m_image;

	[SerializeField]
	private RawImage m_filterImage;

	[SerializeField]
	private Text m_title;

	[SerializeField]
	private GameObject m_imageWaiter;

	[SerializeField]
	private GameObject m_completedMark;

	[SerializeField]
	private GameObject m_plus;

	[SerializeField]
	private GameObject m_adIcon;

	[SerializeField]
	private GameObject m_premiumIcon;

	[SerializeField]
	private Sprite m_enabledPlusSprite;

	[SerializeField]
	private Sprite m_disabledPlusSprite;

	public bool Inited
	{
		get
		{
			return this.m_photoInfo != null;
		}
	}

	public bool Loaded
	{
		get
		{
			return this.m_image.texture != null;
		}
	}

	public string PhotoId
	{
		get
		{
			return (this.m_photoInfo != null) ? this.m_photoInfo.Id : null;
		}
	}

	public Texture2D Texture
	{
		get
		{
			return (Texture2D)this.m_image.texture;
		}
	}

	private void Awake()
	{
		DebugController instance = DebugController.Instance;
		this.m_filterImage.material = new Material(Shader.Find("Custom/TilingShader"));
		IAPWrapper instance2 = IAPWrapper.Instance;
		instance2.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Combine(instance2.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
		AdsWrapper instance3 = AdsWrapper.Instance;
		instance3.OnVideoAvailabilityChanged = (Action)Delegate.Combine(instance3.OnVideoAvailabilityChanged, new Action(this.OnVideoAvailabilityChangedHandler));
	}

	public void Init(PhotoInfo photoInfo)
	{
		if (this.m_title != null && photoInfo != null)
		{
			this.m_title.text = photoInfo.Id;
		}
		this.m_photoInfo = photoInfo;
		this.m_image.texture = null;
		this.m_loadIcon = false;
		this.m_loadedIcon = false;
		if (!this.m_materialInited)
		{
			this.m_image.material = new Material(Shader.Find("Custom/GreyTextureShader"));
			this.m_materialInited = true;
		}
		this.m_image.material.SetTexture("_ResTex", null);
		this.Reinit();
		this.m_completedMark.SetActive(false);
		this.OnVideoAvailabilityChangedHandler();
	}

	public void LoadIcon()
	{
		if (string.IsNullOrEmpty(this.m_photoInfo.Id))
		{
			this.m_image.gameObject.SetActive(false);
			this.m_plus.SetActive(true);
			this.m_adIcon.SetActive(false);
			this.m_premiumIcon.SetActive(false);
		}
		else
		{
			this.m_plus.SetActive(false);
			if (!this.m_loadedIcon)
			{
				if (!this.m_loadIcon)
				{
					this.m_loadIcon = true;
					this.m_imageWaiter.SetActive(true);
					DataManager.Instance.GetPhotoAsset(this.m_photoInfo.Id, delegate (bool res, Texture2D tex)
					{
						if (res)
						{
							try
							{
								this.m_loadedIcon = true;
								this.m_image.texture = tex;
								if (tex.width > tex.height)
								{
									this.m_image.transform.localScale = new Vector3(1f, (float)tex.height / (float)tex.width, 1f);
								}
								else
								{
									this.m_image.transform.localScale = new Vector3((float)tex.width / (float)tex.height, 1f, 1f);
								}
								this.m_imageWaiter.SetActive(false);
								this.m_image.gameObject.SetActive(true);
								string text = MainManager.Instance.SavedWorksList.LastSaveOfImageId(this.m_photoInfo.Id);
								if (text != null)
								{
									Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
									texture2D.filterMode = FilterMode.Point;
									ISavedWorkData savedWorkData = MainManager.Instance.SavedWorksList.LoadById(text);
									texture2D.LoadImage(savedWorkData.Preview);
									this.m_image.material.SetTexture("_ResTex", texture2D);
									this.m_completedMark.SetActive(savedWorkData.Completed);
									Texture2D filter = MainManager.Instance.FilterManager.GetFilter(savedWorkData.FilterId);
									if (filter != null)
									{
										this.m_filterImage.material.mainTextureScale = new Vector2((float)texture2D.width, (float)texture2D.height);
										this.m_filterImage.texture = filter;
										this.m_filterImage.enabled = true;
										this.m_filterImage.transform.localScale = this.m_image.transform.localScale;

										this.m_filterImage.material.SetFloat("RepeatX", texture2D.width);
										this.m_filterImage.material.SetFloat("RepeatY", texture2D.height);
									}
									else
									{
										this.m_filterImage.enabled = false;
									}
								}
								else
								{
									this.m_filterImage.enabled = false;
									this.m_image.uvRect = new Rect(0f, 0f, 1f, 1f);
									this.m_filterImage.uvRect = this.m_image.uvRect;
								}
							}
							catch (Exception ex)
							{
								UnityEngine.Debug.Log(ex.Message);
							}
						}
						this.m_loadIcon = false;
					});
				}
			}
			else
			{
				this.m_image.gameObject.SetActive(true);
			}
		}
	}

	public void UnloadIcon()
	{
		this.m_loadIcon = false;
		this.m_image.gameObject.SetActive(false);
	}

	public void Reinit()
	{
	}

	public void Click()
	{
		this.OnClick.SafeInvoke(this.m_photoInfo, this);
		AudioManager.Instance.PlayClick();
	}

	private void OnPurchaseHandler(bool res, SubscriptionType subscrType)
	{
		if (string.IsNullOrEmpty(this.m_photoInfo.Id))
		{
			this.m_adIcon.SetActive(false);
			this.m_premiumIcon.SetActive(false);
		}
	}

	private void OnVideoAvailabilityChangedHandler()
	{
		if (this.m_plus.activeSelf)
		{
			if (!IAPWrapper.Instance.NoAds && AppData.FreePhotos == 0)
			{
				if (INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.RewardedNo_ContentEasy)
				{
					((Component)this.m_plus.transform.Find("plus")).GetComponent<Image>().sprite = ((!AdsWrapper.Instance.IsVideoAvailable()) ? this.m_disabledPlusSprite : this.m_enabledPlusSprite);
					this.m_adIcon.SetActive(false);
					this.m_premiumIcon.SetActive(false);
				}
			}
			else
			{
				((Component)this.m_plus.transform.Find("plus")).GetComponent<Image>().sprite = this.m_enabledPlusSprite;
				this.m_adIcon.SetActive(false);
			}
		}
	}

	private void OnDestroy()
	{
		DebugController instance = DebugController.Instance;
		IAPWrapper instance2 = IAPWrapper.Instance;
		instance2.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Remove(instance2.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
		AdsWrapper instance3 = AdsWrapper.Instance;
		instance3.OnVideoAvailabilityChanged = (Action)Delegate.Remove(instance3.OnVideoAvailabilityChanged, new Action(this.OnVideoAvailabilityChangedHandler));
	}
}


