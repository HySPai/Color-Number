/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using UnityEngine;

public class ScreenSizer : MonoBehaviour
{
	private RectTransform m_canvas;

	[SerializeField]
	private float m_extends;

	[SerializeField]
	private bool m_onlyWidth;

	[SerializeField]
	private bool m_heightDependsOnWidth;

	[SerializeField]
	private float m_extraHeight;

	[SerializeField]
	private float m_extraHeight3x4 = -1f;

	private void Awake()
	{
		this.FindCanvas();
		if (this.m_canvas != null)
		{
			float num = Mathf.Min(this.m_canvas.rect.width, this.m_canvas.rect.height);
			float num2 = num;
			Vector2 sizeDelta = ((RectTransform)base.transform).sizeDelta;
			float y = sizeDelta.y;
			if (!this.m_onlyWidth)
			{
				y = this.m_canvas.rect.height;
				if (this.m_heightDependsOnWidth)
				{
					bool flag = false;
					if (Screen.width > Screen.height)
					{
						if ((float)Screen.width / (float)Screen.height < 1.55f)
						{
							flag = true;
						}
					}
					else if ((float)Screen.height / (float)Screen.width < 1.55f)
					{
						flag = true;
					}
					y = ((!flag || this.m_extraHeight3x4 != -1f) ? (num2 + this.m_extraHeight) : (num2 + this.m_extraHeight3x4));
				}
			}
			(base.transform as RectTransform).sizeDelta = new Vector2(num2 - this.m_extends * 2f, y);
			RectTransform obj = base.transform as RectTransform;
			Vector2 anchoredPosition = (base.transform as RectTransform).anchoredPosition;
			obj.anchoredPosition = new Vector2(0f, anchoredPosition.y);
		}
	}

	private void FindCanvas()
	{
		Transform transform = base.transform;
		while (true)
		{
			if (transform != null)
			{
				Canvas component = ((Component)transform).GetComponent<Canvas>();
				if (!(component != null))
				{
					transform = transform.parent;
					continue;
				}
				break;
			}
			return;
		}
		this.m_canvas = (transform as RectTransform);
	}
}


