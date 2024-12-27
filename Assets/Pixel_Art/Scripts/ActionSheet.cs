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

public class ActionSheetWrapper : MonoBehaviour
{
	public static ActionSheetUI actionSheet;

	public static void ShowSavedWorkActionSheet(Action<ActionSheetResult> callback)
	{
		actionSheet.ShowButtons(new string[] {
			LocalizationManager.Instance.GetString("continue"),
			LocalizationManager.Instance.GetString("new"),
			LocalizationManager.Instance.GetString("delete"),
			LocalizationManager.Instance.GetString("cancel")
		}, (buttonIndex) =>
		{
			if (buttonIndex == 0)
				callback(ActionSheetResult.Continue);
			else if (buttonIndex == 1)
				callback(ActionSheetResult.New);
			else if (buttonIndex == 2)
				callback(ActionSheetResult.Delete);
			else if (buttonIndex == 3)
				callback(ActionSheetResult.Cancel);
		});
	}

	public static void ShowEmptyPhotoActionSheet(Action<ActionSheetResult> callback)
	{
        actionSheet.ShowButtons(new string[] {
           // LocalizationManager.Instance.GetString("continue"),
			LocalizationManager.Instance.GetString("new"),
			LocalizationManager.Instance.GetString("delete"),
			LocalizationManager.Instance.GetString("cancel")
		}, (buttonIndex) =>
		{
			//if (buttonIndex == 0)
			//	callback(ActionSheetResult.Continue);
			if (buttonIndex == 0)
				callback(ActionSheetResult.New);
			else if (buttonIndex == 1)
				callback(ActionSheetResult.Delete);
			else if (buttonIndex == 2)
				callback(ActionSheetResult.Cancel);
		});
	}

	public static void ShowImagePreviewActionSheet(Action<ActionSheetResult> callback)
	{
		actionSheet.ShowButtons(new string[] {
			LocalizationManager.Instance.GetString("continue"),
			LocalizationManager.Instance.GetString("new"),
			LocalizationManager.Instance.GetString("cancel")
		}, (buttonIndex) =>
		{
			if (buttonIndex == 0)
				callback(ActionSheetResult.Continue);
			else if (buttonIndex == 1)
				callback(ActionSheetResult.New);
			else if (buttonIndex == 2)
				callback(ActionSheetResult.Cancel);
		});
	}
}


