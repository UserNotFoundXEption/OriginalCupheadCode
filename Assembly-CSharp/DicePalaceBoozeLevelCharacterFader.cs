using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001CC RID: 460
public class DicePalaceBoozeLevelCharacterFader : AbstractPausableComponent
{
	// Token: 0x0600158E RID: 5518 RVA: 0x00012564 File Offset: 0x00010764
	public override void Awake()
	{
		base.Awake();
		this.mainSprite = base.GetComponent<SpriteRenderer>();
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x0600158F RID: 5519 RVA: 0x0009C2BC File Offset: 0x0009A4BC
	public IEnumerator main_cr()
	{
		foreach (SpriteRenderer spriteRenderer in this.sprites)
		{
			spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
		}
		this.mainSprite.color = new Color(1f, 1f, 1f, 0f);
		float fadeTime = 0.5f;
		for (;;)
		{
			float holdTime = Random.Range(3f, 6f);
			yield return CupheadTime.WaitForSeconds(this, holdTime);
			if (this.fadingOut)
			{
				float t = 0f;
				while (t < fadeTime)
				{
					this.mainSprite.color = new Color(1f, 1f, 1f, 1f - t / fadeTime);
					foreach (SpriteRenderer spriteRenderer2 in this.sprites)
					{
						spriteRenderer2.color = new Color(1f, 1f, 1f, 1f - t / fadeTime);
					}
					t += CupheadTime.Delta;
					yield return null;
				}
			}
			else
			{
				float t2 = 0f;
				while (t2 < fadeTime)
				{
					this.mainSprite.color = new Color(1f, 1f, 1f, t2 / fadeTime);
					foreach (SpriteRenderer spriteRenderer3 in this.sprites)
					{
						spriteRenderer3.color = new Color(1f, 1f, 1f, t2 / fadeTime);
					}
					t2 += CupheadTime.Delta;
					yield return null;
				}
			}
			this.fadingOut = !this.fadingOut;
		}
		yield break;
	}

	// Token: 0x04001184 RID: 4484
	[SerializeField]
	public SpriteRenderer[] sprites;

	// Token: 0x04001185 RID: 4485
	public SpriteRenderer mainSprite;

	// Token: 0x04001186 RID: 4486
	public bool fadingOut;
}
