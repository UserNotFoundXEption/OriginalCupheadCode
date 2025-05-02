using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000262 RID: 610
public class FlyingCowboyLevelUFO : AbstractProjectile
{
	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06001BE3 RID: 7139 RVA: 0x000179EE File Offset: 0x00015BEE
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001BE4 RID: 7140 RVA: 0x000179F5 File Offset: 0x00015BF5
	public virtual FlyingCowboyLevelUFO Init(Vector3 pos, LevelProperties.FlyingCowboy.UFOEnemy properties, float health)
	{
		base.ResetLifetime();
		base.ResetDistance();
		this.startPos = pos;
		base.transform.position = pos;
		this.properties = properties;
		this.Health = health;
		base.StartCoroutine(this.move_cr());
		return this;
	}

	// Token: 0x06001BE5 RID: 7141 RVA: 0x00017A32 File Offset: 0x00015C32
	public override void Start()
	{
		base.Start();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001BE6 RID: 7142 RVA: 0x00017A5D File Offset: 0x00015C5D
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.Health -= info.damage;
		if (this.Health < 0f && !this.isDead)
		{
			Level.Current.RegisterMinionKilled();
			this.Respawn();
		}
	}

	// Token: 0x06001BE7 RID: 7143 RVA: 0x00017A9D File Offset: 0x00015C9D
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001BE8 RID: 7144 RVA: 0x000ACF1C File Offset: 0x000AB11C
	public IEnumerator move_cr()
	{
		this.isDead = false;
		float leftEdge = -540f;
		float initialLeftEdge = leftEdge - 100f;
		float rightEdge = -640f + this.properties.ufoPathLength;
		float initialX = base.transform.position.x;
		float travelDistance = Mathf.Abs(initialX - initialLeftEdge);
		float travelTime = travelDistance / this.properties.introUFOSpeed;
		float elapsedTime = 0f;
		while (elapsedTime < travelTime)
		{
			elapsedTime += CupheadTime.FixedDelta;
			Vector3 position = base.transform.position;
			position.x = EaseUtils.Ease(EaseUtils.EaseType.easeOutQuad, initialX, initialLeftEdge, elapsedTime / travelTime);
			base.transform.position = position;
			yield return new WaitForFixedUpdate();
		}
		this.movingLeft = false;
		base.StartCoroutine(this.shoot_cr());
		float currentLeftEdge = initialLeftEdge;
		travelDistance = Mathf.Abs(rightEdge - leftEdge);
		travelTime = travelDistance / this.properties.topUFOSpeed;
		elapsedTime = 0f;
		for (;;)
		{
			if (elapsedTime < travelTime)
			{
				elapsedTime += CupheadTime.FixedDelta;
				Vector3 position2 = base.transform.position;
				position2.x = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, (!this.movingLeft) ? currentLeftEdge : rightEdge, (!this.movingLeft) ? rightEdge : currentLeftEdge, elapsedTime / travelTime);
				base.transform.position = position2;
			}
			else
			{
				currentLeftEdge = leftEdge;
				elapsedTime = 0f;
				this.movingLeft = !this.movingLeft;
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06001BE9 RID: 7145 RVA: 0x000ACF38 File Offset: 0x000AB138
	public IEnumerator shoot_cr()
	{
		PatternString shootString = new PatternString(this.properties.topUFOShootString, true, true);
		PatternString parryString = new PatternString(this.properties.bulletParryString, true);
		for (;;)
		{
			MinMax spreadAngle = new MinMax(0f, this.properties.spreadAngle);
			yield return CupheadTime.WaitForSeconds(this, shootString.PopFloat());
			for (int i = 0; i < this.properties.bulletCount; i++)
			{
				float num = spreadAngle.GetFloatAt((float)i / ((float)this.properties.bulletCount - 1f));
				float num2 = spreadAngle.max / 2f;
				num -= num2;
				float rotation = num + -90f;
				BasicProjectile basicProjectile = this.projectilePrefab.Create(base.transform.position, rotation, this.properties.bulletSpeed);
				bool flag = parryString.PopLetter() == 'P';
				basicProjectile.SetParryable(flag);
				if (flag)
				{
					basicProjectile.GetComponent<SpriteRenderer>().color = Color.magenta;
				}
			}
		}
		yield break;
	}

	// Token: 0x06001BEA RID: 7146 RVA: 0x00017ABB File Offset: 0x00015CBB
	public void Respawn()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.respawn_cr());
	}

	// Token: 0x06001BEB RID: 7147 RVA: 0x000ACF54 File Offset: 0x000AB154
	public IEnumerator respawn_cr()
	{
		this.isDead = true;
		base.transform.position = new Vector3(1000f, 1000f);
		float waitTime = this.properties.topUFORespawnDelay;
		yield return CupheadTime.WaitForSeconds(this, waitTime);
		this.Health = this.properties.UFOHealth;
		base.transform.position = this.startPos;
		base.StartCoroutine(this.move_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001BEC RID: 7148 RVA: 0x00017AD0 File Offset: 0x00015CD0
	public void Dead()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040016A7 RID: 5799
	[SerializeField]
	public BasicProjectile projectilePrefab;

	// Token: 0x040016A8 RID: 5800
	public const float LEFT_OFFSET = 100f;

	// Token: 0x040016A9 RID: 5801
	public const float INITIAL_LEFT_OFFSET = 100f;

	// Token: 0x040016AA RID: 5802
	public LevelProperties.FlyingCowboy.UFOEnemy properties;

	// Token: 0x040016AB RID: 5803
	public DamageReceiver damageReceiver;

	// Token: 0x040016AC RID: 5804
	public Vector3 startPos;

	// Token: 0x040016AD RID: 5805
	public bool isDead;

	// Token: 0x040016AE RID: 5806
	public bool movingLeft;

	// Token: 0x040016AF RID: 5807
	public float Health;
}
