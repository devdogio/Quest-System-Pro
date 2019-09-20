using System;
using System.Collections.Generic;

namespace Devdog.General.ThirdParty.FullSerializer {
    partial class fsConverterRegistrar {
        public static Speedup.Devdog_QuestSystemPro_QuestTimeHandler_DirectConverter Register_Devdog_QuestSystemPro_QuestTimeHandler;
    }
}

namespace Devdog.General.ThirdParty.FullSerializer.Speedup {
    public class Devdog_QuestSystemPro_QuestTimeHandler_DirectConverter : fsDirectConverter<Devdog.QuestSystemPro.QuestTimeHandler> {
        protected override fsResult DoSerialize(Devdog.QuestSystemPro.QuestTimeHandler model, Dictionary<string, fsData> serialized) {
            var result = fsResult.Success;

            result += SerializeMember(serialized, null, "failQuestWhenOutOfTime", model.failQuestWhenOutOfTime);

            return result;
        }

        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Devdog.QuestSystemPro.QuestTimeHandler model) {
            var result = fsResult.Success;

            var t0 = model.failQuestWhenOutOfTime;
            result += DeserializeMember(data, null, "failQuestWhenOutOfTime", out t0);
            model.failQuestWhenOutOfTime = t0;

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType) {
            return new Devdog.QuestSystemPro.QuestTimeHandler();
        }
    }
}
