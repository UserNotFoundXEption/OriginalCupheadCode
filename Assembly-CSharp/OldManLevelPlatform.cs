using System;
using UnityEngine;

// Token: 0x020002E2 RID: 738
[Serializable]
public class OldManLevelPlatform
{
	// Token: 0x04001AD9 RID: 6873
	public Transform platform;

	// Token: 0x04001ADA RID: 6874
	public Transform sockBulletPos;

	// Token: 0x04001ADB RID: 6875
	public bool isMoving;

	// Token: 0x04001ADC RID: 6876
	public bool removed;

	// Token: 0x04001ADD RID: 6877
	public float effectiveVel;

	// Token: 0x04001ADE RID: 6878
	public OldManLevelGnomeClimber activeClimber;
}
