using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200015D RID: 349
public class BatLevelLightning : AbstractCollidableObject
{
	// Token: 0x060010D1 RID: 4305 RVA: 0x000911F8 File Offset: 0x0008F3F8
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.collisionChild = this.lightning.GetComponent<CollisionChild>();
		this.collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		this.lightning.SetActive(false);
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x0000E2B7 File Offset: 0x0000C4B7
	public void Init(LevelProperties.Bat.BatLightning properties, Vector2 startPos)
	{
		this.properties = properties;
		base.transform.position = startPos;
		base.StartCoroutine(this.lightning_cr());
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x0009124C File Offset: 0x0008F44C
	public IEnumerator lightning_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.cloudWarning);
		this.lightning.SetActive(true);
		yield return CupheadTime.WaitForSeconds(this, this.properties.lightningOnDuration);
		this.Die();
		yield break;
	}

	// Token: 0x060010D4 RID: 4308 RVA: 0x0000E2DE File Offset: 0x0000C4DE
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060010D5 RID: 4309 RVA: 0x0000E2F5 File Offset: 0x0000C4F5
	public void Die()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000DB0 RID: 3504
	[SerializeField]
	public GameObject lightning;

	// Token: 0x04000DB1 RID: 3505
	public CollisionChild collisionChild;

	// Token: 0x04000DB2 RID: 3506
	public LevelProperties.Bat.BatLightning properties;

	// Token: 0x04000DB3 RID: 3507
	public DamageDealer damageDealer;
}
