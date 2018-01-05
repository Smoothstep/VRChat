using System;
using System.Diagnostics;
using System.Text;
using UnityEngine;

// Token: 0x020005F0 RID: 1520
public static class NGUIText
{
	// Token: 0x0600328B RID: 12939 RVA: 0x000FB459 File Offset: 0x000F9859
	public static void Update()
	{
		NGUIText.Update(true);
	}

	// Token: 0x0600328C RID: 12940 RVA: 0x000FB464 File Offset: 0x000F9864
	public static void Update(bool request)
	{
		NGUIText.finalSize = Mathf.RoundToInt((float)NGUIText.fontSize / NGUIText.pixelDensity);
		NGUIText.finalSpacingX = NGUIText.spacingX * NGUIText.fontScale;
		NGUIText.finalLineHeight = ((float)NGUIText.fontSize + NGUIText.spacingY) * NGUIText.fontScale;
		NGUIText.useSymbols = (NGUIText.bitmapFont != null && NGUIText.bitmapFont.hasSymbols && NGUIText.encoding && NGUIText.symbolStyle != NGUIText.SymbolStyle.None);
		if (NGUIText.dynamicFont != null && request)
		{
			NGUIText.dynamicFont.RequestCharactersInTexture(")_-", NGUIText.finalSize, NGUIText.fontStyle);
			if (!NGUIText.dynamicFont.GetCharacterInfo(')', out NGUIText.mTempChar, NGUIText.finalSize, NGUIText.fontStyle) || (float)NGUIText.mTempChar.maxY == 0f)
			{
				NGUIText.dynamicFont.RequestCharactersInTexture("A", NGUIText.finalSize, NGUIText.fontStyle);
				if (!NGUIText.dynamicFont.GetCharacterInfo('A', out NGUIText.mTempChar, NGUIText.finalSize, NGUIText.fontStyle))
				{
					NGUIText.baseline = 0f;
					return;
				}
			}
			float num = (float)NGUIText.mTempChar.maxY;
			float num2 = (float)NGUIText.mTempChar.minY;
			NGUIText.baseline = Mathf.Round(num + ((float)NGUIText.finalSize - num + num2) * 0.5f);
		}
	}

	// Token: 0x0600328D RID: 12941 RVA: 0x000FB5C6 File Offset: 0x000F99C6
	public static void Prepare(string text)
	{
		if (NGUIText.dynamicFont != null)
		{
			NGUIText.dynamicFont.RequestCharactersInTexture(text, NGUIText.finalSize, NGUIText.fontStyle);
		}
	}

	// Token: 0x0600328E RID: 12942 RVA: 0x000FB5ED File Offset: 0x000F99ED
	public static BMSymbol GetSymbol(string text, int index, int textLength)
	{
		return (!(NGUIText.bitmapFont != null)) ? null : NGUIText.bitmapFont.MatchSymbol(text, index, textLength);
	}

	// Token: 0x0600328F RID: 12943 RVA: 0x000FB614 File Offset: 0x000F9A14
	public static float GetGlyphWidth(int ch, int prev)
	{
		if (NGUIText.bitmapFont != null)
		{
			bool flag = false;
			if (ch == 8201)
			{
				flag = true;
				ch = 32;
			}
			BMGlyph bmglyph = NGUIText.bitmapFont.bmFont.GetGlyph(ch);
			if (bmglyph != null)
			{
				int num = bmglyph.advance;
				if (flag)
				{
					num >>= 1;
				}
				return NGUIText.fontScale * (float)((prev == 0) ? bmglyph.advance : (num + bmglyph.GetKerning(prev)));
			}
		}
		else if (NGUIText.dynamicFont != null && NGUIText.dynamicFont.GetCharacterInfo((char)ch, out NGUIText.mTempChar, NGUIText.finalSize, NGUIText.fontStyle))
		{
			return (float)NGUIText.mTempChar.advance * NGUIText.fontScale * NGUIText.pixelDensity;
		}
		return 0f;
	}

	// Token: 0x06003290 RID: 12944 RVA: 0x000FB6E0 File Offset: 0x000F9AE0
	public static NGUIText.GlyphInfo GetGlyph(int ch, int prev)
	{
		if (NGUIText.bitmapFont != null)
		{
			bool flag = false;
			if (ch == 8201)
			{
				flag = true;
				ch = 32;
			}
			BMGlyph bmglyph = NGUIText.bitmapFont.bmFont.GetGlyph(ch);
			if (bmglyph != null)
			{
				int num = (prev == 0) ? 0 : bmglyph.GetKerning(prev);
				NGUIText.glyph.v0.x = (float)((prev == 0) ? bmglyph.offsetX : (bmglyph.offsetX + num));
				NGUIText.glyph.v1.y = (float)(-(float)bmglyph.offsetY);
				NGUIText.glyph.v1.x = NGUIText.glyph.v0.x + (float)bmglyph.width;
				NGUIText.glyph.v0.y = NGUIText.glyph.v1.y - (float)bmglyph.height;
				NGUIText.glyph.u0.x = (float)bmglyph.x;
				NGUIText.glyph.u0.y = (float)(bmglyph.y + bmglyph.height);
				NGUIText.glyph.u1.x = (float)(bmglyph.x + bmglyph.width);
				NGUIText.glyph.u1.y = (float)bmglyph.y;
				int num2 = bmglyph.advance;
				if (flag)
				{
					num2 >>= 1;
				}
				NGUIText.glyph.advance = (float)(num2 + num);
				NGUIText.glyph.channel = bmglyph.channel;
				NGUIText.glyph.rotatedUVs = false;
				if (NGUIText.fontScale != 1f)
				{
					NGUIText.glyph.v0 *= NGUIText.fontScale;
					NGUIText.glyph.v1 *= NGUIText.fontScale;
					NGUIText.glyph.advance *= NGUIText.fontScale;
				}
				return NGUIText.glyph;
			}
		}
		else if (NGUIText.dynamicFont != null && NGUIText.dynamicFont.GetCharacterInfo((char)ch, out NGUIText.mTempChar, NGUIText.finalSize, NGUIText.fontStyle))
		{
			NGUIText.glyph.v0.x = (float)NGUIText.mTempChar.minX;
			NGUIText.glyph.v1.x = (float)NGUIText.mTempChar.maxX;
			NGUIText.glyph.v0.y = (float)NGUIText.mTempChar.maxY - NGUIText.baseline;
			NGUIText.glyph.v1.y = (float)NGUIText.mTempChar.minY - NGUIText.baseline;
			NGUIText.glyph.u0 = NGUIText.mTempChar.uvBottomLeft;
			NGUIText.glyph.u1 = NGUIText.mTempChar.uvTopRight;
			NGUIText.glyph.advance = (float)NGUIText.mTempChar.advance;
			NGUIText.glyph.channel = 0;
			NGUIText.glyph.rotatedUVs = false;
			NGUIText.glyph.v0.x = Mathf.Round(NGUIText.glyph.v0.x);
			NGUIText.glyph.v0.y = Mathf.Round(NGUIText.glyph.v0.y);
			NGUIText.glyph.v1.x = Mathf.Round(NGUIText.glyph.v1.x);
			NGUIText.glyph.v1.y = Mathf.Round(NGUIText.glyph.v1.y);
			float num3 = NGUIText.fontScale * NGUIText.pixelDensity;
			if (num3 != 1f)
			{
				NGUIText.glyph.v0 *= num3;
				NGUIText.glyph.v1 *= num3;
				NGUIText.glyph.advance *= num3;
			}
			return NGUIText.glyph;
		}
		return null;
	}

	// Token: 0x06003291 RID: 12945 RVA: 0x000FBAB0 File Offset: 0x000F9EB0
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static float ParseAlpha(string text, int index)
	{
		int num = NGUIMath.HexToDecimal(text[index + 1]) << 4 | NGUIMath.HexToDecimal(text[index + 2]);
		return Mathf.Clamp01((float)num / 255f);
	}

