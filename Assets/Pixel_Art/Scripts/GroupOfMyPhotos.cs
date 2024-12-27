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

public class GroupOfMyPhotos : MonoBehaviour
{
	public Action<PhotoInfo, PhotoPreview, GroupOfMyPhotos> OnImageClick;

	private int m_index = -1;

	[SerializeField]
	private List<PhotoPreview> m_previews;

	private int m_emptyIndex;

	private bool m_subscribed;

	public int Index
	{
		get
		{
			return this.m_index;
		}
		set
		{
			this.m_index = value;
			base.gameObject.SetActive(this.m_index >= 0);
		}
	}

	public bool Loaded
	{
		get
		{
			return this.m_previews.All((PhotoPreview a) => a.Loaded);
		}
	}

	public void AddPreview(PhotoInfo photoInfo)
	{
		if (!this.m_subscribed)
		{
			this.Subscribe();
		}
		this.m_previews[this.m_emptyIndex].Init(photoInfo);
		this.m_previews[this.m_emptyIndex].gameObject.SetActive(true);
		this.m_emptyIndex++;
	}

	public void CheckPreview(int index, PhotoInfo photoInfo)
	{
		if (this.m_previews[index].PhotoId != photoInfo.Id)
		{
			this.m_previews[index].Init(photoInfo);
			this.m_previews[index].gameObject.SetActive(true);
		}
	}

	public void Subscribe()
	{
		this.m_subscribed = true;
		for (int i = 0; i < this.m_previews.Count; i++)
		{
			PhotoPreview photoPreview = this.m_previews[i];
			photoPreview.OnClick = (Action<PhotoInfo, PhotoPreview>)Delegate.Combine(photoPreview.OnClick, new Action<PhotoInfo, PhotoPreview>(this.OnImageClickHandler));
		}
	}

	public void LoadIcons()
	{
		base.StartCoroutine(this.LoadIconsCoroutine());
	}

	public void UnloadIcons()
	{
		for (int i = 0; i < this.m_previews.Count; i++)
		{
			if (this.m_previews[i].isActiveAndEnabled)
			{
				this.m_previews[i].UnloadIcon();
			}
		}
	}

	public void Reinit()
	{
		for (int i = 0; i < this.m_previews.Count; i++)
		{
			if (this.m_previews[i].Inited)
			{
				this.m_previews[i].Reinit();
			}
		}
	}

	private void OnImageClickHandler(PhotoInfo photoInfo, PhotoPreview imagePreview)
	{
		this.OnImageClick.SafeInvoke(photoInfo, imagePreview, this);
	}

	public void Clear()
	{
		for (int i = 0; i < this.m_previews.Count; i++)
		{
			this.m_previews[i].gameObject.SetActive(false);
		}
		this.m_emptyIndex = 0;
	}

	public void ClearPreview(int index)
	{
		this.m_previews[index].gameObject.SetActive(false);
		if (this.m_previews.All((PhotoPreview a) => !a.gameObject.activeSelf))
		{
			this.Index = -1;
		}
	}
	private IEnumerator LoadIconsCoroutine()
	{
		for(int i = 0; i < this.m_previews.Count; i++)
		{
			if (this.m_previews[i].Inited)
			{
				this.m_previews[i].LoadIcon();
			}
			yield return null;
		}
	}
}
