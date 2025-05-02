using System;
using Rewired;
using UnityEngine;

// Token: 0x0200057C RID: 1404
public class PlayerInput : AbstractMonoBehaviour
{
	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x06003ACC RID: 15052 RVA: 0x0002FCD2 File Offset: 0x0002DED2
	// (set) Token: 0x06003ACD RID: 15053 RVA: 0x0002FCDA File Offset: 0x0002DEDA
	public PlayerId playerId { get; set; }

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x06003ACE RID: 15054 RVA: 0x0002FCE3 File Offset: 0x0002DEE3
	public bool IsDead
	{
		get
		{
			return this.player != null && this.player.IsDead;
		}
	}

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x06003ACF RID: 15055 RVA: 0x0002FD03 File Offset: 0x0002DF03
	// (set) Token: 0x06003AD0 RID: 15056 RVA: 0x0002FD0B File Offset: 0x0002DF0B
	public Player actions { get; set; }

	// Token: 0x06003AD1 RID: 15057 RVA: 0x0002FD14 File Offset: 0x0002DF14
	public override void Awake()
	{
		base.Awake();
		this.player = base.GetComponent<AbstractPlayerController>();
	}

	// Token: 0x06003AD2 RID: 15058 RVA: 0x0002FD28 File Offset: 0x0002DF28
	public void Start()
	{
		if (Level.Current != null && Level.Current.CameraRotates)
		{
			this.canRotateInput = true;
			this.cameraTransform = Camera.main.transform;
		}
	}

	// Token: 0x06003AD3 RID: 15059 RVA: 0x0002FD60 File Offset: 0x0002DF60
	public void Init(PlayerId playerId)
	{
		this.playerId = playerId;
		this.actions = PlayerManager.GetPlayerInput(playerId);
	}

	// Token: 0x06003AD4 RID: 15060 RVA: 0x0002FD75 File Offset: 0x0002DF75
	public override void StopAllCoroutines()
	{
	}

	// Token: 0x06003AD5 RID: 15061 RVA: 0x00111234 File Offset: 0x0010F434
	public int GetAxisInt(PlayerInput.Axis axis, bool crampedDiagonal = false, bool duckMod = false)
	{
		Vector2 vector;
		vector..ctor(this.actions.GetAxis(0), this.actions.GetAxis(1));
		if (this.canRotateInput)
		{
			if (SettingsData.Data.rotateControlsWithCamera)
			{
				vector = this.cameraTransform.rotation * vector;
			}
			else if (Mathf.Abs(this.cameraTransform.rotation.eulerAngles.z - 180f) <= 1f)
			{
				vector = this.cameraTransform.rotation * vector;
			}
		}
		float magnitude = vector.magnitude;
		float num = (!crampedDiagonal) ? 0.38268f : 0.5f;
		if (magnitude < 0.375f)
		{
			return 0;
		}
		float num2 = ((axis != PlayerInput.Axis.X) ? vector.y : vector.x) / magnitude;
		if (num2 > num)
		{
			return 1;
		}
		if (num2 < ((!duckMod) ? (-num) : -0.705f))
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06003AD6 RID: 15062 RVA: 0x0002FD77 File Offset: 0x0002DF77
	public float GetAxis(PlayerInput.Axis axis)
	{
		if (axis == PlayerInput.Axis.X)
		{
			return this.actions.GetAxis(0);
		}
		return this.actions.GetAxis(1);
	}

	// Token: 0x06003AD7 RID: 15063 RVA: 0x0002FD98 File Offset: 0x0002DF98
	public bool GetButton(CupheadButton button)
	{
		return this.actions.GetButton((int)button);
	}

	// Token: 0x04002F23 RID: 12067
	public AbstractPlayerController player;

	// Token: 0x04002F26 RID: 12070
	public bool canRotateInput;

	// Token: 0x04002F27 RID: 12071
	public Transform cameraTransform;

	// Token: 0x020011FB RID: 4603
	public enum Axis
	{
		// Token: 0x04007D24 RID: 32036
		X,
		// Token: 0x04007D25 RID: 32037
		Y
	}
}
