using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003D9 RID: 985
public class PlatformingLevelGroundMovementEnemy : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002B73 RID: 11123 RVA: 0x000D5FDC File Offset: 0x000D41DC
	public PlatformingLevelGroundMovementEnemy Spawn(Vector3 position, PlatformingLevelGroundMovementEnemy.Direction startDirection, bool destroyEnemyAfterLeavingScreen)
	{
		PlatformingLevelGroundMovementEnemy platformingLevelGroundMovementEnemy = this.InstantiatePrefab<PlatformingLevelGroundMovementEnemy>();
		platformingLevelGroundMovementEnemy.transform.position = position;
		platformingLevelGroundMovementEnemy._destroyEnemyAfterLeavingScreen = destroyEnemyAfterLeavingScreen;
		platformingLevelGroundMovementEnemy._startCondition = AbstractPlatformingLevelEnemy.StartCondition.Instant;
		platformingLevelGroundMovementEnemy._direction = startDirection;
		return platformingLevelGroundMovementEnemy;
	}

	// Token: 0x17000348 RID: 840
	// (get) Token: 0x06002B74 RID: 11124 RVA: 0x000247DF File Offset: 0x000229DF
	public PlatformingLevelGroundMovementEnemy.Direction direction
	{
		get
		{
			return this._direction;
		}
	}

	// Token: 0x17000349 RID: 841
	// (get) Token: 0x06002B75 RID: 11125 RVA: 0x000247E7 File Offset: 0x000229E7
	// (set) Token: 0x06002B76 RID: 11126 RVA: 0x000247EF File Offset: 0x000229EF
	public bool Grounded { get; set; }

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x06002B77 RID: 11127 RVA: 0x000247F8 File Offset: 0x000229F8
	public virtual Collider2D collider
	{
		get
		{
			return this._collider;
		}
	}

	// Token: 0x06002B78 RID: 11128 RVA: 0x000D6014 File Offset: 0x000D4214
	public override void Awake()
	{
		base.Awake();
		this._collider = base.GetComponent<Collider2D>();
		this.directionManager = new PlatformingLevelGroundMovementEnemy.DirectionManager();
		this.jumpManager = new PlatformingLevelGroundMovementEnemy.JumpManager();
		this.timeSinceTurn = 10000f;
		if (this.shadow != null)
		{
			this.shadow.parent = null;
		}
		this.SetTurnTarget("Turn");
	}

	// Token: 0x06002B79 RID: 11129 RVA: 0x00024800 File Offset: 0x00022A00
	public override void OnStart()
	{
	}

	// Token: 0x06002B7A RID: 11130 RVA: 0x000D607C File Offset: 0x000D427C
	public void GoToGround(bool despawnOnPit = true, string groundStateName = "Run")
	{
		base.animator.Play(groundStateName);
		Bounds bounds = this.collider.bounds;
		Vector2 vector = bounds.center - base.transform.position;
		if (!this.gravityReversed)
		{
			this.hits = this.BoxCastAll(new Vector2(bounds.size.x, 1f), Vector2.down, this.groundMask, new Vector2(0f, -bounds.size.y / 4f));
		}
		else
		{
			this.hits = this.BoxCastAll(new Vector2(bounds.size.x, 1f), Vector2.up, this.ceilingMask, new Vector2(0f, bounds.size.y));
		}
		Vector2 vector2 = base.transform.position;
		bool flag = false;
		foreach (RaycastHit2D raycastHit2D in this.hits)
		{
			LevelPlatform component = raycastHit2D.collider.gameObject.GetComponent<LevelPlatform>();
			if (raycastHit2D.collider != null && (this.canSpawnOnPlatforms || component == null || !component.canFallThrough))
			{
				vector2 = raycastHit2D.point;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Object.Destroy(base.gameObject);
		}
		base.transform.SetPosition(null, new float?(vector2.y), null);
		this.HandleRaycasts();
		this.jumpManager.ableToLand = true;
		this.OnGrounded();
		if (despawnOnPit)
		{
			Vector2 vector3 = base.transform.position + vector + new Vector2((this.direction != PlatformingLevelGroundMovementEnemy.Direction.Left) ? (-bounds.size.x / 2f) : (bounds.size.x / 2f), this.turnaroundDistance / 2f - bounds.size.y / 2f);
			Vector2 vector4 = base.transform.position + vector + new Vector2((this.direction != PlatformingLevelGroundMovementEnemy.Direction.Left) ? this.turnaroundDistance : (-this.turnaroundDistance), this.turnaroundDistance / 2f - bounds.size.y / 2f);
			for (int j = 0; j <= 10; j++)
			{
				float num = (float)j / 10f;
				if (!this.gravityReversed)
				{
					if (Physics2D.Raycast(Vector2.Lerp(vector3, vector4, num), Vector2.down, 30f + this.turnaroundDistance, this.groundMask).collider == null)
					{
						Object.Destroy(base.gameObject);
						return;
					}
				}
				else if (Physics2D.Raycast(Vector2.Lerp(vector3, vector4, num), Vector2.up, 30f + this.turnaroundDistance, this.ceilingMask).collider == null)
				{
					Object.Destroy(base.gameObject);
					return;
				}
			}
		}
	}

	// Token: 0x06002B7B RID: 11131 RVA: 0x00024802 File Offset: 0x00022A02
	public void Float(bool playAnim = true)
	{
		if (playAnim)
		{
			base.animator.Play("Float", 0, Random.Range(0f, 1f));
		}
		this.playFloatAnim = playAnim;
		this.floating = true;
	}

	// Token: 0x06002B7C RID: 11132 RVA: 0x00024838 File Offset: 0x00022A38
	public override void Update()
	{
		base.Update();
		this.CalculateDirection();
		this.CalculateRender();
		if (this.shadow != null)
		{
			this.UpdateShadow();
		}
	}

	// Token: 0x06002B7D RID: 11133 RVA: 0x00024863 File Offset: 0x00022A63
	public virtual void FixedUpdate()
	{
		if (base.Dead || base.GetComponent<DamageReceiver>().IsHitPaused)
		{
			return;
		}
		this.HandleRaycasts();
		this.HandleFalling();
		this.Move();
	}

	// Token: 0x06002B7E RID: 11134 RVA: 0x000D640C File Offset: 0x000D460C
	public void HandleFalling()
	{
		if (this.Grounded)
		{
			return;
		}
		if (this.floating)
		{
			this.velocity = new Vector2(0f, -base.Properties.floatSpeed);
		}
		else if (!this.gravityReversed)
		{
			this.velocity.y = this.velocity.y - base.Properties.gravity * CupheadTime.FixedDelta;
			this.jumpManager.ableToLand = (this.velocity.y < 0f);
		}
		else
		{
			this.velocity.y = this.velocity.y + base.Properties.gravity * CupheadTime.FixedDelta;
			this.jumpManager.ableToLand = (this.velocity.y < 0f);
		}
	}

	// Token: 0x06002B7F RID: 11135 RVA: 0x00024893 File Offset: 0x00022A93
	public virtual float GetMoveSpeed()
	{
		if (this.moveSpeed == 0f)
		{
			this.moveSpeed = base.Properties.MoveSpeed;
		}
		return this.moveSpeed;
	}

	// Token: 0x06002B80 RID: 11136 RVA: 0x000248BC File Offset: 0x00022ABC
	public virtual void SetMoveSpeed(float moveSpeed)
	{
		this.moveSpeed = moveSpeed;
	}

	// Token: 0x06002B81 RID: 11137 RVA: 0x000D64E4 File Offset: 0x000D46E4
	public void Move()
	{
		if (this.turning || this.landing || (this.jumping && this.Grounded))
		{
			return;
		}
		this.timeSinceTurn += CupheadTime.FixedDelta;
		float num = (float)((this._direction != PlatformingLevelGroundMovementEnemy.Direction.Right) ? -1 : 1);
		if (this.jumpManager.state == PlatformingLevelGroundMovementEnemy.JumpManager.State.Ready && !this.floating)
		{
			this.velocity.x = this.GetMoveSpeed() * num;
		}
		base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
		if (!this.gravityReversed)
		{
			if (this.Grounded && base.transform.position.y - this.directionManager.down.pos.y < 30f)
			{
				Vector2 vector = base.transform.position;
				vector.y = this.directionManager.down.pos.y;
				base.transform.position = vector;
			}
		}
		else if (this.Grounded && base.transform.position.y + this.directionManager.up.pos.y > 30f)
		{
			Vector2 vector2 = base.transform.position;
			vector2.y = this.directionManager.up.pos.y;
			base.transform.position = vector2;
		}
	}

	// Token: 0x06002B82 RID: 11138 RVA: 0x000D66B0 File Offset: 0x000D48B0
	public void CalculateRender()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position) && !this._enteredScreen)
		{
			this._enteredScreen = true;
		}
		if (this._enteredScreen && this._destroyEnemyAfterLeavingScreen && !CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 100f)))
		{
			Object.Destroy(base.gameObject);
		}
		if (PlatformingLevel.Current != null && (base.transform.position.x < (float)PlatformingLevel.Current.Left - 100f || base.transform.position.x > (float)PlatformingLevel.Current.Right + 100f || base.transform.position.y < (float)PlatformingLevel.Current.Ground - 100f))
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06002B83 RID: 11139 RVA: 0x000248C5 File Offset: 0x00022AC5
	public void LateUpdate()
	{
		this.CalculateDirection();
	}

	// Token: 0x06002B84 RID: 11140 RVA: 0x000D67D4 File Offset: 0x000D49D4
	public virtual void CalculateDirection()
	{
		if (this._direction == PlatformingLevelGroundMovementEnemy.Direction.Right)
		{
			base.transform.SetScale(new float?(-1f), null, null);
		}
		else
		{
			base.transform.SetScale(new float?(1f), null, null);
		}
	}

	// Token: 0x06002B85 RID: 11141 RVA: 0x000248CD File Offset: 0x00022ACD
	public override void Die()
	{
		if (this.shadow != null)
		{
			Object.Destroy(this.shadow.gameObject);
		}
		base.Die();
	}

	// Token: 0x06002B86 RID: 11142 RVA: 0x000248F6 File Offset: 0x00022AF6
	public virtual Coroutine Turn()
	{
		this.turning = true;
		this.timeSinceTurn = 0f;
		return base.StartCoroutine(this.turn_cr());
	}

	// Token: 0x06002B87 RID: 11143 RVA: 0x000D6840 File Offset: 0x000D4A40
	public IEnumerator turn_cr()
	{
		if (this.hasTurnAnimation && base.animator != null)
		{
			base.animator.Play("Turn");
			int target = Animator.StringToHash(base.animator.GetLayerName(0) + "." + this.turnTarget);
			while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != target)
			{
				yield return null;
			}
			float animLength = base.animator.GetCurrentAnimatorStateInfo(0).length;
			while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= (animLength - CupheadTime.Delta) / animLength)
			{
				yield return null;
			}
		}
		if (this._direction == PlatformingLevelGroundMovementEnemy.Direction.Right)
		{
			this._direction = PlatformingLevelGroundMovementEnemy.Direction.Left;
		}
		else
		{
			this._direction = PlatformingLevelGroundMovementEnemy.Direction.Right;
		}
		this.CalculateDirection();
		this.turning = false;
		yield break;
	}

	// Token: 0x06002B88 RID: 11144 RVA: 0x00024916 File Offset: 0x00022B16
	public virtual void SetTurnTarget(string turnTarget)
	{
		this.turnTarget = turnTarget;
	}

	// Token: 0x06002B89 RID: 11145 RVA: 0x000D685C File Offset: 0x000D4A5C
	public IEnumerator floatLand_cr()
	{
		this.floating = false;
		this.landing = true;
		if (!this.lockDirectionWhenLanding)
		{
			this._direction = ((PlayerManager.GetNext().center.x <= base.transform.position.x) ? PlatformingLevelGroundMovementEnemy.Direction.Left : PlatformingLevelGroundMovementEnemy.Direction.Right);
		}
		base.transform.SetPosition(null, new float?(this.directionManager.down.pos.y), null);
		if (this.playFloatAnim)
		{
			base.animator.Play("Land");
		}
		this.playFloatAnim = false;
		yield return base.animator.WaitForAnimationToEnd(this, "Land", false, true);
		this.velocity.y = 0f;
		this.landing = false;
		yield break;
	}

	// Token: 0x06002B8A RID: 11146 RVA: 0x0002491F File Offset: 0x00022B1F
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.jumpLandEffectPrefab = null;
	}

	// Token: 0x06002B8B RID: 11147 RVA: 0x000D6878 File Offset: 0x000D4A78
	public void Jump()
	{
		if (base.Properties.canJump && this.jumpManager.state == PlatformingLevelGroundMovementEnemy.JumpManager.State.Ready && !this.turning)
		{
			this.jumpManager.state = PlatformingLevelGroundMovementEnemy.JumpManager.State.Used;
			base.StartCoroutine(this.jump_cr());
			this.jumping = true;
		}
	}

	// Token: 0x06002B8C RID: 11148 RVA: 0x000D68D0 File Offset: 0x000D4AD0
	public IEnumerator jump_cr()
	{
		if (this.hasJumpAnimation && base.animator != null)
		{
			base.animator.Play("Jump");
			int target = Animator.StringToHash(base.animator.GetLayerName(0) + ".Jump");
			while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != target)
			{
				yield return null;
			}
			float animLength = base.animator.GetCurrentAnimatorStateInfo(0).length;
			while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= (animLength - CupheadTime.Delta) / animLength)
			{
				yield return null;
			}
		}
		float directionSign = (float)((this._direction != PlatformingLevelGroundMovementEnemy.Direction.Right) ? -1 : 1);
		float timeToApex = Mathf.Sqrt(2f * base.Properties.jumpHeight / base.Properties.gravity);
		float x = (!this.manuallySetJumpX) ? base.Properties.jumpLength : this.GetMoveSpeed();
		this.LeaveGround();
		this.velocity.y = base.Properties.gravity * timeToApex;
		this.velocity.x = directionSign * x / (2f * timeToApex);
		if (this.hasJumpAnimation && base.animator != null)
		{
			yield return CupheadTime.WaitForSeconds(this, timeToApex);
			base.animator.SetTrigger("Apex");
		}
		while (this.jumping)
		{
			yield return null;
		}
		this.landing = true;
		if (this.directionManager.down != null)
		{
			base.transform.SetPosition(null, new float?(this.directionManager.down.pos.y), null);
		}
		if (this.jumpLandEffectPrefab != null)
		{
			this.jumpLandEffectPrefab.Create(base.transform.position);
		}
		if (this.hasJumpAnimation && base.animator != null)
		{
			base.animator.SetTrigger("Land");
			yield return base.animator.WaitForAnimationToEnd(this, "Jump_Land", false, true);
		}
		this.landing = false;
		yield break;
	}

	// Token: 0x06002B8D RID: 11149 RVA: 0x0002492E File Offset: 0x00022B2E
	public RaycastHit2D BoxCast(Vector2 size, Vector2 direction, int layerMask)
	{
		return this.BoxCast(size, direction, layerMask, Vector2.zero);
	}

	// Token: 0x06002B8E RID: 11150 RVA: 0x000D68EC File Offset: 0x000D4AEC
	public RaycastHit2D BoxCast(Vector2 size, Vector2 direction, int layerMask, Vector2 offset)
	{
		return Physics2D.BoxCast(this.collider.bounds.center + offset, size, 0f, direction, 2000f, layerMask);
	}

	// Token: 0x06002B8F RID: 11151 RVA: 0x000D692C File Offset: 0x000D4B2C
	public RaycastHit2D[] BoxCastAll(Vector2 size, Vector2 direction, int layerMask, Vector2 offset)
	{
		return Physics2D.BoxCastAll(this.collider.bounds.center + offset, size, 0f, direction, 2000f, layerMask);
	}

	// Token: 0x06002B90 RID: 11152 RVA: 0x000D696C File Offset: 0x000D4B6C
	public RaycastHit2D CircleCast(float radius, Vector2 direction, int layerMask)
	{
		return Physics2D.CircleCast(this.collider.bounds.center, radius, direction, 2000f, layerMask);
	}

	// Token: 0x06002B91 RID: 11153 RVA: 0x0002493E File Offset: 0x00022B3E
	public bool DoesRaycastHitHaveCollider(RaycastHit2D hit)
	{
		return hit.collider != null;
	}

	// Token: 0x06002B92 RID: 11154 RVA: 0x000D69A0 File Offset: 0x000D4BA0
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (Application.isPlaying)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(base.transform.position, 5f);
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(base.transform.position, 5f);
		}
	}

	// Token: 0x06002B93 RID: 11155 RVA: 0x000D69FC File Offset: 0x000D4BFC
	public void HandleRaycasts()
	{
		if (this.fallInPit)
		{
			return;
		}
		bool flag = true;
		if (this.directionManager != null && this.directionManager.up != null)
		{
			flag = this.directionManager.up.able;
		}
		Bounds bounds = this.collider.bounds;
		this.directionManager.Reset();
		RaycastHit2D raycastHit = this.BoxCast(new Vector2(bounds.size.x, 1f), Vector2.up, this.ceilingMask);
		RaycastHit2D raycastHit2 = this.BoxCast(new Vector2(bounds.size.x, 1f), Vector2.down, this.groundMask, new Vector2(base.transform.position.x - bounds.center.x, base.transform.position.y + 30f - bounds.center.y));
		RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position + new Vector2((this.direction != PlatformingLevelGroundMovementEnemy.Direction.Left) ? this.turnaroundDistance : (-this.turnaroundDistance), this.turnaroundDistance / 2f), Vector2.down, 30f + this.turnaroundDistance, this.groundMask);
		RaycastHit2D raycastHit2D2 = Physics2D.Raycast(base.transform.position + new Vector2((this.direction != PlatformingLevelGroundMovementEnemy.Direction.Left) ? this.turnaroundDistance : (-this.turnaroundDistance), this.turnaroundDistance / 2f), Vector2.up, 30f + this.turnaroundDistance, this.ceilingMask);
		this.RaycastObstacle(this.directionManager.up, raycastHit, bounds.size.y / 2f, PlatformingLevelGroundMovementEnemy.RaycastAxis.Y, bounds.center);
		this.RaycastObstacle(this.directionManager.down, raycastHit2, 30f, PlatformingLevelGroundMovementEnemy.RaycastAxis.Y, new Vector2(base.transform.position.x, base.transform.position.y + 30f));
		if (!this.Grounded)
		{
			if (!this.directionManager.down.able)
			{
				this.OnGrounded();
				this.directionManager.left.able = true;
				this.directionManager.right.able = true;
				if (this.floating)
				{
					base.StartCoroutine(this.floatLand_cr());
				}
			}
			if (!this.directionManager.up.able && this.directionManager.up.able != flag)
			{
				this.OnHitCeiling();
			}
		}
		RaycastHit2D raycastHit2D3 = this.gravityReversed ? raycastHit2D2 : raycastHit2D;
		if (this.Grounded && raycastHit2D3.collider == null && this.timeSinceTurn > 0.1f && !this.jumping)
		{
			if (!this.noTurn)
			{
				this.Turn();
			}
			else
			{
				this.LeaveGround();
			}
		}
	}

	// Token: 0x06002B94 RID: 11156 RVA: 0x000D6D4C File Offset: 0x000D4F4C
	public float RaycastObstacle(PlatformingLevelGroundMovementEnemy.DirectionManager.Hit directionProperties, RaycastHit2D raycastHit, float maxDistance, PlatformingLevelGroundMovementEnemy.RaycastAxis axis, Vector2 origin)
	{
		if (!this.DoesRaycastHitHaveCollider(raycastHit))
		{
			return 1000f;
		}
		float num = (axis != PlatformingLevelGroundMovementEnemy.RaycastAxis.X) ? Mathf.Abs(origin.y - raycastHit.point.y) : Mathf.Abs(origin.x - raycastHit.point.x);
		directionProperties.pos = raycastHit.point;
		directionProperties.gameObject = raycastHit.collider.gameObject;
		directionProperties.distance = num;
		if (num < maxDistance)
		{
			directionProperties.able = false;
		}
		return num;
	}

	// Token: 0x06002B95 RID: 11157 RVA: 0x0002494D File Offset: 0x00022B4D
	public void ValidateRaycast()
	{
	}

	// Token: 0x06002B96 RID: 11158 RVA: 0x000D6DE8 File Offset: 0x000D4FE8
	public void OnGrounded()
	{
		if (this.Grounded || !this.jumpManager.ableToLand)
		{
			return;
		}
		LevelPlatform levelPlatform = (!(this.directionManager.down.gameObject == null)) ? this.directionManager.down.gameObject.GetComponent<LevelPlatform>() : null;
		LevelPlatform levelPlatform2 = (!(this.directionManager.up.gameObject == null)) ? this.directionManager.up.gameObject.GetComponent<LevelPlatform>() : null;
		LevelPlatform levelPlatform3 = this.gravityReversed ? levelPlatform2 : levelPlatform;
		if (levelPlatform3 != null)
		{
			levelPlatform3.AddChild(base.transform);
		}
		this.jumpManager.state = PlatformingLevelGroundMovementEnemy.JumpManager.State.Ready;
		this.velocity.y = 0f;
		this.Grounded = true;
		if (this.jumping)
		{
			this.jumping = false;
		}
	}

	// Token: 0x06002B97 RID: 11159 RVA: 0x000D6EE0 File Offset: 0x000D50E0
	public void LeaveGround()
	{
		this.Grounded = false;
		this.jumpManager.ableToLand = false;
		this.velocity.y = 0f;
		this.ClearParent();
		if (this.jumpManager.state == PlatformingLevelGroundMovementEnemy.JumpManager.State.Ready)
		{
			this.jumpManager.state = PlatformingLevelGroundMovementEnemy.JumpManager.State.Used;
		}
	}

	// Token: 0x06002B98 RID: 11160 RVA: 0x000D6F34 File Offset: 0x000D5134
	public void OnHitCeiling()
	{
		if (!this.gravityReversed)
		{
			if (this.jumpManager.ableToLand)
			{
				return;
			}
		}
		else
		{
			if (this.Grounded)
			{
				return;
			}
			this.jumpManager.state = PlatformingLevelGroundMovementEnemy.JumpManager.State.Ready;
			LevelPlatform levelPlatform = (!(this.directionManager.up.gameObject == null)) ? this.directionManager.up.gameObject.GetComponent<LevelPlatform>() : null;
			if (levelPlatform != null)
			{
				levelPlatform.AddChild(base.transform);
			}
			this.Grounded = true;
			if (this.jumping)
			{
				this.jumping = false;
			}
		}
		this.velocity.y = 0f;
		this.directionManager.left.able = true;
		this.directionManager.right.able = true;
	}

	// Token: 0x06002B99 RID: 11161 RVA: 0x0002494F File Offset: 0x00022B4F
	public void ClearParent()
	{
		if (base.transform.parent != null)
		{
			base.transform.parent.GetComponent<LevelPlatform>().OnPlayerExit(base.transform);
		}
		base.transform.parent = null;
	}

	// Token: 0x06002B9A RID: 11162 RVA: 0x000D7014 File Offset: 0x000D5214
	public void UpdateShadow()
	{
		if (this.Grounded)
		{
			this.shadow.gameObject.SetActive(false);
			return;
		}
		RaycastHit2D raycastHit2D = Physics2D.BoxCast(base.transform.position, new Vector2(this.collider.bounds.size.x, 1f), 0f, Vector2.down, this.maxShadowDistance, this.groundMask);
		if (raycastHit2D.collider == null)
		{
			this.shadow.gameObject.SetActive(false);
			return;
		}
		this.shadow.gameObject.SetActive(true);
		this.shadow.SetPosition(new float?(base.transform.position.x), new float?(raycastHit2D.point.y), null);
		float num = base.transform.position.y - this.shadow.position.y;
		this.shadow.GetComponent<Animator>().Play("Idle", 0, num / this.maxShadowDistance);
		this.shadow.GetComponent<Animator>().speed = 0f;
	}

	// Token: 0x04002403 RID: 9219
	public const float SCREEN_PADDING = 100f;

	// Token: 0x04002404 RID: 9220
	public const float DOWN_BOXCAST_Y = 30f;

	// Token: 0x04002405 RID: 9221
	public float startPosition = 0.5f;

	// Token: 0x04002406 RID: 9222
	[SerializeField]
	public PlatformingLevelGroundMovementEnemy.Direction _direction = PlatformingLevelGroundMovementEnemy.Direction.Right;

	// Token: 0x04002407 RID: 9223
	[SerializeField]
	public bool hasJumpAnimation;

	// Token: 0x04002408 RID: 9224
	[SerializeField]
	public bool hasTurnAnimation;

	// Token: 0x04002409 RID: 9225
	[SerializeField]
	public bool canSpawnOnPlatforms;

	// Token: 0x0400240A RID: 9226
	[SerializeField]
	public float turnaroundDistance = 10f;

	// Token: 0x0400240B RID: 9227
	[SerializeField]
	public Transform shadow;

	// Token: 0x0400240C RID: 9228
	[SerializeField]
	public float maxShadowDistance;

	// Token: 0x0400240D RID: 9229
	[SerializeField]
	public Effect jumpLandEffectPrefab;

	// Token: 0x0400240E RID: 9230
	[SerializeField]
	public bool noTurn;

	// Token: 0x0400240F RID: 9231
	[SerializeField]
	public bool lockDirectionWhenLanding;

	// Token: 0x04002410 RID: 9232
	[SerializeField]
	public bool gravityReversed;

	// Token: 0x04002412 RID: 9234
	public Collider2D _collider;

	// Token: 0x04002413 RID: 9235
	public bool _destroyEnemyAfterLeavingScreen;

	// Token: 0x04002414 RID: 9236
	public bool _enteredScreen;

	// Token: 0x04002415 RID: 9237
	public PlatformingLevelGroundMovementEnemy.DirectionManager directionManager;

	// Token: 0x04002416 RID: 9238
	public PlatformingLevelGroundMovementEnemy.JumpManager jumpManager;

	// Token: 0x04002417 RID: 9239
	public bool turning;

	// Token: 0x04002418 RID: 9240
	public bool floating;

	// Token: 0x04002419 RID: 9241
	public bool manuallySetJumpX;

	// Token: 0x0400241A RID: 9242
	public float timeSinceTurn;

	// Token: 0x0400241B RID: 9243
	public string turnTarget;

	// Token: 0x0400241C RID: 9244
	public float moveSpeed;

	// Token: 0x0400241D RID: 9245
	public bool jumping;

	// Token: 0x0400241E RID: 9246
	public bool landing;

	// Token: 0x0400241F RID: 9247
	public bool fallInPit;

	// Token: 0x04002420 RID: 9248
	public bool playFloatAnim;

	// Token: 0x04002421 RID: 9249
	public Vector2 velocity = Vector2.zero;

	// Token: 0x04002422 RID: 9250
	public RaycastHit2D[] hits;

	// Token: 0x04002423 RID: 9251
	public const float RAY_DISTANCE = 2000f;

	// Token: 0x04002424 RID: 9252
	public const float MAX_GROUNDED_FALL_DISTANCE = 30f;

	// Token: 0x04002425 RID: 9253
	public readonly int ceilingMask = 524288;

	// Token: 0x04002426 RID: 9254
	public readonly int groundMask = 1048576;

	// Token: 0x02001000 RID: 4096
	public enum Direction
	{
		// Token: 0x04007286 RID: 29318
		Right = 1,
		// Token: 0x04007287 RID: 29319
		Left = -1
	}

	// Token: 0x02001001 RID: 4097
	public enum RaycastAxis
	{
		// Token: 0x04007289 RID: 29321
		X,
		// Token: 0x0400728A RID: 29322
		Y
	}

	// Token: 0x02001002 RID: 4098
	public class DirectionManager
	{
		// Token: 0x060076FB RID: 30459 RVA: 0x00050EDC File Offset: 0x0004F0DC
		public DirectionManager()
		{
			this.Reset();
		}

		// Token: 0x060076FC RID: 30460 RVA: 0x00050F16 File Offset: 0x0004F116
		public void Reset()
		{
			this.up.Reset();
			this.down.Reset();
			this.left.Reset();
			this.right.Reset();
		}

		// Token: 0x0400728B RID: 29323
		public PlatformingLevelGroundMovementEnemy.DirectionManager.Hit up = new PlatformingLevelGroundMovementEnemy.DirectionManager.Hit();

		// Token: 0x0400728C RID: 29324
		public PlatformingLevelGroundMovementEnemy.DirectionManager.Hit down = new PlatformingLevelGroundMovementEnemy.DirectionManager.Hit();

		// Token: 0x0400728D RID: 29325
		public PlatformingLevelGroundMovementEnemy.DirectionManager.Hit left = new PlatformingLevelGroundMovementEnemy.DirectionManager.Hit();

		// Token: 0x0400728E RID: 29326
		public PlatformingLevelGroundMovementEnemy.DirectionManager.Hit right = new PlatformingLevelGroundMovementEnemy.DirectionManager.Hit();

		// Token: 0x020015DA RID: 5594
		public class Hit
		{
			// Token: 0x06008762 RID: 34658 RVA: 0x0005BA68 File Offset: 0x00059C68
			public Hit()
			{
				this.Reset();
			}

			// Token: 0x06008763 RID: 34659 RVA: 0x0005BA76 File Offset: 0x00059C76
			public Hit(bool able, Vector2 pos, GameObject gameObject, float distance)
			{
				this.able = able;
				this.pos = pos;
				this.gameObject = gameObject;
				this.distance = distance;
			}

			// Token: 0x06008764 RID: 34660 RVA: 0x0005BA9B File Offset: 0x00059C9B
			public void Reset()
			{
				this.able = true;
				this.pos = Vector2.zero;
				this.gameObject = null;
				this.distance = -1f;
			}

			// Token: 0x040091CD RID: 37325
			public bool able;

			// Token: 0x040091CE RID: 37326
			public Vector2 pos;

			// Token: 0x040091CF RID: 37327
			public GameObject gameObject;

			// Token: 0x040091D0 RID: 37328
			public float distance;
		}
	}

	// Token: 0x02001003 RID: 4099
	public class JumpManager
	{
		// Token: 0x0400728F RID: 29327
		public PlatformingLevelGroundMovementEnemy.JumpManager.State state;

		// Token: 0x04007290 RID: 29328
		public bool ableToLand;

		// Token: 0x020015DB RID: 5595
		public enum State
		{
			// Token: 0x040091D2 RID: 37330
			Ready,
			// Token: 0x040091D3 RID: 37331
			Used
		}
	}
}
