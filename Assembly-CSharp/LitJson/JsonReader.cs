using System;
using System.Collections.Generic;
using System.IO;

namespace LitJson
{
	// Token: 0x02000402 RID: 1026
	public class JsonReader
	{
		// Token: 0x06002525 RID: 9509 RVA: 0x000B69C9 File Offset: 0x000B4DC9
		static JsonReader()
		{
			JsonReader.PopulateParseTable();
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x000B69D0 File Offset: 0x000B4DD0
		public JsonReader(string json_text) : this(new StringReader(json_text), true)
		{
		}

		// Token: 0x06002527 RID: 9511 RVA: 0x000B69DF File Offset: 0x000B4DDF
		public JsonReader(TextReader reader) : this(reader, false)
		{
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x000B69EC File Offset: 0x000B4DEC
		private JsonReader(TextReader reader, bool owned)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.read_started = false;
			this.automaton_stack = new Stack<int>();
			this.automaton_stack.Push(65553);
			this.automaton_stack.Push(65543);
			this.lexer = new Lexer(reader);
			this.end_of_input = false;
			this.end_of_json = false;
			this.skip_non_members = true;
			this.reader = reader;
			this.reader_is_owned = owned;
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06002529 RID: 9513 RVA: 0x000B6A7F File Offset: 0x000B4E7F
		// (set) Token: 0x0600252A RID: 9514 RVA: 0x000B6A8C File Offset: 0x000B4E8C
		public bool AllowComments
		{
			get
			{
				return this.lexer.AllowComments;
			}
			set
			{
				this.lexer.AllowComments = value;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x0600252B RID: 9515 RVA: 0x000B6A9A File Offset: 0x000B4E9A
		// (set) Token: 0x0600252C RID: 9516 RVA: 0x000B6AA7 File Offset: 0x000B4EA7
		public bool AllowSingleQuotedStrings
		{
			get
			{
				return this.lexer.AllowSingleQuotedStrings;
			}
			set
			{
				this.lexer.AllowSingleQuotedStrings = value;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x0600252D RID: 9517 RVA: 0x000B6AB5 File Offset: 0x000B4EB5
		// (set) Token: 0x0600252E RID: 9518 RVA: 0x000B6ABD File Offset: 0x000B4EBD
		public bool SkipNonMembers
		{
			get
			{
				return this.skip_non_members;
			}
			set
			{
				this.skip_non_members = value;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x0600252F RID: 9519 RVA: 0x000B6AC6 File Offset: 0x000B4EC6
		public bool EndOfInput
		{
			get
			{
				return this.end_of_input;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06002530 RID: 9520 RVA: 0x000B6ACE File Offset: 0x000B4ECE
		public bool EndOfJson
		{
			get
			{
				return this.end_of_json;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06002531 RID: 9521 RVA: 0x000B6AD6 File Offset: 0x000B4ED6
		public JsonToken Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06002532 RID: 9522 RVA: 0x000B6ADE File Offset: 0x000B4EDE
		public object Value
		{
			get
			{
				return this.token_value;
			}
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x000B6AE8 File Offset: 0x000B4EE8
		private static void PopulateParseTable()
		{
			JsonReader.parse_table = new Dictionary<int, IDictionary<int, int[]>>();
			JsonReader.TableAddRow(ParserToken.Array);
			JsonReader.TableAddCol(ParserToken.Array, 91, new int[]
			{
				91,
				65549
			});
			JsonReader.TableAddRow(ParserToken.ArrayPrime);
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 34, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 91, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 93, new int[]
			{
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 123, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65537, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65538, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65539, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65540, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddRow(ParserToken.Object);
			JsonReader.TableAddCol(ParserToken.Object, 123, new int[]
			{
				123,
				65545
			});
			JsonReader.TableAddRow(ParserToken.ObjectPrime);
			JsonReader.TableAddCol(ParserToken.ObjectPrime, 34, new int[]
			{
				65546,
				65547,
				125
			});
			JsonReader.TableAddCol(ParserToken.ObjectPrime, 125, new int[]
			{
				125
			});
			JsonReader.TableAddRow(ParserToken.Pair);
			JsonReader.TableAddCol(ParserToken.Pair, 34, new int[]
			{
				65552,
				58,
				65550
			});
			JsonReader.TableAddRow(ParserToken.PairRest);
			JsonReader.TableAddCol(ParserToken.PairRest, 44, new int[]
			{
				44,
				65546,
				65547
			});
			JsonReader.TableAddCol(ParserToken.PairRest, 125, new int[]
			{
				65554
			});
			JsonReader.TableAddRow(ParserToken.String);
			JsonReader.TableAddCol(ParserToken.String, 34, new int[]
			{
				34,
				65541,
				34
			});
			JsonReader.TableAddRow(ParserToken.Text);
			JsonReader.TableAddCol(ParserToken.Text, 91, new int[]
			{
				65548
			});
			JsonReader.TableAddCol(ParserToken.Text, 123, new int[]
			{
				65544
			});
			JsonReader.TableAddRow(ParserToken.Value);
			JsonReader.TableAddCol(ParserToken.Value, 34, new int[]
			{
				65552
			});
			JsonReader.TableAddCol(ParserToken.Value, 91, new int[]
			{
				65548
			});
			JsonReader.TableAddCol(ParserToken.Value, 123, new int[]
			{
				65544
			});
			JsonReader.TableAddCol(ParserToken.Value, 65537, new int[]
			{
				65537
			});
			JsonReader.TableAddCol(ParserToken.Value, 65538, new int[]
			{
				65538
			});
			JsonReader.TableAddCol(ParserToken.Value, 65539, new int[]
			{
				65539
			});
			JsonReader.TableAddCol(ParserToken.Value, 65540, new int[]
			{
				65540
			});
			JsonReader.TableAddRow(ParserToken.ValueRest);
			JsonReader.TableAddCol(ParserToken.ValueRest, 44, new int[]
			{
				44,
				65550,
				65551
			});
			JsonReader.TableAddCol(ParserToken.ValueRest, 93, new int[]
			{
				65554
			});
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x000B6E61 File Offset: 0x000B5261
		private static void TableAddCol(ParserToken row, int col, params int[] symbols)
		{
			JsonReader.parse_table[(int)row].Add(col, symbols);
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x000B6E75 File Offset: 0x000B5275
		private static void TableAddRow(ParserToken rule)
		{
			JsonReader.parse_table.Add((int)rule, new Dictionary<int, int[]>());
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x000B6E88 File Offset: 0x000B5288
		private void ProcessNumber(string number)
		{
			double num;
			if ((number.IndexOf('.') != -1 || number.IndexOf('e') != -1 || number.IndexOf('E') != -1) && double.TryParse(number, out num))
			{
				this.token = JsonToken.Double;
				this.token_value = num;
				return;
			}
			int num2;
			if (int.TryParse(number, out num2))
			{
				this.token = JsonToken.Int;
				this.token_value = num2;
				return;
			}
			long num3;
			if (long.TryParse(number, out num3))
			{
				this.token = JsonToken.Long;
				this.token_value = num3;
				return;
			}
			this.token = JsonToken.Int;
			this.token_value = 0;
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x000B6F38 File Offset: 0x000B5338
		private void ProcessSymbol()
		{
			if (this.current_symbol == 91)
			{
				this.token = JsonToken.ArrayStart;
				this.parser_return = true;
			}
			else if (this.current_symbol == 93)
			{
				this.token = JsonToken.ArrayEnd;
				this.parser_return = true;
			}
			else if (this.current_symbol == 123)
			{
				this.token = JsonToken.ObjectStart;
				this.parser_return = true;
			}
			else if (this.current_symbol == 125)
			{
				this.token = JsonToken.ObjectEnd;
				this.parser_return = true;
			}
			else if (this.current_symbol == 34)
			{
				if (this.parser_in_string)
				{
					this.parser_in_string = false;
					this.parser_return = true;
				}
				else
				{
					if (this.token == JsonToken.None)
					{
						this.token = JsonToken.String;
					}
					this.parser_in_string = true;
				}
			}
			else if (this.current_symbol == 65541)
			{
				this.token_value = this.lexer.StringValue;
			}
			else if (this.current_symbol == 65539)
			{
				this.token = JsonToken.Boolean;
				this.token_value = false;
				this.parser_return = true;
			}
			else if (this.current_symbol == 65540)
			{
				this.token = JsonToken.Null;
				this.parser_return = true;
			}
			else if (this.current_symbol == 65537)
			{
				this.ProcessNumber(this.lexer.StringValue);
				this.parser_return = true;
			}
			else if (this.current_symbol == 65546)
			{
				this.token = JsonToken.PropertyName;
			}
			else if (this.current_symbol == 65538)
			{
				this.token = JsonToken.Boolean;
				this.token_value = true;
				this.parser_return = true;
			}
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x000B7100 File Offset: 0x000B5500
		private bool ReadToken()
		{
			if (this.end_of_input)
			{
				return false;
			}
			this.lexer.NextToken();
			if (this.lexer.EndOfInput)
			{
				this.Close();
				return false;
			}
			this.current_input = this.lexer.Token;
			return true;
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x000B7150 File Offset: 0x000B5550
		public void Close()
		{
			if (this.end_of_input)
			{
				return;
			}
			this.end_of_input = true;
			this.end_of_json = true;
			if (this.reader_is_owned)
			{
				this.reader.Dispose();
			}
			this.reader = null;
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x000B718C File Offset: 0x000B558C
		public bool Read()
		{
			if (this.end_of_input)
			{
				return false;
			}
			if (this.end_of_json)
			{
				this.end_of_json = false;
				this.automaton_stack.Clear();
				this.automaton_stack.Push(65553);
				this.automaton_stack.Push(65543);
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.token = JsonToken.None;
			this.token_value = null;
			if (!this.read_started)
			{
				this.read_started = true;
				if (!this.ReadToken())
				{
					return false;
				}
			}
			while (!this.parser_return)
			{
				this.current_symbol = this.automaton_stack.Pop();
				this.ProcessSymbol();
				if (this.current_symbol == this.current_input)
				{
					if (!this.ReadToken())
					{
						if (this.automaton_stack.Peek() != 65553)
						{
							throw new JsonException("Input doesn't evaluate to proper JSON text");
						}
						return this.parser_return;
					}
				}
				else
				{
					int[] array;
					try
					{
						array = JsonReader.parse_table[this.current_symbol][this.current_input];
					}
					catch (KeyNotFoundException inner_exception)
					{
						throw new JsonException((ParserToken)this.current_input, inner_exception);
					}
					if (array[0] != 65554)
					{
						for (int i = array.Length - 1; i >= 0; i--)
						{
							this.automaton_stack.Push(array[i]);
						}
					}
				}
			}
			if (this.automaton_stack.Peek() == 65553)
			{
				this.end_of_json = true;
			}
			return true;
		}

		// Token: 0x0400125F RID: 4703
		private static IDictionary<int, IDictionary<int, int[]>> parse_table;

		// Token: 0x04001260 RID: 4704
		private Stack<int> automaton_stack;

		// Token: 0x04001261 RID: 4705
		private int current_input;

		// Token: 0x04001262 RID: 4706
		private int current_symbol;

		// Token: 0x04001263 RID: 4707
		private bool end_of_json;

		// Token: 0x04001264 RID: 4708
		private bool end_of_input;

		// Token: 0x04001265 RID: 4709
		private Lexer lexer;

		// Token: 0x04001266 RID: 4710
		private bool parser_in_string;

		// Token: 0x04001267 RID: 4711
		private bool parser_return;

		// Token: 0x04001268 RID: 4712
		private bool read_started;

		// Token: 0x04001269 RID: 4713
		private TextReader reader;

		// Token: 0x0400126A RID: 4714
		private bool reader_is_owned;

		// Token: 0x0400126B RID: 4715
		private bool skip_non_members;

		// Token: 0x0400126C RID: 4716
		private object token_value;

		// Token: 0x0400126D RID: 4717
		private JsonToken token;
	}
}
