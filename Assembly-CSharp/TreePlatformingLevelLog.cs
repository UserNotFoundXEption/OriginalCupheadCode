using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003F9 RID: 1017
public class TreePlatformingLevelLog : AbstractPlatformingLevelEnemy
{
	// Token: 0x17000354 RID: 852
	// (get) Token: 0x06002C8D RID: 11405 RVA: 0x00025409 File Offset: 0x00023609
	public bool CanShoot
	{
		get
		{
			return this.canShoot;
		}
	}

	// Token: 0x17000355 RID: 853
	// (get) Token: 0x06002C8E RID: 11406 RVA: 0x00025411 File Offset: 0x00023611
	public float ShootDelay
	{
		get
		{
			return this.shootDelay;
		}
	}

	// Token: 0x06002C8F RID: 11407 RVA: 0x000DA530 File Offset: 0x000D8730
	public override void Start()
	{
		base.Start();
		base._damageReceiver.enabled = false;
		this.pinkPattern = this.pinkString.Split(new char[]
		{
			','
		});
		this.pinkIndex = Random.Range(0, this.pinkPattern.Length);
	}

	// Token: 0x06002C90 RID: 11408 RVA: 0x00025419 File Offset: 0x00023619
	public override void OnStart()
	{
	}

	// Token: 0x06002C91 RID: 11409 RVA: 0x0002541B File Offset: 0x0002361B
	public void SlideDown(float belowBoundsY)
	{
		base.StartCoroutine(this.slide_cr(belowBoundsY));
	}

	// Token: 0x06002C92 RID: 11410 RVA: 0x000DA580 File Offset: 0x000D8780
	public IEnumerator slide_cr(float belowBoundsY)
	{
		this.isSliding = true;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.y > this.start - belowBoundsY)
		{
			base.transform.AddPosition(0f, -base.Properties.MoveSpeed * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		this.start = base.transform.position.y;
		this.isSliding = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002C93 RID: 11411 RVA: 0x0002542B File Offset: 0x0002362B
	public override void Die()
	{
	}

	// Token: 0x06002C94 RID: 11412 RVA: 0x0002542D File Offset: 0x0002362D
	public void KillLog()
	{
		this.SpawnPieces();
		this.isDying = true;
		base.Die();
	}

	// Token: 0x06002C95 RID: 11413 RVA: 0x000DA5A4 File Offset: 0x000D87A4
	public void SpawnPieces()
	{
		AudioManager.Play("level_platform_logface_death");
		this.emitAudioFromObject.Add("level_platform_logface_death");
		foreach (SpriteDeathParts spriteDeathParts in this.parts)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
	}

	// Token: 0x06002C96 RID: 11414 RVA: 0x00025442 File Offset: 0x00023642
	public void OnShoot()
	{
		if (this.canShoot)
		{
			base.animator.SetTrigger("OnShoot");
		}
	}

	// Token: 0x06002C97 RID: 11415 RVA: 0x000DA5FC File Offset: 0x000D87FC
	public void Shoot()
	{
		float num = base.Properties.ProjectileSpeed;
		if (this.facingRight)
		{
			num *= -1f;
		}
		this.projectile.Create(this.root.transform.position, 180f, num, !this.facingRight, this.pinkPattern[this.pinkIndex][0] == 'P');
		this.pinkIndex = (this.pinkIndex + 1) % this.pinkPattern.Length;
		Effect effect = this.projectilePuff.Create(this.root.transform.position);
		effect.GetComponent<SpriteRenderer>().flipY = this.facingRight;
	}

	// Token: 0x06002C98 RID: 11416 RVA: 0x000DA6B4 File Offset: 0x000D88B4
	public void SetDirection(bool isRight)
	{
		this.facingRight = isRight;
		if (this.facingRight)
		{
			Vector3 localScale = base.transform.localScale;
			localScale.x *= -1f;
			base.transform.localScale = localScale;
		}
	}

	// Token: 0x06002C99 RID: 11417 RVA: 0x0002545F File Offset: 0x0002365F
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.projectile = null;
		this.projectilePuff = null;
		this.parts = null;
	}

	// Token: 0x040024BA RID: 9402
	[SerializeField]
	public TreePlatformingLevelLogProjectile projectile;

	// Token: 0x040024BB RID: 9403
	[SerializeField]
	public Transform root;

	// Token: 0x040024BC RID: 9404
	[SerializeField]
	public float shootDelay;

	// Token: 0x040024BD RID: 9405
	[SerializeField]
	public SpriteDeathParts[] parts;

	// Token: 0x040024BE RID: 9406
	[SerializeField]
	public bool canShoot;

	// Token: 0x040024BF RID: 9407
	[SerializeField]
	public string pinkString;

	// Token: 0x040024C0 RID: 9408
	[SerializeField]
	public Effect projectilePuff;

	// Token: 0x040024C1 RID: 9409
	public bool facingRight;

	// Token: 0x040024C2 RID: 9410
	public string[] pinkPattern;

	// Token: 0x040024C3 RID: 9411
	public int pinkIndex;

	// Token: 0x040024C4 RID: 9412
	public bool isDying;

	// Token: 0x040024C5 RID: 9413
	public bool isSliding;

	// Token: 0x040024C6 RID: 9414
	public float start;
}
