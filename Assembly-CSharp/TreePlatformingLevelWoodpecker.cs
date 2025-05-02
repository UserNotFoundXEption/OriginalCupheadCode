using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003FD RID: 1021
public class TreePlatformingLevelWoodpecker : PlatformingLevelShootingEnemy
{
	// Token: 0x06002CBD RID: 11453 RVA: 0x000DAF40 File Offset: 0x000D9140
	public override void Start()
	{
		base.Start();
		this.isDown = false;
		this.startPos = base.transform.position;
		this.endPos = this.setEndPos.transform.position;
		this.midPos = new Vector3(this.endPos.x, this.endPos.y + 200f);
		base.GetComponent<DamageReceiver>().enabled = false;
	}

	// Token: 0x06002CBE RID: 11454 RVA: 0x00025568 File Offset: 0x00023768
	public override void Shoot()
	{
		if (!this.isDown)
		{
			base.StartCoroutine(this.move_down_cr());
		}
	}

	// Token: 0x06002CBF RID: 11455 RVA: 0x000DAFC4 File Offset: 0x000D91C4
	public IEnumerator move_down_cr()
	{
		this.isDown = true;
		base.animator.SetBool("movingDown", true);
		float t = 0f;
		Vector2 start = base.transform.position;
		while (t < base.Properties.WoodpeckermoveDownTime)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / base.Properties.WoodpeckermoveDownTime);
			base.transform.position = Vector2.Lerp(start, this.midPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = this.midPos;
		start = base.transform.position;
		yield return CupheadTime.WaitForSeconds(this, base.Properties.WoodpeckerWarningDuration);
		base.animator.SetTrigger("Continue");
		t = 0f;
		while (t < 0.2f)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / 0.5f);
			base.transform.position = Vector2.Lerp(start, this.endPos, val2);
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		base.transform.position = this.endPos;
		start = base.transform.position;
		base.animator.SetBool("isAttacking", true);
		CupheadLevelCamera.Current.Shake(10f, base.Properties.WoodpeckerAttackDuration, false);
		yield return CupheadTime.WaitForSeconds(this, base.Properties.WoodpeckerAttackDuration);
		base.animator.SetBool("isAttacking", false);
		while (t < base.Properties.WoodpeckermoveUpTime)
		{
			float val3 = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / base.Properties.WoodpeckermoveUpTime);
			base.transform.position = Vector2.Lerp(start, this.startPos, val3);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetBool("movingDown", false);
		this.isDown = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002CC0 RID: 11456 RVA: 0x00025582 File Offset: 0x00023782
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0f, 1f, 0f, 1f);
		Gizmos.DrawWireSphere(this.endPos, 100f);
	}

	// Token: 0x06002CC1 RID: 11457 RVA: 0x000255BD File Offset: 0x000237BD
	public void SoundWoodpeckerStart()
	{
		AudioManager.Play("level_platform_woodpecker_attack_start");
		this.emitAudioFromObject.Add("level_platform_woodpecker_attack_start");
	}

	// Token: 0x040024F1 RID: 9457
	[SerializeField]
	public Transform setEndPos;

	// Token: 0x040024F2 RID: 9458
	public Vector2 endPos;

	// Token: 0x040024F3 RID: 9459
	public Vector2 midPos;

	// Token: 0x040024F4 RID: 9460
	public Vector2 startPos;

	// Token: 0x040024F5 RID: 9461
	public bool isDown;
}
