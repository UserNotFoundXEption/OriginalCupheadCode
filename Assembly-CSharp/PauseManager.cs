using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004F1 RID: 1265
public static class PauseManager
{
	// Token: 0x06003440 RID: 13376 RVA: 0x0002AED8 File Offset: 0x000290D8
	public static void Reset()
	{
		PauseManager.state = PauseManager.State.Unpaused;
	}

	// Token: 0x06003441 RID: 13377 RVA: 0x0002AEE0 File Offset: 0x000290E0
	public static void AddChild(AbstractPausableComponent child)
	{
		PauseManager.children.Add(child);
	}

	// Token: 0x06003442 RID: 13378 RVA: 0x0002AEED File Offset: 0x000290ED
	public static void RemoveChild(AbstractPausableComponent child)
	{
		PauseManager.children.Remove(child);
	}

	// Token: 0x06003443 RID: 13379 RVA: 0x000F5F48 File Offset: 0x000F4148
	public static void Pause()
	{
		if (PauseManager.state == PauseManager.State.Paused)
		{
			return;
		}
		PauseManager.state = PauseManager.State.Paused;
		AudioListener.pause = true;
		PauseManager.oldSpeed = CupheadTime.GlobalSpeed;
		CupheadTime.GlobalSpeed = 0f;
		foreach (AbstractPausableComponent abstractPausableComponent in PauseManager.children)
		{
			abstractPausableComponent.OnPause();
		}
		PauseManager.SetChildren(false);
	}

	// Token: 0x06003444 RID: 13380 RVA: 0x000F5FD4 File Offset: 0x000F41D4
	public static void Unpause()
	{
		if (PauseManager.state == PauseManager.State.Unpaused)
		{
			return;
		}
		PauseManager.state = PauseManager.State.Unpaused;
		AudioListener.pause = false;
		CupheadTime.GlobalSpeed = PauseManager.oldSpeed;
		foreach (AbstractPausableComponent abstractPausableComponent in PauseManager.children)
		{
			abstractPausableComponent.OnUnpause();
		}
		PauseManager.SetChildren(true);
	}

	// Token: 0x06003445 RID: 13381 RVA: 0x0002AEFB File Offset: 0x000290FB
	public static void Toggle()
	{
		if (PauseManager.state == PauseManager.State.Paused)
		{
			PauseManager.Unpause();
		}
		else
		{
			PauseManager.Pause();
		}
	}

	// Token: 0x06003446 RID: 13382 RVA: 0x000F6058 File Offset: 0x000F4258
	public static void SetChildren(bool enabled)
	{
		for (int i = 0; i < PauseManager.children.Count; i++)
		{
			AbstractPausableComponent abstractPausableComponent = PauseManager.children[i];
			if (abstractPausableComponent == null)
			{
				PauseManager.children.Remove(abstractPausableComponent);
				i--;
			}
			else if (enabled)
			{
				abstractPausableComponent.enabled = abstractPausableComponent.preEnabled;
			}
			else
			{
				abstractPausableComponent.preEnabled = abstractPausableComponent.enabled;
				abstractPausableComponent.enabled = false;
			}
		}
	}

	// Token: 0x04002AFC RID: 11004
	public static PauseManager.State state;

	// Token: 0x04002AFD RID: 11005
	public static float oldSpeed;

	// Token: 0x04002AFE RID: 11006
	public static List<AbstractPausableComponent> children = new List<AbstractPausableComponent>();

	// Token: 0x0200114D RID: 4429
	public enum State
	{
		// Token: 0x040079C1 RID: 31169
		Unpaused,
		// Token: 0x040079C2 RID: 31170
		Paused
	}
}
