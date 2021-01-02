#if EASY_SAVE_3

using UnityEngine;

namespace Devdog.QuestSystemPro.Integration.EasySave3
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Integration/Easy Save 3/Easy Save 3 Save Load Manager")]
    public partial class EasySave3SaveLoadManager : SaveLoadManager
    {
        public bool useEncryption;
        public string encryptionPassword = "L6nbHYmnMMs9REJjNrfC";

        protected override void SaveQuestsContainerModel(string key, QuestsContainerSerializationModel model)
        {
            using (var writer = CreateWriter(key))
            {
                writer.Write<QuestsContainerSerializationModel>(key,model);
                writer.Save(true);
            }
        }

        protected override QuestsContainerSerializationModel LoadQuestsContainerModel(string key)
        {
            if (ES3.FileExists(key) == false)
            {
                Debug.Log("Can't load from file " + key + " file does not exist. - Ignore on first load.", gameObject);
                return new QuestsContainerSerializationModel();
            }

            using (var reader = CreateReader(key))
            {
                return reader.Read<QuestsContainerSerializationModel>(key);
            }
        }

        private ES3Reader CreateReader(string key)
        {
            var settings = GetGeneralEasySave3Settings();
            return ES3Reader.Create(key, settings);
        }

        private ES3Writer CreateWriter(string key)
        {
            var settings = GetGeneralEasySave3Settings();

            return ES3Writer.Create(key, settings);
        }

        private ES3Settings GetGeneralEasySave3Settings()
        {
            return new ES3Settings(useEncryption ? ES3.EncryptionType.AES : ES3.EncryptionType.None, encryptionPassword);
        }
    }
}

#endif