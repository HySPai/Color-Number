/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[Serializable]
public class SavedWorksList
{
	private static string s_filePath = "swl.swl";

	public static string Ext = ".dat";

	private Dictionary<string, List<SavedWorkInfo>> m_dict;

	private List<PhotoInfo> m_photoList;

	public static string GetPathToSave(string id)
	{
		return AppPathsConfig.SavesPath + id + SavedWorksList.Ext;
	}

	public void Init()
	{
		SavedWorksList.s_filePath = AppPathsConfig.SavesPath + SavedWorksList.s_filePath;
		if (File.Exists(SavedWorksList.s_filePath))
		{
			try
			{
				this.m_dict = Serializer.LoadFromFile<Dictionary<string, List<SavedWorkInfo>>>(SavedWorksList.s_filePath);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(ex.Message);
			}
			finally
			{
				if (this.m_dict == null)
				{
					this.m_dict = new Dictionary<string, List<SavedWorkInfo>>();
				}
			}
		}
		else
		{
			this.m_dict = new Dictionary<string, List<SavedWorkInfo>>();
		}
		this.CheckSavedWorks();
	}

	private void CheckSavedWorks()
	{
		string[] files = Directory.GetFiles(AppPathsConfig.SavesPath);
		string[] array = files;
		foreach (string text in array)
		{
			if (text.EndsWith(SavedWorksList.Ext))
			{
				ISavedWorkData savedWorkData = Serializer.LoadFromFile<ISavedWorkData>(text);
				if (savedWorkData != null)
				{
					if (!this.m_dict.ContainsKey(savedWorkData.ImageInfo.Id))
					{
						this.m_dict.Add(savedWorkData.ImageInfo.Id, new List<SavedWorkInfo>());
					}
					SavedWorkInfo savedWorkInfo = this.m_dict[savedWorkData.ImageInfo.Id].FirstOrDefault((SavedWorkInfo a) => a.Id == savedWorkData.Id);
					if (savedWorkInfo == null)
					{
						this.m_dict[savedWorkData.ImageInfo.Id].Add(new SavedWorkInfo(savedWorkData.Id, File.GetLastWriteTime(text)));
					}
				}
			}
		}
		this.Save();
	}

	public void UpdateList(string saveId, DateTime dateTime)
	{
		ISavedWorkData savedWorkData = this.LoadById(saveId);
		if (!this.m_dict.ContainsKey(savedWorkData.ImageInfo.Id))
		{
			this.m_dict.Add(savedWorkData.ImageInfo.Id, new List<SavedWorkInfo>());
		}
		SavedWorkInfo savedWorkInfo = this.m_dict[savedWorkData.ImageInfo.Id].FirstOrDefault((SavedWorkInfo a) => a.Id == saveId);
		if (savedWorkInfo == null)
		{
			this.m_dict[savedWorkData.ImageInfo.Id].Add(new SavedWorkInfo(saveId, dateTime));
		}
		else
		{
			savedWorkInfo.DateTime = dateTime;
			UnityEngine.Debug.Log("Update " + savedWorkInfo.Id + "  " + savedWorkInfo.DateTime);
		}
	}

