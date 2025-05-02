using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004FD RID: 1277
public class ArcadePlayerMotor : AbstractArcadePlayerComponent
{
	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x060034E9 RID: 13545 RVA: 0x0002B77B File Offset: 0x0002997B
	// (set) Token: 0x060034EA RID: 13546 RVA: 0x0002B783 File Offset: 0x00029983
	public Trilean2 LookDirection { get; set; }

	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x060034EB RID: 13547 RVA: 0x0002B78C File Offset: 0x0002998C
	// (set) Token: 0x060034EC RID: 13548 RVA: 0x0002B794 File Offset: 0x00029994
	public Trilean2 TrueLookDirection { get; set; }

	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x060034ED RID: 13549 RVA: 0x0002B79D File Offset: 0x0002999D
	// (set) Token: 0x060034EE RID: 13550 RVA: 0x0002B7A5 File Offset: 0x000299A5
	public Trilean2 MoveDirection { get; set; }

	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x060034EF RID: 13551 RVA: 0x0002B7AE File Offset: 0x000299AE
	public ArcadePlayerMotor.JumpManager.State JumpState
	{
		get
		{
			return this.jumpManager.state;
		}
	}

	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x060034F0 RID: 13552 RVA: 0x0002B7BB File Offset: 0x000299BB
	public bool Dashing
	{
		get
		{
			return this.dashManager.IsDashing;
		}
	}

	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x060034F1 RID: 13553 RVA: 0x0002B7C8 File Offset: 0x000299C8
	public int DashDirection
	{
		get
		{
			return this.dashManager.direction;
		}
	}

	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x060034F2 RID: 13554 RVA: 0x0002B7D5 File Offset: 0x000299D5
	// (set) Token: 0x060034F3 RID: 13555 RVA: 0x0002B7DD File Offset: 0x000299DD
	public bool Locked { get; set; }

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x060034F4 RID: 13556 RVA: 0x0002B7E6 File Offset: 0x000299E6
	// (set) Token: 0x060034F5 RID: 13557 RVA: 0x0002B7EE File Offset: 0x000299EE
	public bool Grounded { get; set; }

	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x060034F6 RID: 13558 RVA: 0x0002B7F7 File Offset: 0x000299F7
	// (set) Token: 0x060034F7 RID: 13559 RVA: 0x0002B7FF File Offset: 0x000299FF
	public bool Parrying { get; set; }

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x060034F8 RID: 13560 RVA: 0x0002B808 File Offset: 0x00029A08
	public bool IsHit
	{
		get
		{
			return this.hitManager.state == ArcadePlayerMotor.HitManager.State.Hit;
		}
	}

	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x060034F9 RID: 13561 RVA: 0x0002B818 File Offset: 0x00029A18
	public bool IsUsingSuperOrEx
	{
		get
		{
			return this.superManager.state == ArcadePlayerMotor.SuperManager.State.Super || this.superManager.state == ArcadePlayerMotor.SuperManager.State.Ex;
		}
	}

	// Token: 0x14000077 RID: 119
	// (add) Token: 0x060034FA RID: 13562 RVA: 0x000F7A7C File Offset: 0x000F5C7C
	// (remove) Token: 0x060034FB RID: 13563 RVA: 0x000F7AB4 File Offset: 0x000F5CB4
	public event Action OnGroundedEvent;

	// Token: 0x14000078 RID: 120
	// (add) Token: 0x060034FC RID: 13564 RVA: 0x000F7AEC File Offset: 0x000F5CEC
	// (remove) Token: 0x060034FD RID: 13565 RVA: 0x000F7B24 File Offset: 0x000F5D24
	public event Action OnJumpEvent;

	// Token: 0x14000079 RID: 121
	// (add) Token: 0x060034FE RID: 13566 RVA: 0x000F7B5C File Offset: 0x000F5D5C
	// (remove) Token: 0x060034FF RID: 13567 RVA: 0x000F7B94 File Offset: 0x000F5D94
	public event Action OnParryEvent;

	// Token: 0x1400007A RID: 122
	// (add) Token: 0x06003500 RID: 13568 RVA: 0x000F7BCC File Offset: 0x000F5DCC
	// (remove) Token: 0x06003501 RID: 13569 RVA: 0x000F7C04 File Offset: 0x000F5E04
	public event Action OnParrySuccess;

	// Token: 0x1400007B RID: 123
	// (add) Token: 0x06003502 RID: 13570 RVA: 0x000F7C3C File Offset: 0x000F5E3C
	// (remove) Token: 0x06003503 RID: 13571 RVA: 0x000F7C74 File Offset: 0x000F5E74
	public event Action OnHitEvent;

	// Token: 0x1400007C RID: 124
	// (add) Token: 0x06003504 RID: 13572 RVA: 0x000F7CAC File Offset: 0x000F5EAC
	// (remove) Token: 0x06003505 RID: 13573 RVA: 0x000F7CE4 File Offset: 0x000F5EE4
	public event Action OnDashStartEvent;

	// Token: 0x1400007D RID: 125
	// (add) Token: 0x06003506 RID: 13574 RVA: 0x000F7D1C File Offset: 0x000F5F1C
	// (remove) Token: 0x06003507 RID: 13575 RVA: 0x000F7D54 File Offset: 0x000F5F54
	public event Action OnDashEndEvent;

	// Token: 0x06003508 RID: 13576 RVA: 0x000F7D8C File Offset: 0x000F5F8C
	public override void OnAwake()
	{
		base.OnAwake();
		this.properties = new ArcadePlayerMotor.Properties();
		this.MoveDirection = new Trilean2(0, 0);
		this.LookDirection = new Trilean2(1, 0);
		this.TrueLookDirection = new Trilean2(1, 0);
		this.velocityManager = new ArcadePlayerMotor.VelocityManager(this, this.properties.maxSpeedY, this.properties.yEase);
		this.jumpManager = new ArcadePlayerMotor.JumpManager();
		this.dashManager = new ArcadePlayerMotor.DashManager();
		this.parryManager = new ArcadePlayerMotor.ParryManager();
		this.directionManager = new ArcadePlayerMotor.DirectionManager();
		this.platformManager = new ArcadePlayerMotor.PlatformManager(this);
		this.hitManager = new ArcadePlayerMotor.HitManager();
		this.superManager = new ArcadePlayerMotor.SuperManager();
		this.boundsManager = new ArcadePlayerMotor.BoundsManager(base.transform);
		base.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.allowInput = true;
		this.allowFalling = true;
	}

	// Token: 0x06003509 RID: 13577 RVA: 0x000F7E7C File Offset: 0x000F607C
	public void Start()
	{
		base.player.weaponManager.OnExStart += this.StartEx;
		base.player.weaponManager.OnSuperStart += this.StartSuper;
		base.player.weaponManager.OnExFire += this.OnExFired;
		base.player.weaponManager.OnSuperEnd += this.OnSuperEnd;
		base.player.weaponManager.OnExEnd += this.ResetSuperAndEx;
		base.player.weaponManager.OnSuperEnd += this.ResetSuperAndEx;
		base.player.OnReviveEvent += this.OnRevive;
	}

	// Token: 0x0600350A RID: 13578 RVA: 0x000F7F48 File Offset: 0x000F6148
	public void FixedUpdate()
	{
		if (base.player.IsDead)
		{
			return;
		}
		if (base.player.controlScheme != ArcadePlayerController.ControlScheme.Rocket)
		{
			this.HandleLooking();
		}
		if (base.player.weaponManager.FreezePosition)
		{
			return;
		}
		if (base.player.controlScheme == ArcadePlayerController.ControlScheme.Rocket)
		{
			this.RocketInput();
		}
		else
		{
			this.HandleInput();
			if (base.player.controlScheme == ArcadePlayerController.ControlScheme.Normal && this.allowFalling)
			{
				this.HandleFalling();
			}
			this.Move();
			this.HandleRaycasts();
			Vector2 vector = base.transform.localPosition;
			Vector2 v = vector - this.lastPositionFixed;
			v.x = (float)((int)v.x);
			v.y = (float)((int)v.y);
			this.MoveDirection = v;
			this.lastPositionFixed = new Vector2(vector.x, vector.y);
			this.lastPosition = base.transform.position;
		}
		this.ClampToBounds();
	}

