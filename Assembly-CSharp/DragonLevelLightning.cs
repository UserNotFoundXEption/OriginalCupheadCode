using System;
using UnityEngine;

// Token: 0x02000211 RID: 529
public class DragonLevelLightning : AbstractPausableComponent
{
	// Token: 0x0600184D RID: 6221 RVA: 0x000A3924 File Offset: 0x000A1B24
	public void PlayLightning()
	{
		int num = Random.Range(1, 11);
		base.animator.SetInteger("LightningID", num);
		base.animator.SetTrigger("Continue");
		num = Random.Range(0, this.layerOrder.Length);
		this.spriteRenderer.sortingOrder = this.layerOrder[num];
		AudioManager.Play("level_dragon_amb_thunder");
	}

	// Token: 0x040013C2 RID: 5058
	public readonly int[] layerOrder = new int[]
	{
		91,
		93,
		95
	};

	// Token: 0x040013C3 RID: 5059
	[SerializeField]
	public SpriteRenderer spriteRenderer;
}
