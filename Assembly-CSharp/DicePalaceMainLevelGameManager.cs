using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001FA RID: 506
public class DicePalaceMainLevelGameManager : LevelProperties.DicePalaceMain.Entity
{
	// Token: 0x06001751 RID: 5969 RVA: 0x000A145C File Offset: 0x0009F65C
	public override void LevelInit(LevelProperties.DicePalaceMain properties)
	{
		base.LevelInit(properties);
		Level.Current.OnIntroEvent += this.StartDice;
		this.kingDiceAni = this.kingDice.GetComponent<Animator>();
		this.maxSpaces = this.allBoardSpaces.Length;
		this.GameSetup();
		this.marker.position = this.boardSpacesObj[DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED].Pivot.position;
		this.marker.rotation = this.boardSpacesObj[DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED].Pivot.rotation;
		if (!DicePalaceMainLevelGameInfo.PLAYED_INTRO_SFX)
		{
			AudioManager.Play("vox_intro");
			this.emitAudioFromObject.Add("vox_intro");
			DicePalaceMainLevelGameInfo.PLAYED_INTRO_SFX = true;
		}
	}

	// Token: 0x06001752 RID: 5970 RVA: 0x000A1518 File Offset: 0x0009F718
	public void GameSetup()
	{
		LevelProperties.DicePalaceMain.Dice properties = base.properties.CurrentState.dice;
		this.dice = Object.Instantiate<DicePalaceMainLevelDice>(this.dicePrefab);
		this.dice.Init(Vector2.zero, properties, this.pivotPoint1);
		this.pivotPoint1.position = this.dice.transform.position;
		this.CheckSafeSpaces();
		this.CheckHearts();
	}

	// Token: 0x06001753 RID: 5971 RVA: 0x000A1588 File Offset: 0x0009F788
	public void CheckSafeSpaces()
	{
		for (int i = 0; i < DicePalaceMainLevelGameInfo.SAFE_INDEXES.Count; i++)
		{
			this.allBoardSpaces[DicePalaceMainLevelGameInfo.SAFE_INDEXES[i]] = DicePalaceMainLevelGameManager.BoardSpaces.FreeSpace;
			this.boardSpacesObj[DicePalaceMainLevelGameInfo.SAFE_INDEXES[i] + 1].Clear = true;
		}
	}

	// Token: 0x06001754 RID: 5972 RVA: 0x000A15E0 File Offset: 0x0009F7E0
	public void CheckHearts()
	{
		for (int i = 0; i < DicePalaceMainLevelGameInfo.HEART_INDEXES.Length; i++)
		{
			this.boardSpacesObj[DicePalaceMainLevelGameInfo.HEART_INDEXES[i] + 1].HasHeart = true;
		}
	}

