using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace LitJson
{
	// Token: 0x02000407 RID: 1031
	internal class Lexer
	{
		// Token: 0x0600255F RID: 9567 RVA: 0x000B7CDD File Offset: 0x000B60DD
		static Lexer()
		{
			Lexer.PopulateFsmTables();
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x000B7CE4 File Offset: 0x000B60E4
		public Lexer(TextReader reader)
		{
			this.allow_comments = true;
			this.allow_single_quoted_strings = true;
			this.input_buffer = 0;
			this.string_buffer = new StringBuilder(128);
			this.state = 1;
			this.end_of_input = false;
			this.reader = reader;
			this.fsm_context = new FsmContext();
			this.fsm_context.L = this;
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06002561 RID: 9569 RVA: 0x000B7D48 File Offset: 0x000B6148
		// (set) Token: 0x06002562 RID: 9570 RVA: 0x000B7D50 File Offset: 0x000B6150
		public bool AllowComments
		{
			get
			{
				return this.allow_comments;
			}
			set
			{
				this.allow_comments = value;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06002563 RID: 9571 RVA: 0x000B7D59 File Offset: 0x000B6159
		// (set) Token: 0x06002564 RID: 9572 RVA: 0x000B7D61 File Offset: 0x000B6161
		public bool AllowSingleQuotedStrings
		{
			get
			{
				return this.allow_single_quoted_strings;
			}
			set
			{
				this.allow_single_quoted_strings = value;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06002565 RID: 9573 RVA: 0x000B7D6A File Offset: 0x000B616A
		public bool EndOfInput
		{
			get
			{
				return this.end_of_input;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06002566 RID: 9574 RVA: 0x000B7D72 File Offset: 0x000B6172
		public int Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06002567 RID: 9575 RVA: 0x000B7D7A File Offset: 0x000B617A
		public string StringValue
		{
			get
			{
				return this.string_value;
			}
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x000B7D84 File Offset: 0x000B6184
		private static int HexValue(int digit)
		{
			switch (digit)
			{
			case 65:
				break;
			case 66:
				return 11;
			case 67:
				return 12;
			case 68:
				return 13;
			case 69:
				return 14;
			case 70:
				return 15;
			default:
				switch (digit)
				{
				case 97:
					break;
				case 98:
					return 11;
				case 99:
					return 12;
				case 100:
					return 13;
				case 101:
					return 14;
				case 102:
					return 15;
				default:
					return digit - 48;
				}
				break;
			}
			return 10;
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x000B7DF0 File Offset: 0x000B61F0
		private static void PopulateFsmTables()
		{
			Lexer.StateHandler[] array = new Lexer.StateHandler[28];
			int num = 0;
			if (Lexer.f__mg0 == null)
			{
				Lexer.f__mg0 = new Lexer.StateHandler(Lexer.State1);
			}
			array[num] = Lexer.f__mg0;
			int num2 = 1;
			if (Lexer.f__mg1 == null)
			{
				Lexer.f__mg1 = new Lexer.StateHandler(Lexer.State2);
			}
			array[num2] = Lexer.f__mg1;
			int num3 = 2;
			if (Lexer.f__mg2 == null)
			{
				Lexer.f__mg2 = new Lexer.StateHandler(Lexer.State3);
			}
			array[num3] = Lexer.f__mg2;
			int num4 = 3;
			if (Lexer.f__mg3 == null)
			{
				Lexer.f__mg3 = new Lexer.StateHandler(Lexer.State4);
			}
			array[num4] = Lexer.f__mg3;
			int num5 = 4;
			if (Lexer.f__mg4 == null)
			{
				Lexer.f__mg4 = new Lexer.StateHandler(Lexer.State5);
			}
			array[num5] = Lexer.f__mg4;
			int num6 = 5;
			if (Lexer.f__mg5 == null)
			{
				Lexer.f__mg5 = new Lexer.StateHandler(Lexer.State6);
			}
			array[num6] = Lexer.f__mg5;
			int num7 = 6;
			if (Lexer.f__mg6 == null)
			{
				Lexer.f__mg6 = new Lexer.StateHandler(Lexer.State7);
			}
			array[num7] = Lexer.f__mg6;
			int num8 = 7;
			if (Lexer.f__mg7 == null)
			{
				Lexer.f__mg7 = new Lexer.StateHandler(Lexer.State8);
			}
			array[num8] = Lexer.f__mg7;
			int num9 = 8;
			if (Lexer.f__mg8 == null)
			{
				Lexer.f__mg8 = new Lexer.StateHandler(Lexer.State9);
			}
			array[num9] = Lexer.f__mg8;
			int num10 = 9;
			if (Lexer.f__mg9 == null)
			{
				Lexer.f__mg9 = new Lexer.StateHandler(Lexer.State10);
			}
			array[num10] = Lexer.f__mg9;
			int num11 = 10;
			if (Lexer.f__mgA == null)
			{
				Lexer.f__mgA = new Lexer.StateHandler(Lexer.State11);
			}
			array[num11] = Lexer.f__mgA;
			int num12 = 11;
			if (Lexer.f__mgB == null)
			{
				Lexer.f__mgB = new Lexer.StateHandler(Lexer.State12);
			}
			array[num12] = Lexer.f__mgB;
			int num13 = 12;
			if (Lexer.f__mgC == null)
			{
				Lexer.f__mgC = new Lexer.StateHandler(Lexer.State13);
			}
			array[num13] = Lexer.f__mgC;
			int num14 = 13;
			if (Lexer.f__mgD == null)
			{
				Lexer.f__mgD = new Lexer.StateHandler(Lexer.State14);
			}
			array[num14] = Lexer.f__mgD;
			int num15 = 14;
			if (Lexer.f__mgE == null)
			{
				Lexer.f__mgE = new Lexer.StateHandler(Lexer.State15);
			}
			array[num15] = Lexer.f__mgE;
			int num16 = 15;
			if (Lexer.f__mgF == null)
			{
				Lexer.f__mgF = new Lexer.StateHandler(Lexer.State16);
			}
			array[num16] = Lexer.f__mgF;
			int num17 = 16;
			if (Lexer.f__mg10 == null)
			{
				Lexer.f__mg10 = new Lexer.StateHandler(Lexer.State17);
			}
			array[num17] = Lexer.f__mg10;
			int num18 = 17;
			if (Lexer.f__mg11 == null)
			{
				Lexer.f__mg11 = new Lexer.StateHandler(Lexer.State18);
			}
			array[num18] = Lexer.f__mg11;
			int num19 = 18;
			if (Lexer.f__mg12 == null)
			{
				Lexer.f__mg12 = new Lexer.StateHandler(Lexer.State19);
			}
			array[num19] = Lexer.f__mg12;
			int num20 = 19;
			if (Lexer.f__mg13 == null)
			{
				Lexer.f__mg13 = new Lexer.StateHandler(Lexer.State20);
			}
			array[num20] = Lexer.f__mg13;
			int num21 = 20;
			if (Lexer.f__mg14 == null)
			{
				Lexer.f__mg14 = new Lexer.StateHandler(Lexer.State21);
			}
			array[num21] = Lexer.f__mg14;
			int num22 = 21;
			if (Lexer.f__mg15 == null)
			{
				Lexer.f__mg15 = new Lexer.StateHandler(Lexer.State22);
			}
			array[num22] = Lexer.f__mg15;
			int num23 = 22;
			if (Lexer.f__mg16 == null)
			{
				Lexer.f__mg16 = new Lexer.StateHandler(Lexer.State23);
			}
			array[num23] = Lexer.f__mg16;
			int num24 = 23;
			if (Lexer.f__mg17 == null)
			{
				Lexer.f__mg17 = new Lexer.StateHandler(Lexer.State24);
			}
			array[num24] = Lexer.f__mg17;
			int num25 = 24;
			if (Lexer.f__mg18 == null)
			{
				Lexer.f__mg18 = new Lexer.StateHandler(Lexer.State25);
			}
			array[num25] = Lexer.f__mg18;
			int num26 = 25;
			if (Lexer.f__mg19 == null)
			{
				Lexer.f__mg19 = new Lexer.StateHandler(Lexer.State26);
			}
			array[num26] = Lexer.f__mg19;
			int num27 = 26;
			if (Lexer.f__mg1A == null)
			{
				Lexer.f__mg1A = new Lexer.StateHandler(Lexer.State27);
			}
			array[num27] = Lexer.f__mg1A;
			int num28 = 27;
			if (Lexer.f__mg1B == null)
			{
				Lexer.f__mg1B = new Lexer.StateHandler(Lexer.State28);
			}
			array[num28] = Lexer.f__mg1B;
			Lexer.fsm_handler_table = array;
			Lexer.fsm_return_table = new int[]
			{
				65542,
				0,
				65537,
				65537,
				0,
				65537,
				0,
				65537,
				0,
				0,
				65538,
				0,
				0,
				0,
				65539,
				0,
				0,
				65540,
				65541,
				65542,
				0,
				0,
				65541,
				65542,
				0,
				0,
				0,
				0
			};
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x000B81B4 File Offset: 0x000B65B4
		private static char ProcessEscChar(int esc_char)
		{
			switch (esc_char)
			{
			case 114:
				return '\r';
			default:
				if (esc_char == 34 || esc_char == 39 || esc_char == 47 || esc_char == 92)
				{
					return Convert.ToChar(esc_char);
				}
				if (esc_char == 98)
				{
					return '\b';
				}
				if (esc_char == 102)
				{
					return '\f';
				}
				if (esc_char != 110)
				{
					return '?';
				}
				return '\n';
			case 116:
				return '\t';
			}
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x000B822C File Offset: 0x000B662C
		private static bool State1(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char != 32 && (ctx.L.input_char < 9 || ctx.L.input_char > 13))
				{
					if (ctx.L.input_char >= 49 && ctx.L.input_char <= 57)
					{
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 3;
						return true;
					}
					int num = ctx.L.input_char;
					switch (num)
					{
					case 44:
						break;
					case 45:
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 2;
						return true;
					default:
						switch (num)
						{
						case 91:
						case 93:
							break;
						default:
							switch (num)
							{
							case 123:
							case 125:
								break;
							default:
								if (num == 34)
								{
									ctx.NextState = 19;
									ctx.Return = true;
									return true;
								}
								if (num != 39)
								{
									if (num != 58)
									{
										if (num == 102)
										{
											ctx.NextState = 12;
											return true;
										}
										if (num == 110)
										{
											ctx.NextState = 16;
											return true;
										}
										if (num != 116)
										{
											return false;
										}
										ctx.NextState = 9;
										return true;
									}
								}
								else
								{
									if (!ctx.L.allow_single_quoted_strings)
									{
										return false;
									}
									ctx.L.input_char = 34;
									ctx.NextState = 23;
									ctx.Return = true;
									return true;
								}
								break;
							}
							break;
						}
						break;
					case 47:
						if (!ctx.L.allow_comments)
						{
							return false;
						}
						ctx.NextState = 25;
						return true;
					case 48:
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 4;
						return true;
					}
					ctx.NextState = 1;
					ctx.Return = true;
					return true;
				}
			}
			return true;
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x000B8438 File Offset: 0x000B6838
		private static bool State2(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char >= 49 && ctx.L.input_char <= 57)
			{
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 3;
				return true;
			}
			int num = ctx.L.input_char;
			if (num != 48)
			{
				return false;
			}
			ctx.L.string_buffer.Append((char)ctx.L.input_char);
			ctx.NextState = 4;
			return true;
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x000B84DC File Offset: 0x000B68DC
		private static bool State3(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
				{
					ctx.L.string_buffer.Append((char)ctx.L.input_char);
				}
				else
				{
					if (ctx.L.input_char == 32 || (ctx.L.input_char >= 9 && ctx.L.input_char <= 13))
					{
						ctx.Return = true;
						ctx.NextState = 1;
						return true;
					}
					int num = ctx.L.input_char;
					switch (num)
					{
					case 44:
						break;
					default:
						if (num != 69)
						{
							if (num == 93)
							{
								break;
							}
							if (num != 101)
							{
								if (num != 125)
								{
									return false;
								}
								break;
							}
						}
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 7;
						return true;
					case 46:
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 5;
						return true;
					}
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 1;
					return true;
				}
			}
			return true;
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x000B863C File Offset: 0x000B6A3C
		private static bool State4(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char == 32 || (ctx.L.input_char >= 9 && ctx.L.input_char <= 13))
			{
				ctx.Return = true;
				ctx.NextState = 1;
				return true;
			}
			int num = ctx.L.input_char;
			switch (num)
			{
			case 44:
				break;
			default:
				if (num != 69)
				{
					if (num == 93)
					{
						break;
					}
					if (num != 101)
					{
						if (num != 125)
						{
							return false;
						}
						break;
					}
				}
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 7;
				return true;
			case 46:
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 5;
				return true;
			}
			ctx.L.UngetChar();
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x000B874C File Offset: 0x000B6B4C
		private static bool State5(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
			{
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 6;
				return true;
			}
			return false;
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x000B87B0 File Offset: 0x000B6BB0
		private static bool State6(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
				{
					ctx.L.string_buffer.Append((char)ctx.L.input_char);
				}
				else
				{
					if (ctx.L.input_char == 32 || (ctx.L.input_char >= 9 && ctx.L.input_char <= 13))
					{
						ctx.Return = true;
						ctx.NextState = 1;
						return true;
					}
					int num = ctx.L.input_char;
					if (num != 44)
					{
						if (num != 69)
						{
							if (num == 93)
							{
								goto IL_CA;
							}
							if (num != 101)
							{
								if (num != 125)
								{
									return false;
								}
								goto IL_CA;
							}
						}
						ctx.L.string_buffer.Append((char)ctx.L.input_char);
						ctx.NextState = 7;
						return true;
					}
					IL_CA:
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 1;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x000B88DC File Offset: 0x000B6CDC
		private static bool State7(FsmContext ctx)
		{
			ctx.L.GetChar();
			if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
			{
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
				ctx.NextState = 8;
				return true;
			}
			int num = ctx.L.input_char;
			if (num != 43 && num != 45)
			{
				return false;
			}
			ctx.L.string_buffer.Append((char)ctx.L.input_char);
			ctx.NextState = 8;
			return true;
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x000B8988 File Offset: 0x000B6D88
		private static bool State8(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char >= 48 && ctx.L.input_char <= 57)
				{
					ctx.L.string_buffer.Append((char)ctx.L.input_char);
				}
				else
				{
					if (ctx.L.input_char == 32 || (ctx.L.input_char >= 9 && ctx.L.input_char <= 13))
					{
						ctx.Return = true;
						ctx.NextState = 1;
						return true;
					}
					int num = ctx.L.input_char;
					if (num != 44 && num != 93 && num != 125)
					{
						return false;
					}
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 1;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x000B8A80 File Offset: 0x000B6E80
		private static bool State9(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 114)
			{
				return false;
			}
			ctx.NextState = 10;
			return true;
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x000B8AC0 File Offset: 0x000B6EC0
		private static bool State10(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 117)
			{
				return false;
			}
			ctx.NextState = 11;
			return true;
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x000B8B00 File Offset: 0x000B6F00
		private static bool State11(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 101)
			{
				return false;
			}
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x000B8B44 File Offset: 0x000B6F44
		private static bool State12(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 97)
			{
				return false;
			}
			ctx.NextState = 13;
			return true;
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x000B8B84 File Offset: 0x000B6F84
		private static bool State13(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 108)
			{
				return false;
			}
			ctx.NextState = 14;
			return true;
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000B8BC4 File Offset: 0x000B6FC4
		private static bool State14(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 115)
			{
				return false;
			}
			ctx.NextState = 15;
			return true;
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x000B8C04 File Offset: 0x000B7004
		private static bool State15(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 101)
			{
				return false;
			}
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x000B8C48 File Offset: 0x000B7048
		private static bool State16(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 117)
			{
				return false;
			}
			ctx.NextState = 17;
			return true;
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x000B8C88 File Offset: 0x000B7088
		private static bool State17(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 108)
			{
				return false;
			}
			ctx.NextState = 18;
			return true;
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x000B8CC8 File Offset: 0x000B70C8
		private static bool State18(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 108)
			{
				return false;
			}
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x000B8D0C File Offset: 0x000B710C
		private static bool State19(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				int num = ctx.L.input_char;
				if (num == 34)
				{
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 20;
					return true;
				}
				if (num == 92)
				{
					ctx.StateStack = 19;
					ctx.NextState = 21;
					return true;
				}
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
			}
			return true;
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x000B8DA0 File Offset: 0x000B71A0
		private static bool State20(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 34)
			{
				return false;
			}
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x000B8DE4 File Offset: 0x000B71E4
		private static bool State21(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			switch (num)
			{
			case 114:
			case 116:
				break;
			default:
				if (num != 34 && num != 39 && num != 47 && num != 92 && num != 98 && num != 102 && num != 110)
				{
					return false;
				}
				break;
			case 117:
				ctx.NextState = 22;
				return true;
			}
			ctx.L.string_buffer.Append(Lexer.ProcessEscChar(ctx.L.input_char));
			ctx.NextState = ctx.StateStack;
			return true;
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x000B8E9C File Offset: 0x000B729C
		private static bool State22(FsmContext ctx)
		{
			int num = 0;
			int num2 = 4096;
			ctx.L.unichar = 0;
			while (ctx.L.GetChar())
			{
				if ((ctx.L.input_char < 48 || ctx.L.input_char > 57) && (ctx.L.input_char < 65 || ctx.L.input_char > 70) && (ctx.L.input_char < 97 || ctx.L.input_char > 102))
				{
					return false;
				}
				ctx.L.unichar += Lexer.HexValue(ctx.L.input_char) * num2;
				num++;
				num2 /= 16;
				if (num == 4)
				{
					ctx.L.string_buffer.Append(Convert.ToChar(ctx.L.unichar));
					ctx.NextState = ctx.StateStack;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x000B8FAC File Offset: 0x000B73AC
		private static bool State23(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				int num = ctx.L.input_char;
				if (num == 39)
				{
					ctx.L.UngetChar();
					ctx.Return = true;
					ctx.NextState = 24;
					return true;
				}
				if (num == 92)
				{
					ctx.StateStack = 23;
					ctx.NextState = 21;
					return true;
				}
				ctx.L.string_buffer.Append((char)ctx.L.input_char);
			}
			return true;
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x000B9040 File Offset: 0x000B7440
		private static bool State24(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num != 39)
			{
				return false;
			}
			ctx.L.input_char = 34;
			ctx.Return = true;
			ctx.NextState = 1;
			return true;
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x000B9090 File Offset: 0x000B7490
		private static bool State25(FsmContext ctx)
		{
			ctx.L.GetChar();
			int num = ctx.L.input_char;
			if (num == 42)
			{
				ctx.NextState = 27;
				return true;
			}
			if (num != 47)
			{
				return false;
			}
			ctx.NextState = 26;
			return true;
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x000B90DF File Offset: 0x000B74DF
		private static bool State26(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char == 10)
				{
					ctx.NextState = 1;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x000B9112 File Offset: 0x000B7512
		private static bool State27(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char == 42)
				{
					ctx.NextState = 28;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x000B9148 File Offset: 0x000B7548
		private static bool State28(FsmContext ctx)
		{
			while (ctx.L.GetChar())
			{
				if (ctx.L.input_char != 42)
				{
					if (ctx.L.input_char == 47)
					{
						ctx.NextState = 1;
						return true;
					}
					ctx.NextState = 27;
					return true;
				}
			}
			return true;
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x000B91A8 File Offset: 0x000B75A8
		private bool GetChar()
		{
			if ((this.input_char = this.NextChar()) != -1)
			{
				return true;
			}
			this.end_of_input = true;
			return false;
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x000B91D4 File Offset: 0x000B75D4
		private int NextChar()
		{
			if (this.input_buffer != 0)
			{
				int result = this.input_buffer;
				this.input_buffer = 0;
				return result;
			}
			return this.reader.Read();
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x000B9208 File Offset: 0x000B7608
		public bool NextToken()
		{
			this.fsm_context.Return = false;
			for (;;)
			{
				Lexer.StateHandler stateHandler = Lexer.fsm_handler_table[this.state - 1];
				if (!stateHandler(this.fsm_context))
				{
					break;
				}
				if (this.end_of_input)
				{
					return false;
				}
				if (this.fsm_context.Return)
				{
					goto Block_3;
				}
				this.state = this.fsm_context.NextState;
			}
			throw new JsonException(this.input_char);
			Block_3:
			this.string_value = this.string_buffer.ToString();
			this.string_buffer.Remove(0, this.string_buffer.Length);
			this.token = Lexer.fsm_return_table[this.state - 1];
			if (this.token == 65542)
			{
				this.token = this.input_char;
			}
			this.state = this.fsm_context.NextState;
			return true;
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x000B92EB File Offset: 0x000B76EB
		private void UngetChar()
		{
			this.input_buffer = this.input_char;
		}

		// Token: 0x04001288 RID: 4744
		private static int[] fsm_return_table;

		// Token: 0x04001289 RID: 4745
		private static Lexer.StateHandler[] fsm_handler_table;

		// Token: 0x0400128A RID: 4746
		private bool allow_comments;

		// Token: 0x0400128B RID: 4747
		private bool allow_single_quoted_strings;

		// Token: 0x0400128C RID: 4748
		private bool end_of_input;

		// Token: 0x0400128D RID: 4749
		private FsmContext fsm_context;

		// Token: 0x0400128E RID: 4750
		private int input_buffer;

		// Token: 0x0400128F RID: 4751
		private int input_char;

		// Token: 0x04001290 RID: 4752
		private TextReader reader;

		// Token: 0x04001291 RID: 4753
		private int state;

		// Token: 0x04001292 RID: 4754
		private StringBuilder string_buffer;

		// Token: 0x04001293 RID: 4755
		private string string_value;

		// Token: 0x04001294 RID: 4756
		private int token;

		// Token: 0x04001295 RID: 4757
		private int unichar;

		// Token: 0x04001296 RID: 4758
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg0;

		// Token: 0x04001297 RID: 4759
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg1;

		// Token: 0x04001298 RID: 4760
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg2;

		// Token: 0x04001299 RID: 4761
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg3;

		// Token: 0x0400129A RID: 4762
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg4;

		// Token: 0x0400129B RID: 4763
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg5;

		// Token: 0x0400129C RID: 4764
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg6;

		// Token: 0x0400129D RID: 4765
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg7;

		// Token: 0x0400129E RID: 4766
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg8;

		// Token: 0x0400129F RID: 4767
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg9;

		// Token: 0x040012A0 RID: 4768
		[CompilerGenerated]
		private static Lexer.StateHandler f__mgA;

		// Token: 0x040012A1 RID: 4769
		[CompilerGenerated]
		private static Lexer.StateHandler f__mgB;

		// Token: 0x040012A2 RID: 4770
		[CompilerGenerated]
		private static Lexer.StateHandler f__mgC;

		// Token: 0x040012A3 RID: 4771
		[CompilerGenerated]
		private static Lexer.StateHandler f__mgD;

		// Token: 0x040012A4 RID: 4772
		[CompilerGenerated]
		private static Lexer.StateHandler f__mgE;

		// Token: 0x040012A5 RID: 4773
		[CompilerGenerated]
		private static Lexer.StateHandler f__mgF;

		// Token: 0x040012A6 RID: 4774
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg10;

		// Token: 0x040012A7 RID: 4775
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg11;

		// Token: 0x040012A8 RID: 4776
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg12;

		// Token: 0x040012A9 RID: 4777
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg13;

		// Token: 0x040012AA RID: 4778
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg14;

		// Token: 0x040012AB RID: 4779
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg15;

		// Token: 0x040012AC RID: 4780
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg16;

		// Token: 0x040012AD RID: 4781
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg17;

		// Token: 0x040012AE RID: 4782
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg18;

		// Token: 0x040012AF RID: 4783
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg19;

		// Token: 0x040012B0 RID: 4784
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg1A;

		// Token: 0x040012B1 RID: 4785
		[CompilerGenerated]
		private static Lexer.StateHandler f__mg1B;

		// Token: 0x02000408 RID: 1032
		// (Invoke) Token: 0x0600258C RID: 9612
		private delegate bool StateHandler(FsmContext ctx);
	}
}
