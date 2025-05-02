using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000251 RID: 593
public class FlyingCowboyLevelBird : AbstractProjectile
{
	// Token: 0x06001B05 RID: 6917 RVA: 0x00016EE2 File Offset: 0x000150E2
	public void Initialize(Vector3 startPosition, Vector3 endPosition, float bulletLandingPosition, LevelProperties.FlyingCowboy.Bird properties, FlyingCowboyLevelCowboy cowgirl)
	{
		this.bulletLandingPosition = bulletLandingPosition;
		this.properties = properties;
		this.cowgirl = cowgirl;
		base.StartCoroutine(this.move_cr(startPosition, endPosition, properties));
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06001B06 RID: 6918 RVA: 0x000AA3DC File Offset: 0x000A85DC
	public void InitializeIntro(Vector3 startPosition)
	{
		base.transform.position = startPosition;
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		component.sortingLayerName = "Default";
		component.sortingOrder = -120;
		base.animator.Play("Return");
	}

	// Token: 0x06001B07 RID: 6919 RVA: 0x00016F19 File Offset: 0x00015119
	public void MoveIntro(Vector3 endPosition, LevelProperties.FlyingCowboy.Bird properties)
	{
		base.StartCoroutine(this.moveIntro_cr(endPosition, properties));
	}

	// Token: 0x06001B08 RID: 6920 RVA: 0x00016F2A File Offset: 0x0001512A
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001B09 RID: 6921 RVA: 0x000AA420 File Offset: 0x000A8620
	public void move()
	{
		Vector3 position = base.transform.position;
		position.x += this.properties.speed * CupheadTime.FixedDelta;
		base.transform.position = position;
	}

	// Token: 0x06001B0A RID: 6922 RVA: 0x000AA464 File Offset: 0x000A8664
	public IEnumerator move_cr(Vector3 startPosition, Vector3 endPosition, LevelProperties.FlyingCowboy.Bird properties)
	{
		WaitForFixedUpdate wait = new WaitForFixedUpdate();
		while (base.transform.position.x < endPosition.x - 60f)
		{
			yield return wait;
			this.move();
		}
		while (base.animator.GetCurrentAnimatorStateInfo(1).IsName("Throw"))
		{
			yield return wait;
			this.move();
		}
		float normalizedTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		while (normalizedTime >= 0.181818187f && normalizedTime <= 0.8181818f)
		{
			yield return wait;
			this.move();
			normalizedTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		}
		base.animator.Play("Turn");
		float slowdownTime = KinematicUtilities.CalculateTimeToChangeVelocity(properties.speed, 0f, 60f);
		float elapsedTime = 0f;
		while (elapsedTime < slowdownTime)
		{
			yield return wait;
			elapsedTime += CupheadTime.FixedDelta;
			Vector3 position = base.transform.position;
			position.x += Mathf.Lerp(properties.speed, 0f, elapsedTime / slowdownTime) * CupheadTime.FixedDelta;
			base.transform.position = position;
		}
		yield return base.animator.WaitForNormalizedTime(this, 0.75f, "Turn", 0, false, false, true);
		elapsedTime = 0f;
		while (base.transform.position.x > startPosition.x)
		{
			yield return wait;
			elapsedTime += CupheadTime.FixedDelta;
			float speed = Mathf.Lerp(0f, properties.speed, elapsedTime / 0.25f);
			Vector3 position2 = base.transform.position;
			position2.x -= properties.speed * CupheadTime.FixedDelta;
			base.transform.position = position2;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001B0B RID: 6923 RVA: 0x000AA494 File Offset: 0x000A8694
	public IEnumerator moveIntro_cr(Vector3 endPosition, LevelProperties.FlyingCowboy.Bird properties)
	{
		WaitForFixedUpdate wait = new WaitForFixedUpdate();
		while (base.transform.position.x > endPosition.x)
		{
			yield return wait;
			Vector3 position = base.transform.position;
			position.x -= properties.speed * CupheadTime.FixedDelta;
			base.transform.position = position;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001B0C RID: 6924 RVA: 0x000AA4C0 File Offset: 0x000A86C0
	public IEnumerator attack_cr()
	{
		if (this.bulletLandingPosition > -400f)
		{
			yield break;
		}
		while (base.transform.position.x < -385f)
		{
			yield return null;
		}
		if (this.projectileSpawned || base.animator.GetCurrentAnimatorStateInfo(0).IsName("Turn"))
		{
			yield break;
		}
		base.animator.RoundFrame(0);
		base.animator.Play("Throw", 1);
		yield return base.animator.WaitForNormalizedTime(this, 1f, "Throw", 1, true, false, true);
		base.animator.Play("Off", 1);
		this.holdingFeetRenderer.enabled = false;
		this.emptyFeetRenderer.enabled = true;
		yield break;
	}

	// Token: 0x06001B0D RID: 6925 RVA: 0x000AA4DC File Offset: 0x000A86DC
	public void spawnProjectile()
	{
		if (this.projectileSpawned)
		{
			return;
		}
		this.projectileSpawned = true;
		this.SFX_COWGIRL_COWGIRL_P1_BirdCall();
		Vector3 position = this.projectileSpawnPoint.position;
		float num = KinematicUtilities.CalculateInitialSpeedToReachApex(this.properties.bulletArcHeight, this.properties.bulletGravity);
		float distance = this.bulletLandingPosition - position.x;
		float num2 = KinematicUtilities.CalculateHorizontalSpeedToTravelDistance(distance, num, position.y - FlyingCowboyLevelBirdProjectile.HighLandingPosition, this.properties.bulletGravity);
		Vector2 initialVelocity;
		initialVelocity..ctor(num2, num);
		float num3 = Mathf.Atan2(initialVelocity.y, initialVelocity.x) * 57.29578f;
		FlyingCowboyLevelBirdProjectile flyingCowboyLevelBirdProjectile = this.projectilePrefab.Create(this.projectileSpawnPoint.position) as FlyingCowboyLevelBirdProjectile;
		flyingCowboyLevelBirdProjectile.Initialize(initialVelocity, this.properties.bulletGravity, this.properties.shrapnelSecondStageDelay, this.properties.shrapnelSpeed, this.properties.shrapnelSpreadAngle, this.cowgirl);
		flyingCowboyLevelBirdProjectile.shrapnelCount = this.properties.shrapnelCount;
	}

	// Token: 0x06001B0E RID: 6926 RVA: 0x00016F48 File Offset: 0x00015148
	public void animationEvent_SpawnProjectile()
	{
		this.spawnProjectile();
	}

	// Token: 0x06001B0F RID: 6927 RVA: 0x000AA5EC File Offset: 0x000A87EC
	public void animationEvent_ShiftLayers()
	{
		foreach (SpriteRenderer spriteRenderer in base.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.sortingLayerName = "Background";
		}
	}

	// Token: 0x06001B10 RID: 6928 RVA: 0x00016F50 File Offset: 0x00015150
	public void SFX_COWGIRL_COWGIRL_P1_BirdCall()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_birdcall");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_birdcall");
	}

	// Token: 0x040015D0 RID: 5584
	[SerializeField]
	public FlyingCowboyLevelBirdProjectile projectilePrefab;

	// Token: 0x040015D1 RID: 5585
	[SerializeField]
	public SpriteRenderer holdingFeetRenderer;

	// Token: 0x040015D2 RID: 5586
	[SerializeField]
	public SpriteRenderer emptyFeetRenderer;

	// Token: 0x040015D3 RID: 5587
	[SerializeField]
	public Transform projectileSpawnPoint;

	// Token: 0x040015D4 RID: 5588
	public LevelProperties.FlyingCowboy.Bird properties;

	// Token: 0x040015D5 RID: 5589
	public FlyingCowboyLevelCowboy cowgirl;

	// Token: 0x040015D6 RID: 5590
	public float bulletLandingPosition;

	// Token: 0x040015D7 RID: 5591
	public bool projectileSpawned;
}
