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

public class CamShot : MonoBehaviour
{
	public int resWidth = 1000;

	public int resHeight = 1000;

	public Camera cam;

	private bool takeHiResShot;

	private Texture2D m_Texture;

	public event Action<byte[]> ColorPalleteComplete;

	public GameObject lights;

	private void Start()
	{
		this.m_Texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
	}

	public static string ScreenShotName(int width, int height)
	{
		return string.Format("{0}/Captures/Capture_{1}x{2}_{3}.png", Application.dataPath, width, height, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
	}

	public void TakeHiResShot()
	{
		//lights.SetActive(true);
		this.takeHiResShot = true;
		byte[] shot = FreeImageSaver.MakePngFromOurVirtualThingy(400, 400, 400, 0, this.cam, true);
		this.OnColorPalleteComplete(shot);
		//lights.SetActive(false);
	}

	private void OnColorPalleteComplete(byte[] shot)
	{
		Action<byte[]> colorPalleteComplete = this.ColorPalleteComplete;
		if (colorPalleteComplete != null)
		{
			colorPalleteComplete(shot);
		}
	}
}


