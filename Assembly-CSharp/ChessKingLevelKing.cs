using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000183 RID: 387
public class ChessKingLevelKing : LevelProperties.ChessKing.Entity
{
	// Token: 0x1700024A RID: 586
	// (get) Token: 0x0600124F RID: 4687 RVA: 0x0000F75D File Offset: 0x0000D95D
	// (set) Token: 0x06001250 RID: 4688 RVA: 0x0000F765 File Offset: 0x0000D965
	public bool GOT_PARRIED { get; set; }

	// Token: 0x06001251 RID: 4689 RVA: 0x00094C14 File Offset: 0x00092E14
	public void StartGame()
	{
		this.rats = new List<ChessKingLevelRat>();
		base.StartCoroutine(this.timer_cr());
		LevelProperties.ChessKing.King king = base.properties.CurrentState.king;
		this.trialPoolMainIndex = Random.Range(0, base.properties.CurrentState.king.trialPool.Length);
		this.kingAttackStringMainIndex = Random.Range(0, king.kingAttackString.Length);
		string[] array = king.kingAttackString[this.kingAttackStringMainIndex].Split(new char[]
		{
			','
		});
		this.kingAttackStringIndex = Random.Range(0, array.Length);
		base.StartCoroutine(this.create_trial_cr());
	}

	// Token: 0x06001252 RID: 4690 RVA: 0x0000F76E File Offset: 0x0000D96E
	public void Update()
	{
	}

	// Token: 0x06001253 RID: 4691 RVA: 0x0000F770 File Offset: 0x0000D970
	public override void LevelInit(LevelProperties.ChessKing properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06001254 RID: 4692 RVA: 0x00094CBC File Offset: 0x00092EBC
	public void StateChange()
	{
		LevelProperties.ChessKing.King king = base.properties.CurrentState.king;
		this.trialPoolMainIndex = Random.Range(0, base.properties.CurrentState.king.trialPool.Length);
		this.kingAttackStringMainIndex = Random.Range(0, king.kingAttackString.Length);
		string[] array = king.kingAttackString[this.kingAttackStringMainIndex].Split(new char[]
		{
			','
		});
		this.kingAttackStringIndex = Random.Range(0, array.Length);
	}

	// Token: 0x06001255 RID: 4693 RVA: 0x0000F779 File Offset: 0x0000D979
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		this.GOT_PARRIED = true;
		base.properties.DealDamage((!PlayerManager.BothPlayersActive()) ? 10f : ChessKingLevelKing.multiplayerDamageNerf);
	}

	// Token: 0x06001256 RID: 4694 RVA: 0x0000F7AD File Offset: 0x0000D9AD
	public void BecomeParryable()
	{
		this.GOT_PARRIED = false;
		base.animator.Play("Parryable");
	}

	// Token: 0x06001257 RID: 4695 RVA: 0x00094D40 File Offset: 0x00092F40
	public IEnumerator create_trial_cr()
	{
		this.parryPoints = new List<ChessKingLevelParryPoint>();
		LevelProperties.ChessKing.King p = base.properties.CurrentState.king;
		string[] trial = p.trialPool[this.trialPoolMainIndex].Split(new char[]
		{
			','
		});
		this.GOT_PARRIED = false;
		for (int j = 0; j < trial.Length; j++)
		{
			string[] array = trial[j].Split(new char[]
			{
				':'
			});
			Vector3 dir = Vector3.zero;
			float num = 0f;
			float num2 = 0f;
			float amount = 0f;
			bool flag = false;
			for (int k = 0; k < array.Length; k++)
			{
				switch (k)
				{
				case 0:
					Parser.FloatTryParse(array[k], out num);
					break;
				case 1:
					Parser.FloatTryParse(array[k], out num2);
					break;
				case 2:
					flag = true;
					dir = this.GetDir(array[k]);
					break;
				case 3:
					Parser.FloatTryParse(array[k], out amount);
					break;
				}
			}
			ChessKingLevelParryPoint chessKingLevelParryPoint = Object.Instantiate<ChessKingLevelParryPoint>(this.parryPoint);
			Vector3 pos = new Vector3((float)Level.Current.Left, (float)Level.Current.Ground) + new Vector3(num, num2);
			if (flag)
			{
				chessKingLevelParryPoint.Init(pos, dir, p.bluePointSpeed, amount);
			}
			else
			{
				chessKingLevelParryPoint.Init(pos);
			}
			this.parryPoints.Add(chessKingLevelParryPoint);
		}
		for (int i = 0; i < this.parryPoints.Count; i++)
		{
			this.parryPoints[i].Activate();
			while (!this.parryPoints[i].GOT_PARRIED)
			{
				if (this.groundTrigger.PLAYER_FALLEN)
				{
					break;
				}
				yield return null;
			}
			if (!this.challengeActivated)
			{
				this.groundTrigger.CheckPlayer(true);
				this.MoveBluePoints();
				this.challengeActivated = true;
			}
		}
		this.EndChallenge();
		yield return null;
		yield break;
	}

