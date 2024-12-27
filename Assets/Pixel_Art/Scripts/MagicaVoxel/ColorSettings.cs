/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VoxDLL
{
	public class ColorSettings
	{
		private List<Color> colors = new List<Color>();

		private IDictionary<Color, int> dicColors = new Dictionary<Color, int>();

		public ColorSettings(Color[] pallete)
		{
			this.colors = pallete.ToList<Color>().Distinct<Color>().ToList<Color>();
			//this.colors.Remove(Color.white);
			//this.colors.Remove(new Color32(75, 75, 75, 255));
			this.colors.Remove(new Color32(0, 0, 0, 0));
			int num = 0;
			foreach (Color color in this.colors)
			{
				this.dicColors.Add(color, num);
				num++;
			}
		}

		internal Color GetColorByIndex(int value)
		{
			if (value == -1)
			{
				return Color.white;
			}
			KeyValuePair<Color, int> keyValuePair = this.dicColors.FirstOrDefault<KeyValuePair<Color, int>>((KeyValuePair<Color, int> x) => x.Value == value);
			return keyValuePair.Key;
		}

		internal IDictionary<Color, int> GetColorPallete()
		{
			return this.dicColors;
		}

		internal List<Color> GetColorPalleteList()
		{
			return this.colors;
		}

		internal int GetNumberByColor(Color color)
		{
			if (color == Color.white)
			{
				return -1;
			}
			KeyValuePair<Color, int> keyValuePair = this.dicColors.FirstOrDefault<KeyValuePair<Color, int>>((KeyValuePair<Color, int> x) => x.Key == color);
			return keyValuePair.Value;
		}
	}
}
