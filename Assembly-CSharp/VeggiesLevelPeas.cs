using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003D1 RID: 977
public class VeggiesLevelPeas : LevelProperties.Veggies.Entity
{
	// Token: 0x1700033E RID: 830
	// (get) Token: 0x06002B0F RID: 11023 RVA: 0x0002428B File Offset: 0x0002248B
	// (set) Token: 0x06002B10 RID: 11024 RVA: 0x00024293 File Offset: 0x00022493
	public VeggiesLevelPeas.State state { get; set; }

	// Token: 0x1400005F RID: 95
	// (add) Token: 0x06002B11 RID: 11025 RVA: 0x000D55B8 File Offset: 0x000D37B8
	// (remove) Token: 0x06002B12 RID: 11026 RVA: 0x000D55F0 File Offset: 0x000D37F0
	public event VeggiesLevelPeas.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x06002B13 RID: 11027 RVA: 0x0002429C File Offset: 0x0002249C
	public void Start()
	{
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06002B14 RID: 11028 RVA: 0x000242AA File Offset: 0x000224AA
	public override void LevelInitWithGroup(AbstractLevelPropertyGroup propertyGroup)
	{
		base.LevelInitWithGroup(propertyGroup);
		this.properties = (propertyGroup as LevelProperties.Veggies.Peas);
		this.hp = (float)this.properties.hp;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002B15 RID: 11029 RVA: 0x000D5628 File Offset: 0x000D3828
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06002B16 RID: 11030 RVA: 0x000242E8 File Offset: 0x000224E8
	public void OnInAnimComplete()
	{
		base.GetComponent<Collider2D>().enabled = true;
		base.StartCoroutine(this.peas_cr());
	}

	// Token: 0x06002B17 RID: 11031 RVA: 0x00024303 File Offset: 0x00022503
	public void OnDeathAnimComplete()
	{
		this.state = VeggiesLevelPeas.State.Complete;
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002B18 RID: 11032 RVA: 0x00024317 File Offset: 0x00022517
	public void Die()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x06002B19 RID: 11033 RVA: 0x000D567C File Offset: 0x000D387C
	public IEnumerator peas_cr()
	{
		yield return null;
		yield break;
	}

	// Token: 0x06002B1A RID: 11034 RVA: 0x000D5690 File Offset: 0x000D3890
	public IEnumerator die_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("Idle");
		yield return base.StartCoroutine(base.dieFlash_cr());
		base.animator.SetTrigger("Dead");
		yield break;
	}

	// Token: 0x040023CE RID: 9166
	[SerializeField]
	public VeggiesLevelOnionTearProjectile projectilePrefab;

	// Token: 0x040023CF RID: 9167
	public new LevelProperties.Veggies.Peas properties;

	// Token: 0x040023D0 RID: 9168
	public float hp;

	// Token: 0x02000FF2 RID: 4082
	public enum State
	{
		// Token: 0x04007251 RID: 29265
		Start,
		// Token: 0x04007252 RID: 29266
		Complete
	}

	// Token: 0x02000FF3 RID: 4083
	// (Invoke) Token: 0x060076C3 RID: 30403
	public delegate void OnDamageTakenHandler(float damage);
}
