using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200009F RID: 159
public class CupheadMapCamera : AbstractCupheadGameCamera
{
	// Token: 0x17000160 RID: 352
	// (get) Token: 0x060007C7 RID: 1991 RVA: 0x00007945 File Offset: 0x00005B45
	// (set) Token: 0x060007C8 RID: 1992 RVA: 0x0000794C File Offset: 0x00005B4C
	public static CupheadMapCamera Current { get; set; }

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x060007C9 RID: 1993 RVA: 0x00007954 File Offset: 0x00005B54
	public override float OrthographicSize
	{
		get
		{
			return 3.6f;
		}
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x0000795B File Offset: 0x00005B5B
	public override void Awake()
	{
		base.Awake();
		CupheadMapCamera.Current = this;
		this.SetupColliders();
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x0000796F File Offset: 0x00005B6F
	public void OnDestroy()
	{
		if (CupheadMapCamera.Current == this)
		{
			CupheadMapCamera.Current = null;
		}
	}

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x060007CC RID: 1996 RVA: 0x0007441C File Offset: 0x0007261C
	public Vector2 playerCenter
	{
		get
		{
			if (PlayerManager.Multiplayer)
			{
				return (Map.Current.players[0].transform.position + Map.Current.players[1].transform.position) / 2f;
			}
			return Map.Current.players[0].transform.position;
		}
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x00074494 File Offset: 0x00072694
	public void Update()
	{
		if (Map.Current.CurrentState == Map.State.Event)
		{
			return;
		}
		Vector3 position = base.transform.position;
		Vector3 vector = this.playerCenter;
		if (this.properties.moveX)
		{
			position.x = vector.x;
		}
		if (this.properties.moveY)
		{
			position.y = vector.y;
		}
		position.x = Mathf.Clamp(position.x, this.properties.bounds.left + this.offset.x, this.properties.bounds.right - this.offset.x);
		position.y = Mathf.Clamp(position.y, this.properties.bounds.bottom + this.offset.y, this.properties.bounds.top - this.offset.y);
		if (this.centerOnPlayer)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, position, Time.deltaTime * 6f);
		}
		this.UpdateColliders();
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x000745D4 File Offset: 0x000727D4
	public void Init(Map.Camera properties)
	{
		base.camera.orthographicSize = 3.6f;
		this.properties = properties;
		this.offset = new Vector2(base.Bounds.width / 2f, base.Bounds.height / 2f);
		base.transform.position = this.playerCenter;
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x00074644 File Offset: 0x00072844
	public bool IsCameraFarFromPlayer()
	{
		Vector3 position = base.transform.position;
		Vector3 vector = this.playerCenter;
		vector.x = Mathf.Clamp(vector.x, this.properties.bounds.left + this.offset.x, this.properties.bounds.right - this.offset.x);
		vector.y = Mathf.Clamp(vector.y, this.properties.bounds.bottom + this.offset.y, this.properties.bounds.top - this.offset.y);
		return (double)(position - vector).sqrMagnitude > 0.01;
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x00007987 File Offset: 0x00005B87
	public Coroutine MoveToPosition(Vector2 position, float time, float zoom)
	{
		base.Zoom(zoom, time, EaseUtils.EaseType.easeInOutSine);
		return base.StartCoroutine(this.moveToPosition_cr(position, time));
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x0007471C File Offset: 0x0007291C
	public IEnumerator moveToPosition_cr(Vector2 position, float time)
	{
		Vector2 start = base.transform.position;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			float x = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start.x, position.x, val);
			float y = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start.y, position.y, val);
			base.transform.SetPosition(new float?(x), new float?(y), new float?(0f));
			t += base.LocalDeltaTime;
			yield return null;
		}
		base.transform.position = position;
		yield return null;
		yield break;
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00074748 File Offset: 0x00072948
	public void SetupColliders()
	{
		this.edgeCollider = base.gameObject.AddComponent<EdgeCollider2D>();
		this.edgeCollider.points = new Vector2[2];
		this.secretPathEdgeCollider = new GameObject
		{
			transform = 
			{
				parent = base.transform
			},
			layer = 25
		}.AddComponent<EdgeCollider2D>();
		this.secretPathEdgeCollider.points = this.edgeCollider.points;
		this.UpdateColliders();
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x000747C0 File Offset: 0x000729C0
	public void UpdateColliders()
	{
		Vector2[] points = new Vector2[]
		{
			new Vector3(-base.Bounds.width / 2f, -base.Bounds.height / 2f, 0f),
			new Vector3(-base.Bounds.width / 2f, base.Bounds.height / 2f, 0f),
			new Vector3(base.Bounds.width / 2f, base.Bounds.height / 2f, 0f),
			new Vector3(base.Bounds.width / 2f, -base.Bounds.height / 2f, 0f),
			new Vector3(-base.Bounds.width / 2f, -base.Bounds.height / 2f, 0f)
		};
		this.edgeCollider.points = points;
		this.secretPathEdgeCollider.points = points;
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x000079A1 File Offset: 0x00005BA1
	public void SetActiveCollider(bool active)
	{
		this.edgeCollider.enabled = active;
		this.secretPathEdgeCollider.enabled = active;
	}

	// Token: 0x040005EE RID: 1518
	public bool centerOnPlayer = true;

	// Token: 0x040005EF RID: 1519
	public const float SPEED = 6f;

	// Token: 0x040005F0 RID: 1520
	public const float ORTHO_SIZE = 3.6f;

	// Token: 0x040005F2 RID: 1522
	public Map.Camera properties;

	// Token: 0x040005F3 RID: 1523
	public Vector2 offset;

	// Token: 0x040005F4 RID: 1524
	public EdgeCollider2D edgeCollider;

	// Token: 0x040005F5 RID: 1525
	public EdgeCollider2D secretPathEdgeCollider;
}
