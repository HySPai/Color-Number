/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System;
using System.IO;
using Tools.TextureOptimization;
using UnityEngine;
using UnityEngine.UI;

public class CreationWindow : BaseWindow
{
	private Action<PhotoInfo> OnCreate;

	private Texture2D m_fullTexture;

	private Rect m_rect = new Rect(0f, 0f, 1f, 1f);

	private Rect m_lastRect;

	private float m_maxZoom = 5f;

	private float m_currentImageZoom = 1f;

	private bool m_subscribed;

	private int m_maxSize = 1024;

	private PhotoSource m_currentSource;

	[SerializeField]
	private RawImage m_image;

	[SerializeField]
	private RawImage m_readyImage;

	[SerializeField]
	private CameraPlugin m_cameraPlugin;

	[SerializeField]
	private GameObject m_firstButtonsBlock;

	[SerializeField]
	private GameObject m_okButtonsBlock;

	[SerializeField]
	private Slider m_qualitySlider;

	[SerializeField]
	private CreationWindowInputReceiver m_inputReceiver;

	protected override string WindowName
	{
		get
		{
			return "createImageWindow";
		}
	}

	public void Init(Action<PhotoInfo> handler)
	{
		this.OnCreate = handler;
		if (!this.m_subscribed)
		{
			this.m_subscribed = true;
			CreationWindowInputReceiver inputReceiver = this.m_inputReceiver;
			inputReceiver.OnScrolled = (Action<float>)Delegate.Combine(inputReceiver.OnScrolled, new Action<float>(this.OnScrolledImageHandler));
			CreationWindowInputReceiver inputReceiver2 = this.m_inputReceiver;
			inputReceiver2.OnDragged = (Action<Vector2>)Delegate.Combine(inputReceiver2.OnDragged, new Action<Vector2>(this.OnImageDraggedHandler));
			CreationWindowInputReceiver inputReceiver3 = this.m_inputReceiver;
			inputReceiver3.OnZoomed = (Action<float>)Delegate.Combine(inputReceiver3.OnZoomed, new Action<float>(this.OnZoomedImageHandler));
			base.OnOpen = (Action)Delegate.Combine(base.OnOpen, (Action)delegate
			{
				CameraPlugin cameraPlugin = this.m_cameraPlugin;
				cameraPlugin.OnUpdateTexture = (Action<Texture2D, float>)Delegate.Combine(cameraPlugin.OnUpdateTexture, new Action<Texture2D, float>(this.OnUpdateTextureHandler));
				this.m_cameraPlugin.Initilized();
				this.m_cameraPlugin.Quality = (this.m_qualitySlider.minValue + this.m_qualitySlider.maxValue) / 2f;
				this.m_qualitySlider.value = this.m_cameraPlugin.Quality;
			});
		}
		this.m_qualitySlider.value = (this.m_qualitySlider.minValue + this.m_qualitySlider.maxValue) / 2f;
		this.m_image.texture = null;
	}

	private void OnUpdateTextureHandler(Texture2D tex, float angle)
	{
		GameHelper.CamScaleTexture(tex, 40, 1f, this.m_qualitySlider.value);
		this.m_image.rectTransform.localEulerAngles = new Vector3(0f, 0f, angle);
		this.m_image.texture = tex;
	}

	public void QualitySliderValueChanged(float value)
	{
		this.m_cameraPlugin.Quality = value;
		if (this.m_okButtonsBlock.activeSelf)
		{
			this.UpdateTexture(true);
		}
	}

	public void ChangeCameraButtonCLicked()
	{
		this.m_cameraPlugin.ChangeCamera();
	}

	public void PhotoButtonClicked()
	{
		Texture2D texture2D = this.m_cameraPlugin.TakeSnapshot();
		if (texture2D.width > this.m_maxSize || texture2D.height > this.m_maxSize)
		{
			float num = (float)texture2D.width / (float)texture2D.height;
			int height = (int)Mathf.Min((float)this.m_maxSize, (float)this.m_maxSize / num);
			GameHelper.CamScaleTexture(texture2D, height, num, 1f);
		}
		this.m_fullTexture = texture2D;
		this.m_currentImageZoom = 1f;
		this.m_rect = new Rect(0f, 0f, 1f, 1f);
		this.UpdateTextureScale();
		this.UpdateTexture(true);
		this.m_okButtonsBlock.SetActive(true);
		this.m_firstButtonsBlock.SetActive(false);
		this.m_cameraPlugin.Stop();
		this.m_image.rectTransform.localEulerAngles = Vector3.zero;
		this.m_currentSource = (PhotoSource)((!this.m_cameraPlugin.CurrentDevice.isFrontFacing) ? 2 : 3);
	}