	// Token: 0x0600350B RID: 13579 RVA: 0x0002B83C File Offset: 0x00029A3C
	public void LateUpdate()
	{
		this.ClampToBounds();
	}

	// Token: 0x0600350C RID: 13580 RVA: 0x0002B844 File Offset: 0x00029A44
	public void DisableInput()
	{
		this.allowInput = false;
		this.Locked = false;
		this.MoveDirection = new Trilean2(0, 0);
		this.velocityManager.move = 0f;
	}

	// Token: 0x0600350D RID: 13581 RVA: 0x0002B871 File Offset: 0x00029A71
	public void EnableInput()
	{
		this.allowInput = true;
	}

	// Token: 0x0600350E RID: 13582 RVA: 0x000F8064 File Offset: 0x000F6264
	public void DisableGravity()
	{
		this.allowFalling = false;
		this.MoveDirection = new Trilean2(this.MoveDirection.x, 0);
		this.velocityManager.y = 0f;
	}

	// Token: 0x0600350F RID: 13583 RVA: 0x0002B87A File Offset: 0x00029A7A
	public void EnableGravity()
	{
		this.allowFalling = true;
		this.velocityManager.y = 0f;
	}

	// Token: 0x06003510 RID: 13584 RVA: 0x0002B893 File Offset: 0x00029A93
	public RaycastHit2D BoxCast(Vector2 size, Vector2 direction, int layerMask)
	{
		return this.BoxCast(size, direction, layerMask, Vector2.zero);
	}

	// Token: 0x06003511 RID: 13585 RVA: 0x0002B8A3 File Offset: 0x00029AA3
	public RaycastHit2D BoxCast(Vector2 size, Vector2 direction, int layerMask, Vector2 offset)
	{
		return Physics2D.BoxCast(base.player.colliderManager.Center + offset, size, 0f, direction, 2000f, layerMask);
	}

	// Token: 0x06003512 RID: 13586 RVA: 0x0002B8CE File Offset: 0x00029ACE
	public RaycastHit2D CircleCast(float radius, Vector2 direction, int layerMask)
	{
		return Physics2D.CircleCast(base.player.colliderManager.Center, radius, direction, 2000f, layerMask);
	}

	// Token: 0x06003513 RID: 13587 RVA: 0x0002B8ED File Offset: 0x00029AED
	public bool DoesRaycastHitHaveCollider(RaycastHit2D hit)
	{
		return hit.collider != null;
	}

