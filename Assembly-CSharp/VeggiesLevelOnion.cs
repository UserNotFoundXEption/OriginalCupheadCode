using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003CD RID: 973
public class VeggiesLevelOnion : LevelProperties.Veggies.Entity
{
	// Token: 0x1700033A RID: 826
	// (get) Token: 0x06002AD3 RID: 10963 RVA: 0x00023FB4 File Offset: 0x000221B4
	// (set) Token: 0x06002AD4 RID: 10964 RVA: 0x00023FBC File Offset: 0x000221BC
	public VeggiesLevelOnion.State state { get; set; }

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x06002AD5 RID: 10965 RVA: 0x00023FC5 File Offset: 0x000221C5
	// (set) Token: 0x06002AD6 RID: 10966 RVA: 0x00023FCD File Offset: 0x000221CD
	public bool HappyLeave { get; set; }

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x06002AD7 RID: 10967 RVA: 0x000D4E1C File Offset: 0x000D301C
	// (remove) Token: 0x06002AD8 RID: 10968 RVA: 0x000D4E54 File Offset: 0x000D3054
	public event VeggiesLevelOnion.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x06002AD9 RID: 10969 RVA: 0x000D4E8C File Offset: 0x000D308C
	// (remove) Token: 0x06002ADA RID: 10970 RVA: 0x000D4EC4 File Offset: 0x000D30C4
	public event Action OnHappyLeave;