	public void GalleryButtonClicked()
	{
		if (!UnitySingleton<PickerController>.Instance.IsInit())
		{
			PickerController instance = UnitySingleton<PickerController>.Instance;
			instance.InitComplete = (Action<bool>)Delegate.Combine(instance.InitComplete, new Action<bool>(this.AfterInitPicker));
			UnitySingleton<PickerController>.Instance.Subscribe(new Action<Texture2D>(this.GetImageFromGallery), null, null, null);
			UnitySingleton<PickerController>.Instance.Initilized();
		}
		else
		{
			UnitySingleton<PickerController>.Instance.Subscribe(new Action<Texture2D>(this.GetImageFromGallery), null, null, null);
			UnitySingleton<PickerController>.Instance.OpenGallery();
		}
	}

	private void AfterInitPicker(bool isInit)
	{
		PickerController instance = UnitySingleton<PickerController>.Instance;
		instance.InitComplete = (Action<bool>)Delegate.Remove(instance.InitComplete, new Action<bool>(this.AfterInitPicker));
		if (isInit)
		{
			UnitySingleton<PickerController>.Instance.OpenGallery();
		}
	}

	private void GetImageFromGallery(Texture2D texture)
	{
		UnitySingleton<PickerController>.Instance.UnSubscribe(new Action<Texture2D>(this.GetImageFromGallery), null, null, null);
		this.m_fullTexture = texture;
		this.m_image.texture = texture;
		this.m_image.rectTransform.localEulerAngles = Vector3.zero;
		this.m_currentImageZoom = 1f;
		this.UpdateTextureScale();
		this.UpdateTexture(true);
		this.m_okButtonsBlock.SetActive(true);
		this.m_firstButtonsBlock.SetActive(false);
		this.m_cameraPlugin.Stop();
		this.m_currentSource = PhotoSource.Album;
	}

	private void OnImageDraggedHandler(Vector2 delta)
	{
		if (this.m_okButtonsBlock.activeSelf)
		{
			Canvas canvas = this.m_image.canvas;
			float num = canvas.pixelRect.height / (float)Screen.height;
			delta.x /= canvas.pixelRect.width / this.m_rect.width;
			delta.y /= canvas.pixelRect.width / this.m_rect.height; 
			this.m_rect.center -= delta; 
			this.CheckBorders();
			this.UpdateTexture(false);
		}
	}

	private void OnScrolledImageHandler(float delta)
	{
		if (this.m_okButtonsBlock.activeSelf)
		{
			this.m_currentImageZoom *= Mathf.Pow(1.1f, delta);
			this.UpdateTextureScale();
			this.UpdateTexture(false);
		}
	}

	private void OnZoomedImageHandler(float koef)
	{
		if (this.m_okButtonsBlock.activeSelf)
		{
			this.m_currentImageZoom *= koef;
			this.UpdateTextureScale();
			this.UpdateTexture(false);
		}
	}

	private void UpdateTextureScale()
	{
		this.m_currentImageZoom = Mathf.Max(1f, this.m_currentImageZoom);
		this.m_currentImageZoom = Mathf.Min(this.m_currentImageZoom, this.m_maxZoom);
		Vector2 center = this.m_rect.center;
		if (this.m_fullTexture.width > this.m_fullTexture.height)
		{
			float num = (float)this.m_fullTexture.height / (float)this.m_fullTexture.width;
			this.m_rect = new Rect((1f - num) / 2f, 0f, num / this.m_currentImageZoom, 1f / this.m_currentImageZoom);
		}
		else
		{
			float num2 = (float)this.m_fullTexture.width / (float)this.m_fullTexture.height;
			this.m_rect = new Rect(0f, (1f - num2) / 2f, 1f / this.m_currentImageZoom, num2 / this.m_currentImageZoom);
		}
		this.m_rect.center = center;
		this.CheckBorders();
	}

