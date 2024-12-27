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

public class TouchManager : MonoBehaviour
{
	private RaycastHit hit;

	private Camera cam;

	private Vector3 startPosition;

	private bool isClick;

	private string nameHint;

	private void Awake()
	{
		this.cam = GameObject.Find("Main Camera").GetComponent<Camera>();
	}

	private void Update1()
	{
		if (!Physics.Raycast(this.cam.ScreenPointToRay(Input.mousePosition), out this.hit))
		{
			return;
		}
		if (Input.GetMouseButtonDown(0) && !this.isClick)
		{
			this.isClick = true;
			this.startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}

        bool hasMouseUp = true;
		if (Input.GetMouseButtonUp(0))
		{
            hasMouseUp = false;
			this.isClick = false;
			Vector3 b = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (!(Math.Abs(Vector3.Distance(this.startPosition, b)) > 5f))
			{
				if (EventSystem.current.IsPointerOverGameObject())
				{
					UnityEngine.Debug.Log("Clicked on the UI");
				}
				else if (this.hit.collider.tag == VoxConstants.Tag)
				{
					VoxCubeItem component = ((Component)this.hit.transform).GetComponent<VoxCubeItem>();
					if (component != null)
					{
						component.TochCubeItem();
					}
				}
                hasMouseUp = true;
			}
			return;
		}
        if (hasMouseUp)
        {
            if (UnitySingleton<GameController>.Instance.IsLongClick)
            {
                Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    UnityEngine.Debug.Log("Clicked on the UI");
                }
                else if (this.hit.collider.tag == VoxConstants.Tag && !this.hit.collider.name.Equals(this.nameHint))
                {
                    this.nameHint = this.hit.collider.name;
                    VoxCubeItem component2 = ((Component)this.hit.transform).GetComponent<VoxCubeItem>();
                    if (component2 != null)
                    {
                        component2.TochCubeItem();
                    }
                }
            }
        }
	}

	public void CheckCube(Vector2 touchPos, bool isVibro = true)
	{
		if (Physics.Raycast(this.cam.ScreenPointToRay(touchPos), out this.hit) && this.hit.collider.tag == VoxConstants.Tag)
		{
			VoxCubeItem component = ((Component)this.hit.transform).GetComponent<VoxCubeItem>();
			if (component != null)
			{
				int num = component.TochCubeItem();
				if (num >= 0)
				{
					if (num > 0)
					{
						UnitySingleton<GameController>.Instance.OnCubeColored(component.ColorIndex);
						UnitySingleton<GameController>.Instance.SetCubeItemProgress(component);
					}
					if (isVibro)
					{
						if (num > 0)
						{
							VibroWrapper.PlayVibroRight();
						}
						else
						{
							VibroWrapper.PlayVibroWrong();
							DailyGame.IncresetError();
						}
					}
				}
			}
		}
	}
}


