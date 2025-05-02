using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200017E RID: 382
public class ChessBishopLevelBishop : LevelProperties.ChessBishop.Entity
{
	// Token: 0x06001213 RID: 4627 RVA: 0x00093E88 File Offset: 0x00092088
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		for (int i = 0; i < this.candles.Length; i++)
		{
			this.candles[i].Init(base.properties.CurrentState.candle.candleDistToBlowout);
		}
	}

	// Token: 0x06001214 RID: 4628 RVA: 0x0000F4E1 File Offset: 0x0000D6E1
	public override void LevelInit(LevelProperties.ChessBishop properties)
	{
		base.LevelInit(properties);
		Level.Current.OnIntroEvent += this.onIntroEventHandler;
		this.setupPatternStrings();
	}

	// Token: 0x06001215 RID: 4629 RVA: 0x00093EDC File Offset: 0x000920DC
	public void UpdateBodyFade()
	{
		float num = Mathf.Clamp(this.bodyOpacity, 0f, 1f);
		this.bodyRenderer.color = new Color(1f, 1f, 1f, num);
		this.bodyRenderer.material.SetFloat("_BlurAmount", (1f - num) * 5f);
		this.bodyRenderer.material.SetFloat("_BlurLerp", (1f - num) * 5f);
	}

	// Token: 0x06001216 RID: 4630 RVA: 0x00093F64 File Offset: 0x00092164
	public void FixedUpdate()
	{
		if (PlayerManager.GetPlayer(PlayerId.PlayerOne) && !PlayerManager.GetPlayer(PlayerId.PlayerOne).IsDead)
		{
			this.playerMask[0].transform.position = PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position + Vector3.up * 50f;
		}
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) && !PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead)
		{
			this.playerMask[1].transform.position = PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position + Vector3.up * 50f;
		}
		if (this.introPlaying || this.dead)
		{
			return;
		}
		this.bodyOpacity -= CupheadTime.FixedDelta * this.fadeRate;
		this.UpdateBodyFade();
		if (this.damageDealer != null)
		{
			this.damageDealer.FixedUpdate();
		}
	}

	// Token: 0x06001217 RID: 4631 RVA: 0x0000F506 File Offset: 0x0000D706
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06001218 RID: 4632 RVA: 0x0009406C File Offset: 0x0009226C
	public void StartNewPhase()
	{
		this.stateDidChange = true;
		this.cancelShoot();
		this.StopAllCoroutines();
		this.candleOrderMainIndex %= base.properties.CurrentState.candle.candleOrder.Length;
		this.setupPatternStrings();
		base.StartCoroutine(this.disappear_cr());
	}

	// Token: 0x06001219 RID: 4633 RVA: 0x000940C4 File Offset: 0x000922C4
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		this.cancelShoot();
		base.properties.DealDamage((!PlayerManager.BothPlayersActive()) ? 10f : ChessKingLevelKing.multiplayerDamageNerf);
		if (base.properties.CurrentHealth <= 0f)
		{
			this.die();
		}
		else
		{
			this.bodyOpacity = 1.75f;
			this.bodyAnimator.SetTrigger("Hit");
			this.bodyExplosion.Create(this.bodyExplosionSpawnPoint.position);
			this.turnDormant(this.stateDidChange);
			this.stateDidChange = false;
		}
	}

	// Token: 0x0600121A RID: 4634 RVA: 0x0000F51D File Offset: 0x0000D71D
	public void onIntroEventHandler()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x0600121B RID: 4635 RVA: 0x00094168 File Offset: 0x00092368
	public IEnumerator intro_cr()
	{
		this.bodyAnimator.SetTrigger("StartIntro");
		yield return this.bodyAnimator.WaitForAnimationToEnd(this, "Intro.End", false, true);
		this.isPathTwo = MathUtils.RandomBool();
		this.startPath();
		yield return CupheadTime.WaitForSeconds(this, 0.55f);
		base.animator.SetBool("CanParry", true);
		base.animator.SetTrigger("Appear");
		yield return base.animator.WaitForAnimationToEnd(this, "AppearActive", false, true);
		this.candleOrderMainIndex = Random.Range(0, base.properties.CurrentState.candle.candleOrder.Length);
		this.candlesHolder.SetActive(true);
		this.canMove = true;
		this.introPlaying = false;
		yield break;
	}

	// Token: 0x0600121C RID: 4636 RVA: 0x00094184 File Offset: 0x00092384
	public void turnDormant(bool willDisappear)
	{
		this._canParry = false;
		if (base.properties.CurrentHealth > 0f)
		{
			base.StartCoroutine(this.candles_cr());
		}
		if (!willDisappear)
		{
			base.animator.SetTrigger("ToDormant");
			base.StartCoroutine(this.postHitToggleCollider_cr());
		}
	}

	// Token: 0x0600121D RID: 4637 RVA: 0x000941E0 File Offset: 0x000923E0
	public IEnumerator postHitToggleCollider_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.bishop.colliderOffTime);
		base.GetComponent<Collider2D>().enabled = true;
		yield break;
	}

	// Token: 0x0600121E RID: 4638 RVA: 0x000941FC File Offset: 0x000923FC
	public IEnumerator candles_cr()
	{
		while (this.disappearingState == ChessBishopLevelBishop.DisappearingState.Disappearing)
		{
			yield return null;
		}
		LevelProperties.ChessBishop.Candle p = base.properties.CurrentState.candle;
		string[] candleOrder = p.candleOrder[this.candleOrderMainIndex].Split(new char[]
		{
			','
		});
		int length = candleOrder.Length + ((!PlayerManager.BothPlayersActive()) ? 0 : 3);
		ChessBishopLevelCandle[] activeCandles = new ChessBishopLevelCandle[length];
		int index = 0;
		for (int i = 0; i < candleOrder.Length; i++)
		{
			Parser.IntTryParse(candleOrder[i], out index);
			this.candles[index].LightUp();
			activeCandles[i] = this.candles[index];
		}
		if (PlayerManager.BothPlayersActive())
		{
			List<ChessBishopLevelCandle> list = (from c in this.candles
			where !c.isLit
			select c).ToList<ChessBishopLevelCandle>();
			for (int j = 0; j < 3; j++)
			{
				if (list.Count > 0)
				{
					index = Random.Range(0, list.Count);
					list[index].LightUp();
					activeCandles[candleOrder.Length + j] = list[index];
					list.RemoveAt(index);
				}
			}
		}
		this.candleOrderMainIndex = (this.candleOrderMainIndex + 1) % p.candleOrder.Length;
		yield return null;
		bool candlesStillLit = true;
		while (candlesStillLit)
		{
			candlesStillLit = false;
			for (int k = 0; k < activeCandles.Length; k++)
			{
				if (activeCandles[k] != null && activeCandles[k].isLit)
				{
					candlesStillLit = true;
					break;
				}
			}
			yield return null;
		}
		this.cancelShoot();
		if (this.disappearingState == ChessBishopLevelBishop.DisappearingState.Disappearing)
		{
			this._canParry = true;
			yield break;
		}
		while (this.disappearingState == ChessBishopLevelBishop.DisappearingState.Reappearing)
		{
			yield return null;
		}
		this._canParry = true;
		base.animator.SetTrigger("ToActive");
		yield break;
	}

	// Token: 0x0600121F RID: 4639 RVA: 0x00094218 File Offset: 0x00092418
	public IEnumerator disappear_cr()
	{
		this.disappearingState = ChessBishopLevelBishop.DisappearingState.Disappearing;
		this.isFirstPhase = false;
		this.canMove = false;
		Collider2D collider = base.GetComponent<Collider2D>();
		collider.enabled = false;
		base.animator.SetTrigger("HitDisappear");
		yield return base.animator.WaitForAnimationToEnd(this, "HitDisappear", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.invisibleTime.PopFloat());
		this.disappearingState = ChessBishopLevelBishop.DisappearingState.Reappearing;
		this.isPathTwo = !this.isPathTwo;
		this.startPath();
		base.animator.SetBool("CanParry", this._canParry);
		base.animator.SetTrigger("Appear");
		string animationName = (!base.canParry) ? "AppearDormant" : "AppearActive";
		yield return base.animator.WaitForAnimationToEnd(this, animationName, false, true);
		this.canMove = true;
		collider.enabled = true;
		this.disappearingState = ChessBishopLevelBishop.DisappearingState.None;
		yield break;
	}

	// Token: 0x06001220 RID: 4640 RVA: 0x0000F52C File Offset: 0x0000D72C
	public void startPath()
	{
		if (this.isPathTwo)
		{
			base.StartCoroutine(this.moveHorizontal_cr());
		}
		else
		{
			base.StartCoroutine(this.moveVertical_cr());
		}
	}

	// Token: 0x06001221 RID: 4641 RVA: 0x00094234 File Offset: 0x00092434
	public IEnumerator moveVertical_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		Vector3 pivotOffset = Vector3.up * 2f * 150f;
		this.invert = true;
		float value = -1f;
		float speed = base.properties.CurrentState.bishop.movementSpeed;
		float angle = 3.92699075f;
		if (!this.isFirstPhase)
		{
			float minimumDistance = base.GetComponent<CircleCollider2D>().radius * 1.5f;
			angle = ChessBishopLevelBishop.findMoveVerticalInitialAngle(minimumDistance, value, this.invert, speed, this.pivotPoint, pivotOffset);
		}
		base.StartCoroutine(this.spawnProjectiles_cr());
		for (;;)
		{
			base.transform.position = ChessBishopLevelBishop.calculateMoveVerticalPosition(ref angle, ref value, ref this.invert, speed, this.pivotPoint, pivotOffset);
			while (!this.canMove)
			{
				yield return null;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001222 RID: 4642 RVA: 0x00094250 File Offset: 0x00092450
	public static float findMoveVerticalInitialAngle(float minimumDistance, float value, bool invert, float speed, Transform pivotPoint, Vector3 pivotOffset)
	{
		float num = minimumDistance * minimumDistance;
		List<Vector3> list = new List<Vector3>();
		if (PlayerManager.DoesPlayerExist(PlayerId.PlayerOne))
		{
			list.Add(PlayerManager.GetPlayer(PlayerId.PlayerOne).center);
		}
		if (PlayerManager.DoesPlayerExist(PlayerId.PlayerTwo))
		{
			list.Add(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center);
		}
		int i = 0;
		float num2 = 0f;
		while (i < 20)
		{
			i++;
			num2 = Random.Range(0f, 6.28318548f);
			float num3 = num2;
			float num4 = value;
			bool flag = invert;
			Vector3 vector = ChessBishopLevelBishop.calculateMoveVerticalPosition(ref num3, ref num4, ref flag, speed, pivotPoint, pivotOffset);
			bool flag2 = false;
			foreach (Vector3 vector2 in list)
			{
				if ((vector - vector2).sqrMagnitude < num)
				{
					flag2 = true;
				}
			}
			if (!flag2)
			{
				break;
			}
		}
		return num2;
	}

	// Token: 0x06001223 RID: 4643 RVA: 0x00094354 File Offset: 0x00092554
	public static Vector3 calculateMoveVerticalPosition(ref float angle, ref float value, ref bool invert, float speed, Transform pivotPoint, Vector3 pivotOffset)
	{
		angle += speed * CupheadTime.FixedDelta;
		if (angle > 6.28318548f)
		{
			invert = !invert;
			angle -= 6.28318548f;
		}
		if (angle < 0f)
		{
			angle += 6.28318548f;
		}
		Vector3 vector;
		if (invert)
		{
			vector = pivotPoint.position + pivotOffset;
			value = -1f;
		}
		else
		{
			vector = pivotPoint.position;
			value = 1f;
		}
		Vector3 vector2;
		vector2..ctor(-Mathf.Sin(angle) * 500f, Mathf.Cos(angle) * value * 150f, 0f);
		return vector + vector2;
	}

	// Token: 0x06001224 RID: 4644 RVA: 0x00094404 File Offset: 0x00092604
	public IEnumerator moveHorizontal_cr()
	{
		LevelProperties.ChessBishop.Bishop p = base.properties.CurrentState.bishop;
		YieldInstruction wait = new WaitForFixedUpdate();
		this.invert = true;
		float xSpeed = p.xSpeed;
		float amplitude = p.amplitude;
		float frequency = p.freqMultiplier * 2f * 3.14159274f / (p.maxDistance * 2f);
		if (this.isFirstPhase)
		{
			base.transform.position = new Vector3(500f, base.transform.position.y);
		}
		else
		{
			float minimumDistance = base.GetComponent<CircleCollider2D>().radius * 1.5f;
			base.transform.position = ChessBishopLevelBishop.findMoveHorizontalInitialPosition(minimumDistance, base.transform.position.y, p.maxDistance, xSpeed, amplitude, frequency);
		}
		base.StartCoroutine(this.spawnProjectiles_cr());
		Vector3 goalPos = base.transform.position;
		float distanceTraveled = 1.57079637f;
		for (;;)
		{
			base.transform.position = ChessBishopLevelBishop.calculateMoveHorizontalPosition(ref goalPos, ref xSpeed, ref distanceTraveled, amplitude, frequency, p.maxDistance);
			while (!this.canMove)
			{
				yield return null;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001225 RID: 4645 RVA: 0x00094420 File Offset: 0x00092620
	public static Vector3 findMoveHorizontalInitialPosition(float minimumDistance, float yPosition, float maxDistance, float xSpeed, float amplitude, float frequency)
	{
		float num = minimumDistance * minimumDistance;
		List<Vector3> list = new List<Vector3>();
		if (PlayerManager.DoesPlayerExist(PlayerId.PlayerOne))
		{
			list.Add(PlayerManager.GetPlayer(PlayerId.PlayerOne).center);
		}
		if (PlayerManager.DoesPlayerExist(PlayerId.PlayerTwo))
		{
			list.Add(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center);
		}
		int i = 0;
		Vector3 zero = Vector3.zero;
		while (i < 20)
		{
			i++;
			zero..ctor(Random.Range(-maxDistance, maxDistance), yPosition);
			Vector3 vector = zero;
			float num2 = 1.57079637f;
			float num3 = xSpeed;
			ChessBishopLevelBishop.calculateMoveHorizontalPosition(ref vector, ref num3, ref num2, amplitude, frequency, maxDistance);
			bool flag = false;
			foreach (Vector3 vector2 in list)
			{
				if ((zero - vector2).sqrMagnitude < num)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				break;
			}
		}
		return zero;
	}

	// Token: 0x06001226 RID: 4646 RVA: 0x00094528 File Offset: 0x00092728
	public static Vector3 calculateMoveHorizontalPosition(ref Vector3 goalPosition, ref float xSpeed, ref float distanceTravelled, float amplitude, float frequency, float maxDistance)
	{
		Vector3 vector = goalPosition;
		vector.x += xSpeed * CupheadTime.FixedDelta;
		distanceTravelled += Mathf.Abs(xSpeed) * CupheadTime.FixedDelta;
		if (vector.x > maxDistance || vector.x < -maxDistance)
		{
			xSpeed *= -1f;
		}
		vector.y = amplitude * Mathf.Sin(frequency * distanceTravelled);
		goalPosition = vector;
		if (vector.x < -maxDistance + 100f || vector.x > maxDistance - 100f)
		{
			float num = Mathf.InverseLerp(maxDistance - 100f, maxDistance, Mathf.Abs(vector.x));
			num *= 1.57079637f;
			num = Mathf.Sin(num) * 100f / 2f;
			vector.x = (maxDistance - 100f + num) * Mathf.Sign(vector.x);
		}
		return vector;
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x00094624 File Offset: 0x00092824
	public IEnumerator spawnProjectiles_cr()
	{
		while (!this.canMove)
		{
			yield return null;
		}
		LevelProperties.ChessBishop.Bishop p = base.properties.CurrentState.bishop;
		PatternString delayPattern = new PatternString(p.attackDelayString, true, true);
		for (;;)
		{
			float delay = delayPattern.PopFloat();
			yield return CupheadTime.WaitForSeconds(this, delay);
			this.bulletSpawnCoroutine = base.StartCoroutine(this.shoot_cr());
			while (this.bulletSpawnCoroutine != null)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06001228 RID: 4648 RVA: 0x00094640 File Offset: 0x00092840
	public IEnumerator shoot_cr()
	{
		float previousTime = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		float currentTime = previousTime;
		while (previousTime >= 0.625f || currentTime <= 0.625f)
		{
			yield return null;
			previousTime = currentTime;
			currentTime = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		}
		if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("IdleActive") || base.animator.GetCurrentAnimatorStateInfo(0).IsName("IsDormant"))
		{
			this.mainRenderer.enabled = false;
		}
		this.summonOverlayRenderer.enabled = true;
		previousTime = (currentTime = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
		while (previousTime <= currentTime)
		{
			yield return null;
			previousTime = currentTime;
			currentTime = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		}
		ChessBishopLevelBell bell = this.bellProjectile.Spawn<ChessBishopLevelBell>();
		this.SFX_KOG_Bishop_Shoot();
		bell.Init(this.projectileSpawnPoint.position, PlayerManager.GetNext(), base.properties.CurrentState.bishop);
		previousTime = (currentTime = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
		while (previousTime >= 0.525f || currentTime <= 0.525f)
		{
			yield return null;
			previousTime = currentTime;
			currentTime = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		}
		this.mainRenderer.enabled = true;
		this.summonOverlayRenderer.enabled = false;
		this.bulletSpawnCoroutine = null;
		yield break;
	}

	// Token: 0x06001229 RID: 4649 RVA: 0x0000F558 File Offset: 0x0000D758
	public void cancelShoot()
	{
		if (this.bulletSpawnCoroutine != null)
		{
			base.StopCoroutine(this.bulletSpawnCoroutine);
			this.bulletSpawnCoroutine = null;
		}
		this.mainRenderer.enabled = true;
		this.summonOverlayRenderer.enabled = false;
	}

	// Token: 0x0600122A RID: 4650 RVA: 0x0009465C File Offset: 0x0009285C
	public void die()
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		this.bodyOpacity = 1f;
		this.UpdateBodyFade();
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		this.SFX_KOG_Bishop_Death();
		this.bodyAnimator.Play("Death");
		base.animator.Play("Death");
		base.animator.Update(0f);
	}

	// Token: 0x0600122B RID: 4651 RVA: 0x0000F590 File Offset: 0x0000D790
	public void setupPatternStrings()
	{
		this.invisibleTime = new PatternString(base.properties.CurrentState.bishop.invisibleTimeString, true);
	}

	// Token: 0x0600122C RID: 4652 RVA: 0x0000F5B3 File Offset: 0x0000D7B3
	public void AnimationEvent_SFX_KOG_Bishop_Wakeup()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_wakeup");
	}

	// Token: 0x0600122D RID: 4653 RVA: 0x0000F5BF File Offset: 0x0000D7BF
	public void AnimationEvent_SFX_KOG_Bishop_HeadDisappearsFromBody()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_headdisappearsfrombody");
		this.emitAudioFromObject.Add("sfx_dlc_kog_bishop_headdisappearsfrombody");
	}

	// Token: 0x0600122E RID: 4654 RVA: 0x0000F5DB File Offset: 0x0000D7DB
	public void AnimationEvent_SFX_KOG_Bishop_HeadReappears()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_headreappears");
		this.emitAudioFromObject.Add("sfx_dlc_kog_bishop_headreappears");
	}

	// Token: 0x0600122F RID: 4655 RVA: 0x0000F5F7 File Offset: 0x0000D7F7
	public void SFX_KOG_Bishop_Shoot()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_shoot");
		this.emitAudioFromObject.Add("sfx_dlc_kog_bishop_shoot");
	}

	// Token: 0x06001230 RID: 4656 RVA: 0x0000F613 File Offset: 0x0000D813
	public void SFX_KOG_Bishop_Death()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_death");
		AudioManager.Play("sfx_level_knockout_boom");
	}

	// Token: 0x06001231 RID: 4657 RVA: 0x0000F629 File Offset: 0x0000D829
	public void AnimationEvent_SFX_KOG_Bishop_Vocal()
	{
		base.StartCoroutine(this.SFX_KOG_Bihop_Vocal_cr());
	}

	// Token: 0x06001232 RID: 4658 RVA: 0x000946D8 File Offset: 0x000928D8
	public IEnumerator SFX_KOG_Bihop_Vocal_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AudioManager.Play("sfx_dlc_kog_bishop_vocal");
		this.emitAudioFromObject.Add("sfx_dlc_kog_bishop_vocal");
		yield break;
	}

	// Token: 0x04000E96 RID: 3734
	public const int MULTIPLAYER_CANDLE_DIFFERENCE = 3;

	// Token: 0x04000E97 RID: 3735
	public const float VER_LOOPSIZEY = 150f;

	// Token: 0x04000E98 RID: 3736
	public const float VER_LOOPSIZEX = 500f;

	// Token: 0x04000E99 RID: 3737
	public const float H_EASE_DISTANCE = 100f;

	// Token: 0x04000E9A RID: 3738
	[SerializeField]
	public ChessBishopLevelBell bellProjectile;

	// Token: 0x04000E9B RID: 3739
	[SerializeField]
	public SpriteRenderer mainRenderer;

	// Token: 0x04000E9C RID: 3740
	[SerializeField]
	public SpriteRenderer summonOverlayRenderer;

	// Token: 0x04000E9D RID: 3741
	[SerializeField]
	public Transform projectileSpawnPoint;

	// Token: 0x04000E9E RID: 3742
	[SerializeField]
	public Transform pivotPoint;

	// Token: 0x04000E9F RID: 3743
	[SerializeField]
	public GameObject candlesHolder;

	// Token: 0x04000EA0 RID: 3744
	[SerializeField]
	public ChessBishopLevelCandle[] candles;

	// Token: 0x04000EA1 RID: 3745
	[SerializeField]
	public Animator bodyAnimator;

	// Token: 0x04000EA2 RID: 3746
	[SerializeField]
	public Effect bodyExplosion;

	// Token: 0x04000EA3 RID: 3747
	[SerializeField]
	public Transform bodyExplosionSpawnPoint;

	// Token: 0x04000EA4 RID: 3748
	[SerializeField]
	public SpriteRenderer bodyRenderer;

	// Token: 0x04000EA5 RID: 3749
	public float bodyOpacity = 1f;

	// Token: 0x04000EA6 RID: 3750
	[SerializeField]
	public float fadeRate = 0.75f;

	// Token: 0x04000EA7 RID: 3751
	[SerializeField]
	public GameObject[] playerMask;

	// Token: 0x04000EA8 RID: 3752
	public int candleOrderMainIndex;

	// Token: 0x04000EA9 RID: 3753
	public bool invert;

	// Token: 0x04000EAA RID: 3754
	public bool isPathTwo;

	// Token: 0x04000EAB RID: 3755
	public DamageDealer damageDealer;

	// Token: 0x04000EAC RID: 3756
	public PatternString invisibleTime;

	// Token: 0x04000EAD RID: 3757
	public bool canMove;

	// Token: 0x04000EAE RID: 3758
	public bool isFirstPhase = true;

	// Token: 0x04000EAF RID: 3759
	public bool stateDidChange;

	// Token: 0x04000EB0 RID: 3760
	public ChessBishopLevelBishop.DisappearingState disappearingState;

	// Token: 0x04000EB1 RID: 3761
	public bool dead;

	// Token: 0x04000EB2 RID: 3762
	public bool introPlaying = true;

	// Token: 0x04000EB3 RID: 3763
	public Coroutine bulletSpawnCoroutine;

	// Token: 0x02000AA7 RID: 2727
	public enum DisappearingState
	{
		// Token: 0x04004E4C RID: 20044
		None,
		// Token: 0x04004E4D RID: 20045
		Disappearing,
		// Token: 0x04004E4E RID: 20046
		Reappearing
	}
}
