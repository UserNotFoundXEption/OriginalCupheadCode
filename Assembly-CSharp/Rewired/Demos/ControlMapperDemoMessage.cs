using System;
using System.Collections;
using Rewired.UI.ControlMapper;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x02000633 RID: 1587
	[AddComponentMenu("")]
	public class ControlMapperDemoMessage : MonoBehaviour
	{
		// Token: 0x06004104 RID: 16644 RVA: 0x0012FA40 File Offset: 0x0012DC40
		public void Awake()
		{
			if (this.controlMapper != null)
			{
				this.controlMapper.ScreenClosedEvent += this.OnControlMapperClosed;
				this.controlMapper.ScreenOpenedEvent += this.OnControlMapperOpened;
			}
		}

		// Token: 0x06004105 RID: 16645 RVA: 0x0003415A File Offset: 0x0003235A
		public void Start()
		{
			this.SelectDefault();
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x00034162 File Offset: 0x00032362
		public void OnControlMapperClosed()
		{
			base.gameObject.SetActive(true);
			base.StartCoroutine(this.SelectDefaultDeferred());
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x0003417D File Offset: 0x0003237D
		public void OnControlMapperOpened()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004108 RID: 16648 RVA: 0x0003418B File Offset: 0x0003238B
		public void SelectDefault()
		{
			if (EventSystem.current == null)
			{
				return;
			}
			if (this.defaultSelectable != null)
			{
				EventSystem.current.SetSelectedGameObject(this.defaultSelectable.gameObject);
			}
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x0012FA8C File Offset: 0x0012DC8C
		public IEnumerator SelectDefaultDeferred()
		{
			yield return null;
			this.SelectDefault();
			yield break;
		}

		// Token: 0x040033A6 RID: 13222
		public ControlMapper controlMapper;

		// Token: 0x040033A7 RID: 13223
		public Selectable defaultSelectable;
	}
}
