using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002B4 RID: 692
public class HouseLevelExit : AbstractLevelInteractiveEntity
{
	// Token: 0x06001EF8 RID: 7928 RVA: 0x000B50E0 File Offset: 0x000B32E0
	public override void Activate()
	{
		if (this.activated)
		{
			return;
		}
		base.playerActivating.transform.position += ((LevelPlayerController)base.playerActivating).motor.DistanceToGround() * Vector3.down;
		this.nonActivating = null;
		this.SwitchToRun(base.playerActivating.id);
		if (PlayerManager.Multiplayer)
		{
			this.nonActivating = (LevelPlayerController)PlayerManager.GetPlayer(PlayerId.PlayerTwo - (int)base.playerActivating.id);
		}
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06001EF9 RID: 7929 RVA: 0x000B5180 File Offset: 0x000B3380
	public void SwitchToRun(PlayerId id)
	{
		GameObject gameObject = (id != PlayerId.PlayerOne) ? ((!PlayerManager.player1IsMugman) ? this.mugmanRunning : this.cupheadRunning) : ((!PlayerManager.player1IsMugman) ? this.cupheadRunning : this.mugmanRunning);
		AbstractPlayerController player = PlayerManager.GetPlayer(id);
		if (player.stats.isChalice)
		{
			gameObject = this.chaliceRunning;
		}
		player.gameObject.SetActive(false);
		gameObject.transform.position = player.transform.position;
		gameObject.gameObject.SetActive(true);
		((LevelPlayerController)player).DisableInput();
		((LevelPlayerController)player).PauseAll();
	}

	// Token: 0x06001EFA RID: 7930 RVA: 0x000B5234 File Offset: 0x000B3434
	public IEnumerator go_cr()
	{
		this.activated = true;
		float timeToGround = 0f;
		if (this.nonActivating != null)
		{
			while (this.nonActivating != null && this.nonActivating.gameObject.activeInHierarchy && !this.nonActivating.motor.Grounded)
			{
				timeToGround += CupheadTime.Delta;
				yield return null;
			}
			if (this.nonActivating.gameObject.activeInHierarchy && this.nonActivating != null)
			{
				this.SwitchToRun(this.nonActivating.id);
			}
		}
		if (timeToGround < 1f)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f - timeToGround);
		}
		SceneLoader.LoadScene(this.sceneLoadOnExit, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		yield break;
	}

	// Token: 0x06001EFB RID: 7931 RVA: 0x0001A180 File Offset: 0x00018380
	public override void Show(PlayerId playerId)
	{
		base.state = AbstractLevelInteractiveEntity.State.Ready;
		this.dialogue = LevelUIInteractionDialogue.Create(this.dialogueProperties, PlayerManager.GetPlayer(playerId).input, this.dialogueOffset, 0f, LevelUIInteractionDialogue.TailPosition.Left, false);
	}

	// Token: 0x04001953 RID: 6483
	public bool activated;

	// Token: 0x04001954 RID: 6484
	[SerializeField]
	public GameObject cupheadRunning;

	// Token: 0x04001955 RID: 6485
	[SerializeField]
	public GameObject mugmanRunning;

	// Token: 0x04001956 RID: 6486
	[SerializeField]
	public GameObject chaliceRunning;

	// Token: 0x04001957 RID: 6487
	[SerializeField]
	public Scenes sceneLoadOnExit;

	// Token: 0x04001958 RID: 6488
	public LevelPlayerController nonActivating;
}
