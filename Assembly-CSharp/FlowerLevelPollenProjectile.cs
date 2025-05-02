using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000227 RID: 551
public class FlowerLevelPollenProjectile : BasicProjectile
{
	// Token: 0x0600193E RID: 6462 RVA: 0x000A5E8C File Offset: 0x000A408C
	public void InitPollen(float speed, float strength, int type, Transform target)
	{
		this.pct = 0f;
		this.time = 0.7795515f;
		this.manual = true;
		this.speed = -speed;
		this.waveStrength = strength;
		this.target = target;
		this.Speed = 0f;
		this.move = false;
		if (type == 1)
		{
			this.SetParryable(true);
			base.animator.Play("Pink_Idle");
		}
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.spawn_petals_cr(type));
	}

	// Token: 0x0600193F RID: 6463 RVA: 0x000A5F18 File Offset: 0x000A4118
	public void StartMoving()
	{
		this.manual = false;
		this.move = true;
		this.Speed = this.speed;
		this.initPosY = base.transform.position.y;
	}

	// Token: 0x06001940 RID: 6464 RVA: 0x00015869 File Offset: 0x00013A69
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.rotate_cr());
	}

	// Token: 0x06001941 RID: 6465 RVA: 0x000A5F58 File Offset: 0x000A4158
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (CupheadTime.GlobalSpeed != 0f)
			{
				if (!this.manual)
				{
					Vector3 position = base.transform.position;
					position.y = this.initPosY + Mathf.Sin(this.time * 6f) * (this.waveStrength * this.pct) * CupheadTime.GlobalSpeed;
					base.transform.position = position;
					if (this.pct < 1f)
					{
						this.pct += CupheadTime.FixedDelta * 2f;
					}
					else
					{
						this.pct = 1f;
					}
				}
				else
				{
					base.transform.position = this.target.position;
					this.Speed = 0f;
				}
				this.time += CupheadTime.FixedDelta;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001942 RID: 6466 RVA: 0x000A5F74 File Offset: 0x000A4174
	public IEnumerator rotate_cr()
	{
		float val = (!Rand.Bool()) ? 420f : -420f;
		float frameTime = 0f;
		for (;;)
		{
			frameTime += CupheadTime.Delta;
			if (frameTime > 0.0416666679f)
			{
				frameTime -= 0.0416666679f;
				this.sprite.transform.Rotate(0f, 0f, val * CupheadTime.Delta);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001943 RID: 6467 RVA: 0x000A5F90 File Offset: 0x000A4190
	public IEnumerator spawn_petals_cr(int type)
	{
		for (;;)
		{
			if (type == 1)
			{
				this.petalPink.Create(base.transform.position);
			}
			else
			{
				this.petal.Create(base.transform.position);
			}
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0.2f, 1f));
		}
		yield break;
	}

	// Token: 0x06001944 RID: 6468 RVA: 0x000A5FB4 File Offset: 0x000A41B4
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			CupheadRenderer instance = CupheadRenderer.Instance;
			instance.TouchFuzzy(15f, 8f, 1.2f);
			base.GetComponent<AudioWarble>().HandleWarble();
		}
	}

	// Token: 0x06001945 RID: 6469 RVA: 0x0001587E File Offset: 0x00013A7E
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001946 RID: 6470 RVA: 0x00015891 File Offset: 0x00013A91
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.petal = null;
		this.petalPink = null;
	}

	// Token: 0x0400145B RID: 5211
	public const float ROTATE_FRAME_TIME = 0.0416666679f;

	// Token: 0x0400145C RID: 5212
	[SerializeField]
	public SpriteRenderer sprite;

	// Token: 0x0400145D RID: 5213
	[SerializeField]
	public FlowerLevelPollenPetal petalPink;

	// Token: 0x0400145E RID: 5214
	[SerializeField]
	public FlowerLevelPollenPetal petal;

	// Token: 0x0400145F RID: 5215
	public bool manual;

	// Token: 0x04001460 RID: 5216
	public float time;

	// Token: 0x04001461 RID: 5217
	public float speed;

	// Token: 0x04001462 RID: 5218
	public float waveStrength;

	// Token: 0x04001463 RID: 5219
	public float initPosY;

	// Token: 0x04001464 RID: 5220
	public Transform target;

	// Token: 0x04001465 RID: 5221
	public float pct;
}
