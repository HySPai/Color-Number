/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;

internal class ScreenFormatScaler : MonoBehaviour
{
	[SerializeField]
	private float m_forceScaleX = 1f;

	[SerializeField]
	private float m_forceScaleY = 1f;

	private void Awake()
	{
		bool flag = false;
		if (Screen.width > Screen.height)
		{
			if ((float)Screen.width / (float)Screen.height < 1.45f)
			{
				flag = true;
			}
		}
		else if ((float)Screen.height / (float)Screen.width < 1.45f)
		{
			flag = true;
		}
		if (flag)
		{
			base.transform.localScale = new Vector3(this.m_forceScaleX, this.m_forceScaleY, 1f);
		}
	}
}


