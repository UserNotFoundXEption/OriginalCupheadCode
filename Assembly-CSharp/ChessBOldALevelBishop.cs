using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000172 RID: 370
public class ChessBOldALevelBishop : LevelProperties.ChessBOldA.Entity
{
	// Token: 0x060011BF RID: 4543 RVA: 0x0000F0FD File Offset: 0x0000D2FD
	public void Start()
	{
		this.walls = new List<ChessBOldALevelWall>();
		this.pink.OnActivate += this.GotParried;
		this.damageDealer = DamageDealer.NewEnemy();
		base.GetComponent<SpriteRenderer>().color = Color.red;
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x0000F13C File Offset: 0x0000D33C
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060011C1 RID: 4545 RVA: 0x0000F15A File Offset: 0x0000D35A
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x00092EA0 File Offset: 0x000910A0
	public override void LevelInit(LevelProperties.ChessBOldA properties)
	{
		base.LevelInit(properties);
		Level.Current.OnWinEvent += this.Win;
		LevelProperties.ChessBOldA.Bishop bishop = properties.CurrentState.bishop;
		if (!bishop.canHurtPlayer)
		{
			base.GetComponent<Collider2D>().enabled = false;
		}
		float num = properties.CurrentHealth / (float)properties.CurrentState.bishop.bishopHealth;
		this.HPToDecrease = Mathf.Ceil(num);
		base.transform.SetScale(new float?(bishop.bishopScale), new float?(bishop.bishopScale), new float?(bishop.bishopScale));
		this.pathIndex = 0;
		base.StartCoroutine(this.intro_cr());
		base.StartCoroutine(this.pink_cr());
		this.SetValues();
		base.StartCoroutine(this.turret_cr());
	}

	// Token: 0x060011C3 RID: 4547 RVA: 0x0000F172 File Offset: 0x0000D372
	public void GotParried()
	{
		base.properties.DealDamage(this.HPToDecrease);
		base.StartCoroutine(this.stunned_cr());
	}

	// Token: 0x060011C4 RID: 4548 RVA: 0x00092F74 File Offset: 0x00091174
	public IEnumerator stunned_cr()
	{
		this.RemoveCurrentWalls();
		this.walls.Clear();
		this.isStunned = true;
		this.pink.enabled = false;
		base.GetComponent<SpriteRenderer>().color = Color.yellow;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.bishop.stunnedTime);
		base.GetComponent<SpriteRenderer>().color = Color.red;
		this.pink.enabled = true;
		this.isStunned = false;
		if (base.properties.CurrentHealth > 0f)
		{
			this.phase++;
			this.SetValues();
			this.SetPathValues();
		}
		yield break;
	}

	// Token: 0x060011C5 RID: 4549 RVA: 0x0000F192 File Offset: 0x0000D392
	public void SetValues()
	{
		this.SetPinkValues();
		this.SetWallValues();
	}

	// Token: 0x060011C6 RID: 4550 RVA: 0x0000F1A0 File Offset: 0x0000D3A0
	public void Win()
	{
		this.StopAllCoroutines();
	}

