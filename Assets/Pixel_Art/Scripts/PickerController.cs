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

public class PickerController : UnitySingleton<PickerController>
{ 
	public Action<bool> InitComplete;

	private Action<Texture2D> localImageComplete;

	private bool isInited = false;

	protected void Awake()
	{ 
//#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
//		if (this._handler == null)
//		{
//			var go = GameObject.Find("Unimgpicker");
//			if (go == null)
//			{
//				go = new GameObject();
//				go.name = "Unimgpicker";
//				this._handler = go.AddComponent<Kakera.Unimgpicker>();
//			} 
//		} 
//#endif
//		if (this._handler == null)
//		{
//			this._handler = new PickerUnsupported();
//		}
	}

	public void Initilized()
	{
		isInited = true;
		this.InitComplete(true);
		//this._handler.Init(this.InitComplete);
	}

	private System.Collections.IEnumerator LoadImage(string path)
	{
		Debug.Log("Loading image from gallery " + path);
		var url = path;
#if UNITY_ANDROID
		if (!url.StartsWith("file:"))
		{
			url = "file://" + url;
		}
#endif
		var www = new WWW(url);
		yield return www;

		var texture = www.texture;
		if (texture == null)
		{
			Debug.LogError("Failed to load texture url:" + url);
		}
		 
		if (localImageComplete != null)
		{
			localImageComplete(texture);
		}
	}
	private void GetImageCompleteWrapper(string path)
	{
		StartCoroutine(LoadImage(path));
	}

	public void Subscribe(Action<Texture2D> GetImageComplete, Action<IList<string>> GetImageListComplete, Action<string> GetImageError, Action GetImageListError)
	{
		localImageComplete = GetImageComplete;
		//if (GetImageListComplete != null)
		//{
		//	this._handler.GetImageListPathComplete += GetImageListComplete;
		//}
		//if (GetImageError != null)
		//{
		//	this._handler.GetImagePathError += GetImageError;
		//}
		//if (GetImageComplete != null)
		//{
		//	this.localImageComplete = GetImageComplete;
		//	this._handler.GetImagePathComplete += GetImageCompleteWrapper;
		//}
		//if (GetImageListError != null)
		//{
		//	this._handler.GetImageListPathError += GetImageListError;
		//}
	}

	public void UnSubscribe(Action<Texture2D> GetImageComplete, Action<IList<string>> GetImageListComplete, Action<string> GetImageError, Action GetImageListError)
	{
		localImageComplete = null;
		//if (GetImageListComplete != null)
		//{
		//	this._handler.GetImageListPathComplete -= GetImageListComplete;
		//}
		//if (GetImageError != null)
		//{
		//	this._handler.GetImagePathError -= GetImageError;
		//}
		//if (GetImageComplete != null)
		//{
		//	this._handler.GetImagePathComplete -= GetImageCompleteWrapper;
		//	localImageComplete = null;
		//}
		//if (GetImageListError != null)
		//{
		//	this._handler.GetImageListPathError -= GetImageListError;
		//}
	}

	public void GetAllGalleryImagePaths()
	{
		//this._handler.GetAllGalleryImagePaths();
	}

	public void OpenGallery()
	{
		Debug.Log("open gallery");
		NativeGallery.GetImageFromGallery(new NativeGallery.MediaPickCallback((path) =>
		{
			GetImageCompleteWrapper(path);
		}), "Select an image");

		//this._handler.OpenGallery();
	}

	public bool IsInit()
	{
		return isInited;// this._handler.IsInit;
	}
}


