/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using System.Collections;
using UnityEngine;

public class ColoringVideo : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer m_grayRenderer;

	[SerializeField]
	private MeshRenderer m_gridRenderer;

	[SerializeField]
	private MeshRenderer m_resRenderer;

	[SerializeField]
	private Camera m_videoCamera;

	private int m_width;

	private Color[] m_pixels;

	private ISavedWorkData m_savedWorkData;

	public void Init(Texture2D tex, ISavedWorkData savedWorkData)
	{
		base.gameObject.SetActive(true);
		this.m_savedWorkData = savedWorkData;
		this.m_grayRenderer.sharedMaterial.mainTexture = tex;
		this.m_gridRenderer.sharedMaterial.mainTextureScale = new Vector2((float)tex.width, (float)tex.height);
		this.m_resRenderer.sharedMaterial = new Material(Shader.Find("tk2d/BlendVertexColor"));
		this.m_videoCamera.orthographicSize = NewWorkbookManager.Instance.CameraManager.DefaultOrtoSize;
		this.m_videoCamera.rect = NewWorkbookManager.Instance.CameraManager.DefaultRect;
		this.m_pixels = tex.GetPixels();
		this.m_width = tex.width;
		Texture2D texture2D = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);
		texture2D.filterMode = FilterMode.Point;
		Color[] array = new Color[this.m_pixels.Length];
		Color color = new Color(1f, 1f, 1f, 0f);
		Color white = Color.white;
		for (int i = 0; i < array.Length; i++)
		{
			if (this.m_pixels[i] == white)
			{
				array[i] = white;
			}
			else
			{
				array[i] = color;
			}
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		this.m_resRenderer.sharedMaterial.mainTexture = texture2D;
		base.StartCoroutine(this.ColoringCoroutine());
	}

	private IEnumerator ColoringCoroutine()
	{
		int width = ((Texture2D)this.m_resRenderer.sharedMaterial.mainTexture).width;
		int height = ((Texture2D)this.m_resRenderer.sharedMaterial.mainTexture).height;
		int counter = 0;
		int stepSize = this.m_savedWorkData.History2.Steps.Count / 7 / 30;
		for (int i = 0; i < this.m_savedWorkData.History2.Steps.Count; i++)
		{
			HistoryStep step = this.m_savedWorkData.History2.Steps[i];
			foreach (ShortVector2 vector in step.Vectors)
			{
				((Texture2D)this.m_resRenderer.sharedMaterial.mainTexture).SetPixel(vector.X, vector.Y, this.m_pixels[vector.X + vector.Y * width]);
			}
			counter += step.Vectors.Count;
			if (counter >= stepSize)
			{
				((Texture2D)this.m_resRenderer.sharedMaterial.mainTexture).Apply();
				yield return null;
			}
		}
		((Texture2D)this.m_resRenderer.sharedMaterial.mainTexture).Apply();
		base.gameObject.SetActive(false);
	}
}
