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
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NewWorkbook3D : BaseWindow
{
	[SerializeField]
	private RectTransform m_content;

	[SerializeField]
	private List<RectTransform> m_topPanels;

	[SerializeField]
	private List<RectTransform> m_bottomPanels;

	[SerializeField]
	private Animator m_colorPanelAnimator;

	[SerializeField]
	private ColorsPanel m_colorsPanel;

	[SerializeField]
	private GameObject m_blockPlane;

	[SerializeField]
	private GameObject m_videoWaiter;

	[SerializeField]
	private GameObject m_completedPlane;

	[SerializeField]
	private List<MaskableGraphic> m_completePlateElements;

	protected override string WindowName
	{
		get
		{
			return "gameScreen";
		}
	}

	public override bool EnableBanner
	{
		get
		{
			return true;
		}
	}

	public override void Open()
	{
		base.StartCoroutine(this.OpenCoroutine());
	}

	public void RemoveVideoWaiter()
	{
		this.m_videoWaiter.SetActive(false);
	}

	public void Init(List<Color> colors)
	{
		GameController instance = UnitySingleton<GameController>.Instance;
		instance.OnComplete = (Action)Delegate.Combine(instance.OnComplete, (Action)delegate
		{
			base.StartCoroutine(this.CompletePlateAppearCoroutine());

			if (DailyGame.IsDailyArt())
			{
				DailyGame.Instance.Complete();
			}
		});
		this.m_colorsPanel.Init(colors);
	}

	public void OkButtonClick()
	{
		UnitySingleton<ProgressManager>.Instance.SaveWork(delegate (SavedWorkData3D swd)
		{
			ShareWindow shareWindow = WindowManager.Instance.OpenShareWindow();
			shareWindow.Init(swd);
			AudioManager.Instance.PlayClick();
			AdsWrapper.Instance.ShowInterOrRate("end_level");
		});
	}

	public void BackButtonClick()
	{
		UnitySingleton<ProgressManager>.Instance.SaveWork(delegate
		{
			this.Exit();
		});
		AnalyticsManager.Instance.BackButtonClicked();
		AudioManager.Instance.PlayClick();
	}

	public void CheatButtonClick()
	{
	}

	public void CloseCompletedPlateButtonClick()
	{
		this.m_completedPlane.SetActive(false);
		AdsWrapper.Instance.ShowInterOrRate("end_level");
		AudioManager.Instance.PlayClick();
		AnalyticsManager.Instance.BackButtonClicked();
	}

	public void ShareButtonClick()
	{
		UnitySingleton<ProgressManager>.Instance.SaveWork(delegate (SavedWorkData3D swd)
		{
			ShareWindow shareWindow = WindowManager.Instance.OpenShareWindow();
			shareWindow.Init(swd);
		});
	}

	public override bool Close()
	{
		UnitySingleton<ProgressManager>.Instance.SaveWork(delegate
		{
			this.Exit();
		});
		return false;
	}

	public void Exit()
	{
		AdsWrapper.Instance.ShowInter("gamescreen_exit");
		MainManager.Instance.StartLibrary(MainMenu.LastPage);
	}
	private IEnumerator OpenCoroutine()
	{
		this.SendActiveScreenEvent();
		yield return new WaitForSeconds(0.1f);

		this.m_blockPlane.SetActive(false);
		var topPanelsHeight = this.m_topPanels.Sum((RectTransform a) => a.rect.height);
		var bottomPanelsHeight = this.m_bottomPanels.Sum((RectTransform a) => a.rect.height);
		this.m_content.anchoredPosition = new Vector2(0f, (topPanelsHeight - bottomPanelsHeight) / 2f);
		this.m_content.sizeDelta = new Vector2(0f, topPanelsHeight + bottomPanelsHeight);
		Vector2 sizeDelta2 = this.m_content.sizeDelta;
		var sign = (sizeDelta2.y > 0f);
		var time = 0.5f;
		var sizeSpeed = -this.m_content.sizeDelta / time;
		var posSpeed = -this.m_content.anchoredPosition / time;

		yield return null;

		while (true)
		{ 
			var deltaTime = Mathf.Min(0.05f, Time.deltaTime);
			var sizeDelta = sizeSpeed * deltaTime;
			var posDelta = posSpeed * deltaTime; 
			var newSign = (this.m_content.sizeDelta.y + sizeDelta.y > 0f);
			if (sign != newSign)
			{
				this.m_content.sizeDelta = Vector2.zero;
				this.m_content.anchoredPosition = Vector2.zero;
				if (!AppData.SpecPageOpened && AppData.TutorialCompleted)
				{
					this.m_colorPanelAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
				}
				yield break;
			} 
			this.m_content.sizeDelta += sizeDelta; 
			this.m_content.anchoredPosition += posDelta;
			yield return null;
		}
	}
	private IEnumerator CompletePlateAppearCoroutine()
	{
		while (Input.touchCount != 0)
		{
			yield return null;
		}
		this.m_completedPlane.SetActive(true);
		var alphas = this.m_completePlateElements.Select(delegate (MaskableGraphic a)
		{
			Color color = a.color;
			return color.a;
		}).ToList();

		foreach (var item in this.m_completePlateElements)
		{
			item.SetAlpha(0);
		} 
		var time = 0.1f;
		var timer = time;

		yield return null;
		while (true)
		{
			timer -= Mathf.Min(Time.deltaTime, 0.05f);
			if (timer < 0f)
			{
				for (int i = 0; i < this.m_completePlateElements.Count; i++)
				{
					this.m_completePlateElements[i].SetAlpha(alphas[i]);
				}
				yield break;
			}
			for (int j = 0; j < this.m_completePlateElements.Count; j++)
			{
				this.m_completePlateElements[j].SetAlpha(alphas[j] * (time - timer));
			}
			yield return null;
		}
	}
}

