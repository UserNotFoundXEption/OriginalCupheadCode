using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000407 RID: 1031
public class CircusPlatformingLevelDunkPlatform : AbstractCollidableObject
{
	// Token: 0x06002CFE RID: 11518 RVA: 0x00025906 File Offset: 0x00023B06
	public void Start()
	{
		this.collider2d = base.GetComponent<Collider2D>();
	}

	// Token: 0x06002CFF RID: 11519 RVA: 0x000DBB30 File Offset: 0x000D9D30
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<CircusPlatformingLevelCannonProjectile>())
		{
			this.collider2d.enabled = false;
			base.animator.SetTrigger("Hit");
			base.StartCoroutine(this.waitSpin_cr());
		}
	}

	// Token: 0x06002D00 RID: 11520 RVA: 0x00025914 File Offset: 0x00023B14
	public void Drop()
	{
		base.StartCoroutine(this.deactivate_cr());
	}

	// Token: 0x06002D01 RID: 11521 RVA: 0x000DBB80 File Offset: 0x000D9D80
	public IEnumerator deactivate_cr()
	{
		base.animator.SetTrigger("Drop");
		this.platform.enabled = false;
		yield return CupheadTime.WaitForSeconds(this, this.platformDown);
		base.animator.SetTrigger("Raise");
		yield break;
	}

	// Token: 0x06002D02 RID: 11522 RVA: 0x00025923 File Offset: 0x00023B23
	public void ActivatePlatform()
	{
		this.collider2d.enabled = true;
		this.platform.enabled = true;
	}

	// Token: 0x06002D03 RID: 11523 RVA: 0x000DBB9C File Offset: 0x000D9D9C
	public IEnumerator waitSpin_cr()
	{
		AudioManager.Play("circus_platform_plank_target");
		this.emitAudioFromObject.Add("circus_platform_plank_target");
		yield return CupheadTime.WaitForSeconds(this, this.targetSpin);
		base.animator.SetTrigger("SpinStop");
		yield break;
	}

	// Token: 0x06002D04 RID: 11524 RVA: 0x0002593D File Offset: 0x00023B3D
	public void DropSFX()
	{
		AudioManager.Play("circus_platform_plank_drop");
		this.emitAudioFromObject.Add("circus_platform_plank_drop");
	}

	// Token: 0x06002D05 RID: 11525 RVA: 0x00025959 File Offset: 0x00023B59
	public void RaiseSFX()
	{
		AudioManager.Play("circus_platform_plank_raise");
		this.emitAudioFromObject.Add("circus_platform_plank_raise");
	}

	// Token: 0x06002D06 RID: 11526 RVA: 0x00025975 File Offset: 0x00023B75
	public void PlankSFX()
	{
		AudioManager.Play("circus_platform_plank_target");
		this.emitAudioFromObject.Add("circus_platform_plank_target");
	}

	// Token: 0x0400253C RID: 9532
	public const string HitParameterName = "Hit";

	// Token: 0x0400253D RID: 9533
	public const string DropParameterName = "Drop";

	// Token: 0x0400253E RID: 9534
	public const string RaiseParameterName = "Raise";

	// Token: 0x0400253F RID: 9535
	public const string StopSpinParameterName = "SpinStop";

	// Token: 0x04002540 RID: 9536
	[SerializeField]
	public Collider2D platform;

	// Token: 0x04002541 RID: 9537
	[SerializeField]
	public float platformDown;

	// Token: 0x04002542 RID: 9538
	[SerializeField]
	public float targetSpin;

	// Token: 0x04002543 RID: 9539
	public Collider2D collider2d;
}
