using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003B8 RID: 952
public class TrainLevelPumpkin : AbstractCollidableObject
{
	// Token: 0x06002A29 RID: 10793 RVA: 0x000D3738 File Offset: 0x000D1938
	public void Create(Vector2 pos, int direction, float speed, float health, float fallTime, Transform target)
	{
		TrainLevelPumpkin trainLevelPumpkin = this.InstantiatePrefab<TrainLevelPumpkin>();
		trainLevelPumpkin.transform.position = pos;
		trainLevelPumpkin.transform.SetScale(new float?((float)(-(float)direction)), new float?(1f), new float?(1f));
		trainLevelPumpkin.direction = direction;
		trainLevelPumpkin.health = health;
		trainLevelPumpkin.speed = speed;
		trainLevelPumpkin.target = target;
		trainLevelPumpkin.fallTime = fallTime;
	}

	// Token: 0x06002A2A RID: 10794 RVA: 0x000D37AC File Offset: 0x000D19AC
	public void Start()
	{
		base.StartCoroutine(this.x_cr());
		base.StartCoroutine(this.drop_cr());
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.brick = (this.brickPrefab.Create() as TrainLevelPumpkinProjectile);
		this.brick.fallTime = this.fallTime;
		this.brick.transform.SetParent(base.transform);
		this.brick.transform.ResetLocalTransforms();
	}

	// Token: 0x06002A2B RID: 10795 RVA: 0x00023765 File Offset: 0x00021965
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06002A2C RID: 10796 RVA: 0x00023790 File Offset: 0x00021990
	public void Drop()
	{
		if (this.brick != null)
		{
			this.brick.Drop();
			this.brick = null;
			base.StartCoroutine(this.y_cr());
		}
	}

	// Token: 0x06002A2D RID: 10797 RVA: 0x000237C2 File Offset: 0x000219C2
	public void Die()
	{
		this.StopAllCoroutines();
		this.Drop();
		base.animator.Play("Die");
		AudioManager.Play("train_pumpkin_die");
		this.emitAudioFromObject.Add("train_pumpkin_die");
	}

	// Token: 0x06002A2E RID: 10798 RVA: 0x000237FA File Offset: 0x000219FA
	public void OnDeathAnimComplete()
	{
		this.End();
	}

	// Token: 0x06002A2F RID: 10799 RVA: 0x00023802 File Offset: 0x00021A02
	public void End()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002A30 RID: 10800 RVA: 0x000D3844 File Offset: 0x000D1A44
	public IEnumerator x_cr()
	{
		for (;;)
		{
			base.transform.AddPosition(this.speed * CupheadTime.Delta * (float)this.direction, 0f, 0f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002A31 RID: 10801 RVA: 0x000D3860 File Offset: 0x000D1A60
	public IEnumerator y_cr()
	{
		float ySpeed = 1f;
		float mult = 1.1f;
		for (;;)
		{
			base.transform.AddPosition(0f, ySpeed * CupheadTime.Delta, 0f);
			if (CupheadTime.Delta != 0f)
			{
				ySpeed *= mult;
			}
			yield return null;
			if (base.transform.position.y > 720f)
			{
				this.End();
			}
		}
		yield break;
	}

	// Token: 0x06002A32 RID: 10802 RVA: 0x000D387C File Offset: 0x000D1A7C
	public IEnumerator drop_cr()
	{
		bool check = true;
		while (check)
		{
			if (this.direction > 0 && base.transform.position.x > this.target.position.x)
			{
				check = false;
				this.Drop();
			}
			if (this.direction < 0 && base.transform.position.x < this.target.position.x)
			{
				check = false;
				this.Drop();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002A33 RID: 10803 RVA: 0x00023815 File Offset: 0x00021A15
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.brickPrefab = null;
	}

	// Token: 0x04002333 RID: 9011
	[SerializeField]
	public TrainLevelPumpkinProjectile brickPrefab;

	// Token: 0x04002334 RID: 9012
	public TrainLevelPumpkinProjectile brick;

	// Token: 0x04002335 RID: 9013
	public int direction;

	// Token: 0x04002336 RID: 9014
	public float speed;

	// Token: 0x04002337 RID: 9015
	public float health;

	// Token: 0x04002338 RID: 9016
	public float fallTime;

	// Token: 0x04002339 RID: 9017
	public Transform target;

	// Token: 0x0400233A RID: 9018
	public DamageReceiver damageReceiver;
}
