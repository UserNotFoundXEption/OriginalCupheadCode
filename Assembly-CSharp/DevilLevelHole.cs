using System;
using System.Collections;

// Token: 0x020001B1 RID: 433
public class DevilLevelHole : AbstractCollidableObject
{
	// Token: 0x17000265 RID: 613
	// (get) Token: 0x060014A1 RID: 5281 RVA: 0x000117C8 File Offset: 0x0000F9C8
	// (set) Token: 0x060014A2 RID: 5282 RVA: 0x000117CF File Offset: 0x0000F9CF
	public static bool PHASE_1_COMPLETE { get; set; }

	// Token: 0x060014A3 RID: 5283 RVA: 0x000117D7 File Offset: 0x0000F9D7
	public void Start()
	{
		DevilLevelHole.PHASE_1_COMPLETE = false;
		base.StartCoroutine(this.check_player_cr());
	}

	// Token: 0x060014A4 RID: 5284 RVA: 0x0009A500 File Offset: 0x00098700
	public IEnumerator check_player_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		for (;;)
		{
			if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null && !PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead)
			{
				if (PlayerManager.GetPlayer(PlayerId.PlayerOne).IsDead)
				{
					if (PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position.y < base.transform.position.y)
					{
						break;
					}
				}
				else if (PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position.y < base.transform.position.y && PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position.y < base.transform.position.y)
				{
					goto Block_6;
				}
			}
			else if (PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position.y < base.transform.position.y)
			{
				goto Block_7;
			}
			yield return null;
		}
		DevilLevelHole.PHASE_1_COMPLETE = true;
		goto IL_1A4;
		Block_6:
		DevilLevelHole.PHASE_1_COMPLETE = true;
		goto IL_1A4;
		Block_7:
		DevilLevelHole.PHASE_1_COMPLETE = true;
		IL_1A4:
		yield break;
	}
}
