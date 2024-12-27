/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;

public class OrientatableCamera : MonoBehaviour
{
	[SerializeField]
	private Canvas m_canvas;

	[SerializeField]
	private float m_uiSize;

	[SerializeField]
	private float m_topUiSize;

	[SerializeField]
	private BannerSpace m_topPortraitOffset;

	private Camera m_mainCamera;

	public Rect CameraRect { get; private set; }

	private void Awake()
	{
		this.m_mainCamera = base.GetComponent<Camera>();
		this.OnOrientationChangedHandler(ScreenOrientation.Portrait);
	}

	private void OnOrientationChangedHandler(ScreenOrientation orientation)
	{
		Rect rect = (this.m_canvas.transform as RectTransform).rect;
		float num = Mathf.Max(rect.width, rect.height);
		switch (orientation)
		{
			case ScreenOrientation.LandscapeLeft:
			case ScreenOrientation.LandscapeRight:
				{
					float b2 = this.m_uiSize / num + 0.005f;
					b2 = Mathf.Max(0.01f, b2);
					this.CameraRect = new Rect(0f, 0f, 1f - b2, 1f);
					break;
				}
			case ScreenOrientation.Portrait:
			case ScreenOrientation.PortraitUpsideDown:
				{
					float b = this.m_uiSize / num - 0.005f;
					b = Mathf.Max(0.01f, b);
					float num2 = 0f;
					num2 = this.m_topUiSize / num;
					this.CameraRect = new Rect(0f, b, 1f, 1f - b - num2);
					break;
				}
		}
	}
}


