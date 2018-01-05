using System;
using UnityEngine;

// Token: 0x02000473 RID: 1139
public class BitMaskAttribute : PropertyAttribute
{
	// Token: 0x06002779 RID: 10105 RVA: 0x000CC590 File Offset: 0x000CA990
	public BitMaskAttribute(Type attributeType)
	{
		this.propertyType = attributeType;
	}

	// Token: 0x04001560 RID: 5472
	public Type propertyType;
}
