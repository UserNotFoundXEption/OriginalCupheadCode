using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D2 RID: 210
public class StringVariantGenerator
{
	// Token: 0x060009F2 RID: 2546 RVA: 0x0007A664 File Offset: 0x00078864
	public StringVariantGenerator()
	{
		this.characterGenerators = new Dictionary<char, StringVariantGenerator.CharacterGenerator>
		{
			{
				'a',
				new StringVariantGenerator.CharacterGenerator("aA*", null, null)
			},
			{
				'b',
				new StringVariantGenerator.CharacterGenerator("bB(", null, null)
			},
			{
				'c',
				new StringVariantGenerator.CharacterGenerator("cC)", null, null)
			},
			{
				'd',
				new StringVariantGenerator.CharacterGenerator("dD", null, null)
			},
			{
				'e',
				new StringVariantGenerator.CharacterGenerator("eE&", null, null)
			},
			{
				'f',
				new StringVariantGenerator.CharacterGenerator("fF", null, null)
			},
			{
				'g',
				new StringVariantGenerator.CharacterGenerator("gG", null, null)
			},
			{
				'h',
				new StringVariantGenerator.CharacterGenerator("hH-", null, null)
			},
			{
				'i',
				new StringVariantGenerator.CharacterGenerator("iI", null, null)
			},
			{
				'j',
				new StringVariantGenerator.CharacterGenerator("jJ", null, null)
			},
			{
				'k',
				new StringVariantGenerator.CharacterGenerator("kK", null, null)
			},
			{
				'l',
				new StringVariantGenerator.CharacterGenerator("lL%", null, null)
			},
			{
				'm',
				new StringVariantGenerator.CharacterGenerator("mM", null, null)
			},
			{
				'n',
				new StringVariantGenerator.CharacterGenerator("nN^", null, null)
			},
			{
				'o',
				new StringVariantGenerator.CharacterGenerator("oO+", null, null)
			},
			{
				'p',
				new StringVariantGenerator.CharacterGenerator("pP", null, null)
			},
			{
				'q',
				new StringVariantGenerator.CharacterGenerator("qQ", null, null)
			},
			{
				'r',
				new StringVariantGenerator.CharacterGenerator("rR@", null, null)
			},
			{
				's',
				new StringVariantGenerator.CharacterGenerator("sS#", null, null)
			},
			{
				't',
				new StringVariantGenerator.CharacterGenerator("tT$", null, null)
			},
			{
				'u',
				new StringVariantGenerator.CharacterGenerator("uU", null, null)
			},
			{
				'v',
				new StringVariantGenerator.CharacterGenerator("vV", null, null)
			},
			{
				'w',
				new StringVariantGenerator.CharacterGenerator("wW", null, null)
			},
			{
				'x',
				new StringVariantGenerator.CharacterGenerator("xX", null, null)
			},
			{
				'y',
				new StringVariantGenerator.CharacterGenerator("yY", null, null)
			},
			{
				'z',
				new StringVariantGenerator.CharacterGenerator("zZ", null, null)
			},
			{
				'-',
				new StringVariantGenerator.CharacterGenerator(":;", new List<string>
				{
					"["
				}, new List<string>
				{
					"["
				})
			},
			{
				'!',
				new StringVariantGenerator.CharacterGenerator("!1", new List<string>
				{
					"[",
					"{",
					"[[",
					"{{"
				}, null)
			},
			{
				'~',
				new StringVariantGenerator.CharacterGenerator("~`", new List<string>
				{
					"[",
					"{",
					"[[",
					"{{"
				}, null)
			},
			{
				'\'',
				new StringVariantGenerator.CharacterGenerator("'\"", new List<string>
				{
					"[",
					"{",
					string.Empty
				}, new List<string>
				{
					"[",
					"{",
					string.Empty
				})
			},
			{
				'.',
				new StringVariantGenerator.CharacterGenerator(".>", new List<string>
				{
					"[",
					"{",
					"[[",
					"{{"
				}, null)
			},
			{
				',',
				new StringVariantGenerator.CharacterGenerator(",<", new List<string>
				{
					"[",
					"{",
					string.Empty
				}, new List<string>
				{
					"[",
					"{",
					string.Empty
				})
			},
			{
				'?',
				new StringVariantGenerator.CharacterGenerator("?/", new List<string>
				{
					"[",
					"{",
					"[[",
					"{{"
				}, null)
			},
			{
				' ',
				new StringVariantGenerator.CharacterGenerator("   ]  }", null, null)
			}
		};
	}

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x060009F3 RID: 2547 RVA: 0x000091EA File Offset: 0x000073EA
	public static StringVariantGenerator Instance
	{
		get
		{
			if (StringVariantGenerator._instance == null)
			{
				StringVariantGenerator._instance = new StringVariantGenerator();
			}
			return StringVariantGenerator._instance;
		}
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x0007AAB4 File Offset: 0x00078CB4
	public string Generate(string input)
	{
		foreach (StringVariantGenerator.CharacterGenerator characterGenerator in this.characterGenerators.Values)
		{
			characterGenerator.Init();
		}
		string text = string.Empty;
		bool flag = false;
		input = input.Replace(" -", "-");
		input = input.Replace("- ", "-");
		foreach (char c in input)
		{
			if (c == '<')
			{
				flag = true;
			}
			if (c == '>')
			{
				flag = false;
			}
			if (!flag && this.characterGenerators.ContainsKey(c))
			{
				text += this.characterGenerators[c].generate();
			}
			else
			{
				text += c;
			}
		}
		return text;
	}

	// Token: 0x04000799 RID: 1945
	public static StringVariantGenerator _instance;

	// Token: 0x0400079A RID: 1946
	public Dictionary<char, StringVariantGenerator.CharacterGenerator> characterGenerators;

	// Token: 0x02000944 RID: 2372
	public class CharacterGenerator
	{
		// Token: 0x0600547F RID: 21631 RVA: 0x00040080 File Offset: 0x0003E280
		public CharacterGenerator(string variants, List<string> randomPrefixes = null, List<string> randomSuffixes = null)
		{
			this.variants = variants;
			this.randomPrefixes = randomPrefixes;
			this.randomSuffixes = randomSuffixes;
		}

		// Token: 0x06005480 RID: 21632 RVA: 0x0004009D File Offset: 0x0003E29D
		public void Init()
		{
			this.currentIndex = Random.Range(0, this.variants.Length);
		}

		// Token: 0x06005481 RID: 21633 RVA: 0x001C322C File Offset: 0x001C142C
		public string generate()
		{
			this.currentIndex = (this.currentIndex + 1) % this.variants.Length;
			string text = this.variants[this.currentIndex].ToString();
			if (this.randomPrefixes != null)
			{
				text = this.randomPrefixes.RandomChoice<string>() + text;
			}
			if (this.randomSuffixes != null)
			{
				text += this.randomSuffixes.RandomChoice<string>();
			}
			return text;
		}

		// Token: 0x040045CC RID: 17868
		public int currentIndex;

		// Token: 0x040045CD RID: 17869
		public string variants;

		// Token: 0x040045CE RID: 17870
		public List<string> randomPrefixes;

		// Token: 0x040045CF RID: 17871
		public List<string> randomSuffixes;
	}
}