	// Token: 0x06002ADB RID: 10971 RVA: 0x000D4EFC File Offset: 0x000D30FC
	public void Start()
	{
		if (this.properties == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		this.noSecret = true;
		this.circleCollider = base.GetComponent<CircleCollider2D>();
		this.state = VeggiesLevelOnion.State.Idle;
		base.StartCoroutine(this.happyTimer_cr());
		this.SfxGround();
	}

	// Token: 0x06002ADC RID: 10972 RVA: 0x00023FD6 File Offset: 0x000221D6
	public void SfxGround()
	{
		AudioManager.Play("level_veggies_onion_rise");
	}

	// Token: 0x06002ADD RID: 10973 RVA: 0x00023FE2 File Offset: 0x000221E2
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002ADE RID: 10974 RVA: 0x000D4F50 File Offset: 0x000D3150
	public override void LevelInitWithGroup(AbstractLevelPropertyGroup propertyGroup)
	{
		base.LevelInitWithGroup(propertyGroup);
		this.properties = (propertyGroup as LevelProperties.Veggies.Onion);
		this.hp = (float)this.properties.hp;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = new DamageDealer(1f, 0.2f, true, false, false);
		this.damageDealer.SetDirection(DamageDealer.Direction.Neutral, base.transform);
	}

	// Token: 0x06002ADF RID: 10975 RVA: 0x000D4FC4 File Offset: 0x000D31C4
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.state == VeggiesLevelOnion.State.Idle)
		{
			this.state = VeggiesLevelOnion.State.Crying;
			this.noSecret = false;
			base.animator.SetTrigger("SadStart");
			return;
		}
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06002AE0 RID: 10976 RVA: 0x00023FFA File Offset: 0x000221FA
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002AE1 RID: 10977 RVA: 0x00024018 File Offset: 0x00022218
	public void OnDeathAnimComplete()
	{
		this.state = VeggiesLevelOnion.State.Complete;
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002AE2 RID: 10978 RVA: 0x0002402C File Offset: 0x0002222C
	public void Die()
	{
		this.StopCrying();
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x06002AE3 RID: 10979 RVA: 0x00024047 File Offset: 0x00022247
	public void StartExplosions()
	{
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
	}

	// Token: 0x06002AE4 RID: 10980 RVA: 0x00024054 File Offset: 0x00022254
	public void StopExplosions()
	{
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
	}

	// Token: 0x06002AE5 RID: 10981 RVA: 0x000D5040 File Offset: 0x000D3240
	public void StartCrying()
	{
		AudioManager.Play("level_veggies_onion_crying");
		this.rightStream = this.tearStreamPrefab.Create(this.rightRoot.position, 1);
		this.leftStream = this.tearStreamPrefab.Create(this.leftRoot.position, -1);
		this.StartTearCoroutines();
	}

	// Token: 0x06002AE6 RID: 10982 RVA: 0x000D50A4 File Offset: 0x000D32A4
	public void StopCrying()
	{
		AudioManager.Stop("level_veggies_onion_crying");
		this.currentCryLoop = 0;
		this.targetCryLoops = this.properties.cryLoops.RandomInt();
		base.animator.SetBool("ContinueCrying", true);
		if (this.rightStream != null)
		{
			this.rightStream.End();
			this.rightStream = null;
		}
		if (this.leftStream != null)
		{
			this.leftStream.End();
			this.leftStream = null;
		}
		this.StopTearCoroutines();
	}

	// Token: 0x06002AE7 RID: 10983 RVA: 0x00024061 File Offset: 0x00022261
	public void CryLoop()
	{
		this.currentCryLoop++;
		if (this.currentCryLoop >= this.targetCryLoops)
		{
			base.animator.SetBool("ContinueCrying", false);
		}
	}

	// Token: 0x06002AE8 RID: 10984 RVA: 0x000D5138 File Offset: 0x000D3338
	public void BashfulAnimComplete()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector2 pos;
		bool onLeft;
		if (next.transform.position.x > base.transform.position.x)
		{
			pos = this.radishRootRight.position;
			onLeft = false;
		}
		else
		{
			pos = this.radishRootLeft.position;
			onLeft = true;
		}
		this.homingHeartPrefab.CreateRadish(pos, this.properties.heartMaxSpeed, this.properties.heartAcceleration, this.properties.heartHP, onLeft);
		this.state = VeggiesLevelOnion.State.Complete;
	}

	// Token: 0x06002AE9 RID: 10985 RVA: 0x000D51D8 File Offset: 0x000D33D8
	public IEnumerator happyTimer_cr()
	{
		float t = 0f;
		while (t < this.properties.happyTime)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		if (this.noSecret)
		{
			if (this.OnHappyLeave != null)
			{
				this.OnHappyLeave();
			}
			if (this.state == VeggiesLevelOnion.State.Idle)
			{
				this.HappyLeave = true;
				base.animator.SetTrigger("HappyExit");
				base.StartCoroutine(this.handle_dirt_cr());
			}
		}
		yield break;
	}

	// Token: 0x06002AEA RID: 10986 RVA: 0x00024093 File Offset: 0x00022293
	public void StopTearCoroutines()
	{
		if (this.rightTearsCoroutine != null)
		{
			base.StopCoroutine(this.rightTearsCoroutine);
		}
		if (this.leftTearsCoroutine != null)
		{
			base.StopCoroutine(this.leftTearsCoroutine);
		}
		this.rightTearsCoroutine = null;
		this.leftTearsCoroutine = null;
	}

	// Token: 0x06002AEB RID: 10987 RVA: 0x000D51F4 File Offset: 0x000D33F4
	public void StartTearCoroutines()
	{
		this.StopTearCoroutines();
		string pattern = this.properties.tearPatterns.GetRandom<string>().ToUpper();
		this.rightTearsCoroutine = this.tears_cr(VeggiesLevelOnion.Side.Right, pattern);
		this.leftTearsCoroutine = this.tears_cr(VeggiesLevelOnion.Side.Left, pattern);
		base.StartCoroutine(this.rightTearsCoroutine);
		base.StartCoroutine(this.leftTearsCoroutine);
	}

	// Token: 0x06002AEC RID: 10988 RVA: 0x000D5254 File Offset: 0x000D3454
	public IEnumerator tears_cr(VeggiesLevelOnion.Side side, string pattern)
	{
		float tearDelay = Random.Range(0.3f, 0.7f);
		yield return CupheadTime.WaitForSeconds(this, this.properties.tearAnticipate);
		string[] patterns = pattern.Split(new char[]
		{
			','
		});
		int currentPattern = 0;
		int numUntilPink = this.properties.pinkTearRange.RandomInt();
		for (;;)
		{
			if (patterns[currentPattern][0] == 'D')
			{
				float wait = 0f;
				bool success = Parser.FloatTryParse(patterns[currentPattern].Replace("D", string.Empty), out wait);
				if (success)
				{
					yield return CupheadTime.WaitForSeconds(this, wait);
				}
			}
			else
			{
				string[] destinations = patterns[currentPattern].Split(new char[]
				{
					'-'
				});
				for (int i = 0; i < destinations.Length; i++)
				{
					float x = 0f;
					bool success2 = Parser.FloatTryParse(destinations[i], out x);
					if (success2)
					{
						numUntilPink--;
						if (numUntilPink <= 0)
						{
							yield return CupheadTime.WaitForSeconds(this, tearDelay);
							numUntilPink = this.properties.pinkTearRange.RandomInt();
							this.pinkProjectilePrefab.Create(this.properties.tearTime, (side != VeggiesLevelOnion.Side.Right) ? (-x) : x);
						}
						else
						{
							yield return CupheadTime.WaitForSeconds(this, tearDelay);
							this.projectilePrefab.Create(this.properties.tearTime, (side != VeggiesLevelOnion.Side.Right) ? (-x) : x);
						}
						tearDelay = Random.Range(0.3f, 0.7f);
					}
				}
			}
			currentPattern = (int)Mathf.Repeat((float)(currentPattern + 1), (float)patterns.Length);
			yield return CupheadTime.WaitForSeconds(this, this.properties.tearCommaDelay);
		}
		yield break;
	}

	// Token: 0x06002AED RID: 10989 RVA: 0x000D5280 File Offset: 0x000D3480
	public IEnumerator die_cr()
	{
		this.circleCollider.enabled = false;
		AudioManager.Play("level_veggies_onion_die");
		base.animator.Play("Sad_Die");
		this.StartExplosions();
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.StopExplosions();
		yield return null;
		yield break;
	}

	// Token: 0x06002AEE RID: 10990 RVA: 0x000D529C File Offset: 0x000D349C
	public IEnumerator handle_dirt_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		base.animator.SetTrigger("FadeDirt");
		yield break;
	}

