using System;
using UnityEngine;

// Token: 0x02000182 RID: 386
public class ChessKingLevelGroundTrigger : AbstractCollidableObject
{
	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06001249 RID: 4681 RVA: 0x0000F734 File Offset: 0x0000D934
	// (set) Token: 0x0600124A RID: 4682 RVA: 0x0000F73C File Offset: 0x0000D93C
	public bool PLAYER_FALLEN { get; set; }

	// Token: 0x0600124B RID: 4683 RVA: 0x0000F745 File Offset: 0x0000D945
	public void CheckPlayer(bool checkPlayer)
	{
		this.checkingPlayer = checkPlayer;
		this.PLAYER_FALLEN = false;
	}

	// Token: 0x0600124C RID: 4684 RVA: 0x00094B64 File Offset: 0x00092D64
	public void Update()
	{
		if (this.checkingPlayer)
		{
			if (PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position.y < base.transform.position.y)
			{
				this.PLAYER_FALLEN = true;
			}
			else
			{
				this.PLAYER_FALLEN = false;
			}
		}
	}

	// Token: 0x0600124D RID: 4685 RVA: 0x00094BC0 File Offset: 0x00092DC0
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(new Vector3(-800f, base.transform.position.y), new Vector3(800f, base.transform.position.y));
	}

	// Token: 0x04000ED0 RID: 3792
	public bool checkingPlayer;
}
