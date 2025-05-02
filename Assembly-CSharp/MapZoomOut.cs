using System;
using UnityEngine;

// Token: 0x020004A9 RID: 1193
public class MapZoomOut : AbstractCollidableObject
{
	// Token: 0x0600318B RID: 12683 RVA: 0x000EA124 File Offset: 0x000E8324
	public void Start()
	{
		this._filter = default(ContactFilter2D).NoFilter();
		this._startSize = this._camera.camera.orthographicSize;
		this._collider = base.GetComponent<BoxCollider2D>();
		this._maxDistance = this._collider.bounds.extents.y;
		this._zoomDistance = this._maxZoomOut - this._startSize;
	}

	// Token: 0x0600318C RID: 12684 RVA: 0x000EA19C File Offset: 0x000E839C
	public void Update()
	{
		int num = this._collider.OverlapCollider(this._filter, this.buffer);
		float num2 = 0f;
		float num3 = 0f;
		for (int i = 0; i < num; i++)
		{
			MapPlayerController component = this.buffer[i].GetComponent<MapPlayerController>();
			if (!(component == null))
			{
				num3 += 1f;
				float magnitude = (component.transform.position - base.transform.position).magnitude;
				num2 = ((num2 < magnitude) ? magnitude : num2);
			}
		}
		if ((PlayerManager.Multiplayer && num3 == 2f) || (!PlayerManager.Multiplayer && num3 == 1f))
		{
			this._currentZoomRatio = 1f - Mathf.Clamp(num2 / this._maxDistance, 0f, 1f);
		}
		else
		{
			this._currentZoomRatio = 0f;
		}
		this._camera.camera.orthographicSize = Mathf.Lerp(this._camera.camera.orthographicSize, this.EaseInOutQuad(this._startSize, this._zoomDistance, this._currentZoomRatio), Time.deltaTime * this.ZoomSharpness);
	}

	// Token: 0x0600318D RID: 12685 RVA: 0x000EA2E8 File Offset: 0x000E84E8
	public float EaseInOutQuad(float startValue, float endValue, float time)
	{
		time *= 2f;
		if (time < 1f)
		{
			return endValue / 2f * time * time + startValue;
		}
		time -= 1f;
		return -endValue / 2f * (time * (time - 2f) - 1f) + startValue;
	}

	// Token: 0x040028C4 RID: 10436
	[SerializeField]
	public CupheadMapCamera _camera;

	// Token: 0x040028C5 RID: 10437
	[SerializeField]
	public float _maxZoomOut;

	// Token: 0x040028C6 RID: 10438
	[SerializeField]
	public float ZoomSharpness = 1f;

	// Token: 0x040028C7 RID: 10439
	public float _startSize;

	// Token: 0x040028C8 RID: 10440
	public float _maxDistance;

	// Token: 0x040028C9 RID: 10441
	public float _zoomDistance;

	// Token: 0x040028CA RID: 10442
	public float _currentZoomRatio;

	// Token: 0x040028CB RID: 10443
	public BoxCollider2D _collider;

	// Token: 0x040028CC RID: 10444
	public ContactFilter2D _filter;

	// Token: 0x040028CD RID: 10445
	public Collider2D[] buffer = new Collider2D[10];
}
