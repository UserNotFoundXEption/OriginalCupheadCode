using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired
{
	// Token: 0x02000656 RID: 1622
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class InputManager : InputManager_Base
	{
		// Token: 0x06004469 RID: 17513 RVA: 0x0013D314 File Offset: 0x0013B514
		public override void DetectPlatform()
		{
			this.editorPlatform = 0;
			this.platform = 0;
			this.webplayerPlatform = 0;
			this.isEditor = false;
			string text = SystemInfo.deviceName ?? string.Empty;
			string text2 = SystemInfo.deviceModel ?? string.Empty;
			this.platform = 1;
		}

		// Token: 0x0600446A RID: 17514 RVA: 0x00036622 File Offset: 0x00034822
		public override void CheckRecompile()
		{
		}

		// Token: 0x0600446B RID: 17515 RVA: 0x00036624 File Offset: 0x00034824
		public override IExternalTools GetExternalTools()
		{
			return new ExternalTools();
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x0003662B File Offset: 0x0003482B
		public bool CheckDeviceName(string searchPattern, string deviceName, string deviceModel)
		{
			return Regex.IsMatch(deviceName, searchPattern, RegexOptions.IgnoreCase) || Regex.IsMatch(deviceModel, searchPattern, RegexOptions.IgnoreCase);
		}
	}
}
