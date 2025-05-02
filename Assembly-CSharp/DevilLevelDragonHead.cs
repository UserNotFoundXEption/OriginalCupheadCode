using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001AC RID: 428
public class DevilLevelDragonHead : AbstractCollidableObject
{
	// Token: 0x0600145A RID: 5210 RVA: 0x00099E0C File Offset: 0x0009800C
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		foreach (CollisionChild collisionChild in this.children.GetComponentsInChildren<CollisionChild>())
		{
			collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		}
		this.children.gameObject.SetActive(false);
	}

	// Token: 0x0600145B RID: 5211 RVA: 0x000112FE File Offset: 0x0000F4FE
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600145C RID: 5212 RVA: 0x00011316 File Offset: 0x0000F516
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600145D RID: 5213 RVA: 0x00099E74 File Offset: 0x00098074
	public void Attack(DevilLevelSittingDevil parent, bool isLeft)
	{
		this.state = DevilLevelDragonHead.State.Moving;
		base.transform.SetScale(new float?((float)((!isLeft) ? -1 : 1)), null, null);
		base.StartCoroutine(this.move_cr(parent, isLeft));
	}

	// Token: 0x0600145E RID: 5214 RVA: 0x00099EC8 File Offset: 0x000980C8
	public IEnumerator move_cr(DevilLevelSittingDevil parent, bool isLeft)
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		base.transform.position = ((!isLeft) ? this.rightRoot.position : this.leftRoot.position);
		Vector3 dir = (!isLeft) ? Vector3.left : Vector3.right;
		yield return parent.animator.WaitForAnimationToEnd(this, "Morph_Start" + ((!isLeft) ? "_Right" : "_Left"), false, true);
		parent.animator.SetTrigger("OnDragonAttack");
		this.children.gameObject.SetActive(true);
		while (this.state == DevilLevelDragonHead.State.Moving)
		{
			base.transform.position += dir * (this.speed * CupheadTime.FixedDelta) * parent.animator.speed;
			yield return wait;
		}
		yield return parent.animator.WaitForAnimationToEnd(this, "Morph_Attack", 1, false, true);
		this.state = DevilLevelDragonHead.State.Idle;
		this.children.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x0600145F RID: 5215 RVA: 0x0001133F File Offset: 0x0000F53F
	public void SetPosition(Vector3 pos)
	{
		this.children.gameObject.SetActive(false);
		base.transform.position = pos;
	}

	// Token: 0x040010A2 RID: 4258
	[SerializeField]
	public float speed;

	// Token: 0x040010A3 RID: 4259
	[SerializeField]
	public Transform leftRoot;

	// Token: 0x040010A4 RID: 4260
	[SerializeField]
	public Transform rightRoot;

	// Token: 0x040010A5 RID: 4261
	[SerializeField]
	public Transform children;

	// Token: 0x040010A6 RID: 4262
	public DevilLevelDragonHead.State state;

	// Token: 0x040010A7 RID: 4263
	public const float FRAME_RATE = 0.0416666679f;

	// Token: 0x040010A8 RID: 4264
	public DamageDealer damageDealer;

	// Token: 0x02000B24 RID: 2852
	public enum State
	{
		// Token: 0x040051B2 RID: 20914
		Idle,
		// Token: 0x040051B3 RID: 20915
		Moving,
		// Token: 0x040051B4 RID: 20916
		Stopped
	}
}