	// Token: 0x06002AEF RID: 10991 RVA: 0x000240D1 File Offset: 0x000222D1
	public void OnionVoiceExisBashfulSFX()
	{
		AudioManager.Play("level_veggies_onion_exit_bashful");
		this.emitAudioFromObject.Add("level_veggies_onion_exit_bashful");
	}

	// Token: 0x040023A6 RID: 9126
	public const float START_SHOOTING_TIME = 0.6f;

	// Token: 0x040023A9 RID: 9129
	[SerializeField]
	public Transform leftRoot;

	// Token: 0x040023AA RID: 9130
	[SerializeField]
	public Transform rightRoot;

	// Token: 0x040023AB RID: 9131
	[SerializeField]
	public Transform radishRootRight;

	// Token: 0x040023AC RID: 9132
	[SerializeField]
	public Transform radishRootLeft;

	// Token: 0x040023AD RID: 9133
	[SerializeField]
	public VeggiesLevelOnionTearsStream tearStreamPrefab;

	// Token: 0x040023AE RID: 9134
	[SerializeField]
	public VeggiesLevelOnionTearProjectile projectilePrefab;

	// Token: 0x040023AF RID: 9135
	[SerializeField]
	public VeggiesLevelOnionTearProjectile pinkProjectilePrefab;

	// Token: 0x040023B0 RID: 9136
	[SerializeField]
	public VeggiesLevelOnionHomingHeart homingHeartPrefab;

	// Token: 0x040023B1 RID: 9137
	public new LevelProperties.Veggies.Onion properties;

	// Token: 0x040023B2 RID: 9138
	public CircleCollider2D circleCollider;

	// Token: 0x040023B3 RID: 9139
	public float hp;

	// Token: 0x040023B4 RID: 9140
	public DamageDealer damageDealer;

	// Token: 0x040023B5 RID: 9141
	public VeggiesLevelOnionTearsStream rightStream;

	// Token: 0x040023B6 RID: 9142
	public VeggiesLevelOnionTearsStream leftStream;

	// Token: 0x040023B7 RID: 9143
	public int currentCryLoop;

	// Token: 0x040023B8 RID: 9144
	public int targetCryLoops = 8;

	// Token: 0x040023BB RID: 9147
	public bool noSecret;

	// Token: 0x040023BC RID: 9148
	public IEnumerator rightTearsCoroutine;

	// Token: 0x040023BD RID: 9149
	public IEnumerator leftTearsCoroutine;

	// Token: 0x02000FE7 RID: 4071
	public enum Side
	{
		// Token: 0x04007217 RID: 29207
		Left = -1,
		// Token: 0x04007218 RID: 29208
		Right = 1
	}

	// Token: 0x02000FE8 RID: 4072
	public enum State
	{
		// Token: 0x0400721A RID: 29210
		Idle,
		// Token: 0x0400721B RID: 29211
		Crying,
		// Token: 0x0400721C RID: 29212
		Complete
	}

	// Token: 0x02000FE9 RID: 4073
	// (Invoke) Token: 0x06007695 RID: 30357
	public delegate void OnDamageTakenHandler(float damage);
}
