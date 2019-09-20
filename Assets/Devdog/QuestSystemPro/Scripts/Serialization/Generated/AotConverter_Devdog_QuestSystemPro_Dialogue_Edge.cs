using System;
using System.Collections.Generic;

namespace Devdog.General.ThirdParty.FullSerializer {
    partial class fsConverterRegistrar {
        public static Speedup.Devdog_QuestSystemPro_Dialogue_Edge_DirectConverter Register_Devdog_QuestSystemPro_Dialogue_Edge;
    }
}

namespace Devdog.General.ThirdParty.FullSerializer.Speedup {
    public class Devdog_QuestSystemPro_Dialogue_Edge_DirectConverter : fsDirectConverter<Devdog.QuestSystemPro.Dialogue.Edge> {
        protected override fsResult DoSerialize(Devdog.QuestSystemPro.Dialogue.Edge model, Dictionary<string, fsData> serialized) {
            var result = fsResult.Success;

            result += SerializeMember(serialized, null, "toNodeIndex", model.toNodeIndex);
            result += SerializeMember(serialized, null, "conditions", model.conditions);

            return result;
        }

        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Devdog.QuestSystemPro.Dialogue.Edge model) {
            var result = fsResult.Success;

            var t0 = model.toNodeIndex;
            result += DeserializeMember(data, null, "toNodeIndex", out t0);
            model.toNodeIndex = t0;

            var t1 = model.conditions;
            result += DeserializeMember(data, null, "conditions", out t1);
            model.conditions = t1;

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType) {
            return new Devdog.QuestSystemPro.Dialogue.Edge();
        }
    }
}
