using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200042E RID: 1070
public class HarbourPlatformingLevelKrill : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002E2B RID: 11819 RVA: 0x000DED58 File Offset: 0x000DCF58
	public override void Start()
	{
		base.Start();
		this.gravity = base.Properties.krillGravity;
		this.velocity.x = Random.Range(-base.Properties.krillVelocityX.min, -base.Properties.krillVelocityX.max);
		this.velocity.y = Random.Range(base.Properties.krillVelocityY.min, base.Properties.krillVelocityY.max);
		this._canParry = this.isParryable;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002E2C RID: 11820 RVA: 0x00026845 File Offset: 0x00024A45
	public override void OnStart()
	{
	}

	// Token: 0x06002E2D RID: 11821 RVA: 0x00026847 File Offset: 0x00024A47
	public void SetType(string type)
	{
		base.GetComponent<PlatformingLevelEnemyAnimationHandler>().SelectAnimation(type);
	}

	// Token: 0x06002E2E RID: 11822 RVA: 0x000DEDF8 File Offset: 0x000DCFF8
	public IEnumerator move_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.Properties.krillLaunchDelay);
		this.JumpSFX();
		for (;;)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.Delta, this.velocity.y * CupheadTime.Delta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002E2F RID: 11823 RVA: 0x00026855 File Offset: 0x00024A55
	public void JumpSFX()
	{
		AudioManager.Play("harbour_shrimp_jump");
		this.emitAudioFromObject.Add("harbour_shrimp_jump");
	}

	// Token: 0x06002E30 RID: 11824 RVA: 0x00026871 File Offset: 0x00024A71
	public override void Die()
	{
		AudioManager.Play("harbour_krill_death");
		this.emitAudioFromObject.Add("harbour_krill_death");
		base.Die();
	}

	// Token: 0x04002642 RID: 9794
	public Vector2 velocity;

	// Token: 0x04002643 RID: 9795
	public float gravity;

	// Token: 0x04002644 RID: 9796
	public bool isParryable;
}
