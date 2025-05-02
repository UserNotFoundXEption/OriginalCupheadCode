using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200032A RID: 810
public class RobotLevelHelihead : AbstractCollidableObject
{
	// Token: 0x0600233C RID: 9020 RVA: 0x0001DD30 File Offset: 0x0001BF30
	public override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.current = RobotLevelHelihead.state.first;
		base.Awake();
	}

	// Token: 0x0600233D RID: 9021 RVA: 0x0001DD6D File Offset: 0x0001BF6D
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (this.properties != null && this.properties.CurrentHealth <= 0f)
		{
			this.StopAllCoroutines();
		}
	}

	// Token: 0x0600233E RID: 9022 RVA: 0x000BFD78 File Offset: 0x000BDF78
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		Level.Current.timeline.DealDamage(info.damage);
		this.properties.DealDamage(info.damage);
		if (this.properties.CurrentHealth <= 0f && this.current != RobotLevelHelihead.state.dead)
		{
			this.current = RobotLevelHelihead.state.dead;
			this.StartDeath();
		}
	}

	// Token: 0x0600233F RID: 9023 RVA: 0x000BFDDC File Offset: 0x000BDFDC
	public void InitHeliHead(LevelProperties.Robot properties)
	{
		this.introActive = true;
		this.screenHeights = properties.CurrentState.heliHead.onScreenHeight.Split(new char[]
		{
			','
		});
		this.coordinateIndex = Random.Range(0, this.screenHeights.Length);
		this.attackTypeIndex = Random.Range(0, properties.CurrentState.inventor.gemColourString.Split(new char[]
		{
			','
		}).Length);
		this.pivotPoint = new GameObject("pivotPoint");
		this.pivotPoint.transform.position = new Vector3((float)(Level.Current.Right - Level.Current.Width / 4), (float)(Level.Current.Ground + Level.Current.Height / 2), 0f);
		this.speed = (float)properties.CurrentState.heliHead.heliheadMovementSpeed;
		this.attackDelay = properties.CurrentState.heliHead.attackDelay;
		this.width = 300f;
		this.properties = properties;
		this.speed = (float)properties.CurrentState.heliHead.heliheadMovementSpeed;
		base.transform.Rotate(Vector3.forward, 90f);
		base.transform.position = this.spawnPoint.position;
		base.StartCoroutine(this.horizontalMovement_cr());
		base.StartCoroutine(this.attack_cr());
		base.StartCoroutine(this.check_sound_cr());
	}

	// Token: 0x06002340 RID: 9024 RVA: 0x0001DDAB File Offset: 0x0001BFAB
	public void SpinSFX()
	{
		AudioManager.Play("robot_headspin");
		this.emitAudioFromObject.Add("robot_headspin");
	}

	// Token: 0x06002341 RID: 9025 RVA: 0x000BFF5C File Offset: 0x000BE15C
	public IEnumerator check_sound_cr()
	{
		bool onscreen = false;
		while (this.current == RobotLevelHelihead.state.first)
		{
			if (base.transform.position.x < (float)Level.Current.Right && base.transform.position.x > (float)Level.Current.Left)
			{
				if (!onscreen)
				{
					this.SpinSFX();
					onscreen = true;
				}
			}
			else if (onscreen)
			{
				AudioManager.Stop("robot_headspin");
				onscreen = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002342 RID: 9026 RVA: 0x000BFF78 File Offset: 0x000BE178
	public IEnumerator horizontalMovement_cr()
	{
		this.offScreen = false;
		yield return new WaitForEndOfFrame();
		base.transform.position += Vector3.left * base.GetComponent<SpriteRenderer>().bounds.size.x / 10f;
		for (;;)
		{
			base.transform.position += Vector3.left * this.speed * CupheadTime.Delta;
			if (base.transform.position.x <= (float)Level.Current.Left - this.width)
			{
				base.GetComponent<BoxCollider2D>().enabled = false;
				this.offScreen = true;
				if (this.introActive)
				{
					this.introActive = false;
					base.animator.Play("Loop");
				}
				this.speed = 0f;
				base.transform.position = new Vector3(base.transform.position.x, (float)(Level.Current.Ground + Parser.IntParse(this.screenHeights[this.coordinateIndex])), base.transform.position.z);
				base.transform.Rotate(Vector3.forward, 180f);
				this.coordinateIndex++;
				if (this.coordinateIndex >= this.screenHeights.Length)
				{
					this.coordinateIndex = 0;
				}
				yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.heliHead.offScreenDelay);
				this.speed = (float)(-(float)this.properties.CurrentState.heliHead.heliheadMovementSpeed);
				base.transform.position += Vector3.right * 50f;
				this.offScreen = false;
				base.GetComponent<BoxCollider2D>().enabled = true;
			}
			if (base.transform.position.x >= (float)Level.Current.Right + this.width)
			{
				base.GetComponent<BoxCollider2D>().enabled = false;
				this.offScreen = true;
				this.speed = 0f;
				base.transform.position = new Vector3(base.transform.position.x, (float)(Level.Current.Ground + Parser.IntParse(this.screenHeights[this.coordinateIndex])), base.transform.position.z);
				base.transform.Rotate(Vector3.forward, 180f);
				base.transform.position += Vector3.left * 50f;
				this.coordinateIndex++;
				if (this.coordinateIndex >= this.screenHeights.Length)
				{
					this.coordinateIndex = 0;
				}
				yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.heliHead.offScreenDelay);
				this.speed = (float)this.properties.CurrentState.heliHead.heliheadMovementSpeed;
				this.offScreen = false;
				base.GetComponent<BoxCollider2D>().enabled = true;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002343 RID: 9027 RVA: 0x000BFF94 File Offset: 0x000BE194
	public IEnumerator inventorIntro_cr()
	{
		Vector3 end = new Vector3(this.pivotPoint.transform.position.x, -760f);
		Vector3 start = base.transform.position;
		float pct = 0f;
		while (pct < 1f)
		{
			base.transform.position = Vector3.Lerp(start, end, pct);
			pct += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = end;
		base.StartCoroutine(this.stateEasing_cr());
		yield break;
	}

	// Token: 0x06002344 RID: 9028 RVA: 0x000BFFB0 File Offset: 0x000BE1B0
	public IEnumerator stateEasing_cr()
	{
		base.transform.rotation = Quaternion.identity;
		Vector3 start = base.transform.position;
		Vector3 end = new Vector3(this.pivotPoint.transform.position.x - 200f, this.pivotPoint.transform.position.y);
		float pct = 0f;
		while (pct < 1f)
		{
			base.transform.position = Vector3.Lerp(start, end, pct);
			pct += CupheadTime.Delta;
			yield return null;
		}
		AudioManager.Stop("robot_headspin");
		base.animator.Play("Inventor Intro");
		base.StartCoroutine(this.verticalMovement_cr());
		this.speed *= 2f;
		yield return base.animator.WaitForAnimationToEnd(this, "End", true, true);
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.inventor.initialAttackDelay);
		float normalizedTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
		float delay = 0f;
		if (base.animator.GetCurrentAnimatorStateInfo(0).length / normalizedTime < 1f)
		{
			delay -= normalizedTime;
		}
		else
		{
			delay += 1f - normalizedTime;
		}
		yield return CupheadTime.WaitForSeconds(this, delay);
		base.StartCoroutine(this.blockade_cr());
		if (this.properties.CurrentState.inventor.gemColourString.Split(new char[]
		{
			','
		})[this.attackTypeIndex] == "R")
		{
			base.animator.Play("Red Gem Attack");
			yield return base.animator.WaitForAnimationToEnd(this, "Red Gem Attack", false, true);
			base.animator.Play("RedGemFXIntro", 2);
			this.gem.InitFinalStage(this, this.properties, false);
		}
		else
		{
			base.animator.Play("Blue Gem Attack");
			yield return base.animator.WaitForAnimationToEnd(this, "Blue Gem Attack", false, true);
			base.animator.Play("BlueGemFXIntro", 2);
			this.gem.InitFinalStage(this, this.properties, true);
		}
		this.speed /= 2f;
		base.StartCoroutine(this.easeValues_cr(true));
		yield break;
	}

	// Token: 0x06002345 RID: 9029 RVA: 0x000BFFCC File Offset: 0x000BE1CC
	public IEnumerator verticalMovement_cr()
	{
		this.speed = 1f;
		float time = 0f;
		for (;;)
		{
			time += CupheadTime.Delta * 2f;
			base.transform.position = this.pivotPoint.transform.position + Vector3.left * 200f + Vector3.up * Mathf.Sin(time * this.speed) * this.verticalMovementStrength + Vector3.right * Mathf.Sin(time * (2f * this.speed)) * this.horizontalMovementStrength;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002346 RID: 9030 RVA: 0x000BFFE8 File Offset: 0x000BE1E8
	public IEnumerator easeValues_cr(bool easeIn = true)
	{
		if (easeIn)
		{
			this.speed = this.properties.CurrentState.inventor.inventorIdleSpeedMultiplier;
		}
		base.StartCoroutine(this.easeStrength_cr(easeIn));
		if (easeIn)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.inventor.attackDuration.RandomFloat());
			if (this.properties.CurrentState.inventor.gemColourString.Split(new char[]
			{
				','
			})[this.attackTypeIndex] == "R")
			{
				base.animator.SetTrigger("RedGemAttack");
			}
			else
			{
				base.animator.SetTrigger("BlueGemAttack");
			}
			this.gem.OnAttackEnd();
			this.attackTypeIndex++;
			if (this.attackTypeIndex >= this.properties.CurrentState.inventor.gemColourString.Split(new char[]
			{
				','
			}).Length)
			{
				this.attackTypeIndex = 0;
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.inventor.attackDelay.RandomFloat());
			base.StartCoroutine(this.easeValues_cr(false));
		}
		else
		{
			float normalizedTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
			float delay = 0f;
			if (base.animator.GetCurrentAnimatorStateInfo(0).length / normalizedTime < 1f)
			{
				delay -= normalizedTime;
			}
			else
			{
				delay += 1f - normalizedTime;
			}
			yield return CupheadTime.WaitForSeconds(this, delay);
			if (this.properties.CurrentState.inventor.gemColourString.Split(new char[]
			{
				','
			})[this.attackTypeIndex] == "R")
			{
				base.animator.Play("Red Gem Attack");
				yield return base.animator.WaitForAnimationToEnd(this, "Red Gem Attack", false, true);
				base.animator.Play("RedGemFXIntro", 2);
				this.gem.InitFinalStage(this, this.properties, false);
			}
			else
			{
				base.animator.Play("Blue Gem Attack");
				yield return base.animator.WaitForAnimationToEnd(this, "Blue Gem Attack", false, true);
				base.animator.Play("BlueGemFXIntro", 2);
				this.gem.InitFinalStage(this, this.properties, true);
			}
			base.StartCoroutine(this.easeValues_cr(true));
		}
		yield break;
	}

	// Token: 0x06002347 RID: 9031 RVA: 0x0001DDC7 File Offset: 0x0001BFC7
	public void OnGemEnd()
	{
		this.GemEndSFX();
		base.animator.SetTrigger("StopGemFX");
	}

	// Token: 0x06002348 RID: 9032 RVA: 0x0001DDDF File Offset: 0x0001BFDF
	public void GemStartSFX()
	{
		AudioManager.Play("robot_diamond_attack_start");
		this.emitAudioFromObject.Add("robot_diamond_attack_start");
		AudioManager.PlayLoop("robot_diamond_attack_loop");
		this.emitAudioFromObject.Add("robot_diamond_attack_loop");
	}

	// Token: 0x06002349 RID: 9033 RVA: 0x0001DE15 File Offset: 0x0001C015
	public void GemEndSFX()
	{
		AudioManager.Stop("robot_diamond_attack_loop");
		AudioManager.Play("robot_diamond_attack_end");
		this.emitAudioFromObject.Add("robot_diamond_attack_end");
	}

	// Token: 0x0600234A RID: 9034 RVA: 0x0001DE3B File Offset: 0x0001C03B
	public void IntroSFX()
	{
		AudioManager.Play("robot_head_transform");
		this.emitAudioFromObject.Add("robot_head_transform");
	}

	// Token: 0x0600234B RID: 9035 RVA: 0x000C000C File Offset: 0x000BE20C
	public IEnumerator easeSpeed_cr()
	{
		float pct = 0f;
		while (pct < 1f)
		{
			this.speed = 1f + (this.properties.CurrentState.inventor.inventorIdleSpeedMultiplier - 1f) * pct;
			pct += 10f * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600234C RID: 9036 RVA: 0x000C0028 File Offset: 0x000BE228
	public IEnumerator easeStrength_cr(bool easeIn)
	{
		float pct = 0f;
		float hStrength = this.horizontalMovementStrength;
		float vStrength = this.verticalMovementStrength;
		while (pct < 1f)
		{
			if (easeIn)
			{
				this.horizontalMovementStrength = hStrength + (25f - hStrength) * pct;
				this.verticalMovementStrength = vStrength + (160f - vStrength) * pct;
			}
			else
			{
				this.horizontalMovementStrength = hStrength + (0f - hStrength) * pct;
				this.verticalMovementStrength = vStrength + (20f - vStrength) * pct;
			}
			pct += 0.25f * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600234D RID: 9037 RVA: 0x000C004C File Offset: 0x000BE24C
	public IEnumerator attack_cr()
	{
		for (;;)
		{
			if (this.offScreen)
			{
				this.attackDelay -= CupheadTime.Delta;
				if (this.attackDelay <= 0f)
				{
					this.SpawnBombBot();
					this.attackDelay = 100f;
				}
			}
			else
			{
				this.attackDelay = this.properties.CurrentState.heliHead.attackDelay;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600234E RID: 9038 RVA: 0x000C0068 File Offset: 0x000BE268
	public IEnumerator blockade_cr()
	{
		float groupSize = (float)this.properties.CurrentState.inventor.blockadeGroupSize;
		int dir = 1;
		for (;;)
		{
			int i = 0;
			while ((float)i < groupSize)
			{
				if (dir > 0)
				{
					RobotLevelBlockade robotLevelBlockade = this.blockadeSegement.Create(new Vector3((float)Level.Current.Right, (float)Level.Current.Ceiling, 0f), dir);
					robotLevelBlockade.InitBlockade(dir, this.properties.CurrentState.inventor.blockadeHorizontalSpeed, this.properties.CurrentState.inventor.blockadeVerticalSpeed);
				}
				else
				{
					RobotLevelBlockade robotLevelBlockade2 = this.blockadeSegement.Create(new Vector3((float)Level.Current.Right, (float)Level.Current.Ground, 0f), dir);
					robotLevelBlockade2.InitBlockade(dir, this.properties.CurrentState.inventor.blockadeHorizontalSpeed, this.properties.CurrentState.inventor.blockadeVerticalSpeed);
				}
				dir *= -1;
				yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.inventor.blockadeIndividualDelay);
				yield return null;
				i++;
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.inventor.blockadeGroupDelay);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600234F RID: 9039 RVA: 0x000C0084 File Offset: 0x000BE284
	public void SpawnBombBot()
	{
		HomingProjectile homingProjectile = this.bombBotPrefab.GetComponent<RobotLevelHatchBombBot>().Create(base.transform.GetChild(0).transform.position, (float)((int)base.transform.eulerAngles.z + 90), (float)this.properties.CurrentState.bombBot.initialBombMovementSpeed, (float)this.properties.CurrentState.bombBot.bombHomingSpeed, this.properties.CurrentState.bombBot.bombRotationSpeed, (float)this.properties.CurrentState.bombBot.bombLifeTime, 4f, PlayerManager.GetNext());
		homingProjectile.GetComponent<RobotLevelHatchBombBot>().InitBombBot(this.properties.CurrentState.bombBot);
	}

	// Token: 0x06002350 RID: 9040 RVA: 0x0001DE57 File Offset: 0x0001C057
	public void ChangeState()
	{
		this.current = RobotLevelHelihead.state.second;
		this.StopAllCoroutines();
		base.StartCoroutine(this.inventorIntro_cr());
	}

	// Token: 0x06002351 RID: 9041 RVA: 0x0001DE73 File Offset: 0x0001C073
	public override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
		AudioManager.Stop("robot_diamond_attack_loop");
	}

	// Token: 0x06002352 RID: 9042 RVA: 0x0001DE8B File Offset: 0x0001C08B
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002353 RID: 9043 RVA: 0x000C0154 File Offset: 0x000BE354
	public void StartDeath()
	{
		this.StopAllCoroutines();
		if (this.OnDeath != null)
		{
			this.OnDeath();
		}
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("OnDeath");
		AudioManager.Stop("robot_diamond_attack_loop");
	}

	// Token: 0x04001D48 RID: 7496
	[SerializeField]
	public float verticalMovementStrength;

	// Token: 0x04001D49 RID: 7497
	[SerializeField]
	public float horizontalMovementStrength;

	// Token: 0x04001D4A RID: 7498
	[SerializeField]
	public Transform spawnPoint;

	// Token: 0x04001D4B RID: 7499
	public GameObject pivotPoint;

	// Token: 0x04001D4C RID: 7500
	public bool introActive;

	// Token: 0x04001D4D RID: 7501
	public int coordinateIndex;

	// Token: 0x04001D4E RID: 7502
	public string[] screenHeights;

	// Token: 0x04001D4F RID: 7503
	public float speed;

	// Token: 0x04001D50 RID: 7504
	public float width;

	// Token: 0x04001D51 RID: 7505
	public float attackDelay;

	// Token: 0x04001D52 RID: 7506
	public bool offScreen;

	// Token: 0x04001D53 RID: 7507
	public int attackTypeIndex;

	// Token: 0x04001D54 RID: 7508
	public RobotLevelHelihead.state current;

	// Token: 0x04001D55 RID: 7509
	public LevelProperties.Robot properties;

	// Token: 0x04001D56 RID: 7510
	public DamageDealer damageDealer;

	// Token: 0x04001D57 RID: 7511
	public DamageReceiver damageReceiver;

	// Token: 0x04001D58 RID: 7512
	[SerializeField]
	public GameObject bombBotPrefab;

	// Token: 0x04001D59 RID: 7513
	[SerializeField]
	public RobotLevelBlockade blockadeSegement;

	// Token: 0x04001D5A RID: 7514
	[SerializeField]
	public RobotLevelGem gem;

	// Token: 0x04001D5B RID: 7515
	public Action OnDeath;

	// Token: 0x02000E63 RID: 3683
	public enum state
	{
		// Token: 0x040067DD RID: 26589
		first,
		// Token: 0x040067DE RID: 26590
		second,
		// Token: 0x040067DF RID: 26591
		dead
	}
}
