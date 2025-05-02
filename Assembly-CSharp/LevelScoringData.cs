using System;
using UnityEngine;

// Token: 0x020005BD RID: 1469
public class LevelScoringData
{
	// Token: 0x06003D9F RID: 15775 RVA: 0x00119354 File Offset: 0x00117554
	public LevelScoringData.Grade CalculateGrade()
	{
		if (this.pacifistRun && !this.usedDjimmi)
		{
			return LevelScoringData.Grade.P;
		}
		ScoringEditorData scoringProperties = Cuphead.Current.ScoringProperties;
		float num = Mathf.Clamp(100f - 100f * (this.time - this.goalTime) / (this.goalTime * (scoringProperties.bestTimeMultiplierForNoScore - 1f)), 0f, 100f);
		float num2 = Mathf.Clamp(100f - 100f * ((scoringProperties.hitsForNoScore - (float)this.finalHP) / scoringProperties.hitsForNoScore), 0f, 100f);
		float num3 = 100f * Mathf.Min((float)this.numParries, scoringProperties.parriesForHighestGrade + scoringProperties.bonusParries) / scoringProperties.parriesForHighestGrade;
		float num4 = 100f * Mathf.Min((float)this.superMeterUsed, scoringProperties.superMeterUsageForHighestGrade + scoringProperties.bonusSuperMeterUsage) / scoringProperties.superMeterUsageForHighestGrade;
		if (this.useCoinsInsteadOfSuperMeter)
		{
			num4 = 100f * ((float)this.coinsCollected / 5f);
		}
		float num5 = num * scoringProperties.timeWeight + num2 * scoringProperties.hitsWeight + num3 * scoringProperties.parriesWeight + num4 * scoringProperties.superMeterUsageWeight;
		ScoringEditorData.GradingCurveEntry[] array = (this.difficulty != Level.Mode.Easy) ? ((this.difficulty != Level.Mode.Normal) ? scoringProperties.hardGradingCurve : scoringProperties.mediumGradingCurve) : scoringProperties.easyGradingCurve;
		LevelScoringData.Grade grade = LevelScoringData.Grade.DMinus;
		foreach (ScoringEditorData.GradingCurveEntry gradingCurveEntry in array)
		{
			grade = gradingCurveEntry.grade;
			if (num5 < gradingCurveEntry.upperLimit)
			{
				break;
			}
		}
		if (this.usedDjimmi && grade > LevelScoringData.Grade.BPlus)
		{
			grade = LevelScoringData.Grade.BPlus;
		}
		return grade;
	}

	// Token: 0x040030F3 RID: 12531
	public float time;

	// Token: 0x040030F4 RID: 12532
	public float goalTime;

	// Token: 0x040030F5 RID: 12533
	public int finalHP;

	// Token: 0x040030F6 RID: 12534
	public int numTimesHit;

	// Token: 0x040030F7 RID: 12535
	public int numParries;

	// Token: 0x040030F8 RID: 12536
	public int superMeterUsed;

	// Token: 0x040030F9 RID: 12537
	public int coinsCollected;

	// Token: 0x040030FA RID: 12538
	public Level.Mode difficulty;

	// Token: 0x040030FB RID: 12539
	public bool pacifistRun;

	// Token: 0x040030FC RID: 12540
	public bool useCoinsInsteadOfSuperMeter;

	// Token: 0x040030FD RID: 12541
	public bool usedDjimmi;

	// Token: 0x040030FE RID: 12542
	public bool player1IsChalice;

	// Token: 0x040030FF RID: 12543
	public bool player2IsChalice;

	// Token: 0x02001241 RID: 4673
	public enum Grade
	{
		// Token: 0x04007E71 RID: 32369
		DMinus,
		// Token: 0x04007E72 RID: 32370
		D,
		// Token: 0x04007E73 RID: 32371
		DPlus,
		// Token: 0x04007E74 RID: 32372
		CMinus,
		// Token: 0x04007E75 RID: 32373
		C,
		// Token: 0x04007E76 RID: 32374
		CPlus,
		// Token: 0x04007E77 RID: 32375
		BMinus,
		// Token: 0x04007E78 RID: 32376
		B,
		// Token: 0x04007E79 RID: 32377
		BPlus,
		// Token: 0x04007E7A RID: 32378
		AMinus,
		// Token: 0x04007E7B RID: 32379
		A,
		// Token: 0x04007E7C RID: 32380
		APlus,
		// Token: 0x04007E7D RID: 32381
		S,
		// Token: 0x04007E7E RID: 32382
		P
	}
}
