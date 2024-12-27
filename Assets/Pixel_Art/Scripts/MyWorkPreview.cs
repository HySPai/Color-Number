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

public class MyWorkPreview : MonoBehaviour
{
	public Action<string, MyWorkPreview> OnClick;

	private string m_saveId;

	private ISavedWorkData m_swd;

	[SerializeField]
	private RawImage m_image;

	[SerializeField]
	private GameObject m_imageWaiter;

	[SerializeField]
	private GameObject m_completedTick;

	[SerializeField]
	private RawImage m_filterImage;

	[SerializeField]
	private GameObject m_3dMark;

	public bool IsInited { get; private set; }

	public bool Loaded
	{
		get
		{
			return this.m_image.texture != null;
		}
	}

	public Texture2D Texture
	{
		get
		{
			if (this.m_image.texture != null && this.m_image.texture is Texture2D)
				return (Texture2D)this.m_image.texture;
			return null;
		}
	}

	public string SaveId
	{
		get
		{
			return this.m_saveId;
		}
	}

	private void Awake()
	{
		this.m_filterImage.material = new Material(Shader.Find("Custom/TilingShader"));
	}

	public void Init(string saveId)
	{
		this.IsInited = true;
		this.m_saveId = saveId;
		this.m_image.material = new Material(Shader.Find("Custom/GreyTextureShader"));
		if (this.m_image.texture != null)
		{
			UnityEngine.Object.Destroy(this.m_image.texture);
		}
		Texture texture = this.m_image.material.GetTexture("_ResTex");
		if (texture != null)
		{
			UnityEngine.Object.Destroy(texture);
			this.m_image.material.SetTexture("_ResTex", null);
		}
		this.m_image.texture = null;
		this.m_swd = MainManager.Instance.SavedWorksList.LoadById(this.m_saveId);
		if (this.m_swd != null)
		{
			this.m_completedTick.SetActive(this.m_swd.Completed);
			this.m_3dMark.SetActive(this.m_swd.ImageInfo.Is3D);
		}
		else
		{
			this.m_completedTick.SetActive(false);
			this.m_3dMark.SetActive(false);
			this.m_imageWaiter.SetActive(true);
		}
	}

	public void LoadIcon()
	{
		if (this.m_image.texture == null && this.m_swd != null)
		{
			this.m_imageWaiter.SetActive(true);
			if (this.m_swd.ImageInfo.Url != null)
			{
				DataManager.Instance.GetImageAsset(this.m_swd.ImageInfo, delegate (bool res, Texture2D tex)
				{
					this.m_image.texture = tex;
					this.m_image.enabled = true;
					this.m_imageWaiter.SetActive(false);
					Texture2D texture2D2 = new Texture2D(1, 1, TextureFormat.ARGB32, false);
					texture2D2.filterMode = (FilterMode)(this.m_swd.ImageInfo.Is3D ? 1 : 0);
					texture2D2.LoadImage(this.m_swd.Preview);
					this.m_image.material.SetTexture("_ResTex", texture2D2);
					Texture2D filter2 = MainManager.Instance.FilterManager.GetFilter(this.m_swd.FilterId);
					if (filter2 != null)
					{
						this.m_filterImage.material.mainTextureScale = new Vector2((float)texture2D2.width, (float)texture2D2.height);
						this.m_filterImage.texture = filter2;
						this.m_filterImage.enabled = true;
						this.m_filterImage.transform.localScale = this.m_image.transform.localScale;

						this.m_filterImage.material.SetFloat("RepeatX", this.m_image.texture.width);
						this.m_filterImage.material.SetFloat("RepeatY", this.m_image.texture.height);
					}
					else
					{
						this.m_filterImage.enabled = false;
					}
				});
			}
			else
			{
				DataManager.Instance.GetPhotoAsset(this.m_swd.ImageInfo.Id, delegate (bool res, Texture2D tex)
				{
					this.m_image.texture = tex;
					if (tex.width > tex.height)
					{
						this.m_image.transform.localScale = new Vector3(1f, (float)tex.height / (float)tex.width, 1f);
					}
					else
					{
						this.m_image.transform.localScale = new Vector3((float)tex.width / (float)tex.height, 1f, 1f);
					}
					this.m_image.enabled = true;
					this.m_imageWaiter.SetActive(false);
					Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
					texture2D.filterMode = FilterMode.Point;
					texture2D.LoadImage(this.m_swd.Preview);
					this.m_image.material.SetTexture("_ResTex", texture2D);
					Texture2D filter = MainManager.Instance.FilterManager.GetFilter(this.m_swd.FilterId);
					if (filter != null)
					{
						this.m_filterImage.material.mainTextureScale = new Vector2((float)texture2D.width, (float)texture2D.height);
						this.m_filterImage.texture = filter;
						this.m_filterImage.enabled = true;
						this.m_filterImage.transform.localScale = this.m_image.transform.localScale;

						this.m_filterImage.material.SetFloat("RepeatX", this.m_image.texture.width);
						this.m_filterImage.material.SetFloat("RepeatY", this.m_image.texture.height);
					}
					else
					{
						this.m_filterImage.enabled = false;
					}
				});
			}
		}
	}

	public void UnloadIcon()
	{
		if (this.m_image.texture != null)
		{
			this.m_image.texture = null;
			this.m_image.enabled = false;
		}
	}

	public void Click()
	{
		this.OnClick.SafeInvoke(this.m_saveId, this);
		AudioManager.Instance.PlayClick();
	}
}


