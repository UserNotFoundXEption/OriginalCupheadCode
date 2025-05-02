using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000063 RID: 99
public static class TransformExtensions
{
	// Token: 0x0600053A RID: 1338 RVA: 0x00005AA2 File Offset: 0x00003CA2
	public static void ResetScale(this Transform transform)
	{
		transform.localScale = Vector3.one;
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x00005AAF File Offset: 0x00003CAF
	public static void ResetPosition(this Transform transform)
	{
		transform.position = Vector3.zero;
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x00005ABC File Offset: 0x00003CBC
	public static void ResetLocalPosition(this Transform transform)
	{
		transform.localPosition = Vector3.zero;
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x00005AC9 File Offset: 0x00003CC9
	public static void ResetRotation(this Transform transform)
	{
		transform.eulerAngles = Vector3.zero;
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x00005AD6 File Offset: 0x00003CD6
	public static void ResetLocalRotation(this Transform transform)
	{
		transform.localEulerAngles = Vector3.zero;
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x00005AE3 File Offset: 0x00003CE3
	public static void ResetTransforms(this Transform transform)
	{
		transform.ResetPosition();
		transform.ResetRotation();
		transform.ResetScale();
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x00005AF7 File Offset: 0x00003CF7
	public static void ResetLocalTransforms(this Transform transform)
	{
		transform.ResetLocalPosition();
		transform.ResetLocalRotation();
		transform.ResetScale();
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x0006C2B4 File Offset: 0x0006A4B4
	public static void SetPosition(this Transform transform, float? x = null, float? y = null, float? z = null)
	{
		Vector3 position = transform.position;
		if (x != null)
		{
			position.x = x.Value;
		}
		if (y != null)
		{
			position.y = y.Value;
		}
		if (z != null)
		{
			position.z = z.Value;
		}
		transform.position = position;
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x0006C320 File Offset: 0x0006A520
	public static void SetLocalPosition(this Transform transform, float? x = null, float? y = null, float? z = null)
	{
		Vector3 localPosition = transform.localPosition;
		if (x != null)
		{
			localPosition.x = x.Value;
		}
		if (y != null)
		{
			localPosition.y = y.Value;
		}
		if (z != null)
		{
			localPosition.z = z.Value;
		}
		transform.localPosition = localPosition;
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x0006C38C File Offset: 0x0006A58C
	public static void SetEulerAngles(this Transform transform, float? x = null, float? y = null, float? z = null)
	{
		Vector3 eulerAngles = transform.eulerAngles;
		if (x != null)
		{
			eulerAngles.x = x.Value;
		}
		if (y != null)
		{
			eulerAngles.y = y.Value;
		}
		if (z != null)
		{
			eulerAngles.z = z.Value;
		}
		transform.eulerAngles = eulerAngles;
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0006C3F8 File Offset: 0x0006A5F8
	public static void SetLocalEulerAngles(this Transform transform, float? x = null, float? y = null, float? z = null)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		if (x != null)
		{
			localEulerAngles.x = x.Value;
		}
		if (y != null)
		{
			localEulerAngles.y = y.Value;
		}
		if (z != null)
		{
			localEulerAngles.z = z.Value;
		}
		transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x0006C464 File Offset: 0x0006A664
	public static void SetScale(this Transform transform, float? x = null, float? y = null, float? z = null)
	{
		Vector3 localScale = transform.localScale;
		if (x != null)
		{
			localScale.x = x.Value;
		}
		if (y != null)
		{
			localScale.y = y.Value;
		}
		if (z != null)
		{
			localScale.z = z.Value;
		}
		transform.localScale = localScale;
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x0006C4D0 File Offset: 0x0006A6D0
	public static void AddPosition(this Transform transform, float x = 0f, float y = 0f, float z = 0f)
	{
		Vector3 position = transform.position;
		position.x += x;
		position.y += y;
		position.z += z;
		transform.position = position;
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x0006C518 File Offset: 0x0006A718
	public static void AddLocalPosition(this Transform transform, float x = 0f, float y = 0f, float z = 0f)
	{
		Vector3 localPosition = transform.localPosition;
		localPosition.x += x;
		localPosition.y += y;
		localPosition.z += z;
		transform.localPosition = localPosition;
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x00005B0B File Offset: 0x00003D0B
	public static void AddPositionForward2D(this Transform transform, float forward)
	{
		transform.position += transform.right * forward;
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x0006C560 File Offset: 0x0006A760
	public static void AddEulerAngles(this Transform transform, float x = 0f, float y = 0f, float z = 0f)
	{
		Vector3 eulerAngles = transform.eulerAngles;
		eulerAngles.x += x;
		eulerAngles.y += y;
		eulerAngles.z += z;
		transform.eulerAngles = eulerAngles;
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x0006C5A8 File Offset: 0x0006A7A8
	public static void AddLocalEulerAngles(this Transform transform, float x = 0f, float y = 0f, float z = 0f)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		localEulerAngles.x += x;
		localEulerAngles.y += y;
		localEulerAngles.z += z;
		transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x0006C5F0 File Offset: 0x0006A7F0
	public static void AddScale(this Transform transform, float x = 0f, float y = 0f, float z = 0f)
	{
		Vector3 localScale = transform.localScale;
		localScale.x += x;
		localScale.y += y;
		localScale.z += z;
		transform.localScale = localScale;
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x00005B2A File Offset: 0x00003D2A
	public static void MoveForward(this Transform transform, float amount)
	{
		transform.position += transform.forward * amount;
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x00005B49 File Offset: 0x00003D49
	public static void MoveForward2D(this Transform transform, float amount)
	{
		transform.position += transform.right * amount;
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x00005B68 File Offset: 0x00003D68
	public static void LookAt2D(this Transform transform, Transform target)
	{
		transform.LookAt2D(target.position);
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x0006C638 File Offset: 0x0006A838
	public static void LookAt2D(this Transform transform, Vector3 target)
	{
		Vector3 vector = target - transform.position;
		vector.Normalize();
		transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(vector.y, vector.x) * 57.29578f);
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x0006C688 File Offset: 0x0006A888
	public static Transform[] GetChildTransforms(this Transform transform)
	{
		List<Transform> list = new List<Transform>();
		IEnumerator enumerator = transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform item = (Transform)obj;
				list.Add(item);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		list.Remove(transform);
		return list.ToArray();
	}
}
