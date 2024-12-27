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
public class SavedWorkData : ISavedWorkData
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

	public SavedWorkData()
	{
		this.Id = Guid.NewGuid().ToString();
		this.UvRect = new SerializableRect(new Rect(0f, 0f, 1f, 1f));
	}

	public void Init(ImageInfo imageInfo, byte[] preview, bool completed, List<HistoryStep> history)
	{
		this.ImageInfo = imageInfo;
		this.Preview = preview;
		this.Completed = completed;
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
		this.History2 = new History();
		this.History2.Init(this.FullFileName + ".his");
		if (this.History != null && this.History.Count > 0)
		{
			foreach (HistoryStep item in this.History)
			{
				this.History2.AddStep(item);
			}
			this.History = null;
		}
		else if (this.History2.Steps.Count == 0)
		{
			this.GenerateHistoryForOldSave();
		}
		nc.ApplyResTexture(texture2D, this.Completed, this.History2);
	}

	private void GenerateHistoryForOldSave()
	{
		Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		texture2D.LoadImage(this.Preview);
		Color[] pixels = texture2D.GetPixels();
		int width = texture2D.width;
		for (int i = 0; i < pixels.Length; i++)
		{
			if (pixels[i].a > 0.5f && pixels[i] != Color.white)
			{
				HistoryStep historyStep = new HistoryStep();
				int x = i % width;
				int y = i / width;
				historyStep.Add(new ShortVector2(x, y));
				this.History2.AddStep(historyStep);
			}
		}
		this.History2.Shuffle();
	}

	public void SetImageInfo(ImageInfo ii)
	{
		this.ImageInfo = ii;
	}

	public void InitHistory()
	{
		if (this.History2 == null)
		{
			this.History2 = new History();
			this.History2.Init(this.FullFileName + ".his");
		}
	}

	public override string ToString()
	{
		return "SavedWorkData " + this.Id;
	}
}


