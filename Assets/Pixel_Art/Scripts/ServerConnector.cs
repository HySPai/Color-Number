/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using MiniJSON;
using Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ServerConnector : MonoBehaviour
{
	public Action OnInternetAppeared;

	private bool m_internetAvailable;

	private bool m_createUserRequest;

	public void Init()
	{
		base.StartCoroutine(this.CheckInternetCoroutine());
	}

	private void AppearInternet()
	{
		this.OnInternetAppeared.SafeInvoke();
	}

	public void CreateUser(string uid, Action<string> handler)
	{ 
	}

	public void SendPushToken(string uid, string password, string oneSignalUserId, Action<bool> handler)
	{ 
	}
	public IEnumerator LoadStreamingAsset(string url, Action<WWW> handler)
	{
#if !UNITY_ANDROID || UNITY_EDITOR
		if (!url.StartsWith("file://", StringComparison.CurrentCultureIgnoreCase)) {
            url = "file://" + url;
        }
#endif
        Debug.Log("load streaming asset");
		WWW www = new WWW(url);
		yield return www;

		if (!string.IsNullOrEmpty(www.error))
		{ 
			Debug.Log("not found " + url + " " + www.error);
			handler(www);
			yield break;
		}
		else
        {
            Debug.Log("OK " + url);
			handler(www); 
		}

		yield return 0;
	}
	public IEnumerator GetLocalStreamData(Action<ImagesInfo> handler, Action notFoundHandler = null)
	{
		var localFile = System.IO.Path.Combine(Application.streamingAssetsPath, AppPathsConfig.ImagesFile);
		Debug.Log("Local stream asset: " + localFile);
#if UNITY_ANDROID || UNITY_IOS
		StartCoroutine(LoadStreamingAsset(localFile, (www) =>
		{
			if (!string.IsNullOrEmpty(www.error))
			{
				//Debug.Log(localFile + " not found");
				if (notFoundHandler != null)
					notFoundHandler();
			}
			else
			{
                Debug.Log("using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(www.text)))");
				using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(www.text)))
				{
					var imagesInfo = Serializer.LoadFromTextStream<ImagesInfo>(stream);

                    Debug.Log("imagesInfo count = " + imagesInfo.Count);
					handler.SafeInvoke(imagesInfo);
				}
			}
		}));
#else
		if (File.Exists(localFile))
        {
            Debug.Log("FOUND: " + localFile);
            var imagesInfo = Serializer.LoadFromFile<ImagesInfo>(localFile);
            handler.SafeInvoke(imagesInfo);
   //         using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(localFile)))
			//{
			//	var imagesInfo = Serializer.LoadFromTextStream<ImagesInfo>(stream);
			//	handler.SafeInvoke(imagesInfo);
			//}
		}
		else
		{
			if (notFoundHandler != null)
				notFoundHandler();
		}
