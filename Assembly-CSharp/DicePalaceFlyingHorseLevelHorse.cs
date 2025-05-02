using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001EA RID: 490
public class DicePalaceFlyingHorseLevelHorse : LevelProperties.DicePalaceFlyingHorse.Entity
{
	// Token: 0x17000278 RID: 632
	// (get) Token: 0x060016A0 RID: 5792 RVA: 0x0001340F File Offset: 0x0001160F
	// (set) Token: 0x060016A1 RID: 5793 RVA: 0x00013417 File Offset: 0x00011617
	public DicePalaceFlyingHorseLevelHorse.MiniHorseType miniHorseType { get; set; }

	// Token: 0x060016A2 RID: 5794 RVA: 0x00013420 File Offset: 0x00011620
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060016A3 RID: 5795 RVA: 0x0009F670 File Offset: 0x0009D870
	public override void LevelInit(LevelProperties.DicePalaceFlyingHorse properties)
	{
		base.LevelInit(properties);
		Level.Current.OnLevelStartEvent += this.StartAttacks;
		Level.Current.OnWinEvent += this.Death;
		this.giftPosXMainIndex = Random.Range(0, properties.CurrentState.giftBombs.giftPositionStringX.Length);
		this.giftPosYMainIndex = Random.Range(0, properties.CurrentState.giftBombs.giftPositionStringY.Length);
		this.giftPosXIndex = Random.Range(0, properties.CurrentState.giftBombs.giftPositionStringX[this.giftPosXMainIndex].Split(new char[]
		{
			','
		}).Length);
		this.giftPosYIndex = Random.Range(0, properties.CurrentState.giftBombs.giftPositionStringY[this.giftPosYMainIndex].Split(new char[]
		{
			','
		}).Length);
		this.playerAimMaxCounter = properties.CurrentState.giftBombs.playerAimRange.RandomInt();
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060016A4 RID: 5796 RVA: 0x00013456 File Offset: 0x00011656
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060016A5 RID: 5797 RVA: 0x00013469 File Offset: 0x00011669
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060016A6 RID: 5798 RVA: 0x00013487 File Offset: 0x00011687
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060016A7 RID: 5799 RVA: 0x0009F780 File Offset: 0x0009D980
	public IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		base.animator.SetTrigger("Continue");
		AudioManager.Play("dice_palace_flying_horse_intro");
		this.emitAudioFromObject.Add("dice_palace_flying_horse_intro");
		yield return null;
		yield break;
	}

	// Token: 0x060016A8 RID: 5800 RVA: 0x0001349F File Offset: 0x0001169F
	public void StartAttacks()
	{
		base.StartCoroutine(this.presents_cr());
		base.StartCoroutine(this.mini_horses_cr());
	}

	// Token: 0x060016A9 RID: 5801 RVA: 0x000134BB File Offset: 0x000116BB
	public void SpawnPresent()
	{
		base.StartCoroutine(this.spawn_present_cr());
	}

