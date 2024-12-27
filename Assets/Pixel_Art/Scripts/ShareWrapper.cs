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
using UnityEngine;

public class ShareWrapper : MonoBehaviour
{
	public enum SocialType
	{
		Instagram,
		Facebook,
		NativeShare
	}

	public enum ShareStatus
	{
		Success,
		Failure,
		AppNotInstalled
	}

	public enum NativeShareType
	{
		Image = 1,
		Video = 2
	}

	public static void Init()
	{
	}

	public static void ShareApp()
	{
		PlatformShare(NativeShareType.Image, SocialType.NativeShare, "Pixel Art - Color by Number, visit us at https://play.google.com/store/apps/details?id=com.ino.games.pixeldot", null); 
	}

	public static void ShareText(string handline, string subject, string text)
	{
		PlatformShare(NativeShareType.Image, SocialType.NativeShare, text, null); 
	}

	public static void ShareImage(string handline, string subject, string text, string filePath)
	{
		PlatformShare(NativeShareType.Image, SocialType.NativeShare, text, filePath);
	}


	public static bool IsAppInstalled(string bundleID)
	{
#if UNITY_ANDROID
		AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
		Debug.Log(" ********LaunchOtherApp ");
		AndroidJavaObject launchIntent = null;
		//if the app is installed, no errors. Else, doesn't get past next line
		try
		{
			launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleID);
			//        
			//        ca.Call("startActivity",launchIntent);
		}
		catch (Exception ex)
		{
			Debug.Log("exception" + ex.Message);
		}
		if (launchIntent == null)
			return false;
		return true;
#else
         return true;
#endif
	}

	private static bool CanShare(SocialType socialType)
	{
		/*
		 *  Facebook - "com.facebook.katana"
		 *	Twitter - "com.twitter.android"
		 *	Instagram - "com.instagram.android"
		 *	Pinterest - "com.pinterest"
		*/
		string applicationId = null;
		if (socialType == SocialType.Instagram)
		{
			applicationId = "com.instagram.android";
		}
		else if (socialType == SocialType.Facebook)
		{
			applicationId = "com.facebook.katana";
		}
		if (!string.IsNullOrEmpty(applicationId) && !IsAppInstalled(applicationId))
		{
			return false;
		}
		return true;
	}

	private static ShareStatus PlatformShare(NativeShareType type, SocialType socialType, string text, string pathToImage)
	{
		/*
		 *  Facebook - "com.facebook.katana"
		 *	Twitter - "com.twitter.android"
		 *	Instagram - "com.instagram.android"
		 *	Pinterest - "com.pinterest"
		*/
		string applicationId = null;
		if (socialType == SocialType.Instagram)
		{
			applicationId = "com.instagram.android";
		}
		else if (socialType == SocialType.Facebook)
		{
			applicationId = "com.facebook.katana";
		}

#if UNITY_ANDROID

		if (!string.IsNullOrEmpty(applicationId) && !IsAppInstalled(applicationId))
		{
			return ShareStatus.AppNotInstalled;
		}

		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
		if (!string.IsNullOrEmpty(pathToImage))
		{
			AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + pathToImage);
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
		}
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), text);
		intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");

		intentObject.Call<AndroidJavaObject>("addFlags", intentClass.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK"));
		intentObject.Call<AndroidJavaObject>("setPackage", applicationId);

		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

		currentActivity.Call("startActivity", intentObject);
		return ShareStatus.Success;
#endif

#if UNITY_IPHONE
        //GeneralSharingiOSBridge.ShareTextWithImage(pathToImage, text);

		NativeShare.Share(text, pathToImage);
		return ShareStatus.Success;
#endif

	}

	public static void ShareVideo(string handline, string subject, string text, string filePath)
	{
	}

	public static bool CanFbShare()
	{
		return CanShare(SocialType.Facebook);
	}

	public static void ShareImageFb(string handline, string subject, string text, string uri)
	{
		PlatformShare(NativeShareType.Image, SocialType.Facebook, text, uri);
	}

	public static void ShareVideoFb(string handline, string subject, string text, string filePath)
	{
	}

	public static bool CanInstShare()
	{
		return CanShare(SocialType.Instagram);
	}

	public static void ShareImageInstagram(string handline, string subject, string text, string filePath)
	{
		PlatformShare(NativeShareType.Image, SocialType.Instagram, text, filePath);
	}

	public static void ShareVideoInstagram(string handline, string subject, string text, string filePath)
	{
	}

	public static AndroidJavaObject Texture2DToAndroidBitmap(Texture2D texture2D)
	{
		byte[] encoded = texture2D.EncodeToPNG();
		using (var bf = new AndroidJavaClass("android.graphics.BitmapFactory"))
		{
			return bf.CallStatic<AndroidJavaObject>("decodeByteArray", encoded, 0, encoded.Length);
		}
	}
	public static void SaveIntoGallery(string imagePath, string title, string description, Action<bool> handler)
	{
#if UNITY_EDITOR
		handler(true);
#else
		NativeGallery.SaveImageToGallery(imagePath, title, "Capture_{0}.jpg", (error) =>
		{
			handler.SafeInvoke(error == null);
		});
#endif
	}
}

