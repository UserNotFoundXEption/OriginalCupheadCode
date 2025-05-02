using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005BC RID: 1468
public class WaitForFrameTimePersistent : IEnumerator
{
	// Token: 0x06003D94 RID: 15764 RVA: 0x00031A60 File Offset: 0x0002FC60
	public WaitForFrameTimePersistent(float frameTime, bool useUnalteredTime = false)
	{
		this.frameTime = frameTime;
		this.useUnalteredTime = useUnalteredTime;
	}

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x06003D95 RID: 15765 RVA: 0x00031A76 File Offset: 0x0002FC76
	// (set) Token: 0x06003D96 RID: 15766 RVA: 0x00031A7E File Offset: 0x0002FC7E
	public float accumulator { get; set; }

	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x06003D97 RID: 15767 RVA: 0x00031A87 File Offset: 0x0002FC87
	// (set) Token: 0x06003D98 RID: 15768 RVA: 0x00031A8F File Offset: 0x0002FC8F
	public float frameTime { get; set; }

	// Token: 0x17000509 RID: 1289
	// (get) Token: 0x06003D99 RID: 15769 RVA: 0x00031A98 File Offset: 0x0002FC98
	public float totalDelta
	{
		get
		{
			return this.frameTime + this.accumulator;
		}
	}

	// Token: 0x1700050A RID: 1290
	// (get) Token: 0x06003D9A RID: 15770 RVA: 0x00031AA7 File Offset: 0x0002FCA7
	public virtual object Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003D9B RID: 15771 RVA: 0x001192F0 File Offset: 0x001174F0
	public bool MoveNext()
	{
		this.accumulator += this.deltaTime();
		bool flag = this.accumulator >= this.frameTime;
		if (flag)
		{
			this.accumulator -= Mathf.Floor(this.accumulator / this.frameTime) * this.frameTime;
		}
		return !flag;
	}

	// Token: 0x06003D9C RID: 15772 RVA: 0x00031AAA File Offset: 0x0002FCAA
	public virtual float deltaTime()
	{
		return (!this.useUnalteredTime) ? CupheadTime.Delta : Time.deltaTime;
	}

	// Token: 0x06003D9D RID: 15773 RVA: 0x00031ACB File Offset: 0x0002FCCB
	public void Reset()
	{
		this.accumulator = 0f;
	}

	// Token: 0x040030F2 RID: 12530
	public bool useUnalteredTime;
}
