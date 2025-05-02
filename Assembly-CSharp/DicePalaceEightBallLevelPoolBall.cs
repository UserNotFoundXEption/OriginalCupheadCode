using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001E9 RID: 489
public class DicePalaceEightBallLevelPoolBall : AbstractProjectile
{
	// Token: 0x06001694 RID: 5780 RVA: 0x0009F460 File Offset: 0x0009D660
	public DicePalaceEightBallLevelPoolBall Create(Vector2 pos, float horSpeed, float verSpeed, float gravity, float delay, bool onLeft, DicePalaceEightBallLevelEightBall parent)
	{
		DicePalaceEightBallLevelPoolBall dicePalaceEightBallLevelPoolBall = base.Create() as DicePalaceEightBallLevelPoolBall;
		dicePalaceEightBallLevelPoolBall.transform.position = pos;
		dicePalaceEightBallLevelPoolBall.horSpeed = horSpeed;
		dicePalaceEightBallLevelPoolBall.verSpeed = verSpeed;
		dicePalaceEightBallLevelPoolBall.gravity = gravity;
		dicePalaceEightBallLevelPoolBall.delay = delay;
		dicePalaceEightBallLevelPoolBall.onLeft = onLeft;
		dicePalaceEightBallLevelPoolBall.parent = parent;
		return dicePalaceEightBallLevelPoolBall;
	}

	// Token: 0x06001695 RID: 5781 RVA: 0x0009F4BC File Offset: 0x0009D6BC
	public override void Start()
	{
		base.Start();
		this.shadowInstance = Object.Instantiate<GameObject>(this.shadowPrefab).transform;
		this.shadowInstance.gameObject.SetActive(false);
		this.dustInstance = Object.Instantiate<GameObject>(this.dustPrefab).transform;
		this.shadowInstance.gameObject.SetActive(false);
		base.StartCoroutine(this.jump_cr());
		base.StartCoroutine(this.check_dying_cr());
		DicePalaceEightBallLevelEightBall dicePalaceEightBallLevelEightBall = this.parent;
		dicePalaceEightBallLevelEightBall.OnEightBallDeath = (Action)Delegate.Combine(dicePalaceEightBallLevelEightBall.OnEightBallDeath, new Action(this.EightBallDead));
	}

	// Token: 0x06001696 RID: 5782 RVA: 0x0009F560 File Offset: 0x0009D760
	public void SetVariation(int index)
	{
		for (int i = 0; i < this.colorVariations.Length; i++)
		{
			this.colorVariations[i].SetActive(false);
		}
		if (index >= 0 && index < this.colorVariations.Length)
		{
			this.colorVariations[index].SetActive(true);
		}
	}

