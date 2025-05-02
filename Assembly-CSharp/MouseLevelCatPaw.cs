using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002C8 RID: 712
public class MouseLevelCatPaw : AbstractCollidableObject
{
	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x06001FBB RID: 8123 RVA: 0x0001ACEC File Offset: 0x00018EEC
	// (set) Token: 0x06001FBC RID: 8124 RVA: 0x0001ACF4 File Offset: 0x00018EF4
	public MouseLevelCatPaw.State state { get; set; }

	// Token: 0x06001FBD RID: 8125 RVA: 0x0001ACFD File Offset: 0x00018EFD
	public override void Awake()
	{
		base.Awake();
		this.initialPos = base.transform.localPosition;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001FBE RID: 8126 RVA: 0x0001AD26 File Offset: 0x00018F26
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001FBF RID: 8127 RVA: 0x0001AD3E File Offset: 0x00018F3E
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001FC0 RID: 8128 RVA: 0x0001AD5C File Offset: 0x00018F5C
	public void Attack(LevelProperties.Mouse.Claw properties)
	{
		this.properties = properties;
		if (this.state == MouseLevelCatPaw.State.Idle)
		{
			this.state = MouseLevelCatPaw.State.Attack;
			base.StartCoroutine(this.attack_cr());
		}
	}

	// Token: 0x06001FC1 RID: 8129 RVA: 0x000B696C File Offset: 0x000B4B6C
	public IEnumerator attack_cr()
	{
		float totalMoveTime = 0.584f;
		float startX = this.initialPos.x;
		float endX = this.initialPos.x + totalMoveTime * this.properties.moveSpeed;
		int hitAnim = Animator.StringToHash(base.animator.GetLayerName(0) + ".Attack_Hit");
		float previousAnimationsTime = 0f;
		for (int i = 0; i < 3; i++)
		{
			base.animator.SetTrigger("Attack");
			float animationTime = (i != 0) ? 0.167f : 0.25f;
			bool hitGround = false;
			while (!hitGround)
			{
				yield return new WaitForEndOfFrame();
				AnimatorStateInfo animState = base.animator.GetCurrentAnimatorStateInfo(0);
				if (animState.fullPathHash == hitAnim)
				{
					hitGround = true;
					previousAnimationsTime += animationTime;
					float moveProgress = previousAnimationsTime / totalMoveTime;
					base.transform.SetLocalPosition(new float?(Mathf.Lerp(startX, endX, moveProgress)), null, null);
					CupheadLevelCamera.Current.Shake(15f, 1f, false);
					yield return CupheadTime.WaitForSeconds(this, this.properties.holdGroundTime);
				}
				else
				{
					float num = animState.normalizedTime * animationTime;
					float num2 = (previousAnimationsTime + num) / totalMoveTime;
					base.transform.SetLocalPosition(new float?(Mathf.Lerp(startX, endX, num2)), null, null);
				}
			}
		}
		base.animator.SetTrigger("Leave");
		base.StartCoroutine(this.timedAudioCatMeow_cr());
		float moveStartX = base.transform.localPosition.x;
		float moveEndX = this.initialPos.x;
		float leaveTime = Mathf.Abs(moveEndX - moveStartX) / this.properties.leaveSpeed;
		float t = 0f;
		while (t < leaveTime)
		{
			t += CupheadTime.Delta;
			base.transform.SetLocalPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInSine, moveStartX, moveEndX, t / leaveTime)), null, null);
			yield return null;
		}
		base.transform.SetLocalPosition(new float?(moveEndX), null, null);
		this.state = MouseLevelCatPaw.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001FC2 RID: 8130 RVA: 0x000B6988 File Offset: 0x000B4B88
	public IEnumerator timedAudioCatMeow_cr()
	{
		yield return new WaitForSeconds(1f);
		AudioManager.Play("level_mouse_cat_claw_end");
		yield break;
	}

	// Token: 0x06001FC3 RID: 8131 RVA: 0x0001AD84 File Offset: 0x00018F84
	public void SoundCatPawAttack()
	{
		AudioManager.Play("level_mouse_cat_paw_attack");
		this.emitAudioFromObject.Add("level_mouse_cat_paw_attack");
	}

	// Token: 0x06001FC4 RID: 8132 RVA: 0x0001ADA0 File Offset: 0x00018FA0
	public void SoundCatMeowVoice()
	{
		AudioManager.Play("level_mouse_cat_meow_voice");
		this.emitAudioFromObject.Add("level_mouse_cat_meow_voice");
	}

	// Token: 0x040019DD RID: 6621
	public LevelProperties.Mouse.Claw properties;

	// Token: 0x040019DE RID: 6622
	public Vector2 initialPos;

	// Token: 0x040019DF RID: 6623
	public DamageDealer damageDealer;

	// Token: 0x02000DC1 RID: 3521
	public enum State
	{
		// Token: 0x04006379 RID: 25465
		Idle,
		// Token: 0x0400637A RID: 25466
		Attack
	}
}
