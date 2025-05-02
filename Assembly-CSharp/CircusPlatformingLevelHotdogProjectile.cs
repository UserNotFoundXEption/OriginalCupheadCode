using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000409 RID: 1033
public class CircusPlatformingLevelHotdogProjectile : BasicProjectile
{
	// Token: 0x14000061 RID: 97
	// (add) Token: 0x06002D15 RID: 11541 RVA: 0x000DBE94 File Offset: 0x000DA094
	// (remove) Token: 0x06002D16 RID: 11542 RVA: 0x000DBECC File Offset: 0x000DA0CC
	public event Action<CircusPlatformingLevelHotdogProjectile> OnDestroyCallback;

	// Token: 0x17000358 RID: 856
	// (get) Token: 0x06002D17 RID: 11543 RVA: 0x00025A60 File Offset: 0x00023C60
	public override float DestroyLifetime
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x06002D18 RID: 11544 RVA: 0x00025A67 File Offset: 0x00023C67
	public override void Awake()
	{
		base.Awake();
		this.collider2d = base.GetComponent<Collider2D>();
	}

	// Token: 0x06002D19 RID: 11545 RVA: 0x000DBF04 File Offset: 0x000DA104
	public override void Start()
	{
		base.Start();
		base.transform.localScale = new Vector3(0f, 1f, 1f);
		this.spark.Create(base.transform.position - new Vector3(10f, 0f, 0f));
		base.StartCoroutine(this.scaleOnStart_cr());
	}

	// Token: 0x06002D1A RID: 11546 RVA: 0x000DBF74 File Offset: 0x000DA174
	public IEnumerator scaleOnStart_cr()
	{
		while (base.transform.localScale.x < 1f)
		{
			base.transform.AddScale(this.scaleFactor * CupheadTime.Delta, 0f, 0f);
			yield return null;
		}
		base.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
		yield break;
	}

	// Token: 0x06002D1B RID: 11547 RVA: 0x000DBF90 File Offset: 0x000DA190
	public void Side(bool isRight)
	{
		if (isRight)
		{
			for (int i = 0; i < this.renderers.Length; i++)
			{
				this.renderers[i].sortingOrder += 3;
			}
		}
	}

	// Token: 0x06002D1C RID: 11548 RVA: 0x000DBFD4 File Offset: 0x000DA1D4
	public void SetCondiment(string type)
	{
		if (type == "K")
		{
			base.animator.Play("Ketchup");
		}
		else if (type == "M")
		{
			base.animator.Play("Mustard");
		}
		else if (type == "R")
		{
			base.animator.Play("Relish");
		}
	}

	// Token: 0x06002D1D RID: 11549 RVA: 0x00025A7B File Offset: 0x00023C7B
	public void EnableCollider(bool enable)
	{
		if (this.collider2d != null)
		{
			this.collider2d.enabled = enable;
		}
	}

	// Token: 0x06002D1E RID: 11550 RVA: 0x00025A9A File Offset: 0x00023C9A
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.OnDestroyCallback != null)
		{
			this.OnDestroyCallback(this);
		}
		this.spark = null;
	}

	// Token: 0x04002559 RID: 9561
	public const string KetchupState = "Ketchup";

	// Token: 0x0400255A RID: 9562
	public const string MustardState = "Mustard";

	// Token: 0x0400255B RID: 9563
	public const string RelishState = "Relish";

	// Token: 0x0400255C RID: 9564
	public const string Ketchup = "K";

	// Token: 0x0400255D RID: 9565
	public const string Mustard = "M";

	// Token: 0x0400255E RID: 9566
	public const string Relish = "R";

	// Token: 0x0400255F RID: 9567
	[SerializeField]
	public float scaleFactor;

	// Token: 0x04002560 RID: 9568
	[SerializeField]
	public SpriteRenderer[] renderers;

	// Token: 0x04002561 RID: 9569
	[SerializeField]
	public Effect spark;

	// Token: 0x04002562 RID: 9570
	public Collider2D collider2d;
}
