using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000513 RID: 1299
public class LevelPlayerMotor : AbstractLevelPlayerComponent
{
	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x06003673 RID: 13939 RVA: 0x0002CA33 File Offset: 0x0002AC33
	// (set) Token: 0x06003674 RID: 13940 RVA: 0x0002CA3B File Offset: 0x0002AC3B
	public Trilean2 LookDirection { get; set; }

	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x06003675 RID: 13941 RVA: 0x0002CA44 File Offset: 0x0002AC44
	// (set) Token: 0x06003676 RID: 13942 RVA: 0x0002CA4C File Offset: 0x0002AC4C
	public Trilean2 TrueLookDirection { get; set; }

	// Token: 0x17000432 RID: 1074
	// (get) Token: 0x06003677 RID: 13943 RVA: 0x0002CA55 File Offset: 0x0002AC55
	// (set) Token: 0x06003678 RID: 13944 RVA: 0x0002CA5D File Offset: 0x0002AC5D
	public Trilean2 MoveDirection { get; set; }

	// Token: 0x17000433 RID: 1075
	// (get) Token: 0x06003679 RID: 13945 RVA: 0x0002CA66 File Offset: 0x0002AC66
	public LevelPlayerMotor.JumpManager.State JumpState
	{
		get
		{
			return this.jumpManager.state;
		}
	}

	// Token: 0x17000434 RID: 1076
	// (get) Token: 0x0600367A RID: 13946 RVA: 0x0002CA73 File Offset: 0x0002AC73
	public bool Dashing
	{
		get
		{
			return this.dashManager.IsDashing;
		}
	}

	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x0600367B RID: 13947 RVA: 0x0002CA80 File Offset: 0x0002AC80
	public int DashDirection
	{
		get
		{
			return this.dashManager.direction;
		}
	}

	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x0600367C RID: 13948 RVA: 0x0002CA8D File Offset: 0x0002AC8D
	public LevelPlayerMotor.DashManager.State DashState
	{
		get
		{
			return this.dashManager.state;
		}
	}

	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x0600367D RID: 13949 RVA: 0x0002CA9A File Offset: 0x0002AC9A
	// (set) Token: 0x0600367E RID: 13950 RVA: 0x0002CAA2 File Offset: 0x0002ACA2
	public bool Locked { get; set; }

	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x0600367F RID: 13951 RVA: 0x0002CAAB File Offset: 0x0002ACAB
	// (set) Token: 0x06003680 RID: 13952 RVA: 0x0002CAB3 File Offset: 0x0002ACB3
	public bool Grounded { get; set; }

	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x06003681 RID: 13953 RVA: 0x0002CABC File Offset: 0x0002ACBC
	// (set) Token: 0x06003682 RID: 13954 RVA: 0x0002CAC4 File Offset: 0x0002ACC4
	public bool Parrying { get; set; }

	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x06003683 RID: 13955 RVA: 0x000FE978 File Offset: 0x000FCB78
	public bool Ducking
	{
		get
		{
			return this.LookDirection.y < 0 && !this.Locked && this.Grounded;
		}
	}

	// Token: 0x1700043B RID: 1083
	// (get) Token: 0x06003684 RID: 13956 RVA: 0x0002CACD File Offset: 0x0002ACCD
	public bool IsHit
	{
		get
		{
			return this.hitManager.state == LevelPlayerMotor.HitManager.State.Hit;
		}
	}

	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x06003685 RID: 13957 RVA: 0x0002CADD File Offset: 0x0002ACDD
	public bool IsUsingSuperOrEx
	{
		get
		{
			return this.superManager.state == LevelPlayerMotor.SuperManager.State.Super || this.superManager.state == LevelPlayerMotor.SuperManager.State.Ex;
		}
	}

	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x06003686 RID: 13958 RVA: 0x0002CB01 File Offset: 0x0002AD01
	// (set) Token: 0x06003687 RID: 13959 RVA: 0x0002CB09 File Offset: 0x0002AD09
	public bool GravityReversed { get; set; }

	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x06003688 RID: 13960 RVA: 0x0002CB12 File Offset: 0x0002AD12
	// (set) Token: 0x06003689 RID: 13961 RVA: 0x0002CB1A File Offset: 0x0002AD1A
	public bool ChaliceDoubleJumped { get; set; }

	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x0600368A RID: 13962 RVA: 0x0002CB23 File Offset: 0x0002AD23
	// (set) Token: 0x0600368B RID: 13963 RVA: 0x0002CB2B File Offset: 0x0002AD2B
	public bool ChaliceDuckDashed { get; set; }

	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x0600368C RID: 13964 RVA: 0x0002CB34 File Offset: 0x0002AD34
	public float GravityReversalMultiplier
	{
		get
		{
			return (float)((!this.GravityReversed) ? 1 : -1);
		}
	}

	// Token: 0x1400008B RID: 139
	// (add) Token: 0x0600368D RID: 13965 RVA: 0x000FE9B4 File Offset: 0x000FCBB4
	// (remove) Token: 0x0600368E RID: 13966 RVA: 0x000FE9EC File Offset: 0x000FCBEC
	public event Action OnGroundedEvent;

	// Token: 0x1400008C RID: 140
	// (add) Token: 0x0600368F RID: 13967 RVA: 0x000FEA24 File Offset: 0x000FCC24
	// (remove) Token: 0x06003690 RID: 13968 RVA: 0x000FEA5C File Offset: 0x000FCC5C
	public event Action OnJumpEvent;

	// Token: 0x1400008D RID: 141
	// (add) Token: 0x06003691 RID: 13969 RVA: 0x000FEA94 File Offset: 0x000FCC94
	// (remove) Token: 0x06003692 RID: 13970 RVA: 0x000FEACC File Offset: 0x000FCCCC
	public event Action OnDoubleJumpEvent;

	// Token: 0x1400008E RID: 142
	// (add) Token: 0x06003693 RID: 13971 RVA: 0x000FEB04 File Offset: 0x000FCD04
	// (remove) Token: 0x06003694 RID: 13972 RVA: 0x000FEB3C File Offset: 0x000FCD3C
	public event Action OnParryEvent;

	// Token: 0x1400008F RID: 143
	// (add) Token: 0x06003695 RID: 13973 RVA: 0x000FEB74 File Offset: 0x000FCD74
	// (remove) Token: 0x06003696 RID: 13974 RVA: 0x000FEBAC File Offset: 0x000FCDAC
	public event Action OnParrySuccess;

	// Token: 0x14000090 RID: 144
	// (add) Token: 0x06003697 RID: 13975 RVA: 0x000FEBE4 File Offset: 0x000FCDE4
	// (remove) Token: 0x06003698 RID: 13976 RVA: 0x000FEC1C File Offset: 0x000FCE1C
	public event Action OnHitEvent;

	// Token: 0x14000091 RID: 145
	// (add) Token: 0x06003699 RID: 13977 RVA: 0x000FEC54 File Offset: 0x000FCE54
	// (remove) Token: 0x0600369A RID: 13978 RVA: 0x000FEC8C File Offset: 0x000FCE8C
	public event Action OnDashStartEvent;

	// Token: 0x14000092 RID: 146
	// (add) Token: 0x0600369B RID: 13979 RVA: 0x000FECC4 File Offset: 0x000FCEC4
	// (remove) Token: 0x0600369C RID: 13980 RVA: 0x000FECFC File Offset: 0x000FCEFC
	public event Action OnDashEndEvent;

	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x0600369D RID: 13981 RVA: 0x0002CB49 File Offset: 0x0002AD49
	// (set) Token: 0x0600369E RID: 13982 RVA: 0x0002CB51 File Offset: 0x0002AD51
	public bool isFloating { get; set; }

	// Token: 0x0600369F RID: 13983 RVA: 0x000FED34 File Offset: 0x000FCF34
	public override void OnAwake()
	{
		base.OnAwake();
		this.properties = new LevelPlayerMotor.Properties();
		this.MoveDirection = new Trilean2(0, 0);
		this.LookDirection = new Trilean2(1, 0);
		this.TrueLookDirection = new Trilean2(1, 0);
		this.velocityManager = new LevelPlayerMotor.VelocityManager(this, this.properties.maxSpeedY, this.properties.yEase);
		this.jumpManager = new LevelPlayerMotor.JumpManager();
		this.dashManager = new LevelPlayerMotor.DashManager();
		this.parryManager = new LevelPlayerMotor.ParryManager();
		this.directionManager = new LevelPlayerMotor.DirectionManager();
		this.platformManager = new LevelPlayerMotor.PlatformManager(this);
		this.hitManager = new LevelPlayerMotor.HitManager();
		this.superManager = new LevelPlayerMotor.SuperManager();
		this.boundsManager = new LevelPlayerMotor.BoundsManager(this);
		base.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.allowInput = true;
		this.allowFalling = true;
		this.allowJumping = true;
		this.forceLaunchUp = false;
	}

	// Token: 0x060036A0 RID: 13984 RVA: 0x000FEE30 File Offset: 0x000FD030
	public void Start()
	{
		base.player.weaponManager.OnExStart += this.StartEx;
		base.player.weaponManager.OnSuperStart += this.StartSuper;
		base.player.weaponManager.OnExFire += this.OnExFired;
		base.player.weaponManager.OnSuperEnd += this.OnSuperEnd;
		base.player.weaponManager.OnExEnd += this.ResetSuperAndEx;
		base.player.weaponManager.OnSuperEnd += this.ResetSuperAndEx;
		base.player.OnReviveEvent += this.OnRevive;
		this.parryController = base.player.GetComponent<LevelPlayerParryController>();
		this.jumpPower = (base.player.stats.isChalice ? this.properties.chaliceFirstJumpPower : this.properties.jumpPower);
	}

