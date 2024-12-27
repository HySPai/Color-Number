/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/




using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOrbit : MonoBehaviour
{
	private enum Directional
	{
		up,
		down,
		right,
		left
	}

	private enum SwipeType
	{
		undefined,
		zoom,
		move
	}

	public Transform target;

	public float distance = 20f;

	public float xSpeedRotate = 40f;

	public float ySpeedRotate = 40f;

	public float xSpeedMove = 1f;

	public float ySpeedMove = 1f;

	public float ySpeedZoom = 50f;

	public int speedToZeroMove = 2;

	public float yMinLimit = -20f;

	public float yMaxLimit = 80f;

	public float distanceMinOrto = 15f;

	public float distanceMaxOrto = 30f;

	public float distanceMinPersp = 110f;

	public float distanceMaxPersp = 150f;

	public float scrollSpeedOrto = 0.5f;

	public float scrollSpeedPerspect = 2f;

	public float intensTouchForMoveAndZoom = 5f;

	private bool isRotate;

	private bool isZoom;

	private bool isMove;

	public Camera selectedCamera;

	public float MINSCALE = 2f;

	public float MAXSCALE = 5f;

	public float minPinchSpeed = 5f;

	public float varianceInDistances = 5f;

	private float touchDelta;

	private Vector2 prevDist = new Vector2(0f, 0f);

	private Vector2 curDist = new Vector2(0f, 0f);

	private float speedTouch0;

	private float speedTouch1;

	private Vector2 startPos1 = Vector2.zero;

	private Vector2 startPos2 = Vector2.zero;

	private Vector2 movePos1 = Vector2.zero;

	private Vector2 movePos2 = Vector2.zero;

	private string text2 = string.Empty;

	private Rigidbody rigidbody;

	public Vector3 position;

	private Vector3 offset;

	private float x;

	private float y;

	private Vector2 direction1;

	private Vector2 direction2;

	private List<Directional> touchDirectional;

	private float startClickTime;

	private float endClickTime;

	private bool isClick;

	private void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		this.touchDirectional = new List<Directional>();
		this.rigidbody = base.GetComponent<Rigidbody>();
		if (this.rigidbody != null)
		{
			this.rigidbody.freezeRotation = true;
		}
		this.distance = this.distanceMaxOrto;
		Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
		Vector3 point = new Vector3(0f, 0f, 0f - this.distance);
		Vector3 vector = rotation * point + this.target.position;
	}

	public void UpdateAngles()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		this.x = eulerAngles.y;
		this.y = eulerAngles.x;
		if (this.y > this.yMaxLimit)
		{
			this.y -= 360f;
		}
		else if (this.x < this.yMinLimit)
		{
			this.y += 360f;
		}
	}

	private void Update()
	{
		if (EventSystem.current != null)
		{
			if (EventSystem.current.IsPointerOverGameObject(0))
			{
				return;
			}
			if (EventSystem.current.IsPointerOverGameObject(1))
			{
				return;
			}
		}
		if (Input.touchCount == 1)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Began)
			{
				this.startClickTime = Time.time;
				this.isClick = true;
				this.text2 = "Touch start ";
			}
			if (this.isClick && Time.time - this.startClickTime > 1f)
			{
				UnitySingleton<GameController>.Instance.IsLongClick = true;
				this.text2 = "isLongClick ";
			}
			if (Input.GetTouch(0).phase == TouchPhase.Moved && !UnitySingleton<GameController>.Instance.IsLongClick)
			{
				this.isRotate = true;
				this.isClick = false;
				UnitySingleton<GameController>.Instance.IsLongClick = false;
				this.text2 = "Touch move ";
			}
		}
		else
		{
			this.isRotate = false;
			this.isClick = false;
			UnitySingleton<GameController>.Instance.IsLongClick = false;
			this.text2 = "Touch stop ";
		}
		if (!UnitySingleton<GameController>.Instance.IsLongClick)
		{
			if ((bool)this.target && this.isRotate && !this.isZoom && !this.isMove)
			{
				this.x += Input.GetAxis("Mouse X") * this.xSpeedRotate * this.distance * 0.02f;
				this.y -= Input.GetAxis("Mouse Y") * this.ySpeedRotate * this.distance * 0.02f;
				if (Math.Abs(Input.GetAxis("Mouse X")) > 20f && Math.Abs(Input.GetAxis("Mouse Y")) > 20f)
				{
					this.isClick = false;
					UnitySingleton<GameController>.Instance.IsLongClick = false;
					this.text2 = "Touch move " + this.x + this.y;
				}
				this.y = MouseOrbit.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
				Quaternion rotation = Quaternion.Euler(this.y, this.x, 0f);
				this.distance = Mathf.Clamp(this.distance - Input.GetAxis("Mouse ScrollWheel") * 5f, this.distanceMinOrto, this.distanceMaxOrto);
				Vector3 vector = new Vector3(0f, 0f, 0f - this.distance);
				base.transform.rotation = rotation;
				base.transform.position = Vector3.Lerp(base.transform.position, Vector3.zero, Time.deltaTime * (float)this.speedToZeroMove);
			}
			if (Input.touchCount == 2)
			{
				this.curDist = Input.GetTouch(0).position - Input.GetTouch(1).position;
				this.prevDist = Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);
				this.direction1 = Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition;
				this.direction2 = Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition;
				this.touchDelta = this.curDist.magnitude - this.prevDist.magnitude;
				this.speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
				this.speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;
				float koef = this.touchDelta / this.ySpeedZoom;
				if (!this.isZoom && !this.isMove)
				{
					Touch[] touches = Input.touches;
					if (touches[0].phase == TouchPhase.Began && touches[1].phase == TouchPhase.Began)
					{
						this.startPos1 = touches[0].position;
						this.startPos2 = touches[1].position;
					}
					if (touches[0].phase == TouchPhase.Moved && touches[1].phase == TouchPhase.Moved && (this.startPos1 - touches[0].position).magnitude > 10f && (this.startPos2 - touches[1].position).magnitude > 10f)
					{
						this.movePos1 = touches[0].position;
						this.movePos2 = touches[1].position;
						if (this.movePos1.y > this.startPos1.y)
						{
							this.touchDirectional.Add(Directional.right);
						}
						else if (this.movePos1.y < this.startPos1.y)
						{
							this.touchDirectional.Add(Directional.left);
						}
						if (this.movePos1.x > this.startPos1.x)
						{
							this.touchDirectional.Add(Directional.up);
						}
						else if (this.movePos1.x < this.startPos1.x)
						{
							this.touchDirectional.Add(Directional.down);
						}
						if (this.movePos2.y > this.startPos2.y)
						{
							this.touchDirectional.Add(Directional.right);
						}
						else if (this.movePos2.y < this.startPos2.y)
						{
							this.touchDirectional.Add(Directional.left);
						}
						if (this.movePos2.x > this.startPos2.x)
						{
							this.touchDirectional.Add(Directional.up);
						}
						else if (this.movePos2.x < this.startPos2.x)
						{
							this.touchDirectional.Add(Directional.down);
						}
						this.ZommOrMove(koef);
					}
				}
				else
				{
					this.ZommOrMove(koef);
				}
			}
			else if (Input.touchCount == 0)
			{
				this.isZoom = false;
				this.isMove = false;
				this.isMove = false;
				this.touchDirectional = new List<Directional>();
			}
			if (Input.GetKeyUp(KeyCode.B))
			{
				if (this.selectedCamera.orthographic)
				{
					this.selectedCamera.orthographic = false;
				}
				else
				{
					this.selectedCamera.orthographic = true;
				}
			}
		}
	}

	private void OnGUI()
	{
		if (VoxConstants.isTest)
		{
			int width = Screen.width;
			int height = Screen.height;
			GUIStyle gUIStyle = new GUIStyle();
			Rect rect = new Rect(0f, 0f, (float)width, (float)(height * 2 / 100));
			Rect rect2 = new Rect(0f, 200f, (float)width, (float)(height * 2 / 100));
			gUIStyle.alignment = TextAnchor.UpperLeft;
			gUIStyle.fontSize = height * 2 / 100;
			gUIStyle.normal.textColor = new Color(0f, 0f, 0.5f, 1f);
			string text = this.touchDelta.ToString();
			if (this.touchDirectional != null)
			{
				foreach (Directional item in this.touchDirectional)
				{
					this.text2 = this.text2 + item.ToString() + " ";
				}
			}
			GUI.Label(rect, text, gUIStyle);
			GUI.Label(rect2, this.text2, gUIStyle);
		}
	}

	public static float ZoomLimit(float dist, float min, float max)
	{
		if (dist < min)
		{
			dist = min;
		}
		if (dist > max)
		{
			dist = max;
		}
		return dist;
	}

	public static float ZoomCheck(float dist)
	{
		if (dist < 0f)
		{
			dist = 0f;
		}
		if (dist >= 1f)
		{
			dist = 1f;
		}
		return dist;
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	private void ZommOrMove(float koef)
	{
		if (this.CheckDirectional() == SwipeType.zoom)
		{
			this.isZoom = true;
			if (this.touchDelta + this.varianceInDistances <= 1f && this.speedTouch0 > this.minPinchSpeed && this.speedTouch1 > this.minPinchSpeed)
			{
				this.ZoomOut(koef);
			}
			if (this.touchDelta + this.varianceInDistances > 1f && this.speedTouch0 > this.minPinchSpeed && this.speedTouch1 > this.minPinchSpeed)
			{
				this.ZoomIn(koef);
			}
		}
		if (this.CheckDirectional() == SwipeType.move)
		{
			this.isMove = true;
			Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;
			Vector2 deltaPosition2 = Input.GetTouch(1).deltaPosition;
			base.transform.Translate((0f - deltaPosition.x) * this.xSpeedMove * this.distance * 0.02f, (0f - deltaPosition.y) * this.xSpeedMove * this.distance * 0.02f, 0f);
		}
	}

	private void ZoomOut(float koef = 1f)
	{
		float num = 1f;
		if (this.selectedCamera.fieldOfView <= this.distanceMaxPersp)
		{
			this.selectedCamera.fieldOfView += this.scrollSpeedPerspect;
		}
		if (this.selectedCamera.orthographicSize <= this.distanceMaxOrto)
		{
			this.selectedCamera.orthographicSize -= this.scrollSpeedOrto * koef;
		}
		num = ((!this.selectedCamera.orthographic) ? (1f - (this.selectedCamera.fieldOfView - this.distanceMinPersp) / (this.distanceMaxPersp - this.distanceMinPersp)) : (1f - (this.selectedCamera.orthographicSize - this.distanceMinOrto) / (this.distanceMaxOrto - this.distanceMinOrto)));
		base.transform.position = Vector3.Lerp(base.transform.position, Vector3.zero, Time.deltaTime * (float)this.speedToZeroMove * 5f);
		UnitySingleton<GameController>.Instance.ChangeCamDepthPosition(MouseOrbit.ZoomCheck(num));
	}

	private void ZoomIn(float koef = 1f)
	{
		float num = 1f;
		if (this.selectedCamera.fieldOfView > this.distanceMinPersp)
		{
			this.selectedCamera.fieldOfView -= this.scrollSpeedPerspect;
		}
		if (this.selectedCamera.orthographicSize >= this.distanceMinOrto)
		{
			this.selectedCamera.orthographicSize -= this.scrollSpeedOrto * koef;
		}
		num = ((!this.selectedCamera.orthographic) ? (1f - (this.selectedCamera.fieldOfView - this.distanceMinPersp) / (this.distanceMaxPersp - this.distanceMinPersp)) : (1f - (this.selectedCamera.orthographicSize - this.distanceMinOrto) / (this.distanceMaxOrto - this.distanceMinOrto)));
		UnitySingleton<GameController>.Instance.ChangeCamDepthPosition(MouseOrbit.ZoomCheck(num));
	}

	private SwipeType CheckDirectional()
	{
		if (this.touchDirectional.Count == 4)
		{
			if (this.touchDirectional[0] != this.touchDirectional[2] && this.touchDirectional[1] != this.touchDirectional[3])
			{
				return SwipeType.zoom;
			}
			return SwipeType.move;
		}
		return SwipeType.undefined;
	}
}


