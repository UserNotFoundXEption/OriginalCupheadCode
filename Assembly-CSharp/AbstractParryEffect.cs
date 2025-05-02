using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004F6 RID: 1270
public abstract class AbstractParryEffect : Effect
{
	// Token: 0x06003452 RID: 13394 RVA: 0x0002AF64 File Offset: 0x00029164
	public AbstractParryEffect()
	{
	}

	// Token: 0x06003453 RID: 13395 RVA: 0x000F6440 File Offset: 0x000F4640
	public AbstractParryEffect Create(AbstractPlayerController player)
	{
		AbstractParryEffect abstractParryEffect = base.Create(player.center, player.transform.localScale) as AbstractParryEffect;
		abstractParryEffect.SetPlayer(player);
		return abstractParryEffect;
	}

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x06003454 RID: 13396
	public abstract bool IsHit { get; }

	// Token: 0x06003455 RID: 13397 RVA: 0x000F6474 File Offset: 0x000F4674
	public override void Initialize(Vector3 position, Vector3 scale, bool randomR)
	{
		base.Initialize(position, scale, randomR);
		base.animator.enabled = false;
		this.sprites.SetActive(false);
		this.projectiles = new List<AbstractProjectile>();
		this.sparks = new List<Effect>();
		this.switches = new List<ParrySwitch>();
		this.entities = new List<AbstractLevelEntity>();
		base.tag = "Parry";
	}