	// Token: 0x06003292 RID: 12946 RVA: 0x000FBAEA File Offset: 0x000F9EEA
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static Color ParseColor(string text, int offset)
	{
		return NGUIText.ParseColor24(text, offset);
	}

	// Token: 0x06003293 RID: 12947 RVA: 0x000FBAF4 File Offset: 0x000F9EF4
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static Color ParseColor24(string text, int offset)
	{
		int num = NGUIMath.HexToDecimal(text[offset]) << 4 | NGUIMath.HexToDecimal(text[offset + 1]);
		int num2 = NGUIMath.HexToDecimal(text[offset + 2]) << 4 | NGUIMath.HexToDecimal(text[offset + 3]);
		int num3 = NGUIMath.HexToDecimal(text[offset + 4]) << 4 | NGUIMath.HexToDecimal(text[offset + 5]);
		float num4 = 0.003921569f;
		return new Color(num4 * (float)num, num4 * (float)num2, num4 * (float)num3);
	}

	// Token: 0x06003294 RID: 12948 RVA: 0x000FBB78 File Offset: 0x000F9F78
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static Color ParseColor32(string text, int offset)
	{
		int num = NGUIMath.HexToDecimal(text[offset]) << 4 | NGUIMath.HexToDecimal(text[offset + 1]);
		int num2 = NGUIMath.HexToDecimal(text[offset + 2]) << 4 | NGUIMath.HexToDecimal(text[offset + 3]);
		int num3 = NGUIMath.HexToDecimal(text[offset + 4]) << 4 | NGUIMath.HexToDecimal(text[offset + 5]);
		int num4 = NGUIMath.HexToDecimal(text[offset + 6]) << 4 | NGUIMath.HexToDecimal(text[offset + 7]);
		float num5 = 0.003921569f;
		return new Color(num5 * (float)num, num5 * (float)num2, num5 * (float)num3, num5 * (float)num4);
	}

	// Token: 0x06003295 RID: 12949 RVA: 0x000FBC23 File Offset: 0x000FA023
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string EncodeColor(Color c)
	{
		return NGUIText.EncodeColor24(c);
	}

	// Token: 0x06003296 RID: 12950 RVA: 0x000FBC2C File Offset: 0x000FA02C
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string EncodeAlpha(float a)
	{
		int num = Mathf.Clamp(Mathf.RoundToInt(a * 255f), 0, 255);
		return NGUIMath.DecimalToHex8(num);
	}

	// Token: 0x06003297 RID: 12951 RVA: 0x000FBC58 File Offset: 0x000FA058
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string EncodeColor24(Color c)
	{
		int num = 16777215 & NGUIMath.ColorToInt(c) >> 8;
		return NGUIMath.DecimalToHex24(num);
	}

	// Token: 0x06003298 RID: 12952 RVA: 0x000FBC7C File Offset: 0x000FA07C
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string EncodeColor32(Color c)
	{
		int num = NGUIMath.ColorToInt(c);
		return NGUIMath.DecimalToHex32(num);
	}

	// Token: 0x06003299 RID: 12953 RVA: 0x000FBC98 File Offset: 0x000FA098
	public static bool ParseSymbol(string text, ref int index)
	{
		int num = 1;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		return NGUIText.ParseSymbol(text, ref index, null, false, ref num, ref flag, ref flag2, ref flag3, ref flag4, ref flag5);
	}

	// Token: 0x0600329A RID: 12954 RVA: 0x000FBCC8 File Offset: 0x000FA0C8
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static bool IsHex(char ch)
	{
		return (ch >= '0' && ch <= '9') || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F');
	}

	// Token: 0x0600329B RID: 12955 RVA: 0x000FBD00 File Offset: 0x000FA100
	public static bool ParseSymbol(string text, ref int index, BetterList<Color> colors, bool premultiply, ref int sub, ref bool bold, ref bool italic, ref bool underline, ref bool strike, ref bool ignoreColor)
	{
		int length = text.Length;
		if (index + 3 > length || text[index] != '[')
		{
			return false;
		}
		if (text[index + 2] == ']')
		{
			if (text[index + 1] == '-')
			{
				if (colors != null && colors.size > 1)
				{
					colors.RemoveAt(colors.size - 1);
				}
				index += 3;
				return true;
			}
			string text2 = text.Substring(index, 3);
			if (text2 != null)
			{
				if (text2 == "[b]")
				{
					bold = true;
					index += 3;
					return true;
				}
				if (text2 == "[i]")
				{
					italic = true;
					index += 3;
					return true;
				}
				if (text2 == "[u]")
				{
					underline = true;
					index += 3;
					return true;
				}
				if (text2 == "[s]")
				{
					strike = true;
					index += 3;
					return true;
				}
				if (text2 == "[c]")
				{
					ignoreColor = true;
					index += 3;
					return true;
				}
			}
		}
		if (index + 4 > length)
		{
			return false;
		}
		if (text[index + 3] == ']')
		{
			string text3 = text.Substring(index, 4);
			if (text3 != null)
			{
				if (text3 == "[/b]")
				{
					bold = false;
					index += 4;
					return true;
				}
				if (text3 == "[/i]")
				{
					italic = false;
					index += 4;
					return true;
				}
				if (text3 == "[/u]")
				{
					underline = false;
					index += 4;
					return true;
				}
				if (text3 == "[/s]")
				{
					strike = false;
					index += 4;
					return true;
				}
				if (text3 == "[/c]")
				{
					ignoreColor = false;
					index += 4;
					return true;
				}
			}
			char ch = text[index + 1];
			char ch2 = text[index + 2];
			if (NGUIText.IsHex(ch) && NGUIText.IsHex(ch2))
			{
				int num = NGUIMath.HexToDecimal(ch) << 4 | NGUIMath.HexToDecimal(ch2);
				NGUIText.mAlpha = (float)num / 255f;
				index += 4;
				return true;
			}
		}
		if (index + 5 > length)
		{
			return false;
		}
		if (text[index + 4] == ']')
		{
			string text4 = text.Substring(index, 5);
			if (text4 != null)
			{
				if (text4 == "[sub]")
				{
					sub = 1;
					index += 5;
					return true;
				}
				if (text4 == "[sup]")
				{
					sub = 2;
					index += 5;
					return true;
				}
			}
		}
		if (index + 6 > length)
		{
			return false;
		}
		if (text[index + 5] == ']')
		{
			string text5 = text.Substring(index, 6);
			if (text5 != null)
			{
				if (text5 == "[/sub]")
				{
					sub = 0;
					index += 6;
					return true;
				}
				if (text5 == "[/sup]")
				{
					sub = 0;
					index += 6;
					return true;
				}
				if (text5 == "[/url]")
				{
					index += 6;
					return true;
				}
			}
		}
		if (text[index + 1] == 'u' && text[index + 2] == 'r' && text[index + 3] == 'l' && text[index + 4] == '=')
		{
			int num2 = text.IndexOf(']', index + 4);
			if (num2 != -1)
			{
				index = num2 + 1;
				return true;
			}
			index = text.Length;
			return true;
		}
		else
		{
			if (index + 8 > length)
			{
				return false;
			}
			if (text[index + 7] == ']')
			{
				Color color = NGUIText.ParseColor24(text, index + 1);
				if (NGUIText.EncodeColor24(color) != text.Substring(index + 1, 6).ToUpper())
				{
					return false;
				}
				if (colors != null)
				{
					color.a = colors[colors.size - 1].a;
					if (premultiply && color.a != 1f)
					{
						color = Color.Lerp(NGUIText.mInvisible, color, color.a);
					}
					colors.Add(color);
				}
				index += 8;
				return true;
			}
			else
			{
				if (index + 10 > length)
				{
					return false;
				}
				if (text[index + 9] != ']')
				{
					return false;
				}
				Color color2 = NGUIText.ParseColor32(text, index + 1);
				if (NGUIText.EncodeColor32(color2) != text.Substring(index + 1, 8).ToUpper())
				{
					return false;
				}
				if (colors != null)
				{
					if (premultiply && color2.a != 1f)
					{
						color2 = Color.Lerp(NGUIText.mInvisible, color2, color2.a);
					}
					colors.Add(color2);
				}
				index += 10;
				return true;
			}
		}
	}

