using System;
using System.Collections.Generic;
using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.General.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Devdog.QuestSystemPro.UI
{
    [RequireComponent(typeof(UIWindow))]
    public class DetailedQuestWindowUI : MonoBehaviour
    {
        [Header("UI References")]
        [Required]
        public QuestWindowUIBase questDetailsWindow;
        public RectTransform questButtonContainer;
        public UIWindowPage noQuestSelected;
        public bool focusFirstQuestOnShow = true;
        public bool showWindowOnAcceptedQuest = true;
        public bool showWindowOnQuestEnded = true;

        public QuestSidebarUI questSidebarUI;


        [Header("Prefabs")]
        public QuestButtonUI questButtonUIPrefab;


        protected Dictionary<Quest, QuestButtonUI> uiCache = new Dictionary<Quest, QuestButtonUI>();
        protected Quest selectedQuest;
        protected INavigationHandler navigationHandler;
        protected UIWindow window;

        protected virtual void Awake()
        {
            navigationHandler = GetComponent<INavigationHandler>();
            if (navigationHandler == null)
            {
                navigationHandler = gameObject.AddComponent<NavigationHandler>();
            }

            window = GetComponent<UIWindow>();
        }

        protected virtual void Start()
        {
            QuestManager.instance.OnQuestStatusChanged += OnQuestStatusChanged;
            window.OnShow += OnShowWindow;
            window.OnHide += OnHideWindow;

            // Initial update.
            foreach (var quest in QuestManager.instance.GetQuestStates().activeQuests)
            {
                OnQuestStatusChanged(QuestStatus.InActive, quest);
            }
        }

        protected virtual void OnDestroy()
        {
            if(QuestManager.instance != null)
            {
                QuestManager.instance.OnQuestStatusChanged -= OnQuestStatusChanged;
            }

            if(window != null)
            {
                window.OnShow -= OnShowWindow;
                window.OnHide -= OnHideWindow;  
            }
        }

        protected virtual void OnShowWindow()
        {
            if (focusFirstQuestOnShow)
            {
                SelectFirstQuest();
            }

            ShowWindowForQuest(selectedQuest);
            SetNavigation();
        }

        protected virtual void OnHideWindow()
        { }

        protected virtual void SelectFirstQuest()
        {
            if(uiCache.Count > 0)
            {
                SelectQuest(uiCache.FirstOrDefault().Key);
            } else
            {
                SelectQuest(null);
            }
        }

        protected virtual void OnQuestStatusChanged(QuestStatus before, Quest quest)
        {
            switch (quest.status)
            {
                case QuestStatus.InActive:
                case QuestStatus.Completed:
                case QuestStatus.Cancelled:

                    if (selectedQuest == quest)
                    {
                        SelectQuest(null);
                    }

                    if (uiCache.ContainsKey(quest))
                    {
                        var a = uiCache[quest];
                        uiCache.Remove(quest);
                        Destroy(a.gameObject);
                    }

                    break;
                case QuestStatus.Active:

                    SaveQuestToggledState(quest, PlayerPrefs.HasKey(QuestUtility.GetQuestCheckedSaveKey(quest)));
                    Repaint(quest);

                    if (showWindowOnAcceptedQuest)
                    {
                        ShowWindowForQuest(quest);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SetNavigation();
        }

        protected virtual void SetNavigation()
        {
            navigationHandler.HandleNavigation(uiCache.Select(o => o.Value.button).Cast<Selectable>().ToArray());
        }

        public virtual void Repaint(Quest quest)
        {
            if (uiCache.ContainsKey(quest) == false)
            {
                uiCache[quest] = CreateUIButtonForQuest(quest);
            }

            uiCache[quest].Repaint(quest);
        }

        protected virtual QuestButtonUI CreateUIButtonForQuest(Quest quest)
        {
            var uiInst = Instantiate<QuestButtonUI>(questButtonUIPrefab);
            uiInst.transform.SetParent(questButtonContainer);
            UIUtility.ResetTransform(uiInst.transform);

            var questTemp = quest;
            uiInst.button.onClick.AddListener(() =>
            {
                SelectQuest(questTemp);
            });

            if (uiInst.toggle != null)
            {
                uiInst.toggle.onValueChanged.AddListener((isOn) =>
                {
                    OnQuestToggleValueChanged(questTemp, isOn);
                });
            }

            return uiInst;
        }

        protected virtual void SelectQuest(Quest quest)
        {
            selectedQuest = quest;
            ShowWindowForQuest(selectedQuest);

            if (selectedQuest != null)
            {
                questDetailsWindow.Repaint(selectedQuest);
            }
        }

        protected virtual void ShowWindowForQuest(Quest quest)
        {
            if (quest == null)
            {
                if(questDetailsWindow.window.isVisible || showWindowOnQuestEnded)
                {
                    if (noQuestSelected != null)
                    {
                        noQuestSelected.Show();
                    }
                }
            }
            else
            {
                questDetailsWindow.window.Show();
            }
        }

        protected virtual void OnQuestToggleValueChanged(Quest quest, bool isOn)
        {
            SaveQuestToggledState(quest, isOn);
        }

        private void SaveQuestToggledState(Quest quest, bool isOn)
        {
            var playerPrefsKey = QuestUtility.GetQuestCheckedSaveKey(quest);
            if (isOn)
            {
                PlayerPrefs.SetInt(playerPrefsKey, 1);
                if (questSidebarUI != null && questSidebarUI.ContainsQuest(quest) == false)
                {
                    questSidebarUI.AddQuest(quest);
                }
            }
            else
            {
                if (PlayerPrefs.HasKey(playerPrefsKey))
                {
                    PlayerPrefs.DeleteKey(playerPrefsKey);
                    questSidebarUI.RemoveQuest(quest);
                }
            }
        }
    }
}