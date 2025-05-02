using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002B2 RID: 690
public class GraveyardLevelSplitDevilProjectile : BasicProjectileContinuesOnLevelEnd
{
	// Token: 0x06001EE5 RID: 7909 RVA: 0x000B49B0 File Offset: 0x000B2BB0
	public GraveyardLevelSplitDevilProjectile Create(Vector2 position, float rotation, float speed, GraveyardLevelSplitDevil devil)
	{
		GraveyardLevelSplitDevilProjectile graveyardLevelSplitDevilProjectile = base.Create(position, rotation, speed) as GraveyardLevelSplitDevilProjectile;
		graveyardLevelSplitDevilProjectile.devil = devil;
		graveyardLevelSplitDevilProjectile.animator.SetInteger("FireVariant", Random.Range(0, 3));
		graveyardLevelSplitDevilProjectile.animator.SetInteger("LightVariant", Random.Range(0, 2));
		graveyardLevelSplitDevilProjectile.SetBool("IsFire", !devil.isAngel);
		graveyardLevelSplitDevilProjectile.coll.enabled = !devil.isAngel;
		graveyardLevelSplitDevilProjectile.UpdateFade(2f);
		graveyardLevelSplitDevilProjectile.StartCoroutine(graveyardLevelSplitDevilProjectile.spawn_fx_cr());
		return graveyardLevelSplitDevilProjectile;
	}

	// Token: 0x06001EE6 RID: 7910 RVA: 0x000B4A48 File Offset: 0x000B2C48
	public IEnumerator spawn_fx_cr()
	{
		this.fxAngle = (float)Random.Range(0, 360);
		while (!this.dead && !this.impacted)
		{
			yield return CupheadTime.WaitForSeconds(this, this.fxSpawnDelay);
			int count = 1;
			if (this.fxSpawnDelay < CupheadTime.Delta)
			{
				count = (int)(CupheadTime.Delta / this.fxSpawnDelay);
			}
			for (int i = 0; i < count; i++)
			{
				Effect effect = (!this.devil.isAngel) ? this.fireFX.Create(base.transform.position + MathUtils.AngleToDirection(this.fxAngle) * this.fxDistanceRange.RandomFloat()) : this.lightFX.Create(base.transform.position + MathUtils.AngleToDirection(this.fxAngle) * this.fxDistanceRange.RandomFloat());
				if (!this.devil.isAngel)
				{
					effect.transform.eulerAngles = new Vector3(0f, 0f, base.transform.eulerAngles.z + 50f);
				}
				this.fxAngle = (this.fxAngle + this.fxAngleShiftRange.RandomFloat()) % 360f;
			}
		}
		yield break;
	}

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06001EE7 RID: 7911 RVA: 0x0001A0FA File Offset: 0x000182FA
	public override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001EE8 RID: 7912 RVA: 0x000B4A64 File Offset: 0x000B2C64
	public override void Update()
	{
		if (!this.impacted)
		{
			base.Update();
		}
		if (!this.dead)
		{
			this.coll.enabled = !this.devil.isAngel;
		}
		if (!this.impacted)
		{
			if (base.animator.GetBool("IsFire") == this.devil.isAngel)
			{
				base.animator.Play("LightTransition" + Random.Range(0, 3).ToString(), 2, 0f);
			}
			base.animator.SetBool("IsFire", !this.devil.isAngel);
			this.frameTimer += CupheadTime.Delta;
			while (this.frameTimer > 0.0416666679f)
			{
				this.frameTimer -= 0.0416666679f;
				this.UpdateFade(0.25f);
			}
		}
		if (!this.impacted && Mathf.Abs(base.transform.position.x) < 550f && base.transform.position.y < -297f)
		{
			this.impacted = true;
			this.Speed = 0f;
			base.transform.eulerAngles = Vector3.zero;
			this.fireRend[0].transform.eulerAngles = Vector3.zero;
			this.lightRend[0].transform.eulerAngles = Vector3.zero;
			base.animator.Play((!Rand.Bool()) ? "ImpactB" : "ImpactA", this.devil.isAngel ? 1 : 0);
			this.UpdateFade(2f);
			this.Die();
		}
	}

	// Token: 0x06001EE9 RID: 7913 RVA: 0x000B4C50 File Offset: 0x000B2E50
	public void UpdateFade(float amount)
	{
		foreach (SpriteRenderer spriteRenderer in this.fireRend)
		{
			spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Clamp(spriteRenderer.color.a + ((!this.coll.enabled) ? (-amount) : amount), 0f, 1f));
		}
		foreach (SpriteRenderer spriteRenderer2 in this.lightRend)
		{
			spriteRenderer2.color = new Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, Mathf.Clamp(spriteRenderer2.color.a + ((!this.coll.enabled) ? amount : ((!(spriteRenderer2.gameObject.name == "Ring")) ? (-amount) : (-amount * 0.7f))), 0f, 1f));
		}
	}

	// Token: 0x06001EEA RID: 7914 RVA: 0x0001A0FD File Offset: 0x000182FD
	public override void Die()
	{
		this.dead = true;
		this.coll.enabled = false;
	}

	// Token: 0x04001940 RID: 6464
	public GraveyardLevelSplitDevil devil;

	// Token: 0x04001941 RID: 6465
	[SerializeField]
	public SpriteRenderer[] fireRend;

	// Token: 0x04001942 RID: 6466
	[SerializeField]
	public SpriteRenderer[] lightRend;

	// Token: 0x04001943 RID: 6467
	[SerializeField]
	public Effect fireFX;

	// Token: 0x04001944 RID: 6468
	[SerializeField]
	public Effect lightFX;

	// Token: 0x04001945 RID: 6469
	[SerializeField]
	public Collider2D coll;

	// Token: 0x04001946 RID: 6470
	public float frameTimer;

	// Token: 0x04001947 RID: 6471
	public new bool dead;

	// Token: 0x04001948 RID: 6472
	public bool impacted;

	// Token: 0x04001949 RID: 6473
	[SerializeField]
	public float fxSpawnDelay = 0.15f;

	// Token: 0x0400194A RID: 6474
	[SerializeField]
	public MinMax fxAngleShiftRange = new MinMax(60f, 300f);

	// Token: 0x0400194B RID: 6475
	[SerializeField]
	public MinMax fxDistanceRange = new MinMax(0f, 20f);

	// Token: 0x0400194C RID: 6476
	public float fxAngle;
}
