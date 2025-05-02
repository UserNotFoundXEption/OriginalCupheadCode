using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class VeggiesLevel : Level
{
	// Token: 0x06000465 RID: 1125 RVA: 0x00069D54 File Offset: 0x00067F54
	public override void PartialInit()
	{
		this.properties = LevelProperties.Veggies.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06000466 RID: 1126 RVA: 0x00005238 File Offset: 0x00003438
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Veggies;
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06000467 RID: 1127 RVA: 0x0000523B File Offset: 0x0000343B
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_veggies;
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06000468 RID: 1128 RVA: 0x00069DEC File Offset: 0x00067FEC
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.currentBoss)
			{
			case VeggiesLevel.CurrentBoss.Potato:
				return this._bossPortraitPotato;
			case VeggiesLevel.CurrentBoss.Onion:
				return this._bossPortraitOnion;
			case VeggiesLevel.CurrentBoss.Carrot:
				return this._bossPortraitCarrot;
			}
			return this._bossPortraitPotato;
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06000469 RID: 1129 RVA: 0x00069E3C File Offset: 0x0006803C
	public override string BossQuote
	{
		get
		{
			switch (this.currentBoss)
			{
			case VeggiesLevel.CurrentBoss.Potato:
				return this._bossQuotePotato;
			case VeggiesLevel.CurrentBoss.Onion:
				return this._bossQuoteOnion;
			case VeggiesLevel.CurrentBoss.Carrot:
				return this._bossQuoteCarrot;
			}
			return "QuoteNone";
		}
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x00069E8C File Offset: 0x0006808C
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.potatoStart_cr());
		this.properties.OnBossDamaged -= base.timeline.DealDamage;
		base.timeline = new Level.Timeline();
		base.timeline.health = 0f;
		base.timeline.health += (float)this.properties.CurrentState.potato.hp;
		if (base.mode != Level.Mode.Easy)
		{
			base.timeline.health += (float)this.properties.CurrentState.onion.hp;
		}
		base.timeline.health += (float)this.properties.CurrentState.carrot.hp;
		if (base.mode != Level.Mode.Easy)
		{
			base.timeline.AddEventAtHealth("Onion", base.timeline.GetHealthOfLastEvent() + this.properties.CurrentState.potato.hp);
		}
		base.timeline.AddEventAtHealth("Carrot", base.timeline.GetHealthOfLastEvent() + ((base.mode != Level.Mode.Easy) ? this.properties.CurrentState.onion.hp : this.properties.CurrentState.potato.hp));
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x0000523F File Offset: 0x0000343F
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.prefabs = null;
		this._bossPortraitCarrot = null;
		this._bossPortraitOnion = null;
		this._bossPortraitPotato = null;
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x00005263 File Offset: 0x00003463
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.veggiesPattern_cr());
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x00069FF8 File Offset: 0x000681F8
	public IEnumerator veggiesPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield return base.StartCoroutine(this.potato_cr());
		if (base.mode != Level.Mode.Easy)
		{
			yield return base.StartCoroutine(this.onion_cr());
		}
		yield return base.StartCoroutine(this.carrot_cr());
		yield return base.StartCoroutine(this.win_cr());
		yield break;
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x0006A014 File Offset: 0x00068214
	public IEnumerator potatoStart_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.potato = this.prefabs.potato.InstantiatePrefab<VeggiesLevelPotato>();
		this.potato.OnDamageTakenEvent += base.timeline.DealDamage;
		yield break;
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0006A030 File Offset: 0x00068230
	public IEnumerator potato_cr()
	{
		this.currentBoss = VeggiesLevel.CurrentBoss.Potato;
		this.potato.LevelInitWithGroup(this.properties.CurrentState.potato);
		while (this.potato.state != VeggiesLevelPotato.State.Complete)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0006A04C File Offset: 0x0006824C
	public IEnumerator onion_cr()
	{
		this.currentBoss = VeggiesLevel.CurrentBoss.Onion;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		VeggiesLevelOnion v = this.prefabs.onion.InstantiatePrefab<VeggiesLevelOnion>();
		v.LevelInitWithGroup(this.properties.CurrentState.onion);
		v.OnDamageTakenEvent += base.timeline.DealDamage;
		v.OnHappyLeave += this.OnionHappyLeave;
		while (v.state != VeggiesLevelOnion.State.Complete)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x00005272 File Offset: 0x00003472
	public void OnionHappyLeave()
	{
		this.secretTriggered = true;
		base.timeline.DealDamage((float)this.properties.CurrentState.onion.hp);
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x0006A068 File Offset: 0x00068268
	public IEnumerator beet_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		VeggiesLevelBeet v = this.prefabs.beet.InstantiatePrefab<VeggiesLevelBeet>();
		v.LevelInitWithGroup(this.properties.CurrentState.beet);
		v.OnDamageTakenEvent += base.timeline.DealDamage;
		while (v.state != VeggiesLevelBeet.State.Complete)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x0006A084 File Offset: 0x00068284
	public IEnumerator peas_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		VeggiesLevelPeas v = this.prefabs.peas.InstantiatePrefab<VeggiesLevelPeas>();
		v.LevelInitWithGroup(this.properties.CurrentState.peas);
		v.OnDamageTakenEvent += base.timeline.DealDamage;
		while (v.state != VeggiesLevelPeas.State.Complete)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x0006A0A0 File Offset: 0x000682A0
	public IEnumerator carrot_cr()
	{
		this.currentBoss = VeggiesLevel.CurrentBoss.Carrot;
		VeggiesLevelCarrot v = this.prefabs.carrot.InstantiatePrefab<VeggiesLevelCarrot>();
		v.LevelInit(this.properties);
		v.OnDamageTakenEvent += base.timeline.DealDamage;
		while (v.state != VeggiesLevelCarrot.State.Complete)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x0006A0BC File Offset: 0x000682BC
	public IEnumerator win_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.properties.WinInstantly();
		yield break;
	}

	// Token: 0x04000336 RID: 822
	public LevelProperties.Veggies properties;

	// Token: 0x04000337 RID: 823
	[Space(10f)]
	[SerializeField]
	public VeggiesLevel.Prefabs prefabs;

	// Token: 0x04000338 RID: 824
	public VeggiesLevelPotato potato;

	// Token: 0x04000339 RID: 825
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitPotato;

	// Token: 0x0400033A RID: 826
	[SerializeField]
	public Sprite _bossPortraitOnion;

	// Token: 0x0400033B RID: 827
	[SerializeField]
	public Sprite _bossPortraitCarrot;

	// Token: 0x0400033C RID: 828
	[SerializeField]
	public string _bossQuotePotato;

	// Token: 0x0400033D RID: 829
	[SerializeField]
	public string _bossQuoteOnion;

	// Token: 0x0400033E RID: 830
	[SerializeField]
	public string _bossQuoteCarrot;

	// Token: 0x0400033F RID: 831
	public VeggiesLevel.CurrentBoss currentBoss;

	// Token: 0x02000861 RID: 2145
	public enum CurrentBoss
	{
		// Token: 0x040040ED RID: 16621
		Potato,
		// Token: 0x040040EE RID: 16622
		Onion,
		// Token: 0x040040EF RID: 16623
		Beet,
		// Token: 0x040040F0 RID: 16624
		Peas,
		// Token: 0x040040F1 RID: 16625
		Carrot
	}

	// Token: 0x02000862 RID: 2146
	[Serializable]
	public class Prefabs
	{
		// Token: 0x040040F2 RID: 16626
		public VeggiesLevelPotato potato;

		// Token: 0x040040F3 RID: 16627
		public VeggiesLevelOnion onion;

		// Token: 0x040040F4 RID: 16628
		public VeggiesLevelBeet beet;

		// Token: 0x040040F5 RID: 16629
		public VeggiesLevelPeas peas;

		// Token: 0x040040F6 RID: 16630
		public VeggiesLevelCarrot carrot;
	}
}
