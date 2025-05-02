using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001AA RID: 426
public class DevilLevelDemon : AbstractCollidableObject
{
	// Token: 0x17000261 RID: 609
	// (get) Token: 0x0600143A RID: 5178 RVA: 0x00011092 File Offset: 0x0000F292
	// (set) Token: 0x0600143B RID: 5179 RVA: 0x0001109A File Offset: 0x0000F29A
	public Vector3 JumpRoot { get; set; }

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x0600143C RID: 5180 RVA: 0x000110A3 File Offset: 0x0000F2A3
	// (set) Token: 0x0600143D RID: 5181 RVA: 0x000110AB File Offset: 0x0000F2AB
	public Vector3 RunRoot { get; set; }

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x0600143E RID: 5182 RVA: 0x000110B4 File Offset: 0x0000F2B4
	// (set) Token: 0x0600143F RID: 5183 RVA: 0x000110BC File Offset: 0x0000F2BC
	public Vector3 PillarDestination { get; set; }

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06001440 RID: 5184 RVA: 0x000110C5 File Offset: 0x0000F2C5
	// (set) Token: 0x06001441 RID: 5185 RVA: 0x000110CD File Offset: 0x0000F2CD
	public Vector3 FrontSpawn { get; set; }

	// Token: 0x06001442 RID: 5186 RVA: 0x000110D6 File Offset: 0x0000F2D6
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001443 RID: 5187 RVA: 0x0001110C File Offset: 0x0000F30C
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001444 RID: 5188 RVA: 0x00011124 File Offset: 0x0000F324
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001445 RID: 5189 RVA: 0x00099C20 File Offset: 0x00097E20
	public DevilLevelDemon Create(Vector2 position, float direction, float speed, float hp, DevilLevelSittingDevil parent)
	{
		DevilLevelDemon devilLevelDemon = this.InstantiatePrefab<DevilLevelDemon>();
		devilLevelDemon.transform.position = position;
		devilLevelDemon.frontDirection = direction;
		devilLevelDemon.speed = speed;
		devilLevelDemon.hp = hp;
		devilLevelDemon.parent = parent;
		devilLevelDemon.transform.localScale = new Vector3(direction * 0.9f, 0.9f, 0.9f);
		devilLevelDemon.sprite.color = this.backgroundTint;
		int num = Random.Range(0, 3);
		if (num == 0)
		{
			devilLevelDemon.animator.SetFloat("PeekVariation", (float)Random.Range(0, 3) / 2f);
			devilLevelDemon.animator.SetTrigger("JumpOut");
		}
		else if (num == 1)
		{
			devilLevelDemon.animator.SetFloat("PeekVariation", (float)Random.Range(0, 3) / 2f);
			devilLevelDemon.animator.SetTrigger("RunOut");
		}
		else
		{
			devilLevelDemon.animator.Play("JumpOut");
		}
		return devilLevelDemon;
	}

	// Token: 0x06001446 RID: 5190 RVA: 0x0001114D File Offset: 0x0000F34D
	public void Start()
	{
		DevilLevelSittingDevil devilLevelSittingDevil = this.parent;
		devilLevelSittingDevil.OnPhase1Death = (Action)Delegate.Combine(devilLevelSittingDevil.OnPhase1Death, new Action(this.Die));
	}

	// Token: 0x06001447 RID: 5191 RVA: 0x00011176 File Offset: 0x0000F376
	public void PlaceForJump()
	{
		base.transform.position = this.JumpRoot;
		this.hasJumped = true;
	}

	// Token: 0x06001448 RID: 5192 RVA: 0x00011190 File Offset: 0x0000F390
	public void StartMoving()
	{
		if (!this.moving)
		{
			base.StartCoroutine(this.demonMovement_cr());
			AudioManager.Play("devil_imp_spawn");
			this.emitAudioFromObject.Add("devil_imp_spawn");
		}
	}

