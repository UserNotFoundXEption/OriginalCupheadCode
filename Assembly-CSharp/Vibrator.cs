using System;
using Rewired;
using UnityEngine;

// Token: 0x020000FA RID: 250
public class Vibrator : AbstractMonoBehaviour
{
	// Token: 0x170001DA RID: 474
	// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x0000A5BF File Offset: 0x000087BF
	// (set) Token: 0x06000BA8 RID: 2984 RVA: 0x0000A5C6 File Offset: 0x000087C6
	public static Vibrator Current { get; set; }

	// Token: 0x06000BA9 RID: 2985 RVA: 0x0000A5CE File Offset: 0x000087CE
	public static void Vibrate(float amount, float time, PlayerId player)
	{
		Vibrator.Current._Vibrate(amount, time, player);
	}

	// Token: 0x06000BAA RID: 2986 RVA: 0x0000A5DD File Offset: 0x000087DD
	public static void StopVibrating(PlayerId player)
	{
		Vibrator.Current._StopVibrating(player);
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x0000A5EA File Offset: 0x000087EA
	public override void Awake()
	{
		base.Awake();
		Vibrator.Current = this;
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x00080DB4 File Offset: 0x0007EFB4
	public void Update()
	{
		if (!SettingsData.Data.canVibrate)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			float num = this.durationsLeft[i];
			float num2 = this.currentVibrations[i];
			num -= CupheadTime.Delta;
			if (num <= 0f)
			{
				if (num2 > 0f)
				{
					this.currentVibrations[i] = 0f;
					this._StopVibrating((PlayerId)i);
				}
			}
			else if (num2 <= 0f)
			{
				this._StopVibrating((PlayerId)i);
			}
			else
			{
				this.durationsLeft[i] = num;
			}
		}
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x00080E58 File Offset: 0x0007F058
	public void _Vibrate(float amount, float time, PlayerId playerId)
	{
		if (!SettingsData.Data.canVibrate)
		{
			return;
		}
		if (amount <= 0f || time <= 0f)
		{
			this._StopVibrating(playerId);
			return;
		}
		this.currentVibrations[(int)playerId] = amount;
		this.durationsLeft[(int)playerId] = time;
		Player player = ReInput.players.GetPlayer((int)playerId);
		foreach (Joystick joystick in player.controllers.Joysticks)
		{
			if (joystick.supportsVibration)
			{
				joystick.SetVibration(amount * Vibrator.PlatformMultiplier, amount * Vibrator.PlatformMultiplier);
			}
		}
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x00080F20 File Offset: 0x0007F120
	public void _StopVibrating(PlayerId playerId)
	{
		if (!SettingsData.Data.canVibrate)
		{
			return;
		}
		Player player = ReInput.players.GetPlayer((int)playerId);
		foreach (Joystick joystick in player.controllers.Joysticks)
		{
			joystick.StopVibration();
		}
	}

	// Token: 0x0400095E RID: 2398
	public static float PlatformMultiplier = 1f;

	// Token: 0x04000960 RID: 2400
	public Coroutine vibrateCoroutine;

	// Token: 0x04000961 RID: 2401
	public float[] durationsLeft = new float[2];

	// Token: 0x04000962 RID: 2402
	public float[] currentVibrations = new float[2];
}
