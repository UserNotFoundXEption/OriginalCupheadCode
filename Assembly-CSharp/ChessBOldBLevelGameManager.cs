using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000176 RID: 374
public class ChessBOldBLevelGameManager : AbstractPausableComponent
{
	// Token: 0x17000246 RID: 582
	// (get) Token: 0x060011F1 RID: 4593 RVA: 0x0000F2AC File Offset: 0x0000D4AC
	// (set) Token: 0x060011F2 RID: 4594 RVA: 0x0000F2B4 File Offset: 0x0000D4B4
	public bool WaitingForParry { get; set; }

	// Token: 0x060011F3 RID: 4595 RVA: 0x0000F2BD File Offset: 0x0000D4BD
	public void SetupGameManager(LevelProperties.ChessBOldB properties)
	{
		this.properties = properties;
		this.goingClockwise = Rand.Bool();
		this.InitBalls();
	}

	// Token: 0x060011F4 RID: 4596 RVA: 0x000939E8 File Offset: 0x00091BE8
	public void InitBalls()
	{
		this.birdies = new ChessBOldBReduxLevelBirdie[6];
		Vector3 position;
		position..ctor(0f, 1000f);
		for (int i = 0; i < 6; i++)
		{
			this.birdies[i] = Object.Instantiate<ChessBOldBReduxLevelBirdie>(this.birdiePrefab);
			this.birdies[i].transform.position = position;
			this.birdies[i].ParryBirdie = new ChessBOldBReduxLevelBirdie.OnParryBirdie(this.ParriedBall);
		}
		base.StartCoroutine(this.game_cr());
	}

