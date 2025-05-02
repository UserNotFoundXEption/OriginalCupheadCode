using System;

// Token: 0x02000487 RID: 1159
public class MapLockedEntity : AbstractMapInteractiveEntity
{
	// Token: 0x060030E1 RID: 12513 RVA: 0x00028A30 File Offset: 0x00026C30
	public override void Reset()
	{
		base.Reset();
		this.dialogueProperties = new AbstractUIInteractionDialogue.Properties("???");
	}
}
