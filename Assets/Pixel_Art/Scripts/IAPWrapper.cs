/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPWrapper : MonoBehaviour, IStoreListener
{
    public string AndroidRemoveAds = "pixelart_remove_ads";
    public string IosRemoveAds = "pixelart_remove_ads";
    public string AndroidWeekSubscription = "subscription_one_wweek";
    public string IosWeekSubscription = "subscription_one_wweek";
    public string AndroidMonthSubscription = "subscription_one_month";
    public string IosMonthSubscription = "subscription_one_month";
    public string AndroidYearSubscription = "subscription_one_year";
    public string IosYearSubscription = "subscription_one_year";

    private static IStoreController m_StoreController;

    private static IExtensionProvider m_StoreExtensionProvider;

    public Action<bool, SubscriptionType> OnPurchase;

    private Action<bool, SubscriptionType, PurchaseFailureReason> CurrentPurchaseHandler;

    private string m_nativeFailReason = string.Empty;

    private SubscriptionType m_currentRequestedProduct;

    private float m_responseTimer;

    private string m_place = string.Empty;

    private bool m_testBuy;

    public static IAPWrapper Instance { get; private set; }

    public static int SpecialOfferDurationHours
    {
        get
        {
            return 1;
        }
    }

    public static int MegaSaleDurationHours
    {
        get
        {
            return 24;
        }
    }

    public bool Subscribed
    {
        get
        {
            if (IAPWrapper.m_StoreController != null)
            {
                return (from b in IAPWrapper.m_StoreController.products.all
                        where b.definition.type == ProductType.Subscription
                        select b).Any((Product a) => a.hasReceipt);
            }
            return false;
        }
    }

    public bool NoAds
    {
        get
        {
            return (this.IsInitialized() && IAPWrapper.m_StoreController.products.all.Any((Product a) => a.hasReceipt)) || AppData.NoAds;
        }
    }

    public string CurrentSubscription
    {
        get;
        protected set;
    }

    public int CurrentSubscriptionPeriodIndex
    {
        get;
        protected set;
    }

    private void Awake()
    {
        IAPWrapper.Instance = this;
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        this.Init();
    }
    public void Init()
    {
        if (!this.IsInitialized())
        {
            this.CurrentSubscription = string.Empty;
            this.CurrentSubscriptionPeriodIndex = 0;
            //DataManager.Instance.GetIapSettings(delegate (IapSettings settings)
            //{
            //	if (settings != null)
            //	{
#if !NOIAP
            ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            configurationBuilder.AddProduct(SubscriptionType.one_week_subscription.ToString(), ProductType.Subscription, new IDs {
                        {
                            AndroidWeekSubscription, GooglePlay.Name
                        }
#if UNITY_IOS
				,
                        {
                            IosWeekSubscription,
                            new string[1] {
                                "AppleAppStore"
                            }
                        }
#endif
					});
            configurationBuilder.AddProduct(SubscriptionType.one_month_subscription.ToString(), ProductType.Subscription, new IDs {
                        {
                            AndroidMonthSubscription, GooglePlay.Name
                        }
				
#if UNITY_IOS
				,
                        {
                            IosMonthSubscription,
                            new string[1] {
                                "AppleAppStore"
                            }
                        }
#endif
					});
            configurationBuilder.AddProduct(SubscriptionType.one_year_subscription.ToString(), ProductType.Subscription, new IDs {
                        {
                            AndroidYearSubscription, GooglePlay.Name
                        }
#if UNITY_IOS
				,
                        {
                            IosYearSubscription,
                            new string[1] {
                                "AppleAppStore"
                            }
                        }
#endif
					});
            configurationBuilder.AddProduct(SubscriptionType.remove_ads.ToString(), ProductType.NonConsumable, new IDs {
                        {
                            AndroidRemoveAds, GooglePlay.Name
                        }
                    });
            UnityPurchasing.Initialize(this, configurationBuilder);
#endif
            //}
            //}); 
        }
    }

    private bool IsInitialized()
    {
        return IAPWrapper.m_StoreController != null && IAPWrapper.m_StoreExtensionProvider != null;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        UnityEngine.Debug.Log("OnInitialized: PASS");
        IAPWrapper.m_StoreController = controller;
        IAPWrapper.m_StoreExtensionProvider = extensions;
        if (this.Subscribed)
        {
            this.OnPurchase.SafeInvoke(true, SubscriptionType.one_week_subscription);
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        UnityEngine.Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public Product[] GetProducts()
    {
        if (IAPWrapper.m_StoreController != null)
        {
            string[] enumNames = Enum.GetNames(typeof(SubscriptionType));
            return (from a in IAPWrapper.m_StoreController.products.all
                    where enumNames.Contains(a.definition.id)
                    select a).ToArray();
        }
        return null;
    }

    public void BuyProductStoreSpecific(string productId, string place, Action<bool, SubscriptionType, PurchaseFailureReason> handler)
    {
        Product product = IAPWrapper.m_StoreController.products.WithStoreSpecificID(productId);
        SubscriptionType subscrType = (SubscriptionType)Enum.Parse(typeof(SubscriptionType), product.definition.id);
        this.BuyProduct(subscrType, place, delegate (bool res, SubscriptionType type, PurchaseFailureReason failReason)
        {
            handler(res, type, failReason);
        });
    }

    public void BuyProduct(SubscriptionType subscrType, string place, Action<bool, SubscriptionType, PurchaseFailureReason> handler)
    {
        this.m_place = place;
        if (!InternetConnection.IsAvailable)
        {
            handler.SafeInvoke(false, SubscriptionType.one_week_subscription, PurchaseFailureReason.Unknown);
            DialogToolWrapper.ShowNoInternetDialog();
        }
        else
        {
            string id = subscrType.ToString();
            try
            {
                if (this.IsInitialized())
                {
                    Product product = IAPWrapper.m_StoreController.products.WithID(id);
                    if (product != null && product.availableToPurchase)
                    {
                        UnityEngine.Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                        IAPWrapper.m_StoreController.InitiatePurchase(product);
                        this.CurrentPurchaseHandler = handler;
                        UnityEngine.Debug.Log("ShowActivityIndicator");
                        DialogToolWrapper.ShowActivityIndicator(true);
                        base.StartCoroutine(this.ActivityIndicatorWaiter());
                        this.m_currentRequestedProduct = subscrType;
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
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (args.purchasedProduct == null || args.purchasedProduct.definition == null)
        {
            this.OnPurchase.SafeInvoke(true, SubscriptionType.one_week_subscription);
            this.InvokeCurrentPurchaseHandler(true, SubscriptionType.one_week_subscription, PurchaseFailureReason.Unknown);
        }
        else
        {
            if (args.purchasedProduct.definition.id == SubscriptionType.remove_ads.ToString())
            {
                AppData.NoAds = true;
            }
            SubscriptionType subscriptionType = (SubscriptionType)Enum.Parse(typeof(SubscriptionType), args.purchasedProduct.definition.id);
            this.OnPurchase.SafeInvoke(true, subscriptionType);
            this.InvokeCurrentPurchaseHandler(true, subscriptionType, PurchaseFailureReason.Unknown);
            if (!AppData.Purchases.Contains(args.purchasedProduct.transactionID))
            {
                AnalyticsManager.Instance.InappPurchaseSuccess(args.purchasedProduct, this.m_place, this.m_responseTimer);
                try
                {
                    List<string> purchases = AppData.Purchases;
                    purchases.Add(args.purchasedProduct.transactionID);
                    AppData.Purchases = purchases;
                }
                catch
                {
                }
            }

        }
        this.CurrentPurchaseHandler = null;
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        this.OnPurchaseFailedProcess(product, failureReason);
    }

    private void OnPurchaseFailedProcess(Product product, PurchaseFailureReason failureReason)
    {
        UnityEngine.Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        SubscriptionType subscriptionType = (SubscriptionType)Enum.Parse(typeof(SubscriptionType), product.definition.id);
        this.OnPurchase.SafeInvoke(false, subscriptionType);
        this.InvokeCurrentPurchaseHandler(false, subscriptionType, failureReason);
        if (!AppData.Purchases.Contains(product.transactionID))
        {
            AnalyticsManager.Instance.InappPurchaseFailed(subscriptionType, this.m_place, this.m_responseTimer, failureReason.ToString(), this.m_nativeFailReason);
            try
            {
                List<string> purchases = AppData.Purchases;
                purchases.Add(product.transactionID);
                AppData.Purchases = purchases;
            }
            catch
            {
            }
        }
        this.CurrentPurchaseHandler = null;
    }

    public void ReceiveFailReason(string s)
    {
        this.m_nativeFailReason = s;
    }

    public void RestorePurchases(Action<bool> handler)
    {
        if (!this.IsInitialized())
        {
            handler.SafeInvoke(false);
            UnityEngine.Debug.Log("RestorePurchases FAIL. Not initialized.");
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            UnityEngine.Debug.Log("RestorePurchases started ...");

#if !NOIAP
			IAPWrapper.m_StoreExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(delegate (bool res)
			{
				handler.SafeInvoke(res);
				UnityEngine.Debug.Log("RestorePurchases continuing: " + res + ". If no further messages, no purchases available to restore.");
			});
#endif
		}
		else
		{
			UnityEngine.Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
			handler.SafeInvoke(false);
		}
	}

	private void InvokeCurrentPurchaseHandler(bool result, SubscriptionType type, PurchaseFailureReason reason)
	{
		DialogToolWrapper.ShowActivityIndicator(false);
		this.CurrentPurchaseHandler.SafeInvoke(result, type, reason);
		this.CurrentPurchaseHandler = null;
	} 
	private IEnumerator WaitForNativeFailReason(Action handler)
	{
		for (float counter = 0.3f; this.m_nativeFailReason == string.Empty && counter > 0f; counter -= Time.deltaTime)
		{
			yield return null;
		}
		handler.SafeInvoke();
	}
	private IEnumerator ActivityIndicatorWaiter()
	{
		this.m_responseTimer = 0f;
		if (this.m_responseTimer < 120f)
		{
			if (this.CurrentPurchaseHandler == null)
			{
				DialogToolWrapper.ShowActivityIndicator(false);
				yield break;
			}
			this.m_responseTimer += Time.deltaTime;
			yield return null;
		}
		this.CurrentPurchaseHandler.SafeInvoke(false, SubscriptionType.one_week_subscription, PurchaseFailureReason.Unknown);
		DialogToolWrapper.ShowActivityIndicator(false);
		AnalyticsManager.Instance.InappPurchaseFailed(this.m_currentRequestedProduct, this.m_place, this.m_responseTimer, "Timeout", string.Empty);
	}
	private IEnumerator PostRequest(string host, WWWForm wwwForm, Action<bool, string> handler)
	{
		var www = new WWW(host, wwwForm);
		yield return www;

		if (!string.IsNullOrEmpty(www.error))
		{
			UnityEngine.Debug.Log(www.error);
			handler.SafeInvoke(false, null);
		}
		else
		{ 
			if (www.text.Contains("Fatal error"))
			{
				handler.SafeInvoke(false, null);
			}
			else
			{
				handler.SafeInvoke(true, www.text);
			}
		}
	}
}
