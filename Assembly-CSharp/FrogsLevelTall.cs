using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002A7 RID: 679
public class FrogsLevelTall : LevelProperties.Frogs.Entity
{
	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x06001E7F RID: 7807 RVA: 0x00019C22 File Offset: 0x00017E22
	// (set) Token: 0x06001E80 RID: 7808 RVA: 0x00019C2A File Offset: 0x00017E2A
	public FrogsLevelTall.State state { get; set; }

	// Token: 0x06001E81 RID: 7809 RVA: 0x000B2D50 File Offset: 0x000B0F50
	public override void Awake()
	{
		base.Awake();
		FrogsLevelTall.Current = this;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = new DamageDealer(1f, 0.3f, DamageDealer.DamageSource.Enemy, true, false, false);
	}

	// Token: 0x06001E82 RID: 7810 RVA: 0x00019C33 File Offset: 0x00017E33
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (FrogsLevelTall.Current == this)
		{
			FrogsLevelTall.Current = null;
		}
		this.fireflyPrefab = null;
	}

	// Token: 0x06001E83 RID: 7811 RVA: 0x00019C58 File Offset: 0x00017E58
	public void Start()
	{
		Level.Current.OnIntroEvent += this.OnLevelIntro;
	}

	// Token: 0x06001E84 RID: 7812 RVA: 0x00019C70 File Offset: 0x00017E70
	public void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x06001E85 RID: 7813 RVA: 0x00019C7D File Offset: 0x00017E7D
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001E86 RID: 7814 RVA: 0x00019C9B File Offset: 0x00017E9B
	public override void LevelInit(LevelProperties.Frogs properties)
	{
		base.LevelInit(properties);
		properties.OnBossDeath += this.OnBossDeath;
	}

	// Token: 0x06001E87 RID: 7815 RVA: 0x00019CB6 File Offset: 0x00017EB6
	public void AddFanForce(AbstractPlayerController player)
	{
		if (this.fanForce == null)
		{
			this.fanForce = new LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.All, 0f);
			this.fanForce.enabled = false;
		}
		player.GetComponent<LevelPlayerMotor>().AddForce(this.fanForce);
	}

	// Token: 0x06001E88 RID: 7816 RVA: 0x00019CF1 File Offset: 0x00017EF1
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (FrogsLevel.FINAL_FORM)
		{
			return;
		}
		if (base.properties.CurrentState.stateName != LevelProperties.Frogs.States.Main)
		{
			base.properties.DealDamage(info.damage);
		}
	}

	// Token: 0x06001E89 RID: 7817 RVA: 0x00019D24 File Offset: 0x00017F24
	public void OnBossDeath()
	{
		this.StopAllCoroutines();
		this.fanForce.enabled = false;
		base.animator.SetTrigger("OnDeath");
		AudioManager.Play("level_frogs_tall_death");
	}

	// Token: 0x06001E8A RID: 7818 RVA: 0x00019D52 File Offset: 0x00017F52
	public void OnLevelIntro()
	{
		AudioManager.Play("level_frogs_tall_intro_full");
		base.animator.Play("Intro");
	}

	// Token: 0x06001E8B RID: 7819 RVA: 0x00019D6E File Offset: 0x00017F6E
	public void StartFan()
	{
		if (this.state != FrogsLevelTall.State.Idle && this.state != FrogsLevelTall.State.Complete)
		{
			return;
		}
		this.state = FrogsLevelTall.State.Fan;
		base.StartCoroutine(this.fan_cr());
	}

	// Token: 0x06001E8C RID: 7820 RVA: 0x000B2DA8 File Offset: 0x000B0FA8
	public IEnumerator fan_cr()
	{
		LevelProperties.Frogs.TallFan p = base.properties.CurrentState.tallFan;
		float time = p.duration;
		base.animator.Play("Fan");
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AudioManager.Play("level_frogs_tall_fan_start");
		this.emitAudioFromObject.Add("level_frogs_tall_fan_start");
		base.StartCoroutine(this.fanAccelerate_cr(p));
		yield return CupheadTime.WaitForSeconds(this, 2f);
		AudioManager.PlayLoop("level_frogs_tall_fan_attack_loop");
		this.emitAudioFromObject.Add("level_frogs_tall_fan_attack_loop");
		if (this.firstFan)
		{
			this.firstFan = false;
			float startX = base.transform.position.x;
			yield return CupheadTime.WaitForSeconds(this, 0.25f);
			float t = 0f;
			while (t < 0.5f)
			{
				float val = t / 0.5f;
				float x = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, startX, startX + 60f, val);
				base.transform.SetPosition(new float?(x), null, null);
				t += CupheadTime.Delta;
				yield return null;
			}
			base.transform.SetPosition(new float?(startX + 60f), null, null);
			yield return CupheadTime.WaitForSeconds(this, p.duration.RandomFloat() - 0.75f);
		}
		else
		{
			yield return CupheadTime.WaitForSeconds(this, p.duration.RandomFloat());
		}
		yield return CupheadTime.WaitForSeconds(this, time);
		AudioManager.Play("level_frogs_tall_fan_end");
		this.emitAudioFromObject.Add("level_frogs_tall_fan_end");
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		AudioManager.Stop("level_frogs_tall_fan_attack_loop");
		base.animator.SetTrigger("OnFanEnd");
		base.StartCoroutine(this.fanDecelerate_cr(p));
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		this.state = FrogsLevelTall.State.Complete;
		yield break;
	}

	// Token: 0x06001E8D RID: 7821 RVA: 0x000B2DC4 File Offset: 0x000B0FC4
	public IEnumerator fanAccelerate_cr(LevelProperties.Frogs.TallFan p)
	{
		this.fanForce.enabled = true;
		yield return base.StartCoroutine(this.fanPowerTween_cr(0f, p.power, (float)p.accelerationTime));
		yield break;
	}

	// Token: 0x06001E8E RID: 7822 RVA: 0x000B2DE8 File Offset: 0x000B0FE8
	public IEnumerator fanDecelerate_cr(LevelProperties.Frogs.TallFan p)
	{
		yield return base.StartCoroutine(this.fanPowerTween_cr(p.power, 0f, 0.75f));
		this.fanForce.enabled = false;
		yield break;
	}

	// Token: 0x06001E8F RID: 7823 RVA: 0x000B2E0C File Offset: 0x000B100C
	public IEnumerator fanPowerTween_cr(float start, float end, float time)
	{
		this.fanForce.value = start;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.fanForce.value = Mathf.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.fanForce.value = end;
		yield break;
	}

	// Token: 0x06001E90 RID: 7824 RVA: 0x000B2E3C File Offset: 0x000B103C
	public void StartFireflies()
	{
		this.layer = 0;
		if (this.state != FrogsLevelTall.State.Idle && this.state != FrogsLevelTall.State.Complete)
		{
			return;
		}
		this.state = FrogsLevelTall.State.Fireflies;
		this.fireflyCount = 0;
		base.animator.SetBool("EndFirefly", false);
		base.StartCoroutine(this.fireflies_cr());
	}

	// Token: 0x06001E91 RID: 7825 RVA: 0x00019DA0 File Offset: 0x00017FA0
	public void ResetFireflyRoots()
	{
		this.tempRoots = new List<FrogsLevelTallFireflyRoot>(this.fireflyRoots);
	}

	// Token: 0x06001E92 RID: 7826 RVA: 0x000B2E98 File Offset: 0x000B1098
	public void ShootFirefly()
	{
		AudioManager.Play("level_frogs_tall_spit_shoot");
		this.emitAudioFromObject.Add("level_frogs_tall_spit_shoot");
		FrogsLevelTallFireflyRoot frogsLevelTallFireflyRoot = this.tempRoots[Random.Range(0, this.tempRoots.Count)];
		this.tempRoots.Remove(frogsLevelTallFireflyRoot);
		Vector2 vector = frogsLevelTallFireflyRoot.transform.position;
		Vector2 vector2 = vector;
		Vector2 vector3;
		vector3..ctor(Random.value * (float)((!Rand.Bool()) ? -1 : 1), Random.value * (float)((!Rand.Bool()) ? -1 : 1));
		vector = vector2 + vector3.normalized * frogsLevelTallFireflyRoot.radius * Random.value;
		this.fireflyPrefab.Create(this.spitRoot.position, vector, this.fireflyProperties.speed, this.fireflyProperties.hp, this.fireflyProperties.followDelay, this.fireflyProperties.followTime, this.fireflyProperties.followDistance, this.fireflyProperties.invincibleDuration, PlayerManager.GetNext(), this.layer++);
		this.fireflyCount--;
	}

	// Token: 0x06001E93 RID: 7827 RVA: 0x000B2FD8 File Offset: 0x000B11D8
	public IEnumerator fireflies_cr()
	{
		this.fireflyProperties = base.properties.CurrentState.tallFireflies;
		string patternString = this.fireflyProperties.patterns[Random.Range(0, this.fireflyProperties.patterns.Length)];
		KeyValue[] pattern = KeyValue.ListFromString(patternString, new char[]
		{
			'S',
			'D'
		});
		base.animator.SetTrigger("OnFirefly");
		yield return CupheadTime.WaitForSeconds(this, 2f);
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i].key == "S")
			{
				this.ResetFireflyRoots();
				this.fireflyCount = (int)pattern[i].value;
				base.animator.SetInteger("FireflyCount", this.fireflyCount);
				base.animator.SetTrigger("OnFireflyStart");
				base.animator.SetBool("EndFirefly", i >= pattern.Length - 1);
				while (this.fireflyCount > 0)
				{
					base.animator.SetInteger("FireflyCount", this.fireflyCount);
					yield return null;
				}
				base.animator.SetInteger("FireflyCount", this.fireflyCount);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, pattern[i].value);
			}
		}
		yield return CupheadTime.WaitForSeconds(this, this.fireflyProperties.hesitate);
		this.state = FrogsLevelTall.State.Complete;
		yield break;
	}

	// Token: 0x06001E94 RID: 7828 RVA: 0x00019DB3 File Offset: 0x00017FB3
	public void MorphSFX()
	{
		AudioManager.Play("level_frogs_tall_morph_end");
		this.emitAudioFromObject.Add("level_frogs_tall_morph_end");
	}

	// Token: 0x06001E95 RID: 7829 RVA: 0x00019DCF File Offset: 0x00017FCF
	public void StartMorph()
	{
		this.StopAllCoroutines();
		this.fanForce.value = 0f;
		this.fanForce.enabled = false;
		this.state = FrogsLevelTall.State.Morphing;
		base.animator.Play("Morph");
	}

	// Token: 0x06001E96 RID: 7830 RVA: 0x00019E0A File Offset: 0x0001800A
	public void ContinueMorph()
	{
		base.StartCoroutine(this.morph_cr());
	}

	// Token: 0x06001E97 RID: 7831 RVA: 0x000B2FF4 File Offset: 0x000B11F4
	public IEnumerator morph_cr()
	{
		base.animator.SetTrigger("OnMorphContinue");
		Vector2 start = base.transform.position;
		Vector2 end = new Vector2(631f, -314f);
		float t = 0f;
		while (t < 1f)
		{
			float val = t / 1f;
			float x = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start.x, end.x, val);
			float y = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start.y, end.y, val);
			base.transform.SetPosition(new float?(x), new float?(y), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = end;
		this.state = FrogsLevelTall.State.Morphed;
		yield break;
	}

	// Token: 0x06001E98 RID: 7832 RVA: 0x000B3010 File Offset: 0x000B1210
	public void OnMorphAnimationComplete()
	{
		FrogsLevelMorphed.Current.Enable(FrogsLevel.DEMON_TRIGGERED);
		CupheadLevelCamera.Current.Shake(20f, 0.6f, false);
		base.properties.OnBossDeath -= this.OnBossDeath;
		base.gameObject.SetActive(false);
	}

	// Token: 0x040018DD RID: 6365
	public static FrogsLevelTall Current;

	// Token: 0x040018DE RID: 6366
	[SerializeField]
	public FrogsLevelTallFirefly fireflyPrefab;

	// Token: 0x040018DF RID: 6367
	[SerializeField]
	public FrogsLevelTallFireflyRoot[] fireflyRoots;

	// Token: 0x040018E0 RID: 6368
	[SerializeField]
	public Transform spitRoot;

	// Token: 0x040018E1 RID: 6369
	[Space(10f)]
	public Transform shortMorphRoot;

	// Token: 0x040018E3 RID: 6371
	public LevelPlayerMotor.VelocityManager.Force fanForce;

	// Token: 0x040018E4 RID: 6372
	public DamageReceiver damageReceiver;

	// Token: 0x040018E5 RID: 6373
	public DamageDealer damageDealer;

	// Token: 0x040018E6 RID: 6374
	public int layer;

	// Token: 0x040018E7 RID: 6375
	public const float FAN_START_TIME = 2f;

	// Token: 0x040018E8 RID: 6376
	public const float FAN_END_TIME = 0.5f;

	// Token: 0x040018E9 RID: 6377
	public const float FIRST_FAN_MOVE_OFFSET = 60f;

	// Token: 0x040018EA RID: 6378
	public const float FAN_DECELERATE_TIME = 0.75f;

	// Token: 0x040018EB RID: 6379
	public bool firstFan = true;

	// Token: 0x040018EC RID: 6380
	public int fireflyCount;

	// Token: 0x040018ED RID: 6381
	public List<FrogsLevelTallFireflyRoot> tempRoots;

	// Token: 0x040018EE RID: 6382
	public LevelProperties.Frogs.TallFireflies fireflyProperties;

	// Token: 0x040018EF RID: 6383
	public const float MORPH_MOVE_TIME = 1f;

	// Token: 0x02000D81 RID: 3457
	public enum State
	{
		// Token: 0x040061C8 RID: 25032
		Idle,
		// Token: 0x040061C9 RID: 25033
		Fan,
		// Token: 0x040061CA RID: 25034
		Fireflies,
		// Token: 0x040061CB RID: 25035
		Morphing,
		// Token: 0x040061CC RID: 25036
		Complete = 1000000,
		// Token: 0x040061CD RID: 25037
		Morphed
	}
}
