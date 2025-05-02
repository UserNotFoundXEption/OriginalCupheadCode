using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000441 RID: 1089
public class MountainPlatformingLevelElevatorHandler : AbstractPausableComponent
{
	// Token: 0x17000363 RID: 867
	// (get) Token: 0x06002EC3 RID: 11971 RVA: 0x00026FEF File Offset: 0x000251EF
	// (set) Token: 0x06002EC4 RID: 11972 RVA: 0x00026FF6 File Offset: 0x000251F6
	public static bool elevatorIsMoving { get; set; }

	// Token: 0x06002EC5 RID: 11973 RVA: 0x000E0264 File Offset: 0x000DE464
	public void Start()
	{
		MountainPlatformingLevelElevatorHandler.elevatorIsMoving = false;
		base.StartCoroutine(this.wait_cr());
		this.cameraLockRoutine = base.StartCoroutine(this.lock_camera_cr());
		this.invisibleWall.SetActive(false);
		foreach (PlatformingLevelParallax platformingLevelParallax in this.bottomBackground.GetComponentsInChildren<PlatformingLevelParallax>())
		{
			platformingLevelParallax.enabled = false;
		}
	}

	// Token: 0x06002EC6 RID: 11974 RVA: 0x000E02D0 File Offset: 0x000DE4D0
	public IEnumerator lock_camera_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		for (;;)
		{
			player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			CupheadLevelCamera.Current.LockCamera(CupheadLevelCamera.Current.transform.position.x > this.triggerPoint.transform.position.x);
			if (player2 != null)
			{
				if (!player2.IsDead && !player.IsDead)
				{
					if (player.transform.position.x < this.triggerPoint.transform.position.x && player2.transform.position.x < this.triggerPoint.transform.position.x)
					{
						CupheadLevelCamera.Current.LockCamera(false);
					}
				}
				else if (player2.IsDead)
				{
					if (player.transform.position.x < this.triggerPoint.transform.position.x)
					{
						CupheadLevelCamera.Current.LockCamera(false);
					}
				}
				else if (player.IsDead && player2.transform.position.x < this.triggerPoint.transform.position.x)
				{
					CupheadLevelCamera.Current.LockCamera(false);
				}
			}
			else if (player.transform.position.x < this.triggerPoint.transform.position.x)
			{
				CupheadLevelCamera.Current.LockCamera(false);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002EC7 RID: 11975 RVA: 0x000E02EC File Offset: 0x000DE4EC
	public IEnumerator wait_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		for (;;)
		{
			player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			if (player2 != null)
			{
				if (!player2.IsDead && !player.IsDead)
				{
					if (player.transform.position.x > this.triggerPoint.transform.position.x && player2.transform.position.x > this.triggerPoint2.transform.position.x)
					{
						break;
					}
					if (player.transform.position.x > this.triggerPoint2.transform.position.x && player2.transform.position.x > this.triggerPoint.transform.position.x)
					{
						break;
					}
				}
				else if (player2.IsDead)
				{
					if (player.transform.position.x > this.triggerPoint.transform.position.x)
					{
						break;
					}
				}
				else if (player.IsDead && player2.transform.position.x > this.triggerPoint.transform.position.x)
				{
					break;
				}
			}
			else if (player.transform.position.x > this.triggerPoint.transform.position.x)
			{
				break;
			}
			yield return null;
		}
		base.StopCoroutine(this.cameraLockRoutine);
		CupheadLevelCamera.Current.LockCamera(true);
		this.invisibleWall.SetActive(true);
		AudioManager.Play("castle_lift_start");
		CupheadLevelCamera.Current.Shake(10f, 1f, false);
		yield return CupheadTime.WaitForSeconds(this, 0.9f);
		base.StartCoroutine(this.move_cr());
		yield break;
	}

	// Token: 0x06002EC8 RID: 11976 RVA: 0x000E0308 File Offset: 0x000DE508
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float t = 0f;
		MountainPlatformingLevelElevatorHandler.elevatorIsMoving = true;
		Vector3 startPos = this.scrollingObject.transform.position;
		Vector3 pos = this.scrollingObject.transform.position;
		Vector3 dir = this.pointA.transform.position - this.scrollingObject.transform.position;
		Vector3 middle = this.scrollingObject.transform.position + dir.normalized * (Vector3.Distance(this.scrollingObject.transform.position, this.pointA.transform.position) / 2f);
		foreach (ScrollingBackgroundElevator scrollingBackgroundElevator in this.midgroundSprites)
		{
			scrollingBackgroundElevator.SetUp(MathUtils.AngleToDirection(-45f), this.speed);
		}
		this.backgroundSprite.SetUp(MathUtils.AngleToDirection(-45f), this.speed - this.speed / 4f);
		this.foregroundSprite.SetUp(MathUtils.AngleToDirection(-45f), this.speed + this.speed / 4f);
		this.cloudSprite.SetUp(MathUtils.AngleToDirection(-45f), this.speed + this.speed / 4f);
		foreach (PlatformingLevelParallax platformingLevelParallax in this.topBackground.GetComponentsInChildren<PlatformingLevelParallax>())
		{
			platformingLevelParallax.enabled = false;
		}
		this.bottomBackground.parent = this.scrollingObject.transform;
		this.mudmanSpawner.SpawnMudmen();
		AudioManager.PlayLoop("castle_lift_loop");
		while (this.scrollingObject.transform.position.y < middle.y)
		{
			pos -= MathUtils.AngleToDirection(-45f) * this.speed * CupheadTime.FixedDelta;
			this.scrollingObject.transform.position = pos;
			yield return wait;
		}
		while (t < this.time)
		{
			t += CupheadTime.FixedDelta;
			yield return null;
		}
		Vector3 midPos = this.scrollingObject.transform.position;
		float endTime = Vector3.Distance(startPos, this.pointA.position) / this.speed;
		t = 0f;
		foreach (ScrollingBackgroundElevator scrollingBackgroundElevator2 in this.midgroundSprites)
		{
			scrollingBackgroundElevator2.EaseoutSpeed(endTime);
		}
		this.cloudSprite.EaseoutSpeed(endTime);
		this.foregroundSprite.EaseoutSpeed(endTime);
		this.backgroundSprite.EaseoutSpeed(endTime);
		base.StartCoroutine(this.easeTime(endTime));
		this.cloudSprite.ending = true;
		while (this.easeingTime < endTime)
		{
			pos -= MathUtils.AngleToDirection(-45f) * this.speed * CupheadTime.FixedDelta;
			this.scrollingObject.transform.position = pos;
			yield return wait;
		}
		AudioManager.Stop("castle_lift_loop");
		AudioManager.Play("castle_lift_end");
		CupheadLevelCamera.Current.Shake(10f, 0.5f, false);
		foreach (PlatformingLevelParallax platformingLevelParallax2 in this.bottomBackground.GetComponentsInChildren<PlatformingLevelParallax>())
		{
			platformingLevelParallax2.enabled = true;
			platformingLevelParallax2.UpdateBasePosition();
		}
		foreach (ScrollingBackgroundElevator scrollingBackgroundElevator3 in this.midgroundSprites)
		{
			scrollingBackgroundElevator3.ending = true;
		}
		this.backgroundSprite.ending = true;
		MountainPlatformingLevelElevatorHandler.elevatorIsMoving = false;
		if (PlayerManager.GetFirst().transform.position.x < CupheadLevelCamera.Current.transform.position.x)
		{
			CupheadLevelCamera.Current.OffsetCamera(true, true);
		}
		else
		{
			CupheadLevelCamera.Current.OffsetCamera(true, false);
		}
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		CupheadLevelCamera.Current.OffsetCamera(false, false);
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		CupheadLevelCamera.Current.LockCamera(false);
		yield return null;
		yield break;
	}

