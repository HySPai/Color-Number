/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class AnalyticsPlatform : MonoBehaviour
{
	protected List<AnalyticsEvents> PlatformEvents { get; set; }

	public virtual void Init()
	{
		this.PlatformEvents = Enum.GetValues(typeof(AnalyticsEvents)).OfType<AnalyticsEvents>().ToList();
	}

	public void SendEvent(AnalyticsEvents eventType)
	{
		if (this.PlatformEvents != null)
		{
			if (this.PlatformEvents.Contains(eventType))
			{
				this.SendEventInternal(eventType);
			}
		}
	}

	public void SendEvent(AnalyticsEvents eventType, Dictionary<string, object> parameters)
	{
		if (PlatformEvents != null && this.PlatformEvents.Contains(eventType))
		{
			this.SendEventInternal(eventType, parameters);
		}
	}

	protected void Log(AnalyticsEvents eventType, Dictionary<string, object> parameters)
	{
		if (AnalyticsHelper.IsTestDevice())
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[Analytics] eventName : ").Append(eventType.ToString());
			if (parameters != null)
			{
				foreach (KeyValuePair<string, object> parameter in parameters)
				{
					stringBuilder.Append("\nparam[").Append(parameter.Key).Append("] = ")
						.Append(parameter.Value);
				}
			}
			UnityEngine.Debug.Log(stringBuilder.ToString());
			CustomLogger.Instance.Log(stringBuilder.ToString());
		}
	}

	protected abstract void SendEventInternal(AnalyticsEvents eventType);

	protected abstract void SendEventInternal(AnalyticsEvents eventType, Dictionary<string, object> parameters);
}


