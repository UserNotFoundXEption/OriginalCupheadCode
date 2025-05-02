using System;

// Token: 0x020002F5 RID: 757
public class PirateLevelDogFishScope : AbstractMonoBehaviour
{
	// Token: 0x060021AC RID: 8620 RVA: 0x0001CCC7 File Offset: 0x0001AEC7
	public void In()
	{
		base.animator.Play("In");
	}

	// Token: 0x060021AD RID: 8621 RVA: 0x0001CCD9 File Offset: 0x0001AED9
	public void SoundDogfishPeriStart()
	{
		AudioManager.Play("level_pirate_periscope_warning");
	}
}
