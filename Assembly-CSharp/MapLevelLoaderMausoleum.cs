using System;
using Rewired;
using UnityEngine;

// Token: 0x02000485 RID: 1157
public class MapLevelLoaderMausoleum : MapLevelLoader
{
	// Token: 0x060030D7 RID: 12503 RVA: 0x000E7B0C File Offset: 0x000E5D0C
	public override void Update()
	{
		switch (this.interactor)
		{
		default:
			if (base.PlayerWithinDistance(0))
			{
				Player actions = Map.Current.players[0].input.actions;
				if (actions.GetButton(11) && actions.GetButton(12))
				{
					this.currentDuration += CupheadTime.Delta;
				}
				else
				{
					this.currentDuration = 0f;
				}
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Mugman:
			if (base.PlayerWithinDistance(1))
			{
				Player actions2 = Map.Current.players[1].input.actions;
				if (actions2.GetButton(11) && actions2.GetButton(12))
				{
					this.currentDuration += CupheadTime.Delta;
				}
				else
				{
					this.currentDuration = 0f;
				}
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Either:
		{
			bool flag = false;
			if (base.PlayerWithinDistance(0))
			{
				Player actions3 = Map.Current.players[0].input.actions;
				if (actions3.GetButton(11) && actions3.GetButton(12))
				{
					this.currentDuration += CupheadTime.Delta;
					flag = true;
				}
			}
			if (base.PlayerWithinDistance(1))
			{
				Player actions4 = Map.Current.players[1].input.actions;
				if (actions4.GetButton(11) && actions4.GetButton(12))
				{
					this.currentDuration += CupheadTime.Delta;
					flag = true;
				}
			}
			if (!flag)
			{
				this.currentDuration = 0f;
			}
			break;
		}
		case AbstractMapInteractiveEntity.Interactor.Both:
			if (Map.Current.players[0] == null || Map.Current.players[1] == null)
			{
				return;
			}
			if (base.PlayerWithinDistance(0) && base.PlayerWithinDistance(1))
			{
				if (Map.Current.players[0].input.actions.GetButton(13))
				{
					if (Map.Current.players[1].input.actions.GetButton(13))
					{
						this.currentDuration += CupheadTime.Delta;
					}
					else
					{
						this.currentDuration = 0f;
					}
				}
				else
				{
					this.currentDuration = 0f;
				}
			}
			break;
		}
		if (this.currentDuration >= this.pressDurationToReEnable)
		{
			this.reEnabled = true;
		}
		if (!this.reEnabled)
		{
			return;
		}
		base.Update();
	}

	// Token: 0x04002853 RID: 10323
	[SerializeField]
	public float pressDurationToReEnable = 1f;

	// Token: 0x04002854 RID: 10324
	public bool reEnabled;

	// Token: 0x04002855 RID: 10325
	public float currentDuration;
}
