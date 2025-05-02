using System;
using UnityEngine;

// Token: 0x02000054 RID: 84
public class FramerateCounter : MonoBehaviour
{
	// Token: 0x1700012A RID: 298
	// (get) Token: 0x060004C1 RID: 1217 RVA: 0x00005628 File Offset: 0x00003828
	// (set) Token: 0x060004C2 RID: 1218 RVA: 0x0000562F File Offset: 0x0000382F
	public static FramerateCounter Current { get; set; }

	// Token: 0x060004C3 RID: 1219 RVA: 0x0006ACAC File Offset: 0x00068EAC
	public static void Init()
	{
		if (FramerateCounter.Current == null)
		{
			GameObject gameObject = new GameObject("Framerate Counter");
			FramerateCounter.Current = gameObject.AddComponent<FramerateCounter>();
			Object.DontDestroyOnLoad(gameObject);
		}
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x00005637 File Offset: 0x00003837
	public virtual void Start()
	{
		this.timeleft = this.updateInterval;
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x0006ACE8 File Offset: 0x00068EE8
	public void Update()
	{
		this.timeleft -= Time.deltaTime;
		this.accum += Time.timeScale / Time.deltaTime;
		this.frames++;
		if ((double)this.timeleft <= 0.0)
		{
			float num = this.accum / (float)this.frames;
			string text = string.Format("{0:F2} FPS\n{1} HP", num, this.hpCounter);
			this.text = text;
			if (num < 10f)
			{
				this.color = "red";
			}
			else if (num < 30f)
			{
				this.color = "orange";
			}
			else
			{
				this.color = "lime";
			}
			this.timeleft = this.updateInterval;
			this.accum = 0f;
			this.frames = 0;
		}
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x0006ADD4 File Offset: 0x00068FD4
	public virtual void OnGUI()
	{
		if (!FramerateCounter.SHOW)
		{
			return;
		}
		if (this.style == null)
		{
			this.style = new GUIStyle(GUI.skin.label);
			this.style.alignment = 2;
			this.style.richText = true;
			this.style.padding = new RectOffset(20, 20, 20, 20);
		}
		GUI.Label(new Rect(0f, 0f, (float)(Screen.width + 1), (float)(Screen.height + 1)), "<color=black>" + this.text + "</color>", this.style);
		GUI.Label(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), string.Concat(new string[]
		{
			"<color=",
			this.color,
			">",
			this.text,
			"</color>"
		}), this.style);
	}

	// Token: 0x0400046C RID: 1132
	public static bool SHOW;

	// Token: 0x0400046D RID: 1133
	public float updateInterval = 0.25f;

	// Token: 0x0400046E RID: 1134
	public int hpCounter;

	// Token: 0x0400046F RID: 1135
	public float accum;

	// Token: 0x04000470 RID: 1136
	public int frames;

	// Token: 0x04000471 RID: 1137
	public float timeleft;

	// Token: 0x04000472 RID: 1138
	public GUIStyle style;

	// Token: 0x04000473 RID: 1139
	public string text;

	// Token: 0x04000474 RID: 1140
	public string color = "white";
}
