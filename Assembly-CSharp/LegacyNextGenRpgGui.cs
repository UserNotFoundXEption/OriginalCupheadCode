using System;
using UnityEngine;

// Token: 0x020005FF RID: 1535
public class LegacyNextGenRpgGui : MonoBehaviour
{
	// Token: 0x06003FDE RID: 16350 RVA: 0x0003352A File Offset: 0x0003172A
	public void Awake()
	{
		Dialoguer.Initialize();
	}

	// Token: 0x06003FDF RID: 16351 RVA: 0x00033531 File Offset: 0x00031731
	public void Start()
	{
		this.addDialoguerEvents();
		this._dialogue = false;
	}

	// Token: 0x06003FE0 RID: 16352 RVA: 0x00033540 File Offset: 0x00031740
	public void Update()
	{
		if (this._showWindow && Input.GetMouseButtonDown(0))
		{
			if (this._choices != null)
			{
				this.audioSelect.Play();
			}
			Dialoguer.ContinueDialogue(this._currentChoice);
		}
	}

	// Token: 0x06003FE1 RID: 16353 RVA: 0x00129A0C File Offset: 0x00127C0C
	public void addDialoguerEvents()
	{
		Dialoguer.events.onStarted += this.onDialogueStartedHandler;
		Dialoguer.events.onEnded += this.onDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.onDialogueInstantlyEndedHandler;
		Dialoguer.events.onTextPhase += this.onDialogueTextPhaseHandler;
		Dialoguer.events.onWindowClose += this.onDialogueWindowCloseHandler;
		Dialoguer.events.onMessageEvent += this.onDialoguerMessageEvent;
	}

	// Token: 0x06003FE2 RID: 16354 RVA: 0x00033579 File Offset: 0x00031779
	public void onDialogueStartedHandler()
	{
		this._dialogue = true;
	}

	// Token: 0x06003FE3 RID: 16355 RVA: 0x00033582 File Offset: 0x00031782
	public void onDialogueEndedHandler()
	{
		this._dialogue = false;
		this._showWindow = false;
	}

	// Token: 0x06003FE4 RID: 16356 RVA: 0x00033592 File Offset: 0x00031792
	public void onDialogueInstantlyEndedHandler()
	{
		this._dialogue = false;
		this._showWindow = false;
	}

	// Token: 0x06003FE5 RID: 16357 RVA: 0x00129AA0 File Offset: 0x00127CA0
	public void onDialogueTextPhaseHandler(DialoguerTextData data)
	{
		this._currentChoice = 0;
		if (data.choices != null)
		{
			this._choices = new string[6];
			for (int i = 0; i < 6; i++)
			{
				if (data.choices.Length > i && data.choices[i] != null)
				{
					this._choices[i] = data.choices[i];
					this._currentChoice = i;
				}
			}
		}
		else
		{
			this._choices = null;
		}
		this._text = data.text;
		if (data.name != null && data.name != string.Empty)
		{
			this._text = data.name + ": " + this._text;
		}
		this._showWindow = true;
	}

	// Token: 0x06003FE6 RID: 16358 RVA: 0x000335A2 File Offset: 0x000317A2
	public void onDialogueWindowCloseHandler()
	{
		this._showWindow = false;
	}

	// Token: 0x06003FE7 RID: 16359 RVA: 0x000335AB File Offset: 0x000317AB
	public void onDialoguerMessageEvent(string message, string metadata)
	{
	}

	// Token: 0x06003FE8 RID: 16360 RVA: 0x00129B74 File Offset: 0x00127D74
	public void OnGUI()
	{
		if (!this._dialogue)
		{
			return;
		}
		if (!this._showWindow)
		{
			return;
		}
		GUI.skin = this.guiSkin;
		int num = 260;
		Rect rect;
		rect..ctor((float)Screen.width * 0.5f - 300f, (float)(Screen.height - num), 600f, 80f);
		GUIStyle guistyle = new GUIStyle("label");
		guistyle.alignment = 4;
		this.drawText(this._text, rect, guistyle);
		if (this._choices != null)
		{
			this.drawChoiceRing();
		}
	}

	// Token: 0x06003FE9 RID: 16361 RVA: 0x00129C0C File Offset: 0x00127E0C
	public void drawText(string text, Rect rect)
	{
		GUIStyle style = new GUIStyle("label");
		this.drawText(text, rect, style);
	}

	// Token: 0x06003FEA RID: 16362 RVA: 0x00129C34 File Offset: 0x00127E34
	public void drawText(string text, Rect rect, GUIStyle style)
	{
		GUI.color = Color.black;
		for (int i = 0; i < LegacyNextGenRpgGui.TEXT_OUTLINE_WIDTH; i++)
		{
			for (int j = 0; j < LegacyNextGenRpgGui.TEXT_OUTLINE_WIDTH; j++)
			{
				GUI.Label(new Rect(rect.x + (float)(i + 1), rect.y + (float)(j + 1), rect.width, rect.height), text, style);
				GUI.Label(new Rect(rect.x - (float)(i + 1), rect.y - (float)(j + 1), rect.width, rect.height), text, style);
				GUI.Label(new Rect(rect.x + (float)(i + 1), rect.y - (float)(j + 1), rect.width, rect.height), text, style);
				GUI.Label(new Rect(rect.x - (float)(i + 1), rect.y + (float)(j + 1), rect.width, rect.height), text, style);
			}
		}
		GUI.color = GUI.contentColor;
		GUI.Label(rect, text, style);
	}

