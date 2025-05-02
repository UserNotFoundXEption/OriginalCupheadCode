using System;

// Token: 0x020002BF RID: 703
public class MausoleumLevelRegularGhost : MausoleumLevelGhostBase
{
	// Token: 0x06001F35 RID: 7989 RVA: 0x0001A46E File Offset: 0x0001866E
	public override void Start()
	{
		base.Start();
		base.animator.SetBool("IsA", Rand.Bool());
	}

	// Token: 0x06001F36 RID: 7990 RVA: 0x0001A48B File Offset: 0x0001868B
	public override void OnParry(AbstractPlayerController player)
	{
		AudioManager.Play("mausoleum_regular_ghost_1_death");
		this.emitAudioFromObject.Add("mausoleum_regular_ghost_1_death");
		base.OnParry(player);
	}
}
