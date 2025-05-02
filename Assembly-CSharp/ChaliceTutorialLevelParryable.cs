using System;
using UnityEngine;

// Token: 0x02000171 RID: 369
public class ChaliceTutorialLevelParryable : ParrySwitch
{
	// Token: 0x17000243 RID: 579
	// (get) Token: 0x060011B6 RID: 4534 RVA: 0x0000F08A File Offset: 0x0000D28A
	// (set) Token: 0x060011B7 RID: 4535 RVA: 0x0000F092 File Offset: 0x0000D292
	public bool isDeactivated { get; set; }

	// Token: 0x060011B8 RID: 4536 RVA: 0x0000F09B File Offset: 0x0000D29B
	public override void Awake()
	{
		base.Awake();
		this.Deactivated();
	}

	// Token: 0x060011B9 RID: 4537 RVA: 0x0000F0A9 File Offset: 0x0000D2A9
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		this.Deactivated();
	}

	// Token: 0x060011BA RID: 4538 RVA: 0x0000F0B8 File Offset: 0x0000D2B8
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		base.OnParryPrePause(player);
		AudioManager.Play("sfx_parry_pink_shows");
	}

	// Token: 0x060011BB RID: 4539 RVA: 0x0000F0CB File Offset: 0x0000D2CB
	public void Deactivated()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.isDeactivated = true;
	}

	// Token: 0x060011BC RID: 4540 RVA: 0x0000F0E0 File Offset: 0x0000D2E0
	public void Activated()
	{
		base.GetComponent<Collider2D>().enabled = true;
		this.isDeactivated = false;
	}

	// Token: 0x060011BD RID: 4541 RVA: 0x00092E24 File Offset: 0x00091024
	public void Update()
	{
		this.rend.color = new Color(1f, 1f, 1f, Mathf.Clamp(this.rend.color.a + ((!this.isDeactivated) ? CupheadTime.Delta : (-CupheadTime.Delta)) * 5f, 0f, 1f));
	}

	// Token: 0x04000E37 RID: 3639
	public const float FADE_SPEED = 5f;

	// Token: 0x04000E38 RID: 3640
	[SerializeField]
	public bool firstOne;

	// Token: 0x04000E39 RID: 3641
	[SerializeField]
	public SpriteRenderer rend;
}
