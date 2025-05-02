using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000413 RID: 1043
public class FunhousePlatformingLevelCannonProjectile : BasicProjectile
{
	// Token: 0x1700035C RID: 860
	// (get) Token: 0x06002D64 RID: 11620 RVA: 0x00025E3B File Offset: 0x0002403B
	// (set) Token: 0x06002D65 RID: 11621 RVA: 0x00025E43 File Offset: 0x00024043
	public EnemyProperties Properties { get; set; }

	// Token: 0x06002D66 RID: 11622 RVA: 0x00025E4C File Offset: 0x0002404C
	public override void Start()
	{
		base.Start();
		base.animator.Play("anim_level_starcannon_bullet", -1, Random.value);
	}

	// Token: 0x06002D67 RID: 11623 RVA: 0x00025E6A File Offset: 0x0002406A
	public void Init()
	{
		base.StartCoroutine(this.delayedDeath_cr());
	}

	// Token: 0x06002D68 RID: 11624 RVA: 0x000DCB3C File Offset: 0x000DAD3C
	public IEnumerator delayedDeath_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.Properties.bulletDeathTime);
		this.Die();
		yield break;
	}

	// Token: 0x06002D69 RID: 11625 RVA: 0x000DCB58 File Offset: 0x000DAD58
	public override void Die()
	{
		base.Die();
		Effect effect = this.deathFx.Create(base.transform.position, new Vector3(1.25f, 1.25f, 1f));
		effect.animator.SetInteger("PickAni", Random.Range(0, 3));
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002D6A RID: 11626 RVA: 0x000DCBB8 File Offset: 0x000DADB8
	public override void Move()
	{
		if (this.Speed == 0f)
		{
		}
		base.transform.position += this.direction * this.Speed * CupheadTime.FixedDelta - new Vector3(0f, this._accumulativeGravity * CupheadTime.FixedDelta, 0f);
		this._accumulativeGravity += this.Gravity * CupheadTime.FixedDelta;
	}

	// Token: 0x06002D6B RID: 11627 RVA: 0x00025E79 File Offset: 0x00024079
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.deathFx = null;
	}

	// Token: 0x0400259D RID: 9629
	[SerializeField]
	public Effect deathFx;

	// Token: 0x0400259F RID: 9631
	public Vector3 direction;
}
