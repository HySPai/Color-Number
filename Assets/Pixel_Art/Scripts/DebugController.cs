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

public class DebugController : MonoBehaviour
{
	public Action<bool> OnEnableDebugTexts;

	private Action externalGui;

	[SerializeField]
	private float m_updateInterval = 0.5f;

	private float m_fps;

	private float m_accum;

	private int m_frames;

	private float m_timeleft;

	public static DebugController Instance { get; private set; }

	public bool EnableDebugTexts { get; private set; }

	private void Awake()
	{
		DebugController.Instance = this;
	}

	private void OnGUI()
	{
		if (this.externalGui != null)
		{
			this.externalGui.SafeInvoke();
		}
		else if (this.EnableDebugTexts && GUILayout.Button("fps: " + this.m_fps.ToString()))
		{
			int targetFrameRate = Application.targetFrameRate;
			UnityEngine.Debug.Log(": " + targetFrameRate);
			if (targetFrameRate < 0)
			{
				Application.targetFrameRate = 30;
			}
			else if (targetFrameRate == 30)
			{
				Application.targetFrameRate = 20;
			}
			else
			{
				Application.targetFrameRate = -1;
			}
		}
	}

	private void Update()
	{
		this.CalculateFps();
	}

	public void ShowExternalGui(Action _externalGui)
	{
		this.externalGui = _externalGui;
	}

	public void StopShowExternalGui()
	{
		this.externalGui = null;
	}

	public void ClickDebugTexts()
	{
		this.EnableDebugTexts = !this.EnableDebugTexts;
		this.OnEnableDebugTexts.SafeInvoke(this.EnableDebugTexts);
	}

	private void CalculateFps()
	{
		this.m_timeleft -= Time.deltaTime;
		this.m_accum += Time.timeScale / Time.deltaTime;
		this.m_frames++;
		if ((double)this.m_timeleft <= 0.0)
		{
			this.m_fps = this.m_accum / (float)this.m_frames;
			this.m_timeleft = this.m_updateInterval;
			this.m_accum = 0f;
			this.m_frames = 0;
		}
	}
}


