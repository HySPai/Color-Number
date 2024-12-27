/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

#if UNITY_IPHONE
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using OneSignalPush.MiniJSON;
using System;

public class OneSignalIOS : OneSignalPlatform {

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _init(string listenerName, string appId, bool autoPrompt, bool inAppLaunchURLs, int displayOption, int logLevel, int visualLogLevel);

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _registerForPushNotifications();

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _sendTag(string tagName, string tagValue);

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _sendTags(string tags);

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _getTags();

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _deleteTag(string key);

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _deleteTags(string keys);

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _idsAvailable();

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _setSubscription(bool enable);

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _postNotification(string json);

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _syncHashedEmail(string email);

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _promptLocation();

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _setInFocusDisplayType(int type);

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _promptForPushNotificationsWithUserResponse();

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _addPermissionObserver();

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _removePermissionObserver();

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _addSubscriptionObserver();

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _removeSubscriptionObserver();

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _addEmailSubscriptionObserver();
   
   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _removeEmailSubscriptionObserver();

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public string _getPermissionSubscriptionState();

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _setEmail (string email, string emailAuthCode);

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _setUnauthenticatedEmail (string email);

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _logoutEmail();

   [System.Runtime.InteropServices.DllImport("__Internal")]
   extern static public void _setOneSignalLogLevel(int logLevel, int visualLogLevel);


   public OneSignalIOS(string gameObjectName, string appId, bool autoPrompt, bool inAppLaunchURLs, OneSignal.OSInFocusDisplayOption displayOption, OneSignal.LOG_LEVEL logLevel, OneSignal.LOG_LEVEL visualLevel) {
       _init(gameObjectName, appId, autoPrompt, inAppLaunchURLs, (int)displayOption, (int)logLevel, (int)visualLevel);
   }

   public void RegisterForPushNotifications() {
      _registerForPushNotifications();
   }

   public void SendTag(string tagName, string tagValue) {
      _sendTag(tagName, tagValue);
   }

   public void SendTags(IDictionary<string, string> tags) {
      _sendTags(Json.Serialize(tags));
   }

   public void GetTags() {
      _getTags();
   }

   public void DeleteTag(string key) {
      _deleteTag(key);
   }

   public void DeleteTags(IList<string> keys) {
      _deleteTags(Json.Serialize(keys));
   }

   public void IdsAvailable() {
      _idsAvailable();
   }

   public void SetSubscription(bool enable) {
      _setSubscription(enable);
   }

   public void PostNotification(Dictionary<string, object> data) {
      _postNotification(Json.Serialize(data));
   }


    public void SyncHashedEmail(string email) {
        _syncHashedEmail(email);
    }

    public void PromptLocation() {
        _promptLocation();
    }

   public void SetLogLevel(OneSignal.LOG_LEVEL logLevel, OneSignal.LOG_LEVEL visualLevel) {
      _setOneSignalLogLevel((int)logLevel, (int)visualLevel);
   }

   public void SetInFocusDisplaying(OneSignal.OSInFocusDisplayOption display) {
      _setInFocusDisplayType((int)display);
   }

   public void promptForPushNotificationsWithUserResponse() {
      _promptForPushNotificationsWithUserResponse();
   }

   public void addPermissionObserver() {
      _addPermissionObserver();
   }

   public void removePermissionObserver() {
      _removePermissionObserver();
   }

   public void addSubscriptionObserver() {
      _addSubscriptionObserver();
   }

   public void removeSubscriptionObserver() {
      _removeSubscriptionObserver();
   }

   public void addEmailSubscriptionObserver() {
      _addEmailSubscriptionObserver();
   }

   public void removeEmailSubscriptionObserver() {
      _removeEmailSubscriptionObserver();
   }

   public void SetEmail(string email, string emailAuthCode) {
      _setEmail (email, emailAuthCode);
   }

   public void SetEmail(string email) {
      _setUnauthenticatedEmail (email);
   }

   public void LogoutEmail() {
      _logoutEmail();
   }

   public OSPermissionSubscriptionState getPermissionSubscriptionState() {
      return OneSignalPlatformHelper.parsePermissionSubscriptionState(this, _getPermissionSubscriptionState());
   }

   public OSPermissionStateChanges parseOSPermissionStateChanges(string jsonStat) {
      return OneSignalPlatformHelper.parseOSPermissionStateChanges(this, jsonStat);
   }

	public OSEmailSubscriptionStateChanges parseOSEmailSubscriptionStateChanges(string jsonState) {
		return OneSignalPlatformHelper.parseOSEmailSubscriptionStateChanges (this, jsonState);
	}

   public OSSubscriptionStateChanges parseOSSubscriptionStateChanges(string jsonStat) {
      return OneSignalPlatformHelper.parseOSSubscriptionStateChanges(this, jsonStat);
   }

   public OSPermissionState parseOSPermissionState(object stateDict) {
      var stateDictCasted = stateDict as Dictionary<string, object>;

      var state = new OSPermissionState();
      state.hasPrompted = Convert.ToBoolean(stateDictCasted["hasPrompted"]);
      state.status = (OSNotificationPermission)Convert.ToInt32(stateDictCasted["status"]);

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

      if (stateDictCasted.ContainsKey ("emailUserId")) {
         state.emailUserId = stateDictCasted ["emailUserId"] as string;
      } else {
         state.emailUserId = "";
      }

      if (stateDictCasted.ContainsKey ("emailAddress")) {
         state.emailAddress = stateDictCasted ["emailAddress"] as string;
      } else {
         state.emailAddress = "";
      }
         
      
      state.subscribed = stateDictCasted.ContainsKey("emailUserId") && stateDictCasted["emailUserId"] != null;

      return state;
   }
}
#endif
