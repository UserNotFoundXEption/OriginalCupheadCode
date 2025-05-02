using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000074 RID: 116
public class UIImageAnimationLoop : AbstractMonoBehaviour
{
	// Token: 0x060005C2 RID: 1474 RVA: 0x000061B7 File Offset: 0x000043B7
	public override void Awake()
	{
		base.Awake();
		this.image = base.GetComponent<Image>();
		this.ignoreGlobalTime = this.IgnoreGlobalTime;
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x000061D7 File Offset: 0x000043D7
	public void Start()
	{
		base.StartCoroutine(this.anim_cr());
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x0006D798 File Offset: 0x0006B998
	public void Shuffle()
	{
		List<Sprite> list = new List<Sprite>(this.sprites);
		Random random = new Random();
		int i = list.Count;
		while (i > 1)
		{
			i--;
			int index = random.Next(i + 1);
			Sprite value = list[index];
			list[index] = list[i];
			list[i] = value;
		}
		this.sprites = list.ToArray();
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x0006D804 File Offset: 0x0006BA04
	public IEnumerator anim_cr()
	{
		if (this.mode == UIImageAnimationLoop.Mode.Shuffle)
		{
			this.Shuffle();
		}
		YieldInstruction waitInstruction = new WaitForSeconds(this.frameDelay);
		int i = 0;
		for (;;)
		{
			this.image.sprite = this.sprites[i];
			i++;
			if (i >= this.sprites.Length)
			{
				i = 0;
			}
			if (!this.IgnoreGlobalTime)
			{
				float t = 0f;
				while (t < this.frameDelay)
				{
					t += CupheadTime.Delta[CupheadTime.Layer.Default];
					yield return null;
				}
			}
			else
			{
				yield return waitInstruction;
			}
			if (this.mode == UIImageAnimationLoop.Mode.Random)
			{
				this.Shuffle();
			}
		}
		yield break;
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x000061E6 File Offset: 0x000043E6
	public void OnDestroy()
	{
		this.sprites = null;
	}

	// Token: 0x040004B6 RID: 1206
	[SerializeField]
	public UIImageAnimationLoop.Mode mode;

	// Token: 0x040004B7 RID: 1207
	[SerializeField]
	public float frameDelay = 0.07f;

	// Token: 0x040004B8 RID: 1208
	[SerializeField]
	public Sprite[] sprites;

	// Token: 0x040004B9 RID: 1209
	[SerializeField]
	public bool IgnoreGlobalTime;

	// Token: 0x040004BA RID: 1210
	public Image image;

	// Token: 0x020008B6 RID: 2230
	public enum Mode
	{
		// Token: 0x040042AB RID: 17067
		Linear,
		// Token: 0x040042AC RID: 17068
		Shuffle,
		// Token: 0x040042AD RID: 17069
		Random
	}
}
