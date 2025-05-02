using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000034 RID: 52
public class MausoleumLevel : Level
{
	// Token: 0x06000316 RID: 790 RVA: 0x00065298 File Offset: 0x00063498
	public override void PartialInit()
	{
		this.properties = LevelProperties.Mausoleum.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x06000317 RID: 791 RVA: 0x0000498F File Offset: 0x00002B8F
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Mausoleum;
		}
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x06000318 RID: 792 RVA: 0x00004996 File Offset: 0x00002B96
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_mausoleum;
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x06000319 RID: 793 RVA: 0x00065330 File Offset: 0x00063530
	public override Sprite BossPortrait
	{
		get
		{
			switch (base.mode)
			{
			case Level.Mode.Easy:
				return this._bossPortraitEasy;
			case Level.Mode.Normal:
				return this._bossPortraitNormal;
			case Level.Mode.Hard:
				return this._bossPortraitHard;
			default:
				Debug.LogError("Couldn't find portrait for state " + base.mode + ". Using Main.", null);
				return this._bossPortraitEasy;
			}
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x0600031A RID: 794 RVA: 0x00065398 File Offset: 0x00063598
	public override string BossQuote
	{
		get
		{
			switch (base.mode)
			{
			case Level.Mode.Easy:
				return this._bossQuoteEasy;
			case Level.Mode.Normal:
				return this._bossQuoteNormal;
			case Level.Mode.Hard:
				return this._bossQuoteHard;
			default:
				Debug.LogError("Couldn't find quote for state " + base.mode + ". Using Main.", null);
				return this._bossQuoteEasy;
			}
		}
	}

	// Token: 0x0600031B RID: 795 RVA: 0x00065400 File Offset: 0x00063600
	public override void Awake()
	{
		Level.OverrideDifficulty = true;
		this.LoseGame = (Action)Delegate.Combine(this.LoseGame, new Action(this.Failure));
		Scenes currentMap = PlayerData.Data.CurrentMap;
		if (currentMap != Scenes.scene_map_world_1)
		{
			if (currentMap != Scenes.scene_map_world_2)
			{
				if (currentMap == Scenes.scene_map_world_3)
				{
					base.mode = Level.Mode.Hard;
				}
			}
			else
			{
				base.mode = Level.Mode.Normal;
			}
		}
		else
		{
			base.mode = Level.Mode.Easy;
		}
		this.originalMode = Level.CurrentMode;
		Level.SetCurrentMode(base.mode);
		foreach (GameObject gameObject in this.WorldBackgrounds)
		{
			gameObject.SetActive(false);
		}
		this.WorldBackgrounds[(int)base.mode].SetActive(true);
		this.currentUrnAnimator = this.urnsAnimator[(int)base.mode];
		this.currentChaliceAnimator = this.chaliceCharacterAnimators[(int)base.mode];
		if ((PlayerData.Data.IsUnlocked(PlayerId.Any, Super.level_super_beam) && base.mode == Level.Mode.Easy) || (PlayerData.Data.IsUnlocked(PlayerId.Any, Super.level_super_invincible) && base.mode == Level.Mode.Normal) || (PlayerData.Data.IsUnlocked(PlayerId.Any, Super.level_super_ghost) && base.mode == Level.Mode.Hard))
		{
			this.noChalice = true;
			this.helpSignAnimator.gameObject.SetActive(false);
			this.currentUrnAnimator.SetTrigger("NoGlow");
		}
		base.Awake();
	}

	// Token: 0x0600031C RID: 796 RVA: 0x00065590 File Offset: 0x00063790
	public override void Start()
	{
		this.isMausoleum = true;
		base.Start();
		Dialoguer.events.onMessageEvent += this.OnMessageEvent;
		this.dialogue.chaliceAnimator = this.currentChaliceAnimator;
		Scenes currentMap = PlayerData.Data.CurrentMap;
		if (currentMap != Scenes.scene_map_world_1)
		{
			if (currentMap != Scenes.scene_map_world_2)
			{
				if (currentMap == Scenes.scene_map_world_3)
				{
					this.super = Super.level_super_ghost;
				}
			}
			else
			{
				this.super = Super.level_super_invincible;
			}
		}
		else
		{
			this.super = Super.level_super_beam;
		}
	}

	// Token: 0x0600031D RID: 797 RVA: 0x0000499A File Offset: 0x00002B9A
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.main_pattern_cr());
		if (this.noChalice)
		{
			return;
		}
		base.StartCoroutine(this.helpsignDisappear_cr());
		base.StartCoroutine(this.urnrandomanimation_cr());
	}

