using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001B5 RID: 437
public class DevilLevelSpiderHead : AbstractCollidableObject
{
	// Token: 0x060014DD RID: 5341 RVA: 0x00011B1D File Offset: 0x0000FD1D
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		base.gameObject.SetActive(false);
	}

	// Token: 0x060014DE RID: 5342 RVA: 0x00011B3C File Offset: 0x0000FD3C
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060014DF RID: 5343 RVA: 0x00011B54 File Offset: 0x0000FD54
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060014E0 RID: 5344 RVA: 0x0009AB88 File Offset: 0x00098D88
	public void Attack(float xPos, float downSpeed, float upSpeed)
	{
		base.gameObject.SetActive(true);
		base.animator.SetBool("IsFalling", true);
		this.state = DevilLevelSpiderHead.State.Attacking;
		base.transform.SetPosition(new float?(xPos), null, null);
		base.StartCoroutine(this.attack_cr(downSpeed, upSpeed));
	}

	// Token: 0x060014E1 RID: 5345 RVA: 0x0009ABEC File Offset: 0x00098DEC
	public IEnumerator attack_cr(float downSpeed, float upSpeed)
	{
		float moveTime = Mathf.Abs(this.moveDistanceY) / downSpeed;
		float startY = base.transform.position.y;
		float t = 0f;
		base.GetComponent<Collider2D>().enabled = true;
		AudioManager.Play("devil_spider_fall");
		this.emitAudioFromObject.Add("devil_spider_fall");
		while (t < moveTime)
		{
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, startY, startY + this.moveDistanceY, t / moveTime)), null);
			yield return new WaitForFixedUpdate();
			t += CupheadTime.FixedDelta;
		}
		AudioManager.Play("devil_spider_head_hit_floor");
		this.emitAudioFromObject.Add("devil_spider_head_hit_floor");
		base.animator.SetBool("IsFalling", false);
		base.transform.SetPosition(null, new float?(startY + this.moveDistanceY), null);
		t = 0f;
		moveTime = Mathf.Abs(this.moveDistanceY) / upSpeed;
		yield return base.animator.WaitForAnimationToEnd(this, "Fall_Splat", false, true);
		while (t < moveTime)
		{
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInSine, startY + this.moveDistanceY, startY, t / moveTime)), null);
			yield return new WaitForFixedUpdate();
			t += CupheadTime.FixedDelta;
		}
		base.transform.SetPosition(null, new float?(startY), null);
		this.state = DevilLevelSpiderHead.State.Idle;
		base.GetComponent<Collider2D>().enabled = false;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x04001118 RID: 4376
	public DevilLevelSpiderHead.State state;

	// Token: 0x04001119 RID: 4377
	public DamageDealer damageDealer;

	// Token: 0x0400111A RID: 4378
	[SerializeField]
	public float moveDistanceY;

	// Token: 0x02000B4C RID: 2892
	public enum State
	{
		// Token: 0x040052CE RID: 21198
		Idle,
		// Token: 0x040052CF RID: 21199
		Attacking
	}
}
