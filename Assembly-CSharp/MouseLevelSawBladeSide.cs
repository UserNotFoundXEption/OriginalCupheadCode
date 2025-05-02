using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002D2 RID: 722
public class MouseLevelSawBladeSide : AbstractPausableComponent
{
	// Token: 0x170002DE RID: 734
	// (get) Token: 0x06002014 RID: 8212 RVA: 0x0001B2F7 File Offset: 0x000194F7
	// (set) Token: 0x06002015 RID: 8213 RVA: 0x0001B2FF File Offset: 0x000194FF
	public MouseLevelSawBladeSide.State state { get; set; }

	// Token: 0x06002016 RID: 8214 RVA: 0x000B71A4 File Offset: 0x000B53A4
	public void Begin(LevelProperties.Mouse properties)
	{
		this.properties = properties;
		foreach (MouseLevelSawBlade mouseLevelSawBlade in this.sawBlades)
		{
			mouseLevelSawBlade.Begin(properties);
		}
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06002017 RID: 8215 RVA: 0x000B71EC File Offset: 0x000B53EC
	public void Leave()
	{
		this.StopAllCoroutines();
		foreach (MouseLevelSawBlade mouseLevelSawBlade in this.sawBlades)
		{
			mouseLevelSawBlade.Leave();
		}
	}

	// Token: 0x06002018 RID: 8216 RVA: 0x0001B308 File Offset: 0x00019508
	public void SetPattern(string pattern)
	{
		this.pattern = pattern.Split(new char[]
		{
			','
		});
		this.patternIndex = Random.Range(0, this.pattern.Length);
	}

	// Token: 0x06002019 RID: 8217 RVA: 0x0001B335 File Offset: 0x00019535
	public void FullAttack()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.fullAttack_cr());
	}

	// Token: 0x0600201A RID: 8218 RVA: 0x000B7224 File Offset: 0x000B5424
	public IEnumerator intro_cr()
	{
		while (this.sawBlades[0].state != MouseLevelSawBlade.State.Idle)
		{
			yield return null;
		}
		this.state = MouseLevelSawBladeSide.State.Pattern;
		base.StartCoroutine(this.pattern_cr());
		yield break;
	}

	// Token: 0x0600201B RID: 8219 RVA: 0x000B7240 File Offset: 0x000B5440
	public IEnumerator pattern_cr()
	{
		if (this.pattern == null)
		{
			yield break;
		}
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.brokenCanSawBlades.delayBeforeNextSaw);
			int sawBladeIndex = 0;
			Parser.IntTryParse(this.pattern[this.patternIndex], out sawBladeIndex);
			this.sawBlades[sawBladeIndex - 1].Attack();
			this.patternIndex = (this.patternIndex + 1) % this.pattern.Length;
		}
		yield break;
	}

	// Token: 0x0600201C RID: 8220 RVA: 0x000B725C File Offset: 0x000B545C
	public IEnumerator fullAttack_cr()
	{
		this.state = MouseLevelSawBladeSide.State.FullAttack;
		bool canFullAttack = false;
		while (!canFullAttack)
		{
			canFullAttack = true;
			foreach (MouseLevelSawBlade mouseLevelSawBlade in this.sawBlades)
			{
				if (mouseLevelSawBlade.state != MouseLevelSawBlade.State.Idle)
				{
					canFullAttack = false;
				}
			}
			yield return null;
		}
		AudioManager.Play("level_mouse_buzzsaw_wall");
		foreach (MouseLevelSawBlade mouseLevelSawBlade2 in this.sawBlades)
		{
			mouseLevelSawBlade2.FullAttack();
		}
		while (this.sawBlades[0].state != MouseLevelSawBlade.State.Idle)
		{
			yield return null;
		}
		this.state = MouseLevelSawBladeSide.State.Pattern;
		base.StartCoroutine(this.pattern_cr());
		yield break;
	}

	// Token: 0x04001A1B RID: 6683
	[SerializeField]
	public MouseLevelSawBlade[] sawBlades;

	// Token: 0x04001A1C RID: 6684
	public LevelProperties.Mouse properties;

	// Token: 0x04001A1D RID: 6685
	public string[] pattern;

	// Token: 0x04001A1E RID: 6686
	public int patternIndex;

	// Token: 0x02000DD3 RID: 3539
	public enum State
	{
		// Token: 0x040063E7 RID: 25575
		Init,
		// Token: 0x040063E8 RID: 25576
		Pattern,
		// Token: 0x040063E9 RID: 25577
		FullAttack
	}
}
