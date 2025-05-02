using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003CE RID: 974
public class VeggiesLevelOnionHomingHeart : AbstractProjectile
{
	// Token: 0x1700033C RID: 828
	// (get) Token: 0x06002AF1 RID: 10993 RVA: 0x000240F5 File Offset: 0x000222F5
	// (set) Token: 0x06002AF2 RID: 10994 RVA: 0x000240FD File Offset: 0x000222FD
	public VeggiesLevelOnionHomingHeart.State state { get; set; }

	// Token: 0x06002AF3 RID: 10995 RVA: 0x000D52B8 File Offset: 0x000D34B8
	public VeggiesLevelOnionHomingHeart CreateRadish(Vector2 pos, float max, float acc, int hp, bool onLeft)
	{
		base.transform.position = pos;
		VeggiesLevelOnionHomingHeart veggiesLevelOnionHomingHeart = base.Create() as VeggiesLevelOnionHomingHeart;
		veggiesLevelOnionHomingHeart.maxSpeed = max;
		veggiesLevelOnionHomingHeart.acceletration = acc;
		veggiesLevelOnionHomingHeart.health = (float)hp;
		veggiesLevelOnionHomingHeart.isOnLeft = onLeft;
		return veggiesLevelOnionHomingHeart;
	}

	// Token: 0x1700033D RID: 829
	// (get) Token: 0x06002AF4 RID: 10996 RVA: 0x00024106 File Offset: 0x00022306
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002AF5 RID: 10997 RVA: 0x000D5304 File Offset: 0x000D3504
	public override void Start()
	{
		if (!this.isOnLeft)
		{
			base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
		}
		base.Start();
		this.sprite = base.GetComponent<SpriteRenderer>();
		this.homingMovement = base.GetComponent<GroundHomingMovement>();
		this.homingMovement.maxSpeed = this.maxSpeed;
		this.homingMovement.acceleration = this.acceletration;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.start_cr());
	}

	// Token: 0x06002AF6 RID: 10998 RVA: 0x0002410D File Offset: 0x0002230D
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002AF7 RID: 10999 RVA: 0x0002412B File Offset: 0x0002232B
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002AF8 RID: 11000 RVA: 0x000D53C4 File Offset: 0x000D35C4
	public IEnumerator start_cr()
	{
		this.state = VeggiesLevelOnionHomingHeart.State.Alive;
		this.sprite.sortingLayerName = SpriteLayer.Enemies.ToString();
		this.sprite.sortingOrder = 0;
		yield return base.animator.WaitForAnimationToEnd(this, "Radish_Intro", false, true);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		AudioManager.PlayLoop("level_veggies_raddish_loop");
		this.emitAudioFromObject.Add("level_veggies_raddish_loop");
		this.homingMovement.EnableHoming = true;
		base.StartCoroutine(this.loop_cr());
		yield break;
	}

	// Token: 0x06002AF9 RID: 11001 RVA: 0x00024149 File Offset: 0x00022349
	public void ChangeLayer()
	{
		this.sprite.sortingOrder = 3;
	}

	// Token: 0x06002AFA RID: 11002 RVA: 0x000D53E0 File Offset: 0x000D35E0
	public IEnumerator loop_cr()
	{
		while (this.state != VeggiesLevelOnionHomingHeart.State.Dead)
		{
			this.homingMovement.TrackingPlayer = PlayerManager.GetNext();
			yield return CupheadTime.WaitForSeconds(this, 20f);
		}
		yield break;
	}

	// Token: 0x06002AFB RID: 11003 RVA: 0x000D53FC File Offset: 0x000D35FC
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f && this.state != VeggiesLevelOnionHomingHeart.State.Dead)
		{
			this.state = VeggiesLevelOnionHomingHeart.State.Dead;
			this.homingMovement.enabled = false;
			this.StopAllCoroutines();
			AudioManager.Stop("level_veggies_raddish_loop");
			AudioManager.Play("level_veggies_raddish_End");
			this.emitAudioFromObject.Add("level_veggies_raddish_End");
			base.animator.SetTrigger("Dead");
		}
	}

	// Token: 0x06002AFC RID: 11004 RVA: 0x00024157 File Offset: 0x00022357
	public void CreateEffect()
	{
		this.deathPoof.Create(base.transform.position);
	}

	// Token: 0x06002AFD RID: 11005 RVA: 0x000D5488 File Offset: 0x000D3688
	public void CreatePieces()
	{
		foreach (SpriteDeathParts spriteDeathParts in this.deathPieces)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
	}

	// Token: 0x06002AFE RID: 11006 RVA: 0x00024170 File Offset: 0x00022370
	public void Destroy()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002AFF RID: 11007 RVA: 0x0002417D File Offset: 0x0002237D
	public void RaddishBonkSFX()
	{
		AudioManager.Play("level_veggies_raddish_bonk");
		this.emitAudioFromObject.Add("level_veggies_raddish_bonk");
	}

	// Token: 0x06002B00 RID: 11008 RVA: 0x00024199 File Offset: 0x00022399
	public void RaddishLoopStartSFX()
	{
		AudioManager.Play("level_veggies_raddish_start");
		this.emitAudioFromObject.Add("level_veggies_raddish_start");
	}

	// Token: 0x06002B01 RID: 11009 RVA: 0x000241B5 File Offset: 0x000223B5
	public void RaddishDeathSFX()
	{
		AudioManager.Play("level_veggies_raddish_death");
		this.emitAudioFromObject.Add("level_veggies_raddish_death");
	}

	// Token: 0x06002B02 RID: 11010 RVA: 0x000241D1 File Offset: 0x000223D1
	public void RaddishVoiceDeathSFX()
	{
		AudioManager.Play("veggies_Raddish_Voice_Death");
		this.emitAudioFromObject.Add("veggies_Raddish_Voice_Death");
	}

	// Token: 0x06002B03 RID: 11011 RVA: 0x000241ED File Offset: 0x000223ED
	public void RaddishVoiceIntroSFX()
	{
		AudioManager.Play("veggies_Raddish_Voice_Intro");
		this.emitAudioFromObject.Add("veggies_Raddish_Voice_Intro");
	}

	// Token: 0x040023BE RID: 9150
	[SerializeField]
	public Effect deathPoof;

	// Token: 0x040023BF RID: 9151
	[SerializeField]
	public SpriteDeathParts[] deathPieces;

	// Token: 0x040023C0 RID: 9152
	public AbstractPlayerController player;

	// Token: 0x040023C1 RID: 9153
	public bool isOnLeft;

	// Token: 0x040023C3 RID: 9155
	public SpriteRenderer sprite;

	// Token: 0x040023C4 RID: 9156
	public GroundHomingMovement homingMovement;

	// Token: 0x040023C5 RID: 9157
	public DamageReceiver damageReceiver;

	// Token: 0x040023C6 RID: 9158
	public float maxSpeed;

	// Token: 0x040023C7 RID: 9159
	public float acceletration;

	// Token: 0x040023C8 RID: 9160
	public float health;

	// Token: 0x02000FEE RID: 4078
	public enum State
	{
		// Token: 0x0400723B RID: 29243
		Alive,
		// Token: 0x0400723C RID: 29244
		Dead
	}
}
