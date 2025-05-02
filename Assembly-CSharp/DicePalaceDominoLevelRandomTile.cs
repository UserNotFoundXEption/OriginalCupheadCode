using System;
using UnityEngine;

// Token: 0x020001E6 RID: 486
public class DicePalaceDominoLevelRandomTile : AbstractMonoBehaviour
{
	// Token: 0x06001680 RID: 5760 RVA: 0x0009F174 File Offset: 0x0009D374
	public void ChangeTile()
	{
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		if (component == null)
		{
			return;
		}
		component.sprite = this.sprites[Random.Range(0, this.sprites.Length)];
	}

	// Token: 0x04001241 RID: 4673
	public Sprite[] sprites;
}
