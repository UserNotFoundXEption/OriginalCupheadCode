using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000421 RID: 1057
public class FunhousePlatformingLevelStarCannon : PlatformingLevelPathMovementEnemy
{
	// Token: 0x06002DC9 RID: 11721 RVA: 0x000DDA74 File Offset: 0x000DBC74
	public override void Start()
	{
		base.Start();
		if (this.killable)
		{
			base._damageReceiver.enabled = false;
		}
		base.animator.SetBool("IsA", Rand.Bool());
		base.StartCoroutine(this.check_to_start_cr());
	}

	// Token: 0x06002DCA RID: 11722 RVA: 0x000262EF File Offset: 0x000244EF
	public override void OnStart()
	{
		base.OnStart();
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x06002DCB RID: 11723 RVA: 0x000DDAC0 File Offset: 0x000DBCC0
	public IEnumerator check_to_start_cr()
	{
		while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset)
		{
			yield return null;
		}
		this.OnStart();
		yield return null;
		yield break;
	}

	// Token: 0x06002DCC RID: 11724 RVA: 0x000DDADC File Offset: 0x000DBCDC
	public IEnumerator shoot_cr()
	{
		for (;;)
		{
			while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset || base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - this.offset)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, base.Properties.cannonShotDelay);
			base.animator.SetBool("isShooting", true);
			while (!this.justShot)
			{
				yield return null;
			}
			this.justShot = false;
			yield return CupheadTime.WaitForSeconds(this, 0.7f);
			base.animator.SetBool("isShooting", false);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002DCD RID: 11725 RVA: 0x000DDAF8 File Offset: 0x000DBCF8
	public void ShootStraight()
	{
		this.justShot = true;
		AudioManager.Play("funhouse_starcannon_shoot");
		this.emitAudioFromObject.Add("funhouse_starcannon_shoot");
		this.StraightFX();
		for (int i = 0; i < this.straightRootPositions.Length; i++)
		{
			FunhousePlatformingLevelCannonProjectile funhousePlatformingLevelCannonProjectile = this.projectile.Create(this.straightRootPositions[i].transform.position, 0f, base.Properties.cannonSpeed) as FunhousePlatformingLevelCannonProjectile;
			funhousePlatformingLevelCannonProjectile.direction = this.straightRootPositions[i].transform.rotation * Vector3.right;
			funhousePlatformingLevelCannonProjectile.Properties = base.Properties;
			funhousePlatformingLevelCannonProjectile.Init();
		}
	}

	// Token: 0x06002DCE RID: 11726 RVA: 0x000DDBB4 File Offset: 0x000DBDB4
	public void ShootDiag()
	{
		this.justShot = true;
		AudioManager.Play("funhouse_starcannon_shoot");
		this.emitAudioFromObject.Add("funhouse_starcannon_shoot");
		this.DiagFX();
		for (int i = 0; i < this.diagRootPositions.Length; i++)
		{
			FunhousePlatformingLevelCannonProjectile funhousePlatformingLevelCannonProjectile = this.projectile.Create(this.diagRootPositions[i].transform.position, 0f, base.Properties.cannonSpeed) as FunhousePlatformingLevelCannonProjectile;
			funhousePlatformingLevelCannonProjectile.direction = this.diagRootPositions[i].transform.rotation * Vector3.right;
			funhousePlatformingLevelCannonProjectile.Properties = base.Properties;
			funhousePlatformingLevelCannonProjectile.Init();
		}
	}

	// Token: 0x06002DCF RID: 11727 RVA: 0x00026304 File Offset: 0x00024504
	public void DiagFX()
	{
		this.diagFX.Create(base.transform.position);
	}

	// Token: 0x06002DD0 RID: 11728 RVA: 0x0002631D File Offset: 0x0002451D
	public void StraightFX()
	{
		this.straightFX.Create(base.transform.position);
	}

	// Token: 0x06002DD1 RID: 11729 RVA: 0x00026336 File Offset: 0x00024536
	public void SoundCannonRotate()
	{
		AudioManager.Play("funhouse_starcannon_rotation");
		this.emitAudioFromObject.Add("funhouse_starcannon_rotation");
	}

	// Token: 0x06002DD2 RID: 11730 RVA: 0x00026352 File Offset: 0x00024552
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.projectile = null;
		this.diagFX = null;
		this.straightFX = null;
	}

	// Token: 0x040025F2 RID: 9714
	[SerializeField]
	public Effect diagFX;

	// Token: 0x040025F3 RID: 9715
	[SerializeField]
	public Effect straightFX;

	// Token: 0x040025F4 RID: 9716
	[SerializeField]
	public bool killable;

	// Token: 0x040025F5 RID: 9717
	[SerializeField]
	public Transform[] diagRootPositions;

	// Token: 0x040025F6 RID: 9718
	[SerializeField]
	public Transform[] straightRootPositions;

	// Token: 0x040025F7 RID: 9719
	[SerializeField]
	public FunhousePlatformingLevelCannonProjectile projectile;

	// Token: 0x040025F8 RID: 9720
	public float offset = 50f;

	// Token: 0x040025F9 RID: 9721
	public bool justShot;
}
