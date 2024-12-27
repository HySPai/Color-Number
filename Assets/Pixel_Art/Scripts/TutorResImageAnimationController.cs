/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;
using UnityEngine.UI;

public class TutorResImageAnimationController : MonoBehaviour
{
	[SerializeField]
	private Image m_resImage;

	private Texture2D m_startTex;

	private void Awake()
	{
		this.m_startTex = this.m_resImage.sprite.texture;
		this.ResetTexture();
	}

	public void ResetTexture()
	{
		Texture2D texture2D = new Texture2D(this.m_startTex.width, this.m_startTex.height, TextureFormat.ARGB32, false);
		texture2D.filterMode = FilterMode.Point;
		texture2D.SetPixels(this.m_startTex.GetPixels());
		texture2D.Apply();
		this.m_resImage.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)this.m_startTex.width, (float)this.m_startTex.height), new Vector2(0.5f, 0.5f));
	}

	public void AnimationEventHandler(int index)
	{
		switch (index)
		{
			case -1:
				this.ResetTexture();
				break;
			case 0:
				this.m_resImage.sprite.texture.SetPixel(12, 18, new Color(0.976f, 0.93f, 0.353f));
				break;
			case 1:
				this.m_resImage.sprite.texture.SetPixel(12, 17, new Color(0.976f, 0.93f, 0.353f));
				break;
			case 2:
				this.m_resImage.sprite.texture.SetPixel(12, 16, new Color(0.976f, 0.93f, 0.353f));
				break;
			case 3:
				this.m_resImage.sprite.texture.SetPixel(13, 16, new Color(0.976f, 0.93f, 0.353f));
				break;
			case 4:
				this.m_resImage.sprite.texture.SetPixel(13, 17, new Color(0.976f, 0.93f, 0.353f));
				break;
			case 5:
				this.m_resImage.sprite.texture.SetPixel(14, 18, new Color(0.976f, 0.93f, 0.353f));
				break;
			case 6:
				this.m_resImage.sprite.texture.SetPixel(14, 17, new Color(0.976f, 0.93f, 0.353f));
				break;
			case 7:
				this.m_resImage.sprite.texture.SetPixel(14, 16, new Color(0.976f, 0.93f, 0.353f));
				break;
			case 8:
				this.m_resImage.sprite.texture.SetPixel(14, 15, new Color(0.976f, 0.93f, 0.353f));
				break;
			case 9:
				this.m_resImage.sprite.texture.SetPixel(15, 15, new Color(0.976f, 0.93f, 0.353f));
				break;
			case 10:
				this.m_resImage.sprite.texture.SetPixel(15, 16, new Color(0.976f, 0.93f, 0.353f));
				break;
			case 11:
				this.m_resImage.sprite.texture.SetPixel(15, 17, new Color(0.976f, 0.93f, 0.353f));
				break;
			case 12:
				this.m_resImage.sprite.texture.SetPixel(16, 18, new Color(0.976f, 0.93f, 0.353f));
				break;
			case 13:
				this.m_resImage.sprite.texture.SetPixel(16, 17, new Color(0.976f, 0.93f, 0.353f));
				break;
			case 14:
				this.m_resImage.sprite.texture.SetPixel(16, 16, new Color(0.976f, 0.93f, 0.353f));
				break;
			case 15:
				this.m_resImage.sprite.texture.SetPixel(13, 15, new Color(0.976f, 0.93f, 0.353f));
				break;
		}
		this.m_resImage.sprite.texture.Apply();
	}
}


