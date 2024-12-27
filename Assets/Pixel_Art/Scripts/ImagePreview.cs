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

public class ImagePreview : MonoBehaviour
{
	public Action<ImageInfo, ImagePreview> OnClick;

	private ImageInfo m_imageInfo;

	private bool m_loadIcon;

	private bool m_loadedIcon;

	private bool m_materialInited;

	private string m_saveId;

	[SerializeField]
	private RawImage m_image;

	[SerializeField]
	private RawImage m_filterImage;

	[SerializeField]
	private GameObject m_locked;

	[SerializeField]
	private GameObject m_adLocked;

	[SerializeField]
	private GameObject m_adLocked_mozaic;

	[SerializeField]
	private GameObject m_adLocked_mozaic_other_group;

	[SerializeField]
	private GameObject m_adLocked_icon;

	[SerializeField]
	private Text m_title;

	[SerializeField]
	private GameObject m_imageWaiter;

	[SerializeField]
	private GameObject m_completedMark;

	[SerializeField]
	private GameObject m_3dMark;

	[SerializeField]
	private bool m_isTodayArt = false;

	public bool TodayArt
	{
		get
		{
			return m_isTodayArt;
		}
	}

	public bool Inited
	{
		get
		{
			return this.m_imageInfo != null;
		}
	}
	public bool AdsAvailable
	{
		get
		{
			return this.m_adLocked.activeSelf;
		}
	}
	public bool Loaded
	{
		get
		{
			return this.m_image.texture != null;
		}
	}

	public bool Locked
	{
		get
		{
			return this.m_adLocked.activeSelf;
		}
	}

	public bool IsFreePremium
	{
		get
		{
			return this.m_locked.activeInHierarchy;
		}
	}

	public Texture2D Texture
	{
		get
		{
			return (!(this.m_image.texture == null)) ? ((Texture2D)this.m_image.texture) : null;
		}
	}

	private void Awake()
	{
		this.m_filterImage.material = new Material(Shader.Find("Custom/TilingShader"));
	}

	public bool CheckTheSame(ImageInfo imageInfo)
	{
		return this.m_imageInfo != null && imageInfo != null && this.m_imageInfo.Id == imageInfo.Id;
	}

