using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000422 RID: 1058
public class FunhousePlatformingLevelTuba : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002DD4 RID: 11732 RVA: 0x000DDC70 File Offset: 0x000DBE70
	public override void Start()
	{
		base.Start();
		base.transform.position = this.startPos.transform.position;
		this.start = this.startPos.transform.position;
		this.end = this.endPos.transform.position;
		base.StartCoroutine(this.check_to_start_cr());
	}

	// Token: 0x06002DD5 RID: 11733 RVA: 0x0002638D File Offset: 0x0002458D
	public override void OnStart()
	{
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06002DD6 RID: 11734 RVA: 0x000DDCE4 File Offset: 0x000DBEE4
	public IEnumerator check_to_start_cr()
	{
		while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset)
		{
			yield return null;
		}
		this.OnStart();
		yield return null;
		yield break;
	}

	// Token: 0x06002DD7 RID: 11735 RVA: 0x000DDD00 File Offset: 0x000DBF00
	public IEnumerator attack_cr()
	{
		float time = base.Properties.MoveSpeed;
		float t = 0f;
		yield return CupheadTime.WaitForSeconds(this, base.Properties.tubaInitialDelay);
		for (;;)
		{
			while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset || base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - this.offset)
			{
				yield return null;
			}
			t = 0f;
			base.animator.SetBool("isAttacking", true);
			yield return base.animator.WaitForAnimationToEnd(this, "Tuba_Anti", false, true);
			base.animator.Play("Attack_" + ((!Rand.Bool()) ? "B" : "A"), 1);
			base.StartCoroutine(this.shoot_cr());
			while (t < time)
			{
				t += CupheadTime.Delta;
				Vector2 pos = base.transform.position;
				pos.y = Mathf.Lerp(base.transform.position.y, this.end.y, t / time);
				base.transform.position = pos;
				yield return null;
			}
			t = 0f;
			while (t < time)
			{
				t += CupheadTime.Delta;
				Vector2 pos2 = base.transform.position;
				pos2.y = Mathf.Lerp(base.transform.position.y, this.start.y, t / time);
				base.transform.position = pos2;
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, base.Properties.tubaMainDelayRange.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002DD8 RID: 11736 RVA: 0x000DDD1C File Offset: 0x000DBF1C
	public IEnumerator shoot_cr()
	{
		AudioManager.Play("funhouse_tuba_attack");
		this.emitAudioFromObject.Add("funhouse_tuba_attack");
		float delay = 0f;
		BasicProjectile p = this.projectile.Create(this.root.transform.position, 180f, base.Properties.ProjectileSpeed);
		p.animator.Play("BW");
		p.transform.parent = base.transform;
		p.OnDie += this.OnBwaaDie;
		this.bwaaList.Add(p.gameObject);
		delay = p.transform.GetComponent<SpriteRenderer>().bounds.size.x / 1.4f / base.Properties.ProjectileSpeed;
		yield return CupheadTime.WaitForSeconds(this, delay);
		for (int i = 0; i < base.Properties.tubaACount; i++)
		{
			p = this.projectile.Create(this.root.transform.position, 180f, base.Properties.ProjectileSpeed);
			p.animator.Play("A" + Random.Range(1, 4).ToStringInvariant());
			p.transform.parent = base.transform;
			p.OnDie += this.OnBwaaDie;
			this.bwaaList.Add(p.gameObject);
			delay = p.transform.GetComponent<SpriteRenderer>().bounds.size.x / 2f / base.Properties.ProjectileSpeed;
			yield return CupheadTime.WaitForSeconds(this, delay);
		}
		p = this.projectile.Create(this.root.transform.position, 180f, base.Properties.ProjectileSpeed);
		p.animator.Play("EXCLAIM");
		p.transform.parent = base.transform;
		p.OnDie += this.OnBwaaDie;
		this.bwaaList.Add(p.gameObject);
		yield return CupheadTime.WaitForSeconds(this, delay);
		base.animator.SetBool("isAttacking", false);
		yield return null;
		yield break;
	}

	// Token: 0x06002DD9 RID: 11737 RVA: 0x0002639C File Offset: 0x0002459C
	public void OnBwaaDie(AbstractProjectile p)
	{
		p.OnDie -= this.OnBwaaDie;
		if (this.bwaaList != null)
		{
			this.bwaaList.Remove(p.gameObject);
		}
	}

	// Token: 0x06002DDA RID: 11738 RVA: 0x000DDD38 File Offset: 0x000DBF38
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0f, 1f, 0f, 1f);
		Gizmos.DrawLine(this.startPos.transform.position, this.endPos.transform.position);
		Gizmos.color = new Color(1f, 0f, 0f, 1f);
		Gizmos.DrawWireSphere(this.startPos.transform.position, 10f);
		Gizmos.DrawWireSphere(this.endPos.transform.position, 10f);
	}

	// Token: 0x06002DDB RID: 11739 RVA: 0x000263CD File Offset: 0x000245CD
	public override void Die()
	{
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDeath");
		base.StartCoroutine(this.slide_off_cr());
	}

	// Token: 0x06002DDC RID: 11740 RVA: 0x000DDDE0 File Offset: 0x000DBFE0
	public IEnumerator slide_off_cr()
	{
		for (int i = 0; i < this.bwaaList.Count; i++)
		{
			if (this.bwaaList[i] != null)
			{
				this.bwaaList[i].transform.SetParent(null);
			}
		}
		float t = 0f;
		float time = 3f;
		float start = base.transform.position.y;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			if (base.transform.localScale.y > 0f)
			{
				base.transform.SetPosition(null, new float?(Mathf.Lerp(start, -860f, t / time)), null);
			}
			else
			{
				base.transform.SetPosition(null, new float?(Mathf.Lerp(start, 1220f, t / time)), null);
			}
			yield return wait;
		}
		this.<Die>__BaseCallProxy0();
		yield return null;
		yield break;
	}

	// Token: 0x06002DDD RID: 11741 RVA: 0x000263F2 File Offset: 0x000245F2
	public void SoundTubaAnti()
	{
		AudioManager.Play("funhouse_tuba_anti");
		this.emitAudioFromObject.Add("funhouse_tuba_anti");
	}

	// Token: 0x06002DDE RID: 11742 RVA: 0x0002640E File Offset: 0x0002460E
	public void SoundTubaDeath()
	{
		AudioManager.Play("funhouse_tuba_death");
		this.emitAudioFromObject.Add("funhouse_tuba_death");
	}

	// Token: 0x06002DDF RID: 11743 RVA: 0x0002642A File Offset: 0x0002462A
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.projectile = null;
	}

	// Token: 0x040025FA RID: 9722
	[SerializeField]
	public Transform root;

	// Token: 0x040025FB RID: 9723
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x040025FC RID: 9724
	[SerializeField]
	public Transform startPos;

	// Token: 0x040025FD RID: 9725
	[SerializeField]
	public Transform endPos;

	// Token: 0x040025FE RID: 9726
	public float offset = 50f;

	// Token: 0x040025FF RID: 9727
	public Vector2 start;

	// Token: 0x04002600 RID: 9728
	public Vector2 end;

	// Token: 0x04002601 RID: 9729
	public List<GameObject> bwaaList = new List<GameObject>();
}
