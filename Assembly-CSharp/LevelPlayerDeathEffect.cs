using System;
using UnityEngine;

// Token: 0x02000512 RID: 1298
public class LevelPlayerDeathEffect : Effect
{
	// Token: 0x06003670 RID: 13936 RVA: 0x000FE844 File Offset: 0x000FCA44
	public void Init(Vector2 pos, PlayerId id, bool playerGrounded)
	{
		base.transform.position = pos;
		if (id == PlayerId.PlayerOne || id != PlayerId.PlayerTwo)
		{
			if (PlayerManager.GetPlayer(id).stats.isChalice)
			{
				this.chalice.enabled = true;
			}
			else if (PlayerManager.player1IsMugman)
			{
				this.mugman.enabled = true;
			}
			else
			{
				this.cuphead.enabled = true;
			}
		}
		else if (PlayerManager.GetPlayer(id).stats.isChalice)
		{
			this.chalice.enabled = true;
		}
		else if (PlayerManager.player1IsMugman)
		{
			this.cuphead.enabled = true;
		}
		else
		{
			this.mugman.enabled = true;
		}
		if (playerGrounded)
		{
			this.shadow.enabled = true;
		}
	}

	// Token: 0x06003671 RID: 13937 RVA: 0x0002CA20 File Offset: 0x0002AC20
	public void Init(Vector2 pos)
	{
		base.transform.position = pos;
	}

	// Token: 0x04002C21 RID: 11297
	[SerializeField]
	public SpriteRenderer cuphead;

	// Token: 0x04002C22 RID: 11298
	[SerializeField]
	public SpriteRenderer mugman;

	// Token: 0x04002C23 RID: 11299
	[SerializeField]
	public SpriteRenderer chalice;

	// Token: 0x04002C24 RID: 11300
	[SerializeField]
	public SpriteRenderer shadow;
}
