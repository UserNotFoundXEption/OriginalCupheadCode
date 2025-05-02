using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class MapDialogueInteraction : AbstractMapInteractiveEntity
{
	// Token: 0x060009BE RID: 2494 RVA: 0x000794A8 File Offset: 0x000776A8
	public virtual void Start()
	{
		if (this.speechBubble == null)
		{
			Vector3 vector = base.transform.position;
			if (this.speechBubblePositions != null)
			{
				vector = this.ApplyCustonMargin(vector);
			}
			else
			{
				vector.x += this.speechBubblePosition.x;
				vector.y += this.speechBubblePosition.y;
			}
			this.speechBubble = SpeechBubble.Instance;
			if (this.speechBubble == null)
			{
				this.speechBubble = Object.Instantiate<GameObject>(this.speechBubblePrefab.gameObject, vector, Quaternion.identity, MapUI.Current.sceneCanvas.transform).GetComponent<SpeechBubble>();
			}
		}
		Dialoguer.events.onEnded += this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.OnDialogueEndedHandler;
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x00009040 File Offset: 0x00007240
	public override void OnDestroy()
	{
		base.OnDestroy();
		Dialoguer.events.onEnded -= this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded -= this.OnDialogueEndedHandler;
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x00009074 File Offset: 0x00007274
	public void OnDialogueEndedHandler()
	{
		this.IsDialogueEnded = true;
		this.Check();
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x00079594 File Offset: 0x00077794
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Vector3 vector = base.transform.position;
		if (this.speechBubblePositions != null)
		{
			vector = this.ApplyCustonMargin(vector);
		}
		else
		{
			vector.x += this.speechBubblePosition.x;
			vector.y += this.speechBubblePosition.y;
		}
		Gizmos.DrawWireSphere(vector, this.interactionDistance * 0.5f);
		vector = base.transform.position;
		vector.x += this.panCameraToPosition.x;
		vector.y += this.panCameraToPosition.y;
		Gizmos.DrawWireSphere(vector, this.interactionDistance * 0.5f);
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x00079660 File Offset: 0x00077860
	public override void Activate(MapPlayerController player)
	{
		if (this.dialogues[(int)player.id] != null && this.dialogues[(int)player.id].transform.localScale.x == 1f && MapBasicStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && MapDifficultySelectStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && MapConfirmStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && (Map.Current == null || Map.Current.CurrentState != Map.State.Graveyard))
		{
			base.Activate(player);
			this.StartSpeechBubble();
		}
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x00079708 File Offset: 0x00077908
	public virtual void StartSpeechBubble()
	{
		if (this.speechBubble.displayState == SpeechBubble.DisplayState.Hidden)
		{
			Vector3 vector = base.transform.position;
			if (this.speechBubblePositions != null)
			{
				vector = this.ApplyCustonMargin(vector);
			}
			else
			{
				vector.x += this.speechBubblePosition.x;
				vector.y += this.speechBubblePosition.y;
			}
			this.speechBubble.basePosition = vector;
			vector = base.transform.position;
			vector.x += this.panCameraToPosition.x;
			vector.y += this.panCameraToPosition.y;
			this.speechBubble.panPosition = vector;
			this.speechBubble.maxLines = this.maxLines;
			this.speechBubble.tailOnTheLeft = this.tailOnTheLeft;
			this.speechBubble.expandOnTheRight = this.expandOnTheRight;
			this.speechBubble.hideTail = this.hideTail;
			if (this.cutsceneCoroutine != null)
			{
				base.StopCoroutine(this.cutsceneCoroutine);
			}
			this.cutsceneCoroutine = base.StartCoroutine(this.CutScene_cr());
		}
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x00009083 File Offset: 0x00007283
	public override void Check()
	{
		if (this.disabledActivations)
		{
			return;
		}
		base.Check();
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x00079848 File Offset: 0x00077A48
	public IEnumerator CutScene_cr()
	{
		if (this.speechBubble.displayState != SpeechBubble.DisplayState.Hidden)
		{
			yield break;
		}
		for (int i = 0; i < Map.Current.players.Length; i++)
		{
			if (!(Map.Current.players[i] == null))
			{
				Map.Current.players[i].Disable();
			}
		}
		yield return null;
		this.currentlySpeaking = true;
		Dialoguer.StartDialogue(this.dialogueInteraction);
		DialoguerEvents.EndedHandler afterDialogue = null;
		afterDialogue = delegate
		{
			Dialoguer.events.onEnded -= afterDialogue;
			this.$this.StartCoroutine(this.$this.reactivate_input_cr());
		};
		Dialoguer.events.onEnded += afterDialogue;
		yield break;
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x00079864 File Offset: 0x00077A64
	public IEnumerator reactivate_input_cr()
	{
		if (CupheadMapCamera.Current != null)
		{
			CupheadMapCamera.Current.SetActiveCollider(false);
		}
		while (CupheadMapCamera.Current != null && CupheadMapCamera.Current.IsCameraFarFromPlayer())
		{
			yield return null;
		}
		if (CupheadMapCamera.Current != null)
		{
			CupheadMapCamera.Current.SetActiveCollider(true);
		}
		for (int i = 0; i < Map.Current.players.Length; i++)
		{
			if (!(Map.Current.players[i] == null))
			{
				Map.Current.players[i].Enable();
			}
		}
		this.currentlySpeaking = false;
		yield break;
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x00079880 File Offset: 0x00077A80
	public Vector3 ApplyCustonMargin(Vector3 pos)
	{
		int num = 0;
		bool flag = false;
		while (!flag && num < this.speechBubblePositions.Length)
		{
			flag = (this.speechBubblePositions[num].languageApplied == Localization.language);
			num = ((!flag) ? (num + 1) : num);
		}
		if (flag)
		{
			MapDialogueInteraction.speechBubblePositionLanguage speechBubblePositionLanguage = this.speechBubblePositions[num];
			pos.x += speechBubblePositionLanguage.speechBubblePosition.x;
			pos.y += speechBubblePositionLanguage.speechBubblePosition.y;
		}
		else
		{
			pos.x += this.speechBubblePosition.x;
			pos.y += this.speechBubblePosition.y;
		}
		return pos;
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x00079958 File Offset: 0x00077B58
	public IEnumerator DebugStartDialogue()
	{
		yield return base.StartCoroutine(this.debugActivate_input_cr());
		yield break;
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x00079974 File Offset: 0x00077B74
	public IEnumerator debugActivate_input_cr()
	{
		bool DoneLanguage = false;
		int index = 0;
		WaitForSeconds Wait = new WaitForSeconds(0.3f);
		Localization.Languages startedLanguage = Localization.language;
		int ConditionIndex = 0;
		bool DoneVariables = false;
		yield return Wait;
		yield return Wait;
		while (!DoneLanguage)
		{
			DoneVariables = false;
			ConditionIndex = 0;
			while (!DoneVariables)
			{
				if (this.DebugDialogerCondition.Count > 0)
				{
					Dialoguer.SetGlobalFloat(this.DebugDialogerCondition[ConditionIndex].ConditionId, this.DebugDialogerCondition[ConditionIndex].Values);
				}
				this.IsDialogueEnded = false;
				index = 0;
				yield return Wait;
				yield return Wait;
				this.Activate(Map.Current.players[0]);
				yield return Wait;
				while (!this.IsDialogueEnded)
				{
					string ConditionKey = string.Empty;
					if (this.DebugDialogerCondition.Count > 0)
					{
						ConditionKey = string.Concat(new object[]
						{
							"_",
							this.DebugDialogerCondition[ConditionIndex].ConditionId,
							"_",
							this.DebugDialogerCondition[ConditionIndex].Values
						});
					}
					ScreenshotHandler.cameraType camera = ScreenshotHandler.cameraType.Map;
					string folderName = "LOC_Screenshot";
					object[] array = new object[6];
					array[0] = this.dialogueInteraction.ToString();
					array[1] = ConditionKey;
					array[2] = "_";
					array[3] = Localization.language;
					array[4] = "_";
					int num = 5;
					int num2;
					index = (num2 = index) + 1;
					array[num] = num2;
					ScreenshotHandler.TakeScreenshot_Static(camera, folderName, string.Concat(array));
					yield return Wait;
					Dialoguer.ContinueDialogue();
					yield return Wait;
				}
				ConditionIndex++;
				if (ConditionIndex >= this.DebugDialogerCondition.Count)
				{
					DoneVariables = true;
				}
			}
			yield return null;
			yield return null;
			yield return null;
			if (Localization.language == startedLanguage)
			{
				DoneLanguage = true;
			}
		}
		yield break;
	}

	// Token: 0x04000753 RID: 1875
	[SerializeField]
	public SpeechBubble speechBubblePrefab;

	// Token: 0x04000754 RID: 1876
	[SerializeField]
	public Vector2 speechBubblePosition;

	// Token: 0x04000755 RID: 1877
	[SerializeField]
	public MapDialogueInteraction.speechBubblePositionLanguage[] speechBubblePositions;

	// Token: 0x04000756 RID: 1878
	[SerializeField]
	public Vector2 panCameraToPosition;

	// Token: 0x04000757 RID: 1879
	[SerializeField]
	public int maxLines = -1;

	// Token: 0x04000758 RID: 1880
	[SerializeField]
	public bool tailOnTheLeft;

	// Token: 0x04000759 RID: 1881
	[SerializeField]
	public bool hideTail;

	// Token: 0x0400075A RID: 1882
	[SerializeField]
	public bool expandOnTheRight;

	// Token: 0x0400075B RID: 1883
	public SpeechBubble speechBubble;

	// Token: 0x0400075C RID: 1884
	public DialoguerDialogues dialogueInteraction;

	// Token: 0x0400075D RID: 1885
	public Coroutine cutsceneCoroutine;

	// Token: 0x0400075E RID: 1886
	[HideInInspector]
	public bool disabledActivations;

	// Token: 0x0400075F RID: 1887
	[Header("DEBUG")]
	public List<MapDialogueInteraction.DEBUG_DialoguerCondition> DebugDialogerCondition = new List<MapDialogueInteraction.DEBUG_DialoguerCondition>();

	// Token: 0x04000760 RID: 1888
	public bool IsDialogueEnded;

	// Token: 0x04000761 RID: 1889
	public bool currentlySpeaking;

	// Token: 0x02000938 RID: 2360
	[Serializable]
	public struct speechBubblePositionLanguage
	{
		// Token: 0x0400458A RID: 17802
		public Localization.Languages languageApplied;

		// Token: 0x0400458B RID: 17803
		public Vector2 speechBubblePosition;
	}

	// Token: 0x02000939 RID: 2361
	[Serializable]
	public struct DEBUG_DialoguerCondition
	{
		// Token: 0x0400458C RID: 17804
		public int ConditionId;

		// Token: 0x0400458D RID: 17805
		public float Values;
	}
}
