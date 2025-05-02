using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200024C RID: 588
public class FlyingBlimpLevelUFO : AbstractCollidableObject
{
	// Token: 0x06001ADD RID: 6877 RVA: 0x000A9BD8 File Offset: 0x000A7DD8
	public override void Awake()
	{
		base.Awake();
		this.beamTriggered = false;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = DamageDealer.NewEnemy();
		this.collisionChild = this.beamPrefab.GetComponent<CollisionChild>();
		this.collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		float num = (float)Random.Range(0, 2);
		if (num == 0f)
		{
			this.beamPrefab.SetScale(new float?(-base.transform.localScale.x), new float?(base.transform.localScale.y), new float?(base.transform.localScale.z));
		}
	}

	// Token: 0x06001ADE RID: 6878 RVA: 0x00016D2F File Offset: 0x00014F2F
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001ADF RID: 6879 RVA: 0x00016D47 File Offset: 0x00014F47
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06001AE0 RID: 6880 RVA: 0x00016D72 File Offset: 0x00014F72
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001AE1 RID: 6881 RVA: 0x000A9CB4 File Offset: 0x000A7EB4
	public void Init(Vector2 startPos, Vector2 midPos, Vector2 endPos, float speed, float health, LevelProperties.FlyingBlimp.UFO properties)
	{
		base.transform.position = startPos;
		this.ufoMidPoint = midPos;
		this.ufoStopPoint = endPos;
		this.speed = speed;
		this.properties = properties;
		this.health = health;
		base.StartCoroutine(this.to_position_cr());
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x000A9D10 File Offset: 0x000A7F10
	public IEnumerator to_position_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position != this.ufoMidPoint)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.ufoMidPoint, this.speed * CupheadTime.FixedDelta);
			yield return wait;
		}
		base.transform.GetComponent<SpriteRenderer>().sortingOrder = 3;
		while (base.transform.position != this.ufoStopPoint)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.ufoStopPoint, this.speed * CupheadTime.FixedDelta);
			yield return wait;
		}
		base.StartCoroutine(this.move_cr());
		yield break;
	}

	// Token: 0x06001AE3 RID: 6883 RVA: 0x000A9D2C File Offset: 0x000A7F2C
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float offset = 50f;
		while (base.transform.position.x > -640f - offset)
		{
			this.player = PlayerManager.GetNext();
			float dist = this.player.transform.position.x - base.transform.position.x;
			Vector3 pos = base.transform.position;
			pos.x += -this.speed * CupheadTime.FixedDelta;
			base.transform.position = pos;
			this.proximity = ((!this.typeB) ? this.properties.UFOProximityA : this.properties.UFOProximityB);
			if (dist > -this.proximity && dist < this.proximity && !this.beamTriggered)
			{
				this.beamTriggered = true;
				base.StartCoroutine(this.ActivateBeam());
			}
			yield return wait;
		}
		this.Die();
		yield break;
	}

	// Token: 0x06001AE4 RID: 6884 RVA: 0x000A9D48 File Offset: 0x000A7F48
	public IEnumerator ActivateBeam()
	{
		base.animator.SetTrigger("StartBeam");
		yield return CupheadTime.WaitForSeconds(this, this.properties.UFOWarningBeamDuration);
		AudioManager.Play("level_flying_blimp_moon_UFO_fire_laser");
		base.animator.SetTrigger("Continue");
		yield return CupheadTime.WaitForSeconds(this, this.properties.beamDuration);
		base.animator.SetTrigger("End");
		yield break;
	}

	// Token: 0x06001AE5 RID: 6885 RVA: 0x00016D90 File Offset: 0x00014F90
	public void Die()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040015A6 RID: 5542
	public bool typeB;

	// Token: 0x040015A7 RID: 5543
	[SerializeField]
	public Transform beamPrefab;

	// Token: 0x040015A8 RID: 5544
	public DamageDealer damageDealer;

	// Token: 0x040015A9 RID: 5545
	public DamageReceiver damageReceiver;

	// Token: 0x040015AA RID: 5546
	public CollisionChild collisionChild;

	// Token: 0x040015AB RID: 5547
	public Vector3 ufoMidPoint;

	// Token: 0x040015AC RID: 5548
	public Vector3 ufoStopPoint;

	// Token: 0x040015AD RID: 5549
	public AbstractPlayerController player;

	// Token: 0x040015AE RID: 5550
	public LevelProperties.FlyingBlimp.UFO properties;

	// Token: 0x040015AF RID: 5551
	public float speed;

	// Token: 0x040015B0 RID: 5552
	public float health;

	// Token: 0x040015B1 RID: 5553
	public float proximity;

	// Token: 0x040015B2 RID: 5554
	public bool beamTriggered;
}
