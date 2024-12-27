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
using UnityEngine.SceneManagement;

public class NewWorkbookManager : MonoBehaviour
{
	private ImageInfo m_imageInfo;

	private ImageOpenType m_imageOpenType;

	private ISavedWorkData m_savedWorkData;

	private Texture2D m_tex;

	private DateTime m_startSession = default(DateTime);

	private bool m_stopInit = true;

	[SerializeField]
	private NumberColoring m_numberColoring;

	[SerializeField]
	private ColoringVideo m_coloringVideo;

	[SerializeField]
	private CameraManager m_cameraManager;

	[SerializeField]
	private GameObject m_videoWaiter;

	public static NewWorkbookManager Instance { get; private set; }

	public CameraManager CameraManager
	{
		get
		{
			return this.m_cameraManager;
		}
	}

	public NumberColoring NumberColoring
	{
		get
		{
			return this.m_numberColoring;
		}
	}

	private void Awake()
	{
		NewWorkbookManager.Instance = this;
		WorkbookModel.Init();
		CurrentColorModel currentColorModel = WorkbookModel.Instance.CurrentColorModel;
		currentColorModel.OnStateChanged = (Action<CurrentColorModel, bool>)Delegate.Combine(currentColorModel.OnStateChanged, new Action<CurrentColorModel, bool>(this.OnCurrentColorChangedHandler));
		AdsWrapper.Instance.RequestVideo();
	}

	private void Start()
	{
		base.StartCoroutine(this.StopInitAndShowInterCoroutine());
	}

	public void Init(ImageInfo imageInfo, ImageOpenType imageOpenType, ISavedWorkData savedWorkData)
	{
		Application.targetFrameRate = 30;
		this.m_imageInfo = imageInfo;
		this.m_imageOpenType = imageOpenType;
		this.m_savedWorkData = savedWorkData;
		base.StartCoroutine(this.InitCoroutine());
	}
	private void OnCurrentColorChangedHandler(CurrentColorModel model, bool val)
	{
		this.NumberColoring.SetColor(model.Color);
	}

	public ISavedWorkData SaveWork(bool force = false)
	{
		UnityEngine.Debug.Log("save");
		if (this.m_savedWorkData != null || SavedWorkData.IsNeedToSave(this.m_numberColoring) || force)
		{
			byte[] res = this.m_numberColoring.GetRes();
			if (res != null)
			{
				this.m_savedWorkData = MainManager.Instance.SavedWorksList.Save(this.m_imageInfo, res, this.m_savedWorkData, this.m_numberColoring.Completed);
			}
		}
		return this.m_savedWorkData;
	}

	public void PlayVideo()
	{
		this.SaveWork(false);
		if (this.m_savedWorkData != null)
		{
			this.m_coloringVideo.Init(this.NumberColoring.GetFullTexture(), this.m_savedWorkData);
		}
	}

