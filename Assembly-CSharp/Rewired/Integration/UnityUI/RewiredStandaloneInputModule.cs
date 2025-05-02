using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Rewired.Integration.UnityUI
{
	// Token: 0x02000653 RID: 1619
	[AddComponentMenu("Event/Rewired Standalone Input Module")]
	public class RewiredStandaloneInputModule : PointerInputModule
	{
		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x060043B0 RID: 17328 RVA: 0x00035FD5 File Offset: 0x000341D5
		// (set) Token: 0x060043B1 RID: 17329 RVA: 0x0013A000 File Offset: 0x00138200
		public bool UseAllRewiredGamePlayers
		{
			get
			{
				return this.useAllRewiredGamePlayers;
			}
			set
			{
				bool flag = value != this.useAllRewiredGamePlayers;
				this.useAllRewiredGamePlayers = value;
				if (flag)
				{
					this.SetupRewiredVars();
				}
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x060043B2 RID: 17330 RVA: 0x00035FDD File Offset: 0x000341DD
		// (set) Token: 0x060043B3 RID: 17331 RVA: 0x0013A030 File Offset: 0x00138230
		public bool UseRewiredSystemPlayer
		{
			get
			{
				return this.useRewiredSystemPlayer;
			}
			set
			{
				bool flag = value != this.useRewiredSystemPlayer;
				this.useRewiredSystemPlayer = value;
				if (flag)
				{
					this.SetupRewiredVars();
				}
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x060043B4 RID: 17332 RVA: 0x00035FE5 File Offset: 0x000341E5
		// (set) Token: 0x060043B5 RID: 17333 RVA: 0x00035FF7 File Offset: 0x000341F7
		public int[] RewiredPlayerIds
		{
			get
			{
				return (int[])this.rewiredPlayerIds.Clone();
			}
			set
			{
				this.rewiredPlayerIds = ((value == null) ? new int[0] : ((int[])value.Clone()));
				this.SetupRewiredVars();
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x060043B6 RID: 17334 RVA: 0x00036021 File Offset: 0x00034221
		// (set) Token: 0x060043B7 RID: 17335 RVA: 0x00036029 File Offset: 0x00034229
		public bool UsePlayingPlayersOnly
		{
			get
			{
				return this.usePlayingPlayersOnly;
			}
			set
			{
				this.usePlayingPlayersOnly = value;
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x060043B8 RID: 17336 RVA: 0x00036032 File Offset: 0x00034232
		// (set) Token: 0x060043B9 RID: 17337 RVA: 0x0003603A File Offset: 0x0003423A
		public bool MoveOneElementPerAxisPress
		{
			get
			{
				return this.moveOneElementPerAxisPress;
			}
			set
			{
				this.moveOneElementPerAxisPress = value;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x060043BA RID: 17338 RVA: 0x00036043 File Offset: 0x00034243
		// (set) Token: 0x060043BB RID: 17339 RVA: 0x0003604B File Offset: 0x0003424B
		public bool allowMouseInput
		{
			get
			{
				return this.m_allowMouseInput;
			}
			set
			{
				this.m_allowMouseInput = value;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x060043BC RID: 17340 RVA: 0x00036054 File Offset: 0x00034254
		// (set) Token: 0x060043BD RID: 17341 RVA: 0x0003605C File Offset: 0x0003425C
		public bool allowMouseInputIfTouchSupported
		{
			get
			{
				return this.m_allowMouseInputIfTouchSupported;
			}
			set
			{
				this.m_allowMouseInputIfTouchSupported = value;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x060043BE RID: 17342 RVA: 0x00036065 File Offset: 0x00034265
		public bool isMouseSupported
		{
			get
			{
				return Input.mousePresent && this.m_allowMouseInput && (!this.isTouchSupported || this.m_allowMouseInputIfTouchSupported);
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060043BF RID: 17343 RVA: 0x00036097 File Offset: 0x00034297
		// (set) Token: 0x060043C0 RID: 17344 RVA: 0x0003609F File Offset: 0x0003429F
		[Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead")]
		public bool allowActivationOnMobileDevice
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x060043C1 RID: 17345 RVA: 0x000360A8 File Offset: 0x000342A8
		// (set) Token: 0x060043C2 RID: 17346 RVA: 0x000360B0 File Offset: 0x000342B0
		public bool forceModuleActive
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x060043C3 RID: 17347 RVA: 0x000360B9 File Offset: 0x000342B9
		// (set) Token: 0x060043C4 RID: 17348 RVA: 0x000360C1 File Offset: 0x000342C1
		public float inputActionsPerSecond
		{
			get
			{
				return this.m_InputActionsPerSecond;
			}
			set
			{
				this.m_InputActionsPerSecond = value;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x060043C5 RID: 17349 RVA: 0x000360CA File Offset: 0x000342CA
		// (set) Token: 0x060043C6 RID: 17350 RVA: 0x000360D2 File Offset: 0x000342D2
		public float repeatDelay
		{
			get
			{
				return this.m_RepeatDelay;
			}
			set
			{
				this.m_RepeatDelay = value;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x060043C7 RID: 17351 RVA: 0x000360DB File Offset: 0x000342DB
		// (set) Token: 0x060043C8 RID: 17352 RVA: 0x000360E3 File Offset: 0x000342E3
		public string horizontalAxis
		{
			get
			{
				return this.m_HorizontalAxis;
			}
			set
			{
				this.m_HorizontalAxis = value;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060043C9 RID: 17353 RVA: 0x000360EC File Offset: 0x000342EC
		// (set) Token: 0x060043CA RID: 17354 RVA: 0x000360F4 File Offset: 0x000342F4
		public string verticalAxis
		{
			get
			{
				return this.m_VerticalAxis;
			}
			set
			{
				this.m_VerticalAxis = value;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x060043CB RID: 17355 RVA: 0x000360FD File Offset: 0x000342FD
		// (set) Token: 0x060043CC RID: 17356 RVA: 0x00036105 File Offset: 0x00034305
		public string submitButton
		{
			get
			{
				return this.m_SubmitButton;
			}
			set
			{
				this.m_SubmitButton = value;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x060043CD RID: 17357 RVA: 0x0003610E File Offset: 0x0003430E
		// (set) Token: 0x060043CE RID: 17358 RVA: 0x00036116 File Offset: 0x00034316
		public string cancelButton
		{
			get
			{
				return this.m_CancelButton;
			}
			set
			{
				this.m_CancelButton = value;
			}
		}

		// Token: 0x060043CF RID: 17359 RVA: 0x0013A060 File Offset: 0x00138260
		public override void Awake()
		{
			base.Awake();
			this.isTouchSupported = Input.touchSupported;
			TouchInputModule component = base.GetComponent<TouchInputModule>();
			if (component != null)
			{
				component.enabled = false;
			}
			this.InitializeRewired();
		}

		// Token: 0x060043D0 RID: 17360 RVA: 0x0013A0A0 File Offset: 0x001382A0
		public override void UpdateModule()
		{
			this.CheckEditorRecompile();
			if (this.recompiling)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.m_HasFocus && this.ShouldIgnoreEventsOnNoFocus())
			{
				return;
			}
			if (this.isMouseSupported)
			{
				this.m_LastMousePosition = this.m_MousePosition;
				this.m_MousePosition = Input.mousePosition;
			}
		}

		// Token: 0x060043D1 RID: 17361 RVA: 0x0003611F File Offset: 0x0003431F
		public override bool IsModuleSupported()
		{
			return true;
		}

		// Token: 0x060043D2 RID: 17362 RVA: 0x0013A108 File Offset: 0x00138308
		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}
			if (this.recompiling)
			{
				return false;
			}
			if (!ReInput.isReady)
			{
				return false;
			}
			bool flag = this.m_ForceModuleActive;
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null)
				{
					if (!this.usePlayingPlayersOnly || player.isPlaying)
					{
						flag |= player.GetButtonDown(this.m_SubmitButton);
						flag |= player.GetButtonDown(this.m_CancelButton);
						if (this.moveOneElementPerAxisPress)
						{
							flag |= (player.GetButtonDown(this.m_HorizontalAxis) || player.GetNegativeButtonDown(this.m_HorizontalAxis));
							flag |= (player.GetButtonDown(this.m_VerticalAxis) || player.GetNegativeButtonDown(this.m_VerticalAxis));
						}
						else
						{
							flag |= !Mathf.Approximately(player.GetAxisRaw(this.m_HorizontalAxis), 0f);
							flag |= !Mathf.Approximately(player.GetAxisRaw(this.m_VerticalAxis), 0f);
						}
					}
				}
			}
			if (this.isMouseSupported)
			{
				flag |= ((this.m_MousePosition - this.m_LastMousePosition).sqrMagnitude > 0f);
				flag |= Input.GetMouseButtonDown(0);
			}
			if (this.isTouchSupported)
			{
				for (int j = 0; j < Input.touchCount; j++)
				{
					Touch touch = Input.GetTouch(j);
					flag |= (touch.phase == null || touch.phase == 1 || touch.phase == 2);
				}
			}
			return flag;
		}

		// Token: 0x060043D3 RID: 17363 RVA: 0x0013A2D0 File Offset: 0x001384D0
		public override void ActivateModule()
		{
			if (!this.m_HasFocus && this.ShouldIgnoreEventsOnNoFocus())
			{
				return;
			}
			base.ActivateModule();
			if (this.isMouseSupported)
			{
				Vector2 vector = Input.mousePosition;
				this.m_MousePosition = vector;
				this.m_LastMousePosition = vector;
			}
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x00036122 File Offset: 0x00034322
		public override void DeactivateModule()
		{
			base.DeactivateModule();
			base.ClearSelection();
		}

		// Token: 0x060043D5 RID: 17365 RVA: 0x0013A354 File Offset: 0x00138554
		public override void Process()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.m_HasFocus && this.ShouldIgnoreEventsOnNoFocus())
			{
				return;
			}
			bool flag = this.SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag |= this.SendMoveEventToSelectedObject();
				}
				if (!flag)
				{
					this.SendSubmitEventToSelectedObject();
				}
			}
			if (!this.ProcessTouchEvents() && this.isMouseSupported)
			{
				this.ProcessMouseEvent();
			}
		}

		// Token: 0x060043D6 RID: 17366 RVA: 0x0013A3D4 File Offset: 0x001385D4
		public bool ProcessTouchEvents()
		{
			if (!this.isTouchSupported)
			{
				return false;
			}
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if (touch.type != 1)
				{
					bool pressed;
					bool flag;
					PointerEventData touchPointerEventData = base.GetTouchPointerEventData(touch, ref pressed, ref flag);
					this.ProcessTouchPress(touchPointerEventData, pressed, flag);
					if (!flag)
					{
						this.ProcessMove(touchPointerEventData);
						this.ProcessDrag(touchPointerEventData);
					}
					else
					{
						base.RemovePointerData(touchPointerEventData);
					}
				}
			}
			return Input.touchCount > 0;
		}

		// Token: 0x060043D7 RID: 17367 RVA: 0x0013A460 File Offset: 0x00138660
		public void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
		{
			GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
			if (pressed)
			{
				pointerEvent.eligibleForClick = true;
				pointerEvent.delta = Vector2.zero;
				pointerEvent.dragging = false;
				pointerEvent.useDragThreshold = true;
				pointerEvent.pressPosition = pointerEvent.position;
				pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
				base.DeselectIfSelectionChanged(gameObject, pointerEvent);
				if (pointerEvent.pointerEnter != gameObject)
				{
					base.HandlePointerExitAndEnter(pointerEvent, gameObject);
					pointerEvent.pointerEnter = gameObject;
				}
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, pointerEvent, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == pointerEvent.lastPress)
				{
					float num = unscaledTime - pointerEvent.clickTime;
					if (num < 0.3f)
					{
						pointerEvent.clickCount++;
					}
					else
					{
						pointerEvent.clickCount = 1;
					}
					pointerEvent.clickTime = unscaledTime;
				}
				else
				{
					pointerEvent.clickCount = 1;
				}
				pointerEvent.pointerPress = gameObject2;
				pointerEvent.rawPointerPress = gameObject;
				pointerEvent.clickTime = unscaledTime;
				pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (released)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (pointerEvent.pointerPress == eventHandler && pointerEvent.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
				}
				else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, pointerEvent, ExecuteEvents.dropHandler);
				}
				pointerEvent.eligibleForClick = false;
				pointerEvent.pointerPress = null;
				pointerEvent.rawPointerPress = null;
				if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.dragging = false;
				pointerEvent.pointerDrag = null;
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.pointerDrag = null;
				ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler);
				pointerEvent.pointerEnter = null;
			}
		}

		// Token: 0x060043D8 RID: 17368 RVA: 0x0013A6B4 File Offset: 0x001388B4
		public bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			if (this.recompiling)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null)
				{
					if (!this.usePlayingPlayersOnly || player.isPlaying)
					{
						if (player.GetButtonDown(this.m_SubmitButton))
						{
							ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
							break;
						}
						if (player.GetButtonDown(this.m_CancelButton))
						{
							ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
							break;
						}
					}
				}
			}
			return baseEventData.used;
		}

		// Token: 0x060043D9 RID: 17369 RVA: 0x0013A7A0 File Offset: 0x001389A0
		public Vector2 GetRawMoveVector()
		{
			if (this.recompiling)
			{
				return Vector2.zero;
			}
			Vector2 zero = Vector2.zero;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null)
				{
					if (!this.usePlayingPlayersOnly || player.isPlaying)
					{
						if (this.moveOneElementPerAxisPress)
						{
							float num = 0f;
							if (player.GetButtonDown(this.m_HorizontalAxis))
							{
								num = 1f;
							}
							else if (player.GetNegativeButtonDown(this.m_HorizontalAxis))
							{
								num = -1f;
							}
							float num2 = 0f;
							if (player.GetButtonDown(this.m_VerticalAxis))
							{
								num2 = 1f;
							}
							else if (player.GetNegativeButtonDown(this.m_VerticalAxis))
							{
								num2 = -1f;
							}
							zero.x += num;
							zero.y += num2;
						}
						else
						{
							zero.x += player.GetAxisRaw(this.m_HorizontalAxis);
							zero.y += player.GetAxisRaw(this.m_VerticalAxis);
						}
						flag |= (player.GetButtonDown(this.m_HorizontalAxis) || player.GetNegativeButtonDown(this.m_HorizontalAxis));
						flag2 |= (player.GetButtonDown(this.m_VerticalAxis) || player.GetNegativeButtonDown(this.m_VerticalAxis));
					}
				}
			}
			if (flag)
			{
				if (zero.x < 0f)
				{
					zero.x = -1f;
				}
				if (zero.x > 0f)
				{
					zero.x = 1f;
				}
			}
			if (flag2)
			{
				if (zero.y < 0f)
				{
					zero.y = -1f;
				}
				if (zero.y > 0f)
				{
					zero.y = 1f;
				}
			}
			return zero;
		}

		// Token: 0x060043DA RID: 17370 RVA: 0x0013A9CC File Offset: 0x00138BCC
		public bool SendMoveEventToSelectedObject()
		{
			if (this.recompiling)
			{
				return false;
			}
			float unscaledTime = Time.unscaledTime;
			Vector2 rawMoveVector = this.GetRawMoveVector();
			if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
			{
				this.m_ConsecutiveMoveCount = 0;
				return false;
			}
			bool flag = Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0f;
			bool flag2 = this.CheckButtonOrKeyMovement(unscaledTime);
			bool flag3 = flag2;
			if (!flag3)
			{
				if (this.m_RepeatDelay > 0f)
				{
					if (flag && this.m_ConsecutiveMoveCount == 1)
					{
						flag3 = (unscaledTime > this.m_PrevActionTime + this.m_RepeatDelay);
					}
					else
					{
						flag3 = (unscaledTime > this.m_PrevActionTime + 1f / this.m_InputActionsPerSecond);
					}
				}
				else
				{
					flag3 = (unscaledTime > this.m_PrevActionTime + 1f / this.m_InputActionsPerSecond);
				}
			}
			if (!flag3)
			{
				return false;
			}
			AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
			if (axisEventData.moveDir == 4)
			{
				return false;
			}
			ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
			if (!flag)
			{
				this.m_ConsecutiveMoveCount = 0;
			}
			this.m_ConsecutiveMoveCount++;
			this.m_PrevActionTime = unscaledTime;
			this.m_LastMoveVector = rawMoveVector;
			return axisEventData.used;
		}

		// Token: 0x060043DB RID: 17371 RVA: 0x0013AB38 File Offset: 0x00138D38
		public bool CheckButtonOrKeyMovement(float time)
		{
			bool flag = false;
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null)
				{
					if (!this.usePlayingPlayersOnly || player.isPlaying)
					{
						flag |= (player.GetButtonDown(this.m_HorizontalAxis) || player.GetNegativeButtonDown(this.m_HorizontalAxis));
						flag |= (player.GetButtonDown(this.m_VerticalAxis) || player.GetNegativeButtonDown(this.m_VerticalAxis));
					}
				}
			}
			return flag;
		}

		// Token: 0x060043DC RID: 17372 RVA: 0x00036130 File Offset: 0x00034330
		public void ProcessMouseEvent()
		{
			this.ProcessMouseEvent(0);
		}

		// Token: 0x060043DD RID: 17373 RVA: 0x0013ABE0 File Offset: 0x00138DE0
		public void ProcessMouseEvent(int id)
		{
			PointerInputModule.MouseState mousePointerEventData = this.GetMousePointerEventData();
			PointerInputModule.MouseButtonEventData eventData = mousePointerEventData.GetButtonState(0).eventData;
			this.ProcessMousePress(eventData);
			this.ProcessMove(eventData.buttonData);
			this.ProcessDrag(eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(1).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(1).eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(2).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(2).eventData.buttonData);
			if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject);
				ExecuteEvents.ExecuteHierarchy<IScrollHandler>(eventHandler, eventData.buttonData, ExecuteEvents.scrollHandler);
			}
		}

		// Token: 0x060043DE RID: 17374 RVA: 0x0013ACC0 File Offset: 0x00138EC0
		public bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		// Token: 0x060043DF RID: 17375 RVA: 0x0013AD0C File Offset: 0x00138F0C
		public void ProcessMousePress(PointerInputModule.MouseButtonEventData data)
		{
			PointerEventData buttonData = data.buttonData;
			GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
			if (data.PressedThisFrame())
			{
				buttonData.eligibleForClick = true;
				buttonData.delta = Vector2.zero;
				buttonData.dragging = false;
				buttonData.useDragThreshold = true;
				buttonData.pressPosition = buttonData.position;
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
				base.DeselectIfSelectionChanged(gameObject, buttonData);
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					float num = unscaledTime - buttonData.clickTime;
					if (num < 0.3f)
					{
						buttonData.clickCount++;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.clickTime = unscaledTime;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.pointerPress = gameObject2;
				buttonData.rawPointerPress = gameObject;
				buttonData.clickTime = unscaledTime;
				buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (buttonData.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (data.ReleasedThisFrame())
			{
				ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
				}
				else if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
				}
				buttonData.eligibleForClick = false;
				buttonData.pointerPress = null;
				buttonData.rawPointerPress = null;
				if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
				}
				buttonData.dragging = false;
				buttonData.pointerDrag = null;
				if (gameObject != buttonData.pointerEnter)
				{
					base.HandlePointerExitAndEnter(buttonData, null);
					base.HandlePointerExitAndEnter(buttonData, gameObject);
				}
			}
		}

		// Token: 0x060043E0 RID: 17376 RVA: 0x00036139 File Offset: 0x00034339
		public virtual void OnApplicationFocus(bool hasFocus)
		{
			this.m_HasFocus = hasFocus;
		}

		// Token: 0x060043E1 RID: 17377 RVA: 0x00036142 File Offset: 0x00034342
		public bool ShouldIgnoreEventsOnNoFocus()
		{
			return !ReInput.isReady || ReInput.configuration.ignoreInputWhenAppNotInFocus;
		}

		// Token: 0x060043E2 RID: 17378 RVA: 0x0003615A File Offset: 0x0003435A
		public void InitializeRewired()
		{
			if (!ReInput.isReady)
			{
				Debug.LogError("Rewired is not initialized! Are you missing a Rewired Input Manager in your scene?", null);
				return;
			}
			ReInput.EditorRecompileEvent += this.OnEditorRecompile;
			this.SetupRewiredVars();
		}

		// Token: 0x060043E3 RID: 17379 RVA: 0x0013AF30 File Offset: 0x00139130
		public void SetupRewiredVars()
		{
			if (this.useAllRewiredGamePlayers)
			{
				IList<Player> list = (!this.useRewiredSystemPlayer) ? ReInput.players.Players : ReInput.players.AllPlayers;
				this.playerIds = new int[list.Count];
				for (int i = 0; i < list.Count; i++)
				{
					this.playerIds[i] = list[i].id;
				}
			}
			else
			{
				int num = this.rewiredPlayerIds.Length + ((!this.useRewiredSystemPlayer) ? 0 : 1);
				this.playerIds = new int[num];
				for (int j = 0; j < this.rewiredPlayerIds.Length; j++)
				{
					this.playerIds[j] = ReInput.players.GetPlayer(this.rewiredPlayerIds[j]).id;
				}
				if (this.useRewiredSystemPlayer)
				{
					this.playerIds[num - 1] = ReInput.players.GetSystemPlayer().id;
				}
			}
		}

		// Token: 0x060043E4 RID: 17380 RVA: 0x00036189 File Offset: 0x00034389
		public void CheckEditorRecompile()
		{
			if (!this.recompiling)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			this.recompiling = false;
			this.InitializeRewired();
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x000361AF File Offset: 0x000343AF
		public void OnEditorRecompile()
		{
			this.recompiling = true;
			this.ClearRewiredVars();
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x000361BE File Offset: 0x000343BE
		public void ClearRewiredVars()
		{
			Array.Clear(this.playerIds, 0, this.playerIds.Length);
		}

		// Token: 0x040034FA RID: 13562
		public const string DEFAULT_ACTION_MOVE_HORIZONTAL = "UIHorizontal";

		// Token: 0x040034FB RID: 13563
		public const string DEFAULT_ACTION_MOVE_VERTICAL = "UIVertical";

		// Token: 0x040034FC RID: 13564
		public const string DEFAULT_ACTION_SUBMIT = "UISubmit";

		// Token: 0x040034FD RID: 13565
		public const string DEFAULT_ACTION_CANCEL = "UICancel";

		// Token: 0x040034FE RID: 13566
		public int[] playerIds;

		// Token: 0x040034FF RID: 13567
		public bool recompiling;

		// Token: 0x04003500 RID: 13568
		public bool isTouchSupported;

		// Token: 0x04003501 RID: 13569
		[SerializeField]
		[Tooltip("Use all Rewired game Players to control the UI. This does not include the System Player. If enabled, this setting overrides individual Player Ids set in Rewired Player Ids.")]
		public bool useAllRewiredGamePlayers;

		// Token: 0x04003502 RID: 13570
		[SerializeField]
		[Tooltip("Allow the Rewired System Player to control the UI.")]
		public bool useRewiredSystemPlayer;

		// Token: 0x04003503 RID: 13571
		[SerializeField]
		[Tooltip("A list of Player Ids that are allowed to control the UI. If Use All Rewired Game Players = True, this list will be ignored.")]
		public int[] rewiredPlayerIds = new int[1];

		// Token: 0x04003504 RID: 13572
		[SerializeField]
		[Tooltip("Allow only Players with Player.isPlaying = true to control the UI.")]
		public bool usePlayingPlayersOnly;

		// Token: 0x04003505 RID: 13573
		[SerializeField]
		[Tooltip("Makes an axis press always move only one UI selection. Enable if you do not want to allow scrolling through UI elements by holding an axis direction.")]
		public bool moveOneElementPerAxisPress;

		// Token: 0x04003506 RID: 13574
		public float m_PrevActionTime;

		// Token: 0x04003507 RID: 13575
		public Vector2 m_LastMoveVector;

		// Token: 0x04003508 RID: 13576
		public int m_ConsecutiveMoveCount;

		// Token: 0x04003509 RID: 13577
		public Vector2 m_LastMousePosition;

		// Token: 0x0400350A RID: 13578
		public Vector2 m_MousePosition;

		// Token: 0x0400350B RID: 13579
		public bool m_HasFocus = true;

		// Token: 0x0400350C RID: 13580
		[SerializeField]
		public string m_HorizontalAxis = "UIHorizontal";

		// Token: 0x0400350D RID: 13581
		[SerializeField]
		[Tooltip("Name of the vertical axis for movement (if axis events are used).")]
		public string m_VerticalAxis = "UIVertical";

		// Token: 0x0400350E RID: 13582
		[SerializeField]
		[Tooltip("Name of the action used to submit.")]
		public string m_SubmitButton = "UISubmit";

		// Token: 0x0400350F RID: 13583
		[SerializeField]
		[Tooltip("Name of the action used to cancel.")]
		public string m_CancelButton = "UICancel";

		// Token: 0x04003510 RID: 13584
		[SerializeField]
		[Tooltip("Number of selection changes allowed per second when a movement button/axis is held in a direction.")]
		public float m_InputActionsPerSecond = 10f;

		// Token: 0x04003511 RID: 13585
		[SerializeField]
		[Tooltip("Delay in seconds before vertical/horizontal movement starts repeating continouously when a movement direction is held.")]
		public float m_RepeatDelay;

		// Token: 0x04003512 RID: 13586
		[SerializeField]
		[Tooltip("Allows the mouse to be used to select elements.")]
		public bool m_allowMouseInput = true;

		// Token: 0x04003513 RID: 13587
		[SerializeField]
		[Tooltip("Allows the mouse to be used to select elements if the device also supports touch control.")]
		public bool m_allowMouseInputIfTouchSupported = true;

		// Token: 0x04003514 RID: 13588
		[SerializeField]
		[FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
		[Tooltip("Forces the module to always be active.")]
		public bool m_ForceModuleActive;
	}
}
