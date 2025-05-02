using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003CF RID: 975
public class VeggiesLevelOnionTearProjectile : AbstractProjectile
{
	// Token: 0x06002B05 RID: 11013 RVA: 0x000D54C8 File Offset: 0x000D36C8
	public AbstractProjectile Create(float time, float x)
	{
		VeggiesLevelOnionTearProjectile veggiesLevelOnionTearProjectile = base.Create(new Vector2(x, 420f)) as VeggiesLevelOnionTearProjectile;
		veggiesLevelOnionTearProjectile.direction = this.direction;
		veggiesLevelOnionTearProjectile.time = time;
		veggiesLevelOnionTearProjectile.Init();
		return veggiesLevelOnionTearProjectile;
	}

	// Token: 0x06002B06 RID: 11014 RVA: 0x00024211 File Offset: 0x00022411
	public void Init()
	{
		base.StartCoroutine(this.projectile_cr());
	}

	// Token: 0x06002B07 RID: 11015 RVA: 0x00024220 File Offset: 0x00022420
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002B08 RID: 11016 RVA: 0x000D5508 File Offset: 0x000D3708
	public override void Die()
	{
		base.Die();
		base.transform.SetScale(new float?((float)((!MathUtils.RandomBool()) ? -1 : 1)), null, null);
	}

	// Token: 0x06002B09 RID: 11017 RVA: 0x000D5550 File Offset: 0x000D3750
	public IEnumerator projectile_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float startY = base.transform.position.y;
		float endY = (float)Level.Current.Ground;
		float calculatedTime = this.time;
		float t = 0f;
		while (t < calculatedTime)
		{
			float val = t / calculatedTime;
			float y = EaseUtils.Ease(EaseUtils.EaseType.easeInQuad, startY, endY, val);
			base.transform.SetPosition(null, new float?(y), null);
			t += CupheadTime.FixedDelta;
			yield return wait;
		}
		base.transform.SetPosition(null, new float?(endY), null);
		AudioManager.Play("level_veggies_onion_teardrop");
		this.Die();
		yield break;
	}

	// Token: 0x040023C9 RID: 9161
	public const float startY = 420f;

	// Token: 0x040023CA RID: 9162
	public float time;

	// Token: 0x040023CB RID: 9163
	public int direction;
}
