using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000351 RID: 849
public class RumRunnersLevelSnoutTongue : ParrySwitch
{
	// Token: 0x1400004A RID: 74
	// (add) Token: 0x0600253A RID: 9530 RVA: 0x000C5EC0 File Offset: 0x000C40C0
	// (remove) Token: 0x0600253B RID: 9531 RVA: 0x000C5EF8 File Offset: 0x000C40F8
	public event CollisionChild.OnCollisionHandler OnPlayerCollision;

	// Token: 0x0600253C RID: 9532 RVA: 0x0001F61A File Offset: 0x0001D81A
	public void OnEnable()
	{
		base.GetComponent<CollisionChild>().OnPlayerCollision += this.onPlayerCollision;
	}

	// Token: 0x0600253D RID: 9533 RVA: 0x0001F633 File Offset: 0x0001D833
	public void OnDisable()
	{
		base.GetComponent<CollisionChild>().OnPlayerCollision -= this.onPlayerCollision;
	}

	// Token: 0x0600253E RID: 9534 RVA: 0x0001F64C File Offset: 0x0001D84C
	public override void Awake()
	{
		base.Awake();
		base.gameObject.tag = "Enemy";
		this.collisionChild = base.GetComponent<CollisionChild>();
	}

	// Token: 0x0600253F RID: 9535 RVA: 0x0001F670 File Offset: 0x0001D870
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		if (this.parrySpark)
		{
			this.parrySpark.Create(player.transform.position);
		}
		base.FirePrePauseEvent();
		player.stats.ParryOneQuarter();
	}

	// Token: 0x06002540 RID: 9536 RVA: 0x0001F6AA File Offset: 0x0001D8AA
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		base.IsParryable = false;
		this.collisionChild.enabled = false;
		base.StartCoroutine(this.parryCooldown_cr());
	}

	// Token: 0x06002541 RID: 9537 RVA: 0x000C5F30 File Offset: 0x000C4130
	public new IEnumerator parryCooldown_cr()
	{
		float t = 0f;
		while (t < this.coolDown)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		base.IsParryable = true;
		this.collisionChild.enabled = true;
		yield break;
	}

	// Token: 0x06002542 RID: 9538 RVA: 0x0001F6D3 File Offset: 0x0001D8D3
	public void onPlayerCollision(GameObject hit, CollisionPhase phase)
	{
		if (base.IsParryable && this.OnPlayerCollision != null)
		{
			this.OnPlayerCollision(hit, phase);
		}
	}

	// Token: 0x04001ECF RID: 7887
	public CollisionChild collisionChild;
}
