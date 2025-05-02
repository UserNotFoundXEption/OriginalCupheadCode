using System;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class DLCCutsceneParallaxLayer : AbstractPausableComponent
{
	// Token: 0x17000165 RID: 357
	// (get) Token: 0x060007DD RID: 2013 RVA: 0x00007A0F File Offset: 0x00005C0F
	public Vector2 _offset
	{
		get
		{
			return this._startPosition - this._cameraStartPosition;
		}
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00007A27 File Offset: 0x00005C27
	public virtual void Start()
	{
		this._camera = CupheadCutsceneCamera.Current;
		this._startPosition = base.transform.position;
		this._cameraStartPosition = this._camera.transform.position;
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x00074948 File Offset: 0x00072B48
	public void LateUpdate()
	{
		DLCCutsceneParallaxLayer.Type type = this.type;
		if (type == DLCCutsceneParallaxLayer.Type.Comparative || type != DLCCutsceneParallaxLayer.Type.Centered)
		{
			this.UpdateComparative();
		}
		else
		{
			this.UpdateCentered();
		}
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00074988 File Offset: 0x00072B88
	public virtual void UpdateComparative()
	{
		Vector3 position = base.transform.position;
		position.x = this._offset.x + this._camera.transform.position.x * this.percentage;
		position.y = this._offset.y + this._camera.transform.position.y * this.percentage;
		base.transform.position = position;
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x00074A18 File Offset: 0x00072C18
	public void UpdateCentered()
	{
		Vector3 position = base.transform.position;
		position.x = this._startPosition.x + (this._camera.transform.position.x - this._startPosition.x) * this.percentage;
		position.y = this._startPosition.y + (this._camera.transform.position.y - this._startPosition.y) * this.percentage;
		base.transform.position = position;
	}

	// Token: 0x040005F7 RID: 1527
	public DLCCutsceneParallaxLayer.Type type;

	// Token: 0x040005F8 RID: 1528
	[Range(-3f, 3f)]
	public float percentage;

	// Token: 0x040005F9 RID: 1529
	public Vector2 bottomLeft;

	// Token: 0x040005FA RID: 1530
	public Vector2 topRight;

	// Token: 0x040005FB RID: 1531
	public AbstractCupheadCamera _camera;

	// Token: 0x040005FC RID: 1532
	public bool _initialized;

	// Token: 0x040005FD RID: 1533
	public Vector3 _startPosition;

	// Token: 0x040005FE RID: 1534
	public Vector3 _cameraStartPosition;

	// Token: 0x040005FF RID: 1535
	[SerializeField]
	public bool overrideCameraRange;

	// Token: 0x04000600 RID: 1536
	[SerializeField]
	public MinMax overrideCameraX;

	// Token: 0x04000601 RID: 1537
	[SerializeField]
	public MinMax overrideCameraY;

	// Token: 0x020008FB RID: 2299
	public enum Type
	{
		// Token: 0x04004455 RID: 17493
		MinMax,
		// Token: 0x04004456 RID: 17494
		Comparative,
		// Token: 0x04004457 RID: 17495
		Centered
	}
}
