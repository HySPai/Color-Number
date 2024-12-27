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
using UnityEngine;

[Serializable]
public class LockitInfo
{
	private Dictionary<string, Dictionary<SystemLanguage, string>> m_dict;

	public int Version { get; set; }

	public string this[string key]
	{
		get
		{
			if (this.m_dict.ContainsKey(key))
			{
				if (this.m_dict[key].ContainsKey(LocalizationManager.Instance.CurrentLanguage))
				{
					string text = this.m_dict[key][LocalizationManager.Instance.CurrentLanguage];
					if (!string.IsNullOrEmpty(text))
					{
						return text;
					}
				}
				return this.m_dict[key][LocalizationManager.Instance.DefaultLanguage];
			}
			return key;
		}
	}

	public LockitInfo()
	{
		this.m_dict = new Dictionary<string, Dictionary<SystemLanguage, string>>();
	}

	public void Init(Dictionary<string, object> dict)
	{
		if (dict != null)
		{
			foreach (object value2 in dict.Values)
			{
				Dictionary<string, object> dictionary = value2 as Dictionary<string, object>;
				string key = string.Empty;
				foreach (string key6 in dictionary.Keys)
				{
					switch (key6)
					{
						case "key":
							key = (string)dictionary[key6];
							this.m_dict.Add(key, new Dictionary<SystemLanguage, string>());
							break;
						default:
							{
								SystemLanguage key2 = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), key6);
								string value = (string)dictionary[key6];
								this.m_dict[key].Add(key2, value);
								break;
							}
						case "id":
							break;
					}
				}
			}
			string key3 = "help_text";
			string key4 = "how_to_cancel_android";
			if (this.m_dict.ContainsKey(key3))
			{
				List<SystemLanguage> list = this.m_dict[key3].Keys.ToList();
				for (int i = 0; i < list.Count; i++)
				{
					SystemLanguage key5 = list[i];
					this.m_dict[key3][key5] = this.m_dict[key3][key5].Replace("<how_to_cancel/>", this.m_dict[key4][key5]);
				}
			}
		}
	}
}


