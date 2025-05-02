using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003C4 RID: 964
public class TutorialPlayerDeathEffect : PlayerDeathEffect
{
	// Token: 0x06002A73 RID: 10867 RVA: 0x00023BBC File Offset: 0x00021DBC
	public override void Awake()
	{
		base.Awake();
		this.tr = base.transform;
		this.startPos = this.tr.position;
	}

	// Token: 0x06002A74 RID: 10868 RVA: 0x00023BE1 File Offset: 0x00021DE1
	public override void Start()
	{
		base.Start();
		this.Init();
	}

	// Token: 0x06002A75 RID: 10869 RVA: 0x000D3F3C File Offset: 0x000D213C
	public void Update()
	{
		if (this.tr.localPosition.y >= 270f)
		{
			this.tr.position = this.startPos;
		}
	}

	// Token: 0x06002A76 RID: 10870 RVA: 0x00023BEF File Offset: 0x00021DEF
	public override void OnParrySwitch()
	{
		base.OnParrySwitch();
		if (this.parrySwitch.enabled)
		{
			base.animator.SetTrigger("OnParryTutorial");
		}
		this.parrySwitch.enabled = false;
	}

	// Token: 0x06002A77 RID: 10871 RVA: 0x000D3F78 File Offset: 0x000D2178
	public void Init()
	{
		this.tr.position = this.startPos;
		this.playerId = PlayerId.PlayerOne;
		base.animator.SetInteger("Mode", 0);
		base.animator.SetBool("CanParry", true);
		this.spriteRenderer = this.cuphead;
		this.cuphead.gameObject.SetActive(true);
		this.mugman.gameObject.SetActive(false);
		this.parrySwitch.enabled = true;
		this.parrySwitch.gameObject.SetActive(true);
	}

	// Token: 0x06002A78 RID: 10872 RVA: 0x00023C23 File Offset: 0x00021E23
	public override void OnReviveParryAnimComplete()
	{
		this.StopAllCoroutines();
		base.animator.Play("Level_Start");
		this.exiting = false;
		this.Init();
		base.StartCoroutine(base.float_cr());
	}

	// Token: 0x06002A79 RID: 10873 RVA: 0x000D400C File Offset: 0x000D220C
	public override IEnumerator checkOutOfFrame_cr()
	{
		yield return null;
		yield break;
	}

	// Token: 0x06002A7A RID: 10874 RVA: 0x00023C55 File Offset: 0x00021E55
	public new void OnDestroy()
	{
		this.tr = null;
	}

	// Token: 0x0400236B RID: 9067
	public Vector3 startPos;

	// Token: 0x0400236C RID: 9068
	public Transform tr;
}
