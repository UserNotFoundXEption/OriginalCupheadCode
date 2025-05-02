using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000153 RID: 339
public class BaronessLevelMiniBossBase : AbstractCollidableObject
{
	// Token: 0x1400003F RID: 63
	// (add) Token: 0x06001047 RID: 4167 RVA: 0x0008FA08 File Offset: 0x0008DC08
	// (remove) Token: 0x06001048 RID: 4168 RVA: 0x0008FA40 File Offset: 0x0008DC40
	public event BaronessLevelMiniBossBase.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x06001049 RID: 4169 RVA: 0x0008FA78 File Offset: 0x0008DC78
	public virtual void Start()
	{
		this.endColor = base.GetComponent<SpriteRenderer>().color;
		base.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1f);
		base.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Background.ToString();
		base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 150;
		base.StartCoroutine(this.switchLayer_cr(this.layerSwitch));
	}

	// Token: 0x0600104A RID: 4170 RVA: 0x0000DBCC File Offset: 0x0000BDCC
	public virtual void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
	}

	// Token: 0x0600104B RID: 4171 RVA: 0x0008FB04 File Offset: 0x0008DD04
	public virtual IEnumerator switchLayer_cr(int layerswitch)
	{
		base.StartCoroutine(this.fade_color_cr());
		yield return CupheadTime.WaitForSeconds(this, 3f);
		base.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Enemies.ToString();
		base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 260;
		yield break;
	}

	// Token: 0x0600104C RID: 4172 RVA: 0x0008FB20 File Offset: 0x0008DD20
	public virtual IEnumerator fade_color_cr()
	{
		float t = 0f;
		Color start = new Color(0f, 0f, 0f, 1f);
		while (t < this.fadeTime)
		{
			base.GetComponent<SpriteRenderer>().color = Color.Lerp(start, this.endColor, t / this.fadeTime);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.GetComponent<SpriteRenderer>().color = this.endColor;
		yield return null;
		yield break;
	}

	// Token: 0x0600104D RID: 4173 RVA: 0x0000DBEA File Offset: 0x0000BDEA
	public virtual float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x0600104E RID: 4174 RVA: 0x0000DC0B File Offset: 0x0000BE0B
	public virtual void StartExplosions()
	{
		if (base.GetComponent<LevelBossDeathExploder>() != null)
		{
			base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		}
	}

	// Token: 0x0600104F RID: 4175 RVA: 0x0000DC29 File Offset: 0x0000BE29
	public virtual void EndExplosions()
	{
		if (base.GetComponent<LevelBossDeathExploder>() != null)
		{
			base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		}
	}

	// Token: 0x06001050 RID: 4176 RVA: 0x0000DC47 File Offset: 0x0000BE47
	public virtual void Die()
	{
		this.EndExplosions();
		this.StopAllCoroutines();
		if (base.GetComponent<Collider2D>() != null)
		{
			base.GetComponent<Collider2D>().enabled = false;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000D3B RID: 3387
	public bool isDying;

	// Token: 0x04000D3C RID: 3388
	public bool startInvisible;

	// Token: 0x04000D3D RID: 3389
	public int layerSwitch = 4;

	// Token: 0x04000D3E RID: 3390
	public BaronessLevelCastle.BossPossibility bossId;

	// Token: 0x04000D40 RID: 3392
	public float fadeTime = 0.5f;

	// Token: 0x04000D41 RID: 3393
	public Color endColor;

	// Token: 0x02000A34 RID: 2612
	// (Invoke) Token: 0x060058E5 RID: 22757
	public delegate void OnDamageTakenHandler(float damage);
}
