#if EASY_SAVE_2

public class ES2UserType_DevdogQuestSystemProQuestSerializationModel : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Devdog.QuestSystemPro.QuestSerializationModel data = (Devdog.QuestSystemPro.QuestSerializationModel)obj;
		
        // Add your writer.Write calls here.
		writer.Write(data.ID);
		writer.Write(data.repeatedTimes);
		writer.Write(data.status);
		writer.Write(data.tasks);
	}
	
	public override object Read(ES2Reader reader)
	{
		Devdog.QuestSystemPro.QuestSerializationModel data = new Devdog.QuestSystemPro.QuestSerializationModel();
		Read(reader, data);
		return data;
	}

	public override void Read(ES2Reader reader, object c)
	{
		Devdog.QuestSystemPro.QuestSerializationModel data = (Devdog.QuestSystemPro.QuestSerializationModel)c;
		
        // Add your reader.Read calls here to read the data into the object.
		data.ID = reader.Read<System.Int32>();
		data.repeatedTimes = reader.Read<System.Int32>();
		data.status = reader.Read<Devdog.QuestSystemPro.QuestStatus>();
		data.tasks = reader.ReadArray<Devdog.QuestSystemPro.TaskSerializationModel>();
	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_DevdogQuestSystemProQuestSerializationModel():base(typeof(Devdog.QuestSystemPro.QuestSerializationModel)){}
}

#endif