using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000317 RID: 791
public class RetroArcadeSnakeBodyPart : AbstractCollidableObject
{
	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x060022CB RID: 8907 RVA: 0x0001D8E2 File Offset: 0x0001BAE2
	// (set) Token: 0x060022CC RID: 8908 RVA: 0x0001D8EA File Offset: 0x0001BAEA
	public RetroArcadeSnakeBodyPart.Direction currentDirection { get; set; }

	// Token: 0x060022CD RID: 8909 RVA: 0x000BECA0 File Offset: 0x000BCEA0
	public RetroArcadeSnakeBodyPart Create(Vector2 pos, bool isHead, RetroArcadeSnakeBodyPart.Direction direction, RetroArcadeSnakeManager manager, RetroArcadeSnakeBodyPart previousPart, float speed)
	{
		RetroArcadeSnakeBodyPart retroArcadeSnakeBodyPart = this.InstantiatePrefab<RetroArcadeSnakeBodyPart>();
		retroArcadeSnakeBodyPart.transform.position = pos;
		retroArcadeSnakeBodyPart.currentDirection = direction;
		retroArcadeSnakeBodyPart.partInFront = previousPart;
		retroArcadeSnakeBodyPart.speed = speed;
		retroArcadeSnakeBodyPart.isHead = isHead;
		retroArcadeSnakeBodyPart.manager = manager;
		return retroArcadeSnakeBodyPart;
	}

	// Token: 0x060022CE RID: 8910 RVA: 0x0001D8F3 File Offset: 0x0001BAF3
	public void Start()
	{
		this.canTurn = true;
		this.ChangeDirection(this.currentDirection, false);
		if (this.isHead)
		{
			base.StartCoroutine(this.head_check_cr());
		}
		else
		{
			base.StartCoroutine(this.body_check_cr());
		}
	}

	// Token: 0x060022CF RID: 8911 RVA: 0x0001D933 File Offset: 0x0001BB33
	public void FixedUpdate()
	{
		if (!this.isDead)
		{
			base.transform.position += this.dir * this.speed * CupheadTime.FixedDelta;
		}
	}

	// Token: 0x060022D0 RID: 8912 RVA: 0x000BECEC File Offset: 0x000BCEEC
	public IEnumerator body_check_cr()
	{
		for (;;)
		{
			if (this.currentDirection != this.partInFront.currentDirection)
			{
				if (this.currentDirection == RetroArcadeSnakeBodyPart.Direction.Right)
				{
					if (base.transform.position.x >= this.partInFront.turnPos.x)
					{
						this.ClampDirectionChange();
					}
				}
				else if (this.currentDirection == RetroArcadeSnakeBodyPart.Direction.Left)
				{
					if (base.transform.position.x <= this.partInFront.turnPos.x)
					{
						this.ClampDirectionChange();
					}
				}
				else if (this.currentDirection == RetroArcadeSnakeBodyPart.Direction.Up)
				{
					if (base.transform.position.y >= this.partInFront.turnPos.y)
					{
						this.ClampDirectionChange();
					}
				}
				else if (this.currentDirection == RetroArcadeSnakeBodyPart.Direction.Down && base.transform.position.y <= this.partInFront.turnPos.y)
				{
					this.ClampDirectionChange();
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060022D1 RID: 8913 RVA: 0x000BED08 File Offset: 0x000BCF08
	public void ClampDirectionChange()
	{
		base.transform.position = this.partInFront.transform.position + -this.partInFront.dir * 60f;
		this.ChangeDirection(this.partInFront.currentDirection, false);
	}

	// Token: 0x060022D2 RID: 8914 RVA: 0x000BED64 File Offset: 0x000BCF64
	public IEnumerator head_check_cr()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		for (;;)
		{
			if (this.currentDirection == RetroArcadeSnakeBodyPart.Direction.Up || this.currentDirection == RetroArcadeSnakeBodyPart.Direction.Down)
			{
				if (base.transform.position.y >= 230f || base.transform.position.y <= -120f)
				{
					this.SwitchToHorizontal();
				}
				else
				{
					if (player != null && !player.IsDead)
					{
						this.CheckPlayerY(player);
					}
					if (player2 != null && !player2.IsDead)
					{
						this.CheckPlayerY(player2);
					}
				}
			}
			else if (base.transform.position.x >= 330f || base.transform.position.x <= -330f)
			{
				this.SwitchToVertical();
			}
			else
			{
				if (player != null && !player.IsDead)
				{
					this.CheckPlayerX(player);
				}
				if (player2 != null && !player2.IsDead)
				{
					this.CheckPlayerX(player2);
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060022D3 RID: 8915 RVA: 0x000BED80 File Offset: 0x000BCF80
	public void CheckPlayerX(AbstractPlayerController player)
	{
		float num = player.transform.position.x - base.transform.position.x;
		if (Mathf.Abs(num) < 20f)
		{
			if (player.transform.position.y < base.transform.position.y)
			{
				if (base.transform.position.y <= -100f)
				{
					return;
				}
				this.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Down, true);
			}
			else
			{
				if (base.transform.position.y >= 210f)
				{
					return;
				}
				this.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Up, true);
			}
		}
	}

	// Token: 0x060022D4 RID: 8916 RVA: 0x000BEE44 File Offset: 0x000BD044
	public void CheckPlayerY(AbstractPlayerController player)
	{
		float num = player.transform.position.y - base.transform.position.y;
		if (Mathf.Abs(num) < 20f)
		{
			if (player.transform.position.x < base.transform.position.x)
			{
				if (player.transform.position.x <= -310f)
				{
					return;
				}
				this.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Left, true);
			}
			else
			{
				if (player.transform.position.x >= 310f)
				{
					return;
				}
				this.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Right, true);
			}
		}
	}

	// Token: 0x060022D5 RID: 8917 RVA: 0x000BEF08 File Offset: 0x000BD108
	public void SwitchToHorizontal()
	{
		if (base.transform.position.x < 0f)
		{
			this.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Right, true);
		}
		else
		{
			this.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Left, true);
		}
	}

	// Token: 0x060022D6 RID: 8918 RVA: 0x000BEF48 File Offset: 0x000BD148
	public void SwitchToVertical()
	{
		if (base.transform.position.y < 0f)
		{
			this.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Up, true);
		}
		else
		{
			this.ChangeDirection(RetroArcadeSnakeBodyPart.Direction.Down, true);
		}
	}

