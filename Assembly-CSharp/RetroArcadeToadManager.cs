using System;
using UnityEngine;

// Token: 0x0200031D RID: 797
public class RetroArcadeToadManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x060022F3 RID: 8947 RVA: 0x000BF2C8 File Offset: 0x000BD4C8
	public void StartToad()
	{
		this.p = base.properties.CurrentState.toad;
		this.numDied = 0;
		this.toad1 = this.toadPrefab.Create(this, this.p, true);
		this.toad2 = this.toadPrefab.Create(this, this.p, false);
	}

	// Token: 0x060022F4 RID: 8948 RVA: 0x000BF324 File Offset: 0x000BD524
	public void OnToadDie()
	{
		this.numDied++;
		if (this.numDied >= 2)
		{
			this.StopAllCoroutines();
			Object.Destroy(this.toad1.gameObject);
			Object.Destroy(this.toad2.gameObject);
			base.properties.DealDamageToNextNamedState();
		}
	}

	// Token: 0x04001CFB RID: 7419
	[SerializeField]
	public RetroArcadeToad toadPrefab;

	// Token: 0x04001CFC RID: 7420
	public LevelProperties.RetroArcade.Toad p;

	// Token: 0x04001CFD RID: 7421
	public RetroArcadeToad toad1;

	// Token: 0x04001CFE RID: 7422
	public RetroArcadeToad toad2;

	// Token: 0x04001CFF RID: 7423
	public int numDied;
}
