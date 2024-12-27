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
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class NewInappsWindowElement : MonoBehaviour
{
	public Action<Product> OnSubscribe;

	private Product m_product;

	[SerializeField]
	private Text m_price;

	[SerializeField]
	private Text m_description;

	[SerializeField]
	private Text m_descriptionDescr;

	[SerializeField]
	private Image m_back;

	[SerializeField]
	private GameObject m_inMonth;

	public void Init(Product product)
	{
		this.m_product = product;
		SubscriptionType type = (SubscriptionType)Enum.Parse(typeof(SubscriptionType), product.definition.id);
		//DataManager.Instance.GetIapSettings(delegate (IapSettings settings)
		//{
		//if (type == SubscriptionType.one_year_subscription)
		//{
		//	string localizedPriceString = product.metadata.localizedPriceString;
		//	MatchCollection matchCollection = Regex.Matches(localizedPriceString, "[0-9,\\.]+([^~]*[0-9,\\.]+)*");
		//	string text = string.Empty;
		//	IEnumerator enumerator = matchCollection.GetEnumerator();
		//	try
		//	{
		//		while (enumerator.MoveNext())
		//		{
		//			Match match = (Match)enumerator.Current;
		//			string text2 = match.Value.Trim();
		//			if (text2.Length > text.Length)
		//			{
		//				text = text2;
		//			}
		//		}
		//	}
		//	finally
		//	{
		//		IDisposable disposable;
		//		if ((disposable = (enumerator as IDisposable)) != null)
		//		{
		//			disposable.Dispose();
		//		}
		//	}
		//	string oldValue = text.Trim('_').Replace("_", " ");
		//	float num = (float)product.metadata.localizedPrice / 12f;
		//	string text3 = product.metadata.localizedPriceString.Replace(oldValue, num.ToString("F"));
		//	this.m_price.text = text3;
		//	this.m_inMonth.SetActive(true);
		//}
		//else
		{
			this.m_price.text = product.metadata.localizedPriceString;
			this.m_inMonth.SetActive(false);
		}
		//});
		if (type != SubscriptionType.one_month_subscription)
		{
			if (type == SubscriptionType.one_year_subscription)
			{
				this.m_description.text = LocalizationManager.Instance.GetString("one_year_android");
			}
		}
		else
		{
			this.m_description.text = LocalizationManager.Instance.GetString("one_month_android");
		}
	}

	public void InitAsTrial(Product product)
	{
		this.m_product = product;
		string empty = string.Empty;
		this.m_description.text = LocalizationManager.Instance.GetString("trial_continue_android").ToUpper();
		empty = LocalizationManager.Instance.GetString("trial_descr_new");
		if (empty.Contains("{0}"))
		{
			empty = string.Format(empty, product.metadata.localizedPriceString);
		}
		this.m_descriptionDescr.text = empty;
	}

	public void ClickSubscribe()
	{
		this.OnSubscribe.SafeInvoke(this.m_product);
	}
}


