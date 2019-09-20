using System;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    public class Variable<T> : Variable
    {
        [NonSerialized]
        public T value;

        public Variable()
            : base()
        {

        }

        public Variable(T value)
            : base()
        {
            this.value = value;
        }

        public override string ToString()
        {
            if (value != null)
            {
                return value.ToString();
            }

            return "null (" + typeof(T).Name + ")";
        }

//        public override void SetValueBoxed(object value)
//        {
//            if(value != null)
//            {
//                Assert.IsTrue(value is T, "Given (boxed) value is not of right type! Given type: " + value.GetType() + " Variable type: " + typeof(T).Name);
//            }
//
//            this.value = (T) value;
//        }
    }

    [System.Serializable]
    public abstract class Variable
    {
        [HideInInspector]
        public readonly Guid guid;

        [Required]
        public string name;

        [IgnoreCustomSerialization]
        public Dialogue dialogue { get; set; }

        protected Variable()
        {
            this.guid = Guid.NewGuid();
        }

//        public abstract void SetValueBoxed(object value);
    }
}