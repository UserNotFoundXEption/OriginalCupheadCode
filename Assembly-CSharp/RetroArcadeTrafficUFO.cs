using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200031F RID: 799
public class RetroArcadeTrafficUFO : AbstractCollidableObject
{
	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x060022FE RID: 8958 RVA: 0x0001DACC File Offset: 0x0001BCCC
	// (set) Token: 0x060022FF RID: 8959 RVA: 0x0001DAD4 File Offset: 0x0001BCD4
	public bool IsMoving { get; set; }

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x06002300 RID: 8960 RVA: 0x0001DADD File Offset: 0x0001BCDD
	// (set) Token: 0x06002301 RID: 8961 RVA: 0x0001DAE5 File Offset: 0x0001BCE5
	public bool IsDead { get; set; }

	// Token: 0x06002302 RID: 8962 RVA: 0x0001DAEE File Offset: 0x0001BCEE
	public void Start()
	{
		base.gameObject.SetActive(false);
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002303 RID: 8963 RVA: 0x0001DB07 File Offset: 0x0001BD07
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002304 RID: 8964 RVA: 0x0001DB1F File Offset: 0x0001BD1F
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002305 RID: 8965 RVA: 0x0001DB3D File Offset: 0x0001BD3D
	public void StartMoving(List<Vector3> positions, float speed, float delay)
	{
		this.IsMoving = true;
		base.StartCoroutine(this.check_pieces_cr());
		base.StartCoroutine(this.move_cr(positions, speed, delay));
	}

	// Token: 0x06002306 RID: 8966 RVA: 0x000BF5B8 File Offset: 0x000BD7B8
	public IEnumerator move_cr(List<Vector3> positions, float speed, float delay)
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (int i = 0; i < positions.Count; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, delay);
			Vector3 dir = (positions[i] - base.transform.position).normalized;
			while (Vector3.Distance(base.transform.position, positions[i]) > 3f)
			{
				base.transform.position += dir * speed * CupheadTime.FixedDelta;
				yield return wait;
			}
			yield return null;
		}
		this.IsMoving = false;
		yield break;
	}

	// Token: 0x06002307 RID: 8967 RVA: 0x000BF5E8 File Offset: 0x000BD7E8
	public IEnumerator check_pieces_cr()
	{
		RetroArcadeTrafficUFOPiece[] pieces = base.GetComponentsInChildren<RetroArcadeTrafficUFOPiece>();
		int countDeadOnes = 0;
		for (;;)
		{
			countDeadOnes = 0;
			for (int i = 0; i < pieces.Length; i++)
			{
				if (pieces[i].IsDead)
				{
					countDeadOnes++;
				}
			}
			if (countDeadOnes >= pieces.Length)
			{
				break;
			}
			yield return null;
		}
		this.IsDead = true;
		yield return null;
		yield break;
	}

	// Token: 0x04001D06 RID: 7430
	public const float DIST_TO_SWITCH = 3f;

	// Token: 0x04001D09 RID: 7433
	public DamageDealer damageDealer;
}
