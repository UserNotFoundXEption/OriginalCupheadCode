using System;
using UnityEngine;

// Token: 0x02000140 RID: 320
public class AirshipJellyLevelKnob : ParrySwitch
{
	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06000F28 RID: 3880 RVA: 0x0000CD00 File Offset: 0x0000AF00
	// (set) Token: 0x06000F29 RID: 3881 RVA: 0x0000CD0D File Offset: 0x0000AF0D
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

	// Token: 0x06000F2A RID: 3882 RVA: 0x0008D100 File Offset: 0x0008B300
	public static AirshipJellyLevelKnob Create(AirshipJellyLevelJelly jelly)
	{
		GameObject gameObject = new GameObject("Airship_Jelly_Knob");
		AirshipJellyLevelKnob airshipJellyLevelKnob = gameObject.AddComponent<AirshipJellyLevelKnob>();
		airshipJellyLevelKnob.target = jelly.knobRoot;
		airshipJellyLevelKnob.tag = "ParrySwitch";
		return airshipJellyLevelKnob;
	}

	// Token: 0x06000F2B RID: 3883 RVA: 0x0008D138 File Offset: 0x0008B338
	public override void Awake()
	{
		base.Awake();
		CircleCollider2D circleCollider2D = base.gameObject.AddComponent<CircleCollider2D>();
		circleCollider2D.radius = 20f;
		circleCollider2D.isTrigger = true;
	}

	// Token: 0x06000F2C RID: 3884 RVA: 0x0000CD1B File Offset: 0x0000AF1B
	public void Update()
	{
		this.UpdateLocation();
	}

	// Token: 0x06000F2D RID: 3885 RVA: 0x0000CD23 File Offset: 0x0000AF23
	public void LateUpdate()
	{
		this.UpdateLocation();
	}

	// Token: 0x06000F2E RID: 3886 RVA: 0x0000CD2B File Offset: 0x0000AF2B
	public void UpdateLocation()
	{
		if (this.target != null)
		{
			base.transform.position = this.target.position;
		}
	}

	// Token: 0x04000C67 RID: 3175
	public Transform target;
}
