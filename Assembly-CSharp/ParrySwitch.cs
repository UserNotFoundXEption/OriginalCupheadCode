using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005B5 RID: 1461
public class ParrySwitch : AbstractSwitch
{
	// Token: 0x140000D2 RID: 210
	// (add) Token: 0x06003D57 RID: 15703 RVA: 0x00118BE8 File Offset: 0x00116DE8
	// (remove) Token: 0x06003D58 RID: 15704 RVA: 0x00118C20 File Offset: 0x00116E20
	public event Action OnPrePauseActivate;

	// Token: 0x17000504 RID: 1284
	// (get) Token: 0x06003D59 RID: 15705 RVA: 0x0003178C File Offset: 0x0002F98C
	// (set) Token: 0x06003D5A RID: 15706 RVA: 0x00031794 File Offset: 0x0002F994
	public bool IsParryable { get; set; }

	// Token: 0x06003D5B RID: 15707 RVA: 0x0003179D File Offset: 0x0002F99D
	public void FirePrePauseEvent()
	{
		if (this.OnPrePauseActivate != null)
		{
			this.OnPrePauseActivate();
		}
	}

	// Token: 0x06003D5C RID: 15708 RVA: 0x000317B5 File Offset: 0x0002F9B5
	public override void Awake()
	{
		base.Awake();
		base.tag = "ParrySwitch";
		this.IsParryable = true;
		if (base.GetComponent<Collider2D>() == null)
		{
		}
	}

	// Token: 0x06003D5D RID: 15709 RVA: 0x000317E0 File Offset: 0x0002F9E0
	public virtual void OnParryPrePause(AbstractPlayerController player)
	{
		if (this.parrySpark)
		{
			this.parrySpark.Create(base.transform.position);
		}
		this.FirePrePauseEvent();
	}

	// Token: 0x06003D5E RID: 15710 RVA: 0x0003180F File Offset: 0x0002FA0F
	public virtual void OnParryPostPause(AbstractPlayerController player)
	{
		base.DispatchEvent();
	}

	// Token: 0x06003D5F RID: 15711 RVA: 0x00031817 File Offset: 0x0002FA17
	public void ActivateFromOtherSource()
	{
		if (this.parrySpark)
		{
			this.parrySpark.Create(base.transform.position);
		}
		base.DispatchEvent();
	}

	// Token: 0x06003D60 RID: 15712 RVA: 0x00031846 File Offset: 0x0002FA46
	public void StartParryCooldown()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.parryCooldown_cr());
	}

	// Token: 0x06003D61 RID: 15713 RVA: 0x00118C58 File Offset: 0x00116E58
	public IEnumerator parryCooldown_cr()
	{
		float t = 0f;
		while (t < this.coolDown)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		Collider2D collider = base.GetComponent<Collider2D>();
		collider.enabled = true;
		yield return null;
		yield break;
	}

	// Token: 0x06003D62 RID: 15714 RVA: 0x00031861 File Offset: 0x0002FA61
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.parrySpark = null;
	}

	// Token: 0x040030D7 RID: 12503
	[SerializeField]
	public Effect parrySpark;

	// Token: 0x040030D8 RID: 12504
	[SerializeField]
	public float coolDown = 0.4f;
}
