using System;
using UnityEngine;

// Token: 0x02000405 RID: 1029
public class CircusPlatformingLevelCannonProjectile : BasicProjectile
{
	// Token: 0x06002CF8 RID: 11512 RVA: 0x000DBA30 File Offset: 0x000D9C30
	public void SetColor(string color)
	{
		int num = Random.Range(0, 2);
		if (color != null)
		{
			if (!(color == "P"))
			{
				if (!(color == "G"))
				{
					if (color == "O")
					{
						base.animator.SetInteger("Variation", num + 4);
					}
				}
				else
				{
					base.animator.SetInteger("Variation", num + 2);
				}
			}
			else
			{
				this.SetParryable(true);
				base.animator.SetInteger("Variation", num);
			}
		}
	}

	// Token: 0x04002535 RID: 9525
	public const string VariationParameterName = "Variation";

	// Token: 0x04002536 RID: 9526
	public const string Pink = "P";

	// Token: 0x04002537 RID: 9527
	public const string Green = "G";

	// Token: 0x04002538 RID: 9528
	public const string Orange = "O";
}
