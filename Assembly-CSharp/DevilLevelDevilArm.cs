using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001AB RID: 427
public class DevilLevelDevilArm : AbstractCollidableObject
{
	// Token: 0x06001450 RID: 5200 RVA: 0x00099D94 File Offset: 0x00097F94
	public override void Awake()
	{
		base.Awake();
		this.startX = base.transform.position.x;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001451 RID: 5201 RVA: 0x0001124A File Offset: 0x0000F44A
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001452 RID: 5202 RVA: 0x00011262 File Offset: 0x0000F462
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001453 RID: 5203 RVA: 0x0001128B File Offset: 0x0000F48B
	public void Attack(float speed)
	{
		this.state = DevilLevelDevilArm.State.Attacking;
		base.animator.SetTrigger("ArmsIn");
		base.StartCoroutine(this.attack_cr(speed));
	}

	// Token: 0x06001454 RID: 5204 RVA: 0x00099DCC File Offset: 0x00097FCC
	public IEnumerator attack_cr(float speed)
	{
		this.speed = speed;
		bool isClapping = false;
		float t = 0f;
		base.GetComponent<Collider2D>().enabled = true;
		while (t < speed)
		{
			base.transform.SetPosition(new float?(Mathf.Lerp(this.startX, this.endPos.position.x, t / speed)), null, null);
			yield return new WaitForFixedUpdate();
			t += CupheadTime.FixedDelta;
			if (t / speed > 0.85f && !isClapping)
			{
				base.animator.SetTrigger("OnAttack");
				isClapping = true;
			}
		}
		base.transform.SetPosition(new float?(this.endPos.position.x), null, null);
		CupheadLevelCamera.Current.Shake(20f, 0.7f, false);
		yield return new WaitForFixedUpdate();
		yield break;
	}

	// Token: 0x06001455 RID: 5205 RVA: 0x000112B2 File Offset: 0x0000F4B2
	public void MoveAway()
	{
		base.StartCoroutine(this.move_away_cr());
	}

	// Token: 0x06001456 RID: 5206 RVA: 0x00099DF0 File Offset: 0x00097FF0
	public IEnumerator move_away_cr()
	{
		float currentPos = base.transform.position.x;
		float t = 0f;
		float moveTime = this.speed;
		for (t = 0f; t < moveTime; t += CupheadTime.FixedDelta)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / moveTime);
			base.transform.SetPosition(new float?(Mathf.Lerp(currentPos, this.startX, val)), null, null);
			yield return new WaitForFixedUpdate();
		}
		base.transform.SetPosition(new float?(this.startX), null, null);
		this.RamSlapSFXActive = false;
		base.GetComponent<Collider2D>().enabled = false;
		this.state = DevilLevelDevilArm.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001457 RID: 5207 RVA: 0x000112C1 File Offset: 0x0000F4C1
	public void HandclapSFX()
	{
		if (this.isRight)
		{
			AudioManager.Play("devil_hand_clap");
		}
	}

	// Token: 0x06001458 RID: 5208 RVA: 0x000112D8 File Offset: 0x0000F4D8
	public void RamSlapSFX()
	{
		if (!this.RamSlapSFXActive)
		{
			AudioManager.Play("devil_ram_slap");
			this.RamSlapSFXActive = true;
		}
	}

	// Token: 0x0400109B RID: 4251
	public DevilLevelDevilArm.State state;

	// Token: 0x0400109C RID: 4252
	public bool RamSlapSFXActive;

	// Token: 0x0400109D RID: 4253
	public DamageDealer damageDealer;

	// Token: 0x0400109E RID: 4254
	[SerializeField]
	public Transform endPos;

	// Token: 0x0400109F RID: 4255
	public float speed;

	// Token: 0x040010A0 RID: 4256
	public float startX;

	// Token: 0x040010A1 RID: 4257
	public bool isRight;

	// Token: 0x02000B21 RID: 2849
	public enum State
	{
		// Token: 0x0400519F RID: 20895
		Idle,
		// Token: 0x040051A0 RID: 20896
		Attacking
	}
}
