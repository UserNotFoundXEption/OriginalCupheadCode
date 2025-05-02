using System;
using UnityEngine;

// Token: 0x020003AC RID: 940
public class TrainLevelEngineBossTail : ParrySwitch
{
	// Token: 0x17000330 RID: 816
	// (get) Token: 0x060029C3 RID: 10691 RVA: 0x0002323F File Offset: 0x0002143F
	// (set) Token: 0x060029C4 RID: 10692 RVA: 0x00023263 File Offset: 0x00021463
	public bool tailEnabled
	{
		get
		{
			return !(this.circleCollider == null) && this.circleCollider.enabled;
		}
		set
		{
			if (this.circleCollider != null)
			{
				this.circleCollider.enabled = value;
			}
		}
	}

	// Token: 0x060029C5 RID: 10693 RVA: 0x000D2B48 File Offset: 0x000D0D48
	public static TrainLevelEngineBossTail Create(Transform target)
	{
		GameObject gameObject = new GameObject("Engine_Boss_Tail");
		TrainLevelEngineBossTail trainLevelEngineBossTail = gameObject.AddComponent<TrainLevelEngineBossTail>();
		trainLevelEngineBossTail.target = target;
		trainLevelEngineBossTail.tag = "ParrySwitch";
		return trainLevelEngineBossTail;
	}

	// Token: 0x060029C6 RID: 10694 RVA: 0x00023282 File Offset: 0x00021482
	public override void Awake()
	{
		base.Awake();
		this.circleCollider = base.gameObject.AddComponent<CircleCollider2D>();
		this.circleCollider.radius = 40f;
		this.circleCollider.isTrigger = true;
	}

	// Token: 0x060029C7 RID: 10695 RVA: 0x000232B7 File Offset: 0x000214B7
	public void Update()
	{
		this.UpdateLocation();
	}

	// Token: 0x060029C8 RID: 10696 RVA: 0x000232BF File Offset: 0x000214BF
	public void LateUpdate()
	{
		this.UpdateLocation();
	}

	// Token: 0x060029C9 RID: 10697 RVA: 0x000232C7 File Offset: 0x000214C7
	public void UpdateLocation()
	{
		if (this.target != null)
		{
			base.transform.position = this.target.position;
		}
	}

	// Token: 0x040022FA RID: 8954
	public CircleCollider2D circleCollider;

	// Token: 0x040022FB RID: 8955
	public Transform target;
}
