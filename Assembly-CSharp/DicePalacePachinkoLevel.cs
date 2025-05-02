using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class DicePalacePachinkoLevel : AbstractDicePalaceLevel
{
	// Token: 0x060001FC RID: 508 RVA: 0x000625C0 File Offset: 0x000607C0
	public override void PartialInit()
	{
		this.properties = LevelProperties.DicePalacePachinko.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x060001FD RID: 509 RVA: 0x000041D9 File Offset: 0x000023D9
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalacePachinko;
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x060001FE RID: 510 RVA: 0x000041E0 File Offset: 0x000023E0
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalacePachinko;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x060001FF RID: 511 RVA: 0x000041E7 File Offset: 0x000023E7
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_pachinko;
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x06000200 RID: 512 RVA: 0x000041EB File Offset: 0x000023EB
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x06000201 RID: 513 RVA: 0x000041F3 File Offset: 0x000023F3
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000202 RID: 514 RVA: 0x00062658 File Offset: 0x00060858
	public override void Start()
	{
		base.Start();
		this.pipes.LevelInit(this.properties);
		this.pachinko.LevelInit(this.properties);
		foreach (Transform disc in this.starDiscs)
		{
			base.StartCoroutine(this.star_disc_cr(disc));
		}
	}

	// Token: 0x06000203 RID: 515 RVA: 0x000041FB File Offset: 0x000023FB
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacepachinkoPattern_cr());
	}

	// Token: 0x06000204 RID: 516 RVA: 0x000626BC File Offset: 0x000608BC
	public IEnumerator star_disc_cr(Transform disc)
	{
		bool fadingOut = Rand.Bool();
		for (;;)
		{
			float fadeTime = Random.Range(0.1f, 0.3f);
			float holdTime = Random.Range(0.1f, 0.3f);
			yield return CupheadTime.WaitForSeconds(this, holdTime);
			if (fadingOut)
			{
				float t = 0f;
				while (t < fadeTime)
				{
					disc.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - t / fadeTime);
					t += CupheadTime.Delta;
					yield return null;
				}
			}
			else
			{
				float t2 = 0f;
				while (t2 < fadeTime)
				{
					disc.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, t2 / fadeTime);
					t2 += CupheadTime.Delta;
					yield return null;
				}
			}
			fadingOut = !fadingOut;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000205 RID: 517 RVA: 0x000626E0 File Offset: 0x000608E0
	public IEnumerator dicepalacepachinkoPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000206 RID: 518 RVA: 0x000626FC File Offset: 0x000608FC
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalacePachinko.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x0400016B RID: 363
	public LevelProperties.DicePalacePachinko properties;

	// Token: 0x0400016C RID: 364
	[SerializeField]
	public Transform[] starDiscs;

	// Token: 0x0400016D RID: 365
	[SerializeField]
	public DicePalacePachinkoLevelPipes pipes;

	// Token: 0x0400016E RID: 366
	[SerializeField]
	public DicePalacePachinkoLevelPachinko pachinko;

	// Token: 0x0400016F RID: 367
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000170 RID: 368
	[SerializeField]
	public string _bossQuote;
}