	// Token: 0x0600329C RID: 12956 RVA: 0x000FC1D4 File Offset: 0x000FA5D4
	public static string StripSymbols(string text)
	{
		if (text != null)
		{
			int i = 0;
			int length = text.Length;
			while (i < length)
			{
				char c = text[i];
				if (c == '[')
				{
					int num = 0;
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					bool flag4 = false;
					bool flag5 = false;
					int num2 = i;
					if (NGUIText.ParseSymbol(text, ref num2, null, false, ref num, ref flag, ref flag2, ref flag3, ref flag4, ref flag5))
					{
						text = text.Remove(i, num2 - i);
						length = text.Length;
						continue;
					}
				}
				i++;
			}
		}
		return text;
	}

	// Token: 0x0600329D RID: 12957 RVA: 0x000FC25C File Offset: 0x000FA65C
	public static void Align(BetterList<Vector3> verts, int indexOffset, float printedWidth)
	{
		NGUIText.Alignment alignment = NGUIText.alignment;
		if (alignment != NGUIText.Alignment.Right)
		{
			if (alignment != NGUIText.Alignment.Center)
			{
				if (alignment == NGUIText.Alignment.Justified)
				{
					if (printedWidth < (float)NGUIText.rectWidth * 0.65f)
					{
						return;
					}
					float num = ((float)NGUIText.rectWidth - printedWidth) * 0.5f;
					if (num < 1f)
					{
						return;
					}
					int num2 = (verts.size - indexOffset) / 4;
					if (num2 < 1)
					{
						return;
					}
					float num3 = 1f / (float)(num2 - 1);
					float num4 = (float)NGUIText.rectWidth / printedWidth;
					int i = indexOffset + 4;
					int num5 = 1;
					while (i < verts.size)
					{
						float num6 = verts.buffer[i].x;
						float num7 = verts.buffer[i + 2].x;
						float num8 = num7 - num6;
						float num9 = num6 * num4;
						float a = num9 + num8;
						float num10 = num7 * num4;
						float b = num10 - num8;
						float t = (float)num5 * num3;
						num6 = Mathf.Lerp(num9, b, t);
						num7 = Mathf.Lerp(a, num10, t);
						num6 = Mathf.Round(num6);
						num7 = Mathf.Round(num7);
						verts.buffer[i++].x = num6;
						verts.buffer[i++].x = num6;
						verts.buffer[i++].x = num7;
						verts.buffer[i++].x = num7;
						num5++;
					}
				}
			}
			else
			{
				float num11 = ((float)NGUIText.rectWidth - printedWidth) * 0.5f;
				if (num11 < 0f)
				{
					return;
				}
				int num12 = Mathf.RoundToInt((float)NGUIText.rectWidth - printedWidth);
				int num13 = Mathf.RoundToInt((float)NGUIText.rectWidth);
				bool flag = (num12 & 1) == 1;
				bool flag2 = (num13 & 1) == 1;
				if ((flag && !flag2) || (!flag && flag2))
				{
					num11 += 0.5f * NGUIText.fontScale;
				}
				for (int j = indexOffset; j < verts.size; j++)
				{
					Vector3[] buffer = verts.buffer;
					int num14 = j;
					buffer[num14].x = buffer[num14].x + num11;
				}
			}
		}
		else
		{
			float num15 = (float)NGUIText.rectWidth - printedWidth;
			if (num15 < 0f)
			{
				return;
			}
			for (int k = indexOffset; k < verts.size; k++)
			{
				Vector3[] buffer2 = verts.buffer;
				int num16 = k;
				buffer2[num16].x = buffer2[num16].x + num15;
			}
		}
	}

	// Token: 0x0600329E RID: 12958 RVA: 0x000FC4E8 File Offset: 0x000FA8E8
	public static int GetExactCharacterIndex(BetterList<Vector3> verts, BetterList<int> indices, Vector2 pos)
	{
		for (int i = 0; i < indices.size; i++)
		{
			int num = i << 1;
			int i2 = num + 1;
			float x = verts[num].x;
			if (pos.x >= x)
			{
				float x2 = verts[i2].x;
				if (pos.x <= x2)
				{
					float y = verts[num].y;
					if (pos.y >= y)
					{
						float y2 = verts[i2].y;
						if (pos.y <= y2)
						{
							return indices[i];
						}
					}
				}
			}
		}
		return 0;
	}

	// Token: 0x0600329F RID: 12959 RVA: 0x000FC5B0 File Offset: 0x000FA9B0
	public static int GetApproximateCharacterIndex(BetterList<Vector3> verts, BetterList<int> indices, Vector2 pos)
	{
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		int i = 0;
		for (int j = 0; j < verts.size; j++)
		{
			float num3 = Mathf.Abs(pos.y - verts[j].y);
			if (num3 <= num2)
			{
				float num4 = Mathf.Abs(pos.x - verts[j].x);
				if (num3 < num2)
				{
					num2 = num3;
					num = num4;
					i = j;
				}
				else if (num4 < num)
				{
					num = num4;
					i = j;
				}
			}
		}
		return indices[i];
	}

	// Token: 0x060032A0 RID: 12960 RVA: 0x000FC656 File Offset: 0x000FAA56
	[DebuggerHidden]
	[DebuggerStepThrough]
	private static bool IsSpace(int ch)
	{
		return ch == 32 || ch == 8202 || ch == 8203 || ch == 8201;
	}

	// Token: 0x060032A1 RID: 12961 RVA: 0x000FC684 File Offset: 0x000FAA84
	[DebuggerHidden]
	[DebuggerStepThrough]
	public static void EndLine(ref StringBuilder s)
	{
		int num = s.Length - 1;
		if (num > 0 && NGUIText.IsSpace((int)s[num]))
		{
			s[num] = '\n';
		}
		else
		{
			s.Append('\n');
		}
	}

	// Token: 0x060032A2 RID: 12962 RVA: 0x000FC6D0 File Offset: 0x000FAAD0
	[DebuggerHidden]
	[DebuggerStepThrough]
	private static void ReplaceSpaceWithNewline(ref StringBuilder s)
	{
		int num = s.Length - 1;
		if (num > 0 && NGUIText.IsSpace((int)s[num]))
		{
			s[num] = '\n';
		}
	}

	// Token: 0x060032A3 RID: 12963 RVA: 0x000FC70C File Offset: 0x000FAB0C
	public static Vector2 CalculatePrintedSize(string text)
	{
		Vector2 zero = Vector2.zero;
		if (!string.IsNullOrEmpty(text))
		{
			if (NGUIText.encoding)
			{
				text = NGUIText.StripSymbols(text);
			}
			NGUIText.Prepare(text);
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			int length = text.Length;
			int prev = 0;
			for (int i = 0; i < length; i++)
			{
				int num4 = (int)text[i];
				if (num4 == 10)
				{
					if (num > num3)
					{
						num3 = num;
					}
					num = 0f;
					num2 += NGUIText.finalLineHeight;
				}
				else if (num4 >= 32)
				{
					BMSymbol bmsymbol = (!NGUIText.useSymbols) ? null : NGUIText.GetSymbol(text, i, length);
					if (bmsymbol == null)
					{
						float num5 = NGUIText.GetGlyphWidth(num4, prev);
						if (num5 != 0f)
						{
							num5 += NGUIText.finalSpacingX;
							if (Mathf.RoundToInt(num + num5) > NGUIText.regionWidth)
							{
								if (num > num3)
								{
									num3 = num - NGUIText.finalSpacingX;
								}
								num = num5;
								num2 += NGUIText.finalLineHeight;
							}
							else
							{
								num += num5;
							}
							prev = num4;
						}
					}
					else
					{
						float num6 = NGUIText.finalSpacingX + (float)bmsymbol.advance * NGUIText.fontScale;
						if (Mathf.RoundToInt(num + num6) > NGUIText.regionWidth)
						{
							if (num > num3)
							{
								num3 = num - NGUIText.finalSpacingX;
							}
							num = num6;
							num2 += NGUIText.finalLineHeight;
						}
						else
						{
							num += num6;
						}
						i += bmsymbol.sequence.Length - 1;
						prev = 0;
					}
				}
			}
			zero.x = ((num <= num3) ? num3 : (num - NGUIText.finalSpacingX));
			zero.y = num2 + NGUIText.finalLineHeight;
		}
		return zero;
	}

