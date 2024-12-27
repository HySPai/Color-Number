/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System;

namespace Core.Serialization
{
	public class ShowInInspector : Attribute
	{
		public string FieldName
		{
			get;
			set;
		}

		public string ToolTip
		{
			get;
			protected set;
		}

		public bool ReadOnly
		{
			get;
			set;
		}

		public bool StaticArraySize
		{
			get;
			set;
		}

		public ShowInInspector()
			: this(false)
		{
		}

		public ShowInInspector(bool readOnly)
			: this(null, null, readOnly)
		{
		}

		public ShowInInspector(string fieldName, string toolTip = "", bool readOnly = false)
			: this(fieldName, toolTip, readOnly, false)
		{
		}

		public ShowInInspector(string fieldName, string toolTip, bool readOnly, bool staticArraySize)
		{
			this.FieldName = fieldName;
			this.ToolTip = toolTip;
			this.ReadOnly = readOnly;
			this.StaticArraySize = staticArraySize;
		}
	}
}


