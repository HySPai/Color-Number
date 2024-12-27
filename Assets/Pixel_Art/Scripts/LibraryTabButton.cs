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

public class LibraryTabButton : MonoBehaviour
{
	[SerializeField]
	private Image m_image;

	[SerializeField]
	private Sprite m_activeSprite;

	[SerializeField]
	private Sprite m_nonactiveSprite;

	[SerializeField]
	private Text m_text;

	[SerializeField]
	private Color m_activeColor = Color.white;

	[SerializeField]
	private Color m_nonactiveColor = Color.white;

	public void SetHighlighted(bool value)
	{
		this.m_image.sprite = ((!value) ? this.m_nonactiveSprite : this.m_activeSprite);
		this.m_text.color = ((!value) ? this.m_nonactiveColor : this.m_activeColor);
	}
}


