using System;
using UnityEngine;

// Token: 0x020001E5 RID: 485
public class DicePalaceDominoLevelRandomSpike : AbstractMonoBehaviour
{
	// Token: 0x0600167C RID: 5756 RVA: 0x00013267 File Offset: 0x00011467
	public void Start()
	{
		this.ChangeSpikes();
	}

	// Token: 0x0600167D RID: 5757 RVA: 0x0001326F File Offset: 0x0001146F
	public void ChangeSpikes()
	{
		base.animator.SetTrigger(this.states[Random.Range(0, this.states.Length)]);
		this.melt = false;
	}

	// Token: 0x0600167E RID: 5758 RVA: 0x0009F124 File Offset: 0x0009D324
	public void Update()
	{
		if (this.melt)
		{
			return;
		}
		if (base.transform.position.x <= -410f)
		{
			this.melt = true;
			base.animator.SetTrigger("Melt");
		}
	}

	// Token: 0x0400123F RID: 4671
	[SerializeField]
	public string[] states;

	// Token: 0x04001240 RID: 4672
	public bool melt;
}
