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
using System.Text;
using UnityEngine;

public class NumberColoring : MonoBehaviour
{
	public static int CELL_SIZE = 50;
	public Color borderColor = Color.grey;
	public int borderWidth = 1;

	public Action OnComplete;

	public Action<Color> OnColorComplete;
	public Action<Color, int> OnCountLeft;
	public Action<Color> OnPaintedPixel;

	private int[,] m_bombMatrix = new int[7, 7] {
		{
			0, 0,
			1,
			1,
			1,
			0,
			0
		},
		{
			0,
			1,
			1,
			1,
			1,
			1,
			0
		},
		{
			1,
			1,
			1,
			1,
			1,
			1,
			1
		},
		{
			1,
			1,
			1,
			1,
			1,
			1,
			1
		},
		{
			1,
			1,
			1,
			1,
			1,
			1,
			1
		},
		{
			0,
			1,
			1,
			1,
			1,
			1,
			0
		},
		{
			0,
			0,
			1,
			1,
			1,
			0,
			0
		}
	};

	[SerializeField]
	private Camera m_mainCamera;

	[SerializeField]
	private Camera m_loupeCamera;

	[SerializeField]
	private CameraManager m_cameraManager;

	[SerializeField]
	private MeshRenderer m_grayRenderer;

	[SerializeField]
	private MeshRenderer m_gridRenderer;

	[SerializeField]
	private MeshRenderer m_highlightedGridRenderer;

	[SerializeField]
	public Transform m_numbersContent;

	[SerializeField]
	private Material m_resMaterial;

	[SerializeField]
	private GameObject m_content;

	[SerializeField]
	private TextMesh m_numbersText;

	[SerializeField]
	public List<Color> m_colors;

	[HideInInspector]
	public ImageInfo m_imageInfo;

	[HideInInspector]
	public int m_colorsToColor;

	[HideInInspector]
	public Color[] m_pixels;

	[HideInInspector]
	public int m_width = -1;

	[HideInInspector]
	public int m_height = -1;

	[HideInInspector]
	public bool m_inited;

	[HideInInspector]
	public bool m_isEmpty = true;

	[HideInInspector]
	public History m_history;

	public bool Completed { get; private set; }
	 
	private Color transparent = new Color(1, 1, 1, 0);
	private int m_totalNumbers = 0;
	private Dictionary<Color, int> m_colorsCount = new Dictionary<Color, int>();

	[ContextMenu("InitObject")]
	public void InitObject()
	{
		GameObject gameObject = default(GameObject);
		NumberColoringHelper.InitObject(base.gameObject, out this.m_gridRenderer, out this.m_grayRenderer, out this.m_numbersContent, out this.m_resMaterial, out this.m_mainCamera, out this.m_cameraManager, out gameObject);
	}

	[ContextMenu("CreateHighLightedGrid")]
	public void CreateHighLightedGrid()
	{
		NumberColoringHelper.CreateHighLightedGrid(base.gameObject, out this.m_highlightedGridRenderer);
	}

	[ContextMenu("SaveImages")]
	public void SaveImages()
	{
		File.WriteAllBytes("1.png", ((Texture2D)this.m_grayRenderer.sharedMaterial.mainTexture).EncodeToPNG());
		File.WriteAllBytes("2.png", ((Texture2D)this.m_resMaterial.mainTexture).EncodeToPNG());
	}

	public void Init(ImageInfo imageInfo, ImageOpenType imageOpenType, Texture2D tex)
	{
		this.m_imageInfo = imageInfo;
		this.m_grayRenderer.sharedMaterial.mainTexture = tex;
		this.m_gridRenderer.sharedMaterial.mainTextureScale = new Vector2((float)tex.width, (float)tex.height);
		this.m_highlightedGridRenderer.sharedMaterial.SetTexture("_ImageTex", tex);
		this.m_highlightedGridRenderer.sharedMaterial.mainTextureScale = new Vector2((float)tex.width, (float)tex.height);
		this.m_width = tex.width;
		this.m_height = tex.height;
		this.m_pixels = tex.GetPixels();
		this.m_colors = this.m_pixels.Distinct().ToList();//.Where(c => c.r != 1 && c.g != 1 && c.b != 1).ToList();

		this.m_colors.Remove(Color.white);
		this.m_colors.Remove(transparent);

		Texture2D texture2D = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);
		texture2D.filterMode = FilterMode.Point;
		Color[] array = new Color[this.m_pixels.Length];
		Color color = new Color(1f, 1f, 1f, 0f);
		Color white = Color.white;
		this.m_colorsToColor = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (this.m_pixels[i] == white)
			{
				array[i] = white;
			}
			else
			{
				array[i] = color;
				this.m_colorsToColor++;
			}
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		this.m_resMaterial.mainTexture = texture2D;
		this.m_inited = true; 
		m_colorsCount.Clear();
		 
