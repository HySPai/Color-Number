/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using Assets.Scripts.Navigation;
using Assets.Scripts.Navigation.Scenes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxDLL;

internal class GameScene : BaseScene
{
	[Range(0.01f, 5f)]
	[SerializeField]
	private float sizePerVox = 1f;

	[SerializeField]
	private Material voxMaterial;

	[SerializeField]
	private Transform meshOrigin;

	[SerializeField]
	private GameObject saveCamera;

	[SerializeField]
	private Camera videoCamera;

	private MVMainChunk v;

	private Loader3D load3D;

	private bool m_stopInit = true;

	private ImageInfo m_imageInfo;

	[SerializeField]
	private GameObject m_videoWaiter;

	public static GameScene Instance;

	public override SceneType SceneType
	{
		get
		{
			return SceneType.Game;
		}
	}

	public override void Start()
	{
		Instance = this;
		base.Start();
		base.StartCoroutine(this.StopInitAndShowInterCoroutine());
	}

	public override void OnNavigatedTo(NavigationArgs args)
	{
		GameNavigationArgs gameNavigationArgs = args as GameNavigationArgs;
		base.StartCoroutine(this.InitCoroutine(gameNavigationArgs.ImageInfo, gameNavigationArgs.Data, gameNavigationArgs.SavedWorkData, gameNavigationArgs.ImageOpenType));
	}

	public override void OnNavigatedFrom()
	{
		BackButtonManager instance = UnitySingleton<BackButtonManager>.Instance;
		instance.BackButtonAction = (Action)Delegate.Remove(instance.BackButtonAction, new Action(((BaseScene)this).BackButtonActions));
	}

	public override void BackButtonActions()
	{
		UnityEngine.Debug.Log(" Touch BackButtonActions");
	}

	public void CheatColorAll()
	{
		VoxCubeItem[] array = UnityEngine.Object.FindObjectsOfType<VoxCubeItem>();
		VoxCubeItem[] array2 = array;
		foreach (VoxCubeItem voxCubeItem in array2)
		{
			voxCubeItem.CheatTochCubeItem();
			UnitySingleton<GameController>.Instance.SetCubeItemProgress(voxCubeItem);
		}
	}

	private void VoxLoadCompleteHandler(List<Color> list)
	{
		base.StartCoroutine(this.OpenUICoroutine(list));
	}

