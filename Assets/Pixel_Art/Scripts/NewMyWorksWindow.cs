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

public class NewMyWorksWindow : BaseWindow
{
	private List<string> m_saveIds;

	private List<GroupOfMyWorks> m_imageGroups;

	private int m_currentGroupIndex = -1;

	private bool m_inited;

	private bool m_lock;

	private bool m_needReinit;

	private int m_needSelectIndex = -1;

	private float m_delta = -1f;

	private float m_previewSize = -1f;

	private int m_pairsCount = -1;

	[SerializeField]
	private ScrollRect m_scrollRect;

	[SerializeField]
	private GroupOfMyWorks m_groupOfMyWorksPrefab;

	[SerializeField]
	private RectTransform m_groupOfPreviewsParent;

	[SerializeField]
	private GameObject m_noWorksBlock;

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
			return "mainMenu_worksTab";
		}
	}

	private NewMyWorksWindow()
	{
		this.m_imageGroups = new List<GroupOfMyWorks>();
	}

	public void Init()
	{
		this.m_currentGroupIndex = -1;
		if (!this.m_inited)
		{
			this.m_needReinit = true;
			this.m_inited = true;
		}
		else if (this.m_saveIds.Count > 0)
		{
			this.m_needSelectIndex = 0;
		}
	}

	private void Reinit()
	{
		List<string> saves = MainManager.Instance.SavedWorksList.GetSaves();
		this.m_saveIds = new List<string>();
		this.m_saveIds.AddRange(saves.Distinct());
		this.m_pairsCount = (this.m_saveIds.Count + 1) / 2;
		this.m_delta = 20f;
		float num = ((RectTransform)base.transform).rect.width - this.m_delta * 2f;
		this.m_previewSize = (num - this.m_delta) / 2f;
		this.m_scrollRect.content.sizeDelta = new Vector2(num, (float)this.m_pairsCount * (this.m_previewSize + this.m_delta) + this.m_delta);
		if (this.m_imageGroups.Count == 0)
		{
			for (int i = 0; i < 10; i++)
			{
				GroupOfMyWorks groupOfMyWorks = UnityEngine.Object.Instantiate(this.m_groupOfMyWorksPrefab);
				groupOfMyWorks.transform.SetParent(this.m_groupOfPreviewsParent);
				groupOfMyWorks.transform.localPosition = Vector3.zero;
				groupOfMyWorks.transform.localScale = Vector3.one;
				((RectTransform)groupOfMyWorks.transform).sizeDelta = new Vector2(num, this.m_previewSize);
				groupOfMyWorks.gameObject.SetActive(false);
				GroupOfMyWorks groupOfMyWorks2 = groupOfMyWorks;
				groupOfMyWorks2.OnImageClick = (Action<string, MyWorkPreview, GroupOfMyWorks>)Delegate.Combine(groupOfMyWorks2.OnImageClick, new Action<string, MyWorkPreview, GroupOfMyWorks>(this.OnImageClickHandler));
				groupOfMyWorks.Index = -1;
				this.m_imageGroups.Add(groupOfMyWorks);
			}
		}
		this.OnSelectIndexHandler(0, true);
		this.m_scrollRect.normalizedPosition = new Vector2(0f, 1f);
		this.m_noWorksBlock.SetActive(this.m_saveIds.Count == 0);
	}

	private void Update()
	{
		if (this.m_needReinit)
		{
			this.Reinit();
			this.m_needReinit = false;
		}
		if (this.m_needSelectIndex >= 0)
		{
			this.OnSelectIndexHandler(this.m_needSelectIndex, true);
			this.m_needSelectIndex = -1;
		}
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
				List<GroupOfMyWorks> list = (from a in this.m_imageGroups
											 orderby a.Index
											 select a).ToList();
				this.m_currentGroupIndex = index;
				for (int i = -4; i < 6; i++)
				{
					int localIndex = this.m_currentGroupIndex + i;
					if (localIndex >= 0 && localIndex < this.m_pairsCount)
					{
						GroupOfMyWorks groupOfMyWorks = this.m_imageGroups.FirstOrDefault((GroupOfMyWorks a) => a.Index == localIndex);
						if (groupOfMyWorks == null)
						{
							groupOfMyWorks = ((!flag) ? list[list.Count - 1] : list[0]);
							groupOfMyWorks.Clear();
							groupOfMyWorks.Index = localIndex;
							groupOfMyWorks.AddPreview(this.m_saveIds[localIndex * 2]);
							if (localIndex * 2 + 1 < this.m_saveIds.Count)
							{
								groupOfMyWorks.AddPreview(this.m_saveIds[localIndex * 2 + 1]);
							}
							groupOfMyWorks.LoadIcons();
							((RectTransform)groupOfMyWorks.transform).anchoredPosition = new Vector2(0f, 0f - ((float)localIndex * (this.m_previewSize + this.m_delta) + this.m_delta));
							list.Remove(groupOfMyWorks);
						}
						else
						{
							if (force)
							{
								groupOfMyWorks.CheckPreview(0, this.m_saveIds[localIndex * 2]);
								if (localIndex * 2 + 1 < this.m_saveIds.Count)
								{
									groupOfMyWorks.CheckPreview(1, this.m_saveIds[localIndex * 2 + 1]);
								}
								else
								{
									groupOfMyWorks.ClearPreview(1);
								}
								groupOfMyWorks.LoadIcons();
							}
							list.Remove(groupOfMyWorks);
						}
					}
					else if (localIndex == this.m_pairsCount)
					{
						GroupOfMyWorks groupOfMyWorks2 = this.m_imageGroups.FirstOrDefault((GroupOfMyWorks a) => a.Index == localIndex);
						if (groupOfMyWorks2 != null)
						{
							groupOfMyWorks2.Clear();
						}
					}
				}
			}
		}
		else if (this.m_pairsCount == 0)
		{
			foreach (GroupOfMyWorks imageGroup in this.m_imageGroups)
			{
				imageGroup.Clear();
			}
		}
	}

	public void OnImageClickHandler(string saveId, MyWorkPreview preview, GroupOfMyWorks groupOfMyWorks)
	{
		DailyGame.ResetState(false);
		this.m_currentGroupIndex = this.m_imageGroups.IndexOf(groupOfMyWorks);
		this.SelectPreview(saveId, preview);
		this.DrawButtonClick(saveId);
	}

	private void SelectPreview(string saveId, MyWorkPreview preview)
	{
		ISavedWorkData savedWorkData = MainManager.Instance.SavedWorksList.LoadById(saveId);
		ImageInfo imageInfo = savedWorkData.ImageInfo;
		AnalyticsManager.Instance.ImageSelected(imageInfo);
		this.m_bigPreview.Init((RectTransform)preview.transform, preview.Texture, false, savedWorkData);
	}

	public void DrawButtonClick(string saveId)
	{
		if (!this.m_lock && this.m_currentGroupIndex >= 0)
		{
			this.m_lock = true;
			ActionSheetWrapper.ShowSavedWorkActionSheet(delegate (ActionSheetResult a)
			{
				switch (a)
				{
					case ActionSheetResult.Continue:
						this.LoadButtonClick(saveId);
						break;
					case ActionSheetResult.New:
						this.NewImageButtonClick(saveId);
						break;
					case ActionSheetResult.Delete:
						this.DeleteButtonClick(saveId);
						break;
					case ActionSheetResult.Cancel:
						this.m_bigPreview.Close();
						break;
				}
				this.m_lock = false;
			});
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

	private void NewImageButtonClick(string saveId)
	{
		ISavedWorkData savedWorkData = MainManager.Instance.SavedWorksList.LoadById(saveId);
		ImageInfo imageInfo = DataManager.Instance.GetImageInfo(savedWorkData.ImageInfo.Id);
		if (imageInfo == null)
		{
			imageInfo = savedWorkData.ImageInfo;
		}
		if (imageInfo.CustomAccessStatus == AccessStatus.Free || IAPWrapper.Instance.Subscribed)
		{
			this.m_lock = true;
			ISavedWorkData savedWorkData2 = MainManager.Instance.SavedWorksList.LoadById(saveId);
			MainManager.Instance.StartWorkbook(savedWorkData2.ImageInfo, ImageOpenType.Reopened, MainMenuPage.MyWorks, null);
		}
		else
		{
			this.m_lock = true;
			ISavedWorkData savedWorkData3 = MainManager.Instance.SavedWorksList.LoadById(saveId);
			MainManager.Instance.StartWorkbook(savedWorkData3.ImageInfo, ImageOpenType.Reopened, MainMenuPage.MyWorks, null);
		}
	}

	public void DeleteButtonClick(string saveId)
	{
		this.m_bigPreview.Close();
		AnalyticsManager.Instance.DeleteButtonClick();
		DialogToolWrapper.ShowDeleteDialog(delegate (bool res)
		{
			if (res)
			{
				AnalyticsManager.Instance.DeleteSuccess();
				MainManager.Instance.SavedWorksList.Delete(saveId);
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

	public bool GetStateBigmPreview()
	{
		return this.m_bigPreview.isActiveAndEnabled;
	}
}