	// Token: 0x060032A4 RID: 12964 RVA: 0x000FC8C8 File Offset: 0x000FACC8
	public static int CalculateOffsetToFit(string text)
	{
		if (string.IsNullOrEmpty(text) || NGUIText.regionWidth < 1)
		{
			return 0;
		}
		NGUIText.Prepare(text);
		int length = text.Length;
		int prev = 0;
		int i = 0;
		int length2 = text.Length;
		while (i < length2)
		{
			BMSymbol bmsymbol = (!NGUIText.useSymbols) ? null : NGUIText.GetSymbol(text, i, length);
			if (bmsymbol == null)
			{
				int num = (int)text[i];
				float glyphWidth = NGUIText.GetGlyphWidth(num, prev);
				if (glyphWidth != 0f)
				{
					NGUIText.mSizes.Add(NGUIText.finalSpacingX + glyphWidth);
				}
				prev = num;
			}
			else
			{
				NGUIText.mSizes.Add(NGUIText.finalSpacingX + (float)bmsymbol.advance * NGUIText.fontScale);
				int j = 0;
				int num2 = bmsymbol.sequence.Length - 1;
				while (j < num2)
				{
					NGUIText.mSizes.Add(0f);
					j++;
				}
				i += bmsymbol.sequence.Length - 1;
				prev = 0;
			}
			i++;
		}
		float num3 = (float)NGUIText.regionWidth;
		int num4 = NGUIText.mSizes.size;
		while (num4 > 0 && num3 > 0f)
		{
			num3 -= NGUIText.mSizes[--num4];
		}
		NGUIText.mSizes.Clear();
		if (num3 < 0f)
		{
			num4++;
		}
		return num4;
	}

	// Token: 0x060032A5 RID: 12965 RVA: 0x000FCA38 File Offset: 0x000FAE38
	public static string GetEndOfLineThatFits(string text)
	{
		int length = text.Length;
		int num = NGUIText.CalculateOffsetToFit(text);
		return text.Substring(num, length - num);
	}

	// Token: 0x060032A6 RID: 12966 RVA: 0x000FCA5D File Offset: 0x000FAE5D
	public static bool WrapText(string text, out string finalText)
	{
		return NGUIText.WrapText(text, out finalText, false);
	}

	// Token: 0x060032A7 RID: 12967 RVA: 0x000FCA68 File Offset: 0x000FAE68
	public static bool WrapText(string text, out string finalText, bool keepCharCount)
	{
		if (NGUIText.regionWidth < 1 || NGUIText.regionHeight < 1 || NGUIText.finalLineHeight < 1f)
		{
			finalText = string.Empty;
			return false;
		}
		float num = (NGUIText.maxLines <= 0) ? ((float)NGUIText.regionHeight) : Mathf.Min((float)NGUIText.regionHeight, NGUIText.finalLineHeight * (float)NGUIText.maxLines);
		int num2 = (NGUIText.maxLines <= 0) ? 1000000 : NGUIText.maxLines;
		num2 = Mathf.FloorToInt(Mathf.Min((float)num2, num / NGUIText.finalLineHeight) + 0.01f);
		if (num2 == 0)
		{
			finalText = string.Empty;
			return false;
		}
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		NGUIText.Prepare(text);
		StringBuilder stringBuilder = new StringBuilder();
		int length = text.Length;
		float num3 = (float)NGUIText.regionWidth;
		int num4 = 0;
		int i = 0;
		int num5 = 1;
		int prev = 0;
		bool flag = true;
		bool flag2 = true;
		bool flag3 = false;
		while (i < length)
		{
			char c = text[i];
			if (c > '⿿')
			{
				flag3 = true;
			}
			if (c == '\n')
			{
				if (num5 == num2)
				{
					break;
				}
				num3 = (float)NGUIText.regionWidth;
				if (num4 < i)
				{
					stringBuilder.Append(text.Substring(num4, i - num4 + 1));
				}
				else
				{
					stringBuilder.Append(c);
				}
				flag = true;
				num5++;
				num4 = i + 1;
				prev = 0;
			}
			else if (NGUIText.encoding && NGUIText.ParseSymbol(text, ref i))
			{
				i--;
			}
			else
			{
				BMSymbol bmsymbol = (!NGUIText.useSymbols) ? null : NGUIText.GetSymbol(text, i, length);
				float num6;
				if (bmsymbol == null)
				{
					float glyphWidth = NGUIText.GetGlyphWidth((int)c, prev);
					if (glyphWidth == 0f)
					{
						goto IL_38A;
					}
					num6 = NGUIText.finalSpacingX + glyphWidth;
				}
				else
				{
					num6 = NGUIText.finalSpacingX + (float)bmsymbol.advance * NGUIText.fontScale;
				}
				num3 -= num6;
				if (NGUIText.IsSpace((int)c) && !flag3 && num4 < i)
				{
					int num7 = i - num4 + 1;
					if (num5 == num2 && num3 <= 0f && i < length)
					{
						char c2 = text[i];
						if (c2 < ' ' || NGUIText.IsSpace((int)c2))
						{
							num7--;
						}
					}
					stringBuilder.Append(text.Substring(num4, num7));
					flag = false;
					num4 = i + 1;
				}
				if (Mathf.RoundToInt(num3) < 0)
				{
					if (flag || num5 == num2)
					{
						stringBuilder.Append(text.Substring(num4, Mathf.Max(0, i - num4)));
						bool flag4 = NGUIText.IsSpace((int)c);
						if (!flag4 && !flag3)
						{
							flag2 = false;
						}
						if (num5++ == num2)
						{
							num4 = i;
							break;
						}
						if (keepCharCount)
						{
							NGUIText.ReplaceSpaceWithNewline(ref stringBuilder);
						}
						else
						{
							NGUIText.EndLine(ref stringBuilder);
						}
						flag = true;
						if (flag4)
						{
							num4 = i + 1;
							num3 = (float)NGUIText.regionWidth;
						}
						else
						{
							num4 = i;
							num3 = (float)NGUIText.regionWidth - num6;
						}
						prev = 0;
					}
					else
					{
						flag = true;
						num3 = (float)NGUIText.regionWidth;
						i = num4 - 1;
						prev = 0;
						if (num5++ == num2)
						{
							break;
						}
						if (keepCharCount)
						{
							NGUIText.ReplaceSpaceWithNewline(ref stringBuilder);
						}
						else
						{
							NGUIText.EndLine(ref stringBuilder);
						}
						goto IL_38A;
					}
				}
				else
				{
					prev = (int)c;
				}
				if (bmsymbol != null)
				{
					i += bmsymbol.length - 1;
					prev = 0;
				}
			}
			IL_38A:
			i++;
		}
		if (num4 < i)
		{
			stringBuilder.Append(text.Substring(num4, i - num4));
		}
		finalText = stringBuilder.ToString();
		return flag2 && (i == length || num5 <= Mathf.Min(NGUIText.maxLines, num2));
	}

