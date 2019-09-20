using Devdog.QuestSystemPro;
using UnityEngine.Assertions;

namespace Devdog.General
{
    /// <summary>
    /// Partial class for the Devdog.General.Player
    /// </summary>
    public partial class Player
    {
        private QuestSystemPlayer _questSystemPlayer;
        public QuestSystemPlayer questSystemPlayer
        {
            get
            {
                if (_questSystemPlayer == null)
                {
                    _questSystemPlayer = GetComponent<QuestSystemPlayer>();
                }

                Assert.IsNotNull(_questSystemPlayer, "QuestSystemPlayer component not found on current player!");
                return _questSystemPlayer;
            }
        }



    }
}