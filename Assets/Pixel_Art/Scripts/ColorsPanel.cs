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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ColorsPanel : MonoBehaviour
{
	public SimpleObjectPooler[] poolers;

	public Action<int, Color> OnSelectColor;

	private List<Image> m_points;

	[SerializeField]
	private LayoutElement m_layoutElement;

	[SerializeField]
	private ColorsPage m_colorsPagePrefab;

	[SerializeField]
	private Transform m_colorsPagesParent;

	[SerializeField]
	private PositScrollRect m_positScrollRect;

	[SerializeField]
	private RectTransform m_specPage;

	[SerializeField]
	private RectTransform m_specPageInternal;

	[SerializeField]
	private List<ColorsPage> m_colorsPages;

	[SerializeField]
	private ColorPointer m_colorPointer;

	[SerializeField]
	private Image m_pointPrefab;

	[SerializeField]
	private RectTransform m_pointsParent;

	[SerializeField]
	private float m_pointsDelta = 20f;

	public GameObject restColorPanel;

	public Image restColorIndicator;

	public Text restLabel;

	public Animator _resetTextAni;

	private bool isWorking3D = false;
	private bool colorButtonFirstClicked = true;
	private void Start()
	{
		isWorking3D = false;
		if (SceneManager.GetActiveScene().name == "3DScene")
		{
			isWorking3D = true;
		}
		BackgroundMusic.PlayInGameBackground();
	}
	public void Init(List<Color> colors)
	{
		restColorPanel.SetActive(false);

		Vector2 sizeDelta = ((RectTransform)base.transform).sizeDelta;
		Vector2 anchoredPosition = ((RectTransform)base.transform).anchoredPosition;
		float num = 0f - anchoredPosition.y + sizeDelta.y;
		int count = 0;
		count = 10;
		while (!(sizeDelta.x / (float)count / num * 2f >= 0.25f))
		{
			count--;
		}
		count++;
		int rowWidth = Mathf.Max(count, 5);
		float colorSize = sizeDelta.x / (float)rowWidth;
		int num5 = rowWidth * 2;
		int num6 = Mathf.CeilToInt((float)colors.Count / (float)num5);
		for (count = 0; count < num6; count++)
		{
			List<Color> range = colors.GetRange(count * num5, Math.Min(num5, colors.Count - count * num5));
			ColorsPage colorsPage = UnityEngine.Object.Instantiate(this.m_colorsPagePrefab);
			colorsPage.transform.SetParent(this.m_colorsPagesParent);
			colorsPage.transform.localScale = Vector3.one;
			colorsPage.gameObject.SetActive(true);
			this.m_colorsPages.Add(colorsPage);
			colorsPage.Init(range, count * num5, rowWidth, colorSize, this.m_colorsPages.Count - 1);
			ColorsPage colorsPage2 = colorsPage;
			colorsPage2.OnColorClick = (Action<ColorImage, SpecialColorPosition>)Delegate.Combine(colorsPage2.OnColorClick, new Action<ColorImage, SpecialColorPosition>(this.OnColorClickedHandler));
		}
		Vector2 sizeDelta2 = this.m_pointsParent.sizeDelta;
		float y = sizeDelta2.y;
		if (colors.Count <= rowWidth)
		{
			LayoutElement layoutElement = this.m_layoutElement;
			float num7 = colorSize + y;
			this.m_layoutElement.preferredHeight = num7;
			layoutElement.minHeight = num7;
		}
		else
		{
			LayoutElement layoutElement2 = this.m_layoutElement;
			float num7 = colorSize * 2f + y;
			this.m_layoutElement.preferredHeight = num7;
			layoutElement2.minHeight = num7;
		}
		this.m_specPage.sizeDelta = new Vector2(this.m_specPage.rect.width, this.m_layoutElement.minHeight);
		this.m_specPageInternal.sizeDelta = new Vector2(0f, colorSize * 2f);
		this.m_specPageInternal.anchoredPosition = new Vector2(0f, y);
		this.m_positScrollRect.Reinit(-1f, false);
		if (this.m_colorsPages.Count > 0)
		{
			this.m_colorsPages[0].SelectFirstColor();
			colorButtonFirstClicked = true;
			this.GeneratePoints();
			PositScrollRect positScrollRect = this.m_positScrollRect;
			positScrollRect.OnSelectIndex = (Action<int>)Delegate.Combine(positScrollRect.OnSelectIndex, new Action<int>(this.OnIndexChangedHandler));
			this.m_positScrollRect.SetIndex(this.m_specPage.gameObject.activeSelf ? 1 : 0);
		}
		else
		{
			this.m_colorPointer.gameObject.SetActive(false);
		}
		if (SceneManager.GetActiveScene().name == "2DScene")
		{
			NumberColoring numberColoring = NewWorkbookManager.Instance.NumberColoring;
			numberColoring.OnColorComplete = (Action<Color>)Delegate.Combine(numberColoring.OnColorComplete, new Action<Color>(this.OnColorCompleteHandler));
			numberColoring.OnCountLeft = (Action<Color, int>)Delegate.Combine(numberColoring.OnCountLeft, new Action<Color, int>(this.OnColorCount));
			numberColoring.OnPaintedPixel = (Action<Color>)Delegate.Combine(numberColoring.OnPaintedPixel, new Action<Color>(this.OnPaintPixel));
		}
		else
		{
			GameController instance = UnitySingleton<GameController>.Instance;
			instance.OnColorComplete = (Action<Color>)Delegate.Combine(instance.OnColorComplete, new Action<Color>(this.OnColorCompleteHandler));
			instance.OnCountLeft = (Action<Color, int>)Delegate.Combine(instance.OnCountLeft, new Action<Color, int>(this.OnColorCount));
			instance.OnPaintedPixel = (Action<Color>)Delegate.Combine(instance.OnPaintedPixel, new Action<Color>(this.OnPaintPixel));
			UnitySingleton<GameController>.Instance.UpdateColors();
		}

		if (this.m_colorsPages.Count > 0)
		{
			this.m_colorsPages[0].SelectFirstColor();
		}
	}

	private void GeneratePoints()
	{
		int num = this.m_colorsPages.Count + (this.m_specPage.gameObject.activeSelf ? 1 : 0);
		float num2 = ((float)(-num) / 2f + 0.5f) * this.m_pointsDelta;
		this.m_points = new List<Image>(num);
		for (int i = 0; i < num; i++)
		{
			Image image = UnityEngine.Object.Instantiate(this.m_pointPrefab);
			image.gameObject.SetActive(true);
			image.transform.SetParent(this.m_pointsParent);
			image.transform.localScale = Vector2.one;
			((RectTransform)image.transform).anchoredPosition = new Vector2(num2 + (float)i * this.m_pointsDelta, 0f);
			this.m_points.Add(image);
		}
	}

	private void OnColorClickedHandler(ColorImage colorImage, SpecialColorPosition pos)
	{
		this.restColorPanel.SetActive(true);

		if (SceneManager.GetActiveScene().name == "3DScene")
		{
			UnitySingleton<GameController>.Instance.SetCurrentIndexCollorPallete(colorImage.ColorIndex);
		}
		WorkbookModel.Instance.CurrentColorModel.UpdateColor(colorImage.Color);
		this.m_colorPointer.UpdateColor(colorImage, pos);

		if (isWorking3D)
		{
			bool stillAvailable = UnitySingleton<GameController>.Instance.CheckColorLeft(colorImage.ColorIndex, !colorButtonFirstClicked);
			this.restColorPanel.SetActive(stillAvailable);
			if (stillAvailable)
			{
				this.restColorPanel.GetComponent<Animator>().Play("RestPanelDefaultAni", -1, 0f);
			}
		}
		else
		{
			bool stillAvailable = NewWorkbookManager.Instance.NumberColoring.CheckColorLeft(colorImage.Color, !colorButtonFirstClicked);
			this.restColorPanel.SetActive(stillAvailable);
			if (stillAvailable)
			{
				this.restColorPanel.GetComponent<Animator>().Play("RestPanelDefaultAni", -1, 0f);
			}
		}

		this.restColorIndicator.color = colorImage.Color;

		colorButtonFirstClicked = false;
	}
	private System.Collections.IEnumerator DestroyEffectRoutine(GameObject obj)
	{
		yield return new WaitForSeconds(1f);
		obj.SetActive(false);
	}
	private void PlayParticle(Color color, Vector3 pos, GameObject particle, bool shouldChangeColor = true, bool repeat = false)
	{
		particle.transform.localScale = Vector3.one;
		particle.transform.position = pos;
		particle.SetActive(true);
		if (shouldChangeColor)
		{
			ParticleSystem component = particle.GetComponent<ParticleSystem>();
			ParticleSystem.MainModule main = component.main;
			main.startColor = color;
			Transform transform = particle.transform.Find("2-2");
			if (transform != null)
			{
				ParticleSystem component2 = (transform).GetComponent<ParticleSystem>();
				if (component2 != null)
				{
					var m = component2.main;
					m.startColor = color;
				}
			}
		}
		if (!repeat)
		{
			base.StartCoroutine(this.DestroyEffectRoutine(particle));
		}
	}
	private void OnPaintPixel(Color color)
	{
		AudioManager.Instance.PlayColor();

		if (isWorking3D)
		{
			this.PlayParticle(color, Camera.main.ScreenPointToRay(Input.mousePosition).origin, this.poolers[1].GetPooledGameObject(), true, false);
		}
		//else
		//{
		//	this.PlayParticle(color, Camera.main.ScreenToViewportPoint(Input.mousePosition), this.poolers[1].GetPooledGameObject(), true, false);
		//}
	}

	private void OnColorCount(Color color, int restColorCount)
	{
		if (!this.restLabel.text.Equals(string.Format("{0}", restColorCount)))
		{
			this.restLabel.text = string.Format("{0}", restColorCount);
			this._resetTextAni.Play("ResetTextChangeOnce", -1, 0f);
		}
		if (restColorCount == 0)
		{
			Animator a = this.restColorPanel.GetComponent<Animator>();
			if (a != null)
			{
				a.Play("Disappear", -1, 0f);
			}
		}
		else
		{
			//this.restColorIndicator.color = color;
		}
	}

	private void OnColorCompleteHandler(Color color)
	{
		foreach (ColorsPage colorsPage in this.m_colorsPages)
		{
			colorsPage.DisableColor(color);
		}

		OnColorCount(color, 0);
	}

	private void OnIndexChangedHandler(int index)
	{
		if (index == 0 && !AppData.SpecPageOpened)
		{
			AppData.SpecPageOpened = true;
		}
		if (index > 0)
		{
			WorkbookModel.Instance.SpecBoostersModel.BombMode = false;
			WorkbookModel.Instance.SpecBoostersModel.LassoMode = false;
		}
		if (index >= 0 && index < this.m_points.Count)
		{
			for (int i = 0; i < this.m_points.Count; i++)
			{
				this.m_points[i].SetAlpha((i != index) ? 0.7f : 1f);
			}
		}
	}
}


