using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000301 RID: 769
public class RetroArcadeBouncyBallHolder : RetroArcadeEnemy
{
	// Token: 0x0600222A RID: 8746 RVA: 0x000BC6B8 File Offset: 0x000BA8B8
	public RetroArcadeBouncyBallHolder Create(RetroArcadeBouncyManager manager, LevelProperties.RetroArcade.Bouncy properties, Vector3 pos, string[] ballTypes)
	{
		RetroArcadeBouncyBallHolder retroArcadeBouncyBallHolder = this.InstantiatePrefab<RetroArcadeBouncyBallHolder>();
		retroArcadeBouncyBallHolder.manager = manager;
		retroArcadeBouncyBallHolder.properties = properties;
		retroArcadeBouncyBallHolder.transform.position = pos;
		retroArcadeBouncyBallHolder.ballTypes = ballTypes;
		return retroArcadeBouncyBallHolder;
	}

	// Token: 0x0600222B RID: 8747 RVA: 0x0001D2F4 File Offset: 0x0001B4F4
	public override void Start()
	{
		this.hp = 1f;
		this.SetBalls();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x0600222C RID: 8748 RVA: 0x000BC6F0 File Offset: 0x000BA8F0
	public void SetBalls()
	{
		this.ballsHeld = new RetroArcadeBouncyBall[this.ballPositions.Length];
		RetroArcadeBouncyBall retroArcadeBouncyBall = this.typeABall;
		float num = 120f;
		int i = 0;
		while (i < this.ballPositions.Length)
		{
			string text = this.ballTypes[i];
			if (text == null)
			{
				goto IL_8F;
			}
			if (!(text == "A"))
			{
				if (!(text == "B"))
				{
					if (!(text == "C"))
					{
						goto IL_8F;
					}
					retroArcadeBouncyBall = this.typeCBall;
				}
				else
				{
					retroArcadeBouncyBall = this.typeBBall;
				}
			}
			else
			{
				retroArcadeBouncyBall = this.typeABall;
			}
			IL_9F:
			RetroArcadeBouncyBall retroArcadeBouncyBall2 = retroArcadeBouncyBall.Create(this.ballPositions[i].position, this.manager, this.properties, num * (float)i);
			this.ballsHeld[i] = retroArcadeBouncyBall2;
			this.ballsHeld[i].transform.parent = base.transform;
			i++;
			continue;
			IL_8F:
			Debug.LogError("Something bad happened", null);
			goto IL_9F;
		}
	}

	// Token: 0x0600222D RID: 8749 RVA: 0x000BC7F8 File Offset: 0x000BA9F8
	public void SeparateBalls()
	{
		base.GetComponent<Collider2D>().enabled = false;
		foreach (RetroArcadeBouncyBall retroArcadeBouncyBall in this.ballsHeld)
		{
			retroArcadeBouncyBall.StartMoving(base.transform.position);
		}
		base.StartCoroutine(this.check_to_die_cr());
	}

	// Token: 0x0600222E RID: 8750 RVA: 0x000BC850 File Offset: 0x000BAA50
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.red;
		foreach (Transform transform in this.ballPositions)
		{
			Gizmos.DrawWireSphere(transform.position, 20f);
		}
	}

	// Token: 0x0600222F RID: 8751 RVA: 0x0001D314 File Offset: 0x0001B514
	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06002230 RID: 8752 RVA: 0x000BC89C File Offset: 0x000BAA9C
	public IEnumerator move_cr()
	{
		this.velocity = MathUtils.AngleToDirection(this.properties.angleRange.RandomFloat());
		for (;;)
		{
			base.transform.position += this.velocity * this.properties.groupMoveSpeed * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06002231 RID: 8753 RVA: 0x000BC8B8 File Offset: 0x000BAAB8
	public override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		Vector3 newVelocity = this.velocity;
		newVelocity.y = Mathf.Min(newVelocity.y, -newVelocity.y);
		this.ChangeDir(newVelocity);
	}

	// Token: 0x06002232 RID: 8754 RVA: 0x000BC8F8 File Offset: 0x000BAAF8
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		Vector3 newVelocity = this.velocity;
		newVelocity.y = Mathf.Max(newVelocity.y, -newVelocity.y);
		this.ChangeDir(newVelocity);
	}

	// Token: 0x06002233 RID: 8755 RVA: 0x000BC938 File Offset: 0x000BAB38
	public void ChangeDir(Vector3 newVelocity)
	{
		this.velocity = newVelocity;
		this.currentAngle = Mathf.Atan2(this.velocity.y, this.velocity.x) * 57.29578f;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.currentAngle));
		foreach (RetroArcadeBouncyBall retroArcadeBouncyBall in this.ballsHeld)
		{
			retroArcadeBouncyBall.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		}
	}

	// Token: 0x06002234 RID: 8756 RVA: 0x000BC9E8 File Offset: 0x000BABE8
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionWalls(hit, phase);
		Vector3 newVelocity = this.velocity;
		if (base.transform.position.x > 0f)
		{
			newVelocity.x = Mathf.Min(newVelocity.x, -newVelocity.x);
			this.ChangeDir(newVelocity);
		}
		else
		{
			newVelocity.x = Mathf.Max(newVelocity.x, -newVelocity.x);
			this.ChangeDir(newVelocity);
		}
	}

	// Token: 0x06002235 RID: 8757 RVA: 0x0001D31C File Offset: 0x0001B51C
	public override void Dead()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		this.SeparateBalls();
	}

	// Token: 0x06002236 RID: 8758 RVA: 0x000BCA6C File Offset: 0x000BAC6C
	public IEnumerator check_to_die_cr()
	{
		bool allDead = true;
		for (;;)
		{
			allDead = true;
			for (int i = 0; i < this.ballsHeld.Length; i++)
			{
				if (!this.ballsHeld[i].IsDead)
				{
					allDead = false;
				}
			}
			if (allDead)
			{
				break;
			}
			yield return null;
		}
		base.IsDead = true;
		yield break;
	}

	// Token: 0x06002237 RID: 8759 RVA: 0x000BCA88 File Offset: 0x000BAC88
	public void DestroyBallsHeld()
	{
		foreach (RetroArcadeBouncyBall retroArcadeBouncyBall in this.ballsHeld)
		{
			Object.Destroy(retroArcadeBouncyBall.gameObject);
		}
	}

	// Token: 0x04001C23 RID: 7203
	[SerializeField]
	public Transform[] ballPositions;

	// Token: 0x04001C24 RID: 7204
	[SerializeField]
	public RetroArcadeBouncyBall typeABall;

	// Token: 0x04001C25 RID: 7205
	[SerializeField]
	public RetroArcadeBouncyBall typeBBall;

	// Token: 0x04001C26 RID: 7206
	[SerializeField]
	public RetroArcadeBouncyBall typeCBall;

	// Token: 0x04001C27 RID: 7207
	public RetroArcadeBouncyBall[] ballsHeld;

	// Token: 0x04001C28 RID: 7208
	public float currentAngle;

	// Token: 0x04001C29 RID: 7209
	public string[] ballTypes;

	// Token: 0x04001C2A RID: 7210
	public Vector3 velocity;

	// Token: 0x04001C2B RID: 7211
	public LevelProperties.RetroArcade.Bouncy properties;

	// Token: 0x04001C2C RID: 7212
	public RetroArcadeBouncyManager manager;
}
