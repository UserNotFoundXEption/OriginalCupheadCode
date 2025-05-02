using System;
using UnityEngine;

// Token: 0x02000335 RID: 821
public class RobotLevelHatchBombBot : HomingProjectile
{
	// Token: 0x060023E1 RID: 9185 RVA: 0x0001E3D9 File Offset: 0x0001C5D9
	public void InitBombBot(LevelProperties.Robot.BombBot properties)
	{
		this.properties = properties;
		this.health = (float)properties.bombHP;
	}

	// Token: 0x060023E2 RID: 9186 RVA: 0x0001E3EF File Offset: 0x0001C5EF
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.Die();
	}

	// Token: 0x060023E3 RID: 9187 RVA: 0x0001E3FF File Offset: 0x0001C5FF
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (hit.GetComponent<RobotLevelRobotBodyPart>() != null)
		{
			this.Die();
		}
		else if (hit.GetComponent<RobotLevelHatchBombBot>() != null)
		{
			this.Die();
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x060023E4 RID: 9188 RVA: 0x0001E43C File Offset: 0x0001C63C
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f && !this.isDead)
		{
			this.Die();
		}
	}

	// Token: 0x060023E5 RID: 9189 RVA: 0x0001E472 File Offset: 0x0001C672
	public override void Awake()
	{
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x060023E6 RID: 9190 RVA: 0x0001E49D File Offset: 0x0001C69D
	public override void Start()
	{
		base.Start();
		this.damageDealer.SetDamage((float)this.properties.bombBossDamage);
		this.damageDealer.SetRate(0f);
	}

	// Token: 0x060023E7 RID: 9191 RVA: 0x0001E4CC File Offset: 0x0001C6CC
	public override void Update()
	{
		this.damageDealer.Update();
		base.Update();
	}

	// Token: 0x060023E8 RID: 9192 RVA: 0x0001E4DF File Offset: 0x0001C6DF
	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060023E9 RID: 9193 RVA: 0x000C215C File Offset: 0x000C035C
	public override void Die()
	{
		base.Die();
		this.isDead = true;
		this.StopAllCoroutines();
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		base.animator.Play("Explode");
		AudioManager.Play("robot_bombbot_death");
		this.emitAudioFromObject.Add("robot_bombbot_death");
	}

	// Token: 0x04001DBE RID: 7614
	[SerializeField]
	public Sprite explosion;

	// Token: 0x04001DBF RID: 7615
	public bool isDead;

	// Token: 0x04001DC0 RID: 7616
	public float health;

	// Token: 0x04001DC1 RID: 7617
	public DamageReceiver damageReceiver;

	// Token: 0x04001DC2 RID: 7618
	public LevelProperties.Robot.BombBot properties;
}
