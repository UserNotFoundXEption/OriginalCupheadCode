using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000187 RID: 391
public class ChessKnightLevelKnightDeathArmor : AbstractPausableComponent
{
	// Token: 0x060012AD RID: 4781 RVA: 0x0000FBEF File Offset: 0x0000DDEF
	public void Start()
	{
		base.animator.Play(this.type.ToString());
		base.StartCoroutine(this.grow_cr());
	}

	// Token: 0x060012AE RID: 4782 RVA: 0x00095B78 File Offset: 0x00093D78
	public IEnumerator grow_cr()
	{
		float elapsed = 0f;
		WaitForFrameTimePersistent wait = new WaitForFrameTimePersistent(0.0416666679f, false);
		for (;;)
		{
			yield return wait;
			elapsed += wait.frameTime + wait.accumulator;
			Vector3 scale = base.transform.localScale;
			scale.x = (1f + elapsed * this.growthSpeed) * Mathf.Sign(scale.x);
			scale.y = 1f + elapsed * this.growthSpeed;
			base.transform.localScale = scale;
		}
		yield break;
	}

	// Token: 0x04000EFF RID: 3839
	[SerializeField]
	public ChessKnightLevelKnightDeathArmor.Type type;

	// Token: 0x04000F00 RID: 3840
	[SerializeField]
	public float growthSpeed;

	// Token: 0x02000AC7 RID: 2759
	public enum Type
	{
		// Token: 0x04004F1F RID: 20255
		Helmet,
		// Token: 0x04004F20 RID: 20256
		Shield,
		// Token: 0x04004F21 RID: 20257
		Sword
	}
}
