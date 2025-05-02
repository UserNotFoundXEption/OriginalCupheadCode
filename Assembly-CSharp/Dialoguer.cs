using System;
using DialoguerCore;

// Token: 0x020005E2 RID: 1506
public class Dialoguer
{
	// Token: 0x17000517 RID: 1303
	// (get) Token: 0x06003E27 RID: 15911 RVA: 0x00031FD7 File Offset: 0x000301D7
	// (set) Token: 0x06003E28 RID: 15912 RVA: 0x00031FDE File Offset: 0x000301DE
	public static bool ready { get; set; }

	// Token: 0x06003E29 RID: 15913 RVA: 0x0011BEC8 File Offset: 0x0011A0C8
	public static void Initialize()
	{
		if (Dialoguer.ready)
		{
			return;
		}
		Dialoguer.events = new DialoguerEvents();
		DialoguerDataManager.Initialize();
		DialoguerEventManager.onStarted += Dialoguer.events.handler_onStarted;
		DialoguerEventManager.onEnded += Dialoguer.events.handler_onEnded;
		DialoguerEventManager.onSuddenlyEnded += Dialoguer.events.handler_SuddenlyEnded;
		DialoguerEventManager.onTextPhase += Dialoguer.events.handler_TextPhase;
		DialoguerEventManager.onWindowClose += Dialoguer.events.handler_WindowClose;
		DialoguerEventManager.onWaitStart += Dialoguer.events.handler_WaitStart;
		DialoguerEventManager.onWaitComplete += Dialoguer.events.handler_WaitComplete;
		DialoguerEventManager.onMessageEvent += Dialoguer.events.handler_MessageEvent;
		Dialoguer.ready = true;
	}

	// Token: 0x06003E2A RID: 15914 RVA: 0x00031FE6 File Offset: 0x000301E6
	public static void StartDialogue(DialoguerDialogues dialogue)
	{
		DialoguerDialogueManager.startDialogue((int)dialogue);
	}

	// Token: 0x06003E2B RID: 15915 RVA: 0x00031FEE File Offset: 0x000301EE
	public static void StartDialogue(DialoguerDialogues dialogue, DialoguerCallback callback)
	{
		DialoguerDialogueManager.startDialogueWithCallback((int)dialogue, callback);
	}

	// Token: 0x06003E2C RID: 15916 RVA: 0x00031FF7 File Offset: 0x000301F7
	public static void StartDialogue(int dialogueId)
	{
		DialoguerDialogueManager.startDialogue(dialogueId);
	}

	// Token: 0x06003E2D RID: 15917 RVA: 0x00031FFF File Offset: 0x000301FF
	public static void StartDialogue(int dialogueId, DialoguerCallback callback)
	{
		DialoguerDialogueManager.startDialogueWithCallback(dialogueId, callback);
	}

	// Token: 0x06003E2E RID: 15918 RVA: 0x00032008 File Offset: 0x00030208
	public static void ContinueDialogue(int choice)
	{
		DialoguerDialogueManager.continueDialogue(choice);
	}

	// Token: 0x06003E2F RID: 15919 RVA: 0x00032010 File Offset: 0x00030210
	public static void ContinueDialogue()
	{
		DialoguerDialogueManager.continueDialogue(0);
	}

	// Token: 0x06003E30 RID: 15920 RVA: 0x00032018 File Offset: 0x00030218
	public static void EndDialogue()
	{
		DialoguerDialogueManager.endDialogue();
	}

	// Token: 0x06003E31 RID: 15921 RVA: 0x0003201F File Offset: 0x0003021F
	public static void SetGlobalBoolean(int booleanId, bool booleanValue)
	{
		DialoguerDataManager.SetGlobalBoolean(booleanId, booleanValue);
	}

	// Token: 0x06003E32 RID: 15922 RVA: 0x00032028 File Offset: 0x00030228
	public static bool GetGlobalBoolean(int booleanId)
	{
		return DialoguerDataManager.GetGlobalBoolean(booleanId);
	}

	// Token: 0x06003E33 RID: 15923 RVA: 0x00032030 File Offset: 0x00030230
	public static void SetGlobalFloat(int floatId, float floatValue)
	{
		DialoguerDataManager.SetGlobalFloat(floatId, floatValue);
	}

	// Token: 0x06003E34 RID: 15924 RVA: 0x00032039 File Offset: 0x00030239
	public static float GetGlobalFloat(int floatId)
	{
		return DialoguerDataManager.GetGlobalFloat(floatId);
	}

	// Token: 0x06003E35 RID: 15925 RVA: 0x00032041 File Offset: 0x00030241
	public static void SetGlobalString(int stringId, string stringValue)
	{
		DialoguerDataManager.SetGlobalString(stringId, stringValue);
	}

	// Token: 0x06003E36 RID: 15926 RVA: 0x0003204A File Offset: 0x0003024A
	public static string GetGlobalString(int stringId)
	{
		return DialoguerDataManager.GetGlobalString(stringId);
	}

	// Token: 0x06003E37 RID: 15927 RVA: 0x00032052 File Offset: 0x00030252
	public static string GetGlobalVariablesState()
	{
		return DialoguerDataManager.GetGlobalVariablesState();
	}

	// Token: 0x06003E38 RID: 15928 RVA: 0x00032059 File Offset: 0x00030259
	public static void SetGlobalVariablesState(string globalVariablesXml)
	{
		DialoguerDataManager.LoadGlobalVariablesState(globalVariablesXml);
	}

	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x06003E39 RID: 15929 RVA: 0x00032061 File Offset: 0x00030261
	// (set) Token: 0x06003E3A RID: 15930 RVA: 0x00032068 File Offset: 0x00030268
	public static DialoguerEvents events { get; set; }
}
