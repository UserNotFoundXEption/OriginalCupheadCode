using System;
using UnityEngine;

// Token: 0x020004BC RID: 1212
public class AbstractMapEquipUICardSide : AbstractMonoBehaviour
{
	// Token: 0x170003AD RID: 941
	// (get) Token: 0x06003264 RID: 12900 RVA: 0x00029C98 File Offset: 0x00027E98
	// (set) Token: 0x06003265 RID: 12901 RVA: 0x00029CA0 File Offset: 0x00027EA0
	public PlayerId playerID { get; set; }

	// Token: 0x06003266 RID: 12902 RVA: 0x00029CA9 File Offset: 0x00027EA9
	public override void Awake()
	{
		base.Awake();
		this.canvasGroup = base.GetComponent<CanvasGroup>();
	}

	// Token: 0x06003267 RID: 12903 RVA: 0x00029CBD File Offset: 0x00027EBD
	public virtual void Init(PlayerId playerID)
	{
		this.playerID = playerID;
	}

	// Token: 0x06003268 RID: 12904 RVA: 0x00029CC6 File Offset: 0x00027EC6
	public void SetActive(bool active)
	{
		this.canvasGroup.alpha = (float)((!active) ? 0 : 1);
	}

	// Token: 0x0400293B RID: 10555
	public CanvasGroup canvasGroup;
}
