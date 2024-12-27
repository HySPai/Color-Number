/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;

public class Loupe : MonoBehaviour
{
	[SerializeField]
	private Transform m_loupeCamera;

	[SerializeField]
	private Canvas m_canvas;

	[SerializeField]
	private Color m_color = Color.white;

	[SerializeField]
	private RectTransform m_loupeRect;

	public float Precision { get; private set; }

	public Vector2 Size
	{
		get
		{
			return ((RectTransform)base.transform).sizeDelta;
		}
	}

	public void UpdatePosition(Vector3 screenPos, Vector3 worldPos)
	{
		Vector2 v = screenPos / this.m_canvas.scaleFactor;
		Vector2 sizeDelta = ((RectTransform)base.transform).sizeDelta;
		float height = ((RectTransform)this.m_canvas.transform).rect.height;
		float width = ((RectTransform)this.m_canvas.transform).rect.width;
		v.x -= ((RectTransform)this.m_canvas.transform).rect.width / 2f;
		v.y -= height / 2f;
		if (v.y + sizeDelta.y > height / 2f)
		{
			Vector2 size = this.Size;
			float num = size.y / 2f - Mathf.Abs(height / 2f - (v.y + sizeDelta.y));
			Vector2 size2 = this.Size;
			float x = size2.x;
			Vector2 size3 = this.Size;
			float num2 = Mathf.Sqrt(x * size3.x / 4f - num * num);
			v.x -= num2;
			v.y = height / 2f - sizeDelta.y / 2f;
			if (v.x - sizeDelta.x / 2f < (0f - width) / 2f)
			{
				v.x += num2 * 2f;
			}
		}
		else
		{
			v.y += sizeDelta.y / 2f;
		}
		float x2 = v.x;
		Vector2 size4 = this.Size;
		if (x2 + size4.x / 4f > width / 2f)
		{
			float x3 = v.x;
			float num3 = width / 2f - v.x;
			Vector2 size5 = this.Size;
			v.x = x3 + (num3 - size5.x / 4f);
		}
		else
		{
			float x4 = v.x;
			Vector2 size6 = this.Size;
			if (x4 - size6.x / 4f < (0f - width) / 2f)
			{
				float x5 = v.x;
				float num4 = (0f - width) / 2f - v.x;
				Vector2 size7 = this.Size;
				v.x = x5 + (num4 + size7.x / 4f);
			}
		}
		base.transform.localPosition = v;
		Vector3 position = this.m_loupeCamera.position;
		worldPos.z = position.z;
		this.m_loupeCamera.position = worldPos;
	}

	public void SwitchOnOff(bool value)
	{
		if (AppData.LoupeEnabled)
		{
			base.gameObject.SetActive(value);
			this.m_loupeCamera.gameObject.SetActive(value);
		}
	}

	public void SetLoupePrecision(Camera mainCamera, float koef)
	{
		float height = ((RectTransform)this.m_canvas.transform).rect.height;
		float num = this.m_loupeRect.rect.height / (height * mainCamera.rect.height) * mainCamera.orthographicSize;
		((Component)this.m_loupeCamera).GetComponent<Camera>().orthographicSize = num * koef;
	}

	public void SetLoupePrecision(float camSize)
	{
		this.Precision = camSize;
		((Component)this.m_loupeCamera).GetComponent<Camera>().orthographicSize = camSize;
	}
}


