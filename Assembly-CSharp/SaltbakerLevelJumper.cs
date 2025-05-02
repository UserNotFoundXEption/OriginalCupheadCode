using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000376 RID: 886
public class SaltbakerLevelJumper : AbstractProjectile
{
	// Token: 0x0600271E RID: 10014 RVA: 0x000CAE5C File Offset: 0x000C905C
	public SaltbakerLevelJumper Create(Vector3 position, SaltbakerLevel parent, LevelProperties.Saltbaker.Swooper swooperProperties, LevelProperties.Saltbaker.Jumper jumperProperties, float firstDelay, bool isSwooper)
	{
		SaltbakerLevelJumper saltbakerLevelJumper = this.InstantiatePrefab<SaltbakerLevelJumper>();
		saltbakerLevelJumper.transform.position = position;
		if (isSwooper)
		{
			saltbakerLevelJumper.transform.position += Vector3.up * -94f;
		}
		saltbakerLevelJumper.count = ((!isSwooper) ? jumperProperties.numberFireJumpers : swooperProperties.numberFireSwoopers);
		saltbakerLevelJumper.apexHeight = ((!isSwooper) ? (jumperProperties.apexHeight - 68f) : (swooperProperties.apexHeight + -94f));
		saltbakerLevelJumper.apexTime = ((!isSwooper) ? jumperProperties.apexTime : swooperProperties.apexTime);
		saltbakerLevelJumper.initialFallDelay = ((!isSwooper) ? jumperProperties.initialFallDelay : swooperProperties.initialFallDelay);
		saltbakerLevelJumper.jumpDelay = ((!isSwooper) ? jumperProperties.jumpDelay : swooperProperties.jumpDelay);
		saltbakerLevelJumper.levelEdgeOffset = ((!isSwooper) ? 260f : 75f);
		saltbakerLevelJumper.parent = parent;
		saltbakerLevelJumper.firstDelay = firstDelay;
		saltbakerLevelJumper.isSwooper = isSwooper;
		saltbakerLevelJumper.coll = saltbakerLevelJumper.GetComponent<CircleCollider2D>();
		saltbakerLevelJumper.FXbottom.transform.parent = null;
		saltbakerLevelJumper.aimPosition = saltbakerLevelJumper.transform.position;
		if (saltbakerLevelJumper.isSwooper)
		{
			saltbakerLevelJumper.StartCoroutine(saltbakerLevelJumper.arc_cr());
		}
		else
		{
			saltbakerLevelJumper.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
			saltbakerLevelJumper.StartCoroutine(saltbakerLevelJumper.fall_cr());
		}
		return saltbakerLevelJumper;
	}

	// Token: 0x0600271F RID: 10015 RVA: 0x00020E0F File Offset: 0x0001F00F
	public override void OnDieLifetime()
	{
	}

	// Token: 0x06002720 RID: 10016 RVA: 0x00020E11 File Offset: 0x0001F011
	public Vector3 GetAimPos()
	{
		return this.aimPosition;
	}