	private void ClearVoxMeshes()
	{
		MVVoxModelMesh[] componentsInChildren = base.gameObject.GetComponentsInChildren<MVVoxModelMesh>();
		MVVoxModelMesh[] array = componentsInChildren;
		foreach (MVVoxModelMesh mVVoxModelMesh in array)
		{
			UnityEngine.Object.DestroyImmediate(mVVoxModelMesh.gameObject);
		}
		MVVoxModelVoxel[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<MVVoxModelVoxel>();
		MVVoxModelVoxel[] array2 = componentsInChildren2;
		foreach (MVVoxModelVoxel mVVoxModelVoxel in array2)
		{
			UnityEngine.Object.DestroyImmediate(mVVoxModelVoxel.gameObject);
		}
	}

	private void VoxLoad(byte[] data = null)
	{
		if (data != null)
		{
			this.load3D = new Loader3D();
			this.load3D.Init(new Action<Color[]>(this.LoadPalleteComplete), new Action<List<VoxCubeItem>>(this.LoadVoxCubeComplete));
			if (data != null)
			{
				this.load3D.Load(data);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("[MVVoxModel] Invalid file path or data");
		}
	}

	private void LoadVoxCubeComplete(List<VoxCubeItem> items)
	{
		UnitySingleton<GameController>.Instance.VoxCubeItems = items;
	}

	private void LoadPalleteComplete(Color[] colors)
	{
		UnitySingleton<GameController>.Instance.Initilized(colors);
	}

	private void LoadCustomVox(CashImage3D vox = null)
	{
		UnitySingleton<GameController>.Instance.SetCustomData(vox);
		this.VoxLoad(vox.Vox);
		vox.Init(this.load3D.MV);

		Vector3 position = vox.Position.ToVector3();
		this.videoCamera.transform.position = position;
		this.saveCamera.transform.position = position; 
		Quaternion rotation = vox.Rotation.ToQuaternion();
		this.videoCamera.transform.rotation = rotation;
		this.saveCamera.transform.rotation = rotation;
		Camera component = this.saveCamera.GetComponent<Camera>(); 
		((Component)this.videoCamera).GetComponent<Camera>().orthographicSize = vox.CamSize / 2f;
		component.orthographicSize = vox.CamSize / 2f;
		Camera.main.orthographicSize = vox.CamSize / 1.5f;
		Camera.main.transform.position = Vector3.zero;
		Camera.main.transform.rotation = vox.Rotation.ToQuaternion();
		float z = Mathf.InverseLerp(15f, 5f, Camera.main.orthographicSize);
		UnitySingleton<GameController>.Instance.ChangeCamDepthPosition(z);
	}

	private void OnFailLoad()
	{
		UnityEngine.Debug.LogError("[MVVoxModel] Invalid load data from StreamingAssets");
	}

	private void OnDestroy()
	{
	}
	
	private IEnumerator StopInitAndShowInterCoroutine()
	{
		if ((INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.None || INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.RewardedNo_ContentMedium_Old || INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.RewardedNo_ContentMedium_Revealed) && (this.m_imageInfo == null || this.m_imageInfo.CustomAccessStatus != 0) && !IAPWrapper.Instance.Subscribed && !AppData.UnlockedImages.Contains(this.m_imageInfo.Id))
		{
			this.m_stopInit = false;
			yield break;
		}
		if (!IAPWrapper.Instance.NoAds && AppData.TutorialCompleted)
		{
			yield return new WaitForSeconds(0.1f);
			if (AdsWrapper.Instance.ShowInter("preview"))
			{
				yield return new WaitForSeconds(1f); 
				this.m_stopInit = false;
				yield break;
			}
		}
		this.m_stopInit = false;
	}
	private IEnumerator InitCoroutine(ImageInfo info, CashImage3D vox, SavedWorkData3D savedWorkData, ImageOpenType imageOpenType)
	{  
		this.m_imageInfo = info;
		yield return null;

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
			bool video = false;
			if ((INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.None || INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.RewardedNo_ContentMedium_Old || INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.RewardedNo_ContentMedium_Revealed) && info.CustomAccessStatus != 0 && !IAPWrapper.Instance.Subscribed && !AppData.UnlockedImages.Contains(info.Id))
			{
				this.m_videoWaiter.SetActive(true); 
				AppData.UnlockedImages.Add(info.Id);
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

					timer -= 1f;
					if (video)
					{
						break;
					} 
				}
			}
			this.m_videoWaiter.SetActive(false);
			WorkbookModel.Init();
			UnitySingleton<ProgressManager>.Instance.Init(info, saveCamera, savedWorkData);
			this.ClearVoxMeshes();
			UnitySingleton<GameController>.Instance.VoxLoadComplete += new Action<List<Color>>(this.VoxLoadCompleteHandler);
			UnitySingleton<GameController>.Instance.VideoCamera = this.videoCamera;
			this.LoadCustomVox(vox);
			AnalyticsManager.Instance.ImageOpened(info, (int)vox.CubesCount, vox.ColorsCount.Count, imageOpenType);
			yield break;
		}
	}
	private IEnumerator OpenUICoroutine(List<Color> colors)
	{
		yield return null;
		var workbook = WindowManager.Instance.OpenWorkbook3D();
		workbook.Init(colors);
		yield return null;
		 
		UnityEngine.Object.FindObjectOfType<CameraManager3D>().Init();
		if (!AppData.TutorialCompleted)
		{
			TutorialWindow tutorialWindow = WindowManager.Instance.OpenTutorial3D();
			if (tutorialWindow != null)
			{
				tutorialWindow.Init("picture", "3D");
				this.gameObject.SetActive(false);
				TutorialWindow tutorialWindow2 = tutorialWindow;
				tutorialWindow2.OnStartClose = (Action)Delegate.Combine(tutorialWindow2.OnStartClose, (Action)delegate
				{
					if ((UnityEngine.Object)this.gameObject != (UnityEngine.Object)null)
					{
						this.gameObject.SetActive(true);
					}
				});
			}
		}
	}
}
