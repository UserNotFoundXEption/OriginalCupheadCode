using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class FlowerLevel : Level
{
	// Token: 0x06000245 RID: 581 RVA: 0x00062E5C File Offset: 0x0006105C
	public override void PartialInit()
	{
		this.properties = LevelProperties.Flower.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x06000246 RID: 582 RVA: 0x0000439A File Offset: 0x0000259A
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Flower;
		}
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06000247 RID: 583 RVA: 0x000043A1 File Offset: 0x000025A1
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_flower;
		}
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x06000248 RID: 584 RVA: 0x00062EF4 File Offset: 0x000610F4
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Flower.States.Main:
			case LevelProperties.Flower.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Flower.States.PhaseTwo:
				return this._bossPortraitPhaseTwo;
			default:
				Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x06000249 RID: 585 RVA: 0x00062F68 File Offset: 0x00061168
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Flower.States.Main:
			case LevelProperties.Flower.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Flower.States.PhaseTwo:
				return this._bossQuotePhaseTwo;
			default:
				Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x0600024A RID: 586 RVA: 0x000043A5 File Offset: 0x000025A5
	public override void Start()
	{
		base.Start();
		this.flower.LevelInit(this.properties);
	}

	// Token: 0x0600024B RID: 587 RVA: 0x000043BE File Offset: 0x000025BE
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.flowerPattern_cr());
	}

	// Token: 0x0600024C RID: 588 RVA: 0x00062FDC File Offset: 0x000611DC
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Flower.States.PhaseTwo)
		{
			if (Level.Current.mode == Level.Mode.Easy)
			{
				this.properties.WinInstantly();
				AudioManager.PlayLoop("flower_phase1_death_loop");
				AudioManager.Play("flower_phase1_death_scream");
			}
			else
			{
				this.StopAllCoroutines();
				this.flower.PhaseTwoTrigger();
			}
		}
	}

	// Token: 0x0600024D RID: 589 RVA: 0x000043CD File Offset: 0x000025CD
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitPhaseTwo = null;
	}

	// Token: 0x0600024E RID: 590 RVA: 0x0006304C File Offset: 0x0006124C
	public IEnumerator flowerPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600024F RID: 591 RVA: 0x00063068 File Offset: 0x00061268
	public IEnumerator nextPattern_cr()
	{
		switch (this.properties.CurrentState.NextPattern)
		{
		case LevelProperties.Flower.Pattern.Laser:
			yield return base.StartCoroutine(this.laserAttack_cr());
			break;
		case LevelProperties.Flower.Pattern.PodHands:
			yield return base.StartCoroutine(this.potHands_cr());
			break;
		case LevelProperties.Flower.Pattern.GattlingGun:
			yield return base.StartCoroutine(this.gattlingGun_cr());
			break;
		default:
			yield return CupheadTime.WaitForSeconds(this, 1f);
			break;
		}
		yield break;
	}

	// Token: 0x06000250 RID: 592 RVA: 0x00063084 File Offset: 0x00061284
	public IEnumerator laserAttack_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.laser.hesitateAfterAttack);
		this.attacking = true;
		this.flower.StartLaser(new Action(this.OnLaserAttackComplete));
		while (this.attacking)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000251 RID: 593 RVA: 0x000043E3 File Offset: 0x000025E3
	public void OnLaserAttackComplete()
	{
		this.attacking = false;
	}

	// Token: 0x06000252 RID: 594 RVA: 0x000630A0 File Offset: 0x000612A0
	public IEnumerator potHands_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.podHands.hesitateAfterAttack);
		this.attacking = true;
		this.flower.StartPotHands(new Action(this.OnPotHandsAttackComplete));
		while (this.attacking)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000253 RID: 595 RVA: 0x000043EC File Offset: 0x000025EC
	public void OnPotHandsAttackComplete()
	{
		this.attacking = false;
	}

	// Token: 0x06000254 RID: 596 RVA: 0x000630BC File Offset: 0x000612BC
	public IEnumerator gattlingGun_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.gattlingGun.hesitateAfterAttack);
		this.attacking = true;
		this.flower.StartGattlingGun(new Action(this.OnGattlingGunComplete));
		while (this.attacking)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000255 RID: 597 RVA: 0x000043F5 File Offset: 0x000025F5
	public void OnGattlingGunComplete()
	{
		this.attacking = false;
	}

	// Token: 0x04000196 RID: 406
	public LevelProperties.Flower properties;

	// Token: 0x04000197 RID: 407
	[SerializeField]
	public FlowerLevelFlower flower;

	// Token: 0x04000198 RID: 408
	public bool attacking;

	// Token: 0x04000199 RID: 409
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x0400019A RID: 410
	[SerializeField]
	public Sprite _bossPortraitPhaseTwo;

	// Token: 0x0400019B RID: 411
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x0400019C RID: 412
	[SerializeField]
	public string _bossQuotePhaseTwo;

	// Token: 0x020007A6 RID: 1958
	[Serializable]
	public class Prefabs
	{
	}
}
