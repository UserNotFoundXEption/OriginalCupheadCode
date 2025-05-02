using System;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class DLCPreLastBossCutscene : DLCGenericCutscene
{
	// Token: 0x060008DA RID: 2266 RVA: 0x00076B18 File Offset: 0x00074D18
	public override void Start()
	{
		base.Start();
		if (this.trappedChar == DLCGenericCutscene.TrappedChar.None)
		{
			this.trappedChar = base.DetectCharacter();
		}
		DLCGenericCutscene.TrappedChar trappedChar = this.trappedChar;
		if (trappedChar != DLCGenericCutscene.TrappedChar.Chalice)
		{
			if (trappedChar != DLCGenericCutscene.TrappedChar.Mugman)
			{
				if (trappedChar == DLCGenericCutscene.TrappedChar.Cuphead)
				{
					this.trappedChalice[0].SetActive(false);
					this.trappedChalice[1].SetActive(false);
					this.trappedMugman[0].SetActive(false);
					this.trappedMugman[1].SetActive(false);
					this.text[5] = this.altText;
					this.text[6] = this.altTextTrappedCharCuphead;
				}
			}
			else
			{
				this.trappedChalice[0].SetActive(false);
				this.trappedChalice[1].SetActive(false);
				this.trappedCuphead[0].SetActive(false);
				this.trappedCuphead[1].SetActive(false);
				this.text[5] = this.altText;
				this.text[6] = this.altTextTrappedCharMugman;
			}
		}
		else
		{
			this.trappedMugman[0].SetActive(false);
			this.trappedMugman[1].SetActive(false);
			this.trappedCuphead[0].SetActive(false);
			this.trappedCuphead[1].SetActive(false);
		}
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x00008704 File Offset: 0x00006904
	public override void OnCutsceneOver()
	{
		SceneLoader.LoadLevel(Levels.Saltbaker, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x040006B5 RID: 1717
	[SerializeField]
	public DLCGenericCutscene.TrappedChar trappedChar;

	// Token: 0x040006B6 RID: 1718
	[SerializeField]
	public GameObject[] trappedChalice;

	// Token: 0x040006B7 RID: 1719
	[SerializeField]
	public GameObject[] trappedMugman;

	// Token: 0x040006B8 RID: 1720
	[SerializeField]
	public GameObject[] trappedCuphead;

	// Token: 0x040006B9 RID: 1721
	[SerializeField]
	public GameObject altText;

	// Token: 0x040006BA RID: 1722
	[SerializeField]
	public GameObject altTextTrappedCharCuphead;

	// Token: 0x040006BB RID: 1723
	[SerializeField]
	public GameObject altTextTrappedCharMugman;
}
