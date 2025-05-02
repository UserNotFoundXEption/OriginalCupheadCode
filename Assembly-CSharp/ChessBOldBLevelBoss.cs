using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000175 RID: 373
public class ChessBOldBLevelBoss : LevelProperties.ChessBOldB.Entity
{
	// Token: 0x060011E8 RID: 4584 RVA: 0x0000F27F File Offset: 0x0000D47F
	public override void LevelInit(LevelProperties.ChessBOldB properties)
	{
		base.LevelInit(properties);
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060011E9 RID: 4585 RVA: 0x00093848 File Offset: 0x00091A48
	public IEnumerator intro_cr()
	{
		this.brown = this.bossTwo.GetComponent<SpriteRenderer>().color;
		yield return CupheadTime.WaitForSeconds(this, 4f);
		this.MoveBosses();
		base.StartCoroutine(this.wait_to_shoot());
		yield return null;
		yield break;
	}

	// Token: 0x060011EA RID: 4586 RVA: 0x00093864 File Offset: 0x00091A64
	public void HandleHurt(bool gettingHurt)
	{
		this.bossOne.GetComponent<SpriteRenderer>().color = ((!gettingHurt) ? this.brown : Color.red);
		this.bossTwo.GetComponent<SpriteRenderer>().color = ((!gettingHurt) ? this.brown : Color.red);
		this.isMoving = !gettingHurt;
	}

	// Token: 0x060011EB RID: 4587 RVA: 0x000938D0 File Offset: 0x00091AD0
	public void OnStateChanged()
	{
		LevelProperties.ChessBOldB.Boss boss = base.properties.CurrentState.boss;
		this.moveTime = boss.bossTime;
		this.bulletDelayStringMainIndex = Random.Range(0, boss.bulletDelayString.Length);
		this.bulletDelayString = boss.bulletDelayString[this.bulletDelayStringMainIndex].Split(new char[]
		{
			','
		});
		this.bulletDelayStringIndex = Random.Range(0, this.bulletDelayString.Length);
	}

	// Token: 0x060011EC RID: 4588 RVA: 0x0000F295 File Offset: 0x0000D495
	public void MoveBosses()
	{
		base.StartCoroutine(this.move_bosses_cr());
	}

	// Token: 0x060011ED RID: 4589 RVA: 0x00093948 File Offset: 0x00091B48
	public IEnumerator move_bosses_cr()
	{
		LevelProperties.ChessBOldB.Boss p = base.properties.CurrentState.boss;
		float t = 0f;
		float one = 1f;
		this.moveTime = p.bossTime;
		bool countingUp = true;
		YieldInstruction wait = new WaitForFixedUpdate();
		this.isMoving = true;
		for (;;)
		{
			while (!this.isMoving)
			{
				yield return null;
			}
			t += CupheadTime.FixedDelta;
			this.bossOne.transform.SetPosition(null, new float?(Mathf.Lerp(225f, -225f, (!countingUp) ? (one - t / this.moveTime) : (t / this.moveTime))), null);
			this.bossTwo.transform.SetPosition(null, new float?(Mathf.Lerp(225f, -225f, (!countingUp) ? (t / this.moveTime) : (one - t / this.moveTime))), null);
			if (t >= this.moveTime)
			{
				countingUp = !countingUp;
				t = 0f;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060011EE RID: 4590 RVA: 0x00093964 File Offset: 0x00091B64
	public IEnumerator wait_to_shoot()
	{
		bool leftOneShoot = Rand.Bool();
		LevelProperties.ChessBOldB.Boss p = base.properties.CurrentState.boss;
		this.OnStateChanged();
		float delay = 0f;
		for (;;)
		{
			while (!this.isMoving)
			{
				yield return null;
			}
			p = base.properties.CurrentState.boss;
			this.bulletDelayString = p.bulletDelayString[this.bulletDelayStringMainIndex].Split(new char[]
			{
				','
			});
			Parser.FloatTryParse(this.bulletDelayString[this.bulletDelayStringIndex], out delay);
			yield return CupheadTime.WaitForSeconds(this, delay);
			GameObject boss = (!leftOneShoot) ? this.bossTwo : this.bossOne;
			this.Shoot(boss);
			leftOneShoot = !leftOneShoot;
			if (this.bulletDelayStringIndex < this.bulletDelayString.Length - 1)
			{
				this.bulletDelayStringIndex++;
			}
			else
			{
				this.bulletDelayStringMainIndex = (this.bulletDelayStringMainIndex + 1) % p.bulletDelayString.Length;
				this.bulletDelayStringIndex = 0;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060011EF RID: 4591 RVA: 0x00093980 File Offset: 0x00091B80
	public void Shoot(GameObject boss)
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 vector = next.center - boss.transform.position;
		float rotation = MathUtils.DirectionToAngle(vector);
		this.projectile.Create(boss.transform.position, rotation, base.properties.CurrentState.boss.bulletSpeed);
	}

	// Token: 0x04000E63 RID: 3683
	public const float Y_POS = 225f;

	// Token: 0x04000E64 RID: 3684
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x04000E65 RID: 3685
	[SerializeField]
	public GameObject bossOne;

	// Token: 0x04000E66 RID: 3686
	[SerializeField]
	public GameObject bossTwo;

	// Token: 0x04000E67 RID: 3687
	public Color brown;

	// Token: 0x04000E68 RID: 3688
	public bool isMoving;

	// Token: 0x04000E69 RID: 3689
	public float moveTime;

	// Token: 0x04000E6A RID: 3690
	public int bulletDelayStringMainIndex;

	// Token: 0x04000E6B RID: 3691
	public string[] bulletDelayString;

	// Token: 0x04000E6C RID: 3692
	public int bulletDelayStringIndex;
}
