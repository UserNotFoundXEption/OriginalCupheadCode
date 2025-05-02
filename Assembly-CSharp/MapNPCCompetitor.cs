using System;
using UnityEngine;

// Token: 0x02000494 RID: 1172
public class MapNPCCompetitor : AbstractMonoBehaviour
{
	// Token: 0x06003129 RID: 12585 RVA: 0x000E8CD4 File Offset: 0x000E6ED4
	public void Update()
	{
		base.animator.SetBool("PlayerClose", this.interaction.PlayerWithinDistance(0) || (PlayerManager.Multiplayer && this.interaction.PlayerWithinDistance(1)) || this.interaction.currentlySpeaking);
		this.blinkTimer -= CupheadTime.Delta;
		if (this.blinkTimer < 0f)
		{
			this.blinkTimer = this.blinkRange.RandomFloat();
			base.animator.SetTrigger("Blink");
		}
	}

	// Token: 0x04002890 RID: 10384
	[SerializeField]
	public MapDialogueInteraction interaction;

	// Token: 0x04002891 RID: 10385
	[SerializeField]
	public MinMax blinkRange = new MinMax(2.5f, 4.5f);

	// Token: 0x04002892 RID: 10386
	public float blinkTimer;
}
