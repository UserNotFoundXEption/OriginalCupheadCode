using System;
using System.Collections.Generic;
using System.ComponentModel;
using Rewired.Utils.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Utils
{
	// Token: 0x02000657 RID: 1623
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class ExternalTools : IExternalTools
	{
		// Token: 0x0600446E RID: 17518 RVA: 0x0003664D File Offset: 0x0003484D
		public object GetPlatformInitializer()
		{
			return null;
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x00036650 File Offset: 0x00034850
		public string GetFocusedEditorWindowTitle()
		{
			return string.Empty;
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x00036657 File Offset: 0x00034857
		public bool LinuxInput_IsJoystickPreconfigured(string name)
		{
			return false;
		}

		// Token: 0x140000F9 RID: 249
		// (add) Token: 0x06004471 RID: 17521 RVA: 0x0013D368 File Offset: 0x0013B568
		// (remove) Token: 0x06004472 RID: 17522 RVA: 0x0013D3A0 File Offset: 0x0013B5A0
		public event Action<uint, bool> XboxOneInput_OnGamepadStateChange;

		// Token: 0x06004473 RID: 17523 RVA: 0x0003665A File Offset: 0x0003485A
		public int XboxOneInput_GetUserIdForGamepad(uint id)
		{
			return 0;
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x0003665D File Offset: 0x0003485D
		public ulong XboxOneInput_GetControllerId(uint unityJoystickId)
		{
			return 0UL;
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x00036661 File Offset: 0x00034861
		public bool XboxOneInput_IsGamepadActive(uint unityJoystickId)
		{
			return false;
		}

		// Token: 0x06004476 RID: 17526 RVA: 0x00036664 File Offset: 0x00034864
		public string XboxOneInput_GetControllerType(ulong xboxControllerId)
		{
			return string.Empty;
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x0003666B File Offset: 0x0003486B
		public uint XboxOneInput_GetJoystickId(ulong xboxControllerId)
		{
			return 0u;
		}

		// Token: 0x06004478 RID: 17528 RVA: 0x0003666E File Offset: 0x0003486E
		public void XboxOne_Gamepad_UpdatePlugin()
		{
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x00036670 File Offset: 0x00034870
		public bool XboxOne_Gamepad_SetGamepadVibration(ulong xboxOneJoystickId, float leftMotor, float rightMotor, float leftTriggerLevel, float rightTriggerLevel)
		{
			return false;
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x00036673 File Offset: 0x00034873
		public void XboxOne_Gamepad_PulseVibrateMotor(ulong xboxOneJoystickId, int motorInt, float startLevel, float endLevel, ulong durationMS)
		{
		}

		// Token: 0x0600447B RID: 17531 RVA: 0x00036675 File Offset: 0x00034875
		public Vector3 PS4Input_GetLastAcceleration(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x0600447C RID: 17532 RVA: 0x0003667C File Offset: 0x0003487C
		public Vector3 PS4Input_GetLastGyro(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x0600447D RID: 17533 RVA: 0x00036683 File Offset: 0x00034883
		public Vector4 PS4Input_GetLastOrientation(int id)
		{
			return Vector4.zero;
		}

		// Token: 0x0600447E RID: 17534 RVA: 0x0003668A File Offset: 0x0003488A
		public void PS4Input_GetLastTouchData(int id, out int touchNum, out int touch0x, out int touch0y, out int touch0id, out int touch1x, out int touch1y, out int touch1id)
		{
			touchNum = 0;
			touch0x = 0;
			touch0y = 0;
			touch0id = 0;
			touch1x = 0;
			touch1y = 0;
			touch1id = 0;
		}

		// Token: 0x0600447F RID: 17535 RVA: 0x000366A6 File Offset: 0x000348A6
		public void PS4Input_GetPadControllerInformation(int id, out float touchpixelDensity, out int touchResolutionX, out int touchResolutionY, out int analogDeadZoneLeft, out int analogDeadZoneright, out int connectionType)
		{
			touchpixelDensity = 0f;
			touchResolutionX = 0;
			touchResolutionY = 0;
			analogDeadZoneLeft = 0;
			analogDeadZoneright = 0;
			connectionType = 0;
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x000366C2 File Offset: 0x000348C2
		public void PS4Input_PadSetMotionSensorState(int id, bool bEnable)
		{
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x000366C4 File Offset: 0x000348C4
		public void PS4Input_PadSetTiltCorrectionState(int id, bool bEnable)
		{
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x000366C6 File Offset: 0x000348C6
		public void PS4Input_PadSetAngularVelocityDeadbandState(int id, bool bEnable)
		{
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x000366C8 File Offset: 0x000348C8
		public void PS4Input_PadSetLightBar(int id, int red, int green, int blue)
		{
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x000366CA File Offset: 0x000348CA
		public void PS4Input_PadResetLightBar(int id)
		{
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x000366CC File Offset: 0x000348CC
		public void PS4Input_PadSetVibration(int id, int largeMotor, int smallMotor)
		{
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x000366CE File Offset: 0x000348CE
		public void PS4Input_PadResetOrientation(int id)
		{
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x000366D0 File Offset: 0x000348D0
		public bool PS4Input_PadIsConnected(int id)
		{
			return false;
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x000366D3 File Offset: 0x000348D3
		public object PS4Input_PadGetUsersDetails(int slot)
		{
			return null;
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x000366D6 File Offset: 0x000348D6
		public Vector3 PS4Input_GetLastMoveAcceleration(int id, int index)
		{
			return Vector3.zero;
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x000366DD File Offset: 0x000348DD
		public Vector3 PS4Input_GetLastMoveGyro(int id, int index)
		{
			return Vector3.zero;
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x000366E4 File Offset: 0x000348E4
		public int PS4Input_MoveGetButtons(int id, int index)
		{
			return 0;
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x000366E7 File Offset: 0x000348E7
		public int PS4Input_MoveGetAnalogButton(int id, int index)
		{
			return 0;
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x000366EA File Offset: 0x000348EA
		public bool PS4Input_MoveIsConnected(int id, int index)
		{
			return false;
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x000366ED File Offset: 0x000348ED
		public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers, int[] primaryHandles, int[] secondaryHandles)
		{
			return 0;
		}

		// Token: 0x0600448F RID: 17551 RVA: 0x000366F0 File Offset: 0x000348F0
		public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers, int[] primaryHandles)
		{
			return 0;
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x000366F3 File Offset: 0x000348F3
		public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers)
		{
			return 0;
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x000366F6 File Offset: 0x000348F6
		public IntPtr PS4Input_MoveGetControllerInputForTracking()
		{
			return IntPtr.Zero;
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x000366FD File Offset: 0x000348FD
		public void GetDeviceVIDPIDs(out List<int> vids, out List<int> pids)
		{
			vids = new List<int>();
			pids = new List<int>();
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x0003670D File Offset: 0x0003490D
		public bool UnityUI_Graphic_GetRaycastTarget(object graphic)
		{
			return !(graphic as Graphic == null) && (graphic as Graphic).raycastTarget;
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x0003672D File Offset: 0x0003492D
		public void UnityUI_Graphic_SetRaycastTarget(object graphic, bool value)
		{
			if (graphic as Graphic == null)
			{
				return;
			}
			(graphic as Graphic).raycastTarget = value;
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06004495 RID: 17557 RVA: 0x0003674D File Offset: 0x0003494D
		public bool UnityInput_IsTouchPressureSupported
		{
			get
			{
				return Input.touchPressureSupported;
			}
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x00036754 File Offset: 0x00034954
		public float UnityInput_GetTouchPressure(ref Touch touch)
		{
			return touch.pressure;
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x0003675C File Offset: 0x0003495C
		public float UnityInput_GetTouchMaximumPossiblePressure(ref Touch touch)
		{
			return touch.maximumPossiblePressure;
		}
	}
}
