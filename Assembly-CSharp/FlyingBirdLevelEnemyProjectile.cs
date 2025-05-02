using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200022F RID: 559
public class FlyingBirdLevelEnemyProjectile : AbstractProjectile
{
	// Token: 0x060019C2 RID: 6594 RVA: 0x000A6FB0 File Offset: 0x000A51B0
	public virtual AbstractProjectile Create(float time, float height, Vector2 pos)
	{
		FlyingBirdLevelEnemyProjectile flyingBirdLevelEnemyProjectile = this.Create(pos, 0f) as FlyingBirdLevelEnemyProjectile;
		flyingBirdLevelEnemyProjectile.time = time;
		flyingBirdLevelEnemyProjectile.height = height;
		flyingBirdLevelEnemyProjectile.DamagesType.OnlyPlayer();
		flyingBirdLevelEnemyProjectile.CollisionDeath.OnlyPlayer();
		flyingBirdLevelEnemyProjectile.Init();
		return flyingBirdLevelEnemyProjectile;
	}

	// Token: 0x060019C3 RID: 6595 RVA: 0x00015FC5 File Offset: 0x000141C5
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060019C4 RID: 6596 RVA: 0x00015FE3 File Offset: 0x000141E3
	public void Init()
	{
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x060019C5 RID: 6597 RVA: 0x000A6FFC File Offset: 0x000A51FC
	public void Check()
	{
		if (base.transform.position.y < -460f)
		{
			this.StopAllCoroutines();
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060019C6 RID: 6598 RVA: 0x000A7038 File Offset: 0x000A5238
	public IEnumerator go_cr()
	{
		float start = base.transform.position.y;
		float end = start + this.height;
		float t = 0f;
		float speed = 0f;
		t = 0f;
		while (t < this.time)
		{
			float val = t / this.time;
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, start, end, val)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		while (t < this.time)
		{
			float val2 = t / this.time;
			float last = base.transform.position.y;
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInSine, end, start, val2)), null);
			speed = base.transform.position.y - last;
			t += CupheadTime.Delta;
			yield return null;
		}
		for (;;)
		{
			this.Check();
			base.transform.AddPosition(0f, speed * CupheadTime.GlobalSpeed, 0f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x040014AA RID: 5290
	public float time;

	// Token: 0x040014AB RID: 5291
	public float height;
}
