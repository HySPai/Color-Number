/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/


using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputReceiver3D : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IScrollHandler, IEventSystemHandler
{
	public enum TapState
	{
		None,
		Down,
		Drag,
		Multitouch,
		BrushMode
	}

	[SerializeField]
	private Canvas m_canvas;

	private DateTime m_downTime = DateTime.MinValue;

	private float m_longTapTime = 0.15f;

	private Vector3 m_tapPosition = Vector3.zero;

	private Vector2 m_mousePrev = Vector2.zero;

	private float m_touchesDelta = -3.40282347E+38f;

	public TapState CurrentTapState { get; private set; }

	private void Update()
	{
		switch (this.CurrentTapState)
		{
			case TapState.Down:
				if (!this.CheckMultiTouch())
				{
					this.CheckLongTap();
				}
				break;
			case TapState.Drag:
				this.CheckMultiTouch();
				break;
			case TapState.Multitouch:
				if (Input.touches.Length == 0)
				{
					this.CurrentTapState = TapState.None;
				}
				break;
			case TapState.BrushMode:
				if (this.CheckMultiTouch())
				{
					break;
				}
				break;
			case TapState.None:
				this.CheckMultiTouch();
				break;
		}
	}

	private bool CheckMultiTouch()
	{
		if (Input.touches.Length > 1)
		{
			Touch[] touches = Input.touches;
			Vector2 vector = touches[0].position - touches[1].position;
			this.m_touchesDelta = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);
			this.m_mousePrev = Input.mousePosition;
			if (this.CurrentTapState == TapState.BrushMode)
			{
				WorkbookModel.Instance.ColorizationModeModel.CurrentSpaceType = ColorizationModeModel.BrushType.Singular;
			}
			this.CurrentTapState = TapState.Multitouch;
			return true;
		}
		return false;
	}

	private void CheckLongTap()
	{
		if (this.m_downTime > DateTime.MinValue && (DateTime.Now - this.m_downTime).TotalSeconds > (double)this.m_longTapTime)
		{
			WorkbookModel.Instance.ColorizationModeModel.CurrentSpaceType = ColorizationModeModel.BrushType.Plural;
			this.CurrentTapState = TapState.BrushMode;
		}
	}

	public void OnPointerDown(PointerEventData data)
	{
		if (!this.CheckMultiTouch())
		{
			if (this.CurrentTapState == TapState.BrushMode)
			{
				WorkbookModel.Instance.ColorizationModeModel.CurrentSpaceType = ColorizationModeModel.BrushType.Singular;
			}
			this.CurrentTapState = TapState.Down;
			this.m_downTime = DateTime.Now;
			this.m_tapPosition = Input.mousePosition;
			this.m_mousePrev = Input.mousePosition;
		}
	}

	public void OnPointerUp(PointerEventData data)
	{
		switch (this.CurrentTapState)
		{
			case TapState.Down:
				UnityEngine.Object.FindObjectOfType<TouchManager>().CheckCube(data.position, true);
				break;
		}
		this.CurrentTapState = TapState.None;
	}

	public void OnDrag(PointerEventData data)
	{
		switch (this.CurrentTapState)
		{
			case TapState.None:
				break;
			case TapState.Down:
				this.CurrentTapState = TapState.Drag;
				break;
			case TapState.Drag:
				{
					Vector2 vector2 = (Vector2)Input.mousePosition - this.m_mousePrev;
					float num2 = vector2.x / (float)Screen.width * 180f;
					float num3 = vector2.y / (float)Screen.width * 180f;
					Quaternion rotation = Camera.main.transform.rotation;
					Vector3 eulerAngles = rotation.eulerAngles;
					float num4 = eulerAngles.x - num3;
					Vector3 eulerAngles2 = rotation.eulerAngles;
					float y = eulerAngles2.y + num2;
					if (num4 > 180f)
					{
						num4 -= 360f;
					}
					else if (num4 < -180f)
					{
						num4 += 360f;
					}
					num4 = Mathf.Clamp(num4, -80f, 80f);
					rotation.eulerAngles = new Vector3(num4, y, 0f);
					Camera.main.transform.rotation = rotation;
					this.m_mousePrev = Input.mousePosition;
					Vector3 vector3 = Vector3.zero - Camera.main.transform.position;
					if (vector3.magnitude > 0.2f)
					{
						vector3 = vector3 / vector3.magnitude * 0.2f;
					}
					Transform transform = Camera.main.transform;
					transform.position += vector3;
					break;
				}
			case TapState.Multitouch:
				{
					Touch[] touches = Input.touches;
					if (touches.Length >= 2)
					{
						Vector2 vector4 = touches[0].position - touches[1].position;
						float num5 = Mathf.Sqrt(vector4.x * vector4.x + vector4.y * vector4.y);
						float num6 = num5 / this.m_touchesDelta;
						float value = Camera.main.orthographicSize / num6;
						value = Mathf.Clamp(value, 3f, 25f);
						Vector3 vector5 = Camera.main.ScreenToWorldPoint(data.position);
						Vector3 a3 = Camera.main.ScreenToWorldPoint(this.m_mousePrev);
						Camera.main.orthographicSize = value;
						this.m_touchesDelta = num5;
						Vector3 b = Camera.main.ScreenToWorldPoint(Input.mousePosition);
						Vector3 b2 = a3 - b;
						Transform transform2 = Camera.main.transform;
						transform2.position += b2;
						float z = Mathf.InverseLerp(15f, 5f, value);
						UnitySingleton<GameController>.Instance.ChangeCamDepthPosition(z);
						this.m_mousePrev = Input.mousePosition;
					}
					break;
				}
			case TapState.BrushMode:
				{
					Vector2 vector = data.position - data.delta;
					float[] obj = new float[3] {
					1f,
					0f,
					0f
				};
					Vector2 delta = data.delta;
					obj[1] = Mathf.Abs(delta.x);
					Vector2 delta2 = data.delta;
					obj[2] = Mathf.Abs(delta2.y);
					float num = Mathf.Max(obj);
					Vector2 a = data.delta / num;
					Vector2 a2 = vector;
					TouchManager touchManager = UnityEngine.Object.FindObjectOfType<TouchManager>();
					for (int i = 1; (float)i < num; i++)
					{
						touchManager.CheckCube(a2 + (float)i * a, false);
					}
					touchManager.CheckCube(data.position, false);
					break;
				}
		}
	}

	public void OnScroll(PointerEventData data)
	{
		float orthographicSize = Camera.main.orthographicSize;
		Vector2 scrollDelta = data.scrollDelta;
		float value = orthographicSize * Mathf.Pow(1.1f, 0f - scrollDelta.y);
		value = Mathf.Clamp(value, 3f, 25f);
		Vector2 position = data.position;
		Vector3 a = Camera.main.ScreenToWorldPoint(position);
		Camera.main.orthographicSize = value;
		Vector3 b = Camera.main.ScreenToWorldPoint(position);
		Vector3 translation = a - b;
		Camera.main.transform.Translate(translation);
		float z = Mathf.InverseLerp(15f, 5f, value);
		UnitySingleton<GameController>.Instance.ChangeCamDepthPosition(z);
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			if (this.CurrentTapState == TapState.BrushMode)
			{
				this.CurrentTapState = TapState.None;
				WorkbookModel.Instance.ColorizationModeModel.CurrentSpaceType = ColorizationModeModel.BrushType.Singular;
			}
			this.m_downTime = DateTime.MinValue;
		}
	}
}