	// Token: 0x0600031E RID: 798 RVA: 0x000049CF File Offset: 0x00002BCF
	public override void OnStateChanged()
	{
		base.OnStateChanged();
	}

	// Token: 0x0600031F RID: 799 RVA: 0x000049D7 File Offset: 0x00002BD7
	public void OnMessageEvent(string message, string metaData)
	{
		if (message == "PowerUpGiven")
		{
			this.currentChaliceAnimator.Play("Chalice_Magic_Burst");
			base.StartCoroutine(this.chalice_animation_cr());
			base.StartCoroutine(this.play_chalice_sound_cr());
		}
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00065628 File Offset: 0x00063828
	public IEnumerator chalice_animation_cr()
	{
		yield return this.currentChaliceAnimator.WaitForAnimationToEnd(this, "Chalice_Magic_Burst_mid", false, true);
		this.PlaySuperPowerup();
		yield return null;
		yield break;
	}

	// Token: 0x06000321 RID: 801 RVA: 0x00065644 File Offset: 0x00063844
	public void PlaySuperPowerup()
	{
		foreach (AbstractPlayerController abstractPlayerController in this.players)
		{
			if (!(abstractPlayerController == null))
			{
				if (!this.PowerUpSFXActive)
				{
					this.PowerUpSFXActive = true;
				}
				float num = (abstractPlayerController.transform.position.y >= -195f) ? 368f : 146f;
				this.chaliceBeamEffect.Create(new Vector3(abstractPlayerController.transform.position.x - 10f, num));
				abstractPlayerController.animator.Play("Super_Power_Up");
			}
		}
	}

	// Token: 0x06000322 RID: 802 RVA: 0x00065700 File Offset: 0x00063900
	public IEnumerator play_chalice_sound_cr()
	{
		yield return this.currentChaliceAnimator.WaitForAnimationToEnd(this, "Chalice_Magic_Burst", false, true);
		AudioManager.Play("player_power_up");
		this.emitAudioFromObject.Add("player_power_up");
		yield return null;
		yield break;
	}

	// Token: 0x06000323 RID: 803 RVA: 0x0006571C File Offset: 0x0006391C
	public IEnumerator mausoleumPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00065738 File Offset: 0x00063938
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.Mausoleum.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x06000325 RID: 805 RVA: 0x00065754 File Offset: 0x00063954
	public IEnumerator helpsignDisappear_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		this.helpSignAnimator.SetTrigger("Outro");
		yield break;
	}

	// Token: 0x06000326 RID: 806 RVA: 0x00065770 File Offset: 0x00063970
	public IEnumerator urnrandomanimation_cr()
	{
		while (!this.isLevelOver)
		{
			yield return CupheadTime.WaitForSeconds(this, 2f);
			AudioManager.Play("mausoleum_ghost_jar_shake");
			this.emitAudioFromObject.Add("mausoleum_ghost_jar_shake");
			int rand = Random.Range(1, 4);
			if (rand == 1)
			{
				this.currentUrnAnimator.SetTrigger("Shake");
			}
			else if (rand == 2)
			{
				this.currentUrnAnimator.SetTrigger("SmallVibrate");
			}
			else if (rand == 3)
			{
				this.currentUrnAnimator.SetTrigger("BigVibrate");
			}
		}
		yield break;
	}

