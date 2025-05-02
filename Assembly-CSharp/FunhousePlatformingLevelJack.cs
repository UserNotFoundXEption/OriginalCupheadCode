using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000419 RID: 1049
public class FunhousePlatformingLevelJack : AbstractPlatformingLevelEnemy
{
	// Token: 0x1700035D RID: 861
	// (get) Token: 0x06002D8D RID: 11661 RVA: 0x00026055 File Offset: 0x00024255
	// (set) Token: 0x06002D8E RID: 11662 RVA: 0x0002605D File Offset: 0x0002425D
	public bool HomingEnabled { get; set; }

	// Token: 0x06002D8F RID: 11663 RVA: 0x00026066 File Offset: 0x00024266
	public override void OnStart()
	{
	}

	// Token: 0x06002D90 RID: 11664 RVA: 0x000DD058 File Offset: 0x000DB258
	public override void Start()
	{
		base.Start();
		bool flag = Rand.Bool();
		base.animator.Play((!flag) ? "Green_Idle_A" : "Pink_Idle_A");
		this._canParry = flag;
		this.HomingEnabled = true;
		this.player = PlayerManager.GetNext();
		this.launchVelocity = this.homingDirection * base.Properties.jackLaunchVelocity;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.switch_cr());
	}

	// Token: 0x06002D91 RID: 11665 RVA: 0x00026068 File Offset: 0x00024268
	public override void Update()
	{
		base.Update();
		this.CalculateRender();
	}

	// Token: 0x06002D92 RID: 11666 RVA: 0x00026076 File Offset: 0x00024276
	public void SelectDirection(bool fromBottom)
	{
		this.homingDirection = ((!fromBottom) ? Vector2.down : Vector2.up);
	}

	// Token: 0x06002D93 RID: 11667 RVA: 0x000DD0E0 File Offset: 0x000DB2E0
	public IEnumerator move_cr()
	{
		float t = 0f;
		while (t < base.Properties.jacktimeBeforeDeath + base.Properties.jackEaseTime + base.Properties.jacktimeBeforeHoming)
		{
			while (!this.HomingEnabled)
			{
				yield return null;
			}
			t += CupheadTime.FixedDelta;
			if (this.player != null && !this.player.IsDead)
			{
				Vector3 center = this.player.center;
				Vector2 direction = (center - base.transform.position).normalized;
				Quaternion quaternion = Quaternion.Euler(0f, 0f, MathUtils.DirectionToAngle(direction));
				Quaternion quaternion2 = Quaternion.Euler(0f, 0f, MathUtils.DirectionToAngle(this.homingDirection));
				this.homingDirection = MathUtils.AngleToDirection(Quaternion.Slerp(quaternion2, quaternion, Mathf.Min(1f, CupheadTime.FixedDelta * base.Properties.jackRotationSpeed)).eulerAngles.z);
			}
			Vector2 homingVelocity = this.homingDirection * base.Properties.jackHomingMoveSpeed;
			Vector2 velocity = homingVelocity;
			if (t < base.Properties.jacktimeBeforeHoming)
			{
				velocity = this.launchVelocity;
			}
			else if (t < base.Properties.jacktimeBeforeHoming + base.Properties.jackEaseTime)
			{
				float num = EaseUtils.EaseOutSine(0f, 1f, (t - base.Properties.jacktimeBeforeHoming) / base.Properties.jackEaseTime);
				velocity = Vector2.Lerp(this.launchVelocity, homingVelocity, num);
			}
			base.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		this.Die();
		yield break;
	}

	// Token: 0x06002D94 RID: 11668 RVA: 0x000DD0FC File Offset: 0x000DB2FC
	public IEnumerator switch_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(1.5f, 3f));
			base.animator.SetTrigger("OnSwitch");
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002D95 RID: 11669 RVA: 0x00026093 File Offset: 0x00024293
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.Die();
	}

	// Token: 0x06002D96 RID: 11670 RVA: 0x000260A3 File Offset: 0x000242A3
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<FunhousePlatformingLevelJack>() != null)
		{
			this.Die();
		}
	}

	// Token: 0x06002D97 RID: 11671 RVA: 0x000DD118 File Offset: 0x000DB318
	public void CalculateRender()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position) && !this._enteredScreen)
		{
			this._enteredScreen = true;
		}
		if (this._enteredScreen && !CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 100f)))
		{
			Object.Destroy(base.gameObject);
		}
		if (PlatformingLevel.Current != null && (base.transform.position.x < (float)PlatformingLevel.Current.Left - 100f || base.transform.position.x > (float)PlatformingLevel.Current.Right + 100f || base.transform.position.y < (float)PlatformingLevel.Current.Ground - 100f))
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06002D98 RID: 11672 RVA: 0x000260C4 File Offset: 0x000242C4
	public override void Die()
	{
		AudioManager.Play("funhouse_jack_death");
		this.emitAudioFromObject.Add("funhouse_jack_death");
		base.Die();
	}

	// Token: 0x040025C4 RID: 9668
	public const float SCREEN_PADDING = 100f;

	// Token: 0x040025C5 RID: 9669
	public AbstractPlayerController player;

	// Token: 0x040025C6 RID: 9670
	public bool _enteredScreen;

	// Token: 0x040025C7 RID: 9671
	public Vector2 homingDirection = Vector2.down;

	// Token: 0x040025C8 RID: 9672
	public Vector2 launchVelocity;
}
