using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003DC RID: 988
public class PlatformingLevelPathMovementEnemy : AbstractPlatformingLevelEnemy
{
	// Token: 0x1700034B RID: 843
	// (get) Token: 0x06002BA3 RID: 11171 RVA: 0x00024A0F File Offset: 0x00022C0F
	// (set) Token: 0x06002BA4 RID: 11172 RVA: 0x00024A17 File Offset: 0x00022C17
	public float[] allValues { get; set; }

	// Token: 0x06002BA5 RID: 11173 RVA: 0x000D72E8 File Offset: 0x000D54E8
	public PlatformingLevelPathMovementEnemy Spawn(Vector3 position, VectorPath path, float startPosition, bool destroyEnemyAfterLeavingScreen)
	{
		PlatformingLevelPathMovementEnemy platformingLevelPathMovementEnemy = this.InstantiatePrefab<PlatformingLevelPathMovementEnemy>();
		platformingLevelPathMovementEnemy.transform.position = position;
		platformingLevelPathMovementEnemy.startPosition = startPosition;
		platformingLevelPathMovementEnemy.path = path;
		platformingLevelPathMovementEnemy._destroyEnemyAfterLeavingScreen = destroyEnemyAfterLeavingScreen;
		platformingLevelPathMovementEnemy._startCondition = AbstractPlatformingLevelEnemy.StartCondition.Instant;
		return platformingLevelPathMovementEnemy;
	}

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x06002BA6 RID: 11174 RVA: 0x00024A20 File Offset: 0x00022C20
	public virtual SpriteRenderer spriteRenderer
	{
		get
		{
			return this._spriteRenderer;
		}
	}

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x06002BA7 RID: 11175 RVA: 0x00024A28 File Offset: 0x00022C28
	public virtual Collider2D collider
	{
		get
		{
			return this._collider;
		}
	}

	// Token: 0x06002BA8 RID: 11176 RVA: 0x000D7328 File Offset: 0x000D5528
	public override void Start()
	{
		base.Start();
		this._offset = base.transform.position;
		this.MoveCallback(this.startPosition);
		this._collider = base.GetComponent<Collider2D>();
		this._spriteRenderer = base.GetComponent<SpriteRenderer>();
		if (base.Properties.MoveLoopMode == EnemyProperties.LoopMode.DelayAtPoint)
		{
			this.SetUp();
		}
	}

	// Token: 0x06002BA9 RID: 11177 RVA: 0x000D7388 File Offset: 0x000D5588
	public override void OnStart()
	{
		this.hasStarted = true;
		switch (base.Properties.MoveLoopMode)
		{
		case EnemyProperties.LoopMode.PingPong:
			base.StartCoroutine(this.pingpong_cr());
			break;
		case EnemyProperties.LoopMode.Repeat:
			base.StartCoroutine(this.repeat_cr());
			break;
		case EnemyProperties.LoopMode.Once:
			base.StartCoroutine(this.once_cr());
			break;
		case EnemyProperties.LoopMode.DelayAtPoint:
			base.StartCoroutine(this.delay_at_point_cr());
			break;
		}
	}

	// Token: 0x06002BAA RID: 11178 RVA: 0x00024A30 File Offset: 0x00022C30
	public override void Update()
	{
		base.Update();
		this.CalculateCollider();
		this.CalculateDirection();
		this.CalculateRender();
	}

