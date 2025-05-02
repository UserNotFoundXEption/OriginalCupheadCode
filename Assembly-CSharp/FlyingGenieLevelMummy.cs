using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200026F RID: 623
public class FlyingGenieLevelMummy : BasicProjectile
{
	// Token: 0x170002AC RID: 684
	// (get) Token: 0x06001C96 RID: 7318 RVA: 0x000183FC File Offset: 0x000165FC
	// (set) Token: 0x06001C97 RID: 7319 RVA: 0x00018404 File Offset: 0x00016604
	public FlyingGenieLevelMummy.MummyType type { get; set; }

	// Token: 0x06001C98 RID: 7320 RVA: 0x000AE670 File Offset: 0x000AC870
	public FlyingGenieLevelMummy Create(Vector3 position, float speed, float rotation, LevelProperties.FlyingGenie.Coffin properties, FlyingGenieLevelMummy.MummyType type, float hp, int sortingOrder)
	{
		FlyingGenieLevelMummy flyingGenieLevelMummy = base.Create(position, rotation, speed) as FlyingGenieLevelMummy;
		flyingGenieLevelMummy.transform.position = position;
		flyingGenieLevelMummy.properties = properties;
		flyingGenieLevelMummy.type = type;
		flyingGenieLevelMummy.hp = hp;
		flyingGenieLevelMummy.rotation = rotation;
		flyingGenieLevelMummy.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
		flyingGenieLevelMummy.purpleSprite.sortingOrder = sortingOrder + 1;
		flyingGenieLevelMummy.purpleColor = this.purpleSprite.color;
		return flyingGenieLevelMummy;
	}

	// Token: 0x06001C99 RID: 7321 RVA: 0x0001840D File Offset: 0x0001660D
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06001C9A RID: 7322 RVA: 0x000AE6EC File Offset: 0x000AC8EC
	public override void Start()
	{
		base.Start();
		AudioManager.Play("genie_mummy_voice_attack");
		this.emitAudioFromObject.Add("genie_mummy_voice_attack");
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.fade_purple_cr());
		FlyingGenieLevelMummy.MummyType type = this.type;
		if (type != FlyingGenieLevelMummy.MummyType.Classic)
		{
			if (type != FlyingGenieLevelMummy.MummyType.Chomper)
			{
				if (type == FlyingGenieLevelMummy.MummyType.Grabby)
				{
					base.animator.Play("Grabby");
				}
			}
			else
			{
				base.animator.Play("Chomper");
			}
		}
		else
		{
			this.CalculateSin();
			if (this.properties.mummyASinWave)
			{
				base.StartCoroutine(this.classic_bounce_cr());
			}
			base.animator.Play("Classic");
		}
	}

	// Token: 0x06001C9B RID: 7323 RVA: 0x000AE7CC File Offset: 0x000AC9CC
	public IEnumerator fade_purple_cr()
	{
		float t = 0f;
		float time = 1.5f;
		Color start = this.purpleSprite.color;
		Color end = new Color(1f, 1f, 1f, 0f);
		while (t < time)
		{
			this.purpleSprite.color = Color.Lerp(start, end, t / time);
			this.purpleColor = this.purpleSprite.color;
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001C9C RID: 7324 RVA: 0x000AE7E8 File Offset: 0x000AC9E8
	public void CalculateSin()
	{
		Vector3 vector = MathUtils.AngleToDirection(this.rotation);
		Vector2 zero = Vector2.zero;
		zero.x = (vector.x + base.transform.position.x) / 2f;
		zero.y = (vector.y + base.transform.position.y) / 2f;
		float num = -((vector.x - base.transform.position.x) / (vector.y - base.transform.position.y));
		float num2 = zero.y - num * zero.x;
		Vector2 zero2 = Vector2.zero;
		zero2.x = zero.x + 1f;
		zero2.y = num * zero2.x + num2;
		this.normalized = Vector3.zero;
		this.normalized = zero2 - zero;
		this.normalized.Normalize();
	}

	// Token: 0x06001C9D RID: 7325 RVA: 0x000AE904 File Offset: 0x000ACB04
	public IEnumerator classic_bounce_cr()
	{
		Vector3 pos = base.transform.position;
		float angle = 0f;
		for (;;)
		{
			angle += 10f * CupheadTime.Delta;
			if (CupheadTime.Delta != 0f)
			{
				pos = this.normalized * Mathf.Sin(angle) * 2f;
				base.transform.position += pos;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001C9E RID: 7326 RVA: 0x000AE920 File Offset: 0x000ACB20
	public IEnumerator grabby_speed_cr()
	{
		float t = 0f;
		float time = 0.5f;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			this.Speed = Mathf.Lerp(-this.properties.mummyCSpeed, 0f, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001C9F RID: 7327 RVA: 0x00018438 File Offset: 0x00016638
	public void ChangeSpeed()
	{
		if (this.properties.mummyCSlowdown)
		{
			this.Speed = -this.properties.mummyCSpeed;
			base.StartCoroutine(this.grabby_speed_cr());
		}
	}

	// Token: 0x06001CA0 RID: 7328 RVA: 0x000AE93C File Offset: 0x000ACB3C
	public override void Die()
	{
		this.StopAllCoroutines();
		this.Explosion();
		base.Die();
		AudioManager.Stop("genie_mummy_voice_attack");
		AudioManager.Play("genie_mummy_voice_die");
		this.emitAudioFromObject.Add("genie_mummy_voice_die");
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x06001CA1 RID: 7329 RVA: 0x00018469 File Offset: 0x00016669
	public void Explosion()
	{
		this.sparkFX.Create(base.transform.position, this.purpleColor);
	}

	// Token: 0x04001736 RID: 5942
	[SerializeField]
	public FlyingGenieLevelMummyDeathEffect sparkFX;

	// Token: 0x04001737 RID: 5943
	[SerializeField]
	public SpriteRenderer purpleSprite;

	// Token: 0x04001739 RID: 5945
	public LevelProperties.FlyingGenie.Coffin properties;

	// Token: 0x0400173A RID: 5946
	public Vector3 normalized;

	// Token: 0x0400173B RID: 5947
	public DamageReceiver damageReceiver;

	// Token: 0x0400173C RID: 5948
	public float hp;

	// Token: 0x0400173D RID: 5949
	public float rotation;

	// Token: 0x0400173E RID: 5950
	public Color purpleColor;

	// Token: 0x02000D14 RID: 3348
	public enum MummyType
	{
		// Token: 0x04005EF0 RID: 24304
		Classic,
		// Token: 0x04005EF1 RID: 24305
		Chomper,
		// Token: 0x04005EF2 RID: 24306
		Grabby
	}
}
