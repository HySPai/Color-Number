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
using UnityEngine;

[Serializable]
public class SavedWorkData3D : ISavedWorkData
{
	[NonSerialized]
	private History m_history;

	public string Id { get; private set; }

	public ImageInfo ImageInfo { get; private set; }

	public string FileName { get; private set; }

	public byte[] Preview { get; private set; }

	public bool Completed { get; private set; }

	public int FilterId { get; set; }

	public Rect UvRect1
	{
		get
		{
			if (this.UvRect != null)
			{
				return this.UvRect.ToRect();
			}
			return new Rect(0f, 0f, 1f, 1f);
		}
	}

	public SerializableRect UvRect { get; set; }

	[Obsolete("History saves in enother file now. Use History2")]
	public List<HistoryStep> History { get; private set; }

	public History History2
	{
		get
		{
			return this.m_history;
		}
		private set
		{
			this.m_history = value;
		}
	}

	public List<bool> Progress { get; private set; }

	public bool Is3D
	{
		get
		{
			return this.ImageInfo.Is3D;
		}
	}

	public string FullFileName
	{
		get
		{
			return SavedWorksList.GetPathToSave(this.Id);
		}
	}

	public SavedWorkData3D()
	{
		this.Id = Guid.NewGuid().ToString();
		this.UvRect = new SerializableRect(new Rect(0f, 0f, 1f, 1f));
		this.Progress = new List<bool>();
	}

	public void Init(ImageInfo imageInfo, byte[] preview, bool completed, List<HistoryStep> history)
	{
		this.ImageInfo = imageInfo;
		this.Preview = preview;
		this.Completed = completed;
	}

	public void Init(ImageInfo imageInfo, byte[] preview, bool completed, List<HistoryStep> history, List<bool> progress)
	{
		this.ImageInfo = imageInfo;
		this.Preview = preview;
		this.Completed = completed;
		this.Progress = progress;
	}

	public static bool IsNeedToSave(NumberColoring go)
	{
		if (go == null)
		{
			return false;
		}
		return !go.IsEmpty();
	}

	public void SetFile(string file)
	{
		this.FileName = file;
	}

	public void Apply(NumberColoring nc)
	{
		Transform transform = nc.transform;
		Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		texture2D.filterMode = FilterMode.Point;
		texture2D.LoadImage(this.Preview);
	}

	public void SetImageInfo(ImageInfo ii)
	{
		this.ImageInfo = ii;
	}

	public void InitHistory()
	{
	}

	public override string ToString()
	{
		return "SavedWorkData3D " + this.Id;
	}
}


