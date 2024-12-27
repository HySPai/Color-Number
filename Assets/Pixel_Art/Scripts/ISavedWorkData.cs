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

public interface ISavedWorkData
{
	string Id
	{
		get;
	}

	ImageInfo ImageInfo
	{
		get;
	}

	string FileName
	{
		get;
	}

	byte[] Preview
	{
		get;
	}

	bool Completed
	{
		get;
	}

	int FilterId { get; set; }

	Rect UvRect1
	{
		get;
	}

	SerializableRect UvRect { get; set; }

	[Obsolete("History saves in enother file now. Use History2")]
	List<HistoryStep> History
	{
		get;
	}

	History History2
	{
		get;
	}

	string FullFileName
	{
		get;
	}

	void Init(ImageInfo imageInfo, byte[] preview, bool completed, List<HistoryStep> history);

	void SetFile(string file);

	void Apply(NumberColoring nc);

	void InitHistory();
}


