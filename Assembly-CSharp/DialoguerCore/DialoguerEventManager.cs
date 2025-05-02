using System;

namespace DialoguerCore
{
	// Token: 0x020005E5 RID: 1509
	public class DialoguerEventManager
	{
		// Token: 0x140000DB RID: 219
		// (add) Token: 0x06003E5F RID: 15967 RVA: 0x0011C3D0 File Offset: 0x0011A5D0
		// (remove) Token: 0x06003E60 RID: 15968 RVA: 0x0011C404 File Offset: 0x0011A604
		public static event DialoguerEventManager.StartedHandler onStarted;

		// Token: 0x06003E61 RID: 15969 RVA: 0x0003224E File Offset: 0x0003044E
		public static void dispatchOnStarted()
		{
			if (DialoguerEventManager.onStarted != null)
			{
				DialoguerEventManager.onStarted();
			}
		}

		// Token: 0x140000DC RID: 220
		// (add) Token: 0x06003E62 RID: 15970 RVA: 0x0011C438 File Offset: 0x0011A638
		// (remove) Token: 0x06003E63 RID: 15971 RVA: 0x0011C46C File Offset: 0x0011A66C
		public static event DialoguerEventManager.EndedHandler onEnded;

		// Token: 0x06003E64 RID: 15972 RVA: 0x00032264 File Offset: 0x00030464
		public static void dispatchOnEnded()
		{
			if (DialoguerEventManager.onEnded != null)
			{
				DialoguerEventManager.onEnded();
			}
		}

		// Token: 0x140000DD RID: 221
		// (add) Token: 0x06003E65 RID: 15973 RVA: 0x0011C4A0 File Offset: 0x0011A6A0
		// (remove) Token: 0x06003E66 RID: 15974 RVA: 0x0011C4D4 File Offset: 0x0011A6D4
		public static event DialoguerEventManager.SuddenlyEndedHandler onSuddenlyEnded;

		// Token: 0x06003E67 RID: 15975 RVA: 0x0003227A File Offset: 0x0003047A
		public static void dispatchOnSuddenlyEnded()
		{
			if (DialoguerEventManager.onSuddenlyEnded != null)
			{
				DialoguerEventManager.onSuddenlyEnded();
			}
		}

		// Token: 0x140000DE RID: 222
		// (add) Token: 0x06003E68 RID: 15976 RVA: 0x0011C508 File Offset: 0x0011A708
		// (remove) Token: 0x06003E69 RID: 15977 RVA: 0x0011C53C File Offset: 0x0011A73C
		public static event DialoguerEventManager.TextPhaseHandler onTextPhase;

		// Token: 0x06003E6A RID: 15978 RVA: 0x00032290 File Offset: 0x00030490
		public static void dispatchOnTextPhase(DialoguerTextData data)
		{
			if (DialoguerEventManager.onTextPhase != null)
			{
				DialoguerEventManager.onTextPhase(data);
			}
		}

		// Token: 0x140000DF RID: 223
		// (add) Token: 0x06003E6B RID: 15979 RVA: 0x0011C570 File Offset: 0x0011A770
		// (remove) Token: 0x06003E6C RID: 15980 RVA: 0x0011C5A4 File Offset: 0x0011A7A4
		public static event DialoguerEventManager.WindowCloseHandler onWindowClose;

		// Token: 0x06003E6D RID: 15981 RVA: 0x000322A7 File Offset: 0x000304A7
		public static void dispatchOnWindowClose()
		{
			if (DialoguerEventManager.onWindowClose != null)
			{
				DialoguerEventManager.onWindowClose();
			}
		}

		// Token: 0x140000E0 RID: 224
		// (add) Token: 0x06003E6E RID: 15982 RVA: 0x0011C5D8 File Offset: 0x0011A7D8
		// (remove) Token: 0x06003E6F RID: 15983 RVA: 0x0011C60C File Offset: 0x0011A80C
		public static event DialoguerEventManager.WaitStartHandler onWaitStart;

		// Token: 0x06003E70 RID: 15984 RVA: 0x000322BD File Offset: 0x000304BD
		public static void dispatchOnWaitStart()
		{
			if (DialoguerEventManager.onWaitStart != null)
			{
				DialoguerEventManager.onWaitStart();
			}
		}

		// Token: 0x140000E1 RID: 225
		// (add) Token: 0x06003E71 RID: 15985 RVA: 0x0011C640 File Offset: 0x0011A840
		// (remove) Token: 0x06003E72 RID: 15986 RVA: 0x0011C674 File Offset: 0x0011A874
		public static event DialoguerEventManager.WaitCompleteHandler onWaitComplete;

		// Token: 0x06003E73 RID: 15987 RVA: 0x000322D3 File Offset: 0x000304D3
		public static void dispatchOnWaitComplete()
		{
			if (DialoguerEventManager.onWaitComplete != null)
			{
				DialoguerEventManager.onWaitComplete();
			}
		}

		// Token: 0x140000E2 RID: 226
		// (add) Token: 0x06003E74 RID: 15988 RVA: 0x0011C6A8 File Offset: 0x0011A8A8
		// (remove) Token: 0x06003E75 RID: 15989 RVA: 0x0011C6DC File Offset: 0x0011A8DC
		public static event DialoguerEventManager.MessageEventHandler onMessageEvent;

		// Token: 0x06003E76 RID: 15990 RVA: 0x000322E9 File Offset: 0x000304E9
		public static void dispatchOnMessageEvent(string message, string metadata)
		{
			if (DialoguerEventManager.onMessageEvent != null)
			{
				DialoguerEventManager.onMessageEvent(message, metadata);
			}
		}

		// Token: 0x02001258 RID: 4696
		// (Invoke) Token: 0x06008168 RID: 33128
		public delegate void StartedHandler();

		// Token: 0x02001259 RID: 4697
		// (Invoke) Token: 0x0600816C RID: 33132
		public delegate void EndedHandler();

		// Token: 0x0200125A RID: 4698
		// (Invoke) Token: 0x06008170 RID: 33136
		public delegate void SuddenlyEndedHandler();

		// Token: 0x0200125B RID: 4699
		// (Invoke) Token: 0x06008174 RID: 33140
		public delegate void TextPhaseHandler(DialoguerTextData data);

		// Token: 0x0200125C RID: 4700
		// (Invoke) Token: 0x06008178 RID: 33144
		public delegate void WindowCloseHandler();

		// Token: 0x0200125D RID: 4701
		// (Invoke) Token: 0x0600817C RID: 33148
		public delegate void WaitStartHandler();

		// Token: 0x0200125E RID: 4702
		// (Invoke) Token: 0x06008180 RID: 33152
		public delegate void WaitCompleteHandler();

		// Token: 0x0200125F RID: 4703
		// (Invoke) Token: 0x06008184 RID: 33156
		public delegate void MessageEventHandler(string message, string metadata);
	}
}
