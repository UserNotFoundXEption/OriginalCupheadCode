using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200023C RID: 572
public class FlyingBlimpLevelBlimpLady : LevelProperties.FlyingBlimp.Entity
{
	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06001A27 RID: 6695 RVA: 0x000163D8 File Offset: 0x000145D8
	// (set) Token: 0x06001A28 RID: 6696 RVA: 0x000163E0 File Offset: 0x000145E0
	public FlyingBlimpLevelBlimpLady.State state { get; set; }

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06001A29 RID: 6697 RVA: 0x000163E9 File Offset: 0x000145E9
	// (set) Token: 0x06001A2A RID: 6698 RVA: 0x000163F1 File Offset: 0x000145F1
	public bool moving { get; set; }

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06001A2B RID: 6699 RVA: 0x000163FA File Offset: 0x000145FA
	// (set) Token: 0x06001A2C RID: 6700 RVA: 0x00016402 File Offset: 0x00014602
	public bool fading { get; set; }

	// Token: 0x14000044 RID: 68
	// (add) Token: 0x06001A2D RID: 6701 RVA: 0x000A7E04 File Offset: 0x000A6004
	// (remove) Token: 0x06001A2E RID: 6702 RVA: 0x000A7E3C File Offset: 0x000A603C
	public event Action OnDeathEvent;

	// Token: 0x06001A2F RID: 6703 RVA: 0x000A7E74 File Offset: 0x000A6074
	public override void Awake()
	{
		base.Awake();
		this.state = FlyingBlimpLevelBlimpLady.State.Intro;
		this.pivotOffset = Vector3.up * 2f * this.loopSize;
		this.pivotPoint.position = base.transform.position;
		this.startPos = base.transform.position;
		this.ResetPivotPos(base.transform.position);
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.constellationHandler.color = new Color(1f, 1f, 1f, 0f);
	}

	// Token: 0x06001A30 RID: 6704 RVA: 0x0001640B File Offset: 0x0001460B
	public override void LevelInit(LevelProperties.FlyingBlimp properties)
	{
		base.LevelInit(properties);
		this.originalSpeed = properties.CurrentState.move.pathSpeed;
		this.moving = true;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001A31 RID: 6705 RVA: 0x0001643E File Offset: 0x0001463E
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
	}

