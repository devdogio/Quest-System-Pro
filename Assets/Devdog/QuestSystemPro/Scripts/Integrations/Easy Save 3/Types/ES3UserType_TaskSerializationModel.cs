#if EASY_SAVE_3
using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("key", "progress", "startTime", "status", "gaveRewards")]
	public class ES3UserType_TaskSerializationModel : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_TaskSerializationModel() : base(typeof(Devdog.QuestSystemPro.TaskSerializationModel)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Devdog.QuestSystemPro.TaskSerializationModel)obj;
			
			writer.WriteProperty("key", instance.key, ES3Type_string.Instance);
			writer.WriteProperty("progress", instance.progress, ES3Type_float.Instance);
			writer.WriteProperty("startTime", instance.startTime);
			writer.WriteProperty("status", instance.status);
			writer.WriteProperty("gaveRewards", instance.gaveRewards, ES3Type_bool.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Devdog.QuestSystemPro.TaskSerializationModel)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "key":
						instance.key = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "progress":
						instance.progress = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "startTime":
						instance.startTime = reader.Read<System.Nullable<System.DateTime>>();
						break;
					case "status":
						instance.status = reader.Read<Devdog.QuestSystemPro.TaskStatus>();
						break;
					case "gaveRewards":
						instance.gaveRewards = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Devdog.QuestSystemPro.TaskSerializationModel();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_TaskSerializationModelArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_TaskSerializationModelArray() : base(typeof(Devdog.QuestSystemPro.TaskSerializationModel[]), ES3UserType_TaskSerializationModel.Instance)
		{
			Instance = this;
		}
	}
}
#endif