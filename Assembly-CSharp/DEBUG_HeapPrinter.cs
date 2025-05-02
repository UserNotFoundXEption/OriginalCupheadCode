using System;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

// Token: 0x020005C8 RID: 1480
public class DEBUG_HeapPrinter : MonoBehaviour
{
	// Token: 0x06003DE0 RID: 15840 RVA: 0x00031CE4 File Offset: 0x0002FEE4
	public void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06003DE1 RID: 15841 RVA: 0x0011A84C File Offset: 0x00118A4C
	public void OnGUI()
	{
		if (!this.styleInitialized)
		{
			this.styleInitialized = true;
			this.style = new GUIStyle(GUI.skin.box);
			this.style.alignment = 5;
			this.style.fontStyle = 1;
			this.style.richText = true;
			this.style.fontSize = 24;
		}
		long totalMemory = GC.GetTotalMemory(false);
		long value = totalMemory - this.previousMemory;
		this.counter.Add(value);
		if (this.previousMemory > totalMemory)
		{
			this.highlightTimer = 0f;
		}
		float num = DEBUG_HeapPrinter.Size.x;
		float num2 = DEBUG_HeapPrinter.Size.y;
		string value2 = string.Empty;
		if (this.highlightTimer < DEBUG_HeapPrinter.HighlightTime)
		{
			this.highlightTimer += Time.unscaledDeltaTime;
			this.style.fontSize = DEBUG_HeapPrinter.LargeFontSize;
			this.builder.Append("<color=red>");
			value2 = "</color>";
			num *= 2f;
			num2 *= 2f;
		}
		else
		{
			this.style.fontSize = DEBUG_HeapPrinter.SmallFontSize;
		}
		long value3 = totalMemory / 1024L;
		long value4 = Profiler.GetMonoHeapSizeLong() / 1024L;
		this.builder.Append(value3);
		this.builder.Append(" / ");
		this.builder.Append(value4);
		this.builder.Append("\n");
		this.builder.Append((this.counter.Average() / 1024f).ToString("F2"));
		this.builder.Append("kb / frame");
		this.builder.Append(value2);
		GUI.Box(new Rect((float)Screen.width - num, (float)Screen.height - num2, num, num2), this.builder.ToString(), this.style);
		this.builder.Length = 0;
		this.previousMemory = totalMemory;
	}

	// Token: 0x040031B4 RID: 12724
	public static readonly Vector2 Size = new Vector2(250f, 70f);

	// Token: 0x040031B5 RID: 12725
	public static readonly float HighlightTime = 3f;

	// Token: 0x040031B6 RID: 12726
	public static readonly int SmallFontSize = 24;

	// Token: 0x040031B7 RID: 12727
	public static readonly int LargeFontSize = 50;

	// Token: 0x040031B8 RID: 12728
	public static readonly int CounterSize = 30;

	// Token: 0x040031B9 RID: 12729
	public bool styleInitialized;

	// Token: 0x040031BA RID: 12730
	public GUIStyle style;

	// Token: 0x040031BB RID: 12731
	public long previousMemory = long.MaxValue;

	// Token: 0x040031BC RID: 12732
	public float highlightTimer = float.MaxValue;

	// Token: 0x040031BD RID: 12733
	public StringBuilder builder = new StringBuilder(100);

	// Token: 0x040031BE RID: 12734
	public DEBUG_HeapPrinter.CircularCounter counter = new DEBUG_HeapPrinter.CircularCounter(DEBUG_HeapPrinter.CounterSize);

	// Token: 0x0200124E RID: 4686
	public class CircularCounter
	{
		// Token: 0x0600813F RID: 33087 RVA: 0x000564E3 File Offset: 0x000546E3
		public CircularCounter(int size)
		{
			this.values = new long[size];
		}

		// Token: 0x06008140 RID: 33088 RVA: 0x000564F7 File Offset: 0x000546F7
		public void Add(long value)
		{
			this.values[this.currentIndex] = value;
			this.currentIndex++;
			if (this.currentIndex >= this.values.Length)
			{
				this.currentIndex = 0;
			}
		}

		// Token: 0x06008141 RID: 33089 RVA: 0x0029A50C File Offset: 0x0029870C
		public float Average()
		{
			long num = 0L;
			for (int i = 0; i < this.values.Length; i++)
			{
				num += this.values[i];
			}
			return (float)num / (float)this.values.Length;
		}

		// Token: 0x04007EDB RID: 32475
		public long[] values;

		// Token: 0x04007EDC RID: 32476
		public int currentIndex;
	}
}
