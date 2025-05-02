using System;
using System.Collections;
using UnityEngine.SceneManagement;

// Token: 0x020000DF RID: 223
public class LoadFirstScene : AbstractMonoBehaviour
{
	// Token: 0x06000A52 RID: 2642 RVA: 0x00009626 File Offset: 0x00007826
	public void Start()
	{
		SceneManager.LoadScene(0);
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x0007B740 File Offset: 0x00079940
	public IEnumerator load_cr()
	{
		yield return null;
		yield break;
	}
}
