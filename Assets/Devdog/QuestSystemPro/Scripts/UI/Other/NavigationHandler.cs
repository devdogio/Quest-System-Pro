using System;
using System.Collections;
using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Devdog.QuestSystemPro.UI
{
    public sealed class NavigationHandler : MonoBehaviour, INavigationHandler
    {
        private enum NavigationType
        {
            Automatic,
            None,
            ExplicitHorizontal,
            ExplicitVertical
        }

        [SerializeField]
        private NavigationType _navigationType = NavigationType.Automatic;

        [SerializeField]
        private bool _selectFirst = true;

        [SerializeField]
        private bool _overwriteSelection = false;

        public void HandleNavigation(Selectable[] selectables)
        {
            switch (_navigationType)
            {
                case NavigationType.Automatic:
                    {
                        foreach (var selectable in selectables)
                        {
                            selectable.navigation = Navigation.defaultNavigation;
                        }
                        break;
                    }
                case NavigationType.None:
                    {
                        foreach (var selectable in selectables)
                        {
                            selectable.navigation = new Navigation() { mode = Navigation.Mode.None, selectOnDown = null, selectOnLeft = null, selectOnRight = null, selectOnUp = null };
                        }
                        break;
                    }
                case NavigationType.ExplicitHorizontal:
                    {
                        for (int i = 0; i < selectables.Length; i++)
                        {
                            Navigation navigation = new Navigation() { mode = Navigation.Mode.Explicit };
                            navigation.selectOnUp = null;
                            navigation.selectOnDown = null;

                            if (i > 0)
                            {
                                var prev = selectables[i - 1];
                                navigation.selectOnLeft = prev;
                            }

                            if (i + 1 < selectables.Length)
                            {
                                var next = selectables[i + 1];
                                navigation.selectOnRight = next;
                            }

                            selectables[i].navigation = navigation;
                        }
                        break;
                    }
                case NavigationType.ExplicitVertical:
                    {
                        for (int i = 0; i < selectables.Length; i++)
                        {
                            Navigation navigation = new Navigation() { mode = Navigation.Mode.Explicit };
                            navigation.selectOnLeft = null;
                            navigation.selectOnRight = null;

                            if (i > 0)
                            {
                                var prev = selectables[i - 1];
                                navigation.selectOnUp = prev;
                            }

                            if (i + 1 < selectables.Length)
                            {
                                var next = selectables[i + 1];
                                navigation.selectOnDown = next;
                            }

                            selectables[i].navigation = navigation;
                        }
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_selectFirst)
            {
                SelectFirst(selectables);
            }
        }

        private void SelectFirst(Selectable[] selectables)
        {
            if (selectables.Length > 0)
            {
                var hasValidSelection = selectables.Any(o => EventSystem.current.currentSelectedGameObject == o.gameObject);
                if (hasValidSelection == false || _overwriteSelection)
                {
                    selectables[0].Select();
                }
            }
        }
    }
}
