using System;
using UnityEngine;

// Token: 0x020005B1 RID: 1457
public class SpriteErrorManager : AbstractMonoBehaviour
{
	// Token: 0x06003D2F RID: 15663 RVA: 0x000315AF File Offset: 0x0002F7AF
	public override void Awake()
	{
		base.Awake();
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		SpriteErrorManager.Pair.InitializePairs(this.errors);
	}

	// Token: 0x06003D30 RID: 15664 RVA: 0x001186F0 File Offset: 0x001168F0
	public void OnWillRenderObject()
	{
		foreach (SpriteErrorManager.Pair pair in this.errors)
		{
			string name = this.spriteRenderer.sprite.name;
			if (name == pair.name)
			{
				if (this.lastFrame == name || pair.chance > Random.Range(1, 101))
				{
					this.spriteRenderer.sprite = pair.sprite;
					this.lastFrame = name;
				}
				return;
			}
			this.lastFrame = string.Empty;
		}
	}

	// Token: 0x040030C3 RID: 12483
	public const string ERROR_STRING = "_error";

	// Token: 0x040030C4 RID: 12484
	[SerializeField]
	public SpriteErrorManager.Pair[] errors;

	// Token: 0x040030C5 RID: 12485
	public string lastFrame;

	// Token: 0x040030C6 RID: 12486
	public SpriteRenderer spriteRenderer;

	// Token: 0x0200123E RID: 4670
	[Serializable]
	public class Pair
	{
		// Token: 0x060080F7 RID: 33015 RVA: 0x00297E28 File Offset: 0x00296028
		public static void InitializePairs(SpriteErrorManager.Pair[] p)
		{
			for (int i = 0; i < p.Length; i++)
			{
				p[i].name = p[i].sprite.name.Replace("_error", string.Empty);
			}
		}

		// Token: 0x04007E67 RID: 32359
		public Sprite sprite;

		// Token: 0x04007E68 RID: 32360
		[Range(1f, 100f)]
		public int chance = 10;

		// Token: 0x04007E69 RID: 32361
		[HideInInspector]
		public string name;
	}
}
