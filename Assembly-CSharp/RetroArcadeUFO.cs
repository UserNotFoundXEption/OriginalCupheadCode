using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000321 RID: 801
public class RetroArcadeUFO : RetroArcadeEnemy
{
	// Token: 0x0600230A RID: 8970 RVA: 0x0001DB73 File Offset: 0x0001BD73
	public void LevelInit(LevelProperties.RetroArcade properties)
	{
		this.properties = properties;
	}

	// Token: 0x0600230B RID: 8971 RVA: 0x000BF604 File Offset: 0x000BD804
	public void StartUFO()
	{
		base.gameObject.SetActive(true);
		this.p = this.properties.CurrentState.uFO;
		base.transform.SetPosition(new float?(0f), new float?(500f), null);
		base.MoveY(-200f, 500f);
		this.alien = this.alienPrefab.Create(this, this.p);
		this.mole = this.molePrefab.Create(this.p);
		this.turrets = new List<RetroArcadeUFOTurret>();
		for (int i = 0; i < this.p.turretCount; i++)
		{
			RetroArcadeUFOTurret item = this.turretPrefab.Create(this, this.p, (float)i / (float)this.p.turretCount);
			this.turrets.Add(item);
		}
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x0600230C RID: 8972 RVA: 0x000BF6FC File Offset: 0x000BD8FC
	public IEnumerator shoot_cr()
	{
		for (;;)
		{
			float waitTime = this.p.shotRate.min * Mathf.Pow(this.p.shotRate.max / this.p.shotRate.min, 1f - this.alien.NormalizedHpRemaining);
			yield return CupheadTime.WaitForSeconds(this, waitTime);
			foreach (RetroArcadeUFOTurret retroArcadeUFOTurret in this.turrets)
			{
				retroArcadeUFOTurret.Shoot();
			}
		}
		yield break;
	}

	// Token: 0x0600230D RID: 8973 RVA: 0x000BF718 File Offset: 0x000BD918
	public IEnumerator moveOffscreen_cr()
	{
		base.MoveY(200f, 500f);
		while (this.movingY)
		{
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0600230E RID: 8974 RVA: 0x0001DB7C File Offset: 0x0001BD7C
	public void OnAlienDie()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.moveOffscreen_cr());
		this.properties.DealDamageToNextNamedState();
		this.mole.OnWaveEnd();
	}

	// Token: 0x04001D0A RID: 7434
	public const float OFFSCREEN_Y = 500f;

	// Token: 0x04001D0B RID: 7435
	public const float ONSCREEN_Y = 300f;

	// Token: 0x04001D0C RID: 7436
	public const float MOVE_Y_SPEED = 500f;

	// Token: 0x04001D0D RID: 7437
	public const float WIDTH = 600f;

	// Token: 0x04001D0E RID: 7438
	public const float HEIGHT = 300f;

	// Token: 0x04001D0F RID: 7439
	public const float INNER_WIDTH = 500f;

	// Token: 0x04001D10 RID: 7440
	public const float INNER_HEIGHT = 150f;

	// Token: 0x04001D11 RID: 7441
	public const float INNER_TURNAROUND_X = 220f;

	// Token: 0x04001D12 RID: 7442
	public LevelProperties.RetroArcade properties;

	// Token: 0x04001D13 RID: 7443
	public LevelProperties.RetroArcade.UFO p;

	// Token: 0x04001D14 RID: 7444
	[SerializeField]
	public RetroArcadeUFOTurret turretPrefab;

	// Token: 0x04001D15 RID: 7445
	[SerializeField]
	public RetroArcadeUFOAlien alienPrefab;

	// Token: 0x04001D16 RID: 7446
	[SerializeField]
	public RetroArcadeUFOMole molePrefab;

	// Token: 0x04001D17 RID: 7447
	public RetroArcadeUFOAlien alien;

	// Token: 0x04001D18 RID: 7448
	public List<RetroArcadeUFOTurret> turrets;

	// Token: 0x04001D19 RID: 7449
	public RetroArcadeUFOMole mole;
}
