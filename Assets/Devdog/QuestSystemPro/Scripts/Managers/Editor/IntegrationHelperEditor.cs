using UnityEngine;
using System.Collections;
using UnityEditor;
using Devdog.General.Editors;

namespace Devdog.QuestSystemPro.Editors
{
    public class IntegrationHelperEditor : IntegrationHelperEditorBase
    {
        [MenuItem(QuestSystemPro.ToolsMenuPath + "Integrations", false, 0)]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<IntegrationHelperEditor>(true, "Integrations", true);
        }


        protected override void DrawIntegrations()
        {

            ShowIntegration("Inventory Pro", "Inventory Pro is a highly flexible and easy to use inventory, that can be used for all game types. ", GetUrlForProductWithID("31226"), "INVENTORY_PRO");
            ShowIntegration("Inventory Pro Legacy", "Using V2.4 or older? Use enable the legacy version. ", GetUrlForProductWithID("31226"), "INVENTORY_PRO_LEGACY");
            ShowIntegration("PlayMaker", "Quickly make gameplay prototypes, A.I. behaviors, animation graphs, interactive objects, cut-scenes, walkthroughs", GetUrlForProductWithID("368"), "PLAYMAKER");
            ShowIntegration("Rewired", "Rewired is an advanced input system that completely redefines how you work with input, giving you an unprecedented level of control over one of the most important components of your game.", GetUrlForProductWithID("21676"), "REWIRED");
            ShowIntegration("Love/Hate", "Love/Hate is a relationship and personality simulator for Unity. It models characters’ feelings about each other using emotional states and value-based judgment of deeds.", GetUrlForProductWithID("33063"), "LOVE_HATE");
            ShowIntegration("SALSA", "Simple Automated Lip Sync Approximation provides high quality, language-agnostic, lip sync approximation for your 2D and 3D characters using Sprite texture types or BlendShapes.", GetUrlForProductWithID("16944"), "SALSA");
            ShowIntegration("LipSync Pro", "LipSync Pro is an editor extension for creating high-quality, offline lipsyncing and facial animation inside Unity.", GetUrlForProductWithID("32117"), "LIPSYNC");
            ShowIntegration("RT-Voice", "RT-Voice uses the computer's (already implemented) TTS (text-to-speech) voices to turn the written lines into speech and dialogue at run-time!", GetUrlForProductWithID("48394"), "RT_VOICE");
            ShowIntegration("Easy Save 2", "Easy save is a fast and easy tool to load and save almost any data type.", GetUrlForProductWithID("768"), "EASY_SAVE_2");

        }
    }
}