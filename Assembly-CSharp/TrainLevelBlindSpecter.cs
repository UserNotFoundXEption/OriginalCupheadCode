using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003A7 RID: 935
public class TrainLevelBlindSpecter : LevelProperties.Train.Entity
{
	// Token: 0x1400004F RID: 79
	// (add) Token: 0x0600296E RID: 10606 RVA: 0x000D1FF8 File Offset: 0x000D01F8
	// (remove) Token: 0x0600296F RID: 10607 RVA: 0x000D2030 File Offset: 0x000D0230
	public event TrainLevelBlindSpecter.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x14000050 RID: 80
	// (add) Token: 0x06002970 RID: 10608 RVA: 0x000D2068 File Offset: 0x000D0268
	// (remove) Token: 0x06002971 RID: 10609 RVA: 0x000D20A0 File Offset: 0x000D02A0
	public event Action OnDeathEvent;

	// Token: 0x06002972 RID: 10610 RVA: 0x000D20D8 File Offset: 0x000D02D8
	public override void Awake()
	{
		base.Awake();
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = DamageDealer.NewEnemy();
		base.animator.enabled = false;
		this.spriteRenderer.enabled = false;
	}

	// Token: 0x06002973 RID: 10611 RVA: 0x00022D3B File Offset: 0x00020F3B
	public void Start()
	{
		Level.Current.OnIntroEvent += this.OnIntro;
	}

	// Token: 0x06002974 RID: 10612 RVA: 0x00022D53 File Offset: 0x00020F53
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002975 RID: 10613 RVA: 0x00022D6B File Offset: 0x00020F6B
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002976 RID: 10614 RVA: 0x000D2140 File Offset: 0x000D0340
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.dead)
		{
			return;
		}
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06002977 RID: 10615 RVA: 0x00022D94 File Offset: 0x00020F94
	public override void LevelInit(LevelProperties.Train properties)
	{
		base.LevelInit(properties);
		this.health = (float)properties.CurrentState.blindSpecter.health;
	}

	// Token: 0x06002978 RID: 10616 RVA: 0x00022DB4 File Offset: 0x00020FB4
	public void OnIntro()
	{
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06002979 RID: 10617 RVA: 0x000D21A0 File Offset: 0x000D03A0
	public void Die()
	{
		if (this.dead)
		{
			return;
		}
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		this.dead = true;
		this.damageReceiver.enabled = false;
		this.StopAllCoroutines();
		base.animator.Play("Death");
		AudioManager.Play("train_blindspector_death");
		this.emitAudioFromObject.Add("train_blindspector_death");
	}

	// Token: 0x0600297A RID: 10618 RVA: 0x00022DC3 File Offset: 0x00020FC3
	public void OnDeathAnimComplete()
	{
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600297B RID: 10619 RVA: 0x00022DE6 File Offset: 0x00020FE6
	public void SfxIntro()
	{
		AudioManager.Play("level_train_blindspecter_intro");
	}

	// Token: 0x0600297C RID: 10620 RVA: 0x000D2208 File Offset: 0x000D0408
	public void FireEyeball()
	{
		AudioManager.Play("train_blindspector_attack");
		this.emitAudioFromObject.Add("train_blindspector_attack");
		float value = Random.value;
		Vector2 time;
		time..ctor(this.blindSpecterProperties.timeX.RandomFloat(), this.blindSpecterProperties.timeY.GetFloatAt(value));
		this.eyePrefab.Create(this.eyeRoot.position, time, this.blindSpecterProperties.heightMax.GetFloatAt(value), this.shots % 2 > 0, this.blindSpecterProperties.eyeHealth);
		this.shots++;
	}

	// Token: 0x0600297D RID: 10621 RVA: 0x000D22B0 File Offset: 0x000D04B0
	public IEnumerator loop_cr()
	{
		base.animator.enabled = true;
		this.spriteRenderer.enabled = true;
		base.animator.Play("Intro");
		this.blindSpecterProperties = base.properties.CurrentState.blindSpecter;
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		yield return CupheadTime.WaitForSeconds(this, 2f);
		for (;;)
		{
			this.shots = 0;
			base.animator.Play("Attack_Start");
			while (this.shots < this.blindSpecterProperties.attackLoops * 2)
			{
				yield return null;
			}
			base.animator.SetTrigger("Continue");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_End", false, true);
			yield return CupheadTime.WaitForSeconds(this, this.blindSpecterProperties.hesitate);
		}
		yield break;
	}

	// Token: 0x0600297E RID: 10622 RVA: 0x00022DF2 File Offset: 0x00020FF2
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.eyePrefab = null;
	}

	// Token: 0x040022B2 RID: 8882
	[SerializeField]
	public Transform eyeRoot;

	// Token: 0x040022B3 RID: 8883
	[SerializeField]
	public TrainLevelBlindSpecterEyeProjectile eyePrefab;

	// Token: 0x040022B4 RID: 8884
	public SpriteRenderer spriteRenderer;

	// Token: 0x040022B5 RID: 8885
	public LevelProperties.Train.BlindSpecter blindSpecterProperties;

	// Token: 0x040022B6 RID: 8886
	public int shots;

	// Token: 0x040022B7 RID: 8887
	public DamageDealer damageDealer;

	// Token: 0x040022B8 RID: 8888
	public DamageReceiver damageReceiver;

	// Token: 0x040022B9 RID: 8889
	public float health;

	// Token: 0x040022BA RID: 8890
	public bool dead;

	// Token: 0x02000F9D RID: 3997
	// (Invoke) Token: 0x06007543 RID: 30019
	public delegate void OnDamageTakenHandler(float damage);
}
