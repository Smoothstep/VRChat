using System;

// Token: 0x02000582 RID: 1410
[Serializable]
public class InvStat
{
	// Token: 0x06002FB7 RID: 12215 RVA: 0x000E903F File Offset: 0x000E743F
	public static string GetName(InvStat.Identifier i)
	{
		return i.ToString();
	}

	// Token: 0x06002FB8 RID: 12216 RVA: 0x000E9050 File Offset: 0x000E7450
	public static string GetDescription(InvStat.Identifier i)
	{
		switch (i)
		{
		case InvStat.Identifier.Strength:
			return "Strength increases melee damage";
		case InvStat.Identifier.Constitution:
			return "Constitution increases health";
		case InvStat.Identifier.Agility:
			return "Agility increases armor";
		case InvStat.Identifier.Intelligence:
			return "Intelligence increases mana";
		case InvStat.Identifier.Damage:
			return "Damage adds to the amount of damage done in combat";
		case InvStat.Identifier.Crit:
			return "Crit increases the chance of landing a critical strike";
		case InvStat.Identifier.Armor:
			return "Armor protects from damage";
		case InvStat.Identifier.Health:
			return "Health prolongs life";
		case InvStat.Identifier.Mana:
			return "Mana increases the number of spells that can be cast";
		default:
			return null;
		}
	}

	// Token: 0x06002FB9 RID: 12217 RVA: 0x000E90C4 File Offset: 0x000E74C4
	public static int CompareArmor(InvStat a, InvStat b)
	{
		int num = (int)a.id;
		int num2 = (int)b.id;
		if (a.id == InvStat.Identifier.Armor)
		{
			num -= 10000;
		}
		else if (a.id == InvStat.Identifier.Damage)
		{
			num -= 5000;
		}
		if (b.id == InvStat.Identifier.Armor)
		{
			num2 -= 10000;
		}
		else if (b.id == InvStat.Identifier.Damage)
		{
			num2 -= 5000;
		}
		if (a.amount < 0)
		{
			num += 1000;
		}
		if (b.amount < 0)
		{
			num2 += 1000;
		}
		if (a.modifier == InvStat.Modifier.Percent)
		{
			num += 100;
		}
		if (b.modifier == InvStat.Modifier.Percent)
		{
			num2 += 100;
		}
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06002FBA RID: 12218 RVA: 0x000E9198 File Offset: 0x000E7598
	public static int CompareWeapon(InvStat a, InvStat b)
	{
		int num = (int)a.id;
		int num2 = (int)b.id;
		if (a.id == InvStat.Identifier.Damage)
		{
			num -= 10000;
		}
		else if (a.id == InvStat.Identifier.Armor)
		{
			num -= 5000;
		}
		if (b.id == InvStat.Identifier.Damage)
		{
			num2 -= 10000;
		}
		else if (b.id == InvStat.Identifier.Armor)
		{
			num2 -= 5000;
		}
		if (a.amount < 0)
		{
			num += 1000;
		}
		if (b.amount < 0)
		{
			num2 += 1000;
		}
		if (a.modifier == InvStat.Modifier.Percent)
		{
			num += 100;
		}
		if (b.modifier == InvStat.Modifier.Percent)
		{
			num2 += 100;
		}
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x04001A13 RID: 6675
	public InvStat.Identifier id;

	// Token: 0x04001A14 RID: 6676
	public InvStat.Modifier modifier;

	// Token: 0x04001A15 RID: 6677
	public int amount;

	// Token: 0x02000583 RID: 1411
	public enum Identifier
	{
		// Token: 0x04001A17 RID: 6679
		Strength,
		// Token: 0x04001A18 RID: 6680
		Constitution,
		// Token: 0x04001A19 RID: 6681
		Agility,
		// Token: 0x04001A1A RID: 6682
		Intelligence,
		// Token: 0x04001A1B RID: 6683
		Damage,
		// Token: 0x04001A1C RID: 6684
		Crit,
		// Token: 0x04001A1D RID: 6685
		Armor,
		// Token: 0x04001A1E RID: 6686
		Health,
		// Token: 0x04001A1F RID: 6687
		Mana,
		// Token: 0x04001A20 RID: 6688
		Other
	}

	// Token: 0x02000584 RID: 1412
	public enum Modifier
	{
		// Token: 0x04001A22 RID: 6690
		Added,
		// Token: 0x04001A23 RID: 6691
		Percent
	}
}
