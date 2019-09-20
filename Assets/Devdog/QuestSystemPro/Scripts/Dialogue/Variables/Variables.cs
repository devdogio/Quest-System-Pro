using System;
using System.Collections.Generic;
using Devdog.General;

namespace Devdog.QuestSystemPro.Dialogue
{
    public static class Variables
    {
        public static List<VariablesContainer> variableContainers = new List<VariablesContainer>();

        public static Variable Get(Guid g)
        {
            //DevdogLogger.LogVerbose("Getting variable with GUID: " + g + " Looking through " + variableContainers.Count + " containers.");
            foreach (var container in variableContainers)
            {
                var v = container.Get(g);
                if (v != null)
                {
                    return v;
                }
            }

            DevdogLogger.LogWarning("Trying to get variable with GUID " + g + " - Not found.");
            return null;
        }

        public static Variable<T> Get<T>(Guid g)
        {
            return (Variable<T>)Get(g);
        }


//        public static Variable Get(string name, Dialogue dialogue)
//        {
//            foreach (var variable in variables)
//            {
//                if (variable.name == name && variable.dialogue == dialogue)
//                {
//                    return variable;
//                }
//            }
//
//            DevdogLogger.Log("Trying to get variable with name " + name + " - Not found.");
//            return null;
//        }
//
//        public static T Get<T>(string name, Dialogue dialogue) where T : Variable
//        {
//            return (T)Get(name, dialogue);
//        }
    }
}