using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000207 RID: 519
public class DicePalaceRouletteLevelPlatform : ParrySwitch
{
	// Token: 0x17000288 RID: 648
	// (get) Token: 0x060017D1 RID: 6097 RVA: 0x000144DB File Offset: 0x000126DB
	// (set) Token: 0x060017D2 RID: 6098 RVA: 0x00014503 File Offset: 0x00012703
	public bool enabled
	{
		get
		{
			return base.GetComponent<CircleCollider2D>().enabled && !this.platform.GetComponent<BoxCollider2D>().enabled;
		}
		set
		{
			base.GetComponent<CircleCollider2D>().enabled = value;
			this.platform.GetComponent<BoxCollider2D>().enabled = !value;
		}
	}

	// Token: 0x060017D3 RID: 6099 RVA: 0x00014525 File Offset: 0x00012725
	public void Start()
	{
		this.maxCounter = Random.Range(1, 4);
		base.animator.SetBool("isOffset", this.isOffset);
		this.enabled = true;
		base.StartCoroutine(this.sparkles_cr());
	}

	// Token: 0x060017D4 RID: 6100 RVA: 0x0001455E File Offset: 0x0001275E
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		this.enabled = false;
		base.animator.SetBool("isFlipped", !this.enabled);
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x060017D5 RID: 6101 RVA: 0x000A28BC File Offset: 0x000A0ABC
	public IEnumerator timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.platformOpenDuration);
		this.enabled = true;
		base.animator.SetBool("isFlipped", !this.enabled);
		yield break;
	}

	// Token: 0x060017D6 RID: 6102 RVA: 0x0001458D File Offset: 0x0001278D
	public void Init(LevelProperties.DicePalaceRoulette.Platform properties)
	{
		this.properties = properties;
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x000A28D8 File Offset: 0x000A0AD8
	public void CheckSheen()
	{
		if (this.counter < this.maxCounter)
		{
			this.sheen.enabled = false;
			this.counter++;
		}
		else
		{
			this.sheen.enabled = true;
			this.maxCounter = Random.Range(1, 4);
			this.counter = 0;
		}
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x000A2938 File Offset: 0x000A0B38
	public IEnumerator sparkles_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0.25f, 1f));
			if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_1") || base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_2"))
			{
				base.animator.SetTrigger("onSparkle");
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400134F RID: 4943
	[SerializeField]
	public SpriteRenderer sheen;

	// Token: 0x04001350 RID: 4944
	[SerializeField]
	public bool isOffset;

	// Token: 0x04001351 RID: 4945
	[SerializeField]
	public GameObject platform;

	// Token: 0x04001352 RID: 4946
	public LevelProperties.DicePalaceRoulette.Platform properties;

	// Token: 0x04001353 RID: 4947
	public Color pink;

	// Token: 0x04001354 RID: 4948
	public int maxCounter;

	// Token: 0x04001355 RID: 4949
	public int counter;
}
