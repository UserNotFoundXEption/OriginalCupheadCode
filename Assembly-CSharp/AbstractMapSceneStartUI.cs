using System;
using System.Collections;
using Rewired;
using UnityEngine;

// Token: 0x020004B9 RID: 1209
public class AbstractMapSceneStartUI : AbstractMonoBehaviour
{
	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x0600322C RID: 12844 RVA: 0x000299EB File Offset: 0x00027BEB
	// (set) Token: 0x0600322D RID: 12845 RVA: 0x000299F3 File Offset: 0x00027BF3
	public AbstractMapSceneStartUI.State CurrentState { get; set; }

	// Token: 0x1400006E RID: 110
	// (add) Token: 0x0600322E RID: 12846 RVA: 0x000EC38C File Offset: 0x000EA58C
	// (remove) Token: 0x0600322F RID: 12847 RVA: 0x000EC3C4 File Offset: 0x000EA5C4
	public event Action OnLoadLevelEvent;

	// Token: 0x1400006F RID: 111
	// (add) Token: 0x06003230 RID: 12848 RVA: 0x000EC3FC File Offset: 0x000EA5FC
	// (remove) Token: 0x06003231 RID: 12849 RVA: 0x000EC434 File Offset: 0x000EA634
	public event Action OnBackEvent;

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x06003232 RID: 12850 RVA: 0x000EC46C File Offset: 0x000EA66C
	public bool Able
	{
		get
		{
			return this.CurrentState == AbstractMapSceneStartUI.State.Active && AbstractEquipUI.Current.CurrentState == AbstractEquipUI.ActiveState.Inactive && Map.Current.CurrentState == Map.State.Ready && !InterruptingPrompt.IsInterrupting() && (!(Map.Current != null) || Map.Current.CurrentState != Map.State.Graveyard);
		}
	}

	// Token: 0x06003233 RID: 12851 RVA: 0x000299FC File Offset: 0x00027BFC
	public override void Awake()
	{
		base.Awake();
		this.timeLayer = CupheadTime.Layer.UI;
		this.ignoreGlobalTime = true;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.SetAlpha(0f);
	}

	// Token: 0x06003234 RID: 12852 RVA: 0x00029A29 File Offset: 0x00027C29
	public virtual void Start()
	{
		PlayerManager.OnPlayerJoinedEvent += this.OnPlayerJoined;
		PlayerManager.OnPlayerJoinedEvent += this.OnPlayerLeft;
	}

	// Token: 0x06003235 RID: 12853 RVA: 0x00029A4D File Offset: 0x00027C4D
	public void OnDestroy()
	{
		PlayerManager.OnPlayerJoinedEvent -= this.OnPlayerJoined;
		PlayerManager.OnPlayerJoinedEvent -= this.OnPlayerLeft;
	}

	// Token: 0x06003236 RID: 12854 RVA: 0x00029A71 File Offset: 0x00027C71
	public bool GetButtonDown(CupheadButton button)
	{
		return this.Able && !InterruptingPrompt.IsInterrupting() && (this.player != null && this.player.GetButtonDown((int)button));
	}

	// Token: 0x06003237 RID: 12855 RVA: 0x00029AA9 File Offset: 0x00027CA9
	public void OnPlayerJoined(PlayerId playerId)
	{
	}

	// Token: 0x06003238 RID: 12856 RVA: 0x00029AAB File Offset: 0x00027CAB
	public void OnPlayerLeft(PlayerId playerId)
	{
	}

	// Token: 0x06003239 RID: 12857 RVA: 0x00029AAD File Offset: 0x00027CAD
	public void LoadLevel()
	{
		this.CurrentState = AbstractMapSceneStartUI.State.Loading;
		if (this.OnLoadLevelEvent != null)
		{
			this.OnLoadLevelEvent();
		}
		this.OnLoadLevelEvent = null;
	}

	// Token: 0x0600323A RID: 12858 RVA: 0x00029AD3 File Offset: 0x00027CD3
	public void In(MapPlayerController playerController)
	{
		this.player = playerController.input.actions;
		base.StartCoroutine(this.fade_cr(1f, AbstractMapSceneStartUI.State.Active));
	}

	// Token: 0x0600323B RID: 12859 RVA: 0x00029AF9 File Offset: 0x00027CF9
	public void Out()
	{
		base.StartCoroutine(this.fade_cr(0f, AbstractMapSceneStartUI.State.Inactive));
		if (this.OnBackEvent != null)
		{
			this.OnBackEvent();
		}
		this.OnBackEvent = null;
	}

	// Token: 0x0600323C RID: 12860 RVA: 0x00029B2B File Offset: 0x00027D2B
	public void SetAlpha(float alpha)
	{
		this.canvasGroup.alpha = alpha;
	}

	// Token: 0x0600323D RID: 12861 RVA: 0x000EC4DC File Offset: 0x000EA6DC
	public IEnumerator fade_cr(float end, AbstractMapSceneStartUI.State endState)
	{
		float t = 0f;
		float start = this.canvasGroup.alpha;
		this.CurrentState = AbstractMapSceneStartUI.State.Animating;
		while (t < 0.2f)
		{
			float val = t / 0.2f;
			this.SetAlpha(Mathf.Lerp(start, end, val));
			t += Time.deltaTime;
			yield return null;
		}
		this.SetAlpha(end);
		this.CurrentState = endState;
		yield break;
	}

	// Token: 0x04002928 RID: 10536
	[HideInInspector]
	public string level;

	// Token: 0x0400292A RID: 10538
	public CanvasGroup canvasGroup;

	// Token: 0x0400292B RID: 10539
	public Player player;

	// Token: 0x02001119 RID: 4377
	public enum State
	{
		// Token: 0x040078BF RID: 30911
		Inactive,
		// Token: 0x040078C0 RID: 30912
		Animating,
		// Token: 0x040078C1 RID: 30913
		Active,
		// Token: 0x040078C2 RID: 30914
		Loading
	}
}