	// Token: 0x060032A8 RID: 12968 RVA: 0x000FCE5C File Offset: 0x000FB25C
	public static void Print(string text, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		int size = verts.size;
		NGUIText.Prepare(text);
		NGUIText.mColors.Add(Color.white);
		NGUIText.mAlpha = 1f;
		int prev = 0;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = (float)NGUIText.finalSize;
		Color a = NGUIText.tint * NGUIText.gradientBottom;
		Color b = NGUIText.tint * NGUIText.gradientTop;
		Color32 color = NGUIText.tint;
		int length = text.Length;
		Rect rect = default(Rect);
		float num5 = 0f;
		float num6 = 0f;
		float num7 = num4 * NGUIText.pixelDensity;
		bool flag = false;
		int num8 = 0;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		if (NGUIText.bitmapFont != null)
		{
			rect = NGUIText.bitmapFont.uvRect;
			num5 = rect.width / (float)NGUIText.bitmapFont.texWidth;
			num6 = rect.height / (float)NGUIText.bitmapFont.texHeight;
		}
		for (int i = 0; i < length; i++)
		{
			int num9 = (int)text[i];
			float num10 = num;
			if (num9 == 10)
			{
				if (num > num3)
				{
					num3 = num;
				}
				if (NGUIText.alignment != NGUIText.Alignment.Left)
				{
					NGUIText.Align(verts, size, num - NGUIText.finalSpacingX);
					size = verts.size;
				}
				num = 0f;
				num2 += NGUIText.finalLineHeight;
				prev = 0;
			}
			else if (num9 < 32)
			{
				prev = num9;
			}
			else if (NGUIText.encoding && NGUIText.ParseSymbol(text, ref i, NGUIText.mColors, NGUIText.premultiply, ref num8, ref flag2, ref flag3, ref flag4, ref flag5, ref flag6))
			{
				Color color2;
				if (flag6)
				{
					color2 = NGUIText.mColors[NGUIText.mColors.size - 1];
					color2.a *= NGUIText.mAlpha * NGUIText.tint.a;
				}
				else
				{
					color2 = NGUIText.tint * NGUIText.mColors[NGUIText.mColors.size - 1];
					color2.a *= NGUIText.mAlpha;
				}
				color = color2;
				int j = 0;
				int num11 = NGUIText.mColors.size - 2;
				while (j < num11)
				{
					color2.a *= NGUIText.mColors[j].a;
					j++;
				}
				if (NGUIText.gradient)
				{
					a = NGUIText.gradientBottom * color2;
					b = NGUIText.gradientTop * color2;
				}
				i--;
			}
			else
			{
				BMSymbol bmsymbol = (!NGUIText.useSymbols) ? null : NGUIText.GetSymbol(text, i, length);
				if (bmsymbol != null)
				{
					float num12 = num + (float)bmsymbol.offsetX * NGUIText.fontScale;
					float num13 = num12 + (float)bmsymbol.width * NGUIText.fontScale;
					float num14 = -(num2 + (float)bmsymbol.offsetY * NGUIText.fontScale);
					float num15 = num14 - (float)bmsymbol.height * NGUIText.fontScale;
					if (Mathf.RoundToInt(num + (float)bmsymbol.advance * NGUIText.fontScale) > NGUIText.regionWidth)
					{
						if (num == 0f)
						{
							return;
						}
						if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
						{
							NGUIText.Align(verts, size, num - NGUIText.finalSpacingX);
							size = verts.size;
						}
						num12 -= num;
						num13 -= num;
						num15 -= NGUIText.finalLineHeight;
						num14 -= NGUIText.finalLineHeight;
						num = 0f;
						num2 += NGUIText.finalLineHeight;
					}
					verts.Add(new Vector3(num12, num15));
					verts.Add(new Vector3(num12, num14));
					verts.Add(new Vector3(num13, num14));
					verts.Add(new Vector3(num13, num15));
					num += NGUIText.finalSpacingX + (float)bmsymbol.advance * NGUIText.fontScale;
					i += bmsymbol.length - 1;
					prev = 0;
					if (uvs != null)
					{
						Rect uvRect = bmsymbol.uvRect;
						float xMin = uvRect.xMin;
						float yMin = uvRect.yMin;
						float xMax = uvRect.xMax;
						float yMax = uvRect.yMax;
						uvs.Add(new Vector2(xMin, yMin));
						uvs.Add(new Vector2(xMin, yMax));
						uvs.Add(new Vector2(xMax, yMax));
						uvs.Add(new Vector2(xMax, yMin));
					}
					if (cols != null)
					{
						if (NGUIText.symbolStyle == NGUIText.SymbolStyle.Colored)
						{
							for (int k = 0; k < 4; k++)
							{
								cols.Add(color);
							}
						}
						else
						{
							Color32 item = Color.white;
							item.a = color.a;
							for (int l = 0; l < 4; l++)
							{
								cols.Add(item);
							}
						}
					}
				}
				else
				{
					NGUIText.GlyphInfo glyphInfo = NGUIText.GetGlyph(num9, prev);
					if (glyphInfo != null)
					{
						prev = num9;
						if (num8 != 0)
						{
							NGUIText.GlyphInfo glyphInfo2 = glyphInfo;
							glyphInfo2.v0.x = glyphInfo2.v0.x * 0.75f;
							NGUIText.GlyphInfo glyphInfo3 = glyphInfo;
							glyphInfo3.v0.y = glyphInfo3.v0.y * 0.75f;
							NGUIText.GlyphInfo glyphInfo4 = glyphInfo;
							glyphInfo4.v1.x = glyphInfo4.v1.x * 0.75f;
							NGUIText.GlyphInfo glyphInfo5 = glyphInfo;
							glyphInfo5.v1.y = glyphInfo5.v1.y * 0.75f;
							if (num8 == 1)
							{
								NGUIText.GlyphInfo glyphInfo6 = glyphInfo;
								glyphInfo6.v0.y = glyphInfo6.v0.y - NGUIText.fontScale * (float)NGUIText.fontSize * 0.4f;
								NGUIText.GlyphInfo glyphInfo7 = glyphInfo;
								glyphInfo7.v1.y = glyphInfo7.v1.y - NGUIText.fontScale * (float)NGUIText.fontSize * 0.4f;
							}
							else
							{
								NGUIText.GlyphInfo glyphInfo8 = glyphInfo;
								glyphInfo8.v0.y = glyphInfo8.v0.y + NGUIText.fontScale * (float)NGUIText.fontSize * 0.05f;
								NGUIText.GlyphInfo glyphInfo9 = glyphInfo;
								glyphInfo9.v1.y = glyphInfo9.v1.y + NGUIText.fontScale * (float)NGUIText.fontSize * 0.05f;
							}
						}
						float num12 = glyphInfo.v0.x + num;
						float num15 = glyphInfo.v0.y - num2;
						float num13 = glyphInfo.v1.x + num;
						float num14 = glyphInfo.v1.y - num2;
						float num16 = glyphInfo.advance;
						if (NGUIText.finalSpacingX < 0f)
						{
							num16 += NGUIText.finalSpacingX;
						}
						if (Mathf.RoundToInt(num + num16) > NGUIText.regionWidth)
						{
							if (num == 0f)
							{
								return;
							}
							if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
							{
								NGUIText.Align(verts, size, num - NGUIText.finalSpacingX);
								size = verts.size;
							}
							num12 -= num;
							num13 -= num;
							num15 -= NGUIText.finalLineHeight;
							num14 -= NGUIText.finalLineHeight;
							num = 0f;
							num2 += NGUIText.finalLineHeight;
							num10 = 0f;
						}
						if (NGUIText.IsSpace(num9))
						{
							if (flag4)
							{
								num9 = 95;
							}
							else if (flag5)
							{
								num9 = 45;
							}
						}
						num += ((num8 != 0) ? ((NGUIText.finalSpacingX + glyphInfo.advance) * 0.75f) : (NGUIText.finalSpacingX + glyphInfo.advance));
						if (!NGUIText.IsSpace(num9))
						{
							if (uvs != null)
							{
								if (NGUIText.bitmapFont != null)
								{
									glyphInfo.u0.x = rect.xMin + num5 * glyphInfo.u0.x;
									glyphInfo.u1.x = rect.xMin + num5 * glyphInfo.u1.x;
									glyphInfo.u0.y = rect.yMax - num6 * glyphInfo.u0.y;
									glyphInfo.u1.y = rect.yMax - num6 * glyphInfo.u1.y;
								}
								int m = 0;
								int num17 = (!flag2) ? 1 : 4;
								while (m < num17)
								{
									if (glyphInfo.rotatedUVs)
									{
										uvs.Add(glyphInfo.u0);
										uvs.Add(new Vector2(glyphInfo.u1.x, glyphInfo.u0.y));
										uvs.Add(glyphInfo.u1);
										uvs.Add(new Vector2(glyphInfo.u0.x, glyphInfo.u1.y));
									}
									else
									{
										uvs.Add(glyphInfo.u0);
										uvs.Add(new Vector2(glyphInfo.u0.x, glyphInfo.u1.y));
										uvs.Add(glyphInfo.u1);
										uvs.Add(new Vector2(glyphInfo.u1.x, glyphInfo.u0.y));
									}
									m++;
								}
							}
							if (cols != null)
							{
								if (glyphInfo.channel == 0 || glyphInfo.channel == 15)
								{
									if (NGUIText.gradient)
									{
										float num18 = num7 + glyphInfo.v0.y / NGUIText.fontScale;
										float num19 = num7 + glyphInfo.v1.y / NGUIText.fontScale;
										num18 /= num7;
										num19 /= num7;
										NGUIText.s_c0 = Color.Lerp(a, b, num18);
										NGUIText.s_c1 = Color.Lerp(a, b, num19);
										int n = 0;
										int num20 = (!flag2) ? 1 : 4;
										while (n < num20)
										{
											cols.Add(NGUIText.s_c0);
											cols.Add(NGUIText.s_c1);
											cols.Add(NGUIText.s_c1);
											cols.Add(NGUIText.s_c0);
											n++;
										}
									}
									else
									{
										int num21 = 0;
										int num22 = (!flag2) ? 4 : 16;
										while (num21 < num22)
										{
											cols.Add(color);
											num21++;
										}
									}
								}
								else
								{
									Color color3 = color;
									color3 *= 0.49f;
									switch (glyphInfo.channel)
									{
									case 1:
										color3.b += 0.51f;
										break;
									case 2:
										color3.g += 0.51f;
										break;
									case 4:
										color3.r += 0.51f;
										break;
									case 8:
										color3.a += 0.51f;
										break;
									}
									Color32 item2 = color3;
									int num23 = 0;
									int num24 = (!flag2) ? 4 : 16;
									while (num23 < num24)
									{
										cols.Add(item2);
										num23++;
									}
								}
							}
							if (!flag2)
							{
								if (!flag3)
								{
									verts.Add(new Vector3(num12, num15));
									verts.Add(new Vector3(num12, num14));
									verts.Add(new Vector3(num13, num14));
									verts.Add(new Vector3(num13, num15));
								}
								else
								{
									float num25 = (float)NGUIText.fontSize * 0.1f * ((num14 - num15) / (float)NGUIText.fontSize);
									verts.Add(new Vector3(num12 - num25, num15));
									verts.Add(new Vector3(num12 + num25, num14));
									verts.Add(new Vector3(num13 + num25, num14));
									verts.Add(new Vector3(num13 - num25, num15));
								}
							}
							else
							{
								for (int num26 = 0; num26 < 4; num26++)
								{
									float num27 = NGUIText.mBoldOffset[num26 * 2];
									float num28 = NGUIText.mBoldOffset[num26 * 2 + 1];
									float num29 = (!flag3) ? 0f : ((float)NGUIText.fontSize * 0.1f * ((num14 - num15) / (float)NGUIText.fontSize));
									verts.Add(new Vector3(num12 + num27 - num29, num15 + num28));
									verts.Add(new Vector3(num12 + num27 + num29, num14 + num28));
									verts.Add(new Vector3(num13 + num27 + num29, num14 + num28));
									verts.Add(new Vector3(num13 + num27 - num29, num15 + num28));
								}
							}
							if (flag4 || flag5)
							{
								NGUIText.GlyphInfo glyphInfo10 = NGUIText.GetGlyph((!flag5) ? 95 : 45, prev);
								if (glyphInfo10 != null)
								{
									if (uvs != null)
									{
										if (NGUIText.bitmapFont != null)
										{
											glyphInfo10.u0.x = rect.xMin + num5 * glyphInfo10.u0.x;
											glyphInfo10.u1.x = rect.xMin + num5 * glyphInfo10.u1.x;
											glyphInfo10.u0.y = rect.yMax - num6 * glyphInfo10.u0.y;
											glyphInfo10.u1.y = rect.yMax - num6 * glyphInfo10.u1.y;
										}
										float x = (glyphInfo10.u0.x + glyphInfo10.u1.x) * 0.5f;
										int num30 = 0;
										int num31 = (!flag2) ? 1 : 4;
										while (num30 < num31)
										{
											uvs.Add(new Vector2(x, glyphInfo10.u0.y));
											uvs.Add(new Vector2(x, glyphInfo10.u1.y));
											uvs.Add(new Vector2(x, glyphInfo10.u1.y));
											uvs.Add(new Vector2(x, glyphInfo10.u0.y));
											num30++;
										}
									}
									if (flag && flag5)
									{
										num15 = (-num2 + glyphInfo10.v0.y) * 0.75f;
										num14 = (-num2 + glyphInfo10.v1.y) * 0.75f;
									}
									else
									{
										num15 = -num2 + glyphInfo10.v0.y;
										num14 = -num2 + glyphInfo10.v1.y;
									}
									if (flag2)
									{
										for (int num32 = 0; num32 < 4; num32++)
										{
											float num33 = NGUIText.mBoldOffset[num32 * 2];
											float num34 = NGUIText.mBoldOffset[num32 * 2 + 1];
											verts.Add(new Vector3(num10 + num33, num15 + num34));
											verts.Add(new Vector3(num10 + num33, num14 + num34));
											verts.Add(new Vector3(num + num33, num14 + num34));
											verts.Add(new Vector3(num + num33, num15 + num34));
										}
									}
									else
									{
										verts.Add(new Vector3(num10, num15));
										verts.Add(new Vector3(num10, num14));
										verts.Add(new Vector3(num, num14));
										verts.Add(new Vector3(num, num15));
									}
									if (NGUIText.gradient)
									{
										float num35 = num7 + glyphInfo10.v0.y / NGUIText.fontScale;
										float num36 = num7 + glyphInfo10.v1.y / NGUIText.fontScale;
										num35 /= num7;
										num36 /= num7;
										NGUIText.s_c0 = Color.Lerp(a, b, num35);
										NGUIText.s_c1 = Color.Lerp(a, b, num36);
										int num37 = 0;
										int num38 = (!flag2) ? 1 : 4;
										while (num37 < num38)
										{
											cols.Add(NGUIText.s_c0);
											cols.Add(NGUIText.s_c1);
											cols.Add(NGUIText.s_c1);
											cols.Add(NGUIText.s_c0);
											num37++;
										}
									}
									else
									{
										int num39 = 0;
										int num40 = (!flag2) ? 4 : 16;
										while (num39 < num40)
										{
											cols.Add(color);
											num39++;
										}
									}
								}
							}
						}
					}
				}
			}
		}
		if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
		{
			NGUIText.Align(verts, size, num - NGUIText.finalSpacingX);
			size = verts.size;
		}
		NGUIText.mColors.Clear();
	}

