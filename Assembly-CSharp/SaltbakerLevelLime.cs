using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000378 RID: 888
public class SaltbakerLevelLime : SaltbakerLevelPhaseOneProjectile
{
	// Token: 0x06002730 RID: 10032 RVA: 0x000CB0EC File Offset: 0x000C92EC
	public virtual SaltbakerLevelLime Init(Vector3 position, bool onLeft, bool isHigh, LevelProperties.Saltbaker.Limes properties, int id, int anim)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = position;
		this.properties = properties;
		this.onLeft = onLeft;
		this.isHigh = isHigh;
		this.Move();
		base.animator.Play(anim.ToString());
		this.sfxID = id;
		this.SFX_SALTBAKER_P1_LimeProjectileLoop();
		return this;
	}

	// Token: 0x06002731 RID: 10033 RVA: 0x00020F0C File Offset: 0x0001F10C
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002732 RID: 10034 RVA: 0x00020F2A File Offset: 0x0001F12A
	public new void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002733 RID: 10035 RVA: 0x00020F39 File Offset: 0x0001F139
	public override void OnDestroy()
	{
		AudioManager.Stop("sfx_dlc_saltbaker_p1_lime_projectile_loop_" + (this.sfxID + 1));
		base.OnDestroy();
	}

	// Token: 0x06002734 RID: 10036 RVA: 0x000CB154 File Offset: 0x000C9354
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float startPosX = (float)((!this.onLeft) ? Level.Current.Right : Level.Current.Left);
		float curveStartY = (!this.isHigh) ? this.properties.lowStartY : this.properties.highStartY;
		float curveEndY = (!this.isHigh) ? this.properties.lowEndY : this.properties.highEndY;
		float boomerangSpeed = this.properties.straightSpeed;
		float distToTurn = this.properties.distToTurn;
		float loopSizeX = 100f;
		float gravity = this.properties.straightGravity;
		float speed = boomerangSpeed;
		float pivotY = Mathf.Lerp(curveStartY, curveEndY, 0.5f);
		float pivotX = (!this.onLeft) ? (-distToTurn) : distToTurn;
		float loopSizeY = Mathf.Abs(pivotY - curveStartY);
		this.pivot = new Vector3(pivotX, pivotY);
		float offset = (!this.isHigh) ? (-loopSizeY) : loopSizeY;
		base.transform.SetPosition(new float?(startPosX), new float?(this.pivot.y + offset), null);
		if (this.onLeft)
		{
			while (base.transform.position.x < distToTurn)
			{
				base.transform.position += Vector3.right * speed * CupheadTime.FixedDelta;
				base.HandleShadow(0f, 40f);
				yield return wait;
			}
		}
		else
		{
			while (base.transform.position.x > -distToTurn)
			{
				base.transform.position += Vector3.left * speed * CupheadTime.FixedDelta;
				base.HandleShadow(0f, 40f);
				yield return wait;
			}
		}
		float angleToStopAt = 3.14159274f;
		float angle = 0f;
		float angleStartSpeed = this.properties.angleSpeedToLerp.min;
		float angleEndSpeed = this.properties.angleSpeedToLerp.max;
		float timeTolerp = this.properties.angleLerpTime;
		float t = 0f;
		angle *= 0.0174532924f;
		while (angle < angleToStopAt)
		{
			t += CupheadTime.FixedDelta;
			float s = Mathf.Lerp(angleStartSpeed, angleEndSpeed, t / timeTolerp);
			angle += s * CupheadTime.FixedDelta;
			Vector3 handleRotationX;
			if (this.onLeft)
			{
				handleRotationX = new Vector3(Mathf.Sin(angle) * loopSizeX, 0f, 0f);
			}
			else
			{
				handleRotationX = new Vector3(-Mathf.Sin(angle) * loopSizeX, 0f, 0f);
			}
			Vector3 handleRotationY;
			if (this.isHigh)
			{
				handleRotationY = new Vector3(0f, Mathf.Cos(angle) * loopSizeY, 0f);
			}
			else
			{
				handleRotationY = new Vector3(0f, -Mathf.Cos(angle) * loopSizeY, 0f);
			}
			base.transform.position = this.pivot;
			base.transform.position += handleRotationX + handleRotationY;
			base.HandleShadow(0f, 40f);
			yield return new WaitForFixedUpdate();
		}
		speed = boomerangSpeed;
		if (this.onLeft)
		{
			while (base.transform.position.x > (float)Level.Current.Left - 300f)
			{
				speed += gravity * CupheadTime.FixedDelta;
				base.transform.position += Vector3.left * speed * CupheadTime.FixedDelta;
				base.HandleShadow(0f, 40f);
				yield return wait;
			}
		}
		else
		{
			while (base.transform.position.x < (float)Level.Current.Right + 300f)
			{
				speed += gravity * CupheadTime.FixedDelta;
				base.transform.position += Vector3.right * speed * CupheadTime.FixedDelta;
				base.HandleShadow(0f, 40f);
				yield return wait;
			}
		}
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		AudioManager.Stop("sfx_dlc_saltbaker_p1_lime_projectile_loop_" + (this.sfxID + 1));
		yield break;
	}

	// Token: 0x06002735 RID: 10037 RVA: 0x00020F5D File Offset: 0x0001F15D
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawWireSphere(this.pivot, 10f);
	}

	// Token: 0x06002736 RID: 10038 RVA: 0x000CB170 File Offset: 0x000C9370
	public void SFX_SALTBAKER_P1_LimeProjectileLoop()
	{
		string key = "sfx_dlc_saltbaker_p1_lime_projectile_loop_" + (this.sfxID + 1);
		AudioManager.PlayLoop(key);
		this.emitAudioFromObject.Add(key);
	}

	// Token: 0x04002070 RID: 8304
	public LevelProperties.Saltbaker.Limes properties;

	// Token: 0x04002071 RID: 8305
	public bool isDead;

	// Token: 0x04002072 RID: 8306
	public bool onLeft;

	// Token: 0x04002073 RID: 8307
	public bool isHigh;

	// Token: 0x04002074 RID: 8308
	public int sfxID;

	// Token: 0x04002075 RID: 8309
	public Vector3 pivot;
}
