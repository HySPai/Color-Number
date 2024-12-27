/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/




using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOrbit_1 : MonoBehaviour
{
	public Transform target;

	public float distance = 20f;

	public float xSpeed = 120f;

	public float ySpeed = 120f;

	public float yMinLimit = -20f;

	public float yMaxLimit = 80f;

	public float distanceMinOrto = 15f;

	public float distanceMaxOrto = 30f;

	public float distanceMinPersp = 110f;

	public float distanceMaxPersp = 150f;

	public float scrollSpeedOrto = 0.5f;

	public float scrollSpeedPerspect = 2f;

	private bool isCanRotate;

	private bool isCanZoom;

	public int speed = 4;

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

	private Rigidbody rigidbody;

	public Vector3 position;

	private float x;

	private float y;

	private void Start()
	{
		this.rigidbody = base.GetComponent<Rigidbody>();
		if (this.rigidbody != null)
		{
			this.rigidbody.freezeRotation = true;
		}
		this.distance = this.distanceMaxOrto;
		Vector3 point = new Vector3(0f, 0f, 0f - this.distance);
		Vector3 vector = base.transform.rotation * point + this.target.position;
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
		if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(0))
		{
			return;
		}
		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			this.isCanRotate = true;
		}
		else
		{
			this.isCanRotate = false;
		}
		if ((bool)this.target && this.isCanRotate && !this.isCanZoom)
		{
			float axis = Input.GetAxis("Mouse X");
			float axis2 = Input.GetAxis("Mouse Y");
			this.x += axis * this.xSpeed * this.distance * 0.02f;
			this.y -= axis2 * this.ySpeed * this.distance * 0.02f;
			this.y = MouseOrbit_1.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
			Quaternion rotation = Quaternion.Euler(this.y, this.x, 0f);
			this.distance = Mathf.Clamp(this.distance - Input.GetAxis("Mouse ScrollWheel") * 5f, this.distanceMinOrto, this.distanceMaxOrto);
			Vector3 point = new Vector3(0f, 0f, 0f - this.distance);
			Vector3 vector = rotation * point + this.target.position;
			base.transform.rotation = rotation;
			base.transform.position = vector;
		}
		if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
		{
			this.isCanZoom = true;
			this.curDist = Input.GetTouch(0).position - Input.GetTouch(1).position;
			this.prevDist = Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);
			this.touchDelta = this.curDist.magnitude - this.prevDist.magnitude;
			this.speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
			this.speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;
			float koef = this.touchDelta / 10f;
			if (this.touchDelta + this.varianceInDistances <= 1f && this.speedTouch0 > this.minPinchSpeed && this.speedTouch1 > this.minPinchSpeed)
			{
				this.ZoomIn(koef);
			}
			if (this.touchDelta + this.varianceInDistances > 1f && this.speedTouch0 > this.minPinchSpeed && this.speedTouch1 > this.minPinchSpeed)
			{
				this.ZoomOut(koef);
			}
		}
		else if (Input.touchCount == 0)
		{
			this.isCanZoom = false;
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

	private void ZoomIn(float koef = 1f)
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
		UnitySingleton<GameController>.Instance.ChangeCamDepthPosition(MouseOrbit_1.ZoomCheck(num));
	}

	private void ZoomOut(float koef = 1f)
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
		UnitySingleton<GameController>.Instance.ChangeCamDepthPosition(MouseOrbit_1.ZoomCheck(num));
	}
}


