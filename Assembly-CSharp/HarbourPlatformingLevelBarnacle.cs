using System;

// Token: 0x02000425 RID: 1061
public class HarbourPlatformingLevelBarnacle : PlatformingLevelShootingEnemy
{
	// Token: 0x06002DF7 RID: 11767 RVA: 0x000264E4 File Offset: 0x000246E4
	public void AttackSFX()
	{
		AudioManager.Play("harbour_barnacle_attack");
		this.emitAudioFromObject.Add("harbour_barnacle_attack");
	}

	// Token: 0x06002DF8 RID: 11768 RVA: 0x00026500 File Offset: 0x00024700
	public override void Die()
	{
		AudioManager.Play("harbour_barnacle_death");
		this.emitAudioFromObject.Add("harbour_barnacle_death");
		base.Die();
	}
}
