/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

public abstract class PlatformIapWrapper : MonoBehaviour, IStoreListener
{
	protected IStoreController m_StoreController;

	protected IExtensionProvider m_StoreExtensionProvider;

	protected string m_nativeFailReason = string.Empty;

	public abstract bool Subscribed
	{
		get;
	}

	public abstract bool NoAds
	{
		get;
	}

	public abstract string CurrentSubscription
	{
		get;
		protected set;
	}

	public abstract int CurrentSubscriptionPeriodIndex
	{
		get;
		protected set;
	}

	public abstract event Action<bool, SubscriptionType> OnPurchase;

	public event Action<bool, SubscriptionType, PurchaseFailureReason> CurrentPurchaseHandler;

	public virtual void Init()
	{
		if (this.CurrentSubscription == null)
		{
			this.CurrentSubscription = string.Empty;
		}
	}

	public bool IsInitialized()
	{
		return this.m_StoreController != null && this.m_StoreExtensionProvider != null;
	}

	public virtual void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		this.m_StoreController = controller;
		this.m_StoreExtensionProvider = extensions;
		this.OnPurchaseInvoke(this.Subscribed, SubscriptionType.one_week_subscription);
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		UnityEngine.Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}

	public Product[] GetProducts()
	{
		if (this.IsInitialized())
		{
			string[] enumNames = Enum.GetNames(typeof(SubscriptionType));
			return (from a in this.m_StoreController.products.all
					where enumNames.Contains(a.definition.id)
					select a).ToArray();
		}
		return null;
	}

	public void BuyProductStoreSpecific(string productId, Action<bool, SubscriptionType, PurchaseFailureReason> handler)
	{
		Product product = this.m_StoreController.products.WithStoreSpecificID(productId);
		SubscriptionType subscrType = (SubscriptionType)Enum.Parse(typeof(SubscriptionType), product.definition.id);
		this.BuyProduct(subscrType, delegate (bool res, SubscriptionType type, PurchaseFailureReason reason)
		{
			handler(res, type, reason);
		});
	}

	public virtual void BuyProduct(SubscriptionType subscrType, Action<bool, SubscriptionType, PurchaseFailureReason> handler)
	{
		string id = subscrType.ToString();
		try
		{
			if (this.IsInitialized())
			{
				Product product = this.m_StoreController.products.WithID(id);
				if (product != null && product.availableToPurchase)
				{
					UnityEngine.Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
					this.m_StoreController.InitiatePurchase(product);
					this.CurrentPurchaseHandler = handler;
				}
				else
				{
					UnityEngine.Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				}
			}
			else
			{
				UnityEngine.Debug.Log("BuyProductID FAIL. Not initialized.");
			}
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.Log("BuyProductID: FAIL. Exception during purchase. " + arg);
		}
	}

	public virtual PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		if (args.purchasedProduct == null || args.purchasedProduct.definition == null)
		{
			this.OnPurchaseInvoke(true, SubscriptionType.one_week_subscription);
			this.CurrentPurchaseHandler.SafeInvoke(true, SubscriptionType.one_week_subscription, PurchaseFailureReason.Unknown);
		}
		else
		{
			SubscriptionType subscriptionType = (SubscriptionType)Enum.Parse(typeof(SubscriptionType), args.purchasedProduct.definition.id);
			this.OnPurchaseInvoke(true, subscriptionType);
			this.CurrentPurchaseHandler.SafeInvoke(true, subscriptionType, PurchaseFailureReason.Unknown);
			if (!AppData.Purchases.Contains(args.purchasedProduct.transactionID))
			{
				AnalyticsManager.Instance.InappPurchaseSuccess(args.purchasedProduct, string.Empty, 0f);
				AppData.Purchases.Add(args.purchasedProduct.transactionID);
			}
		}
		this.CurrentPurchaseHandler = null;
		return PurchaseProcessingResult.Complete;
	}

	public virtual void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		UnityEngine.Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
		SubscriptionType subscriptionType = (SubscriptionType)Enum.Parse(typeof(SubscriptionType), product.definition.id);
		this.OnPurchaseInvoke(false, subscriptionType);
		this.CurrentPurchaseHandler.SafeInvoke(false, subscriptionType, failureReason);
		this.CurrentPurchaseHandler = null;
		AnalyticsManager.Instance.InappPurchaseFailed((SubscriptionType)Enum.Parse(typeof(SubscriptionType), product.definition.id), string.Empty, 0f, failureReason.ToString(), this.m_nativeFailReason);
	}

	public virtual void RestorePurchases(Action<bool> handler)
	{
		handler.SafeInvoke(false);
	}

	protected abstract void OnPurchaseInvoke(bool res, SubscriptionType subscrType);
}


