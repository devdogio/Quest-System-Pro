using UnityEngine;
using System.Collections;
using Devdog.QuestSystemPro;

public class ReloadQuestsTest : MonoBehaviour {


	private void ReloadQuests()
	{
		foreach (var quest in QuestManager.instance.quests)
		{
			quest.ResetProgress();
		}

//		Resources.LoadAll("Assets/Devdog/QuestSystemPro/Demos/Files", typeof(Quest));
	}
	
	void OnGUI()
	{
		if (GUILayout.Button("Reload quests"))
		{
			ReloadQuests();
		}
	}
}
