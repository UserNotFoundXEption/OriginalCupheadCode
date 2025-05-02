using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000141 RID: 321
public class AirshipStorkLevelBaby : AbstractCollidableObject
{
	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06000F30 RID: 3888 RVA: 0x0000CD5C File Offset: 0x0000AF5C
	// (set) Token: 0x06000F31 RID: 3889 RVA: 0x0000CD64 File Offset: 0x0000AF64
	public AirshipStorkLevelBaby.State state { get; set; }

	// Token: 0x06000F32 RID: 3890 RVA: 0x0000CD6D File Offset: 0x0000AF6D
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x0000CDA3 File Offset: 0x0000AFA3
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x0000CDBB File Offset: 0x0000AFBB
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f && this.state != AirshipStorkLevelBaby.State.Dying)
		{
			this.state = AirshipStorkLevelBaby.State.Dying;
			this.Die();
		}
	}

	// Token: 0x06000F35 RID: 3893 RVA: 0x0000CDF9 File Offset: 0x0000AFF9
	public void Init(LevelProperties.AirshipStork.Babies properties, Vector2 pos, float health)
	{
		this.properties = properties;
		this.health = health;
		base.transform.position = pos;
		base.StartCoroutine(this.jump_cr());
	}

	// Token: 0x06000F36 RID: 3894 RVA: 0x0000CE27 File Offset: 0x0000B027
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06000F37 RID: 3895 RVA: 0x0008D16C File Offset: 0x0008B36C
	public IEnumerator jump_cr()
	{
		this.state = AirshipStorkLevelBaby.State.Move;
		string[] pattern = this.properties.babyDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int i = Random.Range(0, pattern.Length);
		float waitTime = 0f;
		this.onGroundY = (float)Level.Current.Ground;
		while (base.transform.position.x > -740f)
		{
			if (pattern[i][0] == 'D')
			{
				Parser.FloatTryParse(pattern[i].Substring(1), out waitTime);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, waitTime);
				bool goingUp = true;
				bool highJump = pattern[i][0] == 'H';
				float velocityY = (!highJump) ? this.properties.lowVerticalSpeed : this.properties.highVerticalSpeed;
				float speedX = (!highJump) ? this.properties.lowHorizontalSpeed : this.properties.highHorizontalSpeed;
				this.gravity = ((!highJump) ? this.properties.lowGravity : this.properties.highGravity);
				while (goingUp || base.transform.position.y > this.onGroundY)
				{
					velocityY -= this.gravity * CupheadTime.FixedDelta;
					base.transform.AddPosition(-speedX * CupheadTime.FixedDelta, velocityY * CupheadTime.FixedDelta, 0f);
					if (velocityY < 0f && goingUp)
					{
						goingUp = false;
					}
					yield return null;
				}
				base.transform.SetPosition(null, new float?(this.onGroundY), null);
			}
			i = (i + 1) % pattern.Length;
		}
		this.Die();
		yield break;
	}

	// Token: 0x06000F38 RID: 3896 RVA: 0x0000CE3E File Offset: 0x0000B03E
	public void Die()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000C69 RID: 3177
	public LevelProperties.AirshipStork.Babies properties;

	// Token: 0x04000C6A RID: 3178
	public DamageDealer damageDealer;

	// Token: 0x04000C6B RID: 3179
	public DamageReceiver damageReceiver;

	// Token: 0x04000C6C RID: 3180
	public float onGroundY;

	// Token: 0x04000C6D RID: 3181
	public float gravity;

	// Token: 0x04000C6E RID: 3182
	public float health;

	// Token: 0x020009F1 RID: 2545
	public enum State
	{
		// Token: 0x040049BB RID: 18875
		Move,
		// Token: 0x040049BC RID: 18876
		Dying
	}
}
