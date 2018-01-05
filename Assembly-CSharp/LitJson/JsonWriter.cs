using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace LitJson
{
	// Token: 0x02000405 RID: 1029
	public class JsonWriter
	{
		// Token: 0x0600253D RID: 9533 RVA: 0x000B733C File Offset: 0x000B573C
		public JsonWriter()
		{
			this.inst_string_builder = new StringBuilder();
			this.writer = new StringWriter(this.inst_string_builder);
			this.Init();
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x000B7366 File Offset: 0x000B5766
		public JsonWriter(StringBuilder sb) : this(new StringWriter(sb))
		{
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x000B7374 File Offset: 0x000B5774
		public JsonWriter(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
			this.Init();
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06002540 RID: 9536 RVA: 0x000B739A File Offset: 0x000B579A
		// (set) Token: 0x06002541 RID: 9537 RVA: 0x000B73A2 File Offset: 0x000B57A2
		public int IndentValue
		{
			get
			{
				return this.indent_value;
			}
			set
			{
				this.indentation = this.indentation / this.indent_value * value;
				this.indent_value = value;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06002542 RID: 9538 RVA: 0x000B73C0 File Offset: 0x000B57C0
		// (set) Token: 0x06002543 RID: 9539 RVA: 0x000B73C8 File Offset: 0x000B57C8
		public bool PrettyPrint
		{
			get
			{
				return this.pretty_print;
			}
			set
			{
				this.pretty_print = value;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06002544 RID: 9540 RVA: 0x000B73D1 File Offset: 0x000B57D1
		public TextWriter TextWriter
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06002545 RID: 9541 RVA: 0x000B73D9 File Offset: 0x000B57D9
		// (set) Token: 0x06002546 RID: 9542 RVA: 0x000B73E1 File Offset: 0x000B57E1
		public bool Validate
		{
			get
			{
				return this.validate;
			}
			set
			{
				this.validate = value;
			}
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x000B73EC File Offset: 0x000B57EC
		private void DoValidation(Condition cond)
		{
			if (!this.context.ExpectingValue)
			{
				this.context.Count++;
			}
			if (!this.validate)
			{
				return;
			}
			if (this.has_reached_end)
			{
				throw new JsonException("A complete JSON symbol has already been written");
			}
			switch (cond)
			{
			case Condition.InArray:
				if (!this.context.InArray)
				{
					throw new JsonException("Can't close an array here");
				}
				break;
			case Condition.InObject:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't close an object here");
				}
				break;
			case Condition.NotAProperty:
				if (this.context.InObject && !this.context.ExpectingValue)
				{
					throw new JsonException("Expected a property");
				}
				break;
			case Condition.Property:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't add a property here");
				}
				break;
			case Condition.Value:
				if (!this.context.InArray && (!this.context.InObject || !this.context.ExpectingValue))
				{
					throw new JsonException("Can't add a value here");
				}
				break;
			}
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x000B7550 File Offset: 0x000B5950
		private void Init()
		{
			this.has_reached_end = false;
			this.hex_seq = new char[4];
			this.indentation = 0;
			this.indent_value = 4;
			this.pretty_print = false;
			this.validate = true;
			this.ctx_stack = new Stack<WriterContext>();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x000B75B4 File Offset: 0x000B59B4
		private static void IntToHex(int n, char[] hex)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = n % 16;
				if (num < 10)
				{
					hex[3 - i] = (char)(48 + num);
				}
				else
				{
					hex[3 - i] = (char)(65 + (num - 10));
				}
				n >>= 4;
			}
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x000B7601 File Offset: 0x000B5A01
		private void Indent()
		{
			if (this.pretty_print)
			{
				this.indentation += this.indent_value;
			}
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x000B7624 File Offset: 0x000B5A24
		private void Put(string str)
		{
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				for (int i = 0; i < this.indentation; i++)
				{
					this.writer.Write(' ');
				}
			}
			this.writer.Write(str);
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x000B767C File Offset: 0x000B5A7C
		private void PutNewline()
		{
			this.PutNewline(true);
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x000B7688 File Offset: 0x000B5A88
		private void PutNewline(bool add_comma)
		{
			if (add_comma && !this.context.ExpectingValue && this.context.Count > 1)
			{
				this.writer.Write(',');
			}
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				this.writer.Write('\n');
			}
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x000B76F4 File Offset: 0x000B5AF4
		private void PutString(string str)
		{
			this.Put(string.Empty);
			this.writer.Write('"');
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				char c = str[i];
				switch (c)
				{
				case '\b':
					this.writer.Write("\\b");
					break;
				case '\t':
					this.writer.Write("\\t");
					break;
				case '\n':
					this.writer.Write("\\n");
					break;
				default:
					if (c != '"' && c != '\\')
					{
						if (str[i] >= ' ' && str[i] <= '~')
						{
							this.writer.Write(str[i]);
						}
						else
						{
							JsonWriter.IntToHex((int)str[i], this.hex_seq);
							this.writer.Write("\\u");
							this.writer.Write(this.hex_seq);
						}
					}
					else
					{
						this.writer.Write('\\');
						this.writer.Write(str[i]);
					}
					break;
				case '\f':
					this.writer.Write("\\f");
					break;
				case '\r':
					this.writer.Write("\\r");
					break;
				}
			}
			this.writer.Write('"');
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x000B786F File Offset: 0x000B5C6F
		private void Unindent()
		{
			if (this.pretty_print)
			{
				this.indentation -= this.indent_value;
			}
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x000B788F File Offset: 0x000B5C8F
		public override string ToString()
		{
			if (this.inst_string_builder == null)
			{
				return string.Empty;
			}
			return this.inst_string_builder.ToString();
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x000B78B0 File Offset: 0x000B5CB0
		public void Reset()
		{
			this.has_reached_end = false;
			this.ctx_stack.Clear();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
			if (this.inst_string_builder != null)
			{
				this.inst_string_builder.Remove(0, this.inst_string_builder.Length);
			}
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x000B790E File Offset: 0x000B5D0E
		public void Write(bool boolean)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put((!boolean) ? "false" : "true");
			this.context.ExpectingValue = false;
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x000B7944 File Offset: 0x000B5D44
		public void Write(decimal number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x000B7970 File Offset: 0x000B5D70
		public void Write(double number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			string text = Convert.ToString(number, JsonWriter.number_format);
			this.Put(text);
			if (text.IndexOf('.') == -1 && text.IndexOf('E') == -1)
			{
				this.writer.Write(".0");
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x000B79D5 File Offset: 0x000B5DD5
		public void Write(int number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x000B7A01 File Offset: 0x000B5E01
		public void Write(long number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x000B7A2D File Offset: 0x000B5E2D
		public void Write(string str)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			if (str == null)
			{
				this.Put("null");
			}
			else
			{
				this.PutString(str);
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x000B7A65 File Offset: 0x000B5E65
		public void Write(ulong number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x000B7A94 File Offset: 0x000B5E94
		public void WriteArrayEnd()
		{
			this.DoValidation(Condition.InArray);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("]");
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x000B7B08 File Offset: 0x000B5F08
		public void WriteArrayStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("[");
			this.context = new WriterContext();
			this.context.InArray = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x000B7B5C File Offset: 0x000B5F5C
		public void WriteObjectEnd()
		{
			this.DoValidation(Condition.InObject);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("}");
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x000B7BD0 File Offset: 0x000B5FD0
		public void WriteObjectStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("{");
			this.context = new WriterContext();
			this.context.InObject = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x000B7C24 File Offset: 0x000B6024
		public void WritePropertyName(string property_name)
		{
			this.DoValidation(Condition.Property);
			this.PutNewline();
			this.PutString(property_name);
			if (this.pretty_print)
			{
				if (property_name.Length > this.context.Padding)
				{
					this.context.Padding = property_name.Length;
				}
				for (int i = this.context.Padding - property_name.Length; i >= 0; i--)
				{
					this.writer.Write(' ');
				}
				this.writer.Write(": ");
			}
			else
			{
				this.writer.Write(':');
			}
			this.context.ExpectingValue = true;
		}

		// Token: 0x04001279 RID: 4729
		private static NumberFormatInfo number_format = NumberFormatInfo.InvariantInfo;

		// Token: 0x0400127A RID: 4730
		private WriterContext context;

		// Token: 0x0400127B RID: 4731
		private Stack<WriterContext> ctx_stack;

		// Token: 0x0400127C RID: 4732
		private bool has_reached_end;

		// Token: 0x0400127D RID: 4733
		private char[] hex_seq;

		// Token: 0x0400127E RID: 4734
		private int indentation;

		// Token: 0x0400127F RID: 4735
		private int indent_value;

		// Token: 0x04001280 RID: 4736
		private StringBuilder inst_string_builder;

		// Token: 0x04001281 RID: 4737
		private bool pretty_print;

		// Token: 0x04001282 RID: 4738
		private bool validate;

		// Token: 0x04001283 RID: 4739
		private TextWriter writer;
	}
}
