using System;
using UnityEngine;

// Token: 0x020000AD RID: 173
public class TriggerZone : MonoBehaviour
{
	// Token: 0x06000825 RID: 2085 RVA: 0x00007D8C File Offset: 0x00005F8C
	public void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(base.transform.position, this.size);
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x00075A38 File Offset: 0x00073C38
	public bool Contains(Vector3 position)
	{
		Rect zero = Rect.zero;
		zero.size = this.size;
		zero.center = base.transform.position;
		return zero.Contains(position);
	}

	// Token: 0x0400063B RID: 1595
	[SerializeField]
	public Vector2 size;
}
