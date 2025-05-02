using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002F7 RID: 759
public class PirateLevelSquid : LevelProperties.Pirate.Entity
{
	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x060021C0 RID: 8640 RVA: 0x0001CD96 File Offset: 0x0001AF96
	// (set) Token: 0x060021C1 RID: 8641 RVA: 0x0001CD9E File Offset: 0x0001AF9E
	public PirateLevelSquid.State state { get; set; }

	// Token: 0x060021C2 RID: 8642 RVA: 0x0001CDA7 File Offset: 0x0001AFA7
	public override void Awake()
	{
		base.Awake();
		base.transform.position = PirateLevelSquid.START_POS;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.onDamageTaken;
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x060021C3 RID: 8643 RVA: 0x000BB364 File Offset: 0x000B9564
	public void Update()
	{
		if (this.state == PirateLevelSquid.State.Attack)
		{
			if (this.attackTime > this.squid.maxTime)
			{
				this.Exit();
			}
			else
			{
				this.attackTime += CupheadTime.Delta;
			}
		}
		float num = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, Mathf.PingPong(this.bobTime, 1f));
		base.transform.SetPosition(null, new float?(Mathf.Lerp(this.startY, this.endY, num)), null);
		this.bobTime += CupheadTime.Delta;
	}

	// Token: 0x060021C4 RID: 8644 RVA: 0x000BB424 File Offset: 0x000B9624
	public override void LevelInit(LevelProperties.Pirate properties)
	{
		base.LevelInit(properties);
		this.squid = properties.CurrentState.squid;
		float value = this.squid.xPos.RandomFloat();
		base.transform.SetPosition(new float?(value), null, null);
		this.splashPrefab.Create(base.transform.position + new Vector3(0f, -40f, 0f));
		AudioManager.Play("level_pirate_squid_splash");
		this.hp = this.squid.hp;
		this.startY = base.transform.position.y;
		this.endY = this.startY + -20f;
		this.state = PirateLevelSquid.State.Enter;
		AudioManager.Play("level_pirate_squid_enter");
		properties.OnBossDeath += this.OnBossDeath;
	}

	// Token: 0x060021C5 RID: 8645 RVA: 0x0001CDE7 File Offset: 0x0001AFE7
	public void PlayPopSFX()
	{
		AudioManager.Play("level_pirate_squid_attack_pop");
	}

	// Token: 0x060021C6 RID: 8646 RVA: 0x0001CDF3 File Offset: 0x0001AFF3
	public void onDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x060021C7 RID: 8647 RVA: 0x000BB51C File Offset: 0x000B971C
	public void Exit()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.state = PirateLevelSquid.State.Exit;
		AudioManager.Play("level_pirate_squid_exit");
		base.animator.SetTrigger("OnExit");
		base.properties.OnBossDeath -= this.OnBossDeath;
	}

	// Token: 0x060021C8 RID: 8648 RVA: 0x000BB570 File Offset: 0x000B9770
	public void Die()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.state = PirateLevelSquid.State.Die;
		AudioManager.Play("level_pirate_squid_death");
		base.animator.SetTrigger("OnDeath");
		base.properties.OnBossDeath -= this.OnBossDeath;
	}

	// Token: 0x060021C9 RID: 8649 RVA: 0x0001CE1E File Offset: 0x0001B01E
	public void OnBossDeath()
	{
		this.Die();
	}

	// Token: 0x060021CA RID: 8650 RVA: 0x000BB5C4 File Offset: 0x000B97C4
	public IEnumerator attack_cr()
	{
		if (!this.InkAttackSoundActive)
		{
			AudioManager.PlayLoop("level_pirate_squid_attack_loop");
			this.InkAttackSoundActive = true;
		}
		while (this.state == PirateLevelSquid.State.Attack)
		{
			Vector2 v = Vector2.zero;
			v.y = this.squid.blobVelY;
			v.x = this.squid.blobVelX;
			this.inkBlob.Create(this.inkOrigin.position, v, this.squid.blobGravity);
			yield return CupheadTime.WaitForSeconds(this, this.squid.blobDelay);
		}
		AudioManager.Stop("level_pirate_squid_attack_loop");
		this.InkAttackSoundActive = false;
		yield break;
	}

	// Token: 0x060021CB RID: 8651 RVA: 0x0001CE26 File Offset: 0x0001B026
	public void OnEnterAnimationComplete()
	{
		base.GetComponent<Collider2D>().enabled = true;
		this.state = PirateLevelSquid.State.Attack;
		this.attackTime = 0f;
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x060021CC RID: 8652 RVA: 0x0001CE53 File Offset: 0x0001B053
	public void OnExitAnimationComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060021CD RID: 8653 RVA: 0x0001CE60 File Offset: 0x0001B060
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.inkBlob = null;
		this.splashPrefab = null;
	}

	// Token: 0x04001BCB RID: 7115
	public static readonly Vector2 START_POS = new Vector2(-200f, -220f);

	// Token: 0x04001BCC RID: 7116
	public const float BOB_OFFSET = -20f;

	// Token: 0x04001BCD RID: 7117
	[SerializeField]
	public Transform inkOrigin;

	// Token: 0x04001BCE RID: 7118
	[SerializeField]
	public PirateLevelSquidProjectile inkBlob;

	// Token: 0x04001BCF RID: 7119
	[SerializeField]
	public Effect splashPrefab;

	// Token: 0x04001BD1 RID: 7121
	public float hp;

	// Token: 0x04001BD2 RID: 7122
	public float startY;

	// Token: 0x04001BD3 RID: 7123
	public float endY;

	// Token: 0x04001BD4 RID: 7124
	public float bobTime;

	// Token: 0x04001BD5 RID: 7125
	public float attackTime;

	// Token: 0x04001BD6 RID: 7126
	public LevelProperties.Pirate.Squid squid;

	// Token: 0x04001BD7 RID: 7127
	public bool InkAttackSoundActive;

	// Token: 0x02000E18 RID: 3608
	public enum State
	{
		// Token: 0x04006607 RID: 26119
		Init,
		// Token: 0x04006608 RID: 26120
		Enter,
		// Token: 0x04006609 RID: 26121
		Attack,
		// Token: 0x0400660A RID: 26122
		Exit,
		// Token: 0x0400660B RID: 26123
		Die
	}
}
