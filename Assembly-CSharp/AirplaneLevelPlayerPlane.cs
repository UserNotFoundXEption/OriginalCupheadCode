using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200012E RID: 302
public class AirplaneLevelPlayerPlane : LevelProperties.Airplane.Entity
{
	// Token: 0x06000E3F RID: 3647 RVA: 0x00089950 File Offset: 0x00087B50
	public override void LevelInit(LevelProperties.Airplane properties)
	{
		base.LevelInit(properties);
		this.tiltableBasePos = this.tiltable.localPosition;
		this.maxParallaxX = CupheadLevelCamera.Current.Bounds.xMax - properties.CurrentState.plane.endScreenOffset;
		this.rotationDist = Vector3.Distance(this.edgeLeft.position, this.edgeRight.position);
		this.rotationVal = this.rotationDist / 2f;
		base.StartCoroutine(this.handle_player_move_cr());
		base.StartCoroutine(this.handle_tilt_cr());
		this.puffTimer[0] = 1f;
		this.puffTimer[1] = 0.8f;
		this.SFX_DOGFIGHT_PlayerPlane_Loop();
		this.SFX_DOGFIGHT_PlayerPlane_HighSpeed_Loop();
	}

	// Token: 0x06000E40 RID: 3648 RVA: 0x00089A14 File Offset: 0x00087C14
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.player1 != null)
		{
			LevelPlayerWeaponManager component = this.player1.gameObject.GetComponent<LevelPlayerWeaponManager>();
			component.OnSuperStart -= this.StartP1Super;
			component.OnSuperEnd -= this.EndP1Super;
			component.OnExStart -= this.StartP1Super;
			component.OnExEnd -= this.EndP1Super;
		}
		if (this.player2 != null)
		{
			LevelPlayerWeaponManager component2 = this.player2.gameObject.GetComponent<LevelPlayerWeaponManager>();
			component2.OnSuperStart -= this.StartP2Super;
			component2.OnSuperEnd -= this.EndP2Super;
			component2.OnExStart -= this.StartP2Super;
			component2.OnExEnd -= this.EndP2Super;
		}
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06000E41 RID: 3649 RVA: 0x00089B04 File Offset: 0x00087D04
	public void Update()
	{
		if (((AirplaneLevel)Level.Current).Rotating)
		{
			if (this.playerInSuper[0])
			{
				this.restorePlayerPos[0] = true;
			}
			if (this.playerInSuper[1])
			{
				this.restorePlayerPos[1] = true;
			}
		}
		if (this.player1 == null)
		{
			this.player1 = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			if (this.player1 != null)
			{
				LevelPlayerWeaponManager component = this.player1.gameObject.GetComponent<LevelPlayerWeaponManager>();
				component.OnSuperStart += this.StartP1Super;
				component.OnSuperEnd += this.EndP1Super;
				component.OnExStart += this.StartP1Super;
				component.OnExEnd += this.EndP1Super;
			}
		}
		if (this.player2 == null)
		{
			this.player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			if (this.player2 != null)
			{
				LevelPlayerWeaponManager component2 = this.player2.gameObject.GetComponent<LevelPlayerWeaponManager>();
				component2.OnSuperStart += this.StartP2Super;
				component2.OnSuperEnd += this.EndP2Super;
				component2.OnExStart += this.StartP2Super;
				component2.OnExEnd += this.EndP2Super;
			}
		}
		if (this.player1 != null)
		{
			this.p1IsColliding = (this.player1.transform.parent == this.airplane1.transform);
			this.player1.transform.SetEulerAngles(null, null, new float?(0f));
		}
		else
		{
			this.p1IsColliding = false;
		}
		if (this.player2 != null)
		{
			this.p2IsColliding = (this.player2.transform.parent == this.airplane1.transform);
			this.player2.transform.SetEulerAngles(null, null, new float?(0f));
		}
		else
		{
			this.p2IsColliding = false;
		}
		this.autoTiltTime = Mathf.Clamp(this.autoTiltTime + CupheadTime.Delta * ((!this.autoX || !this.autoTilt) ? -1f : 3f), 0f, 1f);
		for (int i = 0; i < this.puffTimer.Length; i++)
		{
			this.puffTimer[i] -= CupheadTime.Delta;
			if (this.puffTimer[i] <= 0f)
			{
				this.puffTimer[i] += ((i != 0) ? 0.8f : 1f);
				Effect effect = this.planePuffFX.Create(this.planePuffPos[i].position);
				effect.transform.SetEulerAngles(null, null, new float?((float)((i != 0) ? 30 : -30)));
			}
		}
	}

	// Token: 0x06000E42 RID: 3650 RVA: 0x00089E40 File Offset: 0x00088040
	public void StartP1Super()
	{
		this.playerInSuper[0] = true;
		this.playerRelativePosAtSuperStart[0] = this.player1.transform.position.y - base.transform.position.y;
	}

	// Token: 0x06000E43 RID: 3651 RVA: 0x00089E8C File Offset: 0x0008808C
	public void EndP1Super()
	{
		this.playerInSuper[0] = false;
		if (this.restorePlayerPos[0])
		{
			this.player1.transform.position = new Vector3(this.player1.transform.position.x, base.transform.position.y + this.playerRelativePosAtSuperStart[0]);
		}
		this.restorePlayerPos[0] = false;
	}

	// Token: 0x06000E44 RID: 3652 RVA: 0x00089F04 File Offset: 0x00088104
	public void StartP2Super()
	{
		this.playerInSuper[1] = true;
		this.playerRelativePosAtSuperStart[1] = this.player2.transform.position.y - base.transform.position.y;
	}

	// Token: 0x06000E45 RID: 3653 RVA: 0x00089F50 File Offset: 0x00088150
	public void EndP2Super()
	{
		this.playerInSuper[1] = false;
		if (this.restorePlayerPos[1])
		{
			this.player2.transform.position = new Vector3(this.player1.transform.position.x, base.transform.position.y + this.playerRelativePosAtSuperStart[1]);
		}
		this.restorePlayerPos[1] = false;
	}

	// Token: 0x06000E46 RID: 3654 RVA: 0x00089FC8 File Offset: 0x000881C8
	public void AutoMoveToPos(Vector3 pos, bool controlTilt = true, bool holdForYToReleaseX = true)
	{
		if (this.autoMoveCoroutine != null)
		{
			base.StopCoroutine(this.autoMoveCoroutine);
		}
		this.autoTilt = controlTilt;
		this.autoDest = pos;
		this.autoX = true;
		this.autoY = true;
		this.autoMoveCoroutine = base.StartCoroutine(this.auto_move_to_pos_cr(holdForYToReleaseX));
	}

	// Token: 0x06000E47 RID: 3655 RVA: 0x0008A01C File Offset: 0x0008821C
	public IEnumerator auto_move_to_pos_cr(bool holdForYToReleaseX)
	{
		if (Mathf.Abs(this.autoDest.y - base.transform.position.y) > 50f)
		{
			this.moveSpeed.y = Mathf.Sign(base.transform.position.y - this.autoDest.y) * 100f;
		}
		float maxYSpeed = 100f;
		float yDir = Mathf.Sign(this.autoDest.y - base.transform.position.y);
		YieldInstruction wait = new WaitForFixedUpdate();
		while (this.autoX || this.autoY)
		{
			if (!CupheadTime.IsPaused())
			{
				if (this.autoX)
				{
					float num = (Mathf.Abs(this.autoDest.x - base.transform.position.x) >= 100f) ? 1f : 0.5f;
					this.moveSpeed.x = Mathf.Clamp(this.moveSpeed.x + Mathf.Sign(this.autoDest.x - base.transform.position.x) * 5f * num, -400f, 400f);
					this.MoveAirplane();
					if (Mathf.Abs(base.transform.position.x - this.autoDest.x) < 50f && Mathf.Abs(base.transform.position.y - this.autoDest.y) < (float)((!holdForYToReleaseX) ? 1000 : 20))
					{
						this.autoX = false;
						this.moveSpeed.x = Mathf.Clamp(this.moveSpeed.x, base.properties.CurrentState.plane.speedAtMaxTilt.min, base.properties.CurrentState.plane.speedAtMaxTilt.max);
					}
				}
				if (this.autoY)
				{
					float num2 = (Mathf.Abs(this.autoDest.y - base.transform.position.y) >= 50f) ? 1f : 0.5f;
					this.moveSpeed.y = Mathf.Clamp(this.moveSpeed.y + Mathf.Sign(this.autoDest.y - base.transform.position.y) * 3f * num2, -maxYSpeed, maxYSpeed);
					if (yDir != Mathf.Sign(this.autoDest.y - base.transform.position.y))
					{
						maxYSpeed *= 0.99f;
					}
					if (Mathf.Abs(base.transform.position.y - this.autoDest.y) < 5f && this.moveSpeed.y < 2f)
					{
						this.autoY = false;
						this.moveSpeed.y = 0f;
						base.transform.position = new Vector3(base.transform.position.x, this.autoDest.y);
					}
				}
			}
			yield return wait;
		}
		this.autoX = false;
		yield break;
	}

	// Token: 0x06000E48 RID: 3656 RVA: 0x0008A040 File Offset: 0x00088240
	public void HandleDip()
	{
		this.p1contactTime = ((!this.p1IsColliding) ? 0f : Mathf.Clamp(this.p1contactTime + CupheadTime.Delta, 0f, 1.2f));
		this.p2contactTime = ((!this.p2IsColliding) ? 0f : Mathf.Clamp(this.p2contactTime + CupheadTime.Delta, 0f, 1.2f));
		float num = Mathf.Clamp(Mathf.Sin(this.p1contactTime / 1.2f * 3.14159274f) * 12f + Mathf.Sin(this.p2contactTime / 1.2f * 3.14159274f) * 12f, 0f, 12f);
		if (!this.p1IsColliding && !this.p2IsColliding)
		{
			this.tiltable.localPosition = Vector3.Lerp(this.tiltable.transform.localPosition, this.tiltableBasePos + Vector3.up * 9f, 0.1f);
		}
		else
		{
			this.tiltable.localPosition = Vector3.Lerp(this.tiltable.transform.localPosition, this.tiltableBasePos + Vector3.down * num, 0.5f);
		}
	}

	// Token: 0x06000E49 RID: 3657 RVA: 0x0008A1A4 File Offset: 0x000883A4
	public float GetDestRotationVal()
	{
		LevelProperties.Airplane.Plane plane = base.properties.CurrentState.plane;
		float num = Vector3.Distance(this.edgeRight.position, Vector3.Lerp(this.edgeLeft.position, this.edgeRight.position, Mathf.InverseLerp(plane.speedAtMaxTilt.min, plane.speedAtMaxTilt.max, this.moveSpeed.x)));
		float num2;
		if (this.p1IsColliding && this.p2IsColliding && this.player1 != null && this.player2 != null)
		{
			num2 = Vector3.Distance(Vector3.Lerp(this.player1.transform.position, this.player2.transform.position, 0.5f), this.edgeRight.position);
		}
		else
		{
			AbstractPlayerController abstractPlayerController = null;
			if (this.p1IsColliding && this.player1 != null)
			{
				abstractPlayerController = this.player1;
			}
			else if (this.p2IsColliding && this.player2 != null)
			{
				abstractPlayerController = this.player2;
			}
			if (abstractPlayerController != null)
			{
				num2 = Vector3.Distance(this.edgeRight.position, abstractPlayerController.transform.position);
			}
			else
			{
				num2 = num;
			}
		}
		return Mathf.Lerp(num2, num, this.autoTiltTime);
	}

	// Token: 0x06000E4A RID: 3658 RVA: 0x0008A31C File Offset: 0x0008851C
	public IEnumerator handle_tilt_cr()
	{
		LevelProperties.Airplane.Plane p = base.properties.CurrentState.plane;
		float destRotationVal = this.rotationVal;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (!CupheadTime.IsPaused())
			{
				if (!this.p1IsColliding && !this.p2IsColliding && this.moveSpeed.x == 0f)
				{
					Mathf.Lerp(destRotationVal, Vector3.Distance(this.edgeRight.position, base.transform.position), 0.05f);
				}
				else
				{
					destRotationVal = this.GetDestRotationVal();
				}
				if (Mathf.Abs(destRotationVal - this.rotationVal) > 0.1f)
				{
					this.rotationVal = Mathf.Lerp(this.rotationVal, destRotationVal, 0.15f);
				}
				else
				{
					this.rotationVal = destRotationVal;
				}
				this.tiltable.transform.SetEulerAngles(null, null, new float?(p.tiltAngle.GetFloatAt(this.rotationVal / this.rotationDist)));
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06000E4B RID: 3659 RVA: 0x0008A338 File Offset: 0x00088538
	public IEnumerator handle_player_move_cr()
	{
		LevelProperties.Airplane.Plane p = base.properties.CurrentState.plane;
		float destMoveSpeed = 0f;
		bool goingLeft = false;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (!CupheadTime.IsPaused())
			{
				while (this.autoX)
				{
					yield return null;
				}
				goingLeft = (this.moveSpeed.x < 0f);
				if (!this.p1IsColliding && !this.p2IsColliding)
				{
					if (this.moveSpeed.x < 0f && goingLeft)
					{
						this.moveSpeed.x = this.moveSpeed.x + p.decelerationAmount;
					}
					else if (this.moveSpeed.x > 0f && !goingLeft)
					{
						this.moveSpeed.x = this.moveSpeed.x - p.decelerationAmount;
					}
					destMoveSpeed = this.moveSpeed.x;
				}
				else
				{
					destMoveSpeed = -p.speedAtMaxTilt.GetFloatAt(this.rotationVal / this.rotationDist);
					if (Mathf.Abs(destMoveSpeed - this.moveSpeed.x) > 4.1f)
					{
						this.moveSpeed.x = this.moveSpeed.x + Mathf.Sign(destMoveSpeed - this.moveSpeed.x) * 4.1f;
					}
					else
					{
						this.moveSpeed.x = destMoveSpeed;
					}
				}
				this.MoveAirplane();
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06000E4C RID: 3660 RVA: 0x0000C23B File Offset: 0x0000A43B
	public void SetXRange(float min, float max)
	{
		this.minX = min;
		this.maxX = max;
	}

	// Token: 0x06000E4D RID: 3661 RVA: 0x0008A354 File Offset: 0x00088554
	public void SetPartAngles()
	{
		float num = base.transform.position.x / this.maxParallaxX;
		for (int i = 0; i < this.planeParts.Length; i++)
		{
			this.planeParts[i].SetLocalEulerAngles(null, null, new float?(Mathf.LerpUnclamped(0f, this.planePartAngleRanges[i], num)));
			this.planeParts[i].SetLocalPosition(new float?(Mathf.LerpUnclamped(0f, this.planePartPosOffsets[i].x, num)), new float?(Mathf.Lerp(0f, this.planePartPosOffsets[i].y, Mathf.Abs(num))), null);
		}
	}

	// Token: 0x06000E4E RID: 3662 RVA: 0x0008A42C File Offset: 0x0008862C
	public void MoveAirplane()
	{
		if (CupheadTime.IsPaused())
		{
			return;
		}
		this.HandleDip();
		this.SetPartAngles();
		base.transform.position += Vector3.up * this.moveSpeed.y * CupheadTime.FixedDelta;
		base.transform.position += Vector3.right * this.moveSpeed.x * CupheadTime.FixedDelta;
		if (!this.autoX)
		{
			if (base.transform.position.x < this.minX && this.moveSpeed.x < 0f)
			{
				this.moveSpeed.x = this.moveSpeed.x * (12.5f * CupheadTime.FixedDelta);
				this.AutoMoveToPos(new Vector3(this.minX + 50f, base.transform.position.y), false, false);
			}
			if (base.transform.position.x > this.maxX && this.moveSpeed.x > 0f)
			{
				this.moveSpeed.x = this.moveSpeed.x * (12.5f * CupheadTime.FixedDelta);
				this.AutoMoveToPos(new Vector3(this.maxX - 50f, base.transform.position.y), false, false);
			}
		}
		this.updateCount++;
		if (this.updateCount % 5 == 0)
		{
			this.UpdateSound();
		}
	}

	// Token: 0x06000E4F RID: 3663 RVA: 0x0008A5E0 File Offset: 0x000887E0
	public void UpdateSound()
	{
		float num = Mathf.Abs(this.moveSpeed.x) / base.properties.CurrentState.plane.speedAtMaxTilt.max;
		if (Mathf.Abs(num - this.lastNormalizedSpeed) < 0.01f)
		{
			return;
		}
		this.lastNormalizedSpeed = num;
		AudioManager.ChangeSFXPitch("sfx_dlc_dogfight_playerplane_loop", 1f + num * this.pitchIncreaseFactor, 0f);
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_playerplane_loop", this.volume.GetFloatAt(num), 0f);
		float floatAt = this.volumeHighSpeed.GetFloatAt(Mathf.InverseLerp(this.volumeHighSpeedSpeedFloor, 1f, num));
		if (floatAt > this.cachedHighSpeedVolume)
		{
			this.cachedHighSpeedVolume += this.highSpeedVolumeIncreaseRate * CupheadTime.FixedDelta;
			if (this.cachedHighSpeedVolume > floatAt)
			{
				this.cachedHighSpeedVolume = floatAt;
			}
		}
		if (floatAt < this.cachedHighSpeedVolume)
		{
			this.cachedHighSpeedVolume -= this.highSpeedVolumeDecreaseRate * CupheadTime.FixedDelta;
			if (this.cachedHighSpeedVolume < floatAt)
			{
				this.cachedHighSpeedVolume = floatAt;
			}
		}
		AudioManager.ChangeSFXPitch("sfx_dlc_dogfight_playerplane_highspeed_loop", 1f + num * this.pitchIncreaseFactorHighSpeed, 0f);
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_playerplane_highspeed_loop", this.cachedHighSpeedVolume, 0f);
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x0000C24B File Offset: 0x0000A44B
	public void SFX_DOGFIGHT_PlayerPlane_Loop()
	{
		AudioManager.PlayLoop("sfx_dlc_dogfight_playerplane_loop");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_playerplane_loop");
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_dogfight_playerplane_loop", 0.25f, 3f);
	}

	// Token: 0x06000E51 RID: 3665 RVA: 0x0000C27B File Offset: 0x0000A47B
	public void SFX_DOGFIGHT_PlayerPlane_HighSpeed_Loop()
	{
		AudioManager.PlayLoop("sfx_dlc_dogfight_playerplane_highspeed_loop");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_playerplane_highspeed_loop");
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_dogfight_playerplane_highspeed_loop", 0.6f, 3f);
	}

	// Token: 0x06000E52 RID: 3666 RVA: 0x0000C2AB File Offset: 0x0000A4AB
	public void SFX_DOGFIGHT_PlayerPlane_StopLoop()
	{
		AudioManager.Stop("sfx_dlc_dogfight_playerplane_loop");
	}

	// Token: 0x06000E53 RID: 3667 RVA: 0x0000C2B7 File Offset: 0x0000A4B7
	public void AnimationEvent_SFX_DOGFIGHT_PlayerPlane_CanteenCheer()
	{
		AudioManager.Play("sfx_dlc_dogfight_p2_pilotclap");
	}

	// Token: 0x06000E54 RID: 3668 RVA: 0x0008A730 File Offset: 0x00088930
	public void WORKAROUND_NullifyFields()
	{
		this.volume = null;
		this.volumeHighSpeed = null;
		this.edgeLeft = null;
		this.edgeRight = null;
		this.airplane1 = null;
		this.tiltable = null;
		this.planeParts = null;
		this.planePartAngleRanges = null;
		this.planePartPosOffsets = null;
		this.planePuffFX = null;
		this.planePuffPos = null;
		this.player1 = null;
		this.player2 = null;
		this.playerInSuper = null;
		this.restorePlayerPos = null;
		this.playerRelativePosAtSuperStart = null;
		this.puffTimer = null;
		this.autoMoveCoroutine = null;
	}

	// Token: 0x04000B54 RID: 2900
	public const float PUFF_DELAY_L = 1f;

	// Token: 0x04000B55 RID: 2901
	public const float PUFF_DELAY_R = 0.8f;

	// Token: 0x04000B56 RID: 2902
	public const float AUTO_MOVE_MAX_X_DIST = 50f;

	// Token: 0x04000B57 RID: 2903
	public const float AUTO_MOVE_MAX_Y_DIST = 5f;

	// Token: 0x04000B58 RID: 2904
	public const float AUTO_MOVE_MAX_Y_END_SPEED = 2f;

	// Token: 0x04000B59 RID: 2905
	public const float AUTO_MOVE_MAX_X_SPEED = 400f;

	// Token: 0x04000B5A RID: 2906
	public const float AUTO_MOVE_MAX_Y_SPEED = 100f;

	// Token: 0x04000B5B RID: 2907
	public const float AUTO_ACCEL_X = 5f;

	// Token: 0x04000B5C RID: 2908
	public const float AUTO_ACCEL_Y = 3f;

	// Token: 0x04000B5D RID: 2909
	public const float MIN_TILT_DIFFERENCE = 0.1f;

	// Token: 0x04000B5E RID: 2910
	public const float TILT_ATTENUATION = 0.15f;

	// Token: 0x04000B5F RID: 2911
	public const float ACCEL_SPEED = 4.1f;

	// Token: 0x04000B60 RID: 2912
	public const float BOUNCE_TIME = 1.2f;

	// Token: 0x04000B61 RID: 2913
	public const float BOUNCE_DIST = 12f;

	// Token: 0x04000B62 RID: 2914
	public const float RISE_DIST = 9f;

	// Token: 0x04000B63 RID: 2915
	public const float RISE_RATE = 0.1f;

	// Token: 0x04000B64 RID: 2916
	public const float BOUNCE_RATE = 0.5f;

	// Token: 0x04000B65 RID: 2917
	public const float DAMP_ON_BOUNDARY_COLLIDE = 12.5f;

	// Token: 0x04000B66 RID: 2918
	[SerializeField]
	public float pitchIncreaseFactor = 0.5f;

	// Token: 0x04000B67 RID: 2919
	[SerializeField]
	public float pitchIncreaseFactorHighSpeed = 0.5f;

	// Token: 0x04000B68 RID: 2920
	[SerializeField]
	public MinMax volume = new MinMax(0.25f, 0.5f);

	// Token: 0x04000B69 RID: 2921
	[SerializeField]
	public MinMax volumeHighSpeed = new MinMax(0.25f, 0.5f);

	// Token: 0x04000B6A RID: 2922
	public float cachedHighSpeedVolume = 1E-06f;

	// Token: 0x04000B6B RID: 2923
	[SerializeField]
	public float highSpeedVolumeIncreaseRate = 1f;

	// Token: 0x04000B6C RID: 2924
	[SerializeField]
	public float highSpeedVolumeDecreaseRate = 0.25f;

	// Token: 0x04000B6D RID: 2925
	[SerializeField]
	public float volumeHighSpeedSpeedFloor = 0.5f;

	// Token: 0x04000B6E RID: 2926
	[SerializeField]
	public Transform edgeLeft;

	// Token: 0x04000B6F RID: 2927
	[SerializeField]
	public Transform edgeRight;

	// Token: 0x04000B70 RID: 2928
	[SerializeField]
	public Transform airplane1;

	// Token: 0x04000B71 RID: 2929
	[SerializeField]
	public Transform tiltable;

	// Token: 0x04000B72 RID: 2930
	[SerializeField]
	public Transform[] planeParts;

	// Token: 0x04000B73 RID: 2931
	[SerializeField]
	public float[] planePartAngleRanges;

	// Token: 0x04000B74 RID: 2932
	[SerializeField]
	public Vector2[] planePartPosOffsets;

	// Token: 0x04000B75 RID: 2933
	[SerializeField]
	public Effect planePuffFX;

	// Token: 0x04000B76 RID: 2934
	[SerializeField]
	public Transform[] planePuffPos;

	// Token: 0x04000B77 RID: 2935
	public AbstractPlayerController player1;

	// Token: 0x04000B78 RID: 2936
	public AbstractPlayerController player2;

	// Token: 0x04000B79 RID: 2937
	public bool p1IsColliding;

	// Token: 0x04000B7A RID: 2938
	public bool p2IsColliding;

	// Token: 0x04000B7B RID: 2939
	public Vector3 tiltableBasePos;

	// Token: 0x04000B7C RID: 2940
	public float maxParallaxX;

	// Token: 0x04000B7D RID: 2941
	public bool autoX;

	// Token: 0x04000B7E RID: 2942
	public bool autoY;

	// Token: 0x04000B7F RID: 2943
	public Vector3 autoDest;

	// Token: 0x04000B80 RID: 2944
	public float autoTiltTime;

	// Token: 0x04000B81 RID: 2945
	public bool autoTilt;

	// Token: 0x04000B82 RID: 2946
	public float minX;

	// Token: 0x04000B83 RID: 2947
	public float maxX;

	// Token: 0x04000B84 RID: 2948
	public float rotationDist;

	// Token: 0x04000B85 RID: 2949
	public float rotationVal;

	// Token: 0x04000B86 RID: 2950
	public float p1contactTime;

	// Token: 0x04000B87 RID: 2951
	public float p2contactTime;

	// Token: 0x04000B88 RID: 2952
	public bool[] playerInSuper = new bool[2];

	// Token: 0x04000B89 RID: 2953
	public bool[] restorePlayerPos = new bool[2];

	// Token: 0x04000B8A RID: 2954
	public float[] playerRelativePosAtSuperStart = new float[2];

	// Token: 0x04000B8B RID: 2955
	public float[] puffTimer = new float[2];

	// Token: 0x04000B8C RID: 2956
	public Vector3 moveSpeed;

	// Token: 0x04000B8D RID: 2957
	public Coroutine autoMoveCoroutine;

	// Token: 0x04000B8E RID: 2958
	public float lastNormalizedSpeed;

	// Token: 0x04000B8F RID: 2959
	public int updateCount;
}
