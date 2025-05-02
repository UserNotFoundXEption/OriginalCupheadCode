using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200035E RID: 862
public class SallyStagePlayLevelHouse : AbstractPausableComponent
{
	// Token: 0x06002608 RID: 9736 RVA: 0x0001FEC2 File Offset: 0x0001E0C2
	public void StartPhase2(SallyStagePlayLevel parent, LevelProperties.SallyStagePlay properties)
	{
		this.SetUp(parent, properties);
	}

	// Token: 0x06002609 RID: 9737 RVA: 0x0001FECC File Offset: 0x0001E0CC
	public void StartAttacks()
	{
		if (!SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE)
		{
			base.StartCoroutine(this.family_cr());
		}
		else
		{
			base.StartCoroutine(this.nuns_cr());
		}
	}

	// Token: 0x0600260A RID: 9738 RVA: 0x0001FEF7 File Offset: 0x0001E0F7
	public void SetUp(SallyStagePlayLevel parent, LevelProperties.SallyStagePlay properties)
	{
		this.parent = parent;
		this.properties = properties;
		parent.OnPhase3 += this.OnPhase3;
		base.StartCoroutine(this.setup_windows_cr());
	}

	// Token: 0x0600260B RID: 9739 RVA: 0x000C81B8 File Offset: 0x000C63B8
	public IEnumerator setup_windows_cr()
	{
		Vector3 pos = Vector3.zero;
		int num = 1;
		this.windows = new SallyStagePlayLevelWindow[9];
		for (int i = 0; i < 9; i++)
		{
			this.windows[i] = Object.Instantiate<SallyStagePlayLevelWindow>(this.windowPrefab);
			this.windows[i].transform.position = this.windowRoots[i].position;
			this.windows[i].Init(this.windowRoots[i].position, this.parent);
			this.windows[i].transform.parent = base.transform;
			this.windows[i].windowNum = num + i;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600260C RID: 9740 RVA: 0x000C81D4 File Offset: 0x000C63D4
	public IEnumerator nuns_cr()
	{
		LevelProperties.SallyStagePlay.Nun p = this.properties.CurrentState.nun;
		string[] windowPattern = p.appearPosition.GetRandom<string>().Split(new char[]
		{
			','
		});
		int windowPos = 0;
		int pinkStringMainIndex = Random.Range(0, p.pinkString.Length);
		string[] pinkString = p.pinkString[pinkStringMainIndex].Split(new char[]
		{
			','
		});
		int pinkStringIndex = Random.Range(0, pinkString.Length);
		for (;;)
		{
			for (int i = 0; i < windowPattern.Length; i++)
			{
				Parser.IntTryParse(windowPattern[i], out windowPos);
				foreach (SallyStagePlayLevelWindow window in this.windows)
				{
					if (window.windowNum == windowPos)
					{
						window.WindowOpenNun(this.properties, pinkString[pinkStringIndex][0] == 'P');
						if (pinkStringIndex < pinkString.Length - 1)
						{
							pinkStringIndex++;
						}
						else
						{
							pinkStringMainIndex = (pinkStringMainIndex + 1) % p.pinkString.Length;
							pinkStringIndex = 0;
						}
						yield return CupheadTime.WaitForSeconds(this, p.reappearDelayRange.RandomFloat());
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600260D RID: 9741 RVA: 0x000C81F0 File Offset: 0x000C63F0
	public IEnumerator family_cr()
	{
		LevelProperties.SallyStagePlay.Baby p = this.properties.CurrentState.baby;
		string[] windowPattern = p.appearPosition.GetRandom<string>().Split(new char[]
		{
			','
		});
		int windowPos = 0;
		for (;;)
		{
			for (int i = 0; i < windowPattern.Length; i++)
			{
				Parser.IntTryParse(windowPattern[i], out windowPos);
				foreach (SallyStagePlayLevelWindow window in this.windows)
				{
					if (window.windowNum == windowPos)
					{
						window.WindowOpenBaby(this.properties);
						yield return CupheadTime.WaitForSeconds(this, p.hesitate);
						yield return CupheadTime.WaitForSeconds(this, p.reappearDelayRange.RandomFloat());
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600260E RID: 9742 RVA: 0x0001FF26 File Offset: 0x0001E126
	public void OnPhase3()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject, 1f);
		this.parent.OnPhase3 -= this.OnPhase3;
	}

	// Token: 0x0600260F RID: 9743 RVA: 0x0001FF55 File Offset: 0x0001E155
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.windowPrefab = null;
	}

	// Token: 0x04001F80 RID: 8064
	[SerializeField]
	public Transform[] windowRoots;

	// Token: 0x04001F81 RID: 8065
	[SerializeField]
	public SallyStagePlayLevelWindow windowPrefab;

	// Token: 0x04001F82 RID: 8066
	public SallyStagePlayLevelWindow[] windows;

	// Token: 0x04001F83 RID: 8067
	public LevelProperties.SallyStagePlay properties;

	// Token: 0x04001F84 RID: 8068
	public SallyStagePlayLevel parent;

	// Token: 0x04001F85 RID: 8069
	public const int WINDOW_NUM = 9;
}
