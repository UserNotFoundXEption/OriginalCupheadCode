using System;
using UnityEngine;

// Token: 0x02000601 RID: 1537
public class UnityDefaultGui : MonoBehaviour
{
	// Token: 0x06003FF0 RID: 16368 RVA: 0x000335D0 File Offset: 0x000317D0
	public void Awake()
	{
		Dialoguer.Initialize();
	}

	// Token: 0x06003FF1 RID: 16369 RVA: 0x000335D7 File Offset: 0x000317D7
	public void Start()
	{
		this.addDialoguerEvents();
	}

	// Token: 0x06003FF2 RID: 16370 RVA: 0x0012A3DC File Offset: 0x001285DC
	public void OnGUI()
	{
		if (!this._showing)
		{
			return;
		}
		if (!this._windowShowing)
		{
			return;
		}
		GUI.color = this._guiColor;
		GUI.depth = 10;
		Rect rect;
		rect..ctor((float)Screen.width * 0.5f - 250f, (float)Screen.height - 200f - 100f, 500f, 200f);
		Rect rect2;
		rect2..ctor(rect.x, rect.y, rect.width, rect.height - (float)(45 * this._choices.Length));
		GUI.Box(rect2, string.Empty);
		GUI.color = GUI.contentColor;
		GUI.Label(new Rect(rect2.x + 10f, rect2.y + 10f, rect2.width - 20f, rect2.height - 20f), this._windowText);
		if (this._selectionClicked)
		{
			return;
		}
		for (int i = 0; i < this._choices.Length; i++)
		{
			Rect rect3;
			rect3..ctor(rect.x, rect.yMax - (float)(45 * (this._choices.Length - i)) + 5f, rect.width, 40f);
			if (GUI.Button(rect3, this._choices[i]))
			{
				this._selectionClicked = true;
				Dialoguer.ContinueDialogue(i);
			}
		}
		GUI.color = GUI.contentColor;
	}

	// Token: 0x06003FF3 RID: 16371 RVA: 0x0012A558 File Offset: 0x00128758
	public void addDialoguerEvents()
	{
		Dialoguer.events.onStarted += this.onStartedHandler;
		Dialoguer.events.onEnded += this.onEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.onInstantlyEndedHandler;
		Dialoguer.events.onTextPhase += this.onTextPhaseHandler;
		Dialoguer.events.onWindowClose += this.onWindowCloseHandler;
	}

	// Token: 0x06003FF4 RID: 16372 RVA: 0x000335DF File Offset: 0x000317DF
	public void onStartedHandler()
	{
		this._showing = true;
	}

	// Token: 0x06003FF5 RID: 16373 RVA: 0x000335E8 File Offset: 0x000317E8
	public void onEndedHandler()
	{
		this._showing = false;
	}

	// Token: 0x06003FF6 RID: 16374 RVA: 0x000335F1 File Offset: 0x000317F1
	public void onInstantlyEndedHandler()
	{
		this._showing = true;
		this._windowShowing = false;
		this._selectionClicked = false;
	}

	// Token: 0x06003FF7 RID: 16375 RVA: 0x0012A5D4 File Offset: 0x001287D4
	public void onTextPhaseHandler(DialoguerTextData data)
	{
		this._guiColor = GUI.contentColor;
		this._windowText = data.text;
		if (data.windowType == DialoguerTextPhaseType.Text)
		{
			this._choices = new string[]
			{
				"Continue"
			};
		}
		else
		{
			this._choices = data.choices;
		}
		string theme = data.theme;
		if (theme != null)
		{
			if (theme == "bad")
			{
				this._guiColor = Color.red;
				goto IL_AD;
			}
			if (theme == "good")
			{
				this._guiColor = Color.green;
				goto IL_AD;
			}
		}
		this._guiColor = GUI.contentColor;
		IL_AD:
		this._windowShowing = true;
		this._selectionClicked = false;
	}

	// Token: 0x06003FF8 RID: 16376 RVA: 0x00033608 File Offset: 0x00031808
	public void onWindowCloseHandler()
	{
		this._windowShowing = false;
		this._selectionClicked = false;
	}

	// Token: 0x040032EF RID: 13039
	public const float HEIGHT = 200f;

	// Token: 0x040032F0 RID: 13040
	public const float WIDTH = 500f;

	// Token: 0x040032F1 RID: 13041
	public bool _showing;

	// Token: 0x040032F2 RID: 13042
	public bool _windowShowing;

	// Token: 0x040032F3 RID: 13043
	public bool _selectionClicked;

	// Token: 0x040032F4 RID: 13044
	public string _windowText = string.Empty;

	// Token: 0x040032F5 RID: 13045
	public string[] _choices;

	// Token: 0x040032F6 RID: 13046
	public Color _guiColor;
}
