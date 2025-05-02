using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000188 RID: 392
public class ChessKnightLevelLightningController : AbstractMonoBehaviour
{
	// Token: 0x060012B0 RID: 4784 RVA: 0x0000FC4E File Offset: 0x0000DE4E
	public void Start()
	{
		base.StartCoroutine(this.lightning_cr());
	}

	// Token: 0x060012B1 RID: 4785 RVA: 0x00095B94 File Offset: 0x00093D94
	public IEnumerator lightning_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.lightningDelayRange.RandomFloat());
			base.animator.Play((Random.Range(0f, 3f) >= 1f) ? "Short" : "Long");
			base.animator.Update(0f);
			if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Long"))
			{
				this.SFX_KOG_Thunder();
			}
			yield return base.animator.WaitForAnimationToStart(this, "None", false);
		}
		yield break;
	}

	// Token: 0x060012B2 RID: 4786 RVA: 0x00095BB0 File Offset: 0x00093DB0
	public void LateUpdate()
	{
		int num = (int)(this.rend.sprite.name[this.rend.sprite.name.Length - 1] - '1');
		if (num == 51)
		{
			num = 3;
		}
		this.glowTexture.enabled = (num < 3);
		if (this.glowTexture.enabled)
		{
			this.glowTexture.material.SetColor("_OutlineColor", new Color(1f, 1f, 1f, this.glowIntensity[num]));
			this.glowTexture.material.SetFloat("_DimFactor", this.glowIntensity[num] * 0.6f);
		}
	}

	// Token: 0x060012B3 RID: 4787 RVA: 0x0000FC5D File Offset: 0x0000DE5D
	public void SFX_KOG_Thunder()
	{
		AudioManager.Play("sfx_dlc_kog_knight_castlethunder");
	}

	// Token: 0x04000F01 RID: 3841
	[SerializeField]
	public MinMax lightningDelayRange = new MinMax(3f, 8f);

	// Token: 0x04000F02 RID: 3842
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x04000F03 RID: 3843
	[SerializeField]
	public Renderer glowTexture;

	// Token: 0x04000F04 RID: 3844
	[SerializeField]
	public float[] glowIntensity = new float[]
	{
		0.8f,
		0.4f,
		0.1f
	};
}
