using System;
using UnityEngine;

// Token: 0x020000FE RID: 254
public abstract class AbstractLevelState<PATTERN, STATE_NAMES>
{
	// Token: 0x06000BDA RID: 3034 RVA: 0x00081A94 File Offset: 0x0007FC94
	public AbstractLevelState(float healthTrigger, PATTERN[][] patterns, STATE_NAMES stateName)
	{
		this.healthTrigger = Mathf.Clamp(healthTrigger, 0f, 1f);
		this.patterns = patterns[Random.Range(0, patterns.Length)];
		this.patternIndex = Random.Range(0, this.patterns.Length);
		this.stateName = stateName;
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x06000BDB RID: 3035 RVA: 0x0000A7AB File Offset: 0x000089AB
	public PATTERN NextPattern
	{
		get
		{
			this.patternIndex++;
			if (this.patternIndex >= this.patterns.Length)
			{
				this.patternIndex = 0;
			}
			return this.patterns[this.patternIndex];
		}
	}

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x06000BDC RID: 3036 RVA: 0x00081AEC File Offset: 0x0007FCEC
	public PATTERN PeekNextPattern
	{
		get
		{
			int num = this.patternIndex + 1;
			if (num >= this.patterns.Length)
			{
				num = 0;
			}
			return this.patterns[num];
		}
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x06000BDD RID: 3037 RVA: 0x0000A7E6 File Offset: 0x000089E6
	public PATTERN CurrentPattern
	{
		get
		{
			return this.patterns[this.patternIndex];
		}
	}

	// Token: 0x0400097B RID: 2427
	public readonly float healthTrigger;

	// Token: 0x0400097C RID: 2428
	public readonly PATTERN[] patterns;

	// Token: 0x0400097D RID: 2429
	public readonly STATE_NAMES stateName;

	// Token: 0x0400097E RID: 2430
	public int patternIndex;
}
