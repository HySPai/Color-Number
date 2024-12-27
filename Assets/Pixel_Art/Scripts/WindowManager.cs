/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
	[SerializeField]
	private Transform m_uiParent;

	[SerializeField]
	private Canvas m_canvas;

	private List<BaseWindow> m_openedWindows;

	private MainMenu m_mainMenu;

	private CreationWindow m_creationWindow;

	private DailyArtWindow m_dailyArtWindow;

	private NewWorkbook m_workbook;

	private NewWorkbook3D m_workbook3D;

	private ShareWindow m_shareWindow;

	private SettingsWindow m_settingsWindow;

	private HelpWindow m_helpWindow;

	private DetailsWindow m_detailsWindow;

	private NewInappsWindow m_inappsWindow;

	private TrialInappsWindow m_trialInappsWindow;

	private IapPopup m_abTestWindow;

	private TutorialWindow m_tutorialWindow;

	private TutorialWindow m_tutorialWindow3D;

	private FilterWindow m_filterWindow;

	public static WindowManager Instance { get; private set; }

	public int OpenedWindowsCount
	{
		get
		{
			return (this.m_openedWindows != null) ? this.m_openedWindows.Count : 0;
		}
	}

	public bool BannerEnabled
	{
		get
		{
			BaseWindow baseWindow = this.m_openedWindows.LastOrDefault();
			return baseWindow != null && baseWindow.EnableBanner;
		}
	}

	private void Awake()
	{
		WindowManager.Instance = this;
		this.m_openedWindows = new List<BaseWindow>();
	}

	public MainMenu OpenMainMenu()
	{
		if (this.m_mainMenu == null)
		{
			this.m_mainMenu = ((Component)this.m_uiParent.Find("main_menu")).GetComponent<MainMenu>();
		}
		this.m_openedWindows.Add(this.m_mainMenu);
		AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
		this.m_mainMenu.InitCanvas(this.m_canvas, true);
		this.m_mainMenu.Open();
		return this.m_mainMenu;
	}

	public MainMenu GetMainMenu()
	{
		return this.m_mainMenu;
	}

	public CreationWindow OpenCreationWindow()
	{
		if (this.m_creationWindow == null)
		{
			this.m_creationWindow = Object.Instantiate(PrefabLoader.Load<CreationWindow>("creation_window"));
			this.m_creationWindow.transform.SetParent(this.m_uiParent);
			this.m_creationWindow.transform.localScale = Vector2.one;
			((RectTransform)this.m_creationWindow.transform).anchoredPosition = Vector3.zero;
			((RectTransform)this.m_creationWindow.transform).sizeDelta = Vector2.zero;
		}
		this.m_openedWindows.Add(this.m_creationWindow);
		this.m_creationWindow.transform.SetAsLastSibling();
		AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
		this.m_creationWindow.InitCanvas(this.m_canvas, true);
		this.m_creationWindow.Open();
		return this.m_creationWindow;
	}

	public DailyArtWindow OpenDailyArtWindow()
	{
		if (this.m_dailyArtWindow == null)
		{
			this.m_dailyArtWindow = Object.Instantiate(PrefabLoader.Load<DailyArtWindow>("daily_art"));
			this.m_dailyArtWindow.transform.SetParent(this.m_uiParent);
			this.m_dailyArtWindow.transform.localScale = Vector2.one;
			((RectTransform)this.m_dailyArtWindow.transform).anchoredPosition = Vector3.zero;
			((RectTransform)this.m_dailyArtWindow.transform).sizeDelta = Vector2.zero;
		}
		this.m_openedWindows.Add(this.m_dailyArtWindow);
		this.m_dailyArtWindow.transform.SetAsLastSibling();
		AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
		this.m_dailyArtWindow.InitCanvas(this.m_canvas, true);
		this.m_dailyArtWindow.Open();
		return this.m_dailyArtWindow;
	}

	public NewWorkbook OpenWorkbook()
	{
		if (this.m_workbook == null)
		{
			this.m_workbook = ((Component)this.m_uiParent.Find("workbook")).GetComponent<NewWorkbook>();
		}
		this.m_openedWindows.Add(this.m_workbook);
		AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
		this.m_workbook.InitCanvas(this.m_canvas, true);
		this.m_workbook.Open();
		return this.m_workbook;
	}

	public NewWorkbook3D OpenWorkbook3D()
	{
		if (this.m_workbook3D == null)
		{
			this.m_workbook3D = ((Component)this.m_uiParent.Find("workbook")).GetComponent<NewWorkbook3D>();
		}
		this.m_openedWindows.Add(this.m_workbook3D);
		AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
		this.m_workbook3D.InitCanvas(this.m_canvas, true);
		this.m_workbook3D.Open();
		return this.m_workbook3D;
	}

	public TutorialWindow OpenTutorial()
	{
		if (this.m_tutorialWindow == null)
		{
			var r = PrefabLoader.Load<TutorialWindow>("tutorial_window_3d");
			if (r == null)
				return null;

			this.m_tutorialWindow = Object.Instantiate(r);
			this.m_tutorialWindow.transform.SetParent(this.m_uiParent);
			this.m_tutorialWindow.transform.localScale = Vector2.one;
			((RectTransform)this.m_tutorialWindow.transform).anchoredPosition = Vector3.zero;
			((RectTransform)this.m_tutorialWindow.transform).sizeDelta = Vector2.zero;
		}
		this.m_openedWindows.Add(this.m_tutorialWindow);
		this.m_tutorialWindow.transform.SetAsLastSibling();
		AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
		this.m_tutorialWindow.InitCanvas(this.m_canvas, true);
		this.m_tutorialWindow.Open();
		return this.m_tutorialWindow;
	}

	public TutorialWindow OpenTutorial3D()
	{
		if (this.m_tutorialWindow3D == null)
		{
			var r = PrefabLoader.Load<TutorialWindow>("tutorial_window_3d");
			if (r == null)
				return null;
			this.m_tutorialWindow3D = Object.Instantiate(r);
			if (m_tutorialWindow3D == null)
				return null;

			this.m_tutorialWindow3D.transform.SetParent(this.m_uiParent);
			this.m_tutorialWindow3D.transform.localScale = Vector2.one;
			((RectTransform)this.m_tutorialWindow3D.transform).anchoredPosition = Vector3.zero;
			((RectTransform)this.m_tutorialWindow3D.transform).sizeDelta = Vector2.zero;
		}
		this.m_openedWindows.Add(this.m_tutorialWindow3D);
		this.m_tutorialWindow3D.transform.SetAsLastSibling();
		AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
		this.m_tutorialWindow3D.InitCanvas(this.m_canvas, true);
		this.m_tutorialWindow3D.Open();
		return this.m_tutorialWindow3D;
	}

	public FilterWindow OpenFilterWindow()
	{
		if (this.m_filterWindow == null)
		{
			this.m_filterWindow = Object.Instantiate(PrefabLoader.Load<FilterWindow>("filter_window"));
			this.m_filterWindow.transform.SetParent(this.m_uiParent);
			this.m_filterWindow.transform.localScale = Vector2.one;
			((RectTransform)this.m_filterWindow.transform).anchoredPosition = Vector3.zero;
			((RectTransform)this.m_filterWindow.transform).sizeDelta = Vector2.zero;
		}
		this.m_openedWindows.Add(this.m_filterWindow);
		this.m_filterWindow.transform.SetAsLastSibling();
		AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
		this.m_filterWindow.InitCanvas(this.m_canvas, true);
		this.m_filterWindow.Open();
		return this.m_filterWindow;
	}

	public ShareWindow OpenShareWindow()
	{
		if (this.m_shareWindow == null)
		{
			this.m_shareWindow = Object.Instantiate(PrefabLoader.Load<ShareWindow>("share_window"));
			this.m_shareWindow.transform.SetParent(this.m_uiParent);
			this.m_shareWindow.transform.localScale = Vector2.one;
			((RectTransform)this.m_shareWindow.transform).anchoredPosition = Vector3.zero;
			((RectTransform)this.m_shareWindow.transform).sizeDelta = Vector2.zero;

			this.m_shareWindow.OnOpen = (System.Action)System.Delegate.Combine(this.m_shareWindow.OnOpen, new System.Action(() =>
			{
				if (GameScene.Instance != null)
				{
					GameScene.Instance.gameObject.SetActive(false);
				}
			}));
			this.m_shareWindow.OnClose = (System.Action)System.Delegate.Combine(this.m_shareWindow.OnClose, new System.Action(() =>
			{
				if (GameScene.Instance != null)
				{
					GameScene.Instance.gameObject.SetActive(true);
				}
			}));
		}
		this.m_openedWindows.Add(this.m_shareWindow);
		this.m_shareWindow.transform.SetAsLastSibling();
		AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
		this.m_shareWindow.InitCanvas(this.m_canvas, true);
		this.m_shareWindow.Open();
		AnalyticsManager.Instance.ShareWindowOpened();
		return this.m_shareWindow;
	}

	public SettingsWindow OpenSettingsWindow()
	{
		if (this.m_settingsWindow == null)
		{
			this.m_settingsWindow = Object.Instantiate(PrefabLoader.Load<SettingsWindow>("settings_window"));
			this.m_settingsWindow.transform.SetParent(this.m_uiParent);
			this.m_settingsWindow.transform.localScale = Vector2.one;
			((RectTransform)this.m_settingsWindow.transform).anchoredPosition = Vector3.zero;
			((RectTransform)this.m_settingsWindow.transform).sizeDelta = Vector2.zero;
		}
		this.m_openedWindows.Add(this.m_settingsWindow);
		this.m_settingsWindow.transform.SetAsLastSibling();
		AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
		this.m_settingsWindow.InitCanvas(this.m_canvas, true);
		this.m_settingsWindow.Open();
		AnalyticsManager.Instance.SettingsWindowOpened();
		return this.m_settingsWindow;
	}

	public HelpWindow OpenHelpWindow()
	{
		if (this.m_helpWindow == null)
		{
			this.m_helpWindow = Object.Instantiate(PrefabLoader.Load<HelpWindow>("help_window"));
			this.m_helpWindow.transform.SetParent(this.m_uiParent);
			this.m_helpWindow.transform.localScale = Vector2.one;
			((RectTransform)this.m_helpWindow.transform).anchoredPosition = Vector3.zero;
			((RectTransform)this.m_helpWindow.transform).sizeDelta = Vector2.zero;
		}
		this.m_openedWindows.Add(this.m_helpWindow);
		this.m_helpWindow.transform.SetAsLastSibling();
		AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
		this.m_helpWindow.InitCanvas(this.m_canvas, true);
		this.m_helpWindow.Open();
		AnalyticsManager.Instance.HelpWindowOpened();
		return this.m_helpWindow;
	}

	public DetailsWindow OpenDetailsWindow()
	{
		if (this.m_detailsWindow == null)
		{
			this.m_detailsWindow = Object.Instantiate(PrefabLoader.Load<DetailsWindow>("details_window"));
			this.m_detailsWindow.transform.SetParent(this.m_uiParent);
			this.m_detailsWindow.transform.localScale = Vector2.one;
			((RectTransform)this.m_detailsWindow.transform).anchoredPosition = Vector3.zero;
			((RectTransform)this.m_detailsWindow.transform).sizeDelta = Vector2.zero;
		}
		this.m_openedWindows.Add(this.m_detailsWindow);
		this.m_detailsWindow.transform.SetAsLastSibling();
		AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
		this.m_detailsWindow.InitCanvas(this.m_canvas, true);
		this.m_detailsWindow.Open();
		return this.m_detailsWindow;
	}

	public NewInappsWindow OpenInappsWindow()
	{
		if (this.m_inappsWindow == null)
		{
			this.m_inappsWindow = Object.Instantiate(PrefabLoader.Load<NewInappsWindow>("inapps_window_android"));
			this.m_inappsWindow.transform.SetParent(this.m_uiParent);
			this.m_inappsWindow.transform.localScale = Vector2.one;
			((RectTransform)this.m_inappsWindow.transform).anchoredPosition = Vector3.zero;
			((RectTransform)this.m_inappsWindow.transform).sizeDelta = Vector2.zero;
		}
		this.m_openedWindows.Add(this.m_inappsWindow);
		this.m_inappsWindow.transform.SetAsLastSibling();
		AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
		this.m_inappsWindow.InitCanvas(this.m_canvas, true);
		this.m_inappsWindow.Open();
		return this.m_inappsWindow;
	}

	//public TrialInappsWindow OpenTrialInappsWindow()
	//{
	//	if (this.m_trialInappsWindow == null)
	//	{
	//		this.m_trialInappsWindow = Object.Instantiate(PrefabLoader.Load<TrialInappsWindow>("trial_inapps_window"));
	//		this.m_trialInappsWindow.transform.SetParent(this.m_uiParent);
	//		this.m_trialInappsWindow.transform.localScale = Vector2.one;
	//		((RectTransform)this.m_trialInappsWindow.transform).anchoredPosition = Vector3.zero;
	//		((RectTransform)this.m_trialInappsWindow.transform).sizeDelta = Vector2.zero;
	//	}
	//	this.m_openedWindows.Add(this.m_trialInappsWindow);
	//	this.m_trialInappsWindow.transform.SetAsLastSibling();
	//	AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
	//	this.m_trialInappsWindow.InitCanvas(this.m_canvas, true);
	//	this.m_trialInappsWindow.Open();
	//	return this.m_trialInappsWindow;
	//}

	public IapPopup OpenAbTestWindow()
	{
		if (this.m_abTestWindow == null)
		{
			this.m_abTestWindow = Object.Instantiate(PrefabLoader.Load<IapPopup>("ab_test_inapps_window"));
			this.m_abTestWindow.transform.SetParent(this.m_uiParent);
			this.m_abTestWindow.transform.localScale = Vector2.one;
			((RectTransform)this.m_abTestWindow.transform).anchoredPosition = Vector3.zero;
			((RectTransform)this.m_abTestWindow.transform).sizeDelta = Vector2.zero;
		}
		this.m_openedWindows.Add(this.m_abTestWindow);
		this.m_abTestWindow.transform.SetAsLastSibling();
		AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
		this.m_abTestWindow.InitCanvas(this.m_canvas, true);
		this.m_abTestWindow.Open();
		return this.m_abTestWindow;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && this.m_openedWindows.Count > 0)
		{
			BaseWindow window = this.m_openedWindows.Last();
			this.CloseMe(window);
		}
	}

	public void CloseMe(BaseWindow window)
	{
		if (window.Close())
		{
			this.m_openedWindows.Remove(window);
			AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
			BaseWindow baseWindow = this.m_openedWindows.Last();
			baseWindow.SendActiveScreenEvent();
		}
	}

	public void CloseMe(BaseWindow window, BaseWindow.WindowOpenStyle style)
	{
		if (window.Close(style))
		{
			this.m_openedWindows.Remove(window);
			AdsWrapper.Instance.OpenedWindowsCountChangedHandler();
			BaseWindow baseWindow = this.m_openedWindows.Last();
			baseWindow.SendActiveScreenEvent();
		}
	}
}


