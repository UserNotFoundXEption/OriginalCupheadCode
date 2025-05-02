using System;
using UnityEngine;

// Token: 0x02000138 RID: 312
public class AirshipLevelKnob : ParrySwitch
{
	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x0000C83F File Offset: 0x0000AA3F
	// (set) Token: 0x06000EC6 RID: 3782 RVA: 0x0000C84C File Offset: 0x0000AA4C
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

	// Token: 0x06000EC7 RID: 3783 RVA: 0x0008C3E0 File Offset: 0x0008A5E0
	public static AirshipLevelKnob Create(Transform root)
	{
		GameObject gameObject = new GameObject("Airship_Knob");
		AirshipLevelKnob airshipLevelKnob = gameObject.AddComponent<AirshipLevelKnob>();
		airshipLevelKnob.target = root;
		airshipLevelKnob.tag = "ParrySwitch";
		return airshipLevelKnob;
	}

	// Token: 0x06000EC8 RID: 3784 RVA: 0x0008C414 File Offset: 0x0008A614
	public override void Awake()
	{
		base.Awake();
		CircleCollider2D circleCollider2D = base.gameObject.AddComponent<CircleCollider2D>();
		circleCollider2D.radius = 20f;
		circleCollider2D.isTrigger = true;
	}

	// Token: 0x06000EC9 RID: 3785 RVA: 0x0000C85A File Offset: 0x0000AA5A
	public void Update()
	{
		this.UpdateLocation();
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x0000C862 File Offset: 0x0000AA62
	public void LateUpdate()
	{
		this.UpdateLocation();
	}

	// Token: 0x06000ECB RID: 3787 RVA: 0x0000C86A File Offset: 0x0000AA6A
	public void UpdateLocation()
	{
		if (this.target != null)
		{
			base.transform.position = this.target.position;
		}
	}

	// Token: 0x04000C1D RID: 3101
	public Transform target;
}