	// Token: 0x060032A9 RID: 12969 RVA: 0x000FDEAC File Offset: 0x000FC2AC
	public static void PrintApproximateCharacterPositions(string text, BetterList<Vector3> verts, BetterList<int> indices)
	{
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		NGUIText.Prepare(text);
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = (float)NGUIText.fontSize * NGUIText.fontScale * 0.5f;
		int length = text.Length;
		int size = verts.size;
		int prev = 0;
		for (int i = 0; i < length; i++)
		{
			int num5 = (int)text[i];
			verts.Add(new Vector3(num, -num2 - num4));
			indices.Add(i);
			if (num5 == 10)
			{
				if (num > num3)
				{
					num3 = num;
				}
				if (NGUIText.alignment != NGUIText.Alignment.Left)
				{
					NGUIText.Align(verts, size, num - NGUIText.finalSpacingX);
					size = verts.size;
				}
				num = 0f;
				num2 += NGUIText.finalLineHeight;
				prev = 0;
			}
			else if (num5 < 32)
			{
				prev = 0;
			}
			else if (NGUIText.encoding && NGUIText.ParseSymbol(text, ref i))
			{
				i--;
			}
			else
			{
				BMSymbol bmsymbol = (!NGUIText.useSymbols) ? null : NGUIText.GetSymbol(text, i, length);
				if (bmsymbol == null)
				{
					float num6 = NGUIText.GetGlyphWidth(num5, prev);
					if (num6 != 0f)
					{
						num6 += NGUIText.finalSpacingX;
						if (Mathf.RoundToInt(num + num6) > NGUIText.regionWidth)
						{
							if (num == 0f)
							{
								return;
							}
							if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
							{
								NGUIText.Align(verts, size, num - NGUIText.finalSpacingX);
								size = verts.size;
							}
							num = num6;
							num2 += NGUIText.finalLineHeight;
						}
						else
						{
							num += num6;
						}
						verts.Add(new Vector3(num, -num2 - num4));
						indices.Add(i + 1);
						prev = num5;
					}
				}
				else
				{
					float num7 = (float)bmsymbol.advance * NGUIText.fontScale + NGUIText.finalSpacingX;
					if (Mathf.RoundToInt(num + num7) > NGUIText.regionWidth)
					{
						if (num == 0f)
						{
							return;
						}
						if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
						{
							NGUIText.Align(verts, size, num - NGUIText.finalSpacingX);
							size = verts.size;
						}
						num = num7;
						num2 += NGUIText.finalLineHeight;
					}
					else
					{
						num += num7;
					}
					verts.Add(new Vector3(num, -num2 - num4));
					indices.Add(i + 1);
					i += bmsymbol.sequence.Length - 1;
					prev = 0;
				}
			}
		}
		if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
		{
			NGUIText.Align(verts, size, num - NGUIText.finalSpacingX);
		}
	}

