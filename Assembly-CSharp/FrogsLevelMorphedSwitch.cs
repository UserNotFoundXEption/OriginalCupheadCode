using System;
using UnityEngine;

// Token: 0x0200029F RID: 671
public class FrogsLevelMorphedSwitch : ParrySwitch
{
	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x06001E47 RID: 7751 RVA: 0x00019892 File Offset: 0x00017A92
	// (set) Token: 0x06001E48 RID: 7752 RVA: 0x0001989F File Offset: 0x00017A9F
	public bool enabled
	{
		get
		{
			return base.GetComponent<Collider2D>().enabled;
		}
		set
		{
			base.GetComponent<Collider2D>().enabled = value;
		}
	}

	// Token: 0x06001E49 RID: 7753 RVA: 0x000B29A8 File Offset: 0x000B0BA8
	public static FrogsLevelMorphedSwitch Create(FrogsLevelMorphed parent)
	{
		GameObject gameObject = new GameObject("Frogs_Morphed_Handle");
		FrogsLevelMorphedSwitch frogsLevelMorphedSwitch = gameObject.AddComponent<FrogsLevelMorphedSwitch>();
		frogsLevelMorphedSwitch.target = parent.switchRoot;
		return frogsLevelMorphedSwitch;
	}

	// Token: 0x06001E4A RID: 7754 RVA: 0x000B29D4 File Offset: 0x000B0BD4
	public override void Awake()
	{
		base.Awake();
		CircleCollider2D circleCollider2D = base.gameObject.AddComponent<CircleCollider2D>();
		circleCollider2D.radius = 50f;
		circleCollider2D.isTrigger = true;
	}

	// Token: 0x06001E4B RID: 7755 RVA: 0x000198AD File Offset: 0x00017AAD
	public void Update()
	{
		this.UpdateLocation();
	}

	// Token: 0x06001E4C RID: 7756 RVA: 0x000198B5 File Offset: 0x00017AB5
	public void LateUpdate()
	{
		this.UpdateLocation();
	}

	// Token: 0x06001E4D RID: 7757 RVA: 0x000198BD File Offset: 0x00017ABD
	public void UpdateLocation()
	{
		if (this.target != null)
		{
			base.transform.position = this.target.position;
		}
	}

	// Token: 0x040018C2 RID: 6338
	public Transform target;
}
