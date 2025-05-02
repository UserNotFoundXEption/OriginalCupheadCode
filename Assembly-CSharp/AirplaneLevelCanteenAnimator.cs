using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000126 RID: 294
public class AirplaneLevelCanteenAnimator : LevelProperties.Airplane.Entity
{
	// Token: 0x06000DF9 RID: 3577 RVA: 0x0000BEBC File Offset: 0x0000A0BC
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06000DFA RID: 3578 RVA: 0x000887D0 File Offset: 0x000869D0
	public override void LevelInit(LevelProperties.Airplane properties)
	{
		base.LevelInit(properties);
		this.level = (Level.Current as AirplaneLevel);
		this.curState = properties.CurrentState.stateName;
		this.idleLoops = Random.Range(3, 6);
		base.StartCoroutine(this.check_players_cr());
		base.StartCoroutine(this.handle_canteen_cr());
	}

	// Token: 0x06000DFB RID: 3579 RVA: 0x0008882C File Offset: 0x00086A2C
	public IEnumerator check_players_cr()
	{
		while (this.p1health == -1)
		{
			this.player1 = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			this.player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			this.p1health = ((!this.player1) ? -1 : PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health);
			this.p2health = ((!this.player2) ? -1 : PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000DFC RID: 3580 RVA: 0x00088848 File Offset: 0x00086A48
	public void OnPlayerHit(bool dead)
	{
		base.animator.Play((!dead) ? ((!this.playerHitAltAnim) ? "CanteenHitB" : "CanteenHitA") : "CanteenOnePlayerDied");
		base.animator.SetBool("CanteenTrackBoss", false);
		if (!dead)
		{
			this.playerHitAltAnim = !this.playerHitAltAnim;
		}
	}

	// Token: 0x06000DFD RID: 3581 RVA: 0x0000BECA File Offset: 0x0000A0CA
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		if (!Level.Won)
		{
			base.animator.SetBool("CanteenTrackBoss", false);
			base.animator.Play("CanteenAllPlayersDied");
		}
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x000888B0 File Offset: 0x00086AB0
	public IEnumerator handle_canteen_cr()
	{
		for (;;)
		{
			if (Level.Won)
			{
				base.animator.SetBool("CanteenTrackBoss", false);
				base.animator.Play("CanteenWin");
			}
			else if (this.triggerCheer)
			{
				base.animator.SetBool("CanteenTrackBoss", false);
				base.animator.Play("CanteenCheer");
				this.triggerCheer = false;
			}
			else
			{
				this.player1 = PlayerManager.GetPlayer(PlayerId.PlayerOne);
				this.player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
				if (this.player1)
				{
					if (PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health < this.p1health)
					{
						this.p1health = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health;
						this.OnPlayerHit(this.p1health == 0);
					}
					else if (PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health > this.p1health)
					{
						base.animator.SetBool("CanteenTrackBoss", false);
						base.animator.Play("CanteenCheer");
					}
					this.p1health = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health;
				}
				if (this.player2)
				{
					if (PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health < this.p2health)
					{
						this.p2health = PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health;
						this.OnPlayerHit(this.p2health == 0);
					}
					else if (PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health > this.p2health)
					{
						base.animator.SetBool("CanteenTrackBoss", false);
						base.animator.Play("CanteenCheer");
					}
					this.p2health = PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000DFF RID: 3583 RVA: 0x000888CC File Offset: 0x00086ACC
	public void LookAtBoss()
	{
		switch (base.properties.CurrentState.stateName)
		{
		case LevelProperties.Airplane.States.Main:
		case LevelProperties.Airplane.States.Generic:
		case LevelProperties.Airplane.States.Rocket:
			if (this.level.CurrentEnemyPos().x - base.transform.position.x < -250f)
			{
				base.animator.Play("CanteenLookUpLeft");
				base.animator.SetInteger("CanteenLookUpDir", -1);
				base.animator.SetBool("CanteenTrackBoss", true);
			}
			else if (this.level.CurrentEnemyPos().x - base.transform.position.x > 250f)
			{
				base.animator.Play("CanteenLookUpRight");
				base.animator.SetInteger("CanteenLookUpDir", 1);
				base.animator.SetBool("CanteenTrackBoss", true);
			}
			else
			{
				base.animator.Play("CanteenLookUp");
				base.animator.SetInteger("CanteenLookUpDir", 0);
				base.animator.SetBool("CanteenTrackBoss", true);
			}
			break;
		case LevelProperties.Airplane.States.Terriers:
			base.animator.SetBool("CanteenTrackBoss", false);
			switch (Random.Range(0, 6))
			{
			case 0:
				base.animator.Play("CanteenLookUpLeft");
				break;
			case 1:
				base.animator.Play("CanteenLookUp");
				break;
			case 2:
				base.animator.Play("CanteenLookUpRight");
				break;
			case 3:
				base.animator.Play("CanteenLookDownRight");
				break;
			case 4:
				base.animator.Play("CanteenLookDown");
				break;
			case 5:
				base.animator.Play("CanteenLookDownLeft");
				break;
			}
			break;
		case LevelProperties.Airplane.States.Leader:
			if (this.level.ScreenHorizontal())
			{
				if (this.level.CurrentEnemyPos().x - base.transform.position.x < -100f)
				{
					base.animator.Play("CanteenLookUpLeft");
					base.animator.SetInteger("CanteenLookUpDir", -1);
					base.animator.SetBool("CanteenTrackBoss", true);
				}
				else if (this.level.CurrentEnemyPos().x - base.transform.position.x > 100f)
				{
					base.animator.Play("CanteenLookUpRight");
					base.animator.SetInteger("CanteenLookUpDir", 1);
					base.animator.SetBool("CanteenTrackBoss", true);
				}
				else
				{
					base.animator.Play("CanteenLookUp");
					base.animator.SetInteger("CanteenLookUpDir", 0);
					base.animator.SetBool("CanteenTrackBoss", true);
				}
			}
			else
			{
				int num = Random.Range(0, 2);
				if (num != 0)
				{
					if (num == 1)
					{
						base.animator.Play("CanteenLookUpRight");
					}
				}
				else
				{
					base.animator.Play("CanteenLookUpLeft");
				}
			}
			break;
		}
		this.lookLoops = Random.Range(7, 9);
	}

	// Token: 0x06000E00 RID: 3584 RVA: 0x0000BEFD File Offset: 0x0000A0FD
	public void ForceLook(Vector3 target, int loops)
	{
		this.lookLoops = loops;
		this.idleLoops = 1;
		this.idleClipPos = -1;
		this.forceLookTarget = target;
	}

	// Token: 0x06000E01 RID: 3585 RVA: 0x00088C48 File Offset: 0x00086E48
	public void LookInDirection()
	{
		base.animator.SetBool("CanteenTrackBoss", false);
		switch ((int)(((double)Vector3.SignedAngle(Vector3.up, this.forceLookTarget - base.transform.position, Vector3.back) + 202.5) % 360.0) / 45)
		{
		case 0:
			base.animator.Play("CanteenLookDown");
			break;
		case 1:
			base.animator.Play("CanteenLookDownLeft");
			break;
		case 2:
		case 3:
			base.animator.Play("CanteenLookUpLeft");
			break;
		case 4:
			base.animator.Play("CanteenLookUp");
			break;
		case 5:
		case 6:
			base.animator.Play("CanteenLookUpRight");
			break;
		case 7:
			base.animator.Play("CanteenLookDownRight");
			break;
		}
	}

	// Token: 0x06000E02 RID: 3586 RVA: 0x00088D50 File Offset: 0x00086F50
	public void OnCanteenIdleLoop()
	{
		this.idleLoops--;
		if (this.idleLoops == 0)
		{
			int num = this.idleClipPos;
			switch (num + 1)
			{
			case 0:
				this.LookInDirection();
				base.animator.SetBool("CanteenTrackBoss", false);
				break;
			case 1:
				base.animator.SetTrigger("CanteenBlink");
				base.animator.SetBool("CanteenTrackBoss", false);
				break;
			case 2:
				if (Random.Range(0, (base.properties.CurrentState.stateName != LevelProperties.Airplane.States.Terriers) ? 10 : 4) == 0)
				{
					this.LookAtBoss();
				}
				else
				{
					base.animator.SetTrigger("CanteenGlanceAround");
					base.animator.SetBool("CanteenTrackBoss", false);
				}
				break;
			case 3:
				base.animator.SetTrigger("CanteenBlink");
				base.animator.SetBool("CanteenTrackBoss", false);
				break;
			case 4:
				this.LookAtBoss();
				break;
			}
			this.idleClipPos = (this.idleClipPos + 1) % 4;
			this.idleLoops = Random.Range(3, 6);
		}
	}

	// Token: 0x06000E03 RID: 3587 RVA: 0x0000BF1B File Offset: 0x0000A11B
	public void OnCanteenLookLoop()
	{
		this.lookLoops--;
		if (this.lookLoops <= 0)
		{
			base.animator.SetTrigger("CanteenEndLookLoop");
		}
	}

	// Token: 0x06000E04 RID: 3588 RVA: 0x00088E88 File Offset: 0x00087088
	public void Update()
	{
		if (base.animator.GetBool("CanteenTrackBoss"))
		{
			switch (base.properties.CurrentState.stateName)
			{
			case LevelProperties.Airplane.States.Main:
			case LevelProperties.Airplane.States.Generic:
			case LevelProperties.Airplane.States.Rocket:
				if (this.level.CurrentEnemyPos().x - base.transform.position.x < -250f)
				{
					base.animator.SetInteger("CanteenLookUpDir", -1);
				}
				else if (this.level.CurrentEnemyPos().x - base.transform.position.x > 250f)
				{
					base.animator.SetInteger("CanteenLookUpDir", 1);
				}
				else
				{
					base.animator.SetInteger("CanteenLookUpDir", 0);
				}
				break;
			case LevelProperties.Airplane.States.Leader:
				if (this.level.ScreenHorizontal())
				{
					if (this.level.CurrentEnemyPos().x - base.transform.position.x < -100f)
					{
						base.animator.SetInteger("CanteenLookUpDir", -1);
					}
					else if (this.level.CurrentEnemyPos().x - base.transform.position.x > 100f)
					{
						base.animator.SetInteger("CanteenLookUpDir", 1);
					}
					else
					{
						base.animator.SetInteger("CanteenLookUpDir", 0);
					}
				}
				break;
			}
		}
	}

	// Token: 0x06000E05 RID: 3589 RVA: 0x0000BF47 File Offset: 0x0000A147
	public void WORKAROUND_NullifyFields()
	{
		this.player1 = null;
		this.player2 = null;
		this.level = null;
	}

	// Token: 0x04000B03 RID: 2819
	public const int MIN_IDLE_LOOPS = 3;

	// Token: 0x04000B04 RID: 2820
	public const int MAX_IDLE_LOOPS = 6;

	// Token: 0x04000B05 RID: 2821
	public const int MIN_LOOK_LOOPS = 7;

	// Token: 0x04000B06 RID: 2822
	public const int MAX_LOOK_LOOPS = 9;

	// Token: 0x04000B07 RID: 2823
	public const float PHASE_ONE_LOOK_ANGLE_THRESHOLD = 250f;

	// Token: 0x04000B08 RID: 2824
	public const float PHASE_THREE_LOOK_ANGLE_THRESHOLD = 100f;

	// Token: 0x04000B09 RID: 2825
	public int idleLoops;

	// Token: 0x04000B0A RID: 2826
	public int idleClipPos;

	// Token: 0x04000B0B RID: 2827
	public int lookLoops;

	// Token: 0x04000B0C RID: 2828
	public Vector3 forceLookTarget;

	// Token: 0x04000B0D RID: 2829
	public AbstractPlayerController player1;

	// Token: 0x04000B0E RID: 2830
	public AbstractPlayerController player2;

	// Token: 0x04000B0F RID: 2831
	public int p1health = -1;

	// Token: 0x04000B10 RID: 2832
	public int p2health = -1;

	// Token: 0x04000B11 RID: 2833
	public bool playerHitAltAnim;

	// Token: 0x04000B12 RID: 2834
	public bool triggerCheer;

	// Token: 0x04000B13 RID: 2835
	public LevelProperties.Airplane.States curState;

	// Token: 0x04000B14 RID: 2836
	public AirplaneLevel level;
}
