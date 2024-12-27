/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System.Reflection;

namespace Core.Serialization
{
	public class FieldAccessor : IAccessor
	{
		private FieldInfo m_fi;

		public string Name
		{
			get
			{
				return this.m_fi.Name;
			}
		}

		public FieldAccessor(FieldInfo fi)
		{
			this.m_fi = fi;
		}

		public object GetValue(object obj)
		{
			return this.m_fi.GetValue(obj);
		}

		public void SetValue(object obj, object value)
		{
			this.m_fi.SetValue(obj, value);
		}
	}
}