	// Token: 0x060032AA RID: 12970 RVA: 0x000FE15C File Offset: 0x000FC55C
	public static void PrintExactCharacterPositions(string text, BetterList<Vector3> verts, BetterList<int> indices)
	{
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		NGUIText.Prepare(text);
		float num = (float)NGUIText.fontSize * NGUIText.fontScale;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		int length = text.Length;
		int size = verts.size;
		int prev = 0;
		for (int i = 0; i < length; i++)
		{
			int num5 = (int)text[i];
			if (num5 == 10)
			{
				if (num2 > num4)
				{
					num4 = num2;
				}
				if (NGUIText.alignment != NGUIText.Alignment.Left)
				{
					NGUIText.Align(verts, size, num2 - NGUIText.finalSpacingX);
					size = verts.size;
				}
				num2 = 0f;
				num3 += NGUIText.finalLineHeight;
				prev = 0;
			}
			else if (num5 < 32)
			{
				prev = 0;
			}
			else if (NGUIText.encoding && NGUIText.ParseSymbol(text, ref i))
			{
				i--;
			}
			else
			{
				BMSymbol bmsymbol = (!NGUIText.useSymbols) ? null : NGUIText.GetSymbol(text, i, length);
				if (bmsymbol == null)
				{
					float glyphWidth = NGUIText.GetGlyphWidth(num5, prev);
					if (glyphWidth != 0f)
					{
						float num6 = glyphWidth + NGUIText.finalSpacingX;
						if (Mathf.RoundToInt(num2 + num6) > NGUIText.regionWidth)
						{
							if (num2 == 0f)
							{
								return;
							}
							if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
							{
								NGUIText.Align(verts, size, num2 - NGUIText.finalSpacingX);
								size = verts.size;
							}
							num2 = 0f;
							num3 += NGUIText.finalLineHeight;
							prev = 0;
							i--;
						}
						else
						{
							indices.Add(i);
							verts.Add(new Vector3(num2, -num3 - num));
							verts.Add(new Vector3(num2 + num6, -num3));
							prev = num5;
							num2 += num6;
						}
					}
				}
				else
				{
					float num7 = (float)bmsymbol.advance * NGUIText.fontScale + NGUIText.finalSpacingX;
					if (Mathf.RoundToInt(num2 + num7) > NGUIText.regionWidth)
					{
						if (num2 == 0f)
						{
							return;
						}
						if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
						{
							NGUIText.Align(verts, size, num2 - NGUIText.finalSpacingX);
							size = verts.size;
						}
						num2 = 0f;
						num3 += NGUIText.finalLineHeight;
						prev = 0;
						i--;
					}
					else
					{
						indices.Add(i);
						verts.Add(new Vector3(num2, -num3 - num));
						verts.Add(new Vector3(num2 + num7, -num3));
						i += bmsymbol.sequence.Length - 1;
						num2 += num7;
						prev = 0;
					}
				}
			}
		}
		if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
		{
			NGUIText.Align(verts, size, num2 - NGUIText.finalSpacingX);
		}
	}

