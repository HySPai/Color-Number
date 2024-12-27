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

[RequireComponent(typeof(Text))]
[DisallowMultipleComponent]
public class FontSwitcher : MonoBehaviour
{
	private Text m_text;

	private void Awake()
	{
		this.m_text = base.GetComponent<Text>();
		switch (LocalizationManager.Instance.CurrentLanguage)
		{
			case SystemLanguage.Japanese:
			case SystemLanguage.Korean:
			case SystemLanguage.Thai:
			case SystemLanguage.Vietnamese:
			case SystemLanguage.ChineseSimplified:
			case SystemLanguage.ChineseTraditional:
				this.m_text.font = (Resources.Load("MyriadPro-Regular-dynamic") as Font);
				break;
			case SystemLanguage.Arabic:
				this.m_text.font = (Resources.Load("MyriadPro-Regular-dynamic") as Font);
				break;
		}
	}

	[ContextMenu("ChangeFont")]
	private void ChangeFont()
	{
		this.m_text = base.GetComponent<Text>();
		if (!this.m_text.font.name.Contains("helvetica") && !this.m_text.font.name.Contains("HELVETICA"))
		{
			return;
		}
		this.m_text.font = (Resources.Load("RobotoCondensed-Regular") as Font);
	}
}


