using System;
using UnityEngine;

// Token: 0x02000206 RID: 518
public class DicePalaceRouletteLevelMarblesLaunch : AbstractMonoBehaviour
{
	// Token: 0x17000287 RID: 647
	// (set) Token: 0x060017CE RID: 6094 RVA: 0x000144B3 File Offset: 0x000126B3
	public bool IsFirstTime
	{
		set
		{
			base.animator.SetBool("IsFirstTime", value);
		}
	}

	// Token: 0x060017CF RID: 6095 RVA: 0x000144C6 File Offset: 0x000126C6
	public void KillMarblesLaunch()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0400134E RID: 4942
	public const string IsFirstTimeParameterName = "IsFirstTime";
}
