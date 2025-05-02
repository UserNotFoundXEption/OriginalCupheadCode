using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000154 RID: 340
public class BaronessLevelWaffle : BaronessLevelMiniBossBase
{
	// Token: 0x1700023A RID: 570
	// (get) Token: 0x06001052 RID: 4178 RVA: 0x0000DC90 File Offset: 0x0000BE90
	// (set) Token: 0x06001053 RID: 4179 RVA: 0x0000DC98 File Offset: 0x0000BE98
	public BaronessLevelWaffle.State state { get; set; }

	// Token: 0x06001054 RID: 4180 RVA: 0x0008FB3C File Offset: 0x0008DD3C
	public override void Awake()
	{
		base.Awake();
		float num = (float)Random.Range(0, 2);
		this.pathA = (num == 0f);
		this.check = true;
		this.isDead = false;
		this.isDying = false;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.mouth.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		base.GetComponent<Collider2D>().enabled = true;
		for (int i = 0; i < this.diagonalPieces.Length; i++)
		{
			this.diagonalPieces[i].wafflepiece.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
			this.diagonalPieces[i].wafflepiece.GetComponent<Collider2D>().enabled = false;
			this.diagonalPieces[i].wafflepiece.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		}
		for (int j = 0; j < this.straightPieces.Length; j++)
		{
			this.straightPieces[j].wafflepiece.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
			this.straightPieces[j].wafflepiece.GetComponent<Collider2D>().enabled = false;
			this.straightPieces[j].wafflepiece.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		}
	}

	// Token: 0x06001055 RID: 4181 RVA: 0x0000DCA1 File Offset: 0x0000BEA1
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001056 RID: 4182 RVA: 0x0008FCD0 File Offset: 0x0008DED0
	public void Init(LevelProperties.Baroness.Waffle properties, Vector2 pos, Transform pivot, float speed, float health)
	{
		this.properties = properties;
		this.speed = speed;
		this.health = health;
		base.transform.position = pos;
		this.pivotPoint = pivot;
		this.state = BaronessLevelWaffle.State.Enter;
		base.StartCoroutine(this.enter_cr());
		base.StartCoroutine(this.switchLayer_cr());
	}