	// Token: 0x060036A1 RID: 13985 RVA: 0x000FEF44 File Offset: 0x000FD144
	public void FixedUpdate()
	{
		if (base.player.IsDead)
		{
			return;
		}
		this.HandleLooking();
		if (base.player.weaponManager.FreezePosition)
		{
			return;
		}
		this.HandleInput();
		if (this.allowFalling)
		{
			this.HandleFalling();
		}
		if (!this.Grounded)
		{
			this.jumpManager.timeInAir += CupheadTime.FixedDelta;
			if (this.jumpManager.state == LevelPlayerMotor.JumpManager.State.Ready && this.jumpManager.timeInAir > 0.0834f)
			{
				this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used;
			}
		}
		this.Move();
		this.HandleRaycasts();
		Vector2 vector = base.transform.localPosition;
		Vector2 v = vector - ((!this.platformManager.OnPlatform && base.player.stats.isChalice) ? this.lastPosition : this.lastPositionFixed);
		v.x = (float)((int)v.x);
		v.y = (float)((int)v.y);
		this.MoveDirection = v;
		this.lastPositionFixed = new Vector2(vector.x, vector.y);
		this.lastPosition = base.transform.position;
		this.ClampToBounds();
	}

	// Token: 0x060036A2 RID: 13986 RVA: 0x000FF0A4 File Offset: 0x000FD2A4
	public void DisableInput()
	{
		this.allowInput = false;
		this.Locked = false;
		this.MoveDirection = new Trilean2(0, 0);
		this.velocityManager.move = 0f;
		this.velocityManager.dash = 0f;
		this.velocityManager.verticalDash = 0f;
	}

	// Token: 0x060036A3 RID: 13987 RVA: 0x0002CB5A File Offset: 0x0002AD5A
	public void EnableInput()
	{
		this.allowInput = true;
	}

	// Token: 0x060036A4 RID: 13988 RVA: 0x0002CB63 File Offset: 0x0002AD63
	public void DisableJump()
	{
		this.allowJumping = false;
	}

	// Token: 0x060036A5 RID: 13989 RVA: 0x0002CB6C File Offset: 0x0002AD6C
	public void EnableJump()
	{
		this.allowJumping = true;
	}

	// Token: 0x060036A6 RID: 13990 RVA: 0x000FF0FC File Offset: 0x000FD2FC
	public void DisableGravity()
	{
		this.allowFalling = false;
		this.MoveDirection = new Trilean2(this.MoveDirection.x, 0);
		this.velocityManager.y = 0f;
	}

	// Token: 0x060036A7 RID: 13991 RVA: 0x0002CB75 File Offset: 0x0002AD75
	public void EnableGravity()
	{
		this.allowFalling = true;
		this.velocityManager.y = 0f;
	}

	// Token: 0x060036A8 RID: 13992 RVA: 0x000FF140 File Offset: 0x000FD340
	public void SetGravityReversed(bool reversed)
	{
		if (reversed != this.GravityReversed)
		{
			this.GravityReversed = reversed;
			base.player.animationController.OnGravityReversed();
			base.transform.AddPosition(0f, -(base.player.center.y - base.transform.position.y) * (float)((!base.player.stats.isChalice) ? 2 : 4), 0f);
			this.reversingGravity = true;
		}
	}

	// Token: 0x060036A9 RID: 13993 RVA: 0x0002CB8E File Offset: 0x0002AD8E
	public RaycastHit2D BoxCast(Vector2 size, Vector2 direction, int layerMask)
	{
		return this.BoxCast(size, direction, layerMask, Vector2.zero);
	}

	// Token: 0x060036AA RID: 13994 RVA: 0x0002CB9E File Offset: 0x0002AD9E
	public RaycastHit2D BoxCast(Vector2 size, Vector2 direction, int layerMask, Vector2 offset)
	{
		return Physics2D.BoxCast(base.player.colliderManager.DefaultCenter + offset, size, 0f, direction, 2000f, layerMask);
	}

	// Token: 0x060036AB RID: 13995 RVA: 0x0002CBC9 File Offset: 0x0002ADC9
	public RaycastHit2D CircleCast(float radius, Vector2 direction, int layerMask)
	{
		return Physics2D.CircleCast(base.player.colliderManager.DefaultCenter, radius, direction, 2000f, layerMask);
	}

	// Token: 0x060036AC RID: 13996 RVA: 0x0002CBE8 File Offset: 0x0002ADE8
	public bool DoesRaycastHitHaveCollider(RaycastHit2D hit)
	{
		return hit.collider != null;
	}

	// Token: 0x060036AD RID: 13997 RVA: 0x000FF1D4 File Offset: 0x000FD3D4
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

