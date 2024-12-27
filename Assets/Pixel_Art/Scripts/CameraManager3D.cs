/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using System.Collections;
using UnityEngine;

public class CameraManager3D : MonoBehaviour
{
	[SerializeField]
	private RectTransform m_canvasRectTransform;

	[SerializeField]
	private Camera m_camera;

	[SerializeField]
	private RectTransform m_cameraSpace;

	public void Init()
	{
		base.StartCoroutine(this.DefferedUpdate());
	}

	private IEnumerator DefferedUpdate()
	{
		float num = 0f;
		RectTransform rectTransform = this.m_cameraSpace;
		while (true)
		{
			if (!(rectTransform != null))
			{
				break;
			}
			if (!(rectTransform.name != "Canvas"))
			{
				break;
			}
			float num2 = num;
			Vector2 anchoredPosition = rectTransform.anchoredPosition;
			num = num2 + anchoredPosition.y;
			rectTransform = (RectTransform)rectTransform.parent;
		}
		float num3 = 0f - num;
		Vector2 sizeDelta = this.m_cameraSpace.sizeDelta;
		float y = 1f - (num3 + sizeDelta.y) / this.m_canvasRectTransform.rect.height;
		Vector2 sizeDelta2 = this.m_cameraSpace.sizeDelta;
		float height = sizeDelta2.y / this.m_canvasRectTransform.rect.height;
		this.m_camera.rect = new Rect(0f, y, 1f, height);
		yield break;
	}
}


