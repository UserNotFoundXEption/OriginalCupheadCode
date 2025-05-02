using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000310 RID: 784
public class RetroArcadeBigRobot : RetroArcadeEnemy
{
	// Token: 0x060022A1 RID: 8865 RVA: 0x000BE0D8 File Offset: 0x000BC2D8
	public RetroArcadeBigRobot Create(float xPos, LevelProperties.RetroArcade.Robots properties, float sinOffset, RetroArcadeRobotManager manager, string[] orbiterPattern)
	{
		RetroArcadeBigRobot retroArcadeBigRobot = this.InstantiatePrefab<RetroArcadeBigRobot>();
		retroArcadeBigRobot.t = sinOffset * 3.14159274f * 2f;
		float num = this.OFFSCREEN_Y + properties.smallRobotRotationDistance - properties.mainRobotY.min;
		retroArcadeBigRobot.properties = properties;
		retroArcadeBigRobot.transform.position = new Vector2(xPos, retroArcadeBigRobot.getYPos(retroArcadeBigRobot.t) + num);
		retroArcadeBigRobot.hp = properties.mainRobotHp;
		retroArcadeBigRobot.manager = manager;
		float num2 = sinOffset * 360f;
		retroArcadeBigRobot.orbiters = new RetroArcadeOrbiterRobot[3];
		for (int i = 0; i < 3; i++)
		{
			int num3;
			if (Parser.IntTryParse(orbiterPattern[i], out num3) && num3 > 0 && num3 <= this.orbiterPrefabs.Length)
			{
				retroArcadeBigRobot.orbiters[i] = retroArcadeBigRobot.orbiterPrefabs[num3 - 1].Create(retroArcadeBigRobot, properties, num2);
				num2 += 120f;
			}
		}
		retroArcadeBigRobot.MoveY(-num, properties.mainRobotMoveSpeed);
		retroArcadeBigRobot.StartCoroutine(retroArcadeBigRobot.shoot_cr());
		retroArcadeBigRobot.StartCoroutine(retroArcadeBigRobot.orbiterShoot_cr());
		return retroArcadeBigRobot;
	}

	// Token: 0x060022A2 RID: 8866 RVA: 0x0001D766 File Offset: 0x0001B966
	public override void Start()
	{
		base.PointsWorth = this.properties.pointsGained;
		base.PointsBonus = this.properties.pointsBonus;
	}

	// Token: 0x060022A3 RID: 8867 RVA: 0x000BE1F4 File Offset: 0x000BC3F4
	public override void FixedUpdate()
	{
		if (this.movingY || this.groupDead)
		{
			return;
		}
		this.t += CupheadTime.FixedDelta * (this.properties.mainRobotMoveSpeed / (this.properties.mainRobotY.max - this.properties.mainRobotY.min)) * 3.14159274f;
		base.transform.SetPosition(null, new float?(this.getYPos(this.t)), null);
		bool flag = true;
		foreach (RetroArcadeOrbiterRobot retroArcadeOrbiterRobot in this.orbiters)
		{
			if (!retroArcadeOrbiterRobot.IsDead)
			{
				flag = false;
			}
		}
		if (flag)
		{
			base.StartCoroutine(this.moveOffscreen_cr());
			this.groupDead = true;
			this.manager.OnRobotGroupDie();
		}
	}

	// Token: 0x060022A4 RID: 8868 RVA: 0x0001D78A File Offset: 0x0001B98A
	public float getYPos(float t)
	{
		return this.properties.mainRobotY.GetFloatAt(Mathf.Sin(t) * 0.5f + 0.5f);
	}

	// Token: 0x060022A5 RID: 8869 RVA: 0x000BE2E4 File Offset: 0x000BC4E4
	public IEnumerator moveOffscreen_cr()
	{
		base.MoveY(this.OFFSCREEN_Y + this.properties.smallRobotRotationDistance - base.transform.position.y, 500f);
		while (this.movingY)
		{
			yield return null;
		}
		foreach (RetroArcadeOrbiterRobot retroArcadeOrbiterRobot in this.orbiters)
		{
			Object.Destroy(retroArcadeOrbiterRobot.gameObject);
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060022A6 RID: 8870 RVA: 0x000BE300 File Offset: 0x000BC500
	public IEnumerator shoot_cr()
	{
		while (this.movingY)
		{
			yield return null;
		}
		string[] pattern = this.properties.mainRobotShootString.Split(new char[]
		{
			','
		});
		int currentIndex = Random.Range(0, pattern.Length);
		while (!base.IsDead)
		{
			float waitTime = 0f;
			Parser.FloatTryParse(pattern[currentIndex], out waitTime);
			yield return CupheadTime.WaitForSeconds(this, waitTime);
			if (base.IsDead)
			{
				break;
			}
			float shootAngle = MathUtils.DirectionToAngle(PlayerManager.GetNext().center - this.projectileRoot.position);
			this.projectilePrefab.Create(this.projectileRoot.position, this.properties.mainRobotShootSpeed, shootAngle, this.properties.mainRobotShotBounce);
		}
		yield break;
	}

	// Token: 0x060022A7 RID: 8871 RVA: 0x000BE31C File Offset: 0x000BC51C
	public IEnumerator orbiterShoot_cr()
	{
		while (this.movingY)
		{
			yield return null;
		}
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.smallRobotAttackDelay.RandomFloat());
			List<RetroArcadeOrbiterRobot> aliveOrbiters = new List<RetroArcadeOrbiterRobot>();
			foreach (RetroArcadeOrbiterRobot retroArcadeOrbiterRobot in this.orbiters)
			{
				if (!retroArcadeOrbiterRobot.IsDead)
				{
					aliveOrbiters.Add(retroArcadeOrbiterRobot);
				}
			}
			if (aliveOrbiters.Count == 0)
			{
				break;
			}
			aliveOrbiters.RandomChoice<RetroArcadeOrbiterRobot>().Shoot();
		}
		yield break;
	}

	// Token: 0x04001C9A RID: 7322
	[SerializeField]
	public RetroArcadeOrbiterRobot[] orbiterPrefabs;

	// Token: 0x04001C9B RID: 7323
	[SerializeField]
	public RetroArcadeRobotBouncingProjectile projectilePrefab;

	// Token: 0x04001C9C RID: 7324
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x04001C9D RID: 7325
	public float OFFSCREEN_Y = 300f;

	// Token: 0x04001C9E RID: 7326
	public const float MOVE_OFFSCREEN_SPEED = 500f;

	// Token: 0x04001C9F RID: 7327
	public LevelProperties.RetroArcade.Robots properties;

	// Token: 0x04001CA0 RID: 7328
	public float t;

	// Token: 0x04001CA1 RID: 7329
	public RetroArcadeOrbiterRobot[] orbiters;

	// Token: 0x04001CA2 RID: 7330
	public RetroArcadeRobotManager manager;

	// Token: 0x04001CA3 RID: 7331
	public bool groupDead;
}
