using System;
using UnityEngine;

// Token: 0x0200036A RID: 874
public class SaltbakerLevelBGMint : MonoBehaviour
{
	// Token: 0x060026B3 RID: 9907 RVA: 0x000207F0 File Offset: 0x0001E9F0
	public void StartAnimation(int which)
	{
		this.anim.Play(which.ToString(), 0, Random.Range(0f, 0.5f));
	}

	// Token: 0x060026B4 RID: 9908 RVA: 0x000C98DC File Offset: 0x000C7ADC
	public void AniEvent_JumpDown()
	{
		base.transform.position += this.nextPos.position - this.startPos.position;
		if (base.transform.position.y < -1000f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04001FE8 RID: 8168
	[SerializeField]
	public Transform startPos;

	// Token: 0x04001FE9 RID: 8169
	[SerializeField]
	public Transform nextPos;

	// Token: 0x04001FEA RID: 8170
	[SerializeField]
	public Animator anim;
}