	// Token: 0x06003FEB RID: 16363 RVA: 0x00129D54 File Offset: 0x00127F54
	public void drawChoiceRing()
	{
		Rect rect;
		rect..ctor((float)Screen.width * 0.5f - 128f, (float)(Screen.height - 128 - 50), 256f, 128f);
		if (this._ringeRects == null)
		{
			this._ringeRects = new Rect[6];
			this._ringeRects[0] = new Rect(rect.center.x, rect.y - 40f, (float)Screen.width * 0.5f, rect.height * 0.3333f + 40f);
			this._ringeRects[1] = new Rect(rect.center.x, rect.y + rect.height * 0.3333f, (float)Screen.width * 0.5f, rect.height * 0.3333f);
			this._ringeRects[2] = new Rect(rect.center.x, rect.y + rect.height * 0.3333f * 2f, (float)Screen.width * 0.5f, rect.height * 0.3333f + 40f);
			this._ringeRects[3] = new Rect(0f, rect.y - 40f, (float)Screen.width * 0.5f, rect.height * 0.3333f + 40f);
			this._ringeRects[4] = new Rect(0f, rect.y + rect.height * 0.3333f, (float)Screen.width * 0.5f, rect.height * 0.3333f);
			this._ringeRects[5] = new Rect(0f, rect.y + rect.height * 0.3333f * 2f, (float)Screen.width * 0.5f, rect.height * 0.3333f + 40f);
		}
		if (this._choicesTextRects == null)
		{
			this._choicesTextRects = new Rect[6];
			this._choicesTextRects[0] = new Rect(rect.center.x + rect.width * 0.5f - 10f, rect.y, (float)Screen.width * 0.5f - rect.width * 0.5f + 10f, rect.height * 0.3333f);
			this._choicesTextRects[1] = new Rect(rect.center.x + rect.width * 0.5f + 10f, rect.y + rect.height * 0.3333f - 5f, (float)Screen.width * 0.5f - rect.width * 0.5f - 10f, rect.height * 0.3333f);
			this._choicesTextRects[2] = new Rect(rect.center.x + rect.width * 0.5f, rect.y + rect.height * 0.3333f * 2f, (float)Screen.width * 0.5f - rect.width * 0.5f, rect.height * 0.3333f);
			this._choicesTextRects[3] = new Rect(0f, rect.y, (float)Screen.width * 0.5f - rect.width * 0.5f + 10f, rect.height * 0.3333f);
			this._choicesTextRects[4] = new Rect(0f, rect.y + rect.height * 0.3333f - 5f, (float)Screen.width * 0.5f - rect.width * 0.5f - 10f, rect.height * 0.3333f);
			this._choicesTextRects[5] = new Rect(0f, rect.y + rect.height * 0.3333f * 2f, (float)Screen.width * 0.5f - rect.width * 0.5f, rect.height * 0.3333f);
		}
		GUI.DrawTexture(rect, this.ringBase);
		for (int i = 0; i < 6; i++)
		{
			if (this._choices[i] != null && this._choices[i] != string.Empty)
			{
				if (this._currentChoice != i && this._ringeRects[i].Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)))
				{
					this._currentChoice = i;
					this.audioChoice.PlayOneShot(this.audioChoice.clip);
				}
				if (this._currentChoice == i)
				{
					GUI.DrawTexture(rect, this.ringHover.getPieces()[i]);
				}
				else
				{
					GUI.DrawTexture(rect, this.ringNormal.getPieces()[i]);
				}
				GUIStyle guistyle = new GUIStyle("label");
				if (i > 2)
				{
					guistyle.alignment = 5;
				}
				else
				{
					guistyle.alignment = 3;
				}
				this.drawText(this._choices[i], this._choicesTextRects[i], guistyle);
			}
		}
	}

	// Token: 0x040032D8 RID: 13016
	public static int TEXT_OUTLINE_WIDTH = 1;

	// Token: 0x040032D9 RID: 13017
	public GUISkin guiSkin;

	// Token: 0x040032DA RID: 13018
	public AudioSource audioChoice;

	// Token: 0x040032DB RID: 13019
	public AudioSource audioSelect;

	// Token: 0x040032DC RID: 13020
	public Texture ringBase;

	// Token: 0x040032DD RID: 13021
	public Texture ringTop;

	// Token: 0x040032DE RID: 13022
	public Texture ringBottom;

	// Token: 0x040032DF RID: 13023
	public NextGenRingPieces ringNormal;

	// Token: 0x040032E0 RID: 13024
	public NextGenRingPieces ringHover;

	// Token: 0x040032E1 RID: 13025
	public int _currentChoice;

	// Token: 0x040032E2 RID: 13026
	public Rect[] _ringeRects;

	// Token: 0x040032E3 RID: 13027
	public Rect[] _choicesTextRects;

	// Token: 0x040032E4 RID: 13028
	public bool _dialogue;

	// Token: 0x040032E5 RID: 13029
	public bool _showWindow;

	// Token: 0x040032E6 RID: 13030
	public string _text;

	// Token: 0x040032E7 RID: 13031
	public string[] _choices;
}
