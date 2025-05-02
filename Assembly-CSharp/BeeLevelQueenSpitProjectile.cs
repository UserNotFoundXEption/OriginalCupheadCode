using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200016B RID: 363
public class BeeLevelQueenSpitProjectile : AbstractProjectile
{
	// Token: 0x06001170 RID: 4464 RVA: 0x000926BC File Offset: 0x000908BC
	public BeeLevelQueenSpitProjectile Create(Vector2 pos, Vector2 scale, float speed, Vector2 time)
	{
		BeeLevelQueenSpitProjectile beeLevelQueenSpitProjectile = base.Create(pos, 0f, scale) as BeeLevelQueenSpitProjectile;
		beeLevelQueenSpitProjectile.speed = speed;
		beeLevelQueenSpitProjectile.time = time;
		return beeLevelQueenSpitProjectile;
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06001171 RID: 4465 RVA: 0x0000ECA2 File Offset: 0x0000CEA2
	public override float DestroyLifetime
	{
		get
		{
			return 1000f;
		}
	}

	// Token: 0x06001172 RID: 4466 RVA: 0x0000ECA9 File Offset: 0x0000CEA9
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.rotate_cr());
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.trail_cr());
	}

	// Token: 0x06001173 RID: 4467 RVA: 0x0000ECD8 File Offset: 0x0000CED8
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001174 RID: 4468 RVA: 0x0000ECF6 File Offset: 0x0000CEF6
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001175 RID: 4469 RVA: 0x0000ED1F File Offset: 0x0000CF1F
	public void End()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001176 RID: 4470 RVA: 0x000926EC File Offset: 0x000908EC
	public IEnumerator trail_cr()
	{
		for (;;)
		{
			this.trailPrefab.Create(base.transform.position);
			yield return CupheadTime.WaitForSeconds(this, 0.25f);
		}
		yield break;
	}

	// Token: 0x06001177 RID: 4471 RVA: 0x00092708 File Offset: 0x00090908
	public IEnumerator move_cr()
	{
		float scale = base.transform.localScale.x;
		for (;;)
		{
			Vector2 move = base.transform.right * this.speed * CupheadTime.Delta * scale;
			base.transform.AddPosition(move.x, move.y, 0f);
			yield return null;
			if (base.transform.position.y > 720f)
			{
				this.End();
			}
		}
		yield break;
	}

	// Token: 0x06001178 RID: 4472 RVA: 0x00092724 File Offset: 0x00090924
	public IEnumerator rotate_cr()
	{
		float rotTime = 0.15f;
		float scale = base.transform.localScale.x;
		yield return CupheadTime.WaitForSeconds(this, 0.05f);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.time.x);
			yield return base.StartCoroutine(this.tweenRotation_cr(0f, 90f * scale, rotTime));
			yield return CupheadTime.WaitForSeconds(this, this.time.y);
			yield return base.StartCoroutine(this.tweenRotation_cr(90f * scale, 180f * scale, rotTime));
			AudioManager.Play("bee_spit_bullet_turn");
			this.emitAudioFromObject.Add("bee_spit_bullet_turn");
			yield return CupheadTime.WaitForSeconds(this, this.time.x);
			yield return base.StartCoroutine(this.tweenRotation_cr(180f * scale, 90f * scale, rotTime));
			yield return CupheadTime.WaitForSeconds(this, this.time.y);
			yield return base.StartCoroutine(this.tweenRotation_cr(90f * scale, 0f, rotTime));
		}
		yield break;
	}

	// Token: 0x06001179 RID: 4473 RVA: 0x00092740 File Offset: 0x00090940
	public IEnumerator tweenRotation_cr(float start, float end, float time)
	{
		base.transform.SetEulerAngles(null, null, new float?(start));
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			base.transform.SetEulerAngles(null, null, new float?(EaseUtils.Ease(EaseUtils.EaseType.linear, start, end, val)));
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetEulerAngles(null, null, new float?(end));
		yield break;
	}

	// Token: 0x0600117A RID: 4474 RVA: 0x00092770 File Offset: 0x00090970
	public IEnumerator tween_cr(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease)
	{
		base.transform.position = start;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			float x = EaseUtils.Ease(ease, start.x, end.x, val);
			float y = EaseUtils.Ease(ease, start.y, end.y, val);
			base.transform.SetLocalPosition(new float?(x), new float?(y), new float?(0f));
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = end;
		yield break;
	}

	// Token: 0x0600117B RID: 4475 RVA: 0x0000ED32 File Offset: 0x0000CF32
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.trailPrefab = null;
	}

	// Token: 0x04000E12 RID: 3602
	[SerializeField]
	public Effect trailPrefab;

	// Token: 0x04000E13 RID: 3603
	public Vector2 time = new Vector2(0.43f, 0.06f);

	// Token: 0x04000E14 RID: 3604
	public float speed = 700f;
}
