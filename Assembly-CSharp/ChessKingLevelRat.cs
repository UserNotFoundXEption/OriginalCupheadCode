using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000185 RID: 389
public class ChessKingLevelRat : AbstractCollidableObject
{
	// Token: 0x06001271 RID: 4721 RVA: 0x00095148 File Offset: 0x00093348
	public void Init(Vector3 position, float speed)
	{
		base.transform.position = position;
		this.startPosX = base.transform.position.x;
		this.speed = speed;
		this.Move();
	}

	// Token: 0x06001272 RID: 4722 RVA: 0x0000F8BE File Offset: 0x0000DABE
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001273 RID: 4723 RVA: 0x0000F8D1 File Offset: 0x0000DAD1
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001274 RID: 4724 RVA: 0x0000F8E9 File Offset: 0x0000DAE9
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001275 RID: 4725 RVA: 0x0000F907 File Offset: 0x0000DB07
	public void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001276 RID: 4726 RVA: 0x00095188 File Offset: 0x00093388
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float leftBound = (float)Level.Current.Left + 75f;
		float rightBound = this.startPosX;
		bool goingRight = false;
		for (;;)
		{
			if (goingRight)
			{
				while (base.transform.position.x < rightBound)
				{
					base.transform.position += Vector3.right * this.speed * CupheadTime.FixedDelta;
					yield return wait;
				}
				goingRight = false;
				base.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
			}
			else
			{
				while (base.transform.position.x > leftBound)
				{
					base.transform.position += Vector3.left * this.speed * CupheadTime.FixedDelta;
					yield return wait;
				}
				goingRight = true;
				base.transform.SetScale(new float?(-1f), new float?(1f), new float?(1f));
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000EE6 RID: 3814
	public float speed;

	// Token: 0x04000EE7 RID: 3815
	public float startPosX;

	// Token: 0x04000EE8 RID: 3816
	public DamageDealer damageDealer;
}
