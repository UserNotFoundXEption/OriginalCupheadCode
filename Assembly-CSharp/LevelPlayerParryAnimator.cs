using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000514 RID: 1300
public class LevelPlayerParryAnimator : AbstractMonoBehaviour
{
	// Token: 0x060036E7 RID: 14055 RVA: 0x001016E4 File Offset: 0x000FF8E4
	public override void Awake()
	{
		base.Awake();
		this.r = base.GetComponent<SpriteRenderer>();
		foreach (Sprite sprite in this.sprites)
		{
			sprite.name = sprite.name.Replace("_pink", string.Empty);
		}
	}

	// Token: 0x060036E8 RID: 14056 RVA: 0x00101740 File Offset: 0x000FF940
	public void Set()
	{
		foreach (Sprite sprite in this.sprites)
		{
			if (sprite.name.Contains(this.r.sprite.name))
			{
				this.r.sprite = sprite;
				return;
			}
		}
	}

	// Token: 0x060036E9 RID: 14057 RVA: 0x0002CE49 File Offset: 0x0002B049
	public void StartSet()
	{
		base.StartCoroutine(this.set_cr());
		base.StartCoroutine(this.setLate_cr());
	}

	// Token: 0x060036EA RID: 14058 RVA: 0x0002CE65 File Offset: 0x0002B065
	public void StopSet()
	{
		this.StopAllCoroutines();
	}

	// Token: 0x060036EB RID: 14059 RVA: 0x0010179C File Offset: 0x000FF99C
	public IEnumerator set_cr()
	{
		for (;;)
		{
			this.Set();
			yield return null;
		}
		yield break;
	}

	// Token: 0x060036EC RID: 14060 RVA: 0x001017B8 File Offset: 0x000FF9B8
	public IEnumerator setLate_cr()
	{
		for (;;)
		{
			this.Set();
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	// Token: 0x04002C56 RID: 11350
	[SerializeField]
	public Sprite[] sprites;

	// Token: 0x04002C57 RID: 11351
	public SpriteRenderer r;
}
