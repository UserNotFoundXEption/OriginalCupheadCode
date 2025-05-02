using System;

// Token: 0x020004F0 RID: 1264
public class SoundEmitter
{
	// Token: 0x0600343E RID: 13374 RVA: 0x0002AEBB File Offset: 0x000290BB
	public SoundEmitter(AbstractPausableComponent parent)
	{
		this.parent = parent;
	}

	// Token: 0x0600343F RID: 13375 RVA: 0x0002AECA File Offset: 0x000290CA
	public void Add(string key)
	{
		this.parent.EmitSound(key);
	}

	// Token: 0x04002AFB RID: 11003
	public AbstractPausableComponent parent;
}
