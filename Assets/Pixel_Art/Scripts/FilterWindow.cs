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
public class FilterWindow : BaseWindow
{
	private ISavedWorkData m_savedWorkData;

	private List<FilterInfo> m_filters;

	private List<FilterElement> m_filterElements;

	private int m_currentIndex = -1;

	private float m_maxZoom = 1f;

	private float m_currentImageZoom = 1f;

	[SerializeField]
	private PositScrollRect m_positScrollRect;

	[SerializeField]
	private RawImage m_image;

	[SerializeField]
	private RawImage m_filterImage;

	[SerializeField]
	private Text m_filterName;

	[SerializeField]
	private FilterElement m_filterElementPrefab;

	[SerializeField]
	private Texture2D m_emptyFilter;

	[SerializeField]
	private FilterWIndowInputReceiver m_inputReceiver;

	//private Color[] m_originalTextureData;

	private Vector2 _imageSize;
	protected override string WindowName
	{
		get
		{
			return "filtersWindow";
		}
	}

	protected void Awake()
	{
		this.m_image.material = new Material(Shader.Find("Custom/GreyTextureShader"));
		this.m_filterImage.material = new Material(Shader.Find("Custom/TilingShader"));
		PositScrollRect positScrollRect = this.m_positScrollRect;
		positScrollRect.OnSelectIndex = (Action<int>)Delegate.Combine(positScrollRect.OnSelectIndex, new Action<int>(this.OnSelectIndexHandler));
		PositScrollRect positScrollRect2 = this.m_positScrollRect;
		positScrollRect2.OnDragSelectIndex = (Action<int>)Delegate.Combine(positScrollRect2.OnDragSelectIndex, new Action<int>(this.OnSelectIndexHandler));
		PositScrollRect positScrollRect3 = this.m_positScrollRect;
		positScrollRect3.OnScaleElement = (Action<int, float, float>)Delegate.Combine(positScrollRect3.OnScaleElement, new Action<int, float, float>(this.OnScaleHandler));
		this.m_filterElements = new List<FilterElement>();
		FilterWIndowInputReceiver inputReceiver = this.m_inputReceiver;
		inputReceiver.OnScrolled = (Action<float>)Delegate.Combine(inputReceiver.OnScrolled, new Action<float>(this.OnScrolledImageHandler));
		FilterWIndowInputReceiver inputReceiver2 = this.m_inputReceiver;
		inputReceiver2.OnDragged = (Action<Vector2>)Delegate.Combine(inputReceiver2.OnDragged, new Action<Vector2>(this.OnImageDraggedHandler));
		FilterWIndowInputReceiver inputReceiver3 = this.m_inputReceiver;
		inputReceiver3.OnZoomed = (Action<float>)Delegate.Combine(inputReceiver3.OnZoomed, new Action<float>(this.OnZoomedImageHandler));
	}

	public void Init(ISavedWorkData savedWorkData, float maxZoom, Rect uvRect = default(Rect))
	{
		Rect rect = new Rect(0f, 0f, 1f, 1f);
		if (uvRect == default(Rect) || uvRect == rect)
		{
			uvRect = rect;
		}
		else
		{
			Vector2 min = uvRect.min;
			float x = (min.x + 1f) / 2f;
			Vector2 min2 = uvRect.min;
			uvRect.center = new Vector2(x, (min2.y + 1f) / 2f);
		}
		AnalyticsManager.Instance.FilterWindowOpened();
		this.m_savedWorkData = savedWorkData;
		this.ReinitImage(uvRect);
		if (this.m_filters == null)
		{
			this.m_filters = MainManager.Instance.FilterManager.GetFilters();
			foreach (FilterInfo filter in this.m_filters)
			{
				FilterElement filterElement = UnityEngine.Object.Instantiate(this.m_filterElementPrefab);
				filterElement.transform.SetParent(this.m_filterElementPrefab.transform.parent);
				filterElement.transform.localScale = Vector3.one;
				filterElement.gameObject.SetActive(true);
				FilterElement filterElement2 = filterElement;
				filterElement2.OnClick = (Action<FilterElement>)Delegate.Combine(filterElement2.OnClick, new Action<FilterElement>(this.OnFilterClickHandler));
				this.m_filterElements.Add(filterElement);
				if (filter.Texture != null)
				{
					filterElement.Init(filter);
				}
				else
				{
					filterElement.InitEmpty(this.m_emptyFilter);
				}
			}
		}
		this.m_positScrollRect.Reinit(0f, false);
		this.m_positScrollRect.SetContentSize();
		this.OnSelectIndexHandler(this.m_savedWorkData.FilterId);
		this.m_positScrollRect.SetIndex(this.m_savedWorkData.FilterId);
		this.m_maxZoom = maxZoom;
	}

