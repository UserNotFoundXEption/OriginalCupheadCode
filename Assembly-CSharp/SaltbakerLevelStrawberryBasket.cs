using System;
using UnityEngine;

// Token: 0x02000380 RID: 896
public class SaltbakerLevelStrawberryBasket : MonoBehaviour
{
	// Token: 0x060027A5 RID: 10149 RVA: 0x000CC56C File Offset: 0x000CA76C
	public void StartRunIn(bool sbOnLeft)
	{
		base.transform.position = new Vector3((float)((!sbOnLeft) ? (Level.Current.Left - 300) : (Level.Current.Right + 300)), base.transform.position.y);
		base.transform.localScale = new Vector3((float)((!sbOnLeft) ? -1 : 1), 1f);
		this.vel = ((float)(Level.Current.Left + Level.Current.Right) / 2f + 80f * base.transform.localScale.x - base.transform.position.x) / 1.04166663f;
		this.anim.Play("RunIn");
		this.moving = true;
	}

	// Token: 0x060027A6 RID: 10150 RVA: 0x00021481 File Offset: 0x0001F681
	public void GetGrabbed()
	{
		this.moving = false;
	}

	// Token: 0x060027A7 RID: 10151 RVA: 0x000CC658 File Offset: 0x000CA858
	public void StartRunOut()
	{
		this.anim.Play("RunOut");
		this.anim.Update(0f);
		this.SFX_SALTBAKER_P1_StrawberryBag_CryingRunOff();
		this.moving = true;
		this.vel *= 0.8f;
	}

	// Token: 0x060027A8 RID: 10152 RVA: 0x000CC6A4 File Offset: 0x000CA8A4
	public void Update()
	{
		if (this.moving)
		{
			base.transform.position += this.vel * Vector3.right * CupheadTime.Delta;
			if (Mathf.Abs(base.transform.position.x) > 2000f)
			{
				this.anim.StopPlayback();
				this.moving = false;
			}
		}
	}

	// Token: 0x060027A9 RID: 10153 RVA: 0x000CC728 File Offset: 0x000CA928
	public void LateUpdate()
	{
		this.rend.enabled = (this.saltbakerTopperRend.sprite == null || this.saltbaker.animator.GetCurrentAnimatorStateInfo(0).IsName("PhaseOneToTwo"));
	}

	// Token: 0x060027AA RID: 10154 RVA: 0x0002148A File Offset: 0x0001F68A
	public void SFX_SALTBAKER_P1_StrawberryBag_CryingRunOff()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_strawberrybag_cryingrunoff");
	}

	// Token: 0x040020DB RID: 8411
	public const float GRAB_OFFSET = 80f;

	// Token: 0x040020DC RID: 8412
	[SerializeField]
	public Animator anim;

	// Token: 0x040020DD RID: 8413
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x040020DE RID: 8414
	[SerializeField]
	public SpriteRenderer saltbakerTopperRend;

	// Token: 0x040020DF RID: 8415
	[SerializeField]
	public SaltbakerLevelSaltbaker saltbaker;

	// Token: 0x040020E0 RID: 8416
	public bool moving;

	// Token: 0x040020E1 RID: 8417
	public float vel;
}
