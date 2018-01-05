using System;
using System.Collections;

namespace LitJson
{
	// Token: 0x020003F3 RID: 1011
	public interface IJsonWrapper : IList, IOrderedDictionary, ICollection, IEnumerable, IDictionary
	{
		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06002418 RID: 9240
		bool IsArray { get; }

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06002419 RID: 9241
		bool IsBoolean { get; }

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x0600241A RID: 9242
		bool IsDouble { get; }

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x0600241B RID: 9243
		bool IsInt { get; }

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x0600241C RID: 9244
		bool IsLong { get; }

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x0600241D RID: 9245
		bool IsObject { get; }

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x0600241E RID: 9246
		bool IsString { get; }

		// Token: 0x0600241F RID: 9247
		bool GetBoolean();

		// Token: 0x06002420 RID: 9248
		double GetDouble();

		// Token: 0x06002421 RID: 9249
		int GetInt();

		// Token: 0x06002422 RID: 9250
		JsonType GetJsonType();

		// Token: 0x06002423 RID: 9251
		long GetLong();

		// Token: 0x06002424 RID: 9252
		string GetString();

		// Token: 0x06002425 RID: 9253
		void SetBoolean(bool val);

		// Token: 0x06002426 RID: 9254
		void SetDouble(double val);

		// Token: 0x06002427 RID: 9255
		void SetInt(int val);

		// Token: 0x06002428 RID: 9256
		void SetJsonType(JsonType type);

		// Token: 0x06002429 RID: 9257
		void SetLong(long val);

		// Token: 0x0600242A RID: 9258
		void SetString(string val);

		// Token: 0x0600242B RID: 9259
		string ToJson();

		// Token: 0x0600242C RID: 9260
		void ToJson(JsonWriter writer);
	}
}
