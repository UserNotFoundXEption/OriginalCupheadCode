using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000035 RID: 53
public class MouseLevel : Level
{
	// Token: 0x06000331 RID: 817 RVA: 0x00065AB8 File Offset: 0x00063CB8
	public override void PartialInit()
	{
		this.properties = LevelProperties.Mouse.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x06000332 RID: 818 RVA: 0x00004A56 File Offset: 0x00002C56
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Mouse;
		}
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06000333 RID: 819 RVA: 0x00004A5D File Offset: 0x00002C5D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_mouse;
		}
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000334 RID: 820 RVA: 0x00065B50 File Offset: 0x00063D50
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Mouse.States.Main:
			case LevelProperties.Mouse.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Mouse.States.BrokenCan:
				return this._bossPortraitBrokenCan;
			case LevelProperties.Mouse.States.Cat:
				return this._bossPortraitCat;
			default:
				Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06000335 RID: 821 RVA: 0x00065BD0 File Offset: 0x00063DD0
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Mouse.States.Main:
			case LevelProperties.Mouse.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Mouse.States.BrokenCan:
				return this._bossQuoteBrokenCan;
			case LevelProperties.Mouse.States.Cat:
				return this._bossQuoteCat;
			default:
				Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x06000336 RID: 822 RVA: 0x00004A61 File Offset: 0x00002C61
	public override void Start()
	{
		base.Start();
		this.mouseCan.LevelInit(this.properties);
		this.mouseBrokenCan.LevelInit(this.properties);
		this.cat.LevelInit(this.properties);
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00004A9C File Offset: 0x00002C9C
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.mousePattern_cr());
	}

	// Token: 0x06000338 RID: 824 RVA: 0x00065C50 File Offset: 0x00063E50
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Mouse.States.BrokenCan)
		{
			this.StopAllCoroutines();
			this.mouseCan.Explode(new Action(this.StartMouseCanPlatform), new Action(this.OnMouseCanTransitionComplete));
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Mouse.States.Cat)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.catIntro_cr());
		}
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00004AAB File Offset: 0x00002CAB
	public void StartMouseCanPlatform()
	{
		this.wallAnimator.SetTrigger("OnContinue");
		AudioManager.Play("level_mouse_phase2_background_shelf_drop");
	}

	// Token: 0x0600033A RID: 826 RVA: 0x00004AC7 File Offset: 0x00002CC7
	public void OnMouseCanTransitionComplete()
	{
		base.StartCoroutine(this.mousePattern_cr());
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00004AD6 File Offset: 0x00002CD6
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitBrokenCan = null;
		this._bossPortraitCat = null;
		this._bossPortraitMain = null;
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00065CD0 File Offset: 0x00063ED0
	public IEnumerator mousePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.canMove.initialHesitate);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600033D RID: 829 RVA: 0x00065CEC File Offset: 0x00063EEC
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.Mouse.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.Mouse.Pattern.Dash || this.hasMoved)
		{
			this.hasMoved = true;
			switch (p)
			{
			case LevelProperties.Mouse.Pattern.Dash:
				yield return base.StartCoroutine(this.canDash_cr());
				break;
			case LevelProperties.Mouse.Pattern.CherryBomb:
				yield return base.StartCoroutine(this.cherryBomb_cr());
				break;
			case LevelProperties.Mouse.Pattern.Catapult:
				yield return base.StartCoroutine(this.catapult_cr());
				break;
			case LevelProperties.Mouse.Pattern.RomanCandle:
				yield return base.StartCoroutine(this.romanCandle_cr());
				break;
			default:
				yield return CupheadTime.WaitForSeconds(this, 1f);
				break;
			case LevelProperties.Mouse.Pattern.LeftClaw:
				yield return base.StartCoroutine(this.claw_cr(true));
				break;
			case LevelProperties.Mouse.Pattern.RightClaw:
				yield return base.StartCoroutine(this.claw_cr(false));
				break;
			case LevelProperties.Mouse.Pattern.GhostMouse:
				yield return base.StartCoroutine(this.ghostMouse_cr());
				break;
			}
		}
		yield break;
	}

	// Token: 0x0600033E RID: 830 RVA: 0x00065D08 File Offset: 0x00063F08
	public IEnumerator canDash_cr()
	{
		while (this.mouseCan.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		this.mouseCan.StartDash();
		while (this.mouseCan.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600033F RID: 831 RVA: 0x00065D24 File Offset: 0x00063F24
	public IEnumerator cherryBomb_cr()
	{
		while (this.mouseCan.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		this.mouseCan.StartCherryBomb();
		while (this.mouseCan.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000340 RID: 832 RVA: 0x00065D40 File Offset: 0x00063F40
	public IEnumerator catapult_cr()
	{
		while (this.mouseCan.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		this.mouseCan.StartCatapult();
		while (this.mouseCan.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000341 RID: 833 RVA: 0x00065D5C File Offset: 0x00063F5C
	public IEnumerator romanCandle_cr()
	{
		while (this.mouseCan.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		this.mouseCan.StartRomanCandle();
		while (this.mouseCan.state != MouseLevelCanMouse.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000342 RID: 834 RVA: 0x00065D78 File Offset: 0x00063F78
	public IEnumerator claw_cr(bool left)
	{
		while (this.cat.state != MouseLevelCat.State.Idle)
		{
			yield return null;
		}
		this.cat.StartClaw(left);
		while (this.cat.state != MouseLevelCat.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000343 RID: 835 RVA: 0x00065D9C File Offset: 0x00063F9C
	public IEnumerator ghostMouse_cr()
	{
		while (this.cat.state != MouseLevelCat.State.Idle)
		{
			yield return null;
		}
		this.cat.StartGhostMouse();
		while (this.cat.state != MouseLevelCat.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000344 RID: 836 RVA: 0x00065DB8 File Offset: 0x00063FB8
	public IEnumerator catIntro_cr()
	{
		this.mouseBrokenCan.Transform();
		while (this.cat.state != MouseLevelCat.State.Idle)
		{
			yield return null;
		}
		base.StartCoroutine(this.mousePattern_cr());
		yield break;
	}

	// Token: 0x04000236 RID: 566
	public LevelProperties.Mouse properties;

	// Token: 0x04000237 RID: 567
	[SerializeField]
	public MouseLevelCanMouse mouseCan;

	// Token: 0x04000238 RID: 568
	[SerializeField]
	public MouseLevelBrokenCanMouse mouseBrokenCan;

	// Token: 0x04000239 RID: 569
	[SerializeField]
	public MouseLevelCat cat;

	// Token: 0x0400023A RID: 570
	[SerializeField]
	public Animator wallAnimator;

	// Token: 0x0400023B RID: 571
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x0400023C RID: 572
	[SerializeField]
	public Sprite _bossPortraitBrokenCan;

	// Token: 0x0400023D RID: 573
	[SerializeField]
	public Sprite _bossPortraitCat;

	// Token: 0x0400023E RID: 574
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x0400023F RID: 575
	[SerializeField]
	public string _bossQuoteBrokenCan;

	// Token: 0x04000240 RID: 576
	[SerializeField]
	public string _bossQuoteCat;

	// Token: 0x04000241 RID: 577
	public bool hasMoved;

	// Token: 0x020007FA RID: 2042
	[Serializable]
	public class Prefabs
	{
	}
}
