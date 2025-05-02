using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000157 RID: 343
public class BaronessLevelPeppermint : ParrySwitch
{
	// Token: 0x0600107A RID: 4218 RVA: 0x0000DE5C File Offset: 0x0000C05C
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x0600107B RID: 4219 RVA: 0x0000DE6F File Offset: 0x0000C06F
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600107C RID: 4220 RVA: 0x0000DE8D File Offset: 0x0000C08D
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600107D RID: 4221 RVA: 0x0000DEA5 File Offset: 0x0000C0A5
	public void Init(Vector2 pos, float speed)
	{
		base.transform.position = pos;
		this.speed = speed;
		AudioManager.Play("level_baroness_candy_roll");
		base.StartCoroutine(this.fade_color_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x0600107E RID: 4222 RVA: 0x00090560 File Offset: 0x0008E760
	public virtual IEnumerator fade_color_cr()
	{
		float fadeTime = 0.7f;
		float t = 0f;
		while (t < fadeTime)
		{
			base.GetComponent<SpriteRenderer>().color = new Color(t / fadeTime, t / fadeTime, t / fadeTime, 1f);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		yield return null;
		yield break;
	}

	// Token: 0x0600107F RID: 4223 RVA: 0x0009057C File Offset: 0x0008E77C
	public IEnumerator move_cr()
	{
		float offsetX = 220f;
		Vector3 pos = base.transform.position;
		for (;;)
		{
			if (base.transform.position.x > -640f - offsetX)
			{
				pos.x = Mathf.MoveTowards(base.transform.position.x, -640f - offsetX, this.speed * CupheadTime.FixedDelta);
			}
			else
			{
				this.Die();
			}
			base.transform.position = pos;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06001080 RID: 4224 RVA: 0x0000DEE3 File Offset: 0x0000C0E3
	public void Die()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001081 RID: 4225 RVA: 0x0000DEF0 File Offset: 0x0000C0F0
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		base.OnParryPrePause(player);
		player.stats.ParryOneQuarter();
	}

	// Token: 0x06001082 RID: 4226 RVA: 0x0000DF04 File Offset: 0x0000C104
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		base.IsParryable = false;
		base.StartCoroutine(this.peppermintParryCooldown_cr());
	}

	// Token: 0x06001083 RID: 4227 RVA: 0x00090598 File Offset: 0x0008E798
	public IEnumerator peppermintParryCooldown_cr()
	{
		float t = 0f;
		while (t < this.coolDown)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		base.IsParryable = true;
		yield return null;
		yield break;
	}

	// Token: 0x04000D74 RID: 3444
	public DamageDealer damageDealer;

	// Token: 0x04000D75 RID: 3445
	public float speed;
}
