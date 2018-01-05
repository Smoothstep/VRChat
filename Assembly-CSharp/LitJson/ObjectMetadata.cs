using System;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x020003F9 RID: 1017
	internal struct ObjectMetadata
	{
		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x0600249F RID: 9375 RVA: 0x000B50CE File Offset: 0x000B34CE
		// (set) Token: 0x060024A0 RID: 9376 RVA: 0x000B50EC File Offset: 0x000B34EC
		public Type ElementType
		{
			get
			{
				if (this.element_type == null)
				{
					return typeof(JsonData);
				}
				return this.element_type;
			}
			set
			{
				this.element_type = value;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x060024A1 RID: 9377 RVA: 0x000B50F5 File Offset: 0x000B34F5
		// (set) Token: 0x060024A2 RID: 9378 RVA: 0x000B50FD File Offset: 0x000B34FD
		public bool IsDictionary
		{
			get
			{
				return this.is_dictionary;
			}
			set
			{
				this.is_dictionary = value;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x060024A3 RID: 9379 RVA: 0x000B5106 File Offset: 0x000B3506
		// (set) Token: 0x060024A4 RID: 9380 RVA: 0x000B510E File Offset: 0x000B350E
		public IDictionary<string, PropertyMetadata> Properties
		{
			get
			{
				return this.properties;
			}
			set
			{
				this.properties = value;
			}
		}

		// Token: 0x04001226 RID: 4646
		private Type element_type;

		// Token: 0x04001227 RID: 4647
		private bool is_dictionary;

		// Token: 0x04001228 RID: 4648
		private IDictionary<string, PropertyMetadata> properties;
	}
}
