using System;
using System.Collections.Generic;

namespace Devdog.General.ThirdParty.FullSerializer {
    partial class fsConverterRegistrar {
        public static Speedup.Devdog_QuestSystemPro_Dialogue_CameraPositionLookup_DirectConverter Register_Devdog_QuestSystemPro_Dialogue_CameraPositionLookup;
    }
}

namespace Devdog.General.ThirdParty.FullSerializer.Speedup {
    public class Devdog_QuestSystemPro_Dialogue_CameraPositionLookup_DirectConverter : fsDirectConverter<Devdog.QuestSystemPro.Dialogue.CameraPositionLookup> {
        protected override fsResult DoSerialize(Devdog.QuestSystemPro.Dialogue.CameraPositionLookup model, Dictionary<string, fsData> serialized) {
            var result = fsResult.Success;

            result += SerializeMember(serialized, null, "from", model.from);
            result += SerializeMember(serialized, null, "to", model.to);
            result += SerializeMember(serialized, null, "duration", model.duration);
            result += SerializeMember(serialized, null, "animationCurve", model.animationCurve);

            return result;
        }

        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Devdog.QuestSystemPro.Dialogue.CameraPositionLookup model) {
            var result = fsResult.Success;

            var t0 = model.from;
            result += DeserializeMember(data, null, "from", out t0);
            model.from = t0;

            var t1 = model.to;
            result += DeserializeMember(data, null, "to", out t1);
            model.to = t1;

            var t2 = model.duration;
            result += DeserializeMember(data, null, "duration", out t2);
            model.duration = t2;

            var t3 = model.animationCurve;
            result += DeserializeMember(data, null, "animationCurve", out t3);
            model.animationCurve = t3;

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType) {
            return new Devdog.QuestSystemPro.Dialogue.CameraPositionLookup();
        }
    }
}
