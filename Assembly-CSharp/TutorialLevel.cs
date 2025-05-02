using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class TutorialLevel : Level
{
	// Token: 0x0600045B RID: 1115 RVA: 0x00069BAC File Offset: 0x00067DAC
	public override void PartialInit()
	{
		this.properties = LevelProperties.Tutorial.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x0600045C RID: 1116 RVA: 0x000051F0 File Offset: 0x000033F0
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Tutorial;
		}
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x0600045D RID: 1117 RVA: 0x000051F3 File Offset: 0x000033F3
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_tutorial;
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x0600045E RID: 1118 RVA: 0x000051F6 File Offset: 0x000033F6
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x0600045F RID: 1119 RVA: 0x000051FE File Offset: 0x000033FE
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x00005206 File Offset: 0x00003406
	public override void Start()
	{
		base.Start();
		this.background.SetParent(UnityEngine.Camera.main.transform);
		this.background.ResetLocalTransforms();
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x0000522E File Offset: 0x0000342E
	public override void OnLevelStart()
	{
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x00069C44 File Offset: 0x00067E44
	public void GoBackToHouse()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		this.playerGoBackToHouseEffects[0].gameObject.SetActive(true);
		this.playerGoBackToHouseEffects[0].transform.position = player.transform.position;
		player.gameObject.SetActive(false);
		this.playerGoBackToHouseEffects[0].animator.SetTrigger("OnStartTutorial");
		player = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player != null)
		{
			this.playerGoBackToHouseEffects[1].gameObject.SetActive(true);
			this.playerGoBackToHouseEffects[1].transform.position = player.transform.position;
			player.gameObject.SetActive(false);
			this.playerGoBackToHouseEffects[1].animator.SetTrigger("OnStartTutorial");
		}
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x00069D14 File Offset: 0x00067F14
	public override void OnDestroy()
	{
		base.OnDestroy();
		for (int i = 0; i < this.playerGoBackToHouseEffects.Length; i++)
		{
			this.playerGoBackToHouseEffects[i].Clean();
		}
		this.playerGoBackToHouseEffects = null;
	}

	// Token: 0x04000331 RID: 817
	public LevelProperties.Tutorial properties;

	// Token: 0x04000332 RID: 818
	[SerializeField]
	public Transform background;

	// Token: 0x04000333 RID: 819
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000334 RID: 820
	[SerializeField]
	[Multiline]
	public string _bossQuote;

	// Token: 0x04000335 RID: 821
	[SerializeField]
	public PlayerDeathEffect[] playerGoBackToHouseEffects;

	// Token: 0x02000860 RID: 2144
	[Serializable]
	public class Prefabs
	{
	}
}
