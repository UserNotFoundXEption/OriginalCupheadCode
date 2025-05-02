using System;
using UnityEngine;

// Token: 0x02000379 RID: 889
public class SaltbakerLevelMintHandSFXHandler : MonoBehaviour
{
	// Token: 0x06002738 RID: 10040 RVA: 0x00020F7D File Offset: 0x0001F17D
	public void AniEvent_SFXLeafRustle()
	{
		this.main.SFXLeafRustle();
	}

	// Token: 0x06002739 RID: 10041 RVA: 0x00020F8A File Offset: 0x0001F18A
	public void AniEvent_SFXLaunchThrow()
	{
		this.main.SFXLaunchThrow();
	}

	// Token: 0x04002076 RID: 8310
	[SerializeField]
	public SaltbakerLevelSaltbaker main;
}
