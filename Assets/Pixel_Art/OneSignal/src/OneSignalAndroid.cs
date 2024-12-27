/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using OneSignalPush.MiniJSON;
using System;

public class OneSignalAndroid : OneSignalPlatform {
   private static AndroidJavaObject mOneSignal = null;

   public OneSignalAndroid(string gameObjectName, string googleProjectNumber, string appId, OneSignal.OSInFocusDisplayOption displayOption, OneSignal.LOG_LEVEL logLevel, OneSignal.LOG_LEVEL visualLevel) {
      mOneSignal = new AndroidJavaObject("com.onesignal.OneSignalUnityProxy", gameObjectName, googleProjectNumber, appId, (int)logLevel, (int)visualLevel);
      SetInFocusDisplaying(displayOption);
   }

   public void SetLogLevel(OneSignal.LOG_LEVEL logLevel, OneSignal.LOG_LEVEL visualLevel) {
      mOneSignal.Call("setLogLevel", (int)logLevel, (int)visualLevel);
   }

   public void SendTag(string tagName, string tagValue) {
      mOneSignal.Call("sendTag", tagName, tagValue);
   }

   public void SendTags(IDictionary<string, string> tags) {
      mOneSignal.Call("sendTags", Json.Serialize(tags));
   }

   public void GetTags() {
      mOneSignal.Call("getTags");
   }

   public void DeleteTag(string key) {
      mOneSignal.Call("deleteTag", key);
   }

   public void DeleteTags(IList<string> keys) {
      mOneSignal.Call("deleteTags", Json.Serialize(keys));
   }


   public void IdsAvailable() {
      mOneSignal.Call("idsAvailable");
   }

   // Doesn't apply to Android, doesn't have a native permission prompt
   public void RegisterForPushNotifications() { }
   public void promptForPushNotificationsWithUserResponse() {}

   public void EnableVibrate(bool enable) {
      mOneSignal.Call("enableVibrate", enable);
   }

   public void EnableSound(bool enable) {
      mOneSignal.Call("enableSound", enable);
   }

   public void SetInFocusDisplaying(OneSignal.OSInFocusDisplayOption display) {
      mOneSignal.Call("setInFocusDisplaying", (int)display);
   }

   public void SetSubscription(bool enable) {
      mOneSignal.Call("setSubscription", enable);
   }

   public void PostNotification(Dictionary<string, object> data) {
      mOneSignal.Call("postNotification", Json.Serialize(data));
   }

   public void SyncHashedEmail(string email) {
      mOneSignal.Call("syncHashedEmail", email);
   }

   public void PromptLocation() {
      mOneSignal.Call("promptLocation");
   }

   public void ClearOneSignalNotifications() {
      mOneSignal.Call("clearOneSignalNotifications");
   }

   public void addPermissionObserver() {
      mOneSignal.Call("addPermissionObserver");
   }

   public void removePermissionObserver() {
      mOneSignal.Call("removePermissionObserver");
   }

   public void addSubscriptionObserver() {
      mOneSignal.Call("addSubscriptionObserver");
   }
   public void removeSubscriptionObserver() {
      mOneSignal.Call("removeSubscriptionObserver");
   }
   
   public void addEmailSubscriptionObserver() {
      mOneSignal.Call("addEmailSubscriptionObserver");
   }

   public void removeEmailSubscriptionObserver() {
      mOneSignal.Call("removeEmailSubscriptionObserver");
   }

   public OSPermissionSubscriptionState getPermissionSubscriptionState() {
      return OneSignalPlatformHelper.parsePermissionSubscriptionState(this, mOneSignal.Call<string>("getPermissionSubscriptionState"));
   }

   public OSPermissionStateChanges parseOSPermissionStateChanges(string jsonStat) {
      return OneSignalPlatformHelper.parseOSPermissionStateChanges(this, jsonStat);
   }

   public OSSubscriptionStateChanges parseOSSubscriptionStateChanges(string jsonStat) {
      return OneSignalPlatformHelper.parseOSSubscriptionStateChanges(this, jsonStat);
   }

   public OSEmailSubscriptionStateChanges parseOSEmailSubscriptionStateChanges(string jsonState) {
      return OneSignalPlatformHelper.parseOSEmailSubscriptionStateChanges (this, jsonState);
   }

   public OSPermissionState parseOSPermissionState(object stateDict) {
      var stateDictCasted = stateDict as Dictionary<string, object>;

      var state = new OSPermissionState();
      state.hasPrompted = true;
      var toIsEnabled = Convert.ToBoolean(stateDictCasted["enabled"]);
      state.status = toIsEnabled ? OSNotificationPermission.Authorized : OSNotificationPermission.Denied;

      return state;
   }

   public OSSubscriptionState parseOSSubscriptionState(object stateDict) {
      var stateDictCasted = stateDict as Dictionary<string, object>;

      var state = new OSSubscriptionState();
      state.subscribed = Convert.ToBoolean(stateDictCasted["subscribed"]);
      state.userSubscriptionSetting = Convert.ToBoolean(stateDictCasted["userSubscriptionSetting"]);
      state.userId = stateDictCasted["userId"] as string;
      state.pushToken = stateDictCasted["pushToken"] as string;

      return state;
	}

   public OSEmailSubscriptionState parseOSEmailSubscriptionState(object stateDict) {
      var stateDictCasted = stateDict as Dictionary<string, object>;

      var state = new OSEmailSubscriptionState ();
      state.subscribed = Convert.ToBoolean (stateDictCasted ["subscribed"]);
      state.emailUserId = stateDictCasted ["emailUserId"] as string;
      state.emailAddress = stateDictCasted ["emailAddress"] as string;

      return state;
   }

   public void SetEmail(string email, string emailAuthCode) {
      mOneSignal.Call("setEmail", email, emailAuthCode);
   }

   public void SetEmail(string email) {
      mOneSignal.Call("setEmail", email, null);
   }

   public void LogoutEmail() {
      mOneSignal.Call("logoutEmail");
   }
}
#endif
