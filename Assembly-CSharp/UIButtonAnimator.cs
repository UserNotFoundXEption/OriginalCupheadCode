using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;

// Token: 0x02000073 RID: 115
public class UIButtonAnimator : AbstractMonoBehaviour
{
	// Token: 0x1700013A RID: 314
	// (get) Token: 0x060005BD RID: 1469 RVA: 0x0000616E File Offset: 0x0000436E
	// (set) Token: 0x060005BE RID: 1470 RVA: 0x0000617B File Offset: 0x0000437B
	public string Text
	{
		get
		{
			return this.tmpText.text;
		}
		set
		{
			this.tmpText.SetText(value);
		}
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x00006189 File Offset: 0x00004389
	public void Start()
	{
		this.tmpText = base.GetComponent<TextMeshProUGUI>();
		base.StartCoroutine(this.animate_cr());
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x0006D77C File Offset: 0x0006B97C
	public IEnumerator animate_cr()
	{
		yield return null;
		yield return null;
		string first = this.Text;
		string second = this.Text;
		MatchCollection keys = Regex.Matches(this.Text, "{([^}]*)}", RegexOptions.Multiline | RegexOptions.ExplicitCapture);
		CupheadButton[] buttons = new CupheadButton[keys.Count];
		for (int i = 0; i < keys.Count; i++)
		{
			buttons[i] = (CupheadButton)Enum.Parse(typeof(CupheadButton), keys[i].Value.Substring(1, keys[i].Value.Length - 2));
		}
		for (int j = 0; j < CupheadInput.pairs.Length; j++)
		{
			for (int k = 0; k < buttons.Length; k++)
			{
				CupheadInput.InputSymbols inputSymbols = CupheadInput.InputSymbolForButton(buttons[k]);
				if (inputSymbols == CupheadInput.pairs[j].symbol)
				{
					first = first.Replace("{" + buttons[k].ToString() + "}", CupheadInput.pairs[j].first);
				}
			}
		}
		for (int l = 0; l < CupheadInput.pairs.Length; l++)
		{
			for (int m = 0; m < buttons.Length; m++)
			{
				CupheadInput.InputSymbols inputSymbols2 = CupheadInput.InputSymbolForButton(buttons[m]);
				if (inputSymbols2 == CupheadInput.pairs[l].symbol)
				{
					second = second.Replace("{" + buttons[m].ToString() + "}", CupheadInput.pairs[l].second);
				}
			}
		}
		this.Text = first;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.4f);
			this.Text = second;
			yield return CupheadTime.WaitForSeconds(this, 0.4f);
			this.Text = first;
		}
		yield break;
	}

	// Token: 0x040004B4 RID: 1204
	public const float FRAME_DELAY = 0.4f;

	// Token: 0x040004B5 RID: 1205
	public TextMeshProUGUI tmpText;
}
