using System;
using UnityEngine;

// Token: 0x020004AF RID: 1199
public class MapSprite : AbstractPausableComponent
{
	// Token: 0x17000395 RID: 917
	// (get) Token: 0x060031B6 RID: 12726 RVA: 0x0002953E File Offset: 0x0002773E
	public virtual bool ChangesDepth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060031B7 RID: 12727 RVA: 0x000EAF6C File Offset: 0x000E916C
	public override void Awake()
	{
		base.Awake();
		this.SetLayer(base.GetComponent<SpriteRenderer>());
		foreach (SpriteRenderer layer in base.GetComponentsInChildren<SpriteRenderer>())
		{
			this.SetLayer(layer);
		}
	}

	// Token: 0x060031B8 RID: 12728 RVA: 0x00029541 File Offset: 0x00027741
	public void SetLayer(SpriteRenderer renderer)
	{
		if (!this.ChangesDepth || renderer == null)
		{
			return;
		}
		renderer.sortingLayerName = "Map";
		renderer.sortingOrder = 0;
	}

	// Token: 0x060031B9 RID: 12729 RVA: 0x000EAFB4 File Offset: 0x000E91B4
	public virtual void Update()
	{
		Vector3 position = base.transform.position;
		base.transform.position = new Vector3(position.x, position.y, position.y + this.zOffset);
	}

	// Token: 0x040028EC RID: 10476
	[SerializeField]
	public float zOffset;
}