	// Token: 0x06001A32 RID: 6706 RVA: 0x000A7F38 File Offset: 0x000A6138
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && this.state != FlyingBlimpLevelBlimpLady.State.Death)
		{
			this.state = FlyingBlimpLevelBlimpLady.State.Death;
			this.StartDeath();
		}
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x00016446 File Offset: 0x00014646
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001A34 RID: 6708 RVA: 0x0001645E File Offset: 0x0001465E
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001A35 RID: 6709 RVA: 0x000A7F84 File Offset: 0x000A6184
	public void ResetPivotPos(Vector3 newPos)
	{
		this.pivotPoint.position = newPos;
		Vector3 position = base.transform.position;
		position.y = newPos.y + this.loopSize;
		base.transform.position = position;
	}

	// Token: 0x06001A36 RID: 6710 RVA: 0x000A7FCC File Offset: 0x000A61CC
	public IEnumerator intro_cr()
	{
		AudioManager.Play("level_flying_blimp_intro");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro_End", false, true);
		AudioManager.PlayLoop("level_flying_blimp_pedal_loop");
		base.StartCoroutine(this.move_cr());
		while (this.movementSpeed < this.originalSpeed)
		{
			this.movementSpeed += 0.2f;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.move.initalAttackDelayRange.RandomFloat());
		this.state = FlyingBlimpLevelBlimpLady.State.Idle;
		yield break;
	}

	// Token: 0x06001A37 RID: 6711 RVA: 0x000A7FE8 File Offset: 0x000A61E8
	public IEnumerator move_cr()
	{
		this.angle = ((!Rand.Bool()) ? 0f : 6.28318548f);
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (this.moving)
			{
				this.PathMovement();
				yield return wait;
			}
			else
			{
				yield return wait;
			}
		}
		yield break;
	}

	// Token: 0x06001A38 RID: 6712 RVA: 0x000A8004 File Offset: 0x000A6204
	public void PathMovement()
	{
		this.angle += this.movementSpeed * CupheadTime.FixedDelta;
		if (this.angle > 6.28318548f)
		{
			this.invert = !this.invert;
			this.angle -= 6.28318548f;
		}
		if (this.angle < 0f)
		{
			this.angle += 6.28318548f;
		}
		float num;
		if (this.invert)
		{
			base.transform.position = this.pivotPoint.position + this.pivotOffset;
			num = -1f;
		}
		else
		{
			base.transform.position = this.pivotPoint.position;
			num = 1f;
		}
		Vector3 vector;
		vector..ctor(-Mathf.Sin(this.angle) * this.loopSize, Mathf.Cos(this.angle) * num * this.loopSize, 0f);
		base.transform.position += vector;
	}

	// Token: 0x06001A39 RID: 6713 RVA: 0x0001647C File Offset: 0x0001467C
	public void ChangeMat(Material mat)
	{
		base.GetComponent<SpriteRenderer>().material = mat;
	}

	// Token: 0x06001A3A RID: 6714 RVA: 0x0001648A File Offset: 0x0001468A
	public void StartDash()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.dash_cr());
	}

	// Token: 0x06001A3B RID: 6715 RVA: 0x000A811C File Offset: 0x000A631C
	public IEnumerator dash_cr()
	{
		bool startedClouds = false;
		this.smallClouds = true;
		this.transitionToSummon = true;
		Animator constAnimator = this.constellationHandler.GetComponent<Animator>();
		this.state = FlyingBlimpLevelBlimpLady.State.Dash;
		LevelProperties.FlyingBlimp.DashSummon p = base.properties.CurrentState.dashSummon;
		string[] pattern = p.patternString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.moving = false;
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i][0] == 'D')
			{
				float waitTime = 0f;
				Parser.FloatTryParse(pattern[i].Substring(1), out waitTime);
				yield return CupheadTime.WaitForSeconds(this, waitTime);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, 0.1f);
				AudioManager.Stop("level_flying_blimp_pedal_loop");
				AudioManager.Play("level_flying_blimp_inhale");
				base.animator.Play("Dash_Start");
				yield return base.animator.WaitForAnimationToEnd(this, "Dash_Start", false, true);
				yield return CupheadTime.WaitForSeconds(this, p.hold);
				AudioManager.Play("level_flying_blimp_exhale");
				base.animator.SetBool("Deflate", true);
				yield return CupheadTime.WaitForSeconds(this, 0.2f);
				this.fading = true;
				base.StartCoroutine(this.fade_constellation_cr(true));
				AudioManager.Play("level_flying_blimp_lady_constellation_loop");
				switch (this.constellation)
				{
				case FlyingBlimpLevelBlimpLady.constellationPossibility.Taurus:
					constAnimator.Play("Taurus");
					break;
				case FlyingBlimpLevelBlimpLady.constellationPossibility.Sagittarius:
					constAnimator.Play("Sagittarius");
					break;
				case FlyingBlimpLevelBlimpLady.constellationPossibility.Gemini:
					constAnimator.Play("Gemini");
					break;
				}
				base.animator.SetTrigger("Move");
				while (base.transform.position.x >= -1280f)
				{
					if (this.state != FlyingBlimpLevelBlimpLady.State.Death)
					{
						base.transform.position += base.transform.right * -p.dashSpeed * CupheadTime.Delta;
					}
					yield return null;
				}
				this.dashExplosions = false;
				base.animator.SetTrigger("ToOff");
				yield return CupheadTime.WaitForSeconds(this, p.reeentryDelay);
				base.animator.SetTrigger("StartSummon");
				AudioManager.PlayLoop("level_flying_blimp_pedal_loop");
				AudioManager.Play("level_flying_blimp_lady_constellation_transform");
				Vector3 endPos = this.startPos;
				if (this.constellation == FlyingBlimpLevelBlimpLady.constellationPossibility.Gemini)
				{
					endPos.x = this.startPos.x + 100f;
					this.ResetPivotPos(endPos);
				}
				Vector3 pos = base.transform.position;
				pos.y = this.startPos.y;
				base.transform.position = pos;
				while (base.transform.position.x <= endPos.x)
				{
					if (base.transform.position.x >= this.transformationPoint.position.x && !startedClouds)
					{
						this.fading = false;
						base.StartCoroutine(this.fade_constellation_cr(false));
						base.StartCoroutine(this.spawn_clouds_cr());
						startedClouds = true;
					}
					if (this.state != FlyingBlimpLevelBlimpLady.State.Death)
					{
						base.transform.position += base.transform.right * p.summonSpeed * CupheadTime.FixedDelta;
					}
					yield return new WaitForFixedUpdate();
				}
				if (base.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.Generic)
				{
					this.transitionToSummon = false;
				}
				else
				{
					this.transitionToSummon = true;
				}
				AudioManager.Stop("level_flying_blimp_pedal_loop");
				base.animator.Play("Big_Cloud");
				this.smallClouds = false;
				base.animator.SetBool("Deflate", false);
			}
		}
		yield break;
	}

	// Token: 0x06001A3C RID: 6716 RVA: 0x000A8138 File Offset: 0x000A6338
	public IEnumerator select_constellation_cr()
	{
		if (this.transitionToSummon)
		{
			switch (this.constellation)
			{
			case FlyingBlimpLevelBlimpLady.constellationPossibility.Taurus:
				this.ToTaurus();
				base.animator.Play("Taurus_Idle");
				this.ChangeMat(this.taurusMat);
				break;
			case FlyingBlimpLevelBlimpLady.constellationPossibility.Sagittarius:
				this.ToSagittarius();
				base.animator.Play("Sag_Cloud");
				base.animator.Play("Sagittarius_Idle");
				this.ChangeMat(this.sagittariusMat);
				break;
			case FlyingBlimpLevelBlimpLady.constellationPossibility.Gemini:
				this.ToGemini();
				base.animator.Play("Gemini");
				base.animator.Play("Sphere_Idle");
				this.ChangeMat(this.geminiMat);
				break;
			}
			this.transitionToSummon = false;
		}
		else
		{
			this.ResetPivotPos(this.startPos);
			AudioManager.Play("level_flying_blimp_lady_constellation_transform_end");
			base.animator.Play("Appear");
			this.ChangeMat(this.blimpMat);
			if (this.constellation == FlyingBlimpLevelBlimpLady.constellationPossibility.Sagittarius)
			{
				base.animator.Play("Sag_Off");
			}
			yield return base.animator.WaitForAnimationToEnd(this, "Appear", false, true);
			this.moving = true;
			AudioManager.PlayLoop("level_flying_blimp_pedal_loop");
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.dashSummon.summonHesitate);
			this.state = FlyingBlimpLevelBlimpLady.State.Idle;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001A3D RID: 6717 RVA: 0x000A8154 File Offset: 0x000A6354
	public IEnumerator check_state_cr(LevelProperties.FlyingBlimp.States currentState)
	{
		this.isLooping = true;
		while (base.properties.CurrentState.stateName == currentState)
		{
			yield return null;
		}
		this.isLooping = false;
		this.waitLoopTime = 0f;
		yield break;
	}

	// Token: 0x06001A3E RID: 6718 RVA: 0x000164B5 File Offset: 0x000146B5
	public void StartSmoke()
	{
		base.animator.Play("Dash_Smoke");
		this.dashExplosions = true;
		base.StartCoroutine(this.spawn_explosions_cr());
	}

	// Token: 0x06001A3F RID: 6719 RVA: 0x000164DB File Offset: 0x000146DB
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.explosionOffset, this.explosionRadius);
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x000A8178 File Offset: 0x000A6378
	public IEnumerator spawn_explosions_cr()
	{
		while (this.dashExplosions)
		{
			Effect explosion = Object.Instantiate<Effect>(this.dashExplosionEffect);
			explosion.transform.position = base.transform.position;
			yield return CupheadTime.WaitForSeconds(this, 0.2f);
		}
		yield break;
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x000A8194 File Offset: 0x000A6394
	public IEnumerator spawn_clouds_cr()
	{
		while (this.smallClouds)
		{
			GameObject cloud = Object.Instantiate<GameObject>(this.cloudEffect);
			Vector3 scale = new Vector3(1f, 1f, 1f);
			scale.x = ((!Rand.Bool()) ? (-scale.x) : scale.x);
			scale.y = ((!Rand.Bool()) ? (-scale.y) : scale.y);
			cloud.transform.SetScale(new float?(scale.x), new float?(scale.y), new float?(1f));
			cloud.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
			cloud.GetComponent<SpriteRenderer>().sortingOrder = Random.Range(0, 3);
			cloud.transform.position = this.GetRandomPoint();
			base.StartCoroutine(this.delete_cloud_cr(cloud));
			yield return CupheadTime.WaitForSeconds(this, 0.05f);
		}
		yield break;
	}

	// Token: 0x06001A42 RID: 6722 RVA: 0x000A81B0 File Offset: 0x000A63B0
	public IEnumerator delete_cloud_cr(GameObject cloud)
	{
		yield return cloud.GetComponent<Animator>().WaitForAnimationToEnd(this, "Cloud", false, true);
		Object.Destroy(cloud);
		yield break;
	}

	// Token: 0x06001A43 RID: 6723 RVA: 0x000A81D4 File Offset: 0x000A63D4
	public Vector2 GetRandomPoint()
	{
		Vector2 vector = base.transform.position + this.explosionOffset;
		Vector2 vector2;
		vector2..ctor((float)Random.Range(-1, 1), (float)Random.Range(-1, 1));
		Vector2 vector3 = vector2.normalized * (this.explosionRadius * Random.value) * 2f;
		return vector + vector3;
	}

	// Token: 0x06001A44 RID: 6724 RVA: 0x000A8240 File Offset: 0x000A6440
	public IEnumerator fade_constellation_cr(bool fadeIn)
	{
		float fadeTime = 0.5f;
		float blackMaxFade = 0.25f;
		float blackMidFade = 0.13f;
		float blackCurrentFade = 0f;
		if (fadeIn)
		{
			float t = 0f;
			while (t < fadeTime)
			{
				this.constellationHandler.color = new Color(1f, 1f, 1f, t / fadeTime);
				if (blackCurrentFade < blackMaxFade)
				{
					this.blackDim.color = new Color(0f, 0f, 0f, blackCurrentFade + t / 3f);
				}
				t += CupheadTime.Delta;
				yield return null;
			}
			this.constellationHandler.color = new Color(1f, 1f, 1f, 1f);
			this.blackDim.color = new Color(0f, 0f, 0f, blackMaxFade);
		}
		else
		{
			float t2 = 0f;
			blackCurrentFade = blackMaxFade;
			while (t2 < fadeTime)
			{
				this.constellationHandler.color = new Color(1f, 1f, 1f, 1f - t2 / fadeTime);
				if (blackCurrentFade > blackMidFade)
				{
					this.blackDim.color = new Color(0f, 0f, 0f, blackCurrentFade - t2 / 3f);
				}
				t2 += CupheadTime.Delta;
				yield return null;
			}
			this.constellationHandler.color = new Color(1f, 1f, 1f, 0f);
			this.blackDim.color = new Color(0f, 0f, 0f, blackMidFade);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001A45 RID: 6725 RVA: 0x000A8264 File Offset: 0x000A6464
	public IEnumerator final_fade_cr()
	{
		float fadeTime = 0.5f;
		float blackMidFade = 0.13f;
		float t = 0f;
		while (t < fadeTime)
		{
			this.blackDim.color = new Color(0f, 0f, 0f, blackMidFade - t / 3f);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.constellationHandler.color = new Color(1f, 1f, 1f, 0f);
		yield return null;
		yield break;
	}

	// Token: 0x06001A46 RID: 6726 RVA: 0x00016518 File Offset: 0x00014718
	public void StartTaurus()
	{
		this.constellation = FlyingBlimpLevelBlimpLady.constellationPossibility.Taurus;
		this.StartDash();
		base.StartCoroutine(this.check_state_cr(base.properties.CurrentState.stateName));
	}

	// Token: 0x06001A47 RID: 6727 RVA: 0x00016544 File Offset: 0x00014744
	public void ToTaurus()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.taurus_cr());
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x000A8280 File Offset: 0x000A6480
	public IEnumerator taurus_cr()
	{
		LevelProperties.FlyingBlimp.Taurus p = base.properties.CurrentState.taurus;
		this.waitLoopTime = p.attackDelayRange.RandomFloat();
		this.moving = true;
		this.state = FlyingBlimpLevelBlimpLady.State.Taurus;
		this.movementSpeed = p.movementSpeed;
		do
		{
			float t = 0f;
			while (t < this.waitLoopTime)
			{
				t += CupheadTime.Delta;
				if (!this.isLooping)
				{
					break;
				}
				yield return null;
			}
			t = 0f;
			this.moving = false;
			base.animator.SetTrigger("TaurusATK");
			yield return base.animator.WaitForAnimationToStart(this, "Taurus_Attack", false);
			AudioManager.Play("level_flying_blimp_taurus_attack");
			AudioManager.Stop("level_flying_blimp_taurus_idle");
			this.emitAudioFromObject.Add("level_flying_blimp_taurus_attack");
			yield return base.animator.WaitForAnimationToEnd(this, "Taurus_Attack", false, true);
			this.moving = true;
			yield return null;
		}
		while (this.isLooping);
		base.animator.Play("Big_Cloud");
		this.movementSpeed = this.originalSpeed;
		base.StartCoroutine(this.final_fade_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x0001656F File Offset: 0x0001476F
	public void StartSagittarius()
	{
		this.constellation = FlyingBlimpLevelBlimpLady.constellationPossibility.Sagittarius;
		this.StartDash();
		base.StartCoroutine(this.check_state_cr(base.properties.CurrentState.stateName));
	}

	// Token: 0x06001A4A RID: 6730 RVA: 0x0001659B File Offset: 0x0001479B
	public void ToSagittarius()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.sagittarius_cr());
	}

	// Token: 0x06001A4B RID: 6731 RVA: 0x000A829C File Offset: 0x000A649C
	public IEnumerator sagittarius_cr()
	{
		LevelProperties.FlyingBlimp.Sagittarius p = base.properties.CurrentState.sagittarius;
		this.waitLoopTime = p.attackDelayRange.RandomFloat();
		this.moving = true;
		this.state = FlyingBlimpLevelBlimpLady.State.Sagittarius;
		this.movementSpeed = (float)p.movementSpeed;
		do
		{
			base.animator.SetTrigger("SagittariusATK");
			yield return base.animator.WaitForAnimationToStart(this, "Sagittarius_Attack_Loop", false);
			AudioManager.Play("level_flying_blimp_sagittarius_anticipation");
			yield return CupheadTime.WaitForSeconds(this, p.arrowWarning);
			base.animator.SetTrigger("Continue");
			AudioManager.Stop("level_flying_blimp_sagittarius_anticipation");
			float t = 0f;
			while (t < this.waitLoopTime)
			{
				t += CupheadTime.Delta;
				if (!this.isLooping)
				{
					break;
				}
				yield return null;
			}
			t = 0f;
			yield return null;
		}
		while (this.isLooping);
		base.animator.Play("Big_Cloud");
		this.movementSpeed = this.originalSpeed;
		base.StartCoroutine(this.final_fade_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001A4C RID: 6732 RVA: 0x000A82B8 File Offset: 0x000A64B8
	public void FireArrowsStars()
	{
		LevelProperties.FlyingBlimp.Sagittarius sagittarius = base.properties.CurrentState.sagittarius;
		int num = 3;
		for (int i = 0; i < num; i++)
		{
			AbstractPlayerController next = PlayerManager.GetNext();
			float num2 = sagittarius.homingSpreadAngle.GetFloatAt((float)i / ((float)num - 1f));
			float num3 = sagittarius.homingSpreadAngle.max / 2f;
			num2 -= num3;
			float num4 = Mathf.Atan2(0f, -360f) * 57.29578f;
			this.sagittariusStarPrefab.Create(this.arrowEffectRoot.transform.position, num4 + num2, sagittarius.arrowInitialSpeed, sagittarius.homingSpeed, sagittarius.homingRotation, sagittarius.homingDurationRange.RandomFloat(), sagittarius.homingDelay, next, (float)sagittarius.arrowHP);
		}
		this.arrowEffect.Create(this.arrowEffectRoot.transform.position);
		this.sagittariusArrowPrefab.Create(this.arrowRoot.position, 180f, sagittarius.arrowInitialSpeed);
	}

	// Token: 0x06001A4D RID: 6733 RVA: 0x000165C6 File Offset: 0x000147C6
	public void StartGemini()
	{
		this.constellation = FlyingBlimpLevelBlimpLady.constellationPossibility.Gemini;
		this.StartDash();
		base.StartCoroutine(this.check_state_cr(base.properties.CurrentState.stateName));
	}

	// Token: 0x06001A4E RID: 6734 RVA: 0x000165F2 File Offset: 0x000147F2
	public void ToGemini()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.gemini_cr());
	}

	// Token: 0x06001A4F RID: 6735 RVA: 0x000A83DC File Offset: 0x000A65DC
	public IEnumerator gemini_cr()
	{
		LevelProperties.FlyingBlimp.Gemini p = base.properties.CurrentState.gemini;
		this.waitLoopTime = base.properties.CurrentState.gemini.spawnerDelay.RandomFloat();
		this.pivotPoint.position = base.transform.position;
		this.moving = true;
		bool repeat = false;
		this.state = FlyingBlimpLevelBlimpLady.State.Gemini;
		do
		{
			if (this.geminiObject == null)
			{
				if (repeat)
				{
					AudioManager.Play("level_flying_blimp_gemini_sphere_reappear");
					base.animator.Play("Sphere_Reappear");
				}
				float t = 0f;
				while (t < this.waitLoopTime)
				{
					t += CupheadTime.Delta;
					if (!this.isLooping)
					{
						break;
					}
					yield return null;
				}
				t = 0f;
				yield return base.animator.WaitForAnimationToEnd(this, "Gemini", false, true);
				base.animator.SetTrigger("GeminiATK");
				AudioManager.Play("level_flying_blimp_gemini_attack");
				base.animator.Play("Gemini_Attack");
				yield return CupheadTime.WaitForSeconds(this, p.spawnerSpeed);
				this.SpawnGemini();
				repeat = true;
				yield return null;
			}
			yield return null;
		}
		while (this.isLooping);
		base.animator.Play("Big_Cloud");
		this.movementSpeed = this.originalSpeed;
		base.StartCoroutine(this.final_fade_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001A50 RID: 6736 RVA: 0x000A83F8 File Offset: 0x000A65F8
	public void SpawnGemini()
	{
		this.geminiTarget = this.objectSpawnRoot.transform.position;
		Vector2 vector = this.geminiTarget;
		Vector2 vector2;
		vector2..ctor(Random.value * (float)((!Rand.Bool()) ? -1 : 1), Random.value * (float)((!Rand.Bool()) ? -1 : 1));
		this.geminiTarget = vector + vector2.normalized * this.objectSpawnRoot.radius * Random.value;
		this.geminiObject = Object.Instantiate<FlyingBlimpLevelGeminiShoot>(this.geminiObjectPrefab);
		this.geminiObject.Init(base.properties.CurrentState.gemini, this.geminiTarget);
	}

	// Token: 0x06001A51 RID: 6737 RVA: 0x0001661D File Offset: 0x0001481D
	public void SwitchCloneBottomLayer()
	{
		this.geminiClone.sortingOrder = 1;
		base.GetComponent<SpriteRenderer>().sortingOrder = 3;
	}

	// Token: 0x06001A52 RID: 6738 RVA: 0x00016637 File Offset: 0x00014837
	public void SwitchCloneTopLayer()
	{
		this.geminiClone.sortingOrder = 3;
		base.GetComponent<SpriteRenderer>().sortingOrder = 1;
	}

	// Token: 0x06001A53 RID: 6739 RVA: 0x00016651 File Offset: 0x00014851
	public void SwitchSphereLayer(int layer)
	{
		this.sphere.sortingOrder = layer;
	}

	// Token: 0x06001A54 RID: 6740 RVA: 0x000A84BC File Offset: 0x000A66BC
	public void SummonTornado()
	{
		LevelProperties.FlyingBlimp.Tornado properties = base.properties.CurrentState.tornado;
		this.tornado = Object.Instantiate<FlyingBlimpLevelTornado>(this.tornadoPrefab);
		this.tornado.Init(this.projectileRoot.transform.position, PlayerManager.GetNext(), properties);
	}

	// Token: 0x06001A55 RID: 6741 RVA: 0x0001665F File Offset: 0x0001485F
	public void MoveTornado()
	{
		if (this.tornadoPrefab != null)
		{
			base.StartCoroutine(this.tornado.move_cr());
		}
	}

	// Token: 0x06001A56 RID: 6742 RVA: 0x00016684 File Offset: 0x00014884
	public void StartTornado()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.tornado_cr());
	}

	// Token: 0x06001A57 RID: 6743 RVA: 0x000A8514 File Offset: 0x000A6714
	public IEnumerator tornado_cr()
	{
		this.state = FlyingBlimpLevelBlimpLady.State.Tornado;
		LevelProperties.FlyingBlimp.Tornado p = base.properties.CurrentState.tornado;
		this.moving = false;
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		AudioManager.Stop("level_flying_blimp_pedal_loop");
		AudioManager.Play("level_flying_blimp_tornado");
		this.SummonTornado();
		base.animator.Play("Tornado_Start");
		yield return base.animator.WaitForAnimationToEnd(this, "Tornado_Start", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.loopDuration);
		base.animator.SetTrigger("FinishTornado");
		yield return base.animator.WaitForAnimationToEnd(this, "Tornado_Finish", false, true);
		AudioManager.PlayLoop("level_flying_blimp_pedal_loop");
		this.moving = true;
		yield return CupheadTime.WaitForSeconds(this, p.hesitateAfterAttack);
		this.state = FlyingBlimpLevelBlimpLady.State.Idle;
		yield break;
	}

	// Token: 0x06001A58 RID: 6744 RVA: 0x000166AF File Offset: 0x000148AF
	public void StartShoot()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x06001A59 RID: 6745 RVA: 0x000A8530 File Offset: 0x000A6730
	public IEnumerator shoot_cr()
	{
		this.state = FlyingBlimpLevelBlimpLady.State.Shoot;
		LevelProperties.FlyingBlimp.Shoot p = base.properties.CurrentState.shoot;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AudioManager.Play("level_flying_blimp_fire");
		base.animator.Play("Shoot_Start");
		yield return base.animator.WaitForAnimationToEnd(this, "Shoot_Start", false, true);
		this.spawnProjectile();
		yield return CupheadTime.WaitForSeconds(this, 0.7f);
		yield return CupheadTime.WaitForSeconds(this, p.hesitateAfterAttackRange.RandomFloat());
		this.state = FlyingBlimpLevelBlimpLady.State.Idle;
		yield break;
	}

	// Token: 0x06001A5A RID: 6746 RVA: 0x000166DA File Offset: 0x000148DA
	public void spawnProjectile()
	{
		this.shootProjectilePrefab.Create(this.projectileRoot.position, 0f, base.properties.CurrentState.shoot);
	}

	// Token: 0x06001A5B RID: 6747 RVA: 0x000A854C File Offset: 0x000A674C
	public void SummonEnemy(FlyingBlimpLevelEnemy prefab, Vector3 startPoint, float stopPoint, bool type)
	{
		FlyingBlimpLevelEnemy flyingBlimpLevelEnemy = Object.Instantiate<FlyingBlimpLevelEnemy>(prefab);
		Vector2 vector = flyingBlimpLevelEnemy.transform.position;
		vector.y = 360f - startPoint.y;
		vector.x = 740f;
		stopPoint = base.properties.CurrentState.enemy.stopDistance.RandomFloat();
		flyingBlimpLevelEnemy.transform.position = vector;
		flyingBlimpLevelEnemy.Init(base.properties, startPoint, stopPoint, type, this);
	}

	// Token: 0x06001A5C RID: 6748 RVA: 0x000A85D0 File Offset: 0x000A67D0
	public IEnumerator spawnEnemy_cr()
	{
		LevelProperties.FlyingBlimp.Enemy p = base.properties.CurrentState.enemy;
		string[] spawnPattern = p.spawnString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] typePattern = p.typeString.GetRandom<string>().Split(new char[]
		{
			','
		});
		bool AParryable = false;
		float waitTime = 0f;
		int counter = 0;
		int typeIndex = 0;
		int spawnIndex = Random.Range(0, spawnPattern.Length);
		Vector3 spawnPos = Vector3.zero;
		for (;;)
		{
			for (int i = spawnIndex; i < spawnPattern.Length; i++)
			{
				if (waitTime > 0f)
				{
					yield return CupheadTime.WaitForSeconds(this, waitTime);
				}
				if (spawnPattern[i][0] == 'D')
				{
					Parser.FloatTryParse(spawnPattern[i].Substring(1), out waitTime);
				}
				else
				{
					string[] array = spawnPattern[i].Split(new char[]
					{
						'-'
					});
					foreach (string s in array)
					{
						float y = 0f;
						float stopPoint = 0f;
						Parser.FloatTryParse(s, out y);
						Parser.FloatTryParse(s, out stopPoint);
						FlyingBlimpLevelEnemy prefab = null;
						if (typePattern[typeIndex][0] == 'A')
						{
							prefab = this.enemyPrefabA;
							if ((float)counter >= p.APinkOccurance.RandomFloat())
							{
								AParryable = true;
								counter = 0;
							}
							else
							{
								AParryable = false;
								counter++;
							}
						}
						else if (typePattern[typeIndex][0] == 'B')
						{
							prefab = this.enemyPrefabB;
							AParryable = false;
						}
						spawnPos.y = y;
						if (this.state != FlyingBlimpLevelBlimpLady.State.Death)
						{
							this.SummonEnemy(prefab, spawnPos, stopPoint, AParryable);
						}
						typeIndex = (typeIndex + 1) % typePattern.Length;
					}
					waitTime = p.stringDelay;
				}
				i %= spawnPattern.Length;
			}
			spawnIndex = 0;
		}
		yield break;
	}

	// Token: 0x06001A5D RID: 6749 RVA: 0x0001670D File Offset: 0x0001490D
	public void StartDeath()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x06001A5E RID: 6750 RVA: 0x000A85EC File Offset: 0x000A67EC
	public IEnumerator die_cr()
	{
		base.animator.SetTrigger("Death");
		this.moving = false;
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		base.GetComponent<Collider2D>().enabled = false;
		yield return null;
		yield break;
	}

	// Token: 0x06001A5F RID: 6751 RVA: 0x00016722 File Offset: 0x00014922
	public void SpawnMoonLady()
	{
		base.StartCoroutine(this.spawn_moon_lady_cr());
	}

	// Token: 0x06001A60 RID: 6752 RVA: 0x000A8608 File Offset: 0x000A6808
	public IEnumerator spawn_moon_lady_cr()
	{
		while (this.angle > 0.2617994f || this.angle < -0.2617994f)
		{
			yield return null;
		}
		this.moving = false;
		this.moonLady.StartIntro();
		Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06001A61 RID: 6753 RVA: 0x00016731 File Offset: 0x00014931
	public void SagAttackSFX()
	{
		AudioManager.Play("level_flying_blimp_sagittarius_attack");
		this.emitAudioFromObject.Add("level_flying_blimp_sagittarius_attack");
	}

	// Token: 0x06001A62 RID: 6754 RVA: 0x0001674D File Offset: 0x0001494D
	public void TaurusIdleSFX()
	{
		AudioManager.Play("level_flying_blimp_taurus_idle");
		this.emitAudioFromObject.Add("level_flying_blimp_taurus_idle");
	}

	// Token: 0x040014F7 RID: 5367
	[Header("Phase Materials")]
	[SerializeField]
	public Material blimpMat;

	// Token: 0x040014F8 RID: 5368
	[SerializeField]
	public Material taurusMat;

	// Token: 0x040014F9 RID: 5369
	[SerializeField]
	public Material sagittariusMat;

	// Token: 0x040014FA RID: 5370
	[SerializeField]
	public Material geminiMat;

	// Token: 0x040014FB RID: 5371
	[Space(10f)]
	[SerializeField]
	public Transform pivotPoint;

	// Token: 0x040014FC RID: 5372
	[SerializeField]
	public Transform transformationPoint;

	// Token: 0x040014FD RID: 5373
	[SerializeField]
	public Effect dashExplosionEffect;

	// Token: 0x040014FE RID: 5374
	[SerializeField]
	public GameObject cloudEffect;

	// Token: 0x040014FF RID: 5375
	[SerializeField]
	public GameObject bigCloud;

	// Token: 0x04001500 RID: 5376
	[SerializeField]
	public SpriteRenderer constellationHandler;

	// Token: 0x04001501 RID: 5377
	[SerializeField]
	public SpriteRenderer blackDim;

	// Token: 0x04001502 RID: 5378
	[SerializeField]
	public FlyingBlimpLevelEnemy enemyPrefabA;

	// Token: 0x04001503 RID: 5379
	[SerializeField]
	public FlyingBlimpLevelEnemy enemyPrefabB;

	// Token: 0x04001504 RID: 5380
	[SerializeField]
	public FlyingBlimpLevelTornado tornadoPrefab;

	// Token: 0x04001505 RID: 5381
	public FlyingBlimpLevelTornado tornado;

	// Token: 0x04001506 RID: 5382
	[SerializeField]
	public FlyingBlimpLevelShootProjectile shootProjectilePrefab;

	// Token: 0x04001507 RID: 5383
	[SerializeField]
	public FlyingBlimpLevelArrowProjectile sagittariusStarPrefab;

	// Token: 0x04001508 RID: 5384
	[SerializeField]
	public BasicProjectile sagittariusArrowPrefab;

	// Token: 0x04001509 RID: 5385
	[SerializeField]
	public FlyingBlimpLevelGeminiShoot geminiObjectPrefab;

	// Token: 0x0400150A RID: 5386
	public FlyingBlimpLevelGeminiShoot geminiObject;

	// Token: 0x0400150B RID: 5387
	[SerializeField]
	public SpriteRenderer geminiClone;

	// Token: 0x0400150C RID: 5388
	[SerializeField]
	public SpriteRenderer sphere;

	// Token: 0x0400150D RID: 5389
	[SerializeField]
	public FlyingBlimpLevelSpawnRadius objectSpawnRoot;

	// Token: 0x0400150E RID: 5390
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x0400150F RID: 5391
	[SerializeField]
	public Transform arrowRoot;

	// Token: 0x04001510 RID: 5392
	[SerializeField]
	public Transform arrowEffectRoot;

	// Token: 0x04001511 RID: 5393
	[SerializeField]
	public FlyingBlimpLevelMoonLady moonLady;

	// Token: 0x04001512 RID: 5394
	[SerializeField]
	public Vector2 explosionOffset = Vector2.zero;

	// Token: 0x04001513 RID: 5395
	[SerializeField]
	public Effect arrowEffect;

	// Token: 0x04001514 RID: 5396
	[SerializeField]
	public float explosionRadius = 100f;

	// Token: 0x04001515 RID: 5397
	public float angle;

	// Token: 0x04001516 RID: 5398
	public float originalSpeed;

	// Token: 0x04001517 RID: 5399
	public float loopSize = 80f;

	// Token: 0x04001518 RID: 5400
	public float movementSpeed;

	// Token: 0x04001519 RID: 5401
	public float waitLoopTime;

	// Token: 0x0400151A RID: 5402
	public Vector3 startPos;

	// Token: 0x0400151B RID: 5403
	public Vector3 pivotOffset;

	// Token: 0x0400151C RID: 5404
	public Vector3 getPos;

	// Token: 0x0400151D RID: 5405
	public Vector2 geminiTarget;

	// Token: 0x0400151E RID: 5406
	public bool invert;

	// Token: 0x0400151F RID: 5407
	public bool isLooping;

	// Token: 0x04001520 RID: 5408
	public bool smallClouds;

	// Token: 0x04001521 RID: 5409
	public bool dashExplosions;

	// Token: 0x04001522 RID: 5410
	public bool transitionToSummon = true;

	// Token: 0x04001523 RID: 5411
	public DamageDealer damageDealer;

	// Token: 0x04001524 RID: 5412
	public DamageReceiver damageReceiver;

	// Token: 0x04001525 RID: 5413
	public Coroutine patternCoroutine;

	// Token: 0x04001526 RID: 5414
	public FlyingBlimpLevelBlimpLady.constellationPossibility constellation;

	// Token: 0x02000C6E RID: 3182
	public enum State
	{
		// Token: 0x040059E0 RID: 23008
		Intro,
		// Token: 0x040059E1 RID: 23009
		Idle,
		// Token: 0x040059E2 RID: 23010
		Dash,
		// Token: 0x040059E3 RID: 23011
		Tornado,
		// Token: 0x040059E4 RID: 23012
		Shoot,
		// Token: 0x040059E5 RID: 23013
		Taurus,
		// Token: 0x040059E6 RID: 23014
		Sagittarius,
		// Token: 0x040059E7 RID: 23015
		Gemini,
		// Token: 0x040059E8 RID: 23016
		Death
	}

	// Token: 0x02000C6F RID: 3183
	public enum constellationPossibility
	{
		// Token: 0x040059EA RID: 23018
		Taurus = 1,
		// Token: 0x040059EB RID: 23019
		Sagittarius,
		// Token: 0x040059EC RID: 23020
		Gemini
	}
}
