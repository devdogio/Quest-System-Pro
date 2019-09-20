using System;
using System.Collections.Generic;
using Devdog.General;
using Devdog.General.Localization;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.General.UI;
using Devdog.QuestSystemPro.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.QuestSystemPro.Dialogue.UI
{
    public abstract class NodeUIBase : MonoBehaviour
    {
        public Text message;
        public RectTransform playerDecisionsContainer;

        public LocalizedString moveToNextNodeText = new LocalizedString("DefaultMoveToNextNodeText");

        [Header("Audio & Visuals")]
        public AudioClipInfo onShowAudio;
        public MotionInfo onShowAnimation;

        [Header("Prefabs")]
        public PlayerDecisionUI defaultPlayerDecisionUIPrefab;

        [NonSerialized]
        protected List<PlayerDecisionUI> decisions = new List<PlayerDecisionUI>();

        [NonSerialized]
        protected NodeBase currentNode;

        [NonSerialized]
        protected IVariablesStringFormatter variablesStringFormatter;

        [NonSerialized]
        protected INavigationHandler navigationHandler;

        [NonSerialized]
        protected Animator animator;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            navigationHandler = GetComponent<INavigationHandler>();
            variablesStringFormatter = new VariablesStringFormatter();
            decisions = new List<PlayerDecisionUI>();
        }

        public virtual void Repaint(NodeBase node)
        {
            decisions.Clear();
            currentNode = node;
            RemoveOldDecisions();

            if (gameObject.activeInHierarchy == false)
            {
                return;
            }

            SetText(variablesStringFormatter.Format(currentNode.message, currentNode.owner.variables));
            SetDecisions();
            if (navigationHandler != null)
            {
                navigationHandler.HandleNavigation(decisions.Select(o => o.button).Cast<Selectable>().ToArray());
            }

            AudioManager.AudioPlayOneShot(onShowAudio);
            animator.Play(onShowAnimation);
        }

        protected void RemoveOldDecisions()
        {
            if (playerDecisionsContainer != null)
            {
                foreach (Transform t in playerDecisionsContainer)
                {
                    Destroy(t.gameObject);
                }
            }
        }

        protected virtual void SetText(string msg)
        {
            var textAnimator = message.GetComponent<ITextAnimator>();
            if (textAnimator != null)
            {
                textAnimator.AnimateText(msg);
            }
            else
            {
                message.text = msg;
            }
        }

        protected virtual void SetDecisions()
        {
            SetDefaultPlayerDecision();
        }

        protected virtual void SetDefaultPlayerDecision()
        {
            if (playerDecisionsContainer != null)
            {
                var playerDecisionInst = UnityEngine.Object.Instantiate<PlayerDecisionUI>(defaultPlayerDecisionUIPrefab);
                playerDecisionInst.transform.SetParent(playerDecisionsContainer);
                playerDecisionInst.transform.ResetTRSRect();

                playerDecisionInst.Repaint(new PlayerDecision() { option = moveToNextNodeText }, null, currentNode.edges.Length == 0 || currentNode.edges.Any(o => o.CanUse(currentNode.owner)));
                playerDecisionInst.button.onClick.AddListener(OnDefaultPlayerDecisionClicked);

                decisions.Add(playerDecisionInst);
            }
        }

        protected virtual void OnDefaultPlayerDecisionClicked()
        {
            currentNode.Finish(true);
        }
    }
}