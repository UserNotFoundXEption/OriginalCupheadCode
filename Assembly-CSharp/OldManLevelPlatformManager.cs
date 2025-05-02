using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002E1 RID: 737
public class OldManLevelPlatformManager : LevelProperties.OldMan.Entity
{
	// Token: 0x060020B4 RID: 8372 RVA: 0x000B88D0 File Offset: 0x000B6AD0
	public Vector3[] GetPlatformPositions()
	{
		Vector3[] array = new Vector3[this.allPlatforms.Length];
		for (int i = 0; i < this.allPlatforms.Length; i++)
		{
			array[i] = this.allPlatforms[i].platform.position;
		}
		return array;
	}

	// Token: 0x060020B5 RID: 8373 RVA: 0x0001BD92 File Offset: 0x00019F92
	public Transform GetPlatform(int i)
	{
		if (this.allPlatforms[i] == null)
		{
			return null;
		}
		return this.allPlatforms[i].platform.transform;
	}

	// Token: 0x060020B6 RID: 8374 RVA: 0x0001BDB5 File Offset: 0x00019FB5
	public bool PlatformRemoved(int which)
	{
		return this.allPlatforms[which].removed;
	}

	// Token: 0x060020B7 RID: 8375 RVA: 0x000B8924 File Offset: 0x000B6B24
	public override void LevelInit(LevelProperties.OldMan properties)
	{
		base.LevelInit(properties);
		for (int i = 0; i < this.allPlatforms.Length; i++)
		{
			this.allPlatforms[i].platform.SetPosition(null, new float?(properties.CurrentState.platforms.minHeight), null);
		}
		base.StartCoroutine(this.handle_platforms_cr());
		base.StartCoroutine(this.handle_remove_platforms_cr());
	}

	// Token: 0x060020B8 RID: 8376 RVA: 0x0001BDC4 File Offset: 0x00019FC4
	public void EndPhase()
	{
		this.inPhaseOne = false;
		this.mainBeardTufts.enabled = false;
		this.beardSettles[0].transform.parent.gameObject.SetActive(true);
	}

