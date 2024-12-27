/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;

public class ImageWaiter : MonoBehaviour
{
	[SerializeField]
	private float m_timer;

	[SerializeField]
	private float m_time = 1f;

	[SerializeField]
	private Transform[] m_indicators;

	private void Update()
	{
		this.m_timer += Time.deltaTime;
		if (this.m_timer > 2f * this.m_time)
		{
			this.m_timer = 0f;
		}
		for (int i = 0; i < this.m_indicators.Length; i++)
		{
			float num = (this.m_timer - (float)i * this.m_time / 6f) / this.m_time;
			if (num < 0f)
			{
				num += 2f;
			}
			float num2;
			if (num < 1f)
			{
				num2 = Mathf.Lerp(0.3f, 1f, num);
			}
			else
			{
				num -= 1f;
				num2 = Mathf.Lerp(1f, 0.3f, num);
			}
			this.m_indicators[i].localScale = new Vector3(num2, num2, 1f);
		}
	}
}


