using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005FD RID: 1533
public class DialogueriTween : MonoBehaviour
{
	// Token: 0x06003EC8 RID: 16072 RVA: 0x0003272D File Offset: 0x0003092D
	public static void Init(GameObject target)
	{
		DialogueriTween.MoveBy(target, Vector3.zero, 0f);
	}

	// Token: 0x06003EC9 RID: 16073 RVA: 0x0011DD50 File Offset: 0x0011BF50
	public static void CameraFadeFrom(float amount, float time)
	{
		if (DialogueriTween.cameraFade)
		{
			DialogueriTween.CameraFadeFrom(DialogueriTween.Hash(new object[]
			{
				"amount",
				amount,
				"time",
				time
			}));
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.", null);
		}
	}

	// Token: 0x06003ECA RID: 16074 RVA: 0x0003273F File Offset: 0x0003093F
	public static void CameraFadeFrom(Hashtable args)
	{
		if (DialogueriTween.cameraFade)
		{
			DialogueriTween.ColorFrom(DialogueriTween.cameraFade, args);
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.", null);
		}
	}

	// Token: 0x06003ECB RID: 16075 RVA: 0x0011DDB0 File Offset: 0x0011BFB0
	public static void CameraFadeTo(float amount, float time)
	{
		if (DialogueriTween.cameraFade)
		{
			DialogueriTween.CameraFadeTo(DialogueriTween.Hash(new object[]
			{
				"amount",
				amount,
				"time",
				time
			}));
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.", null);
		}
	}

	// Token: 0x06003ECC RID: 16076 RVA: 0x0003276B File Offset: 0x0003096B
	public static void CameraFadeTo(Hashtable args)
	{
		if (DialogueriTween.cameraFade)
		{
			DialogueriTween.ColorTo(DialogueriTween.cameraFade, args);
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.", null);
		}
	}

