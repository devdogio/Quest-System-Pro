using System;
using System.Collections.Generic;

namespace Devdog.General.ThirdParty.FullSerializer {
    partial class fsConverterRegistrar {
        public static Speedup.Devdog_QuestSystemPro_Dialogue_VariableRef_string__DirectConverter Register_Devdog_QuestSystemPro_Dialogue_VariableRef_string_;
    }
}

namespace Devdog.General.ThirdParty.FullSerializer.Speedup {
    public class Devdog_QuestSystemPro_Dialogue_VariableRef_string__DirectConverter : fsDirectConverter<Devdog.QuestSystemPro.Dialogue.VariableRef<string>> {
        protected override fsResult DoSerialize(Devdog.QuestSystemPro.Dialogue.VariableRef<string> model, Dictionary<string, fsData> serialized) {
            var result = fsResult.Success;

            result += SerializeMember(serialized, null, "guid", model.guid);

            return result;
        }

        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Devdog.QuestSystemPro.Dialogue.VariableRef<string> model) {
            var result = fsResult.Success;

            var t0 = model.guid;
            result += DeserializeMember(data, null, "guid", out t0);
            model.guid = t0;

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType) {
            return new Devdog.QuestSystemPro.Dialogue.VariableRef<string>();
        }
    }
}
