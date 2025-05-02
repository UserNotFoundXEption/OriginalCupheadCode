using System;
using UnityEngine;

// Token: 0x02000269 RID: 617
public class FlyingGenieLevelGenieHead : AbstractProjectile
{
	// Token: 0x06001C49 RID: 7241 RVA: 0x00017EF8 File Offset: 0x000160F8
	public void Init(Vector3 pos, float health, FlyingGenieLevelGenie parent)
	{
		base.transform.position = pos;
		this.parent = parent;
		this.health = health;
	}

	// Token: 0x06001C4A RID: 7242 RVA: 0x000ADE48 File Offset: 0x000AC048
	public override void Start()
	{
		base.Start();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.darkSprite.sortingOrder = base.GetComponent<SpriteRenderer>().sortingOrder + 1;
	}

	// Token: 0x06001C4B RID: 7243 RVA: 0x00017F14 File Offset: 0x00016114
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001C4C RID: 7244 RVA: 0x00017F32 File Offset: 0x00016132
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001C4D RID: 7245 RVA: 0x00017F50 File Offset: 0x00016150
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f)
		{
			this.Die();
		}
		this.parent.DoDamage(info.damage);
	}

	// Token: 0x06001C4E RID: 7246 RVA: 0x000ADE98 File Offset: 0x000AC098
	public override void Die()
	{
		AudioManager.Play("genie_pillar_destruction");
		this.emitAudioFromObject.Add("genie_pillar_destruction");
		this.headExplode.Create(new Vector3(base.transform.position.x - 75f, base.transform.position.y));
		base.GetComponent<SpriteRenderer>().enabled = false;
		this.darkSprite.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
	}

	// Token: 0x040016FD RID: 5885
	[SerializeField]
	public Effect headExplode;

	// Token: 0x040016FE RID: 5886
	[SerializeField]
	public SpriteRenderer darkSprite;

	// Token: 0x040016FF RID: 5887
	public DamageReceiver damageReceiver;

	// Token: 0x04001700 RID: 5888
	public FlyingGenieLevelGenie parent;

	// Token: 0x04001701 RID: 5889
	public float health;
}
