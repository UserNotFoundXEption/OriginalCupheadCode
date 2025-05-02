using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x02000076 RID: 118
public class UITMPAnimator : AbstractMonoBehaviour
{
	// Token: 0x060005CD RID: 1485 RVA: 0x00006222 File Offset: 0x00004422
	public override void Awake()
	{
		base.Awake();
		this.text = base.GetComponent<TextMeshProUGUI>();
		base.StartCoroutine(this.animateCharacters_cr());
		this.ignoreGlobalTime = true;
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x0006D8A8 File Offset: 0x0006BAA8
	public IEnumerator animateCharacters_cr()
	{
		this.text.havePropertiesChanged = true;
		for (;;)
		{
			this.text.ForceMeshUpdate();
			TMP_TextInfo textInfo = this.text.textInfo;
			int characterCount = textInfo.characterCount;
			if (characterCount != 0)
			{
				for (int i = 0; i < characterCount; i++)
				{
					if (textInfo.characterInfo[i].isVisible)
					{
						Vector3 vector;
						vector..ctor(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0f);
						int vertexIndex = (int)textInfo.characterInfo[i].vertexIndex;
						int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
						Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
						vertices[vertexIndex] += vector;
						vertices[vertexIndex + 1] += vector;
						vertices[vertexIndex + 2] += vector;
						vertices[vertexIndex + 3] += vector;
					}
				}
				this.text.UpdateVertexData();
				yield return new WaitForSeconds(0.07f);
			}
		}
		yield break;
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x0006D8C4 File Offset: 0x0006BAC4
	public IEnumerator updateTextLikeAMoron_cr()
	{
		this.text.transform.SetScale(new float?(1f), new float?(1f), new float?(0.99f));
		yield return null;
		this.text.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
		yield break;
	}

	// Token: 0x040004C1 RID: 1217
	public TextMeshProUGUI text;
}
