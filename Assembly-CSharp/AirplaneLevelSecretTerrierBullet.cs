using System;
using UnityEngine;

// Token: 0x02000132 RID: 306
public class AirplaneLevelSecretTerrierBullet : AbstractProjectile
{
	// Token: 0x06000E82 RID: 3714 RVA: 0x0008B1EC File Offset: 0x000893EC
	public AirplaneLevelSecretTerrierBullet Create(Vector3 pos, Vector3 targetPos, LevelProperties.Airplane.SecretTerriers props, Vector3 scale)
	{
		AirplaneLevelSecretTerrierBullet airplaneLevelSecretTerrierBullet = base.Create() as AirplaneLevelSecretTerrierBullet;
		airplaneLevelSecretTerrierBullet.speed = props.dogBulletArcSpeed;
		airplaneLevelSecretTerrierBullet.arcHeight = props.dogBulletArcHeight;
		airplaneLevelSecretTerrierBullet.splitAngle = props.dogBulletSplitAngle;
		airplaneLevelSecretTerrierBullet.splitSpeed = props.dogBulletSplitSpeed;
		airplaneLevelSecretTerrierBullet.willSplit = props.dogBulletWillSplit;
		airplaneLevelSecretTerrierBullet.hp = props.dogBulletHealth;
		airplaneLevelSecretTerrierBullet.transform.position = pos;
		airplaneLevelSecretTerrierBullet.posTimer = 0f;
		airplaneLevelSecretTerrierBullet.startPos = pos;
		airplaneLevelSecretTerrierBullet.destPos = targetPos;
		airplaneLevelSecretTerrierBullet.lastPos = airplaneLevelSecretTerrierBullet.startPos;
		airplaneLevelSecretTerrierBullet.transform.localScale = scale;
		airplaneLevelSecretTerrierBullet.damageReceiver = airplaneLevelSecretTerrierBullet.GetComponent<DamageReceiver>();
		airplaneLevelSecretTerrierBullet.damageReceiver.OnDamageTaken += airplaneLevelSecretTerrierBullet.OnDamageTaken;
		return airplaneLevelSecretTerrierBullet;
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x0000C4DE File Offset: 0x0000A6DE
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
	}

	// Token: 0x06000E84 RID: 3716 RVA: 0x0008B2B0 File Offset: 0x000894B0
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.exploded)
		{
			return;
		}
		if (this.posTimer < 1f)
		{
			this.lastPos = base.transform.position;
			base.transform.position = Vector3.Lerp(this.startPos, this.destPos, this.posTimer) + Vector3.up * Mathf.Sin(this.posTimer * 3.14159274f) * this.arcHeight;
			this.posTimer += this.speed * CupheadTime.FixedDelta;
		}
		else
		{
			base.transform.position += this.destPos - this.lastPos;
			this.lastPos += Vector3.up * this.arcHeight * CupheadTime.FixedDelta * 0.25f;
		}
	}

	// Token: 0x06000E85 RID: 3717 RVA: 0x0000C4E7 File Offset: 0x0000A6E7
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000E86 RID: 3718 RVA: 0x0008B3B8 File Offset: 0x000895B8
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.exploded)
		{
			return;
		}
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.exploded = true;
			this.coll.enabled = false;
			base.animator.Play((!Rand.Bool()) ? "ExplodeB" : "ExplodeA");
			AudioManager.Play("sfx_dlc_dogfight_ps_terrier_pineappleexplode");
		}
	}

	// Token: 0x06000E87 RID: 3719 RVA: 0x0008B438 File Offset: 0x00089638
	public void AniEvent_SpawnShrapnel()
	{
		this.splitBulletPrefab.Create(base.transform.position, MathUtils.DirectionToAngle(Vector3.right), this.splitSpeed);
		this.splitBulletPrefab.Create(base.transform.position, MathUtils.DirectionToAngle(Vector3.right) - this.splitAngle, this.splitSpeed);
		this.splitBulletPrefab.Create(base.transform.position, MathUtils.DirectionToAngle(Vector3.right) + this.splitAngle, this.splitSpeed);
	}

	// Token: 0x06000E88 RID: 3720 RVA: 0x0000C505 File Offset: 0x0000A705
	public void AniEvent_EndExplosion()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x0000C512 File Offset: 0x0000A712
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			this.damageDealer.DealDamage(hit);
		}
		AudioManager.Play("sfx_dlc_dogfight_ps_terrier_pineapplehitplayer");
	}

	// Token: 0x04000BBE RID: 3006
	public float speed;

	// Token: 0x04000BBF RID: 3007
	public float arcHeight;

	// Token: 0x04000BC0 RID: 3008
	public float splitAngle;

	// Token: 0x04000BC1 RID: 3009
	public float splitSpeed;

	// Token: 0x04000BC2 RID: 3010
	public float hp;

	// Token: 0x04000BC3 RID: 3011
	public bool willSplit;

	// Token: 0x04000BC4 RID: 3012
	public Vector3 startPos;

	// Token: 0x04000BC5 RID: 3013
	public Vector3 destPos;

	// Token: 0x04000BC6 RID: 3014
	public Vector3 lastPos;

	// Token: 0x04000BC7 RID: 3015
	public float posTimer;

	// Token: 0x04000BC8 RID: 3016
	public bool exploded;

	// Token: 0x04000BC9 RID: 3017
	[SerializeField]
	public CircleCollider2D coll;

	// Token: 0x04000BCA RID: 3018
	[SerializeField]
	public BasicProjectile splitBulletPrefab;

	// Token: 0x04000BCB RID: 3019
	public DamageReceiver damageReceiver;
}
