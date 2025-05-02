using System;
using UnityEngine;

// Token: 0x02000593 RID: 1427
public class DamageReceiverChild : AbstractMonoBehaviour
{
	// Token: 0x170004EE RID: 1262
	// (get) Token: 0x06003C49 RID: 15433 RVA: 0x00030C39 File Offset: 0x0002EE39
	public DamageReceiver Receiver
	{
		get
		{
			return this.receiver;
		}
	}

	// Token: 0x06003C4A RID: 15434 RVA: 0x00030C41 File Offset: 0x0002EE41
	public void Start()
	{
		base.tag = this.receiver.tag;
	}

	// Token: 0x04002FCE RID: 12238
	[SerializeField]
	public DamageReceiver receiver;
}
