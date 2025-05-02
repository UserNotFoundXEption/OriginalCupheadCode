using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200049D RID: 1181
public class MapNPCQuadratus : AbstractMapInteractiveEntity
{
	// Token: 0x06003156 RID: 12630 RVA: 0x0002910E File Offset: 0x0002730E
	public void Start()
	{
		this.AddDialoguerEvents();
		if (this.entitySpriteRenderer == null)
		{
			this.entitySpriteRenderer = base.GetComponent<SpriteRenderer>();
		}
	}

	// Token: 0x06003157 RID: 12631 RVA: 0x00029133 File Offset: 0x00027333
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.RemoveDialoguerEvents();
	}

	// Token: 0x06003158 RID: 12632 RVA: 0x000E951C File Offset: 0x000E771C
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
		Dialoguer.events.onEnded += this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.OnDialogueEndedHandler;
	}

	// Token: 0x06003159 RID: 12633 RVA: 0x000E956C File Offset: 0x000E776C
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
		Dialoguer.events.onEnded -= this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded -= this.OnDialogueEndedHandler;
	}

	// Token: 0x0600315A RID: 12634 RVA: 0x00029141 File Offset: 0x00027341
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "QuadratusGift")
		{
		}
	}

	// Token: 0x0600315B RID: 12635 RVA: 0x00029153 File Offset: 0x00027353
	public override void Activate()
	{
	}

	// Token: 0x0600315C RID: 12636 RVA: 0x000E95BC File Offset: 0x000E77BC
	public override MapUIInteractionDialogue Show(PlayerInput player)
	{
		int num = PlayerData.Data.DeathCount(PlayerId.Any);
		float num2 = Mathf.Sqrt((float)num);
		if (num > 48 && num2 == Mathf.Round(num2))
		{
			Dialoguer.SetGlobalFloat(15, 1f);
		}
		else
		{
			Dialoguer.SetGlobalFloat(15, 0f);
		}
		Dialoguer.SetGlobalFloat(this.dialoguerScholarVariableID, 3f);
		PlayerData.SaveCurrentFile();
		base.StartCoroutine(this.tween_cr(this.entitySpriteRenderer.color.a, 0.65f, EaseUtils.EaseType.easeInOutCubic, 0.5f));
		return null;
	}

	// Token: 0x0600315D RID: 12637 RVA: 0x000E9654 File Offset: 0x000E7854
	public override void Hide(MapUIInteractionDialogue dialogue)
	{
		if (Map.Current.CurrentState != Map.State.Ready)
		{
			return;
		}
		if (Map.Current.players[0].state != MapPlayerController.State.Walking)
		{
			return;
		}
		base.StartCoroutine(this.tween_cr(this.entitySpriteRenderer.color.a, 0f, EaseUtils.EaseType.easeInOutCubic, 0.5f));
	}

	// Token: 0x0600315E RID: 12638 RVA: 0x00029155 File Offset: 0x00027355
	public void OnDialogueEndedHandler()
	{
	}

	// Token: 0x0600315F RID: 12639 RVA: 0x000E96B4 File Offset: 0x000E78B4
	public IEnumerator tween_cr(float start, float end, EaseUtils.EaseType ease, float time)
	{
		if (start == end)
		{
			yield break;
		}
		float t = 0f;
		Color currentColor = Color.white;
		currentColor.a = start;
		this.entitySpriteRenderer.color = currentColor;
		while (t < time)
		{
			float val = EaseUtils.Ease(ease, start, end, t / time);
			currentColor.a = val;
			this.entitySpriteRenderer.color = currentColor;
			t += CupheadTime.Delta;
			yield return null;
		}
		currentColor.a = end;
		this.entitySpriteRenderer.color = currentColor;
		yield return null;
		yield break;
	}

	// Token: 0x040028AC RID: 10412
	public const int QUADRATUS_STATE_ID = 15;

	// Token: 0x040028AD RID: 10413
	[SerializeField]
	public SpriteRenderer entitySpriteRenderer;

	// Token: 0x040028AE RID: 10414
	[SerializeField]
	public int dialoguerScholarVariableID = 11;
}
