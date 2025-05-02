using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000372 RID: 882
public class SaltbakerLevelFeistTurret : AbstractCollidableObject
{
	// Token: 0x17000322 RID: 802
	// (get) Token: 0x060026EC RID: 9964 RVA: 0x00020B83 File Offset: 0x0001ED83
	// (set) Token: 0x060026ED RID: 9965 RVA: 0x00020B8B File Offset: 0x0001ED8B
	public bool IsActivated { get; set; }

	// Token: 0x060026EE RID: 9966 RVA: 0x000CA1D8 File Offset: 0x000C83D8
	public void Start()
	{
		this.SFX_SALTBAKER_P2_Saltshaker_Appear();
		this.basePos = base.transform.position;
		this.fxRend.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?((float)Random.Range(0, 360)));
		this.coll.enabled = true;
	}

	// Token: 0x060026EF RID: 9967 RVA: 0x000CA240 File Offset: 0x000C8440
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.health <= 0f && this.IsActivated)
		{
			return;
		}
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			if (this.IsActivated && this.parent.phaseTwoStarted)
			{
				if (!this.parent.preventAdditionalTurretLaunch)
				{
					if (this.parent.PreDamagePhaseTwoAndReturnWhetherDoomed(this.startHealth))
					{
						this.parent.preventAdditionalTurretLaunch = true;
					}
					base.StartCoroutine(this.fire_and_wait_to_respawn_cr());
				}
				else
				{
					this.health = 1f;
				}
			}
			else
			{
				this.health = 1f;
			}
		}
	}

	// Token: 0x060026F0 RID: 9968 RVA: 0x00020B94 File Offset: 0x0001ED94
	public void FixedUpdate()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.FixedUpdate();
		}
	}

	// Token: 0x060026F1 RID: 9969 RVA: 0x00020BAC File Offset: 0x0001EDAC
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060026F2 RID: 9970 RVA: 0x000CA308 File Offset: 0x000C8508
	public void Setup(LevelProperties.Saltbaker.Turrets properties, SaltbakerLevelSaltbaker parent, int index)
	{
		this.properties = properties;
		this.parent = parent;
		this.coll = base.GetComponent<Collider2D>();
		this.sprite = base.GetComponent<SpriteRenderer>();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.index = index;
	}

	// Token: 0x060026F3 RID: 9971 RVA: 0x00020BCA File Offset: 0x0001EDCA
	public void AniEvent_Activate()
	{
		this.health = this.properties.turretHealth;
		this.startHealth = this.health;
		this.IsActivated = true;
		this.coll.enabled = true;
	}

	// Token: 0x060026F4 RID: 9972 RVA: 0x000CA370 File Offset: 0x000C8570
	public IEnumerator fire_and_wait_to_respawn_cr()
	{
		base.animator.Play("Explode", 1, 0f);
		if (this.shootCR != null)
		{
			base.StopCoroutine(this.shootCR);
		}
		base.transform.position = this.basePos;
		base.animator.ResetTrigger("Shoot");
		this.coll.enabled = false;
		this.IsActivated = false;
		base.transform.localScale = new Vector3(1f, 1f);
		base.animator.Play("Attack" + this.index);
		this.SFX_SALTBAKER_P2_Saltshaker_DieLaunch();
		base.animator.Update(0f);
		yield return new WaitForEndOfFrame();
		yield return base.animator.WaitForAnimationToEnd(this, "Attack" + this.index, false, true);
		this.sprite.enabled = false;
		yield return CupheadTime.WaitForSeconds(this, this.properties.respawnTime - 0.75f);
		base.transform.localScale = new Vector3(-Mathf.Sign(base.transform.position.x), 1f);
		this.fxRend.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?((float)Random.Range(0, 360)));
		this.sprite.sortingLayerName = "Projectiles";
		base.animator.Play("Intro");
		base.animator.Update(0f);
		this.SFX_SALTBAKER_P2_Saltshaker_Appear();
		this.sprite.enabled = true;
		yield break;
	}

	// Token: 0x060026F5 RID: 9973 RVA: 0x00020BFC File Offset: 0x0001EDFC
	public void AniEvent_DamageSaltbaker()
	{
		this.SFX_SALTBAKER_P2_Saltshaker_LaunchImpact();
		this.parent.DamageSaltbaker(this.startHealth, this.index);
	}

	// Token: 0x060026F6 RID: 9974 RVA: 0x00020C1B File Offset: 0x0001EE1B
	public void Shoot(bool isPink, float warning)
	{
		this.shootCR = base.StartCoroutine(this.shoot_cr(isPink, warning));
	}

	// Token: 0x060026F7 RID: 9975 RVA: 0x000CA38C File Offset: 0x000C858C
	public IEnumerator shoot_cr(bool isPink, float warning)
	{
		Vector3 upPos = base.transform.position + Vector3.up * (float)((base.transform.position.y >= 0f) ? -24 : 40);
		this.shootPink = isPink;
		base.animator.Play("ShootStart");
		base.animator.Update(0f);
		while (base.animator.GetCurrentAnimatorStateInfo(0).IsName("ShootStart"))
		{
			base.transform.position = Vector3.Lerp(this.basePos, upPos, EaseUtils.EaseOutSine(0f, 1f, base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
			yield return null;
		}
		base.transform.position = upPos;
		if (warning > 0.4f)
		{
			yield return CupheadTime.WaitForSeconds(this, warning - 0.4f);
			this.SFX_SALTBAKER_P2_Saltshaker_PreSneeze();
			yield return CupheadTime.WaitForSeconds(this, 0.4f);
		}
		else
		{
			this.SFX_SALTBAKER_P2_Saltshaker_PreSneeze();
			yield return CupheadTime.WaitForSeconds(this, warning);
		}
		base.animator.SetTrigger("Shoot");
		yield return base.animator.WaitForAnimationToStart(this, "ShootEnd", false);
		while (base.animator.GetCurrentAnimatorStateInfo(0).IsName("ShootEnd"))
		{
			base.transform.position = Vector3.Lerp(upPos, this.basePos, EaseUtils.EaseInSine(0f, 1f, base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
			yield return null;
		}
		base.transform.position = this.basePos;
		yield break;
	}

	// Token: 0x060026F8 RID: 9976 RVA: 0x000CA3B8 File Offset: 0x000C85B8
	public void AniEvent_SpawnProjectile()
	{
		float num = MathUtils.DirectionToAngle(PlayerManager.GetNext().center - base.transform.position);
		SaltbakerLevelTurretBullet saltbakerLevelTurretBullet = this.shootPink ? this.pinkBulletPrefab : this.bulletPrefab;
		saltbakerLevelTurretBullet = saltbakerLevelTurretBullet.Create(this.sneezeFX.transform.position + MathUtils.AngleToDirection(num) * 150f, num, this.properties.shotSpeed, this.parent);
		saltbakerLevelTurretBullet.transform.localScale = base.transform.localScale;
		this.sneezeFX.transform.localScale = base.transform.localScale;
		this.sneezeFX.transform.eulerAngles = new Vector3(0f, 0f, num - 45f);
		this.SFX_SALTBAKER_P2_Saltshaker_SneezeAttack();
	}

	// Token: 0x060026F9 RID: 9977 RVA: 0x00020C31 File Offset: 0x0001EE31
	public void AniEvent_BottomLeftTurretSnapForward()
	{
		this.sprite.sortingLayerName = "Foreground";
	}

	// Token: 0x060026FA RID: 9978 RVA: 0x000CA4A8 File Offset: 0x000C86A8
	public void LateUpdate()
	{
		this.pepperText.enabled = false;
		this.pepperTextFlip.enabled = false;
		SpriteRenderer spriteRenderer = (base.transform.localScale.x != 1f) ? this.pepperTextFlip : this.pepperText;
		spriteRenderer.enabled = (this.sprite.enabled && this.sprite.sprite != null && !base.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack0") && !base.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !base.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && !base.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"));
	}

	// Token: 0x060026FB RID: 9979 RVA: 0x000CA5A4 File Offset: 0x000C87A4
	public void Die()
	{
		this.coll.enabled = false;
		this.sprite.sortingLayerName = "Projectiles";
		this.StopAllCoroutines();
		base.animator.ResetTrigger("Shoot");
		base.transform.position = this.basePos;
		base.animator.SetBool("Dead", true);
		this.SFX_SALTBAKER_P2_Saltshaker_Disappear();
	}

	// Token: 0x060026FC RID: 9980 RVA: 0x00020C43 File Offset: 0x0001EE43
	public void SFX_SALTBAKER_P2_Saltshaker_Appear()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_saltshaker_appear");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p2_saltshaker_appear");
	}

	// Token: 0x060026FD RID: 9981 RVA: 0x00020C5F File Offset: 0x0001EE5F
	public void SFX_SALTBAKER_P2_Saltshaker_Disappear()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_saltshaker_disappear");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p2_saltshaker_disappear");
	}

	// Token: 0x060026FE RID: 9982 RVA: 0x00020C7B File Offset: 0x0001EE7B
	public void SFX_SALTBAKER_P2_Saltshaker_SneezeAttack()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_saltshaker_sneezeattack");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p2_saltshaker_sneezeattack");
	}

	// Token: 0x060026FF RID: 9983 RVA: 0x00020C97 File Offset: 0x0001EE97
	public void SFX_SALTBAKER_P2_Saltshaker_PreSneeze()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_saltshaker_sneezepre");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p2_saltshaker_sneezepre");
	}

	// Token: 0x06002700 RID: 9984 RVA: 0x00020CB3 File Offset: 0x0001EEB3
	public void SFX_SALTBAKER_P2_Saltshaker_DieLaunch()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_saltshaker_dielaunch");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p2_saltshaker_dielaunch");
	}

	// Token: 0x06002701 RID: 9985 RVA: 0x00020CCF File Offset: 0x0001EECF
	public void SFX_SALTBAKER_P2_Saltshaker_LaunchImpact()
	{
		AudioManager.Play("sfx_DLC_Saltbaker_P2_Saltshaker_LaunchImpact");
	}

	// Token: 0x04002025 RID: 8229
	[SerializeField]
	public SaltbakerLevelTurretBullet bulletPrefab;

	// Token: 0x04002026 RID: 8230
	[SerializeField]
	public SaltbakerLevelTurretBullet pinkBulletPrefab;

	// Token: 0x04002027 RID: 8231
	public LevelProperties.Saltbaker.Turrets properties;

	// Token: 0x04002028 RID: 8232
	public SaltbakerLevelSaltbaker parent;

	// Token: 0x04002029 RID: 8233
	public Collider2D coll;

	// Token: 0x0400202A RID: 8234
	public SpriteRenderer sprite;

	// Token: 0x0400202B RID: 8235
	[SerializeField]
	public SpriteRenderer pepperText;

	// Token: 0x0400202C RID: 8236
	[SerializeField]
	public SpriteRenderer pepperTextFlip;

	// Token: 0x0400202D RID: 8237
	[SerializeField]
	public GameObject fxRend;

	// Token: 0x0400202E RID: 8238
	[SerializeField]
	public GameObject sneezeFX;

	// Token: 0x0400202F RID: 8239
	public DamageDealer damageDealer;

	// Token: 0x04002030 RID: 8240
	public DamageReceiver damageReceiver;

	// Token: 0x04002031 RID: 8241
	public float health;

	// Token: 0x04002032 RID: 8242
	public float startHealth;

	// Token: 0x04002033 RID: 8243
	public int index;

	// Token: 0x04002034 RID: 8244
	public bool shootPink;

	// Token: 0x04002035 RID: 8245
	public Coroutine shootCR;

	// Token: 0x04002036 RID: 8246
	public Vector3 basePos;
}
