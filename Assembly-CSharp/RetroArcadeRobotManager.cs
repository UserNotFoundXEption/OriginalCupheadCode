using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000314 RID: 788
public class RetroArcadeRobotManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x060022B8 RID: 8888 RVA: 0x0001D865 File Offset: 0x0001BA65
	public void StartRobots()
	{
		this.p = base.properties.CurrentState.robots;
		this.phase = 0;
		base.StartCoroutine(this.bonus_cr());
		this.StartNewPhase();
	}

	// Token: 0x060022B9 RID: 8889 RVA: 0x000BE694 File Offset: 0x000BC894
	public void StartNewPhase()
	{
		this.numDied = 0;
		string[] array = this.p.robotWaves[this.phase].Split(new char[]
		{
			','
		});
		this.numRobotsToKill = array.Length;
		for (int i = 0; i < this.numRobotsToKill; i++)
		{
			int num;
			Parser.IntTryParse(array[i], out num);
			if (num > 0 && num <= this.p.robotsXPositions.Length)
			{
				string[] array2 = this.p.robotColorPattern[this.phase].Split(new char[]
				{
					','
				});
				string[] orbiterPattern = array2[i].Split(new char[]
				{
					'-'
				});
				float xPos = this.p.robotsXPositions[num - 1];
				this.bigRobotPrefab.Create(xPos, this.p, (float)i / 3f, this, orbiterPattern);
			}
		}
	}

	// Token: 0x060022BA RID: 8890 RVA: 0x000BE778 File Offset: 0x000BC978
	public IEnumerator bonus_cr()
	{
		for (int i = 0; i < this.p.bonusCount; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, this.p.bonusDelay.RandomFloat());
			this.bonusRobotPrefab.Create((!Rand.Bool()) ? RetroArcadeBonusRobot.Direction.Right : RetroArcadeBonusRobot.Direction.Left, this.p);
		}
		yield break;
	}

	// Token: 0x060022BB RID: 8891 RVA: 0x000BE794 File Offset: 0x000BC994
	public void OnRobotGroupDie()
	{
		this.numDied++;
		if (this.numDied >= this.numRobotsToKill)
		{
			if (this.phase >= this.p.robotWaves.Length - 1)
			{
				base.properties.DealDamageToNextNamedState();
				this.StopAllCoroutines();
			}
			else
			{
				this.phase++;
				this.StartNewPhase();
			}
		}
	}

	// Token: 0x04001CB1 RID: 7345
	public const float BIG_ROBOT_SPACING = 160f;

	// Token: 0x04001CB2 RID: 7346
	[SerializeField]
	public RetroArcadeBigRobot bigRobotPrefab;

	// Token: 0x04001CB3 RID: 7347
	[SerializeField]
	public RetroArcadeBonusRobot bonusRobotPrefab;

	// Token: 0x04001CB4 RID: 7348
	public LevelProperties.RetroArcade.Robots p;

	// Token: 0x04001CB5 RID: 7349
	public int numDied;

	// Token: 0x04001CB6 RID: 7350
	public int phase;

	// Token: 0x04001CB7 RID: 7351
	public int numRobotsToKill;
}
