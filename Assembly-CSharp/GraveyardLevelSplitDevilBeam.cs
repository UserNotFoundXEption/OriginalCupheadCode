using System;
using UnityEngine;

// Token: 0x020002AF RID: 687
public class GraveyardLevelSplitDevilBeam : AbstractProjectile
{
	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06001ED2 RID: 7890 RVA: 0x0001A06F File Offset: 0x0001826F
	// (set) Token: 0x06001ED3 RID: 7891 RVA: 0x0001A077 File Offset: 0x00018277
	public GraveyardLevelSplitDevil devil { get; set; }

	// Token: 0x06001ED4 RID: 7892 RVA: 0x0001A080 File Offset: 0x00018280
	public override void RandomizeVariant()
	{
	}

	// Token: 0x06001ED5 RID: 7893 RVA: 0x000B3E58 File Offset: 0x000B2058
	public GraveyardLevelSplitDevilBeam Create(Vector3 pos, float xVelocity, float warningTime, GraveyardLevelSplitDevil devil)
	{
		GraveyardLevelSplitDevilBeam graveyardLevelSplitDevilBeam = base.Create(pos) as GraveyardLevelSplitDevilBeam;
		graveyardLevelSplitDevilBeam.xVelocity = xVelocity;
		graveyardLevelSplitDevilBeam.DestroyDistance = (float)(Level.Current.Width + 200);
		graveyardLevelSplitDevilBeam.devil = devil;
		graveyardLevelSplitDevilBeam.warningTime = warningTime;
		graveyardLevelSplitDevilBeam.fireOn = !graveyardLevelSplitDevilBeam.devil.isAngel;
		graveyardLevelSplitDevilBeam.coll.enabled = !graveyardLevelSplitDevilBeam.devil.isAngel;
		graveyardLevelSplitDevilBeam.UpdateFade(1f);
		if (graveyardLevelSplitDevilBeam.fireOn)
		{
			graveyardLevelSplitDevilBeam.fireAnim.Play("Form", 1, 0f);
			graveyardLevelSplitDevilBeam.fireAnim.Update(0f);
			Effect effect = this.igniteFX.Create(graveyardLevelSplitDevilBeam.transform.position, this.fireAnim);
			effect.transform.parent = graveyardLevelSplitDevilBeam.transform;
			AudioManager.Play("sfx_dlc_graveyard_beamchange_fireon");
			graveyardLevelSplitDevilBeam.emitAudioFromObject.Add("sfx_dlc_graveyard_beamchange_fireon");
		}
		else
		{
			foreach (SpriteRenderer spriteRenderer in graveyardLevelSplitDevilBeam.lightRend)
			{
				spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
			}
		}
		CupheadLevelCamera.Current.StartShake(4f);
		return graveyardLevelSplitDevilBeam;
	}

	// Token: 0x06001ED6 RID: 7894 RVA: 0x000B3FB0 File Offset: 0x000B21B0
	public override void Update()
	{
		base.Update();
		if (base.dead)
		{
			return;
		}
		if (this.devil.dead)
		{
			this.coll.enabled = false;
			this.forceFade = true;
		}
		if (this.warningTime <= 0f)
		{
			base.transform.AddPosition(this.xVelocity * CupheadTime.Delta, 0f, 0f);
			if (this.fireAnim.GetBool("Smoke"))
			{
				this.flameTrailDistanceTracker += Mathf.Abs(this.xVelocity) * CupheadTime.Delta;
			}
		}
		else
		{
			this.warningTime -= CupheadTime.Delta;
		}
		while (this.flameTrailDistanceTracker > this.flameTrailSpacing && !this.forceFade)
		{
			this.flameTrailDistanceTracker -= this.flameTrailSpacing;
			this.SpawnTrailFX();
		}
		if (Mathf.Abs(base.transform.position.x) < (float)((Mathf.Sign(base.transform.position.x) != Mathf.Sign(this.xVelocity)) ? 600 : 400) && !this.fireAnim.GetBool("Smoke"))
		{
			this.SpawnTrailFX();
		}
		if (Mathf.Abs(base.transform.position.x) < 550f && !this.onGround)
		{
			this.onGround = true;
			this.lightAnim.Play((!Rand.Bool()) ? "B" : "A", 1, 0f);
		}
		this.fireAnim.SetBool("Smoke", Mathf.Abs(base.transform.position.x) < (float)((Mathf.Sign(base.transform.position.x) != Mathf.Sign(this.xVelocity)) ? 600 : 400));
		this.coll.enabled = !this.devil.isAngel;
		if (this.fireOn != !this.devil.isAngel)
		{
			if (this.fireOn && !this.fireAnim.GetCurrentAnimatorStateInfo(1).IsName("Form") && this.fireAnim.GetBool("Smoke"))
			{
				Effect effect = this.bottomSmokeFX.Create(base.transform.position);
				if (this.bottomSmokeFXTypeA)
				{
					effect.Play();
				}
				this.bottomSmokeFXTypeA = !this.bottomSmokeFXTypeA;
				effect.transform.parent = base.transform;
				Effect effect2 = this.midSmokeFX.Create(this.midSmokePos.position);
				effect2.transform.localScale = this.midSmokePos.localScale;
				effect2.transform.parent = base.transform;
			}
			if (!this.fireOn)
			{
				AudioManager.Play("sfx_dlc_graveyard_beamchange_fireon");
				this.emitAudioFromObject.Add("sfx_dlc_graveyard_beamchange_fireon");
				Effect effect3 = this.igniteFX.Create(base.transform.position, this.fireAnim);
				effect3.transform.parent = base.transform;
			}
			this.fireAnim.Play((!this.fireOn) ? "Form" : "Dissipate", 1, 0f);
			this.fireAnim.Update(0f);
			this.fireOn = !this.devil.isAngel;
		}
		this.frameTimer += CupheadTime.Delta;
		while (this.frameTimer > 0.0416666679f)
		{
			this.frameTimer -= 0.0416666679f;
			this.UpdateFade(0.25f);
		}
	}

