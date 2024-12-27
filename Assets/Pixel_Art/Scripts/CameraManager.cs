/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	[SerializeField]
	private Rect m_bounds = default(Rect);

	[SerializeField]
	private Rect m_zeroBounds = default(Rect);

	[SerializeField]
	private float m_zoom = 1f;

	[SerializeField]
	private float m_maxZoom = 1f;

	[SerializeField]
	private RectTransform m_canvasRectTransform;

	[SerializeField]
	private Camera m_camera;

	[SerializeField]
	private Camera m_backCamera;

	[SerializeField]
	private RectTransform m_cameraSpace;

	[SerializeField]
	private Canvas m_canvas;

	[SerializeField]
	private Loupe m_loupe;

	public float Zoom
	{
		get
		{
			return this.m_zoom;
		}
	}

	public float MaxZoom
	{
		get
		{
			return this.m_maxZoom;
		}
	}

	public float RecommendMaxZoom
	{
		get
		{
			return this.m_maxZoom + 0.5f;
		}
	}

	public Vector2 CameraPos
	{
		get
		{
			return this.m_camera.transform.position;
		}
	}

	public RectTransform WorkspaceRect
	{
		get
		{
			return this.m_cameraSpace;
		}
	}

	public Loupe Loupe
	{
		get
		{
			return this.m_loupe;
		}
	}

	public float DefaultOrtoSize { get; private set; }

	public Rect DefaultRect { get; private set; }

	public void Init(float width, float height)
	{
		float offset = 0.9f;

		float halfSize = Mathf.Max(width, height) / 2f;
		this.m_bounds = new Rect(-width / halfSize / 2f - offset, -height / halfSize / 2f - 2 * offset, width / halfSize + 2 * offset, height / halfSize + 2 * offset);

		this.m_zeroBounds = new Rect(-width / halfSize / 2f, -height / halfSize / 2f, width / halfSize, height / halfSize);
		Rect zeroBounds = this.m_zeroBounds;
		zeroBounds.center += new Vector2(0f, 0.2f);
		this.m_bounds.center = new Vector2(0f, 0.2f);
		if (!MySystemInfo.IsTablet)
		{
			this.m_loupe.SetLoupePrecision(1.92f / halfSize);
		}
		else
		{
			this.m_loupe.SetLoupePrecision(2f / halfSize);
		}
		this.InternalUpdate(Vector2.zero);
		base.StartCoroutine(this.DefferedUpdate());
	}

	private void InternalUpdate(Vector2 cameraDeltaPos)
	{
		float num = 0f;
		RectTransform rectTransform = this.m_cameraSpace;
		 
		while (true)
		{
			if (!(rectTransform != null))
			{
				break;
			}
			if (!(rectTransform.name != "Canvas"))
			{
				break;
			}
			float num2 = num;
			Vector2 anchoredPosition = rectTransform.anchoredPosition;
			num = num2 + anchoredPosition.y;
			rectTransform = (RectTransform)rectTransform.parent;
		}
		float num3 = 0f - num;
		Vector2 sizeDelta = this.m_cameraSpace.sizeDelta;
		float y = 1f - (num3 + sizeDelta.y) / this.m_canvasRectTransform.rect.height;
		Vector2 sizeDelta2 = this.m_cameraSpace.sizeDelta;
		float num4 = (sizeDelta2.y + 150f) / this.m_canvasRectTransform.rect.height;
		this.m_camera.rect = new Rect(0f, y, 1f, num4);
		this.DefaultRect = this.m_camera.rect;
		float a = num4 * (float)Screen.height / (this.m_camera.rect.width * (float)Screen.width);
		a = (this.DefaultOrtoSize = Mathf.Max(a, 1.2f));
		this.m_camera.orthographicSize = a / this.m_zoom;
		float num6 = MySystemInfo.IsTablet ? (this.m_loupe.Precision / (400f / (this.m_canvasRectTransform.rect.height * this.m_camera.rect.height))) : (this.m_loupe.Precision / (500f / (this.m_canvasRectTransform.rect.height * this.m_camera.rect.height)));
		this.m_maxZoom = (a / num6);
		Rect rect = default(Rect);
		rect.height = this.m_camera.orthographicSize * 2f;
		rect.width = (float)Screen.width * this.m_camera.rect.width * rect.height / ((float)Screen.height * this.m_camera.rect.height);
		rect.center = (Vector2)this.m_camera.transform.position + cameraDeltaPos;
		Rect rect2 = (!(this.m_zoom > 1f)) ? this.m_zeroBounds : this.m_bounds;
		if (rect.width > rect2.width)
		{
			Vector2 center = rect2.center;
			float x = center.x;
			Vector2 center2 = rect.center;
			rect.center = new Vector2(x, center2.y);
		}
		else
		{
			if (rect.xMin < rect2.xMin)
			{
				rect.center += new Vector2(rect2.xMin - rect.xMin, 0f);
			}
			if (rect.xMax > rect2.xMax)
			{
				rect.center += new Vector2(rect2.xMax - rect.xMax, 0f);
			}
		}
		if (rect.height > rect2.height)
		{
			Vector2 center3 = rect.center;
			float x2 = center3.x;
			Vector2 center4 = rect2.center;
			rect.center = new Vector2(x2, center4.y);
		}
		else
		{
			if (rect.yMin < rect2.yMin)
			{
				rect.center += new Vector2(0f, rect2.yMin - rect.yMin);
			}
			if (rect.yMax > rect2.yMax)
			{
				rect.center += new Vector2(0f, rect2.yMax - rect.yMax);
			}
		}
		Transform transform = this.m_camera.transform;
		Vector2 center5 = rect.center;
		float x3 = center5.x;
		Vector2 center6 = rect.center;
		transform.position = new Vector3(x3, center6.y, -10f);
	}

	public void Move(Vector2 deltaPos)
	{
		float d = this.m_camera.orthographicSize / this.m_camera.rect.height / (float)Screen.height * 2f;
		this.InternalUpdate(-deltaPos * d);
	}

	public bool CheckMinZoom(Vector3 mousePos)
	{
		return false;
	}

	public void ChangeZoom(float koef)
	{
		this.m_zoom *= koef;
		this.m_zoom = Mathf.Max(1f, this.m_zoom);
		this.m_zoom = Mathf.Min(this.m_zoom, this.m_maxZoom);
		this.InternalUpdate(Vector2.zero);
	}

	public void EndZoom()
	{
		if (this.m_zoom > this.RecommendMaxZoom)
		{
			base.StartCoroutine(this.ToRecommendMaxZoomCoroutine());
		}
	}

	public void SwitchLoupeOnOff(bool value)
	{
		this.m_loupe.SwitchOnOff(value);
	}

	public void UpdateLoupe(Vector2 screenPos)
	{
		Ray ray = this.m_camera.ScreenPointToRay(screenPos);
		Vector2 zero = Vector2.zero;
		if (this.GetWorldPos(ray, out zero))
		{
			this.m_loupe.UpdatePosition(screenPos, zero);
		}
	}

	private bool GetWorldPos(Ray ray, out Vector2 worldPos)
	{
		RaycastHit[] array = Physics.RaycastAll(ray);
		if (array != null && array.Length > 0)
		{
			worldPos = array[0].point;
			return true;
		}
		worldPos = Vector2.zero;
		return false;
	}

	public void Scroll(float delta)
	{
		this.m_zoom *= Mathf.Pow(1.1f, delta);
		this.m_zoom = Mathf.Max(1f, this.m_zoom);
		this.m_zoom = Mathf.Min(this.m_zoom, this.m_maxZoom);
		this.InternalUpdate(Vector2.zero);
	}

	public void EnableBackCamera()
	{
		this.m_backCamera.gameObject.SetActive(true);
	}

	private IEnumerator DefferedUpdate()
	{
		yield return null;
		this.InternalUpdate(Vector2.zero);
	}


	private IEnumerator MoveZoomCoroutine(Vector2 worldPos)
	{
		var startPos = this.m_camera.transform.position;
		var startZoom = 1;
		var time = 0.3f;
		var timer = time; 
		Vector3 worldPos3D = worldPos;
		worldPos3D.z = this.m_camera.transform.position.z;
		while (true)
		{
			var deltaTime = Time.deltaTime;
			timer -= deltaTime;
			if (timer > 0)
			{
				this.m_zoom += (this.m_maxZoom - (float)startZoom) * deltaTime / time;
				this.InternalUpdate(Vector2.zero);
				worldPos3D.x = worldPos.x - worldPos.x / this.m_zoom * (float)startZoom;
				worldPos3D.y = worldPos.y - worldPos.y / this.m_zoom * (float)startZoom;
				this.m_camera.transform.position = worldPos3D;
				yield return null;
			}
			else
			{
				break;
			}
		}
		worldPos3D.x = worldPos.x - worldPos.x / this.m_maxZoom * (float)startZoom;
		worldPos3D.y = worldPos.y - worldPos.y / this.m_maxZoom * (float)startZoom;
		this.m_camera.transform.position = worldPos3D;
		this.m_zoom = this.m_maxZoom;
		this.InternalUpdate(Vector2.zero);

		yield break;
	}

	private IEnumerator ToRecommendMaxZoomCoroutine()
	{
		var step = (this.RecommendMaxZoom - this.m_zoom) / 3f;
		for(int i = 0; i < 3; i++)
		{
			this.m_zoom += step;
			this.InternalUpdate(Vector2.zero);
			yield return null;
		} 
	}
}
