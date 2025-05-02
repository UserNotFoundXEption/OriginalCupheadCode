using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class ChessRookLevelPinkCannonBall : AbstractProjectile
{
	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06001339 RID: 4921 RVA: 0x00010316 File Offset: 0x0000E516
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x0600133A RID: 4922 RVA: 0x0001031D File Offset: 0x0000E51D
	// (set) Token: 0x0600133B RID: 4923 RVA: 0x00010325 File Offset: 0x0000E525
	public bool finishedOriginalArc { get; set; }

	// Token: 0x0600133C RID: 4924 RVA: 0x0001032E File Offset: 0x0000E52E
	public override void OnLevelEnd()
	{
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x00010330 File Offset: 0x0000E530
	public override void Start()
	{
		base.Start();
		this.coll = base.GetComponent<Collider2D>();
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x000972FC File Offset: 0x000954FC
	public ChessRookLevelPinkCannonBall Create(Vector3 position, float apexHeight, float targetDistance, LevelProperties.ChessRook.PinkCannonBall properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = position;
		this.properties = properties;
		this.apexHeight = apexHeight;
		this.targetDistance = targetDistance;
		this.gravity = properties.pinkGravity;
		this.Move();
		return this;
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x00010344 File Offset: 0x0000E544
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x00010362 File Offset: 0x0000E562
	public void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x0009734C File Offset: 0x0009554C
	public IEnumerator move_cr()
	{
		AudioManager.Play("sfx_dlc_kog_rook_ghosthead_launch");
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_ghosthead_launch");
		float endPosY = (float)Level.Current.Ground;
		this.newRoot = new Vector3(base.transform.position.x - this.targetDistance, endPosY);
		float x = this.newRoot.x - base.transform.position.x;
		float apexDist = this.apexHeight;
		float toSqrRootForviY = Mathf.Abs(2f * this.gravity * apexDist);
		float viY = Mathf.Sqrt(toSqrRootForviY);
		float timeToApex = Mathf.Abs(viY / this.gravity);
		float toSqrtForTimeToG = Mathf.Abs(2f * (base.transform.position.y + apexDist) / this.gravity);
		float timeToGround = Mathf.Sqrt(toSqrtForTimeToG);
		float viX = x / (timeToApex + timeToGround);
		bool stillMoving = true;
		if (this.finishedOriginalArc && !this.playerOnBottom)
		{
			viY = this.properties.velocityAfterSlam;
			this.gravity = this.properties.gravityAfterSlam;
			yield return null;
		}
		Vector3 speed = new Vector3(viX, viY);
		while (stillMoving)
		{
			speed += new Vector3(0f, this.gravity * CupheadTime.FixedDelta);
			base.transform.Translate(speed * CupheadTime.FixedDelta);
			yield return new WaitForFixedUpdate();
			if (this.gotParried)
			{
				stillMoving = false;
				break;
			}
			if (base.transform.position.y < (float)(Level.Current.Ground + 120))
			{
				this.Sink(speed.x);
			}
			if (base.transform.position.y < (float)(Level.Current.Ground + 40))
			{
				this.coll.enabled = false;
			}
			if (base.transform.position.y < (float)(Level.Current.Ground - 40))
			{
				this.Die();
			}
		}
		if (this.gotParried)
		{
			this.Bounce();
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001342 RID: 4930 RVA: 0x00097368 File Offset: 0x00095568
	public void Sink(float speedX)
	{
		if (this.sinking)
		{
			return;
		}
		this.sinking = true;
		this.parryColl.enabled = false;
		this.sinkFX.Create(new Vector3(base.transform.position.x + speedX / 9f, (float)(Level.Current.Ground - 40)));
		AudioManager.Play("sfx_dlc_kog_rook_ghosthead_hitground_explode");
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_ghosthead_hitground_explode");
	}

	// Token: 0x06001343 RID: 4931 RVA: 0x000973E8 File Offset: 0x000955E8
	public void Explosion()
	{
		this.StopAllCoroutines();
		this.parryColl.enabled = false;
		this.coll.enabled = false;
		this.rotatingExplosion.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		this.topExplosion.flipX = false;
		base.animator.Play("Explode");
		AudioManager.Play("sfx_dlc_kog_rook_ghosthead_hitsrook");
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_ghosthead_hitsrook");
	}

	// Token: 0x06001344 RID: 4932 RVA: 0x00010371 File Offset: 0x0000E571
	public override void Die()
	{
		this.Recycle<ChessRookLevelPinkCannonBall>();
	}

	// Token: 0x06001345 RID: 4933 RVA: 0x00097474 File Offset: 0x00095674
	public void Bounce()
	{
		this.gravity = this.properties.pinkReactionGravity;
		this.apexHeight = this.properties.bounceUpApexHeight + this.heightAddition;
		this.targetDistance = ((!this.playerOnLeft) ? (this.properties.bounceUpTargetDist + this.distAddition) : (-this.properties.bounceUpTargetDist - this.distAddition));
		if (!this.finishedOriginalArc)
		{
			this.finishedOriginalArc = true;
		}
		this.gotParried = false;
		this.Move();
	}

	// Token: 0x06001346 RID: 4934 RVA: 0x00097504 File Offset: 0x00095704
	public void GotParried(AbstractPlayerController player)
	{
		this.playerOnLeft = (player.transform.position.x < base.transform.position.x);
		this.playerOnBottom = true;
		Vector3 vector = player.center - base.transform.position;
		float num = MathUtils.DirectionToAngle(vector);
		if (num < 0f)
		{
			num = 360f + num;
		}
		if (num >= 180f - this.properties.goodQuadrantClemencyLeft && num <= 270f + this.properties.goodQuadrantClemencyBottom)
		{
			base.animator.SetTrigger("Parried");
			base.GetComponent<SpriteRenderer>().sortingOrder = 1;
			float num2 = Mathf.InverseLerp(270f + this.properties.goodQuadrantClemencyBottom, 180f, num);
			this.distAddition = this.properties.distanceAddition.GetFloatAt(num2);
			this.heightAddition = this.properties.heightAddition.GetFloatAt(1f - num2);
		}
		else if (num > 270f + this.properties.goodQuadrantClemencyBottom)
		{
			float num3 = Mathf.InverseLerp(270f + this.properties.goodQuadrantClemencyBottom, 360f, num);
			this.distAddition = this.properties.distanceAdditionBack.GetFloatAt(num3);
			this.heightAddition = this.properties.heightAdditionBack.GetFloatAt(1f - num3);
		}
		else
		{
			if (this.playerOnLeft)
			{
				base.animator.SetTrigger("Parried");
			}
			float i = Mathf.InverseLerp(180f, 0f, num);
			this.distAddition = ((!this.playerOnLeft) ? this.properties.distanceAdditionBack.GetFloatAt(i) : this.properties.distanceAddition.GetFloatAt(i));
			this.heightAddition = 0f;
			base.GetComponent<SpriteRenderer>().sortingOrder = 1;
			this.playerOnBottom = false;
		}
		this.gotParried = true;
		base.StartCoroutine(this.collider_off_cr());
	}

	// Token: 0x06001347 RID: 4935 RVA: 0x00097728 File Offset: 0x00095928
	public IEnumerator collider_off_cr()
	{
		this.parryColl.enabled = false;
		this.coll.enabled = false;
		yield return CupheadTime.WaitForSeconds(this, this.properties.bounceCollisionOffTimer);
		this.parryColl.enabled = true;
		this.coll.enabled = true;
		yield break;
	}

	// Token: 0x06001348 RID: 4936 RVA: 0x00097744 File Offset: 0x00095944
	public void LateUpdate()
	{
		this.shadow.transform.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground);
		int num = (int)(Mathf.Abs(base.transform.position.y - (float)Level.Current.Ground) / this.maxShadowDistance * (float)this.shadowSprites.Length);
		this.shadow.enabled = (this.coll.enabled && num >= 0 && num < this.shadowSprites.Length);
		if (this.shadow.enabled)
		{
			this.shadow.sprite = this.shadowSprites[num];
		}
		if (Level.Current.Ending)
		{
			this.coll.enabled = false;
			this.parryColl.enabled = false;
		}
	}

	// Token: 0x04000F8C RID: 3980
	public LevelProperties.ChessRook.PinkCannonBall properties;

	// Token: 0x04000F8D RID: 3981
	public Collider2D coll;

	// Token: 0x04000F8E RID: 3982
	public Vector3 newRoot;

	// Token: 0x04000F8F RID: 3983
	public float apexHeight;

	// Token: 0x04000F90 RID: 3984
	public float targetDistance;

	// Token: 0x04000F91 RID: 3985
	public float gravity;

	// Token: 0x04000F92 RID: 3986
	public float distAddition;

	// Token: 0x04000F93 RID: 3987
	public float heightAddition;

	// Token: 0x04000F94 RID: 3988
	public bool gotParried;

	// Token: 0x04000F95 RID: 3989
	public bool playerOnLeft;

	// Token: 0x04000F96 RID: 3990
	public bool playerOnBottom;

	// Token: 0x04000F97 RID: 3991
	[SerializeField]
	public Collider2D parryColl;

	// Token: 0x04000F98 RID: 3992
	[SerializeField]
	public SpriteRenderer shadow;

	// Token: 0x04000F99 RID: 3993
	[SerializeField]
	public Sprite[] shadowSprites;

	// Token: 0x04000F9A RID: 3994
	[SerializeField]
	public SpriteRenderer topExplosion;

	// Token: 0x04000F9B RID: 3995
	[SerializeField]
	public SpriteRenderer rotatingExplosion;

	// Token: 0x04000F9C RID: 3996
	[SerializeField]
	public SpriteRenderer bigExplosion;

	// Token: 0x04000F9D RID: 3997
	[SerializeField]
	public Effect sinkFX;

	// Token: 0x04000F9E RID: 3998
	public bool sinking;

	// Token: 0x04000F9F RID: 3999
	[SerializeField]
	public float maxShadowDistance = 750f;
}
