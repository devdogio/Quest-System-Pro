using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Devdog.General.Editors;
using UnityEditor.Callbacks;

namespace Devdog.QuestSystemPro.Editors
{
    [InitializeOnLoad]
    public class GettingStartedEditor : GettingStartedEditorBase
    {
        private const string MenuItemPath = QuestSystemPro.ToolsMenuPath + "Getting started";
        private static bool _doInit = false;

        public GettingStartedEditor()
        {
            version = QuestSystemPro.Version;
            productName = QuestSystemPro.ProductName;
            documentationUrl = QuestSystemPro.ProductUrl;
            youtubeUrl = "https://www.youtube.com/playlist?list=PL_HIoK0xBTK467JKbvTw9LEjX_cTah59H";
            reviewProductUrl = "https://www.assetstore.unity3d.com/en/#!/content/63460";
        }

		static GettingStartedEditor()
        {
            // Init
            _doInit = true;
        }

        private void OnEnable()
        {
            if (_doInit)
            {
                if (EditorPrefs.GetBool(editorPrefsKey))
                {
                    ShowWindowInternal();
                }
            }

            _doInit = false;
        }

        [MenuItem(MenuItemPath, false, 1)] // Always at bottom
        protected static void ShowWindowInternal()
        {
            window = GetWindow<GettingStartedEditor>();
            window.GetImages();
            window.ShowUtility();
        }

        public override void ShowWindow()
        {
        	ShowWindowInternal();
        }

        protected override void DrawGettingStarted()
        {
            DrawBox(0, 0, "Documentation", "The official documentation has a detailed description of all components and code examples.", documentationIcon, () =>
            {
                Application.OpenURL(documentationUrl);
            });

            DrawBox(1, 0, "Video tutorials", "The video tutorials cover all interfaces and a complete set up.", videoTutorialsIcon, () =>
            {
                Application.OpenURL(youtubeUrl);
            });

            DrawBox(2, 0, "Discord", "Join the community on Discord for support.", discordIcon, () =>
            {
                Application.OpenURL(discordUrl);
            });
            
            DrawBox(3, 0, "Integrations", "Combine the power of assets and enable integrations.", integrationsIcon, () =>
            {
                IntegrationHelperEditor.ShowWindow();
            });

            DrawBox(4, 0, "Rate / Review", "Like " + productName + "? Share the experience :)", reviewIcon, () =>
            {
                Application.OpenURL(reviewProductUrl);
            });

            base.DrawGettingStarted();
        }
    }
}