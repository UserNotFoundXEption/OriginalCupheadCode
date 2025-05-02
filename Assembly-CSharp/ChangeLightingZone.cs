using System;
using UnityEngine;

// Token: 0x02000474 RID: 1140
public class ChangeLightingZone : AbstractCollidableObject
{
	// Token: 0x06003071 RID: 12401 RVA: 0x000E60A8 File Offset: 0x000E42A8
	public void Start()
	{
		this._filter = default(ContactFilter2D).NoFilter();
	}

	// Token: 0x06003072 RID: 12402 RVA: 0x000E60CC File Offset: 0x000E42CC
	public void Update()
	{
		int num = this._collider.OverlapCollider(this._filter, this.buffer);
		for (int i = 0; i < num; i++)
		{
			MapPlayerAnimationController component = this.buffer[i].GetComponent<MapPlayerAnimationController>();
			if (!(component == null))
			{
				float magnitude = (component.transform.position - base.transform.position).magnitude;
				float num2 = Mathf.Clamp(magnitude / this._maxDistance, 0f, 1f);
				component.spriteRenderer.color = Color.Lerp(this._minTint, this._maxTint, num2);
			}
		}
	}

	// Token: 0x0400280D RID: 10253
	[SerializeField]
	public Color _minTint;

	// Token: 0x0400280E RID: 10254
	[SerializeField]
	public Color _maxTint;

	// Token: 0x0400280F RID: 10255
	[SerializeField]
	public BoxCollider2D _collider;

	// Token: 0x04002810 RID: 10256
	[SerializeField]
	public float _maxDistance;

	// Token: 0x04002811 RID: 10257
	public ContactFilter2D _filter;

	// Token: 0x04002812 RID: 10258
	public Collider2D[] buffer = new Collider2D[10];
}
