#if EASY_SAVE_2

using System;

public class ES2UserType_DevdogQuestSystemProTaskSerializationModel : ES2Type
{
    protected const string DateTimePattern = "yyyy-MM-dd HH:mm:ss";

    public override void Write(object obj, ES2Writer writer)
	{
		Devdog.QuestSystemPro.TaskSerializationModel data = (Devdog.QuestSystemPro.TaskSerializationModel)obj;
		
        // Add your writer.Write calls here.
		writer.Write(data.key);
		writer.Write(data.progress);
		writer.Write(data.status);
		writer.Write(data.gaveRewards);
	    writer.Write(data.startTime.HasValue ? data.startTime.Value : DateTime.MinValue);
	}

    public override object Read(ES2Reader reader)
	{
		Devdog.QuestSystemPro.TaskSerializationModel data = new Devdog.QuestSystemPro.TaskSerializationModel();
		Read(reader, data);
		return data;
	}

	public override void Read(ES2Reader reader, object c)
	{
		Devdog.QuestSystemPro.TaskSerializationModel data = (Devdog.QuestSystemPro.TaskSerializationModel)c;
		
        // Add your reader.Read calls here to read the data into the object.
		data.key = reader.Read<System.String>();
		data.progress = reader.Read<System.Single>();
		data.status = reader.Read<Devdog.QuestSystemPro.TaskStatus>();
		data.gaveRewards = reader.Read<System.Boolean>();
        data.startTime = reader.Read<System.DateTime>();
	    if (data.startTime.Value.ToString() == System.DateTime.MinValue.ToString())
	    {
	        data.startTime = null; // Clear value
	    }
    }

    /* ! Don't modify anything below this line ! */
    public ES2UserType_DevdogQuestSystemProTaskSerializationModel():base(typeof(Devdog.QuestSystemPro.TaskSerializationModel)){}
}

#endif