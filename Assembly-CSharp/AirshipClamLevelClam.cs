using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200013B RID: 315
public class AirshipClamLevelClam : LevelProperties.AirshipClam.Entity
{
	// Token: 0x06000EE0 RID: 3808 RVA: 0x0000C9BC File Offset: 0x0000ABBC
	public override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x0000C9F2 File Offset: 0x0000ABF2
	public void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x0000C9FF File Offset: 0x0000ABFF
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x0008C5E8 File Offset: 0x0008A7E8
	public override void LevelInit(LevelProperties.AirshipClam properties)
	{
		base.LevelInit(properties);
		this.pivotPoint.x = (float)(Level.Current.Left + Level.Current.Width / 2);
		this.pivotPoint.y = (float)Level.Current.Ground + (float)Level.Current.Height * 0.65f;
		this.pivotPoint.z = 0f;
		this.attacking = false;
		this.clamOut = false;
		this.time = 0f;
		this.idleSpeed = properties.CurrentState.spit.movementSpeedScale;
		this.pShotAttackDelayIndex = Random.Range(0, properties.CurrentState.spit.attackDelayString.Split(new char[]
		{
			','
		}).Length);
		this.barnacleAttackDelayIndex = Random.Range(0, properties.CurrentState.barnacles.attackDelayString.Split(new char[]
		{
			','
		}).Length);
		this.barnacleTypeIndex = Random.Range(0, properties.CurrentState.barnacles.typeString.Split(new char[]
		{
			','
		}).Length);
		this.clamOutShotCountIndex = Random.Range(0, properties.CurrentState.clamOut.shotString.Split(new char[]
		{
			','
		}).Length);
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06000EE4 RID: 3812 RVA: 0x0008C74C File Offset: 0x0008A94C
	public IEnumerator move_cr()
	{
		for (;;)
		{
			if (!this.attacking)
			{
				Vector3 pos = this.pivotPoint + Vector3.right * Mathf.Sin(this.time * this.idleSpeed) * 300f;
				base.transform.position = pos + Vector3.up * Mathf.Sin(this.time * (this.idleSpeed * 4f)) * 50f;
				this.time += CupheadTime.Delta;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000EE5 RID: 3813 RVA: 0x0000CA12 File Offset: 0x0000AC12
	public void OnSpitStart(Action callback)
	{
		this.callback = callback;
		base.StartCoroutine(this.spit_cr());
	}

	// Token: 0x06000EE6 RID: 3814 RVA: 0x0008C768 File Offset: 0x0008A968
	public IEnumerator spit_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.spit.initialShotDelay);
		this.attacking = true;
		base.animator.SetTrigger("OnPearlShot");
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.spit.preShotDelay);
		Vector3 target = PlayerManager.GetNext().center + Vector3.up * 50f;
		float rotation = Vector3.Angle(Vector3.down, base.transform.position - target);
		if (target.x > base.transform.position.x)
		{
			rotation += 270f;
		}
		else
		{
			rotation = 270f - rotation;
		}
		this.pearlPrefab.Create(this.spawnPoints[0].position, -rotation, base.properties.CurrentState.spit.bulletSpeed);
		base.animator.SetTrigger("OnPearlShot");
		yield return base.animator.WaitForAnimationToEnd(this, true);
		this.attacking = false;
		yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(base.properties.CurrentState.spit.attackDelayString.Split(new char[]
		{
			','
		})[this.pShotAttackDelayIndex]));
		this.pShotAttackDelayIndex++;
		if (this.pShotAttackDelayIndex >= base.properties.CurrentState.spit.attackDelayString.Split(new char[]
		{
			','
		}).Length)
		{
			this.pShotAttackDelayIndex = 0;
		}
		if (this.callback != null)
		{
			this.callback();
		}
		yield break;
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x0000CA28 File Offset: 0x0000AC28
	public void OnBarnaclesStart(Action callback)
	{
		this.callback = callback;
		base.StartCoroutine(this.spawnBarnacles_cr());
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x0008C784 File Offset: 0x0008A984
	public IEnumerator spawnBarnacles_cr()
	{
		bool parryable = false;
		for (float duration = base.properties.CurrentState.barnacles.attackDuration.RandomFloat(); duration > 0f; duration -= Parser.FloatParse(base.properties.CurrentState.barnacles.attackDelayString.Split(new char[]
		{
			','
		})[this.barnacleAttackDelayIndex]))
		{
			if (base.properties.CurrentState.barnacles.typeString.Split(new char[]
			{
				','
			})[this.barnacleTypeIndex][0] == 'P')
			{
				parryable = true;
			}
			this.barnacleTypeIndex++;
			if (this.barnacleTypeIndex >= base.properties.CurrentState.barnacles.typeString.Split(new char[]
			{
				','
			}).Length)
			{
				this.barnacleTypeIndex = 0;
			}
			if (!parryable)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.barnaclePrefab.gameObject, this.spawnPoints[0].position, Quaternion.identity);
				gameObject.transform.localScale = Vector3.one * base.properties.CurrentState.barnacles.barnacleScale;
				gameObject.GetComponent<AirshipClamLevelBarnacle>().InitBarnacle(-1, base.properties);
				gameObject = Object.Instantiate<GameObject>(this.barnaclePrefab.gameObject, this.spawnPoints[1].position, Quaternion.identity);
				gameObject.transform.localScale = Vector3.one * base.properties.CurrentState.barnacles.barnacleScale;
				gameObject.GetComponent<AirshipClamLevelBarnacle>().InitBarnacle(1, base.properties);
			}
			else
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.barnacleParryablePrefab.gameObject, this.spawnPoints[0].position, Quaternion.identity);
				gameObject2.transform.localScale = Vector3.one * base.properties.CurrentState.barnacles.barnacleScale;
				gameObject2.GetComponent<AirshipClamLevelBarnacleParryable>().InitBarnacle(-1, base.properties);
				gameObject2 = Object.Instantiate<GameObject>(this.barnacleParryablePrefab.gameObject, this.spawnPoints[1].position, Quaternion.identity);
				gameObject2.transform.localScale = Vector3.one * base.properties.CurrentState.barnacles.barnacleScale;
				gameObject2.GetComponent<AirshipClamLevelBarnacleParryable>().InitBarnacle(1, base.properties);
			}
			yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(base.properties.CurrentState.barnacles.attackDelayString.Split(new char[]
			{
				','
			})[this.barnacleAttackDelayIndex]));
		}
		this.barnacleAttackDelayIndex++;
		if (this.barnacleAttackDelayIndex >= base.properties.CurrentState.barnacles.attackDelayString.Split(new char[]
		{
			','
		}).Length)
		{
			this.barnacleAttackDelayIndex = 0;
		}
		if (!this.clamOut && this.callback != null)
		{
			this.callback();
		}
		yield break;
	}

	// Token: 0x06000EE9 RID: 3817 RVA: 0x0000CA3E File Offset: 0x0000AC3E
	public void OnStringShot()
	{
		base.animator.SetBool("OnStringShot", true);
		base.StartCoroutine(this.clamOut_cr());
	}

	// Token: 0x06000EEA RID: 3818 RVA: 0x0008C7A0 File Offset: 0x0008A9A0
	public IEnumerator clamOut_cr()
	{
		this.damageReceiver.enabled = true;
		int max = Parser.IntParse(base.properties.CurrentState.clamOut.shotString.Split(new char[]
		{
			','
		})[this.clamOutShotCountIndex]);
		Vector3 target = PlayerManager.GetNext().center + Vector3.up * 50f;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.clamOut.preShotDelay);
		for (int i = 0; i < max; i++)
		{
			if (i != 0)
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.clamOut.bulletRepeatDelay);
			}
			target = PlayerManager.GetNext().center;
			target = PlayerManager.GetNext().center + Vector3.up * 50f;
			float rotation = Vector3.Angle(Vector3.down, base.transform.position - target);
			if (target.x > base.transform.position.x)
			{
				rotation += 270f;
			}
			else
			{
				rotation = 270f - rotation;
			}
			this.pearlPrefab.Create(this.spawnPoints[0].position, -rotation, base.properties.CurrentState.spit.bulletSpeed);
		}
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.clamOut.bulletMainDelay);
		this.clamOutShotCountIndex++;
		if (this.clamOutShotCountIndex >= base.properties.CurrentState.clamOut.shotString.Split(new char[]
		{
			','
		}).Length)
		{
			this.clamOutShotCountIndex = 0;
		}
		base.animator.SetBool("OnStringShot", false);
		yield return base.animator.WaitForAnimationToEnd(this, true);
		this.damageReceiver.enabled = false;
		if (this.callback != null)
		{
			this.callback();
		}
		yield break;
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x0000CA5E File Offset: 0x0000AC5E
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06000EEC RID: 3820 RVA: 0x0008C7BC File Offset: 0x0008A9BC
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (!this.attacking)
		{
			AirshipClamLevelBarnacleParryable component = hit.GetComponent<AirshipClamLevelBarnacleParryable>();
			if (component != null && component.parried)
			{
				base.animator.SetBool("OnBarnacles", false);
				this.OnStringShot();
				Object.Destroy(hit.gameObject);
			}
		}
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x06000EED RID: 3821 RVA: 0x0000CA80 File Offset: 0x0000AC80
	public override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x04000C29 RID: 3113
	[SerializeField]
	public BasicProjectile pearlPrefab;

	// Token: 0x04000C2A RID: 3114
	[SerializeField]
	public AirshipClamLevelBarnacle barnaclePrefab;

	// Token: 0x04000C2B RID: 3115
	[SerializeField]
	public AirshipClamLevelBarnacleParryable barnacleParryablePrefab;

	// Token: 0x04000C2C RID: 3116
	public bool attacking;

	// Token: 0x04000C2D RID: 3117
	public float idleSpeed;

	// Token: 0x04000C2E RID: 3118
	public int pShotAttackDelayIndex;

	// Token: 0x04000C2F RID: 3119
	public int barnacleAttackDelayIndex;

	// Token: 0x04000C30 RID: 3120
	public int barnacleTypeIndex;

	// Token: 0x04000C31 RID: 3121
	public bool clamOut;

	// Token: 0x04000C32 RID: 3122
	public int clamOutShotCountIndex;

	// Token: 0x04000C33 RID: 3123
	public Vector3 pivotPoint;

	// Token: 0x04000C34 RID: 3124
	[SerializeField]
	public Transform[] spawnPoints;

	// Token: 0x04000C35 RID: 3125
	public Action callback;

	// Token: 0x04000C36 RID: 3126
	public float time;

	// Token: 0x04000C37 RID: 3127
	public DamageDealer damageDealer;

	// Token: 0x04000C38 RID: 3128
	public DamageReceiver damageReceiver;
}
