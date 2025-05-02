using System;
using UnityEngine;

// Token: 0x020001E7 RID: 487
public class DicePalaceDominoLevelScrollingFloor : MonoBehaviour
{
	// Token: 0x06001682 RID: 5762 RVA: 0x000132B3 File Offset: 0x000114B3
	public void Start()
	{
		this.RefreshTilesAndSpikes();
	}

	// Token: 0x06001683 RID: 5763 RVA: 0x0009F1B0 File Offset: 0x0009D3B0
	public void Update()
	{
		Vector3 position = base.transform.position;
		position.x -= this.speed * CupheadTime.Delta;
		base.transform.position = position;
		if (base.transform.position.x <= 0f)
		{
			position.x += this.resetPositionX;
			base.transform.position = position;
			this.RefreshTilesAndSpikes();
		}
	}

	// Token: 0x06001684 RID: 5764 RVA: 0x0009F238 File Offset: 0x0009D438
	public void RefreshTilesAndSpikes()
	{
		for (int i = 0; i < this.dominoLevelRandomTiles.Length; i++)
		{
			this.dominoLevelRandomTiles[i].ChangeTile();
		}
		for (int j = 0; j < this.dominoLevelRandomSpikes.Length; j++)
		{
			this.dominoLevelRandomSpikes[j].ChangeSpikes();
		}
	}

	// Token: 0x06001685 RID: 5765 RVA: 0x000132BB File Offset: 0x000114BB
	public void OnDestroy()
	{
		this.dominoLevelRandomTiles = null;
		this.dominoLevelRandomSpikes = null;
	}

	// Token: 0x04001242 RID: 4674
	public float speed;

	// Token: 0x04001243 RID: 4675
	public float resetPositionX = 2808f;

	// Token: 0x04001244 RID: 4676
	[SerializeField]
	public DicePalaceDominoLevelRandomTile[] dominoLevelRandomTiles;

	// Token: 0x04001245 RID: 4677
	[SerializeField]
	public DicePalaceDominoLevelRandomSpike[] dominoLevelRandomSpikes;
}
