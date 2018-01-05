using System;

// Token: 0x0200076E RID: 1902
public class Region
{
	// Token: 0x06003E3E RID: 15934 RVA: 0x00139380 File Offset: 0x00137780
	public Region(CloudRegionCode code)
	{
		this.Code = code;
		this.Cluster = code.ToString();
	}

	// Token: 0x06003E3F RID: 15935 RVA: 0x001393A2 File Offset: 0x001377A2
	public Region(CloudRegionCode code, string regionCodeString, string address)
	{
		this.Code = code;
		this.Cluster = regionCodeString;
		this.HostAndPort = address;
	}

	// Token: 0x06003E40 RID: 15936 RVA: 0x001393C0 File Offset: 0x001377C0
	public static CloudRegionCode Parse(string codeAsString)
	{
		if (codeAsString == null)
		{
			return CloudRegionCode.none;
		}
		int num = codeAsString.IndexOf('/');
		if (num > 0)
		{
			codeAsString = codeAsString.Substring(0, num);
		}
		codeAsString = codeAsString.ToLower();
		if (Enum.IsDefined(typeof(CloudRegionCode), codeAsString))
		{
			return (CloudRegionCode)Enum.Parse(typeof(CloudRegionCode), codeAsString);
		}
		return CloudRegionCode.none;
	}

	// Token: 0x06003E41 RID: 15937 RVA: 0x00139424 File Offset: 0x00137824
	internal static CloudRegionFlag ParseFlag(CloudRegionCode region)
	{
		if (Enum.IsDefined(typeof(CloudRegionFlag), region.ToString()))
		{
			return (CloudRegionFlag)Enum.Parse(typeof(CloudRegionFlag), region.ToString());
		}
		return (CloudRegionFlag)0;
	}

	// Token: 0x06003E42 RID: 15938 RVA: 0x00139478 File Offset: 0x00137878
	[Obsolete]
	internal static CloudRegionFlag ParseFlag(string codeAsString)
	{
		codeAsString = codeAsString.ToLower();
		CloudRegionFlag result = (CloudRegionFlag)0;
		if (Enum.IsDefined(typeof(CloudRegionFlag), codeAsString))
		{
			result = (CloudRegionFlag)Enum.Parse(typeof(CloudRegionFlag), codeAsString);
		}
		return result;
	}

	// Token: 0x06003E43 RID: 15939 RVA: 0x001394BB File Offset: 0x001378BB
	public override string ToString()
	{
		return string.Format("'{0}' \t{1}ms \t{2}", this.Cluster, this.Ping, this.HostAndPort);
	}

	// Token: 0x040026B3 RID: 9907
	public CloudRegionCode Code;

	// Token: 0x040026B4 RID: 9908
	public string Cluster;

	// Token: 0x040026B5 RID: 9909
	public string HostAndPort;

	// Token: 0x040026B6 RID: 9910
	public int Ping;
}
