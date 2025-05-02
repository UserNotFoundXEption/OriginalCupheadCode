using System;
using UnityEngine;

// Token: 0x020000A9 RID: 169
public class ParallaxLayer : AbstractPausableComponent
{
	// Token: 0x17000168 RID: 360
	// (get) Token: 0x06000801 RID: 2049 RVA: 0x00007C99 File Offset: 0x00005E99
	public Vector2 _offset
	{
		get
		{
			return this._startPosition - this._cameraStartPosition;
		}
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x00007CB1 File Offset: 0x00005EB1
	public virtual void Start()
	{
		this._camera = CupheadLevelCamera.Current;
		this._startPosition = base.transform.position;
		this._cameraStartPosition = this._camera.transform.position;
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x000752A4 File Offset: 0x000734A4
	public void LateUpdate()
	{
		switch (this.type)
		{
		case ParallaxLayer.Type.MinMax:
			this.UpdateMinMax();
			break;
		default:
			this.UpdateComparative();
			break;
		case ParallaxLayer.Type.Centered:
			this.UpdateCentered();
			break;
		}
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x000752F0 File Offset: 0x000734F0
	public virtual void UpdateComparative()
	{
		Vector3 position = base.transform.position;
		position.x = this._offset.x + this._camera.transform.position.x * this.percentage;
		position.y = this._offset.y + this._camera.transform.position.y * this.percentage;
		base.transform.position = position;
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x00075380 File Offset: 0x00073580
	public virtual void UpdateMinMax()
	{
		Vector3 position = base.transform.position;
		Vector2 vector = this._camera.transform.position;
		Vector2 zero = Vector2.zero;
		float num = vector.x + Mathf.Abs(this._camera.Left);
		float num2 = this._camera.Right + Mathf.Abs(this._camera.Left);
		float num3 = vector.y + Mathf.Abs(this._camera.Bottom);
		float num4 = this._camera.Top + Mathf.Abs(this._camera.Bottom);
		if (this.overrideCameraRange)
		{
			num = vector.x + Mathf.Abs(this.overrideCameraX.min);
			num3 = vector.y + Mathf.Abs(this.overrideCameraY.min);
			num2 = this.overrideCameraX.max - this.overrideCameraX.min;
			num4 = this.overrideCameraY.max - this.overrideCameraY.min;
		}
		zero.x = num / num2;
		zero.y = num3 / num4;
		if (float.IsNaN(zero.x))
		{
			zero.x = 0.5f;
		}
		if (float.IsNaN(zero.y))
		{
			zero.y = 0.5f;
		}
		position.x = Mathf.Lerp(this.bottomLeft.x, this.topRight.x, zero.x) + this._camera.transform.position.x;
		position.y = Mathf.Lerp(this.bottomLeft.y, this.topRight.y, zero.y) + this._camera.transform.position.y;
		base.transform.position = position;
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x00075578 File Offset: 0x00073778
	public void UpdateCentered()
	{
		Vector3 position = base.transform.position;
		position.x = this._startPosition.x + (this._camera.transform.position.x - this._startPosition.x) * this.percentage;
		position.y = this._startPosition.y + (this._camera.transform.position.y - this._startPosition.y) * this.percentage;
		base.transform.position = position;
	}

	// Token: 0x0400062B RID: 1579
	public ParallaxLayer.Type type;

	// Token: 0x0400062C RID: 1580
	[Range(-3f, 3f)]
	public float percentage;

	// Token: 0x0400062D RID: 1581
	public Vector2 bottomLeft;

	// Token: 0x0400062E RID: 1582
	public Vector2 topRight;

	// Token: 0x0400062F RID: 1583
	public CupheadLevelCamera _camera;

	// Token: 0x04000630 RID: 1584
	public bool _initialized;

	// Token: 0x04000631 RID: 1585
	public Vector3 _startPosition;

	// Token: 0x04000632 RID: 1586
	public Vector3 _cameraStartPosition;

	// Token: 0x04000633 RID: 1587
	[SerializeField]
	public bool overrideCameraRange;

	// Token: 0x04000634 RID: 1588
	[SerializeField]
	public MinMax overrideCameraX;

	// Token: 0x04000635 RID: 1589
	[SerializeField]
	public MinMax overrideCameraY;

	// Token: 0x02000901 RID: 2305
	public enum Type
	{
		// Token: 0x04004479 RID: 17529
		MinMax,
		// Token: 0x0400447A RID: 17530
		Comparative,
		// Token: 0x0400447B RID: 17531
		Centered
	}
}
