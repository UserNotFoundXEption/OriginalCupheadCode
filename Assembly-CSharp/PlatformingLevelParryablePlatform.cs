using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000460 RID: 1120
public class PlatformingLevelParryablePlatform : ParrySwitch
{
	// Token: 0x17000378 RID: 888
	// (get) Token: 0x06002FC6 RID: 12230 RVA: 0x00027CA3 File Offset: 0x00025EA3
	// (set) Token: 0x06002FC7 RID: 12231 RVA: 0x00027CB0 File Offset: 0x00025EB0
	public bool enabled
	{
		get
		{
			return base.GetComponent<Collider2D>().enabled;
		}
		set
		{
			base.GetComponent<Collider2D>().enabled = value;
		}
	}

	// Token: 0x06002FC8 RID: 12232 RVA: 0x000E2AF4 File Offset: 0x000E0CF4
	public void Start()
	{
		this.platform.SetActive(false);
		this.platform.transform.SetScale(new float?(this.platformWidth), new float?(5f), new float?(5f));
		this.pink = base.GetComponent<SpriteRenderer>().color;
	}

	// Token: 0x06002FC9 RID: 12233 RVA: 0x000E2B50 File Offset: 0x000E0D50
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		this.platform.SetActive(true);
		this.enabled = false;
		base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x06002FCA RID: 12234 RVA: 0x000E2BA4 File Offset: 0x000E0DA4
	public IEnumerator timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.openDuration);
		this.platform.SetActive(false);
		this.enabled = true;
		base.GetComponent<SpriteRenderer>().color = this.pink;
		yield break;
	}

	// Token: 0x04002792 RID: 10130
	[SerializeField]
	public GameObject platform;

	// Token: 0x04002793 RID: 10131
	[SerializeField]
	public float openDuration = 3f;

	// Token: 0x04002794 RID: 10132
	[SerializeField]
	public float platformWidth = 36f;

	// Token: 0x04002795 RID: 10133
	public Color pink;
}
