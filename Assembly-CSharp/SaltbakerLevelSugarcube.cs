using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000381 RID: 897
public class SaltbakerLevelSugarcube : SaltbakerLevelPhaseOneProjectile
{
	// Token: 0x060027AC RID: 10156 RVA: 0x0002149E File Offset: 0x0001F69E
	public override void OnDieDistance()
	{
	}

	// Token: 0x060027AD RID: 10157 RVA: 0x000214A0 File Offset: 0x0001F6A0
	public override void OnDieLifetime()
	{
	}

	// Token: 0x060027AE RID: 10158 RVA: 0x000CC778 File Offset: 0x000CA978
	public virtual SaltbakerLevelSugarcube Init(Vector2 pos, bool onLeft, LevelProperties.Saltbaker.Sugarcubes properties, float phase, SaltbakerLevelSaltbaker parent, int anim, bool parryable)
	{
		base.ResetLifetime();
		base.ResetDistance();
		this.root = pos;
		base.transform.position = pos;
		this.properties = properties;
		this.onLeft = onLeft;
		this.Move();
		this.phase = phase * 0.0174532924f;
		base.animator.Play((!parryable) ? anim.ToString() : "Pink");
		this.SetParryable(parryable);
		return this;
	}

	// Token: 0x060027AF RID: 10159 RVA: 0x000214A2 File Offset: 0x0001F6A2
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060027B0 RID: 10160 RVA: 0x000214C0 File Offset: 0x0001F6C0
	public new void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060027B1 RID: 10161 RVA: 0x000CC804 File Offset: 0x000CAA04
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float xVelocity = this.properties.sineFreq * this.properties.sineWavelength;
		xVelocity = ((!this.onLeft) ? (-xVelocity) : xVelocity);
		base.transform.localScale = new Vector3(Mathf.Sign(xVelocity), 1f);
		float t = 0f;
		bool ismoving = true;
		while (ismoving)
		{
			t += CupheadTime.FixedDelta;
			Vector3 pos = base.transform.position;
			float yAbsolute = this.properties.sineAmplitude * Mathf.Sin(this.properties.sineFreq * t + this.phase);
			pos.y = this.root.y + yAbsolute;
			pos.x += xVelocity * CupheadTime.FixedDelta;
			base.transform.position = pos;
			base.HandleShadow(50f, 0f);
			if ((this.onLeft && base.transform.position.x > (float)Level.Current.Right + 50f) || (!this.onLeft && base.transform.position.x < (float)Level.Current.Left - 50f))
			{
				ismoving = false;
			}
			yield return wait;
		}
		this.Death();
		yield return null;
		yield break;
	}

	// Token: 0x060027B2 RID: 10162 RVA: 0x000214CF File Offset: 0x0001F6CF
	public void Death()
	{
		this.Recycle<SaltbakerLevelSugarcube>();
	}

	// Token: 0x040020E2 RID: 8418
	public LevelProperties.Saltbaker.Sugarcubes properties;

	// Token: 0x040020E3 RID: 8419
	public Vector3 root;

	// Token: 0x040020E4 RID: 8420
	public bool onLeft;

	// Token: 0x040020E5 RID: 8421
	public float phase;
}
