using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200038F RID: 911
public class SnowCultLevelEyeProjectile : AbstractProjectile
{
	// Token: 0x1700032A RID: 810
	// (get) Token: 0x0600282F RID: 10287 RVA: 0x00021BB3 File Offset: 0x0001FDB3
	// (set) Token: 0x06002830 RID: 10288 RVA: 0x00021BBB File Offset: 0x0001FDBB
	public bool IsGone { get; set; }

	// Token: 0x06002831 RID: 10289 RVA: 0x000CDDF0 File Offset: 0x000CBFF0
	public virtual SnowCultLevelEyeProjectile Init(Vector3 startPos, Vector3 endPos, bool onRight, bool upsideDown, LevelProperties.SnowCult.EyeAttack properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		this.startPos = startPos;
		base.transform.position = startPos;
		this.properties = properties;
		this.endPos = endPos;
		this.onRight = onRight;
		base.transform.localScale = new Vector3((float)((!onRight) ? -1 : 1), 1f);
		this.upsideDown = upsideDown;
		this.readyToOpenMouth = false;
		this.angle = 0f;
		this.IsGone = false;
		base.StartCoroutine(this.move_cr());
		this.beamCR = base.StartCoroutine(this.beam_cr());
		return this;
	}

	// Token: 0x06002832 RID: 10290 RVA: 0x00021BC4 File Offset: 0x0001FDC4
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002833 RID: 10291 RVA: 0x000CDE98 File Offset: 0x000CC098
	public IEnumerator beam_cr()
	{
		this.beamAnimator.Play("AuraStart");
		float delay = this.properties.initialBeamDelay.RandomFloat();
		while (!this.IsGone)
		{
			yield return CupheadTime.WaitForSeconds(this, delay);
			delay = this.properties.beamDelay;
			this.beamAnimator.SetBool("Attack", true);
			this.SFX_SNOWCULT_JackFrostEyeballZap();
			yield return CupheadTime.WaitForSeconds(this, this.properties.beamDuration);
			this.beamAnimator.SetBool("Attack", false);
		}
		yield break;
	}

