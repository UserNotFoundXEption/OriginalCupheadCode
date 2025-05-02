using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000640 RID: 1600
	public interface ICustomSelectable : ICancelHandler, IEventSystemHandler
	{
		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x060042D3 RID: 17107
		// (set) Token: 0x060042D4 RID: 17108
		Sprite disabledHighlightedSprite { get; set; }

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x060042D5 RID: 17109
		// (set) Token: 0x060042D6 RID: 17110
		Color disabledHighlightedColor { get; set; }

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x060042D7 RID: 17111
		// (set) Token: 0x060042D8 RID: 17112
		string disabledHighlightedTrigger { get; set; }

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x060042D9 RID: 17113
		// (set) Token: 0x060042DA RID: 17114
		bool autoNavUp { get; set; }

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x060042DB RID: 17115
		// (set) Token: 0x060042DC RID: 17116
		bool autoNavDown { get; set; }

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x060042DD RID: 17117
		// (set) Token: 0x060042DE RID: 17118
		bool autoNavLeft { get; set; }

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x060042DF RID: 17119
		// (set) Token: 0x060042E0 RID: 17120
		bool autoNavRight { get; set; }

		// Token: 0x140000F7 RID: 247
		// (add) Token: 0x060042E1 RID: 17121
		// (remove) Token: 0x060042E2 RID: 17122
		event UnityAction CancelEvent;
	}
}
