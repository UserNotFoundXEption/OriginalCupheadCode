using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200041F RID: 1055
public class FunhousePlatformingLevelRocket : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x06002DBB RID: 11707 RVA: 0x0002621F File Offset: 0x0002441F
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x06002DBC RID: 11708 RVA: 0x000DD748 File Offset: 0x000DB948
	public override void Start()
	{
		base.Start();
		this.collisionChild = base.GetComponentInChildren<CollisionChild>();
		this.collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		this.collisionDamageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002DBD RID: 11709 RVA: 0x000DD798 File Offset: 0x000DB998
	public void Init(Vector2 pos, bool gravityReversed, bool onRight)
	{
		base.transform.position = pos;
		AudioManager.PlayLoop("funhouse_rocket_idle_loop");
		FunhousePlatformingLevelRocket.ROCKETS_ALIVE++;
		this.emitAudioFromObject.Add("funhouse_rocket_idle_loop");
		this.gravityReversed = gravityReversed;
		if (gravityReversed)
		{
			base.transform.SetScale(null, new float?(-1f), null);
		}
		this._direction = ((!onRight) ? PlatformingLevelGroundMovementEnemy.Direction.Right : PlatformingLevelGroundMovementEnemy.Direction.Left);
		base.StartCoroutine(this.launch_cr());
	}

	// Token: 0x06002DBE RID: 11710 RVA: 0x000DD830 File Offset: 0x000DBA30
	public IEnumerator launch_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		AbstractPlayerController player = PlayerManager.GetNext();
		float dist = player.transform.position.x - base.transform.position.x;
		while (Mathf.Abs(dist) > this.distToLaunch)
		{
			player = PlayerManager.GetNext();
			dist = player.transform.position.x - base.transform.position.x;
			yield return null;
		}
		FunhousePlatformingLevelRocket.ROCKETS_ALIVE--;
		if (FunhousePlatformingLevelRocket.ROCKETS_ALIVE == 0)
		{
			AudioManager.Stop("funhouse_rocket_idle_loop");
		}
		this.landing = true;
		this.launched = true;
		base.animator.SetTrigger("OnShoot");
		while (this.launched)
		{
			if (this.gravityReversed)
			{
				base.transform.position += Vector3.down * this.launchSpeed * CupheadTime.FixedDelta;
			}
			else
			{
				base.transform.position += Vector3.up * this.launchSpeed * CupheadTime.FixedDelta;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002DBF RID: 11711 RVA: 0x00026227 File Offset: 0x00024427
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		if (this.launched && phase == CollisionPhase.Enter)
		{
			this.Die();
		}
	}

	// Token: 0x06002DC0 RID: 11712 RVA: 0x00026248 File Offset: 0x00024448
	public override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (this.launched && phase == CollisionPhase.Enter)
		{
			this.Die();
		}
	}

	// Token: 0x06002DC1 RID: 11713 RVA: 0x000DD84C File Offset: 0x000DBA4C
	public override void Die()
	{
		AudioManager.Stop("funhouse_rocket_trans_to_spin");
		AudioManager.Play("funhouse_rocket_explode");
		this.emitAudioFromObject.Add("funhouse_rocket_explode");
		this.explosion.Create(this.sprite.transform.position);
		base.Die();
	}

	// Token: 0x06002DC2 RID: 11714 RVA: 0x00026269 File Offset: 0x00024469
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (!this.launched)
		{
			FunhousePlatformingLevelRocket.ROCKETS_ALIVE--;
		}
		if (FunhousePlatformingLevelRocket.ROCKETS_ALIVE == 0)
		{
			AudioManager.Stop("funhouse_rocket_idle_loop");
		}
	}

	// Token: 0x06002DC3 RID: 11715 RVA: 0x0002629C File Offset: 0x0002449C
	public void SoundRocketTransToSpin()
	{
		AudioManager.Play("funhouse_rocket_trans_to_spin");
		this.emitAudioFromObject.Add("funhouse_rocket_trans_to_spin");
		AudioManager.Play("funhouse_rocket_explode");
		this.emitAudioFromObject.Add("funhouse_rocket_explode");
	}

	// Token: 0x040025E4 RID: 9700
	public static int ROCKETS_ALIVE;

	// Token: 0x040025E5 RID: 9701
	[SerializeField]
	public Transform sprite;

	// Token: 0x040025E6 RID: 9702
	[SerializeField]
	public FunhousePlatformingLevelExplosionFX explosion;

	// Token: 0x040025E7 RID: 9703
	[SerializeField]
	public float distToLaunch;

	// Token: 0x040025E8 RID: 9704
	[SerializeField]
	public float launchSpeed;

	// Token: 0x040025E9 RID: 9705
	public bool launched;

	// Token: 0x040025EA RID: 9706
	public CollisionChild collisionChild;

	// Token: 0x040025EB RID: 9707
	[SerializeField]
	public DamageReceiver collisionDamageReceiver;
}
