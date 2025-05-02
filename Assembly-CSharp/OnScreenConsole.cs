using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020005CB RID: 1483
public class OnScreenConsole : MonoBehaviour
{
	// Token: 0x06003DEC RID: 15852 RVA: 0x00031D84 File Offset: 0x0002FF84
	public void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		this.logger = new OnScreenConsole.OnScreenConsoleLogger();
	}

	// Token: 0x06003DED RID: 15853 RVA: 0x0011ACEC File Offset: 0x00118EEC
	public void OnGUI()
	{
		if (this.style == null)
		{
			this.style = new GUIStyle(GUI.skin.GetStyle("Box"));
			this.style.alignment = 6;
			this.style.wordWrap = true;
		}
		foreach (string text in this.logger.logQueue)
		{
			string value = text;
			if (text.Length > OnScreenConsole.MaximumStringLength)
			{
				value = text.Substring(0, OnScreenConsole.MaximumStringLength);
			}
			this.builder.AppendLine(value);
		}
		if (this.builder.Length > 0)
		{
			this.builder.Length--;
		}
		int num = (int)(OnScreenConsole.Size.x * (float)Screen.width);
		int num2 = (int)(OnScreenConsole.Size.y * (float)Screen.height);
		GUI.Box(new Rect((float)(Screen.width - num), (float)(Screen.height - num2), (float)num, (float)num2), this.builder.ToString(), this.style);
		this.builder.Length = 0;
	}

	// Token: 0x040031C1 RID: 12737
	public static readonly Vector2 Size = new Vector2(0.5f, 0.4f);

	// Token: 0x040031C2 RID: 12738
	public static readonly int MaximumStringLength = 500;

	// Token: 0x040031C3 RID: 12739
	public OnScreenConsole.OnScreenConsoleLogger logger;

	// Token: 0x040031C4 RID: 12740
	public GUIStyle style;

	// Token: 0x040031C5 RID: 12741
	public StringBuilder builder = new StringBuilder();

	// Token: 0x0200124F RID: 4687
	public class OnScreenConsoleLogger : ILogHandler
	{
		// Token: 0x06008142 RID: 33090 RVA: 0x0005652F File Offset: 0x0005472F
		public OnScreenConsoleLogger()
		{
			Debug.unityLogger.logHandler = this;
		}

		// Token: 0x06008143 RID: 33091 RVA: 0x00056562 File Offset: 0x00054762
		public void LogFormat(LogType logType, Object context, string format, params object[] args)
		{
			this.addToQueue(string.Format("[{0}, {1}] {2}", logType, Time.frameCount, string.Format(format, args)));
			this.defaultLogHandler.LogFormat(logType, context, format, args);
		}

		// Token: 0x06008144 RID: 33092 RVA: 0x0005659C File Offset: 0x0005479C
		public void LogException(Exception exception, Object context)
		{
			this.defaultLogHandler.LogException(exception, context);
		}

		// Token: 0x06008145 RID: 33093 RVA: 0x000565AB File Offset: 0x000547AB
		public void addToQueue(string value)
		{
			if (this.logQueue.Count == OnScreenConsole.OnScreenConsoleLogger.QueueSize)
			{
				this.logQueue.Dequeue();
			}
			this.logQueue.Enqueue(value);
		}

		// Token: 0x04007EDD RID: 32477
		public static readonly int QueueSize = 15;

		// Token: 0x04007EDE RID: 32478
		public Queue<string> logQueue = new Queue<string>(OnScreenConsole.OnScreenConsoleLogger.QueueSize);

		// Token: 0x04007EDF RID: 32479
		public ILogHandler defaultLogHandler = Debug.unityLogger.logHandler;
	}
}
