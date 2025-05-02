using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200040C RID: 1036
public class CircusPlatformingLevelPoleBot : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002D35 RID: 11573 RVA: 0x000DC384 File Offset: 0x000DA584
	public override void Start()
	{
		base.Start();
		this.start = base.transform.position.y;
		this.velocity = new Vector2(Random.Range(this.minVelocity.min, this.minVelocity.max), Random.Range(this.maxVelocity.min, this.maxVelocity.max));
		this.startVelocity = this.velocity;
		this.gravity = 1000f;
	}

	// Token: 0x06002D36 RID: 11574 RVA: 0x00025BDA File Offset: 0x00023DDA
	public void SlideDown()
	{
		base.StartCoroutine(this.slide_cr());
	}

	// Token: 0x06002D37 RID: 11575 RVA: 0x000DC408 File Offset: 0x000DA608
	public IEnumerator slide_cr()
	{
		this.isSliding = true;
		base.animator.SetBool("Falling", true);
		YieldInstruction wait = new WaitForFixedUpdate();
		yield return CupheadTime.WaitForSeconds(this, this.fallDelay);
		while (base.transform.position.y > this.start - base.GetComponent<BoxCollider2D>().size.y * 1.38f)
		{
			base.transform.AddPosition(0f, -base.Properties.poleSpeedMovement * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		this.start = base.transform.position.y;
		this.isSliding = false;
		base.animator.SetBool("Falling", false);
		yield return null;
		yield break;
	}

	// Token: 0x06002D38 RID: 11576 RVA: 0x00025BE9 File Offset: 0x00023DE9
	public override void OnStart()
	{
	}

	// Token: 0x06002D39 RID: 11577 RVA: 0x00025BEB File Offset: 0x00023DEB
	public override void Die()
	{
		this.PoleBotDeathSFX();
		this.isDying = true;
		base.animator.SetTrigger("Dead");
		base.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.fly_cr());
	}

	// Token: 0x06002D3A RID: 11578 RVA: 0x000DC424 File Offset: 0x000DA624
	public IEnumerator fly_cr()
	{
		base._damageReceiver.enabled = false;
		YieldInstruction wait = new WaitForFixedUpdate();
		float timeToApex = Mathf.Sqrt(2f * base.transform.position.y / this.gravity);
		this.startVelocity.y = timeToApex * this.gravity;
		while (base.transform.position.y > CupheadLevelCamera.Current.Bounds.yMin)
		{
			base.transform.AddPosition(-this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
			base.transform.Rotate(Vector3.forward, this.deadSpin * CupheadTime.Delta);
			yield return wait;
		}
		this.<Die>__BaseCallProxy0();
		yield return null;
		yield break;
	}

	// Token: 0x06002D3B RID: 11579 RVA: 0x00025C23 File Offset: 0x00023E23
	public void PoleBotIdleSFX()
	{
		if (!AudioManager.CheckIfPlaying("circus_pole_guy_idle"))
		{
			AudioManager.Play("circus_pole_guy_idle");
			this.emitAudioFromObject.Add("circus_pole_guy_idle");
		}
	}

	// Token: 0x06002D3C RID: 11580 RVA: 0x00025C4E File Offset: 0x00023E4E
	public void PoleBotFallSFX()
	{
		AudioManager.Play("circus_pole_guy_falling");
		this.emitAudioFromObject.Add("circus_pole_guy_falling");
	}

	// Token: 0x06002D3D RID: 11581 RVA: 0x00025C6A File Offset: 0x00023E6A
	public void PoleBotDeathSFX()
	{
		AudioManager.Play("circus_pole_guy_death");
		this.emitAudioFromObject.Add("circus_pole_guy_death");
	}

	// Token: 0x04002570 RID: 9584
	public const string FallingParameterName = "Falling";

	// Token: 0x04002571 RID: 9585
	public const string DeadParameterName = "Dead";

	// Token: 0x04002572 RID: 9586
	public Vector2 velocity;

	// Token: 0x04002573 RID: 9587
	public Vector2 startVelocity;

	// Token: 0x04002574 RID: 9588
	public float gravity;

	// Token: 0x04002575 RID: 9589
	public bool isDying;

	// Token: 0x04002576 RID: 9590
	public bool isSliding;

	// Token: 0x04002577 RID: 9591
	[SerializeField]
	public float fallDelay;

	// Token: 0x04002578 RID: 9592
	[SerializeField]
	public float deadSpin;

	// Token: 0x04002579 RID: 9593
	[SerializeField]
	public MinMax minVelocity;

	// Token: 0x0400257A RID: 9594
	[SerializeField]
	public MinMax maxVelocity;

	// Token: 0x0400257B RID: 9595
	public float start;
}
