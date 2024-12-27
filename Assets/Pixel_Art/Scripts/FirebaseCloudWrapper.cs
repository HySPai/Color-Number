/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

//using Firebase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
//using System.Threading.Tasks;
using UnityEngine;

//using Firebase.Storage;
//public class FirebaseCloudWrapper : MonoBehaviour {

//	protected string MyStorageBucket = "gs://pixeldot-colorbynumber.appspot.com/";
//	private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
//	protected FirebaseStorage storage;

//	public static FirebaseCloudWrapper Instance;
//	// Use this for initialization
//	private bool inited = true;
//	private bool busy = false;
//	private void Awake()
//	{
//		Instance = this;
//		Init();
//	}
//	private void Init(Action callback = null)
//	{ 
//		if (!inited)
//		{
//			//InitializeFirebase();
//			FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
//			{
//				dependencyStatus = task.Result;
//				if (dependencyStatus == DependencyStatus.Available)
//				{
//					InitializeFirebase();
//				}
//				else
//				{
//					Debug.LogError(
//					  "Could not resolve all Firebase dependencies: " + dependencyStatus);
//				}
//				if (callback != null)
//				{
//					callback();
//				}
//			});
//		}
//		else
//		{
//			if (callback != null)
//			{
//				callback();
//			}
//		}
//	}
//	private void InitializeFirebase()
//	{
//		var appBucket = FirebaseApp.DefaultInstance.Options.StorageBucket;
//		storage = FirebaseStorage.DefaultInstance;
//		if (!string.IsNullOrEmpty(appBucket))
//		{
//			MyStorageBucket = string.Format("gs://{0}/", appBucket);
//		}
//		inited = true;
//	}

//	public static void GetDownloadUrl(string url, Action<string> handler)
//	{
//		if (Instance != null)
//		{
//			Instance.AsyncDownload(url, handler);
//		}
//	}

//	private void AsyncDownload(string url, Action<string> handler)
//	{ 
//		Init(() =>
//		{ 
//			//handler(false, null);

//			//const long maxAllowedSize = 1 * 1024 * 1024;
//			//reference.GetBytesAsync(maxAllowedSize).ContinueWith((System.Threading.Tasks.Task<byte[]> task) =>
//			//{
//			//	if (task.IsFaulted || task.IsCanceled)
//			//	{
//			//		Debug.Log(url);
//			//		Debug.LogError(task.Exception);
//			//		handler(false, null);
//			//		// Uh-oh, an error occurred!
//			//	}
//			//	else
//			//	{
//			//		handler(true, task.Result);
//			//		Debug.Log("Finished downloading!");
//			//	}
//			//});
//			StartCoroutine(DownloadFromFirebaseStorage(url, handler));
//		}); 
//	}
	 
//	protected IEnumerator DownloadFromFirebaseStorage(string url, Action<string> handler)
//	{
//		if (busy)
//			yield return null;

//		busy = true;
//		Debug.Log("Download file " + url);
//		string firebaseStorageLocation = MyStorageBucket + url;
//		StorageReference reference = FirebaseStorage.DefaultInstance.GetReferenceFromUrl(firebaseStorageLocation);
//		// Download in memory with a maximum allowed size of 1MB (1 * 1024 * 1024 bytes)

//		reference.GetDownloadUrlAsync().ContinueWith((Task<Uri> task) =>
//		{
//			busy = false;
//			if (!task.IsFaulted && !task.IsCanceled)
//			{
//				var encodedUrl = WWW.EscapeURL(url);
//				var onlineUrl = task.Result.ToString().Replace(url, encodedUrl);

//				Debug.Log("Download URL: " + onlineUrl);
//				handler(onlineUrl);
//				// ... now download the file via WWW or UnityWebRequest.
//			}
//			else
//			{
//				Debug.LogError(task.Exception);
//			}
//		}); 
//	}
//	public void DebugLog(string s)
//	{
//		Debug.Log(s);
//	}
//}
