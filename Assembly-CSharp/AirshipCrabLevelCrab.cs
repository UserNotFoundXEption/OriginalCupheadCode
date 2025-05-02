using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200013D RID: 317
public class AirshipCrabLevelCrab : LevelProperties.AirshipCrab.Entity
{
	// Token: 0x1700022C RID: 556
	// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x0000CB14 File Offset: 0x0000AD14
	// (set) Token: 0x06000EF6 RID: 3830 RVA: 0x0000CB1C File Offset: 0x0000AD1C
	public AirshipCrabLevelCrab.State state { get; set; }

	// Token: 0x06000EF7 RID: 3831 RVA: 0x0008C838 File Offset: 0x0008AA38
	public override void Awake()
	{
		base.Awake();
		this.gems = new List<AirshipCrabLevelGems>();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = this.crabHitBox.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.crabHitBox.enabled = false;
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x0008C898 File Offset: 0x0008AA98
	public override void LevelInit(LevelProperties.AirshipCrab properties)
	{
		base.LevelInit(properties);
		base.StartCoroutine(this.intro_cr());
		this.closedPos = base.transform.position;
		this.openPos = base.transform.position;
		this.openPos.y = base.transform.position.y + properties.CurrentState.main.openCrabOffsetY;
	}

	// Token: 0x06000EF9 RID: 3833 RVA: 0x0000CB25 File Offset: 0x0000AD25
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06000EFA RID: 3834 RVA: 0x0000CB38 File Offset: 0x0000AD38
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000EFB RID: 3835 RVA: 0x0000CB50 File Offset: 0x0000AD50
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06000EFC RID: 3836 RVA: 0x0008C90C File Offset: 0x0008AB0C
	public IEnumerator intro_cr()
	{
		this.state = AirshipCrabLevelCrab.State.Closed;
		yield return CupheadTime.WaitForSeconds(this, 3f);
		base.StartCoroutine(this.barnacle_cr());
		base.StartCoroutine(this.spawn_gems_cr());
		yield break;
	}

	// Token: 0x06000EFD RID: 3837 RVA: 0x0008C928 File Offset: 0x0008AB28
	public void StateHandler()
	{
		if (this.state == AirshipCrabLevelCrab.State.Open)
		{
			base.transform.position = this.openPos;
			this.crabHitBox.enabled = true;
			if (this.stateCoroutine != null)
			{
				base.StopCoroutine(this.stateCoroutine);
			}
			this.stateCoroutine = base.StartCoroutine(this.bubbles_cr());
		}
		else if (this.state == AirshipCrabLevelCrab.State.Closed)
		{
			base.transform.position = this.closedPos;
			this.crabHitBox.enabled = false;
			if (this.stateCoroutine != null)
			{
				base.StopCoroutine(this.stateCoroutine);
			}
			this.stateCoroutine = base.StartCoroutine(this.gems_cr());
		}
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x0008C9E0 File Offset: 0x0008ABE0
	public IEnumerator barnacle_cr()
	{
		LevelProperties.AirshipCrab.Barnicles p = base.properties.CurrentState.barnicles;
		float offsetY = p.barnicleOffsetY;
		float offsetX = p.barnicleOffsetX;
		float rotation = Mathf.Atan2(0f, (float)Level.Current.Left) * 57.29578f;
		for (;;)
		{
			int i = 0;
			while ((float)i < p.barnicleAmount)
			{
				Vector2 pos = this.barncileRoot.position;
				pos.y = this.barncileRoot.position.y + offsetY * (float)i;
				pos.x = this.barncileRoot.position.x + offsetX;
				this.barnicleProjectile.Create(pos, rotation, p.bulletSpeed);
				yield return CupheadTime.WaitForSeconds(this, p.shotDelay);
				i++;
			}
			yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		}
		yield break;
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x0008C9FC File Offset: 0x0008ABFC
	public IEnumerator spawn_gems_cr()
	{
		this.state = AirshipCrabLevelCrab.State.Closed;
		LevelProperties.AirshipCrab.Gems p = base.properties.CurrentState.gems;
		string[] anglePattern = p.angleString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int angleIndex = 0;
		float angle = 0f;
		float offsetX = p.gemOffsetX;
		float offsetY = p.gemOffsetY;
		int i = 0;
		while ((float)i < p.gemAmount)
		{
			Parser.FloatTryParse(anglePattern[angleIndex], out angle);
			Vector2 pos = this.barncileRoot.position;
			pos.y = this.barncileRoot.position.y + offsetY * (float)i;
			pos.x = this.barncileRoot.position.x + offsetX;
			AirshipCrabLevelGems gem = Object.Instantiate<AirshipCrabLevelGems>(this.gemProjectile);
			gem.Init(p, pos, angle);
			this.gems.Add(gem);
			angleIndex = (angleIndex + 1) % anglePattern.Length;
			yield return null;
			i++;
		}
		this.releaseAllAtOnce = false;
		base.StartCoroutine(this.gems_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06000F00 RID: 3840 RVA: 0x0008CA18 File Offset: 0x0008AC18
	public IEnumerator gems_cr()
	{
		LevelProperties.AirshipCrab.Gems p = base.properties.CurrentState.gems;
		string[] delayPattern = p.gemReleaseDelay.GetRandom<string>().Split(new char[]
		{
			','
		});
		float waitTime = 0f;
		float t = 0f;
		int delayIndex = 0;
		int counter = 0;
		bool checking = true;
		bool startTimer = false;
		bool resetTimer = false;
		int i = 0;
		while ((float)i < p.gemATKAmount)
		{
			if (!this.gems[i].moving)
			{
				if (!this.releaseAllAtOnce)
				{
					Parser.FloatTryParse(delayPattern[delayIndex], out waitTime);
				}
				this.gems[i].parried = false;
				this.gems[i].lastSideHit = AirshipCrabLevelGems.SideHit.None;
				this.gems[i].PickMovement();
				if (!this.releaseAllAtOnce)
				{
					yield return CupheadTime.WaitForSeconds(this, waitTime);
					delayIndex %= delayPattern.Length;
				}
			}
			i++;
		}
		while (checking)
		{
			int num = 0;
			while ((float)num < p.gemATKAmount)
			{
				if (this.gems[num].parried)
				{
					counter++;
				}
				if (this.gems[num].startTimer)
				{
					this.gems[num].startTimer = false;
					startTimer = true;
					resetTimer = true;
				}
				if ((float)counter == p.gemATKAmount)
				{
					checking = false;
					break;
				}
				num++;
			}
			if (startTimer)
			{
				if (resetTimer)
				{
					t = 0f;
					resetTimer = false;
				}
				if (t < p.gemHoldDuration)
				{
					t += CupheadTime.Delta;
				}
				else
				{
					this.releaseAllAtOnce = true;
					this.state = AirshipCrabLevelCrab.State.Closed;
					this.StateHandler();
					startTimer = false;
				}
			}
			counter = 0;
			yield return null;
		}
		this.releaseAllAtOnce = false;
		this.state = AirshipCrabLevelCrab.State.Open;
		this.StateHandler();
		yield break;
	}

	// Token: 0x06000F01 RID: 3841 RVA: 0x0008CA34 File Offset: 0x0008AC34
	public IEnumerator bubbles_cr()
	{
		this.state = AirshipCrabLevelCrab.State.Open;
		LevelProperties.AirshipCrab.Bubbles p = base.properties.CurrentState.bubbles;
		string[] bubblePattern = p.bubbleCount.GetRandom<string>().Split(new char[]
		{
			','
		});
		int index = 0;
		base.StartCoroutine(this.bubble_timer_cr());
		while (this.state == AirshipCrabLevelCrab.State.Open)
		{
			float count;
			Parser.FloatTryParse(bubblePattern[index], out count);
			int i = 0;
			while ((float)i < count)
			{
				AirshipCrabLevelBubbles bubbles = Object.Instantiate<AirshipCrabLevelBubbles>(this.bubbleProjectile);
				bubbles.Init(this.bubbleRoot.transform.position, p, p.bubbleSpeed);
				yield return CupheadTime.WaitForSeconds(this, p.bubbleRepeatDelay);
				i++;
			}
			index = (index + 1) % bubblePattern.Length;
			yield return CupheadTime.WaitForSeconds(this, p.bubbleMainDelay);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000F02 RID: 3842 RVA: 0x0008CA50 File Offset: 0x0008AC50
	public IEnumerator bubble_timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.bubbles.openTimer);
		this.state = AirshipCrabLevelCrab.State.Closed;
		base.StopCoroutine(this.bubbles_cr());
		this.StateHandler();
		yield break;
	}

	// Token: 0x04000C3C RID: 3132
	[SerializeField]
	public Transform barncileRoot;

	// Token: 0x04000C3D RID: 3133
	[SerializeField]
	public Transform bubbleRoot;

	// Token: 0x04000C3E RID: 3134
	[SerializeField]
	public Collider2D crabHitBox;

	// Token: 0x04000C3F RID: 3135
	[SerializeField]
	public BasicProjectile barnicleProjectile;

	// Token: 0x04000C40 RID: 3136
	[SerializeField]
	public AirshipCrabLevelBubbles bubbleProjectile;

	// Token: 0x04000C41 RID: 3137
	[SerializeField]
	public AirshipCrabLevelGems gemProjectile;

	// Token: 0x04000C43 RID: 3139
	public DamageDealer damageDealer;

	// Token: 0x04000C44 RID: 3140
	public DamageReceiver damageReceiver;

	// Token: 0x04000C45 RID: 3141
	public Vector3 closedPos;

	// Token: 0x04000C46 RID: 3142
	public Vector3 openPos;

	// Token: 0x04000C47 RID: 3143
	public List<AirshipCrabLevelGems> gems;

	// Token: 0x04000C48 RID: 3144
	public Coroutine stateCoroutine;

	// Token: 0x04000C49 RID: 3145
	public bool releaseAllAtOnce;

	// Token: 0x020009DE RID: 2526
	public enum State
	{
		// Token: 0x04004947 RID: 18759
		Closed,
		// Token: 0x04004948 RID: 18760
		Open,
		// Token: 0x04004949 RID: 18761
		Dead
	}
}
