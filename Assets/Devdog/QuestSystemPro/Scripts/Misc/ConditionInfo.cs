using System;
using Devdog.General;

namespace Devdog.QuestSystemPro
{
    public struct ConditionInfo : IEquatable<ConditionInfo>
    {
        public readonly bool status;
        public readonly MultiLangString message;
        public object[] vars;

        public static ConditionInfo success
        {
            get
            {
                return new ConditionInfo(true);
            }
        }

        public ConditionInfo(bool conditionStatus, MultiLangString conditionMessage = null)
            : this(conditionStatus, conditionMessage, new object[0])
        { }

        public ConditionInfo(bool conditionStatus, MultiLangString conditionMessage, params object[] vars)
        {
            status = conditionStatus;
            message = conditionMessage;
            this.vars = vars;
        }

        public static bool operator ==(ConditionInfo a, bool b)
        {
            return a.status == b;
        }

        public static bool operator !=(ConditionInfo a, bool b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return MessageToString();
        }

        public string TitleToString()
        {
            if (message == null)
                return string.Empty;

            return message.TitleToString(vars);
        }

        public string MessageToString()
        {
            if (message == null)
                return string.Empty;

            return message.MessageToString(vars);
        }

        public bool Equals(ConditionInfo other)
        {
            return status == other.status && Equals(message, other.message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ConditionInfo && Equals((ConditionInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (status.GetHashCode() * 397) ^ (message != null ? message.GetHashCode() : 0);
            }
        }
    }
}
