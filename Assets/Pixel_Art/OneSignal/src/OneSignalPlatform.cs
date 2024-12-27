/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System.Collections.Generic;

// Shared interface so OneSignal.cs can use each mobile platform in a generic way
public interface OneSignalPlatform {
   void SetLogLevel(OneSignal.LOG_LEVEL logLevel, OneSignal.LOG_LEVEL visualLevel);
   void RegisterForPushNotifications();
   void promptForPushNotificationsWithUserResponse();
   void SendTag(string tagName, string tagValue);
   void SendTags(IDictionary<string, string> tags);
   void GetTags();
   void DeleteTag(string key);
   void DeleteTags(IList<string> keys);
   void IdsAvailable();
   void SetSubscription(bool enable);
   void PostNotification(Dictionary<string, object> data);
   void SyncHashedEmail(string email);
   void PromptLocation();

   void SetEmail (string email);
   void SetEmail(string email, string emailAuthToken);
   void LogoutEmail();

   void SetInFocusDisplaying(OneSignal.OSInFocusDisplayOption display);

   void addPermissionObserver();
   void removePermissionObserver();
   void addSubscriptionObserver();
   void removeSubscriptionObserver();
   void addEmailSubscriptionObserver();
   void removeEmailSubscriptionObserver();

   OSPermissionSubscriptionState getPermissionSubscriptionState();

   OSPermissionState parseOSPermissionState(object stateDict);
   OSSubscriptionState parseOSSubscriptionState(object stateDict);
   OSEmailSubscriptionState parseOSEmailSubscriptionState (object stateDict);

   OSPermissionStateChanges parseOSPermissionStateChanges(string stateChangesJSONString);
   OSSubscriptionStateChanges parseOSSubscriptionStateChanges(string stateChangesJSONString);
   OSEmailSubscriptionStateChanges parseOSEmailSubscriptionStateChanges(string stateChangesJSONString);
}