	// Token: 0x06002EC9 RID: 11977 RVA: 0x000E0324 File Offset: 0x000DE524
	public IEnumerator easeTime(float time)
	{
		float startSpeed = this.speed;
		this.easeingTime = 0f;
		while (this.easeingTime < time)
		{
			this.easeingTime += CupheadTime.Delta;
			this.speed = Mathf.Lerp(startSpeed, 0f, this.easeingTime / time);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002ECA RID: 11978 RVA: 0x000E0348 File Offset: 0x000DE548
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0f, 0f, 1f, 1f);
		Gizmos.DrawLine(this.triggerPoint.transform.position, new Vector3(this.triggerPoint.transform.position.x, 5000f, 0f));
		Gizmos.DrawLine(this.triggerPoint2.transform.position, new Vector3(this.triggerPoint2.transform.position.x, 5000f, 0f));
		Gizmos.color = new Color(0f, 1f, 0f, 1f);
		Gizmos.DrawWireSphere(this.pointA.transform.position, 100f);
		Gizmos.DrawLine(this.pointA.transform.position, this.scrollingObject.transform.position);
	}

	// Token: 0x040026CB RID: 9931
	[SerializeField]
	public MountainPlatformingLevelMudmanSpawner mudmanSpawner;

	// Token: 0x040026CC RID: 9932
	[SerializeField]
	public Transform topBackground;

	// Token: 0x040026CD RID: 9933
	[SerializeField]
	public Transform bottomBackground;

	// Token: 0x040026CE RID: 9934
	[SerializeField]
	public float speed;

	// Token: 0x040026CF RID: 9935
	[SerializeField]
	public GameObject scrollingObject;

	// Token: 0x040026D0 RID: 9936
	[SerializeField]
	public Transform triggerPoint;

	// Token: 0x040026D1 RID: 9937
	[SerializeField]
	public Transform triggerPoint2;

	// Token: 0x040026D2 RID: 9938
	[SerializeField]
	public Transform pointA;

	// Token: 0x040026D3 RID: 9939
	[SerializeField]
	public GameObject invisibleWall;

	// Token: 0x040026D4 RID: 9940
	[SerializeField]
	public ScrollingBackgroundElevator cloudSprite;

	// Token: 0x040026D5 RID: 9941
	[SerializeField]
	public ScrollingBackgroundElevator foregroundSprite;

	// Token: 0x040026D6 RID: 9942
	[SerializeField]
	public ScrollingBackgroundElevator backgroundSprite;

	// Token: 0x040026D7 RID: 9943
	[SerializeField]
	public ScrollingBackgroundElevator[] midgroundSprites;

	// Token: 0x040026D8 RID: 9944
	[SerializeField]
	public float time;

	// Token: 0x040026D9 RID: 9945
	public float easeingTime;

	// Token: 0x040026DA RID: 9946
	public Coroutine cameraLockRoutine;
}