	// Token: 0x06001755 RID: 5973 RVA: 0x00013E29 File Offset: 0x00012029
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.dicePrefab = null;
	}

	// Token: 0x06001756 RID: 5974 RVA: 0x00013E38 File Offset: 0x00012038
	public void StartDice()
	{
		if (Level.IsTowerOfPower)
		{
			this.EndBoardGame(this.dice);
		}
		else
		{
			base.StartCoroutine(this.check_for_rolled_cr());
		}
	}

	// Token: 0x06001757 RID: 5975 RVA: 0x00013E62 File Offset: 0x00012062
	public void RevealDice()
	{
		this.dice.StartRoll();
	}

	// Token: 0x06001758 RID: 5976 RVA: 0x000A161C File Offset: 0x0009F81C
	public IEnumerator check_for_rolled_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.dice.revealDelay);
		DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY = false;
		this.kingDiceAni.SetTrigger("OnReveal");
		yield return this.kingDiceAni.WaitForAnimationToEnd(this.kingDice, "Dice_Reveal", false, true);
		LevelProperties.DicePalaceMain.Dice p = base.properties.CurrentState.dice;
		int spacesToMove = 0;
		bool playingGame = true;
		while (playingGame)
		{
			while (this.dice.waitingToRoll)
			{
				yield return null;
			}
			spacesToMove = 0;
			DicePalaceMainLevelDice.Roll roll = this.dice.roll;
			if (roll != DicePalaceMainLevelDice.Roll.One)
			{
				if (roll != DicePalaceMainLevelDice.Roll.Two)
				{
					if (roll == DicePalaceMainLevelDice.Roll.Three)
					{
						spacesToMove = 3;
					}
				}
				else
				{
					spacesToMove = 2;
				}
			}
			else
			{
				spacesToMove = 1;
			}
			int spaces = (DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED + spacesToMove <= 1) ? 0 : (DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED + spacesToMove - 1);
			if (spaces < this.maxSpaces && this.allBoardSpaces[spaces] != DicePalaceMainLevelGameManager.BoardSpaces.FreeSpace && this.allBoardSpaces[spaces] != DicePalaceMainLevelGameManager.BoardSpaces.StartOver && spaces + 1 < this.boardSpacesObj.Length && !this.boardSpacesObj[spaces + 1].HasHeart)
			{
				AudioManager.Stop("vox_curious");
				AudioManager.Play("vox_laugh");
				this.emitAudioFromObject.Add("vox_laugh");
			}
			yield return base.StartCoroutine(this.MoveMarker(spacesToMove, false));
			DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED += spacesToMove;
			if (DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED > this.maxSpaces)
			{
				DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED = this.maxSpaces;
				playingGame = false;
				this.kingDiceAni.SetBool("IsSafe", true);
				break;
			}
			DicePalaceMainLevelGameManager.BoardSpaces space = this.allBoardSpaces[spaces];
			this.kingDiceAni.SetTrigger("OnEager");
			if (playingGame)
			{
				if (space == DicePalaceMainLevelGameManager.BoardSpaces.FreeSpace || DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED == this.previousSpace)
				{
					AudioManager.Play("vox_curious");
					this.emitAudioFromObject.Add("vox_curious");
					this.kingDiceAni.SetBool("IsSafe", true);
				}
				else if (space == DicePalaceMainLevelGameManager.BoardSpaces.StartOver)
				{
					AudioManager.Play("vox_startover");
					this.emitAudioFromObject.Add("vox_startover");
					AudioManager.Stop("vox_curious");
					DicePalaceMainLevelGameInfo.SAFE_INDEXES.Add(spaces);
					this.boardSpacesObj[spaces + 1].Clear = true;
					this.CheckSafeSpaces();
					yield return base.StartCoroutine(this.MoveMarker(-this.maxSpaces, true));
					this.kingDiceAni.SetBool("IsSafe", true);
					DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED = 0;
					yield return CupheadTime.WaitForSeconds(this, p.pauseWhenRolled);
				}
				else
				{
					this.previousSpace = DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED;
					this.kingDiceAni.SetBool("IsSafe", false);
					DicePalaceMainLevelGameInfo.SAFE_INDEXES.Add(spaces);
					for (int i = 0; i < DicePalaceMainLevelGameInfo.HEART_INDEXES.Length; i++)
					{
						if (DicePalaceMainLevelGameInfo.HEART_INDEXES[i] == spaces)
						{
							if (DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS == null)
							{
								DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS = new PlayersStatsBossesHub();
							}
							PlayerStatsManager playerStats = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats;
							if (playerStats.Health > 0)
							{
								DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.BonusHP++;
								playerStats.SetHealth(playerStats.Health + 1);
							}
							if (PlayerManager.Multiplayer)
							{
								if (DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS == null)
								{
									DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS = new PlayersStatsBossesHub();
								}
								PlayerStatsManager stats = PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats;
								if (stats.Health > 0)
								{
									DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.BonusHP++;
									stats.SetHealth(stats.Health + 1);
								}
							}
							this.boardSpacesObj[DicePalaceMainLevelGameInfo.HEART_INDEXES[i] + 1].HasHeart = false;
							this.heart.transform.position = this.boardSpacesObj[DicePalaceMainLevelGameInfo.HEART_INDEXES[i] + 1].HeartSpacePosition;
							this.heart.SetActive(true);
							AudioManager.Play("pop_up");
							this.emitAudioFromObject.Add("pop_up");
							yield return CupheadTime.WaitForSeconds(this, 1.5f);
							this.heart.SetActive(false);
							DicePalaceMainLevelGameInfo.HEART_INDEXES[i] = -1;
							break;
						}
					}
					yield return base.StartCoroutine(this.start_mini_boss_cr(this.SelectLevel(space)));
				}
				this.dice.waitingToRoll = true;
				yield return null;
			}
			yield return null;
		}
		this.EndBoardGame(this.dice);
		yield break;
	}

	// Token: 0x06001759 RID: 5977 RVA: 0x000A1638 File Offset: 0x0009F838
	public IEnumerator MoveMarker(int spacesToMove, bool resetBoard)
	{
		int side = 1;
		if (spacesToMove < 0)
		{
			side = -1;
			spacesToMove *= -1;
		}
		for (int i = 0; i < spacesToMove; i++)
		{
			int index = DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED + (1 + i) * side;
			if (index < 0 || index >= this.boardSpacesObj.Length)
			{
				break;
			}
			float t = 0f;
			Vector3 startPos = this.marker.position;
			Vector3 endPos = this.boardSpacesObj[index].Pivot.position;
			Quaternion startRot = this.marker.rotation;
			Quaternion endRot = this.boardSpacesObj[index].Pivot.rotation;
			if (!resetBoard)
			{
				this.markerAnimator.SetTrigger("Move");
				yield return this.markerAnimator.WaitForAnimationToStart(this, "Move", false);
			}
			float movement = (!resetBoard) ? 0.3f : 0.0833333358f;
			while (t < movement)
			{
				this.marker.position = Vector3.Lerp(startPos, endPos, t / movement);
				this.marker.rotation = Quaternion.Lerp(startRot, endRot, t / movement);
				t += CupheadTime.Delta;
				yield return null;
			}
			this.marker.position = endPos;
			this.marker.rotation = endRot;
			AudioManager.Play("counter_move");
			this.emitAudioFromObject.Add("counter_move");
			if (!resetBoard)
			{
				this.markerAnimator.SetTrigger("Marker");
			}
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		yield break;
	}

	// Token: 0x0600175A RID: 5978 RVA: 0x000A1664 File Offset: 0x0009F864
	public Levels SelectLevel(DicePalaceMainLevelGameManager.BoardSpaces space)
	{
		Levels result = Levels.DicePalaceMain;
		switch (space)
		{
		case DicePalaceMainLevelGameManager.BoardSpaces.Booze:
			result = Levels.DicePalaceBooze;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.Chips:
			result = Levels.DicePalaceChips;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.Cigar:
			result = Levels.DicePalaceCigar;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.Domino:
			result = Levels.DicePalaceDomino;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.EightBall:
			result = Levels.DicePalaceEightBall;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.FlyingHorse:
			result = Levels.DicePalaceFlyingHorse;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.FlyingMemory:
			result = Levels.DicePalaceFlyingMemory;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.Pachinko:
			result = Levels.DicePalacePachinko;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.Rabbit:
			result = Levels.DicePalaceRabbit;
			break;
		case DicePalaceMainLevelGameManager.BoardSpaces.Roulette:
			result = Levels.DicePalaceRoulette;
			break;
		}
		return result;
	}

	// Token: 0x0600175B RID: 5979 RVA: 0x000A1720 File Offset: 0x0009F920
	public IEnumerator start_mini_boss_cr(Levels level)
	{
		this.kingDiceAni.SetTrigger("OnEat");
		DicePalaceMainLevelGameInfo.SetPlayersStats();
		yield return this.kingDiceAni.WaitForAnimationToStart(this, "Eat_Screen", false);
		AudioManager.Play("king_dice_eat_screen");
		this.kingDice.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Player.ToString();
		this.kingDice.GetComponent<SpriteRenderer>().sortingOrder = 2000;
		Level.ScoringData.time += Level.Current.LevelTime;
		yield return this.kingDiceAni.WaitForAnimationToEnd(this, "Eat_Screen", false, true);
		SceneLoader.LoadLevel(level, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, null);
		yield return null;
		yield break;
	}

	// Token: 0x0600175C RID: 5980 RVA: 0x000A1744 File Offset: 0x0009F944
	public void EndBoardGame(DicePalaceMainLevelDice dice1)
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.flashEnd_cr());
		base.StartCoroutine(this.announcerSfx_cr());
		this.kingDice.StartKingDiceBattle();
		if (dice1 != null)
		{
			Object.Destroy(dice1.gameObject);
		}
		DicePalaceMainLevelGameInfo.CleanUpRetry();
	}

	// Token: 0x0600175D RID: 5981 RVA: 0x000A1798 File Offset: 0x0009F998
	public IEnumerator flashEnd_cr()
	{
		DicePalaceMainLevelBoardSpace endSpace = this.boardSpacesObj[this.boardSpacesObj.Length - 1];
		for (;;)
		{
			endSpace.Clear = true;
			yield return CupheadTime.WaitForSeconds(this, this.endSpaceFlashRate);
			endSpace.Clear = false;
			yield return CupheadTime.WaitForSeconds(this, this.endSpaceFlashRate);
		}
		yield break;
	}

	// Token: 0x0600175E RID: 5982 RVA: 0x000A17B4 File Offset: 0x0009F9B4
	public IEnumerator announcerSfx_cr()
	{
		AudioManager.Play("level_announcer_ready");
		AudioManager.Play("level_bell_intro");
		yield return CupheadTime.WaitForSeconds(this, 2f);
		AudioManager.Play("level_announcer_begin");
		yield break;
	}

	// Token: 0x040012F6 RID: 4854
	public const float MarkerMovementTime = 0.3f;

	// Token: 0x040012F7 RID: 4855
	public const float MarkerFastMove = 0.0833333358f;

	// Token: 0x040012F8 RID: 4856
	[SerializeField]
	public DicePalaceMainLevelGameManager.BoardSpaces[] allBoardSpaces;

	// Token: 0x040012F9 RID: 4857
	[SerializeField]
	public DicePalaceMainLevelBoardSpace[] boardSpacesObj;

	// Token: 0x040012FA RID: 4858
	[SerializeField]
	public DicePalaceMainLevelBoardSpace startSpaceObj;

	// Token: 0x040012FB RID: 4859
	[SerializeField]
	public DicePalaceMainLevelBoardSpace endSpaceObj;

	// Token: 0x040012FC RID: 4860
	[SerializeField]
	public DicePalaceMainLevelKingDice kingDice;

	// Token: 0x040012FD RID: 4861
	[SerializeField]
	public DicePalaceMainLevelDice dicePrefab;

	// Token: 0x040012FE RID: 4862
	[SerializeField]
	public Transform pivotPoint1;

	// Token: 0x040012FF RID: 4863
	[SerializeField]
	public Transform marker;

	// Token: 0x04001300 RID: 4864
	[SerializeField]
	public Animator markerAnimator;

	// Token: 0x04001301 RID: 4865
	[SerializeField]
	public float endSpaceFlashRate;

	// Token: 0x04001302 RID: 4866
	[SerializeField]
	public GameObject heart;

	// Token: 0x04001303 RID: 4867
	public Animator kingDiceAni;

	// Token: 0x04001304 RID: 4868
	public DicePalaceMainLevelDice dice;

	// Token: 0x04001305 RID: 4869
	public DicePalaceMainLevelGameInfo gameInfo;

	// Token: 0x04001306 RID: 4870
	public int previousSpace;

	// Token: 0x04001307 RID: 4871
	public int maxSpaces;

	// Token: 0x02000BBD RID: 3005
	public enum BoardSpaces
	{
		// Token: 0x040055A2 RID: 21922
		Booze,
		// Token: 0x040055A3 RID: 21923
		Chips,
		// Token: 0x040055A4 RID: 21924
		Cigar,
		// Token: 0x040055A5 RID: 21925
		Domino,
		// Token: 0x040055A6 RID: 21926
		EightBall,
		// Token: 0x040055A7 RID: 21927
		FlyingHorse,
		// Token: 0x040055A8 RID: 21928
		FlyingMemory,
		// Token: 0x040055A9 RID: 21929
		Pachinko,
		// Token: 0x040055AA RID: 21930
		Rabbit,
		// Token: 0x040055AB RID: 21931
		Roulette,
		// Token: 0x040055AC RID: 21932
		FreeSpace,
		// Token: 0x040055AD RID: 21933
		StartOver
	}
}
