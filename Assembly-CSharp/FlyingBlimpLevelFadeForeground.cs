using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000242 RID: 578
public class FlyingBlimpLevelFadeForeground : FlyingBlimpLevelScrollingSpriteSpawnerBase
{
	// Token: 0x06001A8F RID: 6799 RVA: 0x000A9010 File Offset: 0x000A7210
	public override void Awake()
	{
		base.Awake();
		this.fadeTime = 10f;
		this.nightSprite = new Transform[this.spritePrefabs.Length];
		if (this.spawnedChild != null)
		{
			this.spawnedChild.transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		}
		for (int i = 0; i < this.nightSprite.Length; i++)
		{
			this.nightSprite[i] = this.spritePrefabs[i].sprite.transform.GetChild(0);
			this.nightSprite[i].transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	// Token: 0x06001A90 RID: 6800 RVA: 0x000169A3 File Offset: 0x00014BA3
	public override void OnSpawn(GameObject obj)
	{
		base.OnSpawn(obj);
		this.spawnedChild = obj.transform.GetChild(0);
	}

	// Token: 0x06001A91 RID: 6801 RVA: 0x000169BE File Offset: 0x00014BBE
	public void Update()
	{
		if (this.moonLady.state == FlyingBlimpLevelMoonLady.State.Morph && !this.startedChange)
		{
			this.startedChange = true;
			this.StartChange();
		}
	}

	// Token: 0x06001A92 RID: 6802 RVA: 0x000169E9 File Offset: 0x00014BE9
	public void StartChange()
	{
		base.StartCoroutine(this.change_cr());
	}

	// Token: 0x06001A93 RID: 6803 RVA: 0x000A90C4 File Offset: 0x000A72C4
	public IEnumerator change_cr()
	{
		float t = 0f;
		float startSpeed = this.speed;
		float endSpeed = this.speed + this.speed * 0.3f;
		while (t < this.fadeTime)
		{
			if (this.spawnedChild != null)
			{
				this.spawnedChild.transform.gameObject.GetComponent<SpriteRenderer>().enabled = true;
				this.spawnedChild.transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, t / this.fadeTime);
			}
			for (int j = 0; j < this.nightSprite.Length; j++)
			{
				if (this.nightSprite[j].transform != null)
				{
					this.nightSprite[j].transform.gameObject.GetComponent<SpriteRenderer>().enabled = true;
					this.nightSprite[j].transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, t / this.fadeTime);
				}
			}
			this.speed = Mathf.Lerp(startSpeed, endSpeed, t / this.fadeTime);
			t += CupheadTime.Delta;
			yield return null;
		}
		if (this.spawnedChild != null)
		{
			this.spawnedChild.transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		}
		for (int i = 0; i < this.nightSprite.Length; i++)
		{
			if (this.nightSprite[i].transform != null)
			{
				this.nightSprite[i].transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
				yield return null;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x04001552 RID: 5458
	[SerializeField]
	public FlyingBlimpLevelMoonLady moonLady;

	// Token: 0x04001553 RID: 5459
	public Transform[] nightSprite;

	// Token: 0x04001554 RID: 5460
	public Transform spawnedChild;

	// Token: 0x04001555 RID: 5461
	public float fadeTime;

	// Token: 0x04001556 RID: 5462
	public int index;

	// Token: 0x04001557 RID: 5463
	public bool startedChange;
}
