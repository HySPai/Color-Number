/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/


using System.Collections;
using UnityEngine;
public class MonoHelper : UnitySingleton<MonoHelper>
{
	public void StartCourutine(IEnumerator coroutine)
	{
		base.StartCoroutine(coroutine);
	}

	public T FindOrCreate<T>() where T : Component
	{
		T val = (T)Object.FindObjectOfType(typeof(T));
		if (val == null)
		{
			GameObject gameObject = new GameObject();
			val = gameObject.AddComponent<T>();
			val.name = typeof(T).ToString();
		}
		return val;
	}

	public T FindOrCreate<T>(string nameObject) where T : Component
	{
		T val = (T)Object.FindObjectOfType(typeof(T));
		if (val == null)
		{
			GameObject gameObject = new GameObject();
			val = gameObject.AddComponent<T>();
			gameObject.name = nameObject;
		}
		return val;
	}

	public T GetOrCreateComponent<T>() where T : Component
	{
		T val = base.GetComponent<T>();
		if (val == null)
		{
			val = base.gameObject.AddComponent<T>();
		}
		return val;
	}
}
