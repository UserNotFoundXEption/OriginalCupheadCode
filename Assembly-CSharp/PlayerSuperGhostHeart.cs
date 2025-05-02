using System;
using UnityEngine;

// Token: 0x02000530 RID: 1328
public class PlayerSuperGhostHeart : AbstractLevelEntity
{
	// Token: 0x060037E9 RID: 14313 RVA: 0x001057C8 File Offset: 0x001039C8
	public void FixedUpdate()
	{
		base.transform.AddPosition(0f, WeaponProperties.LevelSuperGhost.heartSpeed * CupheadTime.FixedDelta * this.gravityMultiplier, 0f);
		if (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(50f, 50f)))
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060037EA RID: 14314 RVA: 0x00105838 File Offset: 0x00103A38
	public PlayerSuperGhostHeart Create(Vector2 pos, float gravityMultiplier)
	{
		PlayerSuperGhostHeart playerSuperGhostHeart = this.InstantiatePrefab<PlayerSuperGhostHeart>();
		playerSuperGhostHeart.transform.position = pos;
		playerSuperGhostHeart.transform.localScale = new Vector3(playerSuperGhostHeart.transform.localScale.x, gravityMultiplier, playerSuperGhostHeart.transform.localScale.z);
		playerSuperGhostHeart.gravityMultiplier = gravityMultiplier;
		return playerSuperGhostHeart;
	}

	// Token: 0x060037EB RID: 14315 RVA: 0x0002D9FD File Offset: 0x0002BBFD
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		player.stats.AddEx();
		this.spark.Create(base.transform.position);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04002D0F RID: 11535
	public float gravityMultiplier;

	// Token: 0x04002D10 RID: 11536
	[SerializeField]
	public Effect spark;
}