	// Token: 0x06003456 RID: 13398 RVA: 0x000F64DC File Offset: 0x000F46DC
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		if (this.cancel)
		{
			return;
		}
		base.OnCollision(hit, phase);
		if (!this.player.IsDead && phase == CollisionPhase.Enter)
		{
			AbstractProjectile component = hit.GetComponent<AbstractProjectile>();
			if (component == null)
			{
				CollisionChild component2 = hit.GetComponent<CollisionChild>();
				AbstractCollidableObject abstractCollidableObject;
				if (component2 != null && component2.ForwardParry(out abstractCollidableObject))
				{
					component = abstractCollidableObject.GetComponent<AbstractProjectile>();
				}
			}
			if (component != null && component.CanParry)
			{
				this.projectiles.Add(component);
				if (!this.player.stats.NextParryActivatesHealerCharm())
				{
					this.sparks.Add(this.spark.Create(component.transform.position));
				}
				if (!this.didHitSomething)
				{
					base.StartCoroutine(this.hit_cr(false));
				}
			}
			ParrySwitch component3 = hit.GetComponent<ParrySwitch>();
			if (component3 != null && component3.enabled && component3.IsParryable)
			{
				this.switches.Add(component3);
				if (!this.didHitSomething)
				{
					base.StartCoroutine(this.hit_cr(false));
				}
			}
			AbstractLevelEntity component4 = hit.GetComponent<AbstractLevelEntity>();
			if (component4 != null && component4.enabled && component4.canParry)
			{
				this.entities.Add(component4);
				if (!this.didHitSomething)
				{
					base.StartCoroutine(this.hit_cr(false));
				}
			}
			if ((this.player.stats.Loadout.charm == Charm.charm_parry_attack || this.player.stats.CurseWhetsone) && !this.didHitSomething && !Level.IsChessBoss)
			{
				IParryAttack component5 = this.player.GetComponent<IParryAttack>();
				if (component5 != null && !component5.AttackParryUsed)
				{
					DamageReceiver damageReceiver = hit.GetComponent<DamageReceiver>();
					if (damageReceiver == null)
					{
						DamageReceiverChild component6 = hit.GetComponent<DamageReceiverChild>();
						if (component6 != null)
						{
							damageReceiver = component6.Receiver;
						}
					}
					if (damageReceiver != null && damageReceiver.type == DamageReceiver.Type.Enemy)
					{
						component5.HasHitEnemy = true;
						DamageDealer damageDealer = new DamageDealer(WeaponProperties.CharmParryAttack.damage, 0f, false, true, false);
						damageDealer.DealDamage(hit);
						this.ShowParryAttackEffect(hit);
						base.StartCoroutine(this.hit_cr(true));
					}
				}
			}
		}
	}

	// Token: 0x06003457 RID: 13399 RVA: 0x000F6750 File Offset: 0x000F4950
	public void ShowParryAttackEffect(GameObject hit)
	{
		int num = Physics2D.RaycastNonAlloc(hit.transform.position, base.transform.position - hit.transform.position, this.contactsBuffer, (base.transform.position - hit.transform.position).magnitude);
		if (num == 0)
		{
			return;
		}
		Vector3 position = this.contactsBuffer[0].point;
		for (int i = 1; i < num; i++)
		{
			if (this.contactsBuffer[i].collider.tag == "Parry")
			{
				position = this.contactsBuffer[i].point;
			}
		}
		ParryAttackSpark parryAttackSpark = this.parryAttack.Create(position) as ParryAttackSpark;
		parryAttackSpark.IsCuphead = (this.player.id == PlayerId.PlayerOne);
		this.sparks.Add(parryAttackSpark);
		parryAttackSpark.Play();
	}

	// Token: 0x06003458 RID: 13400 RVA: 0x0002AF79 File Offset: 0x00029179
	public virtual void SetPlayer(AbstractPlayerController player)
	{
		this.player = player;
		base.transform.SetParent(player.transform);
		base.StartCoroutine(this.lifetime_cr());
	}

	// Token: 0x06003459 RID: 13401 RVA: 0x0002AFA0 File Offset: 0x000291A0
	public virtual void OnHitCancel()
	{
		if (this == null)
		{
			return;
		}
		this.Cancel();
		AudioManager.Stop("player_parry");
	}

	// Token: 0x0600345A RID: 13402 RVA: 0x000F6864 File Offset: 0x000F4A64
	public virtual void Cancel()
	{
		foreach (Effect effect in this.sparks)
		{
			Object.Destroy(effect.gameObject);
		}
		this.cancel = true;
		this.CancelSwitch();
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600345B RID: 13403 RVA: 0x0002AFBF File Offset: 0x000291BF
	public virtual void CancelSwitch()
	{
	}

	// Token: 0x0600345C RID: 13404 RVA: 0x0002AFC1 File Offset: 0x000291C1
	public virtual void OnPaused()
	{
	}

	// Token: 0x0600345D RID: 13405 RVA: 0x0002AFC3 File Offset: 0x000291C3
	public virtual void OnUnpaused()
	{
	}

	// Token: 0x0600345E RID: 13406 RVA: 0x0002AFC5 File Offset: 0x000291C5
	public virtual void OnSuccess()
	{
	}

	// Token: 0x0600345F RID: 13407 RVA: 0x0002AFC7 File Offset: 0x000291C7
	public virtual void OnEnd()
	{
	}

	// Token: 0x06003460 RID: 13408 RVA: 0x000F68E4 File Offset: 0x000F4AE4
	public IEnumerator lifetime_cr()
	{
		if (this.player != null && (this.player.stats.Loadout.charm != Charm.charm_parry_plus || Level.IsChessBoss))
		{
			if (this.player.stats.isChalice)
			{
				yield return CupheadTime.WaitForSeconds(this, (Level.Current.playerMode != PlayerMode.Plane) ? 0.3f : 0.4f);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, 0.2f);
			}
			base.GetComponent<Collider2D>().enabled = false;
			this.CancelSwitch();
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06003461 RID: 13409 RVA: 0x000F6900 File Offset: 0x000F4B00
	public IEnumerator hit_cr(bool hitEnemy = false)
	{
		if (this.player.IsDead || !this.player.gameObject.activeInHierarchy || !base.gameObject.activeInHierarchy)
		{
			yield break;
		}
		bool hit = false;
		this.didHitSomething = true;
		IParryAttack parryController = this.player.GetComponent<IParryAttack>();
		if (parryController != null)
		{
			parryController.AttackParryUsed = true;
		}
		base.animator.enabled = true;
		this.sprites.SetActive(true);
		if (!hitEnemy)
		{
			foreach (ParrySwitch parrySwitch in this.switches)
			{
				parrySwitch.OnParryPrePause(this.player);
			}
			foreach (AbstractLevelEntity abstractLevelEntity in this.entities)
			{
				abstractLevelEntity.OnParry(this.player);
			}
			foreach (AbstractProjectile abstractProjectile in this.projectiles)
			{
				abstractProjectile.OnParry(this.player);
				this.player.stats.OnParry(abstractProjectile.ParryMeterMultiplier, abstractProjectile.CountParryTowardsScore);
			}
		}
		if (this.player.IsDead || !this.player.gameObject.activeInHierarchy || !base.gameObject.activeInHierarchy)
		{
			yield break;
		}
		if (Level.Current == null || !Level.IsChessBoss || !Level.Current.Ending)
		{
			PauseManager.Pause();
		}
		AudioManager.Play("player_parry");
		this.OnPaused();
		float pauseTime = (!hitEnemy) ? 0.185f : 0.13875f;
		float t = 0f;
		while (t < pauseTime)
		{
			hit = this.IsHit;
			if (hit)
			{
				t = pauseTime;
			}
			t += Time.fixedDeltaTime;
			for (int i = 0; i < 2; i++)
			{
				PlayerId playerId = (i != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
				if (this.player != null && this.player.id == playerId)
				{
					if (pauseTime - t < 0.134f)
					{
						this.player.BufferInputs();
					}
				}
				else
				{
					AbstractPlayerController abstractPlayerController = PlayerManager.GetPlayer(playerId);
					if (abstractPlayerController != null)
					{
						abstractPlayerController.BufferInputs();
					}
				}
			}
			yield return new WaitForFixedUpdate();
		}
		while (LevelNewPlayerGUI.Current != null && LevelNewPlayerGUI.Current.gameObject.activeInHierarchy)
		{
			yield return null;
		}
		if (!hit)
		{
			this.OnSuccess();
			if (Level.Current == null || !Level.IsChessBoss || !Level.Current.Ending)
			{
				PauseManager.Unpause();
			}
			this.OnUnpaused();
			this.OnEnd();
			base.transform.parent = null;
			base.GetComponent<Collider2D>().enabled = false;
			if (!hitEnemy)
			{
				foreach (ParrySwitch parrySwitch2 in this.switches)
				{
					parrySwitch2.OnParryPostPause(this.player);
				}
			}
		}
		yield break;
	}

	// Token: 0x06003462 RID: 13410 RVA: 0x0002AFC9 File Offset: 0x000291C9
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.spark = null;
		this.parryAttack = null;
	}

	// Token: 0x04002B0A RID: 11018
	public const string TAG = "Parry";

	// Token: 0x04002B0B RID: 11019
	public const float PAUSE_TIME = 0.185f;

	// Token: 0x04002B0C RID: 11020
	public const float COLLIDER_LIFETIME = 0.2f;

	// Token: 0x04002B0D RID: 11021
	public const float CHALICE_COLLIDER_LIFETIME = 0.3f;

	// Token: 0x04002B0E RID: 11022
	public const float CHALICE_PLANE_COLLIDER_LIFETIME = 0.4f;

	// Token: 0x04002B0F RID: 11023
	public const float SPRITE_LIFETIME = 1f;

	// Token: 0x04002B10 RID: 11024
	[SerializeField]
	public GameObject sprites;

	// Token: 0x04002B11 RID: 11025
	[SerializeField]
	public Effect spark;

	// Token: 0x04002B12 RID: 11026
	[SerializeField]
	public ParryAttackSpark parryAttack;

	// Token: 0x04002B13 RID: 11027
	public AbstractPlayerController player;

	// Token: 0x04002B14 RID: 11028
	public bool didHitSomething;

	// Token: 0x04002B15 RID: 11029
	public bool cancel;

	// Token: 0x04002B16 RID: 11030
	public List<AbstractProjectile> projectiles;

	// Token: 0x04002B17 RID: 11031
	public List<Effect> sparks;

	// Token: 0x04002B18 RID: 11032
	public List<ParrySwitch> switches;

	// Token: 0x04002B19 RID: 11033
	public List<AbstractLevelEntity> entities;

	// Token: 0x04002B1A RID: 11034
	public RaycastHit2D[] contactsBuffer = new RaycastHit2D[10];
}