	public void ForceUpdateList()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(AppPathsConfig.SavesPath);
		IEnumerable<FileInfo> enumerable = from a in directoryInfo.GetFiles()
										   where a.FullName.EndsWith(".dat")
										   select a;
		foreach (FileInfo item in enumerable)
		{
			IEnumerable<SavedWorkInfo> source = this.m_dict.SelectMany((KeyValuePair<string, List<SavedWorkInfo>> a) => a.Value);
			string fileName = item.Name.Replace(".dat", string.Empty);
			if (!source.Any((SavedWorkInfo a) => a.Id == fileName))
			{
				this.UpdateList(fileName, item.LastWriteTime);
			}
		}
		this.Save();
	}

	public List<string> GetSaves()
	{
		return (from b in this.m_dict.SelectMany((KeyValuePair<string, List<SavedWorkInfo>> a) => a.Value)
				orderby b.DateTime descending
				select b into c
				select c.Id).ToList();
	}

	public List<PhotoInfo> GetPhotos()
	{
		this.m_photoList = Serializer.LoadFromFile<List<PhotoInfo>>(AppPathsConfig.PhotosPath + AppPathsConfig.PhotoFileList);
		if (this.m_photoList == null)
		{
			this.m_photoList = new List<PhotoInfo>();
		}
		return this.m_photoList;
	}

	public void AddPhoto(PhotoInfo photoInfo)
	{
		this.m_photoList.Add(photoInfo);
		this.SavePhotos();
	}

	public void DeletePhoto(string photoId)
	{
		if (this.m_photoList != null)
		{
			PhotoInfo photoInfo = this.m_photoList.FirstOrDefault((PhotoInfo a) => a.Id == photoId);
			if (photoInfo != null)
			{
				this.m_photoList.Remove(photoInfo);
			}
			this.SavePhotos();
		}
	}

	private void SavePhotos()
	{
		Serializer.SaveToFile(AppPathsConfig.PhotosPath + AppPathsConfig.PhotoFileList, this.m_photoList, false);
	}

	public void Save()
	{
		Serializer.SaveToFile(SavedWorksList.s_filePath, this.m_dict, false);
	}

	public ISavedWorkData Save(ImageInfo imageInfo, byte[] preview, ISavedWorkData savedWorkData, bool completed)
	{
		if (savedWorkData == null)
		{
			savedWorkData = new SavedWorkData();
		}
		savedWorkData.Init(imageInfo, preview, completed, null);
		if (!string.IsNullOrEmpty(savedWorkData.FileName))
		{
			Serializer.SaveToFile(savedWorkData.FullFileName, savedWorkData, true);
			if (savedWorkData.History2 == null)
			{
				savedWorkData.InitHistory();
			}
			savedWorkData.History2.Save();
		}
		else if (!File.Exists(savedWorkData.FullFileName))
		{
			savedWorkData.SetFile(imageInfo.Id.ToString());
			Serializer.SaveToFile(savedWorkData.FullFileName, savedWorkData, true);
			if (savedWorkData.History2 == null)
			{
				savedWorkData.InitHistory();
			}
			savedWorkData.History2.Save();
		}
		if (!this.m_dict.ContainsKey(imageInfo.Id))
		{
			this.m_dict.Add(imageInfo.Id, new List<SavedWorkInfo>());
		}
		SavedWorkInfo savedWorkInfo = this.m_dict[imageInfo.Id].FirstOrDefault((SavedWorkInfo a) => a.Id == savedWorkData.Id);
		if (savedWorkInfo != null)
		{
			savedWorkInfo.DateTime = DateTime.Now;
		}
		else
		{
			this.m_dict[imageInfo.Id].Add(new SavedWorkInfo(savedWorkData.Id, DateTime.Now));
		}
		this.Save();
		return savedWorkData;
	}

	public SavedWorkData3D Save3D(ImageInfo imageInfo, byte[] preview, SavedWorkData3D savedWorkData, bool completed, List<HistoryStep> history, List<bool> progress)
	{
		if (savedWorkData == null)
		{
			savedWorkData = new SavedWorkData3D();
		}
		savedWorkData.Init(imageInfo, preview, completed, history, progress);
		if (!string.IsNullOrEmpty(savedWorkData.FileName))
		{
			Serializer.SaveToFile(savedWorkData.FullFileName, savedWorkData, true);
		}
		else if (!File.Exists(savedWorkData.FullFileName))
		{
			savedWorkData.SetFile(imageInfo.Id.ToString());
			Serializer.SaveToFile(savedWorkData.FullFileName, savedWorkData, true);
		}
		else
		{
			int num = 0;
			while (File.Exists(AppPathsConfig.SavesPath + imageInfo.Id + "_" + num.ToString() + SavedWorksList.Ext))
			{
				num++;
			}
			savedWorkData.SetFile(imageInfo.Id + "_" + num.ToString());
			Serializer.SaveToFile(AppPathsConfig.SavesPath + imageInfo.Id + "_" + num.ToString() + SavedWorksList.Ext, savedWorkData, true);
		}
		if (!this.m_dict.ContainsKey(imageInfo.Id))
		{
			this.m_dict.Add(imageInfo.Id, new List<SavedWorkInfo>());
		}
		SavedWorkInfo savedWorkInfo = this.m_dict[imageInfo.Id].FirstOrDefault((SavedWorkInfo a) => a.Id == savedWorkData.Id);
		if (savedWorkInfo != null)
		{
			savedWorkInfo.DateTime = DateTime.Now;
		}
		else
		{
			this.m_dict[imageInfo.Id].Add(new SavedWorkInfo(savedWorkData.Id, DateTime.Now));
		}
		this.Save();
		return savedWorkData;
	}

	public void Load(NumberColoring nc, string fileName)
	{
		ISavedWorkData savedWorkData = Serializer.LoadFromFile<ISavedWorkData>(fileName);
		savedWorkData.Apply(nc);
	}

	public ISavedWorkData Load(string fileName)
	{
		return Serializer.LoadFromFile<ISavedWorkData>(fileName);
	}

	public ISavedWorkData LoadById(string saveId)
	{
		return Serializer.LoadFromFile<ISavedWorkData>(SavedWorksList.GetPathToSave(saveId));
	}

	public void Delete(string saveId)
	{
		ISavedWorkData swd = this.LoadById(saveId);
		if (this.m_dict.ContainsKey(swd.ImageInfo.Id))
		{
			SavedWorkInfo savedWorkInfo = this.m_dict[swd.ImageInfo.Id].FirstOrDefault((SavedWorkInfo a) => a.Id == swd.Id);
			if (savedWorkInfo != null)
			{
				this.m_dict[swd.ImageInfo.Id].Remove(savedWorkInfo);
			}
		}
		File.Delete(swd.FullFileName);
		this.Save();
	}

	public string LastSaveOfImageId(string imageId)
	{
		if (this.m_dict != null && this.m_dict.ContainsKey(imageId))
		{
			SavedWorkInfo savedWorkInfo = (from a in this.m_dict[imageId]
										   orderby a.DateTime
										   select a).LastOrDefault();
			if (savedWorkInfo != null)
			{
				return savedWorkInfo.Id;
			}
		}
		return null;
	}
}


