using System;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class MapParallaxLayer : AbstractPausableComponent
{
	// Token: 0x17000167 RID: 359
	// (get) Token: 0x060007FC RID: 2044 RVA: 0x00007C53 File Offset: 0x00005E53
	public Vector2 _offset
	{
		get
		{
			return this._startPosition - this._cameraStartPosition;
		}
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x00007C6B File Offset: 0x00005E6B
	public void Start()
	{
		this._camera = CupheadMapCamera.Current;
		this._startPosition = base.transform.position;
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x00007C89 File Offset: 0x00005E89
	public void LateUpdate()
	{
		this.UpdateComparative();
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x00075214 File Offset: 0x00073414
	public void UpdateComparative()
	{
		Vector3 position = base.transform.position;
		position.x = this._offset.x + this._camera.transform.position.x * this.percentage;
		position.y = this._offset.y + this._camera.transform.position.y * this.percentage;
		base.transform.position = position;
	}

	// Token: 0x04000627 RID: 1575
	[Range(-3f, 3f)]
	public float percentage;

	// Token: 0x04000628 RID: 1576
	[SerializeField]
	public Vector3 _cameraStartPosition;

	// Token: 0x04000629 RID: 1577
	public CupheadMapCamera _camera;

	// Token: 0x0400062A RID: 1578
	public Vector3 _startPosition;
}
