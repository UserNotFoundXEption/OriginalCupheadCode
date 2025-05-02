using System;
using System.Linq;
using UnityEngine;

// Token: 0x020004B1 RID: 1201
public class MapPlayerAnimationController : AbstractMapPlayerComponent
{
	// Token: 0x17000398 RID: 920
	// (get) Token: 0x060031C8 RID: 12744 RVA: 0x000295D3 File Offset: 0x000277D3
	// (set) Token: 0x060031C9 RID: 12745 RVA: 0x000295DB File Offset: 0x000277DB
	public MapPlayerAnimationController.Direction direction { get; set; }

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x060031CA RID: 12746 RVA: 0x000295E4 File Offset: 0x000277E4
	// (set) Token: 0x060031CB RID: 12747 RVA: 0x000295EC File Offset: 0x000277EC
	public MapPlayerAnimationController.State state { get; set; }

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x060031CC RID: 12748 RVA: 0x000295F5 File Offset: 0x000277F5
	// (set) Token: 0x060031CD RID: 12749 RVA: 0x000295FD File Offset: 0x000277FD
	public SpriteRenderer spriteRenderer { get; set; }

	// Token: 0x060031CE RID: 12750 RVA: 0x000EB13C File Offset: 0x000E933C
	public void Init(MapPlayerPose pose)
	{
		this.Cuphead.enabled = false;
		this.Mugman.enabled = false;
		this.ghostInPortal[0].sortingLayerName = "Effects";
		this.ghostInPortal[1].sortingLayerName = "Effects";
		this.portal.sortingLayerName = "Effects";
		PlayerId id = base.player.id;
		if (id == PlayerId.PlayerOne || id != PlayerId.PlayerTwo)
		{
			this.spriteRenderer = ((!PlayerManager.player1IsMugman) ? this.Cuphead : this.Mugman);
			base.animator.SetInteger("Player", 0);
		}
		else
		{
			base.animator.SetInteger("Player", 1);
			this.spriteRenderer = ((!PlayerManager.player1IsMugman) ? this.Mugman : this.Cuphead);
		}
		this.spriteRenderer.enabled = true;
		switch (pose)
		{
		case MapPlayerPose.Default:
			this.state = MapPlayerAnimationController.State.Idle;
			break;
		case MapPlayerPose.Joined:
		case MapPlayerPose.Won:
			base.animator.Play((!PlayerManager.playerWasChalice[(int)base.player.id]) ? "Jump" : "WinChalice_Loop");
			if (PlayerManager.playerWasChalice[(int)base.player.id])
			{
				if (PlayerManager.player1IsMugman)
				{
					this.ghostInPortal[(int)base.player.id].enabled = false;
				}
				else
				{
					this.ghostInPortal[(int)(PlayerId.PlayerTwo - base.player.id)].enabled = false;
				}
				if (base.player.id == PlayerId.PlayerTwo)
				{
					base.transform.localScale = new Vector3(-1f, 1f);
				}
			}
			break;
		}
		this.SetProperties();
	}

	// Token: 0x060031CF RID: 12751 RVA: 0x00029606 File Offset: 0x00027806
	public void MovePortalSwapToFront()
	{
		this.Chalice.sortingLayerName = "Effects";
	}

	// Token: 0x060031D0 RID: 12752 RVA: 0x000EB314 File Offset: 0x000E9514
	public void Update()
	{
		if (base.player.state == MapPlayerController.State.Stationary)
		{
			this.SetStationary();
			return;
		}
		if (!MapPlayerController.CanMove())
		{
			this.SetStationary();
			return;
		}
		Vector2 vector;
		vector..ctor(base.player.input.actions.GetAxis(0), base.player.input.actions.GetAxis(1));
		this.state = ((vector.magnitude <= 0.3f) ? MapPlayerAnimationController.State.Idle : MapPlayerAnimationController.State.Walk);
		this.SetProperties();
		this.UpdateDjimmiCodeTimer();
	}

	// Token: 0x060031D1 RID: 12753 RVA: 0x00029618 File Offset: 0x00027818
	public void SetStationary()
	{
		this.state = MapPlayerAnimationController.State.Idle;
		this.axis.x = 0;
		this.axis.y = 0;
		this.SetProperties();
	}

	// Token: 0x060031D2 RID: 12754 RVA: 0x00029649 File Offset: 0x00027849
	public void CompleteJump()
	{
		base.animator.SetTrigger("OnJumpComplete");
	}

