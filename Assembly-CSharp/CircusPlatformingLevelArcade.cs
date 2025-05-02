using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003FE RID: 1022
public class CircusPlatformingLevelArcade : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002CC3 RID: 11459 RVA: 0x000255EC File Offset: 0x000237EC
	public override void OnStart()
	{
		base.StartCoroutine(this.shoot_cr());
		this.goingRight = Rand.Bool();
	}

	// Token: 0x06002CC4 RID: 11460 RVA: 0x00025606 File Offset: 0x00023806
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.check_to_start_cr());
	}

	// Token: 0x06002CC5 RID: 11461 RVA: 0x000DAFE0 File Offset: 0x000D91E0
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

	// Token: 0x06002CC6 RID: 11462 RVA: 0x000DAFFC File Offset: 0x000D91FC
	public IEnumerator shoot_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.Properties.arcadeAttackDelayInit.RandomFloat());
		for (;;)
		{
			while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset || base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - this.offset)
			{
				yield return null;
			}
			base.animator.SetBool("IsAttacking", true);
			this.isAttacking = true;
			while (this.isAttacking)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, base.Properties.arcadeAttackDelay.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002CC7 RID: 11463 RVA: 0x000DB018 File Offset: 0x000D9218
	public void Shoot()
	{
		this.goingRight = !this.goingRight;
		this.introBulletInstance = Object.Instantiate<Transform>(this.introBullet);
		base.StartCoroutine(this.shoot_intro_cr());
		base.animator.SetBool("IsAttacking", false);
		base.StartCoroutine(this.drop_cr());
	}

	// Token: 0x06002CC8 RID: 11464 RVA: 0x000DB070 File Offset: 0x000D9270
	public IEnumerator shoot_intro_cr()
	{
		while (this.introBulletInstance.position.y < (float)Level.Current.Ceiling + 100f)
		{
			this.introBulletInstance.position += Vector3.up * base.Properties.arcadeBulletSpeed * CupheadTime.Delta;
			yield return null;
		}
		Object.Destroy(this.introBulletInstance.gameObject);
		yield break;
	}

	// Token: 0x06002CC9 RID: 11465 RVA: 0x000DB08C File Offset: 0x000D928C
	public IEnumerator drop_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.Properties.arcadeBulletReturnDelay);
		AbstractPlayerController player = PlayerManager.GetNext();
		float sizeX = 100f;
		float posX = (!this.goingRight) ? this.bulletSpawnB.transform.position.x : this.bulletSpawnA.transform.position.x;
		for (int i = 0; i < base.Properties.arcadeBulletCount; i++)
		{
			if (player == null)
			{
				player = PlayerManager.GetNext();
			}
			yield return null;
			this.bullet.Create(new Vector2((!this.goingRight) ? (posX - sizeX * (float)i) : (posX + sizeX * (float)i), CupheadLevelCamera.Current.Bounds.yMax + 50f), -90f, base.Properties.arcadeBulletSpeed);
			yield return CupheadTime.WaitForSeconds(this, base.Properties.arcadeBulletIndividualDelay);
		}
		this.isAttacking = false;
		yield break;
	}

	// Token: 0x06002CCA RID: 11466 RVA: 0x000DB0A8 File Offset: 0x000D92A8
	public override void Die()
	{
		AudioManager.Play("circus_arcade_death");
		this.emitAudioFromObject.Add("circus_arcade_death");
		base.animator.Play("Death");
		this.effect.Create(base.transform.position);
		this.StopAllCoroutines();
		if (this.introBulletInstance != null)
		{
			Object.Destroy(this.introBulletInstance.gameObject);
		}
		base.StartCoroutine(this.Explosion_cr());
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06002CCB RID: 11467 RVA: 0x000DB138 File Offset: 0x000D9338
	public IEnumerator Explosion_cr()
	{
		this.exploder.StartExplosion();
		yield return new WaitForSeconds(2.5f);
		this.exploder.StopExplosions();
		yield break;
	}

	// Token: 0x06002CCC RID: 11468 RVA: 0x0002561B File Offset: 0x0002381B
	public void AttackSFX()
	{
		AudioManager.Play("circus_arcade_attack");
		this.emitAudioFromObject.Add("circus_arcade_attack");
	}

	// Token: 0x06002CCD RID: 11469 RVA: 0x00025637 File Offset: 0x00023837
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawWireSphere(this.bulletSpawnA.position, 50f);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(this.bulletSpawnB.position, 50f);
	}

	// Token: 0x06002CCE RID: 11470 RVA: 0x00025673 File Offset: 0x00023873
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.effect = null;
		this.bullet = null;
		this.introBullet = null;
		this.introBulletInstance = null;
	}

	// Token: 0x040024F6 RID: 9462
	[SerializeField]
	public Transform bulletSpawnA;

	// Token: 0x040024F7 RID: 9463
	[SerializeField]
	public Transform bulletSpawnB;

	// Token: 0x040024F8 RID: 9464
	[SerializeField]
	public Effect effect;

	// Token: 0x040024F9 RID: 9465
	[SerializeField]
	public Transform arcadeRoot;

	// Token: 0x040024FA RID: 9466
	[SerializeField]
	public Transform introBullet;

	// Token: 0x040024FB RID: 9467
	[SerializeField]
	public BasicProjectile bullet;

	// Token: 0x040024FC RID: 9468
	[SerializeField]
	public LevelBossDeathExploder exploder;

	// Token: 0x040024FD RID: 9469
	public float offset = 50f;

	// Token: 0x040024FE RID: 9470
	public bool isAttacking;

	// Token: 0x040024FF RID: 9471
	public bool goingRight;

	// Token: 0x04002500 RID: 9472
	public Transform introBulletInstance;
}
