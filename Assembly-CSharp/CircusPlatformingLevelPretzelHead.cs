using System;
using UnityEngine;

// Token: 0x0200040F RID: 1039
public class CircusPlatformingLevelPretzelHead : MonoBehaviour
{
	// Token: 0x06002D4C RID: 11596 RVA: 0x00025CFB File Offset: 0x00023EFB
	public void JumpSFX()
	{
		this.circusPlatformingLevelPretzel.JumpSFX();
	}

	// Token: 0x04002589 RID: 9609
	[SerializeField]
	public CircusPlatformingLevelPretzel circusPlatformingLevelPretzel;
}
