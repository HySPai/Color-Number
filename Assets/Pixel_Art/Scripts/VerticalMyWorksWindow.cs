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
using UnityEngine.UI;

public class VerticalMyWorksWindow : MonoBehaviour
{
	private bool m_inited;

	private List<MyWorkPreview> m_imageElements;

	private List<string> m_saveIds;

	private int m_currentImageIndex;

	private bool m_lock;

	[SerializeField]
	private ScrollRect m_scrollRect;

	[SerializeField]
	private MyWorkPreview m_myWorkPreviewPrefab;

	[SerializeField]
	private RectTransform m_imagePreviewElementsParent;

	[SerializeField]
	private ShareWindow m_shareWindow;

	[SerializeField]
	private GameObject m_noWorksBlock;

	public bool ShareWindowShowed
	{
		get
		{
			return this.m_shareWindow.isActiveAndEnabled;
		}
	}

	private VerticalMyWorksWindow()
	{
		this.m_imageElements = new List<MyWorkPreview>();
		this.m_saveIds = new List<string>();
	}

	public void Init()
	{
		this.m_currentImageIndex = -1;
		if (!this.m_inited)
		{
			this.Reinit();
			this.m_inited = true;
		}
		else if (this.m_saveIds.Count > 0)
		{
			this.OnSelectIndexHandler(0);
		}
	}

	private void Reinit()
	{
		this.m_saveIds = new List<string>();
		int num = 0;
		List<string> saves = MainManager.Instance.SavedWorksList.GetSaves();
		for (num = 0; num < saves.Count; num++)
		{
			string text = saves[num];
			this.m_saveIds.Add(text);
			if (num >= this.m_imageElements.Count)
			{
				MyWorkPreview myWorkPreview = UnityEngine.Object.Instantiate(this.m_myWorkPreviewPrefab);
				myWorkPreview.transform.SetParent(this.m_imagePreviewElementsParent);
				myWorkPreview.transform.localPosition = Vector3.zero;
				myWorkPreview.transform.localScale = Vector3.one;
				this.m_imageElements.Add(myWorkPreview);
			}
			this.m_imageElements[num].Init(text);
			this.m_imageElements[num].gameObject.SetActive(true);
		}
		for (; num < this.m_imageElements.Count; num++)
		{
			this.m_imageElements[num].gameObject.SetActive(false);
		}
		this.m_noWorksBlock.SetActive(saves.Count == 0);
		this.SetId(0);
		this.OnSelectIndexHandler(0);
	}

	public void InitAfterSetActive()
	{
		base.StartCoroutine(this.NormalizePosition());
	}
	public void ShowId(string id)
	{
		if (!string.IsNullOrEmpty(id))
		{
			base.StartCoroutine(this.ShowIdCoroutine(id));
		}
	}

	public void ImageClick(string saveId)
	{
		this.m_currentImageIndex = this.m_saveIds.IndexOf(saveId);
		this.SetId(this.m_currentImageIndex);
		this.DrawButtonClick();
	}

	public void OnScrollRectScrolled(Vector2 pos)
	{
		if (!this.m_lock && this.m_imageElements.Count > 0)
		{
			Vector2 sizeDelta = ((Component)this.m_imageElements[0]).GetComponent<RectTransform>().sizeDelta;
			float y = sizeDelta.y;
			float spacing = ((Component)this.m_scrollRect.content).GetComponent<VerticalLayoutGroup>().spacing;
			Vector2 sizeDelta2 = this.m_scrollRect.content.sizeDelta;
			float num = sizeDelta2.y * (1f - pos.y);
			int index = Mathf.RoundToInt((num - spacing) / (y + spacing));
			this.OnSelectIndexHandler(index);
		}
	}

	private void SetId(int index)
	{
		if (this.m_imageElements.Count > 0)
		{
			Vector2 sizeDelta = ((Component)this.m_imageElements[0]).GetComponent<RectTransform>().sizeDelta;
			float y = sizeDelta.y;
			float spacing = ((Component)this.m_scrollRect.content).GetComponent<VerticalLayoutGroup>().spacing;
			float y2 = (float)index * (y + spacing);
			Vector2 anchoredPosition = this.m_scrollRect.content.anchoredPosition;
			anchoredPosition.y = y2;
			this.m_scrollRect.content.anchoredPosition = anchoredPosition;
		}
	}

	public void DrawButtonClick()
	{
		if (!this.m_lock && this.m_currentImageIndex >= 0)
		{
			this.m_lock = true;
			ActionSheetWrapper.ShowSavedWorkActionSheet(delegate (ActionSheetResult a)
			{
				switch (a)
				{
					case ActionSheetResult.Continue:
						this.LoadButtonClick();
						break;
					case ActionSheetResult.New:
						this.NewImageButtonClick();
						break;
					case ActionSheetResult.Share:
						this.ShareButtonClick();
						break;
					case ActionSheetResult.Delete:
						this.DeleteButtonClick();
						break;
				}
				this.m_lock = false;
			});
		}
	}

