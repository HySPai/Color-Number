/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectPooler : ObjectPooler
{
	public GameObject GameObjectToPool;

	public int PoolSize = 20;

	public bool PoolCanExpand = true;

	protected GameObject _waitingPool;

	protected List<GameObject> _pooledGameObjects;

	protected override void FillObjectPool()
	{
		this._waitingPool = new GameObject("[SimpleObjectPooler] " + base.name);
		this._pooledGameObjects = new List<GameObject>();
		for (int i = 0; i < this.PoolSize; i++)
		{
			this.AddOneObjectToThePool();
		}
	}

	public void DisableAll()
	{
		if (this._pooledGameObjects != null)
		{
			for (int i = 0; i < this._pooledGameObjects.Count; i++)
			{
				if (this._pooledGameObjects[i].gameObject != null && this._pooledGameObjects[i].gameObject.activeInHierarchy)
				{
					this._pooledGameObjects[i].gameObject.SetActive(false);
				}
			}
		}
	}

	public override GameObject GetPooledGameObject()
	{
		for (int i = 0; i < this._pooledGameObjects.Count; i++)
		{
			if (!this._pooledGameObjects[i].gameObject.activeInHierarchy)
			{
				return this._pooledGameObjects[i];
			}
		}
		if (this.PoolCanExpand)
		{
			return this.AddOneObjectToThePool();
		}
		return null;
	}

	protected virtual GameObject AddOneObjectToThePool()
	{
		if (this.GameObjectToPool == null)
		{
			UnityEngine.Debug.LogWarning("The " + base.gameObject.name + " ObjectPooler doesn't have any GameObjectToPool defined.", base.gameObject);
			return null;
		}
		GameObject gameObject = Object.Instantiate(this.GameObjectToPool);
		gameObject.gameObject.SetActive(false);
		gameObject.transform.parent = this._waitingPool.transform;
		gameObject.name = this.GameObjectToPool.name + "-" + this._pooledGameObjects.Count;
		this._pooledGameObjects.Add(gameObject);
		return gameObject;
	}
}


