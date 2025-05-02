using System;
using UnityEngine;

// Token: 0x0200045C RID: 1116
public class PlatformingLevelEnemyAnimationHandler : AbstractPausableComponent
{
	// Token: 0x06002FA7 RID: 12199 RVA: 0x000E255C File Offset: 0x000E075C
	public void SelectAnimation(string type1)
	{
		for (int i = 0; i < this.numOfTypes; i++)
		{
			if (type1.Substring(0, 1) == "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(new char[]
			{
				','
			})[i])
			{
				this.index1 = i;
			}
			if (this.secondaryTypes > 0 && type1.Substring(1, 1) == "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(new char[]
			{
				','
			})[i])
			{
				this.index2 = i + 1;
			}
		}
		foreach (SpriteRenderer spriteRenderer in base.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.enabled = false;
		}
		base.GetComponentsInChildren<SpriteRenderer>()[this.index1].enabled = true;
		if (this.secondaryTypes > 0)
		{
			base.GetComponent<Animator>().SetInteger("type", this.index2);
		}
	}

	// Token: 0x0400277C RID: 10108
	[SerializeField]
	public int numOfTypes;

	// Token: 0x0400277D RID: 10109
	[SerializeField]
	public int secondaryTypes;

	// Token: 0x0400277E RID: 10110
	public int index1;

	// Token: 0x0400277F RID: 10111
	public int index2;

	// Token: 0x04002780 RID: 10112
	public const string LETTERS = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
}