	private void LoadButtonClick()
	{
		this.m_lock = true;
		this.ClearPreviews();
	}

	private void NewImageButtonClick()
	{
		ISavedWorkData savedWorkData = MainManager.Instance.SavedWorksList.LoadById(this.m_saveIds[this.m_currentImageIndex]);
		ImageInfo imageInfo = DataManager.Instance.GetImageInfo(savedWorkData.ImageInfo.Id);
		if (imageInfo == null)
		{
			imageInfo = savedWorkData.ImageInfo;
		}
		if (imageInfo.CustomAccessStatus == AccessStatus.Free || IAPWrapper.Instance.Subscribed)
		{
			this.m_lock = true;
			this.ClearPreviews();
			ISavedWorkData savedWorkData2 = MainManager.Instance.SavedWorksList.LoadById(this.m_saveIds[this.m_currentImageIndex]);
			MainManager.Instance.StartWorkbook(savedWorkData2.ImageInfo, ImageOpenType.Reopened, MainMenuPage.MyWorks, null);
		}
		else
		{
			NewInappsWindow newInappsWindow = WindowManager.Instance.OpenInappsWindow();
			newInappsWindow.Init("image", null);
		}
	}

	public void DeleteButtonClick()
	{
		if (this.m_currentImageIndex >= 0)
		{
			DialogToolWrapper.ShowDeleteDialog(delegate (bool res)
			{
				if (res)
				{
					MainManager.Instance.SavedWorksList.Delete(this.m_saveIds[this.m_currentImageIndex]);
					this.m_currentImageIndex = -1;
					this.m_lock = false;
					this.Reinit();
				}
			});
		}
	}

	public void ShareButtonClick()
	{
		if (this.m_currentImageIndex >= 0)
		{
			this.m_shareWindow.InitFromLibrary(this.m_saveIds[this.m_currentImageIndex]);
		}
	}

	public void ShareWindowClose()
	{
		this.m_shareWindow.Close();
	}

	private void OnSelectIndexHandler(int index)
	{
		if (index >= 0 && index < this.m_imageElements.Count && !this.m_lock && index != this.m_currentImageIndex)
		{
			this.m_currentImageIndex = index;
			if (index >= 0)
			{
				this.m_imageElements[index].LoadIcon();
				if (index + 1 < this.m_imageElements.Count)
				{
					this.m_imageElements[index + 1].LoadIcon();
					if (index + 2 < this.m_imageElements.Count)
					{
						this.m_imageElements[index + 2].LoadIcon();
					}
				}
				if (index - 1 >= 0)
				{
					this.m_imageElements[index - 1].LoadIcon();
					if (index - 2 >= 0)
					{
						this.m_imageElements[index - 2].LoadIcon();
					}
				}
				if (index - 5 >= 0)
				{
					this.m_imageElements[index - 5].UnloadIcon();
				}
				if (index + 5 < this.m_imageElements.Count)
				{
					this.m_imageElements[index + 5].UnloadIcon();
				}
			}
		}
	}

	public void UnloadAll()
	{
		for (int i = 0; i < this.m_imageElements.Count; i++)
		{
			this.m_imageElements[i].UnloadIcon();
		}
	}

	private void ClearPreviews()
	{
		for (int i = 0; i < this.m_imageElements.Count; i++)
		{
			if (i != this.m_currentImageIndex)
			{
				this.m_imageElements[i].UnloadIcon();
			}
		}
	}

	public void OnDisable()
	{
		if (this.m_shareWindow.isActiveAndEnabled)
		{
			this.m_shareWindow.Close();
		}
	}
	private IEnumerator NormalizePosition()
	{
		yield return null;

		for (int i = 0; i < this.m_imageElements.Count; i++)
		{
			Vector3 localPosition = ((RectTransform)this.m_imageElements[i].transform).localPosition;
			localPosition.z = -0.1f;
			((RectTransform)this.m_imageElements[i].transform).localPosition = localPosition;
		}
		if (this.m_scrollRect != null)
		{
			this.m_scrollRect.normalizedPosition = new Vector2(0f, 1f);
		}
		this.m_scrollRect.normalizedPosition = new Vector2(0f, 1f);
		this.OnScrollRectScrolled(this.m_scrollRect.normalizedPosition);
	}
	private IEnumerator ShowIdCoroutine(string id)
	{
		yield return null;
		this.SetId(this.m_saveIds.IndexOf(id));
	}
}
