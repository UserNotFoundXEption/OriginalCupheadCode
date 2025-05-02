using System;
using UnityEngine;

// Token: 0x0200063A RID: 1594
public class ControlMapperResizer : MonoBehaviour
{
	// Token: 0x06004256 RID: 16982 RVA: 0x00136828 File Offset: 0x00134A28
	public void Update()
	{
		float num = Mathf.Clamp((float)Screen.width / (float)Screen.height / 1.77777779f, 0f, 1f);
		if (this.cachedSize != num)
		{
			this.cachedSize = num;
			base.transform.localScale = new Vector3(num, num);
		}
	}

	// Token: 0x0400343C RID: 13372
	public float cachedSize;
}