	public void Exit()
	{
		this.SaveWork(false);
		AdsWrapper.Instance.ShowInter("gamescreen_exit");
		MainManager.Instance.StartLibrary(MainMenu.LastPage);
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			this.SaveWork(false);
		}
	}

	private void OnDestroy()
	{
		AdsWrapper.Instance.OnWorkbookUnload();
		AnalyticsManager.Instance.ColoringSession((DateTime.Now - this.m_startSession).TotalSeconds);
		if (this.m_savedWorkData != null && this.m_savedWorkData.History2 != null)
		{
			this.m_savedWorkData.History2.Close();
		}
	}
	private IEnumerator InitCoroutine()
	{
		while (true)
		{ 
			if (this.m_stopInit)
			{
				yield return null;
				continue;
			}
			if (INPluginWrapper.Instance.IsAdShown())
			{
				yield return null;
				continue;
			}
			if (this.m_imageInfo.Url != null)
			{
				DataManager.Instance.GetImageAsset(this.m_imageInfo, delegate (bool res, Texture2D tex)
				{
					if ((UnityEngine.Object)tex == (UnityEngine.Object)null)
					{
						MainManager.Instance.StartLibrary(MainMenu.LastPage);
						DialogToolWrapper.ShowNoInternetDialog();
					}
					else
					{
						this.m_tex = tex;
						this.StartCoroutine(this.LoadImageCoroutine());
					}
				});
			}
			else
			{
				DataManager.Instance.GetPhotoAsset(this.m_imageInfo.Id, delegate (bool res, Texture2D tex)
				{
					if ((UnityEngine.Object)tex == (UnityEngine.Object)null)
					{
						MainManager.Instance.StartLibrary(MainMenu.LastPage);
						DialogToolWrapper.ShowNoInternetDialog();
					}
					else
					{
						this.m_tex = tex;
						this.StartCoroutine(this.LoadImageCoroutine());
					}
				});
			}

			yield break;
		}
	}


	private IEnumerator StopInitAndShowInterCoroutine()
	{ 
		if (SceneManager.GetActiveScene().name != "2DScene")
		{
			this.m_stopInit = false;
		}
		else
		{
			if (INPluginWrapper.Instance.GetAbTestGroup() != ABTestGroup.None && INPluginWrapper.Instance.GetAbTestGroup() != ABTestGroup.RewardedNo_ContentMedium_Old && INPluginWrapper.Instance.GetAbTestGroup() != ABTestGroup.RewardedNo_ContentMedium_Revealed)
			{ 
				if (!IAPWrapper.Instance.NoAds && AppData.TutorialCompleted)
				{
					yield return new WaitForSeconds(0.1f);
					if (AdsWrapper.Instance.ShowInter("preview"))
					{
						yield return new WaitForSeconds(1f); 
					}
				}
			}
			if (this.m_imageInfo != null && (this.m_imageInfo.CustomAccessStatus == AccessStatus.Free || IAPWrapper.Instance.Subscribed || AppData.UnlockedImages.Contains(this.m_imageInfo.Id)))
			{
				if (!IAPWrapper.Instance.NoAds && AppData.TutorialCompleted)
				{
					yield return new WaitForSeconds(0.1f);
					if (AdsWrapper.Instance.ShowInter("preview"))
					{
						yield return new WaitForSeconds(1f); 
					}
				}
			} 
		}
		this.m_stopInit = false;
	}


	private IEnumerator LoadImageCoroutine()
	{ 
		this.m_numberColoring.Init(this.m_imageInfo, this.m_imageOpenType, this.m_tex);
		yield return null;

		this.CameraManager.EnableBackCamera();
		var video = false;
		if ((INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.None || INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.RewardedNo_ContentMedium_Old || INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.RewardedNo_ContentMedium_Revealed) && this.m_imageInfo.CustomAccessStatus != 0 && !IAPWrapper.Instance.Subscribed && !AppData.UnlockedImages.Contains(this.m_imageInfo.Id))
		{
			this.m_videoWaiter.SetActive(true); 
			AppData.UnlockedImages.Add(this.m_imageInfo.Id); 
			var timer = 7f; 

			while (timer >= 0f)
			{
#if UNITY_EDITOR
				video = true; 
				AdsWrapper.Instance.ShowVideo("image", null);
#elif UNITY_ANDROID
				var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call("runOnUiThread", (AndroidJavaRunnable)delegate
                {
                    if (AdsWrapper.Instance.IsVideoAvailable())
                    {
                        video = true;
                        AdsWrapper.Instance.ShowVideo("image", null);
                    }
                });
#else
				if (AdsWrapper.Instance.IsVideoAvailable())
                {
                    video = true;
                    AdsWrapper.Instance.ShowVideo("image", null);
                }
#endif 
				yield return new WaitForSeconds(1f);
				if (video)
					break;
				timer -= 1f;
			}
		}

		var workbook = WindowManager.Instance.OpenWorkbook();
		workbook.RemoveVideoWaiter();
		if (this.m_savedWorkData != null)
		{
			this.m_savedWorkData.Apply(this.m_numberColoring);
		}
		workbook.Init(this.m_numberColoring.GetColors());
		this.m_cameraManager.Init((float)this.m_tex.width, (float)this.m_tex.height);

		while (Time.deltaTime > 0.1f)
		{
			yield return null;
		}
		if (!AppData.TutorialCompleted)
		{
			TutorialWindow tutorialWindow = WindowManager.Instance.OpenTutorial();
			if (tutorialWindow != null)
			{
				tutorialWindow.Init("picture", "2D");
			}
		}
		this.m_startSession = DateTime.Now;
		AnalyticsManager.Instance.StartColoringSession();
		this.StartCoroutine(this.SaveWorkCoroutine());
		yield break;
	}


	private IEnumerator SaveWorkCoroutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(30f);
			this.SaveWork(false);
		}
	}
}
