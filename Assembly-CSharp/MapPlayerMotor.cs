using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004B7 RID: 1207
public class MapPlayerMotor : AbstractMapPlayerComponent
{
	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x0600321C RID: 12828 RVA: 0x0002994A File Offset: 0x00027B4A
	// (set) Token: 0x0600321D RID: 12829 RVA: 0x00029952 File Offset: 0x00027B52
	public Vector2 velocity { get; set; }

	// Token: 0x0600321E RID: 12830 RVA: 0x000EC0C0 File Offset: 0x000EA2C0
	public void Update()
	{
		if (!MapPlayerController.CanMove() || base.player.state == MapPlayerController.State.Stationary)
		{
			this.velocity = Vector2.zero;
			this.axis = Vector2.zero;
			base.rigidbody2D.velocity = Vector2.zero;
			return;
		}
		if (PauseManager.state == PauseManager.State.Paused)
		{
			return;
		}
		this.HandleInput();
		MapPlayerController.State state = base.player.state;
		if (state != MapPlayerController.State.Walking)
		{
			if (state == MapPlayerController.State.Ladder)
			{
				this.MoveLadder();
			}
		}
		else
		{
			this.MoveWalking();
		}
	}

	// Token: 0x0600321F RID: 12831 RVA: 0x000EC158 File Offset: 0x000EA358
	public void LateUpdate()
	{
		MapPlayerController.State state = base.player.state;
		if (state == MapPlayerController.State.Ladder)
		{
			this.ClampPositionLadder();
		}
	}

	// Token: 0x06003220 RID: 12832 RVA: 0x0002995B File Offset: 0x00027B5B
	public override void OnPause()
	{
		base.OnPause();
		base.rigidbody2D.velocity = Vector2.zero;
	}

	// Token: 0x06003221 RID: 12833 RVA: 0x000EC188 File Offset: 0x000EA388
	public void HandleInput()
	{
		if (base.player.EquipMenuOpen)
		{
			return;
		}
		this.axis = new Vector2((float)base.input.GetAxisInt(PlayerInput.Axis.X, false, false), (float)base.input.GetAxisInt(PlayerInput.Axis.Y, false, false));
		float magnitude = this.axis.magnitude;
		if (magnitude < 0.0001f)
		{
			this.axis = Vector2.zero;
		}
		else
		{
			this.axis /= magnitude;
		}
	}

	// Token: 0x06003222 RID: 12834 RVA: 0x000EC208 File Offset: 0x000EA408
	public void MoveWalking()
	{
		this.velocity = Vector2.Lerp(this.velocity, new Vector2(this.axis.x * 2.5f, this.axis.y * 2.5f), CupheadTime.Delta * 100f);
		base.rigidbody2D.velocity = this.velocity;
	}

	// Token: 0x06003223 RID: 12835 RVA: 0x00029973 File Offset: 0x00027B73
	public void MoveLadder()
	{
		this.velocity = new Vector2(0f, this.axis.y * 2.5f);
		base.rigidbody2D.velocity = this.velocity;
	}

	// Token: 0x06003224 RID: 12836 RVA: 0x000EC270 File Offset: 0x000EA470
	public void ClampPositionLadder()
	{
		MapPlayerLadderObject mapPlayerLadderObject = base.player.ladderManager.Current;
		MapPlayerController.State state = base.player.state;
		if (state == MapPlayerController.State.Ladder)
		{
			base.transform.SetPosition(null, new float?(Mathf.Clamp(base.transform.position.y, mapPlayerLadderObject.bottom.y, mapPlayerLadderObject.top.y)), null);
		}
	}

	// Token: 0x06003225 RID: 12837 RVA: 0x000299A7 File Offset: 0x00027BA7
	public override void OnLadderEnter(Vector2 point, MapPlayerLadderObject ladder, MapLadder.Location location)
	{
		base.OnLadderEnter(point, ladder, location);
		base.StartCoroutine(this.onLadderStart_cr(point, location));
	}

	// Token: 0x06003226 RID: 12838 RVA: 0x000299C1 File Offset: 0x00027BC1
	public override void OnLadderExit(Vector2 point, Vector2 exit, MapLadder.Location location)
	{
		base.OnLadderExit(point, exit, location);
		base.StartCoroutine(this.onLadderEnd_cr(exit, location));
	}

	// Token: 0x06003227 RID: 12839 RVA: 0x000EC304 File Offset: 0x000EA504
	public IEnumerator onLadderStart_cr(Vector2 endPos, MapLadder.Location location)
	{
		location = ((location != MapLadder.Location.Top) ? MapLadder.Location.Top : MapLadder.Location.Bottom);
		yield return base.StartCoroutine(this.ladder_cr(base.transform.position, endPos, location));
		base.player.LadderEnterComplete();
		yield break;
	}

	// Token: 0x06003228 RID: 12840 RVA: 0x000EC330 File Offset: 0x000EA530
	public IEnumerator onLadderEnd_cr(Vector2 endPos, MapLadder.Location location)
	{
		yield return base.StartCoroutine(this.ladder_cr(base.transform.position, endPos, location));
		base.player.LadderExitComplete();
		yield break;
	}

	// Token: 0x06003229 RID: 12841 RVA: 0x000EC35C File Offset: 0x000EA55C
	public IEnumerator ladder_cr(Vector2 startPos, Vector2 endPos, MapLadder.Location location)
	{
		Vector2 centerPos = new Vector2(Mathf.Lerp(startPos.x, endPos.x, 0.5f), (location != MapLadder.Location.Top) ? (startPos.y + 0.2f) : (endPos.y + 0.2f));
		float t = 0f;
		float time = 0.15f;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, t / time);
			Vector2 newPos = Vector2.Lerp(startPos, centerPos, val);
			base.transform.SetPosition(new float?(newPos.x), new float?(newPos.y), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		while (t < time)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / time);
			Vector2 newPos2 = Vector2.Lerp(centerPos, endPos, val2);
			base.transform.SetPosition(new float?(newPos2.x), new float?(newPos2.y), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400291F RID: 10527
	public const float SPEED = 2.5f;

	// Token: 0x04002920 RID: 10528
	public const float DIAGONAL_FALLOFF = 0.75f;

	// Token: 0x04002921 RID: 10529
	public const float FALLOFF_SPEED = 100f;

	// Token: 0x04002922 RID: 10530
	public const float INPUT_THRESHOLD = 0.3f;

	// Token: 0x04002924 RID: 10532
	public Vector2 axis;

	// Token: 0x04002925 RID: 10533
	public const float LADDER_ENTER_TIME = 0.3f;

	// Token: 0x04002926 RID: 10534
	public const float LADDER_EXIT_TIME = 0.3f;

	// Token: 0x04002927 RID: 10535
	public const float LADDER_EXIT_JUMP = 0.2f;
}