	// Token: 0x06002834 RID: 10292 RVA: 0x000CDEB4 File Offset: 0x000CC0B4
	public IEnumerator move_in_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		if (!this.onRight)
		{
			while (base.transform.position.x < this.properties.distanceToTurn)
			{
				this.lastPos = base.transform.position;
				base.transform.position += Vector3.right * this.properties.eyeStraightSpeed * CupheadTime.FixedDelta;
				yield return wait;
			}
		}
		else
		{
			while (base.transform.position.x > -this.properties.distanceToTurn)
			{
				this.lastPos = base.transform.position;
				base.transform.position += Vector3.left * this.properties.eyeStraightSpeed * CupheadTime.FixedDelta;
				yield return wait;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002835 RID: 10293 RVA: 0x000CDED0 File Offset: 0x000CC0D0
	public IEnumerator move_out_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		if (!this.onRight)
		{
			while (base.transform.position.x < this.endPos.x - this.openMouthDistance)
			{
				this.lastPos = base.transform.position;
				base.transform.position += Vector3.right * this.properties.eyeStraightSpeed * CupheadTime.FixedDelta;
				yield return wait;
			}
		}
		else
		{
			while (base.transform.position.x > this.endPos.x + this.openMouthDistance)
			{
				this.lastPos = base.transform.position;
				base.transform.position += Vector3.left * this.properties.eyeStraightSpeed * CupheadTime.FixedDelta;
				yield return wait;
			}
		}
		this.readyToOpenMouth = true;
		if (!this.onRight)
		{
			while (base.transform.position.x < this.endPos.x - this.beamEndDistance)
			{
				this.lastPos = base.transform.position;
				base.transform.position += Vector3.right * this.properties.eyeStraightSpeed * CupheadTime.FixedDelta;
				yield return wait;
			}
		}
		else
		{
			while (base.transform.position.x > this.endPos.x + this.beamEndDistance)
			{
				this.lastPos = base.transform.position;
				base.transform.position += Vector3.left * this.properties.eyeStraightSpeed * CupheadTime.FixedDelta;
				yield return wait;
			}
		}
		base.StopCoroutine(this.beamCR);
		this.beamAnimator.SetBool("Attack", false);
		this.beamAnimator.SetTrigger("End");
		if (!this.onRight)
		{
			while (base.transform.position.x < this.endPos.x - this.animatorTakeoverDistance)
			{
				this.lastPos = base.transform.position;
				base.transform.position += Vector3.right * this.properties.eyeStraightSpeed * CupheadTime.FixedDelta;
				yield return wait;
			}
		}
		else
		{
			while (base.transform.position.x > this.endPos.x + this.animatorTakeoverDistance)
			{
				this.lastPos = base.transform.position;
				base.transform.position += Vector3.left * this.properties.eyeStraightSpeed * CupheadTime.FixedDelta;
				yield return wait;
			}
		}
		this.readyToCloseMouth = true;
		this.controlledByParent = true;
		yield return null;
		this.shadow.SetActive(true);
		yield break;
	}

	// Token: 0x06002836 RID: 10294 RVA: 0x000CDEEC File Offset: 0x000CC0EC
	public IEnumerator move_cr()
	{
		this.SFX_SNOWCULT_JackFrostEyeballLoop();
		float loopSpeed = this.properties.eyeCurveSpeed;
		float pivotY = (this.startPos.y + this.endPos.y) / 2f;
		float loopSizeY = Mathf.Abs(this.startPos.y - this.endPos.y) / 2f;
		float loopSizeX = loopSizeY;
		float pivotX = (!this.onRight) ? this.properties.distanceToTurn : (-this.properties.distanceToTurn);
		Vector3 pivot = new Vector3(pivotX, pivotY);
		float angleToStopAt = 3.14159274f;
		if (!this.upsideDown)
		{
			base.transform.SetPosition(null, new float?(pivot.y - loopSizeY), null);
		}
		else
		{
			base.transform.SetPosition(null, new float?(pivot.y + loopSizeY), null);
		}
		this.angle *= 0.0174532924f;
		yield return base.StartCoroutine(this.move_in_cr());
		while (this.angle < angleToStopAt)
		{
			this.angle += loopSpeed * CupheadTime.FixedDelta;
			Vector3 handleRotationX;
			if (!this.onRight)
			{
				handleRotationX = new Vector3(Mathf.Sin(this.angle) * loopSizeX, 0f, 0f);
			}
			else
			{
				handleRotationX = new Vector3(-Mathf.Sin(this.angle) * loopSizeX, 0f, 0f);
			}
			Vector3 handleRotationY;
			if (!this.upsideDown)
			{
				handleRotationY = new Vector3(0f, -Mathf.Cos(this.angle) * loopSizeY, 0f);
			}
			else
			{
				handleRotationY = new Vector3(0f, Mathf.Cos(this.angle) * loopSizeY, 0f);
			}
			this.lastPos = base.transform.position;
			base.transform.position = pivot;
			base.transform.position += handleRotationX + handleRotationY;
			yield return new WaitForFixedUpdate();
		}
		this.onRight = !this.onRight;
		base.StartCoroutine(this.move_out_cr());
		yield break;
	}

	// Token: 0x06002837 RID: 10295 RVA: 0x00021BE2 File Offset: 0x0001FDE2
	public void ReturnToSnowflake()
	{
		this.SFX_SNOWCULT_JackFrostEyeballLoopStop();
		this.Recycle<SnowCultLevelEyeProjectile>();
		this.IsGone = true;
	}

	// Token: 0x06002838 RID: 10296 RVA: 0x00021BF7 File Offset: 0x0001FDF7
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.dead)
		{
			base.transform.position += this.vel * CupheadTime.FixedDelta;
		}
	}

	// Token: 0x06002839 RID: 10297 RVA: 0x000CDF08 File Offset: 0x000CC108
	public void LateUpdate()
	{
		if (this.main.dead != this.dead)
		{
			this.dead = true;
			this.vel /= CupheadTime.FixedDelta;
			this.SFX_SNOWCULT_JackFrostEyeballLoopStop();
			this.StopAllCoroutines();
		}
		else if (!this.dead)
		{
			this.vel = base.transform.position - this.lastPos;
		}
		if (this.controlledByParent)
		{
			base.transform.position = this.main.eyeProjectileGuide.position;
		}
	}

	// Token: 0x0600283A RID: 10298 RVA: 0x00021C30 File Offset: 0x0001FE30
	public void SFX_SNOWCULT_JackFrostEyeballLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_snowcult_p3_snowflake_eyeball_attack_loop");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_eyeball_attack_loop");
	}

	// Token: 0x0600283B RID: 10299 RVA: 0x00021C4C File Offset: 0x0001FE4C
	public void SFX_SNOWCULT_JackFrostEyeballLoopStop()
	{
		AudioManager.Stop("sfx_dlc_snowcult_p3_snowflake_eyeball_attack_loop");
	}

	// Token: 0x0600283C RID: 10300 RVA: 0x00021C58 File Offset: 0x0001FE58
	public void SFX_SNOWCULT_JackFrostEyeballZap()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_eyeball_zap");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_eyeball_zap");
	}

	// Token: 0x04002163 RID: 8547
	public LevelProperties.SnowCult.EyeAttack properties;

	// Token: 0x04002164 RID: 8548
	public Vector3 endPos;

	// Token: 0x04002165 RID: 8549
	public Vector3 startPos;

	// Token: 0x04002166 RID: 8550
	public float angle;

	// Token: 0x04002167 RID: 8551
	public bool onRight;

	// Token: 0x04002168 RID: 8552
	public bool upsideDown;

	// Token: 0x04002169 RID: 8553
	public bool readyToOpenMouth;

	// Token: 0x0400216A RID: 8554
	public bool readyToCloseMouth;

	// Token: 0x0400216C RID: 8556
	[SerializeField]
	public Animator beamAnimator;

	// Token: 0x0400216D RID: 8557
	[SerializeField]
	public float openMouthDistance = 400f;

	// Token: 0x0400216E RID: 8558
	[SerializeField]
	public float beamEndDistance = 200f;

	// Token: 0x0400216F RID: 8559
	[SerializeField]
	public float animatorTakeoverDistance = 31f;

	// Token: 0x04002170 RID: 8560
	public SnowCultLevelJackFrost main;

	// Token: 0x04002171 RID: 8561
	public Coroutine beamCR;

	// Token: 0x04002172 RID: 8562
	public bool controlledByParent;

	// Token: 0x04002173 RID: 8563
	[SerializeField]
	public GameObject shadow;

	// Token: 0x04002174 RID: 8564
	public Vector3 vel;

	// Token: 0x04002175 RID: 8565
	public Vector3 lastPos;

	// Token: 0x04002176 RID: 8566
	public new bool dead;
}
