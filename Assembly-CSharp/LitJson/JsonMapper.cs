using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace LitJson
{
	// Token: 0x020003FF RID: 1023
	public class JsonMapper
	{
		// Token: 0x060024B9 RID: 9401 RVA: 0x000B5118 File Offset: 0x000B3518
		static JsonMapper()
		{
			JsonMapper.max_nesting_depth = 100;
			JsonMapper.array_metadata = new Dictionary<Type, ArrayMetadata>();
			JsonMapper.conv_ops = new Dictionary<Type, IDictionary<Type, MethodInfo>>();
			JsonMapper.object_metadata = new Dictionary<Type, ObjectMetadata>();
			JsonMapper.type_properties = new Dictionary<Type, IList<PropertyMetadata>>();
			JsonMapper.static_writer = new JsonWriter();
			JsonMapper.datetime_format = DateTimeFormatInfo.InvariantInfo;
			JsonMapper.base_exporters_table = new Dictionary<Type, ExporterFunc>();
			JsonMapper.custom_exporters_table = new Dictionary<Type, ExporterFunc>();
			JsonMapper.base_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();
			JsonMapper.custom_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();
			JsonMapper.RegisterBaseExporters();
			JsonMapper.RegisterBaseImporters();
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x000B51D4 File Offset: 0x000B35D4
		private static bool HasInterface(Type type, string name)
		{
			return type.GetInterface(name, true) != null;
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x000B51E4 File Offset: 0x000B35E4
		public static PropertyInfo[] GetPublicInstanceProperties(Type type)
		{
			return type.GetProperties();
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x000B51EC File Offset: 0x000B35EC
		private static void AddArrayMetadata(Type type)
		{
			if (JsonMapper.array_metadata.ContainsKey(type))
			{
				return;
			}
			ArrayMetadata value = default(ArrayMetadata);
			value.IsArray = type.IsArray;
			if (JsonMapper.HasInterface(type, "System.Collections.IList"))
			{
				value.IsList = true;
			}
			foreach (PropertyInfo propertyInfo in JsonMapper.GetPublicInstanceProperties(type))
			{
				if (!(propertyInfo.Name != "Item"))
				{
					ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
					if (indexParameters.Length == 1)
					{
						if (indexParameters[0].ParameterType == typeof(int))
						{
							value.ElementType = propertyInfo.PropertyType;
						}
					}
				}
			}
			object obj = JsonMapper.array_metadata_lock;
			lock (obj)
			{
				try
				{
					JsonMapper.array_metadata.Add(type, value);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x000B52FC File Offset: 0x000B36FC
		private static void AddObjectMetadata(Type type)
		{
			if (JsonMapper.object_metadata.ContainsKey(type))
			{
				return;
			}
			ObjectMetadata value = default(ObjectMetadata);
			if (JsonMapper.HasInterface(type, "System.Collections.IDictionary"))
			{
				value.IsDictionary = true;
			}
			value.Properties = new Dictionary<string, PropertyMetadata>();
			foreach (PropertyInfo propertyInfo in JsonMapper.GetPublicInstanceProperties(type))
			{
				if (propertyInfo.Name == "Item")
				{
					ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
					if (indexParameters.Length == 1)
					{
						if (indexParameters[0].ParameterType == typeof(string))
						{
							value.ElementType = propertyInfo.PropertyType;
						}
					}
				}
				else
				{
					PropertyMetadata value2 = default(PropertyMetadata);
					value2.Info = propertyInfo;
					value2.Type = propertyInfo.PropertyType;
					value.Properties.Add(propertyInfo.Name, value2);
				}
			}
			foreach (FieldInfo fieldInfo in type.GetFields())
			{
				PropertyMetadata value3 = default(PropertyMetadata);
				value3.Info = fieldInfo;
				value3.IsField = true;
				value3.Type = fieldInfo.FieldType;
				value.Properties.Add(fieldInfo.Name, value3);
			}
			object obj = JsonMapper.object_metadata_lock;
			lock (obj)
			{
				try
				{
					JsonMapper.object_metadata.Add(type, value);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x000B54A0 File Offset: 0x000B38A0
		private static void AddTypeProperties(Type type)
		{
			if (JsonMapper.type_properties.ContainsKey(type))
			{
				return;
			}
			IList<PropertyMetadata> list = new List<PropertyMetadata>();
			foreach (PropertyInfo propertyInfo in JsonMapper.GetPublicInstanceProperties(type))
			{
				if (!(propertyInfo.Name == "Item"))
				{
					list.Add(new PropertyMetadata
					{
						Info = propertyInfo,
						IsField = false
					});
				}
			}
			foreach (FieldInfo info in type.GetFields())
			{
				list.Add(new PropertyMetadata
				{
					Info = info,
					IsField = true
				});
			}
			object obj = JsonMapper.type_properties_lock;
			lock (obj)
			{
				try
				{
					JsonMapper.type_properties.Add(type, list);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x000B55B4 File Offset: 0x000B39B4
		private static MethodInfo GetConvOp(Type t1, Type t2)
		{
			object obj = JsonMapper.conv_ops_lock;
			lock (obj)
			{
				if (!JsonMapper.conv_ops.ContainsKey(t1))
				{
					JsonMapper.conv_ops.Add(t1, new Dictionary<Type, MethodInfo>());
				}
			}
			if (JsonMapper.conv_ops[t1].ContainsKey(t2))
			{
				return JsonMapper.conv_ops[t1][t2];
			}
			MethodInfo method = t1.GetMethod("op_Implicit", new Type[]
			{
				t2
			});
			object obj2 = JsonMapper.conv_ops_lock;
			lock (obj2)
			{
				try
				{
					JsonMapper.conv_ops[t1].Add(t2, method);
				}
				catch (ArgumentException)
				{
					return JsonMapper.conv_ops[t1][t2];
				}
			}
			return method;
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x000B56AC File Offset: 0x000B3AAC
		private static object ReadValue(Type inst_type, JsonReader reader)
		{
			reader.Read();
			if (reader.Token == JsonToken.ArrayEnd)
			{
				return null;
			}
			if (reader.Token == JsonToken.Null)
			{
				if (!inst_type.IsClass)
				{
					throw new JsonException(string.Format("Can't assign null to an instance of type {0}", inst_type));
				}
				return null;
			}
			else
			{
				if (reader.Token != JsonToken.Double && reader.Token != JsonToken.Int && reader.Token != JsonToken.Long && reader.Token != JsonToken.String && reader.Token != JsonToken.Boolean)
				{
					object obj = null;
					if (reader.Token == JsonToken.ArrayStart)
					{
						if (inst_type.FullName == "System.Object")
						{
							inst_type = typeof(object[]);
						}
						JsonMapper.AddArrayMetadata(inst_type);
						ArrayMetadata arrayMetadata = JsonMapper.array_metadata[inst_type];
						if (!arrayMetadata.IsArray && !arrayMetadata.IsList)
						{
							throw new JsonException(string.Format("Type {0} can't act as an array", inst_type));
						}
						IList list;
						Type elementType;
						if (!arrayMetadata.IsArray)
						{
							list = (IList)Activator.CreateInstance(inst_type);
							elementType = arrayMetadata.ElementType;
						}
						else
						{
							list = new ArrayList();
							elementType = inst_type.GetElementType();
						}
						for (;;)
						{
							object obj2 = JsonMapper.ReadValue(elementType, reader);
							if (obj2 == null && reader.Token == JsonToken.ArrayEnd)
							{
								break;
							}
							list.Add(obj2);
						}
						if (arrayMetadata.IsArray)
						{
							int count = list.Count;
							obj = Array.CreateInstance(elementType, count);
							for (int i = 0; i < count; i++)
							{
								((Array)obj).SetValue(list[i], i);
							}
						}
						else
						{
							obj = list;
						}
					}
					else if (reader.Token == JsonToken.ObjectStart)
					{
						if (inst_type == typeof(object))
						{
							inst_type = typeof(Dictionary<string, object>);
						}
						JsonMapper.AddObjectMetadata(inst_type);
						ObjectMetadata objectMetadata = JsonMapper.object_metadata[inst_type];
						obj = Activator.CreateInstance(inst_type);
						string text;
						for (;;)
						{
							reader.Read();
							if (reader.Token == JsonToken.ObjectEnd)
							{
								break;
							}
							text = (string)reader.Value;
							if (objectMetadata.Properties.ContainsKey(text))
							{
								PropertyMetadata propertyMetadata = objectMetadata.Properties[text];
								if (propertyMetadata.IsField)
								{
									((FieldInfo)propertyMetadata.Info).SetValue(obj, JsonMapper.ReadValue(propertyMetadata.Type, reader));
								}
								else
								{
									PropertyInfo propertyInfo = (PropertyInfo)propertyMetadata.Info;
									if (propertyInfo.CanWrite)
									{
										propertyInfo.SetValue(obj, JsonMapper.ReadValue(propertyMetadata.Type, reader), null);
									}
									else
									{
										JsonMapper.ReadValue(propertyMetadata.Type, reader);
									}
								}
							}
							else if (!objectMetadata.IsDictionary)
							{
								if (!reader.SkipNonMembers)
								{
									goto Block_31;
								}
								JsonMapper.ReadSkip(reader);
							}
							else
							{
								((IDictionary)obj).Add(text, JsonMapper.ReadValue(objectMetadata.ElementType, reader));
							}
						}
						return obj;
						Block_31:
						throw new JsonException(string.Format("The type {0} doesn't have the property '{1}'", inst_type, text));
					}
					return obj;
				}
				Type type = reader.Value.GetType();
				if (inst_type.IsAssignableFrom(type))
				{
					return reader.Value;
				}
				if (JsonMapper.custom_importers_table.ContainsKey(type) && JsonMapper.custom_importers_table[type].ContainsKey(inst_type))
				{
					ImporterFunc importerFunc = JsonMapper.custom_importers_table[type][inst_type];
					return importerFunc(reader.Value);
				}
				if (JsonMapper.base_importers_table.ContainsKey(type) && JsonMapper.base_importers_table[type].ContainsKey(inst_type))
				{
					ImporterFunc importerFunc2 = JsonMapper.base_importers_table[type][inst_type];
					return importerFunc2(reader.Value);
				}
				if (inst_type.IsEnum)
				{
					return Enum.ToObject(inst_type, reader.Value);
				}
				MethodInfo convOp = JsonMapper.GetConvOp(inst_type, type);
				if (convOp != null)
				{
					return convOp.Invoke(null, new object[]
					{
						reader.Value
					});
				}
				throw new JsonException(string.Format("Can't assign value '{0}' (type {1}) to type {2}", reader.Value, type, inst_type));
			}
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x000B5AC0 File Offset: 0x000B3EC0
		private static IJsonWrapper ReadValue(WrapperFactory factory, JsonReader reader)
		{
			reader.Read();
			if (reader.Token == JsonToken.ArrayEnd || reader.Token == JsonToken.Null)
			{
				return null;
			}
			IJsonWrapper jsonWrapper = factory();
			if (reader.Token == JsonToken.String)
			{
				jsonWrapper.SetString((string)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Double)
			{
				jsonWrapper.SetDouble((double)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Int)
			{
				jsonWrapper.SetInt((int)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Long)
			{
				jsonWrapper.SetLong((long)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Boolean)
			{
				jsonWrapper.SetBoolean((bool)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.ArrayStart)
			{
				jsonWrapper.SetJsonType(JsonType.Array);
				for (;;)
				{
					IJsonWrapper jsonWrapper2 = JsonMapper.ReadValue(factory, reader);
					if (jsonWrapper2 == null && reader.Token == JsonToken.ArrayEnd)
					{
						break;
					}
					jsonWrapper.Add(jsonWrapper2);
				}
			}
			else if (reader.Token == JsonToken.ObjectStart)
			{
				jsonWrapper.SetJsonType(JsonType.Object);
				for (;;)
				{
					reader.Read();
					if (reader.Token == JsonToken.ObjectEnd)
					{
						break;
					}
					string key = (string)reader.Value;
					jsonWrapper[key] = JsonMapper.ReadValue(factory, reader);
				}
			}
			return jsonWrapper;
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x000B5C22 File Offset: 0x000B4022
		private static void ReadSkip(JsonReader reader)
		{
			JsonMapper.ToWrapper(() => new JsonMockWrapper(), reader);
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x000B5C48 File Offset: 0x000B4048
		private static void RegisterBaseExporters()
		{
			JsonMapper.base_exporters_table[typeof(byte)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToInt32((byte)obj));
			};
			JsonMapper.base_exporters_table[typeof(char)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToString((char)obj));
			};
			JsonMapper.base_exporters_table[typeof(DateTime)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToString((DateTime)obj, JsonMapper.datetime_format));
			};
			JsonMapper.base_exporters_table[typeof(decimal)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write((decimal)obj);
			};
			JsonMapper.base_exporters_table[typeof(sbyte)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToInt32((sbyte)obj));
			};
			JsonMapper.base_exporters_table[typeof(short)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToInt32((short)obj));
			};
			JsonMapper.base_exporters_table[typeof(ushort)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToInt32((ushort)obj));
			};
			JsonMapper.base_exporters_table[typeof(uint)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToUInt64((uint)obj));
			};
			JsonMapper.base_exporters_table[typeof(ulong)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write((ulong)obj);
			};
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x000B5E10 File Offset: 0x000B4210
		private static void RegisterBaseImporters()
		{
			ImporterFunc importer = (object input) => Convert.ToByte((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(byte), importer);
			importer = ((object input) => Convert.ToUInt64((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(ulong), importer);
			importer = ((object input) => Convert.ToSByte((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(sbyte), importer);
			importer = ((object input) => Convert.ToInt16((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(short), importer);
			importer = ((object input) => Convert.ToUInt16((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(ushort), importer);
			importer = ((object input) => Convert.ToUInt32((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(uint), importer);
			importer = ((object input) => Convert.ToSingle((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(float), importer);
			importer = ((object input) => Convert.ToDouble((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(double), importer);
			importer = ((object input) => Convert.ToDecimal((double)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(double), typeof(decimal), importer);
			importer = ((object input) => Convert.ToUInt32((long)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(long), typeof(uint), importer);
			importer = ((object input) => Convert.ToChar((string)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(string), typeof(char), importer);
			importer = ((object input) => Convert.ToDateTime((string)input, JsonMapper.datetime_format));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(string), typeof(DateTime), importer);
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x000B60F9 File Offset: 0x000B44F9
		private static void RegisterImporter(IDictionary<Type, IDictionary<Type, ImporterFunc>> table, Type json_type, Type value_type, ImporterFunc importer)
		{
			if (!table.ContainsKey(json_type))
			{
				table.Add(json_type, new Dictionary<Type, ImporterFunc>());
			}
			table[json_type][value_type] = importer;
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x000B6124 File Offset: 0x000B4524
		private static void WriteValue(object obj, JsonWriter writer, bool writer_is_private, int depth)
		{
			if (depth > JsonMapper.max_nesting_depth)
			{
				throw new JsonException(string.Format("Max allowed object depth reached while trying to export from type {0}", obj.GetType()));
			}
			if (obj == null)
			{
				writer.Write(null);
				return;
			}
			if (obj is IJsonWrapper)
			{
				if (writer_is_private)
				{
					writer.TextWriter.Write(((IJsonWrapper)obj).ToJson());
				}
				else
				{
					((IJsonWrapper)obj).ToJson(writer);
				}
				return;
			}
			if (obj is string)
			{
				writer.Write((string)obj);
				return;
			}
			if (obj is double)
			{
				writer.Write((double)obj);
				return;
			}
			if (obj is int)
			{
				writer.Write((int)obj);
				return;
			}
			if (obj is bool)
			{
				writer.Write((bool)obj);
				return;
			}
			if (obj is long)
			{
				writer.Write((long)obj);
				return;
			}
			if (obj is Array)
			{
				writer.WriteArrayStart();
				IEnumerator enumerator = ((Array)obj).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						JsonMapper.WriteValue(obj2, writer, writer_is_private, depth + 1);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				writer.WriteArrayEnd();
				return;
			}
			if (obj is IList)
			{
				writer.WriteArrayStart();
				IEnumerator enumerator2 = ((IList)obj).GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj3 = enumerator2.Current;
						JsonMapper.WriteValue(obj3, writer, writer_is_private, depth + 1);
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator2 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
				writer.WriteArrayEnd();
				return;
			}
			if (obj is IDictionary)
			{
				writer.WriteObjectStart();
				IDictionaryEnumerator enumerator3 = ((IDictionary)obj).GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						object obj4 = enumerator3.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj4;
						writer.WritePropertyName((string)dictionaryEntry.Key);
						JsonMapper.WriteValue(dictionaryEntry.Value, writer, writer_is_private, depth + 1);
					}
				}
				finally
				{
					IDisposable disposable3;
					if ((disposable3 = (enumerator3 as IDisposable)) != null)
					{
						disposable3.Dispose();
					}
				}
				writer.WriteObjectEnd();
				return;
			}
			Type type = obj.GetType();
			if (JsonMapper.custom_exporters_table.ContainsKey(type))
			{
				ExporterFunc exporterFunc = JsonMapper.custom_exporters_table[type];
				exporterFunc(obj, writer);
				return;
			}
			if (JsonMapper.base_exporters_table.ContainsKey(type))
			{
				ExporterFunc exporterFunc2 = JsonMapper.base_exporters_table[type];
				exporterFunc2(obj, writer);
				return;
			}
			if (obj is Enum)
			{
				Type underlyingType = Enum.GetUnderlyingType(type);
				if (underlyingType == typeof(long) || underlyingType == typeof(uint) || underlyingType == typeof(ulong))
				{
					writer.Write((ulong)obj);
				}
				else
				{
					writer.Write((int)obj);
				}
				return;
			}
			JsonMapper.AddTypeProperties(type);
			IList<PropertyMetadata> list = JsonMapper.type_properties[type];
			writer.WriteObjectStart();
			foreach (PropertyMetadata propertyMetadata in list)
			{
				if (propertyMetadata.IsField)
				{
					writer.WritePropertyName(propertyMetadata.Info.Name);
					JsonMapper.WriteValue(((FieldInfo)propertyMetadata.Info).GetValue(obj), writer, writer_is_private, depth + 1);
				}
				else
				{
					PropertyInfo propertyInfo = (PropertyInfo)propertyMetadata.Info;
					if (propertyInfo.CanRead)
					{
						writer.WritePropertyName(propertyMetadata.Info.Name);
						JsonMapper.WriteValue(propertyInfo.GetValue(obj, null), writer, writer_is_private, depth + 1);
					}
				}
			}
			writer.WriteObjectEnd();
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x000B6524 File Offset: 0x000B4924
		public static string ToJson(object obj)
		{
			object obj2 = JsonMapper.static_writer_lock;
			string result;
			lock (obj2)
			{
				JsonMapper.static_writer.Reset();
				JsonMapper.WriteValue(obj, JsonMapper.static_writer, true, 0);
				result = JsonMapper.static_writer.ToString();
			}
			return result;
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x000B657C File Offset: 0x000B497C
		public static void ToJson(object obj, JsonWriter writer)
		{
			JsonMapper.WriteValue(obj, writer, false, 0);
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x000B6587 File Offset: 0x000B4987
		public static JsonData ToObject(JsonReader reader)
		{
			return (JsonData)JsonMapper.ToWrapper(() => new JsonData(), reader);
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x000B65B4 File Offset: 0x000B49B4
		public static JsonData ToObject(TextReader reader)
		{
			JsonReader reader2 = new JsonReader(reader);
			return (JsonData)JsonMapper.ToWrapper(() => new JsonData(), reader2);
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x000B65F0 File Offset: 0x000B49F0
		public static JsonData ToObject(string json)
		{
			return (JsonData)JsonMapper.ToWrapper(() => new JsonData(), json);
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x000B661A File Offset: 0x000B4A1A
		public static T ToObject<T>(JsonReader reader)
		{
			return (T)((object)JsonMapper.ReadValue(typeof(T), reader));
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x000B6634 File Offset: 0x000B4A34
		public static T ToObject<T>(TextReader reader)
		{
			JsonReader reader2 = new JsonReader(reader);
			return (T)((object)JsonMapper.ReadValue(typeof(T), reader2));
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x000B6660 File Offset: 0x000B4A60
		public static T ToObject<T>(string json)
		{
			JsonReader reader = new JsonReader(json);
			return (T)((object)JsonMapper.ReadValue(typeof(T), reader));
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x000B6689 File Offset: 0x000B4A89
		public static IJsonWrapper ToWrapper(WrapperFactory factory, JsonReader reader)
		{
			return JsonMapper.ReadValue(factory, reader);
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x000B6694 File Offset: 0x000B4A94
		public static IJsonWrapper ToWrapper(WrapperFactory factory, string json)
		{
			JsonReader reader = new JsonReader(json);
			return JsonMapper.ReadValue(factory, reader);
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x000B66B0 File Offset: 0x000B4AB0
		public static void RegisterExporter<T>(ExporterFunc<T> exporter)
		{
			ExporterFunc value = delegate(object obj, JsonWriter writer)
			{
				exporter((T)((object)obj), writer);
			};
			JsonMapper.custom_exporters_table[typeof(T)] = value;
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x000B66EC File Offset: 0x000B4AEC
		public static void RegisterImporter<TJson, TValue>(ImporterFunc<TJson, TValue> importer)
		{
			ImporterFunc importer2 = (object input) => importer((TJson)((object)input));
			JsonMapper.RegisterImporter(JsonMapper.custom_importers_table, typeof(TJson), typeof(TValue), importer2);
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x000B6732 File Offset: 0x000B4B32
		public static void UnregisterExporters()
		{
			JsonMapper.custom_exporters_table.Clear();
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x000B673E File Offset: 0x000B4B3E
		public static void UnregisterImporters()
		{
			JsonMapper.custom_importers_table.Clear();
		}

		// Token: 0x04001229 RID: 4649
		private static int max_nesting_depth;

		// Token: 0x0400122A RID: 4650
		private static IFormatProvider datetime_format;

		// Token: 0x0400122B RID: 4651
		private static IDictionary<Type, ExporterFunc> base_exporters_table;

		// Token: 0x0400122C RID: 4652
		private static IDictionary<Type, ExporterFunc> custom_exporters_table;

		// Token: 0x0400122D RID: 4653
		private static IDictionary<Type, IDictionary<Type, ImporterFunc>> base_importers_table;

		// Token: 0x0400122E RID: 4654
		private static IDictionary<Type, IDictionary<Type, ImporterFunc>> custom_importers_table;

		// Token: 0x0400122F RID: 4655
		private static IDictionary<Type, ArrayMetadata> array_metadata;

		// Token: 0x04001230 RID: 4656
		private static readonly object array_metadata_lock = new object();

		// Token: 0x04001231 RID: 4657
		private static IDictionary<Type, IDictionary<Type, MethodInfo>> conv_ops;

		// Token: 0x04001232 RID: 4658
		private static readonly object conv_ops_lock = new object();

		// Token: 0x04001233 RID: 4659
		private static IDictionary<Type, ObjectMetadata> object_metadata;

		// Token: 0x04001234 RID: 4660
		private static readonly object object_metadata_lock = new object();

		// Token: 0x04001235 RID: 4661
		private static IDictionary<Type, IList<PropertyMetadata>> type_properties;

		// Token: 0x04001236 RID: 4662
		private static readonly object type_properties_lock = new object();

		// Token: 0x04001237 RID: 4663
		private static JsonWriter static_writer;

		// Token: 0x04001238 RID: 4664
		private static readonly object static_writer_lock = new object();
	}
}
