using System;
using UnityEngine;

// Token: 0x02000358 RID: 856
public class SallyStagePlayLevelBowingAnimation : AbstractPausableComponent
{
	// Token: 0x060025E6 RID: 9702 RVA: 0x0001FCBF File Offset: 0x0001DEBF
	public void Start()
	{
		Level.Current.OnWinEvent += this.OnDeath;
	}

	// Token: 0x060025E7 RID: 9703 RVA: 0x0001FCD7 File Offset: 0x0001DED7
	public void PickNumber()
	{
		this.maxCounter = Random.Range(12, 21);
	}

	// Token: 0x060025E8 RID: 9704 RVA: 0x000C7F50 File Offset: 0x000C6150
	public void Counter()
	{
		if (this.counter < this.maxCounter)
		{
			this.counter++;
		}
		else
		{
			foreach (Animator animator in this.animators)
			{
				animator.SetTrigger("OnBow");
				this.counter = 0;
			}
		}
	}

	// Token: 0x060025E9 RID: 9705 RVA: 0x000C7FB4 File Offset: 0x000C61B4
	public void OnDeath()
	{
		foreach (Animator animator in this.animators)
		{
			animator.SetTrigger("OnDeath");
		}
	}

	// Token: 0x060025EA RID: 9706 RVA: 0x0001FCE8 File Offset: 0x0001DEE8
	public override void OnDestroy()
	{
		base.OnDestroy();
		Level.Current.OnWinEvent -= this.OnDeath;
	}

	// Token: 0x04001F6B RID: 8043
	[SerializeField]
	public Animator[] animators;

	// Token: 0x04001F6C RID: 8044
	public int counter;

	// Token: 0x04001F6D RID: 8045
	public int maxCounter;
}
