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
using UnityEngine;
using UnityEngine.Purchasing;

public class AnalyticsManager : MonoBehaviour
{
	private DateTime m_startTime = DateTime.Now;

	private List<AnalyticsPlatform> m_platforms;

	public static AnalyticsManager Instance { get; set; }

	public void Awake()
	{
		if (AnalyticsManager.Instance == null)
		{
			AnalyticsManager.Instance = this;
		}
		this.m_startTime = DateTime.Now;
		this.m_platforms = new List<AnalyticsPlatform>(); 
		this.m_platforms.Add(base.gameObject.AddComponent<UnityPlatform>()); 
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.Init();
		}
		base.StartCoroutine(this.SendOldUserCoroutine());
	}

	private IEnumerator SendOldUserCoroutine()
	{
		yield return null;
		if (AppData.IsOldUser)
		{
			this.SendOldUser();
		}
	}

	public void Test()
	{
	}

	public void LibraryOpened()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.MainScr_opened);
		}
	}

	public void MyWorksOpened()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.MyWorksScr_opened);
		}
	}

	public void PhotosOpened()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.PhotosScr_opened);
		}
	}

	public void InappsWindowOpened(string placement)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.IapScr_opened);
		}
		foreach (AnalyticsPlatform platform2 in this.m_platforms)
		{
			platform2.SendEvent(AnalyticsEvents.sub_3options_displayed, new Dictionary<string, object> {
				{
					"placement",
					placement
				}
			});
		}
	}

	public void InappsWindowClosed()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.IapScr_closed);
		}
	}

	public void SubscriptionClick(SubscriptionType subscriptionType)
	{
		AnalyticsEvents eventType = AnalyticsEvents.WeekSubscrBtn_tapped;
		switch (subscriptionType)
		{
			case SubscriptionType.one_month_subscription:
				eventType = AnalyticsEvents.MonthSubscrBtn_tapped;
				break;
			case SubscriptionType.one_year_subscription:
				eventType = AnalyticsEvents.YearSubscrBtn_tapped;
				break;
		}
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(eventType);
		}
	}

	public void ABTestWindowOpened(string placement)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.sub_promo_displayed, new Dictionary<string, object> {
				{
					"placement",
					placement
				}
			});
		}
	}

	public void ABTestWindowAction(string action, string placement)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.sub_promo_action, new Dictionary<string, object> {
				{
					"action",
					action
				},
				{
					"placement",
					placement
				}
			});
		}
	}

	public void TrialWindowAction(string action, string placement, bool onStart = false, bool subCampaign = false)
	{
		if (subCampaign)
		{
			foreach (AnalyticsPlatform platform in this.m_platforms)
			{
				platform.SendEvent(AnalyticsEvents.sub_campaign_1option_action, new Dictionary<string, object> {
					{
						"action",
						action
					}
				});
			}
		}
		else if (onStart)
		{
			foreach (AnalyticsPlatform platform2 in this.m_platforms)
			{
				platform2.SendEvent(AnalyticsEvents.sub_launch_1option_action, new Dictionary<string, object> {
					{
						"action",
						action
					}
				});
			}
		}
		else
		{
			foreach (AnalyticsPlatform platform3 in this.m_platforms)
			{
				platform3.SendEvent(AnalyticsEvents.sub_1option_action, new Dictionary<string, object> {
					{
						"action",
						action
					},
					{
						"placement",
						placement
					}
				});
			}
		}
	}

	public void TrialWindowSuccess(bool onStart = false)
	{
		if (onStart)
		{
			foreach (AnalyticsPlatform platform in this.m_platforms)
			{
				platform.SendEvent(AnalyticsEvents.sub_launch_week_trial);
			}
		}
	}

	public void InappsWindowAction(string action, string placement)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.sub_3options_action, new Dictionary<string, object> {
				{
					"action",
					action
				},
				{
					"placement",
					placement
				}
			});
		}
	}

	public void DeleteButtonClick()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.DeleteBtn_tapped);
		}
	}

	public void DeleteSuccess()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.Delete_success);
		}
	}

	public void InterstitialShowed()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.Interstitial_showed);
		}
	}

	public void SettingsWindowOpened()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.InfoScr_opened);
		}
	}

	public void RestoreButtonClicked()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.RestoreBtn_tapped);
		}
	}

	public void PurchasesRestored()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.Purchases_restored);
		}
	}

	public void RateUsClicked()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.RateUs_tapped);
		}
	}

	public void MoreAppsClicked()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.MoreAppsBtn_tapped);
		}
	}

	public void HelpWindowOpened()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.HelpScr_opened);
		}
	}

	public void SupportWindowOpened()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.SupportScr_opened);
		}
	}

	public void TutorOpened(string placement, string type)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.tutorial_opened, new Dictionary<string, object> {
				{
					"placement",
					placement
				},
				{
					"type",
					type
				}
			});
		}
	}

	public void TutorClosed(string placement, string type, int step)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.tutorial_closed, new Dictionary<string, object> {
				{
					"placement",
					placement
				},
				{
					"type",
					type
				},
				{
					"step",
					step
				}
			});
		}
	}

	public void FilterWindowOpened()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.FiltersScr_opened);
		}
	}

	public void FilterWindowClosed(string filterName)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.FiltersScr_closed, new Dictionary<string, object> {
				{
					"filter",
					filterName
				}
			});
		}
	}

	public void LongTapActivated()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.LongTap_activated);
		}
	}

	public void ShareWindowOpened()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.ShareScr_opened);
		}
	}

	public void DeleteWatermarkAttempt()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.DeleteWatermarkBtn_tapped);
		}
	}

	public void SaveButtonClicked()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.SaveBtn_tapped);
		}
	}

	public void SaveSuccess()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.Save_success);
		}
	}

	public void ShareButtonClicked()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.ShareBtn_tapped);
		}
	}

	public void ShareViaFbButtonClicked()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.ShareViaFbBtn_tapped);
		}
	}

	public void ShareViaInstagramButtonClicked()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.ShareViaInstagramBtn_tapped);
		}
	}

	public void ElementColored(ColorizationModeModel.BrushType brushType, bool isRightColor)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.Element_colored, new Dictionary<string, object> {
				{
					"brush",
					((brushType != ColorizationModeModel.BrushType.Plural) ? "no" : "yes")
				},
				{
					"rightColor",
					((!isRightColor) ? "no" : "yes")
				}
			});
		}
	}

	public void PurchaseFromAd(string inapp, string adid)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.purchase_from_ad);
		}
	}

	public void StartColoringSession()
	{
	}

	public void ColoringSession(double seconds)
	{
		try
		{
			foreach (AnalyticsPlatform platform in this.m_platforms)
			{
				platform.SendEvent(AnalyticsEvents.coloring_session, new Dictionary<string, object> {
					{
						"seconds",
						seconds
					}
				});
				platform.SendEvent(AnalyticsEvents.time_gamescreen, new Dictionary<string, object> {
					{
						"seconds",
						(int)seconds
					}
				});
			}
		}
		catch
		{
		}
	}

	public void BackButtonClicked()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.BackBtn_Tapped);
		}
	}

	public void ZoomDone()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.Zoom_done);
		}
	}

	public void TrialInappsWindowOpened(bool onStart, string placement, bool subCampaign = false)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.IapScr_7d_opened);
		}
		if (subCampaign)
		{
			foreach (AnalyticsPlatform platform2 in this.m_platforms)
			{
				platform2.SendEvent(AnalyticsEvents.sub_campaign_1option_displayed);
			}
		}
		else if (onStart)
		{
			foreach (AnalyticsPlatform platform3 in this.m_platforms)
			{
				platform3.SendEvent(AnalyticsEvents.sub_launch_1option_displayed);
			}
		}
		else
		{
			foreach (AnalyticsPlatform platform4 in this.m_platforms)
			{
				platform4.SendEvent(AnalyticsEvents.sub_1option_displayed, new Dictionary<string, object> {
					{
						"placement",
						placement
					}
				});
			}
		}
	}

	public void TrialInappsWindowClosed()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.IapScr_7d_closed);
		}
	}

	public void InappPurchaseSuccess(Product product, string placement, float responseTime)
	{
		if (AnalyticsHelper.IsTestDevice())
		{
			foreach (AnalyticsPlatform platform in this.m_platforms)
			{
				platform.SendEvent(AnalyticsEvents.Subscr_success_sandbox, new Dictionary<string, object> {
					{
						"product",
						product.definition.storeSpecificId
					}
				});
			}
		}
		else
		{
			foreach (AnalyticsPlatform platform2 in this.m_platforms)
			{
				platform2.SendEvent(AnalyticsEvents.Subscr_success, new Dictionary<string, object> {
					{
						"product",
						product.definition.storeSpecificId
					}
				});
			}
		}
		SubscriptionType subscriptionType = (SubscriptionType)Enum.Parse(typeof(SubscriptionType), product.definition.id);
		string value = string.Empty;
		switch (subscriptionType)
		{
			case SubscriptionType.one_week_subscription:
				value = "week";
				break;
			case SubscriptionType.one_month_subscription:
				value = "month";
				break;
			case SubscriptionType.one_year_subscription:
				value = "year";
				break;
		}
		foreach (AnalyticsPlatform platform3 in this.m_platforms)
		{
			platform3.SendEvent(AnalyticsEvents.sub_result, new Dictionary<string, object> {
				{
					"sub_type",
					value
				},
				{
					"response",
					"success"
				},
				{
					"time_1s",
					(int)responseTime
				},
				{
					"placement",
					placement
				}
			});
		}
	}

	public void InappPurchaseFailed(SubscriptionType type, string placement, float responseTime, string reason, string nativeFailReason = "")
	{
		if (reason == "UserCancelled")
		{
			foreach (AnalyticsPlatform platform in this.m_platforms)
			{
				platform.SendEvent(AnalyticsEvents.Subscr_cancel);
			}
		}
		else
		{
			foreach (AnalyticsPlatform platform2 in this.m_platforms)
			{
				platform2.SendEvent(AnalyticsEvents.Subscr_fail, new Dictionary<string, object> {
					{
						"reason",
						reason.ToString()
					}
				});
			}
		}
		string value = string.Empty;
		switch (type)
		{
			case SubscriptionType.one_week_subscription:
				value = "week";
				break;
			case SubscriptionType.one_month_subscription:
				value = "month";
				break;
			case SubscriptionType.one_year_subscription:
				value = "year";
				break;
		}
		foreach (AnalyticsPlatform platform3 in this.m_platforms)
		{
			platform3.SendEvent(AnalyticsEvents.sub_result, new Dictionary<string, object> {
				{
					"sub_type",
					value
				},
				{
					"response",
					(reason + "; " + nativeFailReason)
				},
				{
					"time_1s",
					(int)responseTime
				},
				{
					"placement",
					placement
				}
			});
		}
	}

	public void SendTrial(string productId, string transactionId, string originalTransactionId)
	{
		if (AnalyticsHelper.IsTestDevice())
		{
			foreach (AnalyticsPlatform platform in this.m_platforms)
			{
				platform.SendEvent(AnalyticsEvents.TrialSubscr_success_sandbox, new Dictionary<string, object> {
					{
						"product",
						productId
					},
					{
						"transactionId",
						transactionId
					},
					{
						"originalTransactionId",
						originalTransactionId
					}
				});
			}
		}
		else
		{
			foreach (AnalyticsPlatform platform2 in this.m_platforms)
			{
				platform2.SendEvent(AnalyticsEvents.TrialSubscr_success, new Dictionary<string, object> {
					{
						"product",
						productId
					},
					{
						"transactionId",
						transactionId
					},
					{
						"originalTransactionId",
						originalTransactionId
					}
				});
			}
		}
	}

	public void SendNotTrial(string productId, string transactionId, string originalTransactionId)
	{
		if (AnalyticsHelper.IsTestDevice())
		{
			foreach (AnalyticsPlatform platform in this.m_platforms)
			{
				platform.SendEvent(AnalyticsEvents.NotTrialSubscr_success_sandbox, new Dictionary<string, object> {
					{
						"product",
						productId
					},
					{
						"transactionId",
						transactionId
					},
					{
						"originalTransactionId",
						originalTransactionId
					}
				});
			}
		}
		else
		{
			foreach (AnalyticsPlatform platform2 in this.m_platforms)
			{
				platform2.SendEvent(AnalyticsEvents.NotTrialSubscr_success, new Dictionary<string, object> {
					{
						"product",
						productId
					},
					{
						"transactionId",
						transactionId
					},
					{
						"originalTransactionId",
						originalTransactionId
					}
				});
			}
		}
	}

	public void SubscriptionEvent(AnalyticsEvents eventType)
	{
	}

	public void ImageSelected(ImageInfo info)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.picture_selected, new Dictionary<string, object> {
				{
					"type",
					info.AccessStatus.ToString().ToLower()
				},
				{
					"picture_id",
					info.Id
				},
				{
					"source",
					info.GetRightSource()
				}
			});
		}
	}

	public void ImageOpened(ImageInfo info, int count, int colorsCount, ImageOpenType imageOpenType)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.picture_opened, new Dictionary<string, object> {
				{
					"numbers",
					count
				},
				{
					"colors",
					colorsCount
				},
				{
					"picture_id",
					info.Id
				},
				{
					"status",
					imageOpenType.ToString().ToLower()
				},
				{
					"source",
					info.GetRightSource()
				},
				{
					"type",
					info.CustomAccessStatus.ToString().ToLower()
				}
			});
		}
	}

	public void ImageDone(ImageInfo info, int count, int colorsCount)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.Image_done);
			platform.SendEvent(AnalyticsEvents.picture_completed, new Dictionary<string, object> {
				{
					"numbers",
					count
				},
				{
					"colors",
					colorsCount
				},
				{
					"picture_id",
					info.Id
				},
				{
					"source",
					info.GetRightSource()
				},
				{
					"type",
					info.CustomAccessStatus.ToString().ToLower()
				}
			});
		}
		AppData.PictureCompletedCount++;
		if (!AppData.PictureCompleted)
		{
			AppData.PictureCompleted = true;
		}
		if (AppData.PictureCompletedCount <= 5)
		{
			AnalyticsEvents analyticsEvents = AnalyticsEvents.Undefined;
			switch (AppData.PictureCompletedCount)
			{
				case 1:
					analyticsEvents = AnalyticsEvents.picture_completed_1st;
					break;
				case 2:
					analyticsEvents = AnalyticsEvents.picture_completed_2nd;
					break;
				case 3:
					analyticsEvents = AnalyticsEvents.picture_completed_3rd;
					break;
				case 5:
					analyticsEvents = AnalyticsEvents.picture_completed_5th;
					break;
			}
			if (analyticsEvents != 0)
			{
				foreach (AnalyticsPlatform platform2 in this.m_platforms)
				{
					platform2.SendEvent(analyticsEvents);
				}
			}
		}
	}

	public void BombClicked(int count)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.gamescreen_booster_grenade, new Dictionary<string, object> {
				{
					"count",
					count
				}
			});
		}
	}

	public void BombUsed(int count)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.gamescreen_booster_grenade_used, new Dictionary<string, object> {
				{
					"count",
					count
				}
			});
		}
	}

	public void BombAddClick(int count)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.gamescreen_booster_grenade_add, new Dictionary<string, object> {
				{
					"count",
					count
				}
			});
		}
	}

	public void LassoClicked(int count)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.gamescreen_booster_magicwand, new Dictionary<string, object> {
				{
					"count",
					count
				}
			});
		}
	}

	public void LassoUsed(int count)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.gamescreen_booster_magicwand_used, new Dictionary<string, object> {
				{
					"count",
					count
				}
			});
		}
	}

	public void LassoAddClick(int count)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.gamescreen_booster_magicwand_add, new Dictionary<string, object> {
				{
					"count",
					count
				}
			});
		}
	}

	private void SendSessionTime(int seconds)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.time_total, new Dictionary<string, object> {
				{
					"seconds",
					seconds
				}
			});
		}
	}

	public void SendOldUser()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.previous_users);
		}
	}

	public void LaunchNextDay()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.session_2ndDay);
		}
	}

	public void LaunchNext2Days()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.session_nearest2days);
		}
	}

	public void TrackFirstSessionOfTheDay()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.session_1day, new Dictionary<string, object> {
				{
					"sub_user_group",
					INPluginWrapper.Instance.GetAbTestGroupStr()
				},
				{
					"banner_placement",
					("banner_" + INPluginWrapper.Instance.GetBannerStrategyStr())
				}
			});
		}
	}

	public void ShareEvent(string artType, string via, bool watermark)
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.share_art, new Dictionary<string, object> {
				{
					"art_type",
					artType
				},
				{
					"via",
					via
				},
				{
					"watermark",
					watermark
				}
			});
		}
	}

	public void TrackFirstWeekIntersLimit()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.fire_avg_impression_7d);
		}
	}

	public void TrackFirstWeekSecondsLimit()
	{
		foreach (AnalyticsPlatform platform in this.m_platforms)
		{
			platform.SendEvent(AnalyticsEvents.fire_avg_time_7d);
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		//if (!pauseStatus && FB.IsInitialized)
		//{
		//    FB.ActivateApp();
		//}
	}

	private void OnApplicationQuit()
	{
		this.SendSessionTime((int)(DateTime.Now - this.m_startTime).TotalSeconds);
	}
}
