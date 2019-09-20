using System;

namespace Devdog.QuestSystemPro.Dialogue
{
    public abstract class ActionNodeBase : NodeBase
    {
        [NonSerialized]
        public new bool useAutoFocus = false;

        [NonSerialized]
        public new DialogueOwnerType ownerType;

        [NonSerialized]
        protected new string _message;

        protected ActionNodeBase()
        {
            useAutoFocus = false;
            _message = string.Empty;
        }
    }
}