	// Token: 0x060011C7 RID: 4551 RVA: 0x00092F90 File Offset: 0x00091190
	public IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 4f);
		this.SetPathValues();
		yield return null;
		yield break;
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x00092FAC File Offset: 0x000911AC
	public void SetPathValues()
	{
		LevelProperties.ChessBOldA.BishopPath bishopPath = base.properties.CurrentState.bishopPath;
		int num = Mathf.Clamp(this.phase, 0, bishopPath.pathTypeString.Split(new char[]
		{
			','
		}).Length - 1);
		int num2 = Mathf.Clamp(this.phase, 0, bishopPath.pathSpeedString.Split(new char[]
		{
			','
		}).Length - 1);
		int num3 = Mathf.Clamp(this.phase, 0, bishopPath.pathDirString.Split(new char[]
		{
			','
		}).Length - 1);
		Parser.FloatTryParse(bishopPath.pathSpeedString.Split(new char[]
		{
			','
		})[num2], out this.pathSpeed);
		this.pathIsClockwise = (bishopPath.pathDirString.Split(new char[]
		{
			','
		})[num3][0] == 'R');
		this.previousPathType = this.pathType;
		char c = bishopPath.pathTypeString.Split(new char[]
		{
			','
		})[num][0];
		if (c != 'S')
		{
			if (c != 'I')
			{
				if (c == 'Q')
				{
					this.pathType = ChessBOldALevelBishop.PathType.Square;
					base.StartCoroutine(this.box_cr());
				}
			}
			else
			{
				this.pathType = ChessBOldALevelBishop.PathType.Infinite;
				base.StartCoroutine(this.infinite_cr());
			}
		}
		else
		{
			this.pathType = ChessBOldALevelBishop.PathType.Straight;
			base.StartCoroutine(this.straight_cr());
		}
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x00093124 File Offset: 0x00091324
	public IEnumerator move_cr(Vector3 start, Vector3 end)
	{
		float t = 0f;
		float time = this.pathSpeed;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			base.transform.position = Vector3.Lerp(start, end, t / time);
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x00093150 File Offset: 0x00091350
	public IEnumerator straight_cr()
	{
		LevelProperties.ChessBOldA.BishopPath p = base.properties.CurrentState.bishopPath;
		float t = this.lerpPos;
		float maxTime = this.pathSpeed;
		YieldInstruction wait = new WaitForFixedUpdate();
		float startX = -p.straightPathLength;
		float endX = p.straightPathLength;
		float one = 1f;
		if (this.previousPathType != ChessBOldALevelBishop.PathType.Straight)
		{
			this.straightValue = ((!this.pathIsClockwise) ? (one - t / maxTime) : (t / maxTime));
			yield return base.StartCoroutine(this.move_cr(base.transform.position, new Vector3(Mathf.Lerp(startX, endX, this.straightValue), p.straightPathHeight)));
		}
		while (!this.isStunned)
		{
			if (t < maxTime)
			{
				t += CupheadTime.FixedDelta;
				this.straightValue = ((!this.pathIsClockwise) ? (one - t / maxTime) : (t / maxTime));
				base.transform.SetPosition(new float?(Mathf.Lerp(startX, endX, this.straightValue)), null, null);
			}
			else
			{
				this.pathIsClockwise = !this.pathIsClockwise;
				t = 0f;
			}
			yield return wait;
		}
		this.lerpPos = t;
		yield return null;
		yield break;
	}

	// Token: 0x060011CB RID: 4555 RVA: 0x0009316C File Offset: 0x0009136C
	public IEnumerator infinite_cr()
	{
		LevelProperties.ChessBOldA.BishopPath p = base.properties.CurrentState.bishopPath;
		YieldInstruction wait = new WaitForFixedUpdate();
		float loopSizeX = p.infinitePathLength;
		float loopSizeY = p.infinitePathWidth;
		float speed = this.pathSpeed;
		bool invert = this.pathIsClockwise;
		this.pivotPoint.transform.SetPosition(new float?(loopSizeX), new float?(p.infinitePathHeight), null);
		Vector3 endPos = Vector3.zero;
		Vector3 pivotOffset = Vector3.left * 2f * loopSizeX;
		if (this.previousPathType != ChessBOldALevelBishop.PathType.Infinite)
		{
			endPos = ((!invert) ? this.pivotPoint.position : (this.pivotPoint.position + pivotOffset));
			float value = (float)((!invert) ? -1 : 1);
			Vector3 handleRotationX = new Vector3(Mathf.Cos(this.infinityAngle) * value * loopSizeX, 0f, 0f);
			Vector3 handleRotationY = new Vector3(0f, Mathf.Sin(this.infinityAngle) * loopSizeY, 0f);
			endPos += handleRotationX + handleRotationY;
			yield return base.StartCoroutine(this.move_cr(base.transform.position, endPos));
		}
		while (!this.isStunned)
		{
			this.infinityAngle += speed * CupheadTime.Delta;
			if (this.infinityAngle > 6.28318548f)
			{
				invert = !invert;
				this.infinityAngle -= 6.28318548f;
			}
			if (this.infinityAngle < 0f)
			{
				this.infinityAngle += 6.28318548f;
			}
			float value;
			if (invert)
			{
				base.transform.position = this.pivotPoint.position + pivotOffset;
				value = 1f;
			}
			else
			{
				base.transform.position = this.pivotPoint.position;
				value = -1f;
			}
			Vector3 handleRotationX = new Vector3(Mathf.Cos(this.infinityAngle) * value * loopSizeX, 0f, 0f);
			Vector3 handleRotationY = new Vector3(0f, Mathf.Sin(this.infinityAngle) * loopSizeY, 0f);
			base.transform.position += handleRotationX + handleRotationY;
			yield return wait;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060011CC RID: 4556 RVA: 0x00093188 File Offset: 0x00091388
	public IEnumerator box_cr()
	{
		LevelProperties.ChessBOldA.BishopPath p = base.properties.CurrentState.bishopPath;
		float boxCenter = p.squarePathHeight;
		float length = p.squarePathLength / 2f;
		float height = p.squarePathWidth / 2f;
		Vector3 topLeft = new Vector3(boxCenter - length, boxCenter + height);
		Vector3 topRight = new Vector3(boxCenter + length, boxCenter + height);
		Vector3 bottomLeft = new Vector3(boxCenter - length, boxCenter - height);
		Vector3 bottomRight = new Vector3(boxCenter + length, boxCenter - height);
		Vector3[] positions = new Vector3[]
		{
			topRight,
			bottomRight,
			bottomLeft,
			topLeft
		};
		int incrementBy = (!this.pathIsClockwise) ? -1 : 1;
		float distance = 0f;
		float speed = 0f;
		Vector3 endPos = positions[this.pathIndex];
		if (this.previousPathType != ChessBOldALevelBishop.PathType.Square)
		{
			yield return base.StartCoroutine(this.move_cr(base.transform.position, endPos));
		}
		while (!this.isStunned)
		{
			YieldInstruction wait = new WaitForFixedUpdate();
			distance = Vector3.Distance(base.transform.position, endPos);
			speed = distance / this.pathSpeed;
			while (base.transform.position != endPos)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, endPos, speed * CupheadTime.FixedDelta);
				if (this.isStunned)
				{
					break;
				}
				yield return wait;
			}
			if (!this.isStunned)
			{
				if (this.pathIsClockwise && this.pathIndex >= positions.Length - 1)
				{
					this.pathIndex = 0;
				}
				else if (!this.pathIsClockwise && this.pathIndex <= 0)
				{
					this.pathIndex = positions.Length - 1;
				}
				else
				{
					this.pathIndex += incrementBy;
				}
			}
			endPos = positions[this.pathIndex];
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060011CD RID: 4557 RVA: 0x000931A4 File Offset: 0x000913A4
	public void SetPinkValues()
	{
		LevelProperties.ChessBOldA.Pink pink = base.properties.CurrentState.pink;
		int num = Mathf.Clamp(this.phase, 0, pink.pinkSpeedString.Split(new char[]
		{
			','
		}).Length - 1);
		int num2 = Mathf.Clamp(this.phase, 0, pink.pinkDirString.Split(new char[]
		{
			','
		}).Length - 1);
		Parser.FloatTryParse(pink.pinkSpeedString.Split(new char[]
		{
			','
		})[num], out this.pinkSpeed);
		this.pinkIsClockwise = (pink.pinkDirString.Split(new char[]
		{
			','
		})[num2][0] == 'R');
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x0009325C File Offset: 0x0009145C
	public IEnumerator pink_cr()
	{
		LevelProperties.ChessBOldA.Pink p = base.properties.CurrentState.pink;
		float angle = 0f;
		this.pink.transform.SetScale(new float?(p.pinkScale), new float?(p.pinkScale), null);
		Vector3 handleRotationX = Vector3.zero;
		Vector3 handleRotationY = Vector3.zero;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (this.pinkIsClockwise)
			{
				angle += this.pinkSpeed * CupheadTime.FixedDelta;
			}
			else
			{
				angle -= this.pinkSpeed * CupheadTime.FixedDelta;
			}
			handleRotationX = new Vector3(Mathf.Sin(angle) * p.pinkPathRadius, 0f, 0f);
			handleRotationY = new Vector3(0f, Mathf.Cos(angle) * p.pinkPathRadius, 0f);
			this.pink.transform.position = base.transform.position;
			this.pink.transform.position += handleRotationX + handleRotationY;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060011CF RID: 4559 RVA: 0x00093278 File Offset: 0x00091478
	public void SetWallValues()
	{
		LevelProperties.ChessBOldA.Walls walls = base.properties.CurrentState.walls;
		this.nullIndex = Mathf.Clamp(this.phase, 0, walls.wallNullString.Length - 1);
		int num = Mathf.Clamp(this.phase, 0, walls.wallNumberString.Split(new char[]
		{
			','
		}).Length - 1);
		int num2 = Mathf.Clamp(this.phase, 0, walls.wallSpeedString.Split(new char[]
		{
			','
		}).Length - 1);
		int num3 = Mathf.Clamp(this.phase, 0, walls.wallDirString.Split(new char[]
		{
			','
		}).Length - 1);
		int num4 = 0;
		float speed = 0f;
		int[] array = new int[walls.wallNullString[this.nullIndex].Split(new char[]
		{
			','
		}).Length];
		Parser.IntTryParse(walls.wallNumberString.Split(new char[]
		{
			','
		})[num], out num4);
		Parser.FloatTryParse(walls.wallSpeedString.Split(new char[]
		{
			','
		})[num2], out speed);
		bool isClockwise = walls.wallDirString.Split(new char[]
		{
			','
		})[num3][0] == 'R';
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			flag = Parser.IntTryParse(walls.wallNullString[this.nullIndex].Split(new char[]
			{
				','
			})[i], out array[i]);
		}
		float num5 = 360f / (float)num4;
		for (int j = 0; j < num4; j++)
		{
			bool flag2 = false;
			for (int k = 0; k < array.Length; k++)
			{
				if (!flag)
				{
					break;
				}
				if (j == array[k])
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				ChessBOldALevelWall chessBOldALevelWall = this.wallPrefab.Spawn<ChessBOldALevelWall>();
				chessBOldALevelWall.StartRotate(num5 * (float)j, this, walls.wallPathRadius, speed, isClockwise, walls.wallLength);
				chessBOldALevelWall.transform.parent = base.transform;
				this.walls.Add(chessBOldALevelWall);
			}
		}
	}

	// Token: 0x060011D0 RID: 4560 RVA: 0x000934C4 File Offset: 0x000916C4
	public void RemoveCurrentWalls()
	{
		foreach (ChessBOldALevelWall chessBOldALevelWall in this.walls)
		{
			chessBOldALevelWall.Dead();
		}
	}

	// Token: 0x060011D1 RID: 4561 RVA: 0x00093520 File Offset: 0x00091720
	public IEnumerator turret_cr()
	{
		LevelProperties.ChessBOldA.BishopPath p = base.properties.CurrentState.bishopPath;
		AbstractPlayerController player = PlayerManager.GetNext();
		string[] turretString = p.turretShotDelayString.Split(new char[]
		{
			','
		});
		int turretIndex = Random.Range(0, turretString.Length);
		bool gotStunned = false;
		float delay = 0f;
		for (;;)
		{
			if (gotStunned)
			{
				Parser.FloatTryParse(turretString[turretIndex], out delay);
				yield return CupheadTime.WaitForSeconds(this, delay);
				gotStunned = false;
			}
			Vector3 dir = player.transform.position - base.transform.position;
			this.turretShot.Create(base.transform.position, MathUtils.DirectionToAngle(dir), p.turretShotSpeed);
			player = PlayerManager.GetNext();
			Parser.FloatTryParse(turretString[turretIndex], out delay);
			yield return CupheadTime.WaitForSeconds(this, delay);
			while (this.isStunned)
			{
				gotStunned = true;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x04000E3B RID: 3643
	[SerializeField]
	public BasicProjectile turretShot;

	// Token: 0x04000E3C RID: 3644
	[SerializeField]
	public ParrySwitch pink;

	// Token: 0x04000E3D RID: 3645
	[SerializeField]
	public Transform pivotPoint;

	// Token: 0x04000E3E RID: 3646
	[SerializeField]
	public ChessBOldALevelWall wallPrefab;

	// Token: 0x04000E3F RID: 3647
	public List<ChessBOldALevelWall> walls;

	// Token: 0x04000E40 RID: 3648
	public DamageDealer damageDealer;

	// Token: 0x04000E41 RID: 3649
	public ChessBOldALevelBishop.PathType pathType;

	// Token: 0x04000E42 RID: 3650
	public ChessBOldALevelBishop.PathType previousPathType;

	// Token: 0x04000E43 RID: 3651
	public bool pathIsClockwise;

	// Token: 0x04000E44 RID: 3652
	public float pathSpeed;

	// Token: 0x04000E45 RID: 3653
	public int pathIndex;

	// Token: 0x04000E46 RID: 3654
	public Vector3[] positions;

	// Token: 0x04000E47 RID: 3655
	public bool pinkIsClockwise;

	// Token: 0x04000E48 RID: 3656
	public float pinkSpeed;

	// Token: 0x04000E49 RID: 3657
	public bool isStunned;

	// Token: 0x04000E4A RID: 3658
	public float HPToDecrease;

	// Token: 0x04000E4B RID: 3659
	public float lerpPos;

	// Token: 0x04000E4C RID: 3660
	public int phase;

	// Token: 0x04000E4D RID: 3661
	public int nullIndex;

	// Token: 0x04000E4E RID: 3662
	public float straightValue;

	// Token: 0x04000E4F RID: 3663
	public float infinityAngle;

	// Token: 0x02000A91 RID: 2705
	public enum PathType
	{
		// Token: 0x04004D9B RID: 19867
		Straight,
		// Token: 0x04004D9C RID: 19868
		Infinite,
		// Token: 0x04004D9D RID: 19869
		Square,
		// Token: 0x04004D9E RID: 19870
		Pending
	}
}