	// Token: 0x060022D7 RID: 8919 RVA: 0x000BEF88 File Offset: 0x000BD188
	public void ChangeDirection(RetroArcadeSnakeBodyPart.Direction direction, bool checkTurn)
	{
		if (checkTurn && !this.canTurn)
		{
			return;
		}
		this.currentDirection = direction;
		this.turnPos = base.transform.position;
		switch (this.currentDirection)
		{
		case RetroArcadeSnakeBodyPart.Direction.Left:
			this.dir = Vector3.left;
			break;
		case RetroArcadeSnakeBodyPart.Direction.Right:
			this.dir = Vector3.right;
			break;
		case RetroArcadeSnakeBodyPart.Direction.Up:
			this.dir = Vector3.up;
			break;
		case RetroArcadeSnakeBodyPart.Direction.Down:
			this.dir = Vector3.down;
			break;
		}
		base.StartCoroutine(this.turn_timer_cr());
	}

	// Token: 0x060022D8 RID: 8920 RVA: 0x000BF030 File Offset: 0x000BD230
	public IEnumerator turn_timer_cr()
	{
		float t = 0f;
		float time = 0.5f;
		this.canTurn = false;
		while (t < time)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this.canTurn = true;
		yield return null;
		yield break;
	}

	// Token: 0x060022D9 RID: 8921 RVA: 0x000BF04C File Offset: 0x000BD24C
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (this.isHead && !this.isDead && hit.GetComponent<RetroArcadeSnakeBodyPart>() != this.partBehind)
		{
			this.manager.EndPhase();
			this.isDead = true;
		}
	}

	// Token: 0x060022DA RID: 8922 RVA: 0x0001D971 File Offset: 0x0001BB71
	public void GetPartBehind(RetroArcadeSnakeBodyPart behind)
	{
		this.partBehind = behind;
	}

	// Token: 0x060022DB RID: 8923 RVA: 0x0001D97A File Offset: 0x0001BB7A
	public void Die()
	{
		this.StopAllCoroutines();
		this.isDead = true;
	}

	// Token: 0x04001CCB RID: 7371
	public Vector3 turnPos;

	// Token: 0x04001CCC RID: 7372
	public Vector3 dir;

	// Token: 0x04001CCD RID: 7373
	public const float TOP_Y = 230f;

	// Token: 0x04001CCE RID: 7374
	public const float BOTTOM_Y = -120f;

	// Token: 0x04001CCF RID: 7375
	public const float OFFSCREEN_Y = 300f;

	// Token: 0x04001CD0 RID: 7376
	public const float SIDE_X = 330f;

	// Token: 0x04001CD1 RID: 7377
	public const float MIN_DISTANCE = 20f;

	// Token: 0x04001CD2 RID: 7378
	public const float SPACING = 60f;

	// Token: 0x04001CD4 RID: 7380
	public RetroArcadeSnakeBodyPart partInFront;

	// Token: 0x04001CD5 RID: 7381
	public RetroArcadeSnakeBodyPart partBehind;

	// Token: 0x04001CD6 RID: 7382
	public RetroArcadeSnakeManager manager;

	// Token: 0x04001CD7 RID: 7383
	public float speed;

	// Token: 0x04001CD8 RID: 7384
	public bool canTurn;

	// Token: 0x04001CD9 RID: 7385
	public bool isHead;

	// Token: 0x04001CDA RID: 7386
	public bool isDead;

	// Token: 0x02000E48 RID: 3656
	public enum Direction
	{
		// Token: 0x04006734 RID: 26420
		Left,
		// Token: 0x04006735 RID: 26421
		Right,
		// Token: 0x04006736 RID: 26422
		Up,
		// Token: 0x04006737 RID: 26423
		Down
	}
}
