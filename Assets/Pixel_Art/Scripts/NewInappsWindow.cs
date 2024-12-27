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
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class NewInappsWindow : BaseWindow
{
	public Action OnPurchase;

	private bool restoredAnything;

	[SerializeField]
	private GameObject m_mainPart;

	[SerializeField]
	private NewInappsWindowElement m_prefab;

	[SerializeField]
	private NewInappsWindowElement m_trialInappsElement;

	[SerializeField]
	private Transform m_elementsParent;

	[SerializeField]
	private List<NewInappsWindowElement> m_elements;

	[SerializeField]
	private GameObject m_resultPart;

	[SerializeField]
	private Text m_resultTitle;

	[SerializeField]
	private Text m_resultText;

	[SerializeField]
	private Image m_resultIcon;

	[SerializeField]
	private Sprite m_goodResultSprite;

	[SerializeField]
	private Sprite m_badResultSprite;

	[SerializeField]
	private GameObject m_closeButton;

	[SerializeField]
	private Popup m_popup;

	[SerializeField]
	private GameObject m_waiter;

	[SerializeField]
	private ScrollRect m_textScrollRect;

	[SerializeField]
	private Text m_detailsText;

	[SerializeField]
	private Text m_urlPrefab;

	private bool m_textInited;

	private bool m_waitPurchases;

	private Action m_closeHandler;

	private string m_place;

	private Product[] _products;

	protected override string WindowName
	{
		get
		{
			return "inappsWindow";
		}
	}

	public void Init(string place, Action closeHandler = null)
	{
		this.m_place = place;
		this.m_closeHandler = closeHandler;
		Product[] products = IAPWrapper.Instance.GetProducts();
		if (products != null)
		{
			this.InitProducts(products);
		}
		else
		{
			this.m_trialInappsElement.gameObject.SetActive(false);
			this.m_waiter.SetActive(true);
			this.m_waitPurchases = true;
		}
		this.m_mainPart.SetActive(true);
		this.m_resultPart.SetActive(false);
		this.Open();
		this.InitTexts();
		AnalyticsManager.Instance.InappsWindowOpened(this.m_place);
	}

	private void InitTexts()
	{
		if (!this.m_textInited)
		{
			this.m_textInited = true;
			base.StartCoroutine(this.SetContentSizeCoroutine());
			this.m_textScrollRect.verticalNormalizedPosition = 1f;
			string text = LocalizationManager.Instance.GetString("details_text_android");
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
					Text text3 = UnityEngine.Object.Instantiate(this.m_detailsText);
					text3.transform.SetParent(this.m_detailsText.transform.parent);
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
					text6.transform.SetParent(this.m_detailsText.transform.parent);
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
		}
	}

	private void Update()
	{
		if (this.m_waitPurchases && IAPWrapper.Instance.GetProducts() != null)
		{
			this.m_waitPurchases = false;
			this.InitProducts(IAPWrapper.Instance.GetProducts());
		}
	}

	private void InitProducts(Product[] products)
	{
		this._products = products;
		this.m_waitPurchases = false;
		this.m_waiter.SetActive(false);
		int i = 0;
		for (int j = 0; j < products.Length; j++)
		{
			if (products[j].availableToPurchase && products[j].definition.type == ProductType.Subscription)
			{
				Product product = products[j];
				if (i >= this.m_elements.Count)
				{
					NewInappsWindowElement newInappsWindowElement = null; 
					newInappsWindowElement = ((!(products[j].definition.id != SubscriptionType.one_week_subscription.ToString())) ? this.m_trialInappsElement : UnityEngine.Object.Instantiate(this.m_prefab));
					newInappsWindowElement.transform.SetParent(this.m_elementsParent);
					newInappsWindowElement.transform.localScale = Vector2.one;
					newInappsWindowElement.transform.localRotation = Quaternion.identity;
					NewInappsWindowElement newInappsWindowElement2 = newInappsWindowElement;
					newInappsWindowElement2.OnSubscribe = (Action<Product>)Delegate.Combine(newInappsWindowElement2.OnSubscribe, new Action<Product>(this.OnSubscribeHandler));
					this.m_elements.Add(newInappsWindowElement);
				}
				this.m_elements[i].gameObject.SetActive(true);
				if (products[j].definition.id == SubscriptionType.one_week_subscription.ToString())
				{
					this.m_elements[i++].InitAsTrial(product);
				}
				else
				{
					this.m_elements[i++].Init(product);
				}
			}
		}
		for (; i < this.m_elements.Count; i++)
		{
			this.m_elements[i].gameObject.SetActive(false);
		}
	}

	private void OnSubscribeHandler(Product product)
	{
		SubscriptionType subscriptionType = (SubscriptionType)Enum.Parse(typeof(SubscriptionType), product.definition.id);
		AnalyticsManager.Instance.SubscriptionClick(subscriptionType);
		IAPWrapper.Instance.BuyProduct(subscriptionType, this.m_place, delegate (bool res, SubscriptionType type, PurchaseFailureReason reason)
		{
			if (!res && reason == PurchaseFailureReason.UserCancelled)
			{
				return;
			}
			if (res)
			{
				this.Init(this.m_place, this.m_closeHandler);
				this.OnPurchase.SafeInvoke();
				this.m_closeButton.SetActive(true);
				this.CloseButtonClick();
			}
			else
			{
				this.m_mainPart.SetActive(false);
				this.m_resultPart.SetActive(true);
				this.m_resultTitle.text = LocalizationManager.Instance.GetString((!res) ? "oops" : "thanks");
				this.m_resultText.text = LocalizationManager.Instance.GetString((!res) ? "BuyError" : "BuyConfirmation");
				this.m_resultIcon.sprite = ((!res) ? this.m_badResultSprite : this.m_goodResultSprite);
			}
		});
		AudioManager.Instance.PlayClick();
		switch (subscriptionType)
		{
			case SubscriptionType.one_month_subscription:
				AnalyticsManager.Instance.InappsWindowAction("month", this.m_place);
				break;
			case SubscriptionType.one_week_subscription:
				AnalyticsManager.Instance.InappsWindowAction("week", this.m_place);
				break;
			case SubscriptionType.one_year_subscription:
				AnalyticsManager.Instance.InappsWindowAction("year", this.m_place);
				break;
		}
	}

	public void TermsButtonClick()
	{
		Application.OpenURL(AppPathsConfig.TermsUrl);
	}

	public void PrivacyButtonClick()
	{
		Application.OpenURL(AppPathsConfig.PrivacyUrl);
	}

	public void CloseButtonClick()
	{
		WindowManager.Instance.CloseMe(this);
		AnalyticsManager.Instance.BackButtonClicked();
		AudioManager.Instance.PlayClick();
		AnalyticsManager.Instance.InappsWindowAction("close", this.m_place);
	}

	public void DetailsButtonClick()
	{
		DetailsWindow detailsWindow = WindowManager.Instance.OpenDetailsWindow();
		detailsWindow.Init(this._products);
		AudioManager.Instance.PlayClick();
	}

	public override bool Close()
	{
		if (this.m_closeButton.activeSelf)
		{
			AnalyticsManager.Instance.InappsWindowClosed();
			this.m_closeHandler.SafeInvoke();
			return base.Close();
		}
		return false;
	}

	public void RestoreButtonClick()
	{
		IAPWrapper instance = IAPWrapper.Instance;
		instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Combine(instance.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
		base.StartCoroutine(this.WaitForRestored());
		IAPWrapper.Instance.RestorePurchases(null);
		AnalyticsManager.Instance.RestoreButtonClicked();
		AudioManager.Instance.PlayClick();
		AnalyticsManager.Instance.InappsWindowAction("restore", this.m_place);
	}

	private void OnPurchaseHandler(bool res, SubscriptionType type)
	{
		this.restoredAnything = true;
		IAPWrapper instance = IAPWrapper.Instance;
		instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Remove(instance.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
	}
	private IEnumerator SetContentSizeCoroutine()
	{
		yield return null;
		this.m_textScrollRect.gameObject.SetActive(false);
		this.m_textScrollRect.verticalNormalizedPosition = 1f;
	}
	private IEnumerator WaitForProductsCoroutine()
	{
		while (IAPWrapper.Instance.GetProducts() == null)
		{
			yield return new WaitForSeconds(2f); 
		}
		this.Init(this.m_place, this.m_closeHandler);
	}
	private IEnumerator WaitForRestored()
	{
		float timer = 2f;
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			yield return null;
		}
		IAPWrapper instance = IAPWrapper.Instance;
		instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Remove(instance.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
		if (this.restoredAnything)
		{
			if (IAPWrapper.Instance.Subscribed)
			{
				AnalyticsManager.Instance.PurchasesRestored();
				this.m_popup.Show(LocalizationManager.Instance.GetString("restored1"));
			}
			else
			{
				this.m_popup.Show(LocalizationManager.Instance.GetString("restored2"));
			}
		}
		else
		{
			this.m_popup.Show(LocalizationManager.Instance.GetString("restored0"));
		}
	}
}

