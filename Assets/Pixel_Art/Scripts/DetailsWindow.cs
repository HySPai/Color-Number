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
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class DetailsWindow : BaseWindow
{
	private bool m_inited;

	[SerializeField]
	private ScrollRect m_scrollRect;

	[SerializeField]
	private Text m_detailsText;

	[SerializeField]
	private Text m_urlPrefab;

	[SerializeField]
	private Text m_textPrefab;

	[SerializeField]
	private HorizontalLayoutGroup m_horizontalGroupPrefab;

	[SerializeField]
	private Transform m_logo;

	private string _price1;

	private string _price2;

	private string _price3;

	protected override string WindowName
	{
		get
		{
			return "detailsWindow";
		}
	}

	public void Init(Product[] _products)
	{
		if (_products.Count() > 0)
		{
			this._price1 = _products[0].metadata.localizedPriceString;
		}
		if (_products.Count() >= 1)
		{
			this._price2 = _products[1].metadata.localizedPriceString;
		}
		if (_products.Count() >= 2)
		{
			this._price3 = _products[2].metadata.localizedPriceString;
		}
		this.Init();
	}

	public void Init()
	{
		if (!this.m_inited)
		{
			this.m_inited = true;
			base.StartCoroutine(this.SetContentSizeCoroutine());
			this.m_scrollRect.verticalNormalizedPosition = 1f;
			string @string = LocalizationManager.Instance.GetString("details_text_android");
			@string = string.Format(@string, this._price1, this._price2, this._price3);
			MatchCollection matchCollection = Regex.Matches(@string, "(http|ftp|https)://([\\w_-]+(?:(?:\\.[\\w_-]+)+))([\\w.,@?^=%&:/~+#-]*[\\w@?^=%&/~+#-])?");
			for (int num = matchCollection.Count - 1; num >= 0; num--)
			{
				int index = matchCollection[num].Index;
				@string = @string.Insert(index + matchCollection[num].Length, "</a>");
				@string = ((@string[index - 1] == '\n') ? @string.Insert(index, "<a>") : @string.Insert(index, "\n<a>"));
			}
			string[] array = @string.Split(new string[1] {
				"\n"
			}, StringSplitOptions.RemoveEmptyEntries);
			for (int num2 = 0; num2 < array.Length; num2++)
			{
				string text = string.Empty;
				 
				if (num2 < array.Length && !array[num2].Contains("<a>"))
				{
					text = text + array[num2] + "\n";
				}
				if (!string.IsNullOrEmpty(text))
				{
					Text text2 = UnityEngine.Object.Instantiate(this.m_detailsText);
					text2.transform.SetParent(this.m_detailsText.transform.parent);
					text2.transform.localScale = Vector2.one;
					text2.gameObject.SetActive(true);
					if (text.Contains("<center>"))
					{
						text2.alignment = TextAnchor.UpperCenter;
						text = text.Replace("<center>", string.Empty).Replace("</center>", string.Empty);
					}
					string text3 = text;
					char[] trimChars = new char[1] {
						'\n'
					};
					text = (text2.text = text3.Trim(trimChars).Replace("<br>", "\n"));
					if (LocalizationManager.Instance.CurrentLanguage == SystemLanguage.Arabic)
					{
						text2.alignment = TextAnchor.MiddleRight;
					}
				}
				if (array[num2].Contains("<a>"))
				{
					Match match = Regex.Match(array[num2], "(?<text>[^~]*)<a>(?<url>[^~]+)</a>");
					Text text5 = UnityEngine.Object.Instantiate(this.m_urlPrefab);
					text5.transform.SetParent(this.m_detailsText.transform.parent);
					text5.transform.localScale = Vector2.one;
					text5.gameObject.SetActive(true);
					string url = match.Groups["url"].Value;
					if (url.Contains("<center>"))
					{
						text5.alignment = TextAnchor.UpperCenter;
						url = url.Replace("<center>", string.Empty).Replace("</center>", string.Empty);
					}
					text5.text = url;
					((Component)text5).GetComponent<Button>().onClick.AddListener(delegate
					{
						Application.OpenURL(url);
					});
				}
			}
			this.m_logo.SetAsLastSibling();
		}
	}

	private IEnumerator SetContentSizeCoroutine()
	{
		yield return null;
		this.m_scrollRect.gameObject.SetActive(false);
		this.m_scrollRect.gameObject.SetActive(true);
		this.m_scrollRect.verticalNormalizedPosition = 1f;
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
}
