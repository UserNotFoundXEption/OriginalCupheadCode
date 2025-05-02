using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000569 RID: 1385
public class PlaneSuperChalice : AbstractPlaneSuper
{
	// Token: 0x06003A30 RID: 14896 RVA: 0x0010E9B4 File Offset: 0x0010CBB4
	public override void Start()
	{
		base.Start();
		this.boom.gameObject.SetActive(true);
		this.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.player.stats.OnStoned += this.OnStoned;
	}

	// Token: 0x06003A31 RID: 14897 RVA: 0x0002F5D0 File Offset: 0x0002D7D0
	public void FixedUpdate()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		this.HandleInput();
		if (!this.exploded)
		{
			this.Move();
		}
		this.ClampPosition();
	}

	// Token: 0x06003A32 RID: 14898 RVA: 0x0010EA10 File Offset: 0x0010CC10
	public void EndIntroAnimation()
	{
		base.SnapshotAudio();
		if (this.player != null)
		{
			this.player.UnpauseAll(false);
		}
		this.animHelper.IgnoreGlobal = false;
		PauseManager.Unpause();
		base.StartCoroutine(this.super_cr());
	}

	// Token: 0x06003A33 RID: 14899 RVA: 0x0002F605 File Offset: 0x0002D805
	public void OnStoned()
	{
		this.exploded = true;
	}

	// Token: 0x06003A34 RID: 14900 RVA: 0x0002F60E File Offset: 0x0002D80E
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.exploded = true;
	}

	// Token: 0x06003A35 RID: 14901 RVA: 0x0010EA60 File Offset: 0x0010CC60
	public IEnumerator super_cr()
	{
		this.player.damageReceiver.Vulnerable();
		this.respawnPos = base.transform.position;
		this.state = PlanePlayerWeaponManager.States.Super.Countdown;
		this.damageDealer = new DamageDealer(WeaponProperties.PlaneSuperChaliceSuperBomb.damage, WeaponProperties.PlaneSuperChaliceSuperBomb.damageRate, DamageDealer.DamageSource.Super, false, true, true);
		this.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier;
		this.damageDealer.PlayerId = this.player.id;
		MeterScoreTracker tracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
		tracker.Add(this.damageDealer);
		this.curAngle = MathUtils.DirectionToAngle(Vector3.right);
		this.curSpeed = 0f;
		while (!this.exploded)
		{
			if (this.player != null)
			{
				this.player.transform.position = base.transform.position;
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
			yield return null;
		}
		this.respawnPos = base.transform.position;
		this.Fire();
		if (this.player != null)
		{
			this.player.PauseAll();
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
		base.animator.SetTrigger("Explode");
		AudioManager.Play("player_plane_bomb_explosion");
		AudioManager.Stop("player_plane_bomb_ticktock_loop");
		yield break;
	}

	// Token: 0x06003A36 RID: 14902 RVA: 0x0002F617 File Offset: 0x0002D817
	public override void Fire()
	{
		base.Fire();
	}

	// Token: 0x06003A37 RID: 14903 RVA: 0x0010EA7C File Offset: 0x0010CC7C
	public void PlayerReappear()
	{
		this.RestoreAudio(true);
		if (this.player == null)
		{
			return;
		}
		this.player.motor.OnRevive(this.respawnPos);
		this.player.UnpauseAll(false);
		this.player.SetSpriteVisible(true);
		this.player.damageReceiver.Invulnerable(2f);
	}

	// Token: 0x06003A38 RID: 14904 RVA: 0x0002F61F File Offset: 0x0002D81F
	public void Die()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06003A39 RID: 14905 RVA: 0x0002F62C File Offset: 0x0002D82C
	public void StartBoomScale()
	{
		this.boomRoutine = base.StartCoroutine(this.boomScale_cr());
	}

	// Token: 0x06003A3A RID: 14906 RVA: 0x0010EAEC File Offset: 0x0010CCEC
	public IEnumerator boomScale_cr()
	{
		float t = 0f;
		float frameTime = 0.0416666679f;
		float scale = 1f;
		for (;;)
		{
			t += CupheadTime.Delta;
			while (t > frameTime)
			{
				t -= frameTime;
				scale *= 1.15f;
				this.boom.SetScale(new float?(scale), new float?(scale), null);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003A3B RID: 14907 RVA: 0x0002F640 File Offset: 0x0002D840
	public void Pause()
	{
		if (this.boomRoutine != null)
		{
			base.StopCoroutine(this.boomRoutine);
		}
	}

	// Token: 0x06003A3C RID: 14908 RVA: 0x0010EB08 File Offset: 0x0010CD08
	public void HandleInput()
	{
		Trilean trilean = 0;
		Trilean t = 0;
		float num = 0f;
		if (this.player != null)
		{
			num = this.player.input.actions.GetAxis(1);
		}
		if (num > 0.35f || num < -0.35f)
		{
			t = num;
		}
		this.curAngle += t * WeaponProperties.PlaneSuperChaliceSuperBomb.turnRate;
		this.curAngle = Mathf.Clamp(this.curAngle, -WeaponProperties.PlaneSuperChaliceSuperBomb.maxAngle, WeaponProperties.PlaneSuperChaliceSuperBomb.maxAngle);
		this.curAngle *= WeaponProperties.PlaneSuperChaliceSuperBomb.angleDamp;
		this.accelDirection = MathUtils.AngleToDirection(this.curAngle);
		base.animator.SetInteger("Y", t);
	}

	// Token: 0x06003A3D RID: 14909 RVA: 0x0010EBDC File Offset: 0x0010CDDC
	public void Move()
	{
		Vector2 vector = base.transform.position;
		this.moveDirection = this.accelDirection * this.curSpeed;
		this.curSpeed += WeaponProperties.PlaneSuperChaliceSuperBomb.accel * CupheadTime.FixedDelta;
		base.transform.AddPosition(this.moveDirection.x * CupheadTime.FixedDelta, this.moveDirection.y * CupheadTime.FixedDelta, 0f);
		Vector2 vector2 = base.transform.position;
		this._velocity = (vector2 - vector) / CupheadTime.FixedDelta;
	}

	// Token: 0x06003A3E RID: 14910 RVA: 0x0010EC84 File Offset: 0x0010CE84
	public void ClampPosition()
	{
		Vector2 vector = base.transform.position;
		vector.x = Mathf.Clamp(vector.x, (float)Level.Current.Left, (float)Level.Current.Right - 30f);
		vector.y = Mathf.Clamp(vector.y, (float)Level.Current.Ground, (float)Level.Current.Ceiling);
		if (base.transform.position != vector)
		{
			this.exploded = true;
		}
	}

	// Token: 0x06003A3F RID: 14911 RVA: 0x0010ED1C File Offset: 0x0010CF1C
	public void CheckPosition()
	{
		Vector2 vector = base.transform.position;
		vector.x = Mathf.Clamp(vector.x, (float)Level.Current.Left - 350f, (float)Level.Current.Right + 150f);
		vector.y = Mathf.Clamp(vector.y, (float)Level.Current.Ground - 175f, (float)Level.Current.Ceiling + 325f);
		if (base.transform.position != vector)
		{
			this.missed = true;
		}
	}

	// Token: 0x06003A40 RID: 14912 RVA: 0x0002F659 File Offset: 0x0002D859
	public virtual void RestoreAudio(bool changePitch = true)
	{
		AudioManager.SnapshotReset(SceneLoader.SceneName, 2f);
		if (changePitch)
		{
			AudioManager.ChangeBGMPitch(1f, 2f);
		}
	}

	// Token: 0x06003A41 RID: 14913 RVA: 0x0010EDC8 File Offset: 0x0010CFC8
	public override void OnDestroy()
	{
		this.RestoreAudio(true);
		base.OnDestroy();
		if (this.player != null)
		{
			this.player.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
			this.player.stats.OnStoned -= this.OnStoned;
		}
	}

	// Token: 0x04002E97 RID: 11927
	public const float ANALOG_THRESHOLD = 0.35f;

	// Token: 0x04002E98 RID: 11928
	public const float PADDING_TOP = 65f;

	// Token: 0x04002E99 RID: 11929
	public const float PADDING_BOTTOM = 35f;

	// Token: 0x04002E9A RID: 11930
	public const float PADDING_LEFT = 70f;

	// Token: 0x04002E9B RID: 11931
	public const float PADDING_RIGHT = 30f;

	// Token: 0x04002E9C RID: 11932
	public bool superHappening;

	// Token: 0x04002E9D RID: 11933
	public bool invulnerable;

	// Token: 0x04002E9E RID: 11934
	public float timer;

	// Token: 0x04002E9F RID: 11935
	public Vector2 accelDirection;

	// Token: 0x04002EA0 RID: 11936
	public Vector2 moveDirection;

	// Token: 0x04002EA1 RID: 11937
	public Vector2 _velocity;

	// Token: 0x04002EA2 RID: 11938
	public DamageReceiver damageReceiver;

	// Token: 0x04002EA3 RID: 11939
	public bool exploded;

	// Token: 0x04002EA4 RID: 11940
	public bool missed;

	// Token: 0x04002EA5 RID: 11941
	public Coroutine boomRoutine;

	// Token: 0x04002EA6 RID: 11942
	[SerializeField]
	public Transform boom;

	// Token: 0x04002EA7 RID: 11943
	public float curAngle;

	// Token: 0x04002EA8 RID: 11944
	public float curSpeed;

	// Token: 0x04002EA9 RID: 11945
	public Vector2 respawnPos;
}
