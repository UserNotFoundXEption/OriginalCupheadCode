using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002B9 RID: 697
public class KitchenSaltbakerCounter : DialogueInteractionPoint
{
	// Token: 0x06001F0B RID: 7947 RVA: 0x000B53E4 File Offset: 0x000B35E4
	public override void Start()
	{
		base.Start();
		Dialoguer.events.onTextPhase += this.onDialogueAdvancedHandler;
		Dialoguer.events.onEnded += this.onDialogueEndedHandler;
		if (Dialoguer.GetGlobalFloat(23) == 0f)
		{
			base.StartCoroutine(this.dialogue_on_first_visit_cr());
		}
	}

	// Token: 0x06001F0C RID: 7948 RVA: 0x0001A28B File Offset: 0x0001848B
	public override void OnDestroy()
	{
		base.OnDestroy();
		Dialoguer.events.onTextPhase -= this.onDialogueAdvancedHandler;
		Dialoguer.events.onEnded -= this.onDialogueEndedHandler;
	}

	// Token: 0x06001F0D RID: 7949 RVA: 0x000B5444 File Offset: 0x000B3644
	public void onDialogueAdvancedHandler(DialoguerTextData data)
	{
		if (!base.animator.GetCurrentAnimatorStateInfo(0).IsName("Talk"))
		{
			base.animator.SetTrigger("Talk");
		}
	}

	// Token: 0x06001F0E RID: 7950 RVA: 0x0001A2BF File Offset: 0x000184BF
	public void onDialogueEndedHandler()
	{
		base.animator.SetBool("PlayerClose", false);
	}

	// Token: 0x06001F0F RID: 7951 RVA: 0x0001A2D2 File Offset: 0x000184D2
	public override void Activate()
	{
		base.animator.SetBool("PlayerClose", true);
		base.animator.SetTrigger("Talk");
		base.Activate();
	}

	// Token: 0x06001F10 RID: 7952 RVA: 0x000B5480 File Offset: 0x000B3680
	public IEnumerator dialogue_on_first_visit_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		this.speechBubble.waitForRealease = false;
		this.Activate();
		this.Hide(PlayerId.PlayerOne);
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
		{
			this.Hide(PlayerId.PlayerTwo);
		}
		yield break;
	}

	// Token: 0x06001F11 RID: 7953 RVA: 0x000B549C File Offset: 0x000B369C
	public void Update()
	{
		base.animator.SetBool("PlayerClose", this.conversationIsActive);
		this.blinkTimer -= CupheadTime.Delta;
		if (this.blinkTimer < 0f)
		{
			this.blinkTimer = this.blinkRange.RandomFloat();
			base.animator.SetTrigger("Blink");
		}
	}

	// Token: 0x04001963 RID: 6499
	public const int DIALOGUER_VAR_ID = 23;

	// Token: 0x04001964 RID: 6500
	[SerializeField]
	public MinMax blinkRange = new MinMax(2.5f, 4.5f);

	// Token: 0x04001965 RID: 6501
	public float blinkTimer;
}
