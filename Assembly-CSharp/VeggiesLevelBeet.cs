using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003C6 RID: 966
public class VeggiesLevelBeet : LevelProperties.Veggies.Entity
{
	// Token: 0x17000335 RID: 821
	// (get) Token: 0x06002A80 RID: 10880 RVA: 0x00023C6E File Offset: 0x00021E6E
	// (set) Token: 0x06002A81 RID: 10881 RVA: 0x00023C76 File Offset: 0x00021E76
	public VeggiesLevelBeet.State state { get; set; }

	// Token: 0x1400005A RID: 90
	// (add) Token: 0x06002A82 RID: 10882 RVA: 0x000D414C File Offset: 0x000D234C
	// (remove) Token: 0x06002A83 RID: 10883 RVA: 0x000D4184 File Offset: 0x000D2384
	public event VeggiesLevelBeet.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x06002A84 RID: 10884 RVA: 0x00023C7F File Offset: 0x00021E7F
	public override void Awake()
	{
		base.Awake();
		this.CreatePoints();
	}

	// Token: 0x06002A85 RID: 10885 RVA: 0x00023C8D File Offset: 0x00021E8D
	public void Start()
	{
		this.boxCollider = base.GetComponent<BoxCollider2D>();
		this.boxCollider.enabled = false;
	}

