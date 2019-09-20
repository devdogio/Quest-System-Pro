using System;
using System.Collections.Generic;

namespace Devdog.General.ThirdParty.FullSerializer {
    partial class fsConverterRegistrar {
        public static Speedup.Devdog_QuestSystemPro_TaskOverAchievement_DirectConverter Register_Devdog_QuestSystemPro_TaskOverAchievement;
    }
}

namespace Devdog.General.ThirdParty.FullSerializer.Speedup {
    public class Devdog_QuestSystemPro_TaskOverAchievement_DirectConverter : fsDirectConverter<Devdog.QuestSystemPro.TaskOverAchievement> {
        protected override fsResult DoSerialize(Devdog.QuestSystemPro.TaskOverAchievement model, Dictionary<string, fsData> serialized) {
            var result = fsResult.Success;

            result += SerializeMember(serialized, null, "from", model.from);
            result += SerializeMember(serialized, null, "to", model.to);
            result += SerializeMember(serialized, null, "rewardGivers", model.rewardGivers);

            return result;
        }

        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Devdog.QuestSystemPro.TaskOverAchievement model) {
            var result = fsResult.Success;

            var t0 = model.from;
            result += DeserializeMember(data, null, "from", out t0);
            model.from = t0;

            var t1 = model.to;
            result += DeserializeMember(data, null, "to", out t1);
            model.to = t1;

            var t2 = model.rewardGivers;
            result += DeserializeMember(data, null, "rewardGivers", out t2);
            model.rewardGivers = t2;

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType) {
            return new Devdog.QuestSystemPro.TaskOverAchievement();
        }
    }
}
