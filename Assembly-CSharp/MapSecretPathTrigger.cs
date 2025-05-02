using System;
using UnityEngine;

// Token: 0x020004A2 RID: 1186
public class MapSecretPathTrigger : AbstractMonoBehaviour
{
	// Token: 0x06003173 RID: 12659 RVA: 0x00029241 File Offset: 0x00027441
	public void Start()
	{
		this.size = base.GetComponent<BoxCollider2D>().size;
	}

	// Token: 0x06003174 RID: 12660 RVA: 0x000E9C0C File Offset: 0x000E7E0C
	public bool PointInBounds(Vector3 pos)
	{
		return pos.x > base.transform.position.x - this.size.x / 2f && pos.x < base.transform.position.x + this.size.x / 2f && pos.y > base.transform.position.y - this.size.y / 2f && pos.y < base.transform.position.y + this.size.y / 2f;
	}

	// Token: 0x06003175 RID: 12661 RVA: 0x000E9CE0 File Offset: 0x000E7EE0
	public void OnTriggerStay2D(Collider2D collider)
	{
		MapPlayerController component = collider.GetComponent<MapPlayerController>();
		if (component && this.PointInBounds(component.transform.position))
		{
			component.SecretPathEnter(this.enablePath);
		}
	}

	// Token: 0x040028BB RID: 10427
	[SerializeField]
	public bool enablePath;

	// Token: 0x040028BC RID: 10428
	public Vector2 size;
}
