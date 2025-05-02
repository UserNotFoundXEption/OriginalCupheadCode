using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003C5 RID: 965
public class TutorialShmupLevelParryNext : AbstractCollidableObject
{
	// Token: 0x06002A7C RID: 10876 RVA: 0x000D4020 File Offset: 0x000D2220
	public void Start()
	{
		this.parrySwitch.OnActivate += this.SetNextParry;
		if (this.startAsParry)
		{
			this.image.enabled = true;
			this.parrySwitch.enabled = true;
		}
		else
		{
			this.image.enabled = false;
			this.parrySwitch.enabled = false;
		}
	}

	// Token: 0x06002A7D RID: 10877 RVA: 0x000D4084 File Offset: 0x000D2284
	public void SetNextParry()
	{
		this.nextSphere.parrySwitch.enabled = true;
		this.parrySwitch.enabled = false;
		if (this.lastPlayerController != null)
		{
			this.lastPlayerController.stats.OnParry(1f, true);
			this.lastPlayerController = null;
		}
		this.image.enabled = false;
		this.nextSphere.image.enabled = true;
	}

	// Token: 0x06002A7E RID: 10878 RVA: 0x000D40FC File Offset: 0x000D22FC
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
		if (hit.transform && hit.transform.parent)
		{
			this.lastPlayerController = hit.transform.parent.GetComponent<AbstractPlayerController>();
		}
	}

	// Token: 0x0400236D RID: 9069
	[SerializeField]
	public TutorialShmupLevelParryNext nextSphere;

	// Token: 0x0400236E RID: 9070
	[SerializeField]
	public Image image;

	// Token: 0x0400236F RID: 9071
	[SerializeField]
	public bool startAsParry;

	// Token: 0x04002370 RID: 9072
	[SerializeField]
	public ParrySwitch parrySwitch;

	// Token: 0x04002371 RID: 9073
	public AbstractPlayerController lastPlayerController;
}
