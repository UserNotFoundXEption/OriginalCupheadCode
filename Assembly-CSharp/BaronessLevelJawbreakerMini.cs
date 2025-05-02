using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000152 RID: 338
public class BaronessLevelJawbreakerMini : BaronessLevelMiniBossBase
{
	// Token: 0x17000239 RID: 569
	// (get) Token: 0x06001033 RID: 4147 RVA: 0x0000DB0A File Offset: 0x0000BD0A
	// (set) Token: 0x06001034 RID: 4148 RVA: 0x0000DB12 File Offset: 0x0000BD12
	public BaronessLevelJawbreakerMini.State state { get; set; }

	// Token: 0x06001035 RID: 4149 RVA: 0x0000DB1B File Offset: 0x0000BD1B
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.aim = base.transform;
		this.rotateDeath = false;
		this.bigPath = new List<Vector3>();
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x0008F644 File Offset: 0x0008D844
	public override void Start()
	{
		base.Start();
		this.layerSwitch = 3;
		this.fadeTime = 2f;
		this.sprite.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Background.ToString();
		this.sprite.GetComponent<SpriteRenderer>().sortingOrder = 150;
		base.StartCoroutine(this.check_rotation_cr());
		base.StartCoroutine(this.switch_cr());
	}

	// Token: 0x06001037 RID: 4151 RVA: 0x0000DB4C File Offset: 0x0000BD4C
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001038 RID: 4152 RVA: 0x0008F6B8 File Offset: 0x0008D8B8
	public void Init(LevelProperties.Baroness.Jawbreaker properties, Vector2 pos, Transform targetPos, float rotationSpeed)
	{
		this.properties = properties;
		this.rotationSpeed = rotationSpeed;
		base.transform.position = pos;
		this.targetPosition = targetPos;
		this.state = BaronessLevelJawbreakerMini.State.Spawned;
		base.StartCoroutine(this.blink_cr());
		base.StartCoroutine(this.calculate_path_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001039 RID: 4153 RVA: 0x0000DB6A File Offset: 0x0000BD6A
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x0008F71C File Offset: 0x0008D91C
	public void FixedUpdate()
	{
		if (this.state == BaronessLevelJawbreakerMini.State.Spawned)
		{
			if (this.bigPath.Count != 0)
			{
				float num = this.pathLength / this.properties.jawbreakerMiniSpace;
				base.transform.position -= base.transform.right * (num * this.properties.jawbreakerHomingSpeed) * CupheadTime.FixedDelta;
				this.pathLength = 0f;
				this.aim.LookAt2D(2f * base.transform.position - this.bigPath[0]);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.aim.rotation, this.rotationSpeed * CupheadTime.FixedDelta);
				this.sprite.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
				float num2 = Vector3.Distance(base.transform.position, this.bigPath[0]);
				if (num2 < this.properties.jawbreakerHomingSpeed / 4f)
				{
					this.bigPath.Remove(this.bigPath[0]);
				}
			}
			if (this.state == BaronessLevelJawbreakerMini.State.Dying && this.rotateDeath)
			{
				this.RotateExplode();
				this.rotateDeath = false;
			}
		}
	}

	// Token: 0x0600103B RID: 4155 RVA: 0x0008F8A4 File Offset: 0x0008DAA4
	public IEnumerator switch_cr()
	{
		base.StartCoroutine(this.fade_color_cr());
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.sprite.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Enemies.ToString();
		this.sprite.GetComponent<SpriteRenderer>().sortingOrder = 250;
		yield break;
	}

	// Token: 0x0600103C RID: 4156 RVA: 0x0008F8C0 File Offset: 0x0008DAC0
	public void Turn()
	{
		this.sprite.transform.SetScale(new float?(-this.sprite.transform.localScale.x), new float?(1f), new float?(1f));
	}

	// Token: 0x0600103D RID: 4157 RVA: 0x0008F910 File Offset: 0x0008DB10
	public IEnumerator check_rotation_cr()
	{
		for (;;)
		{
			if (((this.targetPosition.transform.position.x < base.transform.position.x && !this.lookingLeft) || (this.targetPosition.transform.position.x > base.transform.position.x && this.lookingLeft)) && !this.isTurning)
			{
				this.isTurning = true;
				base.animator.SetTrigger("Turn");
				yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
				this.lookingLeft = !this.lookingLeft;
				this.isTurning = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600103E RID: 4158 RVA: 0x0008F92C File Offset: 0x0008DB2C
	public IEnumerator move_cr()
	{
		for (;;)
		{
			this.pathLength = 0f;
			for (int i = 0; i < this.bigPath.Count - 1; i++)
			{
				this.pathLength += Vector3.Distance(this.bigPath[i], this.bigPath[i + 1]);
				if (CupheadTime.Delta == 0f)
				{
					yield return null;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600103F RID: 4159 RVA: 0x0008F948 File Offset: 0x0008DB48
	public IEnumerator calculate_path_cr()
	{
		for (;;)
		{
			for (int i = 0; i < this.positionsInList; i++)
			{
				if (!Mathf.Approximately(CupheadTime.Delta, 0f))
				{
					this.bigPath.Add(this.targetPosition.transform.position);
				}
				yield return new WaitForFixedUpdate();
			}
		}
		yield break;
	}

	// Token: 0x06001040 RID: 4160 RVA: 0x0000DB82 File Offset: 0x0000BD82
	public void Stop()
	{
		this.state = BaronessLevelJawbreakerMini.State.Dying;
		base.StopCoroutine(this.blink_cr());
		base.StopCoroutine(this.calculate_path_cr());
	}

	// Token: 0x06001041 RID: 4161 RVA: 0x0000DBA3 File Offset: 0x0000BDA3
	public void StartDying()
	{
		base.StartCoroutine(this.dying_cr());
	}

	// Token: 0x06001042 RID: 4162 RVA: 0x0008F964 File Offset: 0x0008DB64
	public void RotateExplode()
	{
		float num = (float)Random.Range(0, 360);
		base.transform.rotation = Quaternion.Euler(0f, 0f, num);
	}

	// Token: 0x06001043 RID: 4163 RVA: 0x0008F99C File Offset: 0x0008DB9C
	public IEnumerator dying_cr()
	{
		Collider2D collider = base.GetComponent<Collider2D>();
		collider.enabled = false;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.animator.SetTrigger("Dead");
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		this.rotateDeath = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Death_End", false, true);
		this.KillMini();
		yield break;
	}

	// Token: 0x06001044 RID: 4164 RVA: 0x0008F9B8 File Offset: 0x0008DBB8
	public IEnumerator blink_cr()
	{
		while (this.state == BaronessLevelJawbreakerMini.State.Spawned)
		{
			base.animator.SetTrigger("Blink");
			int timeBetweenNext = Random.Range(2, 4);
			yield return CupheadTime.WaitForSeconds(this, (float)timeBetweenNext);
		}
		yield break;
	}

	// Token: 0x06001045 RID: 4165 RVA: 0x0008F9D4 File Offset: 0x0008DBD4
	public void KillMini()
	{
		this.state = BaronessLevelJawbreakerMini.State.Unspawned;
		Collider2D component = base.GetComponent<Collider2D>();
		component.enabled = false;
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000D2B RID: 3371
	public const float POSITION_FRAME_TIME = 0.0833333358f;

	// Token: 0x04000D2C RID: 3372
	[SerializeField]
	public Transform sprite;

	// Token: 0x04000D2E RID: 3374
	public float rotationSpeed;

	// Token: 0x04000D2F RID: 3375
	public float pathLength;

	// Token: 0x04000D30 RID: 3376
	public int positionsInList = 12;

	// Token: 0x04000D31 RID: 3377
	public bool rotateDeath;

	// Token: 0x04000D32 RID: 3378
	public bool lookingLeft = true;

	// Token: 0x04000D33 RID: 3379
	public bool isTurning;

	// Token: 0x04000D34 RID: 3380
	public DamageDealer damageDealer;

	// Token: 0x04000D35 RID: 3381
	public LevelProperties.Baroness.Jawbreaker properties;

	// Token: 0x04000D36 RID: 3382
	public Vector3 currentPos;

	// Token: 0x04000D37 RID: 3383
	public Transform targetPosition;

	// Token: 0x04000D38 RID: 3384
	public Transform aim;

	// Token: 0x04000D39 RID: 3385
	public List<Vector3> bigPath;

	// Token: 0x04000D3A RID: 3386
	public Quaternion rotate;

	// Token: 0x02000A2D RID: 2605
	public enum State
	{
		// Token: 0x04004B2C RID: 19244
		Unspawned,
		// Token: 0x04004B2D RID: 19245
		Spawned,
		// Token: 0x04004B2E RID: 19246
		Dying
	}
}
