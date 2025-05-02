using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200056D RID: 1389
public class PlaneWeaponBombExProjectile : AbstractProjectile
{
	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x06003A66 RID: 14950 RVA: 0x0002F889 File Offset: 0x0002DA89
	public override float DestroyLifetime
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x06003A67 RID: 14951 RVA: 0x0010F268 File Offset: 0x0010D468
	public void Init()
	{
		this.Cuphead.enabled = ((this.PlayerId == PlayerId.PlayerOne && !PlayerManager.player1IsMugman) || (this.PlayerId == PlayerId.PlayerTwo && PlayerManager.player1IsMugman));
		this.Mugman.enabled = ((this.PlayerId == PlayerId.PlayerOne && PlayerManager.player1IsMugman) || (this.PlayerId == PlayerId.PlayerTwo && !PlayerManager.player1IsMugman));
		base.StartCoroutine(this.trail_cr());
	}

	// Token: 0x06003A68 RID: 14952 RVA: 0x0002F890 File Offset: 0x0002DA90
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag == "Parry")
		{
			return;
		}
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x06003A69 RID: 14953 RVA: 0x0002F8B0 File Offset: 0x0002DAB0
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		this.DealDamage(hit);
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06003A6A RID: 14954 RVA: 0x0002F8C1 File Offset: 0x0002DAC1
	public void DealDamage(GameObject hit)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06003A6B RID: 14955 RVA: 0x0002F8D0 File Offset: 0x0002DAD0
	public override void Die()
	{
		this.move = false;
		base.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
		base.Die();
	}

	// Token: 0x06003A6C RID: 14956 RVA: 0x0010F2F4 File Offset: 0x0010D4F4
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.move)
		{
			return;
		}
		this.t += CupheadTime.FixedDelta;
		if (this.target != null && this.target.gameObject.activeInHierarchy && this.target.isActiveAndEnabled && this.t < WeaponProperties.LevelWeaponHoming.Basic.maxHomingTime)
		{
			float num;
			for (num = MathUtils.DirectionToAngle(this.target.bounds.center - base.transform.position); num > this.rotation + 180f; num -= 360f)
			{
			}
			while (num < this.rotation - 180f)
			{
				num += 360f;
			}
			float num2 = this.rotationSpeed.min;
			if (this.t > this.timeBeforeEaseRotationSpeed + this.rotationSpeedEaseTime)
			{
				num2 = this.rotationSpeed.max;
			}
			else if (this.t > this.timeBeforeEaseRotationSpeed)
			{
				num2 = this.rotationSpeed.GetFloatAt((this.t - this.timeBeforeEaseRotationSpeed) / this.rotationSpeedEaseTime);
			}
			if (Mathf.Abs(num - this.rotation) < num2 * CupheadTime.FixedDelta)
			{
				this.rotation = num;
			}
			else if (num > this.rotation)
			{
				this.rotation += num2 * CupheadTime.FixedDelta;
			}
			else
			{
				this.rotation -= num2 * CupheadTime.FixedDelta;
			}
		}
		Vector3 vector = MathUtils.AngleToDirection(this.rotation);
		base.transform.position += vector * this.speed * CupheadTime.FixedDelta;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.rotation + this.spriteRotation));
		if (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(this.destroyPadding, this.destroyPadding)))
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003A6D RID: 14957 RVA: 0x0010F544 File Offset: 0x0010D744
	public void FindTarget()
	{
		float num = float.MaxValue;
		Collider2D collider2D = null;
		Vector2 vector = base.transform.position + this.speed * (this.timeBeforeEaseRotationSpeed + this.rotationSpeedEaseTime * 0.75f) * MathUtils.AngleToDirection(this.rotation);
		foreach (DamageReceiver damageReceiver in Object.FindObjectsOfType<DamageReceiver>())
		{
			if (damageReceiver.gameObject.activeInHierarchy && damageReceiver.type == DamageReceiver.Type.Enemy)
			{
				foreach (Collider2D collider2D2 in damageReceiver.GetComponents<Collider2D>())
				{
					if (collider2D2.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D2.bounds.center, collider2D2.bounds.size / 2f))
					{
						float sqrMagnitude = (vector - collider2D2.bounds.center).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							num = sqrMagnitude;
							collider2D = collider2D2;
						}
					}
				}
				foreach (DamageReceiverChild damageReceiverChild in damageReceiver.GetComponentsInChildren<DamageReceiverChild>())
				{
					foreach (Collider2D collider2D3 in damageReceiverChild.GetComponents<Collider2D>())
					{
						if (collider2D3.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D3.bounds.center, collider2D3.bounds.size / 2f))
						{
							float sqrMagnitude2 = (vector - collider2D3.bounds.center).sqrMagnitude;
							if (sqrMagnitude2 < num)
							{
								num = sqrMagnitude2;
								collider2D = collider2D3;
							}
						}
					}
				}
			}
		}
		this.target = collider2D;
	}

	// Token: 0x06003A6E RID: 14958 RVA: 0x0010F768 File Offset: 0x0010D968
	public IEnumerator trail_cr()
	{
		while (!base.dead)
		{
			yield return CupheadTime.WaitForSeconds(this, this.trailDelay);
			if (base.dead)
			{
				yield break;
			}
			this.trailFxPrefab.Create(this.trailFxRoot.position + MathUtils.RandomPointInUnitCircle() * this.trailFxMaxOffset);
		}
		yield break;
	}

	// Token: 0x06003A6F RID: 14959 RVA: 0x0002F908 File Offset: 0x0002DB08
	public override void OnLevelEnd()
	{
	}

	// Token: 0x04002EB7 RID: 11959
	[SerializeField]
	public float spriteRotation;

	// Token: 0x04002EB8 RID: 11960
	[SerializeField]
	public Effect trailFxPrefab;

	// Token: 0x04002EB9 RID: 11961
	[SerializeField]
	public Transform trailFxRoot;

	// Token: 0x04002EBA RID: 11962
	[SerializeField]
	public float trailFxMaxOffset;

	// Token: 0x04002EBB RID: 11963
	[SerializeField]
	public float trailDelay;

	// Token: 0x04002EBC RID: 11964
	[SerializeField]
	public float destroyPadding;

	// Token: 0x04002EBD RID: 11965
	[SerializeField]
	public SpriteRenderer Cuphead;

	// Token: 0x04002EBE RID: 11966
	[SerializeField]
	public SpriteRenderer Mugman;

	// Token: 0x04002EBF RID: 11967
	public float speed;

	// Token: 0x04002EC0 RID: 11968
	public MinMax rotationSpeed;

	// Token: 0x04002EC1 RID: 11969
	public float timeBeforeEaseRotationSpeed;

	// Token: 0x04002EC2 RID: 11970
	public float rotationSpeedEaseTime;

	// Token: 0x04002EC3 RID: 11971
	public float rotation;

	// Token: 0x04002EC4 RID: 11972
	public Vector2 velocity;

	// Token: 0x04002EC5 RID: 11973
	public float t;

	// Token: 0x04002EC6 RID: 11974
	public bool move = true;

	// Token: 0x04002EC7 RID: 11975
	public Collider2D target;

	// Token: 0x04002EC8 RID: 11976
	public AbstractPlayerController player;
}