	private void CheckBorders()
	{ 
		if (this.m_rect.min.x < 0f)
		{    
			this.m_rect.center = new Vector2(this.m_rect.center.x - this.m_rect.min.x, this.m_rect.center.y);
		} 
		if (this.m_rect.max.x > 1f)
		{    
			this.m_rect.center = new Vector2(this.m_rect.center.x + 1f - this.m_rect.max.x, this.m_rect.center.y);
		} 
		if (this.m_rect.min.y < 0f)
		{     
			this.m_rect.center = new Vector2(this.m_rect.center.x, this.m_rect.center.y - this.m_rect.min.y);
		} 
		if (this.m_rect.max.y > 1f)
		{   
			this.m_rect.center = new Vector2(this.m_rect.center.x, this.m_rect.center.y + 1f - this.m_rect.max.y);
		}
	}

	private void UpdateTexture(bool force = false)
	{
		if (!(this.m_rect != this.m_lastRect) && !force)
		{
			return;
		}
		 
		this.m_lastRect = this.m_rect;
		Texture2D texture2D = null;
		if (this.m_rect == new Rect(0f, 0f, 1f, 1f))
		{
			Texture2D texture2D2 = new Texture2D(this.m_fullTexture.width, this.m_fullTexture.height, TextureFormat.RGB24, false);
			texture2D2.filterMode = FilterMode.Point;
			texture2D = texture2D2;
			texture2D.SetPixels(this.m_fullTexture.GetPixels());
		}
		else
		{
			int x = (int)(this.m_rect.xMin * (float)this.m_fullTexture.width);
			int y = (int)(this.m_rect.yMin * (float)this.m_fullTexture.height);
			int a = (int)(this.m_rect.width * (float)this.m_fullTexture.width);
			int b = (int)(this.m_rect.height * (float)this.m_fullTexture.height); 

			int num = Mathf.Min(a, b);
			if (num >= this.m_fullTexture.width || num >= this.m_fullTexture.height)
			{

			}
			Color[] pixels = this.m_fullTexture.GetPixels(x, y, num, num);
				
			Texture2D texture2D2 = new Texture2D(num, num, TextureFormat.RGB24, false);
			texture2D2.filterMode = FilterMode.Point;
			texture2D = texture2D2;
			texture2D.SetPixels(pixels);
			texture2D.Apply();
		}
		this.m_image.texture = texture2D;
		GameHelper.CamScaleTexture((Texture2D)this.m_image.texture, 40, 1f, this.m_qualitySlider.value); 
	}

	public void OkButtonClick()
	{
		string text = Guid.NewGuid().ToString();
		Texture2D texture2D = (Texture2D)this.m_image.texture;
		int num = (int)Mathf.Lerp(15f, 50f, (this.m_qualitySlider.value - this.m_qualitySlider.minValue) / (this.m_qualitySlider.maxValue - this.m_qualitySlider.minValue));
		if (TextureColoring.CheckToNeedConverColor(texture2D, num))
		{
			texture2D = TextureColorsReducer.Process(texture2D, num);
		}
		File.WriteAllBytes(AppPathsConfig.DownloadsPath + text + ".png", texture2D.EncodeToPNG());
		PhotoInfo photoInfo = new PhotoInfo(text, this.m_currentSource);
		MainManager.Instance.SavedWorksList.AddPhoto(photoInfo);
		this.OnCreate.SafeInvoke(photoInfo);
	}

	public void CloseButtonClick()
	{
		WindowManager.Instance.CloseMe(this);
		AnalyticsManager.Instance.BackButtonClicked();
	}

	public override bool Close()
	{
		if (this.m_okButtonsBlock.activeSelf)
		{
			this.m_okButtonsBlock.SetActive(false);
			this.m_firstButtonsBlock.SetActive(true);
			this.m_cameraPlugin.Initilized();
			return false;
		}
		this.m_cameraPlugin.Stop();
		return base.Close();
	}
}


