using System;
using UnityEngine;

// Token: 0x020002F3 RID: 755
public class PirateLevelBoatSail : AbstractMonoBehaviour
{
	// Token: 0x06002198 RID: 8600 RVA: 0x0001CB5A File Offset: 0x0001AD5A
	public void RegularEnded()
	{
		if (this.reg >= this.regTarget)
		{
			this.StartFast();
			return;
		}
		this.reg++;
	}

	// Token: 0x06002199 RID: 8601 RVA: 0x0001CB82 File Offset: 0x0001AD82
	public void FastEnded()
	{
		if (this.fast >= this.fastTarget)
		{
			this.StartReg();
			return;
		}
		this.fast++;
	}

	// Token: 0x0600219A RID: 8602 RVA: 0x0001CBAA File Offset: 0x0001ADAA
	public void StartReg()
	{
		this.regTarget = Random.Range(this.regularLoopsMin, this.regularLoopsMax + 1);
		this.reg = 0;
		base.animator.SetBool("Fast", false);
	}

	// Token: 0x0600219B RID: 8603 RVA: 0x0001CBDD File Offset: 0x0001ADDD
	public void StartFast()
	{
		this.fastTarget = Random.Range(this.fastLoopsMin, this.fastLoopsMax + 1);
		this.fast = 0;
		base.animator.SetBool("Fast", true);
	}

	// Token: 0x04001BAA RID: 7082
	[Space(10f)]
	[Range(1f, 20f)]
	[SerializeField]
	public int regularLoopsMin = 3;

	// Token: 0x04001BAB RID: 7083
	[Range(1f, 20f)]
	[SerializeField]
	public int regularLoopsMax = 5;

	// Token: 0x04001BAC RID: 7084
	[Space(10f)]
	[Range(1f, 20f)]
	[SerializeField]
	public int fastLoopsMin = 5;

	// Token: 0x04001BAD RID: 7085
	[Range(1f, 20f)]
	[SerializeField]
	public int fastLoopsMax = 9;

	// Token: 0x04001BAE RID: 7086
	public int reg;

	// Token: 0x04001BAF RID: 7087
	public int fast;

	// Token: 0x04001BB0 RID: 7088
	public int regTarget = 4;

	// Token: 0x04001BB1 RID: 7089
	public int fastTarget = 7;
}
