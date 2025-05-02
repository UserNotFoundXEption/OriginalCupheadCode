using System;
using UnityEngine;

// Token: 0x0200036B RID: 875
public class SaltbakerLevelBGSaltHands : MonoBehaviour
{
	// Token: 0x060026B6 RID: 9910 RVA: 0x000C9944 File Offset: 0x000C7B44
	public void Play()
	{
		base.transform.position = this.positions[this.positionCounter];
		base.transform.localScale = new Vector3((float)((this.positionCounter != 0) ? -1 : 1), (this.positionCounter != 0) ? 1.02f : 1f);
		for (int i = 0; i < this.rends.Length; i++)
		{
			this.rends[i].sortingOrder = ((this.positionCounter != 0) ? 650 : 850) + i * 5;
		}
		this.anim.Play("SaltHands");
		this.SFX_SALTB_Bouncer_MakeBouncer();
		this.positionCounter = 1 - this.positionCounter;
	}

	// Token: 0x060026B7 RID: 9911 RVA: 0x00020822 File Offset: 0x0001EA22
	public void SFX_SALTB_Bouncer_MakeBouncer()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p3_hands_makebouncer");
	}

	// Token: 0x04001FEB RID: 8171
	[SerializeField]
	public Vector2[] positions;

	// Token: 0x04001FEC RID: 8172
	[SerializeField]
	public Animator anim;

	// Token: 0x04001FED RID: 8173
	[SerializeField]
	public SpriteRenderer[] rends;

	// Token: 0x04001FEE RID: 8174
	public int positionCounter;
}
