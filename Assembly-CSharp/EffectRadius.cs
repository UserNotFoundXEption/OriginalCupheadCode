using System;
using UnityEngine;

// Token: 0x020005A4 RID: 1444
public class EffectRadius : AbstractPausableComponent
{
	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x06003CF7 RID: 15607 RVA: 0x0003136B File Offset: 0x0002F56B
	public float radius
	{
		get
		{
			return this._radius;
		}
	}

	// Token: 0x17000501 RID: 1281
	// (get) Token: 0x06003CF8 RID: 15608 RVA: 0x00031373 File Offset: 0x0002F573
	public Vector2 offset
	{
		get
		{
			return this._offset;
		}
	}

	// Token: 0x06003CF9 RID: 15609 RVA: 0x001176B8 File Offset: 0x001158B8
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(1f, 0f, 0f, 1f);
		Gizmos.DrawWireSphere(base.baseTransform.position + this.offset, this.radius);
	}

	// Token: 0x06003CFA RID: 15610 RVA: 0x00117714 File Offset: 0x00115914
	public void CreateInRadius()
	{
		Vector2 vector = base.baseTransform.position + this.offset;
		Vector2 vector2;
		vector2..ctor(Random.value * (float)((!Rand.Bool()) ? -1 : 1), Random.value * (float)((!Rand.Bool()) ? -1 : 1));
		this.target = vector + vector2.normalized * this.radius * Random.value;
		this.effect.Create(this.target);
	}

	// Token: 0x04003075 RID: 12405
	[SerializeField]
	public Effect effect;

	// Token: 0x04003076 RID: 12406
	[SerializeField]
	public float _radius = 100f;

	// Token: 0x04003077 RID: 12407
	[SerializeField]
	public Vector2 _offset = Vector2.zero;

	// Token: 0x04003078 RID: 12408
	public Vector2 target;
}
