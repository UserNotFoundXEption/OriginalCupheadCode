using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000210 RID: 528
public class DragonLevelLeftSideDragon : LevelProperties.Dragon.Entity
{
	// Token: 0x1700028B RID: 651
	// (get) Token: 0x0600182F RID: 6191 RVA: 0x00014B3C File Offset: 0x00012D3C
	// (set) Token: 0x06001830 RID: 6192 RVA: 0x00014B44 File Offset: 0x00012D44
	public DragonLevelLeftSideDragon.State state { get; set; }

	// Token: 0x06001831 RID: 6193 RVA: 0x000A3100 File Offset: 0x000A1300
	public override void Awake()
	{
		base.Awake();
		this.state = DragonLevelLeftSideDragon.State.UnSpawned;
		this.headPicked = DragonLevelLeftSideDragon.HeadPicked.None;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = this.damageBox.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.middleHead.GetComponent<Collider2D>().enabled = false;
		foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
		{
			collider2D.enabled = false;
		}
		this.damageReceiver.enabled = false;
		this.fire.SetColliderEnabled(false);
		this.xPos = base.transform.position.x;
		Vector3 position = base.transform.position;
		position.x = -10000f;
		base.transform.position = position;
	}

	// Token: 0x06001832 RID: 6194 RVA: 0x000A31E4 File Offset: 0x000A13E4
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.dead)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && this.state != DragonLevelLeftSideDragon.State.Dead)
		{
			this.state = DragonLevelLeftSideDragon.State.Dead;
			this.StartDeath();
		}
	}

	// Token: 0x06001833 RID: 6195 RVA: 0x00014B4D File Offset: 0x00012D4D
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001834 RID: 6196 RVA: 0x00014B65 File Offset: 0x00012D65
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001835 RID: 6197 RVA: 0x000A323C File Offset: 0x000A143C
	public override void LevelInit(LevelProperties.Dragon properties)
	{
		base.LevelInit(properties);
		this.potionTypeMainIndex = Random.Range(0, properties.CurrentState.potions.potionTypeString.Length);
		this.potionTypeIndex = Random.Range(0, properties.CurrentState.potions.potionTypeString[this.potionTypeMainIndex].Split(new char[]
		{
			','
		}).Length);
		this.shotPositionMainIndex = Random.Range(0, properties.CurrentState.potions.shotPositionString.Length);
		this.shotPositionIndex = Random.Range(0, properties.CurrentState.potions.shotPositionString[this.shotPositionMainIndex].Split(new char[]
		{
			','
		}).Length);
		this.attackCountMainIndex = Random.Range(0, properties.CurrentState.potions.attackCount.Length);
		this.attackCountIndex = Random.Range(0, properties.CurrentState.potions.attackCount[this.attackCountMainIndex].Split(new char[]
		{
			','
		}).Length);
		this.AttackFrames = 36 - (properties.CurrentState.blowtorch.warningDurationOne + properties.CurrentState.blowtorch.warningDurationTwo);
	}

	// Token: 0x06001836 RID: 6198 RVA: 0x00014B8E File Offset: 0x00012D8E
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.fireMarcherPrefabs = null;
		this.fireMarcherLeaderPrefab = null;
		this.bothPotionPrefab = null;
		this.horizontalPotionPrefab = null;
		this.verticalPotionPrefab = null;
	}

	// Token: 0x06001837 RID: 6199 RVA: 0x00014BB9 File Offset: 0x00012DB9
	public void StartIntro()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001838 RID: 6200 RVA: 0x000A3374 File Offset: 0x000A1574
	public IEnumerator intro_cr()
	{
		AudioManager.Play("level_dragon_left_dragon_intro");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_intro");
		yield return base.TweenPositionX(this.xPos, -350f, 1.3f, EaseUtils.EaseType.easeInSine);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", 0, false, true);
		foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
		{
			collider2D.enabled = true;
		}
		this.damageReceiver.enabled = true;
		base.StartCoroutine(this.fire_cr());
		this.StartFireMarchers();
		yield break;
	}

	// Token: 0x06001839 RID: 6201 RVA: 0x00014BC8 File Offset: 0x00012DC8
	public void TongueIntroSFX()
	{
		AudioManager.Play("level_dragon_left_dragon_tongue_intro");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_tongue_intro");
	}

	// Token: 0x0600183A RID: 6202 RVA: 0x000A3390 File Offset: 0x000A1590
	public IEnumerator fire_cr()
	{
		this.state = DragonLevelLeftSideDragon.State.Fire;
		int patternIndex = 0;
		this.fire.transform.parent = null;
		for (;;)
		{
			string[] pattern = base.properties.CurrentState.fireAndSmoke.PatternString.Split(new char[]
			{
				','
			});
			string text = pattern[patternIndex % pattern.Length];
			if (text != null)
			{
				if (!(text == "F"))
				{
					if (text == "S")
					{
						AudioManager.Play("level_dragon_left_dragon_smoke_start");
						this.emitAudioFromObject.Add("level_dragon_left_dragon_smoke_start");
						base.animator.SetTrigger("StartSmoke");
						yield return base.animator.WaitForAnimationToStart(this, "Smoke_Loop", 2, false);
						AudioManager.Play("level_dragon_left_dragon_smoke_loop");
						this.emitAudioFromObject.Add("level_dragon_left_dragon_smoke_loop");
						yield return CupheadTime.WaitForSeconds(this, Random.Range(0.66f, 1.32f));
						AudioManager.Play("level_dragon_left_dragon_smoke_end");
						this.emitAudioFromObject.Add("level_dragon_left_dragon_smoke_end");
					}
				}
				else
				{
					AudioManager.Play("level_dragon_left_dragon_fire_start");
					this.emitAudioFromObject.Add("level_dragon_left_dragon_fire_start");
					base.animator.SetTrigger("StartFire");
					yield return base.animator.WaitForAnimationToStart(this, "Fire_Loop", 2, false);
					AudioManager.Play("level_dragon_left_dragon_fire_loop");
					this.emitAudioFromObject.Add("level_dragon_left_dragon_fire_loop");
					this.fire.SetColliderEnabled(true);
					yield return CupheadTime.WaitForSeconds(this, Random.Range(0.25f, 1.75f));
					AudioManager.Play("level_dragon_left_dragon_fire_end");
					this.emitAudioFromObject.Add("level_dragon_left_dragon_fire_end");
					this.fire.SetColliderEnabled(false);
				}
			}
			base.animator.SetTrigger("Continue");
			yield return base.animator.WaitForAnimationToStart(this, "Idle", 2, false);
			yield return CupheadTime.WaitForSeconds(this, 0.25f);
			patternIndex++;
		}
		yield break;
	}

	// Token: 0x0600183B RID: 6203 RVA: 0x00014BE4 File Offset: 0x00012DE4
	public void StartFireMarchers()
	{
		base.StartCoroutine(this.spawnFireMarchers_cr());
		base.StartCoroutine(this.fireMarchersJump_cr());
	}

	// Token: 0x0600183C RID: 6204 RVA: 0x000A33AC File Offset: 0x000A15AC
	public IEnumerator spawnFireMarchers_cr()
	{
		this.fireMarcherLeaderPrefab.Create(this.fireMarcherRoot, base.properties.CurrentState.fireMarchers);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.fireMarchers.spawnDelay);
			this.lastFireMarcher = this.fireMarcherPrefabs.RandomChoice<DragonLevelFireMarcher>().Create(this.fireMarcherRoot, base.properties.CurrentState.fireMarchers);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600183D RID: 6205 RVA: 0x000A33C8 File Offset: 0x000A15C8
	public IEnumerator fireMarchersJump_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.fireMarchers.jumpDelay.RandomFloat());
			DragonLevelFireMarcher[] fireMarchers = Object.FindObjectsOfType<DragonLevelFireMarcher>();
			fireMarchers.Shuffle<DragonLevelFireMarcher>();
			foreach (DragonLevelFireMarcher dragonLevelFireMarcher in fireMarchers)
			{
				if (dragonLevelFireMarcher.CanJump())
				{
					dragonLevelFireMarcher.StartJump(PlayerManager.GetNext());
					break;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600183E RID: 6206 RVA: 0x00014C00 File Offset: 0x00012E00
	public void StartThreeHeads()
	{
		this.StopAllCoroutines();
		this.state = DragonLevelLeftSideDragon.State.Transition;
		this.fire.gameObject.SetActive(false);
		base.StartCoroutine(this.three_heads_cr());
	}

	// Token: 0x0600183F RID: 6207 RVA: 0x000A33E4 File Offset: 0x000A15E4
	public IEnumerator three_heads_cr()
	{
		base.animator.SetTrigger("StartThree");
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		while (this.lastFireMarcher != null)
		{
			yield return null;
		}
		base.animator.SetTrigger("FoldTongue");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro_Reverse", 1, false, true);
		base.animator.SetTrigger("ToThree");
		yield return base.animator.WaitForAnimationToStart(this, "Three_Intro", false);
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		AudioManager.Play("level_dragon_three_dragon_intro");
		this.emitAudioFromObject.Add("level_dragon_three_dragon_intro");
		this.state = DragonLevelLeftSideDragon.State.ThreeHeads;
		foreach (DragonLevelBackgroundChange dragonLevelBackgroundChange in this.backgrounds)
		{
			dragonLevelBackgroundChange.StartChange();
		}
		for (int j = 0; j < this.backgroundsToHide.Length; j++)
		{
			this.backgroundsToHide[j].SetActive(false);
		}
		this.spire.StartChange();
		this.rain.StartRain();
		base.StartCoroutine(this.potion_cr());
		base.StartCoroutine(this.blow_torch_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001840 RID: 6208 RVA: 0x00014C2D File Offset: 0x00012E2D
	public void ActivateHeadLayers()
	{
		base.animator.SetTrigger("StartHeads");
	}

	// Token: 0x06001841 RID: 6209 RVA: 0x000A3400 File Offset: 0x000A1600
	public void SpawnPotion(int type)
	{
		this.spittle.gameObject.SetActive(false);
		Vector3 vector = Vector3.zero;
		DragonLevelPotion dragonLevelPotion = this.horizontalPotionPrefab;
		LevelProperties.Dragon.Potions potions = base.properties.CurrentState.potions;
		string[] array = potions.potionTypeString[this.potionTypeMainIndex].Split(new char[]
		{
			','
		});
		if (array[this.potionTypeIndex][0] == 'H')
		{
			dragonLevelPotion = this.horizontalPotionPrefab;
		}
		else if (array[this.potionTypeIndex][0] == 'V')
		{
			dragonLevelPotion = this.verticalPotionPrefab;
		}
		else if (array[this.potionTypeIndex][0] == 'X')
		{
			dragonLevelPotion = this.bothPotionPrefab;
		}
		if (type == 1 || type == 3)
		{
			vector = this.topHead.position;
		}
		else if (type == 2 || type == 4)
		{
			vector = this.bottomHead.position;
		}
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 vector2 = Vector3.zero;
		if (next.transform.position.x > base.transform.position.x)
		{
			vector2 = next.transform.position - vector;
		}
		else
		{
			vector2 = MathUtils.AngleToDirection(90f);
		}
		DragonLevelPotion dragonLevelPotion2 = Object.Instantiate<DragonLevelPotion>(dragonLevelPotion);
		dragonLevelPotion2.Init(vector, base.properties.CurrentState.potions.potionHP, MathUtils.DirectionToAngle(vector2), base.properties.CurrentState.potions);
		if (this.potionTypeIndex < array.Length - 1)
		{
			this.potionTypeIndex++;
		}
		else
		{
			this.potionTypeMainIndex = (this.potionTypeMainIndex + 1) % potions.potionTypeString.Length;
			this.potionTypeIndex = 0;
		}
		this.spittle.gameObject.SetActive(true);
		this.spittle.position = vector;
	}

	// Token: 0x06001842 RID: 6210 RVA: 0x000A35FC File Offset: 0x000A17FC
	public IEnumerator potion_cr()
	{
		LevelProperties.Dragon.Potions p = base.properties.CurrentState.potions;
		string[] attackCountString = p.attackCount[this.attackCountMainIndex].Split(new char[]
		{
			','
		});
		string[] shotPositionString = p.shotPositionString[this.shotPositionMainIndex].Split(new char[]
		{
			','
		});
		int attackCount = 0;
		for (;;)
		{
			attackCountString = p.attackCount[this.attackCountMainIndex].Split(new char[]
			{
				','
			});
			Parser.IntTryParse(attackCountString[this.attackCountIndex], out attackCount);
			for (int i = 0; i < attackCount; i++)
			{
				shotPositionString = p.shotPositionString[this.shotPositionMainIndex].Split(new char[]
				{
					','
				});
				string[] pickedDragon = shotPositionString[this.shotPositionIndex].Split(new char[]
				{
					':'
				});
				foreach (string picked in pickedDragon)
				{
					while (this.torch)
					{
						yield return null;
					}
					if (shotPositionString[this.shotPositionIndex][0] == 'T')
					{
						this.animationString = "High_Attack";
					}
					else if (shotPositionString[this.shotPositionIndex][0] == 'B')
					{
						this.animationString = "Low_Attack";
					}
					if (picked == "A")
					{
						this.layer = 5;
					}
					else if (picked == "C")
					{
						this.layer = 6;
					}
				}
				if (this.layer == 5 && this.animationString == "High_Attack")
				{
					this.headPicked = DragonLevelLeftSideDragon.HeadPicked.CTop;
				}
				else if (this.layer == 6 && this.animationString == "High_Attack")
				{
					this.headPicked = DragonLevelLeftSideDragon.HeadPicked.ATop;
				}
				else if (this.layer == 5 && this.animationString == "Low_Attack")
				{
					this.headPicked = DragonLevelLeftSideDragon.HeadPicked.CBottom;
				}
				else if (this.layer == 6 && this.animationString == "Low_Attack")
				{
					this.headPicked = DragonLevelLeftSideDragon.HeadPicked.ABottom;
				}
				yield return base.animator.WaitForAnimationToEnd(this, this.animationString, this.layer, false, true);
				if (this.shotPositionIndex < shotPositionString.Length - 1)
				{
					this.shotPositionIndex++;
				}
				else
				{
					this.shotPositionMainIndex = (this.shotPositionMainIndex + 1) % p.shotPositionString.Length;
					this.shotPositionIndex = 0;
				}
				yield return CupheadTime.WaitForSeconds(this, p.repeatDelay);
			}
			if (this.attackCountIndex < attackCountString.Length - 1)
			{
				this.attackCountIndex++;
			}
			else
			{
				this.attackCountMainIndex = (this.attackCountMainIndex + 1) % p.attackCount.Length;
				this.attackCountIndex = 0;
			}
			yield return CupheadTime.WaitForSeconds(this, p.attackMainDelay);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001843 RID: 6211 RVA: 0x000A3618 File Offset: 0x000A1818
	public void PotionAttack(DragonLevelLeftSideDragon.HeadPicked picked)
	{
		if (picked == this.headPicked)
		{
			AudioManager.Play("level_dragon_three_dragon_head_attack");
			this.emitAudioFromObject.Add("level_dragon_three_dragon_head_attack");
			base.animator.Play(this.animationString, this.layer);
			this.headPicked = DragonLevelLeftSideDragon.HeadPicked.None;
		}
	}

	// Token: 0x06001844 RID: 6212 RVA: 0x000A366C File Offset: 0x000A186C
	public IEnumerator blow_torch_cr()
	{
		LevelProperties.Dragon.Blowtorch p = base.properties.CurrentState.blowtorch;
		string[] delayPattern = p.attackDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.middleHead.SetScale(new float?(this.middleHead.transform.localScale.x), new float?(p.fireSize), new float?(1f));
		float delay = 0f;
		int delayCountIndex = Random.Range(0, delayPattern.Length);
		for (;;)
		{
			Parser.FloatTryParse(delayPattern[delayCountIndex], out delay);
			yield return CupheadTime.WaitForSeconds(this, delay);
			delayCountIndex = (delayCountIndex + 1) % delayPattern.Length;
			this.torch = true;
			yield return base.animator.WaitForAnimationToEnd(this, "Dragon_Head_Idle_Loop", 3, false, true);
			AudioManager.Play("level_dragon_torch_warning_1_start");
			this.emitAudioFromObject.Add("level_dragon_torch_warning_1_start");
			base.animator.Play("Torch_Warning_One", 4);
			yield return base.animator.WaitForAnimationToEnd(this, "Torch_End", 4, false, true);
			this.torch = false;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001845 RID: 6213 RVA: 0x000A3688 File Offset: 0x000A1888
	public void ActivateTorch()
	{
		this.middleHead.GetComponent<Animator>().SetBool("TorchOn", true);
		AudioManager.Play("level_dragon_three_dragon_head_b_torch_attack_burst");
		this.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_attack_burst");
		AudioManager.PlayLoop("level_dragon_three_dragon_head_b_torch_attack_loop");
		this.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_attack_loop");
	}

	// Token: 0x06001846 RID: 6214 RVA: 0x00014C3F File Offset: 0x00012E3F
	public void DeactivateTorch()
	{
		this.middleHead.GetComponent<Animator>().SetBool("TorchOn", false);
		AudioManager.Stop("level_dragon_three_dragon_head_b_torch_attack_loop");
	}

	// Token: 0x06001847 RID: 6215 RVA: 0x000A36E0 File Offset: 0x000A18E0
	public void Torch1Counter()
	{
		if (this.Counter >= base.properties.CurrentState.blowtorch.warningDurationOne)
		{
			this.Counter = 0;
			AudioManager.Play("level_dragon_three_dragon_head_b_torch_continue_one");
			this.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_continue_one");
			base.animator.Play("Torch_Continue", 4);
		}
		else
		{
			this.Counter++;
		}
	}

	// Token: 0x06001848 RID: 6216 RVA: 0x000A3754 File Offset: 0x000A1954
	public void Attack1Counter()
	{
		if (this.Counter >= this.AttackFrames / 2 + this.AttackFrames % 2)
		{
			this.Counter = 0;
			AudioManager.Play("level_dragon_three_dragon_head_b_torch_warning_2_start");
			this.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_warning_2_start");
			base.animator.Play("Torch_Warning_Two", 4);
		}
		else
		{
			this.Counter++;
		}
	}

	// Token: 0x06001849 RID: 6217 RVA: 0x000A37C4 File Offset: 0x000A19C4
	public void Torch2Counter()
	{
		if (this.Counter >= base.properties.CurrentState.blowtorch.warningDurationTwo)
		{
			this.Counter = 0;
			AudioManager.Play("level_dragon_three_dragon_head_b_torch_continue_one");
			this.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_continue_one");
			base.animator.Play("Torch_Continue_Two", 4);
		}
		else
		{
			this.Counter++;
		}
	}

	// Token: 0x0600184A RID: 6218 RVA: 0x000A3838 File Offset: 0x000A1A38
	public void Attack2Counter()
	{
		if (this.Counter >= this.AttackFrames / 2)
		{
			this.Counter = 0;
			AudioManager.Play("level_dragon_three_dragon_head_b_torch_end");
			this.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_end");
			base.animator.Play("Torch_End", 4);
		}
		else
		{
			this.Counter++;
		}
	}

	// Token: 0x0600184B RID: 6219 RVA: 0x000A38A0 File Offset: 0x000A1AA0
	public void StartDeath()
	{
		AudioManager.Play("level_dragon_three_dragon_death");
		this.emitAudioFromObject.Add("level_dragon_three_dragon_death");
		this.StopAllCoroutines();
		this.middleHead.gameObject.SetActive(false);
		base.animator.SetTrigger("Continue");
		if (Level.Current.mode == Level.Mode.Easy)
		{
			base.animator.SetTrigger("DeadEASY");
		}
		else
		{
			base.animator.SetTrigger("Dead");
		}
	}

	// Token: 0x0400139A RID: 5018
	public const float FRAME_RATE = 0.0416666679f;

	// Token: 0x0400139C RID: 5020
	public DragonLevelLeftSideDragon.HeadPicked headPicked;

	// Token: 0x0400139D RID: 5021
	public const int MAIN_LAYER = 0;

	// Token: 0x0400139E RID: 5022
	public const int TONGUE_LAYER = 1;

	// Token: 0x0400139F RID: 5023
	public const int FIRE_LAYER = 2;

	// Token: 0x040013A0 RID: 5024
	public const float introTime = 1.3f;

	// Token: 0x040013A1 RID: 5025
	public const float mainX = -350f;

	// Token: 0x040013A2 RID: 5026
	[SerializeField]
	public Collider2D damageBox;

	// Token: 0x040013A3 RID: 5027
	[SerializeField]
	public DragonLevelSpire spire;

	// Token: 0x040013A4 RID: 5028
	[SerializeField]
	public DragonLevelRain rain;

	// Token: 0x040013A5 RID: 5029
	[SerializeField]
	public DragonLevelBackgroundChange[] backgrounds;

	// Token: 0x040013A6 RID: 5030
	[SerializeField]
	public GameObject[] backgroundsToHide;

	// Token: 0x040013A7 RID: 5031
	[SerializeField]
	public DragonLevelFire fire;

	// Token: 0x040013A8 RID: 5032
	[SerializeField]
	public Transform fireMarcherRoot;

	// Token: 0x040013A9 RID: 5033
	[SerializeField]
	public DragonLevelFireMarcher[] fireMarcherPrefabs;

	// Token: 0x040013AA RID: 5034
	[SerializeField]
	public DragonLevelFireMarcher fireMarcherLeaderPrefab;

	// Token: 0x040013AB RID: 5035
	[SerializeField]
	public Transform topHead;

	// Token: 0x040013AC RID: 5036
	[SerializeField]
	public Transform bottomHead;

	// Token: 0x040013AD RID: 5037
	[SerializeField]
	public Transform middleHead;

	// Token: 0x040013AE RID: 5038
	[SerializeField]
	public DragonLevelPotion horizontalPotionPrefab;

	// Token: 0x040013AF RID: 5039
	[SerializeField]
	public DragonLevelPotion verticalPotionPrefab;

	// Token: 0x040013B0 RID: 5040
	[SerializeField]
	public DragonLevelPotion bothPotionPrefab;

	// Token: 0x040013B1 RID: 5041
	[SerializeField]
	public Transform spittle;

	// Token: 0x040013B2 RID: 5042
	public DragonLevelFireMarcher lastFireMarcher;

	// Token: 0x040013B3 RID: 5043
	public DamageDealer damageDealer;

	// Token: 0x040013B4 RID: 5044
	public DamageReceiver damageReceiver;

	// Token: 0x040013B5 RID: 5045
	public bool dead;

	// Token: 0x040013B6 RID: 5046
	public bool torch;

	// Token: 0x040013B7 RID: 5047
	public int potionTypeIndex;

	// Token: 0x040013B8 RID: 5048
	public int potionTypeMainIndex;

	// Token: 0x040013B9 RID: 5049
	public int attackCountIndex;

	// Token: 0x040013BA RID: 5050
	public int attackCountMainIndex;

	// Token: 0x040013BB RID: 5051
	public int shotPositionIndex;

	// Token: 0x040013BC RID: 5052
	public int shotPositionMainIndex;

	// Token: 0x040013BD RID: 5053
	public int AttackFrames;

	// Token: 0x040013BE RID: 5054
	public int Counter;

	// Token: 0x040013BF RID: 5055
	public string animationString;

	// Token: 0x040013C0 RID: 5056
	public int layer;

	// Token: 0x040013C1 RID: 5057
	public float xPos;

	// Token: 0x02000BF3 RID: 3059
	public enum State
	{
		// Token: 0x04005705 RID: 22277
		UnSpawned,
		// Token: 0x04005706 RID: 22278
		Fire,
		// Token: 0x04005707 RID: 22279
		Transition,
		// Token: 0x04005708 RID: 22280
		ThreeHeads,
		// Token: 0x04005709 RID: 22281
		Dead
	}

	// Token: 0x02000BF4 RID: 3060
	public enum HeadPicked
	{
		// Token: 0x0400570B RID: 22283
		ATop,
		// Token: 0x0400570C RID: 22284
		ABottom,
		// Token: 0x0400570D RID: 22285
		CTop,
		// Token: 0x0400570E RID: 22286
		CBottom,
		// Token: 0x0400570F RID: 22287
		None
	}
}
