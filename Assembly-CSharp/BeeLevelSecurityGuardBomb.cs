using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200016E RID: 366
public class BeeLevelSecurityGuardBomb : AbstractProjectile
{
	// Token: 0x060011A0 RID: 4512 RVA: 0x00092B90 File Offset: 0x00090D90
	public BeeLevelSecurityGuardBomb Create(Vector2 pos, int direction, float idleTime, float warningTime, float childSpeed, int childCount)
	{
		BeeLevelSecurityGuardBomb beeLevelSecurityGuardBomb = base.Create() as BeeLevelSecurityGuardBomb;
		beeLevelSecurityGuardBomb.direction = direction;
		beeLevelSecurityGuardBomb.idleTime = idleTime;
		beeLevelSecurityGuardBomb.warningTime = warningTime;
		beeLevelSecurityGuardBomb.childSpeed = childSpeed;
		beeLevelSecurityGuardBomb.childCount = childCount;
		beeLevelSecurityGuardBomb.transform.position = pos;
		return beeLevelSecurityGuardBomb;
	}

	// Token: 0x060011A1 RID: 4513 RVA: 0x0000EF13 File Offset: 0x0000D113
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x060011A2 RID: 4514 RVA: 0x0000EF28 File Offset: 0x0000D128
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060011A3 RID: 4515 RVA: 0x00092BE4 File Offset: 0x00090DE4
	public override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
		float num = (float)(360 / this.childCount);
		for (int i = 0; i < this.childCount; i++)
		{
			BasicProjectile basicProjectile = this.childPrefab.Create(base.transform.position, num * (float)i, Vector2.one, this.childSpeed);
			basicProjectile.SetParryable(i % 2 != 0);
		}
	}

	// Token: 0x060011A4 RID: 4516 RVA: 0x00092C5C File Offset: 0x00090E5C
	public IEnumerator go_cr()
	{
		float time = 0.3f;
		Vector2 pos = base.transform.position + new Vector2((float)(50 * this.direction), 100f);
		yield return base.TweenPosition(base.transform.position, pos, time, EaseUtils.EaseType.easeOutSine);
		yield return CupheadTime.WaitForSeconds(this, this.idleTime);
		AudioManager.PlayLoop("bee_guard_bomb_warning");
		this.emitAudioFromObject.Add("bee_guard_bomb_warning");
		base.animator.Play("Warning");
		yield return CupheadTime.WaitForSeconds(this, this.warningTime);
		AudioManager.Stop("bee_guard_bomb_warning");
		AudioManager.Play("bee_guard_bomb_explode");
		this.emitAudioFromObject.Add("bee_guard_bomb_explode");
		this.Die();
		yield break;
	}

	// Token: 0x060011A5 RID: 4517 RVA: 0x0000EF51 File Offset: 0x0000D151
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.childPrefab = null;
	}

	// Token: 0x04000E23 RID: 3619
	[SerializeField]
	public BasicProjectile childPrefab;

	// Token: 0x04000E24 RID: 3620
	public int direction;

	// Token: 0x04000E25 RID: 3621
	public float idleTime;

	// Token: 0x04000E26 RID: 3622
	public float warningTime;

	// Token: 0x04000E27 RID: 3623
	public float childSpeed;

	// Token: 0x04000E28 RID: 3624
	public int childCount;
}
