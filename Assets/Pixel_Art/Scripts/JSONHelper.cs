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
using System.Globalization;
using UnityEngine;

internal static class JSONHelper
{
	public static JSONObject ConvertToJSON(List<string> collection)
	{
		JSONObject jSONObject = new JSONObject(JSONObject.Type.ARRAY);
		foreach (string item in collection)
		{
			jSONObject.Add(item);
		}
		return jSONObject;
	}

	public static List<string> ConvertToStringList(JSONObject jsonData)
	{
		List<string> list = new List<string>();
		foreach (JSONObject item in jsonData.list)
		{
			list.Add(item.str);
		}
		return list;
	}

	public static JSONObject ConvertToJSON(Dictionary<string, bool> collection)
	{
		JSONObject jSONObject = new JSONObject(JSONObject.Type.OBJECT);
		foreach (KeyValuePair<string, bool> item in collection)
		{
			jSONObject.AddField(item.Key, item.Value);
		}
		return jSONObject;
	}

	public static Dictionary<string, bool> ConvertToBoolDictionary(JSONObject jsonData)
	{
		Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
		if (!jsonData.IsNull)
		{
			{
				foreach (string key in jsonData.keys)
				{
					dictionary.Add(key, jsonData[key].b);
				}
				return dictionary;
			}
		}
		return dictionary;
	}

	public static JSONObject ConvertToJSON(Dictionary<string, int> collection)
	{
		JSONObject jSONObject = new JSONObject(JSONObject.Type.OBJECT);
		foreach (KeyValuePair<string, int> item in collection)
		{
			jSONObject.AddField(item.Key, item.Value.ToString());
		}
		return jSONObject;
	}

	public static JSONObject ConvertToJSON(Dictionary<string, object> collection)
	{
		JSONObject jSONObject = new JSONObject(JSONObject.Type.OBJECT);
		foreach (KeyValuePair<string, object> item in collection)
		{
			jSONObject.AddField(item.Key, item.Value.ToString());
		}
		return jSONObject;
	}

	public static JSONObject ConvertToJSON(Dictionary<int, int> collection)
	{
		JSONObject jSONObject = new JSONObject(JSONObject.Type.OBJECT);
		foreach (KeyValuePair<int, int> item in collection)
		{
			jSONObject.AddField(item.Key.ToString(), item.Value.ToString());
		}
		return jSONObject;
	}

	public static Dictionary<int, int> ConvertToIntDictionary(JSONObject jsonData)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		if (!jsonData.IsNull)
		{
			{
				foreach (string key in jsonData.keys)
				{
					dictionary.Add(int.Parse(key), int.Parse(jsonData[key].str));
				}
				return dictionary;
			}
		}
		return dictionary;
	}

	public static Dictionary<string, string> ConvertToStrDictionary(JSONObject jsonData)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (!jsonData.IsNull)
		{
			{
				foreach (string key in jsonData.keys)
				{
					dictionary.Add(key, jsonData[key].str);
				}
				return dictionary;
			}
		}
		return dictionary;
	}

	public static JSONObject ConvertToJSON(Dictionary<string, DateTime> collection)
	{
		JSONObject jSONObject = new JSONObject(JSONObject.Type.OBJECT);
		foreach (KeyValuePair<string, DateTime> item in collection)
		{
			jSONObject.AddField(item.Key, item.Value.ToString(NumberFormatInfo.InvariantInfo));
		}
		return jSONObject;
	}

	public static JSONObject ConvertToJSON(Dictionary<int, DateTime> collection)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (KeyValuePair<int, DateTime> item in collection)
		{
			dictionary.Add(item.Key.ToString(), item.Value.ToString(NumberFormatInfo.InvariantInfo));
		}
		return new JSONObject(dictionary);
	}

	public static Dictionary<string, DateTime> ConvertToStringDateTimeDictionary(JSONObject jsonData)
	{
		Dictionary<string, DateTime> dictionary = new Dictionary<string, DateTime>();
		if (!jsonData.IsNull)
		{
			{
				foreach (string key in jsonData.keys)
				{
					dictionary.Add(key, DateTime.Parse(jsonData[key].str));
				}
				return dictionary;
			}
		}
		return dictionary;
	}

	public static Dictionary<int, DateTime> ConvertToIntDateTimeDictionary(JSONObject jsonData)
	{
		Dictionary<int, DateTime> dictionary = new Dictionary<int, DateTime>();
		foreach (string key in jsonData.keys)
		{
			dictionary.Add(int.Parse(key), DateTime.Parse(jsonData[key].str));
		}
		return dictionary;
	}

	public static JSONObject ConvertToJSON(List<int> collection)
	{
		JSONObject jSONObject = new JSONObject(JSONObject.Type.ARRAY);
		foreach (int item in collection)
		{
			jSONObject.Add(item);
		}
		return jSONObject;
	}

	public static List<int> ConvertToIntList(JSONObject jsonData)
	{
		List<int> list = new List<int>();
		foreach (JSONObject item in jsonData.list)
		{
			list.Add((int)item.f);
		}
		return list;
	}

	public static JSONObject ConvertToJSON(List<byte> collection)
	{
		JSONObject jSONObject = new JSONObject(JSONObject.Type.ARRAY);
		foreach (byte item in collection)
		{
			jSONObject.Add(item);
		}
		return jSONObject;
	}

	public static List<byte> ConvertToByteList(JSONObject jsonData)
	{
		List<byte> list = new List<byte>();
		foreach (JSONObject item in jsonData.list)
		{
			list.Add((byte)item.f);
		}
		return list;
	}

	public static JSONObject ConvertToJSON(Vector3 vector)
	{
		JSONObject jSONObject = new JSONObject();
		jSONObject.AddField("x", vector.x);
		jSONObject.AddField("y", vector.y);
		jSONObject.AddField("z", vector.z);
		return jSONObject;
	}

	public static Vector3 ConvertToVector3(JSONObject jsonData)
	{
		Vector3 zero = Vector3.zero;
		zero.x = jsonData["x"].f;
		zero.y = jsonData["y"].f;
		zero.z = jsonData["z"].f;
		return zero;
	}

	public static JSONObject ConvertToJSON(Quaternion quaternion)
	{
		JSONObject jSONObject = new JSONObject();
		jSONObject.AddField("x", quaternion.x);
		jSONObject.AddField("y", quaternion.y);
		jSONObject.AddField("z", quaternion.z);
		jSONObject.AddField("w", quaternion.w);
		return jSONObject;
	}

	public static Quaternion ConvertToQuaternion(JSONObject jsonData)
	{
		Quaternion result = default(Quaternion);
		result.x = jsonData["x"].f;
		result.y = jsonData["y"].f;
		result.z = jsonData["z"].f;
		result.w = jsonData["w"].f;
		return result;
	}
}