	// Token: 0x06000327 RID: 807 RVA: 0x0006578C File Offset: 0x0006398C
	public IEnumerator legendarychaliceappear_cr()
	{
		AudioManager.StopBGM();
		AudioManager.PlayBGMPlaylistManually(false);
		this.currentUrnAnimator.SetTrigger("Sparkle");
		yield return CupheadTime.WaitForSeconds(this, 2.5f);
		AudioManager.Play("mausoleum_lid_pop");
		this.currentUrnAnimator.SetTrigger("Pop");
		yield return CupheadTime.WaitForSeconds(this, 2f);
		float t = 0f;
		float TIME = 1.5f;
		float arcHeight = 150f;
		float arcHeightAdd = 0f;
		Vector3 startPosition = this.currentChaliceAnimator.gameObject.transform.localPosition;
		Vector3 endPosition = this.currentChaliceAnimator.gameObject.transform.localPosition + new Vector3(400f, 0f, 0f);
		AudioManager.Play("mausoleum_ghost_jar_travel");
		this.emitAudioFromObject.Add("mausoleum_ghost_jar_travel");
		while (t < TIME)
		{
			float val = t / TIME;
			Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, val));
			if (t < TIME / 2f)
			{
				arcHeightAdd = Mathf.Lerp(arcHeight, 0f, EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, 1f - val * 2f));
			}
			else
			{
				arcHeightAdd = Mathf.Lerp(arcHeight, 0f, EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, val * 2f - 1f));
			}
			newPosition.y += arcHeightAdd;
			this.currentChaliceAnimator.gameObject.transform.localPosition = newPosition;
			t += Time.deltaTime;
			yield return null;
		}
		AudioManager.Play("mausoleum_ghost_jar_queen_ghost_appear");
		this.emitAudioFromObject.Add("mausoleum_ghost_jar_queen_ghost_appear");
		this.currentChaliceAnimator.SetTrigger("Transition");
		this.dialogue.BeginDialogue();
		yield return CupheadTime.WaitForSeconds(this, 1f);
		float currentDialogueFloat = Dialoguer.GetGlobalFloat(this.dialoguerVariableID);
		Dialoguer.SetGlobalFloat(14, currentDialogueFloat + 1f);
		PlayerData.SaveCurrentFile();
		yield break;
	}

	// Token: 0x06000328 RID: 808 RVA: 0x000657A8 File Offset: 0x000639A8
	public void SetupTimeline()
	{
		base.timeline = new Level.Timeline();
		base.timeline.health = 0f;
		List<float> list = new List<float>();
		int num = 3;
		for (int i = 0; i < num; i++)
		{
			base.timeline.health += (float)this.properties.CurrentState.main.ghostCount;
			list.Add((float)this.properties.CurrentState.main.ghostCount);
		}
		for (int j = 0; j < num - 1; j++)
		{
			base.timeline.AddEventAtHealth(j.ToStringInvariant(), base.timeline.GetHealthOfLastEvent() + (int)list[j]);
		}
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00065868 File Offset: 0x00063A68
	public IEnumerator main_pattern_cr()
	{
		this.SetupTimeline();
		yield return null;
		for (;;)
		{
			LevelProperties.Mausoleum.Main p = this.properties.CurrentState.main;
			int delayMainIndex = Random.Range(0, p.delayString.Length);
			string[] delayString = p.delayString[delayMainIndex].Split(new char[]
			{
				','
			});
			int delayIndex = Random.Range(0, delayString.Length);
			int spawnMainIndex = Random.Range(0, p.spawnString.Length);
			string[] spawnString = p.spawnString[spawnMainIndex].Split(new char[]
			{
				','
			});
			int spawnIndex = Random.Range(0, spawnString.Length);
			int ghostTypeIndex = Random.Range(0, p.ghostTypeString.Length);
			string[] ghostString = p.ghostTypeString[ghostTypeIndex].Split(new char[]
			{
				','
			});
			int ghostIndex = Random.Range(0, ghostString.Length);
			float delay = 0f;
			int spawnPos = 0;
			this.maxCounter = p.ghostCount;
			MausoleumLevel.SPAWNCOUNTER = 0;
			while (MausoleumLevel.SPAWNCOUNTER < this.maxCounter)
			{
				delayString = p.delayString[delayMainIndex].Split(new char[]
				{
					','
				});
				ghostString = p.ghostTypeString[ghostTypeIndex].Split(new char[]
				{
					','
				});
				string[] ghostSplit = ghostString[ghostIndex].Split(new char[]
				{
					'-'
				});
				float extraDelay = 0f;
				int splitcount = 0;
				foreach (string split in ghostSplit)
				{
					spawnString = p.spawnString[spawnMainIndex].Split(new char[]
					{
						','
					});
					if (Parser.IntParse(spawnString[spawnIndex]) >= 6)
					{
						spawnPos = Parser.IntParse(spawnString[spawnIndex]) - 2;
					}
					else
					{
						spawnPos = Parser.IntParse(spawnString[spawnIndex]) - 1;
					}
					Vector3 direction = this.urn.transform.position - this.positions[spawnPos].transform.position;
					float angle = MathUtils.DirectionToAngle(direction);
					int repeatCount = 0;
					Parser.IntTryParse(ghostString[ghostIndex].Substring(1), out repeatCount);
					if (Parser.IntParse(spawnString[spawnIndex]) == 2 || Parser.IntParse(spawnString[spawnIndex]) == 8)
					{
						MausoleumLevelDelayGhost mausoleumLevelDelayGhost = this.delayGhost.Create(this.positions[spawnPos].transform.position, angle, 0f, this.properties.CurrentState.delayGhost);
						mausoleumLevelDelayGhost.GetParent(this);
					}
					else
					{
						char c2 = ghostString[ghostIndex][0];
						switch (c2)
						{
						case 'B':
							if (repeatCount != 0)
							{
								for (int i = 0; i < repeatCount; i++)
								{
									MausoleumLevelBigGhost b = this.bigGhost.Create(this.positions[spawnPos].transform.position, angle, this.properties.CurrentState.bigGhost.speed, this.properties.CurrentState.bigGhost, this.urn.gameObject);
									b.Counts = (splitcount == 0);
									b.GetParent(this);
									yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.bigGhost.multiDelay);
								}
								extraDelay += this.properties.CurrentState.bigGhost.mainAddDelay;
							}
							else
							{
								MausoleumLevelBigGhost mausoleumLevelBigGhost = this.bigGhost.Create(this.positions[spawnPos].transform.position, angle, this.properties.CurrentState.bigGhost.speed, this.properties.CurrentState.bigGhost, this.urn.gameObject);
								mausoleumLevelBigGhost.Counts = (splitcount == 0);
								mausoleumLevelBigGhost.GetParent(this);
								extraDelay += this.properties.CurrentState.bigGhost.mainAddDelay;
							}
							break;
						case 'C':
						{
							MausoleumLevelCircleGhost c = this.circleGhost.Create(this.positions[spawnPos].transform.position, this.urn.transform.position, angle, this.properties.CurrentState.circleGhost.circleSpeed, this.properties.CurrentState.circleGhost.circleRate) as MausoleumLevelCircleGhost;
							c.Counts = (splitcount == 0);
							c.GetParent(this);
							break;
						}
						case 'D':
						{
							MausoleumLevelDelayGhost d = this.delayGhost.Create(this.positions[spawnPos].transform.position, angle, 0f, this.properties.CurrentState.delayGhost);
							d.Counts = (splitcount == 0);
							d.GetParent(this);
							break;
						}
						default:
							if (c2 != 'R')
							{
								if (c2 == 'S')
								{
									if (repeatCount != 0)
									{
										for (int j = 0; j < repeatCount; j++)
										{
											MausoleumLevelSineGhost s = this.sineGhost.Create(this.positions[spawnPos].transform.position, angle, this.properties.CurrentState.sineGhost.ghostSpeed, this.properties.CurrentState.sineGhost);
											s.Counts = (splitcount == 0);
											s.GetParent(this);
											yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.sineGhost.multiDelay);
										}
										extraDelay = this.properties.CurrentState.sineGhost.mainAddDelay;
									}
									else
									{
										MausoleumLevelSineGhost mausoleumLevelSineGhost = this.sineGhost.Create(this.positions[spawnPos].transform.position, angle, this.properties.CurrentState.sineGhost.ghostSpeed, this.properties.CurrentState.sineGhost);
										mausoleumLevelSineGhost.Counts = (splitcount == 0);
										mausoleumLevelSineGhost.GetParent(this);
										extraDelay = this.properties.CurrentState.sineGhost.mainAddDelay;
									}
								}
							}
							else if (repeatCount != 0)
							{
								for (int k = 0; k < repeatCount; k++)
								{
									MausoleumLevelRegularGhost g = this.regularGhost.Create(this.positions[spawnPos].transform.position, angle, this.properties.CurrentState.regularGhost.speed) as MausoleumLevelRegularGhost;
									g.Counts = (splitcount == 0);
									g.GetParent(this);
									yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.regularGhost.multiDelay);
								}
								extraDelay += this.properties.CurrentState.regularGhost.mainAddDelay;
							}
							else
							{
								MausoleumLevelRegularGhost mausoleumLevelRegularGhost = this.regularGhost.Create(this.positions[spawnPos].transform.position, angle, this.properties.CurrentState.regularGhost.speed) as MausoleumLevelRegularGhost;
								mausoleumLevelRegularGhost.Counts = (splitcount == 0);
								mausoleumLevelRegularGhost.GetParent(this);
								extraDelay += this.properties.CurrentState.regularGhost.mainAddDelay;
							}
							break;
						}
					}
					splitcount++;
					if (spawnIndex < spawnString.Length - 1)
					{
						spawnIndex++;
					}
					else
					{
						spawnMainIndex = (spawnMainIndex + 1) % p.spawnString.Length;
						spawnIndex = 0;
					}
				}
				base.timeline.DealDamage(1f);
				yield return null;
				delay = Parser.FloatParse(delayString[delayIndex]) + extraDelay;
				yield return CupheadTime.WaitForSeconds(this, delay);
				extraDelay = 0f;
				if (delayIndex < delayString.Length - 1)
				{
					delayIndex++;
				}
				else
				{
					delayMainIndex = (delayMainIndex + 1) % p.delayString.Length;
					delayIndex = 0;
				}
				if (ghostIndex < ghostString.Length - 1)
				{
					ghostIndex++;
				}
				else
				{
					ghostTypeIndex = (ghostTypeIndex + 1) % p.ghostTypeString.Length;
					ghostIndex = 0;
				}
				yield return null;
			}
			MausoleumLevelGhostBase[] ghosts = Object.FindObjectsOfType(typeof(MausoleumLevelGhostBase)) as MausoleumLevelGhostBase[];
			bool ghostsAlive = true;
			int ghostCounter = 0;
			while (ghostsAlive)
			{
				ghosts = (Object.FindObjectsOfType(typeof(MausoleumLevelGhostBase)) as MausoleumLevelGhostBase[]);
				for (int m = 0; m < ghosts.Length; m++)
				{
					if (ghosts[m].isDead)
					{
						ghostCounter++;
						if (ghostCounter >= ghosts.Length)
						{
							ghostsAlive = false;
							break;
						}
					}
				}
				ghostCounter = 0;
				if (!ghostsAlive)
				{
					break;
				}
				yield return CupheadTime.WaitForSeconds(this, 0.25f);
				yield return null;
			}
			this.properties.DealDamageToNextNamedState();
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600032A RID: 810 RVA: 0x00065884 File Offset: 0x00063A84
	public void Failure()
	{
		base._OnLose();
		AudioManager.Play("mausoleum_ghost_jar_burst");
		this.emitAudioFromObject.Add("mausoleum_ghost_jar_burst");
		PlayerManager.GetPlayer(PlayerId.PlayerOne).GetComponent<LevelPlayerAnimationController>().ScaredSprite(this.FacingLeft(PlayerManager.GetPlayer(PlayerId.PlayerOne)));
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
		{
			PlayerManager.GetPlayer(PlayerId.PlayerTwo).GetComponent<LevelPlayerAnimationController>().ScaredSprite(this.FacingLeft(PlayerManager.GetPlayer(PlayerId.PlayerTwo)));
		}
		base.timeline.OnPlayerDeath(PlayerId.PlayerOne);
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
		{
			base.timeline.OnPlayerDeath(PlayerId.PlayerTwo);
		}
	}

	// Token: 0x0600032B RID: 811 RVA: 0x00065924 File Offset: 0x00063B24
	public bool FacingLeft(AbstractPlayerController player)
	{
		if (player.transform.position.x > this.urn.transform.position.x)
		{
			return player.transform.localScale.x == 1f;
		}
		return player.transform.localScale.x == -1f;
	}

	// Token: 0x0600032C RID: 812 RVA: 0x000659A4 File Offset: 0x00063BA4
	public override void OnDestroy()
	{
		base.OnDestroy();
		Level.SetCurrentMode(this.originalMode);
		base.StopCoroutine(this.urnrandomanimation_cr());
		Dialoguer.events.onMessageEvent -= this.OnMessageEvent;
		this.circleGhost = null;
		this.regularGhost = null;
		this.bigGhost = null;
		this.delayGhost = null;
		this.sineGhost = null;
		this._bossPortraitEasy = null;
		this._bossPortraitHard = null;
		this._bossPortraitNormal = null;
	}

	// Token: 0x0600032D RID: 813 RVA: 0x00004A13 File Offset: 0x00002C13
	public override void OnPreWin()
	{
		this.isLevelOver = true;
		base.StopCoroutine(this.urnrandomanimation_cr());
		if (this.noChalice)
		{
			base.StartCoroutine(this.win_no_chalice());
			return;
		}
		base.StartCoroutine(this.legendarychaliceappear_cr());
	}

	// Token: 0x0600032E RID: 814 RVA: 0x00065A1C File Offset: 0x00063C1C
	public IEnumerator win_no_chalice()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		SceneLoader.LoadLastMap();
		yield break;
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00065A38 File Offset: 0x00063C38
	public override void OnWin()
	{
		base.OnWin();
		if (!PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, this.super))
		{
			PlayerData.Data.Buy(PlayerId.PlayerOne, this.super);
			PlayerData.Data.Buy(PlayerId.PlayerTwo, this.super);
			Level.SuperUnlocked = true;
		}
		if (PlayerData.Data.NumSupers(PlayerId.PlayerOne) >= 3)
		{
			OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "UnlockedAllSupers");
		}
	}

	// Token: 0x04000216 RID: 534
	public LevelProperties.Mausoleum properties;

	// Token: 0x04000217 RID: 535
	public static int SPAWNCOUNTER;

	// Token: 0x04000218 RID: 536
	[SerializeField]
	public GameObject[] WorldBackgrounds;

	// Token: 0x04000219 RID: 537
	[SerializeField]
	public MausoleumLevelCircleGhost circleGhost;

	// Token: 0x0400021A RID: 538
	[SerializeField]
	public MausoleumLevelRegularGhost regularGhost;

	// Token: 0x0400021B RID: 539
	[SerializeField]
	public MausoleumLevelBigGhost bigGhost;

	// Token: 0x0400021C RID: 540
	[SerializeField]
	public MausoleumLevelDelayGhost delayGhost;

	// Token: 0x0400021D RID: 541
	[SerializeField]
	public MausoleumLevelSineGhost sineGhost;

	// Token: 0x0400021E RID: 542
	[SerializeField]
	public Transform[] positions;

	// Token: 0x0400021F RID: 543
	[SerializeField]
	public MausoleumLevelUrn urn;

	// Token: 0x04000220 RID: 544
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitEasy;

	// Token: 0x04000221 RID: 545
	[SerializeField]
	public Sprite _bossPortraitNormal;

	// Token: 0x04000222 RID: 546
	[SerializeField]
	public Sprite _bossPortraitHard;

	// Token: 0x04000223 RID: 547
	[SerializeField]
	public string _bossQuoteEasy;

	// Token: 0x04000224 RID: 548
	[SerializeField]
	public string _bossQuoteNormal;

	// Token: 0x04000225 RID: 549
	[SerializeField]
	public string _bossQuoteHard;

	// Token: 0x04000226 RID: 550
	[SerializeField]
	public Animator helpSignAnimator;

	// Token: 0x04000227 RID: 551
	[SerializeField]
	public Animator[] urnsAnimator;

	// Token: 0x04000228 RID: 552
	[SerializeField]
	public Animator[] chaliceCharacterAnimators;

	// Token: 0x04000229 RID: 553
	public Animator currentUrnAnimator;

	// Token: 0x0400022A RID: 554
	public Animator currentChaliceAnimator;

	// Token: 0x0400022B RID: 555
	[SerializeField]
	public Effect chaliceBeamEffect;

	// Token: 0x0400022C RID: 556
	[SerializeField]
	public MausoleumDialogueInteraction dialogue;

	// Token: 0x0400022D RID: 557
	[SerializeField]
	public int dialoguerVariableID = 14;

	// Token: 0x0400022E RID: 558
	public bool isLevelOver;

	// Token: 0x0400022F RID: 559
	public bool PowerUpSFXActive;

	// Token: 0x04000230 RID: 560
	public Super super = Super.level_super_beam;

	// Token: 0x04000231 RID: 561
	public int maxCounter;

	// Token: 0x04000232 RID: 562
	public bool noChalice;

	// Token: 0x04000233 RID: 563
	public Action WinGame;

	// Token: 0x04000234 RID: 564
	public Action LoseGame;

	// Token: 0x04000235 RID: 565
	public Level.Mode originalMode;
}
