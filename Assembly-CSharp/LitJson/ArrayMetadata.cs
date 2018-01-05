using System;

namespace LitJson
{
	// Token: 0x020003F8 RID: 1016
	internal struct ArrayMetadata
	{
		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06002499 RID: 9369 RVA: 0x000B5085 File Offset: 0x000B3485
		// (set) Token: 0x0600249A RID: 9370 RVA: 0x000B50A3 File Offset: 0x000B34A3
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

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x0600249B RID: 9371 RVA: 0x000B50AC File Offset: 0x000B34AC
		// (set) Token: 0x0600249C RID: 9372 RVA: 0x000B50B4 File Offset: 0x000B34B4
		public bool IsArray
		{
			get
			{
				return this.is_array;
			}
			set
			{
				this.is_array = value;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x0600249D RID: 9373 RVA: 0x000B50BD File Offset: 0x000B34BD
		// (set) Token: 0x0600249E RID: 9374 RVA: 0x000B50C5 File Offset: 0x000B34C5
		public bool IsList
		{
			get
			{
				return this.is_list;
			}
			set
			{
				this.is_list = value;
			}
		}

		// Token: 0x04001223 RID: 4643
		private Type element_type;

		// Token: 0x04001224 RID: 4644
		private bool is_array;

		// Token: 0x04001225 RID: 4645
		private bool is_list;
	}
}
