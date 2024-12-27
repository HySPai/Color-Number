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

internal class GooglePurchaseData
{
	[Serializable]
	private struct GooglePurchaseReceipt
	{
		public string Payload;
	}

	[Serializable]
	private struct GooglePurchasePayload
	{
		public string json;

		public string signature;
	}

	[Serializable]
	public struct GooglePurchaseJson
	{
		public string autoRenewing;

		public string orderId;

		public string packageName;

		public string productId;

		public string purchaseTime;

		public string purchaseState;

		public string developerPayload;

		public string purchaseToken;
	}

	public string inAppPurchaseData;

	public string inAppDataSignature;

	public GooglePurchaseJson json;

	public GooglePurchaseData(string receipt)
	{
		try
		{
			GooglePurchaseReceipt googlePurchaseReceipt = JsonUtility.FromJson<GooglePurchaseReceipt>(receipt);
			GooglePurchasePayload googlePurchasePayload = JsonUtility.FromJson<GooglePurchasePayload>(googlePurchaseReceipt.Payload);
			GooglePurchaseJson googlePurchaseJson = JsonUtility.FromJson<GooglePurchaseJson>(googlePurchasePayload.json);
			this.inAppPurchaseData = googlePurchasePayload.json;
			this.inAppDataSignature = googlePurchasePayload.signature;
			this.json = googlePurchaseJson;
		}
		catch
		{
			UnityEngine.Debug.Log("Could not parse receipt: " + receipt);
			this.inAppPurchaseData = string.Empty;
			this.inAppDataSignature = string.Empty;
		}
	}
}


