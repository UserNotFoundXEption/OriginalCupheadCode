using System;
using UnityEngine;

// Token: 0x0200047D RID: 1149
public class MapGraveyardGrave : MonoBehaviour
{
	// Token: 0x060030A1 RID: 12449 RVA: 0x000285E9 File Offset: 0x000267E9
	public void Start()
	{
		this.hasCharm = this.HasCharm();
	}

	// Token: 0x060030A2 RID: 12450 RVA: 0x000E6D44 File Offset: 0x000E4F44
	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<MapPlayerController>())
		{
			MapPlayerController component = collision.GetComponent<MapPlayerController>();
			if (component.id == PlayerId.PlayerOne)
			{
				if (this.player1 == null)
				{
					this.player1 = component;
				}
				this.p1InTrigger = true;
			}
			else
			{
				if (this.player2 == null)
				{
					this.player2 = component;
				}
				this.p2InTrigger = true;
			}
		}
	}

	// Token: 0x060030A3 RID: 12451 RVA: 0x000E6DB8 File Offset: 0x000E4FB8
	public void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<MapPlayerController>())
		{
			MapPlayerController component = collision.GetComponent<MapPlayerController>();
			if (component.id == PlayerId.PlayerOne)
			{
				this.p1InTrigger = false;
			}
			else
			{
				this.p2InTrigger = false;
			}
		}
	}

	// Token: 0x060030A4 RID: 12452 RVA: 0x000E6DFC File Offset: 0x000E4FFC
	public bool HasCharm()
	{
		return (PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, Charm.charm_curse) && CharmCurse.CalculateLevel(PlayerId.PlayerOne) >= 0) || (PlayerData.Data.IsUnlocked(PlayerId.PlayerTwo, Charm.charm_curse) && CharmCurse.CalculateLevel(PlayerId.PlayerTwo) >= 0);
	}

	// Token: 0x060030A5 RID: 12453 RVA: 0x000E6E54 File Offset: 0x000E5054
	public void Update()
	{
		if (SceneLoader.IsInBlurTransition)
		{
			return;
		}
		if (this.hasCharm && !this.main.canReenter)
		{
			return;
		}
		if (this.canInteract || (this.hasCharm && this.main.canReenter))
		{
			if (this.p1InTrigger && this.player1.input.actions.GetButtonDown(13))
			{
				if (this.player1.animationController.facingUpwards)
				{
					this.InteractWith(0);
				}
			}
			else if (this.p2InTrigger && this.player2.input.actions.GetButtonDown(13) && this.player2.animationController.facingUpwards)
			{
				this.InteractWith(1);
			}
		}
	}

	// Token: 0x060030A6 RID: 12454 RVA: 0x000E6F3C File Offset: 0x000E513C
	public void InteractWith(int playerNum)
	{
		if (!this.isResetGrave)
		{
			this.canInteract = false;
		}
		this.main.ActivatedGrave(this.index, playerNum, (!this.isResetGrave) ? this.ghostPos.transform.position : Vector3.zero);
	}

	// Token: 0x060030A7 RID: 12455 RVA: 0x000285F7 File Offset: 0x000267F7
	public void SetInteractable(bool value)
	{
		this.canInteract = value;
	}

	// Token: 0x0400282D RID: 10285
	[SerializeField]
	public bool isResetGrave;

	// Token: 0x0400282E RID: 10286
	[SerializeField]
	public int index;

	// Token: 0x0400282F RID: 10287
	public MapPlayerController player1;

	// Token: 0x04002830 RID: 10288
	public MapPlayerController player2;

	// Token: 0x04002831 RID: 10289
	public bool p1InTrigger;

	// Token: 0x04002832 RID: 10290
	public bool p2InTrigger;

	// Token: 0x04002833 RID: 10291
	public bool canInteract;

	// Token: 0x04002834 RID: 10292
	[SerializeField]
	public MapGraveyardHandler main;

	// Token: 0x04002835 RID: 10293
	[SerializeField]
	public Transform ghostPos;

	// Token: 0x04002836 RID: 10294
	public bool hasCharm;
}
