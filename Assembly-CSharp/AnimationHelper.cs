using System;
using UnityEngine;

// Token: 0x02000087 RID: 135
[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class AnimationHelper : AbstractMonoBehaviour
{
	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06000659 RID: 1625 RVA: 0x0000690B File Offset: 0x00004B0B
	// (set) Token: 0x0600065A RID: 1626 RVA: 0x00006913 File Offset: 0x00004B13
	public CupheadTime.Layer Layer
	{
		get
		{
			return this.layer;
		}
		set
		{
			this.layer = value;
			this.Set();
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x0600065B RID: 1627 RVA: 0x00006922 File Offset: 0x00004B22
	// (set) Token: 0x0600065C RID: 1628 RVA: 0x0000692F File Offset: 0x00004B2F
	public float LayerSpeed
	{
		get
		{
			return CupheadTime.GetLayerSpeed(this.Layer);
		}
		set
		{
			CupheadTime.SetLayerSpeed(this.Layer, value);
			this.Set();
		}
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x0600065D RID: 1629 RVA: 0x00006943 File Offset: 0x00004B43
	// (set) Token: 0x0600065E RID: 1630 RVA: 0x0000694B File Offset: 0x00004B4B
	public float Speed
	{
		get
		{
			return this.speed;
		}
		set
		{
			this.speed = value;
			this.Set();
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x0600065F RID: 1631 RVA: 0x0000695A File Offset: 0x00004B5A
	// (set) Token: 0x06000660 RID: 1632 RVA: 0x00006962 File Offset: 0x00004B62
	public bool IgnoreGlobal
	{
		get
		{
			return this.ignoreGlobal;
		}
		set
		{
			this.ignoreGlobal = value;
			this.Set();
		}
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x0006F76C File Offset: 0x0006D96C
	public override void Awake()
	{
		base.Awake();
		if (base.animator == null)
		{
			Debug.LogError("AnimationHelper needs Animator component", null);
			Object.Destroy(this);
			return;
		}
		CupheadTime.OnChangedEvent.Add(new Action(this.Set));
		this.Set();
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x00006971 File Offset: 0x00004B71
	public void Update()
	{
		if (this.autoUpdate)
		{
			this.Set();
		}
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x00006984 File Offset: 0x00004B84
	public void OnDestroy()
	{
		CupheadTime.OnChangedEvent.Remove(new Action(this.Set));
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x0006F7C0 File Offset: 0x0006D9C0
	public void Set()
	{
		if (this.IgnoreGlobal)
		{
			base.animator.speed = this.Speed * this.LayerSpeed;
		}
		else
		{
			base.animator.speed = this.Speed * this.LayerSpeed * CupheadTime.GlobalSpeed;
		}
	}

	// Token: 0x040004D9 RID: 1241
	[SerializeField]
	public CupheadTime.Layer layer;

	// Token: 0x040004DA RID: 1242
	[SerializeField]
	public float speed = 1f;

	// Token: 0x040004DB RID: 1243
	[SerializeField]
	public bool ignoreGlobal;

	// Token: 0x040004DC RID: 1244
	[SerializeField]
	public bool autoUpdate;
}
