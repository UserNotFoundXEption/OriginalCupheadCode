using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001FE RID: 510
public class DicePalacePachinkoLevelPipeBall : AbstractProjectile
{
	// Token: 0x06001785 RID: 6021 RVA: 0x000A1B24 File Offset: 0x0009FD24
	public override void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (base.transform.position.y < (float)(Level.Current.Ground - 20))
		{
			Object.Destroy(base.gameObject);
		}
		base.Update();
	}

	// Token: 0x06001786 RID: 6022 RVA: 0x000A1B80 File Offset: 0x0009FD80
	public void InitBall(LevelProperties.DicePalacePachinko properties)
	{
		this.properties = properties;
		this.directionIndex = Random.Range(0, properties.CurrentState.balls.directionString.Split(new char[]
		{
			','
		}).Length);
		this.speed = properties.CurrentState.balls.movementSpeed;
		this.onGround = false;
		base.StartCoroutine(this.pick_dir_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001787 RID: 6023 RVA: 0x000A1BFC File Offset: 0x0009FDFC
	public IEnumerator move_cr()
	{
		for (;;)
		{
			if (this.bouncing)
			{
				yield return null;
			}
			else
			{
				if (this.onGround)
				{
					base.transform.localPosition += Vector3.right * this.speed * CupheadTime.Delta;
				}
				else
				{
					base.transform.localPosition += Vector3.down * this.speed * CupheadTime.Delta;
				}
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06001788 RID: 6024 RVA: 0x000A1C18 File Offset: 0x0009FE18
	public IEnumerator pick_dir_cr()
	{
		while (!this.onGround)
		{
			yield return null;
		}
		this.directionIndex++;
		this.ChangeDirection();
		yield break;
	}

	// Token: 0x06001789 RID: 6025 RVA: 0x000A1C34 File Offset: 0x0009FE34
	public void ChangeDirection()
	{
		if (this.directionIndex >= this.properties.CurrentState.balls.directionString.Split(new char[]
		{
			','
		}).Length)
		{
			this.directionIndex = 0;
		}
		char c = this.properties.CurrentState.balls.directionString.Split(new char[]
		{
			','
		})[this.directionIndex][0];
		if (c != 'L')
		{
			if (c == 'R')
			{
				this.speed = this.properties.CurrentState.balls.movementSpeed;
			}
		}
		else
		{
			this.speed = -this.properties.CurrentState.balls.movementSpeed;
		}
	}

	// Token: 0x0600178A RID: 6026 RVA: 0x000A1D08 File Offset: 0x0009FF08
	public IEnumerator changeState_cr(bool grounded, bool forceDirection)
	{
		yield return null;
		this.ChangeDirection();
		if (grounded)
		{
			this.onGround = true;
			if (this.currentCollider == this.lastCollider)
			{
				yield break;
			}
			base.animator.SetTrigger("Bounce");
			this.bouncing = true;
			yield return null;
			yield return base.animator.WaitForAnimationToEnd(this, "Bounce", 1, false, true);
			this.lastCollider = this.currentCollider;
			Animator platformAnimnator = this.currentCollider.GetComponent<Animator>();
			if (platformAnimnator == null)
			{
				this.bouncing = false;
				yield break;
			}
			if (base.transform.position.x - this.currentCollider.transform.position.x > 0f)
			{
				platformAnimnator.SetTrigger("Right");
				this.speed = this.properties.CurrentState.balls.movementSpeed;
			}
			else
			{
				platformAnimnator.SetTrigger("Left");
				this.speed = -this.properties.CurrentState.balls.movementSpeed;
			}
			base.transform.SetParent(platformAnimnator.transform, true);
			yield return null;
			this.bouncing = false;
			float finalSpeed = this.speed;
			float acceleration = 0f;
			while (this.onGround)
			{
				acceleration += CupheadTime.Delta;
				this.speed = Mathf.Min(Mathf.Lerp(0f, finalSpeed, acceleration * 2f), finalSpeed);
				yield return null;
			}
			platformAnimnator.SetTrigger("Back");
			base.transform.SetParent(null, true);
			base.transform.rotation = Quaternion.identity;
		}
		else
		{
			this.onGround = false;
			this.speed = this.properties.CurrentState.balls.movementSpeed;
		}
		yield break;
	}

	// Token: 0x0600178B RID: 6027 RVA: 0x000A1D2C File Offset: 0x0009FF2C
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (phase == CollisionPhase.Enter && !this.onGround)
		{
			this.currentCollider = hit.GetComponent<Collider2D>();
			if (hit.GetComponent<LevelPlatform>() != null)
			{
				base.StartCoroutine(this.changeState_cr(true, true));
				base.OnCollisionOther(hit, phase);
			}
			else if (hit.GetComponent<DicePalacePachinkoLevelPeg>() != null)
			{
				base.StartCoroutine(this.changeState_cr(true, hit.GetComponent<DicePalacePachinkoLevelPeg>().forceDirection));
				base.OnCollisionOther(hit, phase);
			}
		}
		else if (phase == CollisionPhase.Exit)
		{
			base.StartCoroutine(this.changeState_cr(false, false));
			base.OnCollisionOther(hit, phase);
		}
	}

	// Token: 0x0600178C RID: 6028 RVA: 0x000140B7 File Offset: 0x000122B7
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionWalls(hit, phase);
	}

	// Token: 0x0600178D RID: 6029 RVA: 0x000140C1 File Offset: 0x000122C1
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x0600178E RID: 6030 RVA: 0x000140D0 File Offset: 0x000122D0
	public override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x0400131A RID: 4890
	public bool onGround;

	// Token: 0x0400131B RID: 4891
	public float speed;

	// Token: 0x0400131C RID: 4892
	public int directionIndex;

	// Token: 0x0400131D RID: 4893
	public LevelProperties.DicePalacePachinko properties;

	// Token: 0x0400131E RID: 4894
	public bool bouncing;

	// Token: 0x0400131F RID: 4895
	public Collider2D lastCollider;

	// Token: 0x04001320 RID: 4896
	public Collider2D currentCollider;
}
