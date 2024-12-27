/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System.Globalization;
using System.Text;

public static class CryptoScript
{
	public static string Crypt(string text)
	{
		int length = text.Length;
		string text2 = "very";
		string text3 = "very_";
		string text4 = "hard";
		string text5 = "password";
		string text6 = 60.ToString(CultureInfo.InvariantCulture);
		string text7 = text2 + text3 + text3 + text2 + text6 + text4;
		return CryptoScript.Algorithm(Encoding.UTF8.GetBytes(text), Encoding.UTF8.GetBytes(text7), text7.Length, Encoding.UTF8);
	}

	public static string Crypt(byte[] bytes)
	{
		int num = bytes.Length;
		string text = "very";
		string text2 = "very_";
		string text3 = "hard";
		string text4 = "password";
		string text5 = 60.ToString(CultureInfo.InvariantCulture);
		string text6 = text + text2 + text2 + text + text5 + text3;
		return CryptoScript.Algorithm(bytes, Encoding.UTF8.GetBytes(text6), text6.Length, Encoding.UTF8);
	}

	private static string Algorithm(byte[] text, byte[] password, int passworLength, Encoding encoding)
	{
		int num = 1;
		byte[] array = new byte[text.Length];
		for (int i = 0; i < text.Length; i++)
		{
			array[i] = (byte)(text[i] ^ password[i % passworLength]);
		}
		return encoding.GetString(array, 0, array.Length);
	}

	public static string Crypt(string text, Encoding encoding)
	{
		int length = text.Length;
		string text2 = "very";
		string text3 = "very_";
		string text4 = "hard";
		string text5 = "password";
		string text6 = 60.ToString(CultureInfo.InvariantCulture);
		string text7 = text2 + text3 + text3 + text2 + text6 + text4;
		return CryptoScript.Algorithm(encoding.GetBytes(text), encoding.GetBytes(text7), text7.Length, encoding);
	}

	public static string Crypt(byte[] bytes, Encoding encoding)
	{
		int num = bytes.Length;
		string text = "very";
		string text2 = "very_";
		string text3 = "hard";
		string text4 = "password";
		string text5 = 60.ToString(CultureInfo.InvariantCulture);
		string text6 = text + text2 + text2 + text + text5 + text3;
		return CryptoScript.Algorithm(bytes, encoding.GetBytes(text6), text6.Length, encoding);
	}
}


