/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using System;
using UnityEngine;

public class DialogToolWrapper : MonoBehaviour
{
	private delegate void DialogCallbackDelegate(int buttonIndex);

	private static DialogToolWrapper s_instance;

	private static Action OnFacebookLogin;

	private static Action RateUsDialogCloseHandler;

	private static Action<bool> DeleteDialogCloseHandler;

	public static void ShowUpgradeDialog()
	{
		ActionSheetWrapper.actionSheet.Show2ButtonsDialog(LocalizationManager.Instance.GetString("upgrade_title"), LocalizationManager.Instance.GetString("upgrade_body"), LocalizationManager.Instance.GetString("upgrade_no"), LocalizationManager.Instance.GetString("upgrade_yes"), (buttonIndex) => { UpgradeDialogCallbackHandler(buttonIndex); });
	}

	public static void ShowLoginToFbDialog(Action onFacebookLogin)
	{
		DialogToolWrapper.OnFacebookLogin = onFacebookLogin;
		ActionSheetWrapper.actionSheet.Show2ButtonsDialog(LocalizationManager.Instance.GetString(string.Empty), LocalizationManager.Instance.GetString("login_to_fb_text"), LocalizationManager.Instance.GetString("cancel"), LocalizationManager.Instance.GetString("ok"), (buttonIndex) => { LoginToFbDialogCallbackHandler(buttonIndex); });
	}

	public static void ShowRateUsDialog(Action closeHandler)
	{
		AppData.AddNewRateUsView();
		DialogToolWrapper.RateUsDialogCloseHandler = closeHandler;
		ActionSheetWrapper.actionSheet.Show2ButtonsDialog(LocalizationManager.Instance.GetString("MyColorful"), LocalizationManager.Instance.GetString("applike") + "\r\n" + LocalizationManager.Instance.GetString("rateus"), LocalizationManager.Instance.GetString("no"), LocalizationManager.Instance.GetString("yes"), (buttonIndex) => { RateUsDialogCallbackHandler(buttonIndex); });
	}

	public static void ShowNoInternetDialog()
	{
		ActionSheetWrapper.actionSheet.ShowOneButtonDialog(LocalizationManager.Instance.GetString(string.Empty), LocalizationManager.Instance.GetString("connectionerror"), LocalizationManager.Instance.GetString("ok"));
	}

	public static void ShowNoAdsDialog()
	{
		ActionSheetWrapper.actionSheet.ShowOneButtonDialog(LocalizationManager.Instance.GetString(string.Empty), LocalizationManager.Instance.GetString("no_ads"), LocalizationManager.Instance.GetString("ok"));
	}

	public static void ShowActivityIndicator(bool show)
	{
	}

	public static void ShowDeleteDialog(Action<bool> callback)
	{
		DialogToolWrapper.DeleteDialogCloseHandler = callback;
		ActionSheetWrapper.actionSheet.Show2ButtonsDialog(LocalizationManager.Instance.GetString("delete_title"), LocalizationManager.Instance.GetString("delete_body"), LocalizationManager.Instance.GetString("cancel"), LocalizationManager.Instance.GetString("delete"), (buttonIndex) =>
		{
			DeleteDialogCallbackHandler(buttonIndex);
		});
	}

	//private static void Show2ButtonsDialogInternal(string title, string message, string firstText, string secondText, string callbackName)
	//{
	//    if (DialogToolWrapper.s_instance == null)
	//    {
	//        GameObject gameObject = new GameObject("DialogPluginListener");
	//        DialogToolWrapper.s_instance = gameObject.AddComponent<DialogToolWrapper>();
	//    }
	//    AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.my.DialogPlugin");
	//    androidJavaClass.CallStatic("ShowDialog", title, message, firstText, secondText, DialogToolWrapper.s_instance.name, callbackName);
	//}

	//private static void ShowOneButtonDialogInternal(string title, string message, string buttonText)
	//{
	//    AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.my.DialogPlugin");
	//    androidJavaClass.CallStatic("ShowOneButtonDialog", title, message, buttonText);
	//}

	public static void UpgradeDialogCallbackHandler(int buttonIndexS)
	{
		if (buttonIndexS == 1)
		{
			NewInappsWindow newInappsWindow = WindowManager.Instance.OpenInappsWindow();
			newInappsWindow.Init(string.Empty, null);
		}
	}

	public static void LoginToFbDialogCallbackHandler(int buttonIndexS)
	{
		if (buttonIndexS == 1)
		{
			 
		}
	}

	public static void RateUsDialogCallbackHandler(int buttonIndexS)
	{
		if (buttonIndexS == 1)
		{
			AppData.AppRated = true;
			RateUsTool.OpenRateUs();
		}
		DialogToolWrapper.RateUsDialogCloseHandler.SafeInvoke();
	}

	public static void DeleteDialogCallbackHandler(int buttonIndexS)
	{
		DialogToolWrapper.DeleteDialogCloseHandler.SafeInvoke(buttonIndexS == 1);
	}
}


