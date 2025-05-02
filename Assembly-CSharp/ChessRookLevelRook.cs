using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000196 RID: 406
public class ChessRookLevelRook : LevelProperties.ChessRook.Entity
{
	// Token: 0x06001357 RID: 4951 RVA: 0x0001042E File Offset: 0x0000E62E
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x00010441 File Offset: 0x0000E641
	public override void LevelInit(LevelProperties.ChessRook properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x00097920 File Offset: 0x00095B20
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.cyan;
		foreach (Transform transform in this.straightShotSpawnPoints)
		{
			Gizmos.DrawLine(transform.transform.position, transform.transform.position - Vector3.right * 1000f);
		}
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x0001044A File Offset: 0x0000E64A
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600135B RID: 4955 RVA: 0x0009798C File Offset: 0x00095B8C
	public override void OnCollisionEnemyProjectile(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemyProjectile(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			ChessRookLevelPinkCannonBall component = hit.GetComponent<ChessRookLevelPinkCannonBall>();
			if (component && component.finishedOriginalArc)
			{
				this.damaged();
				component.Explosion();
			}
		}
	}

	// Token: 0x0600135C RID: 4956 RVA: 0x000979D0 File Offset: 0x00095BD0
	public void damaged()
	{
		if (this.dead)
		{
			return;
		}
		AudioManager.Play("sfx_dlc_kog_rook_hurt");
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_hurt");
		this.hitFlash.Flash(0.7f);
		LevelProperties.ChessRook.States stateName = base.properties.CurrentState.stateName;
		base.properties.DealDamage((!PlayerManager.BothPlayersActive()) ? 10f : ChessKingLevelKing.multiplayerDamageNerf);
		if (base.properties.CurrentHealth <= 0f && !this.dead)
		{
			this.die();
		}
		else if (stateName == LevelProperties.ChessRook.States.PhaseThree || stateName == LevelProperties.ChessRook.States.PhaseFour)
		{
			if (this.transitionCoroutine != null)
			{
				base.StopCoroutine(this.transitionCoroutine);
				base.animator.ResetTrigger("Transition");
				this.transitionCoroutine = null;
				base.animator.Play("LateIntro", ChessRookLevelRook.SparkLayerStateIndex);
			}
			this.hitSparkEffect.Create(base.transform.position);
			base.animator.Play("Hit2", 0, 0f);
		}
		else
		{
			base.animator.Play("Hit1", 0, 0f);
			base.animator.Play("HitSmoke", 3, 0f);
		}
	}

	// Token: 0x0600135D RID: 4957 RVA: 0x00097B24 File Offset: 0x00095D24
	public void OnPhaseChange()
	{
		this.StopAllCoroutines();
		this.StartAttacks();
		base.animator.ResetTrigger("SparkAttack");
		if (base.properties.CurrentState.stateName == LevelProperties.ChessRook.States.PhaseThree)
		{
			this.transitionCoroutine = base.StartCoroutine(this.transition_cr());
		}
	}

	// Token: 0x0600135E RID: 4958 RVA: 0x00010468 File Offset: 0x0000E668
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x00097B78 File Offset: 0x00095D78
	public void animationEvent_IntroFinished()
	{
		this.StartAttacks();
		base.animator.Play("Intro", ChessRookLevelRook.SparkLayerStateIndex);
		AudioManager.PlayLoop("sfx_dlc_kog_rook_grindingwheel_lowspeed");
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_grindingwheel_lowspeed");
		AudioManager.PlayLoop("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel");
		AudioManager.FadeSFXVolume("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel", 0.0001f, 0.0001f);
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel");
		AudioManager.PlayLoop("sfx_dlc_kog_rook_sparks_loop");
	}

	// Token: 0x06001360 RID: 4960 RVA: 0x00097BF4 File Offset: 0x00095DF4
	public IEnumerator transition_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Hit1", false, true);
		base.animator.SetTrigger("Transition");
		yield return base.animator.WaitForAnimationToEnd(this, "Transition", false, true);
		AudioManager.Stop("sfx_dlc_kog_rook_grindingwheel_lowspeed");
		AudioManager.PlayLoop("sfx_dlc_kog_rook_grindingwheel_highspeed");
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_grindingwheel_highspeed");
		this.transitionCoroutine = null;
		yield break;
	}

	// Token: 0x06001361 RID: 4961 RVA: 0x00010480 File Offset: 0x0000E680
	public void animationEvent_EndEarlyPhaseSparks()
	{
		base.animator.Play("EarlyOutro", ChessRookLevelRook.SparkLayerStateIndex);
	}

	// Token: 0x06001362 RID: 4962 RVA: 0x00010497 File Offset: 0x0000E697
	public void animationEvent_StartLatePhaseSparks()
	{
		base.animator.Play("LateIntro", ChessRookLevelRook.SparkLayerStateIndex);
	}