	// Token: 0x060020B9 RID: 8377 RVA: 0x000B89A4 File Offset: 0x000B6BA4
	public IEnumerator handle_remove_platforms_cr()
	{
		float bossHealthMax = base.properties.CurrentHealth;
		float bossHealthMin = bossHealthMax * base.properties.GetNextStateHealthTrigger();
		int currentCount = 0;
		string[] removeOrder = base.properties.CurrentState.platforms.removeOrder[Random.Range(0, base.properties.CurrentState.platforms.removeOrder.Length)].Split(new char[]
		{
			','
		});
		string[] removeThreshold = base.properties.CurrentState.platforms.removeThreshold.Split(new char[]
		{
			','
		});
		if (removeOrder.Length != removeThreshold.Length)
		{
			Debug.Break();
		}
		if (removeOrder.Length == 0)
		{
			yield break;
		}
		for (int i = 0; i < removeOrder.Length; i++)
		{
			int item = 0;
			Parser.IntTryParse(removeOrder[i], out item);
			float item2 = 0f;
			Parser.FloatTryParse(removeThreshold[i], out item2);
			this.removeOrderList.Add(item);
			this.removeThresholdList.Add(item2);
		}
		while (currentCount < this.removeThresholdList.Count)
		{
			float t = Mathf.InverseLerp(bossHealthMax, bossHealthMin, base.properties.CurrentHealth);
			if (t > this.removeThresholdList[currentCount])
			{
				this.RemovePlatform(this.removeOrderList[currentCount]);
				currentCount++;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060020BA RID: 8378 RVA: 0x000B89C0 File Offset: 0x000B6BC0
	public IEnumerator handle_platforms_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		LevelProperties.OldMan.Platforms p = base.properties.CurrentState.platforms;
		int orderMainIndex = Random.Range(0, p.moveOrder.Length);
		string[] orderString = p.moveOrder[orderMainIndex].Split(new char[]
		{
			','
		});
		int orderIndex = Random.Range(0, orderString.Length);
		bool skipPlatform = false;
		bool stoppedMoving = false;
		while (!stoppedMoving)
		{
			if (!this.inPhaseOne)
			{
				stoppedMoving = true;
				yield return null;
			}
			skipPlatform = false;
			orderString = p.moveOrder[orderMainIndex].Split(new char[]
			{
				','
			});
			string[] spawnOrder = orderString[orderIndex].Split(new char[]
			{
				'-'
			});
			foreach (string s in spawnOrder)
			{
				int num = 0;
				Parser.IntTryParse(s, out num);
				if (this.allPlatforms[num].isMoving)
				{
					skipPlatform = true;
				}
				else
				{
					base.StartCoroutine(this.move_platform_cr(this.allPlatforms[num]));
				}
			}
			if (!skipPlatform)
			{
				yield return CupheadTime.WaitForSeconds(this, p.delayRange.RandomFloat());
			}
			if (orderIndex < orderString.Length - 1)
			{
				orderIndex++;
			}
			else
			{
				orderMainIndex = (orderMainIndex + 1) % p.moveOrder.Length;
				orderIndex = 0;
			}
			yield return null;
		}
		base.StartCoroutine(this.end_phase_cr());
		yield break;
	}

	// Token: 0x060020BB RID: 8379 RVA: 0x000B89DC File Offset: 0x000B6BDC
	public IEnumerator end_phase_cr()
	{
		List<int> order = new List<int>(5);
		for (int j = 0; j < 5; j++)
		{
			if (!this.allPlatforms[j].removed)
			{
				order.Add(j);
			}
		}
		for (int k = 0; k < order.Count; k++)
		{
			int value = order[k];
			int index = Random.Range(0, order.Count);
			order[k] = order[index];
			order[index] = value;
		}
		for (int i = 0; i < order.Count; i++)
		{
			base.StartCoroutine(this.slide_out_cr(this.allPlatforms[order[i]]));
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		yield break;
	}

	// Token: 0x060020BC RID: 8380 RVA: 0x000B89F8 File Offset: 0x000B6BF8
	public void RemovePlatform(int which)
	{
		this.allPlatforms[which].removed = true;
		this.mainBeardTufts.enabled = false;
		this.beardSettles[0].transform.parent.gameObject.SetActive(true);
		base.StartCoroutine(this.slide_out_cr(this.allPlatforms[which]));
	}

	// Token: 0x060020BD RID: 8381 RVA: 0x0001BDF6 File Offset: 0x00019FF6
	public void AttachGnome(int which, OldManLevelGnomeClimber c)
	{
		this.allPlatforms[which].activeClimber = c;
	}

	// Token: 0x060020BE RID: 8382 RVA: 0x000B8A54 File Offset: 0x000B6C54
	public IEnumerator move_platform_cr(OldManLevelPlatform movingPlatform)
	{
		LevelProperties.OldMan.Platforms p = base.properties.CurrentState.platforms;
		float t = 0f;
		float time = p.moveTime / 2f;
		movingPlatform.isMoving = true;
		while (t < time && this.inPhaseOne && !movingPlatform.removed)
		{
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			float lastPos = movingPlatform.platform.transform.position.y;
			movingPlatform.platform.SetPosition(null, new float?(Mathf.Lerp(p.minHeight, p.maxHeight, val)), null);
			movingPlatform.effectiveVel = movingPlatform.platform.transform.position.y - lastPos;
			yield return null;
		}
		if (this.inPhaseOne && !movingPlatform.removed)
		{
			t = 0f;
			movingPlatform.platform.SetPosition(null, new float?(p.maxHeight), null);
		}
		while (t < time && this.inPhaseOne && !movingPlatform.removed)
		{
			t += CupheadTime.Delta;
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			float lastPos2 = movingPlatform.platform.transform.position.y;
			movingPlatform.platform.SetPosition(null, new float?(Mathf.Lerp(p.maxHeight, p.minHeight, val2)), null);
			movingPlatform.effectiveVel = movingPlatform.platform.transform.position.y - lastPos2;
			yield return null;
		}
		if (this.inPhaseOne && !movingPlatform.removed)
		{
			movingPlatform.platform.SetPosition(null, new float?(p.minHeight), null);
			movingPlatform.isMoving = false;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060020BF RID: 8383 RVA: 0x000B8A78 File Offset: 0x000B6C78
	public IEnumerator slide_out_cr(OldManLevelPlatform movingPlatform)
	{
		LevelProperties.OldMan.Platforms p = base.properties.CurrentState.platforms;
		int id = 4 - Array.IndexOf<OldManLevelPlatform>(this.allPlatforms, movingPlatform);
		YieldInstruction wait = new WaitForFixedUpdate();
		float moveHeight = p.minHeight * 2f - 50f;
		if (movingPlatform.effectiveVel > 0f)
		{
			movingPlatform.effectiveVel *= 0.5f;
		}
		float t = this.wobbleBeforeRemoveTime;
		while (t > 0f || movingPlatform.activeClimber)
		{
			t -= CupheadTime.Delta;
			movingPlatform.platform.transform.GetChild(0).transform.GetChild(0).localPosition = new Vector3(Mathf.Sin(t * 100f) * 2.5f, 0f);
			yield return null;
		}
		movingPlatform.platform.transform.GetChild(0).transform.GetChild(0).localPosition = Vector3.zero;
		while (movingPlatform.platform.transform.position.y > moveHeight)
		{
			if (!CupheadTime.IsPaused())
			{
				movingPlatform.platform.SetPosition(null, new float?(Mathf.Clamp(movingPlatform.platform.transform.position.y + movingPlatform.effectiveVel, -1000f, 117f)), null);
			}
			movingPlatform.effectiveVel -= 10f * CupheadTime.FixedDelta;
			if (movingPlatform.platform.transform.position.y < -384f)
			{
				this.beardSettles[id].Play("Settle");
			}
			yield return wait;
		}
		if (movingPlatform.platform.GetComponentInChildren<LevelPlayerController>())
		{
			LevelPlayerController[] componentsInChildren = movingPlatform.platform.GetComponentsInChildren<LevelPlayerController>();
			foreach (LevelPlayerController levelPlayerController in componentsInChildren)
			{
				levelPlayerController.transform.parent = null;
			}
		}
		Object.Destroy(movingPlatform.platform.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x04001AD0 RID: 6864
	public const float PLATFORM_EXIT_SPEED = 10f;

	// Token: 0x04001AD1 RID: 6865
	[SerializeField]
	public OldManLevelPlatform[] allPlatforms;

	// Token: 0x04001AD2 RID: 6866
	[SerializeField]
	public Animator[] beardSettles;

	// Token: 0x04001AD3 RID: 6867
	[SerializeField]
	public SpriteRenderer mainBeardTufts;

	// Token: 0x04001AD4 RID: 6868
	[SerializeField]
	public float wobbleBeforeRemoveTime = 1f;

	// Token: 0x04001AD5 RID: 6869
	public bool inPhaseOne = true;

	// Token: 0x04001AD6 RID: 6870
	public float lastPos;

	// Token: 0x04001AD7 RID: 6871
	public List<int> removeOrderList = new List<int>();

	// Token: 0x04001AD8 RID: 6872
	public List<float> removeThresholdList = new List<float>();
}
