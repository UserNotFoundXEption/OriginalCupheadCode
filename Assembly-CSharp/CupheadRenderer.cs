using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000596 RID: 1430
public class CupheadRenderer : AbstractMonoBehaviour
{
	// Token: 0x06003C59 RID: 15449 RVA: 0x00030CD8 File Offset: 0x0002EED8
	public override void Awake()
	{
		base.Awake();
		if (CupheadRenderer.Instance == null)
		{
			CupheadRenderer.Instance = this;
			this.Setup();
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06003C5A RID: 15450 RVA: 0x00030D0D File Offset: 0x0002EF0D
	public void OnDestroy()
	{
		if (CupheadRenderer.Instance == this)
		{
			CupheadRenderer.Instance = null;
		}
	}

	// Token: 0x06003C5B RID: 15451 RVA: 0x00030D25 File Offset: 0x0002EF25
	public void Setup()
	{
		this.rendererCamera = Object.Instantiate<CupheadRendererCamera>(this.cameraPrefab);
		this.rendererCamera.transform.SetParent(base.transform);
		this.rendererCamera.transform.ResetLocalTransforms();
	}

	// Token: 0x06003C5C RID: 15452 RVA: 0x00030D5E File Offset: 0x0002EF5E
	public void TouchFuzzy(float amount, float speed, float time)
	{
		this.rendererCamera.GetComponent<ChromaticAberrationFilmGrain>().PsychedelicEffect(amount, speed, time);
		base.StartCoroutine(this.change_blur_cr(time));
	}

	// Token: 0x06003C5D RID: 15453 RVA: 0x001155DC File Offset: 0x001137DC
	public IEnumerator change_blur_cr(float time)
	{
		float t = 0f;
		float incrementTime = 1f;
		float blurStart = this.rendererCamera.GetComponent<BlurGamma>().blurSize;
		this.rendererCamera.GetComponent<BlurGamma>().blurSize += incrementTime;
		while (this.rendererCamera.GetComponent<BlurGamma>().blurSize > blurStart)
		{
			t += Time.deltaTime;
			if (t >= time / 2f)
			{
				this.rendererCamera.GetComponent<BlurGamma>().blurSize -= incrementTime * Time.deltaTime;
			}
			else
			{
				this.rendererCamera.GetComponent<BlurGamma>().blurSize += incrementTime * Time.deltaTime;
			}
			yield return null;
		}
		this.rendererCamera.GetComponent<BlurGamma>().blurSize = blurStart;
		yield break;
	}

	// Token: 0x04002FE6 RID: 12262
	public static CupheadRenderer Instance;

	// Token: 0x04002FE7 RID: 12263
	[SerializeField]
	public CupheadRendererCamera cameraPrefab;

	// Token: 0x04002FE8 RID: 12264
	public CupheadRendererCamera rendererCamera;

	// Token: 0x04002FE9 RID: 12265
	public Camera bgCamera;

	// Token: 0x04002FEA RID: 12266
	public Canvas canvas;

	// Token: 0x04002FEB RID: 12267
	public Dictionary<CupheadRenderer.RenderLayer, RectTransform> rendererParents;

	// Token: 0x04002FEC RID: 12268
	public Image background;

	// Token: 0x04002FED RID: 12269
	public Image fader;

	// Token: 0x04002FEE RID: 12270
	public bool fuzzyEffectPlaying;

	// Token: 0x02001216 RID: 4630
	public enum RenderLayer
	{
		// Token: 0x04007D77 RID: 32119
		None,
		// Token: 0x04007D78 RID: 32120
		Game,
		// Token: 0x04007D79 RID: 32121
		UI,
		// Token: 0x04007D7A RID: 32122
		Loader
	}
}
