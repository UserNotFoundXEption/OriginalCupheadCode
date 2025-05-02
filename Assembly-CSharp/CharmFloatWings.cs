using System;
using UnityEngine;

// Token: 0x0200051B RID: 1307
public class CharmFloatWings : MonoBehaviour
{
	// Token: 0x06003764 RID: 14180 RVA: 0x001031E0 File Offset: 0x001013E0
	public void Start()
	{
		if (this.useWindEffect)
		{
			SpriteRenderer[] componentsInChildren = base.GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer spriteRenderer in componentsInChildren)
			{
				spriteRenderer.enabled = false;
			}
		}
	}

	// Token: 0x06003765 RID: 14181 RVA: 0x0002D41B File Offset: 0x0002B61B
	public void OnEnable()
	{
		if (!this.useWindEffect)
		{
			this.SpawnFeathers();
		}
	}

	// Token: 0x06003766 RID: 14182 RVA: 0x0002D42E File Offset: 0x0002B62E
	public void OnDisable()
	{
		if (!this.useWindEffect)
		{
			this.SpawnFeathers();
		}
	}

	// Token: 0x06003767 RID: 14183 RVA: 0x00103220 File Offset: 0x00101420
	public void SpawnFeathers()
	{
		for (int i = 0; i < 10; i++)
		{
			this.feather.Create(base.transform.position, new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f)));
		}
	}

	// Token: 0x06003768 RID: 14184 RVA: 0x0010327C File Offset: 0x0010147C
	public void Update()
	{
		if (this.useWindEffect)
		{
			this.spawnTime += CupheadTime.Delta;
			if (this.spawnTime > this.spawnFreq)
			{
				this.spawnTime -= this.spawnFreq;
				this.featherAlt.Create(base.transform.parent.position + Vector3.down * 10f);
			}
		}
	}

	// Token: 0x04002C85 RID: 11397
	[SerializeField]
	public Effect feather;

	// Token: 0x04002C86 RID: 11398
	[SerializeField]
	public Effect featherAlt;

	// Token: 0x04002C87 RID: 11399
	[SerializeField]
	public bool useWindEffect;

	// Token: 0x04002C88 RID: 11400
	public float spawnFreq = 0.1f;

	// Token: 0x04002C89 RID: 11401
	public float spawnTime;
}