	// Token: 0x06001363 RID: 4963 RVA: 0x00097C10 File Offset: 0x00095E10
	public void StartAttacks()
	{
		base.StartCoroutine(this.pink_cannonballs_cr());
		base.StartCoroutine(this.regular_cannonballs_cr());
		if (base.properties.CurrentState.straightShooters.straightShotOn)
		{
			base.StartCoroutine(this.straight_shot_cr());
		}
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x00097C60 File Offset: 0x00095E60
	public IEnumerator pink_cannonballs_cr()
	{
		LevelProperties.ChessRook.PinkCannonBall p = base.properties.CurrentState.pinkCannonBall;
		PatternString delayPattern = new PatternString(p.pinkShotDelayString, true, true);
		PatternString apexHeightPattern = new PatternString(p.pinkShotApexHeightString, true, true);
		PatternString targetPattern = new PatternString(p.pinkShotTargetString, true, true);
		for (;;)
		{
			float delay = delayPattern.PopFloat();
			float apexHeight = apexHeightPattern.PopFloat();
			float targetDistance = targetPattern.PopFloat();
			yield return CupheadTime.WaitForSeconds(this, delay - 0.166666672f);
			this.spawnEffect.Play("Spawn" + ((!Rand.Bool()) ? "B" : "A") + "Head" + ((!this.headTypeB) ? "A" : "B"), 0, 0f);
			this.spawnEffect.Update(0f);
			yield return CupheadTime.WaitForSeconds(this, 0.166666672f);
			ChessRookLevelPinkCannonBall cannonBall = this.cannonballPink.Spawn<ChessRookLevelPinkCannonBall>();
			cannonBall.Create(this.cannonballSpawnRoot.position + base.transform.forward * 1E-05f * (float)this.headZOffset, apexHeight, targetDistance, p);
			cannonBall.animator.Play((!this.headTypeB) ? "A" : "B");
			this.headTypeB = !this.headTypeB;
			this.headZOffset = (this.headZOffset + 1) % 10;
		}
		yield break;
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x00097C7C File Offset: 0x00095E7C
	public IEnumerator regular_cannonballs_cr()
	{
		LevelProperties.ChessRook.RegularCannonBall p = base.properties.CurrentState.regularCannonBall;
		PatternString delayPattern = new PatternString(p.cannonDelayString, true, true);
		PatternString apexHeightPattern = new PatternString(p.cannonApexHeightString, true, true);
		PatternString targetPattern = new PatternString(p.cannonTargetString, true, true);
		for (;;)
		{
			float delay = delayPattern.PopFloat();
			float apexHeight = apexHeightPattern.PopFloat();
			float targetDistance = targetPattern.PopFloat();
			yield return CupheadTime.WaitForSeconds(this, delay - 0.166666672f);
			this.spawnEffect.Play("Spawn" + ((!Rand.Bool()) ? "B" : "A") + "Skull", 0, 0f);
			this.spawnEffect.Update(0f);
			yield return CupheadTime.WaitForSeconds(this, 0.166666672f);
			ChessRookLevelRegularCannonball cannonBall = this.cannonballRegular.Spawn<ChessRookLevelRegularCannonball>();
			cannonBall.Create(this.cannonballSpawnRoot.position + base.transform.forward * 1E-05f * (float)this.headZOffset, apexHeight, targetDistance, p);
			this.headZOffset = (this.headZOffset + 1) % 10;
		}
		yield break;
	}

	// Token: 0x06001366 RID: 4966 RVA: 0x00097C98 File Offset: 0x00095E98
	public IEnumerator straight_shot_cr()
	{
		LevelProperties.ChessRook.StraightShooters p = base.properties.CurrentState.straightShooters;
		PatternString sequencePattern = new PatternString(p.straightShotSeqString, true, true);
		PatternString delayPattern = new PatternString(p.straightShotDelayString, true, true);
		float EarlyPhaseTransitionOffset = 0.145833328f;
		Rangef EarlyPhaseShootOffsetRange = new Rangef(0.166666672f, 0.333333343f);
		for (;;)
		{
			float delay = delayPattern.PopFloat();
			bool isEarlyPhase = base.properties.CurrentState.stateName == LevelProperties.ChessRook.States.Main || base.properties.CurrentState.stateName == LevelProperties.ChessRook.States.PhaseTwo;
			float shootDelay = 0f;
			if (isEarlyPhase)
			{
				shootDelay = Random.Range(EarlyPhaseShootOffsetRange.minimum, EarlyPhaseShootOffsetRange.maximum);
				delay -= EarlyPhaseTransitionOffset;
				delay -= shootDelay;
			}
			yield return CupheadTime.WaitForSeconds(this, delay);
			if (isEarlyPhase)
			{
				base.animator.SetTrigger("SparkAttack");
				yield return base.animator.WaitForAnimationToEnd(this, "Idle1.Main", false, true);
				base.animator.Play("EarlyActiveA", ChessRookLevelRook.SparkLayerStateIndex);
				yield return CupheadTime.WaitForSeconds(this, shootDelay);
			}
			char sequence = sequencePattern.PopLetter();
			int spawnPosIndex = 0;
			if (sequence == 'T')
			{
				spawnPosIndex = 0;
			}
			else if (sequence == 'M')
			{
				spawnPosIndex = 1;
			}
			else if (sequence == 'B')
			{
				spawnPosIndex = 2;
			}
			Vector3 position = this.straightShotSpawnPoints[spawnPosIndex].position;
			this.straightShot.Create(position, 180f, p.straightShotBulletSpeed);
			this.smokeEffect.Create(position);
			this.straightShotSparkEffect.Create(position);
			AudioManager.Play("sfx_dlc_kog_rook_sparks_singles");
		}
		yield break;
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x00097CB4 File Offset: 0x00095EB4
	public void die()
	{
		this.dead = true;
		this.StopAllCoroutines();
		AudioManager.Play("sfx_dlc_kog_rook_death");
		AudioManager.Stop("sfx_dlc_kog_rook_sparks_loop");
		AudioManager.Stop("sfx_dlc_kog_rook_grindingwheel_lowspeed");
		AudioManager.Stop("sfx_dlc_kog_rook_grindingwheel_highspeed");
		AudioManager.Stop("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel");
		base.animator.Play("Death", ChessRookLevelRook.BaseLayerStateIndex);
		base.animator.Play("Off", ChessRookLevelRook.SparkLayerStateIndex);
		base.animator.Play("Off", ChessRookLevelRook.WheelLayerStateIndex);
		this.wheelRenderer.sortingOrder = 1000;
		this.wheelRenderer.sortingLayerName = "Foreground";
	}

	// Token: 0x06001368 RID: 4968 RVA: 0x000104AE File Offset: 0x0000E6AE
	public void SFX_GrindAxe()
	{
		base.StartCoroutine(this.sfx_grind_axe_cr());
	}

	// Token: 0x06001369 RID: 4969 RVA: 0x00097D60 File Offset: 0x00095F60
	public IEnumerator sfx_grind_axe_cr()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel", 0.7f, 0.1f);
		yield return CupheadTime.WaitForSeconds(this, 0.3f);
		AudioManager.FadeSFXVolume("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel", 0.0001f, 0.1f);
		yield break;
	}

	// Token: 0x04000FAA RID: 4010
	public static readonly int BaseLayerStateIndex;

	// Token: 0x04000FAB RID: 4011
	public static readonly int SparkLayerStateIndex = 1;

	// Token: 0x04000FAC RID: 4012
	public static readonly int WheelLayerStateIndex = 2;

	// Token: 0x04000FAD RID: 4013
	[SerializeField]
	public SpriteRenderer wheelRenderer;

	// Token: 0x04000FAE RID: 4014
	[SerializeField]
	public Transform cannonballSpawnRoot;

	// Token: 0x04000FAF RID: 4015
	[SerializeField]
	public ChessRookLevelPinkCannonBall cannonballPink;

	// Token: 0x04000FB0 RID: 4016
	[SerializeField]
	public ChessRookLevelRegularCannonball cannonballRegular;

	// Token: 0x04000FB1 RID: 4017
	[SerializeField]
	public BasicProjectile straightShot;

	// Token: 0x04000FB2 RID: 4018
	[SerializeField]
	public Transform[] straightShotSpawnPoints;

	// Token: 0x04000FB3 RID: 4019
	[SerializeField]
	public Effect hitSparkEffect;

	// Token: 0x04000FB4 RID: 4020
	[SerializeField]
	public Effect straightShotSparkEffect;

	// Token: 0x04000FB5 RID: 4021
	[SerializeField]
	public Effect smokeEffect;

	// Token: 0x04000FB6 RID: 4022
	[SerializeField]
	public Animator spawnEffect;

	// Token: 0x04000FB7 RID: 4023
	[SerializeField]
	public HitFlash hitFlash;

	// Token: 0x04000FB8 RID: 4024
	public DamageDealer damageDealer;

	// Token: 0x04000FB9 RID: 4025
	public Coroutine transitionCoroutine;

	// Token: 0x04000FBA RID: 4026
	public bool dead;

	// Token: 0x04000FBB RID: 4027
	public bool headTypeB;

	// Token: 0x04000FBC RID: 4028
	public int headZOffset;
}
