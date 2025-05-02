using System;
using UnityEngine;

// Token: 0x02000556 RID: 1366
public class WeaponUpshotExProjectile : AbstractProjectile
{
	// Token: 0x06003927 RID: 14631 RVA: 0x0002E8D3 File Offset: 0x0002CAD3
	public override void OnDieDistance()
	{
	}

	// Token: 0x1700047A RID: 1146
	// (get) Token: 0x06003928 RID: 14632 RVA: 0x0002E8D5 File Offset: 0x0002CAD5
	public override float DestroyLifetime
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x1700047B RID: 1147
	// (get) Token: 0x06003929 RID: 14633 RVA: 0x0002E8DC File Offset: 0x0002CADC
	public override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600392A RID: 14634 RVA: 0x0010AC98 File Offset: 0x00108E98
	public override void Start()
	{
		base.Start();
		this.damageDealer.SetDamage(WeaponProperties.LevelWeaponUpshot.Ex.damage);
		this.damageDealer.SetRate(WeaponProperties.LevelWeaponUpshot.Ex.damageRate);
		this.damageDealer.isDLCWeapon = true;
		this.angle = MathUtils.DirectionToAngle(base.transform.right);
		base.transform.position += base.transform.right * 120f;
		base.transform.localScale = new Vector3((float)((base.transform.eulerAngles.z <= 90f || base.transform.eulerAngles.z >= 270f) ? 1 : -1), 1f);
		this.endScale = base.transform.localScale;
		base.transform.localScale *= 0.5f;
		this.startScale = base.transform.localScale;
		base.transform.eulerAngles = Vector3.zero;
		this.startPos = base.transform.position;
		base.animator.Play("EX", 0, Random.Range(0f, 1f));
		this.trailPositions = new Vector2[6];
		for (int i = 0; i < this.trailPositions.Length; i++)
		{
			this.trailPositions[i] = base.transform.position;
		}
	}

	// Token: 0x0600392B RID: 14635 RVA: 0x0010AE38 File Offset: 0x00109038
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		if (this.timeUntilUnfreeze > 0f)
		{
			this.timeUntilUnfreeze -= CupheadTime.FixedDelta;
			return;
		}
		this.time += CupheadTime.FixedDelta;
		this.angle += Mathf.Lerp(WeaponProperties.LevelWeaponUpshot.Ex.minRotationSpeed, WeaponProperties.LevelWeaponUpshot.Ex.maxRotationSpeed, this.time / WeaponProperties.LevelWeaponUpshot.Ex.rotationRampTime) * CupheadTime.FixedDelta * this.rotateDir;
		this.radius += Mathf.Lerp(WeaponProperties.LevelWeaponUpshot.Ex.minRadiusSpeed, WeaponProperties.LevelWeaponUpshot.Ex.maxRadiusSpeed, this.time / WeaponProperties.LevelWeaponUpshot.Ex.radiusRampTime) * CupheadTime.FixedDelta;
		base.transform.position = this.startPos + MathUtils.AngleToDirection(this.angle) * this.radius;
		float num = Mathf.Round(this.time * 24f) / 24f;
		base.transform.localScale = Vector3.Lerp(this.startScale, this.endScale, num * 5f);
		num *= 0.2f;
		this.trail1.color = new Color(1f, 1f, 1f, 0.5f - num);
		this.trail2.color = new Color(1f, 1f, 1f, 0.25f - num);
		this.UpdateTrails();
	}

	// Token: 0x0600392C RID: 14636 RVA: 0x0010AFB8 File Offset: 0x001091B8
	public void UpdateTrails()
	{
		int num = this.currentPositionIndex - 2;
		if (num < 0)
		{
			num += this.trailPositions.Length;
		}
		int num2 = this.currentPositionIndex - 5;
		if (num2 < 0)
		{
			num2 += this.trailPositions.Length;
		}
		this.trail1.transform.position = this.trailPositions[num];
		this.trail2.transform.position = this.trailPositions[num2];
		this.currentPositionIndex = (this.currentPositionIndex + 1) % this.trailPositions.Length;
		this.trailPositions[this.currentPositionIndex] = base.transform.position;
	}

	// Token: 0x0600392D RID: 14637 RVA: 0x0010B084 File Offset: 0x00109284
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		float num = this.damageDealer.DealDamage(hit);
		this.totalDamage += num;
		if (this.totalDamage > WeaponProperties.LevelWeaponUpshot.Ex.maxDamage)
		{
			this.Die();
		}
		if (num > 0f)
		{
			this.hitFXPrefab.Create(Vector3.Lerp(base.transform.position, hit.transform.position, 0.5f) + Random.insideUnitCircle * 20f);
			AudioManager.Play("player_ex_impact_hit");
			this.emitAudioFromObject.Add("player_ex_impact_hit");
			this.timeUntilUnfreeze = WeaponProperties.LevelWeaponUpshot.Ex.freezeTime;
		}
	}

	// Token: 0x0600392E RID: 14638 RVA: 0x0002E8DF File Offset: 0x0002CADF
	public override void Die()
	{
		base.Die();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.Play("Die");
	}

	// Token: 0x04002DF2 RID: 11762
	public float timeUntilUnfreeze;

	// Token: 0x04002DF3 RID: 11763
	public float totalDamage;

	// Token: 0x04002DF4 RID: 11764
	public float angle;

	// Token: 0x04002DF5 RID: 11765
	public float time;

	// Token: 0x04002DF6 RID: 11766
	public float radius;

	// Token: 0x04002DF7 RID: 11767
	public Vector3 startPos;

	// Token: 0x04002DF8 RID: 11768
	public float rotateDir;

	// Token: 0x04002DF9 RID: 11769
	public Vector3 startScale;

	// Token: 0x04002DFA RID: 11770
	public Vector3 endScale;

	// Token: 0x04002DFB RID: 11771
	public Vector2[] trailPositions;

	// Token: 0x04002DFC RID: 11772
	public int currentPositionIndex;

	// Token: 0x04002DFD RID: 11773
	[SerializeField]
	public SpriteRenderer trail1;

	// Token: 0x04002DFE RID: 11774
	[SerializeField]
	public SpriteRenderer trail2;

	// Token: 0x04002DFF RID: 11775
	public const int trailFrameDelay = 3;

	// Token: 0x04002E00 RID: 11776
	[SerializeField]
	public Effect hitFXPrefab;
}