	// Token: 0x06001258 RID: 4696 RVA: 0x0000F7C6 File Offset: 0x0000D9C6
	public void EndChallenge()
	{
		base.StartCoroutine(this.end_challenge());
	}

	// Token: 0x06001259 RID: 4697 RVA: 0x00094D5C File Offset: 0x00092F5C
	public IEnumerator end_challenge()
	{
		if (!this.groundTrigger.PLAYER_FALLEN)
		{
			this.BecomeParryable();
			while (!this.GOT_PARRIED)
			{
				if (this.groundTrigger.PLAYER_FALLEN)
				{
					break;
				}
				yield return null;
			}
		}
		this.challengeActivated = false;
		base.animator.Play("Idle");
		this.groundTrigger.CheckPlayer(false);
		this.ClearPoints();
		if (!this.GOT_PARRIED)
		{
			this.Attack();
		}
		while (this.isAttacking)
		{
			yield return null;
		}
		LevelPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne) as LevelPlayerController;
		while (!player.motor.Grounded)
		{
			yield return null;
		}
		this.trialPoolMainIndex = (this.trialPoolMainIndex + 1) % base.properties.CurrentState.king.trialPool.Length;
		base.StartCoroutine(this.create_trial_cr());
		yield break;
	}

	// Token: 0x0600125A RID: 4698 RVA: 0x00094D78 File Offset: 0x00092F78
	public void ClearPoints()
	{
		for (int i = 0; i < this.parryPoints.Count; i++)
		{
			Object.Destroy(this.parryPoints[i].gameObject);
		}
		this.parryPoints.Clear();
	}

	// Token: 0x0600125B RID: 4699 RVA: 0x00094DC4 File Offset: 0x00092FC4
	public void MoveBluePoints()
	{
		for (int i = 0; i < this.parryPoints.Count; i++)
		{
			this.parryPoints[i].MovePoint();
		}
	}

	// Token: 0x0600125C RID: 4700 RVA: 0x00094E00 File Offset: 0x00093000
	public IEnumerator timer_cr()
	{
		float t = 0f;
		float time = base.properties.CurrentState.king.kingAttackTimer;
		for (;;)
		{
			if (!this.challengeActivated && !this.isAttacking)
			{
				if (t < time)
				{
					t += CupheadTime.Delta;
				}
				else
				{
					this.Attack();
					t = 0f;
				}
			}
			else
			{
				t = 0f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600125D RID: 4701 RVA: 0x00094E1C File Offset: 0x0009301C
	public Vector3 GetDir(string part)
	{
		if (part[0] == 'R')
		{
			return Vector3.right;
		}
		if (part[0] == 'L')
		{
			return Vector3.left;
		}
		if (part[0] == 'U')
		{
			return Vector3.up;
		}
		return Vector3.down;
	}

	// Token: 0x0600125E RID: 4702 RVA: 0x00094E6C File Offset: 0x0009306C
	public void Attack()
	{
		this.isAttacking = true;
		LevelProperties.ChessKing.King king = base.properties.CurrentState.king;
		string[] array = king.kingAttackString[this.kingAttackStringMainIndex].Split(new char[]
		{
			','
		});
		string text = array[this.kingAttackStringIndex];
		if (text != null)
		{
			if (!(text == "F"))
			{
				if (!(text == "B"))
				{
					if (text == "R")
					{
						base.StartCoroutine(this.rat_attack_cr());
					}
				}
				else
				{
					base.StartCoroutine(this.beam_attack_cr());
				}
			}
			else
			{
				base.StartCoroutine(this.full_screen_attack_cr());
			}
		}
		if (this.kingAttackStringIndex < array.Length - 1)
		{
			this.kingAttackStringIndex++;
		}
		else
		{
			this.kingAttackStringMainIndex = (this.kingAttackStringMainIndex + 1) % king.kingAttackString.Length;
			this.kingAttackStringIndex = 0;
		}
	}

	// Token: 0x0600125F RID: 4703 RVA: 0x00094F6C File Offset: 0x0009316C
	public IEnumerator full_screen_attack_cr()
	{
		LevelProperties.ChessKing.Full p = base.properties.CurrentState.full;
		base.animator.SetBool("isAnti", true);
		yield return CupheadTime.WaitForSeconds(this, p.fullAttackAnti);
		this.fullAttack.SetActive(true);
		yield return CupheadTime.WaitForSeconds(this, p.fullAttackDuration);
		this.fullAttack.SetActive(false);
		base.animator.SetBool("isAnti", false);
		yield return CupheadTime.WaitForSeconds(this, p.fullAttackRecovery);
		this.isAttacking = false;
		yield return null;
		yield break;
	}

	// Token: 0x06001260 RID: 4704 RVA: 0x00094F88 File Offset: 0x00093188
	public IEnumerator beam_attack_cr()
	{
		LevelProperties.ChessKing.Beam p = base.properties.CurrentState.beam;
		base.animator.SetBool("isAnti", true);
		yield return CupheadTime.WaitForSeconds(this, p.beamAttackAnti);
		this.beamAttack.SetActive(true);
		yield return CupheadTime.WaitForSeconds(this, p.beamAttackDuration);
		this.beamAttack.SetActive(false);
		base.animator.SetBool("isAnti", false);
		yield return CupheadTime.WaitForSeconds(this, p.beamAttackRecovery);
		this.isAttacking = false;
		yield return null;
		yield break;
	}

	// Token: 0x06001261 RID: 4705 RVA: 0x00094FA4 File Offset: 0x000931A4
	public IEnumerator rat_attack_cr()
	{
		LevelProperties.ChessKing.Rat p = base.properties.CurrentState.rat;
		base.animator.SetBool("isAnti", true);
		yield return CupheadTime.WaitForSeconds(this, p.ratSummonAnti);
		if (this.rats.Count < p.maxRatNumber)
		{
			ChessKingLevelRat chessKingLevelRat = Object.Instantiate<ChessKingLevelRat>(this.ratPrefab);
			chessKingLevelRat.Init(this.ratSpawn.position, p.ratSpeed);
			this.rats.Add(chessKingLevelRat);
		}
		yield return CupheadTime.WaitForSeconds(this, p.ratSummonDuration);
		base.animator.SetBool("isAnti", false);
		yield return CupheadTime.WaitForSeconds(this, p.ratSummonRecovery);
		this.isAttacking = false;
		yield return null;
		yield break;
	}

	// Token: 0x04000ED1 RID: 3793
	public static float multiplayerDamageNerf = 8f;

	// Token: 0x04000ED2 RID: 3794
	public const float Y_SPAWN = -300f;

	// Token: 0x04000ED3 RID: 3795
	[SerializeField]
	public ChessKingLevelRat ratPrefab;

	// Token: 0x04000ED4 RID: 3796
	public List<ChessKingLevelRat> rats;

	// Token: 0x04000ED5 RID: 3797
	[SerializeField]
	public Transform ratSpawn;

	// Token: 0x04000ED6 RID: 3798
	[SerializeField]
	public GameObject beamAttack;

	// Token: 0x04000ED7 RID: 3799
	[SerializeField]
	public GameObject fullAttack;

	// Token: 0x04000ED8 RID: 3800
	[SerializeField]
	public ChessKingLevelGroundTrigger groundTrigger;

	// Token: 0x04000ED9 RID: 3801
	[SerializeField]
	public ChessKingLevelParryPoint parryPoint;

	// Token: 0x04000EDA RID: 3802
	public List<ChessKingLevelParryPoint> parryPoints;

	// Token: 0x04000EDC RID: 3804
	public int kingAttackStringMainIndex;

	// Token: 0x04000EDD RID: 3805
	public int kingAttackStringIndex;

	// Token: 0x04000EDE RID: 3806
	public int trialPoolMainIndex;

	// Token: 0x04000EDF RID: 3807
	public bool challengeActivated;

	// Token: 0x04000EE0 RID: 3808
	public bool isAttacking;
}