	// Token: 0x060032AB RID: 12971 RVA: 0x000FE424 File Offset: 0x000FC824
	public static void PrintCaretAndSelection(string text, int start, int end, BetterList<Vector3> caret, BetterList<Vector3> highlight)
	{
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		NGUIText.Prepare(text);
		int num = end;
		if (start > end)
		{
			end = start;
			start = num;
		}
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = (float)NGUIText.fontSize * NGUIText.fontScale;
		int indexOffset = (caret == null) ? 0 : caret.size;
		int num6 = (highlight == null) ? 0 : highlight.size;
		int length = text.Length;
		int i = 0;
		int prev = 0;
		bool flag = false;
		bool flag2 = false;
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		while (i < length)
		{
			if (caret != null && !flag2 && num <= i)
			{
				flag2 = true;
				caret.Add(new Vector3(num2 - 1f, -num3 - num5));
				caret.Add(new Vector3(num2 - 1f, -num3));
				caret.Add(new Vector3(num2 + 1f, -num3));
				caret.Add(new Vector3(num2 + 1f, -num3 - num5));
			}
			int num7 = (int)text[i];
			if (num7 == 10)
			{
				if (num2 > num4)
				{
					num4 = num2;
				}
				if (caret != null && flag2)
				{
					if (NGUIText.alignment != NGUIText.Alignment.Left)
					{
						NGUIText.Align(caret, indexOffset, num2 - NGUIText.finalSpacingX);
					}
					caret = null;
				}
				if (highlight != null)
				{
					if (flag)
					{
						flag = false;
						highlight.Add(zero2);
						highlight.Add(zero);
					}
					else if (start <= i && end > i)
					{
						highlight.Add(new Vector3(num2, -num3 - num5));
						highlight.Add(new Vector3(num2, -num3));
						highlight.Add(new Vector3(num2 + 2f, -num3));
						highlight.Add(new Vector3(num2 + 2f, -num3 - num5));
					}
					if (NGUIText.alignment != NGUIText.Alignment.Left && num6 < highlight.size)
					{
						NGUIText.Align(highlight, num6, num2 - NGUIText.finalSpacingX);
						num6 = highlight.size;
					}
				}
				num2 = 0f;
				num3 += NGUIText.finalLineHeight;
				prev = 0;
			}
			else if (num7 < 32)
			{
				prev = 0;
			}
			else if (NGUIText.encoding && NGUIText.ParseSymbol(text, ref i))
			{
				i--;
			}
			else
			{
				BMSymbol bmsymbol = (!NGUIText.useSymbols) ? null : NGUIText.GetSymbol(text, i, length);
				float num8 = (bmsymbol == null) ? NGUIText.GetGlyphWidth(num7, prev) : ((float)bmsymbol.advance * NGUIText.fontScale);
				if (num8 != 0f)
				{
					float num9 = num2;
					float num10 = num2 + num8;
					float num11 = -num3 - num5;
					float num12 = -num3;
					if (Mathf.RoundToInt(num10 + NGUIText.finalSpacingX) > NGUIText.regionWidth)
					{
						if (num2 == 0f)
						{
							return;
						}
						if (num2 > num4)
						{
							num4 = num2;
						}
						if (caret != null && flag2)
						{
							if (NGUIText.alignment != NGUIText.Alignment.Left)
							{
								NGUIText.Align(caret, indexOffset, num2 - NGUIText.finalSpacingX);
							}
							caret = null;
						}
						if (highlight != null)
						{
							if (flag)
							{
								flag = false;
								highlight.Add(zero2);
								highlight.Add(zero);
							}
							else if (start <= i && end > i)
							{
								highlight.Add(new Vector3(num2, -num3 - num5));
								highlight.Add(new Vector3(num2, -num3));
								highlight.Add(new Vector3(num2 + 2f, -num3));
								highlight.Add(new Vector3(num2 + 2f, -num3 - num5));
							}
							if (NGUIText.alignment != NGUIText.Alignment.Left && num6 < highlight.size)
							{
								NGUIText.Align(highlight, num6, num2 - NGUIText.finalSpacingX);
								num6 = highlight.size;
							}
						}
						num9 -= num2;
						num10 -= num2;
						num11 -= NGUIText.finalLineHeight;
						num12 -= NGUIText.finalLineHeight;
						num2 = 0f;
						num3 += NGUIText.finalLineHeight;
					}
					num2 += num8 + NGUIText.finalSpacingX;
					if (highlight != null)
					{
						if (start > i || end <= i)
						{
							if (flag)
							{
								flag = false;
								highlight.Add(zero2);
								highlight.Add(zero);
							}
						}
						else if (!flag)
						{
							flag = true;
							highlight.Add(new Vector3(num9, num11));
							highlight.Add(new Vector3(num9, num12));
						}
					}
					zero = new Vector2(num10, num11);
					zero2 = new Vector2(num10, num12);
					prev = num7;
				}
			}
			i++;
		}
		if (caret != null)
		{
			if (!flag2)
			{
				caret.Add(new Vector3(num2 - 1f, -num3 - num5));
				caret.Add(new Vector3(num2 - 1f, -num3));
				caret.Add(new Vector3(num2 + 1f, -num3));
				caret.Add(new Vector3(num2 + 1f, -num3 - num5));
			}
			if (NGUIText.alignment != NGUIText.Alignment.Left)
			{
				NGUIText.Align(caret, indexOffset, num2 - NGUIText.finalSpacingX);
			}
		}
		if (highlight != null)
		{
			if (flag)
			{
				highlight.Add(zero2);
				highlight.Add(zero);
			}
			else if (start < i && end == i)
			{
				highlight.Add(new Vector3(num2, -num3 - num5));
				highlight.Add(new Vector3(num2, -num3));
				highlight.Add(new Vector3(num2 + 2f, -num3));
				highlight.Add(new Vector3(num2 + 2f, -num3 - num5));
			}
			if (NGUIText.alignment != NGUIText.Alignment.Left && num6 < highlight.size)
			{
				NGUIText.Align(highlight, num6, num2 - NGUIText.finalSpacingX);
			}
		}
	}

	// Token: 0x04001CB3 RID: 7347
	public static UIFont bitmapFont;

	// Token: 0x04001CB4 RID: 7348
	public static Font dynamicFont;

	// Token: 0x04001CB5 RID: 7349
	public static NGUIText.GlyphInfo glyph = new NGUIText.GlyphInfo();

	// Token: 0x04001CB6 RID: 7350
	public static int fontSize = 16;

	// Token: 0x04001CB7 RID: 7351
	public static float fontScale = 1f;

	// Token: 0x04001CB8 RID: 7352
	public static float pixelDensity = 1f;

	// Token: 0x04001CB9 RID: 7353
	public static FontStyle fontStyle = FontStyle.Normal;

	// Token: 0x04001CBA RID: 7354
	public static NGUIText.Alignment alignment = NGUIText.Alignment.Left;

	// Token: 0x04001CBB RID: 7355
	public static Color tint = Color.white;

	// Token: 0x04001CBC RID: 7356
	public static int rectWidth = 1000000;

	// Token: 0x04001CBD RID: 7357
	public static int rectHeight = 1000000;

	// Token: 0x04001CBE RID: 7358
	public static int regionWidth = 1000000;

	// Token: 0x04001CBF RID: 7359
	public static int regionHeight = 1000000;

	// Token: 0x04001CC0 RID: 7360
	public static int maxLines = 0;

	// Token: 0x04001CC1 RID: 7361
	public static bool gradient = false;

	// Token: 0x04001CC2 RID: 7362
	public static Color gradientBottom = Color.white;

	// Token: 0x04001CC3 RID: 7363
	public static Color gradientTop = Color.white;

	// Token: 0x04001CC4 RID: 7364
	public static bool encoding = false;

	// Token: 0x04001CC5 RID: 7365
	public static float spacingX = 0f;

	// Token: 0x04001CC6 RID: 7366
	public static float spacingY = 0f;

	// Token: 0x04001CC7 RID: 7367
	public static bool premultiply = false;

	// Token: 0x04001CC8 RID: 7368
	public static NGUIText.SymbolStyle symbolStyle;

	// Token: 0x04001CC9 RID: 7369
	public static int finalSize = 0;

	// Token: 0x04001CCA RID: 7370
	public static float finalSpacingX = 0f;

	// Token: 0x04001CCB RID: 7371
	public static float finalLineHeight = 0f;

	// Token: 0x04001CCC RID: 7372
	public static float baseline = 0f;

	// Token: 0x04001CCD RID: 7373
	public static bool useSymbols = false;

	// Token: 0x04001CCE RID: 7374
	private static Color mInvisible = new Color(0f, 0f, 0f, 0f);

	// Token: 0x04001CCF RID: 7375
	private static BetterList<Color> mColors = new BetterList<Color>();

	// Token: 0x04001CD0 RID: 7376
	private static float mAlpha = 1f;

	// Token: 0x04001CD1 RID: 7377
	private static CharacterInfo mTempChar;

	// Token: 0x04001CD2 RID: 7378
	private static BetterList<float> mSizes = new BetterList<float>();

	// Token: 0x04001CD3 RID: 7379
	private static Color32 s_c0;

	// Token: 0x04001CD4 RID: 7380
	private static Color32 s_c1;

	// Token: 0x04001CD5 RID: 7381
	private static float[] mBoldOffset = new float[]
	{
		-0.25f,
		0f,
		0.25f,
		0f,
		0f,
		-0.25f,
		0f,
		0.25f
	};

	// Token: 0x020005F1 RID: 1521
	public enum Alignment
	{
		// Token: 0x04001CD7 RID: 7383
		Automatic,
		// Token: 0x04001CD8 RID: 7384
		Left,
		// Token: 0x04001CD9 RID: 7385
		Center,
		// Token: 0x04001CDA RID: 7386
		Right,
		// Token: 0x04001CDB RID: 7387
		Justified
	}

	// Token: 0x020005F2 RID: 1522
	public enum SymbolStyle
	{
		// Token: 0x04001CDD RID: 7389
		None,
		// Token: 0x04001CDE RID: 7390
		Normal,
		// Token: 0x04001CDF RID: 7391
		Colored
	}

	// Token: 0x020005F3 RID: 1523
	public class GlyphInfo
	{
		// Token: 0x04001CE0 RID: 7392
		public Vector2 v0;

		// Token: 0x04001CE1 RID: 7393
		public Vector2 v1;

		// Token: 0x04001CE2 RID: 7394
		public Vector2 u0;

		// Token: 0x04001CE3 RID: 7395
		public Vector2 u1;

		// Token: 0x04001CE4 RID: 7396
		public float advance;

		// Token: 0x04001CE5 RID: 7397
		public int channel;

		// Token: 0x04001CE6 RID: 7398
		public bool rotatedUVs;
	}
}
