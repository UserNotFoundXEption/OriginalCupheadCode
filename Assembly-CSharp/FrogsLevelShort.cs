using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002A4 RID: 676
public class FrogsLevelShort : LevelProperties.Frogs.Entity
{
	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x06001E59 RID: 7769 RVA: 0x0001995C File Offset: 0x00017B5C
	// (set) Token: 0x06001E5A RID: 7770 RVA: 0x00019964 File Offset: 0x00017B64
	public FrogsLevelShort.State state { get; set; }

	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x06001E5B RID: 7771 RVA: 0x0001996D File Offset: 0x00017B6D
	// (set) Token: 0x06001E5C RID: 7772 RVA: 0x00019975 File Offset: 0x00017B75
	public FrogsLevelShort.Direction direction { get; set; }

	// Token: 0x06001E5D RID: 7773 RVA: 0x000B2AB0 File Offset: 0x000B0CB0
	public override void Awake()
	{
		base.Awake();
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = new DamageDealer(1f, 0.3f, DamageDealer.DamageSource.Enemy, true, false, false);
	}

	// Token: 0x06001E5E RID: 7774 RVA: 0x0001997E File Offset: 0x00017B7E
	public void Start()
	{
		Level.Current.OnIntroEvent += this.OnLevelIntro;
	}

	// Token: 0x06001E5F RID: 7775 RVA: 0x00019996 File Offset: 0x00017B96
	public void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x06001E60 RID: 7776 RVA: 0x000199A3 File Offset: 0x00017BA3
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001E61 RID: 7777 RVA: 0x000199C1 File Offset: 0x00017BC1
	public override void LevelInit(LevelProperties.Frogs properties)
	{
		base.LevelInit(properties);
		properties.OnBossDeath += this.OnBossDeath;
	}

	// Token: 0x06001E62 RID: 7778 RVA: 0x000199DC File Offset: 0x00017BDC
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (FrogsLevel.FINAL_FORM)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001E63 RID: 7779 RVA: 0x000B2B0C File Offset: 0x000B0D0C
	public void OnBossDeath()
	{
		AudioManager.Play("level_frogs_short_death");
		this.emitAudioFromObject.Add("level_frogs_short_death");
		AudioManager.PlayLoop("level_frogs_short_death_loop");
		this.emitAudioFromObject.Add("level_frogs_short_death_loop");
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDeath");
	}

