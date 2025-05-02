using System;
using UnityEngine;

// Token: 0x0200032B RID: 811
public class RobotLevelHoseLaser : AbstractCollidableObject
{
	// Token: 0x06002355 RID: 9045 RVA: 0x000C01A4 File Offset: 0x000BE3A4
	public RobotLevelHoseLaser Create(Vector3 pos, float angle, RobotLevelRobotBodyPart parent)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(base.gameObject);
		gameObject.transform.parent = parent.transform;
		gameObject.transform.position = pos;
		gameObject.transform.SetEulerAngles(null, null, new float?(angle));
		base.GetComponent<Collider2D>().enabled = false;
		return gameObject.GetComponent<RobotLevelHoseLaser>();
	}

	// Token: 0x06002356 RID: 9046 RVA: 0x0001DEB1 File Offset: 0x0001C0B1
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002357 RID: 9047 RVA: 0x0001DEC4 File Offset: 0x0001C0C4
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002358 RID: 9048 RVA: 0x0001DEDC File Offset: 0x0001C0DC
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x04001D5C RID: 7516
	public DamageDealer damageDealer;
}
