using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200034F RID: 847
public class RumRunnersLevelPoliceman : AbstractCollidableObject
{
	// Token: 0x17000311 RID: 785
	// (get) Token: 0x06002511 RID: 9489 RVA: 0x0001F43C File Offset: 0x0001D63C
	// (set) Token: 0x06002512 RID: 9490 RVA: 0x0001F444 File Offset: 0x0001D644
	public bool isActive { get; set; }

	// Token: 0x06002513 RID: 9491 RVA: 0x000C57E0 File Offset: 0x000C39E0
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.scaleX = base.transform.localScale.x;
		this.collider = base.GetComponent<Collider2D>();
		this.collider.enabled = false;
	}

	// Token: 0x06002514 RID: 9492 RVA: 0x0001F44D File Offset: 0x0001D64D
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002515 RID: 9493 RVA: 0x0001F465 File Offset: 0x0001D665
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002516 RID: 9494 RVA: 0x000C584C File Offset: 0x000C3A4C
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f && this.deathCoroutine == null)
		{
			Level.Current.RegisterMinionKilled();
			this.StopAllCoroutines();
			this.deathCoroutine = base.StartCoroutine(this.death_cr());
		}
	}

	// Token: 0x06002517 RID: 9495 RVA: 0x0001F483 File Offset: 0x0001D683
	public void SetProperties(LevelProperties.RumRunners.Spider properties, RumRunnersLevelSpider spider)
	{
		this.properties = properties;
		this.spider = spider;
	}

	// Token: 0x06002518 RID: 9496 RVA: 0x000C58AC File Offset: 0x000C3AAC
	public void CopAppear(Vector3 appearPos, bool isPink, bool goingLeft)
	{
		if (this.deathCoroutine != null)
		{
			return;
		}
		Vector3 vector = this.spawnPositionOffset;
		vector.x *= (float)((!goingLeft) ? 1 : -1);
		base.transform.position = appearPos + vector;
		this.isPink = isPink;
		this.hp = this.properties.copHealth;
		base.StartCoroutine(this.shooting_cr());
		base.transform.SetScale(new float?((!goingLeft) ? this.scaleX : (-this.scaleX)), null, null);
		this.isActive = true;
	}

	// Token: 0x06002519 RID: 9497 RVA: 0x000C5968 File Offset: 0x000C3B68
	public IEnumerator shooting_cr()
	{
		this.collider.enabled = true;
		this.gunSmokeRenderer.enabled = !this.isPink;
		this.gunSmokeParryRenderer.enabled = this.isPink;
		this.lastShootDirection = this.calculateDirection();
		string animatorParameter;
		string stateBaseName;
		if (this.lastShootDirection == RumRunnersLevelPoliceman.Direction.Down)
		{
			animatorParameter = "ShootingDown";
			stateBaseName = "ShootDown";
			this.currentBulletOrigin = this.bulletOriginDown;
		}
		else if (this.lastShootDirection == RumRunnersLevelPoliceman.Direction.Up)
		{
			animatorParameter = "ShootingUp";
			stateBaseName = "ShootUp";
			this.currentBulletOrigin = this.bulletOriginUp;
		}
		else
		{
			animatorParameter = "Shooting";
			stateBaseName = "ShootStraight";
			this.currentBulletOrigin = this.bulletOriginStraight;
		}
		Coroutine alignmentCoroutine = base.StartCoroutine(this.align_cr());
		base.animator.SetBool(animatorParameter, true);
		yield return base.animator.WaitForAnimationToStart(this, stateBaseName + "Hold", false);
		yield return CupheadTime.WaitForSeconds(this, this.properties.copAttackWarning);
		base.animator.SetTrigger("Shoot");
		yield return base.animator.WaitForAnimationToStart(this, stateBaseName + "ExitHold", false);
		yield return CupheadTime.WaitForSeconds(this, this.properties.copExitDelay);
		base.animator.SetTrigger("ShootExit");
		yield return base.animator.WaitForAnimationToStart(this, "ShootExit", false);
		yield return null;
		while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			yield return null;
		}
		this.collider.enabled = false;
		base.transform.position = new Vector3(0f, 1000f);
		base.animator.SetBool(animatorParameter, false);
		this.isActive = false;
		base.StopCoroutine(alignmentCoroutine);
		yield break;
	}

	// Token: 0x0600251A RID: 9498 RVA: 0x000C5984 File Offset: 0x000C3B84
	public IEnumerator align_cr()
	{
		YieldInstruction waitInstruction = new WaitForFixedUpdate();
		for (;;)
		{
			yield return waitInstruction;
			base.transform.SetPosition(null, new float?(RumRunnersLevel.GroundWalkingPosY(base.transform.position, this.collider, 0f, 200f)), null);
		}
		yield break;
	}

	// Token: 0x0600251B RID: 9499 RVA: 0x000C59A0 File Offset: 0x000C3BA0
	public IEnumerator death_cr()
	{
		this.SFX_RUMRUN_Police_DiePoof();
		Vector3 puffPosition = this.collider.bounds.center;
		base.animator.SetBool("Shooting", false);
		base.animator.SetBool("ShootingUp", false);
		base.animator.SetBool("ShootingDown", false);
		this.isActive = false;
		this.collider.enabled = false;
		base.transform.position = puffPosition;
		base.animator.SetBool("Die", true);
		yield return base.animator.WaitForAnimationToStart(this, "Death", false);
		while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			yield return null;
		}
		base.transform.position = new Vector3(0f, 1000f);
		base.animator.SetBool("Die", false);
		this.deathCoroutine = null;
		yield break;
	}

	// Token: 0x0600251C RID: 9500 RVA: 0x000C59BC File Offset: 0x000C3BBC
	public void animationEvent_SpawnBullet()
	{
		Vector3 vector = this.spider.transform.position - base.transform.position;
		RumRunnersLevelPoliceBullet rumRunnersLevelPoliceBullet = (RumRunnersLevelPoliceBullet)this.regularBullet.Create(this.currentBulletOrigin.transform.position, MathUtils.DirectionToAngle(vector), this.properties.copBulletSpeed);
		rumRunnersLevelPoliceBullet.spiderDamage = this.properties.copBulletBossDamage;
		rumRunnersLevelPoliceBullet.direction = this.lastShootDirection;
		rumRunnersLevelPoliceBullet.SetParryable(this.isPink);
		rumRunnersLevelPoliceBullet.GetComponent<SpriteRenderer>().flipY = (Mathf.Sign(vector.x) < 0f);
	}

	// Token: 0x0600251D RID: 9501 RVA: 0x0001F493 File Offset: 0x0001D693
	public void animationEvent_ExitDisappeared()
	{
		this.collider.enabled = false;
	}

	// Token: 0x0600251E RID: 9502 RVA: 0x000C5A70 File Offset: 0x000C3C70
	public RumRunnersLevelPoliceman.Direction calculateDirection()
	{
		if (base.transform.position.y - this.spider.transform.position.y > RumRunnersLevelPoliceman.SpiderYDistanceThreshold)
		{
			return RumRunnersLevelPoliceman.Direction.Down;
		}
		if (this.spider.transform.position.y - base.transform.position.y > RumRunnersLevelPoliceman.SpiderYDistanceThreshold)
		{
			return RumRunnersLevelPoliceman.Direction.Up;
		}
		return RumRunnersLevelPoliceman.Direction.Straight;
	}

	// Token: 0x0600251F RID: 9503 RVA: 0x0001F4A1 File Offset: 0x0001D6A1
	public void AnimationEvent_SFX_RUMRUN_Police_GunShoot()
	{
		AudioManager.Play("sfx_dlc_rumrun_policegun_shoot");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_policegun_shoot");
	}

	// Token: 0x06002520 RID: 9504 RVA: 0x0001F4BD File Offset: 0x0001D6BD
	public void SFX_RUMRUN_Police_DiePoof()
	{
		AudioManager.Play("sfx_dlc_rumrun_lackey_poof");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_lackey_poof");
		AudioManager.Stop("sfx_dlc_rumrun_policegun_shoot");
	}

	// Token: 0x04001EA8 RID: 7848
	public static readonly float SpiderYDistanceThreshold = 100f;

	// Token: 0x04001EA9 RID: 7849
	[SerializeField]
	public RumRunnersLevelPoliceBullet regularBullet;

	// Token: 0x04001EAA RID: 7850
	[SerializeField]
	public Transform bulletOriginStraight;

	// Token: 0x04001EAB RID: 7851
	[SerializeField]
	public Transform bulletOriginUp;

	// Token: 0x04001EAC RID: 7852
	[SerializeField]
	public Transform bulletOriginDown;

	// Token: 0x04001EAD RID: 7853
	[SerializeField]
	public Vector2 spawnPositionOffset;

	// Token: 0x04001EAE RID: 7854
	[SerializeField]
	public SpriteRenderer gunSmokeRenderer;

	// Token: 0x04001EAF RID: 7855
	[SerializeField]
	public SpriteRenderer gunSmokeParryRenderer;

	// Token: 0x04001EB0 RID: 7856
	public LevelProperties.RumRunners.Spider properties;

	// Token: 0x04001EB1 RID: 7857
	public RumRunnersLevelSpider spider;

	// Token: 0x04001EB2 RID: 7858
	public DamageDealer damageDealer;

	// Token: 0x04001EB3 RID: 7859
	public DamageReceiver damageReceiver;

	// Token: 0x04001EB4 RID: 7860
	public bool isPink;

	// Token: 0x04001EB5 RID: 7861
	public float hp;

	// Token: 0x04001EB6 RID: 7862
	public float scaleX;

	// Token: 0x04001EB7 RID: 7863
	public Collider2D collider;

	// Token: 0x04001EB8 RID: 7864
	public Transform currentBulletOrigin;

	// Token: 0x04001EB9 RID: 7865
	public Coroutine deathCoroutine;

	// Token: 0x04001EBA RID: 7866
	public RumRunnersLevelPoliceman.Direction lastShootDirection;

	// Token: 0x02000EBF RID: 3775
	public enum Direction
	{
		// Token: 0x04006A0F RID: 27151
		Straight,
		// Token: 0x04006A10 RID: 27152
		Up,
		// Token: 0x04006A11 RID: 27153
		Down
	}
}
