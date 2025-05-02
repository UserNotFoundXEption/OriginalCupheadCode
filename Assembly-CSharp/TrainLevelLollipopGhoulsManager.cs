using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003B4 RID: 948
public class TrainLevelLollipopGhoulsManager : LevelProperties.Train.Entity
{
	// Token: 0x14000055 RID: 85
	// (add) Token: 0x06002A0B RID: 10763 RVA: 0x000D31B8 File Offset: 0x000D13B8
	// (remove) Token: 0x06002A0C RID: 10764 RVA: 0x000D31F0 File Offset: 0x000D13F0
	public event TrainLevelLollipopGhoulsManager.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x14000056 RID: 86
	// (add) Token: 0x06002A0D RID: 10765 RVA: 0x000D3228 File Offset: 0x000D1428
	// (remove) Token: 0x06002A0E RID: 10766 RVA: 0x000D3260 File Offset: 0x000D1460
	public event Action OnDeathEvent;

	// Token: 0x06002A0F RID: 10767 RVA: 0x0002368E File Offset: 0x0002188E
	public void Setup()
	{
		this.cars[1].Explode(2);
	}

	// Token: 0x06002A10 RID: 10768 RVA: 0x000D3298 File Offset: 0x000D1498
	public override void LevelInit(LevelProperties.Train properties)
	{
		base.LevelInit(properties);
		this.ghoulLeft.LevelInit(properties);
		this.ghoulRight.LevelInit(properties);
		this.cannons.LevelInit(properties);
		this.ghoulLeft.OnDamageTakenEvent += this.OnDamageTaken;
		this.ghoulLeft.OnDeathEvent += this.OnDeath;
		this.ghoulRight.OnDamageTakenEvent += this.OnDamageTaken;
		this.ghoulRight.OnDeathEvent += this.OnDeath;
	}

	// Token: 0x06002A11 RID: 10769 RVA: 0x0002369E File Offset: 0x0002189E
	public void OnDeath()
	{
		this.deadCount++;
		if (this.deadCount > 1)
		{
			this.EndGhouls();
		}
	}

	// Token: 0x06002A12 RID: 10770 RVA: 0x000236C0 File Offset: 0x000218C0
	public void OnDamageTaken(float damage)
	{
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(damage);
		}
	}

	// Token: 0x06002A13 RID: 10771 RVA: 0x000D332C File Offset: 0x000D152C
	public IEnumerator start_cr()
	{
		AudioManager.Play("level_train_top_explode");
		this.cars[0].Explode(0);
		this.cars[2].Explode(1);
		yield return null;
		this.ghoulLeft.AnimateIn();
		this.ghoulRight.AnimateIn();
		AudioManager.Play("train_lollipop_ghoul_intro");
		this.emitAudioFromObject.Add("train_lollipop_ghoul_intro");
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.lollipopGhouls.initDelay);
		base.StartCoroutine(this.ghouls_cr());
		base.StartCoroutine(this.cannons_cr());
		yield break;
	}

	// Token: 0x06002A14 RID: 10772 RVA: 0x000236D9 File Offset: 0x000218D9
	public void StartGhouls()
	{
		base.StartCoroutine(this.start_cr());
	}

	// Token: 0x06002A15 RID: 10773 RVA: 0x000236E8 File Offset: 0x000218E8
	public void EndGhouls()
	{
		this.StopAllCoroutines();
		this.cannons.End();
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
	}

	// Token: 0x06002A16 RID: 10774 RVA: 0x000D3348 File Offset: 0x000D1548
	public TrainLevelLollipopGhoul NextGhoul()
	{
		if (this.deadCount > 1)
		{
			return null;
		}
		if (this.ghoulRight.state == TrainLevelLollipopGhoul.State.Dead || this.ghoulRight.transform == null)
		{
			return this.ghoulLeft;
		}
		if (this.ghoulLeft.state == TrainLevelLollipopGhoul.State.Dead || this.ghoulLeft.transform == null)
		{
			return this.ghoulRight;
		}
		this.current = (int)Mathf.Repeat((float)(this.current + 1), 2f);
		int num = this.current;
		if (num == 0 || num != 1)
		{
			return this.ghoulLeft;
		}
		return this.ghoulRight;
	}

	// Token: 0x06002A17 RID: 10775 RVA: 0x000D3400 File Offset: 0x000D1600
	public IEnumerator ghouls_cr()
	{
		this.current = Random.Range(0, 2);
		for (;;)
		{
			TrainLevelLollipopGhoul ghoul = this.NextGhoul();
			yield return null;
			if (ghoul != null)
			{
				ghoul.Attack();
				while (ghoul.state == TrainLevelLollipopGhoul.State.Attacking)
				{
					yield return null;
				}
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.lollipopGhouls.mainDelay);
			}
		}
		yield break;
	}

	// Token: 0x06002A18 RID: 10776 RVA: 0x000D341C File Offset: 0x000D161C
	public IEnumerator cannons_cr()
	{
		int cannon = Random.Range(0, 3);
		int direction = 1;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.lollipopGhouls.cannonDelay);
			this.cannons.Shoot(cannon);
			yield return CupheadTime.WaitForSeconds(this, 1f);
			cannon += direction;
			if (cannon >= 3)
			{
				direction = -1;
				cannon = 1;
			}
			else if (cannon < 0)
			{
				cannon = 1;
				direction = 1;
			}
		}
		yield break;
	}

	// Token: 0x0400231E RID: 8990
	[SerializeField]
	public TrainLevelLollipopGhoul ghoulLeft;

	// Token: 0x0400231F RID: 8991
	[SerializeField]
	public TrainLevelLollipopGhoul ghoulRight;

	// Token: 0x04002320 RID: 8992
	[Space(10f)]
	[SerializeField]
	public TrainLevelGhostCannons cannons;

	// Token: 0x04002321 RID: 8993
	[Space(10f)]
	[SerializeField]
	public TrainLevelPassengerCar[] cars;

	// Token: 0x04002324 RID: 8996
	public int deadCount;

	// Token: 0x04002325 RID: 8997
	public int current;

	// Token: 0x02000FBC RID: 4028
	// (Invoke) Token: 0x060075D9 RID: 30169
	public delegate void OnDamageTakenHandler(float damage);
}
