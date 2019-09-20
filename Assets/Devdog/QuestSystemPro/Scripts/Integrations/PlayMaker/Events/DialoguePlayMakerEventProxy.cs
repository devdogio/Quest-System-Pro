#if PLAYMAKER

using Devdog.QuestSystemPro.Dialogue;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro.Integration.PlayMaker
{

    /// <summary>
    /// Relays all dialogue events to PlayMaker
    /// </summary>
    [AddComponentMenu(QuestSystemPro.ProductName + "/Integration/PlayMaker/Dialogue PlayMaker Event Proxy")]
    public partial class DialoguePlayMakerEventProxy : MonoBehaviour
    {
        [Header("Uses this object when empty")]
        [SerializeField]
        private PlayMakerFSM _fsm;
        public PlayMakerFSM fsm
        {
            get
            {
                if (_fsm == null)
                    _fsm = GetComponent<PlayMakerFSM>();

                return _fsm;
            }
        }

        public const string PlayMakerEventCategoryName = "QUEST_SYSTEM_PRO/";


        // <inheritdoc />
        protected void Start()
        {
            var c = DialogueManager.instance;
            c.OnCurrentDialogueStatusChanged += OnCurrentDialogueStatusChanged;
            c.OnCurrentDialogueChanged += OnCurrentDialogueChanged;
            c.OnCurrentDialogueNodeChanged += OnCurrentDialogueNodeChanged;
        }

        protected void OnDestroy()
        {
            var c = DialogueManager.instance;
            c.OnCurrentDialogueStatusChanged -= OnCurrentDialogueStatusChanged;
            c.OnCurrentDialogueChanged -= OnCurrentDialogueChanged;
            c.OnCurrentDialogueNodeChanged -= OnCurrentDialogueNodeChanged;
        }

        private void _FireEvent(string name)
        {
            if (fsm != null)
            {
                fsm.SendEvent(PlayMakerEventCategoryName + name);
            }
        }

        private void FireEvent(string name, int val)
        {
            if (fsm != null)
            {
                var i = fsm.FsmVariables.FindFsmInt(PlayMakerEventCategoryName + "Event_Int");
                Assert.IsNotNull(i, "No FSM Variable found with name " + PlayMakerEventCategoryName + "Event_Int");
                i.Value = val;
            }

            _FireEvent(name);
        }

        private void FireEvent(string name, UnityEngine.Object val)
        {
            if (fsm != null)
            {
                var i = fsm.FsmVariables.FindFsmObject(PlayMakerEventCategoryName + "Event_Object");
                Assert.IsNotNull(i, "No FSM Variable found with name " + PlayMakerEventCategoryName + "Event_Object");
                i.Value = val;
            }

            _FireEvent(name);
        }

        private void FireEvent(string name, string val)
        {
            if (fsm != null)
            {
                var i = fsm.FsmVariables.FindFsmString(PlayMakerEventCategoryName + "Event_String");
                Assert.IsNotNull(i, "No FSM Variable found with name " + PlayMakerEventCategoryName + "Event_String");
                i.Value = val;
            }

            _FireEvent(name);
        }


        private void OnCurrentDialogueNodeChanged(NodeBase before, NodeBase after)
        {
            FireEvent("OnCurrentDialogueNodeChanged", (int)after.index);
        }

        private void OnCurrentDialogueChanged(Dialogue.Dialogue before, Dialogue.Dialogue after, IDialogueOwner owner)
        {
            FireEvent("OnCurrentDialogueChanged", after);
        }

        private void OnCurrentDialogueStatusChanged(DialogueStatus before, DialogueStatus after, Dialogue.Dialogue self, IDialogueOwner owner)
        {
            FireEvent("OnCurrentDialogueStatusChanged", after.ToString());
        }
    }
}

#endif