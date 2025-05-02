using System;
using UnityEngine;

// Token: 0x02000205 RID: 517
public class DicePalaceRouletteLevelMarble : BasicProjectile
{
	// Token: 0x060017CA RID: 6090 RVA: 0x00014466 File Offset: 0x00012666
	public override void Start()
	{
		base.Start();
		base.animator.Play("Fall", 0, Random.value);
	}

	// Token: 0x060017CB RID: 6091 RVA: 0x000A2868 File Offset: 0x000A0A68
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		if (phase == CollisionPhase.Enter)
		{
			base.animator.SetFloat("Variation", (float)Random.Range(0, 3) / 2f);
			base.animator.SetTrigger("Ground");
			this.move = false;
			this.Speed = 0f;
		}
	}

	// Token: 0x060017CC RID: 6092 RVA: 0x00014484 File Offset: 0x00012684
	public void OnAnimEnd()
	{
		AudioManager.Play("dice_palace_roulette_balls_splat");
		this.emitAudioFromObject.Add("dice_palace_roulette_balls_splat");
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0400134A RID: 4938
	public const string FallState = "Fall";

	// Token: 0x0400134B RID: 4939
	public const string GroundParameterName = "Ground";

	// Token: 0x0400134C RID: 4940
	public const string VariationParameterName = "Variation";

	// Token: 0x0400134D RID: 4941
	public const int VariationCount = 3;
}
