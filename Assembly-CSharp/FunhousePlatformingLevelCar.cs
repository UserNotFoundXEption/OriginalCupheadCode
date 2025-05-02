using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000414 RID: 1044
public class FunhousePlatformingLevelCar : AbstractCollidableObject
{
	// Token: 0x06002D6D RID: 11629 RVA: 0x00025E90 File Offset: 0x00024090
	public override void Awake()
	{
		base.Awake();
		FunhousePlatformingLevelCar.CARS_ALIVE++;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002D6E RID: 11630 RVA: 0x00025EAF File Offset: 0x000240AF
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002D6F RID: 11631 RVA: 0x00025EC7 File Offset: 0x000240C7
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002D70 RID: 11632 RVA: 0x000DCC40 File Offset: 0x000DAE40
	public void Init(Vector2 pos, float rotation, float carSpeed, int index, bool leader, bool last)
	{
		base.transform.position = pos;
		base.transform.SetEulerAngles(null, null, new float?(rotation));
		this.speed = carSpeed;
		this.leader = leader;
		this.last = last;
		foreach (GameObject gameObject in this.carSprites)
		{
			gameObject.SetActive(false);
		}
		this.carSprites[index].SetActive(true);
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002D71 RID: 11633 RVA: 0x000DCCDC File Offset: 0x000DAEDC
	public IEnumerator move_cr()
	{
		if (this.leader)
		{
			AudioManager.PlayLoop("funhouse_car_idle");
			this.emitAudioFromObject.Add("funhouse_car_idle");
		}
		YieldInstruction wait = new WaitForFixedUpdate();
		float size = base.GetComponent<Collider2D>().bounds.size.x;
		while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMin - (size + 50f))
		{
			base.transform.position += Vector3.left * this.speed * CupheadTime.FixedDelta;
			yield return wait;
		}
		if (this.last && FunhousePlatformingLevelCar.CARS_ALIVE <= 1)
		{
			AudioManager.Stop("funhouse_car_idle");
		}
		FunhousePlatformingLevelCar.CARS_ALIVE--;
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040025A0 RID: 9632
	public static int CARS_ALIVE;

	// Token: 0x040025A1 RID: 9633
	[SerializeField]
	public GameObject[] carSprites;

	// Token: 0x040025A2 RID: 9634
	public bool leader;

	// Token: 0x040025A3 RID: 9635
	public bool last;

	// Token: 0x040025A4 RID: 9636
	public float speed;

	// Token: 0x040025A5 RID: 9637
	public DamageDealer damageDealer;
}