	private void ReinitImage(Rect uvRect)
	{
		Texture2D resTex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		resTex.filterMode = FilterMode.Point;
		resTex.LoadImage(this.m_savedWorkData.Preview);
		this.m_image.material.SetTexture("_ResTex", resTex);
		if (resTex != null)
		{
			_imageSize = new Vector2(resTex.width, resTex.height);
		}
		this.m_image.enabled = true;
		this.m_filterImage.enabled = false;
		if (this.m_savedWorkData.ImageInfo.Url != null)
		{
			DataManager.Instance.GetImageAsset(this.m_savedWorkData.ImageInfo, delegate (bool res, Texture2D tex)
			{
				//m_originalTextureData = tex.GetPixels().ToArray();

				this.m_image.texture = tex;
				this.m_filterImage.material.mainTextureScale = new Vector2((float)resTex.width, (float)resTex.height);
				Texture2D filter2 = MainManager.Instance.FilterManager.GetFilter(this.m_savedWorkData.FilterId);
				if (filter2 != null)
				{
					this.m_filterImage.texture = filter2;
					this.m_filterImage.enabled = true;
					this.m_filterName.text = filter2.name;

					this.m_filterImage.material.SetFloat("RepeatX", _imageSize.x);
					this.m_filterImage.material.SetFloat("RepeatY", _imageSize.y);
				}
				else
				{
					this.m_filterImage.enabled = false;
				}
				this.m_image.uvRect = uvRect;
				this.m_filterImage.uvRect = this.m_image.uvRect;
				this.m_currentImageZoom = 1f / this.m_image.uvRect.width;

				ApplyFilter(filter2);
			});
		}
		else
		{
			DataManager.Instance.GetPhotoAsset(this.m_savedWorkData.ImageInfo.Id, delegate (bool res, Texture2D tex)
			{
				//m_originalTextureData = tex.GetPixels().ToArray();

				this.m_image.texture = tex;
				this.m_filterImage.material.mainTextureScale = new Vector2((float)resTex.width, (float)resTex.height);
				Texture2D filter = MainManager.Instance.FilterManager.GetFilter(this.m_savedWorkData.FilterId);
				if (filter != null)
				{
					this.m_filterImage.texture = filter;
					this.m_filterImage.enabled = true;
					this.m_filterName.text = filter.name;
					this.m_filterImage.material.SetFloat("RepeatX", _imageSize.x);
					this.m_filterImage.material.SetFloat("RepeatY", _imageSize.y);
				}
				else
				{
					this.m_filterImage.enabled = false;
				}
				this.m_image.uvRect = uvRect;
				this.m_filterImage.uvRect = this.m_image.uvRect;
				this.m_currentImageZoom = 1f / this.m_image.uvRect.width;

				ApplyFilter(filter);

			});
		}
	}

	private void ApplyFilter(Texture2D filter)
	{
		//if (this.m_image.texture != null)
		//{
		//	var tex = new Texture2D(this.m_image.texture.width, this.m_image.texture.height, ((Texture2D)this.m_image.texture).format, false);
		//	if (filter != null)
		//	{
		//		int cellSize = NumberColoring.CELL_SIZE;

		//		if (filter.height != cellSize || filter.width != cellSize)
		//		{
		//			var resizedFilter = new Texture2D(filter.width, filter.height, TextureFormat.ARGB32, false);
		//			Graphics.CopyTexture(filter, resizedFilter);
		//			resizedFilter.Resize(cellSize, cellSize);
		//			filter = resizedFilter;
		//		}
		//		var filterColors = filter.GetPixels().ToArray();
		//		for (int row = 0; row < this.m_image.texture.height; row += cellSize)
		//		{
		//			for (int column = 0; column < this.m_image.texture.width; column += cellSize)
		//			{
		//				int index = row * this.m_image.texture.width + column;
		//				Color color = this.m_originalTextureData[index];
		//				for (int i = 0; i < filterColors.Length; i++)
		//				{
		//					filterColors[i] = color;
		//				}
		//				tex.SetPixels(column, row, cellSize, cellSize, filterColors);
		//			}
		//		}
		//	}
		//	else
		//	{
		//		tex.SetPixels(m_originalTextureData);
		//	}
		//	this.m_image.texture = tex;
		//}
	}

	public void OnScrollRectScrolled(Vector2 pos)
	{
		if (this.m_filters.Count <= 0)
		{
			return;
		}
	}

	public void OnFilterClickHandler(FilterElement filterElement)
	{
		UnityEngine.Debug.Log("m_positScrollRect.Moving: " + this.m_positScrollRect.Moving);
		if (!this.m_positScrollRect.Moving)
		{
			this.m_positScrollRect.MoveToIndex(this.m_filterElements.IndexOf(filterElement));
		}
	}

	private void OnSelectIndexHandler(int index)
	{
		if (index >= 0 && index < this.m_filterElements.Count && index != this.m_currentIndex)
		{
			this.m_currentIndex = index;
			VibroWrapper.PlayVibro();
			Texture2D filter = MainManager.Instance.FilterManager.GetFilter(this.m_currentIndex);
			if (filter != null)
			{
				this.m_filterImage.texture = filter;
				this.m_filterImage.enabled = true;
				this.m_filterName.text = LocalizationManager.Instance.GetString(filter.name);

				this.m_filterImage.material.SetFloat("RepeatX", _imageSize.x);
				this.m_filterImage.material.SetFloat("RepeatY", _imageSize.y);
			}
			else
			{
				this.m_filterImage.enabled = false;
				this.m_filterName.text = string.Empty;
			}

			ApplyFilter(filter);
		}
	}

