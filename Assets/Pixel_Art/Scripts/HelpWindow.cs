/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class HelpWindow : BaseWindow
{
	private bool m_inited;

	[SerializeField]
	private ScrollRect m_scrollRect;

	[SerializeField]
	private Text m_helpText;

	[SerializeField]
	private Text m_urlPrefab;

	[SerializeField]
	private Text m_textPrefab;

	[SerializeField]
	private HorizontalLayoutGroup m_horizontalGroupPrefab;

	[SerializeField]
	private Transform m_logo;

	[SerializeField]
	private SwipeReceiver m_swipeReceiver;

	protected override string WindowName
	{
		get
		{
			return "helpWindow";
		}
	}

	private void Awake()
	{
		SwipeReceiver swipeReceiver = this.m_swipeReceiver;
		swipeReceiver.OnSwipeRight = (Action)Delegate.Combine(swipeReceiver.OnSwipeRight, new Action(this.CloseButtonClick));
	}

	public void Init()
	{
		if (!this.m_inited)
		{
			this.m_inited = true;
			base.StartCoroutine(this.SetContentSizeCoroutine());
			this.m_scrollRect.verticalNormalizedPosition = 1f;
			string text = LocalizationManager.Instance.GetString("help_text");
			MatchCollection matchCollection = Regex.Matches(text, "(http|ftp|https)://([\\w_-]+(?:(?:\\.[\\w_-]+)+))([\\w.,@?^=%&:/~+#-]*[\\w@?^=%&/~+#-])?");
			for (int num = matchCollection.Count - 1; num >= 0; num--)
			{
				int index = matchCollection[num].Index;
				text = text.Insert(index + matchCollection[num].Length, "</a>");
				text = ((text[index - 1] == '\n') ? text.Insert(index, "<a>") : text.Insert(index, "\n<a>"));
			}
			string[] array = text.Split(new string[1] {
				"\n"
			}, StringSplitOptions.RemoveEmptyEntries);
			for (int num2 = 0; num2 < array.Length; num2++)
			{
				string text2 = string.Empty;
				 
				if (num2 < array.Length && !array[num2].Contains("<a>"))
				{
					text2 = text2 + array[num2] + "\n";
				}
				if (!string.IsNullOrEmpty(text2))
				{
					Text text3 = UnityEngine.Object.Instantiate(this.m_helpText);
					text3.transform.SetParent(this.m_helpText.transform.parent);
					text3.transform.localScale = Vector2.one;
					text3.gameObject.SetActive(true);
					if (text2.Contains("<center>"))
					{
						text3.alignment = TextAnchor.UpperCenter;
						text2 = text2.Replace("<center>", string.Empty).Replace("</center>", string.Empty);
					}
					string text4 = text2;
					char[] trimChars = new char[1] {
						'\n'
					};
					text2 = (text3.text = text4.Trim(trimChars).Replace("<br>", "\n"));
					if (LocalizationManager.Instance.CurrentLanguage == SystemLanguage.Arabic)
					{
						text3.alignment = TextAnchor.MiddleRight;
					}
				}
				if (array[num2].Contains("<a>"))
				{
					Match match = Regex.Match(array[num2], "(?<text>[^~]*)<a>(?<url>[^~]+)</a>");
					Text text6 = UnityEngine.Object.Instantiate(this.m_urlPrefab);
					text6.transform.SetParent(this.m_helpText.transform.parent);
					text6.transform.localScale = Vector2.one;
					text6.gameObject.SetActive(true);
					string url = match.Groups["url"].Value;
					if (url.Contains("<center>"))
					{
						text6.alignment = TextAnchor.UpperCenter;
						url = url.Replace("<center>", string.Empty).Replace("</center>", string.Empty);
					}
					text6.text = url;
					((Component)text6).GetComponent<Button>().onClick.AddListener(delegate
					{
						Application.OpenURL(url);
					});
				}
			}
			this.m_logo.SetAsLastSibling();
		}
	}

	public void CloseButtonClick()
	{
		WindowManager.Instance.CloseMe(this);
		AnalyticsManager.Instance.BackButtonClicked();
		AudioManager.Instance.PlayClick();
	}

	public void TermsAndConditionsButtonClick()
	{
	}

	private void OnDestroy()
	{
		SwipeReceiver swipeReceiver = this.m_swipeReceiver;
		swipeReceiver.OnSwipeRight = (Action)Delegate.Remove(swipeReceiver.OnSwipeRight, new Action(this.CloseButtonClick));
	}
	private IEnumerator SetContentSizeCoroutine()
	{
		yield return null;
		this.m_scrollRect.gameObject.SetActive(false);
		this.m_scrollRect.gameObject.SetActive(true);
		this.m_scrollRect.verticalNormalizedPosition = 1f;
	}
}
