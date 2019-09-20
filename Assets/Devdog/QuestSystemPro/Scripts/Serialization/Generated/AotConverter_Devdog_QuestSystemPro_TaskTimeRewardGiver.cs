using System;
using System.Collections.Generic;

namespace Devdog.General.ThirdParty.FullSerializer {
    partial class fsConverterRegistrar {
        public static Speedup.Devdog_QuestSystemPro_TaskTimeRewardGiver_DirectConverter Register_Devdog_QuestSystemPro_TaskTimeRewardGiver;
    }
}

namespace Devdog.General.ThirdParty.FullSerializer.Speedup {
    public class Devdog_QuestSystemPro_TaskTimeRewardGiver_DirectConverter : fsDirectConverter<Devdog.QuestSystemPro.TaskTimeRewardGiver> {
        protected override fsResult DoSerialize(Devdog.QuestSystemPro.TaskTimeRewardGiver model, Dictionary<string, fsData> serialized) {
            var result = fsResult.Success;

            result += SerializeMember(serialized, null, "taskName", model.taskName);
            result += SerializeMember(serialized, null, "addTimeInSeconds", model.addTimeInSeconds);

            return result;
        }

        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Devdog.QuestSystemPro.TaskTimeRewardGiver model) {
            var result = fsResult.Success;

            var t0 = model.taskName;
            result += DeserializeMember(data, null, "taskName", out t0);
            model.taskName = t0;

            var t1 = model.addTimeInSeconds;
            result += DeserializeMember(data, null, "addTimeInSeconds", out t1);
            model.addTimeInSeconds = t1;

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType) {
            return new Devdog.QuestSystemPro.TaskTimeRewardGiver();
        }
    }
}
