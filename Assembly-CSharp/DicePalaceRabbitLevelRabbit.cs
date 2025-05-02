using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000204 RID: 516
public class DicePalaceRabbitLevelRabbit : LevelProperties.DicePalaceRabbit.Entity
{
	// Token: 0x17000286 RID: 646
	// (get) Token: 0x060017AB RID: 6059 RVA: 0x000142AE File Offset: 0x000124AE
	// (set) Token: 0x060017AC RID: 6060 RVA: 0x000142B6 File Offset: 0x000124B6
	public DicePalaceRabbitLevelRabbit.State state { get; set; }

	// Token: 0x060017AD RID: 6061 RVA: 0x000A247C File Offset: 0x000A067C
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.idle_voice_sfx_cr());
		base.StartCoroutine(this.idle_sfx_cr());
	}

	// Token: 0x060017AE RID: 6062 RVA: 0x000142BF File Offset: 0x000124BF
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060017AF RID: 6063 RVA: 0x000142D2 File Offset: 0x000124D2
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060017B0 RID: 6064 RVA: 0x000142EA File Offset: 0x000124EA
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060017B1 RID: 6065 RVA: 0x000A24D8 File Offset: 0x000A06D8
	public override void LevelInit(LevelProperties.DicePalaceRabbit properties)
	{
		base.LevelInit(properties);
		this.attacking = false;
		this.playerOneCircleIndex = Random.Range(0, properties.CurrentState.magicWand.safeZoneString.Split(new char[]
		{
			','
		}).Length);
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		zero.x = (float)Parser.IntParse(properties.CurrentState.general.platformOnePosition.Split(new char[]
		{
			','
		})[0]);
		zero.y = (float)Parser.IntParse(properties.CurrentState.general.platformOnePosition.Split(new char[]
		{
			','
		})[1]);
		this.platform1.transform.position = zero;
		this.platform1.YPositionUp = zero.y;
		zero2.x = (float)Parser.IntParse(properties.CurrentState.general.platformTwoPosition.Split(new char[]
		{
			','
		})[0]);
		zero2.y = (float)Parser.IntParse(properties.CurrentState.general.platformTwoPosition.Split(new char[]
		{
			','
		})[1]);
		this.platform2.transform.position = zero2;
		this.platform2.YPositionUp = zero2.y;
		this.isMagicParryTop = Rand.Bool();
		this.state = DicePalaceRabbitLevelRabbit.State.Idle;
		Level.Current.OnWinEvent += this.Death;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060017B2 RID: 6066 RVA: 0x000A2670 File Offset: 0x000A0870
	public IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro_Continue", false, true);
		base.animator.Play("Off");
		yield return null;
		yield break;
	}

	// Token: 0x060017B3 RID: 6067 RVA: 0x000A268C File Offset: 0x000A088C
	public IEnumerator idle_voice_sfx_cr()
	{
		MinMax delay = new MinMax(1f, 4f);
		for (;;)
		{
			while (!base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, delay);
			AudioManager.Play("dice_palace_rabbit_idle_vox");
			this.emitAudioFromObject.Add("dice_palace_rabbit_idle_vox");
			yield return null;
		}
		yield break;
	}

	// Token: 0x060017B4 RID: 6068 RVA: 0x000A26A8 File Offset: 0x000A08A8
	public IEnumerator idle_sfx_cr()
	{
		bool loopingIdle = false;
		for (;;)
		{
			if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			{
				if (!loopingIdle)
				{
				}
			}
			else if (loopingIdle)
			{
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060017B5 RID: 6069 RVA: 0x00014308 File Offset: 0x00012508
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.orbPrefab = null;
		this.magicPrefab = null;
		this.explosionPrefab = null;
	}

	// Token: 0x060017B6 RID: 6070 RVA: 0x00014325 File Offset: 0x00012525
	public void OnMagicWand()
	{
		base.StartCoroutine(this.magicwand_cr());
	}

	// Token: 0x060017B7 RID: 6071 RVA: 0x000A26C4 File Offset: 0x000A08C4
	public IEnumerator magicwand_cr()
	{
		this.attacking = true;
		this.state = DicePalaceRabbitLevelRabbit.State.MagicWand;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.magicWand.initialAttackDelay);
		AbstractPlayerController player = PlayerManager.GetNext();
		base.animator.SetTrigger("OnAttack");
		base.StartCoroutine(this.orbs_cr(player.id, Parser.IntParse(base.properties.CurrentState.magicWand.safeZoneString.Split(new char[]
		{
			','
		})[this.playerOneCircleIndex])));
		this.playerOneCircleIndex++;
		if (this.playerOneCircleIndex >= base.properties.CurrentState.magicWand.safeZoneString.Split(new char[]
		{
			','
		}).Length)
		{
			this.playerOneCircleIndex = 0;
		}
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.magicWand.attackDelayRange.RandomFloat());
		this.attacking = false;
		base.animator.SetTrigger("OnAttackEnd");
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.magicWand.hesitate);
		this.state = DicePalaceRabbitLevelRabbit.State.Idle;
		yield break;
	}

	// Token: 0x060017B8 RID: 6072 RVA: 0x000A26E0 File Offset: 0x000A08E0
	public IEnumerator orbs_cr(PlayerId target, int safeZone)
	{
		GameObject centerPoint = new GameObject();
		AbstractPlayerController player = PlayerManager.GetPlayer(target);
		centerPoint.transform.position = player.center;
		this.currentCenterPoint = centerPoint;
		Vector3 dir = Vector3.up;
		float dist = base.properties.CurrentState.magicWand.circleDiameter / 2f;
		safeZone = this.GetSafeZone(safeZone);
		Transform[] orbs = new Transform[7];
		int orbsIndex = 0;
		float initialRotation = (float)Random.Range(0, 350);
		for (int i = 0; i < 8; i++)
		{
			if (i != safeZone)
			{
				DicePalaceRabbitLevelOrb dicePalaceRabbitLevelOrb = this.orbPrefab.Create(player.center + dir * dist, 0f, Vector2.one) as DicePalaceRabbitLevelOrb;
				dicePalaceRabbitLevelOrb.transform.parent = centerPoint.transform;
				dicePalaceRabbitLevelOrb.transform.Rotate(Vector3.forward, -initialRotation);
				dicePalaceRabbitLevelOrb.SetAsGold(i % 2 == 1);
				Color color = dicePalaceRabbitLevelOrb.GetComponent<SpriteRenderer>().color;
				color.a = 0.2f;
				dicePalaceRabbitLevelOrb.GetComponent<SpriteRenderer>().color = color;
				orbs[orbsIndex] = dicePalaceRabbitLevelOrb.transform;
				orbsIndex++;
			}
			dir = Quaternion.AngleAxis(-45f, Vector3.forward) * dir;
		}
		centerPoint.transform.Rotate(Vector3.forward, initialRotation);
		while (this.attacking)
		{
			if (player != null && !player.IsDead)
			{
				centerPoint.transform.position = player.center;
			}
			centerPoint.transform.Rotate(Vector3.forward * CupheadTime.FixedDelta, -base.properties.CurrentState.magicWand.spinningSpeed * CupheadTime.FixedDelta);
			for (int j = 0; j < orbs.Length; j++)
			{
				SpriteRenderer component = orbs[j].GetComponent<SpriteRenderer>();
				Color color2 = component.color;
				color2.a += CupheadTime.Delta / 2f;
				component.color = color2;
				if (color2.a >= 1f)
				{
					orbs[j].GetComponent<Collider2D>().enabled = true;
				}
				orbs[j].Rotate(Vector3.forward * CupheadTime.FixedDelta, base.properties.CurrentState.magicWand.spinningSpeed * CupheadTime.FixedDelta);
			}
			yield return new WaitForFixedUpdate();
		}
		for (int k = 0; k < orbs.Length; k++)
		{
			orbs[k].GetComponent<Collider2D>().enabled = true;
		}
		while (Vector3.Angle(Vector3.up, centerPoint.transform.up) > 5f)
		{
			if (player != null && !player.IsDead)
			{
				centerPoint.transform.position = player.center;
			}
			centerPoint.transform.Rotate(Vector3.forward * CupheadTime.FixedDelta, -base.properties.CurrentState.magicWand.spinningSpeed * CupheadTime.FixedDelta);
			for (int l = 0; l < orbs.Length; l++)
			{
				orbs[l].Rotate(Vector3.forward * CupheadTime.FixedDelta, base.properties.CurrentState.magicWand.spinningSpeed * CupheadTime.FixedDelta);
			}
			yield return new WaitForFixedUpdate();
		}
		centerPoint.transform.up = Vector3.up;
		base.StartCoroutine(this.collapse_cr(centerPoint));
		yield break;
	}

	// Token: 0x060017B9 RID: 6073 RVA: 0x000A270C File Offset: 0x000A090C
	public IEnumerator collapse_cr(GameObject centerPoint)
	{
		float dist = base.properties.CurrentState.magicWand.circleDiameter / 2f;
		float explodeDist = base.properties.CurrentState.magicWand.circleDiameter * 0.1f;
		while (dist >= explodeDist)
		{
			for (int i = 0; i < 7; i++)
			{
				Vector3 vector = (centerPoint.transform.GetChild(i).position - centerPoint.transform.position).normalized * dist;
				centerPoint.transform.GetChild(i).position = centerPoint.transform.position + vector;
			}
			dist -= base.properties.CurrentState.magicWand.bulletSpeed * CupheadTime.Delta;
			yield return null;
		}
		AudioManager.Play("projectile_explo");
		this.explosionPrefab.Create(centerPoint.transform.position);
		this.currentCenterPoint = null;
		Object.Destroy(centerPoint);
		yield break;
	}

	// Token: 0x060017BA RID: 6074 RVA: 0x000A2730 File Offset: 0x000A0930
	public int GetSafeZone(int index)
	{
		int result = 0;
		switch (index)
		{
		case 1:
			result = 5;
			break;
		case 2:
			result = 4;
			break;
		case 3:
			result = 3;
			break;
		case 4:
			result = 6;
			break;
		case 6:
			result = 2;
			break;
		case 7:
			result = 7;
			break;
		case 8:
			result = 0;
			break;
		case 9:
			result = 1;
			break;
		}
		return result;
	}

	// Token: 0x060017BB RID: 6075 RVA: 0x000A27B0 File Offset: 0x000A09B0
	public IEnumerator kill_orbs_cr()
	{
		float t = 0f;
		float time = 1f;
		float speed = 2500f;
		float[] angles = new float[7];
		for (int i = 0; i < 7; i++)
		{
			this.currentCenterPoint.transform.GetChild(i).GetComponent<Collider2D>().enabled = false;
			angles[i] = (float)Random.Range(0, 360);
		}
		while (t < time)
		{
			t += CupheadTime.Delta;
			for (int j = 0; j < 7; j++)
			{
				this.currentCenterPoint.transform.GetChild(j).position += MathUtils.AngleToDirection(angles[j]) * speed * CupheadTime.FixedDelta;
			}
			yield return null;
		}
		Object.Destroy(this.currentCenterPoint);
		yield return null;
		yield break;
	}

	// Token: 0x060017BC RID: 6076 RVA: 0x00014334 File Offset: 0x00012534
	public void OnMagicParry()
	{
		base.StartCoroutine(this.magicparry_cr());
	}

	// Token: 0x060017BD RID: 6077 RVA: 0x000A27CC File Offset: 0x000A09CC
	public IEnumerator magicparry_cr()
	{
		this.attacking = true;
		this.state = DicePalaceRabbitLevelRabbit.State.MagicParry;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.magicParry.initialAttackDelay);
		base.animator.SetTrigger("OnAttack");
		string[] positionsSplits = base.properties.CurrentState.magicParry.magicPositions.Split(new char[]
		{
			'-'
		});
		DicePalaceRabbitLevelMagic[] magicOrbs = new DicePalaceRabbitLevelMagic[positionsSplits.Length];
		string[] parryPattern = base.properties.CurrentState.magicParry.pinkString.Split(new char[]
		{
			','
		});
		string[] parryIndexes = parryPattern[this.parryCurrentIndex].Split(new char[]
		{
			'-'
		});
		float yOffset = base.properties.CurrentState.magicParry.yOffset;
		float posY = (!this.isMagicParryTop) ? (-360f + yOffset) : (360f - yOffset);
		int suit = 0;
		for (int i = 0; i < magicOrbs.Length; i++)
		{
			float num = 0f;
			Parser.FloatTryParse(positionsSplits[i], out num);
			num += -640f;
			magicOrbs[i] = (DicePalaceRabbitLevelMagic)this.magicPrefab.Create(new Vector3(num, posY));
			magicOrbs[i].IsOffset(i % 2 == 1);
			magicOrbs[i].AppearTime = base.properties.CurrentState.magicParry.attackDelayRange;
			bool flag = false;
			for (int j = 0; j < parryIndexes.Length; j++)
			{
				int num2 = 0;
				if (Parser.IntTryParse(parryIndexes[j], out num2) && num2 - 1 == i)
				{
					magicOrbs[i].SetParryable(true);
					flag = true;
				}
			}
			if (!flag)
			{
				magicOrbs[i].SetSuit(suit);
				suit = (suit + 1) % 3;
			}
		}
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.magicParry.attackDelayRange);
		for (int k = 0; k < magicOrbs.Length; k++)
		{
			magicOrbs[k].ActivateOrb();
			magicOrbs[k].Move(posY, this.isMagicParryTop, base.properties.CurrentState.magicParry.speed);
		}
		base.animator.SetTrigger("OnAttackEnd");
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.magicParry.hesitate);
		this.attacking = false;
		this.isMagicParryTop = !this.isMagicParryTop;
		this.parryCurrentIndex = (this.parryCurrentIndex + 1) % parryPattern.Length;
		this.state = DicePalaceRabbitLevelRabbit.State.Idle;
		yield break;
	}

	// Token: 0x060017BE RID: 6078 RVA: 0x00014343 File Offset: 0x00012543
	public void AttackSFX()
	{
		base.StartCoroutine(this.attack_sfx_cr());
	}

	// Token: 0x060017BF RID: 6079 RVA: 0x000A27E8 File Offset: 0x000A09E8
	public IEnumerator attack_sfx_cr()
	{
		yield return base.animator.WaitForAnimationToStart(this, "Attack", false);
		yield return base.animator.WaitForAnimationToStart(this, "Attack_End", false);
		yield return null;
		yield break;
	}

	// Token: 0x060017C0 RID: 6080 RVA: 0x000A2804 File Offset: 0x000A0A04
	public void Death()
	{
		AudioManager.Stop("dice_palace_rabbit_idle_loop");
		AudioManager.Stop("dice_palace_rabbit_attack_loop");
		this.SFX_StickTwirlStop();
		base.animator.SetTrigger("Death");
		this.StopAllCoroutines();
		if (this.currentCenterPoint != null)
		{
			base.StartCoroutine(this.kill_orbs_cr());
		}
		base.OnDestroy();
	}

	// Token: 0x060017C1 RID: 6081 RVA: 0x00014352 File Offset: 0x00012552
	public void SFX_IntroContinue()
	{
		AudioManager.Play("intro_continue");
		this.emitAudioFromObject.Add("intro_continue");
	}

	// Token: 0x060017C2 RID: 6082 RVA: 0x0001436E File Offset: 0x0001256E
	public void SFX_Death()
	{
		AudioManager.Play("dice_palace_rabbit_death");
		this.emitAudioFromObject.Add("dice_palace_rabbit_death");
	}

	// Token: 0x060017C3 RID: 6083 RVA: 0x0001438A File Offset: 0x0001258A
	public void SFX_AttackStart()
	{
		AudioManager.Play("dice_palace_rabbit_attack_start");
		this.emitAudioFromObject.Add("dice_palace_rabbit_attack_start");
	}

	// Token: 0x060017C4 RID: 6084 RVA: 0x000143A6 File Offset: 0x000125A6
	public void SFX_Attack()
	{
		if (!this.AttackSFXPlaying)
		{
			AudioManager.PlayLoop("dice_palace_rabbit_attack_loop");
			this.emitAudioFromObject.Add("dice_palace_rabbit_attack_loop");
			this.AttackSFXPlaying = true;
		}
	}

	// Token: 0x060017C5 RID: 6085 RVA: 0x000143D4 File Offset: 0x000125D4
	public void SFX_AttackEnd()
	{
		AudioManager.Stop("dice_palace_rabbit_attack_loop");
		AudioManager.Play("dice_palace_rabbit_attack_end");
		this.emitAudioFromObject.Add("dice_palace_rabbit_attack_end");
		this.AttackSFXPlaying = false;
	}

	// Token: 0x060017C6 RID: 6086 RVA: 0x00014401 File Offset: 0x00012601
	public void SFX_IdleRock()
	{
		AudioManager.Play("idle_rock");
		this.emitAudioFromObject.Add("idle_rock");
	}

	// Token: 0x060017C7 RID: 6087 RVA: 0x0001441D File Offset: 0x0001261D
	public void SFX_StickTwirl()
	{
		if (!this.StickTwirlActive)
		{
			this.StickTwirlActive = true;
			AudioManager.PlayLoop("stick_twirl");
			this.emitAudioFromObject.Add("stick_twirl");
		}
	}

	// Token: 0x060017C8 RID: 6088 RVA: 0x0001444B File Offset: 0x0001264B
	public void SFX_StickTwirlStop()
	{
		this.StickTwirlActive = false;
		AudioManager.Stop("stick_twirl");
	}

	// Token: 0x04001339 RID: 4921
	public const float OrbAppearTime = 2f;

	// Token: 0x0400133A RID: 4922
	[SerializeField]
	public AbstractProjectile orbPrefab;

	// Token: 0x0400133B RID: 4923
	[SerializeField]
	public DicePalaceRabbitLevelMagic magicPrefab;

	// Token: 0x0400133C RID: 4924
	[SerializeField]
	public FlowerLevelPlatform platform1;

	// Token: 0x0400133D RID: 4925
	[SerializeField]
	public FlowerLevelPlatform platform2;

	// Token: 0x0400133E RID: 4926
	[SerializeField]
	public Effect explosionPrefab;

	// Token: 0x0400133F RID: 4927
	public bool attacking;

	// Token: 0x04001340 RID: 4928
	public bool isDying;

	// Token: 0x04001341 RID: 4929
	public int playerOneCircleIndex;

	// Token: 0x04001342 RID: 4930
	public DamageDealer damageDealer;

	// Token: 0x04001343 RID: 4931
	public DamageReceiver damageReceiver;

	// Token: 0x04001344 RID: 4932
	public bool isMagicParryTop;

	// Token: 0x04001345 RID: 4933
	public int parryCurrentIndex;

	// Token: 0x04001346 RID: 4934
	public GameObject currentCenterPoint;

	// Token: 0x04001347 RID: 4935
	public bool AttackSFXPlaying;

	// Token: 0x04001348 RID: 4936
	public bool StickTwirlActive;

	// Token: 0x02000BD3 RID: 3027
	public enum State
	{
		// Token: 0x0400563D RID: 22077
		Idle,
		// Token: 0x0400563E RID: 22078
		MagicWand,
		// Token: 0x0400563F RID: 22079
		MagicParry
	}
}
