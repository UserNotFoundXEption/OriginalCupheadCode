using System;
using System.Collections.Generic;
using DialoguerCore;
using UnityEngine;

// Token: 0x020005EB RID: 1515
public struct DialoguerTextData
{
	// Token: 0x06003E8D RID: 16013 RVA: 0x0011CA3C File Offset: 0x0011AC3C
	public DialoguerTextData(string text, string themeName, bool newWindow, string name, string portrait, string metadata, string audio, float audioDelay, Rect rect, List<string> choices, int dialogueID, int nodeID)
	{
		this.dialogueID = dialogueID;
		this.nodeID = nodeID;
		this.rawText = text;
		this.theme = themeName;
		this.newWindow = newWindow;
		this.name = name;
		this.portrait = portrait;
		this.metadata = metadata;
		this.audio = audio;
		this.audioDelay = audioDelay;
		this.rect = new Rect(rect.x, rect.y, rect.width, rect.height);
		if (choices != null)
		{
			string[] array = choices.ToArray();
			this.choices = (array.Clone() as string[]);
		}
		else
		{
			this.choices = null;
		}
		this._cachedText = null;
	}

	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x06003E8E RID: 16014 RVA: 0x000324BE File Offset: 0x000306BE
	public string text
	{
		get
		{
			if (this._cachedText == null)
			{
				this._cachedText = DialoguerUtils.insertTextPhaseStringVariables(this.rawText);
			}
			return this._cachedText;
		}
	}

	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x06003E8F RID: 16015 RVA: 0x0011CAF0 File Offset: 0x0011ACF0
	public bool usingPositionRect
	{
		get
		{
			return this.rect.x != 0f || this.rect.y != 0f || this.rect.width != 0f || this.rect.height != 0f;
		}
	}

	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x06003E90 RID: 16016 RVA: 0x000324E2 File Offset: 0x000306E2
	public DialoguerTextPhaseType windowType
	{
		get
		{
			return (this.choices != null) ? DialoguerTextPhaseType.BranchedText : DialoguerTextPhaseType.Text;
		}
	}

	// Token: 0x06003E91 RID: 16017 RVA: 0x0011CB60 File Offset: 0x0011AD60
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"\nTheme ID: ",
			this.theme,
			"\nNew Window: ",
			this.newWindow.ToString(),
			"\nName: ",
			this.name,
			"\nPortrait: ",
			this.portrait,
			"\nMetadata: ",
			this.metadata,
			"\nAudio Clip: ",
			this.audio,
			"\nAudio Delay: ",
			this.audioDelay.ToString(),
			"\nRect: ",
			this.rect.ToString(),
			"\nRaw Text: ",
			this.rawText,
			"\nDialogue ID:",
			this.dialogueID,
			"\nNode ID:",
			this.nodeID
		});
	}

	// Token: 0x0400325D RID: 12893
	public readonly int dialogueID;

	// Token: 0x0400325E RID: 12894
	public readonly int nodeID;

	// Token: 0x0400325F RID: 12895
	public readonly string rawText;

	// Token: 0x04003260 RID: 12896
	public readonly string theme;

	// Token: 0x04003261 RID: 12897
	public readonly bool newWindow;

	// Token: 0x04003262 RID: 12898
	public readonly string name;

	// Token: 0x04003263 RID: 12899
	public readonly string portrait;

	// Token: 0x04003264 RID: 12900
	public readonly string metadata;

	// Token: 0x04003265 RID: 12901
	public readonly string audio;

	// Token: 0x04003266 RID: 12902
	public readonly float audioDelay;

	// Token: 0x04003267 RID: 12903
	public readonly Rect rect;

	// Token: 0x04003268 RID: 12904
	public readonly string[] choices;

	// Token: 0x04003269 RID: 12905
	public string _cachedText;
}