	// Token: 0x060036AE RID: 13998 RVA: 0x000FF230 File Offset: 0x000FD430
	public void HandleRaycasts()
	{
		bool flag = true;
		if (this.directionManager != null && this.directionManager.up != null)
		{
			flag = this.directionManager.up.able;
		}
		LevelPlayerColliderManager colliderManager = base.player.colliderManager;
		this.directionManager.Reset();
		RaycastHit2D raycastHit = this.BoxCast(new Vector2(1f, (!flag && base.player.stats.isChalice) ? 1f : colliderManager.DefaultHeight), Vector2.left, this.wallMask);
		RaycastHit2D raycastHit2 = this.BoxCast(new Vector2(1f, (!flag && base.player.stats.isChalice) ? 1f : colliderManager.DefaultHeight), Vector2.right, this.wallMask);
		RaycastHit2D raycastHit3 = this.BoxCast(new Vector2(colliderManager.DefaultWidth, 1f), (!this.GravityReversed) ? Vector2.up : Vector2.down, (!this.GravityReversed) ? this.ceilingMask : this.groundMask);
		this.RaycastObstacle(this.directionManager.left, raycastHit, colliderManager.DefaultWidth / 2f, LevelPlayerMotor.RaycastAxis.X);
		this.RaycastObstacle(this.directionManager.right, raycastHit2, colliderManager.DefaultWidth / 2f, LevelPlayerMotor.RaycastAxis.X);
		this.RaycastObstacle(this.directionManager.up, raycastHit3, colliderManager.DefaultHeight / 2f, LevelPlayerMotor.RaycastAxis.Y);
		Vector2 vector = colliderManager.DefaultCenter + new Vector2(0f, colliderManager.DefaultHeight * this.GravityReversalMultiplier);
		int num = Physics2D.BoxCastNonAlloc(vector, new Vector2(colliderManager.DefaultWidth, 1f), 0f, (!this.GravityReversed) ? Vector2.down : Vector2.up, this.hitBuffer, 1000f, (!this.GravityReversed) ? this.groundMask : this.ceilingMask);
		this.directionManager.down.pos = new Vector2(colliderManager.DefaultCenter.x, -10000f * this.GravityReversalMultiplier);
		for (int i = 0; i < num; i++)
		{
			RaycastHit2D raycastHit2D = this.hitBuffer[i];
			if ((!this.GravityReversed) ? (raycastHit2D.point.y > this.directionManager.down.pos.y) : (raycastHit2D.point.y < this.directionManager.down.pos.y))
			{
				if (!((!this.GravityReversed) ? (raycastHit2D.point.y > 20f + base.transform.position.y) : (raycastHit2D.point.y < -20f + base.transform.position.y)))
				{
					float num2 = Math.Abs(base.transform.position.y - raycastHit2D.point.y);
					this.directionManager.down.pos = new Vector2(vector.x, raycastHit2D.point.y);
					this.directionManager.down.gameObject = raycastHit2D.collider.gameObject;
					this.directionManager.down.distance = num2;
					if (num2 < 20f)
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
			if (!this.directionManager.up.able && (this.reversingGravity || this.directionManager.up.able != flag))
			{
				GameObject gameObject = this.directionManager.up.gameObject;
				LevelPlatform levelPlatform = (!(gameObject == null)) ? gameObject.GetComponent<LevelPlatform>() : null;
				if (!this.GravityReversed || levelPlatform == null || !levelPlatform.canFallThrough)
				{
					this.OnHitCeiling();
				}
			}
		}
		float num3 = Mathf.Abs(base.transform.position.y - this.directionManager.down.pos.y);
		if (this.Grounded && num3 > 30f)
		{
			this.LeaveGround(true);
		}
	}

	// Token: 0x060036AF RID: 13999 RVA: 0x000FF760 File Offset: 0x000FD960
	public float RaycastObstacle(LevelPlayerMotor.DirectionManager.Hit directionProperties, RaycastHit2D raycastHit, float maxDistance, LevelPlayerMotor.RaycastAxis axis)
	{
		if (!this.DoesRaycastHitHaveCollider(raycastHit))
		{
			return 1000f;
		}
		float num = (axis != LevelPlayerMotor.RaycastAxis.X) ? Math.Abs(base.player.colliderManager.DefaultCenter.y - raycastHit.point.y) : Math.Abs(base.player.colliderManager.DefaultCenter.x - raycastHit.point.x);
		directionProperties.pos = raycastHit.point;
		directionProperties.gameObject = raycastHit.collider.gameObject;
		directionProperties.distance = num;
		if (num < maxDistance)
		{
			directionProperties.able = false;
		}
		return num;
	}

	// Token: 0x060036B0 RID: 14000 RVA: 0x000FF81C File Offset: 0x000FDA1C
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
		if (this.jumpManager.doubleJumped)
		{
			this.jumpManager.doubleJumped = false;
		}
		this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Ready;
		this.parryManager.state = LevelPlayerMotor.ParryManager.State.Ready;
		this.velocityManager.y = 0f;
		this.platformManager.ResetAll();
		this.Grounded = true;
		this.Parrying = false;
		this.reversingGravity = false;
		this.dashManager.timeSinceGroundDash = 1000f;
		if (base.player.stats.isChalice)
		{
			this.ChaliceDoubleJumped = false;
		}
		if (this.jumpManager.timeInAir > this.jumpManager.longestTimeInAir)
		{
			this.jumpManager.longestTimeInAir = this.jumpManager.timeInAir;
			OnlineManager.Instance.Interface.SetStat(base.player.id, "HangTime", this.jumpManager.timeInAir);
		}
		if (this.OnGroundedEvent != null)
		{
			this.OnGroundedEvent();
		}
	}

	// Token: 0x060036B1 RID: 14001 RVA: 0x000FF9BC File Offset: 0x000FDBBC
	public void LeaveGround(bool allowLateJump = false)
	{
		if (!this.Dashing && base.player.stats.Loadout.charm == Charm.charm_parry_plus && !Level.IsChessBoss)
		{
			this.ForceParry();
		}
		if (this.Grounded)
		{
			this.Grounded = false;
			this.jumpManager.ableToLand = false;
			this.jumpManager.timeInAir = 0f;
			base.player.stats.ResetJumpParries();
			this.ResetSuperAndEx();
			base.player.weaponManager.ResetEx();
		}
		this.velocityManager.y = 0f;
		this.ClearParent();
		if (this.jumpManager.state == LevelPlayerMotor.JumpManager.State.Ready && !allowLateJump)
		{
			this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used;
		}
	}

	// Token: 0x060036B2 RID: 14002 RVA: 0x000FFA90 File Offset: 0x000FDC90
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

	// Token: 0x060036B3 RID: 14003 RVA: 0x000FFAE0 File Offset: 0x000FDCE0
	public IEnumerator MoveToX_cr(float x, int endingLookDirection = 1)
	{
		if (base.transform.position.x == x)
		{
			yield break;
		}
		float walk = 0f;
		this.MoveDirection = new Trilean2(0, 0);
		this.LookDirection = new Trilean2(1, 0);
		bool left = base.transform.position.x < x;
		if (!left)
		{
			this.LookDirection = new Trilean2(-1, 0);
		}
		while ((!left) ? (base.transform.position.x > x) : (base.transform.position.x < x))
		{
			if ((this.LookDirection.y >= 0 || !this.Grounded) && !this.Locked)
			{
				walk = (float)((!left) ? -1 : 1) * this.properties.moveSpeed;
			}
			this.velocityManager.move = walk;
			yield return null;
		}
		walk = 0f;
		this.velocityManager.move = walk;
		this.MoveDirection = new Trilean2(0, 0);
		this.LookDirection = new Trilean2(endingLookDirection, 0);
		yield return null;
		this.LookDirection = new Trilean2(0, 0);
		yield break;
	}

	// Token: 0x060036B4 RID: 14004 RVA: 0x000FFB0C File Offset: 0x000FDD0C
	public void Move()
	{
		this.velocityManager.Calculate();
		Vector3 vector = this.velocityManager.Total;
		if (this.hitManager.state != LevelPlayerMotor.HitManager.State.Hit && this.superManager.state == LevelPlayerMotor.SuperManager.State.Ready)
		{
			if (!this.velocityManager.yAxisForce)
			{
				this.forceLaunchUp = false;
				if (this.Grounded)
				{
					vector.x += this.velocityManager.GroundForce;
				}
				else
				{
					vector.x += this.velocityManager.AirForce;
				}
			}
			else if (this.Grounded)
			{
				if (!this.forceLaunchUp)
				{
					this.LeaveGround(false);
					this.velocityManager.y = this.properties.jumpPower * 2f;
					this.DisableGravity();
					this.forceLaunchUp = true;
				}
			}
			else
			{
				vector.y += this.velocityManager.AirForce;
				base.FrameDelayedCallback(new Action(this.EnableGravity), 1);
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
		if (this.GravityReversed)
		{
			vector.y *= -1f;
		}
		base.transform.localPosition += vector * CupheadTime.FixedDelta;
		if (this.Grounded)
		{
			Vector2 vector2 = base.transform.position;
			vector2.y = this.directionManager.down.pos.y;
			base.transform.position = vector2;
			LevelPlatform levelPlatform = null;
			if (this.directionManager.down.gameObject != null)
			{
				levelPlatform = this.directionManager.down.gameObject.GetComponent<LevelPlatform>();
			}
			if (levelPlatform == null && base.transform.parent != null)
			{
				this.ClearParent();
			}
			else if (levelPlatform != null && (base.transform.parent == null || levelPlatform.gameObject != base.transform.parent.gameObject))
			{
				this.ClearParent();
				levelPlatform.AddChild(base.transform);
			}
		}
	}

	// Token: 0x060036B5 RID: 14005 RVA: 0x000FFEB4 File Offset: 0x000FE0B4
	public void ClampToBounds()
	{
		float num = base.player.colliderManager.Width / 2f;
		float num2 = this.directionManager.left.pos.x + ((!this.reversingGravity) ? num : (-num));
		float num3 = this.directionManager.right.pos.x - ((!this.reversingGravity) ? num : (-num));
		float num4 = this.directionManager.up.pos.y - ((!this.GravityReversed) ? this.boundsManager.TopY : this.boundsManager.BottomY);
		float num5 = this.directionManager.down.pos.y - ((!this.GravityReversed) ? this.boundsManager.BottomY : this.boundsManager.TopY);
		GameObject gameObject = this.directionManager.up.gameObject;
		LevelPlatform levelPlatform = (!(gameObject == null)) ? gameObject.GetComponent<LevelPlatform>() : null;
		bool flag = !this.GravityReversed || levelPlatform == null || !levelPlatform.canFallThrough;
		Vector3 position = base.transform.position;
		if (!this.directionManager.left.able && base.transform.position.x < num2)
		{
			position.x = num2;
		}
		if (!this.directionManager.right.able && base.transform.position.x > num3)
		{
			position.x = num3;
		}
		if (!this.directionManager.up.able && flag && ((!this.GravityReversed) ? (base.transform.position.y > num4) : (base.transform.position.y < num4)))
		{
			position.y = num4;
		}
		position.x = Mathf.Clamp(position.x, (float)Level.Current.Left + num, (float)Level.Current.Right - num);
		base.transform.position = position;
	}

	// Token: 0x060036B6 RID: 14006 RVA: 0x00100118 File Offset: 0x000FE318
	public void ResetSuperAndEx()
	{
		if (this.superManager.state == LevelPlayerMotor.SuperManager.State.Ready)
		{
			return;
		}
		if (this.jumpManager.state != LevelPlayerMotor.JumpManager.State.Ready)
		{
			this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used;
		}
		base.StopCoroutine(this.exMove_cr());
		this.superManager.state = LevelPlayerMotor.SuperManager.State.Ready;
		this.EnableInput();
		this.EnableGravity();
	}

	// Token: 0x060036B7 RID: 14007 RVA: 0x0002CBF7 File Offset: 0x0002ADF7
	public void StartSuper()
	{
		this.LeaveGround(false);
		this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used;
		this.jumpManager.timer = 0f;
		this.velocityManager.y = 0f;
	}

	// Token: 0x060036B8 RID: 14008 RVA: 0x0002CC2C File Offset: 0x0002AE2C
	public void OnSuperEnd()
	{
		if (this.Grounded)
		{
			this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Ready;
		}
		else
		{
			this.DoPostSuperHop();
		}
	}

	// Token: 0x060036B9 RID: 14009 RVA: 0x00100178 File Offset: 0x000FE378
	public void DoPostSuperHop()
	{
		this.LeaveGround(false);
		this.velocityManager.y = ((base.player.stats.Loadout.super != Super.level_super_invincible) ? this.properties.superKnockUp : this.properties.superInvincibleKnockUp);
	}

	// Token: 0x060036BA RID: 14010 RVA: 0x0002CC50 File Offset: 0x0002AE50
	public void CheckForPostSuperHop()
	{
		this.HandleRaycasts();
		if (!this.Grounded)
		{
			this.DoPostSuperHop();
			base.player.animator.Play("Jump_Launch");
		}
	}

	// Token: 0x060036BB RID: 14011 RVA: 0x0002CC7E File Offset: 0x0002AE7E
	public void StartEx()
	{
		this.exFirePose = base.player.weaponManager.GetDirectionPose();
		this.DisableInput();
		this.DisableGravity();
		this.superManager.state = LevelPlayerMotor.SuperManager.State.Ex;
	}

	// Token: 0x060036BC RID: 14012 RVA: 0x0002CCAE File Offset: 0x0002AEAE
	public void OnExFired()
	{
		if (this.exFirePose == LevelPlayerWeaponManager.Pose.Up || this.exFirePose == LevelPlayerWeaponManager.Pose.Down)
		{
			base.StartCoroutine(this.exDelay_cr());
		}
		else
		{
			base.StartCoroutine(this.exMove_cr());
		}
	}

	// Token: 0x060036BD RID: 14013 RVA: 0x001001D4 File Offset: 0x000FE3D4
	public IEnumerator exDelay_cr()
	{
		while (this.superManager.state != LevelPlayerMotor.SuperManager.State.Ready)
		{
			yield return null;
		}
		this.EnableInput();
		this.EnableGravity();
		this.superManager.state = LevelPlayerMotor.SuperManager.State.Ready;
		yield break;
	}

	// Token: 0x060036BE RID: 14014 RVA: 0x001001F0 File Offset: 0x000FE3F0
	public IEnumerator exMove_cr()
	{
		while (this.superManager.state != LevelPlayerMotor.SuperManager.State.Ready)
		{
			this.velocityManager.move = (float)(this.TrueLookDirection.x * -1) * this.properties.exKnockback;
			yield return null;
		}
		this.EnableInput();
		this.EnableGravity();
		this.superManager.state = LevelPlayerMotor.SuperManager.State.Ready;
		yield break;
	}

	// Token: 0x060036BF RID: 14015 RVA: 0x0010020C File Offset: 0x000FE40C
	public void HandleInput()
	{
		if (!base.player.levelStarted)
		{
			return;
		}
		this.timeSinceInputBuffered += CupheadTime.FixedDelta;
		this.dashManager.timeSinceGroundDash += CupheadTime.FixedDelta;
		if ((!this.allowInput || this.dashManager.IsDashing) && this.hitManager.state == LevelPlayerMotor.HitManager.State.Inactive)
		{
			this.BufferInputs();
		}
		if (!this.allowInput)
		{
			return;
		}
		if (!this.HandleDash())
		{
			if (this.hitManager.state == LevelPlayerMotor.HitManager.State.Hit)
			{
				this.HandleHit();
			}
			else
			{
				if (this.hitManager.state != LevelPlayerMotor.HitManager.State.KnockedUp)
				{
					this.HandleParry();
					this.HandleJumping();
					this.HandleLocked();
				}
				else
				{
					this.HandlePitKnockUp();
				}
				this.HandleWalking();
			}
		}
	}

	// Token: 0x060036C0 RID: 14016 RVA: 0x0002CCE7 File Offset: 0x0002AEE7
	public void BufferInput(LevelPlayerMotor.BufferedInput input)
	{
		this.bufferedInput = input;
		this.timeSinceInputBuffered = 0f;
	}

	// Token: 0x060036C1 RID: 14017 RVA: 0x001002F0 File Offset: 0x000FE4F0
	public void BufferInputs()
	{
		if (base.player.input.actions.GetButtonDown(2))
		{
			this.BufferInput(LevelPlayerMotor.BufferedInput.Jump);
		}
		else if (base.player.input.actions.GetButtonDown(7) && !this.dashManager.IsDashing)
		{
			this.BufferInput(LevelPlayerMotor.BufferedInput.Dash);
		}
		else if (base.player.input.actions.GetButtonDown(4))
		{
			this.BufferInput(LevelPlayerMotor.BufferedInput.Super);
		}
	}

	// Token: 0x060036C2 RID: 14018 RVA: 0x0002CCFB File Offset: 0x0002AEFB
	public void ClearBufferedInput()
	{
		this.timeSinceInputBuffered = 0.134f;
	}

	// Token: 0x060036C3 RID: 14019 RVA: 0x0002CD08 File Offset: 0x0002AF08
	public bool HasBufferedInput(LevelPlayerMotor.BufferedInput input)
	{
		return this.bufferedInput == input && this.timeSinceInputBuffered < 0.134f;
	}

	// Token: 0x060036C4 RID: 14020 RVA: 0x00100380 File Offset: 0x000FE580
	public void HandleJumping()
	{
		if (this.allowJumping)
		{
			if (this.jumpManager.state == LevelPlayerMotor.JumpManager.State.Ready && (base.player.input.actions.GetButtonDown(2) || this.HasBufferedInput(LevelPlayerMotor.BufferedInput.Jump)))
			{
				this.hardExitParry = false;
				this.ClearBufferedInput();
				if (((base.player.stats.ReverseTime > 0f) ? (this.LookDirection.y > 0) : (this.LookDirection.y < 0)) && this.Grounded && base.transform.parent != null)
				{
					LevelPlatform component = base.transform.parent.GetComponent<LevelPlatform>();
					if (component.canFallThrough)
					{
						this.platformManager.Ignore(base.transform.parent);
						this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used;
						this.LeaveGround(false);
						this.jumpManager.timeSinceDownJump = 0f;
						return;
					}
				}
				AudioManager.Play("player_jump");
				this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Hold;
				this.LeaveGround(false);
				this.velocityManager.y = this.jumpPower;
				this.jumpManager.timer = CupheadTime.FixedDelta;
				if (this.OnJumpEvent != null)
				{
					this.OnJumpEvent();
				}
			}
			if (this.jumpManager.state == LevelPlayerMotor.JumpManager.State.Hold)
			{
				if (!this.directionManager.up.able || (this.jumpManager.timer >= this.properties.jumpHoldMin && (base.player.input.actions.GetButtonUp(2) || !base.player.input.actions.GetButton(2))) || this.jumpManager.timer >= this.properties.jumpHoldMax)
				{
					this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used;
					this.jumpManager.timer = 0f;
				}
				if (base.player.stats.isChalice)
				{
					this.velocityManager.y = ((!this.jumpManager.doubleJumped) ? this.properties.chaliceFirstJumpPower : this.properties.chaliceSecondJumpPower);
				}
				else
				{
					this.velocityManager.y = this.jumpPower;
				}
				this.jumpManager.timer += CupheadTime.FixedDelta;
			}
			this.jumpManager.timeSinceDownJump += CupheadTime.FixedDelta;
			if (base.player.stats.isChalice && !this.jumpManager.doubleJumped)
			{
				this.ChaliceDoubleJump();
			}
		}
	}

	// Token: 0x060036C5 RID: 14021 RVA: 0x0002CD26 File Offset: 0x0002AF26
	public void OnChaliceRevive()
	{
		this.ChaliceDoubleJumped = true;
	}

	// Token: 0x060036C6 RID: 14022 RVA: 0x00100660 File Offset: 0x000FE860
	public void ChaliceDoubleJump()
	{
		if ((base.player.input.actions.GetButtonDown(2) || this.HasBufferedInput(LevelPlayerMotor.BufferedInput.Jump)) && this.jumpManager.state == LevelPlayerMotor.JumpManager.State.Used && !this.IsHit)
		{
			this.hardExitParry = false;
			this.ClearBufferedInput();
			if (this.dashManager.state == LevelPlayerMotor.DashManager.State.End && this.parryManager.state == LevelPlayerMotor.ParryManager.State.Ready)
			{
				this.dashManager.state = LevelPlayerMotor.DashManager.State.Ready;
			}
			AudioManager.Play("chalice_doublejump");
			this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Hold;
			this.LeaveGround(false);
			this.jumpManager.doubleJumped = true;
			this.velocityManager.y = this.properties.chaliceSecondJumpPower;
			this.jumpManager.timer = CupheadTime.FixedDelta;
			this.ChaliceDoubleJumped = true;
			this.platformManager.ResetAll();
			if (this.OnJumpEvent != null)
			{
				this.OnJumpEvent();
			}
			if (this.OnDoubleJumpEvent != null)
			{
				this.OnDoubleJumpEvent();
			}
		}
	}

	// Token: 0x060036C7 RID: 14023 RVA: 0x00100778 File Offset: 0x000FE978
	public void HandleParry()
	{
		if (base.player.stats.isChalice)
		{
			return;
		}
		if (this.IsHit)
		{
			return;
		}
		if (this.parryManager.state == LevelPlayerMotor.ParryManager.State.Ready && (base.player.input.actions.GetButtonDown(2) || this.HasBufferedInput(LevelPlayerMotor.BufferedInput.Jump)) && this.jumpManager.state != LevelPlayerMotor.JumpManager.State.Ready && !this.IsHit)
		{
			this.ClearBufferedInput();
			this.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive;
			this.parryManager.state = LevelPlayerMotor.ParryManager.State.NotReady;
			if (this.dashManager.IsDashing)
			{
				this.dashManager.state = LevelPlayerMotor.DashManager.State.End;
			}
			this.Parrying = true;
			if (this.OnParryEvent != null)
			{
				this.OnParryEvent();
			}
		}
	}

	// Token: 0x060036C8 RID: 14024 RVA: 0x00100850 File Offset: 0x000FEA50
	public void OnParryComplete()
	{
		if (base.player.stats.Loadout.charm == Charm.charm_parry_plus && !Level.IsChessBoss)
		{
			this.hardExitParry = true;
		}
		this.LeaveGround(false);
		this.parryManager.state = LevelPlayerMotor.ParryManager.State.Ready;
		this.velocityManager.y = ((!this.parryController.HasHitEnemy) ? this.properties.parryPower : this.properties.parryAttackBounce);
		if (this.OnParrySuccess != null)
		{
			this.OnParrySuccess();
		}
		if (base.player.stats.isChalice)
		{
			this.dashManager.chaliceParryCoolDown = true;
			this.DashComplete();
		}
		this.platformManager.ResetAll();
	}

	// Token: 0x060036C9 RID: 14025 RVA: 0x0002CD2F File Offset: 0x0002AF2F
	public void OnParryHit()
	{
		base.StartCoroutine(this.parryHit_cr());
	}

	// Token: 0x060036CA RID: 14026 RVA: 0x0002CD3E File Offset: 0x0002AF3E
	public void OnParryCanceled()
	{
		this.Parrying = false;
	}

	// Token: 0x060036CB RID: 14027 RVA: 0x0002CD47 File Offset: 0x0002AF47
	public void OnParryAnimEnd()
	{
		this.Parrying = false;
	}

	// Token: 0x060036CC RID: 14028 RVA: 0x00100920 File Offset: 0x000FEB20
	public bool HandleDash()
	{
		if (this.dashManager.state == LevelPlayerMotor.DashManager.State.Ready && (!this.Grounded || this.dashManager.timeSinceGroundDash > 0.1f) && (base.player.input.actions.GetButtonDown(7) || this.HasBufferedInput(LevelPlayerMotor.BufferedInput.Dash)))
		{
			this.ClearBufferedInput();
			AudioManager.Play("player_dash");
			this.dashManager.state = LevelPlayerMotor.DashManager.State.Start;
			this.dashManager.direction = this.TrueLookDirection.x;
			this.dashManager.groundDash = this.Grounded;
			this.ChaliceDuckDashed = (base.player.stats.isChalice && this.Ducking);
			if (this.jumpManager.state == LevelPlayerMotor.JumpManager.State.Hold)
			{
				this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used;
			}
			if (this.OnDashStartEvent != null)
			{
				this.OnDashStartEvent();
			}
			this.velocityManager.move = 0f;
			return true;
		}
		if (this.dashManager.state == LevelPlayerMotor.DashManager.State.Start)
		{
			this.dashManager.state = LevelPlayerMotor.DashManager.State.Dashing;
		}
		if (base.player.stats.isChalice && !this.ChaliceDuckDashed)
		{
			this.ChaliceDashParry();
		}
		if (this.dashManager.state == LevelPlayerMotor.DashManager.State.Dashing)
		{
			this.velocityManager.dash = this.properties.dashSpeed * (float)this.dashManager.direction;
			this.dashManager.timer += CupheadTime.FixedDelta;
			this.velocityManager.y = 0f;
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
		if (this.dashManager.state == LevelPlayerMotor.DashManager.State.End)
		{
			if (this.Grounded)
			{
				this.dashManager.state = LevelPlayerMotor.DashManager.State.Ready;
				if (this.dashManager.groundDash)
				{
					this.dashManager.timeSinceGroundDash = 0f;
				}
				if (base.player.stats.isChalice)
				{
					this.dashManager.chaliceParryCoolDown = false;
					this.dashManager.chaliceParryCoolDownTimer = 0f;
				}
			}
			else
			{
				this.dashManager.groundDash = false;
			}
			this.ChaliceDuckDashed = false;
			if (base.player.stats.isChalice && !this.dashManager.chaliceParryCoolDown)
			{
				this.dashManager.state = LevelPlayerMotor.DashManager.State.Ready;
			}
			if (base.player.stats.isChalice)
			{
				this.ChaliceDashCooldownCheck();
			}
		}
		return false;
	}

	// Token: 0x060036CD RID: 14029 RVA: 0x00100BF0 File Offset: 0x000FEDF0
	public void DashComplete()
	{
		if (base.player.stats.Loadout.charm == Charm.charm_parry_plus && !Level.IsChessBoss)
		{
			this.ForceParry();
		}
		this.dashManager.state = LevelPlayerMotor.DashManager.State.End;
		this.dashManager.timer = 0f;
		this.velocityManager.dash = 0f;
		this.velocityManager.verticalDash = 0f;
		if (this.OnDashEndEvent != null)
		{
			this.OnDashEndEvent();
		}
	}

	// Token: 0x060036CE RID: 14030 RVA: 0x00100C80 File Offset: 0x000FEE80
	public void ForceParry()
	{
		if (this.hitManager.state != LevelPlayerMotor.HitManager.State.Hit && !this.hardExitParry)
		{
			this.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive;
			this.parryManager.state = LevelPlayerMotor.ParryManager.State.NotReady;
			this.Parrying = true;
			if (this.OnParryEvent != null)
			{
				this.OnParryEvent();
			}
		}
	}

	// Token: 0x060036CF RID: 14031 RVA: 0x00100CE0 File Offset: 0x000FEEE0
	public void ChaliceDashParry()
	{
		if (this.dashManager.IsDashing && !this.dashManager.chaliceParryCoolDown && this.hitManager.state != LevelPlayerMotor.HitManager.State.Hit && !this.hardExitParry)
		{
			this.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive;
			this.parryManager.state = LevelPlayerMotor.ParryManager.State.NotReady;
			this.Parrying = true;
			if (this.OnParryEvent != null)
			{
				this.OnParryEvent();
			}
			this.dashManager.chaliceParryCoolDown = true;
		}
	}

	// Token: 0x060036D0 RID: 14032 RVA: 0x0002CD50 File Offset: 0x0002AF50
	public void ResetChaliceDoubleJump()
	{
		this.jumpManager.doubleJumped = false;
		if (base.player.stats.isChalice)
		{
			this.dashManager.chaliceParryCoolDown = false;
			this.dashManager.chaliceParryCoolDownTimer = 0f;
		}
	}

	// Token: 0x060036D1 RID: 14033 RVA: 0x00100D6C File Offset: 0x000FEF6C
	public void ChaliceDashCooldownCheck()
	{
		if (this.dashManager.chaliceParryCoolDown)
		{
			this.dashManager.chaliceParryCoolDownTimer += CupheadTime.FixedDelta;
			if (this.dashManager.chaliceParryCoolDownTimer >= this.properties.dashParryCooldownTime)
			{
				this.dashManager.chaliceParryCoolDown = false;
				this.dashManager.chaliceParryCoolDownTimer = 0f;
			}
		}
	}

	// Token: 0x060036D2 RID: 14034 RVA: 0x0002CD8F File Offset: 0x0002AF8F
	public float DistanceToGround()
	{
		this.HandleRaycasts();
		return this.directionManager.down.distance;
	}

	// Token: 0x060036D3 RID: 14035 RVA: 0x0002CDA7 File Offset: 0x0002AFA7
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

	// Token: 0x060036D4 RID: 14036 RVA: 0x00100DD8 File Offset: 0x000FEFD8
	public void HandleWalking()
	{
		float move = 0f;
		if ((this.LookDirection.y >= 0 || !this.Grounded) && !this.Locked)
		{
			int num = (base.player.stats.ReverseTime > 0f) ? (-base.player.input.GetAxisInt(PlayerInput.Axis.X, false, false)) : base.player.input.GetAxisInt(PlayerInput.Axis.X, false, false);
			move = (float)num * this.properties.moveSpeed;
		}
		this.velocityManager.move = move;
	}

	// Token: 0x060036D5 RID: 14037 RVA: 0x00100E7C File Offset: 0x000FF07C
	public void HandleLooking()
	{
		if (base.player.levelStarted && this.allowInput)
		{
			int num = base.player.input.GetAxisInt(PlayerInput.Axis.X, false, false);
			num = ((base.player.stats.ReverseTime > 0f) ? (-num) : num);
			int num2 = base.player.input.GetAxisInt(PlayerInput.Axis.Y, true, this.Grounded && !this.Locked && !this.IsUsingSuperOrEx);
			num2 = ((base.player.stats.ReverseTime > 0f) ? (-num2) : num2);
			if (this.GravityReversed)
			{
				num2 *= -1;
			}
			this.LookDirection = new Trilean2(num, num2);
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

	// Token: 0x060036D6 RID: 14038 RVA: 0x0002CDE2 File Offset: 0x0002AFE2
	public void ForceLooking(Trilean2 direction)
	{
		this.LookDirection = direction;
		this.TrueLookDirection = direction;
		base.GetComponent<LevelPlayerAnimationController>().ForceDirection();
	}

	// Token: 0x060036D7 RID: 14039 RVA: 0x00100FCC File Offset: 0x000FF1CC
	public void HandleFalling()
	{
		if (this.Grounded || this.dashManager.IsDashing)
		{
			this.isFloating = false;
			this.jumpManager.floatTimer = 0f;
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
		if (base.player.stats.Loadout.charm == Charm.charm_float && this.jumpManager.ableToLand && base.player.input.actions.GetButton(2) && this.jumpManager.floatTimer < WeaponProperties.CharmFloat.maxTime)
		{
			this.isFloating = true;
			float num3 = Mathf.Clamp(this.jumpManager.floatTimer - WeaponProperties.CharmFloat.falloffStartTime, 0f, WeaponProperties.CharmFloat.maxTime - WeaponProperties.CharmFloat.falloffStartTime);
			num3 = Mathf.InverseLerp(0f, WeaponProperties.CharmFloat.maxTime - WeaponProperties.CharmFloat.falloffStartTime, num3);
			this.velocityManager.y = Mathf.Clamp(this.velocityManager.y, 0f, EaseUtils.EaseInSine(WeaponProperties.CharmFloat.minFallSpeed, WeaponProperties.CharmFloat.maxFallSpeed, num3));
			this.jumpManager.floatTimer += CupheadTime.FixedDelta;
		}
		else
		{
			this.isFloating = false;
		}
	}

	// Token: 0x060036D8 RID: 14040 RVA: 0x00101170 File Offset: 0x000FF370
	public void HandlePitKnockUp()
	{
		if (this.hitManager.state != LevelPlayerMotor.HitManager.State.KnockedUp)
		{
			return;
		}
		if (this.hitManager.timer > this.properties.knockUpStunTime)
		{
			this.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive;
			this.velocityManager.hit = 0f;
		}
		else
		{
			this.hitManager.timer += CupheadTime.FixedDelta;
		}
	}

	// Token: 0x060036D9 RID: 14041 RVA: 0x001011E4 File Offset: 0x000FF3E4
	public void HandleHit()
	{
		if (this.hitManager.state != LevelPlayerMotor.HitManager.State.Hit)
		{
			return;
		}
		if (this.hitManager.timer > this.properties.hitStunTime)
		{
			this.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive;
			this.velocityManager.hit = 0f;
		}
		else
		{
			float value = this.hitManager.timer / this.properties.hitStunTime;
			this.velocityManager.hit = EaseUtils.Ease(this.properties.hitKnockbackEase, this.properties.hitKnockbackPower, 0f, value) * (float)this.hitManager.direction;
			this.hitManager.timer += CupheadTime.FixedDelta;
		}
	}

	// Token: 0x060036DA RID: 14042 RVA: 0x001012A8 File Offset: 0x000FF4A8
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (base.player.stats.SuperInvincible)
		{
			return;
		}
		this.hitManager.state = LevelPlayerMotor.HitManager.State.Hit;
		if (this.OnHitEvent != null)
		{
			this.OnHitEvent();
		}
		this.DashComplete();
		this.velocityManager.Clear();
		this.ResetSuperAndEx();
		int direction = this.TrueLookDirection.x * -1;
		this.hitManager.direction = direction;
		this.LeaveGround(false);
		this.velocityManager.y = this.properties.hitJumpPower;
		this.hitManager.timer = 0f;
	}

	// Token: 0x060036DB RID: 14043 RVA: 0x00101354 File Offset: 0x000FF554
	public void OnPitKnockUp(float y, float velocityScale = 1f)
	{
		if (base.player.IsDead)
		{
			base.transform.SetPosition(null, new float?(y + 200f * this.GravityReversalMultiplier), null);
			return;
		}
		if (!base.player.stats.isChalice)
		{
			this.hardExitParry = true;
		}
		base.transform.SetPosition(null, new float?(y), null);
		this.hitManager.state = LevelPlayerMotor.HitManager.State.KnockedUp;
		this.DashComplete();
		this.velocityManager.Clear();
		this.ResetSuperAndEx();
		this.hitManager.direction = 0;
		this.LeaveGround(false);
		if (Level.Current.LevelType == Level.Type.Platforming)
		{
			this.velocityManager.y = this.properties.platformingPitKnockUpPower * velocityScale;
		}
		else
		{
			this.velocityManager.y = this.properties.pitKnockUpPower * velocityScale;
		}
		this.hitManager.timer = 0f;
		this.dashManager.state = LevelPlayerMotor.DashManager.State.Ready;
		this.parryManager.state = LevelPlayerMotor.ParryManager.State.Ready;
	}

	// Token: 0x060036DC RID: 14044 RVA: 0x00101484 File Offset: 0x000FF684
	public void OnTrampolineKnockUp(float y)
	{
		if (base.player.IsDead)
		{
			base.transform.SetPosition(null, new float?(y * this.GravityReversalMultiplier), null);
			return;
		}
		this.LeaveGround(false);
		this.hitManager.state = LevelPlayerMotor.HitManager.State.KnockedUp;
		this.DashComplete();
		this.velocityManager.Clear();
		this.ResetSuperAndEx();
		this.hitManager.direction = 0;
		this.velocityManager.y = y;
		this.hitManager.timer = 0f;
		this.dashManager.state = LevelPlayerMotor.DashManager.State.Ready;
		this.parryManager.state = LevelPlayerMotor.ParryManager.State.Ready;
		this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Ready;
	}

	// Token: 0x060036DD RID: 14045 RVA: 0x00101544 File Offset: 0x000FF744
	public IEnumerator launch_player_cr(float end)
	{
		float time = 0.1f;
		float t = 0f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			float posY = Mathf.Lerp(base.transform.position.y, end, t / time);
			base.transform.SetPosition(null, new float?(posY), null);
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060036DE RID: 14046 RVA: 0x00101568 File Offset: 0x000FF768
	public void OnRevive(Vector3 pos)
	{
		if (this.GravityReversed)
		{
			pos.y -= (base.player.center.y - base.transform.position.y) * 2f;
		}
		base.transform.position = pos;
		this.hitManager.state = LevelPlayerMotor.HitManager.State.KnockedUp;
		this.DashComplete();
		this.velocityManager.Clear();
		this.ResetSuperAndEx();
		this.hitManager.direction = 0;
		this.LeaveGround(false);
		this.velocityManager.y = this.properties.reviveKnockUpPower;
		this.hitManager.timer = 0f;
		base.player.animationController.UpdateAnimator();
	}

	// Token: 0x060036DF RID: 14047 RVA: 0x0002CDFD File Offset: 0x0002AFFD
	public void CancelReviveBounce()
	{
		this.velocityManager.y = 0f;
	}

	// Token: 0x060036E0 RID: 14048 RVA: 0x0002CE0F File Offset: 0x0002B00F
	public void AddForce(LevelPlayerMotor.VelocityManager.Force force)
	{
		this.velocityManager.AddForce(force);
	}

	// Token: 0x060036E1 RID: 14049 RVA: 0x0002CE1D File Offset: 0x0002B01D
	public void RemoveForce(LevelPlayerMotor.VelocityManager.Force force)
	{
		this.velocityManager.RemoveForce(force);
		force.yAxisForce = false;
	}

	// Token: 0x060036E2 RID: 14050 RVA: 0x00101634 File Offset: 0x000FF834
	public void ClearParent()
	{
		if (base.transform.parent != null)
		{
			base.transform.parent.GetComponent<LevelPlatform>().OnPlayerExit(base.transform);
		}
		base.transform.parent = null;
		Vector3 localScale = base.transform.localScale;
		localScale.y = 1f * this.GravityReversalMultiplier;
		base.transform.localScale = localScale;
	}

	// Token: 0x060036E3 RID: 14051 RVA: 0x0002CE32 File Offset: 0x0002B032
	public void OnPlatformingLevelExit()
	{
		base.StartCoroutine(this.platformingExit_cr());
	}

	// Token: 0x060036E4 RID: 14052 RVA: 0x001016AC File Offset: 0x000FF8AC
	public IEnumerator platformingExit_cr()
	{
		for (;;)
		{
			if (this.Dashing)
			{
				this.DashComplete();
			}
			this.allowInput = false;
			this.Locked = false;
			this.LookDirection = new Trilean2(1, 0);
			this.velocityManager.move = this.properties.moveSpeed;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x060036E5 RID: 14053 RVA: 0x001016C8 File Offset: 0x000FF8C8
	public IEnumerator parryHit_cr()
	{
		this.velocityManager.Clear();
		yield return null;
		this.velocityManager.Clear();
		yield break;
	}

	// Token: 0x04002C2E RID: 11310
	[SerializeField]
	public LevelPlayerMotor.Properties properties;

	// Token: 0x04002C2F RID: 11311
	public Vector2 lastPositionFixed;

	// Token: 0x04002C30 RID: 11312
	public Vector2 lastPosition;

	// Token: 0x04002C31 RID: 11313
	public LevelPlayerMotor.VelocityManager velocityManager;

	// Token: 0x04002C32 RID: 11314
	public LevelPlayerMotor.JumpManager jumpManager;

	// Token: 0x04002C33 RID: 11315
	public LevelPlayerMotor.DashManager dashManager;

	// Token: 0x04002C34 RID: 11316
	public LevelPlayerMotor.ParryManager parryManager;

	// Token: 0x04002C35 RID: 11317
	public LevelPlayerMotor.DirectionManager directionManager;

	// Token: 0x04002C36 RID: 11318
	public LevelPlayerMotor.PlatformManager platformManager;

	// Token: 0x04002C37 RID: 11319
	public LevelPlayerMotor.HitManager hitManager;

	// Token: 0x04002C38 RID: 11320
	public LevelPlayerMotor.SuperManager superManager;

	// Token: 0x04002C39 RID: 11321
	public LevelPlayerMotor.BoundsManager boundsManager;

	// Token: 0x04002C3A RID: 11322
	public bool allowInput;

	// Token: 0x04002C3B RID: 11323
	public bool allowJumping;

	// Token: 0x04002C3C RID: 11324
	public bool allowFalling;

	// Token: 0x04002C3D RID: 11325
	public bool forceLaunchUp;

	// Token: 0x04002C3E RID: 11326
	public bool hardExitParry;

	// Token: 0x04002C3F RID: 11327
	public bool reversingGravity;

	// Token: 0x04002C40 RID: 11328
	public float jumpPower;

	// Token: 0x04002C41 RID: 11329
	public RaycastHit2D[] hitBuffer = new RaycastHit2D[25];

	// Token: 0x04002C4A RID: 11338
	public LevelPlayerParryController parryController;

	// Token: 0x04002C4C RID: 11340
	public const float RAY_DISTANCE = 2000f;

	// Token: 0x04002C4D RID: 11341
	public const float MAX_GROUNDED_FALL_DISTANCE = 30f;

	// Token: 0x04002C4E RID: 11342
	public readonly int wallMask = 262144;

	// Token: 0x04002C4F RID: 11343
	public readonly int ceilingMask = 524288;

	// Token: 0x04002C50 RID: 11344
	public readonly int groundMask = 1048576;

	// Token: 0x04002C51 RID: 11345
	public LevelPlayerWeaponManager.Pose exFirePose;

	// Token: 0x04002C52 RID: 11346
	public const float JUMP_BUFFER_TIME = 0.0834f;

	// Token: 0x04002C53 RID: 11347
	public const float INPUT_BUFFER_TIME = 0.134f;

	// Token: 0x04002C54 RID: 11348
	public LevelPlayerMotor.BufferedInput bufferedInput;

	// Token: 0x04002C55 RID: 11349
	public float timeSinceInputBuffered = 0.134f;

	// Token: 0x0200118A RID: 4490
	public enum RaycastAxis
	{
		// Token: 0x04007AF4 RID: 31476
		X,
		// Token: 0x04007AF5 RID: 31477
		Y
	}

	// Token: 0x0200118B RID: 4491
	public enum BufferedInput
	{
		// Token: 0x04007AF7 RID: 31479
		Jump,
		// Token: 0x04007AF8 RID: 31480
		Dash,
		// Token: 0x04007AF9 RID: 31481
		Super
	}

	// Token: 0x0200118C RID: 4492
	public class Properties
	{
		// Token: 0x04007AFA RID: 31482
		public float moveSpeed = 490f;

		// Token: 0x04007AFB RID: 31483
		public float maxSpeedY = 1620f;

		// Token: 0x04007AFC RID: 31484
		public float timeToMaxY = 7.3f;

		// Token: 0x04007AFD RID: 31485
		public EaseUtils.EaseType yEase = EaseUtils.EaseType.linear;

		// Token: 0x04007AFE RID: 31486
		public float jumpHoldMin = 0.01f;

		// Token: 0x04007AFF RID: 31487
		public float jumpHoldMax = 0.16f;

		// Token: 0x04007B00 RID: 31488
		[Range(0f, -1f)]
		public float jumpPower = -0.755f;

		// Token: 0x04007B01 RID: 31489
		public float chaliceFirstJumpPower = -0.63f;

		// Token: 0x04007B02 RID: 31490
		public float chaliceSecondJumpPower = -0.55f;

		// Token: 0x04007B03 RID: 31491
		public float dashSpeed = 1100f;

		// Token: 0x04007B04 RID: 31492
		public float verticalDashSpeed = 935f;

		// Token: 0x04007B05 RID: 31493
		public float dashTime = 0.3f;

		// Token: 0x04007B06 RID: 31494
		public float dashEndTime = 0.21f;

		// Token: 0x04007B07 RID: 31495
		public EaseUtils.EaseType dashEase = EaseUtils.EaseType.easeOutSine;

		// Token: 0x04007B08 RID: 31496
		public float dashParryCooldownTime = 0.3f;

		// Token: 0x04007B09 RID: 31497
		public float platformIgnoreTime = 1f;

		// Token: 0x04007B0A RID: 31498
		public float hitStunTime = 0.3f;

		// Token: 0x04007B0B RID: 31499
		public float hitFalloff = 0.25f;

		// Token: 0x04007B0C RID: 31500
		[Range(0f, -1f)]
		public float hitJumpPower = -0.6f;

		// Token: 0x04007B0D RID: 31501
		public float hitKnockbackPower = 300f;

		// Token: 0x04007B0E RID: 31502
		public EaseUtils.EaseType hitKnockbackEase = EaseUtils.EaseType.linear;

		// Token: 0x04007B0F RID: 31503
		public float knockUpStunTime = 0.2f;

		// Token: 0x04007B10 RID: 31504
		[Range(0f, -3f)]
		public float pitKnockUpPower = -1.5f;

		// Token: 0x04007B11 RID: 31505
		[Range(0f, -3f)]
		public float platformingPitKnockUpPower = -1.5f;

		// Token: 0x04007B12 RID: 31506
		public float parryPower = -1f;

		// Token: 0x04007B13 RID: 31507
		public float parryAttackBounce = -1f;

		// Token: 0x04007B14 RID: 31508
		public float deathSpeed = 5f;

		// Token: 0x04007B15 RID: 31509
		public float reviveKnockUpPower = -1f;

		// Token: 0x04007B16 RID: 31510
		public float exKnockback = 230f;

		// Token: 0x04007B17 RID: 31511
		public float superKnockUp = -0.6f;

		// Token: 0x04007B18 RID: 31512
		public float superInvincibleKnockUp = -1.2f;
	}

	// Token: 0x0200118D RID: 4493
	public class VelocityManager
	{
		// Token: 0x06007DFE RID: 32254 RVA: 0x00054954 File Offset: 0x00052B54
		public VelocityManager(LevelPlayerMotor motor, float maxY, EaseUtils.EaseType yEase)
		{
			this.maxY = maxY;
			this.yEase = yEase;
			this.forces = new List<LevelPlayerMotor.VelocityManager.Force>();
		}

		// Token: 0x1700180F RID: 6159
		// (get) Token: 0x06007DFF RID: 32255 RVA: 0x00054975 File Offset: 0x00052B75
		// (set) Token: 0x06007E00 RID: 32256 RVA: 0x0005497D File Offset: 0x00052B7D
		public bool yAxisForce { get; set; }

		// Token: 0x17001810 RID: 6160
		// (get) Token: 0x06007E01 RID: 32257 RVA: 0x00054986 File Offset: 0x00052B86
		// (set) Token: 0x06007E02 RID: 32258 RVA: 0x0005498E File Offset: 0x00052B8E
		public float GroundForce { get; set; }

		// Token: 0x17001811 RID: 6161
		// (get) Token: 0x06007E03 RID: 32259 RVA: 0x00054997 File Offset: 0x00052B97
		// (set) Token: 0x06007E04 RID: 32260 RVA: 0x0005499F File Offset: 0x00052B9F
		public float AirForce { get; set; }

		// Token: 0x17001812 RID: 6162
		// (get) Token: 0x06007E05 RID: 32261 RVA: 0x000549A8 File Offset: 0x00052BA8
		// (set) Token: 0x06007E06 RID: 32262 RVA: 0x000549CB File Offset: 0x00052BCB
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

		// Token: 0x06007E07 RID: 32263 RVA: 0x0028E144 File Offset: 0x0028C344
		public void Calculate()
		{
			this.GroundForce = 0f;
			this.AirForce = 0f;
			foreach (LevelPlayerMotor.VelocityManager.Force force in this.forces)
			{
				if (force.enabled)
				{
					LevelPlayerMotor.VelocityManager.Force.Type type = force.type;
					if (type != LevelPlayerMotor.VelocityManager.Force.Type.All)
					{
						if (type != LevelPlayerMotor.VelocityManager.Force.Type.Air)
						{
							if (type == LevelPlayerMotor.VelocityManager.Force.Type.Ground)
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
					if (force.yAxisForce)
					{
						this.yAxisForce = true;
					}
				}
			}
		}

		// Token: 0x17001813 RID: 6163
		// (get) Token: 0x06007E08 RID: 32264 RVA: 0x0028E24C File Offset: 0x0028C44C
		public Vector2 Total
		{
			get
			{
				float value = this.y / 2f + 0.5f;
				Vector2 result = default(Vector2);
				result.y = EaseUtils.Ease(this.yEase, this.maxY, -this.maxY, value) + this.verticalDash;
				result.x += this.move + this.dash + this.hit;
				return result;
			}
		}

		// Token: 0x06007E09 RID: 32265 RVA: 0x000549E3 File Offset: 0x00052BE3
		public void Clear()
		{
			this.move = 0f;
			this.dash = 0f;
			this.hit = 0f;
			this.y = 0f;
		}

		// Token: 0x06007E0A RID: 32266 RVA: 0x00054A11 File Offset: 0x00052C11
		public void AddForce(LevelPlayerMotor.VelocityManager.Force force)
		{
			if (this.forces.Contains(force))
			{
				return;
			}
			this.forces.Add(force);
		}

		// Token: 0x06007E0B RID: 32267 RVA: 0x00054A31 File Offset: 0x00052C31
		public void RemoveForce(LevelPlayerMotor.VelocityManager.Force force)
		{
			this.yAxisForce = false;
			if (this.forces.Contains(force))
			{
				this.forces.Remove(force);
			}
		}

		// Token: 0x04007B1C RID: 31516
		public float move;

		// Token: 0x04007B1D RID: 31517
		public float dash;

		// Token: 0x04007B1E RID: 31518
		public float verticalDash;

		// Token: 0x04007B1F RID: 31519
		public float hit;

		// Token: 0x04007B20 RID: 31520
		public List<LevelPlayerMotor.VelocityManager.Force> forces;

		// Token: 0x04007B21 RID: 31521
		public EaseUtils.EaseType yEase;

		// Token: 0x04007B22 RID: 31522
		public float maxY;

		// Token: 0x04007B23 RID: 31523
		public float _y;

		// Token: 0x020015EA RID: 5610
		public class Force
		{
			// Token: 0x06008777 RID: 34679 RVA: 0x0005BBCA File Offset: 0x00059DCA
			public Force()
			{
				this.type = LevelPlayerMotor.VelocityManager.Force.Type.All;
				this.value = 0f;
			}

			// Token: 0x06008778 RID: 34680 RVA: 0x0005BBEB File Offset: 0x00059DEB
			public Force(LevelPlayerMotor.VelocityManager.Force.Type type)
			{
				this.type = type;
				this.value = 0f;
			}

			// Token: 0x06008779 RID: 34681 RVA: 0x0005BC0C File Offset: 0x00059E0C
			public Force(LevelPlayerMotor.VelocityManager.Force.Type type, float force)
			{
				this.type = type;
				this.value = force;
			}

			// Token: 0x0600877A RID: 34682 RVA: 0x0005BC29 File Offset: 0x00059E29
			public Force(LevelPlayerMotor.VelocityManager.Force.Type type, float force, bool yAxis)
			{
				this.type = type;
				this.value = force;
				this.yAxisForce = yAxis;
			}

			// Token: 0x04009206 RID: 37382
			public bool yAxisForce;

			// Token: 0x04009207 RID: 37383
			public bool enabled = true;

			// Token: 0x04009208 RID: 37384
			public readonly LevelPlayerMotor.VelocityManager.Force.Type type;

			// Token: 0x04009209 RID: 37385
			public float value;

			// Token: 0x0200160B RID: 5643
			public enum Type
			{
				// Token: 0x0400929D RID: 37533
				All,
				// Token: 0x0400929E RID: 37534
				Ground,
				// Token: 0x0400929F RID: 37535
				Air
			}
		}
	}

	// Token: 0x0200118E RID: 4494
	public class JumpManager
	{
		// Token: 0x04007B24 RID: 31524
		public LevelPlayerMotor.JumpManager.State state;

		// Token: 0x04007B25 RID: 31525
		public float timer;

		// Token: 0x04007B26 RID: 31526
		public float timeSinceDownJump = 1000f;

		// Token: 0x04007B27 RID: 31527
		public float timeInAir;

		// Token: 0x04007B28 RID: 31528
		public float longestTimeInAir;

		// Token: 0x04007B29 RID: 31529
		public bool ableToLand;

		// Token: 0x04007B2A RID: 31530
		public float floatTimer;

		// Token: 0x04007B2B RID: 31531
		public bool doubleJumped;

		// Token: 0x020015EB RID: 5611
		public enum State
		{
			// Token: 0x0400920B RID: 37387
			Ready,
			// Token: 0x0400920C RID: 37388
			Hold,
			// Token: 0x0400920D RID: 37389
			Used
		}
	}

	// Token: 0x0200118F RID: 4495
	public class DashManager
	{
		// Token: 0x17001814 RID: 6164
		// (get) Token: 0x06007E0E RID: 32270 RVA: 0x0028E2C0 File Offset: 0x0028C4C0
		public bool IsDashing
		{
			get
			{
				LevelPlayerMotor.DashManager.State state = this.state;
				return state == LevelPlayerMotor.DashManager.State.Start || state == LevelPlayerMotor.DashManager.State.Dashing || state == LevelPlayerMotor.DashManager.State.Ending;
			}
		}

		// Token: 0x04007B2C RID: 31532
		public LevelPlayerMotor.DashManager.State state;

		// Token: 0x04007B2D RID: 31533
		public int direction;

		// Token: 0x04007B2E RID: 31534
		public float timer;

		// Token: 0x04007B2F RID: 31535
		public const float DASH_COOLDOWN_DURATION = 0.1f;

		// Token: 0x04007B30 RID: 31536
		public float timeSinceGroundDash = 0.1f;

		// Token: 0x04007B31 RID: 31537
		public bool groundDash;

		// Token: 0x04007B32 RID: 31538
		public float chaliceParryCoolDownTimer;

		// Token: 0x04007B33 RID: 31539
		public bool chaliceParryCoolDown;

		// Token: 0x020015EC RID: 5612
		public enum State
		{
			// Token: 0x0400920F RID: 37391
			Ready,
			// Token: 0x04009210 RID: 37392
			Start,
			// Token: 0x04009211 RID: 37393
			Dashing,
			// Token: 0x04009212 RID: 37394
			Ending,
			// Token: 0x04009213 RID: 37395
			End
		}
	}

	// Token: 0x02001190 RID: 4496
	public class ParryManager
	{
		// Token: 0x04007B34 RID: 31540
		public LevelPlayerMotor.ParryManager.State state;

		// Token: 0x020015ED RID: 5613
		public enum State
		{
			// Token: 0x04009215 RID: 37397
			Ready,
			// Token: 0x04009216 RID: 37398
			NotReady
		}
	}

	// Token: 0x02001191 RID: 4497
	public class PlatformManager
	{
		// Token: 0x06007E10 RID: 32272 RVA: 0x00054A86 File Offset: 0x00052C86
		public PlatformManager(LevelPlayerMotor motor)
		{
			this.ignoredPlatforms = new List<Transform>();
			this.motor = motor;
		}

		// Token: 0x17001815 RID: 6165
		// (get) Token: 0x06007E11 RID: 32273 RVA: 0x00054AA0 File Offset: 0x00052CA0
		public bool OnPlatform
		{
			get
			{
				return this.motor.transform.parent != null;
			}
		}

		// Token: 0x06007E12 RID: 32274 RVA: 0x00054AB8 File Offset: 0x00052CB8
		public void Ignore(Transform platform)
		{
			this.StopCoroutine();
			this.ignoreCoroutine = this.ignorePlatform_cr(platform);
			this.motor.StartCoroutine(this.ignoreCoroutine);
		}

		// Token: 0x06007E13 RID: 32275 RVA: 0x00054ADF File Offset: 0x00052CDF
		public void StopCoroutine()
		{
			if (this.ignoreCoroutine != null)
			{
				this.motor.StopCoroutine(this.ignoreCoroutine);
			}
			this.ignoreCoroutine = null;
		}

		// Token: 0x06007E14 RID: 32276 RVA: 0x00054B04 File Offset: 0x00052D04
		public void Add(Transform platform)
		{
			this.ignoredPlatforms.Add(platform);
		}

		// Token: 0x06007E15 RID: 32277 RVA: 0x00054B12 File Offset: 0x00052D12
		public void Remove(Transform platform)
		{
			this.ignoredPlatforms.Remove(platform);
		}

		// Token: 0x06007E16 RID: 32278 RVA: 0x00054B21 File Offset: 0x00052D21
		public bool IsPlatformIgnored(Transform platform)
		{
			return this.ignoredPlatforms.Contains(platform);
		}

		// Token: 0x06007E17 RID: 32279 RVA: 0x00054B2F File Offset: 0x00052D2F
		public void ResetAll()
		{
			this.StopCoroutine();
			this.ignoredPlatforms = new List<Transform>();
		}

		// Token: 0x06007E18 RID: 32280 RVA: 0x0028E2F4 File Offset: 0x0028C4F4
		public IEnumerator ignorePlatform_cr(Transform platform)
		{
			this.Add(platform);
			yield return CupheadTime.WaitForSeconds(this.motor, this.motor.properties.platformIgnoreTime);
			this.Remove(platform);
			yield break;
		}

		// Token: 0x04007B35 RID: 31541
		public List<Transform> ignoredPlatforms;

		// Token: 0x04007B36 RID: 31542
		public LevelPlayerMotor motor;

		// Token: 0x04007B37 RID: 31543
		public IEnumerator ignoreCoroutine;
	}

	// Token: 0x02001192 RID: 4498
	public class DirectionManager
	{
		// Token: 0x06007E19 RID: 32281 RVA: 0x00054B42 File Offset: 0x00052D42
		public DirectionManager()
		{
			this.Reset();
		}

		// Token: 0x06007E1A RID: 32282 RVA: 0x00054B7C File Offset: 0x00052D7C
		public void Reset()
		{
			this.up.Reset();
			this.down.Reset();
			this.left.Reset();
			this.right.Reset();
		}

		// Token: 0x04007B38 RID: 31544
		public LevelPlayerMotor.DirectionManager.Hit up = new LevelPlayerMotor.DirectionManager.Hit();

		// Token: 0x04007B39 RID: 31545
		public LevelPlayerMotor.DirectionManager.Hit down = new LevelPlayerMotor.DirectionManager.Hit();

		// Token: 0x04007B3A RID: 31546
		public LevelPlayerMotor.DirectionManager.Hit left = new LevelPlayerMotor.DirectionManager.Hit();

		// Token: 0x04007B3B RID: 31547
		public LevelPlayerMotor.DirectionManager.Hit right = new LevelPlayerMotor.DirectionManager.Hit();

		// Token: 0x020015EF RID: 5615
		public class Hit
		{
			// Token: 0x06008781 RID: 34689 RVA: 0x0005BC7C File Offset: 0x00059E7C
			public Hit()
			{
				this.Reset();
			}

			// Token: 0x06008782 RID: 34690 RVA: 0x0005BC8A File Offset: 0x00059E8A
			public Hit(bool able, Vector2 pos, GameObject gameObject, float distance)
			{
				this.able = able;
				this.pos = pos;
				this.gameObject = gameObject;
				this.distance = distance;
			}

			// Token: 0x06008783 RID: 34691 RVA: 0x0005BCAF File Offset: 0x00059EAF
			public void Reset()
			{
				this.able = true;
				this.pos = Vector2.zero;
				this.gameObject = null;
				this.distance = -1f;
			}

			// Token: 0x0400921C RID: 37404
			public bool able;

			// Token: 0x0400921D RID: 37405
			public Vector2 pos;

			// Token: 0x0400921E RID: 37406
			public GameObject gameObject;

			// Token: 0x0400921F RID: 37407
			public float distance;
		}
	}

	// Token: 0x02001193 RID: 4499
	public class HitManager
	{
		// Token: 0x06007E1C RID: 32284 RVA: 0x00054BB2 File Offset: 0x00052DB2
		public void Reset()
		{
			this.state = LevelPlayerMotor.HitManager.State.Inactive;
			this.timer = 0f;
			this.direction = 0;
		}

		// Token: 0x04007B3C RID: 31548
		public LevelPlayerMotor.HitManager.State state;

		// Token: 0x04007B3D RID: 31549
		public float timer;

		// Token: 0x04007B3E RID: 31550
		public int direction;

		// Token: 0x020015F0 RID: 5616
		public enum State
		{
			// Token: 0x04009221 RID: 37409
			Inactive,
			// Token: 0x04009222 RID: 37410
			Hit,
			// Token: 0x04009223 RID: 37411
			KnockedUp
		}
	}

	// Token: 0x02001194 RID: 4500
	public class SuperManager
	{
		// Token: 0x04007B3F RID: 31551
		public LevelPlayerMotor.SuperManager.State state;

		// Token: 0x020015F1 RID: 5617
		public enum State
		{
			// Token: 0x04009225 RID: 37413
			Ready,
			// Token: 0x04009226 RID: 37414
			Ex,
			// Token: 0x04009227 RID: 37415
			Super
		}
	}

	// Token: 0x02001195 RID: 4501
	public class BoundsManager
	{
		// Token: 0x06007E1E RID: 32286 RVA: 0x00054BD5 File Offset: 0x00052DD5
		public BoundsManager(LevelPlayerMotor motor)
		{
			this.Motor = motor;
			this.transform = motor.transform;
			this.boxCollider = (this.transform.GetComponent<Collider2D>() as BoxCollider2D);
		}

		// Token: 0x17001816 RID: 6166
		// (get) Token: 0x06007E1F RID: 32287 RVA: 0x00054C06 File Offset: 0x00052E06
		// (set) Token: 0x06007E20 RID: 32288 RVA: 0x00054C0E File Offset: 0x00052E0E
		public LevelPlayerMotor Motor { get; set; }

		// Token: 0x17001817 RID: 6167
		// (get) Token: 0x06007E21 RID: 32289 RVA: 0x0028E318 File Offset: 0x0028C518
		public Vector3 Top
		{
			get
			{
				return new Vector3(this.Center.x, this.Center.y + this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x17001818 RID: 6168
		// (get) Token: 0x06007E22 RID: 32290 RVA: 0x0028E368 File Offset: 0x0028C568
		public Vector3 TopLeft
		{
			get
			{
				return new Vector3(this.Center.x - this.boxCollider.size.x / 2f, this.Center.y + this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x17001819 RID: 6169
		// (get) Token: 0x06007E23 RID: 32291 RVA: 0x0028E3D0 File Offset: 0x0028C5D0
		public Vector3 TopRight
		{
			get
			{
				return new Vector3(this.Center.x + this.boxCollider.size.x / 2f, this.Center.y + this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x1700181A RID: 6170
		// (get) Token: 0x06007E24 RID: 32292 RVA: 0x0028E438 File Offset: 0x0028C638
		public Vector3 CenterLeft
		{
			get
			{
				return new Vector3(this.Center.x - this.boxCollider.size.x / 2f, this.Center.y, 0f);
			}
		}

		// Token: 0x1700181B RID: 6171
		// (get) Token: 0x06007E25 RID: 32293 RVA: 0x0028E488 File Offset: 0x0028C688
		public Vector3 CenterRight
		{
			get
			{
				return new Vector3(this.Center.x + this.boxCollider.size.x / 2f, this.Center.y, 0f);
			}
		}

		// Token: 0x1700181C RID: 6172
		// (get) Token: 0x06007E26 RID: 32294 RVA: 0x0028E4D8 File Offset: 0x0028C6D8
		public Vector2 Center
		{
			get
			{
				return this.transform.position + new Vector2(this.boxCollider.offset.x, this.Motor.GravityReversalMultiplier * this.boxCollider.offset.y);
			}
		}

		// Token: 0x1700181D RID: 6173
		// (get) Token: 0x06007E27 RID: 32295 RVA: 0x0028E534 File Offset: 0x0028C734
		public Vector3 Bottom
		{
			get
			{
				return new Vector3(this.Center.x, this.Center.y - this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x1700181E RID: 6174
		// (get) Token: 0x06007E28 RID: 32296 RVA: 0x0028E584 File Offset: 0x0028C784
		public Vector3 BottomLeft
		{
			get
			{
				return new Vector3(this.Center.x - this.boxCollider.size.x / 2f, this.Center.y - this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x1700181F RID: 6175
		// (get) Token: 0x06007E29 RID: 32297 RVA: 0x0028E5EC File Offset: 0x0028C7EC
		public Vector3 BottomRight
		{
			get
			{
				return new Vector3(this.Center.x + this.boxCollider.size.x / 2f, this.Center.y - this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x17001820 RID: 6176
		// (get) Token: 0x06007E2A RID: 32298 RVA: 0x0028E654 File Offset: 0x0028C854
		public float TopY
		{
			get
			{
				return this.Top.y - this.transform.position.y;
			}
		}

		// Token: 0x17001821 RID: 6177
		// (get) Token: 0x06007E2B RID: 32299 RVA: 0x0028E684 File Offset: 0x0028C884
		public float BottomY
		{
			get
			{
				return this.Bottom.y - this.transform.position.y;
			}
		}

		// Token: 0x04007B40 RID: 31552
		public readonly Transform transform;

		// Token: 0x04007B41 RID: 31553
		public BoxCollider2D boxCollider;
	}
}
