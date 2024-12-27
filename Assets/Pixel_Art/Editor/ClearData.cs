/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;
using UnityEditor;
using System.IO;

public class ClearData
{
	private static int imageIndex = 1;
	[MenuItem("Tools/Clear Data")]
	private static void NewMenuOption()
	{
		PlayerPrefs.DeleteAll();
	}

	[MenuItem("Tools/Take Screenshot")]
	private static void TakeScreenshot()
	{
		ScreenCapture.CaptureScreenshot("Screenshots/" + (imageIndex++) + ".png");
	}

	[MenuItem("Tools/Clear All Screenshots")]
	private static void ClearAllScreenshots()
	{
		foreach(var file in Directory.GetFiles("Screenshots"))
		{
			File.Delete(file);
		}
		imageIndex = 1;
	}
}
