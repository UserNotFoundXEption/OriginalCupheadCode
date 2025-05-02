using System;
using UnityEngine;

// Token: 0x020003C2 RID: 962
public class TutorialLevelParryNext : AbstractCollidableObject
{
	// Token: 0x06002A6B RID: 10859 RVA: 0x000D3D40 File Offset: 0x000D1F40
	public void Start()
	{
		this.parrySwitch.OnActivate += this.SetNextParry;
		if (this.startAsParry)
		{
			this.spriteRenderer.sprite = this.parrySprite;
			this.spriteRenderer.sharedMaterial = this.parryMaterial;
			this.parrySwitch.enabled = true;
		}
		else
		{
			this.spriteRenderer.sprite = this.normalSprite;
			this.spriteRenderer.sharedMaterial = this.normalMaterial;
			this.parrySwitch.enabled = false;
		}
	}

	// Token: 0x06002A6C RID: 10860 RVA: 0x000D3DD0 File Offset: 0x000D1FD0
	public void SetNextParry()
	{
		this.nextSphere.parrySwitch.enabled = true;
		this.parrySwitch.enabled = false;
		if (this.lastPlayerController != null)
		{
			this.lastPlayerController.stats.OnParry(1f, true);
			this.lastPlayerController = null;
		}
		this.spriteRenderer.sprite = this.normalSprite;
		this.spriteRenderer.sharedMaterial = this.normalMaterial;
		this.nextSphere.spriteRenderer.sprite = this.nextSphere.parrySprite;
		this.nextSphere.spriteRenderer.sharedMaterial = this.parryMaterial;
	}

	// Token: 0x06002A6D RID: 10861 RVA: 0x000D3E7C File Offset: 0x000D207C
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
		if (hit.transform && hit.transform.parent)
		{
			this.lastPlayerController = hit.transform.parent.GetComponent<AbstractPlayerController>();
		}
	}

	// Token: 0x04002361 RID: 9057
	[SerializeField]
	public TutorialLevelParryNext nextSphere;

	// Token: 0x04002362 RID: 9058
	[SerializeField]
	public SpriteRenderer spriteRenderer;

	// Token: 0x04002363 RID: 9059
	[SerializeField]
	public Sprite normalSprite;

	// Token: 0x04002364 RID: 9060
	[SerializeField]
	public Sprite parrySprite;

	// Token: 0x04002365 RID: 9061
	[SerializeField]
	public Material normalMaterial;

	// Token: 0x04002366 RID: 9062
	[SerializeField]
	public Material parryMaterial;

	// Token: 0x04002367 RID: 9063
	[SerializeField]
	public bool startAsParry;

	// Token: 0x04002368 RID: 9064
	[SerializeField]
	public ParrySwitch parrySwitch;

	// Token: 0x04002369 RID: 9065
	public AbstractPlayerController lastPlayerController;
}