	// Token: 0x060016AA RID: 5802 RVA: 0x0009F79C File Offset: 0x0009D99C
	public IEnumerator spawn_present_cr()
	{
		LevelProperties.DicePalaceFlyingHorse.GiftBombs p = base.properties.CurrentState.giftBombs;
		float positionX = 0f;
		float positionY = 0f;
		Vector3 endPos = Vector3.zero;
		AbstractPlayerController player = PlayerManager.GetNext();
		string[] giftPositionXPattern = p.giftPositionStringX[this.giftPosXMainIndex].Split(new char[]
		{
			','
		});
		string[] giftPositionYPattern = p.giftPositionStringY[this.giftPosYMainIndex].Split(new char[]
		{
			','
		});
		if (this.playerAimCounter >= this.playerAimMaxCounter)
		{
			endPos = player.transform.position;
			player = PlayerManager.GetNext();
			this.playerAimMaxCounter = p.playerAimRange.RandomInt();
			this.playerAimCounter = 0;
		}
		else
		{
			Parser.FloatTryParse(giftPositionXPattern[this.giftPosXIndex], out positionX);
			Parser.FloatTryParse(giftPositionYPattern[this.giftPosYIndex], out positionY);
			endPos.x = -640f + positionX;
			endPos.y = 360f - positionY;
			this.playerAimCounter++;
		}
		DicePalaceFlyingHorseLevelPresent present = Object.Instantiate<DicePalaceFlyingHorseLevelPresent>(this.presentPrefab);
		present.Init(this.projectileRoot.position, endPos, base.properties.CurrentState.giftBombs);
		if (this.giftPosXIndex < giftPositionXPattern[this.giftPosXIndex].Length)
		{
			this.giftPosXIndex++;
		}
		else
		{
			this.giftPosXMainIndex = (this.giftPosXMainIndex + 1) % p.giftPositionStringX.Length;
			this.giftPosXIndex = 0;
		}
		if (this.giftPosYIndex < giftPositionYPattern[this.giftPosYIndex].Length)
		{
			this.giftPosYIndex++;
		}
		else
		{
			this.giftPosYMainIndex = (this.giftPosYMainIndex + 1) % p.giftPositionStringY.Length;
			this.giftPosYIndex = 0;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060016AB RID: 5803 RVA: 0x0009F7B8 File Offset: 0x0009D9B8
	public IEnumerator presents_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.giftBombs.giftDelay);
			base.animator.SetTrigger("OnAttack");
			yield return base.animator.WaitForAnimationToStart(this, "Attack", false);
			AudioManager.Play("dice_palace_flying_horse_attack");
			this.emitAudioFromObject.Add("dice_palace_flying_horse_attack");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060016AC RID: 5804 RVA: 0x000134CA File Offset: 0x000116CA
	public void AttackVox()
	{
		AudioManager.Play("dice_palace_horse_vox");
		this.emitAudioFromObject.Add("dice_palace_horse_vox");
	}

	// Token: 0x060016AD RID: 5805 RVA: 0x000134E6 File Offset: 0x000116E6
	public void TrotSFX()
	{
		AudioManager.Play("dice_horse_trot");
		this.emitAudioFromObject.Add("dice_horse_trot");
	}

	// Token: 0x060016AE RID: 5806 RVA: 0x00013502 File Offset: 0x00011702
	public void DieSFX()
	{
		AudioManager.Play("dice_horse_death");
		this.emitAudioFromObject.Add("dice_horse_death");
	}

	// Token: 0x060016AF RID: 5807 RVA: 0x0009F7D4 File Offset: 0x0009D9D4
	public void SpawnMiniHorses(Vector3 startPos, DicePalaceFlyingHorseLevelMiniHorse prefab, DicePalaceFlyingHorseLevelHorse.MiniHorseType type, bool isPink, float threeProx, int lane)
	{
		LevelProperties.DicePalaceFlyingHorse.MiniHorses miniHorses = base.properties.CurrentState.miniHorses;
		AbstractPlayerController next = PlayerManager.GetNext();
		DicePalaceFlyingHorseLevelMiniHorse dicePalaceFlyingHorseLevelMiniHorse = Object.Instantiate<DicePalaceFlyingHorseLevelMiniHorse>(prefab);
		Vector3 position;
		if (lane == 0)
		{
			position = this.topLineBackground.position;
		}
		else if (lane == 1)
		{
			position = this.middleLineBackground.position;
		}
		else
		{
			position = this.bottomLineBackground.position;
		}
		dicePalaceFlyingHorseLevelMiniHorse.Init(startPos, miniHorses.HP, base.properties.CurrentState.miniHorses, next, type, isPink, threeProx, lane, position);
	}

	// Token: 0x060016B0 RID: 5808 RVA: 0x0009F864 File Offset: 0x0009DA64
	public IEnumerator mini_horses_cr()
	{
		LevelProperties.DicePalaceFlyingHorse.MiniHorses p = base.properties.CurrentState.miniHorses;
		string[] typePattern = p.miniTypeString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] delayPattern = p.delayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] pinkPattern = p.miniTwoPinkString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] proxPattern = p.miniThreeProxString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int typeIndex = Random.Range(0, typePattern.Length);
		int delayIndex = Random.Range(0, delayPattern.Length);
		int pinkIndex = Random.Range(0, pinkPattern.Length);
		int proxIndex = Random.Range(0, proxPattern.Length);
		int type = 0;
		int trackCounter = 0;
		int pinkCounter = 0;
		int maxPink = 0;
		int threeProximity = 0;
		float delay = 0f;
		bool isPink = false;
		int lane = 0;
		Vector3 position = base.transform.position;
		DicePalaceFlyingHorseLevelMiniHorse prefab = null;
		DicePalaceFlyingHorseLevelHorse.MiniHorseType getType = DicePalaceFlyingHorseLevelHorse.MiniHorseType.One;
		position.x = (float)Level.Current.Right + 100f;
		for (;;)
		{
			for (int i = typeIndex; i < typePattern.Length; i++)
			{
				Parser.IntTryParse(typePattern[i], out type);
				Parser.FloatTryParse(delayPattern[delayIndex], out delay);
				trackCounter++;
				if (trackCounter <= 1)
				{
					position.y = this.topLine.position.y;
					lane = 0;
				}
				else if (trackCounter == 2)
				{
					position.y = this.middleLine.position.y;
					lane = 1;
				}
				else if (trackCounter >= 3)
				{
					position.y = this.bottomLine.position.y;
					trackCounter = 0;
					lane = 2;
				}
				if (type != 1)
				{
					if (type != 2)
					{
						if (type == 3)
						{
							prefab = this.miniHorse3Prefab;
							getType = DicePalaceFlyingHorseLevelHorse.MiniHorseType.Three;
							Parser.IntTryParse(proxPattern[proxIndex], out threeProximity);
							proxIndex = (proxIndex + 1) % proxPattern.Length;
						}
					}
					else
					{
						prefab = this.miniHorse2Prefab;
						getType = DicePalaceFlyingHorseLevelHorse.MiniHorseType.Two;
						if (pinkCounter == 0)
						{
							isPink = false;
							Parser.IntTryParse(pinkPattern[pinkIndex], out maxPink);
							pinkIndex %= pinkPattern.Length;
							pinkCounter++;
						}
						else if (pinkCounter >= maxPink)
						{
							isPink = true;
							pinkCounter = 0;
						}
						else
						{
							isPink = false;
							pinkCounter++;
						}
					}
				}
				else
				{
					prefab = this.miniHorse1Prefab;
					getType = DicePalaceFlyingHorseLevelHorse.MiniHorseType.One;
				}
				this.SpawnMiniHorses(position, prefab, getType, isPink, (float)threeProximity, lane);
				yield return CupheadTime.WaitForSeconds(this, delay);
				delayIndex = (delayIndex + 1) % delayPattern.Length;
				i %= typePattern.Length;
				typeIndex = 0;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060016B1 RID: 5809 RVA: 0x0009F880 File Offset: 0x0009DA80
	public void Death()
	{
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("OnDeath");
		AudioManager.PlayLoop("dice_palace_flying_horse_death_loop");
		this.emitAudioFromObject.Add("dice_palace_flying_horse_death_loop");
		this.DieSFX();
	}

	// Token: 0x0400125E RID: 4702
	[SerializeField]
	public Transform bottomLine;

	// Token: 0x0400125F RID: 4703
	[SerializeField]
	public Transform middleLine;

	// Token: 0x04001260 RID: 4704
	[SerializeField]
	public Transform topLine;

	// Token: 0x04001261 RID: 4705
	[SerializeField]
	public Transform bottomLineBackground;

	// Token: 0x04001262 RID: 4706
	[SerializeField]
	public Transform middleLineBackground;

	// Token: 0x04001263 RID: 4707
	[SerializeField]
	public Transform topLineBackground;

	// Token: 0x04001264 RID: 4708
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x04001265 RID: 4709
	[SerializeField]
	public DicePalaceFlyingHorseLevelMiniHorse miniHorse1Prefab;

	// Token: 0x04001266 RID: 4710
	[SerializeField]
	public DicePalaceFlyingHorseLevelMiniHorse miniHorse2Prefab;

	// Token: 0x04001267 RID: 4711
	[SerializeField]
	public DicePalaceFlyingHorseLevelMiniHorse miniHorse3Prefab;

	// Token: 0x04001268 RID: 4712
	[SerializeField]
	public DicePalaceFlyingHorseLevelPresent presentPrefab;

	// Token: 0x04001269 RID: 4713
	public DamageDealer damageDealer;

	// Token: 0x0400126A RID: 4714
	public DamageReceiver damageReceiver;

	// Token: 0x0400126B RID: 4715
	public int giftPosYMainIndex;

	// Token: 0x0400126C RID: 4716
	public int giftPosYIndex;

	// Token: 0x0400126D RID: 4717
	public int giftPosXMainIndex;

	// Token: 0x0400126E RID: 4718
	public int giftPosXIndex;

	// Token: 0x0400126F RID: 4719
	public int playerAimMaxCounter;

	// Token: 0x04001270 RID: 4720
	public int playerAimCounter;

	// Token: 0x02000B9B RID: 2971
	public enum MiniHorseType
	{
		// Token: 0x040054B7 RID: 21687
		One,
		// Token: 0x040054B8 RID: 21688
		Two,
		// Token: 0x040054B9 RID: 21689
		Three
	}
}
