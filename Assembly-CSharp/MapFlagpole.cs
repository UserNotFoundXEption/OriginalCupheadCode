using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200047C RID: 1148
public class MapFlagpole : AbstractMapLevelDependentEntity
{
	// Token: 0x1700038E RID: 910
	// (get) Token: 0x06003097 RID: 12439 RVA: 0x00028565 File Offset: 0x00026765
	public override bool ReactToDifficultyChange
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700038F RID: 911
	// (get) Token: 0x06003098 RID: 12440 RVA: 0x00028568 File Offset: 0x00026768
	public override bool ReactToGradeChange
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003099 RID: 12441 RVA: 0x0002856B File Offset: 0x0002676B
	public override void OnConditionMet()
	{
		if (Level.PreviouslyWon || this.forceNoAppearAnimation)
		{
			this.Init(this.difficulty, this.grade);
		}
	}

	// Token: 0x0600309A RID: 12442 RVA: 0x00028594 File Offset: 0x00026794
	public override void DoTransition()
	{
		if (Level.PreviouslyWon)
		{
			base.StartCoroutine(this.shake_cr());
		}
		else
		{
			base.StartCoroutine(this.raise_cr());
		}
	}

	// Token: 0x0600309B RID: 12443 RVA: 0x000285BF File Offset: 0x000267BF
	public override void OnConditionAlreadyMet()
	{
		this.Init(this.difficulty, this.grade);
	}

	// Token: 0x0600309C RID: 12444 RVA: 0x000285D3 File Offset: 0x000267D3
	public override void OnConditionNotMet()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600309D RID: 12445 RVA: 0x000E6BEC File Offset: 0x000E4DEC
	public void Init(Level.Mode difficulty, LevelScoringData.Grade grade)
	{
		if (this._levels.Length == 0)
		{
			return;
		}
		string text = string.Empty;
		bool flag = false;
		for (int i = 0; i < Level.platformingLevels.Length; i++)
		{
			if (Level.platformingLevels[i] == this._levels[0])
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			if (grade < LevelScoringData.Grade.AMinus)
			{
				text = "IdleBelowA";
			}
			else if (grade < LevelScoringData.Grade.P)
			{
				text = "IdleBelowP";
			}
			else
			{
				text = "IdleP";
			}
		}
		else if (difficulty == Level.Mode.Easy)
		{
			text = "IdleEasy";
		}
		else if (difficulty == Level.Mode.Normal && grade < LevelScoringData.Grade.AMinus)
		{
			text = "IdleNormalBelowA";
		}
		else if (difficulty == Level.Mode.Normal && grade >= LevelScoringData.Grade.AMinus)
		{
			text = "IdleNormalA";
		}
		else if (difficulty == Level.Mode.Hard && grade < LevelScoringData.Grade.S)
		{
			text = "IdleExpert";
		}
		else if (difficulty == Level.Mode.Hard && grade >= LevelScoringData.Grade.S)
		{
			text = "IdleExpertS";
		}
		base.animator.Play(text);
		if (this.forceNoAppearAnimation)
		{
			base.CurrentState = AbstractMapLevelDependentEntity.State.Complete;
		}
	}

	// Token: 0x0600309E RID: 12446 RVA: 0x000E6D0C File Offset: 0x000E4F0C
	public IEnumerator raise_cr()
	{
		if (this._levels.Length == 0)
		{
			yield break;
		}
		string trigger = string.Empty;
		bool platformingLevel = false;
		for (int i = 0; i < Level.platformingLevels.Length; i++)
		{
			if (Level.platformingLevels[i] == this._levels[0])
			{
				platformingLevel = true;
				break;
			}
		}
		if (platformingLevel)
		{
			if (this.grade < LevelScoringData.Grade.AMinus)
			{
				trigger = "RaiseBelowA";
			}
			else if (this.grade < LevelScoringData.Grade.P)
			{
				trigger = "RaiseBelowP";
			}
			else
			{
				trigger = "RaiseP";
			}
		}
		else if (this.difficulty == Level.Mode.Easy)
		{
			trigger = "RaiseEasy";
		}
		else if (this.difficulty == Level.Mode.Normal && this.grade < LevelScoringData.Grade.AMinus)
		{
			trigger = "RaiseNormalBelowA";
		}
		else if (this.difficulty == Level.Mode.Normal && this.grade >= LevelScoringData.Grade.AMinus)
		{
			trigger = "RaiseNormalA";
		}
		else if (this.difficulty == Level.Mode.Hard && this.grade < LevelScoringData.Grade.S)
		{
			trigger = "RaiseExpert";
		}
		else if (this.difficulty == Level.Mode.Hard && this.grade >= LevelScoringData.Grade.S)
		{
			trigger = "RaiseExpertS";
		}
		base.animator.SetTrigger(trigger);
		if (PlayerManager.playerWasChalice[0])
		{
			AudioManager.Play("worldmap_level_raise_flag_chalice");
		}
		else if (PlayerManager.player1IsMugman)
		{
			AudioManager.Play("worldmap_level_raise_flag_mugman");
		}
		else
		{
			AudioManager.Play("world_map_flag_raise");
		}
		yield return base.animator.WaitForAnimationToEnd(this, trigger, false, true);
		base.CurrentState = AbstractMapLevelDependentEntity.State.Complete;
		yield break;
	}

	// Token: 0x0600309F RID: 12447 RVA: 0x000E6D28 File Offset: 0x000E4F28
	public IEnumerator shake_cr()
	{
		base.CurrentState = AbstractMapLevelDependentEntity.State.Complete;
		yield return null;
		yield break;
	}

	// Token: 0x0400282C RID: 10284
	[SerializeField]
	public bool forceNoAppearAnimation;
}
