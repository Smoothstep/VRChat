using System;
using System.Collections.Generic;
using System.Linq;

namespace VRC
{
	// Token: 0x02000A5E RID: 2654
	public class Console
	{
		// Token: 0x06005059 RID: 20569 RVA: 0x001B7CFC File Offset: 0x001B60FC
		public static string ProcessConsoleCommand(string consoleCommand)
		{
			string[] array = consoleCommand.Split(new char[]
			{
				' '
			});
			string command = array[0];
			string[] array2 = array.Skip(1).ToArray<string>();
			List<string> list = new List<string>();
			string text = string.Empty;
			bool flag = false;
			foreach (string text2 in array2)
			{
				if (text2.IndexOf('"') == 0)
				{
					if (text2.EndsWith("\""))
					{
						text = text2.Remove(0, 1);
						text = text.Remove(text.Length - 1, 1);
						list.Add(text);
					}
					else
					{
						flag = true;
						text = text2.Remove(0, 1);
					}
				}
				else if (text2.EndsWith("\""))
				{
					flag = false;
					text = text + " " + text2.Remove(text2.Length - 1);
					list.Add(text);
				}
				else if (flag)
				{
					text = text + " " + text2;
				}
				else
				{
					list.Add(text2);
				}
			}
			string[] args = list.ToArray();
			return ConsoleCommandsRepository.Instance.ExecuteCommand(command, args);
		}
	}
}
