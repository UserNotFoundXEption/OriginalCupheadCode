using System;
using System.Collections;

// Token: 0x020001B0 RID: 432
public class DevilLevelHandProjectile : BasicProjectile
{
	// Token: 0x0600149E RID: 5278 RVA: 0x000117AB File Offset: 0x0000F9AB
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.animation_cr());
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x0009A4E4 File Offset: 0x000986E4
	public IEnumerator animation_cr()
	{
		this.move = false;
		yield return base.animator.WaitForAnimationToEnd(this, "Projectile", false, true);
		this.move = true;
		yield break;
	}
}
