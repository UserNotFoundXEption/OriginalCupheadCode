using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D6 RID: 214
public class HitFlash : AbstractMonoBehaviour
{
	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06000A20 RID: 2592 RVA: 0x000093DA File Offset: 0x000075DA
	// (set) Token: 0x06000A21 RID: 2593 RVA: 0x000093E2 File Offset: 0x000075E2
	public bool flashing { get; set; }

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06000A22 RID: 2594 RVA: 0x000093EB File Offset: 0x000075EB
	// (set) Token: 0x06000A23 RID: 2595 RVA: 0x000093F3 File Offset: 0x000075F3
	public bool disabled { get; set; }

	// Token: 0x06000A24 RID: 2596 RVA: 0x0007AD8C File Offset: 0x00078F8C
	public override void Awake()
	{
		base.Awake();
		if (this.includeSelf)
		{
			SpriteRenderer component = base.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				this.self = new HitFlash.RendererProperties(component);
			}
		}
		this.renderers = new List<HitFlash.RendererProperties>();
		for (int i = 0; i < this.otherRenderers.Length; i++)
		{
			this.renderers.Add(new HitFlash.RendererProperties(this.otherRenderers[i]));
		}
		if (this.damageReceiver == null)
		{
			this.damageReceiver = base.GetComponent<DamageReceiver>();
		}
		if (this.damageReceiver)
		{
			this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		}
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x000093FC File Offset: 0x000075FC
	public void Update()
	{
		this.time -= CupheadTime.Delta;
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x00009415 File Offset: 0x00007615
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.Flash(0.1f);
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x00009422 File Offset: 0x00007622
	public override void StopAllCoroutines()
	{
		base.StopAllCoroutines();
		this.SetColor(1f);
		this.SetScale(Vector3.one, 1f);
		this.time = 0f;
		this.flashing = false;
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x00009457 File Offset: 0x00007657
	public void StopAllCoroutinesWithoutSettingScale()
	{
		base.StopAllCoroutines();
		this.SetColor(1f);
		this.time = 0f;
		this.flashing = false;
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x0007AE4C File Offset: 0x0007904C
	public void Flash(float t = 0.1f)
	{
		if (this.disabled)
		{
			return;
		}
		this.time = t;
		if (this.flashing)
		{
			return;
		}
		if (base.gameObject.activeSelf && base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.flash_cr());
		}
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x0007AEA8 File Offset: 0x000790A8
	public void SetColor(float t)
	{
		if (this.self != null)
		{
			Color color = Color.Lerp(this.self.normalColor, this.damageColor, t);
			this.self.renderer.color = color;
		}
		foreach (HitFlash.RendererProperties rendererProperties in this.renderers)
		{
			Color color2 = Color.Lerp(rendererProperties.normalColor, this.damageColor, t);
			rendererProperties.renderer.color = color2;
		}
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x0007AF50 File Offset: 0x00079150
	public void SetScale(Vector3 original, float s)
	{
		if (this.self != null)
		{
			this.self.transform.localScale = original * s;
		}
		foreach (HitFlash.RendererProperties rendererProperties in this.renderers)
		{
			rendererProperties.transform.localScale = rendererProperties.scale * s;
		}
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x0007AFE0 File Offset: 0x000791E0
	public IEnumerator flash_cr()
	{
		this.flashing = true;
		while (this.time > 0f)
		{
			this.SetColor(1f);
			yield return CupheadTime.WaitForSeconds(this, 0.0416f);
			this.SetColor(0f);
			yield return CupheadTime.WaitForSeconds(this, 0.0832f);
		}
		this.flashing = false;
		yield break;
	}

	// Token: 0x0400081C RID: 2076
	[SerializeField]
	public Color damageColor = new Color(1f, 0f, 0f, 1f);

	// Token: 0x0400081D RID: 2077
	[SerializeField]
	public DamageReceiver damageReceiver;

	// Token: 0x0400081E RID: 2078
	[SerializeField]
	public bool includeSelf = true;

	// Token: 0x0400081F RID: 2079
	public SpriteRenderer[] otherRenderers;

	// Token: 0x04000820 RID: 2080
	public float time;

	// Token: 0x04000823 RID: 2083
	public Coroutine coroutine;

	// Token: 0x04000824 RID: 2084
	public HitFlash.RendererProperties self;

	// Token: 0x04000825 RID: 2085
	public List<HitFlash.RendererProperties> renderers;

	// Token: 0x02000949 RID: 2377
	public class RendererProperties
	{
		// Token: 0x06005488 RID: 21640 RVA: 0x000400E5 File Offset: 0x0003E2E5
		public RendererProperties(SpriteRenderer r)
		{
			this.renderer = r;
			this.normalColor = r.color;
			this.transform = r.transform;
			this.scale = r.transform.localScale;
		}

		// Token: 0x040045E4 RID: 17892
		public readonly SpriteRenderer renderer;

		// Token: 0x040045E5 RID: 17893
		public readonly Color normalColor;

		// Token: 0x040045E6 RID: 17894
		public readonly Transform transform;

		// Token: 0x040045E7 RID: 17895
		public readonly Vector3 scale;
	}
}