	// Token: 0x060031D3 RID: 12755 RVA: 0x000EB3A8 File Offset: 0x000E95A8
	public void SetProperties()
	{
		if (this.state == MapPlayerAnimationController.State.Walk)
		{
			this.axis.x = base.player.input.GetAxisInt(PlayerInput.Axis.X, false, false);
			this.axis.y = base.player.input.GetAxisInt(PlayerInput.Axis.Y, false, false);
			if (this.axis.x == -1)
			{
				this.spriteRenderer.transform.SetScale(new float?(-1f), null, null);
			}
			else
			{
				this.spriteRenderer.transform.SetScale(new float?(1f), null, null);
			}
		}
		base.animator.SetInteger("X", this.axis.x);
		base.animator.SetInteger("Y", this.axis.y);
		base.animator.SetInteger("Speed", (this.state != MapPlayerAnimationController.State.Idle) ? 1 : 0);
		this.SetDirectionRotation();
	}

	// Token: 0x060031D4 RID: 12756 RVA: 0x000EB4E4 File Offset: 0x000E96E4
	public void SetDirectionRotation()
	{
		this.facingUpwards = (this.axis.y > 0);
		if (this.axis.x == 1 && this.axis.y == 1)
		{
			this.directionRotation = -45f;
		}
		else if (this.axis.x == 1 && this.axis.y == 0)
		{
			this.directionRotation = -90f;
		}
		else if (this.axis.x == 1 && this.axis.y == -1)
		{
			this.directionRotation = -135f;
		}
		else if (this.axis.x == 0 && this.axis.y == 1)
		{
			this.directionRotation = 0f;
		}
		else if (this.axis.x == 0 && this.axis.y == 0)
		{
			this.directionRotation = 0f;
		}
		else if (this.axis.x == 0 && this.axis.y == -1)
		{
			this.directionRotation = -180f;
		}
		else if (this.axis.x == -1 && this.axis.y == 1)
		{
			this.directionRotation = 45f;
		}
		else if (this.axis.x == -1 && this.axis.y == 0)
		{
			this.directionRotation = 90f;
		}
		else if (this.axis.x == -1 && this.axis.y == -1)
		{
			this.directionRotation = 135f;
		}
		this.UpdateDjimmiCode((int)this.directionRotation);
	}

	// Token: 0x060031D5 RID: 12757 RVA: 0x000EB728 File Offset: 0x000E9928
	public void UpdateDjimmiCode(int direction)
	{
		if (direction == -45 || direction == -135 || direction == 45 || direction == 135)
		{
			return;
		}
		if (direction == this.djimmiCodeEntry[this.djimmiCodeEntry.Length - 1])
		{
			return;
		}
		for (int i = 0; i < this.djimmiCodeEntry.Length - 1; i++)
		{
			this.djimmiCodeEntry[i] = this.djimmiCodeEntry[i + 1];
			this.djimmiCodeTimeStamp[i] = this.djimmiCodeTimeStamp[i + 1];
		}
		this.djimmiCodeEntry[this.djimmiCodeEntry.Length - 1] = direction;
		this.djimmiCodeTimeStamp[this.djimmiCodeEntry.Length - 1] = 2f;
		if (this.djimmiCodeTimeStamp[0] > 0f && (this.djimmiCodeEntry.SequenceEqual(this.djimmiCodeA) || this.djimmiCodeEntry.SequenceEqual(this.djimmiCodeB)))
		{
			for (int j = 0; j < this.djimmiCodeEntry.Length; j++)
			{
				this.djimmiCodeEntry[j] = 0;
				this.djimmiCodeTimeStamp[j] = 0f;
			}
			base.player.TryActivateDjimmi();
		}
	}

	// Token: 0x060031D6 RID: 12758 RVA: 0x000EB854 File Offset: 0x000E9A54
	public void UpdateDjimmiCodeTimer()
	{
		for (int i = 0; i < this.djimmiCodeTimeStamp.Length; i++)
		{
			this.djimmiCodeTimeStamp[i] -= CupheadTime.Delta;
		}
	}

	// Token: 0x060031D7 RID: 12759 RVA: 0x000EB894 File Offset: 0x000E9A94
	public void WalkStepLeft()
	{
		if (this.spriteRenderer == this.Cuphead)
		{
			if (this.current != null)
			{
				this.current.PlaySoundRight(true);
			}
			else
			{
				AudioManager.Play("player_map_walk_one_p1");
			}
		}
		else if (this.current != null)
		{
			this.current.PlaySoundRight(false);
		}
		else
		{
			AudioManager.Play("player_map_walk_one_p2");
		}
		this.dustEffect.Create(base.transform.position, this.directionRotation, true, this.spriteRenderer.sortingOrder);
	}

