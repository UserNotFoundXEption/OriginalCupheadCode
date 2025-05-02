using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000267 RID: 615
public class FlyingGenieLevelGem : AbstractProjectile
{
	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x06001C0A RID: 7178 RVA: 0x00017C08 File Offset: 0x00015E08
	public override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001C0B RID: 7179 RVA: 0x000AD48C File Offset: 0x000AB68C
	public FlyingGenieLevelGem Create(Vector2 pos, AbstractPlayerController player, float offsetY, float speed, bool parryable, bool isBig)
	{
		FlyingGenieLevelGem flyingGenieLevelGem = base.Create() as FlyingGenieLevelGem;
		flyingGenieLevelGem.transform.position = pos;
		flyingGenieLevelGem.player = player;
		flyingGenieLevelGem.offsetY = offsetY;
		flyingGenieLevelGem.speed = speed;
		flyingGenieLevelGem.SetParryable(parryable);
		flyingGenieLevelGem.isBig = isBig;
		return flyingGenieLevelGem;
	}

	// Token: 0x06001C0C RID: 7180 RVA: 0x000AD4E0 File Offset: 0x000AB6E0
	public override void Start()
	{
		base.Start();
		int num = (!this.isBig) ? Random.Range(2, 9) : Random.Range(0, 3);
		if (base.CanParry)
		{
			num = 9;
		}
		base.animator.SetFloat("Variation", (float)num / 9f);
		Vector3 vector;
		vector..ctor(0f, this.offsetY, 0f);
		float value = MathUtils.DirectionToAngle(this.player.transform.position - (base.transform.position + vector));
		base.transform.SetEulerAngles(null, null, new float?(value));
		base.StartCoroutine(this.check_gem_cr());
	}

	// Token: 0x06001C0D RID: 7181 RVA: 0x00017C0B File Offset: 0x00015E0B
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001C0E RID: 7182 RVA: 0x000AD5B4 File Offset: 0x000AB7B4
	public IEnumerator check_gem_cr()
	{
		float startPos = (base.transform.position + Vector3.up * this.outOfChestY).y;
		while (base.transform.position.y < startPos)
		{
			base.transform.AddPosition(0f, this.outOfChestY * this.outOfChestSpeed * CupheadTime.Delta, 0f);
			yield return null;
		}
		this.gemRenderer.sortingLayerName = "Projectiles";
		this.gemRenderer.sortingOrder = 2;
		while (base.transform.position.x > -640f && base.transform.position.y < 360f && base.transform.position.y > -360f)
		{
			base.transform.position += base.transform.right * this.speed * CupheadTime.Delta;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06001C0F RID: 7183 RVA: 0x00017C29 File Offset: 0x00015E29
	public override void Die()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040016C0 RID: 5824
	public const int BigVariations = 3;

	// Token: 0x040016C1 RID: 5825
	public const int VariationsTotal = 10;

	// Token: 0x040016C2 RID: 5826
	public const string VariationParameterName = "Variation";

	// Token: 0x040016C3 RID: 5827
	public const string ProjectilesLayer = "Projectiles";

	// Token: 0x040016C4 RID: 5828
	[SerializeField]
	public SpriteRenderer gemRenderer;

	// Token: 0x040016C5 RID: 5829
	[SerializeField]
	public float outOfChestY;

	// Token: 0x040016C6 RID: 5830
	[SerializeField]
	public float outOfChestSpeed;

	// Token: 0x040016C7 RID: 5831
	public AbstractPlayerController player;

	// Token: 0x040016C8 RID: 5832
	public float offsetY;

	// Token: 0x040016C9 RID: 5833
	public bool isBig;

	// Token: 0x040016CA RID: 5834
	public float velocityX;

	// Token: 0x040016CB RID: 5835
	public float speed;
}
