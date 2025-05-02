using System;
using UnityEngine;

// Token: 0x02000292 RID: 658
public class FlyingMermaidLevelSplashManager : AbstractPausableComponent
{
	// Token: 0x170002BB RID: 699
	// (get) Token: 0x06001DCD RID: 7629 RVA: 0x000B1CEC File Offset: 0x000AFEEC
	public static FlyingMermaidLevelSplashManager Instance
	{
		get
		{
			if (FlyingMermaidLevelSplashManager.splashManager == null)
			{
				FlyingMermaidLevelSplashManager.splashManager = new GameObject
				{
					name = "SplashManager"
				}.AddComponent<FlyingMermaidLevelSplashManager>();
			}
			return FlyingMermaidLevelSplashManager.splashManager;
		}
	}

	// Token: 0x06001DCE RID: 7630 RVA: 0x00019335 File Offset: 0x00017535
	public override void Awake()
	{
		base.Awake();
		FlyingMermaidLevelSplashManager.splashManager = this;
	}

	// Token: 0x06001DCF RID: 7631 RVA: 0x000B1D2C File Offset: 0x000AFF2C
	public void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "EnemyProjectile" && collider.gameObject.GetComponent<FlyingMermaidLevelNoSplashMarker>() == null)
		{
			if (collider.GetComponent<Collider2D>().bounds.size.x > 200f)
			{
				this.SpawnMegaSplashMedium(collider.gameObject, 0f, false, 0f);
			}
			else if (collider.GetComponent<Collider2D>().bounds.size.x > 50f)
			{
				this.SpawnSplashMedium(collider.gameObject, 0f, false, 0f);
			}
			else
			{
				this.SpawnSplashSmall(collider.gameObject);
			}
		}
	}

	// Token: 0x06001DD0 RID: 7632 RVA: 0x00019343 File Offset: 0x00017543
	public void SpawnMegaSplashLarge(GameObject gameObject, float extraX = 0f, bool overrideY = false, float y = 0f)
	{
		this.CreateSplash(this.MegasplashLarge, gameObject, extraX, overrideY, y);
	}

	// Token: 0x06001DD1 RID: 7633 RVA: 0x00019356 File Offset: 0x00017556
	public void SpawnMegaSplashMedium(GameObject gameObject, float extraX = 0f, bool overrideY = false, float y = 0f)
	{
		this.CreateSplash(this.MegasplashMedium, gameObject, extraX, overrideY, y);
	}

	// Token: 0x06001DD2 RID: 7634 RVA: 0x00019369 File Offset: 0x00017569
	public void SpawnSplashMedium(GameObject gameObject, float extraX = 0f, bool overrideY = false, float y = 0f)
	{
		this.CreateSplash(this.SplashMedium, gameObject, extraX, overrideY, y);
	}

	// Token: 0x06001DD3 RID: 7635 RVA: 0x0001937C File Offset: 0x0001757C
	public void SpawnSplashSmall(GameObject gameObject)
	{
		this.CreateSplash(this.SplashSmall, gameObject, 0f, false, 0f);
	}

	// Token: 0x06001DD4 RID: 7636 RVA: 0x000B1DF8 File Offset: 0x000AFFF8
	public void CreateSplash(Effect effect, GameObject gameObject, float extraX = 0f, bool overrideY = false, float y = 0f)
	{
		float num = 0f;
		if (gameObject.GetComponent<Renderer>() != null)
		{
			num = gameObject.GetComponent<Renderer>().bounds.size.y / 4f;
		}
		Vector3 position;
		position..ctor(gameObject.transform.position.x + extraX, gameObject.transform.position.y - num);
		if (overrideY)
		{
			position.y = y;
		}
		Effect effect2 = Object.Instantiate<Effect>(effect);
		effect2.transform.position = position;
		if (gameObject.GetComponent<SpriteRenderer>() != null)
		{
			effect2.GetComponent<SpriteRenderer>().sortingLayerName = gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
			effect2.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
		}
	}

	// Token: 0x06001DD5 RID: 7637 RVA: 0x00019396 File Offset: 0x00017596
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.MegasplashLarge = null;
		this.MegasplashMedium = null;
		this.SplashMedium = null;
		this.SplashSmall = null;
	}

	// Token: 0x04001870 RID: 6256
	public static FlyingMermaidLevelSplashManager splashManager;

	// Token: 0x04001871 RID: 6257
	public Transform spawnRootFront;

	// Token: 0x04001872 RID: 6258
	public Transform spawnRootBack;

	// Token: 0x04001873 RID: 6259
	[SerializeField]
	public Effect MegasplashLarge;

	// Token: 0x04001874 RID: 6260
	[SerializeField]
	public Effect MegasplashMedium;

	// Token: 0x04001875 RID: 6261
	[SerializeField]
	public Effect SplashMedium;

	// Token: 0x04001876 RID: 6262
	[SerializeField]
	public Effect SplashSmall;
}
