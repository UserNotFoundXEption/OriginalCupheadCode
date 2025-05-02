using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000268 RID: 616
public class FlyingGenieLevelGenie : LevelProperties.FlyingGenie.Entity
{
	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06001C11 RID: 7185 RVA: 0x00017C50 File Offset: 0x00015E50
	// (set) Token: 0x06001C12 RID: 7186 RVA: 0x00017C58 File Offset: 0x00015E58
	public FlyingGenieLevelGenie.State state { get; set; }

	// Token: 0x06001C13 RID: 7187 RVA: 0x000AD5D0 File Offset: 0x000AB7D0
	public override void Awake()
	{
		base.Awake();
		this.defaultColor = base.GetComponent<SpriteRenderer>().color;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001C14 RID: 7188 RVA: 0x000AD624 File Offset: 0x000AB824
	public void Start()
	{
		base.StartCoroutine(this.intro_cr());
		this.casketStartPos = this.casket.transform.position;
		base.GetComponent<Collider2D>().enabled = false;
		this.hiero.speed = base.properties.CurrentState.obelisk.obeliskMovementSpeed;
		this.brick.speed = base.properties.CurrentState.obelisk.obeliskMovementSpeed;
	}

	// Token: 0x06001C15 RID: 7189 RVA: 0x000AD6A0 File Offset: 0x000AB8A0
	public override void LevelInit(LevelProperties.FlyingGenie properties)
	{
		base.LevelInit(properties);
		this.treasureAttacks = new List<int>
		{
			0,
			1,
			2
		};
		this.swordPinkPattern = properties.CurrentState.swords.swordPinkString.Split(new char[]
		{
			','
		});
		this.swordPinkIndex = Random.Range(0, this.swordPinkPattern.Length);
		this.gemPinkPattern = properties.CurrentState.gems.gemPinkString.Split(new char[]
		{
			','
		});
		this.gemPinkIndex = Random.Range(0, this.gemPinkPattern.Length);
		this.sphinxPinkPattern = properties.CurrentState.sphinx.scarabPinkString.Split(new char[]
		{
			','
		});
		this.sphinxPinkIndex = Random.Range(0, this.sphinxPinkPattern.Length);
	}

	// Token: 0x06001C16 RID: 7190 RVA: 0x00017C61 File Offset: 0x00015E61
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001C17 RID: 7191 RVA: 0x00017C74 File Offset: 0x00015E74
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001C18 RID: 7192 RVA: 0x00017C92 File Offset: 0x00015E92
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001C19 RID: 7193 RVA: 0x000AD784 File Offset: 0x000AB984
	public IEnumerator intro_cr()
	{
		this.state = FlyingGenieLevelGenie.State.Intro;
		yield return CupheadTime.WaitForSeconds(this, 1.3f);
		base.animator.SetTrigger("Continue");
		this.GenieIntroSFX();
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.main.introHesitate);
		yield return base.animator.WaitForAnimationToEnd(this, "Intro_End", false, true);
		this.state = FlyingGenieLevelGenie.State.Idle;
		this.StartTreasure();
		base.StartCoroutine(this.skull_attack_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001C1A RID: 7194 RVA: 0x00017CAA File Offset: 0x00015EAA
	public void SpawnPuff()
	{
		base.StartCoroutine(this.handle_puff_cr(this.puffEffect.Create(this.puffRoot.position)));
	}

	// Token: 0x06001C1B RID: 7195 RVA: 0x00017CCF File Offset: 0x00015ECF
	public void StartCarpet()
	{
		base.animator.Play("Idle_Carpet");
		base.StartCoroutine(this.handle_carpet_fadein());
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x00017CEE File Offset: 0x00015EEE
	public void EndCarpet()
	{
		base.StartCoroutine(this.handle_carpet_fadeout());
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x000AD7A0 File Offset: 0x000AB9A0
	public IEnumerator handle_puff_cr(Effect puff)
	{
		yield return puff.animator.WaitForAnimationToEnd(this, "Start", false, true);
		SpriteRenderer puffRenderer = puff.GetComponent<SpriteRenderer>();
		while (puff.transform.position.x > -740f)
		{
			puff.transform.position -= Vector3.right * 200f * CupheadTime.Delta;
			if (puff.transform.position.x < -540f)
			{
				Color color = puffRenderer.color;
				color.a -= 1f * CupheadTime.Delta;
				puffRenderer.color = color;
			}
			yield return null;
		}
		Object.Destroy(puff.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06001C1E RID: 7198 RVA: 0x000AD7C4 File Offset: 0x000AB9C4
	public IEnumerator handle_carpet_fadein()
	{
		this.carpet.color = new Color(1f, 1f, 1f, 0f);
		float t = 0f;
		float time = 2f;
		while (t < time)
		{
			this.carpet.color = new Color(1f, 1f, 1f, t / time);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.carpet.color = new Color(1f, 1f, 1f, 1f);
		yield return null;
		yield break;
	}

	// Token: 0x06001C1F RID: 7199 RVA: 0x000AD7E0 File Offset: 0x000AB9E0
	public IEnumerator handle_carpet_fadeout()
	{
		this.carpet.color = new Color(1f, 1f, 1f, 1f);
		float t = 0f;
		float time = 2f;
		while (t < time)
		{
			this.carpet.color = new Color(1f, 1f, 1f, 1f - t / time);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.carpet.color = new Color(1f, 1f, 1f, 0f);
		yield return null;
		yield break;
	}

	// Token: 0x06001C20 RID: 7200 RVA: 0x00017CFD File Offset: 0x00015EFD
	public void HitTrigger()
	{
		this.attackLooping = false;
	}

	// Token: 0x06001C21 RID: 7201 RVA: 0x000AD7FC File Offset: 0x000AB9FC
	public void StartTreasure()
	{
		this.state = FlyingGenieLevelGenie.State.Treasure;
		this.skullCounter = 0;
		base.animator.SetBool("OnTreasure", true);
		this.attackLooping = true;
		int num = Random.Range(0, this.treasureAttacks.Count);
		this.treasureCounter = this.treasureAttacks[num];
		switch (this.treasureCounter)
		{
		case 0:
			this.StartSwords();
			break;
		case 1:
			this.StartGems();
			break;
		case 2:
			this.StartSphinx();
			break;
		default:
			Debug.LogError("The counter is messed up: " + this.treasureCounter, null);
			break;
		}
		this.treasureAttacks.Remove(num);
	}

	// Token: 0x06001C22 RID: 7202 RVA: 0x000AD8C0 File Offset: 0x000ABAC0
	public IEnumerator skull_attack_cr()
	{
		for (;;)
		{
			if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Chest_Idle") && this.skullCounter < base.properties.CurrentState.skull.skullCount)
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.skull.skullDelayRange);
				base.animator.SetTrigger("OnSkull");
				AudioManager.Play("genie_skull_release");
				this.emitAudioFromObject.Add("genie_skull_release");
				yield return base.animator.WaitForAnimationToEnd(this, "Chest_Skull_Attack", false, true);
				this.skullCounter++;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001C23 RID: 7203 RVA: 0x000AD8DC File Offset: 0x000ABADC
	public void SpawnSkull()
	{
		this.skullPrefab.Create(this.skullRoot.transform.position, 0f, -base.properties.CurrentState.skull.skullSpeed);
		AudioManager.Play("genie_skull_release_projectile");
		this.emitAudioFromObject.Add("genie_skull_release_projectile");
	}

	// Token: 0x06001C24 RID: 7204 RVA: 0x00017D06 File Offset: 0x00015F06
	public void DisableCarpet()
	{
		base.animator.Play("Off");
	}

	// Token: 0x06001C25 RID: 7205 RVA: 0x00017D18 File Offset: 0x00015F18
	public void EnableCarpet()
	{
		base.animator.Play("Idle_Carpet");
	}

	// Token: 0x06001C26 RID: 7206 RVA: 0x00017D2A File Offset: 0x00015F2A
	public void EnableChestIdle()
	{
		base.animator.Play("Idle_Carpet_Chest");
	}

	// Token: 0x06001C27 RID: 7207 RVA: 0x00017D3C File Offset: 0x00015F3C
	public void StartSwords()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.swords_cr());
	}

	// Token: 0x06001C28 RID: 7208 RVA: 0x000AD940 File Offset: 0x000ABB40
	public IEnumerator swords_cr()
	{
		this.attackLooping = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Chest_Intro", false, true);
		LevelProperties.FlyingGenie.Swords p = base.properties.CurrentState.swords;
		int positionIndex = Random.Range(0, p.patternPositionStrings.Length);
		string[] positionPattern = p.patternPositionStrings[positionIndex].Split(new char[]
		{
			','
		});
		Vector3 endPosition = Vector3.zero;
		float location = 0f;
		while (this.attackLooping)
		{
			positionPattern = p.patternPositionStrings[positionIndex].Split(new char[]
			{
				','
			});
			for (int i = 0; i < positionPattern.Length; i++)
			{
				string[] coordinates = positionPattern[i].Split(new char[]
				{
					'-'
				});
				for (int j = 0; j < coordinates.Length; j++)
				{
					Parser.FloatTryParse(coordinates[j], out location);
					if (j % 2 == 0)
					{
						endPosition.x = -640f + location;
					}
					else
					{
						endPosition.y = 360f - location;
					}
				}
				this.SpawnSwords(endPosition);
				if (!this.attackLooping)
				{
					break;
				}
				yield return CupheadTime.WaitForSeconds(this, p.spawnDelay);
			}
			if (!this.attackLooping)
			{
				break;
			}
			yield return CupheadTime.WaitForSeconds(this, p.repeatDelay);
			positionIndex = (positionIndex + 1) % p.patternPositionStrings.Length;
			yield return null;
		}
		base.animator.SetBool("OnTreasure", false);
		yield return base.animator.WaitForAnimationToEnd(this, "Chest_Outro", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = FlyingGenieLevelGenie.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001C29 RID: 7209 RVA: 0x000AD95C File Offset: 0x000ABB5C
	public void SpawnSwords(Vector3 pos)
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		FlyingGenieLevelSword flyingGenieLevelSword = Object.Instantiate<FlyingGenieLevelSword>(this.swordPrefab);
		flyingGenieLevelSword.Init(this.treasureRoot.position, pos, base.properties.CurrentState.swords, next);
		flyingGenieLevelSword.SetParryable(this.swordPinkPattern[this.swordPinkIndex][0] == 'P');
		this.swordPinkIndex = (this.swordPinkIndex + 1) % this.swordPinkPattern.Length;
	}

	// Token: 0x06001C2A RID: 7210 RVA: 0x00017D67 File Offset: 0x00015F67
	public void StartGems()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.gems_cr());
	}

	// Token: 0x06001C2B RID: 7211 RVA: 0x000AD9D4 File Offset: 0x000ABBD4
	public IEnumerator gems_cr()
	{
		this.attackLooping = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Chest_Intro", false, true);
		AudioManager.Play("genie_chest_jewel_escape");
		this.emitAudioFromObject.Add("genie_chest_jewel_escape");
		AudioManager.PlayLoop("genie_chest_magic_loop");
		this.emitAudioFromObject.Add("genie_chest_magic_loop");
		while (this.attackLooping)
		{
			this.smallGemTimerUp = false;
			this.bigGemTimerUp = false;
			if (this.bigGemsRoutine != null)
			{
				base.StopCoroutine(this.bigGemsRoutine);
			}
			this.bigGemsRoutine = base.StartCoroutine(this.big_gems_cr());
			if (this.smallGemsRoutine != null)
			{
				base.StopCoroutine(this.smallGemsRoutine);
			}
			this.smallGemsRoutine = base.StartCoroutine(this.small_gems_cr());
			while (!this.smallGemTimerUp && !this.bigGemTimerUp)
			{
				if (!this.attackLooping)
				{
					break;
				}
				yield return null;
			}
			if (this.attackLooping)
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.gems.repeatDelay);
			}
			yield return null;
		}
		base.animator.SetBool("OnTreasure", false);
		yield return base.animator.WaitForAnimationToStart(this, "Chest_Outro", false);
		AudioManager.Stop("genie_chest_magic_loop");
		AudioManager.Play("genie_chest_magic_loop_end");
		this.emitAudioFromObject.Add("genie_chest_magic_loop_end");
		yield return base.animator.WaitForAnimationToEnd(this, "Chest_Outro", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.gems.hesitate);
		this.state = FlyingGenieLevelGenie.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001C2C RID: 7212 RVA: 0x000AD9F0 File Offset: 0x000ABBF0
	public IEnumerator small_gems_cr()
	{
		LevelProperties.FlyingGenie.Gems p = base.properties.CurrentState.gems;
		this.smallGemTimerUp = false;
		int mainOffsetIndex = Random.Range(0, p.gemSmallAimOffset.Length);
		string[] smallOffsetString = p.gemSmallAimOffset[mainOffsetIndex].Split(new char[]
		{
			','
		});
		int offsetIndex = Random.Range(0, smallOffsetString.Length);
		float offset = 0f;
		base.StartCoroutine(this.small_gem_timer_cr());
		while (!this.smallGemTimerUp && this.attackLooping)
		{
			smallOffsetString = p.gemSmallAimOffset[mainOffsetIndex].Split(new char[]
			{
				','
			});
			Parser.FloatTryParse(smallOffsetString[offsetIndex], out offset);
			AbstractPlayerController player = PlayerManager.GetNext();
			this.gemPrefab.Create(this.treasureRoot.position, player, offset, p.gemSmallSpeed, this.gemPinkPattern[this.gemPinkIndex][0] == 'P', false);
			this.gemPinkIndex = (this.gemPinkIndex + 1) % this.gemPinkPattern.Length;
			yield return CupheadTime.WaitForSeconds(this, p.gemSmallDelayRange.RandomFloat());
			if (offsetIndex < smallOffsetString.Length - 1)
			{
				offsetIndex++;
			}
			else
			{
				mainOffsetIndex = (mainOffsetIndex + 1) % p.gemSmallAimOffset.Length;
				offsetIndex = 0;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001C2D RID: 7213 RVA: 0x000ADA0C File Offset: 0x000ABC0C
	public IEnumerator small_gem_timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.gems.gemSmallAttackDuration);
		this.smallGemTimerUp = true;
		yield break;
	}

	// Token: 0x06001C2E RID: 7214 RVA: 0x000ADA28 File Offset: 0x000ABC28
	public IEnumerator big_gems_cr()
	{
		LevelProperties.FlyingGenie.Gems p = base.properties.CurrentState.gems;
		this.bigGemTimerUp = false;
		int mainOffsetIndex = Random.Range(0, p.gemBigAimOffset.Length);
		string[] bigOffsetString = p.gemBigAimOffset[mainOffsetIndex].Split(new char[]
		{
			','
		});
		int offsetIndex = Random.Range(0, bigOffsetString.Length);
		float offset = 0f;
		base.StartCoroutine(this.big_gems_timer_cr());
		while (!this.bigGemTimerUp && this.attackLooping)
		{
			Parser.FloatTryParse(bigOffsetString[offsetIndex], out offset);
			AbstractPlayerController player = PlayerManager.GetNext();
			this.gemPrefab.Create(this.treasureRoot.position, player, offset, p.gemBigSpeed, false, true);
			yield return CupheadTime.WaitForSeconds(this, p.gemBigDelayRange.RandomFloat());
			if (offsetIndex < bigOffsetString.Length - 1)
			{
				offsetIndex++;
			}
			else
			{
				mainOffsetIndex = (mainOffsetIndex + 1) % p.gemBigAimOffset.Length;
				offsetIndex = 0;
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001C2F RID: 7215 RVA: 0x000ADA44 File Offset: 0x000ABC44
	public IEnumerator big_gems_timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.gems.gemBigAttackDuration);
		this.bigGemTimerUp = true;
		yield break;
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x00017D92 File Offset: 0x00015F92
	public void StartSphinx()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.sphinx_cr());
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x000ADA60 File Offset: 0x000ABC60
	public IEnumerator sphinx_cr()
	{
		this.attackLooping = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Chest_Intro", false, true);
		AudioManager.Play("genie_chest_jewel_escape");
		this.emitAudioFromObject.Add("genie_chest_jewel_escape");
		AudioManager.PlayLoop("genie_chest_magic_loop_nojingle");
		this.emitAudioFromObject.Add("genie_chest_magic_loop_nojingle");
		LevelProperties.FlyingGenie.Sphinx p = base.properties.CurrentState.sphinx;
		int mainCountIndex = Random.Range(0, p.sphinxCount.Length);
		string[] sphinxCountPattern = p.sphinxCount[mainCountIndex].Split(new char[]
		{
			','
		});
		int countIndex = Random.Range(0, sphinxCountPattern.Length);
		int mainXIndex = Random.Range(0, p.sphinxAimX.Length);
		string[] sphinxPosXPattern = p.sphinxAimX[mainXIndex].Split(new char[]
		{
			','
		});
		int posXIndex = Random.Range(0, sphinxPosXPattern.Length);
		int mainYIndex = Random.Range(0, p.sphinxAimY.Length);
		string[] sphinxPosYPattern = p.sphinxAimY[mainYIndex].Split(new char[]
		{
			','
		});
		int posYIndex = Random.Range(0, sphinxPosYPattern.Length);
		float sphinxCount = 0f;
		while (this.attackLooping)
		{
			sphinxCountPattern = p.sphinxCount[mainCountIndex].Split(new char[]
			{
				','
			});
			sphinxPosXPattern = p.sphinxAimX[mainXIndex].Split(new char[]
			{
				','
			});
			sphinxPosYPattern = p.sphinxAimY[mainYIndex].Split(new char[]
			{
				','
			});
			Parser.FloatTryParse(sphinxCountPattern[countIndex], out sphinxCount);
			int i = 0;
			while ((float)i < sphinxCount)
			{
				this.SpawnSphinx();
				yield return CupheadTime.WaitForSeconds(this, p.sphinxMainDelay);
				if (posXIndex < p.sphinxAimX.Length - 1)
				{
					posXIndex++;
				}
				else
				{
					mainXIndex = (mainXIndex + 1) % p.sphinxAimX.Length;
					posXIndex = 0;
				}
				if (posYIndex < p.sphinxAimY.Length - 1)
				{
					posYIndex++;
				}
				else
				{
					mainYIndex = (mainYIndex + 1) % p.sphinxAimY.Length;
					posYIndex = 0;
				}
				if (!this.attackLooping)
				{
					break;
				}
				i++;
			}
			if (this.attackLooping)
			{
				yield return CupheadTime.WaitForSeconds(this, p.repeatDelay);
			}
			if (countIndex < p.sphinxCount.Length - 1)
			{
				countIndex++;
			}
			else
			{
				mainCountIndex = (mainCountIndex + 1) % p.sphinxCount.Length;
				countIndex = 0;
			}
		}
		base.animator.SetBool("OnTreasure", false);
		yield return base.animator.WaitForAnimationToStart(this, "Chest_Outro", false);
		AudioManager.Stop("genie_chest_magic_loop_nojingle");
		AudioManager.Play("genie_chest_magic_loop_nojingle_end");
		this.emitAudioFromObject.Add("genie_chest_magic_loop_nojingle_end");
		yield return base.animator.WaitForAnimationToEnd(this, "Chest_Outro", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = FlyingGenieLevelGenie.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x000ADA7C File Offset: 0x000ABC7C
	public void SpawnSphinx()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		FlyingGenieLevelSphinx flyingGenieLevelSphinx = Object.Instantiate<FlyingGenieLevelSphinx>(this.sphinxPrefab);
		flyingGenieLevelSphinx.Init(this.treasureRoot.position, base.properties.CurrentState.sphinx, next, this.sphinxPinkPattern, this.sphinxPinkIndex);
		this.sphinxPinkIndex = (this.sphinxPinkIndex + (int)base.properties.CurrentState.sphinx.sphinxSpawnNum) % this.sphinxPinkPattern.Length;
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x00017DBD File Offset: 0x00015FBD
	public void StartCoffin()
	{
		this.state = FlyingGenieLevelGenie.State.Coffin;
		base.animator.SetBool("OnDisappear", true);
		base.StartCoroutine(this.coffin_cr());
	}

	// Token: 0x06001C34 RID: 7220 RVA: 0x000ADAF8 File Offset: 0x000ABCF8
	public IEnumerator coffin_cr()
	{
		this.attackLooping = true;
		LevelProperties.FlyingGenie.Coffin p = base.properties.CurrentState.coffin;
		int mainPosIndex = Random.Range(0, p.mummyAppearString.Length);
		string[] coffinPosPattern = p.mummyAppearString[mainPosIndex].Split(new char[]
		{
			','
		});
		int posIndex = Random.Range(0, coffinPosPattern.Length);
		int mainAngleIndex = Random.Range(0, p.mummyGenieDirection.Length);
		string[] coffinAnglePattern = p.mummyGenieDirection[mainAngleIndex].Split(new char[]
		{
			','
		});
		int angleIndex = Random.Range(0, coffinAnglePattern.Length);
		int mainTypeIndex = Random.Range(0, p.mummyTypeString.Length);
		string[] coffinTypePattern = p.mummyTypeString[mainTypeIndex].Split(new char[]
		{
			','
		});
		int typeIndex = Random.Range(0, coffinTypePattern.Length);
		Vector3 pos = Vector3.zero;
		float position = 0f;
		float angle = 0f;
		int sortingOrder = 0;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		AudioManager.Play("genie_sarcophagus_enter");
		this.emitAudioFromObject.Add("genie_sarcophagus_enter");
		while (this.casket.transform.position.y > -30f)
		{
			this.casket.transform.AddPosition(0f, -800f * CupheadTime.Delta, 0f);
			yield return null;
		}
		CupheadLevelCamera.Current.Shake(10f, 0.4f, false);
		this.casket.GetComponent<Animator>().SetTrigger("StartCasket");
		yield return this.casket.GetComponent<Animator>().WaitForAnimationToEnd(this, "Open_Start", false, true);
		this.goop.ActivateGoop();
		yield return this.goop.GetComponent<Animator>().WaitForAnimationToEnd(this, "Intro", false, true);
		while (this.attackLooping && base.properties.CurrentState.stateName == LevelProperties.FlyingGenie.States.Disappear)
		{
			coffinPosPattern = p.mummyAppearString[mainPosIndex].Split(new char[]
			{
				','
			});
			coffinTypePattern = p.mummyTypeString[mainTypeIndex].Split(new char[]
			{
				','
			});
			coffinAnglePattern = p.mummyGenieDirection[mainAngleIndex].Split(new char[]
			{
				','
			});
			Parser.FloatTryParse(coffinPosPattern[posIndex], out position);
			Parser.FloatTryParse(coffinAnglePattern[angleIndex], out angle);
			pos = new Vector3(this.casket.transform.position.x + 200f, position, 0f);
			if (coffinTypePattern[typeIndex][0] == 'A')
			{
				this.mummyClassic.Create(pos, -p.mummyASpeed, -angle, p, FlyingGenieLevelMummy.MummyType.Classic, p.mummyGenieHP, sortingOrder);
			}
			else if (coffinTypePattern[typeIndex][0] == 'B')
			{
				this.mummyChomper.Create(pos, -p.mummyBSpeed, -angle, p, FlyingGenieLevelMummy.MummyType.Chomper, p.mummyGenieHP, sortingOrder);
			}
			else if (coffinTypePattern[typeIndex][0] == 'C')
			{
				this.mummyChaser.Create(pos, -p.mummyCSpeed, -angle, p, FlyingGenieLevelMummy.MummyType.Grabby, p.mummyGenieHP, sortingOrder);
			}
			yield return CupheadTime.WaitForSeconds(this, p.mummyGenieDelay);
			if (posIndex < coffinPosPattern.Length - 1)
			{
				posIndex++;
			}
			else
			{
				mainPosIndex = (mainPosIndex + 1) % p.mummyAppearString.Length;
				posIndex = 0;
			}
			if (typeIndex < coffinTypePattern.Length - 1)
			{
				typeIndex++;
			}
			else
			{
				mainTypeIndex = (mainTypeIndex + 1) % p.mummyTypeString.Length;
				typeIndex = 0;
			}
			if (angleIndex < coffinAnglePattern.Length - 1)
			{
				angleIndex++;
			}
			else
			{
				mainAngleIndex = (mainAngleIndex + 1) % p.mummyGenieDirection.Length;
				angleIndex = 0;
			}
			sortingOrder += 2;
		}
		this.goop.StartDeath();
		LevelBossDeathExploder explosion = this.casket.GetComponent<LevelBossDeathExploder>();
		explosion.StartExplosion();
		this.casket.GetComponent<Animator>().SetTrigger("OnClose");
		AudioManager.Play("genie_sarcophagus_exit");
		this.emitAudioFromObject.Add("genie_sarcophagus_exit");
		yield return this.casket.GetComponent<Animator>().WaitForAnimationToEnd(this, "Close", false, true);
		while (this.casket.transform.position.x < 1140f)
		{
			this.casket.transform.AddPosition(200f * CupheadTime.Delta, 0f, 0f);
			yield return null;
		}
		this.casket.transform.position = this.casketStartPos;
		this.casket.GetComponent<Animator>().SetTrigger("EndCasket");
		explosion.StopExplosions();
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.FadeIntoIdle();
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = FlyingGenieLevelGenie.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001C35 RID: 7221 RVA: 0x000ADB14 File Offset: 0x000ABD14
	public void StartObelisk()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		if (this.bigGemsRoutine != null)
		{
			base.StopCoroutine(this.bigGemsRoutine);
		}
		if (this.smallGemsRoutine != null)
		{
			base.StopCoroutine(this.smallGemsRoutine);
		}
		this.state = FlyingGenieLevelGenie.State.Disappear;
		base.animator.SetBool("OnDisappear", true);
		base.StartCoroutine(this.obelisk_cr());
		base.StartCoroutine(this.genie_laugh_sound_cr());
	}

	// Token: 0x06001C36 RID: 7222 RVA: 0x000ADB98 File Offset: 0x000ABD98
	public IEnumerator obelisk_cr()
	{
		LevelProperties.FlyingGenie.Obelisk p = base.properties.CurrentState.obelisk;
		this.attackLooping = true;
		Vector3 startPos = Vector3.zero;
		startPos.x = 1340f;
		startPos.y = 360f;
		float t = 0f;
		float time = 1f;
		float angle = 0f;
		bool firstPillar = true;
		this.obelisks = new List<FlyingGenieLevelObelisk>();
		int obelisksListIndex = 0;
		int obeliskPoolSize = 6;
		int obeliskCounter = 0;
		int mainObeliskIndex = Random.Range(0, p.obeliskGeniePos.Length);
		string[] blockOrderPattern = p.obeliskGeniePos[mainObeliskIndex].Split(new char[]
		{
			','
		});
		int obeliskIndex = Random.Range(0, blockOrderPattern.Length);
		int mainBouncerIndex = Random.Range(0, p.bouncerAngleString.Length);
		string[] bouncerPattern = p.bouncerAngleString[mainBouncerIndex].Split(new char[]
		{
			','
		});
		int bouncerIndex = Random.Range(0, bouncerPattern.Length);
		for (int i = 0; i < obeliskPoolSize; i++)
		{
			FlyingGenieLevelObelisk flyingGenieLevelObelisk = Object.Instantiate<FlyingGenieLevelObelisk>(this.obeliskPrefab);
			flyingGenieLevelObelisk.Init(startPos, p, this, i == 0);
			this.obelisks.Add(flyingGenieLevelObelisk);
		}
		yield return base.animator.WaitForAnimationToStart(this, "Genie_Meditate", false);
		this.sawMask.gameObject.SetActive(true);
		while (t < time)
		{
			Vector3 pos = this.hieroBG.position;
			Vector3 pos2 = this.brickBG.position;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInBounce, 0f, 1f, t / time);
			pos.y = Mathf.Lerp(this.hieroBG.position.y, 340f, val);
			pos2.y = Mathf.Lerp(this.brickBG.position.y, -320f, val);
			this.hieroBG.position = pos;
			this.brickBG.position = pos2;
			t += CupheadTime.Delta;
			yield return null;
		}
		while (obeliskCounter < p.obeliskCount)
		{
			Parser.FloatTryParse(bouncerPattern[bouncerIndex], out angle);
			string[] headLocations = blockOrderPattern[obeliskIndex].Split(new char[]
			{
				'-'
			});
			this.obelisks[obelisksListIndex].ActivateObelisk(headLocations);
			if (p.bounceShotOn)
			{
				if (!firstPillar)
				{
					int index;
					if (obelisksListIndex <= 0)
					{
						index = this.obelisks.Count - 1;
					}
					else
					{
						index = obelisksListIndex - 1;
					}
					this.SpawnBouncer(this.obelisks[obelisksListIndex], this.obelisks[index], angle);
				}
				else
				{
					float num = Vector3.Distance(this.obelisks[obelisksListIndex + 1].transform.position, base.transform.position);
					this.obelisks[obelisksListIndex].SetColliders((this.obelisks[obelisksListIndex + 1].transform.position.x + Mathf.Abs(num / 2f)) / 2f, base.transform.position.x - num / 2f);
					firstPillar = false;
				}
			}
			obelisksListIndex = (obelisksListIndex + 1) % this.obelisks.Count;
			yield return null;
			yield return CupheadTime.WaitForSeconds(this, p.obeliskAppearDelay);
			if (obeliskIndex < blockOrderPattern.Length - 1)
			{
				obeliskIndex++;
			}
			else
			{
				mainObeliskIndex = (mainObeliskIndex + 1) % p.obeliskGeniePos.Length;
				obeliskIndex = 0;
			}
			if (bouncerIndex < bouncerPattern.Length - 1)
			{
				bouncerIndex++;
			}
			else
			{
				mainBouncerIndex = (mainBouncerIndex + 1) % p.bouncerAngleString.Length;
				bouncerIndex = 0;
			}
			blockOrderPattern = p.obeliskGeniePos[mainObeliskIndex].Split(new char[]
			{
				','
			});
			bouncerPattern = p.bouncerAngleString[mainBouncerIndex].Split(new char[]
			{
				','
			});
			obeliskCounter++;
			yield return null;
		}
		foreach (FlyingGenieLevelObelisk obelisk in this.obelisks)
		{
			if (obelisk.isOn)
			{
				while (obelisk.transform.position.x > -640f)
				{
					yield return null;
				}
			}
		}
		AudioManager.Stop("genie_pillar_main_loop");
		AudioManager.Stop("genie_pillar_destructable_loop");
		this.sawMask.gameObject.SetActive(false);
		base.StartCoroutine(this.delete_obelisks_cr(this.obelisks));
		this.state = FlyingGenieLevelGenie.State.Idle;
		this.StartCoffin();
		yield return null;
		yield break;
	}

	// Token: 0x06001C37 RID: 7223 RVA: 0x000ADBB4 File Offset: 0x000ABDB4
	public IEnumerator delete_obelisks_cr(List<FlyingGenieLevelObelisk> obelisks)
	{
		float t = 0f;
		float time = 2f;
		foreach (FlyingGenieLevelObelisk obelisk in obelisks)
		{
			if (obelisk.isOn)
			{
				while (obelisk.transform.position.x > -740f)
				{
					yield return null;
				}
			}
			Object.Destroy(obelisk.gameObject);
			yield return null;
		}
		while (t < time)
		{
			Vector3 pos = this.hieroBG.position;
			Vector3 pos2 = this.brickBG.position;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeOutBounce, 0f, 1f, t / time);
			pos.y = Mathf.Lerp(this.hieroBG.position.y, 460f, val);
			pos2.y = Mathf.Lerp(this.brickBG.position.y, -460f, val);
			this.hieroBG.position = pos;
			this.brickBG.position = pos2;
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001C38 RID: 7224 RVA: 0x000ADBD8 File Offset: 0x000ABDD8
	public void SpawnBouncer(FlyingGenieLevelObelisk currentObelisk, FlyingGenieLevelObelisk lastObelisk, float angle)
	{
		float num = Vector3.Distance(lastObelisk.transform.position, currentObelisk.transform.position);
		Vector3 position = lastObelisk.transform.position;
		position.x = currentObelisk.transform.position.x - num / 2f;
		float num2 = 150f;
		float num3 = 180f - (90f + angle / 2f);
		currentObelisk.SetColliders((lastObelisk.transform.position.x + Mathf.Abs(num / 2f)) / 2f, currentObelisk.transform.position.x - num / 2f);
		position.y = Random.Range((float)Level.Current.Ceiling - num2, (float)Level.Current.Ground + num2);
		FlyingGenieLevelBouncer flyingGenieLevelBouncer = Object.Instantiate<FlyingGenieLevelBouncer>(this.bouncerPrefab).Init(position, base.properties.CurrentState.obelisk, -num3);
		flyingGenieLevelBouncer.transform.parent = currentObelisk.transform;
	}

	// Token: 0x06001C39 RID: 7225 RVA: 0x00017DE4 File Offset: 0x00015FE4
	public void DoDamage(float damage)
	{
		base.properties.DealDamage(damage);
	}

	// Token: 0x06001C3A RID: 7226 RVA: 0x00017DF2 File Offset: 0x00015FF2
	public void FadeIntoIdle()
	{
		base.StartCoroutine(this.handle_fade_in_idle());
	}

	// Token: 0x06001C3B RID: 7227 RVA: 0x000ADCF4 File Offset: 0x000ABEF4
	public IEnumerator handle_fade_in_idle()
	{
		base.GetComponent<SpriteRenderer>().color = new Color(this.defaultColor.r, this.defaultColor.g, this.defaultColor.a, 0f);
		this.carpet.color = new Color(1f, 1f, 1f, 0f);
		float t = 0f;
		float time = 0.3f;
		base.animator.Play("To_Phase_2");
		while (t < time)
		{
			this.carpet.color = new Color(1f, 1f, 1f, t / time);
			base.GetComponent<SpriteRenderer>().color = new Color(this.defaultColor.r, this.defaultColor.g, this.defaultColor.a, t / time);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.GetComponent<SpriteRenderer>().color = new Color(this.defaultColor.r, this.defaultColor.g, this.defaultColor.a, 1f);
		this.carpet.color = new Color(1f, 1f, 1f, 1f);
		base.GetComponent<Collider2D>().enabled = true;
		yield return null;
		yield break;
	}

	// Token: 0x06001C3C RID: 7228 RVA: 0x00017E01 File Offset: 0x00016001
	public void StartPhase2()
	{
		base.StartCoroutine(this.start_phase_2_cr());
	}

	// Token: 0x06001C3D RID: 7229 RVA: 0x000ADD10 File Offset: 0x000ABF10
	public IEnumerator start_phase_2_cr()
	{
		this.genieTransformed.animator.Play("Genie_Head_Roll");
		yield return new WaitForEndOfFrame();
		this.genieTransformed.StartMarionette(base.transform.position, this.meditateP1, this.meditateP2);
		Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06001C3E RID: 7230 RVA: 0x000ADD2C File Offset: 0x000ABF2C
	public void CreateMeditateFX()
	{
		PlanePlayerController player = PlayerManager.GetPlayer<PlanePlayerController>(PlayerId.PlayerOne);
		PlanePlayerController player2 = PlayerManager.GetPlayer<PlanePlayerController>(PlayerId.PlayerTwo);
		if (player != null)
		{
			this.meditateP1 = Object.Instantiate<FlyingGenieLevelMeditateFX>(this.meditateEffect);
			this.meditateP1.transform.position = player.transform.position;
			this.meditateP1.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
			this.meditateP1.transform.parent = player.transform;
		}
		if (player2 != null)
		{
			this.meditateP2 = Object.Instantiate<FlyingGenieLevelMeditateFX>(this.meditateEffect);
			this.meditateP2.transform.position = player2.transform.position;
			this.meditateP2.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
			this.meditateP2.transform.parent = player2.transform;
		}
	}

	// Token: 0x06001C3F RID: 7231 RVA: 0x00017E10 File Offset: 0x00016010
	public void GenieIntroSFX()
	{
		AudioManager.Play("genie_entrance");
		this.emitAudioFromObject.Add("genie_entrance");
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x00017E2C File Offset: 0x0001602C
	public void SoundGenieVoiceIntro()
	{
		AudioManager.Play("genie_voice_intro_intake");
		this.emitAudioFromObject.Add("genie_voice_intro_intake");
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x00017E48 File Offset: 0x00016048
	public void SoundGenieVoiceEffort()
	{
		AudioManager.Play("genie_voice_effort");
		this.emitAudioFromObject.Add("genie_voice_effort");
	}

	// Token: 0x06001C42 RID: 7234 RVA: 0x00017E64 File Offset: 0x00016064
	public void SoundGenieVoiceLaugh()
	{
		AudioManager.Play("genie_voice_laugh");
		this.emitAudioFromObject.Add("genie_voice_laugh");
	}

	// Token: 0x06001C43 RID: 7235 RVA: 0x00017E80 File Offset: 0x00016080
	public void SoundGenieVoiceLure()
	{
		AudioManager.Play("genie_voice_lure");
		this.emitAudioFromObject.Add("genie_voice_lure");
	}

	// Token: 0x06001C44 RID: 7236 RVA: 0x00017E9C File Offset: 0x0001609C
	public void SoundGenieVoiceMeditate()
	{
		AudioManager.Play("genie_voice_meditate");
		this.emitAudioFromObject.Add("genie_voice_meditate");
	}

	// Token: 0x06001C45 RID: 7237 RVA: 0x00017EB8 File Offset: 0x000160B8
	public void SoundGenieChestOpen()
	{
		AudioManager.Play("genie_chest_attack_open");
		this.emitAudioFromObject.Add("genie_chest_attack_open");
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x000ADE2C File Offset: 0x000AC02C
	public IEnumerator genie_laugh_sound_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 6f);
		AudioManager.Play("genie_voice_laugh_reverb");
		yield break;
	}

	// Token: 0x06001C47 RID: 7239 RVA: 0x00017ED4 File Offset: 0x000160D4
	public void SoundGenieTeleportDisappear()
	{
		AudioManager.Play("genie_teleport_disappear");
		this.emitAudioFromObject.Add("genie_teleport_disappear");
	}

	// Token: 0x040016CC RID: 5836
	public const float MUMMY_SPAWN_OFFSET = 200f;

	// Token: 0x040016CE RID: 5838
	[SerializeField]
	public Transform hieroBG;

	// Token: 0x040016CF RID: 5839
	[SerializeField]
	public Transform brickBG;

	// Token: 0x040016D0 RID: 5840
	[SerializeField]
	public ScrollingSprite hiero;

	// Token: 0x040016D1 RID: 5841
	[SerializeField]
	public ScrollingSprite brick;

	// Token: 0x040016D2 RID: 5842
	[SerializeField]
	public Transform sawMask;

	// Token: 0x040016D3 RID: 5843
	[SerializeField]
	public GameObject casket;

	// Token: 0x040016D4 RID: 5844
	[SerializeField]
	public FlyingGenieLevelMeditateFX meditateEffect;

	// Token: 0x040016D5 RID: 5845
	[SerializeField]
	public BasicProjectile skullPrefab;

	// Token: 0x040016D6 RID: 5846
	[SerializeField]
	public FlyingGenieLevelBouncer bouncerPrefab;

	// Token: 0x040016D7 RID: 5847
	[SerializeField]
	public FlyingGenieLevelObelisk obeliskPrefab;

	// Token: 0x040016D8 RID: 5848
	[SerializeField]
	public FlyingGenieLevelSphinx sphinxPrefab;

	// Token: 0x040016D9 RID: 5849
	[Space(10f)]
	[SerializeField]
	public FlyingGenieLevelGem gemPrefab;

	// Token: 0x040016DA RID: 5850
	[Space(10f)]
	[SerializeField]
	public FlyingGenieLevelGoop goop;

	// Token: 0x040016DB RID: 5851
	[SerializeField]
	public FlyingGenieLevelMummy mummyClassic;

	// Token: 0x040016DC RID: 5852
	[SerializeField]
	public FlyingGenieLevelMummy mummyChomper;

	// Token: 0x040016DD RID: 5853
	[SerializeField]
	public FlyingGenieLevelMummy mummyChaser;

	// Token: 0x040016DE RID: 5854
	[SerializeField]
	public FlyingGenieLevelSword swordPrefab;

	// Token: 0x040016DF RID: 5855
	[SerializeField]
	public FlyingGenieLevelGenieTransform genieTransformed;

	// Token: 0x040016E0 RID: 5856
	[Space(10f)]
	[SerializeField]
	public Effect puffEffect;

	// Token: 0x040016E1 RID: 5857
	[SerializeField]
	public Transform puffRoot;

	// Token: 0x040016E2 RID: 5858
	[SerializeField]
	public Transform skullRoot;

	// Token: 0x040016E3 RID: 5859
	[SerializeField]
	public SpriteRenderer carpet;

	// Token: 0x040016E4 RID: 5860
	[SerializeField]
	public Transform morphRoot;

	// Token: 0x040016E5 RID: 5861
	[SerializeField]
	public Transform treasureRoot;

	// Token: 0x040016E6 RID: 5862
	public List<int> treasureAttacks;

	// Token: 0x040016E7 RID: 5863
	public List<FlyingGenieLevelObelisk> obelisks;

	// Token: 0x040016E8 RID: 5864
	public FlyingGenieLevelMeditateFX meditate;

	// Token: 0x040016E9 RID: 5865
	public DamageReceiver damageReceiver;

	// Token: 0x040016EA RID: 5866
	public DamageDealer damageDealer;

	// Token: 0x040016EB RID: 5867
	public bool attackLooping;

	// Token: 0x040016EC RID: 5868
	public bool smallGemTimerUp;

	// Token: 0x040016ED RID: 5869
	public bool bigGemTimerUp;

	// Token: 0x040016EE RID: 5870
	public int skullCounter;

	// Token: 0x040016EF RID: 5871
	public int treasureCounter;

	// Token: 0x040016F0 RID: 5872
	public Vector3 casketStartPos;

	// Token: 0x040016F1 RID: 5873
	public Coroutine patternCoroutine;

	// Token: 0x040016F2 RID: 5874
	public Coroutine smallGemsRoutine;

	// Token: 0x040016F3 RID: 5875
	public Coroutine bigGemsRoutine;

	// Token: 0x040016F4 RID: 5876
	public FlyingGenieLevelMeditateFX meditateP1;

	// Token: 0x040016F5 RID: 5877
	public FlyingGenieLevelMeditateFX meditateP2;

	// Token: 0x040016F6 RID: 5878
	public Color defaultColor;

	// Token: 0x040016F7 RID: 5879
	public string[] swordPinkPattern;

	// Token: 0x040016F8 RID: 5880
	public int swordPinkIndex;

	// Token: 0x040016F9 RID: 5881
	public string[] gemPinkPattern;

	// Token: 0x040016FA RID: 5882
	public int gemPinkIndex;

	// Token: 0x040016FB RID: 5883
	public string[] sphinxPinkPattern;

	// Token: 0x040016FC RID: 5884
	public int sphinxPinkIndex;

	// Token: 0x02000CEE RID: 3310
	public enum State
	{
		// Token: 0x04005DA2 RID: 23970
		Intro,
		// Token: 0x04005DA3 RID: 23971
		Idle,
		// Token: 0x04005DA4 RID: 23972
		Transform,
		// Token: 0x04005DA5 RID: 23973
		Treasure,
		// Token: 0x04005DA6 RID: 23974
		Disappear,
		// Token: 0x04005DA7 RID: 23975
		Dead,
		// Token: 0x04005DA8 RID: 23976
		Coffin
	}
}
