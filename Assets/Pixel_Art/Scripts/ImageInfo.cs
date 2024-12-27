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

[Serializable]
public class ImageInfo : IAccessStatusInfo
{
	public string Name { get; set; }

	public string Id { get; set; }

	public AccessStatus AccessStatus { get; set; }

	public string Url { get; set; }

	public bool Is3D { get; set; }

	public string Source { get; set; }
	
	public AccessStatus CustomAccessStatus
	{
		get
		{
			if (!IAPWrapper.Instance.Subscribed && !IAPWrapper.Instance.NoAds)
			{
				switch (INPluginWrapper.Instance.GetAbTestGroup())
				{
					case ABTestGroup.RewardedNo_ContentEasy:
						return AccessStatus.Free;
					case ABTestGroup.RewardedYes_ContentHard:
					case ABTestGroup.RewardedNo_ContentHard:
					case ABTestGroup.Rewarded_yes_content_hard_no1screen:
						return (AccessStatus)((this.AccessStatus == AccessStatus.Free) ? 1 : 0);
					default:
						return this.AccessStatus;
				}
			}
			return AccessStatus.Free;
		}
	}

	public ImageInfo()
	{
		this.Id = null;
		this.AccessStatus = AccessStatus.Free;
		this.Url = null;
	}

	public ImageInfo(string id)
	{
		this.Id = id;
		this.AccessStatus = AccessStatus.Free;
		this.Url = null;
	}

	public ImageInfo(string id, string source)
	{
		this.Id = id;
		this.AccessStatus = AccessStatus.Free;
		this.Url = null;
		this.Source = source;
	}

	public ImageInfo(Dictionary<string, object> dict)
	{
		this.Name = ((long)dict["Name"]).ToString();
		this.Id = (string)dict["ID"];
		this.AccessStatus = (AccessStatus)((!((string)dict["Premium"] == "0")) ? 1 : 0);
		this.Url = (string)dict["ImageUrl"];
		this.Is3D = (dict.ContainsKey("3d") && (string)dict["3d"] == "true");
		this.Source = "gallery_" + ((!this.Is3D) ? "2D" : "3D");
	}

	public string GetRightSource()
	{
		if (string.IsNullOrEmpty(this.Source))
		{
			this.Source = "gallery_" + ((!this.Is3D) ? "2D" : "3D");
		}
		return this.Source;
	}

	public override bool Equals(object obj)
	{
		ImageInfo imageInfo = obj as ImageInfo;
		if (imageInfo == null)
		{
			return false;
		}
		return this.Id.Equals(imageInfo.Id);
	}

	public override int GetHashCode()
	{
		return this.Id.GetHashCode();
	}

	public override string ToString()
	{
		return string.Format("[ImageInfo: Id={0}, AccessStatus={1}, Url={2}, Is3D={3}]", this.Id, this.AccessStatus, this.Url, this.Is3D);
	}
}


