using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200016C RID: 364
public class BeeLevelQueenTriangle : AbstractProjectile
{
	// Token: 0x0600117D RID: 4477 RVA: 0x000927A8 File Offset: 0x000909A8
	public BeeLevelQueenTriangle Create(BeeLevelQueenTriangle.Properties properties)
	{
		BeeLevelQueenTriangle beeLevelQueenTriangle = base.Create() as BeeLevelQueenTriangle;
		beeLevelQueenTriangle.transform.position = properties.player.center;
		beeLevelQueenTriangle.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?((float)Random.Range(0, 360)));
		beeLevelQueenTriangle.properties = properties;
		return beeLevelQueenTriangle;
	}

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x0600117E RID: 4478 RVA: 0x0000ED49 File Offset: 0x0000CF49
	public override float DestroyLifetime
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x0600117F RID: 4479 RVA: 0x00092810 File Offset: 0x00090A10
	public override void Awake()
	{
		base.Awake();
		if (!this.isInvincible)
		{
			this.damageReceiver = base.GetComponent<DamageReceiver>();
			this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		}
		AudioManager.Play("bee_queen_triangle_spawn");
		this.emitAudioFromObject.Add("bee_queen_triangle_spawn");
		AudioManager.PlayLoop("bee_queen_triangle_loop");
		this.emitAudioFromObject.Add("bee_queen_triangle_loop");
	}

	// Token: 0x06001180 RID: 4480 RVA: 0x0000ED50 File Offset: 0x0000CF50
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001181 RID: 4481 RVA: 0x0000ED6E File Offset: 0x0000CF6E
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06001182 RID: 4482 RVA: 0x0000ED83 File Offset: 0x0000CF83
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (!this.isInvincible)
		{
			this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		}
		this.childPrefab = null;
		this.childPrefabInvincible = null;
	}

	// Token: 0x06001183 RID: 4483 RVA: 0x0000EDBB File Offset: 0x0000CFBB
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.properties.health -= info.damage;
		if (this.properties.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06001184 RID: 4484 RVA: 0x0000EDF0 File Offset: 0x0000CFF0
	public override void Die()
	{
		base.Die();
		AudioManager.Stop("bee_queen_triangle_loop");
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
	}

	// Token: 0x06001185 RID: 4485 RVA: 0x00092888 File Offset: 0x00090A88
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (this.properties != null)
		{
			base.transform.AddPosition(this.forward.x * CupheadTime.Delta * this.properties.speed, this.forward.y * CupheadTime.Delta * this.properties.speed, 0f);
			base.transform.AddEulerAngles(0f, 0f, this.properties.rotationSpeed * (float)this.properties.direction * CupheadTime.Delta);
		}
	}

	// Token: 0x06001186 RID: 4486 RVA: 0x00092948 File Offset: 0x00090B48
	public IEnumerator go_cr()
	{
		base.transform.GetComponent<Collider2D>().enabled = false;
		Transform aim = new GameObject("Aim").transform;
		aim.SetParent(base.transform);
		aim.ResetLocalTransforms();
		yield return base.StartCoroutine(this.tweenColor_cr(new Color(0f, 0f, 0f, 0f), new Color(0f, 0f, 0f, 1f), this.properties.introTime / 2f));
		yield return base.StartCoroutine(this.tweenColor_cr(new Color(0f, 0f, 0f, 1f), new Color(1f, 1f, 1f, 1f), this.properties.introTime / 2f));
		aim.LookAt2D(this.properties.player.center);
		this.forward = aim.transform.right;
		base.transform.GetComponent<Collider2D>().enabled = true;
		base.StartCoroutine(this.shoot_cr());
		float t = 0f;
		while (t < 1f)
		{
			float val = t / 1f;
			this.properties.speed = Mathf.Lerp(0f, this.properties.speedMax, val);
			this.properties.rotationSpeed = Mathf.Lerp(0f, this.properties.rotationSpeedMax, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.properties.speed = this.properties.speedMax;
		this.properties.rotationSpeed = this.properties.rotationSpeedMax;
		yield break;
	}

	// Token: 0x06001187 RID: 4487 RVA: 0x00092964 File Offset: 0x00090B64
	public IEnumerator shoot_cr()
	{
		int count = 0;
		while (count < this.properties.childCount)
		{
			AudioManager.Play("bee_queen_triangle_shoot");
			this.emitAudioFromObject.Add("bee_queen_triangle_shoot");
			foreach (Transform transform in this.roots)
			{
				if (this.properties.damageable)
				{
					this.childPrefab.Create(transform.position, transform.eulerAngles.z, this.properties.childSpeed, this.properties.childHealth).SetParryable(true);
				}
				else
				{
					this.childPrefabInvincible.Create(transform.position, transform.eulerAngles.z, this.properties.childSpeed).SetParryable(true);
				}
			}
			base.animator.Play("Attack");
			count++;
			yield return CupheadTime.WaitForSeconds(this, this.properties.childDelay);
		}
		base.animator.Play("Idle");
		yield break;
	}

	// Token: 0x06001188 RID: 4488 RVA: 0x00092980 File Offset: 0x00090B80
	public IEnumerator tweenColor_cr(Color start, Color end, float time)
	{
		SpriteRenderer r = base.GetComponent<SpriteRenderer>();
		r.color = start;
		yield return null;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			r.color = Color.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		r.color = end;
		yield return null;
		yield break;
	}

	// Token: 0x04000E15 RID: 3605
	[SerializeField]
	public bool isInvincible;

	// Token: 0x04000E16 RID: 3606
	[SerializeField]
	public Transform[] roots;

	// Token: 0x04000E17 RID: 3607
	[SerializeField]
	public BasicDamagableProjectile childPrefab;

	// Token: 0x04000E18 RID: 3608
	[SerializeField]
	public BasicProjectile childPrefabInvincible;

	// Token: 0x04000E19 RID: 3609
	public BeeLevelQueenTriangle.Properties properties;

	// Token: 0x04000E1A RID: 3610
	public Vector2 forward;

	// Token: 0x04000E1B RID: 3611
	public DamageReceiver damageReceiver;

	// Token: 0x02000A83 RID: 2691
	public class Properties
	{
		// Token: 0x06005A65 RID: 23141 RVA: 0x001DDFCC File Offset: 0x001DC1CC
		public Properties(AbstractPlayerController player, float introTime, float speed, float rotationSpeed, float health, float childSpeed, float childDelay, float childHealth, int childCount, bool damageable)
		{
			this.player = player;
			this.damageable = damageable;
			this.introTime = introTime;
			this.speedMax = speed;
			this.rotationSpeedMax = rotationSpeed;
			this.healthMax = health;
			this.childSpeed = childSpeed;
			this.childDelay = childDelay;
			this.childHealth = childHealth;
			this.childCount = childCount;
			this.direction = MathUtils.PlusOrMinus();
			this.speed = 0f;
			this.rotationSpeed = 0f;
			this.health = health;
		}

		// Token: 0x04004D3E RID: 19774
		public readonly AbstractPlayerController player;

		// Token: 0x04004D3F RID: 19775
		public readonly bool damageable;

		// Token: 0x04004D40 RID: 19776
		public readonly float introTime;

		// Token: 0x04004D41 RID: 19777
		public readonly float speedMax;

		// Token: 0x04004D42 RID: 19778
		public readonly float rotationSpeedMax;

		// Token: 0x04004D43 RID: 19779
		public readonly float healthMax;

		// Token: 0x04004D44 RID: 19780
		public readonly float childSpeed;

		// Token: 0x04004D45 RID: 19781
		public readonly float childDelay;

		// Token: 0x04004D46 RID: 19782
		public readonly float childHealth;

		// Token: 0x04004D47 RID: 19783
		public readonly int childCount;

		// Token: 0x04004D48 RID: 19784
		public readonly int direction;

		// Token: 0x04004D49 RID: 19785
		public float speed;

		// Token: 0x04004D4A RID: 19786
		public float rotationSpeed;

		// Token: 0x04004D4B RID: 19787
		public float health;
	}
}
