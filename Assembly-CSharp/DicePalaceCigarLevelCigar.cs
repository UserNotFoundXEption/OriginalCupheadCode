using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001D9 RID: 473
public class DicePalaceCigarLevelCigar : LevelProperties.DicePalaceCigar.Entity
{
	// Token: 0x060015FB RID: 5627 RVA: 0x0009E4E0 File Offset: 0x0009C6E0
	public override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		base.Awake();
	}

	// Token: 0x060015FC RID: 5628 RVA: 0x00012A39 File Offset: 0x00010C39
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060015FD RID: 5629 RVA: 0x00012A51 File Offset: 0x00010C51
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060015FE RID: 5630 RVA: 0x00012A6F File Offset: 0x00010C6F
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060015FF RID: 5631 RVA: 0x0009E53C File Offset: 0x0009C73C
	public override void LevelInit(LevelProperties.DicePalaceCigar properties)
	{
		this.onRightSpawn = true;
		this.isFiring = false;
		this.rightAsh.SetActive(false);
		this.spitAttackCountIndex = Random.Range(0, properties.CurrentState.spiralSmoke.attackCount.Split(new char[]
		{
			','
		}).Length);
		this.spitAttackDirectionIndex = Random.Range(0, properties.CurrentState.spiralSmoke.rotationDirectionString.Split(new char[]
		{
			','
		}).Length);
		this.ghostAttackDelayIndex = Random.Range(0, properties.CurrentState.cigaretteGhost.attackDelayString.Split(new char[]
		{
			','
		}).Length);
		this.ghostSpawnPositionIndex = Random.Range(0, properties.CurrentState.cigaretteGhost.spawnPositionString.Split(new char[]
		{
			','
		}).Length);
		base.LevelInit(properties);
		Level.Current.OnIntroEvent += this.OnIntroEnd;
		Level.Current.OnWinEvent += this.OnDeath;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001600 RID: 5632 RVA: 0x0009E65C File Offset: 0x0009C85C
	public IEnumerator intro_cr()
	{
		AudioManager.PlayLoop("dice_palace_cigar_intro_start_loop");
		this.emitAudioFromObject.Add("dice_palace_cigar_intro_start_loop");
		yield return CupheadTime.WaitForSeconds(this, 2f);
		base.animator.SetTrigger("Continue");
		yield return null;
		yield break;
	}

	// Token: 0x06001601 RID: 5633 RVA: 0x00012A82 File Offset: 0x00010C82
	public void StopIntroLoop()
	{
		AudioManager.Stop("dice_palace_cigar_intro_start_loop");
	}

	// Token: 0x06001602 RID: 5634 RVA: 0x00012A8E File Offset: 0x00010C8E
	public void OnIntroEnd()
	{
		base.StartCoroutine(this.attack_cr());
		base.StartCoroutine(this.ghostAttack_cr());
	}

	// Token: 0x06001603 RID: 5635 RVA: 0x0009E678 File Offset: 0x0009C878
	public IEnumerator attack_cr()
	{
		for (;;)
		{
			base.GetComponent<BoxCollider2D>().enabled = true;
			this.maxCounter = Parser.IntParse(base.properties.CurrentState.spiralSmoke.attackCount.Split(new char[]
			{
				','
			})[this.spitAttackCountIndex]);
			this.isFiring = true;
			while (this.isFiring)
			{
				if (this.counter > this.maxCounter)
				{
					this.isFiring = false;
					this.counter = 0;
					break;
				}
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.spiralSmoke.hesitateBeforeAttackDelay);
				this.counter++;
				base.animator.SetTrigger("IsAttacking");
				yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
				yield return null;
			}
			this.spitAttackCountIndex++;
			if (this.spitAttackCountIndex >= base.properties.CurrentState.spiralSmoke.attackCount.Split(new char[]
			{
				','
			}).Length)
			{
				this.spitAttackCountIndex = 0;
			}
			this.spitAttackDirectionIndex++;
			if (this.spitAttackDirectionIndex >= base.properties.CurrentState.spiralSmoke.rotationDirectionString.Split(new char[]
			{
				','
			}).Length)
			{
				this.spitAttackDirectionIndex = 0;
			}
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.cigar.warningDelay);
			base.animator.SetTrigger("OnStateChange");
			yield return base.animator.WaitForAnimationToEnd(this, "Teleport_End", false, true);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001604 RID: 5636 RVA: 0x00012AAA File Offset: 0x00010CAA
	public void TeleportSFX()
	{
		AudioManager.Play("dice_palace_cigar_teleport");
		this.emitAudioFromObject.Add("dice_palace_cigar_teleport");
	}

	// Token: 0x06001605 RID: 5637 RVA: 0x00012AC6 File Offset: 0x00010CC6
	public void AttackSFX()
	{
		AudioManager.Play("dice_palace_cigar_attack");
		this.emitAudioFromObject.Add("dice_palace_cigar_attack");
	}

	// Token: 0x06001606 RID: 5638 RVA: 0x0009E694 File Offset: 0x0009C894
	public void SwitchSides()
	{
		this.leftAsh.SetActive(!this.onRightSpawn);
		this.rightAsh.SetActive(this.onRightSpawn);
		this.onRightSpawn = !this.onRightSpawn;
		if (!this.facingBack)
		{
			base.transform.Rotate(Vector3.up, 180f);
		}
		if (this.onRightSpawn)
		{
			base.transform.position = this.rightSpawnPointFacingRight.position;
		}
		else
		{
			base.transform.position = this.leftSpawnPointFacingRight.position;
		}
		base.StartCoroutine(this.finish_teleport_cr());
	}

	// Token: 0x06001607 RID: 5639 RVA: 0x0009E740 File Offset: 0x0009C940
	public void Rotate()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		if (next.transform.position.x < base.transform.position.x && !this.onRightSpawn)
		{
			base.transform.position = this.leftSpawnPointFacingLeft.position;
			base.transform.Rotate(Vector3.up, 180f);
			this.facingBack = true;
		}
		else if (next.transform.position.x > base.transform.position.x && this.onRightSpawn)
		{
			base.transform.position = this.rightSpawnPointFacingLeft.position;
			base.transform.Rotate(Vector3.up, 180f);
			this.facingBack = true;
		}
		else
		{
			base.transform.Rotate(Vector3.up, 0f);
			this.facingBack = false;
		}
	}

	// Token: 0x06001608 RID: 5640 RVA: 0x0009E84C File Offset: 0x0009CA4C
	public void CheckIfBackward()
	{
		if (this.facingBack)
		{
			base.transform.Rotate(Vector3.up, 180f);
			if (this.onRightSpawn)
			{
				base.transform.position = this.rightSpawnPointFacingRight.position;
			}
			else
			{
				base.transform.position = this.leftSpawnPointFacingRight.position;
			}
			this.facingBack = false;
		}
	}

	// Token: 0x06001609 RID: 5641 RVA: 0x0009E8BC File Offset: 0x0009CABC
	public IEnumerator finish_teleport_cr()
	{
		AudioManager.PlayLoop("dice_palace_cigar_teleport_warning_loop");
		this.emitAudioFromObject.Add("dice_palace_cigar_teleport_warning_loop");
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.cigar.warningDelay);
		this.VOXTeleportWarning();
		AudioManager.Stop("dice_palace_cigar_teleport_warning_loop");
		AudioManager.Play("dice_palace_cigar_teleport_end");
		this.emitAudioFromObject.Add("dice_palace_cigar_teleport_end");
		base.animator.SetTrigger("Continue");
		yield return null;
		yield break;
	}

	// Token: 0x0600160A RID: 5642 RVA: 0x0009E8D8 File Offset: 0x0009CAD8
	public void SpitAttack()
	{
		bool onRight;
		if (this.facingBack)
		{
			onRight = !this.onRightSpawn;
		}
		else
		{
			onRight = this.onRightSpawn;
		}
		AbstractProjectile abstractProjectile = this.spitPrefab.Create(this.spitSpawnPoint.position, (float)((int)base.transform.eulerAngles.y));
		if (base.properties.CurrentState.spiralSmoke.rotationDirectionString.Split(new char[]
		{
			','
		})[this.spitAttackDirectionIndex][0] == '1')
		{
			abstractProjectile.GetComponent<DicePalaceCigarLevelCigarSpit>().InitProjectile(base.properties, true, onRight);
		}
		else
		{
			abstractProjectile.GetComponent<DicePalaceCigarLevelCigarSpit>().InitProjectile(base.properties, false, onRight);
		}
	}

	// Token: 0x0600160B RID: 5643 RVA: 0x0009E9A0 File Offset: 0x0009CBA0
	public IEnumerator ghostAttack_cr()
	{
		for (;;)
		{
			float spawnPosx = Random.Range(this.ghostSpawnPoint.transform.position.x - this.ghostOffset, this.ghostSpawnPoint.transform.position.x + this.ghostOffset);
			yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(base.properties.CurrentState.cigaretteGhost.attackDelayString.Split(new char[]
			{
				','
			})[this.ghostAttackDelayIndex]));
			AbstractProjectile proj = this.ghostPrefab.Create(new Vector2(spawnPosx, this.ghostSpawnPoint.transform.position.y));
			proj.GetComponent<DicePalaceCigarLevelCigaretteGhost>().InitGhost(base.properties);
			this.ghostAttackDelayIndex++;
			if (this.ghostAttackDelayIndex >= base.properties.CurrentState.cigaretteGhost.attackDelayString.Split(new char[]
			{
				','
			}).Length)
			{
				this.ghostAttackDelayIndex = 0;
			}
			this.ghostSpawnPositionIndex++;
			if (this.ghostSpawnPositionIndex >= base.properties.CurrentState.cigaretteGhost.spawnPositionString.Split(new char[]
			{
				','
			}).Length)
			{
				this.ghostSpawnPositionIndex = 0;
			}
		}
		yield break;
	}

	// Token: 0x0600160C RID: 5644 RVA: 0x00012AE2 File Offset: 0x00010CE2
	public void SmokeAB()
	{
		this.smokeA.Create(this.smokeSpawnPoint.transform.position);
		this.smokeB.Create(this.smokeSpawnPoint.transform.position);
	}

	// Token: 0x0600160D RID: 5645 RVA: 0x00012B1C File Offset: 0x00010D1C
	public void SmokeB()
	{
		this.smokeB.Create(this.smokeSpawnPoint.transform.position);
	}

	// Token: 0x0600160E RID: 5646 RVA: 0x0009E9BC File Offset: 0x0009CBBC
	public void SwitchLayer()
	{
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Map.ToString();
		base.GetComponent<SpriteRenderer>().sortingOrder = 200;
	}

	// Token: 0x0600160F RID: 5647 RVA: 0x00012B3A File Offset: 0x00010D3A
	public override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
		this.smokeA = null;
		this.smokeB = null;
		this.spitPrefab = null;
		this.ghostPrefab = null;
	}

	// Token: 0x06001610 RID: 5648 RVA: 0x0009E9F4 File Offset: 0x0009CBF4
	public void OnDeath()
	{
		AudioManager.Play("dice_palace_cigar_death");
		this.emitAudioFromObject.Add("dice_palace_cigar_death");
		this.VOXDeath();
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("OnDeath");
	}

	// Token: 0x06001611 RID: 5649 RVA: 0x00012B64 File Offset: 0x00010D64
	public void DeathSFX()
	{
		AudioManager.Play("dice_palace_cigar_death_end");
		this.emitAudioFromObject.Add("dice_palace_cigar_death_end");
	}

	// Token: 0x06001612 RID: 5650 RVA: 0x00012B80 File Offset: 0x00010D80
	public void VOXIntro()
	{
		AudioManager.Play("cigar_vox_intro");
		this.emitAudioFromObject.Add("cigar_vox_intro");
	}

	// Token: 0x06001613 RID: 5651 RVA: 0x00012B9C File Offset: 0x00010D9C
	public void VOXDeath()
	{
		AudioManager.Play("cigar_vox_death");
		this.emitAudioFromObject.Add("cigar_vox_death");
	}

	// Token: 0x06001614 RID: 5652 RVA: 0x00012BB8 File Offset: 0x00010DB8
	public void VOXTeleport()
	{
		AudioManager.Play("cigar_vox_pre_teleport");
		this.emitAudioFromObject.Add("cigar_vox_pre_teleport");
	}

	// Token: 0x06001615 RID: 5653 RVA: 0x00012BD4 File Offset: 0x00010DD4
	public void VOXTeleportWarning()
	{
		AudioManager.Play("cigar_vox_warning");
		this.emitAudioFromObject.Add("cigar_vox_warning");
	}

	// Token: 0x06001616 RID: 5654 RVA: 0x0009EA44 File Offset: 0x0009CC44
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(new Vector2(this.ghostSpawnPoint.transform.position.x - this.ghostOffset, this.ghostSpawnPoint.transform.position.y), new Vector2(this.ghostSpawnPoint.transform.position.x + this.ghostOffset, this.ghostSpawnPoint.transform.position.y));
	}

	// Token: 0x040011E8 RID: 4584
	[Space(5f)]
	[SerializeField]
	public GameObject leftAshTray;

	// Token: 0x040011E9 RID: 4585
	[SerializeField]
	public GameObject rightAshTray;

	// Token: 0x040011EA RID: 4586
	[Space(5f)]
	[SerializeField]
	public GameObject leftAsh;

	// Token: 0x040011EB RID: 4587
	[SerializeField]
	public GameObject rightAsh;

	// Token: 0x040011EC RID: 4588
	[Space(5f)]
	[SerializeField]
	public Transform leftSpawnPointFacingRight;

	// Token: 0x040011ED RID: 4589
	[SerializeField]
	public Transform leftSpawnPointFacingLeft;

	// Token: 0x040011EE RID: 4590
	[SerializeField]
	public Transform rightSpawnPointFacingLeft;

	// Token: 0x040011EF RID: 4591
	[SerializeField]
	public Transform rightSpawnPointFacingRight;

	// Token: 0x040011F0 RID: 4592
	[Space(5f)]
	[SerializeField]
	public Transform smokeSpawnPoint;

	// Token: 0x040011F1 RID: 4593
	[SerializeField]
	public Effect smokeA;

	// Token: 0x040011F2 RID: 4594
	[SerializeField]
	public Effect smokeB;

	// Token: 0x040011F3 RID: 4595
	[SerializeField]
	public CollisionChild collisionChild;

	// Token: 0x040011F4 RID: 4596
	[Space(10f)]
	[SerializeField]
	public DicePalaceCigarLevelCigarSpit spitPrefab;

	// Token: 0x040011F5 RID: 4597
	[SerializeField]
	public Transform spitSpawnPoint;

	// Token: 0x040011F6 RID: 4598
	[SerializeField]
	public DicePalaceCigarLevelCigaretteGhost ghostPrefab;

	// Token: 0x040011F7 RID: 4599
	[SerializeField]
	public Transform ghostSpawnPoint;

	// Token: 0x040011F8 RID: 4600
	[SerializeField]
	public float ghostOffset;

	// Token: 0x040011F9 RID: 4601
	public bool isVisible;

	// Token: 0x040011FA RID: 4602
	public bool onRightSpawn;

	// Token: 0x040011FB RID: 4603
	public bool isFiring;

	// Token: 0x040011FC RID: 4604
	public bool facingBack;

	// Token: 0x040011FD RID: 4605
	public int spitAttackCountIndex;

	// Token: 0x040011FE RID: 4606
	public int spitAttackDirectionIndex;

	// Token: 0x040011FF RID: 4607
	public int ghostAttackDelayIndex;

	// Token: 0x04001200 RID: 4608
	public int ghostSpawnPositionIndex;

	// Token: 0x04001201 RID: 4609
	public int counter;

	// Token: 0x04001202 RID: 4610
	public int maxCounter;

	// Token: 0x04001203 RID: 4611
	public DamageReceiver damageReceiver;

	// Token: 0x04001204 RID: 4612
	public DamageDealer damageDealer;
}
