using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002CD RID: 717
public class MouseLevelGhostMouse : AbstractCollidableObject
{
	// Token: 0x170002DC RID: 732
	// (get) Token: 0x06001FE4 RID: 8164 RVA: 0x0001AF6D File Offset: 0x0001916D
	// (set) Token: 0x06001FE5 RID: 8165 RVA: 0x0001AF75 File Offset: 0x00019175
	public MouseLevelGhostMouse.State state { get; set; }

	// Token: 0x06001FE6 RID: 8166 RVA: 0x0001AF7E File Offset: 0x0001917E
	public override void Awake()
	{
		base.Awake();
		this.basePos = base.transform.localPosition;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001FE7 RID: 8167 RVA: 0x0001AFB3 File Offset: 0x000191B3
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && this.state != MouseLevelGhostMouse.State.Dying)
		{
			this.Die();
		}
	}

	// Token: 0x06001FE8 RID: 8168 RVA: 0x000B6D0C File Offset: 0x000B4F0C
	public void Spawn(LevelProperties.Mouse properties)
	{
		this.properties = properties;
		if (this.state == MouseLevelGhostMouse.State.Unspawned)
		{
			this.StopAllCoroutines();
			this.state = MouseLevelGhostMouse.State.Intro;
			base.animator.ResetTrigger("AttackBlue");
			base.animator.ResetTrigger("AttackPink");
			base.animator.ResetTrigger("Continue");
			base.StartCoroutine(this.spawn_cr());
		}
	}

	// Token: 0x06001FE9 RID: 8169 RVA: 0x000B6D78 File Offset: 0x000B4F78
	public IEnumerator spawn_cr()
	{
		float spawnOffset = 150f * base.transform.localScale.x;
		float yPos = this.basePos.y + Random.Range(-35f, 35f);
		Vector2 start = new Vector2(this.basePos.x * 0.125f + spawnOffset, yPos);
		this.hp = this.properties.CurrentState.ghostMouse.hp;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		base.animator.SetTrigger("Spawn");
		float t = 0f;
		while (t < 1.083f)
		{
			base.transform.SetLocalPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start.x, this.basePos.x, t / 1.083f)), new float?(yPos), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetLocalPosition(new float?(this.basePos.x), new float?(yPos), null);
		yield return base.animator.WaitForAnimationToStart(this, "Idle_A", false);
		this.state = MouseLevelGhostMouse.State.Idle;
		yield break;
	}

	// Token: 0x06001FEA RID: 8170 RVA: 0x0001AFEA File Offset: 0x000191EA
	public void Attack(bool pink)
	{
		this.state = MouseLevelGhostMouse.State.Attack;
		base.StartCoroutine(this.attack_cr(pink));
	}

	// Token: 0x06001FEB RID: 8171 RVA: 0x000B6D94 File Offset: 0x000B4F94
	public IEnumerator attack_cr(bool pink)
	{
		base.animator.SetTrigger((!pink) ? "AttackBlue" : "AttackPink");
		yield return base.animator.WaitForAnimationToStart(this, (!pink) ? "Attack_Blue_Loop" : "Attack_Pink_Loop", false);
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.ghostMouse.attackAnticipation);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToStart(this, "Idle_B", false);
		this.state = MouseLevelGhostMouse.State.Idle;
		yield break;
	}

	// Token: 0x06001FEC RID: 8172 RVA: 0x000B6DB8 File Offset: 0x000B4FB8
	public void FireBlue()
	{
		this.blueBallPrefab.Create(this.projectileRoot.position, this.properties.CurrentState.ghostMouse.ballSpeed, this.properties.CurrentState.ghostMouse.splitSpeed);
	}

	// Token: 0x06001FED RID: 8173 RVA: 0x000B6E0C File Offset: 0x000B500C
	public void FirePink()
	{
		this.pinkBallPrefab.Create(this.projectileRoot.position, this.properties.CurrentState.ghostMouse.ballSpeed, this.properties.CurrentState.ghostMouse.splitSpeed);
	}

	// Token: 0x06001FEE RID: 8174 RVA: 0x0001B001 File Offset: 0x00019201
	public void Die()
	{
		if (this.state == MouseLevelGhostMouse.State.Unspawned || this.state == MouseLevelGhostMouse.State.Dying)
		{
			return;
		}
		this.state = MouseLevelGhostMouse.State.Dying;
		this.StopAllCoroutines();
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x06001FEF RID: 8175 RVA: 0x000B6E60 File Offset: 0x000B5060
	public IEnumerator death_cr()
	{
		while (this.state == MouseLevelGhostMouse.State.Intro)
		{
			yield return null;
		}
		base.animator.SetTrigger("Die");
		base.transform.Rotate(0f, 0f, (float)Random.Range(-16, 16));
		yield return base.animator.WaitForAnimationToEnd(this, "Death", false, true);
		this.state = MouseLevelGhostMouse.State.Unspawned;
		yield break;
	}

	// Token: 0x06001FF0 RID: 8176 RVA: 0x0001B035 File Offset: 0x00019235
	public void SoundMouseGhostWail()
	{
		AudioManager.Play("level_mouse_ghost_mouse_wail");
		this.emitAudioFromObject.Add("level_mouse_ghost_mouse_wail");
	}

	// Token: 0x06001FF1 RID: 8177 RVA: 0x0001B051 File Offset: 0x00019251
	public void SoundMouseGhostLaugh()
	{
		AudioManager.Play("level_mouse_ghost_mouse_laugh");
		this.emitAudioFromObject.Add("level_mouse_ghost_mouse_laugh");
	}

	// Token: 0x06001FF2 RID: 8178 RVA: 0x0001B06D File Offset: 0x0001926D
	public void SoundMouseGhostAttack()
	{
		AudioManager.Play("level_mouse_ghost_attack");
		this.emitAudioFromObject.Add("level_mouse_ghost_attack");
	}

	// Token: 0x06001FF3 RID: 8179 RVA: 0x0001B089 File Offset: 0x00019289
	public void SoundMouseGhostDeath()
	{
		AudioManager.Play("level_mouse_ghost_death");
		this.emitAudioFromObject.Add("level_mouse_ghost_death");
	}

	// Token: 0x06001FF4 RID: 8180 RVA: 0x0001B0A5 File Offset: 0x000192A5
	public void SoundMouseGhostDeathStart()
	{
		AudioManager.Play("level_mouse_ghost_death_start");
		this.emitAudioFromObject.Add("level_mouse_ghost_death_start");
	}

	// Token: 0x06001FF5 RID: 8181 RVA: 0x0001B0C1 File Offset: 0x000192C1
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.blueBallPrefab = null;
		this.pinkBallPrefab = null;
	}

	// Token: 0x040019F6 RID: 6646
	public const float heightVariation = 35f;

	// Token: 0x040019F7 RID: 6647
	public const float spawnXRatio = 0.125f;

	// Token: 0x040019F9 RID: 6649
	public Vector2 basePos;

	// Token: 0x040019FA RID: 6650
	public LevelProperties.Mouse properties;

	// Token: 0x040019FB RID: 6651
	public float hp;

	// Token: 0x040019FC RID: 6652
	[SerializeField]
	public MouseLevelGhostMouseBall blueBallPrefab;

	// Token: 0x040019FD RID: 6653
	[SerializeField]
	public MouseLevelGhostMouseBall pinkBallPrefab;

	// Token: 0x040019FE RID: 6654
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x02000DC7 RID: 3527
	public enum State
	{
		// Token: 0x0400639F RID: 25503
		Unspawned,
		// Token: 0x040063A0 RID: 25504
		Intro,
		// Token: 0x040063A1 RID: 25505
		Idle,
		// Token: 0x040063A2 RID: 25506
		Attack,
		// Token: 0x040063A3 RID: 25507
		Dying
	}
}
