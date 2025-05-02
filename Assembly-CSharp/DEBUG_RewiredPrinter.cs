using System;
using System.Text;
using Rewired;
using UnityEngine;

// Token: 0x020005CA RID: 1482
public class DEBUG_RewiredPrinter : MonoBehaviour
{
	// Token: 0x06003DE8 RID: 15848 RVA: 0x00031D64 File Offset: 0x0002FF64
	public void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06003DE9 RID: 15849 RVA: 0x0011AB34 File Offset: 0x00118D34
	public void OnGUI()
	{
		if (!ReInput.isReady)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("===PLAYERS===");
		foreach (Player player in ReInput.players.AllPlayers)
		{
			stringBuilder.AppendLine(player.name);
			foreach (Joystick j in player.controllers.Joysticks)
			{
				DEBUG_RewiredPrinter.appendControllerInfo(j, stringBuilder);
			}
		}
		stringBuilder.AppendLine("===UNASSIGNED===");
		foreach (Joystick joystick in ReInput.controllers.Joysticks)
		{
			if (!ReInput.controllers.IsJoystickAssigned(joystick))
			{
				DEBUG_RewiredPrinter.appendControllerInfo(joystick, stringBuilder);
			}
		}
		stringBuilder.AppendLine("===BUTTONS===");
		GUI.Box(new Rect(0f, 0f, 700f, 400f), stringBuilder.ToString());
	}

	// Token: 0x06003DEA RID: 15850 RVA: 0x0011ACAC File Offset: 0x00118EAC
	public static void appendControllerInfo(Joystick j, StringBuilder builder)
	{
		string format = "{0} :: {1}";
		object name = j.name;
		int id = j.id;
		builder.AppendFormat(format, name, id.ToString());
		builder.Append("\n");
	}
}
