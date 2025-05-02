using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001CD RID: 461
public class DicePalaceBoozeLevelDecanter : DicePalaceBoozeLevelBossBase
{
	// Token: 0x06001591 RID: 5521 RVA: 0x0001258D File Offset: 0x0001078D
	public override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x06001592 RID: 5522 RVA: 0x000125C3 File Offset: 0x000107C3
	public void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x06001593 RID: 5523 RVA: 0x0009C2D8 File Offset: 0x0009A4D8
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
		}
	}

	// Token: 0x06001594 RID: 5524 RVA: 0x0009C350 File Offset: 0x0009A550
	public override void LevelInit(LevelProperties.DicePalaceBooze properties)
	{
		this.dropPosition.z = 0f;
		this.dropPosition.y = this.sprayYRoot.position.y;
		this.attackDelayIndex = Random.Range(0, properties.CurrentState.decanter.attackDelayString.Split(new char[]
		{
			','
		}).Length);
		this.attacking = false;
		this.nextPlayerTarget = PlayerId.PlayerOne;
		Level.Current.OnIntroEvent += this.OnIntroEnd;
		Level.Current.OnWinEvent += this.HandleDead;
		this.health = properties.CurrentState.decanter.decanterHP;
		AudioManager.Play("booze_decanter_intro");
		this.emitAudioFromObject.Add("booze_decanter_intro");
		base.LevelInit(properties);
	}

	// Token: 0x06001595 RID: 5525 RVA: 0x000125D0 File Offset: 0x000107D0
	public void OnIntroEnd()
	{
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06001596 RID: 5526 RVA: 0x0009C42C File Offset: 0x0009A62C
	public IEnumerator attack_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(base.properties.CurrentState.decanter.attackDelayString.Split(new char[]
			{
				','
			})[this.attackDelayIndex]) - DicePalaceBoozeLevelBossBase.ATTACK_DELAY);
			base.animator.SetTrigger("OnAttack");
			yield return base.animator.WaitForAnimationToStart(this, "Attack", false);
			AudioManager.Play("booze_decanter_attack");
			this.emitAudioFromObject.Add("booze_decanter_attack");
			base.StartCoroutine(this.spray_cr());
			this.attackDelayIndex++;
			if (this.attackDelayIndex >= base.properties.CurrentState.decanter.attackDelayString.Split(new char[]
			{
				','
			}).Length)
			{
				this.attackDelayIndex = 0;
			}
			if (this.nextPlayerTarget == PlayerId.PlayerOne)
			{
				if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
				{
					this.nextPlayerTarget = PlayerId.PlayerTwo;
				}
			}
			else
			{
				this.nextPlayerTarget = PlayerId.PlayerOne;
			}
			while (this.attacking)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06001597 RID: 5527 RVA: 0x0009C448 File Offset: 0x0009A648
	public IEnumerator spray_cr()
	{
		this.attacking = true;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.decanter.beamAppearDelayRange.RandomFloat());
		AudioManager.Play("booze_decanter_spray_down");
		this.emitAudioFromObject.Add("booze_decanter_spray_down");
		GameObject spray = Object.Instantiate<GameObject>(this.sprayPrefab, this.dropPosition, Quaternion.identity);
		this.attacking = false;
		this.dropPosition.x = PlayerManager.GetPlayer(this.nextPlayerTarget).center.x;
		Vector3 pos = spray.transform.position;
		pos.x = this.dropPosition.x;
		spray.transform.position = pos;
		yield return spray.GetComponent<Animator>().WaitForAnimationToEnd(this, "Spray", false, true);
		Object.Destroy(spray);
		yield break;
	}

	// Token: 0x06001598 RID: 5528 RVA: 0x000125DF File Offset: 0x000107DF
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001599 RID: 5529 RVA: 0x000125FD File Offset: 0x000107FD
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.sprayPrefab = null;
	}

	// Token: 0x04001187 RID: 4487
	[SerializeField]
	public Transform sprayYRoot;

	// Token: 0x04001188 RID: 4488
	[SerializeField]
	public GameObject sprayPrefab;

	// Token: 0x04001189 RID: 4489
	public bool attacking;

	// Token: 0x0400118A RID: 4490
	public int attackDelayIndex;

	// Token: 0x0400118B RID: 4491
	public PlayerId nextPlayerTarget;

	// Token: 0x0400118C RID: 4492
	public Vector3 dropPosition;

	// Token: 0x0400118D RID: 4493
	public DamageDealer damageDealer;

	// Token: 0x0400118E RID: 4494
	public DamageReceiver damageReceiver;
}