	// Token: 0x06002721 RID: 10017 RVA: 0x00020E19 File Offset: 0x0001F019
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002722 RID: 10018 RVA: 0x000CAFEC File Offset: 0x000C91EC
	public IEnumerator arc_cr()
	{
		AnimationHelper animHelper = base.GetComponent<AnimationHelper>();
		if (this.isSwooper)
		{
			base.animator.Play("SwooperIntro");
			yield return base.animator.WaitForAnimationToEnd(this, "SwooperIntro", false, true);
		}
		else
		{
			this.FXbottom.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
			this.FXbottom.GetComponent<SpriteRenderer>().sortingOrder = -2;
		}
		float root = (float)(Level.Current.Left + Level.Current.Right) / 2f;
		yield return CupheadTime.WaitForSeconds(this, this.firstDelay);
		while (!this.dead)
		{
			if (this.isSwooper)
			{
				base.animator.Play("SwooperAntic");
				while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.888f)
				{
					yield return null;
				}
			}
			float t = 0f;
			float endPosY = (!this.isSwooper) ? ((float)Level.Current.Ground + 68f) : (CupheadLevelCamera.Current.Bounds.yMax + -94f);
			this.aimPosition = new Vector3(Mathf.Clamp(PlayerManager.GetNext().center.x, (float)Level.Current.Left + this.levelEdgeOffset, (float)Level.Current.Right - this.levelEdgeOffset), endPosY);
			float offset = Mathf.Sign(root - this.aimPosition.x) * this.coll.bounds.size.x;
			bool foundPos = false;
			while (!foundPos)
			{
				if (this.aimPosition.x < (float)Level.Current.Left + this.levelEdgeOffset || this.aimPosition.x > (float)Level.Current.Right - this.levelEdgeOffset)
				{
					this.aimPosition = base.transform.position;
					foundPos = true;
				}
				else if (this.parent.IsPositionAvailable(this.aimPosition, this))
				{
					foundPos = true;
				}
				else
				{
					this.aimPosition.x = this.aimPosition.x + offset;
				}
			}
			float x = this.aimPosition.x - base.transform.position.x;
			float y = this.aimPosition.y - base.transform.position.y;
			float apexTime2 = this.apexTime * this.apexTime;
			float g = -2f * this.apexHeight / apexTime2;
			float viY = 2f * this.apexHeight / this.apexTime;
			float viX2 = viY * viY;
			float sqrtRooted = viX2 + 2f * g * y;
			float tEnd = (-viY + Mathf.Sqrt(sqrtRooted)) / g;
			float tEnd2 = (-viY - Mathf.Sqrt(sqrtRooted)) / g;
			float tEnd3 = Mathf.Max(tEnd, tEnd2);
			float velocityX = x / tEnd3;
			if (this.isSwooper)
			{
				viY = -viY;
			}
			Vector3 vel = new Vector3(velocityX, viY);
			base.animator.SetInteger("ArcWidth", Mathf.Clamp((int)(Mathf.Abs(velocityX) / 250f), 0, 2));
			int jumpLoopHash = Animator.StringToHash(base.animator.GetLayerName(0) + "." + ((!this.isSwooper) ? "Jumper" : "Swooper") + "JumpLoop");
			float animTimeOnLand = -1f;
			base.animator.SetInteger("Variant", Random.Range(0, 2));
			if (this.isSwooper)
			{
				base.transform.localScale = new Vector3(Mathf.Sign(velocityX), 1f);
				yield return base.animator.WaitForAnimationToEnd(this, "SwooperAntic", false, true);
				tEnd3 -= 0.375f;
			}
			else
			{
				base.animator.SetTrigger("StartJumperAntic");
				yield return base.animator.WaitForAnimationToStart(this, "JumperAntic", false);
				base.transform.localScale = new Vector3(Mathf.Sign(-velocityX), 1f);
				yield return base.animator.WaitForAnimationToEnd(this, "JumperAntic", false, true);
			}
			this.FXbottom.transform.position = base.transform.position + Vector3.up * ((!this.isSwooper) ? -20f : 27f);
			this.FXbottom.transform.localScale = new Vector3(base.transform.localScale.x, (float)((!this.isSwooper) ? 1 : -1));
			this.FXbottom.Play(this.FXanimNames[base.animator.GetInteger("ArcWidth")], 0, 0f);
			bool stillMoving = true;
			YieldInstruction wait = new WaitForFixedUpdate();
			while (stillMoving)
			{
				if (animTimeOnLand < 0f && base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == jumpLoopHash)
				{
					float num = tEnd3 / 0.416666657f;
					animTimeOnLand = (num + base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1f;
					float num2 = animTimeOnLand - ((!this.isSwooper) ? 0.75f : 0.65f);
					float num3 = num - num2;
					animHelper.Speed = num3 / num;
				}
				if (this.isSwooper)
				{
					vel.y -= g * CupheadTime.FixedDelta;
				}
				else
				{
					vel.y += g * CupheadTime.FixedDelta;
				}
				base.transform.Translate(vel * CupheadTime.FixedDelta);
				tEnd3 -= CupheadTime.FixedDelta;
				yield return wait;
				t += CupheadTime.FixedDelta;
				if (t > this.apexTime)
				{
					if (this.isSwooper)
					{
						if (base.transform.position.y >= endPosY)
						{
							stillMoving = false;
						}
						if (tEnd3 <= 0f)
						{
							base.animator.SetBool("EndJump", true);
							animHelper.Speed = 1f;
						}
					}
					else if (base.transform.position.y <= endPosY)
					{
						stillMoving = false;
						base.animator.SetBool("EndJump", true);
						animHelper.Speed = 1f;
					}
				}
			}
			base.transform.SetPosition(null, new float?(endPosY), null);
			if (!this.isSwooper)
			{
				yield return base.animator.WaitForAnimationToEnd(this, "JumperJumpLoop", false, true);
			}
			base.animator.SetBool("EndJump", false);
			if (!this.isSwooper)
			{
				yield return base.animator.WaitForAnimationToStart(this, "JumperIdle", false);
			}
			if (!this.dead)
			{
				yield return CupheadTime.WaitForSeconds(this, this.jumpDelay);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002723 RID: 10019 RVA: 0x000CB008 File Offset: 0x000C9208
	public void AniEvent_FlipX()
	{
		base.transform.localScale = new Vector3(-base.transform.localScale.x, 1f);
	}

	// Token: 0x06002724 RID: 10020 RVA: 0x000CB040 File Offset: 0x000C9240
	public IEnumerator fall_cr()
	{
		base.animator.Play("JumperFallLoop");
		float endPosY = (float)Level.Current.Ground + 68f;
		float apexTime2 = this.apexTime * this.apexTime;
		float g = -2f * this.apexHeight / apexTime2;
		Vector3 vel = Vector3.zero;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.y > 200f)
		{
			vel.y += g * CupheadTime.FixedDelta;
			base.transform.Translate(vel * CupheadTime.FixedDelta);
			yield return wait;
		}
		base.animator.SetTrigger("StartJumperIntro");
		while (base.transform.position.y > endPosY)
		{
			yield return wait;
			vel.y += g * CupheadTime.FixedDelta;
			base.transform.Translate(vel * CupheadTime.FixedDelta);
		}
		base.transform.SetPosition(null, new float?(endPosY), null);
		yield return base.animator.WaitForAnimationToStart(this, "JumperIdle", false);
		base.StartCoroutine(this.arc_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002725 RID: 10021 RVA: 0x00020E37 File Offset: 0x0001F037
	public new void Die()
	{
		this.dead = true;
		base.animator.SetTrigger("Die");
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06002726 RID: 10022 RVA: 0x00020E5C File Offset: 0x0001F05C
	public void AniEvent_DeathComplete()
	{
		Object.Destroy(this.FXbottom.gameObject);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002727 RID: 10023 RVA: 0x00020E79 File Offset: 0x0001F079
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (this.coll != null)
		{
			Gizmos.DrawWireSphere(this.aimPosition, this.coll.radius);
		}
	}

	// Token: 0x04002050 RID: 8272
	public const float SWOOPER_DIVE_LENGTH = 0.375f;

	// Token: 0x04002051 RID: 8273
	public const float JUMPER_INTRO_LENGTH = 0.416666657f;

	// Token: 0x04002052 RID: 8274
	public const float JUMPER_GROUND_OFFSET = 68f;

	// Token: 0x04002053 RID: 8275
	public const float SWOOPER_CEILING_OFFSET = -94f;

	// Token: 0x04002054 RID: 8276
	public const float SWOOPER_FX_OFFSET = 27f;

	// Token: 0x04002055 RID: 8277
	public const float JUMPER_ENTRANCE_Y_POS = 200f;

	// Token: 0x04002056 RID: 8278
	public const float JUMP_LOOP_LENGTH = 0.416666657f;

	// Token: 0x04002057 RID: 8279
	public const float SWOOPER_LOOP_EXIT_TIME = 0.65f;

	// Token: 0x04002058 RID: 8280
	public const float JUMPER_LOOP_EXIT_TIME = 0.75f;

	// Token: 0x04002059 RID: 8281
	public const float LEVEL_EDGE_OFFSET_SWOOPER = 75f;

	// Token: 0x0400205A RID: 8282
	public const float LEVEL_EDGE_OFFSET_JUMPER = 260f;

	// Token: 0x0400205B RID: 8283
	[SerializeField]
	public Animator FXbottom;

	// Token: 0x0400205C RID: 8284
	public string[] FXanimNames = new string[]
	{
		"Thin",
		"Medium",
		"Wide"
	};

	// Token: 0x0400205D RID: 8285
	public Vector3 aimPosition;

	// Token: 0x0400205E RID: 8286
	public float levelEdgeOffset = 75f;

	// Token: 0x0400205F RID: 8287
	public SaltbakerLevel parent;

	// Token: 0x04002060 RID: 8288
	public float firstDelay;

	// Token: 0x04002061 RID: 8289
	public bool isSwooper;

	// Token: 0x04002062 RID: 8290
	public CircleCollider2D coll;

	// Token: 0x04002063 RID: 8291
	public int count;

	// Token: 0x04002064 RID: 8292
	public float apexHeight;

	// Token: 0x04002065 RID: 8293
	public float apexTime;

	// Token: 0x04002066 RID: 8294
	public float initialFallDelay;

	// Token: 0x04002067 RID: 8295
	public float jumpDelay;

	// Token: 0x04002068 RID: 8296
	public new bool dead;
}
