using System;
using UnityEngine;

// Token: 0x020000D4 RID: 212
[Serializable]
public class EnemyProperties
{
	// Token: 0x060009F9 RID: 2553 RVA: 0x0007ABC8 File Offset: 0x00078DC8
	public EnemyProperties()
	{
		this._id = TimeUtils.GetCurrentSecond();
	}

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x060009FA RID: 2554 RVA: 0x00009272 File Offset: 0x00007472
	public string DisplayName
	{
		get
		{
			return this._displayName;
		}
	}

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x060009FB RID: 2555 RVA: 0x0000927A File Offset: 0x0000747A
	public string EnumName
	{
		get
		{
			return this._enumName;
		}
	}

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x060009FC RID: 2556 RVA: 0x00009282 File Offset: 0x00007482
	public int ID
	{
		get
		{
			return this._id;
		}
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x060009FD RID: 2557 RVA: 0x0000928A File Offset: 0x0000748A
	public float Health
	{
		get
		{
			return this._health;
		}
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x060009FE RID: 2558 RVA: 0x00009292 File Offset: 0x00007492
	public bool CanParry
	{
		get
		{
			return this._parryable;
		}
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x060009FF RID: 2559 RVA: 0x0000929A File Offset: 0x0000749A
	public EnemyProperties.LoopMode MoveLoopMode
	{
		get
		{
			return this._moveLoopMode;
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06000A00 RID: 2560 RVA: 0x000092A2 File Offset: 0x000074A2
	public float MoveSpeed
	{
		get
		{
			return this._moveSpeed;
		}
	}

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x06000A01 RID: 2561 RVA: 0x000092AA File Offset: 0x000074AA
	public bool canJump
	{
		get
		{
			return this._canJump;
		}
	}

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x06000A02 RID: 2562 RVA: 0x000092B2 File Offset: 0x000074B2
	public float gravity
	{
		get
		{
			return this._gravity;
		}
	}

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x06000A03 RID: 2563 RVA: 0x000092BA File Offset: 0x000074BA
	public float floatSpeed
	{
		get
		{
			return this._floatSpeed;
		}
	}

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x06000A04 RID: 2564 RVA: 0x000092C2 File Offset: 0x000074C2
	public float jumpHeight
	{
		get
		{
			return this._jumpHeight;
		}
	}

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x06000A05 RID: 2565 RVA: 0x000092CA File Offset: 0x000074CA
	public float jumpLength
	{
		get
		{
			return this._jumpLength;
		}
	}

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x06000A06 RID: 2566 RVA: 0x000092D2 File Offset: 0x000074D2
	public EnemyProperties.AimMode ProjectileAimMode
	{
		get
		{
			return this._projectileAimMode;
		}
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06000A07 RID: 2567 RVA: 0x000092DA File Offset: 0x000074DA
	public bool ProjectileParryable
	{
		get
		{
			return this._projectileParryable;
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06000A08 RID: 2568 RVA: 0x000092E2 File Offset: 0x000074E2
	public float ProjectileSpeed
	{
		get
		{
			return this._projectileSpeed;
		}
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x06000A09 RID: 2569 RVA: 0x000092EA File Offset: 0x000074EA
	public float ArcProjectileMinSpeed
	{
		get
		{
			return this._arcProjectileMinSpeed;
		}
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x06000A0A RID: 2570 RVA: 0x000092F2 File Offset: 0x000074F2
	public float ProjectileAngle
	{
		get
		{
			return this._projectileAngle;
		}
	}

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x06000A0B RID: 2571 RVA: 0x000092FA File Offset: 0x000074FA
	public float ArcProjectileMinAngle
	{
		get
		{
			return this._arcProjectileMinAngle;
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x06000A0C RID: 2572 RVA: 0x00009302 File Offset: 0x00007502
	public float ProjectileGravity
	{
		get
		{
			return this._projectileGravity;
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x06000A0D RID: 2573 RVA: 0x0000930A File Offset: 0x0000750A
	public float ProjectileStoneTime
	{
		get
		{
			return this._projectileStoneTime;
		}
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x06000A0E RID: 2574 RVA: 0x00009312 File Offset: 0x00007512
	public MinMax ProjectileDelay
	{
		get
		{
			return this._projectileDelay;
		}
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x06000A0F RID: 2575 RVA: 0x0000931A File Offset: 0x0000751A
	public MinMax MushroomPinkNumber
	{
		get
		{
			return this._mushroomPinkNumber;
		}
	}

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06000A10 RID: 2576 RVA: 0x00009322 File Offset: 0x00007522
	public float AcornFlySpeed
	{
		get
		{
			return this._acornFlySpeed;
		}
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06000A11 RID: 2577 RVA: 0x0000932A File Offset: 0x0000752A
	public float AcornDropSpeed
	{
		get
		{
			return this._acornDropSpeed;
		}
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x06000A12 RID: 2578 RVA: 0x00009332 File Offset: 0x00007532
	public float AcornPropellerSpeed
	{
		get
		{
			return this._acornPropellerSpeed;
		}
	}

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x06000A13 RID: 2579 RVA: 0x0000933A File Offset: 0x0000753A
	public MinMax BlobRunnerMeltDelay
	{
		get
		{
			return this._blobRunnerMeltDelay;
		}
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x06000A14 RID: 2580 RVA: 0x00009342 File Offset: 0x00007542
	public float BlobRunnerUnmeltLoopTime
	{
		get
		{
			return this._blobRunnerUnnmeltLoopTime;
		}
	}

	// Token: 0x0400079D RID: 1949
	[SerializeField]
	public string _displayName = string.Empty;

	// Token: 0x0400079E RID: 1950
	[SerializeField]
	public string _enumName = string.Empty;

	// Token: 0x0400079F RID: 1951
	[SerializeField]
	public int _id;

	// Token: 0x040007A0 RID: 1952
	[SerializeField]
	public float _health = 1f;

	// Token: 0x040007A1 RID: 1953
	[SerializeField]
	public bool _parryable;

	// Token: 0x040007A2 RID: 1954
	[Header("Movement")]
	[SerializeField]
	public EnemyProperties.LoopMode _moveLoopMode;

	// Token: 0x040007A3 RID: 1955
	[SerializeField]
	public float _moveSpeed = 500f;

	// Token: 0x040007A4 RID: 1956
	[SerializeField]
	public bool _canJump;

	// Token: 0x040007A5 RID: 1957
	[SerializeField]
	public float _gravity = 1200f;

	// Token: 0x040007A6 RID: 1958
	[SerializeField]
	public float _floatSpeed = 400f;

	// Token: 0x040007A7 RID: 1959
	[SerializeField]
	public float _jumpHeight = 200f;

	// Token: 0x040007A8 RID: 1960
	[SerializeField]
	public float _jumpLength = 500f;

	// Token: 0x040007A9 RID: 1961
	[Header("Projectiles")]
	[SerializeField]
	public EnemyProperties.AimMode _projectileAimMode = EnemyProperties.AimMode.Straight;

	// Token: 0x040007AA RID: 1962
	[SerializeField]
	public bool _projectileParryable;

	// Token: 0x040007AB RID: 1963
	[SerializeField]
	public float _projectileSpeed = 500f;

	// Token: 0x040007AC RID: 1964
	[SerializeField]
	public float _arcProjectileMinSpeed = 250f;

	// Token: 0x040007AD RID: 1965
	[SerializeField]
	public float _projectileAngle;

	// Token: 0x040007AE RID: 1966
	[SerializeField]
	public float _arcProjectileMinAngle;

	// Token: 0x040007AF RID: 1967
	[SerializeField]
	public float _projectileGravity = 15f;

	// Token: 0x040007B0 RID: 1968
	[SerializeField]
	public float _projectileStoneTime;

	// Token: 0x040007B1 RID: 1969
	[SerializeField]
	public MinMax _projectileDelay = new MinMax(1f, 1f);

	// Token: 0x040007B2 RID: 1970
	[SerializeField]
	public MinMax _mushroomPinkNumber = new MinMax(3f, 5f);

	// Token: 0x040007B3 RID: 1971
	[SerializeField]
	public float _acornFlySpeed = 500f;

	// Token: 0x040007B4 RID: 1972
	[SerializeField]
	public float _acornDropSpeed = 500f;

	// Token: 0x040007B5 RID: 1973
	[SerializeField]
	public float _acornPropellerSpeed = 300f;

	// Token: 0x040007B6 RID: 1974
	[SerializeField]
	public MinMax _blobRunnerMeltDelay = new MinMax(2f, 3f);

	// Token: 0x040007B7 RID: 1975
	[SerializeField]
	public float _blobRunnerUnnmeltLoopTime = 0.5f;

	// Token: 0x040007B8 RID: 1976
	public float ClamTimeSpeedUp = 0.8f;

	// Token: 0x040007B9 RID: 1977
	public float ClamTimeSpeedDown = 1f;

	// Token: 0x040007BA RID: 1978
	public MinMax ClamMaxPointRange = new MinMax(600f, 700f);

	// Token: 0x040007BB RID: 1979
	public int ClamShotCount = 4;

	// Token: 0x040007BC RID: 1980
	public MinMax ClamDespawnDelayRange = new MinMax(3.5f, 5f);

	// Token: 0x040007BD RID: 1981
	public float fastMovement = 400f;

	// Token: 0x040007BE RID: 1982
	public float slowMovement = 200f;

	// Token: 0x040007BF RID: 1983
	public string dragonFlyAimString;

	// Token: 0x040007C0 RID: 1984
	public string dragonFlyAtkDelayString;

	// Token: 0x040007C1 RID: 1985
	public float dragonFlyWarningDuration;

	// Token: 0x040007C2 RID: 1986
	public float dragonFlyAttackDuration;

	// Token: 0x040007C3 RID: 1987
	public float dragonFlyProjectileSpeed;

	// Token: 0x040007C4 RID: 1988
	public float dragonFlyProjectileDelay;

	// Token: 0x040007C5 RID: 1989
	public float dragonFlyLockDistOffset;

	// Token: 0x040007C6 RID: 1990
	public float dragonFlyInitRiseTime;

	// Token: 0x040007C7 RID: 1991
	public float WoodpeckerWarningDuration;

	// Token: 0x040007C8 RID: 1992
	public float WoodpeckerAttackDuration;

	// Token: 0x040007C9 RID: 1993
	public float WoodpeckermoveDownTime;

	// Token: 0x040007CA RID: 1994
	public float WoodpeckermoveUpTime;

	// Token: 0x040007CB RID: 1995
	public float flyingFishVelocity;

	// Token: 0x040007CC RID: 1996
	public float flyingFishSinVelocity;

	// Token: 0x040007CD RID: 1997
	public float flyingFishSinSize;

	// Token: 0x040007CE RID: 1998
	public float lobsterTuckTime;

	// Token: 0x040007CF RID: 1999
	public float lobsterOffscreenTime;

	// Token: 0x040007D0 RID: 2000
	public float lobsterSpeed;

	// Token: 0x040007D1 RID: 2001
	public float lobsterWarningTime;

	// Token: 0x040007D2 RID: 2002
	public float lobsterY;

	// Token: 0x040007D3 RID: 2003
	public MinMax krillVelocityX;

	// Token: 0x040007D4 RID: 2004
	public MinMax krillVelocityY;

	// Token: 0x040007D5 RID: 2005
	public float krillLaunchDelay;

	// Token: 0x040007D6 RID: 2006
	public float krillGravity;

	// Token: 0x040007D7 RID: 2007
	public float dragonTimeIn;

	// Token: 0x040007D8 RID: 2008
	public float dragonTimeOut;

	// Token: 0x040007D9 RID: 2009
	public float dragonLeaveDelay;

	// Token: 0x040007DA RID: 2010
	public float minerShootSpeed;

	// Token: 0x040007DB RID: 2011
	public float minerDescendTime;

	// Token: 0x040007DC RID: 2012
	public float minerRopeAscendTime;

	// Token: 0x040007DD RID: 2013
	public MinMax minerShotDelay;

	// Token: 0x040007DE RID: 2014
	public float minerDistance;

	// Token: 0x040007DF RID: 2015
	public float wallFaceTravelTime;

	// Token: 0x040007E0 RID: 2016
	public MinMax wallAttackDelay;

	// Token: 0x040007E1 RID: 2017
	public float wallProjectileXSpeed;

	// Token: 0x040007E2 RID: 2018
	public float wallProjectileYSpeed;

	// Token: 0x040007E3 RID: 2019
	public float wallProjectileGravity;

	// Token: 0x040007E4 RID: 2020
	public float flamerCirSpeed;

	// Token: 0x040007E5 RID: 2021
	public MinMax flamerXSpeed;

	// Token: 0x040007E6 RID: 2022
	public float flamerLoopSize;

	// Token: 0x040007E7 RID: 2023
	public float fanVelocity;

	// Token: 0x040007E8 RID: 2024
	public MinMax fanWaitTime;

	// Token: 0x040007E9 RID: 2025
	public MinMax funWallTopDelayRange;

	// Token: 0x040007EA RID: 2026
	public MinMax funWallBottomDelayRange;

	// Token: 0x040007EB RID: 2027
	public float funWallProjectileSpeed;

	// Token: 0x040007EC RID: 2028
	public float funWallMouthOpenTime;

	// Token: 0x040007ED RID: 2029
	public MinMax funWallCarDelayRange;

	// Token: 0x040007EE RID: 2030
	public float funWallCarSpeed;

	// Token: 0x040007EF RID: 2031
	public MinMax funWallTongueDelayRange;

	// Token: 0x040007F0 RID: 2032
	public float funWallTongueLoopTime;

	// Token: 0x040007F1 RID: 2033
	public float jackLaunchVelocity;

	// Token: 0x040007F2 RID: 2034
	public float jackHomingMoveSpeed;

	// Token: 0x040007F3 RID: 2035
	public float jackRotationSpeed;

	// Token: 0x040007F4 RID: 2036
	public float jacktimeBeforeDeath;

	// Token: 0x040007F5 RID: 2037
	public float jacktimeBeforeHoming;

	// Token: 0x040007F6 RID: 2038
	public float jackEaseTime;

	// Token: 0x040007F7 RID: 2039
	public string jackinDirectionString;

	// Token: 0x040007F8 RID: 2040
	public MinMax jackinAppearDelay;

	// Token: 0x040007F9 RID: 2041
	public MinMax jackinDeathAppearDelay;

	// Token: 0x040007FA RID: 2042
	public float jackinWarningDuration;

	// Token: 0x040007FB RID: 2043
	public float jackinShootDelay;

	// Token: 0x040007FC RID: 2044
	public int tubaACount;

	// Token: 0x040007FD RID: 2045
	public float tubaInitialDelay;

	// Token: 0x040007FE RID: 2046
	public MinMax tubaMainDelayRange;

	// Token: 0x040007FF RID: 2047
	public float cannonSpeed;

	// Token: 0x04000800 RID: 2048
	public float cannonShotDelay;

	// Token: 0x04000801 RID: 2049
	public float bulletDeathTime;

	// Token: 0x04000802 RID: 2050
	public MinMax pretzelXSpeedRange;

	// Token: 0x04000803 RID: 2051
	public float pretzelYSpeed;

	// Token: 0x04000804 RID: 2052
	public float pretzelGroundDelay;

	// Token: 0x04000805 RID: 2053
	public MinMax arcadeAttackDelayInit;

	// Token: 0x04000806 RID: 2054
	public MinMax arcadeAttackDelay;

	// Token: 0x04000807 RID: 2055
	public float arcadeBulletSpeed;

	// Token: 0x04000808 RID: 2056
	public MinMax arcadeBulletReturnDelay;

	// Token: 0x04000809 RID: 2057
	public int arcadeBulletCount;

	// Token: 0x0400080A RID: 2058
	public float arcadeBulletIndividualDelay;

	// Token: 0x0400080B RID: 2059
	public MinMax magicianAppearDelayRange;

	// Token: 0x0400080C RID: 2060
	public MinMax magicianDeathDelayRange;

	// Token: 0x0400080D RID: 2061
	public float magicianDurationAppear;

	// Token: 0x0400080E RID: 2062
	public float poleSpeedMovement;

	// Token: 0x02000945 RID: 2373
	public enum AimMode
	{
		// Token: 0x040045D1 RID: 17873
		AimedAtPlayer,
		// Token: 0x040045D2 RID: 17874
		ArcAimedAtPlayer,
		// Token: 0x040045D3 RID: 17875
		Straight,
		// Token: 0x040045D4 RID: 17876
		Spread,
		// Token: 0x040045D5 RID: 17877
		Arc
	}

	// Token: 0x02000946 RID: 2374
	public enum LoopMode
	{
		// Token: 0x040045D7 RID: 17879
		PingPong,
		// Token: 0x040045D8 RID: 17880
		Repeat,
		// Token: 0x040045D9 RID: 17881
		Once,
		// Token: 0x040045DA RID: 17882
		DelayAtPoint
	}
}