	// Token: 0x06001449 RID: 5193 RVA: 0x00099D24 File Offset: 0x00097F24
	public IEnumerator demonMovement_cr()
	{
		this.moving = true;
		if (!this.hasJumped)
		{
			base.transform.position = this.RunRoot;
		}
		Vector3 backDirection = (this.PillarDestination - base.transform.position).normalized;
		while (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(150f, 150f)))
		{
			base.transform.position += backDirection * this.speed * CupheadTime.Delta;
			float scaleDelta = 0.0999999642f * CupheadTime.Delta;
			base.transform.localScale -= new Vector3(this.frontDirection * scaleDelta, scaleDelta, scaleDelta);
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.frontWaitTime);
		base.transform.localScale = new Vector3(-this.frontDirection, 1f, 1f);
		base.transform.position = this.FrontSpawn;
		this.collider2d.enabled = true;
		this.sprite.sortingLayerName = "Enemies";
		this.sprite.sortingOrder = 0;
		this.sprite.color = Color.black;
		for (;;)
		{
			base.transform.AddPosition(this.frontDirection * this.speed * CupheadTime.Delta, 0f, 0f);
			if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(150f, 150f)))
			{
				this.enteredScreen = true;
			}
			else if (this.enteredScreen)
			{
				Object.Destroy(base.gameObject);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600144A RID: 5194 RVA: 0x000111C4 File Offset: 0x0000F3C4
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f)
		{
			this.Die();
		}
	}

	// Token: 0x0600144B RID: 5195 RVA: 0x000111EF File Offset: 0x0000F3EF
	public void RemoveEvent()
	{
		DevilLevelSittingDevil devilLevelSittingDevil = this.parent;
		devilLevelSittingDevil.OnPhase1Death = (Action)Delegate.Remove(devilLevelSittingDevil.OnPhase1Death, new Action(this.Die));
	}

	// Token: 0x0600144C RID: 5196 RVA: 0x00011218 File Offset: 0x0000F418
	public override void OnDestroy()
	{
		this.RemoveEvent();
		base.OnDestroy();
	}

	// Token: 0x0600144D RID: 5197 RVA: 0x00099D40 File Offset: 0x00097F40
	public void Die()
	{
		AudioManager.Play("devil_imp_death");
		this.emitAudioFromObject.Add("devil_imp_death");
		this.explosion.Create(this.collider2d.bounds.center);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600144E RID: 5198 RVA: 0x00011226 File Offset: 0x0000F426
	public void ImpStepSFX()
	{
		AudioManager.Play("devil_imp_step");
		this.emitAudioFromObject.Add("devil_imp_step");
	}

	// Token: 0x04001081 RID: 4225
	public const string PeekVariationParameterName = "PeekVariation";

	// Token: 0x04001082 RID: 4226
	public const string JumpOutParameterName = "JumpOut";

	// Token: 0x04001083 RID: 4227
	public const string RunOutParameterName = "RunOut";

	// Token: 0x04001084 RID: 4228
	public const string JumpOutStateName = "JumpOut";

	// Token: 0x04001085 RID: 4229
	public const string EnemyLayerName = "Enemies";

	// Token: 0x04001086 RID: 4230
	public const int PeekVariations = 3;

	// Token: 0x04001087 RID: 4231
	public const float StartScale = 0.9f;

	// Token: 0x04001088 RID: 4232
	public const float PillarScale = 0.8f;

	// Token: 0x04001089 RID: 4233
	[SerializeField]
	public Collider2D collider2d;

	// Token: 0x0400108A RID: 4234
	[SerializeField]
	public float frontWaitTime;

	// Token: 0x0400108B RID: 4235
	[SerializeField]
	public SpriteRenderer sprite;

	// Token: 0x0400108C RID: 4236
	[SerializeField]
	public Color backgroundTint;

	// Token: 0x0400108D RID: 4237
	[SerializeField]
	public PlatformingLevelGenericExplosion explosion;

	// Token: 0x0400108E RID: 4238
	public DamageDealer damageDealer;

	// Token: 0x0400108F RID: 4239
	public bool enteredScreen;

	// Token: 0x04001090 RID: 4240
	public float frontDirection;

	// Token: 0x04001091 RID: 4241
	public float speed;

	// Token: 0x04001092 RID: 4242
	public float hp;

	// Token: 0x04001093 RID: 4243
	public bool moving;

	// Token: 0x04001094 RID: 4244
	public bool hasJumped;

	// Token: 0x04001095 RID: 4245
	public DamageReceiver damageReceiver;

	// Token: 0x04001096 RID: 4246
	public DevilLevelSittingDevil parent;
}
