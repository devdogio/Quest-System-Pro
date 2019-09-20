using System;
using System.Collections.Generic;

namespace Devdog.General.ThirdParty.FullSerializer {
    partial class fsConverterRegistrar {
        public static Speedup.Devdog_QuestSystemPro_Dialogue_PlayerDecision_DirectConverter Register_Devdog_QuestSystemPro_Dialogue_PlayerDecision;
    }
}

namespace Devdog.General.ThirdParty.FullSerializer.Speedup {
    public class Devdog_QuestSystemPro_Dialogue_PlayerDecision_DirectConverter : fsDirectConverter<Devdog.QuestSystemPro.Dialogue.PlayerDecision> {
        protected override fsResult DoSerialize(Devdog.QuestSystemPro.Dialogue.PlayerDecision model, Dictionary<string, fsData> serialized) {
            var result = fsResult.Success;

            result += SerializeMember(serialized, null, "option", model.option);

            return result;
        }

        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Devdog.QuestSystemPro.Dialogue.PlayerDecision model) {
            var result = fsResult.Success;

            var t0 = model.option;
            result += DeserializeMember(data, null, "option", out t0);
            model.option = t0;

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType) {
            return new Devdog.QuestSystemPro.Dialogue.PlayerDecision();
        }
    }
}
