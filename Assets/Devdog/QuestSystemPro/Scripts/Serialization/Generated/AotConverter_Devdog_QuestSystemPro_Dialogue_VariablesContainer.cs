using System;
using System.Collections.Generic;

namespace Devdog.General.ThirdParty.FullSerializer {
    partial class fsConverterRegistrar {
        public static Speedup.Devdog_QuestSystemPro_Dialogue_VariablesContainer_DirectConverter Register_Devdog_QuestSystemPro_Dialogue_VariablesContainer;
    }
}

namespace Devdog.General.ThirdParty.FullSerializer.Speedup {
    public class Devdog_QuestSystemPro_Dialogue_VariablesContainer_DirectConverter : fsDirectConverter<Devdog.QuestSystemPro.Dialogue.VariablesContainer> {
        protected override fsResult DoSerialize(Devdog.QuestSystemPro.Dialogue.VariablesContainer model, Dictionary<string, fsData> serialized) {
            var result = fsResult.Success;

            result += SerializeMember(serialized, null, "variables", model.variables);

            return result;
        }

        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Devdog.QuestSystemPro.Dialogue.VariablesContainer model) {
            var result = fsResult.Success;

            var t0 = model.variables;
            result += DeserializeMember(data, null, "variables", out t0);
            model.variables = t0;

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType) {
            return new Devdog.QuestSystemPro.Dialogue.VariablesContainer();
        }
    }
}
