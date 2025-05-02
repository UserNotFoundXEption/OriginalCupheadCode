using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001CE RID: 462
public class DicePalaceBoozeLevelMartini : DicePalaceBoozeLevelBossBase
{
	// Token: 0x0600159B RID: 5531 RVA: 0x0009C464 File Offset: 0x0009A664
	public override void Awake()
	{
		this.olives = new List<DicePalaceBoozeLevelOlive>();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x0600159C RID: 5532 RVA: 0x00012614 File Offset: 0x00010814
	public void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x0600159D RID: 5533 RVA: 0x0009C4B0 File Offset: 0x0009A6B0
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		float health = this.health;
		this.health -= info.damage;
		if (health > 0f)
		{
			Level.Current.timeline.DealDamage(Mathf.Clamp(health - this.health, 0f, health));
		}
		if (this.health < 0f && !base.isDead)
		{
			this.StartDying();
			this.MartiniDeathSFX();
		}
	}

	// Token: 0x0600159E RID: 5534 RVA: 0x0009C52C File Offset: 0x0009A72C
	public override void LevelInit(LevelProperties.DicePalaceBooze properties)
	{
		this.activeOlives = 0;
		this.pinkShotIndex = Random.Range(0, properties.CurrentState.martini.pinkString.Split(new char[]
		{
			','
		}).Length);
		int num = properties.CurrentState.martini.olivePositionStringX.Length;
		int num2 = Random.Range(0, num);
		int num3 = Random.Range(0, num);
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.olive.gameObject, this.spawnPoint.position, Quaternion.identity);
			gameObject.GetComponent<DicePalaceBoozeLevelOlive>().InitOlive(properties, Parser.IntParse(properties.CurrentState.martini.pinkString.Split(new char[]
			{
				','
			})[this.pinkShotIndex]), properties.CurrentState.martini.olivePositionStringY[num3], properties.CurrentState.martini.olivePositionStringX[num2]);
			this.pinkShotIndex++;
			if (this.pinkShotIndex >= properties.CurrentState.martini.pinkString.Split(new char[]
			{
				','
			}).Length)
			{
				this.pinkShotIndex = 0;
			}
			num3++;
			if (num3 >= num)
			{
				num3 = 0;
			}
			num2++;
			if (num2 >= num)
			{
				num2 = 0;
			}
			gameObject.SetActive(false);
			this.olives.Add(gameObject.GetComponent<DicePalaceBoozeLevelOlive>());
		}
		Level.Current.OnIntroEvent += this.OnIntroEnd;
		Level.Current.OnWinEvent += this.HandleDead;
		AudioManager.Play("booze_martini_intro");
		this.emitAudioFromObject.Add("booze_martini_intro");
		base.LevelInit(properties);
		this.health = properties.CurrentState.martini.martiniHP;
	}

	// Token: 0x0600159F RID: 5535 RVA: 0x00012621 File Offset: 0x00010821
	public void OnIntroEnd()
	{
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x060015A0 RID: 5536 RVA: 0x0009C6FC File Offset: 0x0009A8FC
	public IEnumerator attack_cr()
	{
		this.oliveIndex = 0;
		int counter = 0;
		for (;;)
		{
			counter = 0;
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.martini.oliveSpawnDelay - DicePalaceBoozeLevelBossBase.ATTACK_DELAY);
			yield return null;
			while (this.olives[this.oliveIndex].gameObject.activeSelf)
			{
				this.oliveIndex = (this.oliveIndex + 1) % this.olives.Count;
				counter++;
				if (counter >= this.olives.Count)
				{
					this.allActive = true;
					break;
				}
				yield return null;
			}
			if (counter < this.olives.Count)
			{
				this.allActive = false;
			}
			if (!this.allActive)
			{
				base.animator.SetTrigger("OnAttack");
				yield return base.animator.WaitForAnimationToStart(this, "Attack", false);
				AudioManager.Play("booze_martini_attack");
				this.emitAudioFromObject.Add("booze_martini_attack");
				yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060015A1 RID: 5537 RVA: 0x0009C718 File Offset: 0x0009A918
	public void ShootOlive()
	{
		this.olives[this.oliveIndex].transform.position = this.spawnPoint.position;
		this.olives[this.oliveIndex].gameObject.SetActive(true);
		this.olives[this.oliveIndex].ResetOlive(Parser.IntParse(base.properties.CurrentState.martini.pinkString.Split(new char[]
		{
			','
		})[this.pinkShotIndex]));
		this.pinkShotIndex++;
		if (this.pinkShotIndex >= base.properties.CurrentState.martini.pinkString.Split(new char[]
		{
			','
		}).Length)
		{
			this.pinkShotIndex = 0;
		}
		this.oliveIndex = (this.oliveIndex + 1) % this.olives.Count;
	}

	// Token: 0x060015A2 RID: 5538 RVA: 0x00012630 File Offset: 0x00010830
	public void OnOliveDeath()
	{
		this.activeOlives--;
	}

	// Token: 0x060015A3 RID: 5539 RVA: 0x00012640 File Offset: 0x00010840
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060015A4 RID: 5540 RVA: 0x0001265E File Offset: 0x0001085E
	public override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
		this.olive = null;
	}

	// Token: 0x060015A5 RID: 5541 RVA: 0x00012673 File Offset: 0x00010873
	public void MartiniDeathSFX()
	{
		AudioManager.Play("martini_death_vox");
		this.emitAudioFromObject.Add("martini_death_vox");
	}

	// Token: 0x0400118F RID: 4495
	[SerializeField]
	public DicePalaceBoozeLevelOlive olive;

	// Token: 0x04001190 RID: 4496
	[SerializeField]
	public Transform spawnPoint;

	// Token: 0x04001191 RID: 4497
	public bool allActive;

	// Token: 0x04001192 RID: 4498
	public int oliveIndex;

	// Token: 0x04001193 RID: 4499
	public int pinkShotIndex;

	// Token: 0x04001194 RID: 4500
	public int activeOlives;

	// Token: 0x04001195 RID: 4501
	public List<DicePalaceBoozeLevelOlive> olives;

	// Token: 0x04001196 RID: 4502
	public DamageDealer damageDealer;

	// Token: 0x04001197 RID: 4503
	public DamageReceiver damageReceiver;
}