	// Token: 0x06002A86 RID: 10886 RVA: 0x000D41BC File Offset: 0x000D23BC
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		foreach (Vector2 vector in this.GetPoints())
		{
			Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
			Gizmos.DrawLine(this.babyRoot.position, vector);
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(vector, 10f);
		}
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.babyRoot.position, new Vector3(-150f, 360f, 0f));
		Gizmos.DrawLine(this.babyRoot.position, new Vector3(640f, 360f, 0f));
	}

	// Token: 0x06002A87 RID: 10887 RVA: 0x000D4298 File Offset: 0x000D2498
	public override void LevelInitWithGroup(AbstractLevelPropertyGroup propertyGroup)
	{
		base.LevelInitWithGroup(propertyGroup);
		this.properties = (propertyGroup as LevelProperties.Veggies.Beet);
		this.hp = (float)this.properties.hp;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = new DamageDealer(1f, 0.2f, true, false, false);
		this.damageDealer.SetDirection(DamageDealer.Direction.Neutral, base.transform);
	}

	// Token: 0x06002A88 RID: 10888 RVA: 0x000D430C File Offset: 0x000D250C
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.state == VeggiesLevelBeet.State.Start)
		{
			return;
		}
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06002A89 RID: 10889 RVA: 0x00023CA7 File Offset: 0x00021EA7
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002A8A RID: 10890 RVA: 0x00023CC5 File Offset: 0x00021EC5
	public void OnInAnimComplete()
	{
		this.boxCollider.enabled = true;
		this.state = VeggiesLevelBeet.State.Go;
		base.StartCoroutine(this.beet_cr());
	}

	// Token: 0x06002A8B RID: 10891 RVA: 0x00023CE7 File Offset: 0x00021EE7
	public void OnDeathAnimComplete()
	{
		this.state = VeggiesLevelBeet.State.Complete;
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002A8C RID: 10892 RVA: 0x00023CFB File Offset: 0x00021EFB
	public void Die()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x06002A8D RID: 10893 RVA: 0x000D436C File Offset: 0x000D256C
	public Vector2[] GetPoints()
	{
		Vector2[] array = new Vector2[8];
		for (int i = 0; i < 8; i++)
		{
			float num = (float)i / 7f;
			array[i] = Vector2.Lerp(new Vector2(-150f, 360f), new Vector2(640f, 360f), num);
		}
		return array;
	}

	// Token: 0x06002A8E RID: 10894 RVA: 0x000D43CC File Offset: 0x000D25CC
	public void CreatePoints()
	{
		Vector2[] array = this.GetPoints();
		this.points = new Transform[array.Length];
		for (int i = 0; i < this.points.Length; i++)
		{
			this.points[i] = new GameObject("Point " + i).transform;
			this.points[i].position = array[i];
			this.points[i].SetParent(base.transform);
		}
	}

	// Token: 0x06002A8F RID: 10895 RVA: 0x000D445C File Offset: 0x000D265C
	public float GetPointAngle(int i)
	{
		this.babyRoot.LookAt2D(this.points[i]);
		return this.babyRoot.eulerAngles.z;
	}

	// Token: 0x06002A90 RID: 10896 RVA: 0x000D4490 File Offset: 0x000D2690
	public IEnumerator beet_cr()
	{
		string[] array = this.properties.babyPatterns[Random.Range(0, this.properties.babyPatterns.Length)].Split(new char[]
		{
			','
		});
		int[] numbers = new int[array.Length];
		for (int j = 0; j < numbers.Length; j++)
		{
			if (!Parser.IntTryParse(array[j], out numbers[j]))
			{
				Debug.LogError("Veggies.Beet.babyPatterns is not formatted correctly!\nExpecting 4,5,6,5,4", null);
				this.StopAllCoroutines();
			}
		}
		int typeIndex = 0;
		int specialIndex = this.properties.alternateRate.RandomInt();
		VeggiesLevelBeetBaby.Type type = VeggiesLevelBeetBaby.Type.Regular;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.idleTime);
			base.animator.SetTrigger("Shoot_Start");
			yield return CupheadTime.WaitForSeconds(this, 1f);
			int loop = 0;
			int point = 0;
			while (loop < numbers.Length)
			{
				for (int i = 0; i < numbers[loop]; i++)
				{
					int newPoint;
					for (newPoint = point; newPoint == point; newPoint = Random.Range(0, 8))
					{
					}
					point = newPoint;
					if (typeIndex >= specialIndex)
					{
						type = ((!Rand.Bool()) ? VeggiesLevelBeetBaby.Type.Fat : VeggiesLevelBeetBaby.Type.Pink);
						typeIndex = 0;
						specialIndex = this.properties.alternateRate.RandomInt();
					}
					else
					{
						type = VeggiesLevelBeetBaby.Type.Regular;
					}
					typeIndex++;
					base.animator.SetTrigger("Shoot_" + type.ToString());
					this.babyPrefab.Create(type, this.properties.babySpeedUp, (float)this.properties.babySpeedSpread, this.properties.babySpreadAngle, this.babyRoot.position, this.GetPointAngle(point));
					yield return CupheadTime.WaitForSeconds(this, this.properties.babyDelay);
				}
				loop++;
				if (loop < numbers.Length)
				{
					yield return CupheadTime.WaitForSeconds(this, this.properties.babyGroupDelay);
				}
			}
			base.animator.SetTrigger("Shoot_End");
			yield return CupheadTime.WaitForSeconds(this, this.properties.babyGroupDelay);
		}
		yield break;
	}

	// Token: 0x06002A91 RID: 10897 RVA: 0x000D44AC File Offset: 0x000D26AC
	public IEnumerator die_cr()
	{
		this.boxCollider.enabled = false;
		base.animator.SetTrigger("Idle");
		yield return base.StartCoroutine(base.dieFlash_cr());
		base.animator.SetTrigger("Dead");
		yield break;
	}

	// Token: 0x04002372 RID: 9074
	public const float MAX_Y = 360f;

	// Token: 0x04002373 RID: 9075
	public const float POINTS_X_MIN = -150f;

	// Token: 0x04002374 RID: 9076
	public const float POINTS_X_MAX = 640f;

	// Token: 0x04002375 RID: 9077
	public const int POINTS_COUNT = 8;

	// Token: 0x04002377 RID: 9079
	[SerializeField]
	public Transform babyRoot;

	// Token: 0x04002378 RID: 9080
	[SerializeField]
	public VeggiesLevelBeetBaby babyPrefab;

	// Token: 0x04002379 RID: 9081
	public new LevelProperties.Veggies.Beet properties;

	// Token: 0x0400237A RID: 9082
	public BoxCollider2D boxCollider;

	// Token: 0x0400237B RID: 9083
	public float hp;

	// Token: 0x0400237C RID: 9084
	public Transform[] points;

	// Token: 0x0400237D RID: 9085
	public DamageDealer damageDealer;

	// Token: 0x02000FD5 RID: 4053
	public enum State
	{
		// Token: 0x040071CC RID: 29132
		Start,
		// Token: 0x040071CD RID: 29133
		Go,
		// Token: 0x040071CE RID: 29134
		Complete
	}

	// Token: 0x02000FD6 RID: 4054
	// (Invoke) Token: 0x06007659 RID: 30297
	public delegate void OnDamageTakenHandler(float damage);
}
