using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000424 RID: 1060
public class FunhousePlatformingLevelWall : PlatformingLevelBigEnemy
{
	// Token: 0x17000360 RID: 864
	// (get) Token: 0x06002DE6 RID: 11750 RVA: 0x00026479 File Offset: 0x00024679
	public bool IsDead
	{
		get
		{
			return this.isDead;
		}
	}

	// Token: 0x06002DE7 RID: 11751 RVA: 0x00026481 File Offset: 0x00024681
	public override void OnLock()
	{
		base.OnLock();
		base.StartCoroutine(this.slide_camera_cr());
	}

	// Token: 0x06002DE8 RID: 11752 RVA: 0x000DDDFC File Offset: 0x000DBFFC
	public IEnumerator slide_camera_cr()
	{
		base.GetComponent<Collider2D>().enabled = true;
		CupheadLevelCamera.Current.SetAutoScroll(true);
		CupheadLevelCamera.Current.LockCamera(false);
		float dist = CupheadLevelCamera.Current.transform.position.x - base.transform.position.x;
		while (dist < -500f)
		{
			dist = CupheadLevelCamera.Current.transform.position.x - base.transform.position.x;
			yield return null;
		}
		CupheadLevelCamera.Current.SetAutoScroll(false);
		CupheadLevelCamera.Current.LockCamera(true);
		yield return null;
		yield break;
	}

