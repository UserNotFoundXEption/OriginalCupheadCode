using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000329 RID: 809
public class RetroArcadeWormTongue : AbstractCollidableObject
{
	// Token: 0x170002FB RID: 763
	// (get) Token: 0x06002331 RID: 9009 RVA: 0x0001DC9C File Offset: 0x0001BE9C
	// (set) Token: 0x06002332 RID: 9010 RVA: 0x0001DCA4 File Offset: 0x0001BEA4
	public float TileRotationSpeed { get; set; }

	// Token: 0x06002333 RID: 9011 RVA: 0x0001DCAD File Offset: 0x0001BEAD
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		base.GetComponentInChildren<Collider2D>().enabled = false;
	}

	// Token: 0x06002334 RID: 9012 RVA: 0x0001DCCC File Offset: 0x0001BECC
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002335 RID: 9013 RVA: 0x0001DCE4 File Offset: 0x0001BEE4
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06002336 RID: 9014 RVA: 0x0001DCFB File Offset: 0x0001BEFB
	public void Init(LevelProperties.RetroArcade.Worm properties)
	{
		this.properties = properties;
	}

	// Token: 0x06002337 RID: 9015 RVA: 0x0001DD04 File Offset: 0x0001BF04
	public void Extend()
	{
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06002338 RID: 9016 RVA: 0x0001DD13 File Offset: 0x0001BF13
	public void Retract()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.retract_cr());
	}

	// Token: 0x06002339 RID: 9017 RVA: 0x000BFD40 File Offset: 0x000BDF40
	public IEnumerator main_cr()
	{
		float extendTime = 4.45f;
		float t = 0f;
		while (t < extendTime)
		{
			base.transform.SetPosition(new float?(this.parent.transform.position.x), new float?(this.parent.transform.position.y + Mathf.Lerp(-250f, 195f, t / extendTime)), null);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		base.GetComponentInChildren<Collider2D>().enabled = true;
		for (;;)
		{
			float rotation = this.tongueSpinner.eulerAngles.z;
			this.tongueSpinner.SetEulerAngles(new float?(0f), new float?(0f), new float?(rotation + this.properties.tongueRotateSpeed * CupheadTime.FixedDelta * -1f));
			base.transform.SetPosition(new float?(this.parent.transform.position.x), new float?(this.parent.transform.position.y + 195f), null);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x0600233A RID: 9018 RVA: 0x000BFD5C File Offset: 0x000BDF5C
	public IEnumerator retract_cr()
	{
		float retractTime = 4.45f;
		base.GetComponentInChildren<Collider2D>().enabled = false;
		float t = 0f;
		while (t < retractTime)
		{
			base.transform.SetPosition(new float?(this.parent.transform.position.x), new float?(this.parent.transform.position.y + Mathf.Lerp(195f, -250f, t / retractTime)), null);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x04001D40 RID: 7488
	public const float RETRACTED_Y_OFFSET = -250f;

	// Token: 0x04001D41 RID: 7489
	public const float EXTENDED_Y_OFFSET = 195f;

	// Token: 0x04001D42 RID: 7490
	public const float EXTEND_MOVE_SPEED = 100f;

	// Token: 0x04001D43 RID: 7491
	public LevelProperties.RetroArcade.Worm properties;

	// Token: 0x04001D44 RID: 7492
	[SerializeField]
	public Transform tongueSpinner;

	// Token: 0x04001D45 RID: 7493
	[SerializeField]
	public RetroArcadeWorm parent;

	// Token: 0x04001D47 RID: 7495
	public DamageDealer damageDealer;
}