	public void Init(ImageInfo imageInfo)
	{
		if (this.m_title != null)
		{
			this.m_title.text = imageInfo.Id;
		}
		this.m_imageInfo = imageInfo;
		if (!this.m_materialInited)
		{
			this.m_image.material = new Material(Shader.Find("Custom/GreyTextureShader"));
			this.m_materialInited = true;
		}
		if (this.m_image.texture != null && !m_isTodayArt)
		{
			UnityEngine.Object.Destroy(this.m_image.texture);
		}
		Texture texture = this.m_image.material.GetTexture("_ResTex");
		if (texture != null && !m_isTodayArt)
		{
			UnityEngine.Object.Destroy(texture);
			this.m_image.material.SetTexture("_ResTex", null);
		}
		this.m_image.texture = null;
		this.m_loadIcon = false;
		this.m_loadedIcon = false;
		if (!m_isTodayArt)
		{
			this.Reinit();
		}
		this.m_completedMark.SetActive(false);
		this.m_3dMark.SetActive(imageInfo.Is3D);
	}
	private void RecheckShaker(bool enabled)
	{
		if (m_isTodayArt)
		{
			var shaker = gameObject.GetComponentInParent<UIShaker>();
			if (shaker != null)
			{
				shaker.enabled = enabled;
			}
		}
	}
	public void LoadIcon()
	{
		if (!this.m_loadedIcon)
		{
			if (!this.m_loadIcon)
			{
				this.m_loadIcon = true;
				this.m_imageWaiter.SetActive(true);
				this.m_saveId = MainManager.Instance.SavedWorksList.LastSaveOfImageId(this.m_imageInfo.Id);
				//if (this.m_imageInfo.Is3D && this.m_saveId != null)
				//{
				//	this.m_imageWaiter.SetActive(false);
				//	this.m_image.gameObject.SetActive(true);
				//	base.StartCoroutine(this.LoadSaveCoroutine());
				//}
				//else
				{
					DataManager.Instance.GetImageAsset(this.m_imageInfo, delegate (bool res, Texture2D tex)
					{
						if (res)
						{
							try
							{
								this.m_loadedIcon = true;
								this.m_image.texture = tex;
								this.m_imageWaiter.SetActive(false);
								this.m_image.gameObject.SetActive(true);
								if (this.m_saveId != null)
								{
									if (gameObject.activeInHierarchy)
									{
										base.StartCoroutine(this.LoadSaveCoroutine());
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
					if (this.m_imageInfo.Is3D)
					{
						DataManager.Instance.GetImageAsset3D(this.m_imageInfo, null);
					}
				}
			}
		}
		else
		{
			this.m_image.gameObject.SetActive(true);
		}
	}

	public void UnloadIcon()
	{
		this.m_loadIcon = false;
		this.m_image.gameObject.SetActive(false);
	}

	public void Reinit()
	{
		if (IAPWrapper.Instance.NoAds || IAPWrapper.Instance.Subscribed)
		{
			this.m_adLocked.SetActive(false);
			if (this.m_adLocked_mozaic != null)
				this.m_adLocked_mozaic.SetActive(false);
			this.m_adLocked.GetComponent<Image>().enabled = false;
		}
		else if (INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.None || INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.RewardedNo_ContentMedium_Old)
		{
			this.m_adLocked.SetActive(this.m_imageInfo.AccessStatus != 0 && !IAPWrapper.Instance.Subscribed && !AppData.UnlockedImages.Contains(this.m_imageInfo.Id));
		}
		else if (INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.RewardedNo_ContentMedium_Revealed)
		{
			this.m_adLocked.SetActive(this.m_imageInfo.AccessStatus != 0 && !IAPWrapper.Instance.Subscribed && !AppData.UnlockedImages.Contains(this.m_imageInfo.Id));
			if (!this.m_adLocked.activeInHierarchy)
			{
				if (this.m_adLocked_mozaic != null)
					this.m_adLocked_mozaic.SetActive(false);
				this.m_adLocked.GetComponent<Image>().enabled = false;
			}
		}
		else
		{
			bool active = this.m_imageInfo.CustomAccessStatus != 0 && !IAPWrapper.Instance.Subscribed && !AppData.UnlockedImages.Contains(this.m_imageInfo.Id);
			this.m_locked.SetActive(active);
			//this.m_adLocked_mozaic_other_group.SetActive(active);
		}
	}

	public void Click()
	{
		this.OnClick.SafeInvoke(this.m_imageInfo, this);
		AudioManager.Instance.PlayClick();
	}
	private IEnumerator LoadSaveCoroutine()
	{
		//if (this.isActiveAndEnabled)
		//{
		//	yield return null;
		//}
		var resTex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		resTex.filterMode = (FilterMode)(this.m_imageInfo.Is3D ? 1 : 0);
		var swd = MainManager.Instance.SavedWorksList.LoadById(this.m_saveId);

		//yield return null;

		resTex.LoadImage(swd.Preview);
		this.m_image.material.SetTexture("_ResTex", resTex);
		this.m_completedMark.SetActive(swd.Completed);

		this.RecheckShaker(!swd.Completed);
		var filter = MainManager.Instance.FilterManager.GetFilter(swd.FilterId);
		if (filter != null)
		{
			this.m_filterImage.material.mainTextureScale = new Vector2((float)resTex.width, (float)resTex.height);
			this.m_filterImage.texture = filter;
			this.m_filterImage.enabled = true;
			this.m_filterImage.material.SetFloat("RepeatX", this.m_image.texture.width);
			this.m_filterImage.material.SetFloat("RepeatY", this.m_image.texture.height);
		}
		else
		{
			this.m_filterImage.enabled = false;
        }

        yield return null;
	}
}
