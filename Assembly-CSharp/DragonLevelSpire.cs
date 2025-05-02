using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000219 RID: 537
public class DragonLevelSpire : AbstractPausableComponent
{
	// Token: 0x06001878 RID: 6264 RVA: 0x00014E4F File Offset: 0x0001304F
	public void Start()
	{
		this.helper = base.GetComponent<AnimationHelper>();
		this.helper.Speed = 0f;
		this.fadeTime = 3f;
		this.replacementSprite.enabled = false;
	}

	// Token: 0x06001879 RID: 6265 RVA: 0x00014E84 File Offset: 0x00013084
	public void Update()
	{
		this.helper.Speed = DragonLevel.SPEED;
	}

	// Token: 0x0600187A RID: 6266 RVA: 0x00014E96 File Offset: 0x00013096
	public void StartChange()
	{
		base.StartCoroutine(this.change_cr());
	}

	// Token: 0x0600187B RID: 6267 RVA: 0x000A3E04 File Offset: 0x000A2004
	public IEnumerator change_cr()
	{
		float t = 0f;
		while (t < this.fadeTime)
		{
			this.replacementSprite.enabled = true;
			this.replacementSprite.color = new Color(1f, 1f, 1f, t / this.fadeTime);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.replacementSprite.color = new Color(1f, 1f, 1f, 1f);
		yield return null;
		yield break;
	}

	// Token: 0x040013DA RID: 5082
	public AnimationHelper helper;

	// Token: 0x040013DB RID: 5083
	[SerializeField]
	public SpriteRenderer replacementSprite;

	// Token: 0x040013DC RID: 5084
	public float fadeTime;
}
