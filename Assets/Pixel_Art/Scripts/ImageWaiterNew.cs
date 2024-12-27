/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;

public class ImageWaiterNew : MonoBehaviour
{
	private float m_angle;

	[SerializeField]
	private Transform m_image;

	private void Update()
	{
		if (this.m_angle <= 10f)
		{
			this.m_angle += 360f;
		}
		if (this.m_angle < 90f)
		{
			this.m_angle -= Mathf.Min(0.05f, Time.deltaTime) * this.m_angle * 3f;
		}
		else
		{
			this.m_angle -= Mathf.Min(0.05f, Time.deltaTime) * 270f;
		}
		Quaternion rotation = this.m_image.rotation;
		rotation.eulerAngles = new Vector3(0f, 0f, this.m_angle);
		this.m_image.rotation = rotation;
	}
}


