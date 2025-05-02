using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class ChaliceTutorialLevel : Level
{
	// Token: 0x060000A2 RID: 162 RVA: 0x0005F268 File Offset: 0x0005D468
	public override void PartialInit()
	{
		this.properties = LevelProperties.ChaliceTutorial.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000379D File Offset: 0x0000199D
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChaliceTutorial;
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060000A4 RID: 164 RVA: 0x000037A4 File Offset: 0x000019A4
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chalice_tutorial;
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x060000A5 RID: 165 RVA: 0x000037A8 File Offset: 0x000019A8
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x060000A6 RID: 166 RVA: 0x000037B0 File Offset: 0x000019B0
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x0005F300 File Offset: 0x0005D500
	public override void Start()
	{
		base.Start();
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			if (abstractPlayerController != null)
			{
				foreach (Transform transform in abstractPlayerController.GetComponentsInChildren<Transform>())
				{
					transform.gameObject.layer = 31;
				}
			}
		}
		base.StartCoroutine(this.parryables_cr());
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x000037B8 File Offset: 0x000019B8
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x000037C6 File Offset: 0x000019C6
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.intro_cr());
		base.StartCoroutine(this.chalicetutorialPattern_cr());
	}

	// Token: 0x060000AA RID: 170 RVA: 0x0005F3A4 File Offset: 0x0005D5A4
	public IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.backgroundAnimator.Play("Zoom");
		yield break;
	}

	// Token: 0x060000AB RID: 171 RVA: 0x0005F3C0 File Offset: 0x0005D5C0
	public IEnumerator parryables_cr()
	{
		for (;;)
		{
			for (int j = 0; j < this.parrybles.Length; j++)
			{
				this.parrybles[j].Deactivated();
			}
			while (!this.backgroundAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			{
				Effect[] es = Object.FindObjectsOfType<Effect>();
				foreach (Effect effect in es)
				{
					effect.gameObject.layer = 31;
					if (effect.transform.childCount > 0)
					{
						foreach (Transform transform in effect.transform.GetChildTransforms())
						{
							transform.gameObject.layer = 31;
						}
					}
				}
				AbstractProjectile[] ps = Object.FindObjectsOfType<AbstractProjectile>();
				foreach (AbstractProjectile abstractProjectile in ps)
				{
					abstractProjectile.gameObject.layer = 31;
					if (abstractProjectile.transform.childCount > 0)
					{
						foreach (Transform transform2 in abstractProjectile.transform.GetChildTransforms())
						{
							transform2.gameObject.layer = 31;
						}
					}
				}
				PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
				yield return null;
			}
			PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
			for (int i = 0; i < this.parrybles.Length; i++)
			{
				this.parrybles[i].Activated();
				while (!this.parrybles[i].isDeactivated)
				{
					if (this.resetParryables)
					{
						break;
					}
					yield return null;
				}
				if (this.resetParryables)
				{
					break;
				}
			}
			this.resetParryables = false;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000AC RID: 172 RVA: 0x0005F3DC File Offset: 0x0005D5DC
	public void Exit()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		this.playerExitEffects[0].gameObject.SetActive(true);
		this.playerExitEffects[0].transform.position = player.transform.position;
		player.gameObject.SetActive(false);
		this.playerExitEffects[0].animator.SetTrigger("OnStartTutorial");
		player = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player != null)
		{
			this.playerExitEffects[1].gameObject.SetActive(true);
			this.playerExitEffects[1].transform.position = player.transform.position;
			player.gameObject.SetActive(false);
			this.playerExitEffects[1].animator.SetTrigger("OnStartTutorial");
		}
	}

	// Token: 0x060000AD RID: 173 RVA: 0x0005F4AC File Offset: 0x0005D6AC
	public IEnumerator chalicetutorialPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000AE RID: 174 RVA: 0x0005F4C8 File Offset: 0x0005D6C8
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.ChaliceTutorial.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x000037E2 File Offset: 0x000019E2
	public void WORKAROUND_NullifyFields()
	{
		this._bossPortrait = null;
		this._bossQuote = null;
		this.backgroundAnimator = null;
		this.parrybles = null;
		this.playerExitEffects = null;
	}

	// Token: 0x040000A9 RID: 169
	public LevelProperties.ChaliceTutorial properties;

	// Token: 0x040000AA RID: 170
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x040000AB RID: 171
	[SerializeField]
	[Multiline]
	public string _bossQuote;

	// Token: 0x040000AC RID: 172
	[SerializeField]
	public Animator backgroundAnimator;

	// Token: 0x040000AD RID: 173
	[SerializeField]
	public ChaliceTutorialLevelParryable[] parrybles;

	// Token: 0x040000AE RID: 174
	public bool finishedPuzzle;

	// Token: 0x040000AF RID: 175
	public bool resetParryables;

	// Token: 0x040000B0 RID: 176
	[SerializeField]
	public PlayerDeathEffect[] playerExitEffects;
}
