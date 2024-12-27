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

public class BulbButton : MonoBehaviour
{
	[SerializeField]
	private GameObject m_highlightedGrid;

	[SerializeField]
	private Image m_image;

	[SerializeField]
	private Sprite m_enableSprite;

	[SerializeField]
	private Sprite m_disableSprite;

	private void Start()
	{
		this.UpdateState();
	}

	public void Click()
	{
		AppData.BulbMode = !AppData.BulbMode;
		this.UpdateState();
	}

	private void UpdateState()
	{
		this.m_image.sprite = ((!AppData.BulbMode) ? this.m_disableSprite : this.m_enableSprite);
		this.m_highlightedGrid.SetActive(AppData.BulbMode);
	}
}


