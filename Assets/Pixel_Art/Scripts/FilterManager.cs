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

public class FilterManager : MonoBehaviour
{
	[SerializeField]
	private List<Texture2D> m_filters;

	public Texture2D GetFilter(int filterId)
	{
		List<Texture2D> list = new List<Texture2D>();
		for (int i = 0; i < this.m_filters.Count; i++)
		{
			list.Add(this.m_filters[i]);
		}
		Texture2D item = list[1];
		list.Remove(item);
		list.Add(item);
		return list[filterId];
	}

	public List<FilterInfo> GetFilters()
	{
		List<FilterInfo> list = new List<FilterInfo>();
		for (int i = 0; i < this.m_filters.Count; i++)
		{
			list.Add(new FilterInfo(this.m_filters[i], i));
		}
		FilterInfo item = list[1];
		list.Remove(item);
		list.Add(item);
		return list;
	}
}


