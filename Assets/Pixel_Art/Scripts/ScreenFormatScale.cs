/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;

public class ScreenFormatScale : MonoBehaviour
{
	[SerializeField]
	private float m_9x16Scale = 1f;

	[SerializeField]
	private float m_3x4Scale = 1f;

	private void Start()
	{
		float num = (float)Screen.height / (float)Screen.width;
		float num2 = Mathf.Lerp(this.m_3x4Scale, this.m_9x16Scale, (num - 1.33f) / 0.449999928f);
		base.transform.localScale = new Vector3(num2, num2, 1f);
	}
}


