using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000252 RID: 594
public class FlyingCowboyLevelBirdProjectile : BasicProjectile
{
	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06001B12 RID: 6930 RVA: 0x00016F74 File Offset: 0x00015174
	public override Vector3 Direction
	{
		get
		{
			return this._direction;
		}
	}

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06001B13 RID: 6931 RVA: 0x00016F7C File Offset: 0x0001517C
	// (set) Token: 0x06001B14 RID: 6932 RVA: 0x00016F84 File Offset: 0x00015184
	public int shrapnelCount { get; set; }

	// Token: 0x06001B15 RID: 6933 RVA: 0x00016F8D File Offset: 0x0001518D
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.landingPosition_cr());
		base.StartCoroutine(this.shrapnel_cr());
		base.StartCoroutine(this.shadow_cr());
	}

	// Token: 0x06001B16 RID: 6934 RVA: 0x000AA624 File Offset: 0x000A8824
	public void Initialize(Vector2 initialVelocity, float gravity, float shrapnelDelay, float shrapnelSpeed, float shrapnelSpreadAngle, FlyingCowboyLevelCowboy cowgirl)
	{
		this.Speed = initialVelocity.magnitude;
		this._direction = initialVelocity.normalized;
		this.gravity = gravity;
		this.shrapnelDelay = shrapnelDelay;
		this.shrapnelSpeed = shrapnelSpeed;
		this.shrapnelSpreadAngle = shrapnelSpreadAngle;
		this.cowgirl = cowgirl;
		this.landingPosition = FlyingCowboyLevelBirdProjectile.HighLandingPosition;
	}

	// Token: 0x06001B17 RID: 6935 RVA: 0x000AA684 File Offset: 0x000A8884
	public override void FixedUpdate()
	{
		Vector3 vector = this.Direction * this.Speed;
		vector.y -= this.gravity * CupheadTime.FixedDelta;
		this._direction = vector.normalized;
		this.Speed = vector.magnitude;
		base.FixedUpdate();
	}

	// Token: 0x06001B18 RID: 6936 RVA: 0x000AA6E0 File Offset: 0x000A88E0
	public IEnumerator landingPosition_cr()
	{
		while (base.transform.position.y > 0f)
		{
			yield return null;
		}
		if (this.cowgirl.onBottom && this.cowgirl.state == FlyingCowboyLevelCowboy.State.BeamAttack)
		{
			this.landingPosition = FlyingCowboyLevelBirdProjectile.LowLandingPosition;
		}
		yield break;
	}

	// Token: 0x06001B19 RID: 6937 RVA: 0x000AA6FC File Offset: 0x000A88FC
	public IEnumerator shadow_cr()
	{
		while (base.transform.position.y > this.landingPosition + FlyingCowboyLevelBirdProjectile.ShadowTriggerDistance)
		{
			yield return null;
		}
		base.animator.Play("Land", FlyingCowboyLevelBirdProjectile.ShadowLayer);
		base.animator.Update(0f);
		while (!base.animator.GetCurrentAnimatorStateInfo(FlyingCowboyLevelBirdProjectile.ShadowLayer).IsName("Off"))
		{
			Vector3 position = this.shadowTransform.position;
			position.y = this.landingPosition;
			this.shadowTransform.position = position;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001B1A RID: 6938 RVA: 0x000AA718 File Offset: 0x000A8918
	public IEnumerator shrapnel_cr()
	{
		while (base.transform.position.y > this.landingPosition)
		{
			yield return null;
		}
		Transform transform = base.transform;
		float? y = new float?(this.landingPosition);
		transform.SetPosition(null, y, null);
		this.move = false;
		float initialAngle = (180f - this.shrapnelSpreadAngle) * 0.5f;
		float angleInterval = this.shrapnelSpreadAngle / (float)(this.shrapnelCount - 1);
		for (int i = 0; i < this.shrapnelCount; i += 2)
		{
			this.shrapnelPrefab.Create(this.spawnPoint.position, initialAngle + angleInterval * (float)i, this.shrapnelSpeed);
		}
		this.SFX_COWGIRL_COWGIRL_P1_DynamiteExp();
		base.animator.Play("Bounce");
		base.animator.Play("A", FlyingCowboyLevelBirdProjectile.ExplosionLayer);
		base.animator.Play("A", FlyingCowboyLevelBirdProjectile.SmokeLayer);
		base.StartCoroutine(this.moveSmoke_cr("A"));
		base.animator.Play("Off", FlyingCowboyLevelBirdProjectile.ShadowLayer);
		yield return CupheadTime.WaitForSeconds(this, this.shrapnelDelay);
		for (int j = 1; j < this.shrapnelCount; j += 2)
		{
			this.shrapnelPrefab.Create(this.spawnPoint.position, initialAngle + angleInterval * (float)j, this.shrapnelSpeed);
		}
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.Play("Off");
		base.animator.Play("B", FlyingCowboyLevelBirdProjectile.ExplosionLayer);
		base.animator.Play("B", FlyingCowboyLevelBirdProjectile.SmokeLayer);
		base.StartCoroutine(this.moveSmoke_cr("B"));
		yield break;
	}

	// Token: 0x06001B1B RID: 6939 RVA: 0x000AA734 File Offset: 0x000A8934
	public IEnumerator moveSmoke_cr(string animationName)
	{
		Vector3 initialPosition = this.smokeTransform.position;
		yield return base.animator.WaitForAnimationToStart(this, animationName, FlyingCowboyLevelBirdProjectile.SmokeLayer, false);
		float speed = 0f;
		while (!base.animator.GetCurrentAnimatorStateInfo(FlyingCowboyLevelBirdProjectile.SmokeLayer).IsName("Off"))
		{
			yield return null;
			speed += CupheadTime.Delta * 1500f;
			Vector3 position = this.smokeTransform.position;
			position.x -= speed * CupheadTime.Delta;
			this.smokeTransform.position = position;
		}
		this.smokeTransform.position = initialPosition;
		yield break;
	}

	// Token: 0x06001B1C RID: 6940 RVA: 0x00016FBC File Offset: 0x000151BC
	public void animationEvent_ExplosionsFinished()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001B1D RID: 6941 RVA: 0x00016FC9 File Offset: 0x000151C9
	public void SFX_COWGIRL_COWGIRL_P1_DynamiteExp()
	{
		AudioManager.Play("sfx_DLC_Cowgirl_P1_DynamiteExp");
		this.emitAudioFromObject.Add("sfx_DLC_Cowgirl_P1_DynamiteExp");
	}

	// Token: 0x040015D8 RID: 5592
	public static readonly int ExplosionLayer = 1;

	// Token: 0x040015D9 RID: 5593
	public static readonly int SmokeLayer = 2;

	// Token: 0x040015DA RID: 5594
	public static readonly int ShadowLayer = 3;

	// Token: 0x040015DB RID: 5595
	public static readonly float ShadowTriggerDistance = 260f;

	// Token: 0x040015DC RID: 5596
	public static readonly float HighLandingPosition = -300f;

	// Token: 0x040015DD RID: 5597
	public static readonly float LowLandingPosition = -340f;

	// Token: 0x040015DE RID: 5598
	public Vector3 _direction;

	// Token: 0x040015E0 RID: 5600
	[SerializeField]
	public Transform shadowTransform;

	// Token: 0x040015E1 RID: 5601
	[SerializeField]
	public Transform spawnPoint;

	// Token: 0x040015E2 RID: 5602
	[SerializeField]
	public Transform smokeTransform;

	// Token: 0x040015E3 RID: 5603
	[SerializeField]
	public BasicProjectile shrapnelPrefab;

	// Token: 0x040015E4 RID: 5604
	public float landingPosition;

	// Token: 0x040015E5 RID: 5605
	public float gravity;

	// Token: 0x040015E6 RID: 5606
	public float shrapnelDelay;

	// Token: 0x040015E7 RID: 5607
	public float shrapnelSpeed;

	// Token: 0x040015E8 RID: 5608
	public float shrapnelSpreadAngle;

	// Token: 0x040015E9 RID: 5609
	public FlyingCowboyLevelCowboy cowgirl;
}
