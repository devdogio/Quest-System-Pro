#if EASY_SAVE_3
using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("activeQuests", "completedQuests", "achievements")]
	public class ES3UserType_QuestsContainerSerializationModel : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_QuestsContainerSerializationModel() : base(typeof(Devdog.QuestSystemPro.QuestsContainerSerializationModel)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Devdog.QuestSystemPro.QuestsContainerSerializationModel)obj;
			
			writer.WriteProperty("activeQuests", instance.activeQuests);
			writer.WriteProperty("completedQuests", instance.completedQuests);
			writer.WriteProperty("achievements", instance.achievements);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Devdog.QuestSystemPro.QuestsContainerSerializationModel)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "activeQuests":
						instance.activeQuests = reader.Read<Devdog.QuestSystemPro.QuestSerializationModel[]>();
						break;
					case "completedQuests":
						instance.completedQuests = reader.Read<Devdog.QuestSystemPro.QuestSerializationModel[]>();
						break;
					case "achievements":
						instance.achievements = reader.Read<Devdog.QuestSystemPro.QuestSerializationModel[]>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Devdog.QuestSystemPro.QuestsContainerSerializationModel();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_QuestsContainerSerializationModelArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_QuestsContainerSerializationModelArray() : base(typeof(Devdog.QuestSystemPro.QuestsContainerSerializationModel[]), ES3UserType_QuestsContainerSerializationModel.Instance)
		{
			Instance = this;
		}
	}
}
#endif