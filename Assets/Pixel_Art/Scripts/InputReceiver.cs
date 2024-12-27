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

public class InputReceiver : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IScrollHandler, IEventSystemHandler
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

	private bool m_currentZoomLogged;

	private bool m_loupeEnabled;
	public TapState CurrentTapState { get; private set; }

	private void Start()
	{
		m_loupeEnabled = AppData.LoupeEnabled;
	}

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
				if (!this.CheckMultiTouch())
				{
					Vector3 mousePosition = Input.mousePosition;
					int height = Screen.height;
					int width = Screen.width;
					Rect rect = ((RectTransform)this.m_canvas.transform).rect;
					float num = rect.width / (float)width;
					Vector2 vector = new Vector2(mousePosition.x / (float)width * rect.width, mousePosition.y / (float)height * rect.height);
					RectTransform workspaceRect = NewWorkbookManager.Instance.CameraManager.WorkspaceRect;
					float x = vector.x;
					Vector2 size = NewWorkbookManager.Instance.CameraManager.Loupe.Size;
					float num2 = x + size.x / 4f;
					Vector2 offsetMax = workspaceRect.offsetMax;
					if (num2 > offsetMax.x)
					{
						Vector2 offsetMax2 = workspaceRect.offsetMax;
						float num3 = offsetMax2.x - vector.x;
						Vector2 size2 = NewWorkbookManager.Instance.CameraManager.Loupe.Size;
						float a = num3 - size2.x / 4f;
						a = Mathf.Max(a, (0f - workspaceRect.rect.width) / 10f);
						//NewWorkbookManager.Instance.CameraManager.Move(new Vector2(a / num * 0.05f, 0f));
						NewWorkbookManager.Instance.CameraManager.UpdateLoupe(Input.mousePosition);
						NewWorkbookManager.Instance.NumberColoring.TryClickPixel(Input.mousePosition);
					}
					else
					{
						float x2 = vector.x;
						Vector2 size3 = NewWorkbookManager.Instance.CameraManager.Loupe.Size;
						float num4 = x2 - size3.x / 4f;
						Vector2 offsetMin = workspaceRect.offsetMin;
						if (num4 < offsetMin.x)
						{
							Vector2 offsetMin2 = workspaceRect.offsetMin;
							float num5 = offsetMin2.x - vector.x;
							Vector2 size4 = NewWorkbookManager.Instance.CameraManager.Loupe.Size;
							float a2 = num5 + size4.x / 4f;
							a2 = Mathf.Min(a2, workspaceRect.rect.width / 10f);
							//NewWorkbookManager.Instance.CameraManager.Move(new Vector2(a2 / num * 0.05f, 0f));
							NewWorkbookManager.Instance.CameraManager.UpdateLoupe(Input.mousePosition);
							NewWorkbookManager.Instance.NumberColoring.TryClickPixel(Input.mousePosition);
						}
					}
					float y = vector.y;
					Vector2 size5 = NewWorkbookManager.Instance.CameraManager.Loupe.Size;
					float num6 = y + size5.y * 0.75f;
					float height2 = rect.height;
					Vector2 offsetMax3 = workspaceRect.offsetMax;
					if (num6 > height2 + offsetMax3.y)
					{
						float height3 = rect.height;
						Vector2 offsetMax4 = workspaceRect.offsetMax;
						float num7 = height3 + offsetMax4.y - vector.y;
						Vector2 size6 = NewWorkbookManager.Instance.CameraManager.Loupe.Size;
						float a3 = num7 - size6.y * 0.75f;
						a3 = Mathf.Max(a3, (0f - workspaceRect.rect.height) / 10f);
						//NewWorkbookManager.Instance.CameraManager.Move(new Vector2(0f, a3 / num * 0.05f));
						NewWorkbookManager.Instance.CameraManager.UpdateLoupe(Input.mousePosition);
						NewWorkbookManager.Instance.NumberColoring.TryClickPixel(Input.mousePosition);
					}
					else //if (!m_loupeEnabled)
					{
						float y2 = vector.y;
						float height4 = rect.height;
						Vector2 offsetMin3 = workspaceRect.offsetMin;
						if (y2 < height4 + offsetMin3.y + workspaceRect.rect.height / 10f)
						{
							float height5 = rect.height;
							Vector2 offsetMin4 = workspaceRect.offsetMin;
							float a4 = height5 + offsetMin4.y + workspaceRect.rect.height / 10f - vector.y;
							a4 = Mathf.Min(a4, workspaceRect.rect.height / 10f);
							//NewWorkbookManager.Instance.CameraManager.Move(new Vector2(0f, a4 / num * 0.05f));
							NewWorkbookManager.Instance.CameraManager.UpdateLoupe(Input.mousePosition);
							NewWorkbookManager.Instance.NumberColoring.TryClickPixel(Input.mousePosition);
						}
					}
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
				NewWorkbookManager.Instance.CameraManager.SwitchLoupeOnOff(false);
				WorkbookModel.Instance.ColorizationModeModel.CurrentSpaceType = ColorizationModeModel.BrushType.Singular;
			}
			this.CurrentTapState = TapState.Multitouch;
			this.m_currentZoomLogged = false;
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
			NewWorkbookManager.Instance.CameraManager.CheckMinZoom(Input.mousePosition);
			NewWorkbookManager.Instance.CameraManager.SwitchLoupeOnOff(true);
			NewWorkbookManager.Instance.CameraManager.UpdateLoupe(Input.mousePosition);
			AnalyticsManager.Instance.LongTapActivated();
		}
	}

	public void OnPointerDown(PointerEventData data)
	{
		if (!this.CheckMultiTouch())
		{
			if (this.CurrentTapState == TapState.BrushMode)
			{
				NewWorkbookManager.Instance.CameraManager.SwitchLoupeOnOff(false);
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
				if (!NewWorkbookManager.Instance.CameraManager.CheckMinZoom(Input.mousePosition))
				{
					NewWorkbookManager.Instance.NumberColoring.TryClickPixel(Input.mousePosition);
				}
				break;
			case TapState.Multitouch:
				NewWorkbookManager.Instance.CameraManager.EndZoom();
				break;
			case TapState.BrushMode:
				NewWorkbookManager.Instance.NumberColoring.TryClickPixel(Input.mousePosition);
				WorkbookModel.Instance.ColorizationModeModel.CurrentSpaceType = ColorizationModeModel.BrushType.Singular;
				NewWorkbookManager.Instance.CameraManager.SwitchLoupeOnOff(false);
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
				NewWorkbookManager.Instance.CameraManager.Move((Vector2)Input.mousePosition - this.m_mousePrev);
				this.m_mousePrev = Input.mousePosition;
				break;
			case TapState.Multitouch:
				{
					Touch[] touches = Input.touches;
					if (touches.Length >= 2)
					{
						Vector2 vector2 = touches[0].position - touches[1].position;
						float num2 = Mathf.Sqrt(vector2.x * vector2.x + vector2.y * vector2.y);
						float koef = num2 / this.m_touchesDelta;
						NewWorkbookManager.Instance.CameraManager.ChangeZoom(koef);
						if (!this.m_currentZoomLogged)
						{
							this.m_currentZoomLogged = true;
							AnalyticsManager.Instance.ZoomDone();
						}
						this.m_touchesDelta = num2;
						NewWorkbookManager.Instance.CameraManager.Move((Vector2)Input.mousePosition - this.m_mousePrev);
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
					for (int i = 1; (float)i < num; i++)
					{
						NewWorkbookManager.Instance.NumberColoring.TryClickPixel(a2 + (float)i * a);
					}
					if (!NewWorkbookManager.Instance.CameraManager.CheckMinZoom(Input.mousePosition))
					{
						NewWorkbookManager.Instance.NumberColoring.TryClickPixel(data.position);
					}
					NewWorkbookManager.Instance.CameraManager.UpdateLoupe(data.position);
					break;
				}
		}
	}

	public void OnScroll(PointerEventData data)
	{
		CameraManager cameraManager = NewWorkbookManager.Instance.CameraManager;
		Vector2 scrollDelta = data.scrollDelta;
		cameraManager.Scroll(scrollDelta.y);
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			if (this.CurrentTapState == TapState.BrushMode)
			{
				this.CurrentTapState = TapState.None;
				NewWorkbookManager.Instance.CameraManager.SwitchLoupeOnOff(false);
				WorkbookModel.Instance.ColorizationModeModel.CurrentSpaceType = ColorizationModeModel.BrushType.Singular;
			}
			this.m_downTime = DateTime.MinValue;
		}
	}
}


