using System;
using UnityEngine;

// Token: 0x020003C7 RID: 967
public class VeggiesLevelBeetBaby : AbstractCollidableObject
{
	// Token: 0x06002A93 RID: 10899 RVA: 0x000D44C8 File Offset: 0x000D26C8
	public VeggiesLevelBeetBaby Create(VeggiesLevelBeetBaby.Type type, float speed, float childSpeed, float range, Vector2 pos, float rot)
	{
		VeggiesLevelBeetBaby veggiesLevelBeetBaby = this.InstantiatePrefab<VeggiesLevelBeetBaby>();
		veggiesLevelBeetBaby.Init(type, speed, childSpeed, range, pos, rot);
		return veggiesLevelBeetBaby;
	}

	// Token: 0x06002A94 RID: 10900 RVA: 0x000D44EC File Offset: 0x000D26EC
	public void Init(VeggiesLevelBeetBaby.Type type, float speed, float childSpeed, float range, Vector2 pos, float rot)
	{
		this.type = type;
		this.speed = speed;
		this.childSpeed = childSpeed;
		this.range = range;
		base.transform.position = pos;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(rot));
		base.animator.Play(type.ToString());
		this.damageDealer = new DamageDealer(1f, 0.2f, true, false, false);
		this.damageDealer.SetDirection(DamageDealer.Direction.Neutral, base.transform);
	}

	// Token: 0x06002A95 RID: 10901 RVA: 0x000D4590 File Offset: 0x000D2790
	public void Update()
	{
		if (this.state == VeggiesLevelBeetBaby.State.Dead)
		{
			return;
		}
		base.transform.position += base.transform.right * this.speed * CupheadTime.Delta;
		if (base.transform.position.y > 360f)
		{
			this.Die();
		}
	}

	// Token: 0x06002A96 RID: 10902 RVA: 0x00023D18 File Offset: 0x00021F18
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06002A97 RID: 10903 RVA: 0x000D4608 File Offset: 0x000D2808
	public void Die()
	{
		this.state = VeggiesLevelBeetBaby.State.Dead;
		base.animator.SetTrigger("Explode");
		int num = (this.type != VeggiesLevelBeetBaby.Type.Fat) ? 3 : 5;
		for (int i = 0; i < num; i++)
		{
			float num2 = (float)i / (float)(num - 1);
			float rot = Mathf.Lerp(0f, this.range, num2) - 90f - this.range / 2f;
			VeggiesLevelBeetBabyBullet veggiesLevelBeetBabyBullet = this.bulletPrefab.Create(this.childSpeed, base.transform.position, rot);
			if (this.type == VeggiesLevelBeetBaby.Type.Pink)
			{
				veggiesLevelBeetBabyBullet.SetParryable(true);
			}
		}
	}

	// Token: 0x06002A98 RID: 10904 RVA: 0x00023D2F File Offset: 0x00021F2F
	public void OnDeathAnimComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0400237F RID: 9087
	public const int BULLET_COUNT = 3;

	// Token: 0x04002380 RID: 9088
	public const int BULLET_COUNT_FAT = 5;

	// Token: 0x04002381 RID: 9089
	[SerializeField]
	public VeggiesLevelBeetBabyBullet bulletPrefab;

	// Token: 0x04002382 RID: 9090
	public VeggiesLevelBeetBaby.Type type;

	// Token: 0x04002383 RID: 9091
	public float speed;

	// Token: 0x04002384 RID: 9092
	public float childSpeed;

	// Token: 0x04002385 RID: 9093
	public float range;

	// Token: 0x04002386 RID: 9094
	public VeggiesLevelBeetBaby.State state;

	// Token: 0x04002387 RID: 9095
	public DamageDealer damageDealer;

	// Token: 0x02000FD9 RID: 4057
	public enum Type
	{
		// Token: 0x040071E1 RID: 29153
		Regular,
		// Token: 0x040071E2 RID: 29154
		Fat,
		// Token: 0x040071E3 RID: 29155
		Pink
	}

	// Token: 0x02000FDA RID: 4058
	public enum State
	{
		// Token: 0x040071E5 RID: 29157
		Go,
		// Token: 0x040071E6 RID: 29158
		Dead
	}
}
