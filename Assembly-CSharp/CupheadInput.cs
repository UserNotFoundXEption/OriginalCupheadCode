using System;
using Rewired;

// Token: 0x020000F9 RID: 249
public class CupheadInput
{
	// Token: 0x06000B9E RID: 2974 RVA: 0x00080918 File Offset: 0x0007EB18
	public static Localization.Translation InputDisplayForButton(CupheadButton button, int rewiredPlayerId = 0)
	{
		Player.ControllerHelper controllers = ReInput.players.GetPlayer(rewiredPlayerId).controllers;
		ActionElementMap firstElementMapWithAction;
		if (controllers != null && controllers.joystickCount > 0)
		{
			ControllerType controllerType = 2;
			Controller lastActiveController = ReInput.players.GetPlayer(rewiredPlayerId).controllers.GetLastActiveController();
			if (lastActiveController != null)
			{
				controllerType = lastActiveController.type;
			}
			firstElementMapWithAction = controllers.maps.GetFirstElementMapWithAction(controllerType, (int)button, true);
		}
		else
		{
			if (PlatformHelper.IsConsole)
			{
				return default(Localization.Translation);
			}
			firstElementMapWithAction = ReInput.players.GetPlayer(rewiredPlayerId).controllers.maps.GetFirstElementMapWithAction((int)button, true);
		}
		if (firstElementMapWithAction == null)
		{
			return new Localization.Translation
			{
				text = string.Empty
			};
		}
		string text = firstElementMapWithAction.elementIdentifierName;
		if (button == CupheadButton.EquipMenu && text.Contains("Shift"))
		{
			text = "Shift";
		}
		string text2 = CupheadInput.handleCustomGlyphs(text, rewiredPlayerId);
		Localization.Translation result = Localization.Translate(text);
		if (text2 == null)
		{
			if (!string.IsNullOrEmpty(result.text))
			{
				text = result.text;
			}
		}
		else
		{
			text = text2;
		}
		text = text.ToUpper();
		text = text.Replace(" SHOULDER", "B");
		text = text.Replace(" BUMPER", "B");
		text = text.Replace(" TRIGGER", "T");
		text = text.Replace("LEFT", "L");
		text = text.Replace("RIGHT", "R");
		text = text.Replace("R SHIFT", "SHIFT");
		text = text.Replace("L SHIFT", "SHIFT");
		text = text.Replace(" +", string.Empty);
		text = text.Replace(" -", string.Empty);
		result.text = text;
		return result;
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x0000A585 File Offset: 0x00008785
	public static string handleCustomGlyphs(string input, int rewiredPlayerId)
	{
		return null;
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x00080AFC File Offset: 0x0007ECFC
	public static CupheadInput.InputSymbols InputSymbolForButton(CupheadButton button)
	{
		CupheadInput.InputSymbols result;
		switch (button)
		{
		case CupheadButton.Jump:
			result = CupheadInput.InputSymbols.XBOX_A;
			break;
		case CupheadButton.Shoot:
			result = CupheadInput.InputSymbols.XBOX_X;
			break;
		case CupheadButton.Super:
			result = CupheadInput.InputSymbols.XBOX_B;
			break;
		case CupheadButton.SwitchWeapon:
			result = CupheadInput.InputSymbols.XBOX_LB;
			break;
		case CupheadButton.Lock:
			result = CupheadInput.InputSymbols.XBOX_RB;
			break;
		case CupheadButton.Dash:
			result = CupheadInput.InputSymbols.XBOX_Y;
			break;
		default:
			if (button != CupheadButton.None)
			{
			}
			result = CupheadInput.InputSymbols.XBOX_NONE;
			break;
		case CupheadButton.Accept:
			result = CupheadInput.InputSymbols.XBOX_A;
			break;
		case CupheadButton.Cancel:
			result = CupheadInput.InputSymbols.XBOX_B;
			break;
		}
		return result;
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x0000A588 File Offset: 0x00008788
	public static string DialogueStringFromButton(CupheadButton button)
	{
		return " {" + button + "} ";
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x00080B98 File Offset: 0x0007ED98
	public static Joystick CheckForUnconnectedControllerPress()
	{
		foreach (Joystick joystick in ReInput.controllers.Joysticks)
		{
			if (!ReInput.controllers.IsJoystickAssigned(joystick))
			{
				if (joystick.GetAnyButtonDown())
				{
					return joystick;
				}
			}
		}
		return null;
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x00080C18 File Offset: 0x0007EE18
	public static Joystick CheckForControllerPress(long systemID)
	{
		foreach (Joystick joystick in ReInput.controllers.Joysticks)
		{
			if (joystick.systemId == systemID && joystick.GetAnyButtonDown())
			{
				return joystick;
			}
		}
		return null;
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x00080CA4 File Offset: 0x0007EEA4
	public static bool AutoAssignController(int rewiredPlayerId)
	{
		foreach (Joystick joystick in ReInput.controllers.Joysticks)
		{
			if (!ReInput.controllers.IsJoystickAssigned(joystick))
			{
				Player player = ReInput.players.GetPlayer(rewiredPlayerId);
				if (player != null)
				{
					if (player.controllers.joystickCount <= 0)
					{
						player.controllers.AddController(joystick, true);
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x0400095D RID: 2397
	public static readonly CupheadInput.Pair[] pairs = new CupheadInput.Pair[]
	{
		new CupheadInput.Pair(CupheadInput.InputSymbols.XBOX_A, "<sprite=0>", "<sprite=1>"),
		new CupheadInput.Pair(CupheadInput.InputSymbols.XBOX_B, "<sprite=2>", "<sprite=3>"),
		new CupheadInput.Pair(CupheadInput.InputSymbols.XBOX_X, "<sprite=4>", "<sprite=5>"),
		new CupheadInput.Pair(CupheadInput.InputSymbols.XBOX_Y, "<sprite=6>", "<sprite=7>")
	};

	// Token: 0x0200096F RID: 2415
	public enum InputDevice
	{
		// Token: 0x040046B1 RID: 18097
		Keyboard,
		// Token: 0x040046B2 RID: 18098
		Controller_1,
		// Token: 0x040046B3 RID: 18099
		Controller_2
	}

	// Token: 0x02000970 RID: 2416
	public enum InputSymbols
	{
		// Token: 0x040046B5 RID: 18101
		XBOX_NONE,
		// Token: 0x040046B6 RID: 18102
		XBOX_A,
		// Token: 0x040046B7 RID: 18103
		XBOX_B,
		// Token: 0x040046B8 RID: 18104
		XBOX_X,
		// Token: 0x040046B9 RID: 18105
		XBOX_Y,
		// Token: 0x040046BA RID: 18106
		XBOX_RB,
		// Token: 0x040046BB RID: 18107
		XBOX_LB
	}

	// Token: 0x02000971 RID: 2417
	public class AnyPlayerInput
	{
		// Token: 0x060054FE RID: 21758 RVA: 0x000404B3 File Offset: 0x0003E6B3
		public AnyPlayerInput(bool checkIfDead = false)
		{
			this.checkIfDead = checkIfDead;
			this.players = new Player[]
			{
				PlayerManager.GetPlayerInput(PlayerId.PlayerOne),
				PlayerManager.GetPlayerInput(PlayerId.PlayerTwo)
			};
		}

		// Token: 0x060054FF RID: 21759 RVA: 0x001C5310 File Offset: 0x001C3510
		public bool GetButton(CupheadButton button)
		{
			foreach (Player player in this.players)
			{
				if (player.GetButton((int)button) && (!this.checkIfDead || !this.IsDead(player)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005500 RID: 21760 RVA: 0x001C5364 File Offset: 0x001C3564
		public bool GetButtonDown(CupheadButton button)
		{
			if (InterruptingPrompt.IsInterrupting())
			{
				return false;
			}
			foreach (Player player in this.players)
			{
				if (player.GetButtonDown((int)button) && (!this.checkIfDead || !this.IsDead(player)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005501 RID: 21761 RVA: 0x001C53C4 File Offset: 0x001C35C4
		public bool GetActionButtonDown()
		{
			if (InterruptingPrompt.IsInterrupting())
			{
				return false;
			}
			foreach (Player player in this.players)
			{
				if ((player.GetButtonDown(13) || player.GetButtonDown(14) || player.GetButtonDown(7) || player.GetButtonDown(15) || player.GetButtonDown(2) || player.GetButtonDown(6) || player.GetButtonDown(8) || player.GetButtonDown(3) || player.GetButtonDown(4) || player.GetButtonDown(5)) && (!this.checkIfDead || !this.IsDead(player)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005502 RID: 21762 RVA: 0x001C5494 File Offset: 0x001C3694
		public bool GetAnyButtonDown()
		{
			if (InterruptingPrompt.IsInterrupting())
			{
				return false;
			}
			foreach (Player player in this.players)
			{
				foreach (Controller controller in player.controllers.Controllers)
				{
					if (controller.GetAnyButtonDown() && (!this.checkIfDead || !this.IsDead(player)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005503 RID: 21763 RVA: 0x001C554C File Offset: 0x001C374C
		public bool GetAnyButtonHeld()
		{
			if (InterruptingPrompt.IsInterrupting())
			{
				return false;
			}
			foreach (Player player in this.players)
			{
				foreach (Controller controller in player.controllers.Controllers)
				{
					if (controller.GetAnyButton() && (!this.checkIfDead || !this.IsDead(player)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005504 RID: 21764 RVA: 0x001C5604 File Offset: 0x001C3804
		public bool GetButtonUp(CupheadButton button)
		{
			foreach (Player player in this.players)
			{
				if (player.GetButtonUp((int)button) && (!this.checkIfDead || !this.IsDead(player)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005505 RID: 21765 RVA: 0x001C5658 File Offset: 0x001C3858
		public bool IsDead(Player player)
		{
			PlayerId id = (player != this.players[0]) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
			AbstractPlayerController player2 = PlayerManager.GetPlayer(id);
			return player2 == null || player2.IsDead;
		}

		// Token: 0x040046BC RID: 18108
		public Player[] players;

		// Token: 0x040046BD RID: 18109
		public bool checkIfDead;
	}

	// Token: 0x02000972 RID: 2418
	public class Pair
	{
		// Token: 0x06005506 RID: 21766 RVA: 0x000404E0 File Offset: 0x0003E6E0
		public Pair(CupheadInput.InputSymbols symbol, string first, string second)
		{
			this.symbol = symbol;
			this.first = first;
			this.second = second;
		}

		// Token: 0x040046BE RID: 18110
		public readonly CupheadInput.InputSymbols symbol;

		// Token: 0x040046BF RID: 18111
		public readonly string first;

		// Token: 0x040046C0 RID: 18112
		public readonly string second;
	}
}
