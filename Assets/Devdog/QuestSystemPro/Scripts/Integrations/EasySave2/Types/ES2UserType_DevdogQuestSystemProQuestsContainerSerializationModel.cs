#if EASY_SAVE_2

public class ES2UserType_DevdogQuestSystemProQuestsContainerSerializationModel : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Devdog.QuestSystemPro.QuestsContainerSerializationModel data = (Devdog.QuestSystemPro.QuestsContainerSerializationModel)obj;
		
        // Add your writer.Write calls here.
		writer.Write(data.activeQuests);
		writer.Write(data.completedQuests);
		writer.Write(data.achievements);
	}
	
	public override object Read(ES2Reader reader)
	{
		Devdog.QuestSystemPro.QuestsContainerSerializationModel data = new Devdog.QuestSystemPro.QuestsContainerSerializationModel();
		Read(reader, data);
		return data;
	}

	public override void Read(ES2Reader reader, object c)
	{
		Devdog.QuestSystemPro.QuestsContainerSerializationModel data = (Devdog.QuestSystemPro.QuestsContainerSerializationModel)c;
		
        // Add your reader.Read calls here to read the data into the object.
		data.activeQuests = reader.ReadArray<Devdog.QuestSystemPro.QuestSerializationModel>();
		data.completedQuests = reader.ReadArray<Devdog.QuestSystemPro.QuestSerializationModel>();
		data.achievements = reader.ReadArray<Devdog.QuestSystemPro.QuestSerializationModel>();
	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_DevdogQuestSystemProQuestsContainerSerializationModel():base(typeof(Devdog.QuestSystemPro.QuestsContainerSerializationModel)){}
}

#endif