	// Token: 0x06001E64 RID: 7780 RVA: 0x000199FA File Offset: 0x00017BFA
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.introDust = null;
		this.rageFireball = null;
		this.rageFireballSpark = null;
		this.clapBullet = null;
		this.clapEffect = null;
	}

	// Token: 0x06001E65 RID: 7781 RVA: 0x00019A25 File Offset: 0x00017C25
	public void SfxClap()
	{
		AudioManager.Play("level_frogs_short_clap_shock");
		this.emitAudioFromObject.Add("level_frogs_short_clap_shock");
	}

	// Token: 0x06001E66 RID: 7782 RVA: 0x00019A41 File Offset: 0x00017C41
	public void SfxEndIntro()
	{
		AudioManager.Stop("level_frogs_short_intro_loop");
		AudioManager.Play("level_frogs_short_intro_start");
		this.emitAudioFromObject.Add("level_frogs_short_intro_start");
	}

	// Token: 0x06001E67 RID: 7783 RVA: 0x00019A67 File Offset: 0x00017C67
	public void OnLevelIntro()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001E68 RID: 7784 RVA: 0x000B2B64 File Offset: 0x000B0D64
	public IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		AudioManager.PlayLoop("level_frogs_short_intro_loop");
		this.emitAudioFromObject.Add("level_frogs_short_intro_loop");
		base.animator.Play("Intro");
		yield break;
	}

	// Token: 0x06001E69 RID: 7785 RVA: 0x00019A76 File Offset: 0x00017C76
	public void PlayIntroEffect()
	{
		this.introDust.Create(base.transform.position);
	}

	// Token: 0x06001E6A RID: 7786 RVA: 0x00019A8F File Offset: 0x00017C8F
	public void StartRage()
	{
		if (this.state != FrogsLevelShort.State.Idle && this.state != FrogsLevelShort.State.Complete)
		{
			return;
		}
		this.state = FrogsLevelShort.State.Rage;
		base.StartCoroutine(this.rage_cr());
	}

	// Token: 0x06001E6B RID: 7787 RVA: 0x000B2B80 File Offset: 0x000B0D80
	public void Shoot(LevelProperties.Frogs.ShortRage properties, Vector3 pos, bool parry)
	{
		int num = (this.direction != FrogsLevelShort.Direction.Left) ? 1 : -1;
		AudioManager.Play("level_frogs_short_fireball");
		this.emitAudioFromObject.Add("level_frogs_short_fireball");
		this.rageFireballSpark.Create(pos, new Vector3((float)num, (float)num, 1f));
		BasicProjectile basicProjectile = this.rageFireball.Create(pos, 0f, new Vector2((float)num, (float)num), properties.shotSpeed * (float)num);
		basicProjectile.SetParryable(parry);
		basicProjectile.CollisionDeath.OnlyPlayer();
	}

	// Token: 0x06001E6C RID: 7788 RVA: 0x000B2C10 File Offset: 0x000B0E10
	public IEnumerator rage_cr()
	{
		LevelProperties.Frogs.ShortRage p = base.properties.CurrentState.shortRage;
		base.animator.SetTrigger("OnRage");
		yield return base.animator.WaitForAnimationToEnd(this, "Rage", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.anticipationDelay);
		base.animator.SetTrigger("OnRageAttack");
		yield return base.animator.WaitForAnimationToEnd(this, "Rage_Anticipate_End", false, true);
		AudioManager.PlayLoop("level_frogs_short_ragefist_attack_loop");
		this.emitAudioFromObject.Add("level_frogs_short_ragefist_attack_loop");
		int shotCount = p.shotCount;
		int root = 0;
		string parryString = p.parryPatterns[Random.Range(0, p.parryPatterns.Length)].ToLower();
		int parryIndex = 0;
		while (shotCount > 0)
		{
			yield return CupheadTime.WaitForSeconds(this, p.shotDelay);
			shotCount--;
			this.Shoot(p, this.rageRoots[root].position, parryString[parryIndex] == 'p');
			root = (int)Mathf.Repeat((float)(root + 1), (float)this.rageRoots.Length);
			parryIndex = (int)Mathf.Repeat((float)(parryIndex + 1), (float)parryString.Length);
		}
		yield return CupheadTime.WaitForSeconds(this, p.shotDelay);
		base.animator.SetTrigger("OnRageEnd");
		AudioManager.Stop("level_frogs_short_ragefist_attack_loop");
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = FrogsLevelShort.State.Complete;
		yield break;
	}

	// Token: 0x06001E6D RID: 7789 RVA: 0x00019AC1 File Offset: 0x00017CC1
	public void StartClap()
	{
		if (this.state != FrogsLevelShort.State.Idle && this.state != FrogsLevelShort.State.Complete)
		{
			return;
		}
		this.state = FrogsLevelShort.State.Clap;
		base.StartCoroutine(this.clap_cr());
	}

	// Token: 0x06001E6E RID: 7790 RVA: 0x000B2C2C File Offset: 0x000B0E2C
	public void ShootClap()
	{
		this.SfxClap();
		this.clapEffect.Create(this.clapRoot.position);
		this.clapBullet.Create(this.direction, this.clapDirection, this.clapRoot.position, this.clapRoot.right * this.clapProperties.bulletSpeed);
	}

	// Token: 0x06001E6F RID: 7791 RVA: 0x000B2CA0 File Offset: 0x000B0EA0
	public IEnumerator clap_cr()
	{
		this.clapProperties = base.properties.CurrentState.shortClap;
		this.clapDirection = ((!Rand.Bool()) ? FrogsLevelShortClapBullet.Direction.Up : FrogsLevelShortClapBullet.Direction.Down);
		this.clapRoot.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.clapProperties.angles.GetRandom<float>()));
		string patternString = this.clapProperties.patterns[Random.Range(0, this.clapProperties.patterns.Length)];
		KeyValue[] pattern = KeyValue.ListFromString(patternString, new char[]
		{
			'S',
			'D'
		});
		base.animator.SetTrigger("OnClap");
		base.animator.SetBool("Clapping", true);
		yield return CupheadTime.WaitForSeconds(this, 1f + this.clapProperties.shotDelay);
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i].key == "S")
			{
				int ii = 0;
				while ((float)ii < pattern[i].value)
				{
					this.clapDirection = ((this.clapDirection != FrogsLevelShortClapBullet.Direction.Down) ? FrogsLevelShortClapBullet.Direction.Down : FrogsLevelShortClapBullet.Direction.Up);
					if (i >= pattern.Length - 1 && (float)ii >= pattern[i].value - 1f)
					{
						base.animator.Play("Clap_End");
						yield return base.animator.WaitForAnimationToEnd(this, "Clap_End", false, true);
					}
					else
					{
						base.animator.Play("Clap_Shoot");
					}
					yield return CupheadTime.WaitForSeconds(this, 0.5f);
					ii++;
				}
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, pattern[i].value);
			}
		}
		base.animator.Play("Idle");
		base.animator.ResetTrigger("OnClap");
		base.animator.SetBool("Clapping", false);
		yield return CupheadTime.WaitForSeconds(this, this.clapProperties.hesitate);
		this.state = FrogsLevelShort.State.Complete;
		yield break;
	}

	// Token: 0x06001E70 RID: 7792 RVA: 0x00019AF3 File Offset: 0x00017CF3
	public void StartRoll()
	{
		this.StopAllCoroutines();
		base.animator.Play("Idle");
		this.state = FrogsLevelShort.State.Roll;
		base.StartCoroutine(this.roll_cr());
	}

	// Token: 0x06001E71 RID: 7793 RVA: 0x00019B1F File Offset: 0x00017D1F
	public bool CheckRollable()
	{
		return this.state == FrogsLevelShort.State.Complete || this.state == FrogsLevelShort.State.Idle;
	}

	// Token: 0x06001E72 RID: 7794 RVA: 0x000B2CBC File Offset: 0x000B0EBC
	public IEnumerator roll_cr()
	{
		yield return null;
		float startX = base.transform.position.x;
		float endX = -(startX + 240f);
		LevelProperties.Frogs.ShortRoll p = base.properties.CurrentState.shortRoll;
		base.animator.SetTrigger("OnRoll");
		yield return CupheadTime.WaitForSeconds(this, 1.2f + p.delay);
		base.animator.SetTrigger("OnRollContinue");
		CupheadLevelCamera.Current.StartShake(4f);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield return base.animator.WaitForAnimationToStart(this, "Roll_Loop", false);
		float t = 0f;
		while (t < p.time)
		{
			float val = t / p.time;
			float x = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, startX, endX, val);
			base.transform.SetPosition(new float?(x), null, null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetPosition(new float?(endX), null, null);
		yield return null;
		CupheadLevelCamera.Current.EndShake(0.5f);
		AudioManager.Stop("level_frogs_short_rolling_loop");
		AudioManager.Play("level_frogs_short_rolling_crash");
		this.emitAudioFromObject.Add("level_frogs_short_rolling_crash");
		this.spriteRenderer.enabled = false;
		this.direction = FrogsLevelShort.Direction.Right;
		yield return CupheadTime.WaitForSeconds(this, p.returnDelay);
		base.transform.SetScale(new float?(-1f), null, null);
		base.transform.SetPosition(new float?(-(startX + 140f)), null, null);
		base.animator.SetTrigger("OnRollContinue");
		AudioManager.Play("level_frogs_short_rolling_end");
		this.emitAudioFromObject.Add("level_frogs_short_rolling_end");
		this.spriteRenderer.enabled = true;
		yield return CupheadTime.WaitForSeconds(this, 1f + p.hesitate);
		this.state = FrogsLevelShort.State.Complete;
		AudioManager.Stop("level_frogs_short_rolling_loop");
		yield break;
	}

	// Token: 0x06001E73 RID: 7795 RVA: 0x00019B3D File Offset: 0x00017D3D
	public void PlayRollSfx()
	{
		AudioManager.PlayLoop("level_frogs_short_rolling_loop");
		this.emitAudioFromObject.Add("level_frogs_short_rolling_loop");
		AudioManager.Play("level_frogs_short_rolling_start");
		this.emitAudioFromObject.Add("level_frogs_short_rolling_start");
	}

	// Token: 0x06001E74 RID: 7796 RVA: 0x00019B73 File Offset: 0x00017D73
	public void StartMorph()
	{
		this.StopAllCoroutines();
		base.animator.Play("Idle");
		this.state = FrogsLevelShort.State.Morphing;
		base.StartCoroutine(this.morphRoll_cr());
	}

	// Token: 0x06001E75 RID: 7797 RVA: 0x000B2CD8 File Offset: 0x000B0ED8
	public IEnumerator morphRoll_cr()
	{
		Vector2 start = base.transform.position;
		Vector2 end = FrogsLevelTall.Current.shortMorphRoot.position;
		base.animator.SetTrigger("OnRoll");
		yield return base.animator.WaitForAnimationToEnd(this, "Roll", false, true);
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		base.animator.SetTrigger("OnRollContinue");
		CupheadLevelCamera.Current.StartShake(4f);
		AudioManager.PlayLoop("level_frogs_short_rolling_loop");
		this.emitAudioFromObject.Add("level_frogs_short_rolling_loop");
		AudioManager.Play("level_frogs_short_rolling_start");
		this.emitAudioFromObject.Add("level_frogs_short_rolling_start");
		float t = 0f;
		while (t < 1f)
		{
			float val = t / 1f;
			float x = EaseUtils.Ease(EaseUtils.EaseType.linear, start.x, end.x, val);
			float y = EaseUtils.Ease(EaseUtils.EaseType.linear, start.y, end.y, val);
			base.transform.SetPosition(new float?(x), new float?(y), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		AudioManager.Stop("level_frogs_short_rolling_loop");
		base.transform.SetPosition(new float?(end.x), new float?(end.y), null);
		CupheadLevelCamera.Current.EndShake(0.5f);
		yield return null;
		FrogsLevelTall.Current.ContinueMorph();
		this.spriteRenderer.enabled = false;
		base.properties.OnBossDeath -= this.OnBossDeath;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x040018C9 RID: 6345
	[SerializeField]
	public Effect introDust;

	// Token: 0x040018CA RID: 6346
	[SerializeField]
	public Transform[] rageRoots;

	// Token: 0x040018CB RID: 6347
	[SerializeField]
	public FrogsLevelShortRageBullet rageFireball;

	// Token: 0x040018CC RID: 6348
	[SerializeField]
	public Effect rageFireballSpark;

	// Token: 0x040018CD RID: 6349
	[SerializeField]
	public FrogsLevelShortClapBullet clapBullet;

	// Token: 0x040018CE RID: 6350
	[SerializeField]
	public Effect clapEffect;

	// Token: 0x040018CF RID: 6351
	[SerializeField]
	public Transform clapRoot;

	// Token: 0x040018D2 RID: 6354
	public SpriteRenderer spriteRenderer;

	// Token: 0x040018D3 RID: 6355
	public DamageReceiver damageReceiver;

	// Token: 0x040018D4 RID: 6356
	public DamageDealer damageDealer;

	// Token: 0x040018D5 RID: 6357
	public LevelProperties.Frogs.ShortClap clapProperties;

	// Token: 0x040018D6 RID: 6358
	public FrogsLevelShortClapBullet.Direction clapDirection;

	// Token: 0x040018D7 RID: 6359
	public const float MORPH_ROLL_TIME = 1f;

	// Token: 0x02000D78 RID: 3448
	public enum State
	{
		// Token: 0x04006188 RID: 24968
		Idle,
		// Token: 0x04006189 RID: 24969
		Rage,
		// Token: 0x0400618A RID: 24970
		Roll,
		// Token: 0x0400618B RID: 24971
		Clap,
		// Token: 0x0400618C RID: 24972
		Morphing,
		// Token: 0x0400618D RID: 24973
		Complete = 1000,
		// Token: 0x0400618E RID: 24974
		Morphed
	}

	// Token: 0x02000D79 RID: 3449
	public enum Direction
	{
		// Token: 0x04006190 RID: 24976
		Left,
		// Token: 0x04006191 RID: 24977
		Right
	}
}