	// Token: 0x06001697 RID: 5783 RVA: 0x00013387 File Offset: 0x00011587
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001698 RID: 5784 RVA: 0x000133A5 File Offset: 0x000115A5
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001699 RID: 5785 RVA: 0x0009F5B8 File Offset: 0x0009D7B8
	public IEnumerator jump_cr()
	{
		bool jumping = false;
		bool goingUp = false;
		bool upsideDown = false;
		float velocityY = this.verSpeed;
		float velocityX = this.horSpeed;
		float ground = (float)Level.Current.Ground + 55f;
		this.dustInstance.gameObject.SetActive(false);
		while (base.transform.position.y > ground)
		{
			velocityY -= this.gravity / 2f * CupheadTime.Delta;
			base.transform.AddPosition(0f, velocityY * CupheadTime.Delta, 0f);
			yield return null;
		}
		Vector3 p = base.transform.position;
		p.y = ground;
		base.transform.position = p;
		this.dustInstance.position = base.transform.position;
		this.dustInstance.gameObject.SetActive(true);
		base.animator.SetTrigger("Smash");
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.delay);
			jumping = true;
			goingUp = true;
			velocityY = this.verSpeed;
			velocityX = ((!this.onLeft) ? (-this.horSpeed) : this.horSpeed);
			base.animator.SetTrigger("Jump");
			this.shadowInstance.gameObject.SetActive(false);
			if (upsideDown)
			{
				yield return base.animator.WaitForAnimationToEnd(this, "UpsideDownJump", true, true);
			}
			else
			{
				yield return base.animator.WaitForAnimationToEnd(this, "Jump", true, true);
			}
			this.shadowInstance.gameObject.SetActive(true);
			this.dustInstance.gameObject.SetActive(false);
			while (jumping)
			{
				this.shadowInstance.position = new Vector3(base.transform.position.x, ground, 0f);
				velocityY -= this.gravity * CupheadTime.Delta;
				base.transform.AddPosition(velocityX * CupheadTime.Delta, velocityY * CupheadTime.Delta, 0f);
				if (velocityY < 0f && goingUp)
				{
					base.animator.SetTrigger("Turn");
					goingUp = false;
					if (upsideDown)
					{
						yield return base.animator.WaitForAnimationToEnd(this, "RightSideUpSmash_start", true, true);
					}
					else
					{
						yield return base.animator.WaitForAnimationToEnd(this, "JumpTurn", true, true);
					}
				}
				if (velocityY < 0f && jumping && base.transform.position.y <= ground)
				{
					base.animator.SetTrigger("Smash");
					jumping = false;
					upsideDown = !upsideDown;
					Vector3 position = base.transform.position;
					position.y = ground;
					base.transform.position = position;
					this.dustInstance.position = base.transform.position;
					this.dustInstance.gameObject.SetActive(true);
				}
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x0600169A RID: 5786 RVA: 0x0009F5D4 File Offset: 0x0009D7D4
	public IEnumerator check_dying_cr()
	{
		for (;;)
		{
			if (this.onLeft)
			{
				if (base.transform.position.x > 840f)
				{
					break;
				}
			}
			else if (base.transform.position.x < -840f)
			{
				break;
			}
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x0600169B RID: 5787 RVA: 0x000133C3 File Offset: 0x000115C3
	public void EightBallDead()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.eight_ball_death_cr());
	}

	// Token: 0x0600169C RID: 5788 RVA: 0x0009F5F0 File Offset: 0x0009D7F0
	public IEnumerator eight_ball_death_cr()
	{
		float speed = 2500f;
		float angle = (float)Random.Range(0, 360);
		Vector3 dir = MathUtils.AngleToDirection(angle);
		base.GetComponent<Collider2D>().enabled = false;
		for (;;)
		{
			base.transform.position += dir * speed * CupheadTime.FixedDelta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600169D RID: 5789 RVA: 0x0009F60C File Offset: 0x0009D80C
	public override void OnDestroy()
	{
		if (this.shadowInstance != null)
		{
			Object.Destroy(this.shadowInstance.gameObject);
		}
		if (this.dustInstance != null)
		{
			Object.Destroy(this.dustInstance.gameObject);
		}
		base.OnDestroy();
		this.shadowPrefab = null;
		this.dustPrefab = null;
	}

	// Token: 0x0600169E RID: 5790 RVA: 0x000133D8 File Offset: 0x000115D8
	public override void Die()
	{
		base.Die();
		DicePalaceEightBallLevelEightBall dicePalaceEightBallLevelEightBall = this.parent;
		dicePalaceEightBallLevelEightBall.OnEightBallDeath = (Action)Delegate.Remove(dicePalaceEightBallLevelEightBall.OnEightBallDeath, new Action(this.EightBallDead));
	}

	// Token: 0x04001251 RID: 4689
	public const float OffsetY = 55f;

	// Token: 0x04001252 RID: 4690
	[SerializeField]
	public GameObject shadowPrefab;

	// Token: 0x04001253 RID: 4691
	[SerializeField]
	public GameObject dustPrefab;

	// Token: 0x04001254 RID: 4692
	[SerializeField]
	public GameObject[] colorVariations;

	// Token: 0x04001255 RID: 4693
	public DicePalaceEightBallLevelEightBall parent;

	// Token: 0x04001256 RID: 4694
	public float horSpeed;

	// Token: 0x04001257 RID: 4695
	public float verSpeed;

	// Token: 0x04001258 RID: 4696
	public float gravity;

	// Token: 0x04001259 RID: 4697
	public float delay;

	// Token: 0x0400125A RID: 4698
	public bool onLeft;

	// Token: 0x0400125B RID: 4699
	public Transform shadowInstance;

	// Token: 0x0400125C RID: 4700
	public Transform dustInstance;
}