	// Token: 0x060031D8 RID: 12760 RVA: 0x000EB940 File Offset: 0x000E9B40
	public void WalkStepRight()
	{
		if (this.spriteRenderer == this.Cuphead)
		{
			if (this.current != null)
			{
				this.current.PlaySoundRight(true);
			}
			else
			{
				AudioManager.Play("player_map_walk_one_p1");
			}
		}
		else if (this.current != null)
		{
			this.current.PlaySoundRight(false);
		}
		else
		{
			AudioManager.Play("player_map_walk_two_p2");
		}
		this.dustEffect.Create(base.transform.position, this.directionRotation, false, this.spriteRenderer.sortingOrder);
	}

	// Token: 0x060031D9 RID: 12761 RVA: 0x0002965B File Offset: 0x0002785B
	public void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.GetComponent<MapSpritePlaySound>())
		{
			this.current = collider.GetComponent<MapSpritePlaySound>();
		}
	}

	// Token: 0x060031DA RID: 12762 RVA: 0x00029679 File Offset: 0x00027879
	public void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.GetComponent<MapSpritePlaySound>())
		{
			this.current = null;
		}
	}

	// Token: 0x060031DB RID: 12763 RVA: 0x00029692 File Offset: 0x00027892
	public void AniEvent_YawnSFX()
	{
		if (base.player.id == PlayerId.PlayerOne)
		{
			AudioManager.Play("worldmap_playeryawn");
			this.emitAudioFromObject.Add("worldmap_playeryawn");
		}
	}

	// Token: 0x060031DC RID: 12764 RVA: 0x000296BE File Offset: 0x000278BE
	public void AniEvent_GhostSwapSFX()
	{
		AudioManager.Play("sfx_DLC_WorldMap_GhostSwap");
		this.emitAudioFromObject.Add("sfx_DLC_WorldMap_GhostSwap");
	}

	// Token: 0x040028EF RID: 10479
	public const int DJIMMI_CODE_LENGTH = 16;

	// Token: 0x040028F0 RID: 10480
	public const float MAX_TIME_FOR_DJIMMI_CODE = 2f;

	// Token: 0x040028F1 RID: 10481
	public int[] djimmiCodeA = new int[]
	{
		0,
		90,
		-180,
		-90,
		0,
		90,
		-180,
		-90,
		0,
		90,
		-180,
		-90,
		0,
		90,
		-180,
		-90
	};

	// Token: 0x040028F2 RID: 10482
	public int[] djimmiCodeB = new int[]
	{
		0,
		-90,
		-180,
		90,
		0,
		-90,
		-180,
		90,
		0,
		-90,
		-180,
		90,
		0,
		-90,
		-180,
		90
	};

	// Token: 0x040028F3 RID: 10483
	public bool facingUpwards;

	// Token: 0x040028F4 RID: 10484
	[SerializeField]
	public SpriteRenderer Cuphead;

	// Token: 0x040028F5 RID: 10485
	[SerializeField]
	public SpriteRenderer Mugman;

	// Token: 0x040028F6 RID: 10486
	[SerializeField]
	public SpriteRenderer Chalice;

	// Token: 0x040028F7 RID: 10487
	[SerializeField]
	public SpriteRenderer[] ghostInPortal;

	// Token: 0x040028F8 RID: 10488
	[SerializeField]
	public SpriteRenderer portal;

	// Token: 0x040028F9 RID: 10489
	[SerializeField]
	public MapPlayerDust dustEffect;

	// Token: 0x040028FD RID: 10493
	public MapSpritePlaySound current;

	// Token: 0x040028FE RID: 10494
	public Trilean2 axis;

	// Token: 0x040028FF RID: 10495
	public bool onBridge;

	// Token: 0x04002900 RID: 10496
	public float directionRotation;

	// Token: 0x04002901 RID: 10497
	public int[] djimmiCodeEntry = new int[16];

	// Token: 0x04002902 RID: 10498
	public float[] djimmiCodeTimeStamp = new float[16];

	// Token: 0x0200110E RID: 4366
	public enum Direction
	{
		// Token: 0x0400788F RID: 30863
		Left,
		// Token: 0x04007890 RID: 30864
		Right
	}

	// Token: 0x0200110F RID: 4367
	public enum State
	{
		// Token: 0x04007892 RID: 30866
		Idle,
		// Token: 0x04007893 RID: 30867
		Walk
	}
}
