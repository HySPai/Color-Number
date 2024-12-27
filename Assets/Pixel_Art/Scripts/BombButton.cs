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

public class BombButton : MonoBehaviour
{
	[SerializeField]
	private Image m_image;

	[SerializeField]
	private GameObject m_activePointer;

	[SerializeField]
	private Text m_bombsCountField;

	[SerializeField]
	private Text m_bombsCountFieldCopy;

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
		specBoostersModel.OnBombModeChanged = (Action<SpecBoostersModel>)Delegate.Combine(specBoostersModel.OnBombModeChanged, new Action<SpecBoostersModel>(this.OnBombModeChangedHandler));
		SpecBoostersModel specBoostersModel2 = WorkbookModel.Instance.SpecBoostersModel;
		specBoostersModel2.OnStateChanged = (Action<SpecBoostersModel>)Delegate.Combine(specBoostersModel2.OnStateChanged, new Action<SpecBoostersModel>(this.OnBombCountChangedHandler));
		AdsWrapper instance2 = AdsWrapper.Instance;
		instance2.OnVideoAvailabilityChanged = (Action)Delegate.Combine(instance2.OnVideoAvailabilityChanged, new Action(this.OnVideoAvailabilityChangedHandler));
		this.OnBombModeChangedHandler(WorkbookModel.Instance.SpecBoostersModel);
		this.OnBombCountChangedHandler(WorkbookModel.Instance.SpecBoostersModel);
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

	private void OnBombModeChangedHandler(SpecBoostersModel model)
	{
		this.m_activePointer.SetActive(model.BombMode);
	}

	private void OnBombCountChangedHandler(SpecBoostersModel model)
	{
		if (IAPWrapper.Instance.NoAds || IAPWrapper.Instance.Subscribed)
		{
			this.m_bombsCountField.gameObject.SetActive(false);
		}
		else if (IAPWrapper.Instance.NoAds)
		{
			this.m_bombsCountField.gameObject.SetActive(false);
		}
		else
		{
			this.m_bombsCountField.text = model.BombCount.ToString();
			this.m_bombsCountFieldCopy.text = model.BombCount.ToString();
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
		this.OnBombCountChangedHandler(WorkbookModel.Instance.SpecBoostersModel);
	}

	public void Click()
	{
		if (IAPWrapper.Instance.NoAds || IAPWrapper.Instance.Subscribed)
		{
			while (WorkbookModel.Instance.SpecBoostersModel.BombCount < 5)
			{
				WorkbookModel.Instance.SpecBoostersModel.AddBomb();
			}
		}
		if (WorkbookModel.Instance.SpecBoostersModel.BombCount <= 0)
		{
			this.PlusClick();
		}
		else
		{
			WorkbookModel.Instance.SpecBoostersModel.ChangeBombMode(false);
			if (WorkbookModel.Instance.SpecBoostersModel.BombMode)
			{
				AnalyticsManager.Instance.BombClicked(WorkbookModel.Instance.SpecBoostersModel.BombCount);
			}
		}
	}

	public void PlusClick()
	{
		AnalyticsManager.Instance.BombAddClick(WorkbookModel.Instance.SpecBoostersModel.BombCount);
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
						AdsWrapper.Instance.ShowVideo("grenade", delegate (bool res)
						{
							if (res)
							{
								WorkbookModel.Instance.SpecBoostersModel.AddBomb();
								WorkbookModel.Instance.SpecBoostersModel.ChangeBombMode(true);
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
								WorkbookModel.Instance.SpecBoostersModel.AddBomb();
								WorkbookModel.Instance.SpecBoostersModel.ChangeBombMode(true);
								WindowManager.Instance.CloseMe(abWindow);
							}
						}, ABTestGroup.RewardedYes_ContentHard, AbTestWindowMode.Bomb);
					}
					else
					{
						var trialInappsWindow = WindowManager.Instance.OpenInappsWindow();
						trialInappsWindow.Init("grenade", null);//, false, false);
					}
					break;
			}
		}
		else
		{
			WorkbookModel.Instance.SpecBoostersModel.AddBomb();
			WorkbookModel.Instance.SpecBoostersModel.ChangeBombMode(true);
		}
	}

	public void CheatButtonClick()
	{
		WorkbookModel.Instance.SpecBoostersModel.AddBomb();
		WorkbookModel.Instance.SpecBoostersModel.AddBomb();
		WorkbookModel.Instance.SpecBoostersModel.AddBomb();
		WorkbookModel.Instance.SpecBoostersModel.AddBomb();
		WorkbookModel.Instance.SpecBoostersModel.AddBomb();
	}

	private void OnDestroy()
	{
		AdsWrapper instance = AdsWrapper.Instance;
		instance.OnVideoAvailabilityChanged = (Action)Delegate.Remove(instance.OnVideoAvailabilityChanged, new Action(this.OnVideoAvailabilityChangedHandler));
		IAPWrapper instance2 = IAPWrapper.Instance;
		instance2.OnPurchase = (Action<bool, SubscriptionType>)Delegate.Remove(instance2.OnPurchase, new Action<bool, SubscriptionType>(this.OnPurchaseHandler));
	}
}


