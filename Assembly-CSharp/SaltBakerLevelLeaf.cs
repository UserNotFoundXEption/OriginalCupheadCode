using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000377 RID: 887
public class SaltBakerLevelLeaf : AbstractProjectile
{
	// Token: 0x06002729 RID: 10025 RVA: 0x000CB05C File Offset: 0x000C925C
	public virtual SaltBakerLevelLeaf Init(Vector3 pos, float xTime, float xDistance, float yGravity, MinMax ySpeed, SaltbakerLevelSaltbaker parent, int animID)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		this.xDistance = xDistance;
		this.xTime = xTime;
		this.ySpeedMinMax = ySpeed;
		this.yGravity = yGravity;
		this.Move();
		this.parent = parent;
		this.parent.OnDeathEvent += this.Death;
		this.animID = animID;
		return this;
	}

	// Token: 0x0600272A RID: 10026 RVA: 0x00020EB0 File Offset: 0x0001F0B0
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600272B RID: 10027 RVA: 0x00020ECE File Offset: 0x0001F0CE
	public void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x0600272C RID: 10028 RVA: 0x000CB0D0 File Offset: 0x000C92D0
	public IEnumerator move_cr()
	{
		float ground = (float)Level.Current.Ground;
		YieldInstruction wait = new WaitForFixedUpdate();
		float val = 0f;
		float yVal = 0f;
		float xVal = 0f;
		float t = 0f;
		float startX = base.transform.position.x;
		float endX = base.transform.position.x + this.xDistance;
		AnimationHelper animationHelper = base.GetComponent<AnimationHelper>();
		animationHelper.Speed = 0f;
		bool dirRight = true;
		while (base.transform.position.y > ground)
		{
			val = t / this.xTime;
			xVal = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, val);
			yVal = ((val >= 0.5f) ? (1f - val) : val);
			float ySpeed = this.ySpeedMinMax.GetFloatAt(yVal);
			float yPos = base.transform.position.y - (ySpeed + this.yGravity) * CupheadTime.FixedDelta;
			string animName = Convert.ToChar(65 + this.animID).ToString();
			t += CupheadTime.FixedDelta;
			base.transform.SetPosition(new float?(Mathf.Lerp(startX, endX, xVal)), new float?(yPos), null);
			base.animator.Play(animName, 0, val / 2f + ((!dirRight) ? 0.5f : 0f));
			if (t >= this.xTime)
			{
				t = 0f;
				endX = startX;
				startX = base.transform.position.x;
				dirRight = !dirRight;
			}
			yield return wait;
		}
		this.boxColl.enabled = false;
		animationHelper.Speed = 1f;
		base.animator.SetTrigger("Die");
		yield return base.animator.WaitForAnimationToStart(this, "None", false);
		this.Recycle<SaltBakerLevelLeaf>();
		yield return null;
		yield break;
	}

	// Token: 0x0600272D RID: 10029 RVA: 0x00020EDD File Offset: 0x0001F0DD
	public void Death()
	{
		this.Recycle<SaltBakerLevelLeaf>();
	}

	// Token: 0x0600272E RID: 10030 RVA: 0x00020EE5 File Offset: 0x0001F0E5
	public override void OnDestroy()
	{
		this.parent.OnDeathEvent -= this.Death;
		base.OnDestroy();
	}

	// Token: 0x04002069 RID: 8297
	public float xTime;

	// Token: 0x0400206A RID: 8298
	public float xDistance;

	// Token: 0x0400206B RID: 8299
	public float yGravity;

	// Token: 0x0400206C RID: 8300
	public MinMax ySpeedMinMax;

	// Token: 0x0400206D RID: 8301
	public SaltbakerLevelSaltbaker parent;

	// Token: 0x0400206E RID: 8302
	public int animID;

	// Token: 0x0400206F RID: 8303
	[SerializeField]
	public BoxCollider2D boxColl;
}
