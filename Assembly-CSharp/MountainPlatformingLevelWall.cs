using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000451 RID: 1105
public class MountainPlatformingLevelWall : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002F3D RID: 12093 RVA: 0x000275D1 File Offset: 0x000257D1
	public override void OnStart()
	{
	}

	// Token: 0x06002F3E RID: 12094 RVA: 0x000E1228 File Offset: 0x000DF428
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
		base.GetComponent<Collider2D>().enabled = false;
		this.head.GetComponent<Collider2D>().enabled = false;
		this.platform.gameObject.SetActive(false);
		base.GetComponent<DamageReceiver>().enabled = false;
		this.head.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.head.gameObject.tag = "Enemy";
		ParrySwitch component = this.head.GetComponent<ParrySwitch>();
		component.OnActivate += component.StartParryCooldown;
	}

	// Token: 0x06002F3F RID: 12095 RVA: 0x000275D3 File Offset: 0x000257D3
	public void FaceOn()
	{
		base.animator.Play("Face_Idle");
		base.animator.Play("Shield_Idle");
	}

	// Token: 0x06002F40 RID: 12096 RVA: 0x000E12D4 File Offset: 0x000DF4D4
	public IEnumerator move_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		AbstractPlayerController player = PlayerManager.GetNext();
		while (player.transform.position.x < this.startTrigger.transform.position.x)
		{
			yield return null;
			if (player == null || player.IsDead)
			{
				player = PlayerManager.GetNext();
			}
		}
		base.GetComponent<Collider2D>().enabled = true;
		this.head.GetComponent<Collider2D>().enabled = true;
		this.platform.gameObject.SetActive(true);
		base.animator.SetTrigger("OnIntro");
		yield return base.animator.WaitForAnimationToEnd(this, "Wall_Intro", false, true);
		base.StartCoroutine(this.shoot_cr());
		float t = 0f;
		float time = base.Properties.wallFaceTravelTime;
		bool movingUp = false;
		float top = this.head.transform.position.y + 100f;
		float bottom = this.head.transform.position.y - 100f;
		float start = this.head.transform.position.y;
		float end = 0f;
		for (;;)
		{
			start = this.head.transform.position.y;
			if (movingUp)
			{
				end = top;
			}
			else
			{
				end = bottom;
			}
			while (t < time)
			{
				float val = t / time;
				this.head.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val)), null);
				t += CupheadTime.Delta;
				yield return null;
			}
			this.head.transform.SetPosition(null, new float?(end), null);
			movingUp = !movingUp;
			t = 0f;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002F41 RID: 12097 RVA: 0x000E12F0 File Offset: 0x000DF4F0
	public IEnumerator shoot_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, base.Properties.wallAttackDelay.RandomFloat());
			base.animator.SetTrigger("Attack");
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002F42 RID: 12098 RVA: 0x000E130C File Offset: 0x000DF50C
	public void ShootProjectileEffect()
	{
		if (this.projectileCount == 2)
		{
			this.projectilePinkEffect.Create(new Vector3(this.projectileRoot.transform.position.x - 20f, this.projectileRoot.transform.position.y));
		}
		else
		{
			this.projectileEffect.Create(new Vector3(this.projectileRoot.transform.position.x - 20f, this.projectileRoot.transform.position.y));
		}
	}

	// Token: 0x06002F43 RID: 12099 RVA: 0x000E13B8 File Offset: 0x000DF5B8
	public void ShootProjectile()
	{
		if (this.projectileCount == 2)
		{
			this.projectileCount = 0;
			this.bouncyPinkProjectile.Create(this.projectileRoot.position, 0f, new Vector2(-base.Properties.wallProjectileXSpeed, base.Properties.wallProjectileYSpeed), base.Properties.wallProjectileGravity, this.groundPosY.position.y);
		}
		else
		{
			this.projectileCount++;
			this.bouncyProjectile.Create(this.projectileRoot.position, 0f, new Vector2(-base.Properties.wallProjectileXSpeed, base.Properties.wallProjectileYSpeed), base.Properties.wallProjectileGravity, this.groundPosY.position.y);
		}
	}

	// Token: 0x06002F44 RID: 12100 RVA: 0x000E14A4 File Offset: 0x000DF6A4
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0f, 0f, 1f, 1f);
		Gizmos.DrawLine(this.startTrigger.transform.position, new Vector3(this.startTrigger.transform.position.x, 5000f, 0f));
	}

	// Token: 0x06002F45 RID: 12101 RVA: 0x000E1514 File Offset: 0x000DF714
	public override void Die()
	{
		this.StopAllCoroutines();
		this.head.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("OnDeath");
		base.StartCoroutine(this.dying_cr());
		base.StartCoroutine(this.death_shake_cr());
		base.StartCoroutine(this.create_explosions_cr());
	}

	// Token: 0x06002F46 RID: 12102 RVA: 0x000275F5 File Offset: 0x000257F5
	public void FaceDead()
	{
		base.animator.Play("Face_Death_Loop");
	}

	// Token: 0x06002F47 RID: 12103 RVA: 0x000E1570 File Offset: 0x000DF770
	public IEnumerator death_shake_cr()
	{
		bool movingUp = false;
		float top = base.transform.position.y + 4f;
		float bottom = base.transform.position.y - 4f;
		float start = base.transform.position.y;
		float end = 0f;
		float t = 0f;
		float time = 0.01f;
		while (!this.isDead)
		{
			start = base.transform.position.y;
			if (movingUp)
			{
				end = top;
			}
			else
			{
				end = bottom;
			}
			while (t < time)
			{
				float val = t / time;
				base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutBounce, start, end, val)), null);
				t += CupheadTime.Delta;
				yield return null;
			}
			base.transform.SetPosition(null, new float?(end), null);
			movingUp = !movingUp;
			t = 0f;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002F48 RID: 12104 RVA: 0x000E158C File Offset: 0x000DF78C
	public IEnumerator create_explosions_cr()
	{
		while (!this.isDead)
		{
			base.GetComponent<EffectRadius>().CreateInRadius();
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0.2f, 0.4f));
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002F49 RID: 12105 RVA: 0x000E15A8 File Offset: 0x000DF7A8
	public IEnumerator dying_cr()
	{
		AudioManager.Play("castle_mountain_wall_death");
		this.emitAudioFromObject.Add("castle_mountain_wall_death");
		yield return base.animator.WaitForAnimationToEnd(this, "Wall_Death", false, true);
		yield return CupheadTime.WaitForSeconds(this, 1.67f);
		float t = 0f;
		float time = 0.65f;
		while (t < time)
		{
			base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - t / time);
			this.head.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - t / time);
			this.shield.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - t / time);
			this.foreground1.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - t / time);
			this.foreground2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - t / time);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.isDead = true;
		this.<Die>__BaseCallProxy0();
		yield return null;
		yield break;
	}

	// Token: 0x06002F4A RID: 12106 RVA: 0x00027607 File Offset: 0x00025807
	public void SoundMountainWallShoot()
	{
		AudioManager.Play("castle_mountain_wall_attack");
		this.emitAudioFromObject.Add("castle_mountain_wall_attack");
	}

	// Token: 0x06002F4B RID: 12107 RVA: 0x00027623 File Offset: 0x00025823
	public void SoundMountainWallIntro()
	{
		AudioManager.Play("castle_mountain_wall_spawn");
		this.emitAudioFromObject.Add("castle_mountain_wall_spawn");
	}

	// Token: 0x06002F4C RID: 12108 RVA: 0x0002763F File Offset: 0x0002583F
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.projectileEffect = null;
		this.projectilePinkEffect = null;
		this.bouncyPinkProjectile = null;
		this.bouncyProjectile = null;
	}

	// Token: 0x04002730 RID: 10032
	[SerializeField]
	public Transform groundPosY;

	// Token: 0x04002731 RID: 10033
	[SerializeField]
	public Transform platform;

	// Token: 0x04002732 RID: 10034
	[SerializeField]
	public SpriteRenderer foreground1;

	// Token: 0x04002733 RID: 10035
	[SerializeField]
	public SpriteRenderer foreground2;

	// Token: 0x04002734 RID: 10036
	[SerializeField]
	public SpriteRenderer shield;

	// Token: 0x04002735 RID: 10037
	[SerializeField]
	public Transform head;

	// Token: 0x04002736 RID: 10038
	[SerializeField]
	public Transform startTrigger;

	// Token: 0x04002737 RID: 10039
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x04002738 RID: 10040
	[SerializeField]
	public Effect projectileEffect;

	// Token: 0x04002739 RID: 10041
	[SerializeField]
	public Effect projectilePinkEffect;

	// Token: 0x0400273A RID: 10042
	[SerializeField]
	public MountainPlatformingLevelWallProjectile bouncyProjectile;

	// Token: 0x0400273B RID: 10043
	[SerializeField]
	public MountainPlatformingLevelWallProjectile bouncyPinkProjectile;

	// Token: 0x0400273C RID: 10044
	public int projectileCount;

	// Token: 0x0400273D RID: 10045
	public bool isDead;
}
