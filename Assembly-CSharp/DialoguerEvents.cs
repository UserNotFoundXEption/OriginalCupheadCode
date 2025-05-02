using System;

// Token: 0x020005E3 RID: 1507
public class DialoguerEvents
{
	// Token: 0x06003E3C RID: 15932 RVA: 0x00032078 File Offset: 0x00030278
	public void ClearAll()
	{
		this.onStarted = null;
		this.onEnded = null;
		this.onTextPhase = null;
		this.onWindowClose = null;
		this.onWaitStart = null;
		this.onWaitComplete = null;
		this.onMessageEvent = null;
	}

	// Token: 0x140000D3 RID: 211
	// (add) Token: 0x06003E3D RID: 15933 RVA: 0x0011BFA0 File Offset: 0x0011A1A0
	// (remove) Token: 0x06003E3E RID: 15934 RVA: 0x0011BFD8 File Offset: 0x0011A1D8
	public event DialoguerEvents.StartedHandler onStarted;

	// Token: 0x06003E3F RID: 15935 RVA: 0x000320AB File Offset: 0x000302AB
	public void handler_onStarted()
	{
		if (this.onStarted != null)
		{
			this.onStarted();
		}
	}

	// Token: 0x140000D4 RID: 212
	// (add) Token: 0x06003E40 RID: 15936 RVA: 0x0011C010 File Offset: 0x0011A210
	// (remove) Token: 0x06003E41 RID: 15937 RVA: 0x0011C048 File Offset: 0x0011A248
	public event DialoguerEvents.EndedHandler onEnded;

	// Token: 0x06003E42 RID: 15938 RVA: 0x000320C3 File Offset: 0x000302C3
	public void handler_onEnded()
	{
		if (this.onEnded != null)
		{
			this.onEnded();
		}
	}

	// Token: 0x140000D5 RID: 213
	// (add) Token: 0x06003E43 RID: 15939 RVA: 0x0011C080 File Offset: 0x0011A280
	// (remove) Token: 0x06003E44 RID: 15940 RVA: 0x0011C0B8 File Offset: 0x0011A2B8
	public event DialoguerEvents.SuddenlyEndedHandler onInstantlyEnded;

	// Token: 0x06003E45 RID: 15941 RVA: 0x000320DB File Offset: 0x000302DB
	public void handler_SuddenlyEnded()
	{
		if (this.onInstantlyEnded != null)
		{
			this.onInstantlyEnded();
		}
	}

	// Token: 0x140000D6 RID: 214
	// (add) Token: 0x06003E46 RID: 15942 RVA: 0x0011C0F0 File Offset: 0x0011A2F0
	// (remove) Token: 0x06003E47 RID: 15943 RVA: 0x0011C128 File Offset: 0x0011A328
	public event DialoguerEvents.TextPhaseHandler onTextPhase;

	// Token: 0x06003E48 RID: 15944 RVA: 0x000320F3 File Offset: 0x000302F3
	public void handler_TextPhase(DialoguerTextData data)
	{
		if (this.onTextPhase != null)
		{
			this.onTextPhase(data);
		}
	}

	// Token: 0x140000D7 RID: 215
	// (add) Token: 0x06003E49 RID: 15945 RVA: 0x0011C160 File Offset: 0x0011A360
	// (remove) Token: 0x06003E4A RID: 15946 RVA: 0x0011C198 File Offset: 0x0011A398
	public event DialoguerEvents.WindowCloseHandler onWindowClose;

	// Token: 0x06003E4B RID: 15947 RVA: 0x0003210C File Offset: 0x0003030C
	public void handler_WindowClose()
	{
		if (this.onWindowClose != null)
		{
			this.onWindowClose();
		}
	}

	// Token: 0x140000D8 RID: 216
	// (add) Token: 0x06003E4C RID: 15948 RVA: 0x0011C1D0 File Offset: 0x0011A3D0
	// (remove) Token: 0x06003E4D RID: 15949 RVA: 0x0011C208 File Offset: 0x0011A408
	public event DialoguerEvents.WaitStartHandler onWaitStart;

	// Token: 0x06003E4E RID: 15950 RVA: 0x00032124 File Offset: 0x00030324
	public void handler_WaitStart()
	{
		if (this.onWaitStart != null)
		{
			this.onWaitStart();
		}
	}

	// Token: 0x140000D9 RID: 217
	// (add) Token: 0x06003E4F RID: 15951 RVA: 0x0011C240 File Offset: 0x0011A440
	// (remove) Token: 0x06003E50 RID: 15952 RVA: 0x0011C278 File Offset: 0x0011A478
	public event DialoguerEvents.WaitCompleteHandler onWaitComplete;

	// Token: 0x06003E51 RID: 15953 RVA: 0x0003213C File Offset: 0x0003033C
	public void handler_WaitComplete()
	{
		if (this.onWaitComplete != null)
		{
			this.onWaitComplete();
		}
	}

	// Token: 0x140000DA RID: 218
	// (add) Token: 0x06003E52 RID: 15954 RVA: 0x0011C2B0 File Offset: 0x0011A4B0
	// (remove) Token: 0x06003E53 RID: 15955 RVA: 0x0011C2E8 File Offset: 0x0011A4E8
	public event DialoguerEvents.MessageEventHandler onMessageEvent;

	// Token: 0x06003E54 RID: 15956 RVA: 0x00032154 File Offset: 0x00030354
	public void handler_MessageEvent(string message, string metadata)
	{
		if (this.onMessageEvent != null)
		{
			this.onMessageEvent(message, metadata);
		}
	}

	// Token: 0x02001250 RID: 4688
	// (Invoke) Token: 0x06008148 RID: 33096
	public delegate void StartedHandler();

	// Token: 0x02001251 RID: 4689
	// (Invoke) Token: 0x0600814C RID: 33100
	public delegate void EndedHandler();

	// Token: 0x02001252 RID: 4690
	// (Invoke) Token: 0x06008150 RID: 33104
	public delegate void SuddenlyEndedHandler();

	// Token: 0x02001253 RID: 4691
	// (Invoke) Token: 0x06008154 RID: 33108
	public delegate void TextPhaseHandler(DialoguerTextData data);

	// Token: 0x02001254 RID: 4692
	// (Invoke) Token: 0x06008158 RID: 33112
	public delegate void WindowCloseHandler();

	// Token: 0x02001255 RID: 4693
	// (Invoke) Token: 0x0600815C RID: 33116
	public delegate void WaitStartHandler();

	// Token: 0x02001256 RID: 4694
	// (Invoke) Token: 0x06008160 RID: 33120
	public delegate void WaitCompleteHandler();

	// Token: 0x02001257 RID: 4695
	// (Invoke) Token: 0x06008164 RID: 33124
	public delegate void MessageEventHandler(string message, string metadata);
}