	// Token: 0x06003514 RID: 13588 RVA: 0x000F80A8 File Offset: 0x000F62A8
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (Application.isPlaying)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(base.player.center, 5f);
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(base.transform.position, 5f);
		}
	}

	// Token: 0x06003515 RID: 13589 RVA: 0x000F8104 File Offset: 0x000F6304
	public void HandleRaycasts()
	{
		bool flag = true;
		if (this.directionManager != null && this.directionManager.up != null)
		{
			flag = this.directionManager.up.able;
		}
		ArcadePlayerColliderManager colliderManager = base.player.colliderManager;
		this.directionManager.Reset();
		RaycastHit2D raycastHit = this.BoxCast(new Vector2(1f, colliderManager.Height), Vector2.left, this.wallMask);
		RaycastHit2D raycastHit2 = this.BoxCast(new Vector2(1f, colliderManager.Height), Vector2.right, this.wallMask);
		RaycastHit2D raycastHit3 = this.BoxCast(new Vector2(colliderManager.Width, 1f), Vector2.up, this.ceilingMask);
		this.RaycastObstacle(this.directionManager.left, raycastHit, base.player.colliderManager.DefaultWidth / 2f, ArcadePlayerMotor.RaycastAxis.X);
		this.RaycastObstacle(this.directionManager.right, raycastHit2, base.player.colliderManager.DefaultWidth / 2f, ArcadePlayerMotor.RaycastAxis.X);
		this.RaycastObstacle(this.directionManager.up, raycastHit3, base.player.colliderManager.Height / 2f, ArcadePlayerMotor.RaycastAxis.Y);
		Vector2 vector = colliderManager.Center + new Vector2(0f, colliderManager.DefaultHeight);
		RaycastHit2D[] array = Physics2D.BoxCastAll(vector, new Vector2(colliderManager.Width, 1f), 0f, Vector2.down, 1000f, this.groundMask);
		this.directionManager.down.pos = new Vector2(colliderManager.Center.x, -10000f);
		foreach (RaycastHit2D raycastHit2D in array)
		{
			if (raycastHit2D.point.y > this.directionManager.down.pos.y)
			{
				if (raycastHit2D.point.y <= 20f + base.transform.position.y)
				{
					float num = Math.Abs(base.transform.position.y - raycastHit2D.point.y);
					this.directionManager.down.pos = new Vector2(vector.x, raycastHit2D.point.y);
					this.directionManager.down.gameObject = raycastHit2D.collider.gameObject;
					this.directionManager.down.distance = num;
					if (num < 20f)
					{
						this.directionManager.down.able = false;
					}
					Debug.DrawLine(vector, this.directionManager.down.pos, Color.red);
				}
			}
		}
		if (!this.Grounded)
		{
			if (!this.directionManager.down.able)
			{
				this.OnGrounded();
				this.directionManager.left.able = true;
				this.directionManager.right.able = true;
			}
			if (!this.directionManager.up.able && this.directionManager.up.able != flag)
			{
				this.OnHitCeiling();
			}
		}
		float num2 = base.transform.position.y - this.directionManager.down.pos.y;
		if (this.Grounded && num2 > 30f)
		{
			this.LeaveGround();
		}
	}

	// Token: 0x06003516 RID: 13590 RVA: 0x000F84C8 File Offset: 0x000F66C8
	public float RaycastObstacle(ArcadePlayerMotor.DirectionManager.Hit directionProperties, RaycastHit2D raycastHit, float maxDistance, ArcadePlayerMotor.RaycastAxis axis)
	{
		if (!this.DoesRaycastHitHaveCollider(raycastHit))
		{
			return 1000f;
		}
		float num = (axis != ArcadePlayerMotor.RaycastAxis.X) ? Math.Abs(base.player.colliderManager.Center.y - raycastHit.point.y) : Math.Abs(base.player.colliderManager.Center.x - raycastHit.point.x);
		directionProperties.pos = raycastHit.point;
		directionProperties.gameObject = raycastHit.collider.gameObject;
		directionProperties.distance = num;
		if (num < maxDistance)
		{
			directionProperties.able = false;
		}
		return num;
	}

	// Token: 0x06003517 RID: 13591 RVA: 0x000F8584 File Offset: 0x000F6784
	public void OnGrounded()
	{
		if (this.Grounded || !this.jumpManager.ableToLand)
		{
			return;
		}
		if (this.platformManager.IsPlatformIgnored(this.directionManager.down.gameObject.transform))
		{
			return;
		}
		LevelPlatform component = this.directionManager.down.gameObject.GetComponent<LevelPlatform>();
		if (component != null)
		{
			if (component.canFallThrough && this.jumpManager.timeSinceDownJump < 0.1f)
			{
				return;
			}
			component.AddChild(base.transform);
		}
		this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Ready;
		this.parryManager.state = ArcadePlayerMotor.ParryManager.State.Ready;
		this.velocityManager.y = 0f;
		this.platformManager.ResetAll();
		this.Grounded = true;
		this.Parrying = false;
		this.dashManager.timeSinceGroundDash = 1000f;
		if (this.OnGroundedEvent != null)
		{
			this.OnGroundedEvent();
		}
	}

	// Token: 0x06003518 RID: 13592 RVA: 0x000F868C File Offset: 0x000F688C
	public void LeaveGround()
	{
		this.Grounded = false;
		this.jumpManager.ableToLand = false;
		this.velocityManager.y = 0f;
		this.ClearParent();
		if (this.jumpManager.state == ArcadePlayerMotor.JumpManager.State.Ready)
		{
			this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used;
		}
	}

	// Token: 0x06003519 RID: 13593 RVA: 0x000F86E0 File Offset: 0x000F68E0
	public void OnHitCeiling()
	{
		if (this.jumpManager.ableToLand)
		{
			return;
		}
		this.velocityManager.y = 0f;
		this.directionManager.left.able = true;
		this.directionManager.right.able = true;
	}

	// Token: 0x0600351A RID: 13594 RVA: 0x000F8730 File Offset: 0x000F6930
	public void Move()
	{
		this.velocityManager.Calculate();
		Vector3 vector = this.velocityManager.Total;
		if (this.hitManager.state != ArcadePlayerMotor.HitManager.State.Hit && this.superManager.state == ArcadePlayerMotor.SuperManager.State.Ready)
		{
			if (this.Grounded)
			{
				vector.x += this.velocityManager.GroundForce;
			}
			else
			{
				vector.x += this.velocityManager.AirForce;
			}
		}
		if (vector.x > 0f && !this.directionManager.right.able)
		{
			vector.x = 0f;
		}
		if (vector.x < 0f && !this.directionManager.left.able)
		{
			vector.x = 0f;
		}
		if (this.platformManager.OnPlatform)
		{
			if (!this.directionManager.right.able && this.MoveDirection.x > 0)
			{
				vector.x = 0f;
				base.transform.SetPosition(new float?(this.lastPosition.x), null, null);
			}
			if (!this.directionManager.left.able && this.MoveDirection.x < 0)
			{
				vector.x = 0f;
				base.transform.SetPosition(new float?(this.lastPosition.x), null, null);
			}
		}
		base.transform.localPosition += vector * CupheadTime.FixedDelta;
		if (this.Grounded)
		{
			Vector2 vector2 = base.transform.position;
			vector2.y = this.directionManager.down.pos.y;
			base.transform.position = vector2;
			LevelPlatform component = this.directionManager.down.gameObject.GetComponent<LevelPlatform>();
			if (component == null && base.transform.parent != null)
			{
				this.ClearParent();
			}
			else if (component != null && (base.transform.parent == null || component.gameObject != base.transform.parent.gameObject))
			{
				this.ClearParent();
				component.AddChild(base.transform);
			}
		}
	}

	// Token: 0x0600351B RID: 13595 RVA: 0x000F8A08 File Offset: 0x000F6C08
	public void ClampToBounds()
	{
		CupheadBounds cupheadBounds = new CupheadBounds();
		cupheadBounds.left = this.directionManager.left.pos.x + base.player.colliderManager.Width / 2f;
		cupheadBounds.right = this.directionManager.right.pos.x - base.player.colliderManager.Width / 2f;
		cupheadBounds.top = this.directionManager.up.pos.y - this.boundsManager.TopY;
		cupheadBounds.bottom = this.directionManager.down.pos.y - this.boundsManager.BottomY;
		Vector3 position = base.transform.position;
		if (!this.directionManager.left.able && base.transform.position.x < cupheadBounds.left)
		{
			position.x = cupheadBounds.left;
		}
		if (!this.directionManager.right.able && base.transform.position.x > cupheadBounds.right)
		{
			position.x = cupheadBounds.right;
		}
		if (!this.directionManager.up.able && base.transform.position.y > cupheadBounds.top)
		{
			position.y = cupheadBounds.top;
		}
		position.x = Mathf.Clamp(position.x, (float)Level.Current.Left + base.player.colliderManager.Width / 2f, (float)Level.Current.Right - base.player.colliderManager.Width / 2f);
		if (base.player.controlScheme != ArcadePlayerController.ControlScheme.Normal)
		{
			position.y = Mathf.Clamp(position.y, (float)Level.Current.Ground + base.player.colliderManager.Height / 2f, (float)Level.Current.Ceiling - base.player.colliderManager.Height / 2f);
		}
		base.transform.position = position;
	}

	// Token: 0x0600351C RID: 13596 RVA: 0x000F8C64 File Offset: 0x000F6E64
	public void ResetSuperAndEx()
	{
		if (this.superManager.state == ArcadePlayerMotor.SuperManager.State.Ready)
		{
			return;
		}
		if (this.jumpManager.state != ArcadePlayerMotor.JumpManager.State.Ready)
		{
			this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used;
		}
		base.StopCoroutine(this.exMove_cr());
		this.superManager.state = ArcadePlayerMotor.SuperManager.State.Ready;
		this.EnableInput();
		this.EnableGravity();
	}

	// Token: 0x0600351D RID: 13597 RVA: 0x0002B8FC File Offset: 0x00029AFC
	public void StartSuper()
	{
		this.LeaveGround();
		this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used;
		this.jumpManager.timer = 0f;
		this.velocityManager.y = 0f;
	}

	// Token: 0x0600351E RID: 13598 RVA: 0x0002B930 File Offset: 0x00029B30
	public void OnSuperEnd()
	{
		if (this.Grounded)
		{
			this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Ready;
		}
		else
		{
			this.LeaveGround();
			this.velocityManager.y = this.properties.superKnockUp;
		}
	}

	// Token: 0x0600351F RID: 13599 RVA: 0x0002B96A File Offset: 0x00029B6A
	public void StartEx()
	{
		this.exFirePose = base.player.weaponManager.GetDirectionPose();
		this.DisableInput();
		this.DisableGravity();
		this.superManager.state = ArcadePlayerMotor.SuperManager.State.Ex;
	}

	// Token: 0x06003520 RID: 13600 RVA: 0x0002B99A File Offset: 0x00029B9A
	public void OnExFired()
	{
		if (this.exFirePose == ArcadePlayerWeaponManager.Pose.Up || this.exFirePose == ArcadePlayerWeaponManager.Pose.Down)
		{
			base.StartCoroutine(this.exDelay_cr());
		}
		else
		{
			base.StartCoroutine(this.exMove_cr());
		}
	}

	// Token: 0x06003521 RID: 13601 RVA: 0x000F8CC4 File Offset: 0x000F6EC4
	public IEnumerator exDelay_cr()
	{
		while (this.superManager.state != ArcadePlayerMotor.SuperManager.State.Ready)
		{
			yield return null;
		}
		this.EnableInput();
		this.EnableGravity();
		this.superManager.state = ArcadePlayerMotor.SuperManager.State.Ready;
		yield break;
	}

	// Token: 0x06003522 RID: 13602 RVA: 0x000F8CE0 File Offset: 0x000F6EE0
	public IEnumerator exMove_cr()
	{
		while (this.superManager.state != ArcadePlayerMotor.SuperManager.State.Ready)
		{
			this.velocityManager.move = (float)(this.TrueLookDirection.x * -1) * this.properties.exKnockback;
			yield return null;
		}
		this.EnableInput();
		this.EnableGravity();
		this.superManager.state = ArcadePlayerMotor.SuperManager.State.Ready;
		yield break;
	}

	// Token: 0x06003523 RID: 13603 RVA: 0x000F8CFC File Offset: 0x000F6EFC
	public void HandleInput()
	{
		if (!base.player.levelStarted)
		{
			return;
		}
		this.timeSinceInputBuffered += CupheadTime.FixedDelta;
		this.dashManager.timeSinceGroundDash += CupheadTime.FixedDelta;
		if ((!this.allowInput || this.dashManager.state != ArcadePlayerMotor.DashManager.State.Ready) && this.hitManager.state == ArcadePlayerMotor.HitManager.State.Inactive)
		{
			this.BufferInputs();
		}
		if (!this.allowInput)
		{
			return;
		}
		if (!this.HandleDash())
		{
			if (this.hitManager.state == ArcadePlayerMotor.HitManager.State.Hit)
			{
				this.HandleHit();
			}
			else
			{
				if (base.player.controlScheme == ArcadePlayerController.ControlScheme.Normal)
				{
					this.HandleParry();
					this.HandleJumping();
				}
				else if (base.player.controlScheme == ArcadePlayerController.ControlScheme.Jetpack)
				{
					this.HandleJetpackJump();
				}
				this.HandleWalking();
			}
		}
	}

	// Token: 0x06003524 RID: 13604 RVA: 0x0002B9D3 File Offset: 0x00029BD3
	public void BufferInput(ArcadePlayerMotor.BufferedInput input)
	{
		this.bufferedInput = input;
		this.timeSinceInputBuffered = 0f;
	}

	// Token: 0x06003525 RID: 13605 RVA: 0x000F8DE8 File Offset: 0x000F6FE8
	public void BufferInputs()
	{
		if (base.player.input.actions.GetButtonDown(2))
		{
			this.BufferInput(ArcadePlayerMotor.BufferedInput.Jump);
		}
		else if (base.player.input.actions.GetButtonDown(7) && !this.dashManager.IsDashing)
		{
			this.BufferInput(ArcadePlayerMotor.BufferedInput.Dash);
		}
		else if (base.player.input.actions.GetButtonDown(4))
		{
			this.BufferInput(ArcadePlayerMotor.BufferedInput.Super);
		}
	}

	// Token: 0x06003526 RID: 13606 RVA: 0x0002B9E7 File Offset: 0x00029BE7
	public void ClearBufferedInput()
	{
		this.timeSinceInputBuffered = 0.134f;
	}

	// Token: 0x06003527 RID: 13607 RVA: 0x0002B9F4 File Offset: 0x00029BF4
	public bool HasBufferedInput(ArcadePlayerMotor.BufferedInput input)
	{
		return this.bufferedInput == input && this.timeSinceInputBuffered < 0.134f;
	}

	// Token: 0x06003528 RID: 13608 RVA: 0x000F8E78 File Offset: 0x000F7078
	public void HandleJumping()
	{
		if (this.jumpManager.state == ArcadePlayerMotor.JumpManager.State.Ready && (base.player.input.actions.GetButtonDown(2) || this.HasBufferedInput(ArcadePlayerMotor.BufferedInput.Jump)))
		{
			if (this.LookDirection.y < 0 && this.Grounded && base.transform.parent != null)
			{
				LevelPlatform component = base.transform.parent.GetComponent<LevelPlatform>();
				if (component.canFallThrough)
				{
					this.platformManager.Ignore(base.transform.parent);
					this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used;
					this.LeaveGround();
					this.jumpManager.timeSinceDownJump = 0f;
					return;
				}
			}
			AudioManager.Play("player_jump");
			this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Hold;
			this.LeaveGround();
			this.velocityManager.y = this.properties.jumpPower;
			this.jumpManager.timer = CupheadTime.FixedDelta;
			if (this.OnJumpEvent != null)
			{
				this.OnJumpEvent();
			}
		}
		if (this.jumpManager.state == ArcadePlayerMotor.JumpManager.State.Hold)
		{
			if (!this.directionManager.up.able || (this.jumpManager.timer >= this.properties.jumpHoldMin && (base.player.input.actions.GetButtonUp(2) || !base.player.input.actions.GetButton(2))) || this.jumpManager.timer >= this.properties.jumpHoldMax)
			{
				this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used;
				this.jumpManager.timer = 0f;
			}
			this.velocityManager.y = this.properties.jumpPower;
			this.jumpManager.timer += CupheadTime.FixedDelta;
		}
		this.jumpManager.timeSinceDownJump += CupheadTime.FixedDelta;
	}

	// Token: 0x06003529 RID: 13609 RVA: 0x000F9098 File Offset: 0x000F7298
	public void HandleParry()
	{
		if (this.IsHit)
		{
			return;
		}
		if (this.parryManager.state == ArcadePlayerMotor.ParryManager.State.Ready && (base.player.input.actions.GetButtonDown(2) || this.HasBufferedInput(ArcadePlayerMotor.BufferedInput.Jump)) && this.jumpManager.state != ArcadePlayerMotor.JumpManager.State.Ready && !this.IsHit)
		{
			this.hitManager.state = ArcadePlayerMotor.HitManager.State.Inactive;
			this.parryManager.state = ArcadePlayerMotor.ParryManager.State.NotReady;
			if (this.dashManager.IsDashing)
			{
				this.dashManager.state = ArcadePlayerMotor.DashManager.State.End;
			}
			this.Parrying = true;
			if (this.OnParryEvent != null)
			{
				this.OnParryEvent();
			}
		}
	}

	// Token: 0x0600352A RID: 13610 RVA: 0x0002BA12 File Offset: 0x00029C12
	public void OnParryComplete()
	{
		this.LeaveGround();
		this.parryManager.state = ArcadePlayerMotor.ParryManager.State.Ready;
		this.velocityManager.y = this.properties.parryPower;
		if (this.OnParrySuccess != null)
		{
			this.OnParrySuccess();
		}
	}

	// Token: 0x0600352B RID: 13611 RVA: 0x0002BA52 File Offset: 0x00029C52
	public void OnParryHit()
	{
		base.StartCoroutine(this.parryHit_cr());
	}

	// Token: 0x0600352C RID: 13612 RVA: 0x0002BA61 File Offset: 0x00029C61
	public void OnParryCanceled()
	{
		this.Parrying = false;
	}

	// Token: 0x0600352D RID: 13613 RVA: 0x0002BA6A File Offset: 0x00029C6A
	public void OnParryAnimEnd()
	{
		this.Parrying = false;
	}

	// Token: 0x0600352E RID: 13614 RVA: 0x000F9154 File Offset: 0x000F7354
	public bool HandleDash()
	{
		if (this.dashManager.state == ArcadePlayerMotor.DashManager.State.Ready && (!this.Grounded || this.dashManager.timeSinceGroundDash > 0.1f) && (base.player.input.actions.GetButtonDown(7) || this.HasBufferedInput(ArcadePlayerMotor.BufferedInput.Dash)))
		{
			AudioManager.Play("player_dash");
			this.dashManager.state = ArcadePlayerMotor.DashManager.State.Start;
			this.dashManager.direction = this.TrueLookDirection.x;
			if (this.jumpManager.state == ArcadePlayerMotor.JumpManager.State.Hold)
			{
				this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used;
			}
			if (this.OnDashStartEvent != null)
			{
				this.OnDashStartEvent();
			}
			this.velocityManager.move = 0f;
			return true;
		}
		if (this.dashManager.state == ArcadePlayerMotor.DashManager.State.Start)
		{
			this.dashManager.state = ArcadePlayerMotor.DashManager.State.Dashing;
		}
		if (this.dashManager.state == ArcadePlayerMotor.DashManager.State.Dashing)
		{
			this.velocityManager.dash = this.properties.dashSpeed * (float)this.dashManager.direction;
			this.dashManager.timer += CupheadTime.FixedDelta;
			this.velocityManager.y = 0f;
			this.LookDirection = new Trilean2(this.LookDirection.x, this.dashManager.direction);
			if (this.dashManager.timer >= this.properties.dashTime)
			{
				this.DashComplete();
			}
			if (!this.Grounded)
			{
				this.jumpManager.ableToLand = true;
			}
			return true;
		}
		if (this.dashManager.state == ArcadePlayerMotor.DashManager.State.End)
		{
			if (this.Grounded)
			{
				this.dashManager.state = ArcadePlayerMotor.DashManager.State.Ready;
				if (this.dashManager.groundDash)
				{
					this.dashManager.timeSinceGroundDash = 0f;
				}
			}
			else
			{
				this.dashManager.groundDash = false;
			}
		}
		return false;
	}

	// Token: 0x0600352F RID: 13615 RVA: 0x000F9368 File Offset: 0x000F7568
	public void DashComplete()
	{
		this.dashManager.state = ArcadePlayerMotor.DashManager.State.End;
		this.dashManager.timer = 0f;
		this.velocityManager.dash = 0f;
		if (this.OnDashEndEvent != null)
		{
			this.OnDashEndEvent();
		}
	}

	// Token: 0x06003530 RID: 13616 RVA: 0x0002BA73 File Offset: 0x00029C73
	public void HandleLocked()
	{
		if (base.player.input.actions.GetButton(6) && this.Grounded)
		{
			this.Locked = true;
		}
		else
		{
			this.Locked = false;
		}
	}

	// Token: 0x06003531 RID: 13617 RVA: 0x000F93B8 File Offset: 0x000F75B8
	public void HandleWalking()
	{
		float num = (base.player.controlScheme != ArcadePlayerController.ControlScheme.Normal) ? this.properties.jetpackMoveSpeed : this.properties.moveSpeed;
		float move = (float)base.player.input.GetAxisInt(PlayerInput.Axis.X, false, false) * num;
		this.velocityManager.move = move;
	}

	// Token: 0x06003532 RID: 13618 RVA: 0x000F9414 File Offset: 0x000F7614
	public void HandleLooking()
	{
		if (base.player.levelStarted && this.allowInput)
		{
			int axisInt = base.player.input.GetAxisInt(PlayerInput.Axis.X, false, false);
			int axisInt2 = base.player.input.GetAxisInt(PlayerInput.Axis.Y, false, false);
			this.LookDirection = new Trilean2(axisInt, axisInt2);
		}
		int x = this.TrueLookDirection.x;
		int y = this.TrueLookDirection.y;
		if (this.LookDirection.x != 0)
		{
			x = this.LookDirection.x;
		}
		y = this.LookDirection.y;
		this.TrueLookDirection = new Trilean2(x, y);
	}

	// Token: 0x06003533 RID: 13619 RVA: 0x000F94F0 File Offset: 0x000F76F0
	public void HandleFalling()
	{
		if (this.Grounded || this.dashManager.IsDashing)
		{
			return;
		}
		if (Level.Current.LevelTime < 0.2f)
		{
			return;
		}
		float num = this.properties.timeToMaxY * 60f;
		float num2 = this.properties.maxSpeedY / num * CupheadTime.FixedDelta;
		this.velocityManager.y += num2;
		this.jumpManager.ableToLand = (this.velocityManager.y > 0f);
	}

	// Token: 0x06003534 RID: 13620 RVA: 0x000F9584 File Offset: 0x000F7784
	public float GetTimeUntilLand()
	{
		if (this.Grounded)
		{
			return 0f;
		}
		float num = this.properties.timeToMaxY * 60f;
		float num2 = this.properties.maxSpeedY / num;
		float num3 = ((float)Level.Current.Ground - base.transform.position.y) / (this.velocityManager.maxY * 2f);
		return -(this.velocityManager.y - Mathf.Sqrt(this.velocityManager.y * this.velocityManager.y - 2f * num2 * num3)) / num2;
	}

	// Token: 0x06003535 RID: 13621 RVA: 0x0002BAAE File Offset: 0x00029CAE
	public float GetTimeUntilDashEnd()
	{
		if (!this.Dashing)
		{
			return 0f;
		}
		return this.properties.dashTime - this.dashManager.timer;
	}

	// Token: 0x06003536 RID: 13622 RVA: 0x000F962C File Offset: 0x000F782C
	public void HandleHit()
	{
		if (this.hitManager.state != ArcadePlayerMotor.HitManager.State.Hit)
		{
			return;
		}
		if (this.hitManager.timer > this.properties.hitStunTime)
		{
			this.hitManager.state = ArcadePlayerMotor.HitManager.State.Inactive;
			this.velocityManager.hit = 0f;
		}
		else
		{
			float value = this.hitManager.timer / this.properties.hitStunTime;
			this.velocityManager.hit = EaseUtils.Ease(this.properties.hitKnockbackEase, this.properties.hitKnockbackPower, 0f, value) * (float)this.hitManager.direction;
			this.hitManager.timer += CupheadTime.FixedDelta;
		}
	}

	// Token: 0x06003537 RID: 13623 RVA: 0x000F96F0 File Offset: 0x000F78F0
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hitManager.state = ArcadePlayerMotor.HitManager.State.Hit;
		if (this.OnHitEvent != null)
		{
			this.OnHitEvent();
		}
		this.DashComplete();
		this.velocityManager.Clear();
		this.ResetSuperAndEx();
		int direction = this.TrueLookDirection.x * -1;
		this.hitManager.direction = direction;
		this.LeaveGround();
		this.velocityManager.y = this.properties.hitJumpPower;
		this.hitManager.timer = 0f;
	}

	// Token: 0x06003538 RID: 13624 RVA: 0x000F9784 File Offset: 0x000F7984
	public void OnRevive(Vector3 pos)
	{
		base.transform.position = pos;
		this.hitManager.state = ArcadePlayerMotor.HitManager.State.KnockedUp;
		this.DashComplete();
		this.velocityManager.Clear();
		this.ResetSuperAndEx();
		this.hitManager.direction = 0;
		this.LeaveGround();
		this.velocityManager.y = this.properties.reviveKnockUpPower;
		this.hitManager.timer = 0f;
	}

	// Token: 0x06003539 RID: 13625 RVA: 0x000F97F8 File Offset: 0x000F79F8
	public void RocketInput()
	{
		this.HandleRocketRotation();
		this.HandleRocketBoost();
		if (!this.HandleDash() && this.hitManager.state == ArcadePlayerMotor.HitManager.State.Hit)
		{
			this.HandleHit();
		}
	}

	// Token: 0x0600353A RID: 13626 RVA: 0x000F9838 File Offset: 0x000F7A38
	public void HandleJetpackJump()
	{
		if (base.player.input.actions.GetButtonDown(2))
		{
			this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Hold;
			this.LeaveGround();
			this.velocityManager.y = this.properties.jetpackAcceleration;
			this.jumpManager.timer = CupheadTime.FixedDelta;
		}
		else if (this.velocityManager.y < this.properties.jetpackGravityMax)
		{
			this.velocityManager.y += this.properties.jetpackGravity;
		}
	}

	// Token: 0x0600353B RID: 13627 RVA: 0x000F98D8 File Offset: 0x000F7AD8
	public void HandleRocketBoost()
	{
		if (base.player.input.actions.GetButton(2))
		{
			if (this.rocketSpeed < this.properties.moveSpeed)
			{
				this.rocketSpeed += this.properties.rocketAcceleration;
			}
			else
			{
				this.rocketSpeed = this.properties.moveSpeed;
			}
		}
		else if (this.rocketSpeed > 0f)
		{
			this.rocketSpeed -= this.properties.rocketAcceleration;
		}
		else
		{
			this.rocketSpeed = 0f;
		}
		base.transform.position += base.transform.up.normalized * this.rocketSpeed * CupheadTime.FixedDelta;
	}

	// Token: 0x0600353C RID: 13628 RVA: 0x0002BAD8 File Offset: 0x00029CD8
	public void HandleRocketRotation()
	{
		base.transform.Rotate(0f, 0f, this.properties.rocketRotation * (float)(-(float)base.player.input.GetAxisInt(PlayerInput.Axis.X, false, false)) * CupheadTime.FixedDelta, 1);
	}

	// Token: 0x0600353D RID: 13629 RVA: 0x0002BB17 File Offset: 0x00029D17
	public void AddForce(ArcadePlayerMotor.VelocityManager.Force force)
	{
		this.velocityManager.AddForce(force);
	}

	// Token: 0x0600353E RID: 13630 RVA: 0x0002BB25 File Offset: 0x00029D25
	public void RemoveForce(ArcadePlayerMotor.VelocityManager.Force force)
	{
		this.velocityManager.RemoveForce(force);
	}

	// Token: 0x0600353F RID: 13631 RVA: 0x0002BB33 File Offset: 0x00029D33
	public void ClearParent()
	{
		if (base.transform.parent != null)
		{
			base.transform.parent.GetComponent<LevelPlatform>().OnPlayerExit(base.transform);
		}
		base.transform.parent = null;
	}

	// Token: 0x06003540 RID: 13632 RVA: 0x000F99C0 File Offset: 0x000F7BC0
	public IEnumerator parryHit_cr()
	{
		CupheadTime.GlobalSpeed = 1f;
		this.velocityManager.Clear();
		yield return null;
		PauseManager.Unpause();
		this.velocityManager.Clear();
		CupheadTime.GlobalSpeed = 1f;
		yield break;
	}

	// Token: 0x04002B53 RID: 11091
	[SerializeField]
	public ArcadePlayerMotor.Properties properties;

	// Token: 0x04002B54 RID: 11092
	public Vector2 lastPositionFixed;

	// Token: 0x04002B55 RID: 11093
	public Vector2 lastPosition;

	// Token: 0x04002B56 RID: 11094
	public ArcadePlayerMotor.VelocityManager velocityManager;

	// Token: 0x04002B57 RID: 11095
	public ArcadePlayerMotor.JumpManager jumpManager;

	// Token: 0x04002B58 RID: 11096
	public ArcadePlayerMotor.DashManager dashManager;

	// Token: 0x04002B59 RID: 11097
	public ArcadePlayerMotor.ParryManager parryManager;

	// Token: 0x04002B5A RID: 11098
	public ArcadePlayerMotor.DirectionManager directionManager;

	// Token: 0x04002B5B RID: 11099
	public ArcadePlayerMotor.PlatformManager platformManager;

	// Token: 0x04002B5C RID: 11100
	public ArcadePlayerMotor.HitManager hitManager;

	// Token: 0x04002B5D RID: 11101
	public ArcadePlayerMotor.SuperManager superManager;

	// Token: 0x04002B5E RID: 11102
	public ArcadePlayerMotor.BoundsManager boundsManager;

	// Token: 0x04002B5F RID: 11103
	public bool allowInput;

	// Token: 0x04002B60 RID: 11104
	public bool allowFalling;

	// Token: 0x04002B61 RID: 11105
	public float rocketSpeed;

	// Token: 0x04002B69 RID: 11113
	public const float RAY_DISTANCE = 2000f;

	// Token: 0x04002B6A RID: 11114
	public const float MAX_GROUNDED_FALL_DISTANCE = 30f;

	// Token: 0x04002B6B RID: 11115
	public readonly int wallMask = 262144;

	// Token: 0x04002B6C RID: 11116
	public readonly int ceilingMask = 524288;

	// Token: 0x04002B6D RID: 11117
	public readonly int groundMask = 1048576;

	// Token: 0x04002B6E RID: 11118
	public ArcadePlayerWeaponManager.Pose exFirePose;

	// Token: 0x04002B6F RID: 11119
	public ArcadePlayerMotor.BufferedInput bufferedInput;

	// Token: 0x04002B70 RID: 11120
	public float timeSinceInputBuffered = 0.134f;

	// Token: 0x0200115C RID: 4444
	public enum RaycastAxis
	{
		// Token: 0x04007A0F RID: 31247
		X,
		// Token: 0x04007A10 RID: 31248
		Y
	}

	// Token: 0x0200115D RID: 4445
	public enum BufferedInput
	{
		// Token: 0x04007A12 RID: 31250
		Jump,
		// Token: 0x04007A13 RID: 31251
		Dash,
		// Token: 0x04007A14 RID: 31252
		Super
	}

	// Token: 0x0200115E RID: 4446
	public class Properties
	{
		// Token: 0x04007A15 RID: 31253
		public float rocketRotation = 300f;

		// Token: 0x04007A16 RID: 31254
		public float rocketMaxSpeed = 400f;

		// Token: 0x04007A17 RID: 31255
		public float rocketAcceleration = 2.5f;

		// Token: 0x04007A18 RID: 31256
		public float jetpackAcceleration = -0.1f;

		// Token: 0x04007A19 RID: 31257
		public float jetpackGravity = 0.001f;

		// Token: 0x04007A1A RID: 31258
		public float jetpackGravityMax = 0.1f;

		// Token: 0x04007A1B RID: 31259
		public const float speedScale = 0.75f;

		// Token: 0x04007A1C RID: 31260
		public float moveSpeed = 367.5f;

		// Token: 0x04007A1D RID: 31261
		public float jetpackMoveSpeed = 187.5f;

		// Token: 0x04007A1E RID: 31262
		public float maxSpeedY = 1215f;

		// Token: 0x04007A1F RID: 31263
		public float timeToMaxY = 7.3f;

		// Token: 0x04007A20 RID: 31264
		public EaseUtils.EaseType yEase = EaseUtils.EaseType.linear;

		// Token: 0x04007A21 RID: 31265
		public float jumpHoldMin = 0.01f;

		// Token: 0x04007A22 RID: 31266
		public float jumpHoldMax = 0.16f;

		// Token: 0x04007A23 RID: 31267
		[Range(0f, -1f)]
		public float jumpPower = -0.566249967f;

		// Token: 0x04007A24 RID: 31268
		public float dashSpeed = 825f;

		// Token: 0x04007A25 RID: 31269
		public float dashTime = 0.3f;

		// Token: 0x04007A26 RID: 31270
		public float dashEndTime = 0.21f;

		// Token: 0x04007A27 RID: 31271
		public EaseUtils.EaseType dashEase = EaseUtils.EaseType.easeOutSine;

		// Token: 0x04007A28 RID: 31272
		public float platformIgnoreTime = 1f;

		// Token: 0x04007A29 RID: 31273
		public float hitStunTime = 0.3f;

		// Token: 0x04007A2A RID: 31274
		public float hitFalloff = 0.25f;

		// Token: 0x04007A2B RID: 31275
		[Range(0f, -1f)]
		public float hitJumpPower = -0.6f;

		// Token: 0x04007A2C RID: 31276
		public float hitKnockbackPower = 225f;

		// Token: 0x04007A2D RID: 31277
		public EaseUtils.EaseType hitKnockbackEase = EaseUtils.EaseType.linear;

		// Token: 0x04007A2E RID: 31278
		public float knockUpStunTime = 0.2f;

		// Token: 0x04007A2F RID: 31279
		public float parryPower = -0.75f;

		// Token: 0x04007A30 RID: 31280
		public float deathSpeed = 3.75f;

		// Token: 0x04007A31 RID: 31281
		public float reviveKnockUpPower = -0.75f;

		// Token: 0x04007A32 RID: 31282
		public float exKnockback = 172.5f;

		// Token: 0x04007A33 RID: 31283
		public float superKnockUp = -0.450000018f;
	}

	// Token: 0x0200115F RID: 4447
	public class VelocityManager
	{
		// Token: 0x06007D64 RID: 32100 RVA: 0x000542B7 File Offset: 0x000524B7
		public VelocityManager(ArcadePlayerMotor motor, float maxY, EaseUtils.EaseType yEase)
		{
			this.maxY = maxY;
			this.yEase = yEase;
			this.forces = new List<ArcadePlayerMotor.VelocityManager.Force>();
		}

		// Token: 0x170017E3 RID: 6115
		// (get) Token: 0x06007D65 RID: 32101 RVA: 0x000542D8 File Offset: 0x000524D8
		// (set) Token: 0x06007D66 RID: 32102 RVA: 0x000542E0 File Offset: 0x000524E0
		public float GroundForce { get; set; }

		// Token: 0x170017E4 RID: 6116
		// (get) Token: 0x06007D67 RID: 32103 RVA: 0x000542E9 File Offset: 0x000524E9
		// (set) Token: 0x06007D68 RID: 32104 RVA: 0x000542F1 File Offset: 0x000524F1
		public float AirForce { get; set; }

		// Token: 0x170017E5 RID: 6117
		// (get) Token: 0x06007D69 RID: 32105 RVA: 0x000542FA File Offset: 0x000524FA
		// (set) Token: 0x06007D6A RID: 32106 RVA: 0x0005431D File Offset: 0x0005251D
		public float y
		{
			get
			{
				this._y = Mathf.Clamp(this._y, -10f, 1f);
				return this._y;
			}
			set
			{
				this._y = Mathf.Clamp(value, -10f, 1f);
			}
		}

		// Token: 0x06007D6B RID: 32107 RVA: 0x0028C79C File Offset: 0x0028A99C
		public void Calculate()
		{
			this.GroundForce = 0f;
			this.AirForce = 0f;
			foreach (ArcadePlayerMotor.VelocityManager.Force force in this.forces)
			{
				if (force.enabled)
				{
					ArcadePlayerMotor.VelocityManager.Force.Type type = force.type;
					if (type != ArcadePlayerMotor.VelocityManager.Force.Type.All)
					{
						if (type != ArcadePlayerMotor.VelocityManager.Force.Type.Air)
						{
							if (type == ArcadePlayerMotor.VelocityManager.Force.Type.Ground)
							{
								this.GroundForce += force.value;
							}
						}
						else
						{
							this.AirForce += force.value;
						}
					}
					else
					{
						this.AirForce += force.value;
						this.GroundForce += force.value;
					}
				}
			}
		}

		// Token: 0x170017E6 RID: 6118
		// (get) Token: 0x06007D6C RID: 32108 RVA: 0x0028C894 File Offset: 0x0028AA94
		public Vector2 Total
		{
			get
			{
				float value = this.y / 2f + 0.5f;
				Vector2 result = default(Vector2);
				result.y = EaseUtils.Ease(this.yEase, this.maxY, -this.maxY, value);
				result.x += this.move + this.dash + this.hit;
				return result;
			}
		}

		// Token: 0x06007D6D RID: 32109 RVA: 0x00054335 File Offset: 0x00052535
		public void Clear()
		{
			this.move = 0f;
			this.dash = 0f;
			this.hit = 0f;
			this.y = 0f;
		}

		// Token: 0x06007D6E RID: 32110 RVA: 0x00054363 File Offset: 0x00052563
		public void AddForce(ArcadePlayerMotor.VelocityManager.Force force)
		{
			if (this.forces.Contains(force))
			{
				return;
			}
			this.forces.Add(force);
		}

		// Token: 0x06007D6F RID: 32111 RVA: 0x00054383 File Offset: 0x00052583
		public void RemoveForce(ArcadePlayerMotor.VelocityManager.Force force)
		{
			if (this.forces.Contains(force))
			{
				this.forces.Remove(force);
			}
		}

		// Token: 0x04007A36 RID: 31286
		public float move;

		// Token: 0x04007A37 RID: 31287
		public float dash;

		// Token: 0x04007A38 RID: 31288
		public float hit;

		// Token: 0x04007A39 RID: 31289
		public List<ArcadePlayerMotor.VelocityManager.Force> forces;

		// Token: 0x04007A3A RID: 31290
		public EaseUtils.EaseType yEase;

		// Token: 0x04007A3B RID: 31291
		public float maxY;

		// Token: 0x04007A3C RID: 31292
		public float _y;

		// Token: 0x020015E1 RID: 5601
		public class Force
		{
			// Token: 0x0600876B RID: 34667 RVA: 0x0005BAE3 File Offset: 0x00059CE3
			public Force()
			{
				this.type = ArcadePlayerMotor.VelocityManager.Force.Type.All;
				this.value = 0f;
			}

			// Token: 0x0600876C RID: 34668 RVA: 0x0005BB04 File Offset: 0x00059D04
			public Force(ArcadePlayerMotor.VelocityManager.Force.Type type)
			{
				this.type = type;
				this.value = 0f;
			}

			// Token: 0x0600876D RID: 34669 RVA: 0x0005BB25 File Offset: 0x00059D25
			public Force(ArcadePlayerMotor.VelocityManager.Force.Type type, float force)
			{
				this.type = type;
				this.value = force;
			}

			// Token: 0x040091E0 RID: 37344
			public bool enabled = true;

			// Token: 0x040091E1 RID: 37345
			public readonly ArcadePlayerMotor.VelocityManager.Force.Type type;

			// Token: 0x040091E2 RID: 37346
			public float value;

			// Token: 0x0200160A RID: 5642
			public enum Type
			{
				// Token: 0x04009299 RID: 37529
				All,
				// Token: 0x0400929A RID: 37530
				Ground,
				// Token: 0x0400929B RID: 37531
				Air
			}
		}
	}

	// Token: 0x02001160 RID: 4448
	public class JumpManager
	{
		// Token: 0x04007A3D RID: 31293
		public ArcadePlayerMotor.JumpManager.State state;

		// Token: 0x04007A3E RID: 31294
		public float timer;

		// Token: 0x04007A3F RID: 31295
		public float timeSinceDownJump = 1000f;

		// Token: 0x04007A40 RID: 31296
		public bool ableToLand;

		// Token: 0x020015E2 RID: 5602
		public enum State
		{
			// Token: 0x040091E4 RID: 37348
			Ready,
			// Token: 0x040091E5 RID: 37349
			Hold,
			// Token: 0x040091E6 RID: 37350
			Used
		}
	}

	// Token: 0x02001161 RID: 4449
	public class DashManager
	{
		// Token: 0x170017E7 RID: 6119
		// (get) Token: 0x06007D72 RID: 32114 RVA: 0x0028C900 File Offset: 0x0028AB00
		public bool IsDashing
		{
			get
			{
				ArcadePlayerMotor.DashManager.State state = this.state;
				return state == ArcadePlayerMotor.DashManager.State.Start || state == ArcadePlayerMotor.DashManager.State.Dashing || state == ArcadePlayerMotor.DashManager.State.Ending;
			}
		}

		// Token: 0x04007A41 RID: 31297
		public ArcadePlayerMotor.DashManager.State state;

		// Token: 0x04007A42 RID: 31298
		public int direction;

		// Token: 0x04007A43 RID: 31299
		public float timer;

		// Token: 0x04007A44 RID: 31300
		public const float DASH_COOLDOWN_DURATION = 0.1f;

		// Token: 0x04007A45 RID: 31301
		public float timeSinceGroundDash = 0.1f;

		// Token: 0x04007A46 RID: 31302
		public bool groundDash;

		// Token: 0x020015E3 RID: 5603
		public enum State
		{
			// Token: 0x040091E8 RID: 37352
			Ready,
			// Token: 0x040091E9 RID: 37353
			Start,
			// Token: 0x040091EA RID: 37354
			Dashing,
			// Token: 0x040091EB RID: 37355
			Ending,
			// Token: 0x040091EC RID: 37356
			End
		}
	}

	// Token: 0x02001162 RID: 4450
	public class ParryManager
	{
		// Token: 0x04007A47 RID: 31303
		public ArcadePlayerMotor.ParryManager.State state;

		// Token: 0x020015E4 RID: 5604
		public enum State
		{
			// Token: 0x040091EE RID: 37358
			Ready,
			// Token: 0x040091EF RID: 37359
			NotReady
		}
	}

	// Token: 0x02001163 RID: 4451
	public class PlatformManager
	{
		// Token: 0x06007D74 RID: 32116 RVA: 0x000543D1 File Offset: 0x000525D1
		public PlatformManager(ArcadePlayerMotor motor)
		{
			this.ignoredPlatforms = new List<Transform>();
			this.motor = motor;
		}

		// Token: 0x170017E8 RID: 6120
		// (get) Token: 0x06007D75 RID: 32117 RVA: 0x000543EB File Offset: 0x000525EB
		public bool OnPlatform
		{
			get
			{
				return this.motor.transform.parent != null;
			}
		}

		// Token: 0x06007D76 RID: 32118 RVA: 0x00054403 File Offset: 0x00052603
		public void Ignore(Transform platform)
		{
			this.StopCoroutine();
			this.ignoreCoroutine = this.ignorePlatform_cr(platform);
			this.motor.StartCoroutine(this.ignoreCoroutine);
		}

		// Token: 0x06007D77 RID: 32119 RVA: 0x0005442A File Offset: 0x0005262A
		public void StopCoroutine()
		{
			if (this.ignoreCoroutine != null)
			{
				this.motor.StopCoroutine(this.ignoreCoroutine);
			}
			this.ignoreCoroutine = null;
		}

		// Token: 0x06007D78 RID: 32120 RVA: 0x0005444F File Offset: 0x0005264F
		public void Add(Transform platform)
		{
			this.ignoredPlatforms.Add(platform);
		}

		// Token: 0x06007D79 RID: 32121 RVA: 0x0005445D File Offset: 0x0005265D
		public void Remove(Transform platform)
		{
			this.ignoredPlatforms.Remove(platform);
		}

		// Token: 0x06007D7A RID: 32122 RVA: 0x0005446C File Offset: 0x0005266C
		public bool IsPlatformIgnored(Transform platform)
		{
			return this.ignoredPlatforms.Contains(platform);
		}

		// Token: 0x06007D7B RID: 32123 RVA: 0x0005447A File Offset: 0x0005267A
		public void ResetAll()
		{
			this.StopCoroutine();
			this.ignoredPlatforms = new List<Transform>();
		}

		// Token: 0x06007D7C RID: 32124 RVA: 0x0028C934 File Offset: 0x0028AB34
		public IEnumerator ignorePlatform_cr(Transform platform)
		{
			this.Add(platform);
			yield return CupheadTime.WaitForSeconds(this.motor, this.motor.properties.platformIgnoreTime);
			this.Remove(platform);
			yield break;
		}

		// Token: 0x04007A48 RID: 31304
		public List<Transform> ignoredPlatforms;

		// Token: 0x04007A49 RID: 31305
		public ArcadePlayerMotor motor;

		// Token: 0x04007A4A RID: 31306
		public IEnumerator ignoreCoroutine;
	}

	// Token: 0x02001164 RID: 4452
	public class DirectionManager
	{
		// Token: 0x06007D7D RID: 32125 RVA: 0x0005448D File Offset: 0x0005268D
		public DirectionManager()
		{
			this.Reset();
		}

		// Token: 0x06007D7E RID: 32126 RVA: 0x000544C7 File Offset: 0x000526C7
		public void Reset()
		{
			this.up.Reset();
			this.down.Reset();
			this.left.Reset();
			this.right.Reset();
		}

		// Token: 0x04007A4B RID: 31307
		public ArcadePlayerMotor.DirectionManager.Hit up = new ArcadePlayerMotor.DirectionManager.Hit();

		// Token: 0x04007A4C RID: 31308
		public ArcadePlayerMotor.DirectionManager.Hit down = new ArcadePlayerMotor.DirectionManager.Hit();

		// Token: 0x04007A4D RID: 31309
		public ArcadePlayerMotor.DirectionManager.Hit left = new ArcadePlayerMotor.DirectionManager.Hit();

		// Token: 0x04007A4E RID: 31310
		public ArcadePlayerMotor.DirectionManager.Hit right = new ArcadePlayerMotor.DirectionManager.Hit();

		// Token: 0x020015E6 RID: 5606
		public class Hit
		{
			// Token: 0x06008774 RID: 34676 RVA: 0x0005BB71 File Offset: 0x00059D71
			public Hit()
			{
				this.Reset();
			}

			// Token: 0x06008775 RID: 34677 RVA: 0x0005BB7F File Offset: 0x00059D7F
			public Hit(bool able, Vector2 pos, GameObject gameObject, float distance)
			{
				this.able = able;
				this.pos = pos;
				this.gameObject = gameObject;
				this.distance = distance;
			}

			// Token: 0x06008776 RID: 34678 RVA: 0x0005BBA4 File Offset: 0x00059DA4
			public void Reset()
			{
				this.able = true;
				this.pos = Vector2.zero;
				this.gameObject = null;
				this.distance = -1f;
			}

			// Token: 0x040091F5 RID: 37365
			public bool able;

			// Token: 0x040091F6 RID: 37366
			public Vector2 pos;

			// Token: 0x040091F7 RID: 37367
			public GameObject gameObject;

			// Token: 0x040091F8 RID: 37368
			public float distance;
		}
	}

	// Token: 0x02001165 RID: 4453
	public class HitManager
	{
		// Token: 0x06007D80 RID: 32128 RVA: 0x000544FD File Offset: 0x000526FD
		public void Reset()
		{
			this.state = ArcadePlayerMotor.HitManager.State.Inactive;
			this.timer = 0f;
			this.direction = 0;
		}

		// Token: 0x04007A4F RID: 31311
		public ArcadePlayerMotor.HitManager.State state;

		// Token: 0x04007A50 RID: 31312
		public float timer;

		// Token: 0x04007A51 RID: 31313
		public int direction;

		// Token: 0x020015E7 RID: 5607
		public enum State
		{
			// Token: 0x040091FA RID: 37370
			Inactive,
			// Token: 0x040091FB RID: 37371
			Hit,
			// Token: 0x040091FC RID: 37372
			KnockedUp
		}
	}

	// Token: 0x02001166 RID: 4454
	public class SuperManager
	{
		// Token: 0x04007A52 RID: 31314
		public ArcadePlayerMotor.SuperManager.State state;

		// Token: 0x020015E8 RID: 5608
		public enum State
		{
			// Token: 0x040091FE RID: 37374
			Ready,
			// Token: 0x040091FF RID: 37375
			Ex,
			// Token: 0x04009200 RID: 37376
			Super
		}
	}

	// Token: 0x02001167 RID: 4455
	public class BoundsManager
	{
		// Token: 0x06007D82 RID: 32130 RVA: 0x00054520 File Offset: 0x00052720
		public BoundsManager(Transform playerTransform)
		{
			this.transform = playerTransform;
			this.boxCollider = (this.transform.GetComponent<Collider2D>() as BoxCollider2D);
		}

		// Token: 0x170017E9 RID: 6121
		// (get) Token: 0x06007D83 RID: 32131 RVA: 0x0028C958 File Offset: 0x0028AB58
		public Vector3 Top
		{
			get
			{
				return new Vector3(this.Center.x, this.Center.y + this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x170017EA RID: 6122
		// (get) Token: 0x06007D84 RID: 32132 RVA: 0x0028C9A8 File Offset: 0x0028ABA8
		public Vector3 TopLeft
		{
			get
			{
				return new Vector3(this.Center.x - this.boxCollider.size.x / 2f, this.Center.y + this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x170017EB RID: 6123
		// (get) Token: 0x06007D85 RID: 32133 RVA: 0x0028CA10 File Offset: 0x0028AC10
		public Vector3 TopRight
		{
			get
			{
				return new Vector3(this.Center.x + this.boxCollider.size.x / 2f, this.Center.y + this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x170017EC RID: 6124
		// (get) Token: 0x06007D86 RID: 32134 RVA: 0x0028CA78 File Offset: 0x0028AC78
		public Vector3 CenterLeft
		{
			get
			{
				return new Vector3(this.Center.x - this.boxCollider.size.x / 2f, this.Center.y, 0f);
			}
		}

		// Token: 0x170017ED RID: 6125
		// (get) Token: 0x06007D87 RID: 32135 RVA: 0x0028CAC8 File Offset: 0x0028ACC8
		public Vector3 CenterRight
		{
			get
			{
				return new Vector3(this.Center.x + this.boxCollider.size.x / 2f, this.Center.y, 0f);
			}
		}

		// Token: 0x170017EE RID: 6126
		// (get) Token: 0x06007D88 RID: 32136 RVA: 0x00054545 File Offset: 0x00052745
		public Vector2 Center
		{
			get
			{
				return this.transform.position + this.boxCollider.offset;
			}
		}

		// Token: 0x170017EF RID: 6127
		// (get) Token: 0x06007D89 RID: 32137 RVA: 0x0028CB18 File Offset: 0x0028AD18
		public Vector3 Bottom
		{
			get
			{
				return new Vector3(this.Center.x, this.Center.y - this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x170017F0 RID: 6128
		// (get) Token: 0x06007D8A RID: 32138 RVA: 0x0028CB68 File Offset: 0x0028AD68
		public Vector3 BottomLeft
		{
			get
			{
				return new Vector3(this.Center.x - this.boxCollider.size.x / 2f, this.Center.y - this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x170017F1 RID: 6129
		// (get) Token: 0x06007D8B RID: 32139 RVA: 0x0028CBD0 File Offset: 0x0028ADD0
		public Vector3 BottomRight
		{
			get
			{
				return new Vector3(this.Center.x + this.boxCollider.size.x / 2f, this.Center.y - this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x170017F2 RID: 6130
		// (get) Token: 0x06007D8C RID: 32140 RVA: 0x0028CC38 File Offset: 0x0028AE38
		public float TopY
		{
			get
			{
				return this.Top.y - this.transform.position.y;
			}
		}

		// Token: 0x170017F3 RID: 6131
		// (get) Token: 0x06007D8D RID: 32141 RVA: 0x0028CC68 File Offset: 0x0028AE68
		public float BottomY
		{
			get
			{
				return this.Bottom.y - this.transform.position.y;
			}
		}

		// Token: 0x04007A53 RID: 31315
		public readonly Transform transform;

		// Token: 0x04007A54 RID: 31316
		public BoxCollider2D boxCollider;
	}
}
