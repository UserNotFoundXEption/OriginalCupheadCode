using System;
using UnityEngine;

// Token: 0x02000492 RID: 1170
public class MapNPCCoinMoneyman : MonoBehaviour
{
	// Token: 0x06003122 RID: 12578 RVA: 0x00028E63 File Offset: 0x00027063
	public void Start()
	{
		this.UpdateCoins();
		this.LookAroundFinished();
	}

	// Token: 0x06003123 RID: 12579 RVA: 0x000E8B78 File Offset: 0x000E6D78
	public void UpdateCoins()
	{
		for (int i = 0; i < this.hiddenCoinIds.Length; i++)
		{
			if (!PlayerData.Data.coinManager.GetCoinCollected(this.hiddenCoinIds[i]))
			{
				return;
			}
		}
		Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
		PlayerData.SaveCurrentFile();
	}

	// Token: 0x06003124 RID: 12580 RVA: 0x000E8BD0 File Offset: 0x000E6DD0
	public void Update()
	{
		if (this.waiting)
		{
			return;
		}
		this.durationBeforeNext -= CupheadTime.Delta;
		this.durationBeforeBlink -= CupheadTime.Delta;
		if (this.durationBeforeBlink <= 0f)
		{
			this.durationBeforeBlink = float.PositiveInfinity;
			this.animator.SetTrigger("blink");
		}
		if (this.durationBeforeNext <= 0f)
		{
			this.waiting = true;
			this.animator.SetTrigger("next");
		}
	}

	// Token: 0x06003125 RID: 12581 RVA: 0x00028E71 File Offset: 0x00027071
	public void LookAroundFinished()
	{
		this.durationBeforeNext = Random.Range(this.idleDurationMin, this.idleDurationMax);
		this.durationBeforeBlink = Random.Range(0f, this.durationBeforeNext);
		this.waiting = false;
	}

	// Token: 0x04002887 RID: 10375
	[SerializeField]
	public Animator animator;

	// Token: 0x04002888 RID: 10376
	[SerializeField]
	public float idleDurationMin;

	// Token: 0x04002889 RID: 10377
	[SerializeField]
	public float idleDurationMax;

	// Token: 0x0400288A RID: 10378
	public float durationBeforeNext;

	// Token: 0x0400288B RID: 10379
	public float durationBeforeBlink;

	// Token: 0x0400288C RID: 10380
	public bool waiting = true;

	// Token: 0x0400288D RID: 10381
	[SerializeField]
	public string[] hiddenCoinIds;

	// Token: 0x0400288E RID: 10382
	[SerializeField]
	public int dialoguerVariableID = 4;
}
