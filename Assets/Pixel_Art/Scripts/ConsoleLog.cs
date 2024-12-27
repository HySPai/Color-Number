/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ConsoleLog : MonoBehaviour
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private struct Log
	{
		public string Message
		{
			get;
			set;
		}

		public string StackTrace
		{
			get;
			set;
		}

		public LogType Type
		{
			get;
			set;
		}
	}

	private List<Log> m_logs = new List<Log>();

	private Vector2 m_scrollPosition;

	private bool m_show;

	private bool m_collapse;

	private bool m_showStack;

	private bool m_showErrors;

	private static Dictionary<LogType, Color> s_logTypeColors = new Dictionary<LogType, Color> {
		{
			LogType.Assert,
			Color.white
		},
		{
			LogType.Error,
			Color.red
		},
		{
			LogType.Exception,
			Color.red
		},
		{
			LogType.Log,
			Color.white
		},
		{
			LogType.Warning,
			Color.yellow
		}
	};

	private const int Margin = 20;

	private Rect m_windowRect = new Rect(20f, 20f, (float)(Screen.width - 40), (float)(Screen.height - 40));

	private Rect m_titleBarRect = new Rect(0f, 0f, 10000f, 20f);

	private GUIContent m_clearLabel = new GUIContent("Clear", "Clear the contents of the console.");

	private GUIContent m_collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");

	private GUIContent m_showStackLabel = new GUIContent("Show Stack", "Hide/Show Stack Tree.");

	private GUIContent m_showErrorsLabel = new GUIContent("Only Errors", "Hide/Show Only Errors.");

	public static ConsoleLog Inst { get; private set; }

	public bool IsShown
	{
		get
		{
			return this.m_show;
		}
		set
		{
			this.m_show = value;
		}
	}

	private void Awake()
	{
		ConsoleLog.Inst = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnEnable()
	{
		Application.logMessageReceived += new Application.LogCallback(this.HandleLog);
	}

	private void OnDisable()
	{
		Application.logMessageReceived -= new Application.LogCallback(this.HandleLog);
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
		//if (DebugController.Instance.EnableDebugTexts)
		//{
		//	GUILayout.Space(50f);
		//	if (ScreenToolWrapper.IsIphoneX)
		//	{
		//		GUILayout.Space(100f);
		//	}
		//	if (GUILayout.Button("Console", GUILayout.Width(500f)))
		//	{
		//		this.m_show = !this.m_show;
		//	}
		//	if (this.m_show)
		//	{
		//		this.m_windowRect = GUILayout.Window(123456, this.m_windowRect, new GUI.WindowFunction(this.ConsoleWindow), "Console");
		//	}
		//}
	}

	private void ConsoleWindow(int windowId)
	{
		this.m_scrollPosition = GUILayout.BeginScrollView(this.m_scrollPosition);
		for (int i = 0; i < this.m_logs.Count; i++)
		{
			Log log = this.m_logs[i];
			if ((!this.m_collapse || i <= 0 || !(log.Message == this.m_logs[i - 1].Message)) && (!this.m_showErrors || this.IsError(log.Type)))
			{
				GUI.contentColor = ConsoleLog.s_logTypeColors[log.Type];
				GUILayout.Label(log.Message);
				GUI.color = Color.grey;
				if (this.m_showStack)
				{
					GUILayout.Label(log.StackTrace);
				}
				GUI.color = Color.white;
			}
		}
		GUILayout.EndScrollView();
		GUI.contentColor = Color.white;
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(this.m_clearLabel))
		{
			this.m_logs.Clear();
		}
		this.m_showStack = GUILayout.Toggle(this.m_showStack, this.m_showStackLabel, GUILayout.ExpandWidth(false));
		this.m_collapse = GUILayout.Toggle(this.m_collapse, this.m_collapseLabel, GUILayout.ExpandWidth(false));
		this.m_showErrors = GUILayout.Toggle(this.m_showErrors, this.m_showErrorsLabel, GUILayout.ExpandWidth(false));
		GUILayout.EndHorizontal();
		GUI.DragWindow(this.m_titleBarRect);
	}

	private void HandleLog(string message, string stackTrace, LogType type)
	{
		if (message.Length > 1000)
		{
			message = message.Substring(0, 5000) + "...";
		}
		this.m_logs.Add(new Log
		{
			Message = message,
			StackTrace = stackTrace,
			Type = type
		});
	}

	private bool IsError(LogType logType)
	{
		return logType == LogType.Error || logType == LogType.Exception || logType == LogType.Assert || logType == LogType.Warning;
	}
}


