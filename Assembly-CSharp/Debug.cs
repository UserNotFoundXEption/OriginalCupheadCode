using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Internal;

// Token: 0x020000CB RID: 203
public static class Debug
{
	// Token: 0x17000188 RID: 392
	// (get) Token: 0x06000986 RID: 2438 RVA: 0x00008E33 File Offset: 0x00007033
	// (set) Token: 0x06000987 RID: 2439 RVA: 0x00008E3A File Offset: 0x0000703A
	public static bool developerConsoleVisible
	{
		get
		{
			return UnityEngine.Debug.developerConsoleVisible;
		}
		set
		{
			UnityEngine.Debug.developerConsoleVisible = value;
		}
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x06000988 RID: 2440 RVA: 0x00008E42 File Offset: 0x00007042
	public static bool isDebugBuild
	{
		get
		{
			return UnityEngine.Debug.isDebugBuild;
		}
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x00008E49 File Offset: 0x00007049
	[Conditional("UNITY_ASSERTIONS")]
	public static void Assert(bool condition)
	{
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x00008E4B File Offset: 0x0000704B
	[Conditional("UNITY_ASSERTIONS")]
	public static void Assert(bool condition, string message)
	{
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x00008E4D File Offset: 0x0000704D
	[Conditional("UNITY_ASSERTIONS")]
	public static void AssertFormat(bool condition, string format, params object[] args)
	{
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x00008E4F File Offset: 0x0000704F
	public static void Break()
	{
		UnityEngine.Debug.Break();
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x00008E56 File Offset: 0x00007056
	public static void ClearDeveloperConsole()
	{
		UnityEngine.Debug.ClearDeveloperConsole();
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x00008E5D File Offset: 0x0000705D
	public static void DebugBreak()
	{
		UnityEngine.Debug.DebugBreak();
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x00008E64 File Offset: 0x00007064
	public static void DrawLine(Vector3 start, Vector3 end)
	{
		UnityEngine.Debug.DrawLine(start, end);
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x00008E6D File Offset: 0x0000706D
	public static void DrawLine(Vector3 start, Vector3 end, Color color)
	{
		UnityEngine.Debug.DrawLine(start, end, color);
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x00008E77 File Offset: 0x00007077
	public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
	{
		UnityEngine.Debug.DrawLine(start, end, color, duration);
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x00008E82 File Offset: 0x00007082
	public static void DrawLine(Vector3 start, Vector3 end, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
	{
		UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x00008E8F File Offset: 0x0000708F
	public static void DrawRay(Vector3 start, Vector3 dir)
	{
		UnityEngine.Debug.DrawRay(start, dir);
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x00008E98 File Offset: 0x00007098
	public static void DrawRay(Vector3 start, Vector3 dir, Color color)
	{
		UnityEngine.Debug.DrawRay(start, dir, color);
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x00008EA2 File Offset: 0x000070A2
	public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
	{
		UnityEngine.Debug.DrawRay(start, dir, color, duration);
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x00008EAD File Offset: 0x000070AD
	public static void DrawRay(Vector3 start, Vector3 dir, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
	{
		UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x00008EBA File Offset: 0x000070BA
	public static void LogInfo(object message, Object context = null)
	{
		UnityEngine.Debug.Log(message, context);
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x00008EC3 File Offset: 0x000070C3
	public static void LogInfoCat(params object[] args)
	{
		UnityEngine.Debug.Log(string.Concat(args));
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x00008ED0 File Offset: 0x000070D0
	[Conditional("VERBOSE")]
	public static void Log(object message, Object context = null)
	{
		UnityEngine.Debug.Log(message, context);
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x00008ED9 File Offset: 0x000070D9
	[Conditional("VERBOSE")]
	public static void LogCat(params object[] args)
	{
		UnityEngine.Debug.Log(string.Concat(args));
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x00008EE6 File Offset: 0x000070E6
	public static void LogError(object message, Object context = null)
	{
		UnityEngine.Debug.LogError(message, context);
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x00008EEF File Offset: 0x000070EF
	public static void LogErrorCat(params object[] args)
	{
		UnityEngine.Debug.LogError(string.Concat(args));
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x00008EFC File Offset: 0x000070FC
	public static void LogErrorFormat(string format, params object[] args)
	{
		UnityEngine.Debug.LogErrorFormat(format, args);
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x00008F05 File Offset: 0x00007105
	public static void LogErrorFormat(Object context, string format, params object[] args)
	{
		UnityEngine.Debug.LogErrorFormat(context, format, args);
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x00008F0F File Offset: 0x0000710F
	public static void LogException(Exception exception)
	{
		UnityEngine.Debug.LogException(exception);
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x00008F17 File Offset: 0x00007117
	public static void LogException(Exception exception, Object context)
	{
		UnityEngine.Debug.LogException(exception, context);
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x00008F20 File Offset: 0x00007120
	[Conditional("VERBOSE")]
	public static void LogFormat(string format, params object[] args)
	{
		UnityEngine.Debug.LogFormat(format, args);
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x00008F29 File Offset: 0x00007129
	[Conditional("VERBOSE")]
	public static void LogFormat(Object context, string format, params object[] args)
	{
		UnityEngine.Debug.LogFormat(context, format, args);
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x00008F33 File Offset: 0x00007133
	[Conditional("VERBOSE")]
	public static void LogWarning(object message, Object context = null)
	{
		UnityEngine.Debug.LogWarning(message, context);
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x00008F3C File Offset: 0x0000713C
	[Conditional("VERBOSE")]
	public static void LogWarningCat(params object[] args)
	{
		UnityEngine.Debug.LogWarning(string.Concat(args));
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x00008F49 File Offset: 0x00007149
	[Conditional("VERBOSE")]
	public static void LogWarningFormat(string format, params object[] args)
	{
		UnityEngine.Debug.LogWarningFormat(format, args);
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x00008F52 File Offset: 0x00007152
	[Conditional("VERBOSE")]
	public static void LogWarningFormat(Object context, string format, params object[] args)
	{
		UnityEngine.Debug.LogWarningFormat(context, format, args);
	}
}
