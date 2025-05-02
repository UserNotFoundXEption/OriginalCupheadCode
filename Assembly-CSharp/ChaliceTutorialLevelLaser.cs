using System;
using UnityEngine;

// Token: 0x02000170 RID: 368
public class ChaliceTutorialLevelLaser : AbstractCollidableObject
{
	// Token: 0x060011B0 RID: 4528 RVA: 0x00092D58 File Offset: 0x00090F58
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.level.resetParryables = true;
		AudioManager.Play("sfx_rip_fail");
		this.hitAnimator.transform.position = new Vector3(base.transform.position.x + this.coll.bounds.size.x / 2f, hit.transform.position.y + 100f);
		this.hitAnimator.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		this.hitAnimator.Play("Hit");
	}

	// Token: 0x060011B1 RID: 4529 RVA: 0x0000F015 File Offset: 0x0000D215
	public void Enabled()
	{
		base.animator.SetBool("On", true);
		this.coll.enabled = true;
	}

	// Token: 0x060011B2 RID: 4530 RVA: 0x0000F034 File Offset: 0x0000D234
	public void Disabled()
	{
		base.animator.SetBool("On", false);
		this.coll.enabled = false;
	}

	// Token: 0x060011B3 RID: 4531 RVA: 0x0000F053 File Offset: 0x0000D253
	public void Update()
	{
		if (!this.parryable.isDeactivated)
		{
			this.Enabled();
		}
		else
		{
			this.Disabled();
		}
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x0000F076 File Offset: 0x0000D276
	public void AniEvent_SFX_Open()
	{
		AudioManager.Play("sfx_rip_open");
	}

	// Token: 0x04000E33 RID: 3635
	[SerializeField]
	public ChaliceTutorialLevel level;

	// Token: 0x04000E34 RID: 3636
	[SerializeField]
	public ChaliceTutorialLevelParryable parryable;

	// Token: 0x04000E35 RID: 3637
	[SerializeField]
	public Collider2D coll;

	// Token: 0x04000E36 RID: 3638
	[SerializeField]
	public Animator hitAnimator;
}
