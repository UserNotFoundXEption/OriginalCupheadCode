using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200051E RID: 1310
public class CharmTurret : AbstractCollidableObject
{
	// Token: 0x06003770 RID: 14192 RVA: 0x001035C4 File Offset: 0x001017C4
	public void Init(GameObject rootObject, float circleSpeed, float projectileSpeed, float delay)
	{
		base.transform.position = rootObject.transform.position;
		this.rootObject = rootObject;
		this.circleSpeed = circleSpeed;
		this.projectileSpeed = projectileSpeed;
		this.delay = delay;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x06003771 RID: 14193 RVA: 0x00103620 File Offset: 0x00101820
	public IEnumerator move_cr()
	{
		for (;;)
		{
			this.angle += this.circleSpeed * CupheadTime.FixedDelta;
			Vector3 handleRotationX = new Vector3(-Mathf.Sin(this.angle) * this.loopSize, 0f, 0f);
			Vector3 handleRotationY = new Vector3(0f, Mathf.Cos(this.angle) * this.loopSize, 0f);
			base.transform.position = this.rootObject.transform.position;
			base.transform.position += handleRotationX + handleRotationY;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003772 RID: 14194 RVA: 0x0010363C File Offset: 0x0010183C
	public IEnumerator shoot_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.delay);
			this.projectile.Create(base.transform.position, 0f, this.projectileSpeed);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002C96 RID: 11414
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x04002C97 RID: 11415
	public float circleSpeed;

	// Token: 0x04002C98 RID: 11416
	public float projectileSpeed;

	// Token: 0x04002C99 RID: 11417
	public float delay;

	// Token: 0x04002C9A RID: 11418
	public float angle;

	// Token: 0x04002C9B RID: 11419
	public float loopSize = 200f;

	// Token: 0x04002C9C RID: 11420
	public GameObject rootObject;
}
