using System;

// Token: 0x020004F7 RID: 1271
public class AbstractPlayerComponent : AbstractCollidableObject
{
	// Token: 0x170003DC RID: 988
	// (get) Token: 0x06003464 RID: 13412 RVA: 0x0002AFE7 File Offset: 0x000291E7
	public AbstractPlayerController basePlayer
	{
		get
		{
			if (this._basePlayer == null)
			{
				this._basePlayer = base.GetComponent<AbstractPlayerController>();
			}
			return this._basePlayer;
		}
	}

	// Token: 0x06003465 RID: 13413 RVA: 0x0002B00C File Offset: 0x0002920C
	public sealed override void Awake()
	{
		base.Awake();
		this.OnAwake();
	}

	// Token: 0x06003466 RID: 13414 RVA: 0x0002B01A File Offset: 0x0002921A
	public virtual void OnAwake()
	{
	}

	// Token: 0x06003467 RID: 13415 RVA: 0x0002B01C File Offset: 0x0002921C
	public virtual void OnLevelStart()
	{
	}

	// Token: 0x04002B1B RID: 11035
	public AbstractPlayerController _basePlayer;
}