	// Token: 0x06001ED7 RID: 7895 RVA: 0x000B43D4 File Offset: 0x000B25D4
	public void UpdateFade(float amount)
	{
		foreach (SpriteRenderer spriteRenderer in this.fireRend)
		{
			spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Clamp(spriteRenderer.color.a + ((!(this.coll.enabled & !this.forceFade)) ? (-amount) : amount), 0f, 1f));
		}
		foreach (SpriteRenderer spriteRenderer2 in this.lightRend)
		{
			spriteRenderer2.color = new Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, Mathf.Clamp(spriteRenderer2.color.a + ((!this.coll.enabled && !this.forceFade) ? amount : (-amount)), 0f, 1f));
		}
		this.groundSpotlight.color = new Color(1f, 1f, 1f, Mathf.Clamp(this.groundSpotlight.color.a + ((this.coll.enabled || !(this.onGround & !this.forceFade)) ? (-amount) : amount), 0f, 1f));
	}

	// Token: 0x06001ED8 RID: 7896 RVA: 0x000B4590 File Offset: 0x000B2790
	public void SpawnTrailFX()
	{
		this.trailFX.Create(new Vector3(Mathf.Clamp(base.transform.position.x + this.flameTrailSpacing * Mathf.Sign(this.xVelocity), -550f, 550f), base.transform.position.y), new Vector3(-Mathf.Sign(this.xVelocity), 1f), this, this.flameTrailAnim);
		this.flameTrailAnim = (this.flameTrailAnim + 1) % 3;
	}

	// Token: 0x06001ED9 RID: 7897 RVA: 0x000B4624 File Offset: 0x000B2824
	public void LateUpdate()
	{
		this.fireRend[0].enabled = (this.fireFormDissipate.sprite == null);
		this.sparkleBeam.transform.localPosition = new Vector3(0f, (float)(-1280 + (int)this.lightAnim.GetCurrentAnimatorStateInfo(0).normalizedTime % 8 * 280));
	}

	// Token: 0x06001EDA RID: 7898 RVA: 0x0001A082 File Offset: 0x00018282
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001EDB RID: 7899 RVA: 0x0001A0A0 File Offset: 0x000182A0
	public override void Die()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001921 RID: 6433
	public float xVelocity;

	// Token: 0x04001923 RID: 6435
	[SerializeField]
	public Animator fireAnim;

	// Token: 0x04001924 RID: 6436
	[SerializeField]
	public Animator lightAnim;

	// Token: 0x04001925 RID: 6437
	[SerializeField]
	public SpriteRenderer[] fireRend;

	// Token: 0x04001926 RID: 6438
	[SerializeField]
	public SpriteRenderer[] lightRend;

	// Token: 0x04001927 RID: 6439
	[SerializeField]
	public SpriteRenderer fireFormDissipate;

	// Token: 0x04001928 RID: 6440
	[SerializeField]
	public Effect bottomSmokeFX;

	// Token: 0x04001929 RID: 6441
	public bool bottomSmokeFXTypeA = true;

	// Token: 0x0400192A RID: 6442
	[SerializeField]
	public Effect midSmokeFX;

	// Token: 0x0400192B RID: 6443
	[SerializeField]
	public Transform midSmokePos;

	// Token: 0x0400192C RID: 6444
	[SerializeField]
	public GraveyardLevelSplitDevilBeamIgniteFX igniteFX;

	// Token: 0x0400192D RID: 6445
	[SerializeField]
	public GraveyardLevelSplitDevilBeamTrailFX trailFX;

	// Token: 0x0400192E RID: 6446
	[SerializeField]
	public float flameTrailSpacing = 128f;

	// Token: 0x0400192F RID: 6447
	[SerializeField]
	public GameObject sparkleBeam;

	// Token: 0x04001930 RID: 6448
	[SerializeField]
	public SpriteRenderer groundSpotlight;

	// Token: 0x04001931 RID: 6449
	public bool onGround;

	// Token: 0x04001932 RID: 6450
	public bool fireOn;

	// Token: 0x04001933 RID: 6451
	[SerializeField]
	public Collider2D coll;

	// Token: 0x04001934 RID: 6452
	public float warningTime;

	// Token: 0x04001935 RID: 6453
	public float frameTimer;

	// Token: 0x04001936 RID: 6454
	public float flameTrailDistanceTracker;

	// Token: 0x04001937 RID: 6455
	public int flameTrailAnim;

	// Token: 0x04001938 RID: 6456
	public bool forceFade;
}
