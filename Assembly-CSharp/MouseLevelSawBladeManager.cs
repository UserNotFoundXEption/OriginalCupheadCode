using System;
using System.Collections;
using System.Linq;
using UnityEngine;

// Token: 0x020002D1 RID: 721
public class MouseLevelSawBladeManager : AbstractPausableComponent
{
	// Token: 0x0600200F RID: 8207 RVA: 0x0001B296 File Offset: 0x00019496
	public void Begin(LevelProperties.Mouse properties)
	{
		this.properties = properties;
		this.leftSawBlades.Begin(properties);
		this.rightSawBlades.Begin(properties);
		base.StartCoroutine(this.pattern_cr());
		base.StartCoroutine(this.fullAttack_cr());
	}

	// Token: 0x06002010 RID: 8208 RVA: 0x0001B2D1 File Offset: 0x000194D1
	public void Leave()
	{
		this.StopAllCoroutines();
		this.leftSawBlades.Leave();
		this.rightSawBlades.Leave();
	}

	// Token: 0x06002011 RID: 8209 RVA: 0x000B716C File Offset: 0x000B536C
	public IEnumerator pattern_cr()
	{
		LevelProperties.Mouse.State patternState = this.properties.CurrentState;
		if (patternState.brokenCanSawBlades.patternString.Length == 0)
		{
			yield break;
		}
		string patternString = patternState.brokenCanSawBlades.patternString.RandomChoice<string>();
		this.leftSawBlades.SetPattern(patternString);
		this.rightSawBlades.SetPattern(patternString);
		for (;;)
		{
			if (this.properties.CurrentState != patternState)
			{
				if (!patternState.brokenCanSawBlades.patternString.SequenceEqual(this.properties.CurrentState.brokenCanSawBlades.patternString))
				{
					patternString = this.properties.CurrentState.brokenCanSawBlades.patternString.RandomChoice<string>();
					this.leftSawBlades.SetPattern(patternString);
					this.rightSawBlades.SetPattern(patternString);
				}
				patternState = this.properties.CurrentState;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002012 RID: 8210 RVA: 0x000B7188 File Offset: 0x000B5388
	public IEnumerator fullAttack_cr()
	{
		for (;;)
		{
			while (this.leftSawBlades.state != MouseLevelSawBladeSide.State.Pattern || this.rightSawBlades.state != MouseLevelSawBladeSide.State.Pattern)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.brokenCanSawBlades.fullAttackTime.RandomFloat());
			AbstractPlayerController player = PlayerManager.GetNext();
			if (player.transform.position.x > 0f)
			{
				this.rightSawBlades.FullAttack();
			}
			else
			{
				this.leftSawBlades.FullAttack();
			}
		}
		yield break;
	}

	// Token: 0x04001A18 RID: 6680
	[SerializeField]
	public MouseLevelSawBladeSide leftSawBlades;

	// Token: 0x04001A19 RID: 6681
	[SerializeField]
	public MouseLevelSawBladeSide rightSawBlades;

	// Token: 0x04001A1A RID: 6682
	public LevelProperties.Mouse properties;

	// Token: 0x02000DD0 RID: 3536
	public enum State
	{
		// Token: 0x040063D7 RID: 25559
		Init,
		// Token: 0x040063D8 RID: 25560
		Idle,
		// Token: 0x040063D9 RID: 25561
		Warning,
		// Token: 0x040063DA RID: 25562
		Attack
	}
}
