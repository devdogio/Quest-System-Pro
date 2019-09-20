using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    public class FakeEdgeCondition : IEdgeCondition
    {
        public bool CanViewEndNode(Dialogue dialogue)
        {
            return false;
        }

        public bool CanUse(Dialogue dialogue)
        {
            return false;
        }

        public ValidationInfo Validate(Dialogue dialogue)
        {
            return new ValidationInfo(ValidationType.Valid);
        }

        public string FormattedString()
        {
            return "Fake condition";
        }
    }
}
