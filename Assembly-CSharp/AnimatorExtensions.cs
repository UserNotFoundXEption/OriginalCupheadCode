using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000059 RID: 89
public static class AnimatorExtensions
{
	// Token: 0x060004D3 RID: 1235 RVA: 0x0006B09C File Offset: 0x0006929C
	public static float GetCurrentClipLength(this Animator animator, int layer = 0)
	{
		AnimatorClipInfo[] currentAnimatorClipInfo = animator.GetCurrentAnimatorClipInfo(layer);
		if (currentAnimatorClipInfo.Length == 0)
		{
			return 0f;
		}
		AnimationClip clip = currentAnimatorClipInfo[0].clip;
		return clip.length;
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x000056B8 File Offset: 0x000038B8
	public static void FloorFrame(this Animator animator, int layer = 0)
	{
		if (AnimatorExtensions.<>f__mg$cache0 == null)
		{
			AnimatorExtensions.<>f__mg$cache0 = new Func<float, float>(Mathf.Floor);
		}
		AnimatorExtensions.roundFrame(animator, layer, AnimatorExtensions.<>f__mg$cache0);
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x000056DE File Offset: 0x000038DE
	public static void RoundFrame(this Animator animator, int layer = 0)
	{
		if (AnimatorExtensions.<>f__mg$cache1 == null)
		{
			AnimatorExtensions.<>f__mg$cache1 = new Func<float, float>(Mathf.Round);
		}
		AnimatorExtensions.roundFrame(animator, layer, AnimatorExtensions.<>f__mg$cache1);
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x0006B0D4 File Offset: 0x000692D4
	public static void roundFrame(Animator animator, int layer, Func<float, float> rounder)
	{
		AnimatorClipInfo[] currentAnimatorClipInfo = animator.GetCurrentAnimatorClipInfo(layer);
		if (currentAnimatorClipInfo.Length == 0)
		{
			return;
		}
		AnimationClip clip = currentAnimatorClipInfo[0].clip;
		float frameRate = clip.frameRate;
		float length = clip.length;
		float normalizedTime = animator.GetCurrentAnimatorStateInfo(layer).normalizedTime;
		float arg = normalizedTime * length * frameRate;
		float num = rounder(arg) / frameRate / length;
		animator.Play(0, layer, num);
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x00005704 File Offset: 0x00003904
	public static Coroutine WaitForAnimationToStart(this Animator animator, MonoBehaviour parent, string animationName, bool waitForEndOfFrame = false)
	{
		return animator.WaitForAnimationToStart(parent, animationName, 0, waitForEndOfFrame);
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x0006B140 File Offset: 0x00069340
	public static Coroutine WaitForAnimationToStart(this Animator animator, MonoBehaviour parent, string animationName, int layer, bool waitForEndOfFrame = false)
	{
		int animationHash = Animator.StringToHash(animator.GetLayerName(layer) + "." + animationName);
		return animator.WaitForAnimationToStart(parent, animationHash, layer, waitForEndOfFrame);
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x00005710 File Offset: 0x00003910
	public static Coroutine WaitForAnimationToStart(this Animator animator, MonoBehaviour parent, int animationHash, int layer, bool waitForEndOfFrame = false)
	{
		return parent.StartCoroutine(AnimatorExtensions.waitForAnimStart_cr(animator, layer, animationHash, waitForEndOfFrame));
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x0006B170 File Offset: 0x00069370
	public static IEnumerator waitForAnimStart_cr(Animator animator, int layer, int animationHash, bool waitForEndOfFrame)
	{
		while (animator.GetCurrentAnimatorStateInfo(layer).fullPathHash != animationHash)
		{
			if (waitForEndOfFrame)
			{
				yield return new WaitForEndOfFrame();
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x00005722 File Offset: 0x00003922
	public static Coroutine WaitForAnimationToEnd(this Animator animator, MonoBehaviour parent, bool waitForEndOfFrame = false)
	{
		return parent.StartCoroutine(AnimatorExtensions.waitForAnimEnd_cr(parent, animator, 0, waitForEndOfFrame));
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x0006B1A0 File Offset: 0x000693A0
	public static IEnumerator waitForAnimEnd_cr(MonoBehaviour parent, Animator animator, int layer, bool waitForEndOfFrame)
	{
		int current = animator.GetCurrentAnimatorStateInfo(layer).fullPathHash;
		while (current == animator.GetCurrentAnimatorStateInfo(layer).fullPathHash)
		{
			if (waitForEndOfFrame)
			{
				yield return new WaitForEndOfFrame();
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x00005733 File Offset: 0x00003933
	public static Coroutine WaitForAnimationToEnd(this Animator animator, MonoBehaviour parent, string name, bool waitForEndOfFrame = false, bool waitForStart = true)
	{
		return animator.WaitForAnimationToEnd(parent, name, 0, waitForEndOfFrame, waitForStart);
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x0006B1CC File Offset: 0x000693CC
	public static Coroutine WaitForAnimationToEnd(this Animator animator, MonoBehaviour parent, string name, int layer, bool waitForEndOfFrame = false, bool waitForStart = true)
	{
		int animationHash = Animator.StringToHash(animator.GetLayerName(layer) + "." + name);
		return animator.WaitForAnimationToEnd(parent, animationHash, layer, waitForEndOfFrame, waitForStart);
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x00005741 File Offset: 0x00003941
	public static Coroutine WaitForAnimationToEnd(this Animator animator, MonoBehaviour parent, int animationHash, int layer = 0, bool waitForEndOfFrame = false, bool waitForStart = true)
	{
		return parent.StartCoroutine(AnimatorExtensions.waitForNamedAnimEnd_cr(parent, animator, animationHash, layer, waitForEndOfFrame, waitForStart));
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x0006B200 File Offset: 0x00069400
	public static IEnumerator waitForNamedAnimEnd_cr(MonoBehaviour parent, Animator animator, int animationHash, int layer, bool waitForEndOfFrame, bool waitForStart = true)
	{
		if (waitForStart)
		{
			yield return parent.StartCoroutine(AnimatorExtensions.waitForAnimStart_cr(animator, layer, animationHash, waitForEndOfFrame));
		}
		while (animator.GetCurrentAnimatorStateInfo(layer).fullPathHash == animationHash)
		{
			if (waitForEndOfFrame)
			{
				yield return new WaitForEndOfFrame();
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x0006B240 File Offset: 0x00069440
	public static Coroutine WaitForNormalizedTime(this Animator animator, MonoBehaviour parent, float normalizedTime, string name = null, int layer = 0, bool allowEqualTime = false, bool waitForEndOfFrame = false, bool waitForStart = true)
	{
		int? animationHash = null;
		if (name != null)
		{
			animationHash = new int?(Animator.StringToHash(animator.GetLayerName(layer) + "." + name));
		}
		return animator.WaitForNormalizedTime(parent, normalizedTime, animationHash, layer, allowEqualTime, waitForEndOfFrame, waitForStart);
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x0006B28C File Offset: 0x0006948C
	public static Coroutine WaitForNormalizedTime(this Animator animator, MonoBehaviour parent, float normalizedTime, int? animationHash, int layer = 0, bool allowEqualTime = false, bool waitForEndOfFrame = false, bool waitForStart = true)
	{
		return parent.StartCoroutine(AnimatorExtensions.waitForNormalizedTime_cr(parent, animator, normalizedTime, animationHash, layer, allowEqualTime, waitForEndOfFrame, waitForStart, false));
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x0006B2B4 File Offset: 0x000694B4
	public static Coroutine WaitForNormalizedTimeLooping(this Animator animator, MonoBehaviour parent, float normalizedTimeDecimal, string name = null, int layer = 0, bool allowEqualTime = false, bool waitForEndOfFrame = false, bool waitForStart = true)
	{
		int? animationHash = null;
		if (name != null)
		{
			animationHash = new int?(Animator.StringToHash(animator.GetLayerName(layer) + "." + name));
		}
		return parent.StartCoroutine(AnimatorExtensions.waitForNormalizedTime_cr(parent, animator, normalizedTimeDecimal, animationHash, layer, allowEqualTime, waitForEndOfFrame, waitForStart, true));
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x0006B308 File Offset: 0x00069508
	public static IEnumerator waitForNormalizedTime_cr(MonoBehaviour parent, Animator animator, float normalizedTime, int? animationHash, int layer, bool allowEqualTime, bool waitForEndOfFrame, bool waitForStart, bool looping)
	{
		if (animationHash != null && waitForStart)
		{
			yield return parent.StartCoroutine(AnimatorExtensions.waitForAnimStart_cr(animator, layer, animationHash.Value, waitForEndOfFrame));
		}
		int target;
		if (animationHash != null)
		{
			target = animationHash.Value;
		}
		else
		{
			target = animator.GetCurrentAnimatorStateInfo(layer).fullPathHash;
		}
		for (;;)
		{
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layer);
			float num = (!looping) ? stateInfo.normalizedTime : MathUtilities.DecimalPart(stateInfo.normalizedTime);
			if (((!allowEqualTime) ? (stateInfo.normalizedTime >= normalizedTime) : (stateInfo.normalizedTime > normalizedTime)) || stateInfo.fullPathHash != target)
			{
				break;
			}
			if (waitForEndOfFrame)
			{
				yield return new WaitForEndOfFrame();
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x0400047F RID: 1151
	[CompilerGenerated]
	private static Func<float, float> <>f__mg$cache0;

	// Token: 0x04000480 RID: 1152
	[CompilerGenerated]
	private static Func<float, float> <>f__mg$cache1;
}
