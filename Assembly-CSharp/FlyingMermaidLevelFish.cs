using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000282 RID: 642
public class FlyingMermaidLevelFish : AbstractProjectile
{
	// Token: 0x06001D1F RID: 7455 RVA: 0x00018ADC File Offset: 0x00016CDC
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06001D20 RID: 7456 RVA: 0x00018AE4 File Offset: 0x00016CE4
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001D21 RID: 7457 RVA: 0x00018B02 File Offset: 0x00016D02
	public override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001D22 RID: 7458 RVA: 0x00018B1B File Offset: 0x00016D1B
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x000B021C File Offset: 0x000AE41C
	public FlyingMermaidLevelFish Create(Vector2 pos, LevelProperties.FlyingMermaid.Fish properties)
	{
		FlyingMermaidLevelFish flyingMermaidLevelFish = base.Create() as FlyingMermaidLevelFish;
		flyingMermaidLevelFish.properties = properties;
		flyingMermaidLevelFish.transform.position = pos;
		flyingMermaidLevelFish.Init();
		return flyingMermaidLevelFish;
	}

	// Token: 0x06001D24 RID: 7460 RVA: 0x00018B39 File Offset: 0x00016D39
	public void Init()
	{
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06001D25 RID: 7461 RVA: 0x000B0254 File Offset: 0x000AE454
	public IEnumerator loop_cr()
	{
		float velocityY = this.properties.flyingUpSpeed;
		for (;;)
		{
			base.transform.AddPosition(-this.properties.flyingSpeed * CupheadTime.Delta, velocityY * CupheadTime.Delta, 0f);
			velocityY -= this.properties.flyingGravity * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x040017C8 RID: 6088
	public LevelProperties.FlyingMermaid.Fish properties;
}
