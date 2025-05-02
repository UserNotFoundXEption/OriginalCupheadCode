using System;

// Token: 0x02000565 RID: 1381
public class PlanePlayerParryEffect : AbstractParryEffect
{
	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x060039EA RID: 14826 RVA: 0x0002F2C2 File Offset: 0x0002D4C2
	public override bool IsHit
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060039EB RID: 14827 RVA: 0x0002F2C5 File Offset: 0x0002D4C5
	public override void SetPlayer(AbstractPlayerController player)
	{
		base.SetPlayer(player);
		this.planePlayer = (player as PlanePlayerController);
	}

	// Token: 0x060039EC RID: 14828 RVA: 0x0002F2DA File Offset: 0x0002D4DA
	public override void OnSuccess()
	{
		base.OnSuccess();
		this.planePlayer.parryController.OnParrySuccess();
	}

	// Token: 0x04002E7A RID: 11898
	public PlanePlayerController planePlayer;
}
