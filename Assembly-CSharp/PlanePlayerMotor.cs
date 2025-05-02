using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000563 RID: 1379
public class PlanePlayerMotor : AbstractPlanePlayerComponent
{
	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x060039BE RID: 14782 RVA: 0x0002F0AE File Offset: 0x0002D2AE
	// (set) Token: 0x060039BF RID: 14783 RVA: 0x0002F0B6 File Offset: 0x0002D2B6
	public Trilean2 MoveDirection { get; set; }

	// Token: 0x060039C0 RID: 14784 RVA: 0x0010D1C0 File Offset: 0x0010B3C0
	public override void OnAwake()
	{
		base.OnAwake();
		this.MoveDirection = default(Trilean2);
		this.properties = new PlanePlayerMotor.Properties();
		base.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.player.OnReviveEvent += this.OnRevive;
		this.pos = base.transform.position;
	}

	// Token: 0x060039C1 RID: 14785 RVA: 0x0002F0BF File Offset: 0x0002D2BF
	public void Start()
	{
		this.pos = base.transform.position;
	}

	// Token: 0x060039C2 RID: 14786 RVA: 0x0002F0D7 File Offset: 0x0002D2D7
	public void FixedUpdate()
	{
		if (base.player.stats.StoneTime > 0f)
		{
			return;
		}
		this.HandleInput();
		this.Move();
		this.HandleRaycasts();
		this.ClampPosition();
	}

	// Token: 0x060039C3 RID: 14787 RVA: 0x0002F10C File Offset: 0x0002D30C
	public void LateUpdate()
	{
		this.ClampPosition();
	}

