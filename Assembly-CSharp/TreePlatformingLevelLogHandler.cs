using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003FA RID: 1018
public class TreePlatformingLevelLogHandler : AbstractPausableComponent
{
	// Token: 0x06002C9B RID: 11419 RVA: 0x000DA700 File Offset: 0x000D8900
	public void Start()
	{
		this.dropPosition = base.transform.position;
		this.SetupLogs();
		this.HP = this.maxHP;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002C9C RID: 11420 RVA: 0x000DA754 File Offset: 0x000D8954
	public void SetupLogs()
	{
		int num = 0;
		this.logs = new List<TreePlatformingLevelLog>();
		this.checkedLogs = new List<bool>(this.logOrder.Length);
		base.GetComponent<HitFlash>().otherRenderers = new SpriteRenderer[this.logOrder.Length];
		for (int i = 0; i < this.logOrder.Length; i++)
		{
			TreePlatformingLevelLog treePlatformingLevelLog = Object.Instantiate<TreePlatformingLevelLog>(this.logPrefabs[(int)this.logOrder[i]]);
			treePlatformingLevelLog.transform.position = new Vector3(base.transform.position.x, (float)Level.Current.Ceiling + 300f);
			treePlatformingLevelLog.transform.parent = base.transform;
			treePlatformingLevelLog.animator.SetBool("hasLegs", i == 0);
			treePlatformingLevelLog.GetComponent<SpriteRenderer>().sortingOrder = num++;
			treePlatformingLevelLog.GetComponent<DamageReceiver>().enabled = false;
			treePlatformingLevelLog.SetDirection(this.facingRight);
			this.logs.Add(treePlatformingLevelLog);
			this.checkedLogs.Add(false);
			base.GetComponent<HitFlash>().otherRenderers[i] = treePlatformingLevelLog.GetComponent<SpriteRenderer>();
			if (treePlatformingLevelLog.CanShoot)
			{
				this.shootableLogs++;
			}
		}
		this.amountToKillLog = this.maxHP / (float)this.logs.Count;
		base.StartCoroutine(this.check_to_start_cr());
	}

	// Token: 0x06002C9D RID: 11421 RVA: 0x000DA8C0 File Offset: 0x000D8AC0
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.HP -= info.damage;
		if (this.HP < this.amountToKillLog * (float)this.logs.Count)
		{
			if (this.HP > 0f)
			{
				this.KillLog();
			}
			else
			{
				if (this.logs.Count > 0 && this.logs[0] != null)
				{
					this.logs[0].KillLog();
				}
				Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x06002C9E RID: 11422 RVA: 0x000DA960 File Offset: 0x000D8B60
	public void KillLog()
	{
		if (this.logs.Count - 1 > 0)
		{
			this.logs[this.logs.Count - 1].KillLog();
			this.logs.RemoveAt(this.logs.Count - 1);
			if (this.checkedLogs.Count - 1 > 0)
			{
				this.checkedLogs.RemoveAt(this.logs.Count - 1);
			}
		}
		if (this.logs.Count > 0)
		{
			base.GetComponent<BoxCollider2D>().offset = new Vector2(0f, (this.logs[0].transform.localPosition.y + this.logs[this.logs.Count - 1].transform.localPosition.y) / 2f);
			base.GetComponent<BoxCollider2D>().size = new Vector2(this.logs[0].GetComponent<BoxCollider2D>().bounds.size.x, this.logs[0].GetComponent<BoxCollider2D>().bounds.size.y * (float)this.logs.Count);
		}
	}

	// Token: 0x06002C9F RID: 11423 RVA: 0x000DAAC0 File Offset: 0x000D8CC0
	public IEnumerator check_to_start_cr()
	{
		base.StartCoroutine(this.drop_logs_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002CA0 RID: 11424 RVA: 0x000DAADC File Offset: 0x000D8CDC
	public IEnumerator drop_logs_cr()
	{
		float t = 0f;
		float time = 0.4f;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (int i = 0; i < this.logs.Count; i++)
		{
			float offset = (i != 0) ? (this.logs[i].GetComponent<BoxCollider2D>().bounds.size.y / 2f + this.logs[i - 1].GetComponent<BoxCollider2D>().bounds.size.y / 2f) : 0f;
			float start = CupheadLevelCamera.Current.Bounds.yMax + 300f;
			float end = this.dropPosition.y + offset;
			while (t < time)
			{
				if (this.logs[i] != null)
				{
					t += CupheadTime.FixedDelta;
					float num = EaseUtils.Ease(EaseUtils.EaseType.punch, 0f, 1f, t / time);
					this.logs[i].transform.SetPosition(null, new float?(Mathf.Lerp(start, end, num)), null);
				}
				yield return wait;
			}
			this.effect.Create(new Vector3(this.logs[i].transform.position.x, this.logs[i].transform.position.y - this.logs[i].GetComponent<BoxCollider2D>().bounds.size.y / 2f));
			t = 0f;
			this.dropPosition = this.logs[i].transform.position;
			this.logs[i].start = this.logs[i].transform.position.y;
			yield return wait;
		}
		foreach (TreePlatformingLevelLog treePlatformingLevelLog in this.logs)
		{
			treePlatformingLevelLog.GetComponent<DamageReceiver>().enabled = true;
		}
		base.GetComponent<BoxCollider2D>().offset = new Vector2(0f, (this.logs[0].transform.localPosition.y + this.logs[this.logs.Count - 1].transform.localPosition.y) / 2f);
		base.GetComponent<BoxCollider2D>().size = new Vector2(this.logs[0].GetComponent<BoxCollider2D>().bounds.size.x, this.logs[0].GetComponent<BoxCollider2D>().bounds.size.y * (float)this.logs.Count);
		base.StartCoroutine(this.shoot_cr());
		yield break;
	}

	// Token: 0x06002CA1 RID: 11425 RVA: 0x000DAAF8 File Offset: 0x000D8CF8
	public IEnumerator check_to_slide_cr()
	{
		float amount = 0f;
		int indexToSlide = 1000;
		for (;;)
		{
			bool hasRemoved = false;
			int i = 0;
			while (i < this.logs.Count)
			{
				if (this.logs[i].isDying && !hasRemoved)
				{
					amount = this.logs[i].GetComponent<BoxCollider2D>().bounds.size.y;
					if (this.logs[i].CanShoot)
					{
						this.shootableLogs--;
					}
					this.logs.RemoveAt(i);
					this.checkedLogs.RemoveAt(i);
					indexToSlide = i;
					hasRemoved = true;
				}
				else
				{
					if (i >= indexToSlide)
					{
						while (this.logs[i].isSliding)
						{
							yield return null;
						}
						if (this.logs[i] != null)
						{
							this.logs[i].SlideDown(amount);
						}
					}
					if (i == this.logs.Count - 1)
					{
						indexToSlide = 1000;
					}
					i++;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002CA2 RID: 11426 RVA: 0x000DAB14 File Offset: 0x000D8D14
	public IEnumerator shoot_cr()
	{
		int rand = 0;
		while (this.shootableLogs > 0 && this.logs.Count > 0)
		{
			yield return CupheadTime.WaitForSeconds(this, this.logs[rand].ShootDelay);
			for (int i = 0; i < this.checkedLogs.Count; i++)
			{
				this.checkedLogs[i] = false;
			}
			while (this.checkedLogs.Contains(false))
			{
				rand = Random.Range(0, this.checkedLogs.Count);
				if (this.logs[rand].CanShoot && !this.checkedLogs[rand])
				{
					AudioManager.Play("level_platform_logface_attack");
					this.emitAudioFromObject.Add("level_platform_logface_attack");
					this.logs[rand].OnShoot();
					break;
				}
				this.checkedLogs[rand] = true;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002CA3 RID: 11427 RVA: 0x000DAB30 File Offset: 0x000D8D30
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		float num = 1000f;
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(new Vector3(base.transform.position.x, base.transform.position.y + num / 2f), new Vector3(this.logPrefabs[0].GetComponent<SpriteRenderer>().bounds.size.x, num, 0f));
	}

	// Token: 0x040024C7 RID: 9415
	[SerializeField]
	public float maxHP;

	// Token: 0x040024C8 RID: 9416
	public float HP;

	// Token: 0x040024C9 RID: 9417
	public float amountToKillLog;

	// Token: 0x040024CA RID: 9418
	[SerializeField]
	public bool facingRight;

	// Token: 0x040024CB RID: 9419
	[Header("SET UP LOGS HERE:")]
	[SerializeField]
	public TreePlatformingLevelLogHandler.LogTypes[] logOrder;

	// Token: 0x040024CC RID: 9420
	[Header("DON'T TOUCH THIS:")]
	[SerializeField]
	public TreePlatformingLevelLog[] logPrefabs;

	// Token: 0x040024CD RID: 9421
	[SerializeField]
	public Effect effect;

	// Token: 0x040024CE RID: 9422
	public List<TreePlatformingLevelLog> logs;

	// Token: 0x040024CF RID: 9423
	public Vector3 dropPosition;

	// Token: 0x040024D0 RID: 9424
	public int shootableLogs;

	// Token: 0x040024D1 RID: 9425
	public int logsKilled;

	// Token: 0x040024D2 RID: 9426
	public List<bool> checkedLogs;

	// Token: 0x040024D3 RID: 9427
	public DamageDealer damageDealer;

	// Token: 0x040024D4 RID: 9428
	public DamageReceiver damageReceiver;

	// Token: 0x02001035 RID: 4149
	public enum LogTypes
	{
		// Token: 0x0400739C RID: 29596
		A,
		// Token: 0x0400739D RID: 29597
		B,
		// Token: 0x0400739E RID: 29598
		C,
		// Token: 0x0400739F RID: 29599
		D,
		// Token: 0x040073A0 RID: 29600
		E,
		// Token: 0x040073A1 RID: 29601
		F
	}
}
