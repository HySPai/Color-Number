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
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MyPhotosWindow : BaseWindow
{
	private List<PhotoInfo> m_photoInfos;

	private List<GroupOfMyPhotos> m_imageGroups;

	private int m_currentGroupIndex = -1;

	private bool m_inited;

	private bool m_lock;

	private bool m_needReinit;

	private float m_delta = -1f;

	private float m_previewSize = -1f;

	private int m_pairsCount = -1;

	[SerializeField]
	private ScrollRect m_scrollRect;

	[SerializeField]
	private GroupOfMyPhotos m_groupOfMyPhotosPrefab;

	[SerializeField]
	private RectTransform m_groupOfPreviewsParent;

	[SerializeField]
	private BigPreview m_bigPreview;

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
			return "mainMenu_createTab";
		}
	}

	private MyPhotosWindow()
	{
		this.m_imageGroups = new List<GroupOfMyPhotos>();
	}

	public void Init()
	{
		this.m_currentGroupIndex = -1;
		if (!this.m_inited)
		{
			this.m_needReinit = true;
			this.m_inited = true;
		}
		else
		{
			this.OnSelectIndexHandler(0, false);
		}
	}

	private void Reinit()
	{
		this.m_photoInfos = new List<PhotoInfo>();
		this.m_photoInfos.AddRange(MainManager.Instance.SavedWorksList.GetPhotos());
		this.m_photoInfos.Insert(0, new PhotoInfo(string.Empty, PhotoSource.Unknown));
		this.m_pairsCount = (this.m_photoInfos.Count + 1) / 2;
		this.m_delta = 20f;
		float num = ((RectTransform)base.transform).rect.width - this.m_delta * 2f;
		this.m_previewSize = (num - this.m_delta) / 2f;
		this.m_scrollRect.content.sizeDelta = new Vector2(num, (float)this.m_pairsCount * (this.m_previewSize + this.m_delta) + this.m_delta);
		if (this.m_imageGroups.Count == 0)
		{
			for (int i = 0; i < 10; i++)
			{
				GroupOfMyPhotos groupOfMyPhotos = UnityEngine.Object.Instantiate(this.m_groupOfMyPhotosPrefab);
				groupOfMyPhotos.transform.SetParent(this.m_groupOfPreviewsParent);
				groupOfMyPhotos.transform.localPosition = Vector3.zero;
				groupOfMyPhotos.transform.localScale = Vector3.one;
				((RectTransform)groupOfMyPhotos.transform).sizeDelta = new Vector2(num, this.m_previewSize);
				groupOfMyPhotos.gameObject.SetActive(false);
				GroupOfMyPhotos groupOfMyPhotos2 = groupOfMyPhotos;
				groupOfMyPhotos2.OnImageClick = (Action<PhotoInfo, PhotoPreview, GroupOfMyPhotos>)Delegate.Combine(groupOfMyPhotos2.OnImageClick, new Action<PhotoInfo, PhotoPreview, GroupOfMyPhotos>(this.OnImageClickHandler));
				groupOfMyPhotos.Index = -1;
				this.m_imageGroups.Add(groupOfMyPhotos);
			}
		}
		this.OnSelectIndexHandler(0, true);
		this.m_scrollRect.normalizedPosition = new Vector2(0f, 1f);
	}

	private void Update()
	{
		if (this.m_needReinit)
		{
			this.Reinit();
			this.m_needReinit = false;
		}
	}

	public void OnScrollRectScrolled(Vector2 pos)
	{
		if (!this.m_lock && this.m_imageGroups.Count > 0)
		{
			Vector2 sizeDelta = this.m_scrollRect.content.sizeDelta;
			float num = sizeDelta.y * (1f - pos.y);
			int num2 = Mathf.RoundToInt((num - this.m_delta) / (this.m_previewSize + this.m_delta));
			if (num2 < this.m_pairsCount)
			{
				this.OnSelectIndexHandler(num2, false);
			}
			else
			{
				this.OnSelectIndexHandler(this.m_pairsCount - 1, false);
			}
		}
	}

	private void OnSelectIndexHandler(int index, bool force = false)
	{
		if (index >= 0 && index < this.m_pairsCount && !this.m_lock)
		{
			if (index != this.m_currentGroupIndex)
			{
				bool flag = index > this.m_currentGroupIndex;
				List<GroupOfMyPhotos> list = (from a in this.m_imageGroups
											  orderby a.Index
											  select a).ToList();
				this.m_currentGroupIndex = index;
				for (int i = -4; i < 6; i++)
				{
					int localIndex = this.m_currentGroupIndex + i;
					if (localIndex >= 0 && localIndex < this.m_pairsCount)
					{
						GroupOfMyPhotos groupOfMyPhotos = this.m_imageGroups.FirstOrDefault((GroupOfMyPhotos a) => a.Index == localIndex);
						if (groupOfMyPhotos == null)
						{
							groupOfMyPhotos = ((!flag) ? list[list.Count - 1] : list[0]);
							groupOfMyPhotos.Clear();
							groupOfMyPhotos.Index = localIndex;
							groupOfMyPhotos.AddPreview(this.m_photoInfos[localIndex * 2]);
							if (localIndex * 2 + 1 < this.m_photoInfos.Count)
							{
								groupOfMyPhotos.AddPreview(this.m_photoInfos[localIndex * 2 + 1]);
							}
							groupOfMyPhotos.LoadIcons();
							((RectTransform)groupOfMyPhotos.transform).anchoredPosition = new Vector2(0f, 0f - ((float)localIndex * (this.m_previewSize + this.m_delta) + this.m_delta));
							list.Remove(groupOfMyPhotos);
						}
						else
						{
							if (force)
							{
								groupOfMyPhotos.CheckPreview(0, this.m_photoInfos[localIndex * 2]);
								if (localIndex * 2 + 1 < this.m_photoInfos.Count)
								{
									groupOfMyPhotos.CheckPreview(1, this.m_photoInfos[localIndex * 2 + 1]);
								}
								else
								{
									groupOfMyPhotos.ClearPreview(1);
								}
								groupOfMyPhotos.LoadIcons();
							}
							list.Remove(groupOfMyPhotos);
						}
					}
					else if (localIndex == this.m_pairsCount)
					{
						GroupOfMyPhotos groupOfMyPhotos2 = this.m_imageGroups.FirstOrDefault((GroupOfMyPhotos a) => a.Index == localIndex);
						if (groupOfMyPhotos2 != null)
						{
							groupOfMyPhotos2.Clear();
						}
					}
				}
			}
		}
		else if (this.m_pairsCount == 0)
		{
			foreach (GroupOfMyPhotos imageGroup in this.m_imageGroups)
			{
				imageGroup.Clear();
			}
		}
	}

	public void OnImageClickHandler(PhotoInfo photoInfo, PhotoPreview preview, GroupOfMyPhotos groupOfMyPhotos)
	{
		DailyGame.ResetState(false);
		if (string.IsNullOrEmpty(photoInfo.Id))
		{
			if (AppData.FreePhotos > 0 || IAPWrapper.Instance.NoAds)
			{
				CreationWindow creationWindow = WindowManager.Instance.OpenCreationWindow();
				creationWindow.Init(delegate (PhotoInfo info)
				{
					this.NewImageButtonClick(info, ImageOpenType.New);
					AppData.FreePhotos--;
				});
			}
			else if (INPluginWrapper.Instance.GetAbTestGroup() == ABTestGroup.RewardedNo_ContentEasy)
			{
				if (AdsWrapper.Instance.IsVideoAvailable())
				{
					AdsWrapper.Instance.ShowVideo("photo", delegate (bool res)
					{
						if (res)
						{
							CreationWindow creationWindow3 = WindowManager.Instance.OpenCreationWindow();
							creationWindow3.Init(delegate (PhotoInfo info)
							{
								this.NewImageButtonClick(info, ImageOpenType.New);
							});
						}
					});
				}
			}
			else if (AdsWrapper.Instance.IsVideoAvailable())
			{
				IapPopup abWindow = WindowManager.Instance.OpenAbTestWindow();
				abWindow.Init(delegate (bool res)
				{
					if (res)
					{
						CreationWindow creationWindow2 = WindowManager.Instance.OpenCreationWindow();
						creationWindow2.Init(delegate (PhotoInfo info)
						{
							this.NewImageButtonClick(info, ImageOpenType.New);
							WindowManager.Instance.CloseMe(abWindow);
						});
					}
				}, ABTestGroup.RewardedYes_ContentHard, AbTestWindowMode.Photo);
			}
			else
			{
				ABTestGroup abTestGroup = INPluginWrapper.Instance.GetAbTestGroup();
				if (abTestGroup == ABTestGroup.RewardedNo_ContentMedium_Old || abTestGroup == ABTestGroup.None)
				{
					NewInappsWindow newInappsWindow = WindowManager.Instance.OpenInappsWindow();
					newInappsWindow.Init("photo", null);
				}
				else
				{
					var trialInappsWindow = WindowManager.Instance.OpenInappsWindow();
					trialInappsWindow.Init("photo", null);//, false, false);
				}
			}
		}
		else
		{
			this.m_currentGroupIndex = this.m_imageGroups.IndexOf(groupOfMyPhotos);
			string text = MainManager.Instance.SavedWorksList.LastSaveOfImageId(photoInfo.Id);
			ISavedWorkData swd = null;
			if (text != null)
			{
				swd = MainManager.Instance.SavedWorksList.LoadById(text);
			}
			this.SelectPreview(preview, swd);
			this.DrawButtonClick(photoInfo, swd);
		}
	}

	private void SelectPreview(PhotoPreview preview, ISavedWorkData swd)
	{
		this.m_bigPreview.Init((RectTransform)preview.transform, preview.Texture, false, swd);
	}

	public void DrawButtonClick(PhotoInfo photoInfo, ISavedWorkData swd)
	{
		AnalyticsManager.Instance.ImageSelected(new ImageInfo(photoInfo.Id, photoInfo.Source.ToString().ToLower()));
		if (!this.m_lock && this.m_currentGroupIndex >= 0)
		{
			this.m_lock = true;
			if (swd != null)
			{
				ActionSheetWrapper.ShowSavedWorkActionSheet(delegate (ActionSheetResult a)
				{
					switch (a)
					{
						case ActionSheetResult.Continue:
							this.LoadButtonClick(swd.Id);
							break;
						case ActionSheetResult.New:
							this.NewImageButtonClick(photoInfo, ImageOpenType.Reopened);
							break;
						case ActionSheetResult.Share:
							this.ShareButtonClick(swd.Id);
							break;
						case ActionSheetResult.Delete:
							this.DeleteButtonClick(photoInfo.Id);
							break;
						case ActionSheetResult.Cancel:
							this.m_bigPreview.Close();
							break;
					}
					this.m_lock = false;
				});
			}
			else
			{
				ActionSheetWrapper.ShowEmptyPhotoActionSheet(delegate (ActionSheetResult a)
				{
					switch (a)
					{
						case ActionSheetResult.New:
							this.NewImageButtonClick(photoInfo, ImageOpenType.New);
							break;
						case ActionSheetResult.Delete:
							this.DeleteButtonClick(photoInfo.Id);
							break;
						case ActionSheetResult.Cancel:
							this.m_bigPreview.Close();
							break;
					}
					this.m_lock = false;
				});
			}
		}
	}

	private void LoadButtonClick(string saveId)
	{
		this.m_lock = true;
		ISavedWorkData savedWorkData = MainManager.Instance.SavedWorksList.LoadById(saveId);
		MainMenu.LastPage = MainMenuPage.MyWorks;
		MainMenu.ImageId = savedWorkData.ImageInfo.Id;
		MainMenu.WorkId = savedWorkData.Id;
		MainManager.Instance.StartWorkbook(savedWorkData, ImageOpenType.Continued, null);
	}

	private void NewImageButtonClick(PhotoInfo photoInfo, ImageOpenType imageOpenType)
	{
		this.m_lock = true;
		MainManager.Instance.StartWorkbook(new ImageInfo(photoInfo.Id, photoInfo.Source.ToString().ToLower()), imageOpenType, MainMenuPage.Photos, null);
	}

	public void DeleteButtonClick(string photoId)
	{
		this.m_bigPreview.Close();
		AnalyticsManager.Instance.DeleteButtonClick();
		DialogToolWrapper.ShowDeleteDialog(delegate (bool res)
		{
			if (res)
			{
				AnalyticsManager.Instance.DeleteSuccess();
				MainManager.Instance.SavedWorksList.DeletePhoto(photoId);
				this.m_currentGroupIndex = -1;
				this.m_lock = false;
				this.Reinit();
			}
		});
	}

	public void ShareButtonClick(string saveId)
	{
		this.m_bigPreview.Close();
		ShareWindow shareWindow = WindowManager.Instance.OpenShareWindow();
		shareWindow.InitFromLibrary(saveId);
	}
}


