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
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class ImagesInfo
{
	private List<ImageInfo> m_images;

	public int Version { get; set; }
	public int Count { get; private set; }

	public List<ImageInfo> Images
	{
		get
		{
			return this.m_images;
		}
	}

	public ImagesInfo()
	{
		this.Version = -1;
		this.m_images = new List<ImageInfo>();
	}

	public void Add(ImageInfo imageInfo)
	{
		this.m_images.Add(imageInfo);
		Count++;
	}
#if UNITY_EDITOR
	public void Arrange(string onlineDatasetLocation, ImagesInfo existingImages)
	{
		if (existingImages != null && existingImages.Images != null)
		{
			var dict = new Dictionary<string, ImageInfo>();
			foreach (var image in existingImages.Images)
			{
				dict[image.Id] = image; 
			}

			var list = new List<ImageInfo>();
			foreach(var image in this.m_images)
			{
				if (!dict.ContainsKey(image.Id))
				{
					list.Add(image);
				}
			}

			foreach (var image in existingImages.Images)
			{
				list.Add(image);
			}
			this.m_images.Clear();

			foreach(var image in list)
			{
				string fileName = Path.Combine(onlineDatasetLocation, image.Url);
				if (File.Exists(fileName) && (!image.Is3D || File.Exists(fileName + ".vox")))
				{
					this.m_images.Add(image);
				}
				else
				{
					Debug.Log(fileName + " does not exist");
				}
			}

			this.Count = this.m_images.Count;
		}
	}
#endif
}


