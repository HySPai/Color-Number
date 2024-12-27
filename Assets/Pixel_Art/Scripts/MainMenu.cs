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
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : BaseWindow
{
	private static bool FirstOpen = true;

	private bool m_buttonsBlocked;

	[SerializeField]
	private GameObject m_blockPlane;

	[SerializeField]
	private NewLibraryWindow m_newLibraryWindow;

	[SerializeField]
	private NewMyWorksWindow m_myWorksWindow;

	[SerializeField]
	private MyPhotosWindow m_photosWindow;

	[SerializeField]
	private SettingsWindow m_settingsWindow;

	[SerializeField]
	private LibraryTabButton m_libraryTab;

	[SerializeField]
	private LibraryTabButton m_myWorksTab;

	[SerializeField]
	private LibraryTabButton m_photosTab;

	[SerializeField]
	private LibraryTabButton m_settingsTab;
	 
	[SerializeField]
	private Text rolloverTimeText;

	[SerializeField]
	private Text bestTimeText;
	 
	public ImagePreview todayArt;

	[SerializeField]
	private Material grayMaterial;

	private bool m_inited; 

	private BaseWindow m_currentWindow;

	public static MainMenuPage LastPage { get; set; }

	public static string ImageId { get; set; }

	public static string WorkId { get; set; }

	protected override string WindowName
	{
		get
		{
			return null;
		}
	}

	public override bool EnableBanner
	{
		get
		{
			return true;
		}
	}

	public static MainMenu Instance;

	private void Awake()
	{
		Instance = this;
		NewMyWorksWindow myWorksWindow = this.m_myWorksWindow;
		myWorksWindow.OnStartOpen = (Action)Delegate.Combine(myWorksWindow.OnStartOpen, new Action(this.OnMyWorksStartOpenHandler));
		NewLibraryWindow newLibraryWindow = this.m_newLibraryWindow;
		newLibraryWindow.OnStartOpen = (Action)Delegate.Combine(newLibraryWindow.OnStartOpen, new Action(this.OnLibraryStartOpenHandler));
		MyPhotosWindow photosWindow = this.m_photosWindow;
		photosWindow.OnStartOpen = (Action)Delegate.Combine(photosWindow.OnStartOpen, new Action(this.OnCreationTabStartOpenHandler));
		SettingsWindow settingsWindow = this.m_settingsWindow;
		settingsWindow.OnStartOpen = (Action)Delegate.Combine(settingsWindow.OnStartOpen, new Action(this.OnSettingsStartOpenHandler));
		BackgroundMusic.PlayMainBackground();
	}

	private void Start()
	{
		base.InvokeRepeating("DailyRolloverCountdown", 0f, 60f);
		 
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)LevelProgressControl.control.dailyTodayPersonalBest);
		string str = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
		this.bestTimeText.text =  str;
		 
	}
	private void DailyRolloverCountdown()
	{
		DateTime dateTime = DateTime.UtcNow.AddHours(-8.0);
		UnityEngine.Debug.Log("UTC Now today (PST) : " + dateTime);
		DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
		UnityEngine.Debug.Log("UTC Rollover midnight 24 hrs early (PST) : " + dateTime2);
		DateTime dateTime3 = dateTime2.AddDays(1.0);
		UnityEngine.Debug.Log("UTC Rollover midnight (PST) : " + dateTime3);
		TimeSpan timeSpan = dateTime3.Subtract(dateTime);
		UnityEngine.Debug.Log("Time until roll over " + timeSpan);
		string str = string.Format("{0:D2}:{1:D2}", timeSpan.Hours, timeSpan.Minutes);
		this.rolloverTimeText.text = "New Puzzle in " + str;
	}

	public void RefreshTodayArt()
	{
		GetNameScript.GetTodayArt(todayArt, grayMaterial);
		DailyRolloverCountdown();

		TimeSpan timeSpan = TimeSpan.FromSeconds((double)LevelProgressControl.control.dailyTodayPersonalBest);
		string str = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
		this.bestTimeText.text = str;
	}

	public void Init(MainMenuPage page)
	{
		Application.targetFrameRate = -1;
		if (!this.m_inited)
		{
			this.m_inited = true;
            Debug.Log("Main menu initialize");
			DataManager.Instance.GetImages(false, delegate (ImagesInfo list)
            {
                Debug.Log("Main Menu 136");
				if (this.m_newLibraryWindow != null)
                {
                    Debug.Log("Main Menu 139");
					this.m_newLibraryWindow.FastOpen();
					this.m_newLibraryWindow.Init(list);
					base.StartCoroutine(this.WaitInitLibraryAndClosePlaneCoroutine());
				}
			});
		}
		base.gameObject.SetActive(true);
		switch (page)
		{
			case MainMenuPage.Library:
				this.m_currentWindow = this.m_newLibraryWindow;
				this.OnLibraryStartOpenHandler();
				AnalyticsManager.Instance.LibraryOpened();
				break;
			case MainMenuPage.MyWorks:
				this.m_newLibraryWindow.FastClose();
				this.m_myWorksWindow.FastOpen();
				this.m_currentWindow = this.m_myWorksWindow;
				this.m_myWorksWindow.Init();
				this.OnMyWorksStartOpenHandler();
				AnalyticsManager.Instance.MyWorksOpened();
				break;
			case MainMenuPage.Photos:
				this.m_newLibraryWindow.FastClose();
				this.m_photosWindow.FastOpen();
				this.m_currentWindow = this.m_photosWindow;
				this.m_photosWindow.Init();
				this.OnCreationTabStartOpenHandler();
				AnalyticsManager.Instance.PhotosOpened();
				break;
		}
	}

	public override void InitCanvas(Canvas canvas, bool setClosePosition = true)
	{
		base.InitCanvas(canvas, setClosePosition);
		this.m_newLibraryWindow.InitCanvas(canvas, false);
		this.m_myWorksWindow.InitCanvas(canvas, true);
		this.m_photosWindow.InitCanvas(canvas, true);
		this.m_settingsWindow.InitCanvas(canvas, true);
	}

	public void UpdateLibrary(ImagesInfo imagesInfo)
	{
		if (this.m_newLibraryWindow != null)
		{
			UnityEngine.Debug.Log("reinit imagesInfo");
			this.m_newLibraryWindow.Init(imagesInfo);
		}
	}

	public override bool Close()
	{
		if (this.m_myWorksWindow.isActiveAndEnabled)
		{
			this.m_myWorksWindow.Close();
			this.m_newLibraryWindow.DefferedOpen(0.1f);
			AnalyticsManager.Instance.LibraryOpened();
			return false;
		}
		if (this.m_photosWindow.isActiveAndEnabled)
		{
			this.m_photosWindow.Close();
			this.m_newLibraryWindow.DefferedOpen(0.1f);
			AnalyticsManager.Instance.PhotosOpened();
		}
		SystemToolsWrapper.Minimize();
		return false;
	}

	public void LibraryButtonClick()
	{
		if (this.m_currentWindow != this.m_newLibraryWindow && !this.m_buttonsBlocked)
		{
			this.m_buttonsBlocked = true;
			AdsWrapper.Instance.ShowInter("tab_change");
			WindowManager.Instance.CloseMe(this.m_currentWindow, WindowOpenStyle.FromRight);
			this.m_currentWindow = this.m_newLibraryWindow;
			this.m_newLibraryWindow.DefferedOpen(0.1f);
			this.SelectTab(this.m_libraryTab);
			AnalyticsManager.Instance.LibraryOpened();
			AudioManager.Instance.PlayClick();
		}
	}

	public void MyWorkButtonClick()
	{
		if (this.m_currentWindow != this.m_myWorksWindow && !this.m_buttonsBlocked)
		{
			this.m_buttonsBlocked = true;
			AdsWrapper.Instance.ShowInter("tab_change");
			this.m_myWorksWindow.Init();
			if (this.m_currentWindow == this.m_newLibraryWindow)
			{
				WindowManager.Instance.CloseMe(this.m_currentWindow, WindowOpenStyle.FromLeft);
				this.m_myWorksWindow.DefferedOpen(0.1f);
			}
			else
			{
				WindowManager.Instance.CloseMe(this.m_currentWindow, WindowOpenStyle.FromRight);
				this.m_myWorksWindow.DefferedOpen(WindowOpenStyle.FromLeft, 0.1f);
			}
			this.m_currentWindow = this.m_myWorksWindow;
			this.SelectTab(this.m_myWorksTab);
			AnalyticsManager.Instance.MyWorksOpened();
			AudioManager.Instance.PlayClick();
		}
	}

	public void CreateButtonClick()
	{
		if (this.m_currentWindow != this.m_photosWindow && !this.m_buttonsBlocked)
		{
			this.m_buttonsBlocked = true;
			AdsWrapper.Instance.ShowInter("tab_change");
			this.m_photosWindow.Init();
			if (this.m_currentWindow == this.m_newLibraryWindow || this.m_currentWindow == this.m_myWorksWindow)
			{
				WindowManager.Instance.CloseMe(this.m_currentWindow, WindowOpenStyle.FromLeft);
				this.m_photosWindow.DefferedOpen(0.1f);
			}
			else
			{
				WindowManager.Instance.CloseMe(this.m_currentWindow, WindowOpenStyle.FromRight);
				this.m_photosWindow.DefferedOpen(WindowOpenStyle.FromLeft, 0.1f);
			}
			this.m_currentWindow = this.m_photosWindow;
			this.SelectTab(this.m_photosTab);
			AnalyticsManager.Instance.PhotosOpened();
			AudioManager.Instance.PlayClick();
		}
	}

	public void QuestionButtonClick()
	{
		if (this.m_currentWindow != this.m_settingsWindow && !this.m_buttonsBlocked)
		{
			this.m_buttonsBlocked = true;
			WindowManager.Instance.CloseMe(this.m_currentWindow, WindowOpenStyle.FromLeft);
			this.m_currentWindow = this.m_settingsWindow;
			AnalyticsManager.Instance.MyWorksOpened();
			AudioManager.Instance.PlayClick();
			this.m_settingsWindow.DefferedOpen(0.1f);
			this.SelectTab(this.m_settingsTab);
		}
	}

	private void OnMyWorksStartOpenHandler()
	{
		this.m_buttonsBlocked = false;
		this.SelectTab(this.m_myWorksTab);
	}

	private void OnCreationTabStartOpenHandler()
	{
		this.m_buttonsBlocked = false;
		this.SelectTab(this.m_photosTab);
	}

	private void OnLibraryStartOpenHandler()
	{
		this.m_buttonsBlocked = false;
		this.SelectTab(this.m_libraryTab);
	}

	private void OnSettingsStartOpenHandler()
	{
		this.m_buttonsBlocked = false;
		this.SelectTab(this.m_settingsTab);
	}

	private void SelectTab(LibraryTabButton tab)
	{
		this.m_libraryTab.SetHighlighted(this.m_libraryTab == tab);
		this.m_myWorksTab.SetHighlighted(this.m_myWorksTab == tab);
		this.m_photosTab.SetHighlighted(this.m_photosTab == tab);
		this.m_settingsTab.SetHighlighted(this.m_settingsTab == tab);
	}

	public override void SendActiveScreenEvent()
	{
		if (this.m_inited)
		{
			this.m_currentWindow.SendActiveScreenEvent();
		}
	}
	private IEnumerator WaitInitLibraryAndClosePlaneCoroutine()
	{
		if (MainMenu.FirstOpen)
		{
			MainMenu.FirstOpen = false;
			float timer = 0.5f;
			yield return null;

			while (true)
			{
				timer -= Time.deltaTime;
				if (!(timer < 0f) && (!this.m_newLibraryWindow.isActiveAndEnabled || !this.m_newLibraryWindow.Inited) && (!this.m_myWorksWindow.isActiveAndEnabled || !this.m_myWorksWindow.Inited))
				{
					if (!this.m_photosWindow.isActiveAndEnabled)
					{
						yield return null;
						continue;
					}
					if (!this.m_photosWindow.Inited)
					{
						yield return null;
						continue;
					}
				}
				break;
			}
		}
		this.m_blockPlane.SetActive(false);
	}

	public void OpenDailyArt()
	{
		AudioManager.Instance.PlayClick();
		var w = WindowManager.Instance.OpenDailyArtWindow();
		
		w.Open();
		if (GetNameScript.Instance != null)
		{
			GetNameScript.Instance.Init();
		}
	}
}
