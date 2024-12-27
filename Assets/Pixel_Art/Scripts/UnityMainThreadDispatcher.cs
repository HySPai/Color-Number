/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// Author: Pim de Witte (pimdewitte.com) and contributors
/// <summary>
/// A thread-safe class which holds a queue with actions to execute on the next Update() method. It can be used to make calls to the main thread for
/// things such as UI Manipulation in Unity. It was developed for use in combination with the Firebase Unity plugin, which uses separate threads for event handling
/// </summary>
public class UnityMainThreadDispatcher : MonoBehaviour {

	private static readonly Queue<Action> _executionQueue = new Queue<Action>();

	public void Update() {
		lock(_executionQueue) {
			while (_executionQueue.Count > 0) {
				_executionQueue.Dequeue().Invoke();
			}
		}
	}

	/// <summary>
	/// Locks the queue and adds the IEnumerator to the queue
	/// </summary>
	/// <param name="action">IEnumerator function that will be executed from the main thread.</param>
	public void Enqueue(IEnumerator action) {
		lock (_executionQueue) {
			_executionQueue.Enqueue (() => {
				StartCoroutine (action);
			});
		}
	}

        /// <summary>
        /// Locks the queue and adds the Action to the queue
	/// </summary>
	/// <param name="action">function that will be executed from the main thread.</param>
	public void Enqueue(Action action)
	{
		Enqueue(ActionWrapper(action));
	}
	IEnumerator ActionWrapper(Action a)
	{
		a();
		yield return null;
	}


	private static UnityMainThreadDispatcher _instance = null;

	public static bool Exists() {
		return _instance != null;
	}

	public static UnityMainThreadDispatcher Instance() {
		if (!Exists ()) {
			throw new Exception ("UnityMainThreadDispatcher could not find the UnityMainThreadDispatcher object. Please ensure you have added the MainThreadExecutor Prefab to your scene.");
		}
		return _instance;
	}


	void Awake() {
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
	}

	void OnDestroy() {
			_instance = null;
	}


}