	private void OnScaleHandler(int index, float scale, float maxScale)
	{
		if (index < this.m_filterElements.Count)
		{
			this.m_filterElements[index].Scale(scale, maxScale);
		}
	}

	private void OnScrolledImageHandler(float delta)
	{
		this.m_currentImageZoom *= Mathf.Pow(1.1f, delta);
		this.m_currentImageZoom = Mathf.Max(1f, this.m_currentImageZoom);
		this.m_currentImageZoom = Mathf.Min(this.m_currentImageZoom, this.m_maxZoom);
		Vector2 center = this.m_image.uvRect.center;
		Rect uvRect = default(Rect);
		uvRect.height = 1f / this.m_currentImageZoom; float num3 = uvRect.width = (uvRect.height);
		uvRect.center = center;
		this.m_image.uvRect = uvRect;
		this.CheckBorders();
		this.m_filterImage.uvRect = this.m_image.uvRect;
	}

	private void OnZoomedImageHandler(float koef)
	{
		this.m_currentImageZoom *= koef;
		this.m_currentImageZoom = Mathf.Max(1f, this.m_currentImageZoom);
		this.m_currentImageZoom = Mathf.Min(this.m_currentImageZoom, this.m_maxZoom);
		Vector2 center = this.m_image.uvRect.center;
		Rect uvRect = default(Rect);
		uvRect.height = 1f / this.m_currentImageZoom; float num3 = uvRect.width = (uvRect.height);
		uvRect.center = center;
		this.m_image.uvRect = uvRect;
		this.CheckBorders();
		this.m_filterImage.uvRect = this.m_image.uvRect;
	}

	private void OnImageDraggedHandler(Vector2 delta)
	{
		Canvas canvas = this.m_image.canvas;
		Rect uvRect = this.m_image.uvRect;
		float num = canvas.pixelRect.height / (float)Screen.height;
		delta.x /= canvas.pixelRect.width / uvRect.width;
		delta.y /= canvas.pixelRect.width / uvRect.height;
		uvRect.center -= delta;
		this.m_image.uvRect = uvRect;
		this.CheckBorders();
		this.m_filterImage.uvRect = this.m_image.uvRect;
	}

	private void CheckBorders()
	{
		Rect uvRect = this.m_image.uvRect;
		Vector2 min = uvRect.min;
		if (min.x < 0f)
		{
			Vector2 center = uvRect.center;
			float x = center.x;
			Vector2 min2 = uvRect.min;
			float x2 = x - min2.x;
			Vector2 center2 = uvRect.center;
			uvRect.center = new Vector2(x2, center2.y);
		}
		Vector2 max = uvRect.max;
		if (max.x > 1f)
		{
			Vector2 center3 = uvRect.center;
			float num = center3.x + 1f;
			Vector2 max2 = uvRect.max;
			float x3 = num - max2.x;
			Vector2 center4 = uvRect.center;
			uvRect.center = new Vector2(x3, center4.y);
		}
		Vector2 min3 = uvRect.min;
		if (min3.y < 0f)
		{
			Vector2 center5 = uvRect.center;
			float x4 = center5.x;
			Vector2 center6 = uvRect.center;
			float y = center6.y;
			Vector2 min4 = uvRect.min;
			uvRect.center = new Vector2(x4, y - min4.y);
		}
		Vector2 max3 = uvRect.max;
		if (max3.y > 1f)
		{
			Vector2 center7 = uvRect.center;
			float x5 = center7.x;
			Vector2 center8 = uvRect.center;
			float num2 = center8.y + 1f;
			Vector2 max4 = uvRect.max;
			uvRect.center = new Vector2(x5, num2 - max4.y);
		}
		this.m_image.uvRect = uvRect;
	}

	public void OkButtonClick()
	{
		this.m_savedWorkData.UvRect = new SerializableRect(this.m_image.uvRect);
		this.m_savedWorkData.FilterId = this.m_currentIndex;
		ShareWindow shareWindow = WindowManager.Instance.OpenShareWindow();
		shareWindow.Init(this.m_savedWorkData);
		Texture2D filter = MainManager.Instance.FilterManager.GetFilter(this.m_currentIndex);
		AnalyticsManager.Instance.FilterWindowClosed((!(filter == null)) ? filter.name : "none");
		AudioManager.Instance.PlayClick();
	}

	public void CloseButtonClick()
	{
		WindowManager.Instance.CloseMe(this);
		Texture2D filter = MainManager.Instance.FilterManager.GetFilter(this.m_currentIndex);
		AnalyticsManager.Instance.BackButtonClicked();
		AnalyticsManager.Instance.FilterWindowClosed((!(filter == null)) ? filter.name : "none");
		AudioManager.Instance.PlayClick();
	}

	public override bool Close()
	{
		this.m_savedWorkData.FilterId = this.m_currentIndex;
		return base.Close();
	}
}


