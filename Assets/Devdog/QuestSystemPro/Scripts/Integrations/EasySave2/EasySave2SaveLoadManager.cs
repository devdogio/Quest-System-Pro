#if EASY_SAVE_2

using UnityEngine;

namespace Devdog.QuestSystemPro.Integration.EasySave2
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Integration/Easy Save 2/Easy Save 2 Save Load Manager")]
    public partial class EasySave2SaveLoadManager : SaveLoadManager
    {
        public bool useEncryption;
        public string encryptionPassword = "L6nbHYmnMMs9REJjNrfC";

        protected override void SaveQuestsContainerModel(string key, QuestsContainerSerializationModel model)
        {
            using (var writer = CreateWriter(key))
            {
                writer.Write<QuestsContainerSerializationModel>(model);
                writer.Save(true);
            }
        }

        protected override QuestsContainerSerializationModel LoadQuestsContainerModel(string key)
        {
            if (ES2.Exists(key) == false)
            {
                Debug.Log("Can't load from file " + key + " file does not exist. - Ignore on first load.", gameObject);
                return new QuestsContainerSerializationModel();
            }

            using (var reader = CreateReader(key))
            {
                return reader.Read<QuestsContainerSerializationModel>();
            }
        }

        private ES2Reader CreateReader(string key)
        {
            var settings = GetGeneralEasySave2Settings();
            return ES2Reader.Create(key, settings);
        }

        private ES2Writer CreateWriter(string key)
        {
            var settings = GetGeneralEasySave2Settings();
            settings.fileMode = ES2Settings.ES2FileMode.Create;

            return ES2Writer.Create(key, settings);
        }

        private ES2Settings GetGeneralEasySave2Settings()
        {
            return new ES2Settings
            {
                encrypt = useEncryption,
                encryptionType = ES2Settings.EncryptionType.AES128,
                encryptionPassword = encryptionPassword,
            };
        }
    }
}

#endif