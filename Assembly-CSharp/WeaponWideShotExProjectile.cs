using System;
using UnityEngine;

// Token: 0x02000559 RID: 1369
public class WeaponWideShotExProjectile : AbstractProjectile
{
	// Token: 0x06003943 RID: 14659 RVA: 0x0002E9C5 File Offset: 0x0002CBC5
	public override void Start()
	{
		base.Start();
		base.transform.position += base.transform.right * 100f;
		this.damageDealer.isDLCWeapon = true;
	}

	// Token: 0x06003944 RID: 14660 RVA: 0x0002EA04 File Offset: 0x0002CC04
	public override void Update()
	{
		base.Update();
		if (this.mainTimer < this.mainDuration)
		{
			this.mainTimer += CupheadTime.Delta;
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003945 RID: 14661 RVA: 0x0010B78C File Offset: 0x0010998C
	public override void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer damageDealer)
	{
		base.OnDealDamage(damage, receiver, damageDealer);
		Collider2D componentInChildren = receiver.GetComponentInChildren<Collider2D>();
		Vector3 vector = receiver.transform.position;
		if (componentInChildren != null)
		{
			vector = componentInChildren.transform.position + new Vector3(componentInChildren.offset.x * receiver.transform.lossyScale.x, componentInChildren.offset.y * receiver.transform.lossyScale.y);
		}
		Vector3 vector2 = MathUtils.AngleToDirection(base.transform.eulerAngles.z);
		Vector3 vector3 = this.origin + vector2 * Vector3.Distance(this.origin, vector);
		this.hitsparkPrefab.Create(Vector3.Lerp(vector3, vector, 0.5f) + MathUtils.RandomPointInUnitCircle() * 30f);
	}

	// Token: 0x06003946 RID: 14662 RVA: 0x0002EA44 File Offset: 0x0002CC44
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		this.damageDealer.DealDamage(hit);
		this.damageDealer.OnDealDamage += this.OnDealDamage;
	}

	// Token: 0x04002E0B RID: 11787
	public float mainDuration;

	// Token: 0x04002E0C RID: 11788
	public float mainTimer;

	// Token: 0x04002E0D RID: 11789
	public Vector3 origin;

	// Token: 0x04002E0E RID: 11790
	[SerializeField]
	public Effect hitsparkPrefab;
}
