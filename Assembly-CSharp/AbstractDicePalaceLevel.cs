using System;

// Token: 0x020001CA RID: 458
public abstract class AbstractDicePalaceLevel : Level
{
	// Token: 0x06001579 RID: 5497 RVA: 0x00012469 File Offset: 0x00010669
	public AbstractDicePalaceLevel()
	{
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x0600157A RID: 5498
	public abstract DicePalaceLevels CurrentDicePalaceLevel { get; }

	// Token: 0x0600157B RID: 5499 RVA: 0x0009C1F4 File Offset: 0x0009A3F4
	public override void Awake()
	{
		base.Awake();
		if (DicePalaceMainLevelGameInfo.GameInfo != null)
		{
			Level.Current.OnLoseEvent += DicePalaceMainLevelGameInfo.GameInfo.CleanUp;
		}
		base.OnLoseEvent += this.ResetScore;
	}

	// Token: 0x0600157C RID: 5500 RVA: 0x00012471 File Offset: 0x00010671
	public override void OnDestroy()
	{
		base.OnDestroy();
		base.OnLoseEvent -= this.ResetScore;
	}

	// Token: 0x0600157D RID: 5501 RVA: 0x0001248B File Offset: 0x0001068B
	public void ResetScore()
	{
		base.OnLoseEvent -= this.ResetScore;
		base.CleanUpScore();
	}

	// Token: 0x0600157E RID: 5502 RVA: 0x000124A5 File Offset: 0x000106A5
	public override void CheckIfInABossesHub()
	{
		base.CheckIfInABossesHub();
		if (!Level.IsTowerOfPower)
		{
			Level.IsDicePalace = true;
		}
	}
}
