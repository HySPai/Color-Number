/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CompositTranslatableText : MonoBehaviour
{
	[SerializeField]
	private bool m_autoTranslate = true;

	[SerializeField]
	private List<string> m_list;

	[SerializeField]
	private string m_format;

	[SerializeField]
	private bool forceUpper;

	private Text m_text;

	private BestFit m_bestFit;

	private void Start()
	{
		if (this.m_autoTranslate)
		{
			this.Translate();
		}
	}

	public void Translate()
	{
		this.m_text = base.GetComponent<Text>();
		this.m_bestFit = base.GetComponent<BestFit>();
		this.m_text.enabled = false;
		if (this.m_text.text != string.Empty)
		{
			string[] args = (from a in this.m_list
							 select LocalizationManager.Instance.GetString(a)).ToArray();
			string text = string.Format(this.m_format, args);
			this.m_text.text = ((!this.forceUpper) ? text : text.ToUpper());
		}
		if (this.m_bestFit != null)
		{
			this.m_bestFit.Refit();
		}
		this.m_text.enabled = true;
	}
}