	// Token: 0x060011F5 RID: 4597 RVA: 0x00093A70 File Offset: 0x00091C70
	public IEnumerator game_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 4f);
		LevelProperties.ChessBOldB.Birdie p = this.properties.CurrentState.birdie;
		YieldInstruction wait = new WaitForFixedUpdate();
		this.OnStateChanged();
		float t = 0f;
		float spinTime = 0f;
		float speedTime = 0f;
		int timesToSwitchDir = 0;
		for (;;)
		{
			t = 0f;
			yield return base.StartCoroutine(this.flash_balls_cr());
			this.WaitingForParry = true;
			p = this.properties.CurrentState.birdie;
			this.spinSpeedString = p.spinSpeedString[this.spinSpeedStringMainIndex].Split(new char[]
			{
				','
			});
			this.spinTimeString = p.spinTimeString[this.spinTimeStringMainIndex].Split(new char[]
			{
				','
			});
			this.changeDirString = p.changeDirectionString[this.changeDirStringMainIndex].Split(new char[]
			{
				','
			});
			this.initialDirString = p.initialDirectionString[this.initialDirStringMainIndex].Split(new char[]
			{
				','
			});
			Parser.FloatTryParse(this.spinTimeString[this.spinTimeStringIndex], out spinTime);
			Parser.FloatTryParse(this.spinSpeedString[this.spinSpeedStringIndex], out speedTime);
			Parser.IntTryParse(this.changeDirString[this.changeDirStringIndex], out timesToSwitchDir);
			this.goingClockwise = (this.initialDirString[this.initialDirStringIndex][0] == 'R');
			for (int i = 0; i < this.birdies.Length; i++)
			{
				this.birdies[i].HandleMovement(speedTime, this.goingClockwise);
			}
			float timeUntilSwitch = spinTime / (float)timesToSwitchDir;
			int dirCounter = 0;
			bool turnedPink = false;
			while (t < spinTime)
			{
				if (!this.WaitingForParry)
				{
					break;
				}
				t += CupheadTime.FixedDelta;
				if (!turnedPink && t >= p.prePinkTime)
				{
					for (int j = 0; j < this.birdies.Length; j++)
					{
						this.birdies[j].TurnPink();
					}
					turnedPink = true;
				}
				if (dirCounter < timesToSwitchDir && t > timeUntilSwitch * (float)dirCounter)
				{
					dirCounter++;
					this.goingClockwise = !this.goingClockwise;
					for (int k = 0; k < this.birdies.Length; k++)
					{
						this.birdies[k].HandleMovement(speedTime, this.goingClockwise);
					}
				}
				yield return wait;
			}
			for (int l = 0; l < this.birdies.Length; l++)
			{
				this.birdies[l].StopMoving();
			}
			while (this.WaitingForParry)
			{
				yield return null;
			}
			if (this.spinSpeedStringIndex < this.spinSpeedString.Length - 1)
			{
				this.spinSpeedStringIndex++;
			}
			else
			{
				this.spinSpeedStringMainIndex = (this.spinSpeedStringMainIndex + 1) % p.spinSpeedString.Length;
				this.spinSpeedStringIndex = 0;
			}
			if (this.spinTimeStringIndex < this.spinTimeString.Length - 1)
			{
				this.spinTimeStringIndex++;
			}
			else
			{
				this.spinTimeStringMainIndex = (this.spinTimeStringMainIndex + 1) % p.spinTimeString.Length;
				this.spinTimeStringIndex = 0;
			}
			if (this.changeDirStringIndex < this.changeDirString.Length - 1)
			{
				this.changeDirStringIndex++;
			}
			else
			{
				this.changeDirStringMainIndex = (this.changeDirStringMainIndex + 1) % p.changeDirectionString.Length;
				this.changeDirStringIndex = 0;
			}
			if (this.initialDirStringIndex < this.initialDirString.Length - 1)
			{
				this.initialDirStringIndex++;
			}
			else
			{
				this.initialDirStringMainIndex = (this.initialDirStringMainIndex + 1) % p.initialDirectionString.Length;
				this.initialDirStringIndex = 0;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060011F6 RID: 4598 RVA: 0x0000F2D7 File Offset: 0x0000D4D7
	public void ParriedBall(bool chosenBall)
	{
		if (chosenBall)
		{
			this.WaitingForParry = false;
			this.properties.DealDamage(1f);
			this.boss.HandleHurt(true);
		}
	}

	// Token: 0x060011F7 RID: 4599 RVA: 0x00093A8C File Offset: 0x00091C8C
	public void OnStateChanged()
	{
		LevelProperties.ChessBOldB.Birdie birdie = this.properties.CurrentState.birdie;
		this.spinSpeedStringMainIndex = Random.Range(0, birdie.spinSpeedString.Length);
		this.spinSpeedString = birdie.spinSpeedString[this.spinSpeedStringMainIndex].Split(new char[]
		{
			','
		});
		this.spinSpeedStringIndex = Random.Range(0, this.spinSpeedString.Length);
		this.spinTimeStringMainIndex = Random.Range(0, birdie.spinTimeString.Length);
		this.spinTimeString = birdie.spinTimeString[this.spinTimeStringMainIndex].Split(new char[]
		{
			','
		});
		this.spinTimeStringIndex = Random.Range(0, this.spinTimeString.Length);
		this.changeDirStringMainIndex = Random.Range(0, birdie.changeDirectionString.Length);
		this.changeDirString = birdie.changeDirectionString[this.changeDirStringMainIndex].Split(new char[]
		{
			','
		});
		this.changeDirStringIndex = Random.Range(0, this.changeDirString.Length);
		this.initialDirStringMainIndex = Random.Range(0, birdie.initialDirectionString.Length);
		this.initialDirString = birdie.initialDirectionString[this.initialDirStringMainIndex].Split(new char[]
		{
			','
		});
		this.initialDirStringIndex = Random.Range(0, this.initialDirString.Length);
		this.chosenStringMainIndex = Random.Range(0, birdie.chosenString.Length);
		this.chosenString = birdie.chosenString[this.chosenStringMainIndex].Split(new char[]
		{
			','
		});
		this.chosenStringIndex = Random.Range(0, this.chosenString.Length);
	}

	// Token: 0x060011F8 RID: 4600 RVA: 0x00093C24 File Offset: 0x00091E24
	public IEnumerator flash_balls_cr()
	{
		LevelProperties.ChessBOldB.Birdie p = this.properties.CurrentState.birdie;
		for (int i = 0; i < this.birdies.Length; i++)
		{
			this.birdies[i].transform.position = new Vector3(0f, 1000f);
		}
		yield return CupheadTime.WaitForSeconds(this, p.fadeInTime);
		this.boss.HandleHurt(false);
		float angleOffset = 60f;
		int chosenIndex = 0;
		this.chosenString = p.chosenString[this.chosenStringMainIndex].Split(new char[]
		{
			','
		});
		Parser.IntTryParse(this.chosenString[this.chosenStringIndex], out chosenIndex);
		for (int j = 0; j < this.birdies.Length; j++)
		{
			bool chosenBall = j == chosenIndex;
			this.birdies[j].Setup(this.pivotPoint, angleOffset * (float)j, this.properties.CurrentState.birdie, 370f, chosenBall);
		}
		Color color = this.birdies[chosenIndex].GetComponent<SpriteRenderer>().color;
		this.birdies[chosenIndex].GetComponent<SpriteRenderer>().color = this.redFlash;
		yield return CupheadTime.WaitForSeconds(this, p.flashTime);
		this.birdies[chosenIndex].GetComponent<SpriteRenderer>().color = color;
		if (this.chosenStringIndex < this.chosenString.Length - 1)
		{
			this.chosenStringIndex++;
		}
		else
		{
			this.chosenStringMainIndex = (this.chosenStringMainIndex + 1) % p.chosenString.Length;
			this.chosenStringIndex = 0;
		}
		yield return null;
		yield break;
	}

	// Token: 0x04000E6D RID: 3693
	public const int NUM_OF_BALLS = 6;

	// Token: 0x04000E6E RID: 3694
	public const float LOOP_SIZE = 370f;

	// Token: 0x04000E6F RID: 3695
	[SerializeField]
	public Color redFlash;

	// Token: 0x04000E70 RID: 3696
	[SerializeField]
	public Transform pivotPoint;

	// Token: 0x04000E71 RID: 3697
	[SerializeField]
	public ChessBOldBLevelBoss boss;

	// Token: 0x04000E72 RID: 3698
	[SerializeField]
	public ChessBOldBReduxLevelBirdie birdiePrefab;

	// Token: 0x04000E73 RID: 3699
	public ChessBOldBReduxLevelBirdie[] birdies;

	// Token: 0x04000E74 RID: 3700
	public LevelProperties.ChessBOldB properties;

	// Token: 0x04000E76 RID: 3702
	public bool goingClockwise;

	// Token: 0x04000E77 RID: 3703
	public int spinSpeedStringMainIndex;

	// Token: 0x04000E78 RID: 3704
	public string[] spinSpeedString;

	// Token: 0x04000E79 RID: 3705
	public int spinSpeedStringIndex;

	// Token: 0x04000E7A RID: 3706
	public int spinTimeStringMainIndex;

	// Token: 0x04000E7B RID: 3707
	public string[] spinTimeString;

	// Token: 0x04000E7C RID: 3708
	public int spinTimeStringIndex;

	// Token: 0x04000E7D RID: 3709
	public int changeDirStringMainIndex;

	// Token: 0x04000E7E RID: 3710
	public string[] changeDirString;

	// Token: 0x04000E7F RID: 3711
	public int changeDirStringIndex;

	// Token: 0x04000E80 RID: 3712
	public int initialDirStringMainIndex;

	// Token: 0x04000E81 RID: 3713
	public string[] initialDirString;

	// Token: 0x04000E82 RID: 3714
	public int initialDirStringIndex;

	// Token: 0x04000E83 RID: 3715
	public int chosenStringMainIndex;

	// Token: 0x04000E84 RID: 3716
	public string[] chosenString;

	// Token: 0x04000E85 RID: 3717
	public int chosenStringIndex;
}
