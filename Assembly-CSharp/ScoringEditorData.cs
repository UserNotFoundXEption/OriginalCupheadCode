using System;

// Token: 0x020005BE RID: 1470
public class ScoringEditorData : AbstractMonoBehaviour
{
	// Token: 0x04003100 RID: 12544
	public float bestTimeMultiplierForNoScore;

	// Token: 0x04003101 RID: 12545
	public float hitsForNoScore;

	// Token: 0x04003102 RID: 12546
	public float parriesForHighestGrade;

	// Token: 0x04003103 RID: 12547
	public float bonusParries;

	// Token: 0x04003104 RID: 12548
	public float superMeterUsageForHighestGrade;

	// Token: 0x04003105 RID: 12549
	public float bonusSuperMeterUsage;

	// Token: 0x04003106 RID: 12550
	public ScoringEditorData.GradingCurveEntry[] easyGradingCurve;

	// Token: 0x04003107 RID: 12551
	public ScoringEditorData.GradingCurveEntry[] mediumGradingCurve;

	// Token: 0x04003108 RID: 12552
	public ScoringEditorData.GradingCurveEntry[] hardGradingCurve;

	// Token: 0x04003109 RID: 12553
	public float timeWeight;

	// Token: 0x0400310A RID: 12554
	public float hitsWeight;

	// Token: 0x0400310B RID: 12555
	public float parriesWeight;

	// Token: 0x0400310C RID: 12556
	public float superMeterUsageWeight;

	// Token: 0x02001242 RID: 4674
	[Serializable]
	public class GradingCurveEntry
	{
		// Token: 0x04007E7F RID: 32383
		public LevelScoringData.Grade grade;

		// Token: 0x04007E80 RID: 32384
		public float upperLimit;
	}
}
