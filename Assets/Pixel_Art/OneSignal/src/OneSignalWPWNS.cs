/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

#if UNITY_WP_8_1
public class OneSignalWPWNS : OneSignalPlatform {

   public OneSignalWPWNS(string appId) {
      OneSignalSDK_WP_WNS.ExternalInitUnity.Init(appId, (message, inAdditionalData, isActive) => {
         if (OneSignal.builder != null && OneSignal.builder.notificationOpenedDelegate != null) {
            Dictionary<string, object> additionalData = null;
            if (inAdditionalData != null)
               additionalData = inAdditionalData.ToDictionary(pair => pair.Key, pair => (object)pair.Value);

            OSNotificationOpenedResult result = new OSNotificationOpenedResult();
            result.action = new OSNotificationAction();
            result.action.type = OSNotificationAction.ActionType.Opened;

            result.notification = new OSNotification();
            result.notification.shown = !isActive;

            result.notification.payload = new OSNotificationPayload();
            result.notification.payload.body = message;
            result.notification.payload.additionalData = additionalData;

            OneSignal.builder.notificationOpenedDelegate(result);
         }
      });
   }
   
   public void SendTag(string tagName, string tagValue) {
      OneSignalSDK_WP_WNS.OneSignal.SendTag(tagName, tagValue);
   }
   
   public void SendTags(IDictionary<string, string> tags) {
      OneSignalSDK_WP_WNS.OneSignal.SendTags(tags.ToDictionary(pair => pair.Key, pair => (object)pair.Value));
   }
   
   public void SendPurchase(double amount) {
      OneSignalSDK_WP_WNS.OneSignal.SendPurchase(amount);
   }
   
   public void GetTags() {
      OneSignalSDK_WP_WNS.OneSignal.GetTags((tags) => {
         OneSignal.tagsReceivedDelegate(tags.ToDictionary(pair => pair.Key, pair => (object)pair.Value));
      });
   }
   
   public void DeleteTag(string key) {
      OneSignalSDK_WP_WNS.OneSignal.DeleteTag(key);
   }
   
   public void DeleteTags(IList<string> key) {
      OneSignalSDK_WP_WNS.OneSignal.DeleteTags(key);
   }
   
   public void IdsAvailable() {
      OneSignalSDK_WP_WNS.OneSignal.GetIdsAvailable((playerId, channelUri) => {
         OneSignal.idsAvailableDelegate(playerId, channelUri);
      });
   }
   
   // The following have not been implemented by the native WP8.1 SDK.
   public void SetSubscription(bool enable) {}
   public void PostNotification(Dictionary<string, object> data) { }
   public void PromptLocation() {}
   public void SyncHashedEmail(string email) {}
   public void SetLogLevel(OneSignal.LOG_LEVEL logLevel, OneSignal.LOG_LEVEL visualLevel) {}
   public void SetInFocusDisplaying(OneSignal.OSInFocusDisplayOption display) {}
   public void addPermissionObserver() {}
   public void removePermissionObserver() { }
   public void addSubscriptionObserver() { }
   public void removeSubscriptionObserver() { }

   public OSPermissionSubscriptionState getPermissionSubscriptionState() {
      var state = new OSPermissionSubscriptionState();
      state.permissionStatus = new OSPermissionState();
      state.subscriptionStatus = new OSSubscriptionState();

      return state;
   }

   public OSPermissionState parseOSPermissionState(object stateDict) {
      return new OSPermissionState();
   }
   public OSSubscriptionState parseOSSubscriptionState(object stateDict) {
      return new OSSubscriptionState();
   }

   public OSPermissionStateChanges parseOSPermissionStateChanges(string stateChangesJSONString) {
      var state = new OSPermissionStateChanges();
      state.to = new OSPermissionState();
      state.from = new OSPermissionState();
      return state;
   }
   public OSSubscriptionStateChanges parseOSSubscriptionStateChanges(string stateChangesJSONString) {
      var state = new OSSubscriptionStateChanges();
      state.to = new OSSubscriptionState();
      state.from = new OSSubscriptionState();
      return state;
   }


   // Doesn't apply to Windows Phone, doesn't have a native permission prompt
   public void RegisterForPushNotifications() {}
   public void promptForPushNotificationsWithUserResponse() { }
}
#endif
