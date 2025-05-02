using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000526 RID: 1318
public class PlayerSuperChaliceBounce : AbstractPlayerSuper
{
	// Token: 0x0600379E RID: 14238 RVA: 0x0002D6EC File Offset: 0x0002B8EC
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x0600379F RID: 14239 RVA: 0x0002D6F4 File Offset: 0x0002B8F4
	public override void StartSuper()
	{
		base.StartSuper();
		base.StartCoroutine(this.super_cr());
	}

	// Token: 0x060037A0 RID: 14240 RVA: 0x00103D7C File Offset: 0x00101F7C
	public IEnumerator super_cr()
	{
		float duration = this.DURATION;
		this.timer = duration;
		this.Fire();
		if (this.LAUNCHED_VERSION)
		{
			yield return new WaitForEndOfFrame();
			this.player.animationController.EnableSpriteRenderer();
		}
		while (this.timer > 0f && !this.interrupted)
		{
			this.timer -= CupheadTime.FixedDelta;
			yield return null;
		}
		if (!this.LAUNCHED_VERSION)
		{
			this.EndSuper(true);
			this.player.transform.position = this.ball.transform.position;
		}
		this.CleanUp();
		yield break;
	}

	// Token: 0x060037A1 RID: 14241 RVA: 0x0002D709 File Offset: 0x0002B909
	public void CleanUp()
	{
		Object.Destroy(this.ball.gameObject);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060037A2 RID: 14242 RVA: 0x00103D98 File Offset: 0x00101F98
	public override void Fire()
	{
		PauseManager.Unpause();
		if (!this.LAUNCHED_VERSION)
		{
			this.player.PauseAll();
			AnimationHelper component = base.GetComponent<AnimationHelper>();
			component.IgnoreGlobal = false;
		}
		else
		{
			this.EndSuper(true);
			this.player.stats.OnSuperEnd();
		}
		this.ball = (this.ball.Create(base.transform.position + Vector3.up * 100f) as PlayerSuperChaliceBounceBall);
		this.ball.player = this.player;
		this.ball.PlayerId = this.player.id;
		this.ball.velocity.x = (float)(this.player.motor.MoveDirection.x * 500);
		this.ball.Damage = this.DAMAGE;
		this.ball.DamageRate = this.DAMAGE_RATE;
		this.ball.super = this;
	}

	// Token: 0x04002CAE RID: 11438
	[NonSerialized]
	public bool LAUNCHED_VERSION = WeaponProperties.LevelSuperChaliceBounce.launchedVersion;

	// Token: 0x04002CAF RID: 11439
	public float DAMAGE = WeaponProperties.LevelSuperChaliceBounce.damage;

	// Token: 0x04002CB0 RID: 11440
	public float DAMAGE_RATE = WeaponProperties.LevelSuperChaliceBounce.damageRate;

	// Token: 0x04002CB1 RID: 11441
	public float DURATION = WeaponProperties.LevelSuperChaliceBounce.duration;

	// Token: 0x04002CB2 RID: 11442
	[SerializeField]
	public PlayerSuperChaliceBounceBall ball;

	// Token: 0x04002CB3 RID: 11443
	public float timer;
}
