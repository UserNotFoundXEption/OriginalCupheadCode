using System;
using UnityEngine;

// Token: 0x020001A8 RID: 424
public class ClownLevelRiders : AbstractCollidableObject
{
	// Token: 0x0600142C RID: 5164 RVA: 0x00010FCD File Offset: 0x0000F1CD
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		if (this.inFront)
		{
			base.animator.SetBool("InFront", true);
		}
		else
		{
			base.animator.SetBool("InFront", false);
		}
	}

	// Token: 0x0600142D RID: 5165 RVA: 0x0001100C File Offset: 0x0000F20C
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x00011024 File Offset: 0x0000F224
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600142F RID: 5167 RVA: 0x00099970 File Offset: 0x00097B70
	public void BackLayers(int cartLayer)
	{
		this.sprites = base.GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < this.sprites.Length; i++)
		{
			this.sprites[i].sortingLayerName = "Background";
			this.sprites[i].sortingOrder = cartLayer;
		}
	}

	// Token: 0x06001430 RID: 5168 RVA: 0x000999C4 File Offset: 0x00097BC4
	public void FrontLayers(int cartLayer)
	{
		this.sprites = base.GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < this.sprites.Length; i++)
		{
			if (this.sprites[i] == this.backRider || this.sprites[i] == this.backSeat)
			{
				this.sprites[i].sortingLayerName = "Default";
				this.sprites[i].sortingOrder = 10 - i;
			}
			else
			{
				this.sprites[i].sortingLayerName = "Player";
				this.sprites[i].sortingOrder = cartLayer;
			}
		}
	}

	// Token: 0x0400106B RID: 4203
	[SerializeField]
	public SpriteRenderer backSeat;

	// Token: 0x0400106C RID: 4204
	[SerializeField]
	public SpriteRenderer backRider;

	// Token: 0x0400106D RID: 4205
	public bool inFront;

	// Token: 0x0400106E RID: 4206
	public DamageDealer damageDealer;

	// Token: 0x0400106F RID: 4207
	public SpriteRenderer[] sprites;
}
