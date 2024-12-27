/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;
public static class GameHelper
{
	public static Texture2D Rotate(Texture2D originalTexture, bool clockwise)
	{
		Color32[] pixels = originalTexture.GetPixels32();
		Color32[] colors = new Color32[pixels.Length];
		int width = originalTexture.width;
		int height = originalTexture.height;
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				int destIndex = (j + 1) * height - i - 1;
				int sourceIndex = (!clockwise) ? (i * width + j) : (pixels.Length - 1 - (i * width + j));
				colors[destIndex] = pixels[sourceIndex];
			}
		}
		Texture2D texture2D = new Texture2D(height, width);
		texture2D.SetPixels32(colors);
		texture2D.Apply();
		return texture2D;
	}

	public static void CamScaleTexture(Texture2D texture, int height, float ratio, float quality)
	{
		TextureScale.Point(texture, (int)((float)height * ratio * quality), (int)((float)height * quality));
	}
}
