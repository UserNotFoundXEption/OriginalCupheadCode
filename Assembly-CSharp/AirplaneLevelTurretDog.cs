using System;
using UnityEngine;

// Token: 0x02000137 RID: 311
public class AirplaneLevelTurretDog : AbstractPausableComponent
{
	// Token: 0x06000EBE RID: 3774 RVA: 0x0008C258 File Offset: 0x0008A458
	public void ShootProjectile()
	{
		this.FX.Create(base.transform.position, base.transform.localScale);
		Vector3 vector;
		vector..ctor(this.rootPos.position.x + 30f, this.rootPos.position.y);
		Vector3 vector2;
		vector2..ctor(this.rootPos.position.x - 30f, this.rootPos.position.y);
		AirplaneLevelTurretBullet airplaneLevelTurretBullet = this.bulletPrefab.Create(vector, new Vector3(this.velocityX, this.velocityY), this.gravity);
		airplaneLevelTurretBullet.GetComponent<SpriteRenderer>().sortingOrder = 1;
		airplaneLevelTurretBullet.GetComponent<Animator>().Play("TennisBallA");
		AirplaneLevelTurretBullet airplaneLevelTurretBullet2 = this.bulletPrefab.Create(this.rootPos.position, new Vector3(0f, this.velocityY), this.gravity);
		airplaneLevelTurretBullet2.GetComponent<SpriteRenderer>().sortingOrder = 2;
		airplaneLevelTurretBullet2.GetComponent<Animator>().Play("TennisBallB");
		AirplaneLevelTurretBullet airplaneLevelTurretBullet3 = this.bulletPrefab.Create(vector2, new Vector3(-this.velocityX, this.velocityY), this.gravity);
		airplaneLevelTurretBullet3.GetComponent<SpriteRenderer>().sortingOrder = 1;
		airplaneLevelTurretBullet3.GetComponent<Animator>().Play("TennisBallC");
	}

	// Token: 0x06000EBF RID: 3775 RVA: 0x0000C7E0 File Offset: 0x0000A9E0
	public void StartAttack(float velocityX, float velocityY, float gravity)
	{
		base.animator.Play("Flap");
		this.velocityX = velocityX;
		this.velocityY = velocityY;
		this.gravity = gravity;
	}

	// Token: 0x06000EC0 RID: 3776 RVA: 0x0000C807 File Offset: 0x0000AA07
	public void AnimationEvent_SFX_DOGFIGHT_BulldogPlane_TurretDogHatchOpen()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_terrierplane_hatchopen");
	}

	// Token: 0x06000EC1 RID: 3777 RVA: 0x0000C813 File Offset: 0x0000AA13
	public void AnimationEvent_SFX_DOGFIGHT_BulldogPlane_TurretDogBark()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_terrierplane_bark");
	}

	// Token: 0x06000EC2 RID: 3778 RVA: 0x0000C81F File Offset: 0x0000AA1F
	public void AnimationEvent_SFX_DOGFIGHT_BulldogPlane_TurretDogWhistle()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_terrierplane_baseball_whistle");
	}

	// Token: 0x06000EC3 RID: 3779 RVA: 0x0000C82B File Offset: 0x0000AA2B
	public void AnimationEvent_SFX_DOGFIGHT_BulldogPlane_TurretDogToss()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_terrierplane_baseball_toss");
	}

	// Token: 0x04000C16 RID: 3094
	public const float BALL_OFFSET = 30f;

	// Token: 0x04000C17 RID: 3095
	[SerializeField]
	public AirplaneLevelTurretBullet bulletPrefab;

	// Token: 0x04000C18 RID: 3096
	[SerializeField]
	public Transform rootPos;

	// Token: 0x04000C19 RID: 3097
	[SerializeField]
	public Effect FX;

	// Token: 0x04000C1A RID: 3098
	public float velocityX;

	// Token: 0x04000C1B RID: 3099
	public float velocityY;

	// Token: 0x04000C1C RID: 3100
	public float gravity;
}