#endif
		yield return null;
	}
	public void GetImagesList(Action<ImagesInfo> handler)
    {
        Debug.Log("GetImagesList begin");
		if (InternetConnection.IsAvailable)
		{
            var url = AppPathsConfig.Host + AppPathsConfig.ImagesFile;
            Debug.Log("GetImagesList InternetConnection.IsAvailable url " + url);
			//FirebaseCloudWrapper.GetDownloadUrl(url, (firebaseUrl) =>
			//{
			StartCoroutine(SendDirectServerRequestForTextResponse(url, (status, text, headers) =>
			{
				if (!status)
				{
					Debug.Log(url + " not found");
					StartCoroutine(GetLocalStreamData(handler));
				}
				else
				{
					using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text)))
					{
						var imagesInfo = Serializer.LoadFromTextStream<ImagesInfo>(stream);
						handler.SafeInvoke(imagesInfo);
					}
				}
			}));
			//}); 
		}
		else
		{
			StartCoroutine(GetLocalStreamData(handler, null));
		}
	}

	private IEnumerator GetLocalStreamData(string path, Action<bool, byte[]> handler, Action notFoundHandler)
	{
		var localFile = System.IO.Path.Combine(Application.streamingAssetsPath, path);
		Debug.Log("Local stream asset: " + localFile);
#if UNITY_ANDROID || UNITY_IOS
		StartCoroutine(LoadStreamingAsset(localFile, (www) =>
		{
			if (!string.IsNullOrEmpty(www.error))
			{
				//Debug.Log(localFile + " not found");
				//handler.SafeInvoke(false, null);
				if (notFoundHandler != null)
				{
					notFoundHandler();
				}
			}
			else
			{
				handler.SafeInvoke(www.bytes != null && www.bytes.Length > 0, www.bytes);
			}
		}));
#else
		if (File.Exists(localFile))
        {
            Debug.Log("FOUND: " + localFile);
			var bytes = File.ReadAllBytes(localFile);
			handler.SafeInvoke(bytes != null && bytes.Length > 0, bytes); 
		}
		else
        { 
			if (notFoundHandler != null)
				notFoundHandler();
		}
#endif
		yield return null;
	}
	public void GetImageAsset(ImageInfo imageInfo, Action<bool, byte[]> handler)
	{
		StartCoroutine(GetLocalStreamData(imageInfo.Url, handler, ()=>
		{
			if (InternetConnection.IsAvailable)
			{
				var url = AppPathsConfig.Host + imageInfo.Url;
				///FirebaseCloudWrapper.GetDownloadUrl(url, (firebaseUrl) =>
				//{
				StartCoroutine(SendDirectServerRequestForBinaryResponse(url, (status, bytes) =>
				{
					if (!status)
					{
						Debug.Log(url + " not found");
						//StartCoroutine(GetLocalStreamData(imageInfo.Url, handler));
						handler.SafeInvoke(false, null);
					}
					else
					{
						handler.SafeInvoke(bytes != null && bytes.Length > 0, bytes);
					}
				}));
				//});

			}
		})); 
	}

	public void GetImageAsset3D(ImageInfo imageInfo, Action<bool, byte[]> handler)
	{
		StartCoroutine(GetLocalStreamData(imageInfo.Url + ".vox", handler, ()=>
		{
			if (InternetConnection.IsAvailable)
			{
				var url = AppPathsConfig.Host + imageInfo.Url + ".vox";
				//FirebaseCloudWrapper.GetDownloadUrl(url, (firebaseUrl) =>
				//{
				StartCoroutine(SendDirectServerRequestForBinaryResponse(url, (status, bytes) =>
				{
					if (!status)
					{
						Debug.Log(url + " not found");
						handler.SafeInvoke(false, null);
						//StartCoroutine(GetLocalStreamData(imageInfo.Url + ".vox", handler));
					}
					else
					{
						handler.SafeInvoke(bytes != null && bytes.Length > 0, bytes);
					}
				}));
				//});

			}
		}));
		

		//string text = imageInfo.Url.Replace("/artnum/", string.Empty);
		//text = imageInfo.Url.Replace(".png", ".3d");
		//base.StartCoroutine(this.SendDirectServerRequestForBinaryResponse(text, delegate (bool result, byte[] bytes)
		//{
		//	handler.SafeInvoke(bytes != null && bytes.Length > 0, bytes);
		//}));
	}
	private IEnumerator CheckInternetCoroutine()
	{
		while (true)
		{
			var internetAvailable = InternetConnection.IsAvailable;
			if (this.m_internetAvailable != internetAvailable)
			{
				this.m_internetAvailable = internetAvailable;
				if (this.m_internetAvailable)
				{
					this.AppearInternet();
				}
			}
			yield return new WaitForSeconds(5f);
		}
	} 

	private IEnumerator SendServerRequestForTextResponse(WWWForm wwwForm, Action<bool, string> handler)
	{
		yield return this.StartCoroutine(this.SendServerRequestForTextResponse(AppPathsConfig.Host, wwwForm, handler));
	}


	private IEnumerator SendServerRequestForTextResponse(string host, WWWForm wwwForm, Action<bool, string> handler)
	{
		var www = new WWW(host, wwwForm);
		yield return www;
		 
		if (!string.IsNullOrEmpty(www.error))
		{
			UnityEngine.Debug.Log(www.error);
			handler.SafeInvoke(false, null);
		}
		else
		{ 
			if (www.text.Contains("Fatal error"))
			{
				handler.SafeInvoke(false, null);
			}
			else
			{
				handler.SafeInvoke(true, www.text);
			}
		}
	}


	private IEnumerator SendDirectServerRequestForTextResponse(string url, Action<bool, string, Dictionary<string, string>> handler)
	{
		var www = new WWW(url);
		yield return www;

        Debug.Log("SendDirectServerRequestForTextResponse ");
		if (!string.IsNullOrEmpty(www.error))
		{
			UnityEngine.Debug.Log(www.error);
			handler.SafeInvoke(false, null, null);
		}
		else
		{ 
			if (www.text.Contains("Fatal error"))
            {
                Debug.Log("SendDirectServerRequestForTextResponse error " + www.text);
				handler.SafeInvoke(false, null, null);
			}
			else
            {
                Debug.Log("SendDirectServerRequestForTextResponse OK");
				handler.SafeInvoke(true, www.text, www.responseHeaders);
			}
		}
	}


	private IEnumerator SendDirectServerRequestForBinaryResponse(string url, Action<bool, byte[]> handler, float timeout = -1f)
	{
		var www = new WWW(url);
		if (timeout > 0f)
		{
			while (timeout > 0f && !www.isDone)
			{
				timeout -= Time.deltaTime;
				yield return null;
				continue;
			}
			if (!www.isDone)
			{
				handler.SafeInvoke(false, null);
				yield break;
			} 
		}

		yield return www; 

		if (!string.IsNullOrEmpty(www.error))
		{
			UnityEngine.Debug.Log(www.error);
			handler.SafeInvoke(false, null);
		}
		else
		{ 
			handler.SafeInvoke(true, www.bytes);
		}
	}
}