	// Token: 0x06002BAB RID: 11179 RVA: 0x000D7410 File Offset: 0x000D5610
	public void CalculateRender()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position) && !this._enteredScreen)
		{
			this._enteredScreen = true;
		}
		if (this._enteredScreen && this._destroyEnemyAfterLeavingScreen && !CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 100f)))
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06002BAC RID: 11180 RVA: 0x00024A4A File Offset: 0x00022C4A
	public void LateUpdate()
	{
		this.CalculateCollider();
		this.CalculateDirection();
	}

	// Token: 0x06002BAD RID: 11181 RVA: 0x000D74A0 File Offset: 0x000D56A0
	public virtual void CalculateCollider()
	{
		if (this.collider == null || this.spriteRenderer == null || base.Dead)
		{
			return;
		}
		if (this.spriteRenderer.isVisible)
		{
			this.collider.enabled = true;
		}
		else
		{
			this.collider.enabled = false;
		}
	}

	// Token: 0x06002BAE RID: 11182 RVA: 0x00024A58 File Offset: 0x00022C58
	public void CalculateDirection()
	{
		if (this._direction == PlatformingLevelPathMovementEnemy.Direction.Forward && this._hasFacingDirection)
		{
			this.spriteRenderer.flipX = true;
		}
		else
		{
			this.spriteRenderer.flipX = false;
		}
	}

	// Token: 0x06002BAF RID: 11183 RVA: 0x00024A8E File Offset: 0x00022C8E
	public void MoveCallback(float value)
	{
		base.transform.position = this._offset + this.path.Lerp(value);
	}

	// Token: 0x06002BB0 RID: 11184 RVA: 0x000D7508 File Offset: 0x000D5708
	public float CalculateRemainingTime(float t, PlatformingLevelPathMovementEnemy.Direction d)
	{
		float num = this.CalculateTime();
		return (d != PlatformingLevelPathMovementEnemy.Direction.Forward) ? (t * num) : ((1f - t) * num);
	}

	// Token: 0x06002BB1 RID: 11185 RVA: 0x000D7534 File Offset: 0x000D5734
	public float CalculateTime()
	{
		return this.path.Distance / base.Properties.MoveSpeed;
	}

	// Token: 0x06002BB2 RID: 11186 RVA: 0x000D755C File Offset: 0x000D575C
	public float CalculatePartTime(int current, int next)
	{
		return Vector3.Distance(this.path.Points[current], this.path.Points[next]) / base.Properties.MoveSpeed;
	}

	// Token: 0x06002BB3 RID: 11187 RVA: 0x00024AB2 File Offset: 0x00022CB2
	public Coroutine Turn()
	{
		return base.StartCoroutine(this.turn_cr());
	}

	// Token: 0x06002BB4 RID: 11188 RVA: 0x000D75A0 File Offset: 0x000D57A0
	public IEnumerator turn_cr()
	{
		if (this._hasTurnAnimation && base.animator != null)
		{
			base.animator.Play("Turn");
			yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
		}
		if (this._direction == PlatformingLevelPathMovementEnemy.Direction.Forward)
		{
			this._direction = PlatformingLevelPathMovementEnemy.Direction.Back;
		}
		else
		{
			this._direction = PlatformingLevelPathMovementEnemy.Direction.Forward;
		}
		yield break;
	}

	// Token: 0x06002BB5 RID: 11189 RVA: 0x000D75BC File Offset: 0x000D57BC
	public IEnumerator pingpong_cr()
	{
		if (this._direction == PlatformingLevelPathMovementEnemy.Direction.Back)
		{
			yield return base.TweenValue(this.startPosition, 0f, this.CalculateRemainingTime(this.startPosition, PlatformingLevelPathMovementEnemy.Direction.Back), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			yield return CupheadTime.WaitForSeconds(this, this.loopRepeatDelay);
			yield return this.Turn();
		}
		else
		{
			yield return base.TweenValue(this.startPosition, 1f, this.CalculateRemainingTime(this.startPosition, PlatformingLevelPathMovementEnemy.Direction.Forward), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			yield return CupheadTime.WaitForSeconds(this, this.loopRepeatDelay);
			yield return this.Turn();
			yield return base.TweenValue(1f, 0f, this.CalculateTime(), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			yield return CupheadTime.WaitForSeconds(this, this.loopRepeatDelay);
			yield return this.Turn();
		}
		for (;;)
		{
			yield return base.TweenValue(0f, 1f, this.CalculateTime(), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			yield return CupheadTime.WaitForSeconds(this, this.loopRepeatDelay);
			yield return this.Turn();
			yield return base.TweenValue(1f, 0f, this.CalculateTime(), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			yield return CupheadTime.WaitForSeconds(this, this.loopRepeatDelay);
			yield return this.Turn();
		}
		yield break;
	}

	// Token: 0x06002BB6 RID: 11190 RVA: 0x000D75D8 File Offset: 0x000D57D8
	public IEnumerator repeat_cr()
	{
		float start = 0f;
		float end = 1f;
		if (this._direction == PlatformingLevelPathMovementEnemy.Direction.Back)
		{
			start = 1f;
			end = 0f;
		}
		yield return base.TweenValue(this.startPosition, end, this.CalculateRemainingTime(this.startPosition, this._direction), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
		for (;;)
		{
			yield return base.TweenValue(start, end, this.CalculateTime(), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			yield return CupheadTime.WaitForSeconds(this, this.loopRepeatDelay);
		}
		yield break;
	}

	// Token: 0x06002BB7 RID: 11191 RVA: 0x000D75F4 File Offset: 0x000D57F4
	public void SetUp()
	{
		this._easeType = EaseUtils.EaseType.linear;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		this.allValues = new float[this.path.Points.Count];
		for (int i = 0; i < this.path.Points.Count; i++)
		{
			int index = (i != 0) ? (i - 1) : 0;
			float num4 = this.path.Points[i].y - this.path.Points[index].y;
			float num5 = this.path.Points[i].x - this.path.Points[index].x;
			float num6 = Mathf.Pow(num4, 2f);
			float num7 = Mathf.Pow(num5, 2f);
			float num8 = num6 + num7;
			num3 += Mathf.Sqrt(num8);
		}
		for (int j = 0; j < this.path.Points.Count; j++)
		{
			num2 += num;
			int index2 = (j != 0) ? (j - 1) : 0;
			float num9 = this.path.Points[j].y - this.path.Points[index2].y;
			float num10 = this.path.Points[j].x - this.path.Points[index2].x;
			float num11 = Mathf.Pow(num9, 2f);
			float num12 = Mathf.Pow(num10, 2f);
			float num13 = num11 + num12;
			num = Mathf.Sqrt(num13);
			this.allValues[j] = (num2 + num) / num3;
		}
	}

	// Token: 0x06002BB8 RID: 11192 RVA: 0x000D77EC File Offset: 0x000D59EC
	public IEnumerator delay_at_point_cr()
	{
		float prevVal = this.startPosition;
		while (this.hasStarted)
		{
			yield return base.TweenValue(prevVal, this.allValues[this.pathIndex], this.CalculatePartTime(this.pathIndex - 1, this.pathIndex), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			yield return null;
			if (this._hasTurnAnimation)
			{
				base.animator.SetTrigger("Turn");
				yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, this.loopRepeatDelay);
			}
			yield return null;
			if (this.pathIndex == this.path.Points.Count - 1)
			{
				break;
			}
			prevVal = this.allValues[this.pathIndex];
			this.pathIndex++;
			yield return null;
		}
		this.EndPath();
		yield return null;
		yield break;
	}

	// Token: 0x06002BB9 RID: 11193 RVA: 0x00024AC0 File Offset: 0x00022CC0
	public virtual void EndPath()
	{
	}

	// Token: 0x06002BBA RID: 11194 RVA: 0x000D7808 File Offset: 0x000D5A08
	public IEnumerator once_cr()
	{
		if (this._direction == PlatformingLevelPathMovementEnemy.Direction.Back)
		{
			yield return base.TweenValue(this.startPosition, 0f, this.CalculateRemainingTime(this.startPosition, PlatformingLevelPathMovementEnemy.Direction.Back), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			this.Die();
		}
		else
		{
			yield return base.TweenValue(this.startPosition, 1f, this.CalculateRemainingTime(this.startPosition, PlatformingLevelPathMovementEnemy.Direction.Forward), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			this.Die();
		}
		yield break;
	}

	// Token: 0x06002BBB RID: 11195 RVA: 0x00024AC2 File Offset: 0x00022CC2
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06002BBC RID: 11196 RVA: 0x00024AD5 File Offset: 0x00022CD5
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06002BBD RID: 11197 RVA: 0x000D7824 File Offset: 0x000D5A24
	public new void DrawGizmos(float a)
	{
		if (Application.isPlaying)
		{
			this.path.DrawGizmos(a, this._offset);
			return;
		}
		this.path.DrawGizmos(a, base.baseTransform.position);
		Gizmos.color = new Color(1f, 0f, 0f, a);
		Gizmos.DrawSphere(this.path.Lerp(this.startPosition) + base.baseTransform.position, 10f);
		Gizmos.DrawWireSphere(this.path.Lerp(this.startPosition) + base.baseTransform.position, 11f);
	}

	// Token: 0x0400242D RID: 9261
	public int pathIndex;

	// Token: 0x0400242E RID: 9262
	public const float SCREEN_PADDING = 100f;

	// Token: 0x0400242F RID: 9263
	public float loopRepeatDelay;

	// Token: 0x04002430 RID: 9264
	public float startPosition = 0.5f;

	// Token: 0x04002431 RID: 9265
	public VectorPath path;

	// Token: 0x04002432 RID: 9266
	[SerializeField]
	public PlatformingLevelPathMovementEnemy.Direction _direction = PlatformingLevelPathMovementEnemy.Direction.Forward;

	// Token: 0x04002433 RID: 9267
	[SerializeField]
	public bool _hasTurnAnimation;

	// Token: 0x04002434 RID: 9268
	[SerializeField]
	public bool _hasFacingDirection;

	// Token: 0x04002435 RID: 9269
	[SerializeField]
	public EaseUtils.EaseType _easeType = EaseUtils.EaseType.linear;

	// Token: 0x04002436 RID: 9270
	public Vector3 _offset;

	// Token: 0x04002437 RID: 9271
	public bool hasStarted;

	// Token: 0x04002438 RID: 9272
	public SpriteRenderer _spriteRenderer;

	// Token: 0x04002439 RID: 9273
	public Collider2D _collider;

	// Token: 0x0400243A RID: 9274
	public bool _destroyEnemyAfterLeavingScreen;

	// Token: 0x0400243B RID: 9275
	public bool _enteredScreen;

	// Token: 0x02001007 RID: 4103
	public enum Direction
	{
		// Token: 0x040072A5 RID: 29349
		Forward = 1,
		// Token: 0x040072A6 RID: 29350
		Back = -1
	}
}
