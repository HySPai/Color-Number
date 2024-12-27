/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
	private float deltaTime;

	private void Update()
	{
		if (VoxConstants.isTest)
		{
			this.deltaTime += (Time.unscaledDeltaTime - this.deltaTime) * 0.1f;
		}
	}

	private void OnGUI()
	{
		if (VoxConstants.isTest)
		{
			int width = Screen.width;
			int height = Screen.height;
			GUIStyle gUIStyle = new GUIStyle();
			Rect position = new Rect(0f, 0f, (float)width, (float)(height * 2 / 100));
			gUIStyle.alignment = TextAnchor.UpperLeft;
			gUIStyle.fontSize = height * 2 / 100;
			gUIStyle.normal.textColor = new Color(0f, 0f, 0.5f, 1f);
			float num = this.deltaTime * 1000f;
			float num2 = 1f / this.deltaTime;
			string text = string.Format("{0:0.0} ms ({1:0.} fps)", num, num2);
			GUI.Label(position, text, gUIStyle);
		}
	}
}


