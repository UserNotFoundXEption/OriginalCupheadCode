using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001F7 RID: 503
public class DicePalaceMainLevelChaliceParryableHearts : AbstractProjectile
{
	// Token: 0x14000041 RID: 65
	// (add) Token: 0x06001735 RID: 5941 RVA: 0x000A11DC File Offset: 0x0009F3DC
	// (remove) Token: 0x06001736 RID: 5942 RVA: 0x000A1214 File Offset: 0x0009F414
	public event Action OnPrePauseActivate;

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06001737 RID: 5943 RVA: 0x00013C40 File Offset: 0x00011E40
	// (set) Token: 0x06001738 RID: 5944 RVA: 0x00013C48 File Offset: 0x00011E48
	public bool IsParryable { get; set; }

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06001739 RID: 5945 RVA: 0x00013C51 File Offset: 0x00011E51
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600173A RID: 5946 RVA: 0x00013C58 File Offset: 0x00011E58
	public override void Awake()
	{
		base.Awake();
		this.SetParryable(true);
		if (base.GetComponent<Collider2D>() == null)
		{
		}
	}

	// Token: 0x0600173B RID: 5947 RVA: 0x00013C78 File Offset: 0x00011E78
	public virtual void OnParryPrePause(AbstractPlayerController player)
	{
		if (this.parrySpark)
		{
			this.parrySpark.Create(base.transform.position);
		}
	}

	// Token: 0x0600173C RID: 5948 RVA: 0x00013CA1 File Offset: 0x00011EA1
	public override void OnParry(AbstractPlayerController player)
	{
		this.SetParryable(false);
		base.StartCoroutine(this.parryCooldown_cr());
	}

	// Token: 0x0600173D RID: 5949 RVA: 0x000A124C File Offset: 0x0009F44C
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
		this.SetParryable(true);
		yield return null;
		yield break;
	}

	// Token: 0x0600173E RID: 5950 RVA: 0x00013CB7 File Offset: 0x00011EB7
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.parrySpark = null;
	}

	// Token: 0x040012E3 RID: 4835
	[SerializeField]
	public Effect parrySpark;

	// Token: 0x040012E4 RID: 4836
	[SerializeField]
	public float coolDown = 0.4f;
}
