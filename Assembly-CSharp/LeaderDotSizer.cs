using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F0 RID: 240
public class LeaderDotSizer : MonoBehaviour
{
	// Token: 0x06000B3B RID: 2875 RVA: 0x0000A1C6 File Offset: 0x000083C6
	public void Start()
	{
		this.SetLeaderDots();
	}

	// Token: 0x06000B3C RID: 2876 RVA: 0x0007E21C File Offset: 0x0007C41C
	public void SetLeaderDots()
	{
		this.leaderDotText.text = ". . . . . . . . . . . . . . . . . . . . . . .";
		float num = this.leaderDotText.rectTransform.sizeDelta.x - this.descriptionText.preferredWidth;
		if (num < 0f)
		{
			this.leaderDotText.text = string.Empty;
			return;
		}
		int num2 = 100000;
		while (this.leaderDotText.text.Length > 2 && this.leaderDotText.preferredWidth > num && num2 > 0)
		{
			num2--;
			this.leaderDotText.text = this.leaderDotText.text.Substring(0, this.leaderDotText.text.Length - 2);
		}
	}

	// Token: 0x040008E5 RID: 2277
	public const string Dots = ". . . . . . . . . . . . . . . . . . . . . . .";

	// Token: 0x040008E6 RID: 2278
	public const float DotsPadding = 5f;

	// Token: 0x040008E7 RID: 2279
	[SerializeField]
	public TextMeshProUGUI descriptionText;

	// Token: 0x040008E8 RID: 2280
	[SerializeField]
	public Text leaderDotText;
}
