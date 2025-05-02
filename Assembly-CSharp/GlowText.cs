using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

// Token: 0x020000E3 RID: 227
public class GlowText : MonoBehaviour
{
	// Token: 0x06000AA7 RID: 2727 RVA: 0x0007BF5C File Offset: 0x0007A15C
	public void InitTMPText(params MaskableGraphic[] tmp_texts)
	{
		if (tmp_texts.Length > this.tmpTextsToGlow.Length)
		{
			return;
		}
		for (int i = 0; i < tmp_texts.Length; i++)
		{
			if (tmp_texts[i] is Text)
			{
				Text text = tmp_texts[i] as Text;
				this.tmpTextsToGlow[i].enabled = true;
				this.tmpTextsToGlow[i].text = text.text;
				this.tmpTextsToGlow[i].fontSize = (float)text.fontSize;
			}
			else if (tmp_texts[i] is TextMeshProUGUI)
			{
				TextMeshProUGUI textMeshProUGUI = tmp_texts[i] as TextMeshProUGUI;
				tmp_texts[i] = (tmp_texts[i] as Text);
				this.tmpTextsToGlow[i].enabled = true;
				this.tmpTextsToGlow[i].text = textMeshProUGUI.text;
				this.tmpTextsToGlow[i].fontSize = textMeshProUGUI.fontSize;
				this.tmpTextsToGlow[i].font = textMeshProUGUI.font;
				this.tmpTextsToGlow[i].outlineWidth = textMeshProUGUI.outlineWidth;
			}
		}
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x0007C05C File Offset: 0x0007A25C
	public void DisableTMPText()
	{
		for (int i = 0; i < this.tmpTextsToGlow.Length; i++)
		{
			this.tmpTextsToGlow[i].enabled = false;
		}
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x0007C090 File Offset: 0x0007A290
	public void InitImages(params Image[] images)
	{
		if (images.Length > this.imagesToGlow.Length)
		{
			return;
		}
		for (int i = 0; i < images.Length; i++)
		{
			this.imagesToGlow[i].enabled = true;
			this.imagesToGlow[i].sprite = images[i].sprite;
			this.imagesToGlow[i].color = images[i].color;
		}
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x0007C0FC File Offset: 0x0007A2FC
	public void DisableImages()
	{
		for (int i = 0; i < this.imagesToGlow.Length; i++)
		{
			this.imagesToGlow[i].enabled = false;
		}
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x00009A22 File Offset: 0x00007C22
	public void BeginGlow()
	{
		this.rawImageGlow.enabled = true;
		base.StartCoroutine(this.Glow_cr());
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x00009A3D File Offset: 0x00007C3D
	public void StopGlow()
	{
		this.rawImageGlow.enabled = false;
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0007C130 File Offset: 0x0007A330
	public IEnumerator Glow_cr()
	{
		RenderTexture rt = RenderTexture.active;
		RenderTexture.active = this.renderTextureGlow;
		GL.Clear(true, true, Color.clear);
		RenderTexture.active = rt;
		this.cameraGlow.GetComponent<PostProcessingBehaviour>().enabled = true;
		yield return null;
		this.cameraGlow.GetComponent<PostProcessingBehaviour>().enabled = false;
		yield break;
	}

	// Token: 0x04000854 RID: 2132
	[SerializeField]
	public RenderTexture renderTextureGlow;

	// Token: 0x04000855 RID: 2133
	[SerializeField]
	public GameObject cameraGlow;

	// Token: 0x04000856 RID: 2134
	[SerializeField]
	public RawImage rawImageGlow;

	// Token: 0x04000857 RID: 2135
	[SerializeField]
	public TextMeshProUGUI[] tmpTextsToGlow;

	// Token: 0x04000858 RID: 2136
	[SerializeField]
	public Image[] imagesToGlow;
}
