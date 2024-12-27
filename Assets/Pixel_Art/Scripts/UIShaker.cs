/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;

public class UIShaker : MonoBehaviour
{
	private bool running = true;


	private Vector3 originPosition;
	private Quaternion originRotation;
	public float shake_decay = 0.002f;
	public float shake_intensity = .4f;

	private float temp_shake_intensity = 0;
	void Update()
	{
		if (running)
		{
			if (temp_shake_intensity > 0)
			{
				//transform.localPosition = originPosition + Random.insideUnitSphere * temp_shake_intensity;
				transform.rotation = new Quaternion(
					originRotation.x + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
					originRotation.y + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
					originRotation.z + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
					originRotation.w + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f);
				temp_shake_intensity -= shake_decay;
			}
			else
			{
				Shake();
			}
		}
	}
	void Shake()
	{
		//originPosition = transform.localPosition;
		originRotation = transform.rotation;
		temp_shake_intensity = shake_intensity;

	}
	private void Start()
	{
		Shake();
	}

	public void Pause()
	{
		this.running = false;
		base.gameObject.transform.rotation = Quaternion.identity;
		temp_shake_intensity = 0;
	}

	public void Resume()
	{
		this.running = true;
		this.Shake();
	}
}
