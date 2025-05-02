using System;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public class KitchenParallaxLayer : ParallaxLayer
{
	// Token: 0x060007F9 RID: 2041 RVA: 0x00007C33 File Offset: 0x00005E33
	public override void Start()
	{
		base.Start();
		this.level = (Level.Current as SaltbakerLevel);
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x00075100 File Offset: 0x00073300
	public override void UpdateMinMax()
	{
		Vector3 position = base.transform.position;
		if (!this.ignoreX)
		{
			Vector2 vector = this._camera.transform.position;
			Vector2 zero = Vector2.zero;
			float num = vector.x + Mathf.Abs(this._camera.Left);
			float num2 = this._camera.Right + Mathf.Abs(this._camera.Left);
			zero.x = num / num2;
			if (float.IsNaN(zero.x))
			{
				zero.x = 0.5f;
			}
			position.x = Mathf.Lerp(this.bottomLeft.x, this.topRight.x, zero.x) + this._camera.transform.position.x;
		}
		position.y = Mathf.Lerp(this.startY, this.endY, this.level.yScrollPos);
		base.transform.position = position;
	}

	// Token: 0x04000623 RID: 1571
	[SerializeField]
	public float startY;

	// Token: 0x04000624 RID: 1572
	[SerializeField]
	public float endY;

	// Token: 0x04000625 RID: 1573
	[SerializeField]
	public bool ignoreX;

	// Token: 0x04000626 RID: 1574
	public SaltbakerLevel level;
}
