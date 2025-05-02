using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000195 RID: 405
public class ChessRookLevelRegularCannonball : AbstractProjectile
{
	// Token: 0x0600134F RID: 4943 RVA: 0x000103AD File Offset: 0x0000E5AD
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06001350 RID: 4944 RVA: 0x000103B5 File Offset: 0x0000E5B5
	public ChessRookLevelRegularCannonball Create(Vector3 position, float apexHeight, float targetDistance, LevelProperties.ChessRook.RegularCannonBall properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = position;
		this.apexHeight = apexHeight;
		this.targetDistance = targetDistance;
		this.gravity = properties.cannonGravity;
		this.Move();
		return this;
	}

	// Token: 0x06001351 RID: 4945 RVA: 0x000103F1 File Offset: 0x0000E5F1
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001352 RID: 4946 RVA: 0x0001040F File Offset: 0x0000E60F
	public void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001353 RID: 4947 RVA: 0x0001041E File Offset: 0x0000E61E
	public override void Die()
	{
		this.Recycle<ChessRookLevelRegularCannonball>();
	}

	// Token: 0x06001354 RID: 4948 RVA: 0x00097838 File Offset: 0x00095A38
	public IEnumerator move_cr()
	{
		AudioManager.Play("sfx_dlc_kog_rook_ghosthead_launch");
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_ghosthead_launch");
		float endPosY = (float)Level.Current.Ground;
		float x = new Vector3(base.transform.position.x - this.targetDistance, endPosY).x - base.transform.position.x;
		float apexDist = this.apexHeight;
		float toSqrRootForviY = Mathf.Abs(2f * this.gravity * apexDist);
		float viY = Mathf.Sqrt(toSqrRootForviY);
		float timeToApex = Mathf.Abs(viY / this.gravity);
		float toSqrtForTimeToG = Mathf.Abs(2f * (base.transform.position.y + apexDist) / this.gravity);
		float timeToGround = Mathf.Sqrt(toSqrtForTimeToG);
		float viX = x / (timeToApex + timeToGround);
		Vector3 speed = new Vector3(viX, viY);
		bool stillMoving = true;
		while (stillMoving)
		{
			speed += new Vector3(0f, this.gravity * CupheadTime.FixedDelta);
			base.transform.Translate(speed * CupheadTime.FixedDelta);
			yield return new WaitForFixedUpdate();
			if (base.transform.position.y < (float)(Level.Current.Ground + 40))
			{
				stillMoving = false;
				break;
			}
		}
		base.animator.Play("Explode", 1, 0f);
		AudioManager.Play("sfx_dlc_kog_rook_ghosthead_hitground_explode");
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_ghosthead_hitground_explode");
		this.rend.flipX = Rand.Bool();
		this.coll.enabled = false;
		yield break;
	}

	// Token: 0x06001355 RID: 4949 RVA: 0x00097854 File Offset: 0x00095A54
	public void LateUpdate()
	{
		this.shadow.transform.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground);
		int num = (int)(Mathf.Abs(base.transform.position.y - (float)Level.Current.Ground) / this.maxShadowDistance * (float)this.shadowSprites.Length);
		this.shadow.enabled = (this.coll.enabled && num >= 0 && num < this.shadowSprites.Length);
		if (this.shadow.enabled)
		{
			this.shadow.sprite = this.shadowSprites[num];
		}
	}

	// Token: 0x04000FA1 RID: 4001
	public float apexTime;

	// Token: 0x04000FA2 RID: 4002
	public float apexHeight;

	// Token: 0x04000FA3 RID: 4003
	public float targetDistance;

	// Token: 0x04000FA4 RID: 4004
	public float gravity;

	// Token: 0x04000FA5 RID: 4005
	[SerializeField]
	public Collider2D coll;

	// Token: 0x04000FA6 RID: 4006
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x04000FA7 RID: 4007
	[SerializeField]
	public SpriteRenderer shadow;

	// Token: 0x04000FA8 RID: 4008
	[SerializeField]
	public Sprite[] shadowSprites;

	// Token: 0x04000FA9 RID: 4009
	[SerializeField]
	public float maxShadowDistance = 750f;
}
