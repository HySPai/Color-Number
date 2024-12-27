/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;
using UnityEngine.UI;

public class BackgroundTileController : MonoBehaviour
{
	private RectTransform m_rectTransform;

	private Rect m_rect = default(Rect);

	private Image m_image;

	[SerializeField]
	private float m_elementSize = 512f;

	private void Awake()
	{
		this.m_rectTransform = (RectTransform)base.transform;
		this.m_image = base.GetComponent<Image>();
		this.m_image.material = new Material(Shader.Find("Custom/TilingShader"));
	}

	private void Update()
	{
		Rect rect = this.m_rectTransform.rect;
		if (rect != this.m_rect)
		{
			this.m_rect = rect;
			this.m_image.material.mainTextureScale = new Vector2(rect.width / this.m_elementSize, rect.height / this.m_elementSize);
		}
	}
}


