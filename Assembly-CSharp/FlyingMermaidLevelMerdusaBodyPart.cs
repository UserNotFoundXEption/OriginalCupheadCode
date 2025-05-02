using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000289 RID: 649
public class FlyingMermaidLevelMerdusaBodyPart : LevelProperties.FlyingMermaid.Entity
{
	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x06001D54 RID: 7508 RVA: 0x00018D55 File Offset: 0x00016F55
	// (set) Token: 0x06001D55 RID: 7509 RVA: 0x00018D5D File Offset: 0x00016F5D
	public bool IsSinking { get; set; }

	// Token: 0x06001D56 RID: 7510 RVA: 0x00018D66 File Offset: 0x00016F66
	public override void Awake()
	{
		base.Awake();
		if (this.damagePlayer)
		{
			this.damageDealer = DamageDealer.NewEnemy();
		}
		base.StartCoroutine(this.main_cr());
		base.StartCoroutine(this.check_to_delete_cr());
	}

	// Token: 0x06001D57 RID: 7511 RVA: 0x00018D9E File Offset: 0x00016F9E
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001D58 RID: 7512 RVA: 0x00018DC7 File Offset: 0x00016FC7
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001D59 RID: 7513 RVA: 0x000B0768 File Offset: 0x000AE968
	public IEnumerator main_cr()
	{
		AudioManager.Play("level_mermaid_merdusa_fallapart_break");
		yield return CupheadTime.WaitForSeconds(this, this.waitTime);
		if (this.stopBobbingAfterWait)
		{
			base.GetComponent<FlyingMermaidLevelFloater>().enabled = false;
		}
		this.IsSinking = true;
		float t = 0f;
		while (t < this.moveTime)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.Delta, this.velocity.y * CupheadTime.Delta, 0f);
			base.transform.Rotate(0f, 0f, this.rotationSpeed * CupheadTime.Delta);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001D5A RID: 7514 RVA: 0x000B0784 File Offset: 0x000AE984
	public FlyingMermaidLevelMerdusaBodyPart Create(Vector2 pos)
	{
		FlyingMermaidLevelMerdusaBodyPart flyingMermaidLevelMerdusaBodyPart = Object.Instantiate<FlyingMermaidLevelMerdusaBodyPart>(this);
		flyingMermaidLevelMerdusaBodyPart.transform.SetPosition(new float?(pos.x), new float?(pos.y), null);
		return flyingMermaidLevelMerdusaBodyPart;
	}

	// Token: 0x06001D5B RID: 7515 RVA: 0x000B07C8 File Offset: 0x000AE9C8
	public IEnumerator check_to_delete_cr()
	{
		while (base.transform.position.x >= -1140f && base.transform.position.x <= 1140f && base.transform.position.y >= -860f && base.transform.position.y <= 1220f)
		{
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040017EE RID: 6126
	[SerializeField]
	public float waitTime;

	// Token: 0x040017EF RID: 6127
	[SerializeField]
	public Vector2 velocity;

	// Token: 0x040017F0 RID: 6128
	[SerializeField]
	public float moveTime;

	// Token: 0x040017F1 RID: 6129
	[SerializeField]
	public bool stopBobbingAfterWait;

	// Token: 0x040017F2 RID: 6130
	[SerializeField]
	public float rotationSpeed;

	// Token: 0x040017F3 RID: 6131
	[SerializeField]
	public bool damagePlayer;

	// Token: 0x040017F4 RID: 6132
	public DamageDealer damageDealer;
}
