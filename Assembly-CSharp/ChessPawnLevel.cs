using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class ChessPawnLevel : ChessLevel
{
	// Token: 0x0600011B RID: 283 RVA: 0x000608E8 File Offset: 0x0005EAE8
	public override void PartialInit()
	{
		this.properties = LevelProperties.ChessPawn.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x0600011C RID: 284 RVA: 0x00003B0F File Offset: 0x00001D0F
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessPawn;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x0600011D RID: 285 RVA: 0x00003B16 File Offset: 0x00001D16
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_pawn;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x0600011E RID: 286 RVA: 0x00003B1A File Offset: 0x00001D1A
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortraitMain;
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x0600011F RID: 287 RVA: 0x00003B22 File Offset: 0x00001D22
	public override string BossQuote
	{
		get
		{
			return this._bossQuoteMain;
		}
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00003B2A File Offset: 0x00001D2A
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		base.OnIntroEvent -= this.onIntroEvent;
		this.pawn = null;
		this.pawns = null;
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00060980 File Offset: 0x0005EB80
	public override void Start()
	{
		Level.IsChessBoss = true;
		base.Start();
		base.OnIntroEvent += this.onIntroEvent;
		int num = (int)this.properties.TotalHealth / 10;
		this.pawns = new ChessPawnLevelPawn[num];
		for (int i = 0; i < num; i++)
		{
			this.pawns[i] = this.pawn.Init(this);
			this.pawns[i].transform.position = this.GetPosition(i) + new Vector3(0f, 300f);
			this.pawns[i].SetIndex(i);
		}
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00003B59 File Offset: 0x00001D59
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00003B68 File Offset: 0x00001D68
	public void TakeDamage()
	{
		this.properties.DealDamage(10f);
		if (this.properties.CurrentHealth <= 0f)
		{
			this.die();
		}
	}

	// Token: 0x06000124 RID: 292 RVA: 0x00060A28 File Offset: 0x0005EC28
	public void onIntroEvent()
	{
		for (int i = 0; i < this.pawns.Length; i++)
		{
			this.pawns[i].StartIntro();
			this.SFX_KOG_PAWN_IntroJeers();
		}
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00060A64 File Offset: 0x0005EC64
	public IEnumerator main_cr()
	{
		LevelProperties.ChessPawn.Pawn p = this.properties.CurrentState.pawn;
		PatternString pawnAttackDelay = new PatternString(p.pawnAttackDelayString, true);
		PatternString pawnDirection = new PatternString(p.pawnDirectionString, true);
		PatternString pawnOrder = new PatternString(p.pawnOrderString, true);
		for (;;)
		{
			bool pink = pawnOrder.PopLetter() == 'P';
			List<int> availableList = new List<int>();
			for (int j = 0; j < this.pawns.Length; j++)
			{
				if (pink == this.pawns[j].CanParry && !this.pawns[j].inUse)
				{
					availableList.Add(j);
				}
			}
			if (availableList.Count > 0)
			{
				float dir = 0f;
				char c = pawnDirection.PopLetter();
				if (c != 'L')
				{
					if (c != 'D')
					{
						if (c == 'R')
						{
							dir = 1f;
						}
					}
					else
					{
						dir = 0f;
					}
				}
				else
				{
					dir = -1f;
				}
				int i = Random.Range(0, availableList.Count);
				if (Mathf.Abs(this.GetPosition(this.pawns[availableList[i]].currentIndex).x + dir * p.pawnDropDistance) > 800f)
				{
					dir = 0f;
				}
				dir *= p.pawnDropDistance;
				this.pawns[availableList[i]].Attack(p.pawnWarningTime, dir, p.pawnDropSpeed, p.pawnRunHesitation, p.pawnRunSpeed, p.pawnReturnDelay);
				yield return CupheadTime.WaitForSeconds(this, pawnAttackDelay.PopFloat() - p.pawnDelayReduction * (float)this.damageCount());
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00060A80 File Offset: 0x0005EC80
	public void die()
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		this.SFX_KOG_PAWN_BeatLevelHarp();
		foreach (ChessPawnLevelPawn chessPawnLevelPawn in this.pawns)
		{
			chessPawnLevelPawn.Death();
		}
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00003B95 File Offset: 0x00001D95
	public int damageCount()
	{
		return (int)(this.properties.TotalHealth - this.properties.CurrentHealth) / 10;
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00060ACC File Offset: 0x0005ECCC
	public int GetReturnIndex()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.pawns.Length; i++)
		{
			list.Add(i);
		}
		for (int j = 0; j < this.pawns.Length; j++)
		{
			list.Remove(this.pawns[j].currentIndex);
		}
		return list[Random.Range(0, list.Count)];
	}

	// Token: 0x06000129 RID: 297 RVA: 0x00003BB2 File Offset: 0x00001DB2
	public Vector3 GetPosition(int index)
	{
		return new Vector3(-622f + (float)index * 180f, 340f);
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00060B40 File Offset: 0x0005ED40
	public bool ClearToRun(float testDir, Vector3 pos)
	{
		bool result = true;
		for (int i = 0; i < this.pawns.Length; i++)
		{
			if (this.pawns[i].speed != 0f && Mathf.Sign(this.pawns[i].speed) == testDir && Vector3.Distance(this.pawns[i].transform.position, pos) < 200f)
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00003BCC File Offset: 0x00001DCC
	public void SFX_KOG_PAWN_IntroJeers()
	{
		AudioManager.Play("sfx_DLC_KOG_Pawn_IntroJeers");
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00003BD8 File Offset: 0x00001DD8
	public void SFX_KOG_PAWN_BeatLevelHarp()
	{
		AudioManager.Play("sfx_dlc_kog_pawn_beatlevelharp");
	}

	// Token: 0x040000F6 RID: 246
	public LevelProperties.ChessPawn properties;

	// Token: 0x040000F7 RID: 247
	public const float WAIT_TO_RUN_DIST = 200f;

	// Token: 0x040000F8 RID: 248
	public const float SPACING = 180f;

	// Token: 0x040000F9 RID: 249
	public const float LEFTMOST_X = -622f;

	// Token: 0x040000FA RID: 250
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x040000FB RID: 251
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x040000FC RID: 252
	[SerializeField]
	public ChessPawnLevelPawn pawn;

	// Token: 0x040000FD RID: 253
	public ChessPawnLevelPawn[] pawns;

	// Token: 0x040000FE RID: 254
	public bool dead;
}
