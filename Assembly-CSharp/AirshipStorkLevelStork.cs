using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class AirshipStorkLevelStork : LevelProperties.AirshipStork.Entity
{
	// Token: 0x06000F3E RID: 3902 RVA: 0x0000CE76 File Offset: 0x0000B076
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06000F3F RID: 3903 RVA: 0x0000CEAC File Offset: 0x0000B0AC
	public void Start()
	{
		CupheadLevelCamera.Current.StartFloat(25f, 3f);
	}

	// Token: 0x06000F40 RID: 3904 RVA: 0x0000CEC2 File Offset: 0x0000B0C2
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000F41 RID: 3905 RVA: 0x0000CEDA File Offset: 0x0000B0DA
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06000F42 RID: 3906 RVA: 0x0008D2DC File Offset: 0x0008B4DC
	public override void LevelInit(LevelProperties.AirshipStork properties)
	{
		base.LevelInit(properties);
		this.knobSwitch = AirshipLevelKnob.Create(this.knobSprite.transform);
		this.knobSwitch.OnActivate += this.OnKnobParry;
		LevelProperties.AirshipStork.Main main = properties.CurrentState.main;
		Vector3 position = base.transform.position;
		position.y = main.headHeight;
		base.transform.position = position;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06000F43 RID: 3907 RVA: 0x0000CEED File Offset: 0x0000B0ED
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06000F44 RID: 3908 RVA: 0x0000CF04 File Offset: 0x0000B104
	public void OnKnobParry()
	{
		base.properties.DealDamage(base.properties.CurrentState.main.parryDamage);
		base.StartCoroutine(this.hurt_cr());
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x0008D35C File Offset: 0x0008B55C
	public IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 5f);
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.spiral_shot_cr());
		base.StartCoroutine(this.babies_cr());
		yield break;
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x0008D378 File Offset: 0x0008B578
	public IEnumerator hurt_cr()
	{
		this.knobSwitch.enabled = false;
		this.knobSprite.GetComponent<SpriteRenderer>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.main.pinkDurationOff);
		this.knobSwitch.enabled = true;
		this.knobSprite.GetComponent<SpriteRenderer>().enabled = true;
		yield break;
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x0008D394 File Offset: 0x0008B594
	public IEnumerator move_cr()
	{
		float offset = 220f;
		float moveTime = 0f;
		LevelProperties.AirshipStork.Main p = base.properties.CurrentState.main;
		string[] leftMovementPattern = p.leftMovementTime.GetRandom<string>().Split(new char[]
		{
			','
		});
		Vector3 pos = base.transform.position;
		Parser.FloatTryParse(leftMovementPattern[this.index], out moveTime);
		float t = 0f;
		for (;;)
		{
			if (this.farRight)
			{
				while (t < moveTime)
				{
					base.transform.position -= base.transform.right * (p.movementSpeed * CupheadTime.Delta);
					t += CupheadTime.Delta;
					yield return null;
				}
				t = 0f;
				this.farRight = !this.farRight;
			}
			else
			{
				while (base.transform.position.x < 640f - offset)
				{
					pos.x = Mathf.MoveTowards(base.transform.position.x, 640f - offset, p.movementSpeed * CupheadTime.Delta);
					base.transform.position = pos;
					yield return null;
				}
				this.farRight = !this.farRight;
			}
			moveTime = (moveTime + 1f) % (float)leftMovementPattern.Length;
		}
		yield break;
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x0008D3B0 File Offset: 0x0008B5B0
	public IEnumerator spiral_shot_cr()
	{
		LevelProperties.AirshipStork.SpiralShot p = base.properties.CurrentState.spiralShot;
		string[] pinkPattern = p.pinkString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] delayPattern = p.shotDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] directionPattern = p.spiralDirection.GetRandom<string>().Split(new char[]
		{
			','
		});
		int delayIndex = Random.Range(0, delayPattern.Length);
		int pinkIndex = Random.Range(0, pinkPattern.Length);
		int directionIndex = Random.Range(0, directionPattern.Length);
		float seconds = 0f;
		int direction = 0;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, seconds);
			Parser.FloatTryParse(delayPattern[delayIndex], out seconds);
			Parser.IntTryParse(directionPattern[directionIndex], out direction);
			if (pinkPattern[pinkIndex][0] == 'R')
			{
				this.projectile.Create(this.projectileRoot.transform.position, 0f, p.movementSpeed, p.spiralRate, direction);
			}
			else if (pinkPattern[pinkIndex][0] == 'P')
			{
				this.projectilePink.Create(this.projectileRoot.transform.position, 0f, p.movementSpeed, p.spiralRate, direction);
			}
			pinkIndex = (pinkIndex + 1) % pinkPattern.Length;
			delayIndex = (delayIndex + 1) % delayPattern.Length;
			directionIndex = (directionIndex + 1) % directionPattern.Length;
		}
		yield break;
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x0008D3CC File Offset: 0x0008B5CC
	public IEnumerator babies_cr()
	{
		LevelProperties.AirshipStork.Babies p = base.properties.CurrentState.babies;
		string[] delayPattern = p.babyDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int index = Random.Range(0, delayPattern.Length);
		float delay = 0f;
		for (;;)
		{
			Parser.FloatTryParse(delayPattern[index], out delay);
			yield return CupheadTime.WaitForSeconds(this, delay);
			Vector2 pos = base.transform.position;
			pos.y = (float)Level.Current.Ground;
			pos.x = (float)Level.Current.Right;
			AirshipStorkLevelBaby baby = Object.Instantiate<AirshipStorkLevelBaby>(this.babyPrefab);
			baby.Init(p, pos, p.HP);
			yield return null;
			index = (index + 1) % delayPattern.Length;
		}
		yield break;
	}

	// Token: 0x04000C72 RID: 3186
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x04000C73 RID: 3187
	[SerializeField]
	public Transform knobSprite;

	// Token: 0x04000C74 RID: 3188
	[SerializeField]
	public AirshipStorkLevelProjectile projectile;

	// Token: 0x04000C75 RID: 3189
	[SerializeField]
	public AirshipStorkLevelProjectile projectilePink;

	// Token: 0x04000C76 RID: 3190
	[SerializeField]
	public AirshipStorkLevelBaby babyPrefab;

	// Token: 0x04000C77 RID: 3191
	public bool farRight = true;

	// Token: 0x04000C78 RID: 3192
	public int index;

	// Token: 0x04000C79 RID: 3193
	public DamageDealer damageDealer;

	// Token: 0x04000C7A RID: 3194
	public DamageReceiver damageReceiver;

	// Token: 0x04000C7B RID: 3195
	public AirshipLevelKnob knobSwitch;
}
