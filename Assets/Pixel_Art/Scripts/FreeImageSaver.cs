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

public class FreeImageSaver : MonoBehaviour
{
	[SerializeField]
	private Canvas m_canvas;

	[SerializeField]
	private Camera m_freeImageCamera;

	[SerializeField]
	private RawImage m_image;

	[SerializeField]
	private RawImage m_filterImage;

	//[SerializeField]
	//private GameObject m_watermark;

	//[SerializeField]
	//private RectTransform m_watermarkTransform;

	protected void Awake()
	{
		this.m_image.material = new Material(Shader.Find("Custom/GreyTextureShader"));
		this.m_filterImage.material = new Material(Shader.Find("Custom/TilingShader"));
	}

	public void GetFilteredImage(ISavedWorkData swd, Action<byte[]> handler, bool antialiasing)
	{
		Texture2D resTex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		resTex.filterMode = FilterMode.Point;
		resTex.LoadImage(swd.Preview);
		Texture2D filter = MainManager.Instance.FilterManager.GetFilter(swd.FilterId);
		if (swd.ImageInfo.Url != null)
		{
			DataManager.Instance.GetImageAsset(swd.ImageInfo, delegate (bool res, Texture2D tex)
			{
				this.GetFilteredImage(tex, resTex, filter, swd.UvRect1, 800, true, handler, antialiasing, 1f);
			});
		}
		else
		{
			DataManager.Instance.GetPhotoAsset(swd.ImageInfo.Id, delegate (bool res, Texture2D tex)
			{
				this.GetFilteredImage(tex, resTex, filter, swd.UvRect1, 800, true, handler, antialiasing, 1f);
			});
		}
	}

	public void GetFilteredImage(Texture2D grayTex, Texture2D resTex, Texture2D filter, Rect uvRect, int size, bool enableWatermark, Action<byte[]> handler, bool antialiasing, float watermarkPos = 1f)
	{
		this.m_image.material.SetTexture("_ResTex", resTex);
		this.m_image.enabled = true;
		if (filter != null)
		{
			this.m_filterImage.material.mainTextureScale = new Vector2((float)resTex.width, (float)resTex.height);
			this.m_filterImage.texture = filter;
			this.m_filterImage.enabled = true;

			this.m_filterImage.material.SetFloat("RepeatX", resTex.width);
			this.m_filterImage.material.SetFloat("RepeatY", resTex.height);
		}
		else
		{
			this.m_filterImage.enabled = false;
		}
		this.m_image.uvRect = uvRect;
		this.m_filterImage.uvRect = this.m_image.uvRect;
		this.m_image.texture = grayTex;
		//this.m_watermark.SetActive(enableWatermark && !IAPWrapper.Instance.NoAds && INPluginWrapper.Instance.GetAbTestGroup() != ABTestGroup.RewardedNo_ContentEasy);
		//RectTransform watermarkTransform = this.m_watermarkTransform;
		//Vector2 sizeDelta = this.m_watermarkTransform.sizeDelta;
		//watermarkTransform.anchoredPosition = new Vector2(sizeDelta.x * (0f - watermarkPos + 1f), 0f);
		handler.SafeInvoke(FreeImageSaver.MakePngFromOurVirtualThingy(size, size, size, 100, this.m_freeImageCamera, antialiasing));
	}

	public void Get3DImage(byte[] bytes, int size, bool enableWatermark, Action<byte[]> handler, bool antialiasing, float watermarkPos = 1f)
	{
		Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGB24, false);
		texture2D.LoadImage(bytes);
		this.m_image.material.SetTexture("_ResTex", texture2D);
		this.m_image.enabled = true;
		this.m_image.uvRect = new Rect(0f, 0f, 1f, 1f);
		this.m_filterImage.uvRect = this.m_image.uvRect;
		this.m_image.texture = texture2D;
		//this.m_watermark.SetActive(enableWatermark && !IAPWrapper.Instance.NoAds && INPluginWrapper.Instance.GetAbTestGroup() != ABTestGroup.RewardedNo_ContentEasy);
		//RectTransform watermarkTransform = this.m_watermarkTransform;
		//Vector2 sizeDelta = this.m_watermarkTransform.sizeDelta;
		//watermarkTransform.anchoredPosition = new Vector2(sizeDelta.x * (0f - watermarkPos + 1f), 0f);
		handler.SafeInvoke(FreeImageSaver.MakePngFromOurVirtualThingy(size, size, size, 100, this.m_freeImageCamera, antialiasing));
	}

	public static Texture2D MakeTexFromOurVirtualThingy(int width, int height, int maxSize, int pixelPerUnit, Camera cam)
	{
		int num;
		int num2;
		if (width < height)
		{
			num = maxSize;
			num2 = (int)((float)width / (float)height) * num;
		}
		else
		{
			num2 = maxSize;
			num = (int)((float)height / (float)width) * num2;
		}
		RenderTexture renderTexture2 = cam.targetTexture = new RenderTexture(num2, num, 24);
		cam.Render();
		RenderTexture.active = renderTexture2;
		Texture2D texture2D = new Texture2D(num2, num, TextureFormat.RGB24, false);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)num2, (float)num), 0, 0);
		texture2D.Apply();
		RenderTexture.active = null;
		cam.targetTexture = null;
		UnityEngine.Object.Destroy(renderTexture2);
		return texture2D;
	}

	public static byte[] MakePngFromOurVirtualThingy(int width, int height, int maxSize, int pixelPerUnit, Camera cam, bool antialising)
	{
		int num;
		int num2;
		if (width < height)
		{
			num = maxSize;
			num2 = (int)((float)width / (float)height) * num;
		}
		else
		{
			num2 = maxSize;
			num = (int)((float)height / (float)width) * num2;
		}
		RenderTexture renderTexture = new RenderTexture(num2, num, 24);
		renderTexture.antiAliasing = ((!antialising) ? 1 : 8);
		RenderTexture renderTexture3 = cam.targetTexture = renderTexture;
		cam.Render();
		RenderTexture.active = renderTexture3;
		Texture2D texture2D = new Texture2D(num2, num, TextureFormat.RGB24, true);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)num2, (float)num), 0, 0);
		RenderTexture.active = null;
		cam.targetTexture = null;
		byte[] result = texture2D.EncodeToJPG(100);
		UnityEngine.Object.Destroy(renderTexture3);
		UnityEngine.Object.Destroy(texture2D);
		return result;
	}
}