	// Token: 0x06001057 RID: 4183 RVA: 0x0000DCBF File Offset: 0x0000BEBF
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001058 RID: 4184 RVA: 0x0008FD30 File Offset: 0x0008DF30
	public IEnumerator switchLayer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		base.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Enemies.ToString();
		base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
		yield break;
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x0008FD4C File Offset: 0x0008DF4C
	public override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.health > 0f)
		{
			base.OnDamageTaken(info);
		}
		this.health -= info.damage;
		if (this.health < 0f && this.state == BaronessLevelWaffle.State.Move)
		{
			DamageDealer.DamageInfo info2 = new DamageDealer.DamageInfo(this.health, info.direction, info.origin, info.damageSource);
			base.OnDamageTaken(info2);
			this.isDead = true;
			base.StartCoroutine(this.death_cr());
		}
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x0000DCD7 File Offset: 0x0000BED7
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.explosion = null;
		this.explosionReverse = null;
		this.straightPieces = null;
		this.diagonalPieces = null;
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x0008FDD8 File Offset: 0x0008DFD8
	public IEnumerator enter_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		this.originalPivotPos = this.pivotPoint.transform.position;
		if (this.pathA)
		{
			this.startPos = this.pivotPoint.position + Vector3.right * this.loopSize;
			this.angle = -1.57079637f;
		}
		else
		{
			this.startPos = this.pivotPoint.position + Vector3.down * this.loopSize;
			this.angle = 3.14159274f;
			this.speed = -this.speed;
		}
		while (base.transform.position != this.startPos)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.startPos, this.properties.movementSpeed * 300f * CupheadTime.FixedDelta);
			yield return wait;
		}
		this.StartCircle();
		yield return null;
		yield break;
	}

	// Token: 0x0600105C RID: 4188 RVA: 0x0000DCFB File Offset: 0x0000BEFB
	public void StartCircle()
	{
		this.state = BaronessLevelWaffle.State.Move;
		base.StartCoroutine(this.circle_cr());
		base.StartCoroutine(this.check_attack_cr());
	}

	// Token: 0x0600105D RID: 4189 RVA: 0x0008FDF4 File Offset: 0x0008DFF4
	public IEnumerator circle_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (!this.isDead)
		{
			if (this.state == BaronessLevelWaffle.State.Move)
			{
				this.MovePivot();
				this.PathMovement();
				this.CheckIfTurn();
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x0600105E RID: 4190 RVA: 0x0008FE10 File Offset: 0x0008E010
	public void MovePivot()
	{
		Vector3 position = this.pivotPoint.transform.position;
		float pivotPointMoveAmount = this.properties.pivotPointMoveAmount;
		if (this.pivotMovingLeft)
		{
			position.x = Mathf.MoveTowards(this.pivotPoint.transform.position.x, this.originalPivotPos.x - pivotPointMoveAmount, this.properties.XAxisSpeed * CupheadTime.FixedDelta * this.hitPauseCoefficient());
			this.pivotMovingLeft = (this.pivotPoint.transform.position.x != this.originalPivotPos.x - pivotPointMoveAmount);
		}
		else
		{
			position.x = Mathf.MoveTowards(this.pivotPoint.transform.position.x, this.originalPivotPos.x + pivotPointMoveAmount, this.properties.XAxisSpeed * CupheadTime.FixedDelta * this.hitPauseCoefficient());
			this.pivotMovingLeft = (this.pivotPoint.transform.position.x == this.originalPivotPos.x + pivotPointMoveAmount);
		}
		this.pivotPoint.transform.position = position;
	}

	// Token: 0x0600105F RID: 4191 RVA: 0x0008FF60 File Offset: 0x0008E160
	public void PathMovement()
	{
		this.angle += this.speed * CupheadTime.FixedDelta * this.hitPauseCoefficient();
		Vector3 vector;
		vector..ctor(-Mathf.Sin(this.angle) * this.loopSize, 0f, 0f);
		Vector3 vector2;
		vector2..ctor(0f, Mathf.Cos(this.angle) * this.loopSize, 0f);
		base.transform.position = this.pivotPoint.position;
		base.transform.position += vector + vector2;
	}

	// Token: 0x06001060 RID: 4192 RVA: 0x00090008 File Offset: 0x0008E208
	public void CheckIfTurn()
	{
		if (this.check)
		{
			if (base.transform.position.y < this.pivotPoint.position.y)
			{
				if (!this.onBottom)
				{
					base.StartCoroutine(this.turn_cr());
					this.check = false;
				}
				this.onBottom = true;
			}
			else
			{
				if (this.onBottom)
				{
					base.StartCoroutine(this.turn_cr());
					this.check = false;
				}
				this.onBottom = false;
			}
		}
	}

	// Token: 0x06001061 RID: 4193 RVA: 0x0009009C File Offset: 0x0008E29C
	public IEnumerator turn_cr()
	{
		base.animator.SetBool("Turn", true);
		yield return base.animator.WaitForAnimationToEnd(this, "Waffle_Turn", false, true);
		base.animator.SetBool("Turn", false);
		this.check = true;
		yield return null;
		yield break;
	}

	// Token: 0x06001062 RID: 4194 RVA: 0x000900B8 File Offset: 0x0008E2B8
	public void Turn()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
	}

	// Token: 0x06001063 RID: 4195 RVA: 0x00090100 File Offset: 0x0008E300
	public IEnumerator check_attack_cr()
	{
		if (!this.isDead)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.attackDelayRange.RandomFloat());
			base.StartCoroutine(this.attack_cr());
			this.state = BaronessLevelWaffle.State.Attack;
			while (this.state == BaronessLevelWaffle.State.Attack)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06001064 RID: 4196 RVA: 0x0009011C File Offset: 0x0008E31C
	public IEnumerator attack_cr()
	{
		if (!this.isDead)
		{
			base.animator.Play("Waffle_Tuck_Start");
			base.GetComponent<Collider2D>().enabled = false;
			float randomValue = (float)Random.Range(0, 2);
			this.diagFirst = (randomValue == 0f);
			yield return CupheadTime.WaitForSeconds(this, this.properties.anticipation);
			base.animator.SetTrigger("Continue");
			base.StartCoroutine(this.waffle_pieces((!this.diagFirst) ? this.straightPieces : this.diagonalPieces, true));
			yield return CupheadTime.WaitForSeconds(this, this.properties.explodeTwoDuration);
			base.StartCoroutine(this.waffle_pieces((!this.diagFirst) ? this.diagonalPieces : this.straightPieces, false));
		}
		yield break;
	}

	// Token: 0x06001065 RID: 4197 RVA: 0x00090138 File Offset: 0x0008E338
	public void hitPause(int i)
	{
		if (this.diagonalPieces[i].wafflepiece.GetComponent<DamageReceiver>().IsHitPaused || this.straightPieces[i].wafflepiece.GetComponent<DamageReceiver>().IsHitPaused)
		{
			BaronessLevelWaffle.pauseValue = 0f;
		}
		else
		{
			BaronessLevelWaffle.pauseValue = 1f;
		}
	}

	// Token: 0x06001066 RID: 4198 RVA: 0x00090198 File Offset: 0x0008E398
	public IEnumerator waffle_pieces(BaronessLevelWaffle.WafflePieces[] pieces, bool isFirst)
	{
		float t = 0f;
		float explodeTime = this.properties.explodeSpeed;
		float returnTime = this.properties.explodeReturnSpeed;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (!this.switchedOn)
		{
			yield return null;
		}
		foreach (BaronessLevelWaffle.WafflePieces wafflePieces in pieces)
		{
			wafflePieces.wafflepiece.GetComponent<Collider2D>().enabled = true;
			wafflePieces.waffleFX.Play("Trail", 0, Random.Range(0f, 0.6f));
		}
		if (isFirst)
		{
			this.explosion.Create(new Vector2(this.mouth.position.x, this.mouth.position.y - 20f));
		}
		while (t < explodeTime)
		{
			for (int j = 0; j < pieces.Length; j++)
			{
				t += CupheadTime.FixedDelta;
				float num = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, t / explodeTime);
				pieces[j].wafflepiece.transform.localPosition = Vector3.Lerp(pieces[j].wafflepiece.transform.localPosition.normalized, pieces[j].direction * this.properties.explodeDistance, num * BaronessLevelWaffle.pauseValue);
				this.hitPause(j);
			}
			yield return wait;
		}
		t = 0f;
		foreach (BaronessLevelWaffle.WafflePieces wafflePieces2 in pieces)
		{
			wafflePieces2.wafflepiece.GetComponent<Collider2D>().enabled = true;
			wafflePieces2.waffleFX.SetTrigger("Death");
		}
		if (isFirst)
		{
			this.explosionReverse.Create(new Vector2(this.mouth.position.x, this.mouth.position.y - 20f));
		}
		while (t < returnTime / 2f)
		{
			for (int l = 0; l < pieces.Length; l++)
			{
				t += CupheadTime.FixedDelta;
				float num2 = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / returnTime);
				pieces[l].wafflepiece.transform.localPosition = Vector3.Lerp(pieces[l].wafflepiece.transform.localPosition, Vector3.zero, num2 * BaronessLevelWaffle.pauseValue);
				this.hitPause(l);
			}
			yield return wait;
		}
		for (int m = 0; m < pieces.Length; m++)
		{
			pieces[m].wafflepiece.GetComponent<Collider2D>().enabled = false;
			pieces[m].wafflepiece.localPosition = Vector3.zero;
		}
		yield return null;
		if (!isFirst)
		{
			base.animator.SetBool("Split", false);
			base.GetComponent<Collider2D>().enabled = true;
			yield return base.animator.WaitForAnimationToEnd(this, "Waffle_Return", false, true);
			this.state = BaronessLevelWaffle.State.Move;
			this.switchedOn = false;
			yield return base.StartCoroutine(this.check_attack_cr());
		}
		yield break;
	}

	// Token: 0x06001067 RID: 4199 RVA: 0x0000DD1E File Offset: 0x0000BF1E
	public void switchAnimation()
	{
		this.switchedOn = true;
		base.animator.SetBool("Split", true);
	}

	// Token: 0x06001068 RID: 4200 RVA: 0x000901C4 File Offset: 0x0008E3C4
	public IEnumerator destroyMouth_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Waffle_Explode_Death", false, true);
		this.mouth.GetComponent<Collider2D>().enabled = false;
		yield break;
	}

	// Token: 0x06001069 RID: 4201 RVA: 0x000901E0 File Offset: 0x0008E3E0
	public IEnumerator death_cr()
	{
		this.pivotPoint.transform.position = this.originalPivotPos;
		YieldInstruction wait = new WaitForFixedUpdate();
		this.StartExplosions();
		Collider2D collider = base.GetComponent<Collider2D>();
		this.isDead = true;
		this.state = BaronessLevelWaffle.State.Dying;
		this.isDying = true;
		base.animator.SetTrigger("Death");
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.animator.SetBool("DeathExplode", true);
		bool explodeDeath = true;
		float untilDestroy = 1500f;
		base.StartCoroutine(this.destroyMouth_cr());
		while (explodeDeath)
		{
			collider.enabled = false;
			for (int i = 0; i < this.diagonalPieces.Length; i++)
			{
				this.diagonalPieces[i].distanceFromCenter = Vector3.Distance(this.diagonalPieces[i].wafflepiece.transform.localPosition, this.mouth.transform.localPosition);
				this.diagonalPieces[i].wafflepiece.GetComponent<Collider2D>().enabled = false;
				this.diagonalPieces[i].wafflepiece.transform.localPosition += this.diagonalPieces[i].direction * 700f * CupheadTime.FixedDelta;
				if (this.diagonalPieces[i].distanceFromCenter >= untilDestroy)
				{
					explodeDeath = false;
					break;
				}
			}
			for (int j = 0; j < this.straightPieces.Length; j++)
			{
				this.straightPieces[j].distanceFromCenter = Vector3.Distance(this.straightPieces[j].wafflepiece.transform.localPosition, this.mouth.transform.localPosition);
				this.straightPieces[j].wafflepiece.GetComponent<Collider2D>().enabled = false;
				this.straightPieces[j].wafflepiece.transform.localPosition += this.straightPieces[j].direction * 700f * CupheadTime.FixedDelta;
				if (this.straightPieces[j].distanceFromCenter >= untilDestroy)
				{
					explodeDeath = false;
					break;
				}
			}
			yield return wait;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x0600106A RID: 4202 RVA: 0x0000DD38 File Offset: 0x0000BF38
	public void SoundWaffleExplode()
	{
		AudioManager.Play("level_baroness_waffle_explode");
		this.emitAudioFromObject.Add("level_baroness_waffle_explode");
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x0000DD54 File Offset: 0x0000BF54
	public void SoundWaffleWingflap()
	{
		AudioManager.Play("level_baroness_waffle_wingflap");
		this.emitAudioFromObject.Add("level_baroness_waffle_wingflap");
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x0000DD70 File Offset: 0x0000BF70
	public void SoundWaffleReform()
	{
		AudioManager.Play("level_baroness_waffle_reform");
		this.emitAudioFromObject.Add("level_baroness_waffle_reform");
	}

	// Token: 0x04000D42 RID: 3394
	public static float pauseValue;

	// Token: 0x04000D44 RID: 3396
	[SerializeField]
	public Effect explosion;

	// Token: 0x04000D45 RID: 3397
	[SerializeField]
	public Effect explosionReverse;

	// Token: 0x04000D46 RID: 3398
	[SerializeField]
	public BaronessLevelWaffle.WafflePieces[] diagonalPieces;

	// Token: 0x04000D47 RID: 3399
	[SerializeField]
	public BaronessLevelWaffle.WafflePieces[] straightPieces;

	// Token: 0x04000D48 RID: 3400
	[SerializeField]
	public Transform mouth;

	// Token: 0x04000D49 RID: 3401
	public LevelProperties.Baroness.Waffle properties;

	// Token: 0x04000D4A RID: 3402
	public Transform pivotPoint;

	// Token: 0x04000D4B RID: 3403
	public DamageDealer damageDealer;

	// Token: 0x04000D4C RID: 3404
	public DamageReceiver damageReceiver;

	// Token: 0x04000D4D RID: 3405
	public float health;

	// Token: 0x04000D4E RID: 3406
	public float speed;

	// Token: 0x04000D4F RID: 3407
	public float angle;

	// Token: 0x04000D50 RID: 3408
	public float loopSize = 200f;

	// Token: 0x04000D51 RID: 3409
	public bool switchedOn;

	// Token: 0x04000D52 RID: 3410
	public bool pathA;

	// Token: 0x04000D53 RID: 3411
	public bool check;

	// Token: 0x04000D54 RID: 3412
	public bool onBottom;

	// Token: 0x04000D55 RID: 3413
	public bool diagFirst;

	// Token: 0x04000D56 RID: 3414
	public bool isDead;

	// Token: 0x04000D57 RID: 3415
	public bool pivotMovingLeft;

	// Token: 0x04000D58 RID: 3416
	public bool isHitPaused;

	// Token: 0x04000D59 RID: 3417
	public Vector3 startPos;

	// Token: 0x04000D5A RID: 3418
	public Vector3 originalPivotPos;

	// Token: 0x02000A37 RID: 2615
	public enum State
	{
		// Token: 0x04004B56 RID: 19286
		Enter,
		// Token: 0x04004B57 RID: 19287
		Move,
		// Token: 0x04004B58 RID: 19288
		Attack,
		// Token: 0x04004B59 RID: 19289
		Dying
	}

	// Token: 0x02000A38 RID: 2616
	[Serializable]
	public class WafflePieces
	{
		// Token: 0x04004B5A RID: 19290
		public Transform wafflepiece;

		// Token: 0x04004B5B RID: 19291
		public Animator waffleFX;

		// Token: 0x04004B5C RID: 19292
		public Vector3 direction;

		// Token: 0x04004B5D RID: 19293
		public float distanceFromCenter;

		// Token: 0x04004B5E RID: 19294
		public DamageDealer damageDealer;
	}
}
