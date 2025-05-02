using System;
using UnityEngine;

// Token: 0x02000508 RID: 1288
public class HealerCharmParticleEffect : AbstractPausableComponent
{
	// Token: 0x060035D7 RID: 13783 RVA: 0x000FB394 File Offset: 0x000F9594
	public override void Awake()
	{
		base.Awake();
		base.transform.localScale = new Vector3((float)MathUtils.PlusOrMinus(), 1f);
		this.acceleration = this.accelerationRange.RandomFloat();
		this.timeBeforeSeek = this.timeBeforeSeekRange.RandomFloat();
		this.maxSpeed = this.maxSpeedRange.RandomFloat();
		base.animator.Play("Loop", 0, Random.Range(0f, 1f));
		base.animator.Update(0f);
	}

	// Token: 0x060035D8 RID: 13784 RVA: 0x000FB428 File Offset: 0x000F9628
	public void SetVars(Vector2 newVel, AbstractPlayerController newTarget, HealerCharmSparkEffect newMain)
	{
		this.target = newTarget;
		this.vel = newVel * this.initialEmissionSpeed;
		this.main = newMain;
		base.transform.position += this.vel * 0.04f;
	}

	// Token: 0x060035D9 RID: 13785 RVA: 0x000FB480 File Offset: 0x000F9680
	public void FixedUpdate()
	{
		if (CupheadTime.FixedDelta == 0f)
		{
			return;
		}
		if (this.target == null)
		{
			return;
		}
		this.frameTimer += CupheadTime.FixedDelta;
		while (this.frameTimer > this.frameTime)
		{
			this.frameTimer -= this.frameTime;
			this.FrameUpdate();
		}
	}

	// Token: 0x060035DA RID: 13786 RVA: 0x000FB4F0 File Offset: 0x000F96F0
	public void FrameUpdate()
	{
		base.transform.position += this.vel * this.frameTime;
		base.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(-25f, 25f, Mathf.InverseLerp(-this.maxSpeed, this.maxSpeed, this.vel.x)) * -Mathf.Sign(base.transform.localScale.x));
		this.timer += this.frameTime;
		if (this.timer > this.timeBeforeSeek)
		{
			Vector3 vector = this.target.center - base.transform.position;
			float magnitude = vector.magnitude;
			if (magnitude < this.contactDistance && this.timer > this.timeBeforeCanCollect)
			{
				if (this.main)
				{
					this.main.StartPlayerFlash();
				}
				Object.Destroy(base.gameObject);
			}
			else
			{
				this.vel += vector * (this.timer - this.timeBeforeSeek) * (this.timer - this.timeBeforeSeek) * this.acceleration * this.frameTime;
				if (this.vel.magnitude > this.maxSpeed)
				{
					this.vel = this.vel.normalized * this.maxSpeed;
				}
				if (this.timer > this.timeBeforeLerp)
				{
					float num = Mathf.InverseLerp(this.timeBeforeLerp, this.maxTime, this.timer);
					num *= num;
					base.transform.position = Vector3.Lerp(base.transform.position, this.target.center, num);
				}
			}
		}
	}

	// Token: 0x04002BB9 RID: 11193
	[SerializeField]
	public float initialEmissionSpeed = 400f;

	// Token: 0x04002BBA RID: 11194
	[SerializeField]
	public MinMax timeBeforeSeekRange = new MinMax(0.025f, 0.075f);

	// Token: 0x04002BBB RID: 11195
	public float timeBeforeSeek;

	// Token: 0x04002BBC RID: 11196
	[SerializeField]
	public float timeBeforeCanCollect = 0.5f;

	// Token: 0x04002BBD RID: 11197
	[SerializeField]
	public float timeBeforeLerp = 1f;

	// Token: 0x04002BBE RID: 11198
	[SerializeField]
	public float maxTime = 0.75f;

	// Token: 0x04002BBF RID: 11199
	[SerializeField]
	public MinMax accelerationRange = new MinMax(150f, 250f);

	// Token: 0x04002BC0 RID: 11200
	public float acceleration;

	// Token: 0x04002BC1 RID: 11201
	[SerializeField]
	public MinMax maxSpeedRange = new MinMax(1500f, 2500f);

	// Token: 0x04002BC2 RID: 11202
	public float maxSpeed;

	// Token: 0x04002BC3 RID: 11203
	[SerializeField]
	public float contactDistance = 25f;

	// Token: 0x04002BC4 RID: 11204
	public AbstractPlayerController target;

	// Token: 0x04002BC5 RID: 11205
	public float timer;

	// Token: 0x04002BC6 RID: 11206
	public Vector3 vel;

	// Token: 0x04002BC7 RID: 11207
	[SerializeField]
	public float frameTime = 0.0416666679f;

	// Token: 0x04002BC8 RID: 11208
	public float frameTimer;

	// Token: 0x04002BC9 RID: 11209
	public HealerCharmSparkEffect main;
}
