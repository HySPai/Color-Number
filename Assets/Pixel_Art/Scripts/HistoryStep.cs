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
using System.Text;

[Serializable]
public class HistoryStep
{
	public List<ShortVector2> Vectors { get; private set; }

	public void Add(ShortVector2 vector)
	{
		if (this.Vectors == null)
		{
			this.Vectors = new List<ShortVector2>();
		}
		this.Vectors.Add(vector);
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < this.Vectors.Count; i++)
		{
			stringBuilder.Append(this.Vectors[i]).Append(";");
		}
		return string.Format("{0}", stringBuilder);
	}
}