		Color[] resPixels = ((Texture2D)this.m_resMaterial.mainTexture).GetPixels();
		Color[] rightPixels = ((Texture2D)this.m_grayRenderer.sharedMaterial.mainTexture).GetPixels();

		m_totalNumbers = 0;
		foreach (var c in this.m_colors)
		{
			int count = 0;
			for (int i = 0; i < rightPixels.Length; i++)
			{
				if (rightPixels[i] == c && resPixels[i].a < 0.9f)
				{
					count++;
					m_totalNumbers++;
				}
			}
			m_colorsCount[c] = count;
		}

		base.StartCoroutine(this.GenerateNumbersCoroutine_new(delegate
		{
			if (this.m_width > this.m_height)
			{
				base.transform.localScale = new Vector3(1f, (float)this.m_height / (float)this.m_width, 1f);
			}
			else if (this.m_width < this.m_height)
			{
				base.transform.localScale = new Vector3((float)this.m_width / (float)this.m_height, 1f, 1f);
			} 
		}));
		base.StartCoroutine(this.DefferedEnableCoroutine());
		this.m_highlightedGridRenderer.gameObject.SetActive(AppData.BulbMode);
		AnalyticsManager.Instance.ImageOpened(this.m_imageInfo, this.m_colorsToColor, this.m_colors.Count, imageOpenType); 
	}

	private void Update()
	{
		if (this.m_inited)
		{
			float value = Mathf.Lerp(1f, 0f, (this.m_cameraManager.Zoom - (this.m_cameraManager.MaxZoom - 1f) * 0.1f - 1f) / (this.m_cameraManager.MaxZoom * 0.4f));
			this.m_grayRenderer.sharedMaterial.SetFloat("_Alpha", value);
		}
	}
	public List<Color> GetColors()
	{
		return this.m_colors;
	}

	public bool IsEmpty()
	{
		try
		{
			if (this.m_isEmpty)
			{
				this.m_isEmpty = (this.m_history == null || this.m_history.Steps.Count == 0);
			}
			return this.m_isEmpty;
		}
		catch
		{
			return true;
		}
	}

	public void SetColor(Color color)
	{
        Debug.Log("set highlight color: " + color);
		this.m_highlightedGridRenderer.sharedMaterial.SetColor("_CurrentColor", color);
	}

	public void TryClickPixel(Vector2 mousePosition)
	{
		Ray ray = this.m_mainCamera.ScreenPointToRay(mousePosition);
		RaycastHit[] array = Physics.RaycastAll(ray);
		if (array != null && array.Length > 0)
		{
			MeshRenderer component = array[0].collider.gameObject.GetComponent<MeshRenderer>();
			if (component != null)
			{
				Texture mainTexture = component.sharedMaterial.mainTexture;
				Vector3 point = array[0].point;
				float x = point.x;
				Vector3 lossyScale = array[0].collider.transform.lossyScale;
				point.x = x / lossyScale.x;
				float y = point.y;
				Vector3 lossyScale2 = array[0].collider.transform.lossyScale;
				point.y = y / lossyScale2.y;
				point.x = (point.x + 1f) / 2f * (float)mainTexture.width;
				point.y = (point.y + 1f) / 2f * (float)mainTexture.height;
				this.SetPixelColor(point, WorkbookModel.Instance.CurrentColorModel.Color);
			}
		}
	}

	public bool CheckClickPixel(Vector2 mousePosition)
	{
		Ray ray = this.m_mainCamera.ScreenPointToRay(mousePosition);
		RaycastHit[] array = Physics.RaycastAll(ray);
		if (array != null && array.Length > 0)
		{
			MeshRenderer component = array[0].collider.gameObject.GetComponent<MeshRenderer>();
			if (component != null)
			{
				Texture mainTexture = component.sharedMaterial.mainTexture;
				Vector3 point = array[0].point;
				float x = point.x;
				Vector3 lossyScale = array[0].collider.transform.lossyScale;
				point.x = x / lossyScale.x;
				float y = point.y;
				Vector3 lossyScale2 = array[0].collider.transform.lossyScale;
				point.y = y / lossyScale2.y;
				point.x = (point.x + 1f) / 2f * (float)mainTexture.width;
				point.y = (point.y + 1f) / 2f * (float)mainTexture.height;
				Color color = WorkbookModel.Instance.CurrentColorModel.Color;
				Color pixel = ((Texture2D)this.m_grayRenderer.sharedMaterial.mainTexture).GetPixel((int)point.x, (int)point.y);
				Color pixel2 = ((Texture2D)this.m_resMaterial.mainTexture).GetPixel((int)point.x, (int)point.y);
				if (pixel != pixel2 && pixel == color)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void SetPixelColor(Vector2 pixelPos, Color color)
	{
		Color pixel = ((Texture2D)this.m_grayRenderer.sharedMaterial.mainTexture).GetPixel((int)pixelPos.x, (int)pixelPos.y);
		if (WorkbookModel.Instance.SpecBoostersModel.LassoMode || WorkbookModel.Instance.SpecBoostersModel.BombMode)
		{
			color = pixel;
		}
		Color pixel2 = ((Texture2D)this.m_resMaterial.mainTexture).GetPixel((int)pixelPos.x, (int)pixelPos.y);
        bool pixelEqual = false;
		if (!(pixel == Color.white) && !(pixel == pixel2))
		{
			if (pixel == color)
			{
				HistoryStep historyStep = new HistoryStep();
				historyStep.Add(new ShortVector2((int)pixelPos.x, (int)pixelPos.y));
				this.AddHistoryStep(historyStep);

                pixelEqual = true;
			}
            if (!pixelEqual)
            {
                if (WorkbookModel.Instance.ColorizationModeModel.CurrentSpaceType == ColorizationModeModel.BrushType.Plural)
                {
                    int num = (int)pixelPos.x;
                    int num2 = (int)pixelPos.y;
                    float num3 = pixelPos.x - (float)num;
                    float num4 = pixelPos.y - (float)num2;
                    if (num3 < 0.25f)
                    {
                        return;
                    }
                    if (num3 > 0.75f)
                    {
                        return;
                    }
                    if (num4 < 0.25f)
                    {
                        return;
                    }
                    if (num4 > 0.75f)
                    {
                        return;
                    }
                }
                color.a = 0.5f;
                Color lhs = pixel2;
                lhs.a = 0.5f;
                if (!(lhs == color))
                {
                    AnalyticsManager.Instance.ElementColored(WorkbookModel.Instance.ColorizationModeModel.CurrentSpaceType, false);
                    pixelEqual = true;
                }
            }
		}
        if (!pixelEqual)
		    return;
        
		((Texture2D)this.m_resMaterial.mainTexture).SetPixel((int)pixelPos.x, (int)pixelPos.y, color);
		((Texture2D)this.m_resMaterial.mainTexture).Apply();
		if (color.a > 0.9f)
		{
			VibroWrapper.PlayVibroRight();
			AnalyticsManager.Instance.ElementColored(WorkbookModel.Instance.ColorizationModeModel.CurrentSpaceType, true);
			Color[] pixels = ((Texture2D)this.m_resMaterial.mainTexture).GetPixels();
			Color[] pixels2 = ((Texture2D)this.m_grayRenderer.sharedMaterial.mainTexture).GetPixels();
			if (WorkbookModel.Instance.SpecBoostersModel.LassoMode)
			{
				this.CheckLassoNeighbours(pixels2, pixels, (int)pixelPos.x, (int)pixelPos.y, color);
				((Texture2D)this.m_resMaterial.mainTexture).SetPixels(pixels);
				((Texture2D)this.m_resMaterial.mainTexture).Apply();
				WorkbookModel.Instance.SpecBoostersModel.SpendLasso();
				AudioManager.Instance.PlayWand();
				AnalyticsManager.Instance.LassoUsed(WorkbookModel.Instance.SpecBoostersModel.LassoCount + 1);
			}
			if (WorkbookModel.Instance.SpecBoostersModel.BombMode)
			{
				this.CheckBombNeighbours(pixels2, pixels, (int)pixelPos.x, (int)pixelPos.y, color);
				((Texture2D)this.m_resMaterial.mainTexture).SetPixels(pixels);
				((Texture2D)this.m_resMaterial.mainTexture).Apply();
				WorkbookModel.Instance.SpecBoostersModel.SpendBomb();
				AudioManager.Instance.PlayBomb();
				AnalyticsManager.Instance.BombUsed(WorkbookModel.Instance.SpecBoostersModel.BombCount + 1);
				foreach (Color color2 in this.m_colors)
				{
					this.CheckColor(color2, pixels, pixels2);
				}
			}
			m_colorsCount[color]--;
			m_totalNumbers--;
			this.CheckColor(color, pixels, pixels2);
			OnPaintedPixel(color);
			 
			if (m_totalNumbers == 0)
			{
				this.Completed = true;
				AnalyticsManager.Instance.ImageDone(this.m_imageInfo, this.m_colorsToColor, this.m_colors.Count);
				this.OnComplete.SafeInvoke();
				AudioManager.Instance.PlayVictory();
			}
		}
		else
		{
			VibroWrapper.PlayVibroWrong();
			DailyGame.IncresetError();
		}
	}

	public bool CheckColorLeft(Color color, bool soundEnabled = true)
	{
		if (this.m_grayRenderer.sharedMaterial.mainTexture != null && this.m_resMaterial.mainTexture != null)
		{
			Color[] pixels = ((Texture2D)this.m_resMaterial.mainTexture).GetPixels();
			Color[] pixels2 = ((Texture2D)this.m_grayRenderer.sharedMaterial.mainTexture).GetPixels();
			return this.CheckColor(color, pixels, pixels2, soundEnabled) > 0;
		}

		return false;
	}

	public int GetColorsNumber()
	{
		if (this.m_grayRenderer.sharedMaterial.mainTexture != null && this.m_resMaterial.mainTexture != null)
		{
			Color[] resPixels = ((Texture2D)this.m_resMaterial.mainTexture).GetPixels();
			Color[] rightPixels = ((Texture2D)this.m_grayRenderer.sharedMaterial.mainTexture).GetPixels();

			int count = 0;
			for (int i = 0; i < rightPixels.Length; i++)
			{
				if (resPixels[i].a < 0.9f)
				{
					count++;
				}
			}
			return count;
		}
		return 0;
	}

	private int CheckColor(Color color, Color[] resPixels, Color[] rightPixels, bool soundEnabled = true)
	{
		int count = 0;
		if (!soundEnabled)
		{
			for (int i = 0; i < rightPixels.Length; i++)
			{
				if (rightPixels[i] == color && resPixels[i].a < 0.9f)
				{
					count++;
				}
			}
			int diff = m_colorsCount[color] - count;
			m_colorsCount[color] = count;
			m_totalNumbers -= diff;
		}
		else
		{
			count = m_colorsCount[color];
		}
		if (OnCountLeft != null)
		{
			OnCountLeft(color, count);
		}
		if (count > 0)
			return count;
		this.OnColorComplete.SafeInvoke(color);
		if (soundEnabled)
		{
			AudioManager.Instance.PlayCompleteColor();
		}
		return 0;
	}

	private void CheckLassoNeighbours1(Color[] rightPixels, Color[] coloredPixels, int x, int y, Color color)
	{
		try
		{
			Vector2[] array = new Vector2[4] {
				Vector2.up,
				Vector2.down,
				Vector2.left,
				Vector2.right
			};
			Stack<Vector2> stack = new Stack<Vector2>();
			Stack<Vector2> stack2 = new Stack<Vector2>();
			bool flag = true;
			stack.Push(new Vector2((float)x, (float)y));
			int num = Mathf.RoundToInt(color.grayscale * 1000f);
			while (stack.Count > 0)
			{
				while (stack.Count > 0)
				{
					Vector2 vector = stack.Pop();
					int num2 = (int)vector.x + (int)vector.y * this.m_width;
					int num3 = Mathf.RoundToInt(coloredPixels[num2].grayscale * 1000f);
					if (num3 != num || flag)
					{
						flag = false;
						coloredPixels[num2] = color;
						Vector2[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							Vector2 vector2 = array2[i];
							int num4 = (int)vector.x + (int)vector2.x;
							int num5 = (int)vector.y + (int)vector2.y;
							Vector2 vector3 = new Vector2((float)num4, (float)num5);
							num2 = num4 + num5 * this.m_width;
							if (num4 >= 0 && num4 < this.m_width && num5 >= 0 && num5 < this.m_height)
							{
								int num6 = Mathf.RoundToInt(coloredPixels[num2].grayscale * 1000f);
								int num7 = Mathf.RoundToInt(rightPixels[num2].grayscale * 1000f);
								if (num6 != num && num7 == num)
								{
									stack2.Push(new Vector2((float)num4, (float)num5));
								}
							}
						}
					}
				}
				stack = stack2;
				stack2 = new Stack<Vector2>();
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	private void CheckLassoNeighbours(Color[] rightPixels, Color[] coloredPixels, int x, int y, Color color)
	{
		Vector2[] array = new Vector2[4] {
			Vector2.up,
			Vector2.down,
			Vector2.left,
			Vector2.right
		};
		Stack<Vector2> stack = new Stack<Vector2>();
		Stack<Vector2> stack2 = new Stack<Vector2>();
		bool flag = true;
		stack.Push(new Vector2((float)x, (float)y));
		int num = (int)((color.r + color.g * 256f + color.b * 65536f) * 1000f);
		HistoryStep historyStep = new HistoryStep();
		while (stack.Count > 0)
		{
			while (stack.Count > 0)
			{
				Vector2 vector = stack.Pop();
				int num2 = (int)vector.x + (int)vector.y * this.m_width;
				int num3 = (int)((coloredPixels[num2].r + coloredPixels[num2].g * 256f + coloredPixels[num2].b * 65536f) * 1000f);
				if (num3 != num || flag)
				{
					flag = false;
					coloredPixels[num2] = color;
					historyStep.Add(new ShortVector2((int)vector.x, (int)vector.y));
					Vector2[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						Vector2 vector2 = array2[i];
						int num4 = (int)vector.x + (int)vector2.x;
						int num5 = (int)vector.y + (int)vector2.y;
						Vector2 vector3 = new Vector2((float)num4, (float)num5);
						num2 = num4 + num5 * this.m_width;
						if (num4 >= 0 && num4 < this.m_width && num5 >= 0 && num5 < this.m_height)
						{
							int num6 = (int)((coloredPixels[num2].r + coloredPixels[num2].g * 256f + coloredPixels[num2].b * 65536f) * 1000f);
							int num7 = (int)((rightPixels[num2].r + rightPixels[num2].g * 256f + rightPixels[num2].b * 65536f) * 1000f);
							if (num6 != num && num7 == num)
							{
								stack2.Push(new Vector2((float)num4, (float)num5));
							}
						}
					}
				}
			}
			stack = stack2;
			stack2 = new Stack<Vector2>();
		}
		this.AddHistoryStep(historyStep);
	}

	private void CheckBombNeighbours(Color[] rightPixels, Color[] coloredPixels, int x, int y, Color color)
	{
		HistoryStep historyStep = new HistoryStep();
		int num = this.m_bombMatrix.GetLength(0) / 2;
		for (int i = -num; i <= num; i++)
		{
			for (int j = -num; j <= num; j++)
			{
				int num2 = x + i;
				int num3 = y + j;
				if (num2 >= 0 && num2 < this.m_width && num3 >= 0 && num3 < this.m_height && this.m_bombMatrix[num + i, num + j] > 0)
				{
					int num4 = num2 + num3 * this.m_width;
					coloredPixels[num4] = rightPixels[num4];
					historyStep.Add(new ShortVector2(num2, num3));
				}
			}
		}
		this.AddHistoryStep(historyStep);
	}

	public byte[] GetRes()
	{
		if (!(this.m_resMaterial == null) && !(this.m_resMaterial.mainTexture == null))
		{
			Texture2D texture2D = new Texture2D(this.m_width, this.m_height, TextureFormat.RGBA32, false);
			texture2D.filterMode = FilterMode.Point;
			Texture2D texture2D2 = texture2D;
			Color[] pixels = ((Texture2D)this.m_resMaterial.mainTexture).GetPixels();
			for (int i = 0; i < pixels.Length; i++)
			{
				if (pixels[i].a < 1f)
				{
					pixels[i].a = 0f;
				}
			}
			texture2D2.SetPixels(pixels);
			texture2D2.Apply();
			return texture2D2.EncodeToPNG();
		}
		return null;
	}

	public Texture2D GetFullTexture()
	{
		return (Texture2D)this.m_grayRenderer.sharedMaterial.mainTexture;
	}

	public History GetHistory()
	{
		return this.m_history;
	}

	private void AddHistoryStep(HistoryStep historyStep)
	{
		if (this.m_history == null)
		{
			ISavedWorkData savedWorkData = NewWorkbookManager.Instance.SaveWork(true);
			savedWorkData.InitHistory();
			this.m_history = savedWorkData.History2;
		}
		this.m_history.AddStep(historyStep);
	}

	public void ApplyResTexture(Texture2D tex, bool completed, History history)
	{
		this.m_resMaterial.mainTexture = tex;
		this.Completed = completed;
		if (history != null)
		{
			this.m_history = history;
		}
	}

	public void CheatComplete()
	{
		this.m_resMaterial.mainTexture = this.m_grayRenderer.sharedMaterial.mainTexture;
		this.Completed = true;
		this.OnComplete.SafeInvoke();
	}
	private IEnumerator DefferedEnableCoroutine()
	{
		yield return null;
		this.m_content.SetActive(true);
		yield return null;
		var allPix = ((Texture2D)this.m_resMaterial.mainTexture).GetPixels();
		var rightPix = ((Texture2D)this.m_grayRenderer.sharedMaterial.mainTexture).GetPixels();
		foreach(var c in m_colors)
		{
			CheckColor(c, allPix, rightPix, false);
		}

		CheckColorLeft(this.m_colors.FirstOrDefault(), false);

		if (DailyGame.IsDailyArt())
		{
			DailyGame.UpdateGUI(this.GetColorsNumber());
		}
	}


	private IEnumerator GenerateNumbersCoroutine_new(Action endAction)
	{ 
		float defaultAlpha = 1;
		float defRed = 0f;
		float defGreen = 0f;
		float defBlue = 0f;
		var numberPixels = new Color[10][];
		numberPixels[0] = ((Texture2D)Resources.Load("number_zero")).GetPixels();
		numberPixels[1] = ((Texture2D)Resources.Load("number_one")).GetPixels();
		numberPixels[2] = ((Texture2D)Resources.Load("number_two")).GetPixels();
		numberPixels[3] = ((Texture2D)Resources.Load("number_three")).GetPixels();
		numberPixels[4] = ((Texture2D)Resources.Load("number_four")).GetPixels();
		numberPixels[5] = ((Texture2D)Resources.Load("number_five")).GetPixels();
		numberPixels[6] = ((Texture2D)Resources.Load("number_six")).GetPixels();
		numberPixels[7] = ((Texture2D)Resources.Load("number_seven")).GetPixels();
		numberPixels[8] = ((Texture2D)Resources.Load("number_eight")).GetPixels();
		numberPixels[9] = ((Texture2D)Resources.Load("number_nine")).GetPixels();
		var sizeKoef = CELL_SIZE;
		var newWidth = this.m_width * sizeKoef;
		var newHeight = this.m_height * sizeKoef;
		var maxSize = 3600;
		var colors = new Color[(sizeKoef) * (sizeKoef)];
		var borderColors = new Color[sizeKoef * this.borderWidth];
		for (int i = 0; i < borderColors.Length; i++)
		{
			borderColors[i] = this.borderColor;
		}
		var colorsDict = new Dictionary<Color, int>();
		for (int i = 0; i < this.m_colors.Count; i++)
		{
			colorsDict.Add(this.m_colors[i], i + 1);
		}
		int mainI = 0;
		while (mainI * maxSize < newWidth)
		{
			var curWidth = Mathf.Min(maxSize, newWidth - mainI * maxSize);
			int mainJ = 0;
			while (mainJ * maxSize < newHeight)
			{
				var curHeight = Mathf.Min(maxSize, newHeight - mainJ * maxSize);
				var bigTex = new Texture2D(curWidth, curHeight, TextureFormat.RGBA32, false);
				var tempColors = new Color[curWidth * curHeight];
				bigTex.SetPixels(tempColors);
				yield return null;
				 
				for (int i = 0; i < curWidth / sizeKoef; i++)
				{
					if (i % 20 == 0)
					{
						yield return null;
					}

					var curI = i + mainI * maxSize / sizeKoef;

					//int firstColumnCell = -1;
					//int lastColumnCell = -1;
					for (int j = 0; j < curHeight / sizeKoef; j++)
					{
						int offset = j + mainJ * maxSize / sizeKoef;
						if (colorsDict.ContainsKey(this.m_pixels[curI + offset * this.m_width]))
						{
							//if (firstColumnCell < 0)
							//{
							//	firstColumnCell = j;
							//}
							int pixelNumber = colorsDict[this.m_pixels[curI + offset * this.m_width]];
							bigTex.SetPixels(i * sizeKoef, j * sizeKoef, sizeKoef, sizeKoef, colors);
							if (pixelNumber < 10)
							{
								bigTex.SetPixels(i * sizeKoef + 18, j * sizeKoef + 15, 14, 20, numberPixels[pixelNumber]);
							}
							else if (pixelNumber < 100)
							{
								int digit1 = pixelNumber / 10;
								int digit2 = pixelNumber - digit1 * 10;
								bigTex.SetPixels(i * sizeKoef + 9, j * sizeKoef + 15, 14, 20, numberPixels[digit1]);
								bigTex.SetPixels(i * sizeKoef + 26, j * sizeKoef + 15, 14, 20, numberPixels[digit2]);
							}
							else if (pixelNumber < 1000)
							{
								int digit1 = pixelNumber / 100;
								int temp = pixelNumber - digit1 * 100;
								int digit2 = temp / 10;
								int digit3 = temp - digit2 * 10;
								bigTex.SetPixels(i * sizeKoef + 3, j * sizeKoef + 9, 8, 12, numberPixels[digit1]);
								bigTex.SetPixels(i * sizeKoef + 11, j * sizeKoef + 9, 8, 12, numberPixels[digit2]);
								bigTex.SetPixels(i * sizeKoef + 19, j * sizeKoef + 9, 8, 12, numberPixels[digit3]);
							}

							//top border
							bigTex.SetPixels(i * sizeKoef, j * sizeKoef, sizeKoef, this.borderWidth, borderColors);

							//left border
							bigTex.SetPixels(i * sizeKoef, j * sizeKoef, this.borderWidth, sizeKoef, borderColors);

							//bottom border
							bigTex.SetPixels(i * sizeKoef, j * sizeKoef + sizeKoef - 1, sizeKoef, this.borderWidth, borderColors);

							//right border
							bigTex.SetPixels(i * sizeKoef + sizeKoef - 1, j * sizeKoef, this.borderWidth, sizeKoef, borderColors);

							//lastColumnCell = j;
						}
					}

					//if (firstColumnCell >= 0)
					//{
					//	//top border
					//	this.bigTex.SetPixels(this.i * this.sizeKoef, firstColumnCell * this.sizeKoef, this.sizeKoef, this._self.borderWidth, borderColors);
					//} 
				}
				bigTex.Apply();
				var curNumbersContent = UnityEngine.Object.Instantiate(this.m_numbersContent);
				curNumbersContent.SetParent(this.m_numbersContent.parent);
				var mf = curNumbersContent.gameObject.AddComponent<MeshFilter>();
				mf.sharedMesh = new Mesh();
				float offsetxy = -1f;
				mf.sharedMesh.vertices = new Vector3[4] {
						new Vector2(offsetxy + 2f / (float)newWidth * (float)mainI * (float)maxSize, offsetxy + 2f / (float)newHeight * (float)mainJ * (float)maxSize),
						new Vector2(offsetxy + 2f / (float)newWidth * (float)(mainI * maxSize +curWidth), offsetxy + 2f / (float)newHeight * (float)mainJ * (float)maxSize),
						new Vector2(offsetxy + 2f / (float)newWidth * (float)mainI * (float)maxSize, offsetxy + 2f / (float)newHeight * (float)(mainJ * maxSize + curHeight)),
						new Vector2(offsetxy + 2f / (float)newWidth * (float)(mainI * maxSize + curWidth), offsetxy + 2f / (float)newHeight * (float)(mainJ * maxSize + curHeight))
					};
				mf.sharedMesh.uv = new Vector2[4] { new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f) };
				mf.sharedMesh.triangles = new int[6] { 0, 3, 1, 2, 3, 0 };
				var mr = curNumbersContent.gameObject.AddComponent<MeshRenderer>();
				mr.sharedMaterial = new Material(Shader.Find("Custom/AlphaBaseShader"));
				//this.mr.sharedMaterial.SetInt("_Inverted", 1);
				mr.sharedMaterial.mainTexture = bigTex;
				mainJ++;
			}
			mainI++;
		}
		endAction.SafeInvoke(); 
	}
}
