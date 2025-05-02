using System;
using UnityEngine;

// Token: 0x020003B1 RID: 945
public class TrainLevelGhostCannons : LevelProperties.Train.Entity
{
	// Token: 0x060029EA RID: 10730 RVA: 0x000234A2 File Offset: 0x000216A2
	public void Shoot(int cannon)
	{
		if (!this.shooting)
		{
			return;
		}
		this.cannon = cannon;
		base.animator.SetInteger("Cannon", cannon);
		base.animator.SetTrigger("OnShoot");
	}

	// Token: 0x060029EB RID: 10731 RVA: 0x000234D8 File Offset: 0x000216D8
	public void End()
	{
		this.shooting = false;
	}

	// Token: 0x060029EC RID: 10732 RVA: 0x000D2EB0 File Offset: 0x000D10B0
	public void ShootAnim()
	{
		if (!this.shooting)
		{
			return;
		}
		AudioManager.Play("train_cannon_shoot");
		this.emitAudioFromObject.Add("train_cannon_shoot");
		this.cannonSmoke.Create(this.cannonRoots[this.cannon].position);
		this.ghostPrefab.Create(this.cannonRoots[this.cannon].position, base.properties.CurrentState.lollipopGhouls.ghostDelay, base.properties.CurrentState.lollipopGhouls.ghostSpeed, base.properties.CurrentState.lollipopGhouls.ghostAimSpeed, base.properties.CurrentState.lollipopGhouls.ghostHealth, base.properties.CurrentState.lollipopGhouls.skullSpeed);
	}

	// Token: 0x060029ED RID: 10733 RVA: 0x000234E1 File Offset: 0x000216E1
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.ghostPrefab = null;
		this.cannonSmoke = null;
	}

	// Token: 0x0400230D RID: 8973
	[SerializeField]
	public Effect cannonSmoke;

	// Token: 0x0400230E RID: 8974
	[SerializeField]
	public Transform[] cannonRoots;

	// Token: 0x0400230F RID: 8975
	[SerializeField]
	public TrainLevelGhostCannonGhost ghostPrefab;

	// Token: 0x04002310 RID: 8976
	public int cannon;

	// Token: 0x04002311 RID: 8977
	public bool shooting = true;
}
