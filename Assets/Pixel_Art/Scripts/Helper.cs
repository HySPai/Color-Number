/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

public static class Helper
{
	public static short[] ReverseX(this short[] array, int width, int height)
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height / 2; j++)
			{
				short num = array[i + j * width];
				array[i + j * width] = array[i + (height - 1 - j) * width];
				array[i + (height - 1 - j) * width] = num;
			}
		}
		return array;
	}

	public static short[] Double(this short[] array, int width, int height)
	{
		short[] array2 = new short[array.Length * 4];
		int num = width * 2;
		int num2 = height * 2;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				array2[i * 2 + j * 2 * num] = (array2[i * 2 + 1 + j * 2 * num] = (array2[i * 2 + (j * 2 + 1) * num] = (array2[i * 2 + 1 + (j * 2 + 1) * num] = array[i + j * width])));
			}
		}
		return array2;
	}

	public static short[] Around(this short[] array, int width, int height)
	{
		short[] array2 = new short[(width + 2) * (height + 2)];
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				array2[i + 1 + (j + 1) * (width + 2)] = array[i + j * width];
			}
		}
		return array2;
	}
}


