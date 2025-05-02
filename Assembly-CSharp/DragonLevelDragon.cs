using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200020D RID: 525
public class DragonLevelDragon : LevelProperties.Dragon.Entity
{
	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06001809 RID: 6153 RVA: 0x00014895 File Offset: 0x00012A95
	// (set) Token: 0x0600180A RID: 6154 RVA: 0x0001489D File Offset: 0x00012A9D
	public DragonLevelDragon.State state { get; set; }

	// Token: 0x0600180B RID: 6155 RVA: 0x000148A6 File Offset: 0x00012AA6
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600180C RID: 6156 RVA: 0x000148DC File Offset: 0x00012ADC
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.dead)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x0600180D RID: 6157 RVA: 0x000148FB File Offset: 0x00012AFB
	public void Start()
	{
		Level.Current.OnIntroEvent += this.OnIntro;
	}

	// Token: 0x0600180E RID: 6158 RVA: 0x00014913 File Offset: 0x00012B13
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600180F RID: 6159 RVA: 0x0001492B File Offset: 0x00012B2B
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001810 RID: 6160 RVA: 0x00014954 File Offset: 0x00012B54
	public void OnBossDeath()
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		AudioManager.Stop("level_dragon_sucking_air");
		this.StopAllCoroutines();
		base.animator.Play("Death");
	}

	// Token: 0x06001811 RID: 6161 RVA: 0x00014989 File Offset: 0x00012B89
	public void StartWingSFX()
	{
		AudioManager.PlayLoop("level_dragon_left_dragon_peashot_idle_loop");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_peashot_idle_loop");
	}

	// Token: 0x06001812 RID: 6162 RVA: 0x000149A5 File Offset: 0x00012BA5
	public void StopWingSFX()
	{
		AudioManager.Stop("level_dragon_left_dragon_peashot_idle_loop");
	}

	// Token: 0x06001813 RID: 6163 RVA: 0x000149B1 File Offset: 0x00012BB1
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.meteorPrefab = null;
		this.smokePrefab = null;
		this.peashotPrefab = null;
	}

	// Token: 0x06001814 RID: 6164 RVA: 0x000149CE File Offset: 0x00012BCE
	public void OnIntro()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001815 RID: 6165 RVA: 0x000A2DD0 File Offset: 0x000A0FD0
	public IEnumerator intro_cr()
	{
		this.state = DragonLevelDragon.State.Init;
		base.animator.Play("Intro");
		AudioManager.Play("level_dragon_left_dragon_intro");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_intro");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		yield return CupheadTime.WaitForSeconds(this, 0.6f);
		this.state = DragonLevelDragon.State.Idle;
		yield break;
	}

	// Token: 0x06001816 RID: 6166 RVA: 0x000149DD File Offset: 0x00012BDD
	public void StartPeashot()
	{
		this.state = DragonLevelDragon.State.Peashot;
		base.StartCoroutine(this.peashot_cr());
	}

	// Token: 0x06001817 RID: 6167 RVA: 0x000149F3 File Offset: 0x00012BF3
	public void PeashotInSFX()
	{
		AudioManager.Play("level_dragon_left_dragon_peashot_in");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_peashot_in");
	}

	// Token: 0x06001818 RID: 6168 RVA: 0x000A2DEC File Offset: 0x000A0FEC
	public IEnumerator peashot_cr()
	{
		LevelProperties.Dragon.Peashot p = base.properties.CurrentState.peashot;
		string[] pattern = p.patternString.GetRandom<string>().Split(new char[]
		{
			','
		});
		base.animator.SetBool("Peashot", true);
		yield return base.animator.WaitForAnimationToEnd(this, "Peashot_In", false, true);
		base.animator.Play("Peashot_Zinger");
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i].ToLower() == "p")
			{
				this.peashotRoot.LookAt2D(PlayerManager.GetNext().center);
				for (int c = 0; c < p.colorString.Length; c++)
				{
					int color = 0;
					char c2 = p.colorString[c];
					if (c2 != 'O')
					{
						if (c2 != 'P')
						{
							if (c2 == 'B')
							{
								color = 1;
							}
						}
						else
						{
							color = 2;
						}
					}
					else
					{
						color = 0;
					}
					AudioManager.Play("level_dragon_left_dragon_peashot_fire");
					this.emitAudioFromObject.Add("level_dragon_left_dragon_peashot_fire");
					(this.peashotPrefab.Create(this.peashotRoot.position, this.peashotRoot.eulerAngles.z, p.speed) as DragonLevelPeashot).color = color;
					yield return CupheadTime.WaitForSeconds(this, p.shotDelay);
				}
			}
			else
			{
				float delay = 0f;
				Parser.FloatTryParse(pattern[i], out delay);
				yield return CupheadTime.WaitForSeconds(this, delay);
			}
		}
		base.animator.SetBool("Peashot", false);
		yield return base.animator.WaitForAnimationToStart(this, "Peashot_Out", false);
		AudioManager.Play("level_dragon_left_dragon_peashot_out");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_peashot_out");
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = DragonLevelDragon.State.Idle;
		yield break;
	}

	// Token: 0x06001819 RID: 6169 RVA: 0x000A2E08 File Offset: 0x000A1008
	public void SpawnSmokeFX()
	{
		Effect effect = Object.Instantiate<Effect>(this.smokePrefab);
		effect.transform.position = this.smokeRoot.transform.position;
		effect.GetComponent<Animator>().Play((!Rand.Bool()) ? "Smoke_FX_B" : "Smoke_FX_A");
	}

	// Token: 0x0600181A RID: 6170 RVA: 0x00014A0F File Offset: 0x00012C0F
	public void StartMeteor()
	{
		this.state = DragonLevelDragon.State.Meteor;
		base.StartCoroutine(this.meteor_cr());
	}

	// Token: 0x0600181B RID: 6171 RVA: 0x000A2E60 File Offset: 0x000A1060
	public void FireMeteor()
	{
		AudioManager.Play("level_dragon_left_dragon_meteor_spit");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_meteor_spit");
		if (this.meteorState == DragonLevelMeteor.State.Both)
		{
			this.meteorPrefab.Create(this.mouthRoot.position, new DragonLevelMeteor.Properties(this.currentMeteorProperties.timeY, this.currentMeteorProperties.speedX, DragonLevelMeteor.State.Up));
			this.meteorPrefab.Create(this.mouthRoot.position, new DragonLevelMeteor.Properties(this.currentMeteorProperties.timeY, this.currentMeteorProperties.speedX, DragonLevelMeteor.State.Down));
		}
		else
		{
			this.meteorPrefab.Create(this.mouthRoot.position, new DragonLevelMeteor.Properties(this.currentMeteorProperties.timeY, this.currentMeteorProperties.speedX, this.meteorState));
		}
	}

	// Token: 0x0600181C RID: 6172 RVA: 0x000A2F44 File Offset: 0x000A1144
	public IEnumerator meteor_cr()
	{
		this.currentMeteorProperties = base.properties.CurrentState.meteor;
		char[] meteorPattern = this.currentMeteorProperties.pattern.GetRandom<string>().ToCharArray();
		base.animator.SetTrigger("OnMeteor");
		base.animator.SetBool("Repeat", true);
		yield return base.animator.WaitForAnimationToStart(this, "MeteorStart", false);
		AudioManager.Play("level_dragon_left_dragon_meteor_start");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_meteor_start");
		for (int i = 0; i < meteorPattern.Length; i++)
		{
			char c = meteorPattern[i];
			switch (c)
			{
			case 'B':
				this.meteorState = DragonLevelMeteor.State.Both;
				break;
			default:
				if (c != 'U')
				{
				}
				this.meteorState = DragonLevelMeteor.State.Up;
				break;
			case 'D':
				this.meteorState = DragonLevelMeteor.State.Down;
				break;
			case 'F':
				this.meteorState = DragonLevelMeteor.State.Forward;
				break;
			}
			if (i >= meteorPattern.Length - 1)
			{
				base.animator.SetBool("Repeat", false);
			}
			yield return base.animator.WaitForAnimationToStart(this, "Meteor_Anticipation_Loop", false);
			AudioManager.Play("level_dragon_left_dragon_meteor_anticipation_loop");
			this.emitAudioFromObject.Add("level_dragon_left_dragon_meteor_anticipation_loop");
			yield return CupheadTime.WaitForSeconds(this, this.currentMeteorProperties.shotDelay);
			base.animator.SetTrigger("OnMeteor");
			AudioManager.Stop("level_dragon_left_dragon_meteor_anticipation_loop");
			yield return base.animator.WaitForAnimationToStart(this, "Meteor_Attack", false);
			AudioManager.Play("level_dragon_left_dragon_meteor_attack");
			yield return base.animator.WaitForAnimationToEnd(this, "Meteor_Attack", false, true);
		}
		yield return base.animator.WaitForAnimationToEnd(this, "Meteor_Attack_End", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.currentMeteorProperties.hesitate);
		this.state = DragonLevelDragon.State.Idle;
		yield break;
	}

	// Token: 0x0600181D RID: 6173 RVA: 0x00014A25 File Offset: 0x00012C25
	public void Leave()
	{
		base.StartCoroutine(this.leave_cr());
	}

	// Token: 0x0600181E RID: 6174 RVA: 0x000A2F60 File Offset: 0x000A1160
	public IEnumerator leave_cr()
	{
		while (this.state != DragonLevelDragon.State.Idle)
		{
			yield return null;
		}
		Vector2 endPos = base.transform.position;
		endPos.x += 500f;
		yield return base.StartCoroutine(this.tween_cr(base.transform, base.transform.position, endPos, EaseUtils.EaseType.easeInSine, 1.5f));
		this.damages.SetActive(false);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		Vector2 dashEndPos = this.dash.position;
		dashEndPos.x = -1300f;
		this.StopWingSFX();
		AudioManager.Play("level_dragon_dash");
		yield return base.StartCoroutine(this.tween_cr(this.dash, this.dash.position, dashEndPos, EaseUtils.EaseType.linear, 1.3f));
		yield return CupheadTime.WaitForSeconds(this, 0.75f);
		this.leftSideDragon.StartIntro();
		Object.Destroy(this.dash.gameObject);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0600181F RID: 6175 RVA: 0x000A2F7C File Offset: 0x000A117C
	public IEnumerator tween_cr(Transform trans, Vector2 start, Vector2 end, EaseUtils.EaseType ease, float time)
	{
		float t = 0f;
		trans.position = start;
		while (t < time)
		{
			float val = EaseUtils.Ease(ease, 0f, 1f, t / time);
			trans.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		trans.position = end;
		yield return null;
		yield break;
	}

	// Token: 0x0400137A RID: 4986
	[Space(10f)]
	[SerializeField]
	public Transform mouthRoot;

	// Token: 0x0400137B RID: 4987
	[SerializeField]
	public DragonLevelMeteor meteorPrefab;

	// Token: 0x0400137C RID: 4988
	[SerializeField]
	public Effect smokePrefab;

	// Token: 0x0400137D RID: 4989
	[SerializeField]
	public Transform smokeRoot;

	// Token: 0x0400137E RID: 4990
	[Space(10f)]
	[SerializeField]
	public DragonLevelPeashot peashotPrefab;

	// Token: 0x0400137F RID: 4991
	[SerializeField]
	public Transform peashotRoot;

	// Token: 0x04001380 RID: 4992
	[Space(10f)]
	[SerializeField]
	public Transform chargeRoot;

	// Token: 0x04001381 RID: 4993
	[SerializeField]
	public Transform dash;

	// Token: 0x04001382 RID: 4994
	[SerializeField]
	public DragonLevelLeftSideDragon leftSideDragon;

	// Token: 0x04001383 RID: 4995
	[SerializeField]
	public GameObject damages;

	// Token: 0x04001384 RID: 4996
	public LevelProperties.Dragon.Meteor currentMeteorProperties;

	// Token: 0x04001385 RID: 4997
	public DamageDealer damageDealer;

	// Token: 0x04001386 RID: 4998
	public DamageReceiver damageReceiver;

	// Token: 0x04001387 RID: 4999
	public bool dead;

	// Token: 0x04001388 RID: 5000
	public DragonLevelMeteor.State meteorState;

	// Token: 0x02000BEB RID: 3051
	public enum State
	{
		// Token: 0x040056C8 RID: 22216
		Init,
		// Token: 0x040056C9 RID: 22217
		Idle,
		// Token: 0x040056CA RID: 22218
		Meteor,
		// Token: 0x040056CB RID: 22219
		Peashot
	}
}
