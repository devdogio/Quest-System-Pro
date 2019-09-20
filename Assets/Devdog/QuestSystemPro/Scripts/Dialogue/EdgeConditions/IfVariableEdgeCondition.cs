using System;
using System.Collections.Generic;

namespace Devdog.QuestSystemPro.Dialogue
{
    public class IfVariableEdgeCondition<T> : SimpleEdgeCondition
    {
        public enum FilterType
        {
            Equal,
            NotEqual,
            GreaterThan,
            LessThan
        }

        public VariableRef<T> variable = new VariableRef<T>();
        public T value;
        public FilterType filterType = FilterType.Equal;

        public override bool CanUse(Dialogue dialogue)
        {
            switch (filterType)
            {
                case FilterType.Equal:
                    return EqualityComparer<T>.Default.Equals(variable.value, value);
                case FilterType.NotEqual:
                    return EqualityComparer<T>.Default.Equals(variable.value, value) == false;
                case FilterType.GreaterThan:
                    return Comparer<T>.Default.Compare(variable.value, value) > 0;
                case FilterType.LessThan:
                    return Comparer<T>.Default.Compare(variable.value, value) < 0;
                default:
                    break;
            }

            return false;
        }

        public override string FormattedString()
        {
            if(variable.guid != Guid.Empty)
            {
                return "Var \"" + variable.variable.name + "\" " + FilterTypeToString(filterType) + " " + value;
            }

            return "<var compare>";
        }

        private string FilterTypeToString(FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.Equal:
                    return "==";
                case FilterType.NotEqual:
                    return "!=";
                case FilterType.GreaterThan:
                    return ">=";
                case FilterType.LessThan:
                    return "<=";
                default:
                    break;
            }

            return "<?>";
        }
    }
}
