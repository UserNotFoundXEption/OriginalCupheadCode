using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000416 RID: 1046
public class FunhousePlatformingLevelDuck : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002D78 RID: 11640 RVA: 0x00025F43 File Offset: 0x00024143
	public override void OnStart()
	{
	}

	// Token: 0x06002D79 RID: 11641 RVA: 0x000DCDB8 File Offset: 0x000DAFB8
	public override void Start()
	{
		base.Start();
		if (this.child != null)
		{
			this.child.OnAnyCollision += this.OnCollision;
			this.child.OnPlayerCollision += this.OnCollisionPlayer;
		}
		if (this.parryable)
		{
			this._canParry = true;
		}
		if (!this.isBigDuck)
		{
			base.StartCoroutine(this.idle_sound_cr());
		}
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002D7A RID: 11642 RVA: 0x00025F45 File Offset: 0x00024145
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<FunhousePlatformingLevelCar>())
		{
			this.Die();
		}
	}

	// Token: 0x06002D7B RID: 11643 RVA: 0x000DCE44 File Offset: 0x000DB044
	public IEnumerator idle_sound_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(5f, 15f));
			AudioManager.Play("funhouse_small_duck_idle_sweet");
			this.emitAudioFromObject.Add("funhouse_small_duck_idle_sweet");
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002D7C RID: 11644 RVA: 0x000DCE60 File Offset: 0x000DB060
	public IEnumerator move_cr()
	{
		if (this.isBigDuck)
		{
			AudioManager.PlayLoop("funhouse_big_duck_idle");
			this.emitAudioFromObject.Add("funhouse_big_duck_idle");
		}
		else if (this.smallFirst)
		{
			AudioManager.PlayLoop("funhouse_small_duck_idle_loop");
			this.emitAudioFromObject.Add("funhouse_small_duck_idle_loop");
		}
		float size = base.GetComponent<Collider2D>().bounds.size.x;
		while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMin - size)
		{
			base.transform.position -= base.transform.right * base.Properties.MoveSpeed * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		this.DoneAnimation();
		yield break;
	}

	// Token: 0x06002D7D RID: 11645 RVA: 0x000DCE7C File Offset: 0x000DB07C
	public override void Die()
	{
		this.StopAllCoroutines();
		if (this.smallLast)
		{
			AudioManager.Stop("funhouse_small_duck_idle_loop");
		}
		if (this.isBigDuck)
		{
			AudioManager.Stop("funhouse_big_duck_idle");
			AudioManager.Play("funhouse_big_duck_death");
			this.emitAudioFromObject.Add("funhouse_big_duck_death");
			base.animator.SetTrigger("OnDeath");
		}
		else
		{
			AudioManager.Play("funhouse_small_duck_death");
			AudioManager.Play("funhouse_small_duck_death");
			base.Die();
		}
	}

	// Token: 0x06002D7E RID: 11646 RVA: 0x00025F65 File Offset: 0x00024165
	public void DoneAnimation()
	{
		if (this.isBigDuck)
		{
			AudioManager.Stop("funhouse_big_duck_idle");
		}
		if (this.smallLast)
		{
			AudioManager.Stop("funhouse_small_duck_idle_loop");
		}
		base.Die();
	}

	// Token: 0x040025A9 RID: 9641
	[SerializeField]
	public bool isBigDuck;

	// Token: 0x040025AA RID: 9642
	[SerializeField]
	public bool parryable;

	// Token: 0x040025AB RID: 9643
	[SerializeField]
	public CollisionChild child;

	// Token: 0x040025AC RID: 9644
	public bool smallFirst;

	// Token: 0x040025AD RID: 9645
	public bool smallLast;
}
