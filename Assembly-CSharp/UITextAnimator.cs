using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000075 RID: 117
public class UITextAnimator : AbstractMonoBehaviour
{
	// Token: 0x060005C8 RID: 1480 RVA: 0x0006D820 File Offset: 0x0006BA20
	public override void Awake()
	{
		base.Awake();
		this.text = base.GetComponent<Text>();
		if (this.useTMP = (this.text == null))
		{
			this.tmp_text = base.GetComponent<TMP_Text>();
			this.textString = this.tmp_text.text;
		}
		else
		{
			this.textString = this.text.text;
		}
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x00006202 File Offset: 0x00004402
	public void Start()
	{
		base.StartCoroutine(this.anim_cr());
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x00006211 File Offset: 0x00004411
	public void SetString(string s)
	{
		this.textString = s;
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x0006D88C File Offset: 0x0006BA8C
	public IEnumerator anim_cr()
	{
		if (this.useTMP)
		{
			for (;;)
			{
				this.tmp_text.text = string.Empty;
				for (int i = 0; i < this.textString.Length; i++)
				{
					TMP_Text tmp_Text = this.tmp_text;
					string text = tmp_Text.text;
					tmp_Text.text = string.Concat(new object[]
					{
						text,
						"<size=",
						this.tmp_text.fontSize + (float)Random.Range(-1, 1),
						">",
						this.textString[i].ToString(),
						"</size>"
					});
				}
				yield return new WaitForSeconds(this.frameDelay);
			}
		}
		else
		{
			for (;;)
			{
				this.text.text = string.Empty;
				for (int j = 0; j < this.textString.Length; j++)
				{
					Text text2 = this.text;
					string text = text2.text;
					text2.text = string.Concat(new object[]
					{
						text,
						"<size=",
						this.text.fontSize + Random.Range(-1, 1),
						">",
						this.textString[j].ToString(),
						"</size>"
					});
				}
				yield return new WaitForSeconds(this.frameDelay);
			}
		}
		yield break;
	}

	// Token: 0x040004BB RID: 1211
	public const int DIFFERENCE = 1;

	// Token: 0x040004BC RID: 1212
	[SerializeField]
	public float frameDelay = 0.07f;

	// Token: 0x040004BD RID: 1213
	public Text text;

	// Token: 0x040004BE RID: 1214
	public TMP_Text tmp_text;

	// Token: 0x040004BF RID: 1215
	public string textString;

	// Token: 0x040004C0 RID: 1216
	public bool useTMP;
}