	// Token: 0x06003ECD RID: 16077 RVA: 0x0011DE10 File Offset: 0x0011C010
	public static void ValueTo(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		if (!args.Contains("onupdate") || !args.Contains("from") || !args.Contains("to"))
		{
			Debug.LogError("iTween Error: ValueTo() requires an 'onupdate' callback function and a 'from' and 'to' property.  The supplied 'onupdate' callback must accept a single argument that is the same type as the supplied 'from' and 'to' properties!", null);
			return;
		}
		args["type"] = "value";
		if (args["from"].GetType() == typeof(Vector2))
		{
			args["method"] = "vector2";
		}
		else if (args["from"].GetType() == typeof(Vector3))
		{
			args["method"] = "vector3";
		}
		else if (args["from"].GetType() == typeof(Rect))
		{
			args["method"] = "rect";
		}
		else if (args["from"].GetType() == typeof(float))
		{
			args["method"] = "float";
		}
		else
		{
			if (args["from"].GetType() != typeof(Color))
			{
				Debug.LogError("iTween Error: ValueTo() only works with interpolating Vector3s, Vector2s, floats, ints, Rects and Colors!", null);
				return;
			}
			args["method"] = "color";
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", DialogueriTween.EaseType.linear);
		}
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003ECE RID: 16078 RVA: 0x00032797 File Offset: 0x00030997
	public static void FadeFrom(GameObject target, float alpha, float time)
	{
		DialogueriTween.FadeFrom(target, DialogueriTween.Hash(new object[]
		{
			"alpha",
			alpha,
			"time",
			time
		}));
	}

	// Token: 0x06003ECF RID: 16079 RVA: 0x000327CC File Offset: 0x000309CC
	public static void FadeFrom(GameObject target, Hashtable args)
	{
		DialogueriTween.ColorFrom(target, args);
	}

	// Token: 0x06003ED0 RID: 16080 RVA: 0x000327D5 File Offset: 0x000309D5
	public static void FadeTo(GameObject target, float alpha, float time)
	{
		DialogueriTween.FadeTo(target, DialogueriTween.Hash(new object[]
		{
			"alpha",
			alpha,
			"time",
			time
		}));
	}

	// Token: 0x06003ED1 RID: 16081 RVA: 0x0003280A File Offset: 0x00030A0A
	public static void FadeTo(GameObject target, Hashtable args)
	{
		DialogueriTween.ColorTo(target, args);
	}

	// Token: 0x06003ED2 RID: 16082 RVA: 0x00032813 File Offset: 0x00030A13
	public static void ColorFrom(GameObject target, Color color, float time)
	{
		DialogueriTween.ColorFrom(target, DialogueriTween.Hash(new object[]
		{
			"color",
			color,
			"time",
			time
		}));
	}

	// Token: 0x06003ED3 RID: 16083 RVA: 0x0011DFAC File Offset: 0x0011C1AC
	public static void ColorFrom(GameObject target, Hashtable args)
	{
		Color color = default(Color);
		Color color2 = default(Color);
		args = DialogueriTween.CleanArgs(args);
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					Hashtable hashtable = (Hashtable)args.Clone();
					hashtable["ischild"] = true;
					DialogueriTween.ColorFrom(transform.gameObject, hashtable);
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
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", DialogueriTween.EaseType.linear);
		}
		if (target.GetComponent(typeof(GUITexture)))
		{
			color = (color2 = target.GetComponent<GUITexture>().color);
		}
		else if (target.GetComponent(typeof(GUIText)))
		{
			color = (color2 = target.GetComponent<GUIText>().material.color);
		}
		else if (target.GetComponent<Renderer>())
		{
			color = (color2 = target.GetComponent<Renderer>().material.color);
		}
		else if (target.GetComponent<Light>())
		{
			color = (color2 = target.GetComponent<Light>().color);
		}
		if (args.Contains("color"))
		{
			color = (Color)args["color"];
		}
		else
		{
			if (args.Contains("r"))
			{
				color.r = (float)args["r"];
			}
			if (args.Contains("g"))
			{
				color.g = (float)args["g"];
			}
			if (args.Contains("b"))
			{
				color.b = (float)args["b"];
			}
			if (args.Contains("a"))
			{
				color.a = (float)args["a"];
			}
		}
		if (args.Contains("amount"))
		{
			color.a = (float)args["amount"];
			args.Remove("amount");
		}
		else if (args.Contains("alpha"))
		{
			color.a = (float)args["alpha"];
			args.Remove("alpha");
		}
		if (target.GetComponent(typeof(GUITexture)))
		{
			target.GetComponent<GUITexture>().color = color;
		}
		else if (target.GetComponent(typeof(GUIText)))
		{
			target.GetComponent<GUIText>().material.color = color;
		}
		else if (target.GetComponent<Renderer>())
		{
			target.GetComponent<Renderer>().material.color = color;
		}
		else if (target.GetComponent<Light>())
		{
			target.GetComponent<Light>().color = color;
		}
		args["color"] = color2;
		args["type"] = "color";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003ED4 RID: 16084 RVA: 0x00032848 File Offset: 0x00030A48
	public static void ColorTo(GameObject target, Color color, float time)
	{
		DialogueriTween.ColorTo(target, DialogueriTween.Hash(new object[]
		{
			"color",
			color,
			"time",
			time
		}));
	}

	// Token: 0x06003ED5 RID: 16085 RVA: 0x0011E33C File Offset: 0x0011C53C
	public static void ColorTo(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					Hashtable hashtable = (Hashtable)args.Clone();
					hashtable["ischild"] = true;
					DialogueriTween.ColorTo(transform.gameObject, hashtable);
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
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", DialogueriTween.EaseType.linear);
		}
		args["type"] = "color";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003ED6 RID: 16086 RVA: 0x0011E43C File Offset: 0x0011C63C
	public static void AudioFrom(GameObject target, float volume, float pitch, float time)
	{
		DialogueriTween.AudioFrom(target, DialogueriTween.Hash(new object[]
		{
			"volume",
			volume,
			"pitch",
			pitch,
			"time",
			time
		}));
	}

	// Token: 0x06003ED7 RID: 16087 RVA: 0x0011E490 File Offset: 0x0011C690
	public static void AudioFrom(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		AudioSource audioSource;
		if (args.Contains("audiosource"))
		{
			audioSource = (AudioSource)args["audiosource"];
		}
		else
		{
			if (!target.GetComponent(typeof(AudioSource)))
			{
				Debug.LogError("iTween Error: AudioFrom requires an AudioSource.", null);
				return;
			}
			audioSource = target.GetComponent<AudioSource>();
		}
		Vector2 vector;
		Vector2 vector2;
		vector.x = (vector2.x = audioSource.volume);
		vector.y = (vector2.y = audioSource.pitch);
		if (args.Contains("volume"))
		{
			vector2.x = (float)args["volume"];
		}
		if (args.Contains("pitch"))
		{
			vector2.y = (float)args["pitch"];
		}
		audioSource.volume = vector2.x;
		audioSource.pitch = vector2.y;
		args["volume"] = vector.x;
		args["pitch"] = vector.y;
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", DialogueriTween.EaseType.linear);
		}
		args["type"] = "audio";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003ED8 RID: 16088 RVA: 0x0011E60C File Offset: 0x0011C80C
	public static void AudioTo(GameObject target, float volume, float pitch, float time)
	{
		DialogueriTween.AudioTo(target, DialogueriTween.Hash(new object[]
		{
			"volume",
			volume,
			"pitch",
			pitch,
			"time",
			time
		}));
	}

	// Token: 0x06003ED9 RID: 16089 RVA: 0x0011E660 File Offset: 0x0011C860
	public static void AudioTo(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", DialogueriTween.EaseType.linear);
		}
		args["type"] = "audio";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EDA RID: 16090 RVA: 0x0003287D File Offset: 0x00030A7D
	public static void Stab(GameObject target, AudioClip audioclip, float delay)
	{
		DialogueriTween.Stab(target, DialogueriTween.Hash(new object[]
		{
			"audioclip",
			audioclip,
			"delay",
			delay
		}));
	}

	// Token: 0x06003EDB RID: 16091 RVA: 0x000328AD File Offset: 0x00030AAD
	public static void Stab(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "stab";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EDC RID: 16092 RVA: 0x000328CE File Offset: 0x00030ACE
	public static void LookFrom(GameObject target, Vector3 looktarget, float time)
	{
		DialogueriTween.LookFrom(target, DialogueriTween.Hash(new object[]
		{
			"looktarget",
			looktarget,
			"time",
			time
		}));
	}

	// Token: 0x06003EDD RID: 16093 RVA: 0x0011E6C0 File Offset: 0x0011C8C0
	public static void LookFrom(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		Vector3 eulerAngles = target.transform.eulerAngles;
		if (args["looktarget"].GetType() == typeof(Transform))
		{
			Transform transform = target.transform;
			Transform transform2 = (Transform)args["looktarget"];
			Vector3? vector = (Vector3?)args["up"];
			transform.LookAt(transform2, (vector == null) ? DialogueriTween.Defaults.up : vector.Value);
		}
		else if (args["looktarget"].GetType() == typeof(Vector3))
		{
			Transform transform3 = target.transform;
			Vector3 vector2 = (Vector3)args["looktarget"];
			Vector3? vector3 = (Vector3?)args["up"];
			transform3.LookAt(vector2, (vector3 == null) ? DialogueriTween.Defaults.up : vector3.Value);
		}
		if (args.Contains("axis"))
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			string text = (string)args["axis"];
			if (text != null)
			{
				if (!(text == "x"))
				{
					if (!(text == "y"))
					{
						if (text == "z")
						{
							eulerAngles2.x = eulerAngles.x;
							eulerAngles2.y = eulerAngles.y;
						}
					}
					else
					{
						eulerAngles2.x = eulerAngles.x;
						eulerAngles2.z = eulerAngles.z;
					}
				}
				else
				{
					eulerAngles2.y = eulerAngles.y;
					eulerAngles2.z = eulerAngles.z;
				}
			}
			target.transform.eulerAngles = eulerAngles2;
		}
		args["rotation"] = eulerAngles;
		args["type"] = "rotate";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EDE RID: 16094 RVA: 0x00032903 File Offset: 0x00030B03
	public static void LookTo(GameObject target, Vector3 looktarget, float time)
	{
		DialogueriTween.LookTo(target, DialogueriTween.Hash(new object[]
		{
			"looktarget",
			looktarget,
			"time",
			time
		}));
	}

	// Token: 0x06003EDF RID: 16095 RVA: 0x0011E8CC File Offset: 0x0011CACC
	public static void LookTo(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		if (args.Contains("looktarget") && args["looktarget"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["looktarget"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
		}
		args["type"] = "look";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EE0 RID: 16096 RVA: 0x00032938 File Offset: 0x00030B38
	public static void MoveTo(GameObject target, Vector3 position, float time)
	{
		DialogueriTween.MoveTo(target, DialogueriTween.Hash(new object[]
		{
			"position",
			position,
			"time",
			time
		}));
	}

	// Token: 0x06003EE1 RID: 16097 RVA: 0x0011E9CC File Offset: 0x0011CBCC
	public static void MoveTo(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		if (args.Contains("position") && args["position"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["position"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "move";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EE2 RID: 16098 RVA: 0x0003296D File Offset: 0x00030B6D
	public static void MoveFrom(GameObject target, Vector3 position, float time)
	{
		DialogueriTween.MoveFrom(target, DialogueriTween.Hash(new object[]
		{
			"position",
			position,
			"time",
			time
		}));
	}

	// Token: 0x06003EE3 RID: 16099 RVA: 0x0011EB0C File Offset: 0x0011CD0C
	public static void MoveFrom(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = DialogueriTween.Defaults.isLocal;
		}
		if (args.Contains("path"))
		{
			Vector3[] array2;
			if (args["path"].GetType() == typeof(Vector3[]))
			{
				Vector3[] array = (Vector3[])args["path"];
				array2 = new Vector3[array.Length];
				Array.Copy(array, array2, array.Length);
			}
			else
			{
				Transform[] array3 = (Transform[])args["path"];
				array2 = new Vector3[array3.Length];
				for (int i = 0; i < array3.Length; i++)
				{
					array2[i] = array3[i].position;
				}
			}
			if (array2[array2.Length - 1] != target.transform.position)
			{
				Vector3[] array4 = new Vector3[array2.Length + 1];
				Array.Copy(array2, array4, array2.Length);
				if (flag)
				{
					array4[array4.Length - 1] = target.transform.localPosition;
					target.transform.localPosition = array4[0];
				}
				else
				{
					array4[array4.Length - 1] = target.transform.position;
					target.transform.position = array4[0];
				}
				args["path"] = array4;
			}
			else
			{
				if (flag)
				{
					target.transform.localPosition = array2[0];
				}
				else
				{
					target.transform.position = array2[0];
				}
				args["path"] = array2;
			}
		}
		else
		{
			Vector3 vector2;
			Vector3 vector;
			if (flag)
			{
				vector = (vector2 = target.transform.localPosition);
			}
			else
			{
				vector = (vector2 = target.transform.position);
			}
			if (args.Contains("position"))
			{
				if (args["position"].GetType() == typeof(Transform))
				{
					Transform transform = (Transform)args["position"];
					vector = transform.position;
				}
				else if (args["position"].GetType() == typeof(Vector3))
				{
					vector = (Vector3)args["position"];
				}
			}
			else
			{
				if (args.Contains("x"))
				{
					vector.x = (float)args["x"];
				}
				if (args.Contains("y"))
				{
					vector.y = (float)args["y"];
				}
				if (args.Contains("z"))
				{
					vector.z = (float)args["z"];
				}
			}
			if (flag)
			{
				target.transform.localPosition = vector;
			}
			else
			{
				target.transform.position = vector;
			}
			args["position"] = vector2;
		}
		args["type"] = "move";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EE4 RID: 16100 RVA: 0x000329A2 File Offset: 0x00030BA2
	public static void MoveAdd(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.MoveAdd(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06003EE5 RID: 16101 RVA: 0x000329D7 File Offset: 0x00030BD7
	public static void MoveAdd(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "move";
		args["method"] = "add";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EE6 RID: 16102 RVA: 0x00032A08 File Offset: 0x00030C08
	public static void MoveBy(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.MoveBy(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06003EE7 RID: 16103 RVA: 0x00032A3D File Offset: 0x00030C3D
	public static void MoveBy(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "move";
		args["method"] = "by";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EE8 RID: 16104 RVA: 0x00032A6E File Offset: 0x00030C6E
	public static void ScaleTo(GameObject target, Vector3 scale, float time)
	{
		DialogueriTween.ScaleTo(target, DialogueriTween.Hash(new object[]
		{
			"scale",
			scale,
			"time",
			time
		}));
	}

	// Token: 0x06003EE9 RID: 16105 RVA: 0x0011EE78 File Offset: 0x0011D078
	public static void ScaleTo(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		if (args.Contains("scale") && args["scale"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["scale"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "scale";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EEA RID: 16106 RVA: 0x00032AA3 File Offset: 0x00030CA3
	public static void ScaleFrom(GameObject target, Vector3 scale, float time)
	{
		DialogueriTween.ScaleFrom(target, DialogueriTween.Hash(new object[]
		{
			"scale",
			scale,
			"time",
			time
		}));
	}

	// Token: 0x06003EEB RID: 16107 RVA: 0x0011EFB8 File Offset: 0x0011D1B8
	public static void ScaleFrom(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		Vector3 localScale2;
		Vector3 localScale = localScale2 = target.transform.localScale;
		if (args.Contains("scale"))
		{
			if (args["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["scale"];
				localScale = transform.localScale;
			}
			else if (args["scale"].GetType() == typeof(Vector3))
			{
				localScale = (Vector3)args["scale"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				localScale.x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				localScale.y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				localScale.z = (float)args["z"];
			}
		}
		target.transform.localScale = localScale;
		args["scale"] = localScale2;
		args["type"] = "scale";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EEC RID: 16108 RVA: 0x00032AD8 File Offset: 0x00030CD8
	public static void ScaleAdd(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.ScaleAdd(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06003EED RID: 16109 RVA: 0x00032B0D File Offset: 0x00030D0D
	public static void ScaleAdd(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "scale";
		args["method"] = "add";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EEE RID: 16110 RVA: 0x00032B3E File Offset: 0x00030D3E
	public static void ScaleBy(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.ScaleBy(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06003EEF RID: 16111 RVA: 0x00032B73 File Offset: 0x00030D73
	public static void ScaleBy(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "scale";
		args["method"] = "by";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EF0 RID: 16112 RVA: 0x00032BA4 File Offset: 0x00030DA4
	public static void RotateTo(GameObject target, Vector3 rotation, float time)
	{
		DialogueriTween.RotateTo(target, DialogueriTween.Hash(new object[]
		{
			"rotation",
			rotation,
			"time",
			time
		}));
	}

	// Token: 0x06003EF1 RID: 16113 RVA: 0x0011F118 File Offset: 0x0011D318
	public static void RotateTo(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		if (args.Contains("rotation") && args["rotation"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["rotation"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "rotate";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EF2 RID: 16114 RVA: 0x00032BD9 File Offset: 0x00030DD9
	public static void RotateFrom(GameObject target, Vector3 rotation, float time)
	{
		DialogueriTween.RotateFrom(target, DialogueriTween.Hash(new object[]
		{
			"rotation",
			rotation,
			"time",
			time
		}));
	}

	// Token: 0x06003EF3 RID: 16115 RVA: 0x0011F258 File Offset: 0x0011D458
	public static void RotateFrom(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = DialogueriTween.Defaults.isLocal;
		}
		Vector3 vector2;
		Vector3 vector;
		if (flag)
		{
			vector = (vector2 = target.transform.localEulerAngles);
		}
		else
		{
			vector = (vector2 = target.transform.eulerAngles);
		}
		if (args.Contains("rotation"))
		{
			if (args["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["rotation"];
				vector = transform.eulerAngles;
			}
			else if (args["rotation"].GetType() == typeof(Vector3))
			{
				vector = (Vector3)args["rotation"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				vector.x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				vector.y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				vector.z = (float)args["z"];
			}
		}
		if (flag)
		{
			target.transform.localEulerAngles = vector;
		}
		else
		{
			target.transform.eulerAngles = vector;
		}
		args["rotation"] = vector2;
		args["type"] = "rotate";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EF4 RID: 16116 RVA: 0x00032C0E File Offset: 0x00030E0E
	public static void RotateAdd(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.RotateAdd(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06003EF5 RID: 16117 RVA: 0x00032C43 File Offset: 0x00030E43
	public static void RotateAdd(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "rotate";
		args["method"] = "add";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EF6 RID: 16118 RVA: 0x00032C74 File Offset: 0x00030E74
	public static void RotateBy(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.RotateBy(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06003EF7 RID: 16119 RVA: 0x00032CA9 File Offset: 0x00030EA9
	public static void RotateBy(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "rotate";
		args["method"] = "by";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EF8 RID: 16120 RVA: 0x00032CDA File Offset: 0x00030EDA
	public static void ShakePosition(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.ShakePosition(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06003EF9 RID: 16121 RVA: 0x00032D0F File Offset: 0x00030F0F
	public static void ShakePosition(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "position";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EFA RID: 16122 RVA: 0x00032D40 File Offset: 0x00030F40
	public static void ShakeScale(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.ShakeScale(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06003EFB RID: 16123 RVA: 0x00032D75 File Offset: 0x00030F75
	public static void ShakeScale(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "scale";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EFC RID: 16124 RVA: 0x00032DA6 File Offset: 0x00030FA6
	public static void ShakeRotation(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.ShakeRotation(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06003EFD RID: 16125 RVA: 0x00032DDB File Offset: 0x00030FDB
	public static void ShakeRotation(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "rotation";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003EFE RID: 16126 RVA: 0x00032E0C File Offset: 0x0003100C
	public static void PunchPosition(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.PunchPosition(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06003EFF RID: 16127 RVA: 0x0011F414 File Offset: 0x0011D614
	public static void PunchPosition(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "position";
		args["easetype"] = DialogueriTween.EaseType.punch;
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003F00 RID: 16128 RVA: 0x00032E41 File Offset: 0x00031041
	public static void PunchRotation(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.PunchRotation(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06003F01 RID: 16129 RVA: 0x0011F464 File Offset: 0x0011D664
	public static void PunchRotation(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "rotation";
		args["easetype"] = DialogueriTween.EaseType.punch;
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003F02 RID: 16130 RVA: 0x00032E76 File Offset: 0x00031076
	public static void PunchScale(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.PunchScale(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06003F03 RID: 16131 RVA: 0x0011F4B4 File Offset: 0x0011D6B4
	public static void PunchScale(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "scale";
		args["easetype"] = DialogueriTween.EaseType.punch;
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06003F04 RID: 16132 RVA: 0x0011F504 File Offset: 0x0011D704
	public void GenerateTargets()
	{
		string text = this.type;
		switch (text)
		{
		case "value":
		{
			string text2 = this.method;
			if (text2 != null)
			{
				if (!(text2 == "float"))
				{
					if (!(text2 == "vector2"))
					{
						if (!(text2 == "vector3"))
						{
							if (!(text2 == "color"))
							{
								if (text2 == "rect")
								{
									this.GenerateRectTargets();
									this.apply = new DialogueriTween.ApplyTween(this.ApplyRectTargets);
								}
							}
							else
							{
								this.GenerateColorTargets();
								this.apply = new DialogueriTween.ApplyTween(this.ApplyColorTargets);
							}
						}
						else
						{
							this.GenerateVector3Targets();
							this.apply = new DialogueriTween.ApplyTween(this.ApplyVector3Targets);
						}
					}
					else
					{
						this.GenerateVector2Targets();
						this.apply = new DialogueriTween.ApplyTween(this.ApplyVector2Targets);
					}
				}
				else
				{
					this.GenerateFloatTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyFloatTargets);
				}
			}
			break;
		}
		case "color":
		{
			string text3 = this.method;
			if (text3 != null)
			{
				if (text3 == "to")
				{
					this.GenerateColorToTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyColorToTargets);
				}
			}
			break;
		}
		case "audio":
		{
			string text4 = this.method;
			if (text4 != null)
			{
				if (text4 == "to")
				{
					this.GenerateAudioToTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyAudioToTargets);
				}
			}
			break;
		}
		case "move":
		{
			string text5 = this.method;
			if (text5 != null)
			{
				if (!(text5 == "to"))
				{
					if (text5 == "by" || text5 == "add")
					{
						this.GenerateMoveByTargets();
						this.apply = new DialogueriTween.ApplyTween(this.ApplyMoveByTargets);
					}
				}
				else if (this.tweenArguments.Contains("path"))
				{
					this.GenerateMoveToPathTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyMoveToPathTargets);
				}
				else
				{
					this.GenerateMoveToTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyMoveToTargets);
				}
			}
			break;
		}
		case "scale":
		{
			string text6 = this.method;
			if (text6 != null)
			{
				if (!(text6 == "to"))
				{
					if (!(text6 == "by"))
					{
						if (text6 == "add")
						{
							this.GenerateScaleAddTargets();
							this.apply = new DialogueriTween.ApplyTween(this.ApplyScaleToTargets);
						}
					}
					else
					{
						this.GenerateScaleByTargets();
						this.apply = new DialogueriTween.ApplyTween(this.ApplyScaleToTargets);
					}
				}
				else
				{
					this.GenerateScaleToTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyScaleToTargets);
				}
			}
			break;
		}
		case "rotate":
		{
			string text7 = this.method;
			if (text7 != null)
			{
				if (!(text7 == "to"))
				{
					if (!(text7 == "add"))
					{
						if (text7 == "by")
						{
							this.GenerateRotateByTargets();
							this.apply = new DialogueriTween.ApplyTween(this.ApplyRotateAddTargets);
						}
					}
					else
					{
						this.GenerateRotateAddTargets();
						this.apply = new DialogueriTween.ApplyTween(this.ApplyRotateAddTargets);
					}
				}
				else
				{
					this.GenerateRotateToTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyRotateToTargets);
				}
			}
			break;
		}
		case "shake":
		{
			string text8 = this.method;
			if (text8 != null)
			{
				if (!(text8 == "position"))
				{
					if (!(text8 == "scale"))
					{
						if (text8 == "rotation")
						{
							this.GenerateShakeRotationTargets();
							this.apply = new DialogueriTween.ApplyTween(this.ApplyShakeRotationTargets);
						}
					}
					else
					{
						this.GenerateShakeScaleTargets();
						this.apply = new DialogueriTween.ApplyTween(this.ApplyShakeScaleTargets);
					}
				}
				else
				{
					this.GenerateShakePositionTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyShakePositionTargets);
				}
			}
			break;
		}
		case "punch":
		{
			string text9 = this.method;
			if (text9 != null)
			{
				if (!(text9 == "position"))
				{
					if (!(text9 == "rotation"))
					{
						if (text9 == "scale")
						{
							this.GeneratePunchScaleTargets();
							this.apply = new DialogueriTween.ApplyTween(this.ApplyPunchScaleTargets);
						}
					}
					else
					{
						this.GeneratePunchRotationTargets();
						this.apply = new DialogueriTween.ApplyTween(this.ApplyPunchRotationTargets);
					}
				}
				else
				{
					this.GeneratePunchPositionTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyPunchPositionTargets);
				}
			}
			break;
		}
		case "look":
		{
			string text10 = this.method;
			if (text10 != null)
			{
				if (text10 == "to")
				{
					this.GenerateLookToTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyLookToTargets);
				}
			}
			break;
		}
		case "stab":
			this.GenerateStabTargets();
			this.apply = new DialogueriTween.ApplyTween(this.ApplyStabTargets);
			break;
		}
	}

	// Token: 0x06003F05 RID: 16133 RVA: 0x0011FB24 File Offset: 0x0011DD24
	public void GenerateRectTargets()
	{
		this.rects = new Rect[3];
		this.rects[0] = (Rect)this.tweenArguments["from"];
		this.rects[1] = (Rect)this.tweenArguments["to"];
	}

	// Token: 0x06003F06 RID: 16134 RVA: 0x0011FB8C File Offset: 0x0011DD8C
	public void GenerateColorTargets()
	{
		this.colors = new Color[1, 3];
		this.colors[0, 0] = (Color)this.tweenArguments["from"];
		this.colors[0, 1] = (Color)this.tweenArguments["to"];
	}

	// Token: 0x06003F07 RID: 16135 RVA: 0x0011FBF4 File Offset: 0x0011DDF4
	public void GenerateVector3Targets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (Vector3)this.tweenArguments["from"];
		this.vector3s[1] = (Vector3)this.tweenArguments["to"];
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06003F08 RID: 16136 RVA: 0x0011FCB8 File Offset: 0x0011DEB8
	public void GenerateVector2Targets()
	{
		this.vector2s = new Vector2[3];
		this.vector2s[0] = (Vector2)this.tweenArguments["from"];
		this.vector2s[1] = (Vector2)this.tweenArguments["to"];
		if (this.tweenArguments.Contains("speed"))
		{
			Vector3 vector;
			vector..ctor(this.vector2s[0].x, this.vector2s[0].y, 0f);
			Vector3 vector2;
			vector2..ctor(this.vector2s[1].x, this.vector2s[1].y, 0f);
			float num = Math.Abs(Vector3.Distance(vector, vector2));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06003F09 RID: 16137 RVA: 0x0011FDB8 File Offset: 0x0011DFB8
	public void GenerateFloatTargets()
	{
		this.floats = new float[3];
		this.floats[0] = (float)this.tweenArguments["from"];
		this.floats[1] = (float)this.tweenArguments["to"];
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(this.floats[0] - this.floats[1]);
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06003F0A RID: 16138 RVA: 0x0011FE54 File Offset: 0x0011E054
	public void GenerateColorToTargets()
	{
		if (base.GetComponent(typeof(GUITexture)))
		{
			this.colors = new Color[1, 3];
			this.colors[0, 0] = (this.colors[0, 1] = base.GetComponent<GUITexture>().color);
		}
		else if (base.GetComponent(typeof(GUIText)))
		{
			this.colors = new Color[1, 3];
			this.colors[0, 0] = (this.colors[0, 1] = base.GetComponent<GUIText>().material.color);
		}
		else if (base.GetComponent<Renderer>())
		{
			this.colors = new Color[base.GetComponent<Renderer>().materials.Length, 3];
			for (int i = 0; i < base.GetComponent<Renderer>().materials.Length; i++)
			{
				this.colors[i, 0] = base.GetComponent<Renderer>().materials[i].GetColor(this.namedcolorvalue.ToString());
				this.colors[i, 1] = base.GetComponent<Renderer>().materials[i].GetColor(this.namedcolorvalue.ToString());
			}
		}
		else if (base.GetComponent<Light>())
		{
			this.colors = new Color[1, 3];
			this.colors[0, 0] = (this.colors[0, 1] = base.GetComponent<Light>().color);
		}
		else
		{
			this.colors = new Color[1, 3];
		}
		if (this.tweenArguments.Contains("color"))
		{
			for (int j = 0; j < this.colors.GetLength(0); j++)
			{
				this.colors[j, 1] = (Color)this.tweenArguments["color"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("r"))
			{
				for (int k = 0; k < this.colors.GetLength(0); k++)
				{
					this.colors[k, 1].r = (float)this.tweenArguments["r"];
				}
			}
			if (this.tweenArguments.Contains("g"))
			{
				for (int l = 0; l < this.colors.GetLength(0); l++)
				{
					this.colors[l, 1].g = (float)this.tweenArguments["g"];
				}
			}
			if (this.tweenArguments.Contains("b"))
			{
				for (int m = 0; m < this.colors.GetLength(0); m++)
				{
					this.colors[m, 1].b = (float)this.tweenArguments["b"];
				}
			}
			if (this.tweenArguments.Contains("a"))
			{
				for (int n = 0; n < this.colors.GetLength(0); n++)
				{
					this.colors[n, 1].a = (float)this.tweenArguments["a"];
				}
			}
		}
		if (this.tweenArguments.Contains("amount"))
		{
			for (int num = 0; num < this.colors.GetLength(0); num++)
			{
				this.colors[num, 1].a = (float)this.tweenArguments["amount"];
			}
		}
		else if (this.tweenArguments.Contains("alpha"))
		{
			for (int num2 = 0; num2 < this.colors.GetLength(0); num2++)
			{
				this.colors[num2, 1].a = (float)this.tweenArguments["alpha"];
			}
		}
	}

	// Token: 0x06003F0B RID: 16139 RVA: 0x001202CC File Offset: 0x0011E4CC
	public void GenerateAudioToTargets()
	{
		this.vector2s = new Vector2[3];
		if (this.tweenArguments.Contains("audiosource"))
		{
			this.audioSource = (AudioSource)this.tweenArguments["audiosource"];
		}
		else if (base.GetComponent(typeof(AudioSource)))
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		else
		{
			Debug.LogError("iTween Error: AudioTo requires an AudioSource.", null);
			this.Dispose();
		}
		this.vector2s[0] = (this.vector2s[1] = new Vector2(this.audioSource.volume, this.audioSource.pitch));
		if (this.tweenArguments.Contains("volume"))
		{
			this.vector2s[1].x = (float)this.tweenArguments["volume"];
		}
		if (this.tweenArguments.Contains("pitch"))
		{
			this.vector2s[1].y = (float)this.tweenArguments["pitch"];
		}
	}

	// Token: 0x06003F0C RID: 16140 RVA: 0x0012040C File Offset: 0x0011E60C
	public void GenerateStabTargets()
	{
		if (this.tweenArguments.Contains("audiosource"))
		{
			this.audioSource = (AudioSource)this.tweenArguments["audiosource"];
		}
		else if (base.GetComponent(typeof(AudioSource)))
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		else
		{
			base.gameObject.AddComponent(typeof(AudioSource));
			this.audioSource = base.GetComponent<AudioSource>();
			this.audioSource.playOnAwake = false;
		}
		this.audioSource.clip = (AudioClip)this.tweenArguments["audioclip"];
		if (this.tweenArguments.Contains("pitch"))
		{
			this.audioSource.pitch = (float)this.tweenArguments["pitch"];
		}
		if (this.tweenArguments.Contains("volume"))
		{
			this.audioSource.volume = (float)this.tweenArguments["volume"];
		}
		this.time = this.audioSource.clip.length / this.audioSource.pitch;
	}

	// Token: 0x06003F0D RID: 16141 RVA: 0x00120554 File Offset: 0x0011E754
	public void GenerateLookToTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = base.transform.eulerAngles;
		if (this.tweenArguments.Contains("looktarget"))
		{
			if (this.tweenArguments["looktarget"].GetType() == typeof(Transform))
			{
				Transform transform = base.transform;
				Transform transform2 = (Transform)this.tweenArguments["looktarget"];
				Vector3? vector = (Vector3?)this.tweenArguments["up"];
				transform.LookAt(transform2, (vector == null) ? DialogueriTween.Defaults.up : vector.Value);
			}
			else if (this.tweenArguments["looktarget"].GetType() == typeof(Vector3))
			{
				Transform transform3 = base.transform;
				Vector3 vector2 = (Vector3)this.tweenArguments["looktarget"];
				Vector3? vector3 = (Vector3?)this.tweenArguments["up"];
				transform3.LookAt(vector2, (vector3 == null) ? DialogueriTween.Defaults.up : vector3.Value);
			}
		}
		else
		{
			Debug.LogError("iTween Error: LookTo needs a 'looktarget' property!", null);
			this.Dispose();
		}
		this.vector3s[1] = base.transform.eulerAngles;
		base.transform.eulerAngles = this.vector3s[0];
		if (this.tweenArguments.Contains("axis"))
		{
			string text = (string)this.tweenArguments["axis"];
			if (text != null)
			{
				if (!(text == "x"))
				{
					if (!(text == "y"))
					{
						if (text == "z")
						{
							this.vector3s[1].x = this.vector3s[0].x;
							this.vector3s[1].y = this.vector3s[0].y;
						}
					}
					else
					{
						this.vector3s[1].x = this.vector3s[0].x;
						this.vector3s[1].z = this.vector3s[0].z;
					}
				}
				else
				{
					this.vector3s[1].y = this.vector3s[0].y;
					this.vector3s[1].z = this.vector3s[0].z;
				}
			}
		}
		this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06003F0E RID: 16142 RVA: 0x0012091C File Offset: 0x0011EB1C
	public void GenerateMoveToPathTargets()
	{
		Vector3[] array2;
		if (this.tweenArguments["path"].GetType() == typeof(Vector3[]))
		{
			Vector3[] array = (Vector3[])this.tweenArguments["path"];
			if (array.Length == 1)
			{
				Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!", null);
				this.Dispose();
			}
			array2 = new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
		}
		else
		{
			Transform[] array3 = (Transform[])this.tweenArguments["path"];
			if (array3.Length == 1)
			{
				Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!", null);
				this.Dispose();
			}
			array2 = new Vector3[array3.Length];
			for (int i = 0; i < array3.Length; i++)
			{
				array2[i] = array3[i].position;
			}
		}
		bool flag;
		int num;
		if (base.transform.position != array2[0])
		{
			if (!this.tweenArguments.Contains("movetopath") || (bool)this.tweenArguments["movetopath"])
			{
				flag = true;
				num = 3;
			}
			else
			{
				flag = false;
				num = 2;
			}
		}
		else
		{
			flag = false;
			num = 2;
		}
		this.vector3s = new Vector3[array2.Length + num];
		if (flag)
		{
			this.vector3s[1] = base.transform.position;
			num = 2;
		}
		else
		{
			num = 1;
		}
		Array.Copy(array2, 0, this.vector3s, num, array2.Length);
		this.vector3s[0] = this.vector3s[1] + (this.vector3s[1] - this.vector3s[2]);
		this.vector3s[this.vector3s.Length - 1] = this.vector3s[this.vector3s.Length - 2] + (this.vector3s[this.vector3s.Length - 2] - this.vector3s[this.vector3s.Length - 3]);
		if (this.vector3s[1] == this.vector3s[this.vector3s.Length - 2])
		{
			Vector3[] array4 = new Vector3[this.vector3s.Length];
			Array.Copy(this.vector3s, array4, this.vector3s.Length);
			array4[0] = array4[array4.Length - 3];
			array4[array4.Length - 1] = array4[2];
			this.vector3s = new Vector3[array4.Length];
			Array.Copy(array4, this.vector3s, array4.Length);
		}
		this.path = new DialogueriTween.CRSpline(this.vector3s);
		if (this.tweenArguments.Contains("speed"))
		{
			float num2 = DialogueriTween.PathLength(this.vector3s);
			this.time = num2 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06003F0F RID: 16143 RVA: 0x00120C7C File Offset: 0x0011EE7C
	public void GenerateMoveToTargets()
	{
		this.vector3s = new Vector3[3];
		if (this.isLocal)
		{
			this.vector3s[0] = (this.vector3s[1] = base.transform.localPosition);
		}
		else
		{
			this.vector3s[0] = (this.vector3s[1] = base.transform.position);
		}
		if (this.tweenArguments.Contains("position"))
		{
			if (this.tweenArguments["position"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)this.tweenArguments["position"];
				this.vector3s[1] = transform.position;
			}
			else if (this.tweenArguments["position"].GetType() == typeof(Vector3))
			{
				this.vector3s[1] = (Vector3)this.tweenArguments["position"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
		{
			this.tweenArguments["looktarget"] = this.vector3s[1];
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06003F10 RID: 16144 RVA: 0x00120F24 File Offset: 0x0011F124
	public void GenerateMoveByTargets()
	{
		this.vector3s = new Vector3[6];
		this.vector3s[4] = base.transform.eulerAngles;
		this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = base.transform.position));
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = this.vector3s[0] + (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = this.vector3s[0].x + (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = this.vector3s[0].y + (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = this.vector3s[0].z + (float)this.tweenArguments["z"];
			}
		}
		base.transform.Translate(this.vector3s[1], this.space);
		this.vector3s[5] = base.transform.position;
		base.transform.position = this.vector3s[0];
		if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
		{
			this.tweenArguments["looktarget"] = this.vector3s[1];
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06003F11 RID: 16145 RVA: 0x001211E8 File Offset: 0x0011F3E8
	public void GenerateScaleToTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (this.vector3s[1] = base.transform.localScale);
		if (this.tweenArguments.Contains("scale"))
		{
			if (this.tweenArguments["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)this.tweenArguments["scale"];
				this.vector3s[1] = transform.localScale;
			}
			else if (this.tweenArguments["scale"].GetType() == typeof(Vector3))
			{
				this.vector3s[1] = (Vector3)this.tweenArguments["scale"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06003F12 RID: 16146 RVA: 0x001213FC File Offset: 0x0011F5FC
	public void GenerateScaleByTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (this.vector3s[1] = base.transform.localScale);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = Vector3.Scale(this.vector3s[1], (Vector3)this.tweenArguments["amount"]);
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x * (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y * (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z * (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06003F13 RID: 16147 RVA: 0x001215C0 File Offset: 0x0011F7C0
	public void GenerateScaleAddTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (this.vector3s[1] = base.transform.localScale);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] += (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x + (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y + (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z + (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06003F14 RID: 16148 RVA: 0x0012177C File Offset: 0x0011F97C
	public void GenerateRotateToTargets()
	{
		this.vector3s = new Vector3[3];
		if (this.isLocal)
		{
			this.vector3s[0] = (this.vector3s[1] = base.transform.localEulerAngles);
		}
		else
		{
			this.vector3s[0] = (this.vector3s[1] = base.transform.eulerAngles);
		}
		if (this.tweenArguments.Contains("rotation"))
		{
			if (this.tweenArguments["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)this.tweenArguments["rotation"];
				this.vector3s[1] = transform.eulerAngles;
			}
			else if (this.tweenArguments["rotation"].GetType() == typeof(Vector3))
			{
				this.vector3s[1] = (Vector3)this.tweenArguments["rotation"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
		this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06003F15 RID: 16149 RVA: 0x00121A6C File Offset: 0x0011FC6C
	public void GenerateRotateAddTargets()
	{
		this.vector3s = new Vector3[5];
		this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = base.transform.eulerAngles));
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] += (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x + (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y + (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z + (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06003F16 RID: 16150 RVA: 0x00121C3C File Offset: 0x0011FE3C
	public void GenerateRotateByTargets()
	{
		this.vector3s = new Vector3[4];
		this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = base.transform.eulerAngles));
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] += Vector3.Scale((Vector3)this.tweenArguments["amount"], new Vector3(360f, 360f, 360f));
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x + 360f * (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y + 360f * (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z + 360f * (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06003F17 RID: 16151 RVA: 0x00121E34 File Offset: 0x00120034
	public void GenerateShakePositionTargets()
	{
		this.vector3s = new Vector3[4];
		this.vector3s[3] = base.transform.eulerAngles;
		this.vector3s[0] = base.transform.position;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x06003F18 RID: 16152 RVA: 0x00121F78 File Offset: 0x00120178
	public void GenerateShakeScaleTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = base.transform.localScale;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x06003F19 RID: 16153 RVA: 0x001220A0 File Offset: 0x001202A0
	public void GenerateShakeRotationTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = base.transform.eulerAngles;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x06003F1A RID: 16154 RVA: 0x001221C8 File Offset: 0x001203C8
	public void GeneratePunchPositionTargets()
	{
		this.vector3s = new Vector3[5];
		this.vector3s[4] = base.transform.eulerAngles;
		this.vector3s[0] = base.transform.position;
		this.vector3s[1] = (this.vector3s[3] = Vector3.zero);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x06003F1B RID: 16155 RVA: 0x00122334 File Offset: 0x00120534
	public void GeneratePunchRotationTargets()
	{
		this.vector3s = new Vector3[4];
		this.vector3s[0] = base.transform.eulerAngles;
		this.vector3s[1] = (this.vector3s[3] = Vector3.zero);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x06003F1C RID: 16156 RVA: 0x00122484 File Offset: 0x00120684
	public void GeneratePunchScaleTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = base.transform.localScale;
		this.vector3s[1] = Vector3.zero;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x06003F1D RID: 16157 RVA: 0x001225C0 File Offset: 0x001207C0
	public void ApplyRectTargets()
	{
		this.rects[2].x = this.ease(this.rects[0].x, this.rects[1].x, this.percentage);
		this.rects[2].y = this.ease(this.rects[0].y, this.rects[1].y, this.percentage);
		this.rects[2].width = this.ease(this.rects[0].width, this.rects[1].width, this.percentage);
		this.rects[2].height = this.ease(this.rects[0].height, this.rects[1].height, this.percentage);
		this.tweenArguments["onupdateparams"] = this.rects[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.rects[1];
		}
	}

	// Token: 0x06003F1E RID: 16158 RVA: 0x0012273C File Offset: 0x0012093C
	public void ApplyColorTargets()
	{
		this.colors[0, 2].r = this.ease(this.colors[0, 0].r, this.colors[0, 1].r, this.percentage);
		this.colors[0, 2].g = this.ease(this.colors[0, 0].g, this.colors[0, 1].g, this.percentage);
		this.colors[0, 2].b = this.ease(this.colors[0, 0].b, this.colors[0, 1].b, this.percentage);
		this.colors[0, 2].a = this.ease(this.colors[0, 0].a, this.colors[0, 1].a, this.percentage);
		this.tweenArguments["onupdateparams"] = this.colors[0, 2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.colors[0, 1];
		}
	}

	// Token: 0x06003F1F RID: 16159 RVA: 0x001228BC File Offset: 0x00120ABC
	public void ApplyVector3Targets()
	{
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		this.tweenArguments["onupdateparams"] = this.vector3s[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.vector3s[1];
		}
	}

	// Token: 0x06003F20 RID: 16160 RVA: 0x001229F4 File Offset: 0x00120BF4
	public void ApplyVector2Targets()
	{
		this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
		this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
		this.tweenArguments["onupdateparams"] = this.vector2s[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.vector2s[1];
		}
	}

	// Token: 0x06003F21 RID: 16161 RVA: 0x00122AE8 File Offset: 0x00120CE8
	public void ApplyFloatTargets()
	{
		this.floats[2] = this.ease(this.floats[0], this.floats[1], this.percentage);
		this.tweenArguments["onupdateparams"] = this.floats[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.floats[1];
		}
	}

	// Token: 0x06003F22 RID: 16162 RVA: 0x00122B68 File Offset: 0x00120D68
	public void ApplyColorToTargets()
	{
		for (int i = 0; i < this.colors.GetLength(0); i++)
		{
			this.colors[i, 2].r = this.ease(this.colors[i, 0].r, this.colors[i, 1].r, this.percentage);
			this.colors[i, 2].g = this.ease(this.colors[i, 0].g, this.colors[i, 1].g, this.percentage);
			this.colors[i, 2].b = this.ease(this.colors[i, 0].b, this.colors[i, 1].b, this.percentage);
			this.colors[i, 2].a = this.ease(this.colors[i, 0].a, this.colors[i, 1].a, this.percentage);
		}
		if (base.GetComponent(typeof(GUITexture)))
		{
			base.GetComponent<GUITexture>().color = this.colors[0, 2];
		}
		else if (base.GetComponent(typeof(GUIText)))
		{
			base.GetComponent<GUIText>().material.color = this.colors[0, 2];
		}
		else if (base.GetComponent<Renderer>())
		{
			for (int j = 0; j < this.colors.GetLength(0); j++)
			{
				base.GetComponent<Renderer>().materials[j].SetColor(this.namedcolorvalue.ToString(), this.colors[j, 2]);
			}
		}
		else if (base.GetComponent<Light>())
		{
			base.GetComponent<Light>().color = this.colors[0, 2];
		}
		if (this.percentage == 1f)
		{
			if (base.GetComponent(typeof(GUITexture)))
			{
				base.GetComponent<GUITexture>().color = this.colors[0, 1];
			}
			else if (base.GetComponent(typeof(GUIText)))
			{
				base.GetComponent<GUIText>().material.color = this.colors[0, 1];
			}
			else if (base.GetComponent<Renderer>())
			{
				for (int k = 0; k < this.colors.GetLength(0); k++)
				{
					base.GetComponent<Renderer>().materials[k].SetColor(this.namedcolorvalue.ToString(), this.colors[k, 1]);
				}
			}
			else if (base.GetComponent<Light>())
			{
				base.GetComponent<Light>().color = this.colors[0, 1];
			}
		}
	}

	// Token: 0x06003F23 RID: 16163 RVA: 0x00122EB8 File Offset: 0x001210B8
	public void ApplyAudioToTargets()
	{
		this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
		this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
		this.audioSource.volume = this.vector2s[2].x;
		this.audioSource.pitch = this.vector2s[2].y;
		if (this.percentage == 1f)
		{
			this.audioSource.volume = this.vector2s[1].x;
			this.audioSource.pitch = this.vector2s[1].y;
		}
	}

	// Token: 0x06003F24 RID: 16164 RVA: 0x00032EAB File Offset: 0x000310AB
	public void ApplyStabTargets()
	{
	}

	// Token: 0x06003F25 RID: 16165 RVA: 0x00122FD0 File Offset: 0x001211D0
	public void ApplyMoveToPathTargets()
	{
		this.preUpdate = base.transform.position;
		float num = this.ease(0f, 1f, this.percentage);
		if (this.isLocal)
		{
			base.transform.localPosition = this.path.Interp(Mathf.Clamp(num, 0f, 1f));
		}
		else
		{
			base.transform.position = this.path.Interp(Mathf.Clamp(num, 0f, 1f));
		}
		if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
		{
			float num2;
			if (this.tweenArguments.Contains("lookahead"))
			{
				num2 = (float)this.tweenArguments["lookahead"];
			}
			else
			{
				num2 = DialogueriTween.Defaults.lookAhead;
			}
			float num3 = this.ease(0f, 1f, Mathf.Min(1f, this.percentage + num2));
			this.tweenArguments["looktarget"] = this.path.Interp(Mathf.Clamp(num3, 0f, 1f));
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x06003F26 RID: 16166 RVA: 0x00123164 File Offset: 0x00121364
	public void ApplyMoveToTargets()
	{
		this.preUpdate = base.transform.position;
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		if (this.isLocal)
		{
			base.transform.localPosition = this.vector3s[2];
		}
		else
		{
			base.transform.position = this.vector3s[2];
		}
		if (this.percentage == 1f)
		{
			if (this.isLocal)
			{
				base.transform.localPosition = this.vector3s[1];
			}
			else
			{
				base.transform.position = this.vector3s[1];
			}
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x06003F27 RID: 16167 RVA: 0x0012332C File Offset: 0x0012152C
	public void ApplyMoveByTargets()
	{
		this.preUpdate = base.transform.position;
		Vector3 eulerAngles = default(Vector3);
		if (this.tweenArguments.Contains("looktarget"))
		{
			eulerAngles = base.transform.eulerAngles;
			base.transform.eulerAngles = this.vector3s[4];
		}
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		base.transform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		if (this.tweenArguments.Contains("looktarget"))
		{
			base.transform.eulerAngles = eulerAngles;
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x06003F28 RID: 16168 RVA: 0x00123514 File Offset: 0x00121714
	public void ApplyScaleToTargets()
	{
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		base.transform.localScale = this.vector3s[2];
		if (this.percentage == 1f)
		{
			base.transform.localScale = this.vector3s[1];
		}
	}

	// Token: 0x06003F29 RID: 16169 RVA: 0x00123638 File Offset: 0x00121838
	public void ApplyLookToTargets()
	{
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		if (this.isLocal)
		{
			base.transform.localRotation = Quaternion.Euler(this.vector3s[2]);
		}
		else
		{
			base.transform.rotation = Quaternion.Euler(this.vector3s[2]);
		}
	}

	// Token: 0x06003F2A RID: 16170 RVA: 0x00123764 File Offset: 0x00121964
	public void ApplyRotateToTargets()
	{
		this.preUpdate = base.transform.eulerAngles;
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		if (this.isLocal)
		{
			base.transform.localRotation = Quaternion.Euler(this.vector3s[2]);
		}
		else
		{
			base.transform.rotation = Quaternion.Euler(this.vector3s[2]);
		}
		if (this.percentage == 1f)
		{
			if (this.isLocal)
			{
				base.transform.localRotation = Quaternion.Euler(this.vector3s[1]);
			}
			else
			{
				base.transform.rotation = Quaternion.Euler(this.vector3s[1]);
			}
		}
		this.postUpdate = base.transform.eulerAngles;
		if (this.physics)
		{
			base.transform.eulerAngles = this.preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x06003F2B RID: 16171 RVA: 0x00123948 File Offset: 0x00121B48
	public void ApplyRotateAddTargets()
	{
		this.preUpdate = base.transform.eulerAngles;
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		base.transform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		this.postUpdate = base.transform.eulerAngles;
		if (this.physics)
		{
			base.transform.eulerAngles = this.preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x06003F2C RID: 16172 RVA: 0x00123AD0 File Offset: 0x00121CD0
	public void ApplyShakePositionTargets()
	{
		if (this.isLocal)
		{
			this.preUpdate = base.transform.localPosition;
		}
		else
		{
			this.preUpdate = base.transform.position;
		}
		Vector3 eulerAngles = default(Vector3);
		if (this.tweenArguments.Contains("looktarget"))
		{
			eulerAngles = base.transform.eulerAngles;
			base.transform.eulerAngles = this.vector3s[3];
		}
		if (this.percentage == 0f)
		{
			base.transform.Translate(this.vector3s[1], this.space);
		}
		if (this.isLocal)
		{
			base.transform.localPosition = this.vector3s[0];
		}
		else
		{
			base.transform.position = this.vector3s[0];
		}
		float num = 1f - this.percentage;
		this.vector3s[2].x = Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
		this.vector3s[2].y = Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
		this.vector3s[2].z = Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
		if (this.isLocal)
		{
			base.transform.localPosition += this.vector3s[2];
		}
		else
		{
			base.transform.position += this.vector3s[2];
		}
		if (this.tweenArguments.Contains("looktarget"))
		{
			base.transform.eulerAngles = eulerAngles;
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x06003F2D RID: 16173 RVA: 0x00123D50 File Offset: 0x00121F50
	public void ApplyShakeScaleTargets()
	{
		if (this.percentage == 0f)
		{
			base.transform.localScale = this.vector3s[1];
		}
		base.transform.localScale = this.vector3s[0];
		float num = 1f - this.percentage;
		this.vector3s[2].x = Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
		this.vector3s[2].y = Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
		this.vector3s[2].z = Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
		base.transform.localScale += this.vector3s[2];
	}

	// Token: 0x06003F2E RID: 16174 RVA: 0x00123E90 File Offset: 0x00122090
	public void ApplyShakeRotationTargets()
	{
		this.preUpdate = base.transform.eulerAngles;
		if (this.percentage == 0f)
		{
			base.transform.Rotate(this.vector3s[1], this.space);
		}
		base.transform.eulerAngles = this.vector3s[0];
		float num = 1f - this.percentage;
		this.vector3s[2].x = Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
		this.vector3s[2].y = Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
		this.vector3s[2].z = Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
		base.transform.Rotate(this.vector3s[2], this.space);
		this.postUpdate = base.transform.eulerAngles;
		if (this.physics)
		{
			base.transform.eulerAngles = this.preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x06003F2F RID: 16175 RVA: 0x00124028 File Offset: 0x00122228
	public void ApplyPunchPositionTargets()
	{
		this.preUpdate = base.transform.position;
		Vector3 eulerAngles = default(Vector3);
		if (this.tweenArguments.Contains("looktarget"))
		{
			eulerAngles = base.transform.eulerAngles;
			base.transform.eulerAngles = this.vector3s[4];
		}
		if (this.vector3s[1].x > 0f)
		{
			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
		}
		else if (this.vector3s[1].x < 0f)
		{
			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
		}
		if (this.vector3s[1].y > 0f)
		{
			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
		}
		else if (this.vector3s[1].y < 0f)
		{
			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
		}
		if (this.vector3s[1].z > 0f)
		{
			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
		}
		else if (this.vector3s[1].z < 0f)
		{
			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
		}
		base.transform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		if (this.tweenArguments.Contains("looktarget"))
		{
			base.transform.eulerAngles = eulerAngles;
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x06003F30 RID: 16176 RVA: 0x0012431C File Offset: 0x0012251C
	public void ApplyPunchRotationTargets()
	{
		this.preUpdate = base.transform.eulerAngles;
		if (this.vector3s[1].x > 0f)
		{
			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
		}
		else if (this.vector3s[1].x < 0f)
		{
			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
		}
		if (this.vector3s[1].y > 0f)
		{
			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
		}
		else if (this.vector3s[1].y < 0f)
		{
			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
		}
		if (this.vector3s[1].z > 0f)
		{
			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
		}
		else if (this.vector3s[1].z < 0f)
		{
			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
		}
		base.transform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		this.postUpdate = base.transform.eulerAngles;
		if (this.physics)
		{
			base.transform.eulerAngles = this.preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x06003F31 RID: 16177 RVA: 0x001245B0 File Offset: 0x001227B0
	public void ApplyPunchScaleTargets()
	{
		if (this.vector3s[1].x > 0f)
		{
			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
		}
		else if (this.vector3s[1].x < 0f)
		{
			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
		}
		if (this.vector3s[1].y > 0f)
		{
			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
		}
		else if (this.vector3s[1].y < 0f)
		{
			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
		}
		if (this.vector3s[1].z > 0f)
		{
			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
		}
		else if (this.vector3s[1].z < 0f)
		{
			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
		}
		base.transform.localScale = this.vector3s[0] + this.vector3s[2];
	}

	// Token: 0x06003F32 RID: 16178 RVA: 0x001247C8 File Offset: 0x001229C8
	public IEnumerator TweenDelay()
	{
		this.delayStarted = Time.time;
		yield return new WaitForSeconds(this.delay);
		if (this.wasPaused)
		{
			this.wasPaused = false;
			this.TweenStart();
		}
		yield break;
	}

	// Token: 0x06003F33 RID: 16179 RVA: 0x001247E4 File Offset: 0x001229E4
	public void TweenStart()
	{
		this.CallBack("onstart");
		if (!this.loop)
		{
			this.ConflictCheck();
			this.GenerateTargets();
		}
		if (this.type == "stab")
		{
			this.audioSource.PlayOneShot(this.audioSource.clip);
		}
		if (this.type == "move" || this.type == "scale" || this.type == "rotate" || this.type == "punch" || this.type == "shake" || this.type == "curve" || this.type == "look")
		{
			this.EnableKinematic();
		}
		this.isRunning = true;
	}

	// Token: 0x06003F34 RID: 16180 RVA: 0x001248E0 File Offset: 0x00122AE0
	public IEnumerator TweenRestart()
	{
		if (this.delay > 0f)
		{
			this.delayStarted = Time.time;
			yield return new WaitForSeconds(this.delay);
		}
		this.loop = true;
		this.TweenStart();
		yield break;
	}

	// Token: 0x06003F35 RID: 16181 RVA: 0x00032EAD File Offset: 0x000310AD
	public void TweenUpdate()
	{
		this.apply();
		this.CallBack("onupdate");
		this.UpdatePercentage();
	}

	// Token: 0x06003F36 RID: 16182 RVA: 0x001248FC File Offset: 0x00122AFC
	public void TweenComplete()
	{
		this.isRunning = false;
		if (this.percentage > 0.5f)
		{
			this.percentage = 1f;
		}
		else
		{
			this.percentage = 0f;
		}
		this.apply();
		if (this.type == "value")
		{
			this.CallBack("onupdate");
		}
		if (this.loopType == DialogueriTween.LoopType.none)
		{
			this.Dispose();
		}
		else
		{
			this.TweenLoop();
		}
		this.CallBack("oncomplete");
	}

	// Token: 0x06003F37 RID: 16183 RVA: 0x00124990 File Offset: 0x00122B90
	public void TweenLoop()
	{
		this.DisableKinematic();
		DialogueriTween.LoopType loopType = this.loopType;
		if (loopType != DialogueriTween.LoopType.loop)
		{
			if (loopType == DialogueriTween.LoopType.pingPong)
			{
				this.reverse = !this.reverse;
				this.runningTime = 0f;
				base.StartCoroutine("TweenRestart");
			}
		}
		else
		{
			this.percentage = 0f;
			this.runningTime = 0f;
			this.apply();
			base.StartCoroutine("TweenRestart");
		}
	}

	// Token: 0x06003F38 RID: 16184 RVA: 0x00124A1C File Offset: 0x00122C1C
	public static Rect RectUpdate(Rect currentValue, Rect targetValue, float speed)
	{
		Rect result;
		result..ctor(DialogueriTween.FloatUpdate(currentValue.x, targetValue.x, speed), DialogueriTween.FloatUpdate(currentValue.y, targetValue.y, speed), DialogueriTween.FloatUpdate(currentValue.width, targetValue.width, speed), DialogueriTween.FloatUpdate(currentValue.height, targetValue.height, speed));
		return result;
	}

	// Token: 0x06003F39 RID: 16185 RVA: 0x00124A84 File Offset: 0x00122C84
	public static Vector3 Vector3Update(Vector3 currentValue, Vector3 targetValue, float speed)
	{
		Vector3 vector = targetValue - currentValue;
		currentValue += vector * speed * Time.deltaTime;
		return currentValue;
	}

	// Token: 0x06003F3A RID: 16186 RVA: 0x00124AB4 File Offset: 0x00122CB4
	public static Vector2 Vector2Update(Vector2 currentValue, Vector2 targetValue, float speed)
	{
		Vector2 vector = targetValue - currentValue;
		currentValue += vector * speed * Time.deltaTime;
		return currentValue;
	}

	// Token: 0x06003F3B RID: 16187 RVA: 0x00124AE4 File Offset: 0x00122CE4
	public static float FloatUpdate(float currentValue, float targetValue, float speed)
	{
		float num = targetValue - currentValue;
		currentValue += num * speed * Time.deltaTime;
		return currentValue;
	}

	// Token: 0x06003F3C RID: 16188 RVA: 0x00032ECB File Offset: 0x000310CB
	public static void FadeUpdate(GameObject target, Hashtable args)
	{
		args["a"] = args["alpha"];
		DialogueriTween.ColorUpdate(target, args);
	}

	// Token: 0x06003F3D RID: 16189 RVA: 0x00032EEA File Offset: 0x000310EA
	public static void FadeUpdate(GameObject target, float alpha, float time)
	{
		DialogueriTween.FadeUpdate(target, DialogueriTween.Hash(new object[]
		{
			"alpha",
			alpha,
			"time",
			time
		}));
	}

	// Token: 0x06003F3E RID: 16190 RVA: 0x00124B04 File Offset: 0x00122D04
	public static void ColorUpdate(GameObject target, Hashtable args)
	{
		DialogueriTween.CleanArgs(args);
		Color[] array = new Color[4];
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogueriTween.ColorUpdate(transform.gameObject, args);
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
		}
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= DialogueriTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = DialogueriTween.Defaults.updateTime;
		}
		if (target.GetComponent(typeof(GUITexture)))
		{
			array[0] = (array[1] = target.GetComponent<GUITexture>().color);
		}
		else if (target.GetComponent(typeof(GUIText)))
		{
			array[0] = (array[1] = target.GetComponent<GUIText>().material.color);
		}
		else if (target.GetComponent<Renderer>())
		{
			array[0] = (array[1] = target.GetComponent<Renderer>().material.color);
		}
		else if (target.GetComponent<Light>())
		{
			array[0] = (array[1] = target.GetComponent<Light>().color);
		}
		if (args.Contains("color"))
		{
			array[1] = (Color)args["color"];
		}
		else
		{
			if (args.Contains("r"))
			{
				array[1].r = (float)args["r"];
			}
			if (args.Contains("g"))
			{
				array[1].g = (float)args["g"];
			}
			if (args.Contains("b"))
			{
				array[1].b = (float)args["b"];
			}
			if (args.Contains("a"))
			{
				array[1].a = (float)args["a"];
			}
		}
		array[3].r = Mathf.SmoothDamp(array[0].r, array[1].r, ref array[2].r, num);
		array[3].g = Mathf.SmoothDamp(array[0].g, array[1].g, ref array[2].g, num);
		array[3].b = Mathf.SmoothDamp(array[0].b, array[1].b, ref array[2].b, num);
		array[3].a = Mathf.SmoothDamp(array[0].a, array[1].a, ref array[2].a, num);
		if (target.GetComponent(typeof(GUITexture)))
		{
			target.GetComponent<GUITexture>().color = array[3];
		}
		else if (target.GetComponent(typeof(GUIText)))
		{
			target.GetComponent<GUIText>().material.color = array[3];
		}
		else if (target.GetComponent<Renderer>())
		{
			target.GetComponent<Renderer>().material.color = array[3];
		}
		else if (target.GetComponent<Light>())
		{
			target.GetComponent<Light>().color = array[3];
		}
	}

	// Token: 0x06003F3F RID: 16191 RVA: 0x00032F1F File Offset: 0x0003111F
	public static void ColorUpdate(GameObject target, Color color, float time)
	{
		DialogueriTween.ColorUpdate(target, DialogueriTween.Hash(new object[]
		{
			"color",
			color,
			"time",
			time
		}));
	}

	// Token: 0x06003F40 RID: 16192 RVA: 0x00124F68 File Offset: 0x00123168
	public static void AudioUpdate(GameObject target, Hashtable args)
	{
		DialogueriTween.CleanArgs(args);
		Vector2[] array = new Vector2[4];
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= DialogueriTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = DialogueriTween.Defaults.updateTime;
		}
		AudioSource audioSource;
		if (args.Contains("audiosource"))
		{
			audioSource = (AudioSource)args["audiosource"];
		}
		else
		{
			if (!target.GetComponent(typeof(AudioSource)))
			{
				Debug.LogError("iTween Error: AudioUpdate requires an AudioSource.", null);
				return;
			}
			audioSource = target.GetComponent<AudioSource>();
		}
		array[0] = (array[1] = new Vector2(audioSource.volume, audioSource.pitch));
		if (args.Contains("volume"))
		{
			array[1].x = (float)args["volume"];
		}
		if (args.Contains("pitch"))
		{
			array[1].y = (float)args["pitch"];
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		audioSource.volume = array[3].x;
		audioSource.pitch = array[3].y;
	}

	// Token: 0x06003F41 RID: 16193 RVA: 0x00125124 File Offset: 0x00123324
	public static void AudioUpdate(GameObject target, float volume, float pitch, float time)
	{
		DialogueriTween.AudioUpdate(target, DialogueriTween.Hash(new object[]
		{
			"volume",
			volume,
			"pitch",
			pitch,
			"time",
			time
		}));
	}

	// Token: 0x06003F42 RID: 16194 RVA: 0x00125178 File Offset: 0x00123378
	public static void RotateUpdate(GameObject target, Hashtable args)
	{
		DialogueriTween.CleanArgs(args);
		Vector3[] array = new Vector3[4];
		Vector3 eulerAngles = target.transform.eulerAngles;
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= DialogueriTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = DialogueriTween.Defaults.updateTime;
		}
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = DialogueriTween.Defaults.isLocal;
		}
		if (flag)
		{
			array[0] = target.transform.localEulerAngles;
		}
		else
		{
			array[0] = target.transform.eulerAngles;
		}
		if (args.Contains("rotation"))
		{
			if (args["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["rotation"];
				array[1] = transform.eulerAngles;
			}
			else if (args["rotation"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["rotation"];
			}
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
		if (flag)
		{
			target.transform.localEulerAngles = array[3];
		}
		else
		{
			target.transform.eulerAngles = array[3];
		}
		if (target.GetComponent<Rigidbody>() != null)
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			target.transform.eulerAngles = eulerAngles;
			target.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(eulerAngles2));
		}
	}

	// Token: 0x06003F43 RID: 16195 RVA: 0x00032F54 File Offset: 0x00031154
	public static void RotateUpdate(GameObject target, Vector3 rotation, float time)
	{
		DialogueriTween.RotateUpdate(target, DialogueriTween.Hash(new object[]
		{
			"rotation",
			rotation,
			"time",
			time
		}));
	}

	// Token: 0x06003F44 RID: 16196 RVA: 0x001253E4 File Offset: 0x001235E4
	public static void ScaleUpdate(GameObject target, Hashtable args)
	{
		DialogueriTween.CleanArgs(args);
		Vector3[] array = new Vector3[4];
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= DialogueriTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = DialogueriTween.Defaults.updateTime;
		}
		array[0] = (array[1] = target.transform.localScale);
		if (args.Contains("scale"))
		{
			if (args["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["scale"];
				array[1] = transform.localScale;
			}
			else if (args["scale"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["scale"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				array[1].x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				array[1].y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				array[1].z = (float)args["z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		target.transform.localScale = array[3];
	}

	// Token: 0x06003F45 RID: 16197 RVA: 0x00032F89 File Offset: 0x00031189
	public static void ScaleUpdate(GameObject target, Vector3 scale, float time)
	{
		DialogueriTween.ScaleUpdate(target, DialogueriTween.Hash(new object[]
		{
			"scale",
			scale,
			"time",
			time
		}));
	}

	// Token: 0x06003F46 RID: 16198 RVA: 0x00125630 File Offset: 0x00123830
	public static void MoveUpdate(GameObject target, Hashtable args)
	{
		DialogueriTween.CleanArgs(args);
		Vector3[] array = new Vector3[4];
		Vector3 position = target.transform.position;
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= DialogueriTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = DialogueriTween.Defaults.updateTime;
		}
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = DialogueriTween.Defaults.isLocal;
		}
		if (flag)
		{
			array[0] = (array[1] = target.transform.localPosition);
		}
		else
		{
			array[0] = (array[1] = target.transform.position);
		}
		if (args.Contains("position"))
		{
			if (args["position"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["position"];
				array[1] = transform.position;
			}
			else if (args["position"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["position"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				array[1].x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				array[1].y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				array[1].z = (float)args["z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		if (args.Contains("orienttopath") && (bool)args["orienttopath"])
		{
			args["looktarget"] = array[3];
		}
		if (args.Contains("looktarget"))
		{
			DialogueriTween.LookUpdate(target, args);
		}
		if (flag)
		{
			target.transform.localPosition = array[3];
		}
		else
		{
			target.transform.position = array[3];
		}
		if (target.GetComponent<Rigidbody>() != null)
		{
			Vector3 position2 = target.transform.position;
			target.transform.position = position;
			target.GetComponent<Rigidbody>().MovePosition(position2);
		}
	}

	// Token: 0x06003F47 RID: 16199 RVA: 0x00032FBE File Offset: 0x000311BE
	public static void MoveUpdate(GameObject target, Vector3 position, float time)
	{
		DialogueriTween.MoveUpdate(target, DialogueriTween.Hash(new object[]
		{
			"position",
			position,
			"time",
			time
		}));
	}

	// Token: 0x06003F48 RID: 16200 RVA: 0x0012599C File Offset: 0x00123B9C
	public static void LookUpdate(GameObject target, Hashtable args)
	{
		DialogueriTween.CleanArgs(args);
		Vector3[] array = new Vector3[5];
		float num;
		if (args.Contains("looktime"))
		{
			num = (float)args["looktime"];
			num *= DialogueriTween.Defaults.updateTimePercentage;
		}
		else if (args.Contains("time"))
		{
			num = (float)args["time"] * 0.15f;
			num *= DialogueriTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = DialogueriTween.Defaults.updateTime;
		}
		array[0] = target.transform.eulerAngles;
		if (args.Contains("looktarget"))
		{
			if (args["looktarget"].GetType() == typeof(Transform))
			{
				Transform transform = target.transform;
				Transform transform2 = (Transform)args["looktarget"];
				Vector3? vector = (Vector3?)args["up"];
				transform.LookAt(transform2, (vector == null) ? DialogueriTween.Defaults.up : vector.Value);
			}
			else if (args["looktarget"].GetType() == typeof(Vector3))
			{
				Transform transform3 = target.transform;
				Vector3 vector2 = (Vector3)args["looktarget"];
				Vector3? vector3 = (Vector3?)args["up"];
				transform3.LookAt(vector2, (vector3 == null) ? DialogueriTween.Defaults.up : vector3.Value);
			}
			array[1] = target.transform.eulerAngles;
			target.transform.eulerAngles = array[0];
			array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
			array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
			array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
			target.transform.eulerAngles = array[3];
			if (args.Contains("axis"))
			{
				array[4] = target.transform.eulerAngles;
				string text = (string)args["axis"];
				if (text != null)
				{
					if (!(text == "x"))
					{
						if (!(text == "y"))
						{
							if (text == "z")
							{
								array[4].x = array[0].x;
								array[4].y = array[0].y;
							}
						}
						else
						{
							array[4].x = array[0].x;
							array[4].z = array[0].z;
						}
					}
					else
					{
						array[4].y = array[0].y;
						array[4].z = array[0].z;
					}
				}
				target.transform.eulerAngles = array[4];
			}
			return;
		}
		Debug.LogError("iTween Error: LookUpdate needs a 'looktarget' property!", null);
	}

	// Token: 0x06003F49 RID: 16201 RVA: 0x00032FF3 File Offset: 0x000311F3
	public static void LookUpdate(GameObject target, Vector3 looktarget, float time)
	{
		DialogueriTween.LookUpdate(target, DialogueriTween.Hash(new object[]
		{
			"looktarget",
			looktarget,
			"time",
			time
		}));
	}

	// Token: 0x06003F4A RID: 16202 RVA: 0x00125D44 File Offset: 0x00123F44
	public static float PathLength(Transform[] path)
	{
		Vector3[] array = new Vector3[path.Length];
		float num = 0f;
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		Vector3[] pts = DialogueriTween.PathControlPointGenerator(array);
		Vector3 vector = DialogueriTween.Interp(pts, 0f);
		int num2 = path.Length * 20;
		for (int j = 1; j <= num2; j++)
		{
			float t = (float)j / (float)num2;
			Vector3 vector2 = DialogueriTween.Interp(pts, t);
			num += Vector3.Distance(vector, vector2);
			vector = vector2;
		}
		return num;
	}

	// Token: 0x06003F4B RID: 16203 RVA: 0x00125DE0 File Offset: 0x00123FE0
	public static float PathLength(Vector3[] path)
	{
		float num = 0f;
		Vector3[] pts = DialogueriTween.PathControlPointGenerator(path);
		Vector3 vector = DialogueriTween.Interp(pts, 0f);
		int num2 = path.Length * 20;
		for (int i = 1; i <= num2; i++)
		{
			float t = (float)i / (float)num2;
			Vector3 vector2 = DialogueriTween.Interp(pts, t);
			num += Vector3.Distance(vector, vector2);
			vector = vector2;
		}
		return num;
	}

	// Token: 0x06003F4C RID: 16204 RVA: 0x00125E44 File Offset: 0x00124044
	public static Texture2D CameraTexture(Color color)
	{
		Texture2D texture2D = new Texture2D(Screen.width, Screen.height, 5, false);
		Color[] array = new Color[Screen.width * Screen.height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = color;
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x06003F4D RID: 16205 RVA: 0x00033028 File Offset: 0x00031228
	public static void PutOnPath(GameObject target, Vector3[] path, float percent)
	{
		target.transform.position = DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(path), percent);
	}

	// Token: 0x06003F4E RID: 16206 RVA: 0x00033041 File Offset: 0x00031241
	public static void PutOnPath(Transform target, Vector3[] path, float percent)
	{
		target.position = DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(path), percent);
	}

	// Token: 0x06003F4F RID: 16207 RVA: 0x00125EA4 File Offset: 0x001240A4
	public static void PutOnPath(GameObject target, Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		target.transform.position = DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(array), percent);
	}

	// Token: 0x06003F50 RID: 16208 RVA: 0x00125EFC File Offset: 0x001240FC
	public static void PutOnPath(Transform target, Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		target.position = DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(array), percent);
	}

	// Token: 0x06003F51 RID: 16209 RVA: 0x00125F4C File Offset: 0x0012414C
	public static Vector3 PointOnPath(Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		return DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(array), percent);
	}

	// Token: 0x06003F52 RID: 16210 RVA: 0x00033055 File Offset: 0x00031255
	public static void DrawLine(Vector3[] line)
	{
		if (line.Length > 0)
		{
			DialogueriTween.DrawLineHelper(line, DialogueriTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06003F53 RID: 16211 RVA: 0x00033070 File Offset: 0x00031270
	public static void DrawLine(Vector3[] line, Color color)
	{
		if (line.Length > 0)
		{
			DialogueriTween.DrawLineHelper(line, color, "gizmos");
		}
	}

	// Token: 0x06003F54 RID: 16212 RVA: 0x00125F98 File Offset: 0x00124198
	public static void DrawLine(Transform[] line)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DialogueriTween.DrawLineHelper(array, DialogueriTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06003F55 RID: 16213 RVA: 0x00125FF0 File Offset: 0x001241F0
	public static void DrawLine(Transform[] line, Color color)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DialogueriTween.DrawLineHelper(array, color, "gizmos");
		}
	}

	// Token: 0x06003F56 RID: 16214 RVA: 0x00033087 File Offset: 0x00031287
	public static void DrawLineGizmos(Vector3[] line)
	{
		if (line.Length > 0)
		{
			DialogueriTween.DrawLineHelper(line, DialogueriTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06003F57 RID: 16215 RVA: 0x000330A2 File Offset: 0x000312A2
	public static void DrawLineGizmos(Vector3[] line, Color color)
	{
		if (line.Length > 0)
		{
			DialogueriTween.DrawLineHelper(line, color, "gizmos");
		}
	}

	// Token: 0x06003F58 RID: 16216 RVA: 0x00126044 File Offset: 0x00124244
	public static void DrawLineGizmos(Transform[] line)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DialogueriTween.DrawLineHelper(array, DialogueriTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06003F59 RID: 16217 RVA: 0x0012609C File Offset: 0x0012429C
	public static void DrawLineGizmos(Transform[] line, Color color)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DialogueriTween.DrawLineHelper(array, color, "gizmos");
		}
	}

	// Token: 0x06003F5A RID: 16218 RVA: 0x000330B9 File Offset: 0x000312B9
	public static void DrawLineHandles(Vector3[] line)
	{
		if (line.Length > 0)
		{
			DialogueriTween.DrawLineHelper(line, DialogueriTween.Defaults.color, "handles");
		}
	}

	// Token: 0x06003F5B RID: 16219 RVA: 0x000330D4 File Offset: 0x000312D4
	public static void DrawLineHandles(Vector3[] line, Color color)
	{
		if (line.Length > 0)
		{
			DialogueriTween.DrawLineHelper(line, color, "handles");
		}
	}

	// Token: 0x06003F5C RID: 16220 RVA: 0x001260F0 File Offset: 0x001242F0
	public static void DrawLineHandles(Transform[] line)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DialogueriTween.DrawLineHelper(array, DialogueriTween.Defaults.color, "handles");
		}
	}

	// Token: 0x06003F5D RID: 16221 RVA: 0x00126148 File Offset: 0x00124348
	public static void DrawLineHandles(Transform[] line, Color color)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DialogueriTween.DrawLineHelper(array, color, "handles");
		}
	}

	// Token: 0x06003F5E RID: 16222 RVA: 0x000330EB File Offset: 0x000312EB
	public static Vector3 PointOnPath(Vector3[] path, float percent)
	{
		return DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(path), percent);
	}

	// Token: 0x06003F5F RID: 16223 RVA: 0x000330F9 File Offset: 0x000312F9
	public static void DrawPath(Vector3[] path)
	{
		if (path.Length > 0)
		{
			DialogueriTween.DrawPathHelper(path, DialogueriTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06003F60 RID: 16224 RVA: 0x00033114 File Offset: 0x00031314
	public static void DrawPath(Vector3[] path, Color color)
	{
		if (path.Length > 0)
		{
			DialogueriTween.DrawPathHelper(path, color, "gizmos");
		}
	}

	// Token: 0x06003F61 RID: 16225 RVA: 0x0012619C File Offset: 0x0012439C
	public static void DrawPath(Transform[] path)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DialogueriTween.DrawPathHelper(array, DialogueriTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06003F62 RID: 16226 RVA: 0x001261F4 File Offset: 0x001243F4
	public static void DrawPath(Transform[] path, Color color)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DialogueriTween.DrawPathHelper(array, color, "gizmos");
		}
	}

	// Token: 0x06003F63 RID: 16227 RVA: 0x0003312B File Offset: 0x0003132B
	public static void DrawPathGizmos(Vector3[] path)
	{
		if (path.Length > 0)
		{
			DialogueriTween.DrawPathHelper(path, DialogueriTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06003F64 RID: 16228 RVA: 0x00033146 File Offset: 0x00031346
	public static void DrawPathGizmos(Vector3[] path, Color color)
	{
		if (path.Length > 0)
		{
			DialogueriTween.DrawPathHelper(path, color, "gizmos");
		}
	}

	// Token: 0x06003F65 RID: 16229 RVA: 0x00126248 File Offset: 0x00124448
	public static void DrawPathGizmos(Transform[] path)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DialogueriTween.DrawPathHelper(array, DialogueriTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06003F66 RID: 16230 RVA: 0x001262A0 File Offset: 0x001244A0
	public static void DrawPathGizmos(Transform[] path, Color color)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DialogueriTween.DrawPathHelper(array, color, "gizmos");
		}
	}

	// Token: 0x06003F67 RID: 16231 RVA: 0x0003315D File Offset: 0x0003135D
	public static void DrawPathHandles(Vector3[] path)
	{
		if (path.Length > 0)
		{
			DialogueriTween.DrawPathHelper(path, DialogueriTween.Defaults.color, "handles");
		}
	}

	// Token: 0x06003F68 RID: 16232 RVA: 0x00033178 File Offset: 0x00031378
	public static void DrawPathHandles(Vector3[] path, Color color)
	{
		if (path.Length > 0)
		{
			DialogueriTween.DrawPathHelper(path, color, "handles");
		}
	}

	// Token: 0x06003F69 RID: 16233 RVA: 0x001262F4 File Offset: 0x001244F4
	public static void DrawPathHandles(Transform[] path)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DialogueriTween.DrawPathHelper(array, DialogueriTween.Defaults.color, "handles");
		}
	}

	// Token: 0x06003F6A RID: 16234 RVA: 0x0012634C File Offset: 0x0012454C
	public static void DrawPathHandles(Transform[] path, Color color)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DialogueriTween.DrawPathHelper(array, color, "handles");
		}
	}

	// Token: 0x06003F6B RID: 16235 RVA: 0x001263A0 File Offset: 0x001245A0
	public static void CameraFadeDepth(int depth)
	{
		if (DialogueriTween.cameraFade)
		{
			DialogueriTween.cameraFade.transform.position = new Vector3(DialogueriTween.cameraFade.transform.position.x, DialogueriTween.cameraFade.transform.position.y, (float)depth);
		}
	}

	// Token: 0x06003F6C RID: 16236 RVA: 0x0003318F File Offset: 0x0003138F
	public static void CameraFadeDestroy()
	{
		if (DialogueriTween.cameraFade)
		{
			Object.Destroy(DialogueriTween.cameraFade);
		}
	}

	// Token: 0x06003F6D RID: 16237 RVA: 0x000331AA File Offset: 0x000313AA
	public static void CameraFadeSwap(Texture2D texture)
	{
		if (DialogueriTween.cameraFade)
		{
			DialogueriTween.cameraFade.GetComponent<GUITexture>().texture = texture;
		}
	}

	// Token: 0x06003F6E RID: 16238 RVA: 0x00126400 File Offset: 0x00124600
	public static GameObject CameraFadeAdd(Texture2D texture, int depth)
	{
		if (DialogueriTween.cameraFade)
		{
			return null;
		}
		DialogueriTween.cameraFade = new GameObject("iTween Camera Fade");
		DialogueriTween.cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)depth);
		DialogueriTween.cameraFade.AddComponent<GUITexture>();
		DialogueriTween.cameraFade.GetComponent<GUITexture>().texture = texture;
		DialogueriTween.cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return DialogueriTween.cameraFade;
	}

	// Token: 0x06003F6F RID: 16239 RVA: 0x00126498 File Offset: 0x00124698
	public static GameObject CameraFadeAdd(Texture2D texture)
	{
		if (DialogueriTween.cameraFade)
		{
			return null;
		}
		DialogueriTween.cameraFade = new GameObject("iTween Camera Fade");
		DialogueriTween.cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)DialogueriTween.Defaults.cameraFadeDepth);
		DialogueriTween.cameraFade.AddComponent<GUITexture>();
		DialogueriTween.cameraFade.GetComponent<GUITexture>().texture = texture;
		DialogueriTween.cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return DialogueriTween.cameraFade;
	}

	// Token: 0x06003F70 RID: 16240 RVA: 0x00126534 File Offset: 0x00124734
	public static GameObject CameraFadeAdd()
	{
		if (DialogueriTween.cameraFade)
		{
			return null;
		}
		DialogueriTween.cameraFade = new GameObject("iTween Camera Fade");
		DialogueriTween.cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)DialogueriTween.Defaults.cameraFadeDepth);
		DialogueriTween.cameraFade.AddComponent<GUITexture>();
		DialogueriTween.cameraFade.GetComponent<GUITexture>().texture = DialogueriTween.CameraTexture(Color.black);
		DialogueriTween.cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return DialogueriTween.cameraFade;
	}

	// Token: 0x06003F71 RID: 16241 RVA: 0x001265D8 File Offset: 0x001247D8
	public static void Resume(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			dialogueriTween.enabled = true;
		}
	}

	// Token: 0x06003F72 RID: 16242 RVA: 0x0012661C File Offset: 0x0012481C
	public static void Resume(GameObject target, bool includechildren)
	{
		DialogueriTween.Resume(target);
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogueriTween.Resume(transform.gameObject, true);
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
		}
	}

	// Token: 0x06003F73 RID: 16243 RVA: 0x00126694 File Offset: 0x00124894
	public static void Resume(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			string text = dialogueriTween.type + dialogueriTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				dialogueriTween.enabled = true;
			}
		}
	}

	// Token: 0x06003F74 RID: 16244 RVA: 0x00126714 File Offset: 0x00124914
	public static void Resume(GameObject target, string type, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			string text = dialogueriTween.type + dialogueriTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				dialogueriTween.enabled = true;
			}
		}
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogueriTween.Resume(transform.gameObject, type, true);
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
		}
	}

	// Token: 0x06003F75 RID: 16245 RVA: 0x00126800 File Offset: 0x00124A00
	public static void Resume()
	{
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			GameObject target = (GameObject)hashtable["target"];
			DialogueriTween.Resume(target);
		}
	}

	// Token: 0x06003F76 RID: 16246 RVA: 0x00126850 File Offset: 0x00124A50
	public static void Resume(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			GameObject value = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			DialogueriTween.Resume((GameObject)arrayList[j], type);
		}
	}

	// Token: 0x06003F77 RID: 16247 RVA: 0x001268DC File Offset: 0x00124ADC
	public static void Pause(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			if (dialogueriTween.delay > 0f)
			{
				dialogueriTween.delay -= Time.time - dialogueriTween.delayStarted;
				dialogueriTween.StopCoroutine("TweenDelay");
			}
			dialogueriTween.isPaused = true;
			dialogueriTween.enabled = false;
		}
	}

	// Token: 0x06003F78 RID: 16248 RVA: 0x0012695C File Offset: 0x00124B5C
	public static void Pause(GameObject target, bool includechildren)
	{
		DialogueriTween.Pause(target);
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogueriTween.Pause(transform.gameObject, true);
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
		}
	}

	// Token: 0x06003F79 RID: 16249 RVA: 0x001269D4 File Offset: 0x00124BD4
	public static void Pause(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			string text = dialogueriTween.type + dialogueriTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				if (dialogueriTween.delay > 0f)
				{
					dialogueriTween.delay -= Time.time - dialogueriTween.delayStarted;
					dialogueriTween.StopCoroutine("TweenDelay");
				}
				dialogueriTween.isPaused = true;
				dialogueriTween.enabled = false;
			}
		}
	}

	// Token: 0x06003F7A RID: 16250 RVA: 0x00126A90 File Offset: 0x00124C90
	public static void Pause(GameObject target, string type, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			string text = dialogueriTween.type + dialogueriTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				if (dialogueriTween.delay > 0f)
				{
					dialogueriTween.delay -= Time.time - dialogueriTween.delayStarted;
					dialogueriTween.StopCoroutine("TweenDelay");
				}
				dialogueriTween.isPaused = true;
				dialogueriTween.enabled = false;
			}
		}
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogueriTween.Pause(transform.gameObject, type, true);
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
		}
	}

	// Token: 0x06003F7B RID: 16251 RVA: 0x00126BB8 File Offset: 0x00124DB8
	public static void Pause()
	{
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			GameObject target = (GameObject)hashtable["target"];
			DialogueriTween.Pause(target);
		}
	}

	// Token: 0x06003F7C RID: 16252 RVA: 0x00126C08 File Offset: 0x00124E08
	public static void Pause(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			GameObject value = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			DialogueriTween.Pause((GameObject)arrayList[j], type);
		}
	}

	// Token: 0x06003F7D RID: 16253 RVA: 0x000331CB File Offset: 0x000313CB
	public static int Count()
	{
		return DialogueriTween.tweens.Count;
	}

	// Token: 0x06003F7E RID: 16254 RVA: 0x00126C94 File Offset: 0x00124E94
	public static int Count(string type)
	{
		int num = 0;
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			string text = (string)hashtable["type"] + (string)hashtable["method"];
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06003F7F RID: 16255 RVA: 0x00126D20 File Offset: 0x00124F20
	public static int Count(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		return components.Length;
	}

	// Token: 0x06003F80 RID: 16256 RVA: 0x00126D44 File Offset: 0x00124F44
	public static int Count(GameObject target, string type)
	{
		int num = 0;
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			string text = dialogueriTween.type + dialogueriTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06003F81 RID: 16257 RVA: 0x00126DC8 File Offset: 0x00124FC8
	public static void Stop()
	{
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			GameObject target = (GameObject)hashtable["target"];
			DialogueriTween.Stop(target);
		}
		DialogueriTween.tweens.Clear();
	}

	// Token: 0x06003F82 RID: 16258 RVA: 0x00126E24 File Offset: 0x00125024
	public static void Stop(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			GameObject value = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			DialogueriTween.Stop((GameObject)arrayList[j], type);
		}
	}

	// Token: 0x06003F83 RID: 16259 RVA: 0x00126EB0 File Offset: 0x001250B0
	public static void StopByName(string name)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			GameObject value = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			DialogueriTween.StopByName((GameObject)arrayList[j], name);
		}
	}

	// Token: 0x06003F84 RID: 16260 RVA: 0x00126F3C File Offset: 0x0012513C
	public static void Stop(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			dialogueriTween.Dispose();
		}
	}

	// Token: 0x06003F85 RID: 16261 RVA: 0x00126F80 File Offset: 0x00125180
	public static void Stop(GameObject target, bool includechildren)
	{
		DialogueriTween.Stop(target);
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogueriTween.Stop(transform.gameObject, true);
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
		}
	}

	// Token: 0x06003F86 RID: 16262 RVA: 0x00126FF8 File Offset: 0x001251F8
	public static void Stop(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			string text = dialogueriTween.type + dialogueriTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				dialogueriTween.Dispose();
			}
		}
	}

	// Token: 0x06003F87 RID: 16263 RVA: 0x00127078 File Offset: 0x00125278
	public static void StopByName(GameObject target, string name)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			if (dialogueriTween._name == name)
			{
				dialogueriTween.Dispose();
			}
		}
	}

	// Token: 0x06003F88 RID: 16264 RVA: 0x001270CC File Offset: 0x001252CC
	public static void Stop(GameObject target, string type, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			string text = dialogueriTween.type + dialogueriTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				dialogueriTween.Dispose();
			}
		}
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogueriTween.Stop(transform.gameObject, type, true);
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
		}
	}

	// Token: 0x06003F89 RID: 16265 RVA: 0x001271B8 File Offset: 0x001253B8
	public static void StopByName(GameObject target, string name, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			if (dialogueriTween._name == name)
			{
				dialogueriTween.Dispose();
			}
		}
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogueriTween.StopByName(transform.gameObject, name, true);
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
		}
	}

	// Token: 0x06003F8A RID: 16266 RVA: 0x00127278 File Offset: 0x00125478
	public static Hashtable Hash(params object[] args)
	{
		Hashtable hashtable = new Hashtable(args.Length / 2);
		if (args.Length % 2 != 0)
		{
			Debug.LogError("Tween Error: Hash requires an even number of arguments!", null);
			return null;
		}
		for (int i = 0; i < args.Length - 1; i += 2)
		{
			hashtable.Add(args[i], args[i + 1]);
		}
		return hashtable;
	}

	// Token: 0x06003F8B RID: 16267 RVA: 0x000331D7 File Offset: 0x000313D7
	public void Awake()
	{
		this.RetrieveArgs();
		this.lastRealTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06003F8C RID: 16268 RVA: 0x001272CC File Offset: 0x001254CC
	public IEnumerator Start()
	{
		if (this.delay > 0f)
		{
			yield return base.StartCoroutine("TweenDelay");
		}
		this.TweenStart();
		yield break;
	}

	// Token: 0x06003F8D RID: 16269 RVA: 0x001272E8 File Offset: 0x001254E8
	public void Update()
	{
		if (this.isRunning && !this.physics)
		{
			if (!this.reverse)
			{
				if (this.percentage < 1f)
				{
					this.TweenUpdate();
				}
				else
				{
					this.TweenComplete();
				}
			}
			else if (this.percentage > 0f)
			{
				this.TweenUpdate();
			}
			else
			{
				this.TweenComplete();
			}
		}
	}

	// Token: 0x06003F8E RID: 16270 RVA: 0x00127360 File Offset: 0x00125560
	public void FixedUpdate()
	{
		if (this.isRunning && this.physics)
		{
			if (!this.reverse)
			{
				if (this.percentage < 1f)
				{
					this.TweenUpdate();
				}
				else
				{
					this.TweenComplete();
				}
			}
			else if (this.percentage > 0f)
			{
				this.TweenUpdate();
			}
			else
			{
				this.TweenComplete();
			}
		}
	}

	// Token: 0x06003F8F RID: 16271 RVA: 0x001273D8 File Offset: 0x001255D8
	public void LateUpdate()
	{
		if (this.tweenArguments.Contains("looktarget") && this.isRunning && (this.type == "move" || this.type == "shake" || this.type == "punch"))
		{
			DialogueriTween.LookUpdate(base.gameObject, this.tweenArguments);
		}
	}

	// Token: 0x06003F90 RID: 16272 RVA: 0x00127458 File Offset: 0x00125658
	public void OnEnable()
	{
		if (this.isRunning)
		{
			this.EnableKinematic();
		}
		if (this.isPaused)
		{
			this.isPaused = false;
			if (this.delay > 0f)
			{
				this.wasPaused = true;
				this.ResumeDelay();
			}
		}
	}

	// Token: 0x06003F91 RID: 16273 RVA: 0x000331EA File Offset: 0x000313EA
	public void OnDisable()
	{
		this.DisableKinematic();
	}

	// Token: 0x06003F92 RID: 16274 RVA: 0x001274A8 File Offset: 0x001256A8
	public static void DrawLineHelper(Vector3[] line, Color color, string method)
	{
		Gizmos.color = color;
		for (int i = 0; i < line.Length - 1; i++)
		{
			if (method == "gizmos")
			{
				Gizmos.DrawLine(line[i], line[i + 1]);
			}
			else if (method == "handles")
			{
				Debug.LogError("iTween Error: Drawing a line with Handles is temporarily disabled because of compatability issues with Unity 2.6!", null);
			}
		}
	}

	// Token: 0x06003F93 RID: 16275 RVA: 0x00127520 File Offset: 0x00125720
	public static void DrawPathHelper(Vector3[] path, Color color, string method)
	{
		Vector3[] pts = DialogueriTween.PathControlPointGenerator(path);
		Vector3 vector = DialogueriTween.Interp(pts, 0f);
		Gizmos.color = color;
		int num = path.Length * 20;
		for (int i = 1; i <= num; i++)
		{
			float t = (float)i / (float)num;
			Vector3 vector2 = DialogueriTween.Interp(pts, t);
			if (method == "gizmos")
			{
				Gizmos.DrawLine(vector2, vector);
			}
			else if (method == "handles")
			{
				Debug.LogError("iTween Error: Drawing a path with Handles is temporarily disabled because of compatability issues with Unity 2.6!", null);
			}
			vector = vector2;
		}
	}

	// Token: 0x06003F94 RID: 16276 RVA: 0x001275AC File Offset: 0x001257AC
	public static Vector3[] PathControlPointGenerator(Vector3[] path)
	{
		int num = 2;
		Vector3[] array = new Vector3[path.Length + num];
		Array.Copy(path, 0, array, 1, path.Length);
		array[0] = array[1] + (array[1] - array[2]);
		array[array.Length - 1] = array[array.Length - 2] + (array[array.Length - 2] - array[array.Length - 3]);
		if (array[1] == array[array.Length - 2])
		{
			Vector3[] array2 = new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
			array2[0] = array2[array2.Length - 3];
			array2[array2.Length - 1] = array2[2];
			array = new Vector3[array2.Length];
			Array.Copy(array2, array, array2.Length);
		}
		return array;
	}

	// Token: 0x06003F95 RID: 16277 RVA: 0x001276E0 File Offset: 0x001258E0
	public static Vector3 Interp(Vector3[] pts, float t)
	{
		int num = pts.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		Vector3 vector = pts[num2];
		Vector3 vector2 = pts[num2 + 1];
		Vector3 vector3 = pts[num2 + 2];
		Vector3 vector4 = pts[num2 + 3];
		return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num3 * num3 * num3) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num3 * num3) + (-vector + vector3) * num3 + 2f * vector2);
	}

	// Token: 0x06003F96 RID: 16278 RVA: 0x001277F8 File Offset: 0x001259F8
	public static void Launch(GameObject target, Hashtable args)
	{
		if (!args.Contains("id"))
		{
			args["id"] = DialogueriTween.GenerateID();
		}
		if (!args.Contains("target"))
		{
			args["target"] = target;
		}
		DialogueriTween.tweens.Insert(0, args);
		target.AddComponent<DialogueriTween>();
	}

	// Token: 0x06003F97 RID: 16279 RVA: 0x00127854 File Offset: 0x00125A54
	public static Hashtable CleanArgs(Hashtable args)
	{
		Hashtable hashtable = new Hashtable(args.Count);
		Hashtable hashtable2 = new Hashtable(args.Count);
		IDictionaryEnumerator enumerator = args.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				hashtable.Add(dictionaryEntry.Key, dictionaryEntry.Value);
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
		IDictionaryEnumerator enumerator2 = hashtable.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
				if (dictionaryEntry2.Value.GetType() == typeof(int))
				{
					int num = (int)dictionaryEntry2.Value;
					float num2 = (float)num;
					args[dictionaryEntry2.Key] = num2;
				}
				if (dictionaryEntry2.Value.GetType() == typeof(double))
				{
					double num3 = (double)dictionaryEntry2.Value;
					float num4 = (float)num3;
					args[dictionaryEntry2.Key] = num4;
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		IDictionaryEnumerator enumerator3 = args.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				object obj3 = enumerator3.Current;
				DictionaryEntry dictionaryEntry3 = (DictionaryEntry)obj3;
				hashtable2.Add(dictionaryEntry3.Key.ToString().ToLower(), dictionaryEntry3.Value);
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = (enumerator3 as IDisposable)) != null)
			{
				disposable3.Dispose();
			}
		}
		args = hashtable2;
		return args;
	}

	// Token: 0x06003F98 RID: 16280 RVA: 0x00127A20 File Offset: 0x00125C20
	public static string GenerateID()
	{
		int num = 15;
		char[] array = new char[]
		{
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'g',
			'h',
			'i',
			'j',
			'k',
			'l',
			'm',
			'n',
			'o',
			'p',
			'q',
			'r',
			's',
			't',
			'u',
			'v',
			'w',
			'x',
			'y',
			'z',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'I',
			'J',
			'K',
			'L',
			'M',
			'N',
			'O',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'W',
			'X',
			'Y',
			'Z',
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8'
		};
		int num2 = array.Length - 1;
		string text = string.Empty;
		for (int i = 0; i < num; i++)
		{
			text += array[(int)Mathf.Floor((float)Random.Range(0, num2))];
		}
		return text;
	}

	// Token: 0x06003F99 RID: 16281 RVA: 0x00127A84 File Offset: 0x00125C84
	public void RetrieveArgs()
	{
		IEnumerator enumerator = DialogueriTween.tweens.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Hashtable hashtable = (Hashtable)obj;
				if ((GameObject)hashtable["target"] == base.gameObject)
				{
					this.tweenArguments = hashtable;
					break;
				}
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
		this.id = (string)this.tweenArguments["id"];
		this.type = (string)this.tweenArguments["type"];
		this._name = (string)this.tweenArguments["name"];
		this.method = (string)this.tweenArguments["method"];
		if (this.tweenArguments.Contains("time"))
		{
			this.time = (float)this.tweenArguments["time"];
		}
		else
		{
			this.time = DialogueriTween.Defaults.time;
		}
		if (base.GetComponent<Rigidbody>() != null)
		{
			this.physics = true;
		}
		if (this.tweenArguments.Contains("delay"))
		{
			this.delay = (float)this.tweenArguments["delay"];
		}
		else
		{
			this.delay = DialogueriTween.Defaults.delay;
		}
		if (this.tweenArguments.Contains("namedcolorvalue"))
		{
			if (this.tweenArguments["namedcolorvalue"].GetType() == typeof(DialogueriTween.NamedValueColor))
			{
				this.namedcolorvalue = (DialogueriTween.NamedValueColor)this.tweenArguments["namedcolorvalue"];
			}
			else
			{
				try
				{
					this.namedcolorvalue = (DialogueriTween.NamedValueColor)Enum.Parse(typeof(DialogueriTween.NamedValueColor), (string)this.tweenArguments["namedcolorvalue"], true);
				}
				catch
				{
					this.namedcolorvalue = DialogueriTween.NamedValueColor._Color;
				}
			}
		}
		else
		{
			this.namedcolorvalue = DialogueriTween.Defaults.namedColorValue;
		}
		if (this.tweenArguments.Contains("looptype"))
		{
			if (this.tweenArguments["looptype"].GetType() == typeof(DialogueriTween.LoopType))
			{
				this.loopType = (DialogueriTween.LoopType)this.tweenArguments["looptype"];
			}
			else
			{
				try
				{
					this.loopType = (DialogueriTween.LoopType)Enum.Parse(typeof(DialogueriTween.LoopType), (string)this.tweenArguments["looptype"], true);
				}
				catch
				{
					this.loopType = DialogueriTween.LoopType.none;
				}
			}
		}
		else
		{
			this.loopType = DialogueriTween.LoopType.none;
		}
		if (this.tweenArguments.Contains("easetype"))
		{
			if (this.tweenArguments["easetype"].GetType() == typeof(DialogueriTween.EaseType))
			{
				this.easeType = (DialogueriTween.EaseType)this.tweenArguments["easetype"];
			}
			else
			{
				try
				{
					this.easeType = (DialogueriTween.EaseType)Enum.Parse(typeof(DialogueriTween.EaseType), (string)this.tweenArguments["easetype"], true);
				}
				catch
				{
					this.easeType = DialogueriTween.Defaults.easeType;
				}
			}
		}
		else
		{
			this.easeType = DialogueriTween.Defaults.easeType;
		}
		if (this.tweenArguments.Contains("space"))
		{
			if (this.tweenArguments["space"].GetType() == typeof(Space))
			{
				this.space = (Space)this.tweenArguments["space"];
			}
			else
			{
				try
				{
					this.space = (Space)Enum.Parse(typeof(Space), (string)this.tweenArguments["space"], true);
				}
				catch
				{
					this.space = DialogueriTween.Defaults.space;
				}
			}
		}
		else
		{
			this.space = DialogueriTween.Defaults.space;
		}
		if (this.tweenArguments.Contains("islocal"))
		{
			this.isLocal = (bool)this.tweenArguments["islocal"];
		}
		else
		{
			this.isLocal = DialogueriTween.Defaults.isLocal;
		}
		if (this.tweenArguments.Contains("ignoretimescale"))
		{
			this.useRealTime = (bool)this.tweenArguments["ignoretimescale"];
		}
		else
		{
			this.useRealTime = DialogueriTween.Defaults.useRealTime;
		}
		this.GetEasingFunction();
	}

	// Token: 0x06003F9A RID: 16282 RVA: 0x00127F78 File Offset: 0x00126178
	public void GetEasingFunction()
	{
		switch (this.easeType)
		{
		case DialogueriTween.EaseType.easeInQuad:
			this.ease = new DialogueriTween.EasingFunction(this.easeInQuad);
			break;
		case DialogueriTween.EaseType.easeOutQuad:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutQuad);
			break;
		case DialogueriTween.EaseType.easeInOutQuad:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutQuad);
			break;
		case DialogueriTween.EaseType.easeInCubic:
			this.ease = new DialogueriTween.EasingFunction(this.easeInCubic);
			break;
		case DialogueriTween.EaseType.easeOutCubic:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutCubic);
			break;
		case DialogueriTween.EaseType.easeInOutCubic:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutCubic);
			break;
		case DialogueriTween.EaseType.easeInQuart:
			this.ease = new DialogueriTween.EasingFunction(this.easeInQuart);
			break;
		case DialogueriTween.EaseType.easeOutQuart:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutQuart);
			break;
		case DialogueriTween.EaseType.easeInOutQuart:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutQuart);
			break;
		case DialogueriTween.EaseType.easeInQuint:
			this.ease = new DialogueriTween.EasingFunction(this.easeInQuint);
			break;
		case DialogueriTween.EaseType.easeOutQuint:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutQuint);
			break;
		case DialogueriTween.EaseType.easeInOutQuint:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutQuint);
			break;
		case DialogueriTween.EaseType.easeInSine:
			this.ease = new DialogueriTween.EasingFunction(this.easeInSine);
			break;
		case DialogueriTween.EaseType.easeOutSine:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutSine);
			break;
		case DialogueriTween.EaseType.easeInOutSine:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutSine);
			break;
		case DialogueriTween.EaseType.easeInExpo:
			this.ease = new DialogueriTween.EasingFunction(this.easeInExpo);
			break;
		case DialogueriTween.EaseType.easeOutExpo:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutExpo);
			break;
		case DialogueriTween.EaseType.easeInOutExpo:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutExpo);
			break;
		case DialogueriTween.EaseType.easeInCirc:
			this.ease = new DialogueriTween.EasingFunction(this.easeInCirc);
			break;
		case DialogueriTween.EaseType.easeOutCirc:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutCirc);
			break;
		case DialogueriTween.EaseType.easeInOutCirc:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutCirc);
			break;
		case DialogueriTween.EaseType.linear:
			this.ease = new DialogueriTween.EasingFunction(this.linear);
			break;
		case DialogueriTween.EaseType.spring:
			this.ease = new DialogueriTween.EasingFunction(this.spring);
			break;
		case DialogueriTween.EaseType.easeInBounce:
			this.ease = new DialogueriTween.EasingFunction(this.easeInBounce);
			break;
		case DialogueriTween.EaseType.easeOutBounce:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutBounce);
			break;
		case DialogueriTween.EaseType.easeInOutBounce:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutBounce);
			break;
		case DialogueriTween.EaseType.easeInBack:
			this.ease = new DialogueriTween.EasingFunction(this.easeInBack);
			break;
		case DialogueriTween.EaseType.easeOutBack:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutBack);
			break;
		case DialogueriTween.EaseType.easeInOutBack:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutBack);
			break;
		case DialogueriTween.EaseType.easeInElastic:
			this.ease = new DialogueriTween.EasingFunction(this.easeInElastic);
			break;
		case DialogueriTween.EaseType.easeOutElastic:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutElastic);
			break;
		case DialogueriTween.EaseType.easeInOutElastic:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutElastic);
			break;
		}
	}

	// Token: 0x06003F9B RID: 16283 RVA: 0x001282F8 File Offset: 0x001264F8
	public void UpdatePercentage()
	{
		if (this.useRealTime)
		{
			this.runningTime += Time.realtimeSinceStartup - this.lastRealTime;
		}
		else
		{
			this.runningTime += Time.deltaTime;
		}
		if (this.reverse)
		{
			this.percentage = 1f - this.runningTime / this.time;
		}
		else
		{
			this.percentage = this.runningTime / this.time;
		}
		this.lastRealTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06003F9C RID: 16284 RVA: 0x00128388 File Offset: 0x00126588
	public void CallBack(string callbackType)
	{
		if (this.tweenArguments.Contains(callbackType) && !this.tweenArguments.Contains("ischild"))
		{
			GameObject gameObject;
			if (this.tweenArguments.Contains(callbackType + "target"))
			{
				gameObject = (GameObject)this.tweenArguments[callbackType + "target"];
			}
			else
			{
				gameObject = base.gameObject;
			}
			if (this.tweenArguments[callbackType].GetType() == typeof(string))
			{
				gameObject.SendMessage((string)this.tweenArguments[callbackType], this.tweenArguments[callbackType + "params"], 1);
			}
			else
			{
				Debug.LogError("iTween Error: Callback method references must be passed as a String!", null);
				Object.Destroy(this);
			}
		}
	}

	// Token: 0x06003F9D RID: 16285 RVA: 0x00128464 File Offset: 0x00126664
	public void Dispose()
	{
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			if ((string)hashtable["id"] == this.id)
			{
				DialogueriTween.tweens.RemoveAt(i);
				break;
			}
		}
		Object.Destroy(this);
	}

	// Token: 0x06003F9E RID: 16286 RVA: 0x001284D4 File Offset: 0x001266D4
	public void ConflictCheck()
	{
		Component[] components = base.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			if (dialogueriTween.type == "value")
			{
				return;
			}
			if (dialogueriTween.isRunning && dialogueriTween.type == this.type)
			{
				if (dialogueriTween.method != this.method)
				{
					return;
				}
				if (dialogueriTween.tweenArguments.Count != this.tweenArguments.Count)
				{
					dialogueriTween.Dispose();
					return;
				}
				IDictionaryEnumerator enumerator = this.tweenArguments.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						if (!dialogueriTween.tweenArguments.Contains(dictionaryEntry.Key))
						{
							dialogueriTween.Dispose();
							return;
						}
						if (!dialogueriTween.tweenArguments[dictionaryEntry.Key].Equals(this.tweenArguments[dictionaryEntry.Key]) && (string)dictionaryEntry.Key != "id")
						{
							dialogueriTween.Dispose();
							return;
						}
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
				this.Dispose();
			}
		}
	}

	// Token: 0x06003F9F RID: 16287 RVA: 0x000331F2 File Offset: 0x000313F2
	public void EnableKinematic()
	{
	}

	// Token: 0x06003FA0 RID: 16288 RVA: 0x000331F4 File Offset: 0x000313F4
	public void DisableKinematic()
	{
	}

	// Token: 0x06003FA1 RID: 16289 RVA: 0x000331F6 File Offset: 0x000313F6
	public void ResumeDelay()
	{
		base.StartCoroutine("TweenDelay");
	}

	// Token: 0x06003FA2 RID: 16290 RVA: 0x00033204 File Offset: 0x00031404
	public float linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	// Token: 0x06003FA3 RID: 16291 RVA: 0x00128658 File Offset: 0x00126858
	public float clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float result;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * value;
			result = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * value;
			result = start + num4;
		}
		else
		{
			result = start + (end - start) * value;
		}
		return result;
	}

	// Token: 0x06003FA4 RID: 16292 RVA: 0x001286D0 File Offset: 0x001268D0
	public float spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * 3.14159274f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	// Token: 0x06003FA5 RID: 16293 RVA: 0x0003320E File Offset: 0x0003140E
	public float easeInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	// Token: 0x06003FA6 RID: 16294 RVA: 0x0003321C File Offset: 0x0003141C
	public float easeOutQuad(float start, float end, float value)
	{
		end -= start;
		return -end * value * (value - 2f) + start;
	}

	// Token: 0x06003FA7 RID: 16295 RVA: 0x00128734 File Offset: 0x00126934
	public float easeInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value + start;
		}
		value -= 1f;
		return -end / 2f * (value * (value - 2f) - 1f) + start;
	}

	// Token: 0x06003FA8 RID: 16296 RVA: 0x00033231 File Offset: 0x00031431
	public float easeInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	// Token: 0x06003FA9 RID: 16297 RVA: 0x00033241 File Offset: 0x00031441
	public float easeOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	// Token: 0x06003FAA RID: 16298 RVA: 0x0012878C File Offset: 0x0012698C
	public float easeInOutCubic(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value + 2f) + start;
	}

	// Token: 0x06003FAB RID: 16299 RVA: 0x00033260 File Offset: 0x00031460
	public float easeInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	// Token: 0x06003FAC RID: 16300 RVA: 0x00033272 File Offset: 0x00031472
	public float easeOutQuart(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return -end * (value * value * value * value - 1f) + start;
	}

	// Token: 0x06003FAD RID: 16301 RVA: 0x001287E0 File Offset: 0x001269E0
	public float easeInOutQuart(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value + start;
		}
		value -= 2f;
		return -end / 2f * (value * value * value * value - 2f) + start;
	}

	// Token: 0x06003FAE RID: 16302 RVA: 0x00033294 File Offset: 0x00031494
	public float easeInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	// Token: 0x06003FAF RID: 16303 RVA: 0x000332A8 File Offset: 0x000314A8
	public float easeOutQuint(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value * value * value + 1f) + start;
	}

	// Token: 0x06003FB0 RID: 16304 RVA: 0x0012883C File Offset: 0x00126A3C
	public float easeInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value * value * value + 2f) + start;
	}

	// Token: 0x06003FB1 RID: 16305 RVA: 0x000332CB File Offset: 0x000314CB
	public float easeInSine(float start, float end, float value)
	{
		end -= start;
		return -end * Mathf.Cos(value / 1f * 1.57079637f) + end + start;
	}

	// Token: 0x06003FB2 RID: 16306 RVA: 0x000332EB File Offset: 0x000314EB
	public float easeOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value / 1f * 1.57079637f) + start;
	}

	// Token: 0x06003FB3 RID: 16307 RVA: 0x00033308 File Offset: 0x00031508
	public float easeInOutSine(float start, float end, float value)
	{
		end -= start;
		return -end / 2f * (Mathf.Cos(3.14159274f * value / 1f) - 1f) + start;
	}

	// Token: 0x06003FB4 RID: 16308 RVA: 0x00033332 File Offset: 0x00031532
	public float easeInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value / 1f - 1f)) + start;
	}

	// Token: 0x06003FB5 RID: 16309 RVA: 0x0003335A File Offset: 0x0003155A
	public float easeOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2f, -10f * value / 1f) + 1f) + start;
	}

	// Token: 0x06003FB6 RID: 16310 RVA: 0x00128898 File Offset: 0x00126A98
	public float easeInOutExpo(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end / 2f * (-Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	// Token: 0x06003FB7 RID: 16311 RVA: 0x00033383 File Offset: 0x00031583
	public float easeInCirc(float start, float end, float value)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1f - value * value) - 1f) + start;
	}

	// Token: 0x06003FB8 RID: 16312 RVA: 0x000333A3 File Offset: 0x000315A3
	public float easeOutCirc(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - value * value) + start;
	}

	// Token: 0x06003FB9 RID: 16313 RVA: 0x0012890C File Offset: 0x00126B0C
	public float easeInOutCirc(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return -end / 2f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}
		value -= 2f;
		return end / 2f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
	}

	// Token: 0x06003FBA RID: 16314 RVA: 0x0012897C File Offset: 0x00126B7C
	public float easeInBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return end - this.easeOutBounce(0f, end, num - value) + start;
	}

	// Token: 0x06003FBB RID: 16315 RVA: 0x001289A8 File Offset: 0x00126BA8
	public float easeOutBounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.363636374f)
		{
			return end * (7.5625f * value * value) + start;
		}
		if (value < 0.727272749f)
		{
			value -= 0.545454562f;
			return end * (7.5625f * value * value + 0.75f) + start;
		}
		if ((double)value < 0.90909090909090906)
		{
			value -= 0.8181818f;
			return end * (7.5625f * value * value + 0.9375f) + start;
		}
		value -= 0.954545438f;
		return end * (7.5625f * value * value + 0.984375f) + start;
	}

	// Token: 0x06003FBC RID: 16316 RVA: 0x00128A50 File Offset: 0x00126C50
	public float easeInOutBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num / 2f)
		{
			return this.easeInBounce(0f, end, value * 2f) * 0.5f + start;
		}
		return this.easeOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
	}

	// Token: 0x06003FBD RID: 16317 RVA: 0x00128AB8 File Offset: 0x00126CB8
	public float easeInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	// Token: 0x06003FBE RID: 16318 RVA: 0x00128AEC File Offset: 0x00126CEC
	public float easeOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value = value / 1f - 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	// Token: 0x06003FBF RID: 16319 RVA: 0x00128B2C File Offset: 0x00126D2C
	public float easeInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end / 2f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end / 2f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	// Token: 0x06003FC0 RID: 16320 RVA: 0x00128BAC File Offset: 0x00126DAC
	public float punch(float amplitude, float value)
	{
		if (value == 0f)
		{
			return 0f;
		}
		if (value == 1f)
		{
			return 0f;
		}
		float num = 0.3f;
		float num2 = num / 6.28318548f * Mathf.Asin(0f);
		return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num2) * 6.28318548f / num);
	}

	// Token: 0x06003FC1 RID: 16321 RVA: 0x00128C24 File Offset: 0x00126E24
	public float easeInElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.28318548f * Mathf.Asin(end / num3);
		}
		return -(num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.28318548f / num2)) + start;
	}

	// Token: 0x06003FC2 RID: 16322 RVA: 0x00128CDC File Offset: 0x00126EDC
	public float easeOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.28318548f * Mathf.Asin(end / num3);
		}
		return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num4) * 6.28318548f / num2) + end + start;
	}

	// Token: 0x06003FC3 RID: 16323 RVA: 0x00128D8C File Offset: 0x00126F8C
	public float easeInOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num / 2f) == 2f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.28318548f * Mathf.Asin(end / num3);
		}
		if (value < 1f)
		{
			return -0.5f * (num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.28318548f / num2)) + start;
		}
		return num3 * Mathf.Pow(2f, -10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.28318548f / num2) * 0.5f + end + start;
	}

	// Token: 0x0400329C RID: 12956
	public static ArrayList tweens = new ArrayList();

	// Token: 0x0400329D RID: 12957
	public static GameObject cameraFade;

	// Token: 0x0400329E RID: 12958
	public string id;

	// Token: 0x0400329F RID: 12959
	public string type;

	// Token: 0x040032A0 RID: 12960
	public string method;

	// Token: 0x040032A1 RID: 12961
	public DialogueriTween.EaseType easeType;

	// Token: 0x040032A2 RID: 12962
	public float time;

	// Token: 0x040032A3 RID: 12963
	public float delay;

	// Token: 0x040032A4 RID: 12964
	public DialogueriTween.LoopType loopType;

	// Token: 0x040032A5 RID: 12965
	public bool isRunning;

	// Token: 0x040032A6 RID: 12966
	public bool isPaused;

	// Token: 0x040032A7 RID: 12967
	public string _name;

	// Token: 0x040032A8 RID: 12968
	public float runningTime;

	// Token: 0x040032A9 RID: 12969
	public float percentage;

	// Token: 0x040032AA RID: 12970
	public float delayStarted;

	// Token: 0x040032AB RID: 12971
	public bool kinematic;

	// Token: 0x040032AC RID: 12972
	public bool isLocal;

	// Token: 0x040032AD RID: 12973
	public bool loop;

	// Token: 0x040032AE RID: 12974
	public bool reverse;

	// Token: 0x040032AF RID: 12975
	public bool wasPaused;

	// Token: 0x040032B0 RID: 12976
	public bool physics;

	// Token: 0x040032B1 RID: 12977
	public Hashtable tweenArguments;

	// Token: 0x040032B2 RID: 12978
	public Space space;

	// Token: 0x040032B3 RID: 12979
	public DialogueriTween.EasingFunction ease;

	// Token: 0x040032B4 RID: 12980
	public DialogueriTween.ApplyTween apply;

	// Token: 0x040032B5 RID: 12981
	public AudioSource audioSource;

	// Token: 0x040032B6 RID: 12982
	public Vector3[] vector3s;

	// Token: 0x040032B7 RID: 12983
	public Vector2[] vector2s;

	// Token: 0x040032B8 RID: 12984
	public Color[,] colors;

	// Token: 0x040032B9 RID: 12985
	public float[] floats;

	// Token: 0x040032BA RID: 12986
	public Rect[] rects;

	// Token: 0x040032BB RID: 12987
	public DialogueriTween.CRSpline path;

	// Token: 0x040032BC RID: 12988
	public Vector3 preUpdate;

	// Token: 0x040032BD RID: 12989
	public Vector3 postUpdate;

	// Token: 0x040032BE RID: 12990
	public DialogueriTween.NamedValueColor namedcolorvalue;

	// Token: 0x040032BF RID: 12991
	public float lastRealTime;

	// Token: 0x040032C0 RID: 12992
	public bool useRealTime;

	// Token: 0x02001261 RID: 4705
	// (Invoke) Token: 0x0600818C RID: 33164
	public delegate float EasingFunction(float start, float end, float value);

	// Token: 0x02001262 RID: 4706
	// (Invoke) Token: 0x06008190 RID: 33168
	public delegate void ApplyTween();

	// Token: 0x02001263 RID: 4707
	public enum EaseType
	{
		// Token: 0x04007EE1 RID: 32481
		easeInQuad,
		// Token: 0x04007EE2 RID: 32482
		easeOutQuad,
		// Token: 0x04007EE3 RID: 32483
		easeInOutQuad,
		// Token: 0x04007EE4 RID: 32484
		easeInCubic,
		// Token: 0x04007EE5 RID: 32485
		easeOutCubic,
		// Token: 0x04007EE6 RID: 32486
		easeInOutCubic,
		// Token: 0x04007EE7 RID: 32487
		easeInQuart,
		// Token: 0x04007EE8 RID: 32488
		easeOutQuart,
		// Token: 0x04007EE9 RID: 32489
		easeInOutQuart,
		// Token: 0x04007EEA RID: 32490
		easeInQuint,
		// Token: 0x04007EEB RID: 32491
		easeOutQuint,
		// Token: 0x04007EEC RID: 32492
		easeInOutQuint,
		// Token: 0x04007EED RID: 32493
		easeInSine,
		// Token: 0x04007EEE RID: 32494
		easeOutSine,
		// Token: 0x04007EEF RID: 32495
		easeInOutSine,
		// Token: 0x04007EF0 RID: 32496
		easeInExpo,
		// Token: 0x04007EF1 RID: 32497
		easeOutExpo,
		// Token: 0x04007EF2 RID: 32498
		easeInOutExpo,
		// Token: 0x04007EF3 RID: 32499
		easeInCirc,
		// Token: 0x04007EF4 RID: 32500
		easeOutCirc,
		// Token: 0x04007EF5 RID: 32501
		easeInOutCirc,
		// Token: 0x04007EF6 RID: 32502
		linear,
		// Token: 0x04007EF7 RID: 32503
		spring,
		// Token: 0x04007EF8 RID: 32504
		easeInBounce,
		// Token: 0x04007EF9 RID: 32505
		easeOutBounce,
		// Token: 0x04007EFA RID: 32506
		easeInOutBounce,
		// Token: 0x04007EFB RID: 32507
		easeInBack,
		// Token: 0x04007EFC RID: 32508
		easeOutBack,
		// Token: 0x04007EFD RID: 32509
		easeInOutBack,
		// Token: 0x04007EFE RID: 32510
		easeInElastic,
		// Token: 0x04007EFF RID: 32511
		easeOutElastic,
		// Token: 0x04007F00 RID: 32512
		easeInOutElastic,
		// Token: 0x04007F01 RID: 32513
		punch
	}

	// Token: 0x02001264 RID: 4708
	public enum LoopType
	{
		// Token: 0x04007F03 RID: 32515
		none,
		// Token: 0x04007F04 RID: 32516
		loop,
		// Token: 0x04007F05 RID: 32517
		pingPong
	}

	// Token: 0x02001265 RID: 4709
	public enum NamedValueColor
	{
		// Token: 0x04007F07 RID: 32519
		_Color,
		// Token: 0x04007F08 RID: 32520
		_SpecColor,
		// Token: 0x04007F09 RID: 32521
		_Emission,
		// Token: 0x04007F0A RID: 32522
		_ReflectColor
	}

	// Token: 0x02001266 RID: 4710
	public static class Defaults
	{
		// Token: 0x04007F0B RID: 32523
		public static float time = 1f;

		// Token: 0x04007F0C RID: 32524
		public static float delay = 0f;

		// Token: 0x04007F0D RID: 32525
		public static DialogueriTween.NamedValueColor namedColorValue = DialogueriTween.NamedValueColor._Color;

		// Token: 0x04007F0E RID: 32526
		public static DialogueriTween.LoopType loopType = DialogueriTween.LoopType.none;

		// Token: 0x04007F0F RID: 32527
		public static DialogueriTween.EaseType easeType = DialogueriTween.EaseType.easeOutExpo;

		// Token: 0x04007F10 RID: 32528
		public static float lookSpeed = 3f;

		// Token: 0x04007F11 RID: 32529
		public static bool isLocal = false;

		// Token: 0x04007F12 RID: 32530
		public static Space space = 1;

		// Token: 0x04007F13 RID: 32531
		public static bool orientToPath = false;

		// Token: 0x04007F14 RID: 32532
		public static Color color = Color.white;

		// Token: 0x04007F15 RID: 32533
		public static float updateTimePercentage = 0.05f;

		// Token: 0x04007F16 RID: 32534
		public static float updateTime = 1f * DialogueriTween.Defaults.updateTimePercentage;

		// Token: 0x04007F17 RID: 32535
		public static int cameraFadeDepth = 999999;

		// Token: 0x04007F18 RID: 32536
		public static float lookAhead = 0.05f;

		// Token: 0x04007F19 RID: 32537
		public static bool useRealTime = false;

		// Token: 0x04007F1A RID: 32538
		public static Vector3 up = Vector3.up;
	}

	// Token: 0x02001267 RID: 4711
	public class CRSpline
	{
		// Token: 0x06008194 RID: 33172 RVA: 0x000565E3 File Offset: 0x000547E3
		public CRSpline(params Vector3[] pts)
		{
			this.pts = new Vector3[pts.Length];
			Array.Copy(pts, this.pts, pts.Length);
		}

		// Token: 0x06008195 RID: 33173 RVA: 0x0029A5E4 File Offset: 0x002987E4
		public Vector3 Interp(float t)
		{
			int num = this.pts.Length - 3;
			int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
			float num3 = t * (float)num - (float)num2;
			Vector3 vector = this.pts[num2];
			Vector3 vector2 = this.pts[num2 + 1];
			Vector3 vector3 = this.pts[num2 + 2];
			Vector3 vector4 = this.pts[num2 + 3];
			return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num3 * num3 * num3) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num3 * num3) + (-vector + vector3) * num3 + 2f * vector2);
		}

		// Token: 0x04007F1B RID: 32539
		public Vector3[] pts;
	}
}
