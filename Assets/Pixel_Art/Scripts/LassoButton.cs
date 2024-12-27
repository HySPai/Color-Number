/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System;
using UnityEngine;
using UnityEngine.UI;

public class LassoButton : MonoBehaviour
{
	[SerializeField]
	private Image m_image;

	[SerializeField]
	private GameObject m_activePointer;

	[SerializeField]
	private Text m_lassosCountField;

	[SerializeField]
	private Text m_lassosCountFieldCopy;

	[SerializeField]
	private GameObject m_plus;

	[SerializeField]
	private Image m_plusBack;

	[SerializeField]
	private GameObject m_adText;

	private bool m_needUpdatePlusState;

	private void Start()
	{
		IAPWrapper instance = IAPWrapper.Instance;
		instance.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Combine(instance.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
		SpecBoostersModel specBoostersModel = WorkbookModel.Instance.SpecBoostersModel;
		specBoostersModel.OnLassoModeChanged = (Action<SpecBoostersModel>)Delegate.Combine(specBoostersModel.OnLassoModeChanged, new Action<SpecBoostersModel>(this.OnLassoModeChangedHandler));
		SpecBoostersModel specBoostersModel2 = WorkbookModel.Instance.SpecBoostersModel;
		specBoostersModel2.OnStateChanged = (Action<SpecBoostersModel>)Delegate.Combine(specBoostersModel2.OnStateChanged, new Action<SpecBoostersModel>(this.OnLassoCountChangedHandler));
		AdsWrapper instance2 = AdsWrapper.Instance;
		instance2.OnVideoAvailabilityChanged = (Action)Delegate.Combine(instance2.OnVideoAvailabilityChanged, new Action(this.OnVideoAvailabilityChangedHandler));
		this.OnLassoModeChangedHandler(WorkbookModel.Instance.SpecBoostersModel);
		this.OnLassoCountChangedHandler(WorkbookModel.Instance.SpecBoostersModel);
		this.OnVideoAvailabilityChangedHandler();
		this.UpdateAdTextState();
	}

	private void Update()
	{
		if (this.m_needUpdatePlusState)
		{
			if (IAPWrapper.Instance.Subscribed || IAPWrapper.Instance.NoAds)
			{
				this.m_plus.SetActive(false);
			}
			else
			{
				this.m_needUpdatePlusState = false;
				ABTestGroup abTestGroup = INPluginWrapper.Instance.GetAbTestGroup();
				if (abTestGroup == ABTestGroup.None || abTestGroup == ABTestGroup.RewardedNo_ContentMedium_Old || abTestGroup == ABTestGroup.RewardedNo_ContentMedium_Revealed || abTestGroup == ABTestGroup.RewardedNo_ContentEasy)
				{
					this.m_plus.SetActive(AdsWrapper.Instance.IsVideoAvailable());
				}
				else
				{
					this.m_plus.SetActive(true);
				}
			}
			this.UpdateAdTextState();
		}
	}

	private void OnLassoModeChangedHandler(SpecBoostersModel model)
	{
		this.m_activePointer.SetActive(model.LassoMode);
	}

	private void OnLassoCountChangedHandler(SpecBoostersModel model)
	{
		if (IAPWrapper.Instance.NoAds)
		{
			this.m_lassosCountField.gameObject.SetActive(false);
		}
		else
		{
			this.m_lassosCountField.text = model.LassoCount.ToString();
			this.m_lassosCountFieldCopy.text = model.LassoCount.ToString();
			this.OnVideoAvailabilityChangedHandler();
		}
	}

	private void OnVideoAvailabilityChangedHandler()
	{
		if (IAPWrapper.Instance.NoAds)
		{
			this.m_plus.gameObject.SetActive(false);
		}
		else
		{
			this.m_needUpdatePlusState = true;
		}
	}

	private void UpdateAdTextState()
	{
		switch (INPluginWrapper.Instance.GetAbTestGroup())
		{
			case ABTestGroup.None:
			case ABTestGroup.RewardedNo_ContentEasy:
			case ABTestGroup.RewardedNo_ContentMedium_Old:
			case ABTestGroup.RewardedNo_ContentMedium_Revealed:
				this.m_adText.SetActive(AdsWrapper.Instance.IsVideoAvailable());
				break;
			default:
				this.m_adText.SetActive(false);
				break;
		}
	}

	private void OnPurchaseHandler(bool res, SubscriptionType subscrType)
	{
		this.OnVideoAvailabilityChangedHandler();
		this.OnLassoCountChangedHandler(WorkbookModel.Instance.SpecBoostersModel);
	}

	public void Click()
	{
		if (IAPWrapper.Instance.NoAds || IAPWrapper.Instance.Subscribed)
		{
			while (WorkbookModel.Instance.SpecBoostersModel.LassoCount < 5)
			{
				WorkbookModel.Instance.SpecBoostersModel.AddLasso();
			}
		}
		if (WorkbookModel.Instance.SpecBoostersModel.LassoCount <= 0)
		{
			this.PlusClick();
		}
		else
		{
			WorkbookModel.Instance.SpecBoostersModel.ChangeLassoMode(false);
			if (WorkbookModel.Instance.SpecBoostersModel.LassoMode)
			{
				AnalyticsManager.Instance.LassoClicked(WorkbookModel.Instance.SpecBoostersModel.LassoCount);
			}
		}
	}

	public void PlusClick()
	{
		AnalyticsManager.Instance.LassoAddClick(WorkbookModel.Instance.SpecBoostersModel.LassoCount);
		if (!IAPWrapper.Instance.NoAds && !IAPWrapper.Instance.Subscribed)
		{
			switch (INPluginWrapper.Instance.GetAbTestGroup())
			{
				case ABTestGroup.None:
				case ABTestGroup.RewardedNo_ContentEasy:
				case ABTestGroup.RewardedNo_ContentMedium_Old:
				case ABTestGroup.RewardedNo_ContentMedium_Revealed:
					if (AdsWrapper.Instance.IsVideoAvailable())
					{
						AdsWrapper.Instance.ShowVideo("wand", delegate (bool res)
						{
							if (res)
							{
								WorkbookModel.Instance.SpecBoostersModel.AddLasso();
								WorkbookModel.Instance.SpecBoostersModel.ChangeLassoMode(true);
							}
						});
					}
					break;
				default:
					if (AdsWrapper.Instance.IsVideoAvailable())
					{
						IapPopup abWindow = WindowManager.Instance.OpenAbTestWindow();
						abWindow.Init(delegate (bool res)
						{
							if (res)
							{
								WorkbookModel.Instance.SpecBoostersModel.AddLasso();
								WorkbookModel.Instance.SpecBoostersModel.ChangeLassoMode(true);
								WindowManager.Instance.CloseMe(abWindow);
							}
						}, ABTestGroup.RewardedYes_ContentHard, AbTestWindowMode.Lasso);
					}
					else
					{
						var trialInappsWindow = WindowManager.Instance.OpenInappsWindow();
						trialInappsWindow.Init("wand", null);//, false, false);
					}
					break;
			}
		}
		else
		{
			WorkbookModel.Instance.SpecBoostersModel.AddLasso();
			WorkbookModel.Instance.SpecBoostersModel.ChangeLassoMode(true);
		}
	}

	public void CheatButtonClick()
	{
		WorkbookModel.Instance.SpecBoostersModel.AddLasso();
		WorkbookModel.Instance.SpecBoostersModel.AddLasso();
		WorkbookModel.Instance.SpecBoostersModel.AddLasso();
		WorkbookModel.Instance.SpecBoostersModel.AddLasso();
		WorkbookModel.Instance.SpecBoostersModel.AddLasso();
	}

	private void OnDestroy()
	{
		AdsWrapper instance = AdsWrapper.Instance;
		instance.OnVideoAvailabilityChanged = (Action)Delegate.Remove(instance.OnVideoAvailabilityChanged, new Action(this.OnVideoAvailabilityChangedHandler));
		IAPWrapper instance2 = IAPWrapper.Instance;
		instance2.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Remove(instance2.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
	}
}


