#if EASY_SAVE_3
using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("ID", "repeatedTimes", "status", "tasks")]
	public class ES3UserType_QuestSerializationModel : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_QuestSerializationModel() : base(typeof(Devdog.QuestSystemPro.QuestSerializationModel)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Devdog.QuestSystemPro.QuestSerializationModel)obj;
			
			writer.WriteProperty("ID", instance.ID, ES3Type_int.Instance);
			writer.WriteProperty("repeatedTimes", instance.repeatedTimes, ES3Type_int.Instance);
			writer.WriteProperty("status", instance.status);
			writer.WriteProperty("tasks", instance.tasks);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Devdog.QuestSystemPro.QuestSerializationModel)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "ID":
						instance.ID = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "repeatedTimes":
						instance.repeatedTimes = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "status":
						instance.status = reader.Read<Devdog.QuestSystemPro.QuestStatus>();
						break;
					case "tasks":
						instance.tasks = reader.Read<Devdog.QuestSystemPro.TaskSerializationModel[]>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Devdog.QuestSystemPro.QuestSerializationModel();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_QuestSerializationModelArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_QuestSerializationModelArray() : base(typeof(Devdog.QuestSystemPro.QuestSerializationModel[]), ES3UserType_QuestSerializationModel.Instance)
		{
			Instance = this;
		}
	}
}
#endif