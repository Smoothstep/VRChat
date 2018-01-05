// Decompiled with JetBrains decompiler
// Type: ByteReader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D96FA5-73D5-49AE-8538-9A130950C0D8
// Assembly location: C:\Program Files (x86)\Steam\SteamApps\common\VRChat\VRChat_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ByteReader
{
    private static BetterList<string> mTemp = new BetterList<string>();
    private byte[] mBuffer;
    private int mOffset;

    public ByteReader(byte[] bytes)
    {
        this.mBuffer = bytes;
    }

    public ByteReader(TextAsset asset)
    {
        this.mBuffer = asset.bytes;
    }

    public static ByteReader Open(string path)
    {
        FileStream fileStream = File.OpenRead(path);
        if (fileStream == null)
            return (ByteReader)null;
        fileStream.Seek(0L, SeekOrigin.End);
        byte[] numArray = new byte[fileStream.Position];
        fileStream.Seek(0L, SeekOrigin.Begin);
        fileStream.Read(numArray, 0, numArray.Length);
        fileStream.Close();
        return new ByteReader(numArray);
    }

    public bool canRead
    {
        get
        {
            if (this.mBuffer != null)
                return this.mOffset < this.mBuffer.Length;
            return false;
        }
    }

    private static string ReadLine(byte[] buffer, int start, int count)
    {
        return Encoding.UTF8.GetString(buffer, start, count);
    }

    public string ReadLine()
    {
        return this.ReadLine(true);
    }

    public string ReadLine(bool skipEmptyLines)
    {
        int length = this.mBuffer.Length;
        if (skipEmptyLines)
        {
            while (this.mOffset < length && (int)this.mBuffer[this.mOffset] < 32)
                ++this.mOffset;
        }
        int mOffset = this.mOffset;
        if (mOffset < length)
        {
            while (mOffset < length)
            {
                switch (this.mBuffer[mOffset++])
                {
                    case 10:
                    case 13:
                        goto label_7;
                    default:
                        continue;
                }
            }
            ++mOffset;
            label_7:
            string str = ByteReader.ReadLine(this.mBuffer, this.mOffset, mOffset - this.mOffset - 1);
            this.mOffset = mOffset;
            return str;
        }
        this.mOffset = length;
        return (string)null;
    }

    public Dictionary<string, string> ReadDictionary()
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        char[] separator = new char[1] { '=' };
        while (this.canRead)
        {
            string str1 = this.ReadLine();
            if (str1 != null)
            {
                if (!str1.StartsWith("//"))
                {
                    string[] strArray = str1.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (strArray.Length == 2)
                    {
                        string index = strArray[0].Trim();
                        string str2 = strArray[1].Trim().Replace("\\n", "\n");
                        dictionary[index] = str2;
                    }
                }
            }
            else
                break;
        }
        return dictionary;
    }

    public BetterList<string> ReadCSV()
    {
        ByteReader.mTemp.Clear();
        string str1 = string.Empty;
        bool flag = false;
        int startIndex = 0;
        while (this.canRead)
        {
            if (flag)
            {
                string str2 = this.ReadLine(false);
                if (str2 == null)
                    return (BetterList<string>)null;
                string str3 = str2.Replace("\\n", "\n");
                str1 = str1 + "\n" + str3;
            }
            else
            {
                string str2 = this.ReadLine(true);
                if (str2 == null)
                    return (BetterList<string>)null;
                str1 = str2.Replace("\\n", "\n");
                startIndex = 0;
            }
            int index = startIndex;
            for (int length = str1.Length; index < length; ++index)
            {
                switch (str1[index])
                {
                    case '"':
                        if (flag)
                        {
                            if (index + 1 >= length)
                            {
                                ByteReader.mTemp.Add(str1.Substring(startIndex, index - startIndex).Replace("\"\"", "\""));
                                return ByteReader.mTemp;
                            }
                            if ((int)str1[index + 1] != 34)
                            {
                                ByteReader.mTemp.Add(str1.Substring(startIndex, index - startIndex).Replace("\"\"", "\""));
                                flag = false;
                                if ((int)str1[index + 1] == 44)
                                {
                                    ++index;
                                    startIndex = index + 1;
                                    break;
                                }
                                break;
                            }
                            ++index;
                            break;
                        }
                        startIndex = index + 1;
                        flag = true;
                        break;
                    case ',':
                        if (!flag)
                        {
                            ByteReader.mTemp.Add(str1.Substring(startIndex, index - startIndex));
                            startIndex = index + 1;
                            break;
                        }
                        break;
                }
            }
            if (startIndex < str1.Length)
            {
                if (!flag)
                    ByteReader.mTemp.Add(str1.Substring(startIndex, str1.Length - startIndex));
                else
                    continue;
            }
            return ByteReader.mTemp;
        }
        return (BetterList<string>)null;
    }
}