	// Token: 0x060039C4 RID: 14788 RVA: 0x0002F114 File Offset: 0x0002D314
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (info.damage > 0f)
		{
			base.StartCoroutine(this.onDamageTaken_cr());
		}
	}

	// Token: 0x060039C5 RID: 14789 RVA: 0x0010D238 File Offset: 0x0010B438
	public void HandleInput()
	{
		this.timeSinceInputBuffered += CupheadTime.FixedDelta;
		if (base.player.WeaponBusy)
		{
			this.BufferInputs();
		}
		if (this.damageStun)
		{
			this.MoveDirection = new Trilean2(-1, 0);
			return;
		}
		Trilean trilean = 0;
		Trilean trilean2 = 0;
		float axis = base.player.input.actions.GetAxis(0);
		float axis2 = base.player.input.actions.GetAxis(1);
		if (axis > 0.35f || axis < -0.35f)
		{
			trilean = axis;
		}
		if (axis2 > 0.35f || axis2 < -0.35f)
		{
			trilean2 = axis2;
		}
		this.MoveDirection = new Trilean2(trilean.Value, trilean2.Value);
	}

	// Token: 0x060039C6 RID: 14790 RVA: 0x0002F133 File Offset: 0x0002D333
	public void BufferInput(PlanePlayerMotor.BufferedInput input)
	{
		this.bufferedInput = input;
		this.timeSinceInputBuffered = 0f;
	}

	// Token: 0x060039C7 RID: 14791 RVA: 0x0010D318 File Offset: 0x0010B518
	public void BufferInputs()
	{
		if (base.player.input.actions.GetButtonDown(2))
		{
			this.BufferInput(PlanePlayerMotor.BufferedInput.Jump);
		}
		else if (base.player.input.actions.GetButtonDown(4))
		{
			this.BufferInput(PlanePlayerMotor.BufferedInput.Super);
		}
	}

	// Token: 0x060039C8 RID: 14792 RVA: 0x0002F147 File Offset: 0x0002D347
	public void ClearBufferedInput()
	{
		this.timeSinceInputBuffered = 0.134f;
	}

	// Token: 0x060039C9 RID: 14793 RVA: 0x0002F154 File Offset: 0x0002D354
	public bool HasBufferedInput(PlanePlayerMotor.BufferedInput input)
	{
		return this.bufferedInput == input && this.timeSinceInputBuffered < 0.134f;
	}

	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x060039CA RID: 14794 RVA: 0x0002F172 File Offset: 0x0002D372
	public Vector2 Velocity
	{
		get
		{
			return this._velocity;
		}
	}

	// Token: 0x060039CB RID: 14795 RVA: 0x0010D370 File Offset: 0x0010B570
	public void Move()
	{
		float num = (!base.player.Shrunk) ? this.properties.speed : this.properties.shrunkSpeed;
		if (this.MoveDirection.x != 0 && this.MoveDirection.y != 0)
		{
			num *= 0.75f;
		}
		this.pos.x = this.pos.x + this.MoveDirection.x * num * CupheadTime.FixedDelta;
		this.pos.y = this.pos.y + this.MoveDirection.y * num * CupheadTime.FixedDelta;
		foreach (PlanePlayerMotor.Force force in this.externalForces)
		{
			if (force.enabled)
			{
				this.pos += force.force * CupheadTime.FixedDelta;
			}
		}
		Vector2 vector = base.transform.position;
		if (PlanePlayerMotor.USE_FALLOFF)
		{
			float num2 = 15f;
			base.transform.position = Vector2.Lerp(base.transform.position, this.pos, num2 * CupheadTime.FixedDelta);
		}
		else
		{
			base.transform.AddPosition(0f, this.MoveDirection.y * num * CupheadTime.FixedDelta, 0f);
		}
		Vector2 vector2 = base.transform.position;
		this._velocity = (vector2 - vector) / CupheadTime.FixedDelta;
	}

	// Token: 0x060039CC RID: 14796 RVA: 0x0010D56C File Offset: 0x0010B76C
	public void HandleRaycasts()
	{
		int num = 262144;
		int num2 = 524288;
		int num3 = 1048576;
		Vector2 vector = base.transform.position + new Vector2(-20f, 15f);
		float num4 = 100f;
		float num5 = 100f;
		RaycastHit2D raycastHit2D = Physics2D.BoxCast(vector, new Vector2(1f, num5), 0f, Vector2.left, num4 * 0.5f, num);
		RaycastHit2D raycastHit2D2 = Physics2D.BoxCast(vector, new Vector2(1f, num5), 0f, Vector2.right, num4 * 0.5f, num);
		RaycastHit2D raycastHit2D3 = Physics2D.BoxCast(vector, new Vector2(num4, 1f), 0f, Vector2.up, num5 * 0.5f, num2);
		RaycastHit2D raycastHit2D4 = Physics2D.BoxCast(vector, new Vector2(num4, 1f), 0f, Vector2.down, num5 * 0.5f, num3);
		if (raycastHit2D.collider != null)
		{
			base.transform.SetPosition(new float?(raycastHit2D.point.x + 70f), null, null);
		}
		if (raycastHit2D2.collider != null)
		{
			base.transform.SetPosition(new float?(raycastHit2D2.point.x - 30f), null, null);
		}
		if (raycastHit2D3.collider != null)
		{
			base.transform.SetPosition(null, new float?(raycastHit2D3.point.y - 65f), null);
		}
		if (raycastHit2D4.collider != null)
		{
			base.transform.SetPosition(null, new float?(raycastHit2D4.point.y + 35f), null);
		}
	}

	// Token: 0x060039CD RID: 14797 RVA: 0x0010D78C File Offset: 0x0010B98C
	public void ClampPosition()
	{
		Vector2 vector = this.pos;
		vector.x = Mathf.Clamp(vector.x, (float)Level.Current.Left + 70f, (float)Level.Current.Right - 30f);
		vector.y = Mathf.Clamp(vector.y, (float)Level.Current.Ground + 35f, (float)Level.Current.Ceiling - 65f);
		this.pos = vector;
	}

	// Token: 0x060039CE RID: 14798 RVA: 0x0010D814 File Offset: 0x0010BA14
	public void OnRevive(Vector3 pos)
	{
		base.transform.position = pos;
		this.pos = pos;
		this.MoveDirection = default(Trilean2);
		this.damageStun = false;
	}

	// Token: 0x060039CF RID: 14799 RVA: 0x0010D850 File Offset: 0x0010BA50
	public IEnumerator onDamageTaken_cr()
	{
		this.damageStun = true;
		yield return CupheadTime.WaitForSeconds(this, 0.15f);
		this.damageStun = false;
		yield break;
	}

	// Token: 0x060039D0 RID: 14800 RVA: 0x0002F17A File Offset: 0x0002D37A
	public void AddForce(PlanePlayerMotor.Force force)
	{
		this.externalForces.Add(force);
	}

	// Token: 0x060039D1 RID: 14801 RVA: 0x0002F188 File Offset: 0x0002D388
	public void RemoveForce(PlanePlayerMotor.Force force)
	{
		this.externalForces.Remove(force);
	}

	// Token: 0x04002E5F RID: 11871
	public const float PADDING_TOP = 65f;

	// Token: 0x04002E60 RID: 11872
	public const float PADDING_BOTTOM = 35f;

	// Token: 0x04002E61 RID: 11873
	public const float PADDING_LEFT = 70f;

	// Token: 0x04002E62 RID: 11874
	public const float PADDING_RIGHT = 30f;

	// Token: 0x04002E63 RID: 11875
	public const float DIAGONAL_FALLOFF = 0.75f;

	// Token: 0x04002E64 RID: 11876
	public const float ANALOG_THRESHOLD = 0.35f;

	// Token: 0x04002E65 RID: 11877
	public const float HIT_STUN_TIME = 0.15f;

	// Token: 0x04002E66 RID: 11878
	public const float EASING_TIME = 15f;

	// Token: 0x04002E67 RID: 11879
	public static bool USE_FALLOFF = true;

	// Token: 0x04002E68 RID: 11880
	[NonSerialized]
	public PlanePlayerMotor.Properties properties;

	// Token: 0x04002E6A RID: 11882
	public bool damageStun;

	// Token: 0x04002E6B RID: 11883
	public Vector2 pos;

	// Token: 0x04002E6C RID: 11884
	public List<PlanePlayerMotor.Force> externalForces = new List<PlanePlayerMotor.Force>();

	// Token: 0x04002E6D RID: 11885
	public PlanePlayerMotor.BufferedInput bufferedInput;

	// Token: 0x04002E6E RID: 11886
	public float timeSinceInputBuffered = 0.134f;

	// Token: 0x04002E6F RID: 11887
	public Vector2 _velocity;

	// Token: 0x020011DE RID: 4574
	public enum BufferedInput
	{
		// Token: 0x04007CB0 RID: 31920
		Jump,
		// Token: 0x04007CB1 RID: 31921
		Super
	}

	// Token: 0x020011DF RID: 4575
	public class Force
	{
		// Token: 0x06007F70 RID: 32624 RVA: 0x0005562C File Offset: 0x0005382C
		public Force(Vector2 force, bool enabled)
		{
			this.force = force;
			this.enabled = enabled;
		}

		// Token: 0x17001883 RID: 6275
		// (get) Token: 0x06007F71 RID: 32625 RVA: 0x00055642 File Offset: 0x00053842
		// (set) Token: 0x06007F72 RID: 32626 RVA: 0x0005564A File Offset: 0x0005384A
		public virtual Vector2 force { get; set; }

		// Token: 0x04007CB3 RID: 31923
		public bool enabled;
	}

	// Token: 0x020011E0 RID: 4576
	[Serializable]
	public class Properties
	{
		// Token: 0x04007CB4 RID: 31924
		public float speed = 520f;

		// Token: 0x04007CB5 RID: 31925
		public float shrunkSpeed = 720f;
	}
}