	// Token: 0x06002DE9 RID: 11753 RVA: 0x000DDE18 File Offset: 0x000DC018
	public override void Start()
	{
		base.Start();
		this.LockDistance = 800f;
		base.StartCoroutine(this.shoot_projectiles_cr(base.Properties.funWallTopDelayRange, true));
		base.StartCoroutine(this.shoot_projectiles_cr(base.Properties.funWallBottomDelayRange, false));
		if (this.isTongue)
		{
			base.StartCoroutine(this.spawn_tongue_cr());
		}
		else
		{
			base.StartCoroutine(this.spawn_cars_cr());
		}
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06002DEA RID: 11754 RVA: 0x00026496 File Offset: 0x00024696
	public override void Shoot()
	{
		if (this.isDead)
		{
			return;
		}
	}

	// Token: 0x06002DEB RID: 11755 RVA: 0x000DDEA0 File Offset: 0x000DC0A0
	public override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.isDead)
		{
			return;
		}
		base.OnDamageTaken(info);
		base.animator.SetTrigger("eyeHit");
		if (!AudioManager.CheckIfPlaying("funhouse_wall1_eye_hit"))
		{
			AudioManager.Play("funhouse_wall1_eye_hit");
			this.emitAudioFromObject.Add("funhouse_wall1_eye_hit");
		}
	}

	// Token: 0x06002DEC RID: 11756 RVA: 0x000DDEFC File Offset: 0x000DC0FC
	public IEnumerator shoot_projectiles_cr(MinMax delay, bool isTop)
	{
		while (!this.bigEnemyCameraLock)
		{
			yield return null;
		}
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, delay.RandomFloat());
			string name = (!isTop) ? "Bottom" : "Top";
			base.animator.SetTrigger("horn" + name);
			AudioManager.Play("funhouse_wall1_horn_attack");
			this.emitAudioFromObject.Add("funhouse_wall1_horn_attack");
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002DED RID: 11757 RVA: 0x000DDF28 File Offset: 0x000DC128
	public void ShootProjectileTop()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 vector = next.transform.position - this.topProjectileRoot.transform.position;
		this.hornEffect.Create(this.topProjectileRoot.transform.position);
		this.shootProjectile.Create(this.topProjectileRoot.transform.position, MathUtils.DirectionToAngle(vector), base.Properties.funWallProjectileSpeed);
	}

	// Token: 0x06002DEE RID: 11758 RVA: 0x000DDFB0 File Offset: 0x000DC1B0
	public void ShootProjectileBottom()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 vector = next.transform.position - this.bottomProjectileRoot.transform.position;
		this.hornEffect.Create(this.bottomProjectileRoot.transform.position);
		this.shootProjectile.Create(this.bottomProjectileRoot.transform.position, MathUtils.DirectionToAngle(vector), base.Properties.funWallProjectileSpeed);
	}

	// Token: 0x06002DEF RID: 11759 RVA: 0x000DE038 File Offset: 0x000DC238
	public IEnumerator spawn_cars_cr()
	{
		while (!this.bigEnemyCameraLock)
		{
			yield return null;
		}
		int typeIndex = 0;
		bool isTop = Rand.Bool();
		Vector3 pos = (!isTop) ? this.bottomTransform.position : this.topTransform.position;
		for (;;)
		{
			GameObject blockage = (!isTop) ? this.mouthBlockageBottom : this.mouthBlockageTop;
			base.animator.SetBool("isTop", isTop);
			yield return CupheadTime.WaitForSeconds(this, base.Properties.funWallCarDelayRange.RandomFloat());
			base.animator.SetBool("isOpen", true);
			string name = (!isTop) ? "Bottom" : "Top";
			yield return base.animator.WaitForAnimationToStart(this, name + "_Open_Start", false);
			AudioManager.Play("funhouse_wall1_wall_open_start");
			this.emitAudioFromObject.Add("funhouse_wall1_wall_open_start");
			AudioManager.Play("funhouse_car_honk_sweet");
			this.SpawnHonk((!isTop) ? this.bottomTransform.position.y : this.topTransform.position.y);
			yield return base.animator.WaitForAnimationToEnd(this, name + "_Open_Start", false, true);
			blockage.SetActive(false);
			for (int i = 0; i < 2; i++)
			{
				FunhousePlatformingLevelCar car = Object.Instantiate<FunhousePlatformingLevelCar>(this.carPrefab);
				car.Init(pos, 180f, base.Properties.funWallCarSpeed, typeIndex, true, true);
				car.transform.SetScale(null, new float?(isTop ? car.transform.localScale.y : (-car.transform.localScale.y)), null);
				typeIndex = ((typeIndex >= 3) ? 0 : (typeIndex + 1));
				yield return CupheadTime.WaitForSeconds(this, this.carDelay);
			}
			yield return CupheadTime.WaitForSeconds(this, base.Properties.funWallMouthOpenTime);
			base.animator.SetBool("isOpen", false);
			AudioManager.Play("funhouse_wall1_wall_close");
			this.emitAudioFromObject.Add("funhouse_wall1_wall_close");
			blockage.SetActive(true);
			isTop = !isTop;
			pos = ((!isTop) ? this.bottomTransform.position : this.topTransform.position);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002DF0 RID: 11760 RVA: 0x000DE054 File Offset: 0x000DC254
	public void SpawnHonk(float rootY)
	{
		Vector2 vector;
		vector..ctor(CupheadLevelCamera.Current.Bounds.xMax, rootY);
		this.honkEffect.Create(vector).transform.parent = CupheadLevelCamera.Current.transform;
	}

	// Token: 0x06002DF1 RID: 11761 RVA: 0x000DE0A0 File Offset: 0x000DC2A0
	public IEnumerator spawn_tongue_cr()
	{
		while (!this.bigEnemyCameraLock)
		{
			yield return null;
		}
		bool isTop = Rand.Bool();
		Vector3 pos = (!isTop) ? this.bottomTransform.position : this.topTransform.position;
		for (;;)
		{
			GameObject blockage = (!(pos == this.bottomTransform.position)) ? this.mouthBlockageTop : this.mouthBlockageBottom;
			base.animator.SetBool("isTop", isTop);
			yield return CupheadTime.WaitForSeconds(this, base.Properties.funWallTongueDelayRange.RandomFloat());
			base.animator.SetBool("isOpen", true);
			string name = (!isTop) ? "Bottom" : "Top";
			yield return CupheadTime.WaitForSeconds(this, 0.8f);
			base.animator.SetTrigger("Continue");
			yield return base.animator.WaitForAnimationToEnd(this, name + "_Open_Start", false, true);
			AudioManager.Play("funhouse_wall1_wall_open_start");
			this.emitAudioFromObject.Add("funhouse_wall1_wall_open_start");
			blockage.SetActive(false);
			this.tongue.transform.SetScale(null, new float?((float)((!isTop) ? 1 : -1)), null);
			this.tongue.transform.position = pos;
			this.tongue.GetComponent<Animator>().SetBool("IsTongue", true);
			AudioManager.Play("funhouse_funwall_tounge_intro");
			this.emitAudioFromObject.Add("funhouse_funwall_tounge_intro");
			yield return CupheadTime.WaitForSeconds(this, base.Properties.funWallTongueLoopTime);
			this.tongue.GetComponent<Animator>().SetBool("IsTongue", false);
			AudioManager.Play("funhouse_funwall_tounge_outro");
			this.emitAudioFromObject.Add("funhouse_funwall_tounge_outro");
			yield return this.tongue.GetComponent<Animator>().WaitForAnimationToEnd(this, "Outro", false, true);
			yield return CupheadTime.WaitForSeconds(this, base.Properties.funWallMouthOpenTime);
			base.animator.SetBool("isOpen", false);
			AudioManager.Play("funhouse_wall1_wall_close");
			this.emitAudioFromObject.Add("funhouse_wall1_wall_close");
			blockage.SetActive(true);
			isTop = !isTop;
			pos = ((!isTop) ? this.bottomTransform.position : this.topTransform.position);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002DF2 RID: 11762 RVA: 0x000264A4 File Offset: 0x000246A4
	public override void OnPass()
	{
		base.OnPass();
		this.StopAllCoroutines();
		this.Die();
	}

	// Token: 0x06002DF3 RID: 11763 RVA: 0x000DE0BC File Offset: 0x000DC2BC
	public override void Die()
	{
		this.isDead = true;
		base.GetComponent<Collider2D>().enabled = false;
		this.deadBlockage.SetActive(true);
		if (CupheadLevelCamera.Current.autoScrolling)
		{
			CupheadLevelCamera.Current.SetAutoScroll(false);
		}
		CupheadLevelCamera.Current.LockCamera(false);
		this.mouthBlockageBottom.SetActive(false);
		this.mouthBlockageTop.SetActive(false);
		this.middleBlockage.SetActive(false);
		if (this.tongue != null)
		{
			this.tongue.gameObject.SetActive(false);
		}
		this.StopAllCoroutines();
		base.StartCoroutine(this.explode_cr());
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("Dead");
		AudioManager.Play("funhouse_wall_death");
		this.emitAudioFromObject.Add("funhouse_wall_death");
	}

	// Token: 0x06002DF4 RID: 11764 RVA: 0x000DE19C File Offset: 0x000DC39C
	public IEnumerator explode_cr()
	{
		this.explosion.StartExplosion();
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.explosion.StopExplosions();
		yield break;
	}

	// Token: 0x06002DF5 RID: 11765 RVA: 0x000264B8 File Offset: 0x000246B8
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.carPrefab = null;
		this.shootProjectile = null;
		this.hornEffect = null;
		this.honkEffect = null;
	}

	// Token: 0x04002602 RID: 9730
	[SerializeField]
	public Effect hornEffect;

	// Token: 0x04002603 RID: 9731
	[SerializeField]
	public Effect honkEffect;

	// Token: 0x04002604 RID: 9732
	[SerializeField]
	public bool isTongue;

	// Token: 0x04002605 RID: 9733
	[SerializeField]
	public FunhousePlatformingLevelCar carPrefab;

	// Token: 0x04002606 RID: 9734
	[SerializeField]
	public BasicProjectile shootProjectile;

	// Token: 0x04002607 RID: 9735
	[SerializeField]
	public GameObject mouthBlockageTop;

	// Token: 0x04002608 RID: 9736
	[SerializeField]
	public GameObject mouthBlockageBottom;

	// Token: 0x04002609 RID: 9737
	[SerializeField]
	public GameObject middleBlockage;

	// Token: 0x0400260A RID: 9738
	[SerializeField]
	public GameObject deadBlockage;

	// Token: 0x0400260B RID: 9739
	[SerializeField]
	public Transform tongue;

	// Token: 0x0400260C RID: 9740
	[SerializeField]
	public Transform topTransform;

	// Token: 0x0400260D RID: 9741
	[SerializeField]
	public Transform bottomTransform;

	// Token: 0x0400260E RID: 9742
	[SerializeField]
	public Transform topProjectileRoot;

	// Token: 0x0400260F RID: 9743
	[SerializeField]
	public Transform bottomProjectileRoot;

	// Token: 0x04002610 RID: 9744
	[SerializeField]
	public LevelBossDeathExploder explosion;

	// Token: 0x04002611 RID: 9745
	public float carDelay = 0.7f;
}
