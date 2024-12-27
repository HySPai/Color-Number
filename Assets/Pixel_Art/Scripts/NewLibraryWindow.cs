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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewLibraryWindow : BaseWindow
{
	private ImagesInfo m_imagesInfo;

	private List<ImageInfo> m_images;

	private List<GroupOfPreviews> m_imageGroups;

	private int m_currentGroupIndex = -1;

	private bool m_lock;

	private bool m_needShowGroupIndex;

	private float m_delta = -1f;

	private float m_previewSize = -1f;

	private int m_pairsCount = -1;

	[SerializeField]
	private ScrollRect m_scrollRect;

	[SerializeField]
	private GroupOfPreviews m_groupOfPreviewsPrefab;

	[SerializeField]
	private RectTransform m_groupOfPreviewsParent;

	[SerializeField]
	private BigPreview m_bigPreview;

	[SerializeField]
	private GameObject m_blockPlane;

	public bool Inited
	{
		get
		{
			return this.m_imageGroups[0].Loaded;
		}
	}

	protected override string WindowName
	{
		get
		{
			return "mainMenu_libraryTab";
		}
	}

	private NewLibraryWindow()
	{
		this.m_imageGroups = new List<GroupOfPreviews>();
	}

	private static NewLibraryWindow instance;
	private void Awake()
	{
		NewLibraryWindow.instance = this;
		IAPWrapper instance = IAPWrapper.Instance;
		instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Combine(instance.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
		DataManager instance2 = DataManager.Instance;
		instance2.OnInternetAppeared = (Action)Delegate.Combine(instance2.OnInternetAppeared, new Action(this.OnInternetAppearedHandler));
	}
	private void Start()
	{ 
		MainMenu.Instance.RefreshTodayArt();
	}
	public void Init(ImagesInfo imagesInfo)
	{
        Debug.Log("New library Images count = " + imagesInfo.Count);
		if (this.m_imagesInfo != null && this.m_imagesInfo.Images.Count == imagesInfo.Images.Count)
		{
			return;
		}
        Debug.Log("New library Create categories");
		this.m_imagesInfo = imagesInfo;
		this.m_currentGroupIndex = -1;
		this.m_images = this.m_imagesInfo.Images;
		this.m_pairsCount = (this.m_images.Count + 1) / 2;
		this.m_delta = 20f;
		float size = ((RectTransform)base.transform).rect.width - this.m_delta * 2f;
		this.m_previewSize = (size - this.m_delta) / 2f;
		this.m_scrollRect.content.sizeDelta = new Vector2(size, (float)this.m_pairsCount * (this.m_previewSize + this.m_delta) + this.m_delta);
		if (this.m_imageGroups.Count == 0)
		{
			for (int i = 0; i < 10; i++)
			{
				GroupOfPreviews groupOfPreviews = UnityEngine.Object.Instantiate(this.m_groupOfPreviewsPrefab);
				groupOfPreviews.transform.SetParent(this.m_groupOfPreviewsParent);
				groupOfPreviews.transform.localPosition = Vector3.zero;
				groupOfPreviews.transform.localScale = Vector3.one;
				((RectTransform)groupOfPreviews.transform).sizeDelta = new Vector2(size, this.m_previewSize);
				groupOfPreviews.gameObject.SetActive(false);
				groupOfPreviews.OnImageClick = (Action<ImageInfo, ImagePreview, GroupOfPreviews>)Delegate.Combine(groupOfPreviews.OnImageClick, new Action<ImageInfo, ImagePreview, GroupOfPreviews>(this.OnImageClickHandler));
				groupOfPreviews.Index = -1;
				this.m_imageGroups.Add(groupOfPreviews);
			}
		}
		int index = this.m_images.FindIndex((ImageInfo a) => a.Id == MainMenu.ImageId) / 2;
		if (index < 0)
		{
			index = 0;
		}
		this.ShowGroupIndex(index, true);

	}

	public static void UpdateDailyClick(bool hasClick)
	{
		if (instance != null)
		{
			MainMenu.Instance.todayArt.OnClick = null;
			if (hasClick)
				MainMenu.Instance.todayArt.OnClick = instance.OpenImage;
		}
	}

	private void Update()
	{
		GameObject blockPlane = this.m_blockPlane;
		Vector2 velocity = this.m_scrollRect.velocity;
		int active;
		if (!(velocity.y > 500f))
		{
			Vector2 velocity2 = this.m_scrollRect.velocity;
			active = ((velocity2.y < -500f) ? 1 : 0);
		}
		else
		{
			active = 1;
		}
		blockPlane.SetActive((byte)active != 0);
		if (this.m_needShowGroupIndex)
		{
			this.ShowGroupIndex(this.m_currentGroupIndex, true);
			this.m_needShowGroupIndex = false;
		}
	}

	public void ShowGroupIndex(int index, bool force = false)
	{
		if (base.isActiveAndEnabled)
		{
			base.StartCoroutine(this.ShowGroupIndexCoroutine(index, force));
		}
		else
		{
			this.m_needShowGroupIndex = true;
		}
	}

	public void OnScrollRectScrolled(Vector2 pos)
	{
        if (!this.m_lock && this.m_imageGroups.Count > 0)
		{
			Vector2 sizeDelta = ((Component)this.m_imageGroups[0]).GetComponent<RectTransform>().sizeDelta;
			float y = sizeDelta.y;
			int num = 20;
			Vector2 sizeDelta2 = this.m_scrollRect.content.sizeDelta;
			float num2 = sizeDelta2.y * (1f - pos.y);
			int index = Mathf.RoundToInt((num2 - (float)num) / (y + (float)num));
			this.OnSelectIndexHandler(index, false);
		}
	}

	private void OnImageClickHandler(ImageInfo imageInfo, ImagePreview imagePreview, GroupOfPreviews groupOfPreviews)
	{
		this.m_currentGroupIndex = this.m_imageGroups.IndexOf(groupOfPreviews);
		OpenImage(imageInfo, imagePreview);
	}

	public void OpenImage(ImageInfo imageInfo, ImagePreview imagePreview)
	{
		DailyGame.ResetState(false);

		AnalyticsManager.Instance.ImageSelected(imageInfo);
		
		this.DrawAction(imageInfo, imagePreview);
	}

	private void SelectPreview(string saveId, ImageInfo imageInfo, ImagePreview preview)
	{
		ISavedWorkData swd = null;
		if (saveId != null)
		{
			swd = MainManager.Instance.SavedWorksList.LoadById(saveId);
		}
		this.m_bigPreview.Init((RectTransform)preview.transform, preview.Texture, preview.Locked, swd);
	}
	public void ExecutePuzzle()
	{
		string strIdent = GetNameScript.GetDailyIdentity(0);
		if (LevelProgressControl.control != null)
		{
			LevelProgressControl.control.puzzleSrcType = LevelProgressControl.PuzzleSourceType.PUZZLE_DAILY;
			LevelProgressControl.control.postcardOrigin = SceneManager.GetActiveScene().name;
			LevelProgressControl.control.puzzleBasename = strIdent;
		}
		PlayerPrefs.SetString("datafile", strIdent); 
	}
	public static void ExecutePuzzle(ImageInfo imageInfo)
	{
		if (instance != null)
		{
			DailyGame.ResetState(true);
			AnalyticsManager.Instance.ImageSelected(imageInfo);
			instance.DrawAction(imageInfo, null);
		}
	}
	public void DrawAction(ImageInfo imageInfo, ImagePreview preview)
	{
		if (!InternetConnection.IsAvailable && !DataManager.Instance.CheckLocalAsset(imageInfo))
		{
			DialogToolWrapper.ShowNoInternetDialog();
		}
		else if (!this.m_lock)
		{
			this.m_lock = true;
			string text = MainManager.Instance.SavedWorksList.LastSaveOfImageId(imageInfo.Id);

			if (preview != null)
			{
				DailyGame.ResetState(preview.TodayArt);
				if (preview.TodayArt)
				{
					ExecutePuzzle();
				}
			}

			if (text == null)
			{ 
				this.NewImageButtonClick(imageInfo, ImageOpenType.New, preview);
			}
			else
			{
				if (preview != null)
				{
					this.SelectPreview(text, imageInfo, preview);
				}
				ActionSheetWrapper.ShowImagePreviewActionSheet(delegate (ActionSheetResult a)
				{
					switch (a)
					{
						case ActionSheetResult.Continue:
							this.LoadButtonClick(imageInfo);
							this.m_lock = false;
							break;
						case ActionSheetResult.New:
							this.NewImageButtonClick(imageInfo, ImageOpenType.Reopened, preview);
							this.m_lock = false;
							break;
						case ActionSheetResult.Cancel:
							this.m_bigPreview.Close();
							this.m_lock = false;
							break;
					}
				});

			}
		}
	}

	public void NewImageButtonClick(ImageInfo imageInfo, ImageOpenType imageOpenType, ImagePreview imagePreview)
	{
		if ((imageInfo.CustomAccessStatus == AccessStatus.Free && imagePreview != null && !imagePreview.AdsAvailable) || IAPWrapper.Instance.Subscribed || AppData.UnlockedImages.Contains(imageInfo.Id))
		{
			base.StartCoroutine(this.NewImageButtonClickCoroutine(imageInfo, imageOpenType));
		}
		else
		{
			if (INPluginWrapper.Instance.GetAbTestGroup() != ABTestGroup.None && INPluginWrapper.Instance.GetAbTestGroup() != ABTestGroup.RewardedNo_ContentMedium_Old && INPluginWrapper.Instance.GetAbTestGroup() != ABTestGroup.RewardedNo_ContentMedium_Revealed)
			{
				switch (INPluginWrapper.Instance.GetAbTestGroup())
				{
					case ABTestGroup.None:
					case ABTestGroup.RewardedNo_ContentMedium_Old:
					case ABTestGroup.RewardedNo_ContentMedium_Revealed:
						{
							NewInappsWindow newInappsWindow2 = WindowManager.Instance.OpenInappsWindow();
							newInappsWindow2.Init("image", delegate
							{
								this.m_bigPreview.Close();
							});
							break;
						}
					case ABTestGroup.RewardedNo_ContentMedium:
					case ABTestGroup.RewardedNo_ContentHard:
						{
							var trialInappsWindow2 = WindowManager.Instance.OpenInappsWindow();
							trialInappsWindow2.Init("image", delegate
							{
								this.m_bigPreview.Close();
							});//, false, false);
							break;
						}
					default:
						if ((imagePreview != null && imagePreview.AdsAvailable && !imagePreview.IsFreePremium))
						{
							AdsWrapper.Instance.ShowVideo("image", delegate (bool res)
							{
								if (res)
								{
									base.StartCoroutine(this.NewImageButtonClickCoroutine(imageInfo, imageOpenType));
								}
							});
						}
						else if (imagePreview != null || imageInfo.AccessStatus == AccessStatus.Premium) //if (AdsWrapper.Instance.IsVideoAvailable())
						{
							IapPopup abTestWindow = WindowManager.Instance.OpenAbTestWindow();
							abTestWindow.Init(imageInfo, imagePreview);
						}
						else
						{
							base.StartCoroutine(this.NewImageButtonClickCoroutine(imageInfo, imageOpenType));
						}
						//else
						//{
						//    switch (INPluginWrapper.Instance.GetAbTestGroup())
						//    {
						//        case ABTestGroup.None:
						//        case ABTestGroup.RewardedNo_ContentMedium_Old:
						//        case ABTestGroup.RewardedNo_ContentMedium_Revealed:
						//        {
						//            NewInappsWindow newInappsWindow = WindowManager.Instance.OpenInappsWindow();
						//            newInappsWindow.Init("image", null);
						//            break;
						//        }
						//        default:
						//        {
						//            TrialInappsWindow trialInappsWindow = WindowManager.Instance.OpenTrialInappsWindow();
						//            trialInappsWindow.Init("image", null, false, false);
						//            break;
						//        }
						//    }
						//}
						this.m_bigPreview.Close();
						break;
				}
			}
			else
			{
				base.StartCoroutine(this.NewImageButtonClickCoroutine(imageInfo, imageOpenType));
			}
			this.m_lock = false;
		}
	}
	public void LoadButtonClick(ImageInfo imageInfo)
	{
		this.m_lock = true;
		this.ClearPreviews();
		string id = MainManager.Instance.SavedWorksList.LastSaveOfImageId(imageInfo.Id);
		ISavedWorkData savedWorkData = MainManager.Instance.SavedWorksList.Load(SavedWorksList.GetPathToSave(id));
		MainMenu.LastPage = MainMenuPage.Library;
		MainMenu.ImageId = savedWorkData.ImageInfo.Id;
		MainMenu.WorkId = savedWorkData.Id;
		MainManager.Instance.StartWorkbook(savedWorkData, ImageOpenType.Continued, delegate
		{
			this.m_bigPreview.Close();
			this.m_lock = false;
		});
	}

	private void ClearPreviews()
	{
	}

	private void OnSelectIndexHandler(int index, bool force = false)
	{
		try
		{
            if (index >= 0 && index < this.m_pairsCount && !this.m_lock || force)
			{
                if (index != this.m_currentGroupIndex || force)
                {
                    bool flag = index > this.m_currentGroupIndex;
                    List<GroupOfPreviews> list = (from a in this.m_imageGroups
                                                  orderby a.Index
                                                  select a).ToList();
                    this.m_currentGroupIndex = index;
                    for (int i = -4; i < 6; i++)
                    {
                        int localIndex = this.m_currentGroupIndex + i;
                        if (localIndex >= 0 && localIndex < this.m_pairsCount)
                        {
                            GroupOfPreviews groupOfPreviews = this.m_imageGroups.FirstOrDefault((GroupOfPreviews a) => a.Index == localIndex);
                            if (groupOfPreviews == null)
                            {
                                groupOfPreviews = ((!flag) ? list[this.m_imageGroups.Count - 1] : list[0]);
                                groupOfPreviews.Index = localIndex;
                                groupOfPreviews.Clear();
                                groupOfPreviews.AddPreview(this.m_images[localIndex * 2]);
                                if (localIndex * 2 + 1 < this.m_images.Count)
                                {
                                    groupOfPreviews.AddPreview(this.m_images[localIndex * 2 + 1]);
                                }
                                groupOfPreviews.LoadIcons();
                                ((RectTransform)groupOfPreviews.transform).anchoredPosition = new Vector2(0f, 0f - ((float)localIndex * (this.m_previewSize + this.m_delta) + this.m_delta));
                                list.Remove(groupOfPreviews);
                            }
                            else if (force)
                            {
                                groupOfPreviews.Index = localIndex;
                                groupOfPreviews.Clear();
                                groupOfPreviews.AddPreview(this.m_images[localIndex * 2]);
                                if (localIndex * 2 + 1 < this.m_images.Count)
                                {
                                    groupOfPreviews.AddPreview(this.m_images[localIndex * 2 + 1]);
                                }
                                groupOfPreviews.LoadIcons();
                            }
                        }
                    }
                }
			} 
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	private void ActivateWindowElement(MonoBehaviour element)
	{
		((GroupOfPreviews)element).LoadIcons();
	}

	private void DesactivateWindowElement(MonoBehaviour element)
	{
		((GroupOfPreviews)element).UnloadIcons();
	}

	private void OnPurchaseHandler(bool res, SubscriptionType subscription)
	{
		foreach (GroupOfPreviews imageGroup in this.m_imageGroups)
		{
			imageGroup.Reinit();
		}
	}

	private void OnInternetAppearedHandler()
	{
		this.OnSelectIndexHandler(this.m_currentGroupIndex, true);
	}

	private void OnDestroy()
	{
		IAPWrapper instance = IAPWrapper.Instance;
		instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Remove(instance.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
		DataManager instance2 = DataManager.Instance;
		instance2.OnInternetAppeared = (Action)Delegate.Remove(instance2.OnInternetAppeared, new Action(this.OnInternetAppearedHandler));
	}
	private IEnumerator ShowGroupIndexCoroutine(int index, bool force = false)
	{
		yield return null;
		 
		var y = (float)index * (this.m_previewSize + this.m_delta);
		var p = this.m_scrollRect.content.anchoredPosition;
		p.y = y;
		this.m_scrollRect.content.anchoredPosition = p;
		this.OnSelectIndexHandler(index, force);
	}
	private IEnumerator NewImageButtonClickCoroutine(ImageInfo imageInfo, ImageOpenType imageOpenType)
	{ 
		this.m_lock = true; 
		this.ClearPreviews();
		MainManager.Instance.StartWorkbook(imageInfo, imageOpenType, MainMenuPage.Library, delegate
		{
			this.m_bigPreview.Close();
			this.m_lock = false;
		});
		yield return null;
	}
}

