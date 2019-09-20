using System;
using Devdog.General;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro.Dialogue
{
    /// <summary>
    /// A reference to a variable. Note variable reference CAN be null!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class VariableRef<T> : VariableRef
    {
        private Variable<T> _variable;
        [IgnoreCustomSerialization]
        public Variable<T> variable
        {
            get
            {
                if (_variable == null)
                {
                    _variable = Variables.Get<T>(guid);
                }

                return _variable;
            }
        }

        [IgnoreCustomSerialization]
        public T value
        {
            get
            {
                Assert.IsNotNull(variable, "Trying to get variable value, but variable is null! - Make sure you assign it in the editor first.");
                return variable.value;
            }
            set
            {
                Assert.IsNotNull(variable, "Trying to set variable value, but variable is null! - Make sure you assign it in the editor first.");
                variable.value = value;
            }
        }


        public VariableRef()
        {

        }

        public override string ToString()
        {
            if (variable != null)
            {
                return variable.ToString();
            }

            return "null (" + typeof(T).Name + ")";
        }
    }

    [System.Serializable]
    public class VariableRef
    {
        public Guid guid;
    }
}