using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200020C RID: 524
public class DragonLevelCloudPlatform : LevelPlatform
{
	// Token: 0x06001801 RID: 6145 RVA: 0x000A2D30 File Offset: 0x000A0F30
	public override void Awake()
	{
		base.Awake();
		base.animator.SetInteger("Cloud", Random.Range(0, 3));
		this.minX = -640f - base.GetComponent<SpriteRenderer>().bounds.size.x / 2f;
		this.maxX = 640f + base.GetComponent<SpriteRenderer>().bounds.size.x / 2f;
	}

	// Token: 0x06001802 RID: 6146 RVA: 0x000147BB File Offset: 0x000129BB
	public override void AddChild(Transform player)
	{
		base.AddChild(player);
		base.animator.SetBool("HasPlayer", true);
	}

	// Token: 0x06001803 RID: 6147 RVA: 0x000147D5 File Offset: 0x000129D5
	public override void OnPlayerExit(Transform player)
	{
		base.OnPlayerExit(player);
		if (base.players.Count <= 0)
		{
			base.animator.SetBool("HasPlayer", false);
		}
	}

	// Token: 0x06001804 RID: 6148 RVA: 0x00014800 File Offset: 0x00012A00
	public void OnDisable()
	{
		this.top.sprite = null;
	}

	// Token: 0x06001805 RID: 6149 RVA: 0x0001480E File Offset: 0x00012A0E
	public void GetProperties(DragonLevelPlatformManager manager, LevelProperties.Dragon.Clouds properties)
	{
		this.properties = properties;
		this.manager = manager;
		this.speed = ((!properties.movingRight) ? properties.cloudSpeed : (-properties.cloudSpeed));
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001806 RID: 6150 RVA: 0x0001484E File Offset: 0x00012A4E
	public void GetProperties(LevelProperties.Dragon.Clouds properties, bool firstTime)
	{
		this.properties = properties;
		this.speed = ((!properties.movingRight) ? properties.cloudSpeed : (-properties.cloudSpeed));
		if (firstTime)
		{
			base.StartCoroutine(this.move_cr());
		}
	}

	// Token: 0x06001807 RID: 6151 RVA: 0x000A2DB4 File Offset: 0x000A0FB4
	public IEnumerator move_cr()
	{
		for (;;)
		{
			base.transform.AddPosition(-DragonLevel.SPEED * this.speed * CupheadTime.Delta, 0f, 0f);
			yield return null;
			if (this.properties.movingRight)
			{
				if (base.transform.position.x >= this.maxX)
				{
					if (this.manager != null)
					{
						this.manager.DestroyObjectPool(this);
					}
					else
					{
						Object.Destroy(base.gameObject);
					}
				}
			}
			else if (base.transform.position.x <= this.minX)
			{
				if (this.manager != null)
				{
					this.manager.DestroyObjectPool(this);
				}
				else
				{
					Object.Destroy(base.gameObject);
				}
			}
		}
		yield break;
	}

	// Token: 0x04001373 RID: 4979
	[SerializeField]
	public SpriteRenderer top;

	// Token: 0x04001374 RID: 4980
	public float minX;

	// Token: 0x04001375 RID: 4981
	public float maxX;

	// Token: 0x04001376 RID: 4982
	public LevelProperties.Dragon.Clouds properties;

	// Token: 0x04001377 RID: 4983
	public DragonLevelPlatformManager manager;

	// Token: 0x04001378 RID: 4984
	public float speed;
}
