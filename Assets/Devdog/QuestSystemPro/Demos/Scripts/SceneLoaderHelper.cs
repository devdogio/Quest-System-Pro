using UnityEngine;
using System.Collections;
using Devdog.General;

namespace Devdog.QuestSystemPro.Demo
{
    public class SceneLoaderHelper : MonoBehaviour
    {
        public void LoadScene(string name)
        {
            SceneUtility.LoadScene(name);
        }

        public void LoadScene(int id)
        {
            SceneUtility.LoadScene(id);
        }
    }
}