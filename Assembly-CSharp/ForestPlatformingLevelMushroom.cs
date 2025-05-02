using System;

// Token: 0x020003EC RID: 1004
public class ForestPlatformingLevelMushroom : PlatformingLevelShootingEnemy
{
	// Token: 0x06002C2A RID: 11306 RVA: 0x00025030 File Offset: 0x00023230
	public override void Awake()
	{
		base.Awake();
		ForestPlatformingLevelMushroomProjectile.numUntilPink = base.Properties.MushroomPinkNumber.RandomInt();
	}

	// Token: 0x06002C2B RID: 11307 RVA: 0x0002504D File Offset: 0x0002324D
	public override void Shoot()
	{
		base.Shoot();
	}

	// Token: 0x06002C2C RID: 11308 RVA: 0x000D93BC File Offset: 0x000D75BC
	public void EmergeFromGround()
	{
		base.setDirection((this._target.center.x <= base.transform.position.x) ? PlatformingLevelShootingEnemy.Direction.Left : PlatformingLevelShootingEnemy.Direction.Right);
	}

	// Token: 0x06002C2D RID: 11309 RVA: 0x00025055 File Offset: 0x00023255
	public void PlayMushroomSound()
	{
		AudioManager.Play("level_mushroom_shoot");
		this.emitAudioFromObject.Add("level_mushroom_shoot");
	}

	// Token: 0x06002C2E RID: 11310 RVA: 0x00025071 File Offset: 0x00023271
	public override void Die()
	{
		AudioManager.Play("level_mermaid_turtle_shell_pop");
		this.emitAudioFromObject.Add("level_mermaid_turtle_shell_pop");
		base.Die();
	}
}
