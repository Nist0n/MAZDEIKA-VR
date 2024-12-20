﻿using System.Collections.Generic;
using TinyGiantStudio.EditorHelpers;
using TinyGiantStudio.Layout;
using TinyGiantStudio.Modules;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace TinyGiantStudio.Text
{
    [CustomEditor(typeof(Modular3DText))]
    public class Modular3DTextEditor : Editor
    {
        Modular3DText myTarget;
        SerializedObject soTarget;


        //SerializedProperty text;

        //main settings
        SerializedProperty font;
        SerializedProperty material;
        SerializedProperty fontSize;

        SerializedProperty autoLetterSize;
        SerializedProperty _wordSpacing;


        //effects
        SerializedProperty useModules;
        SerializedProperty startAddingModuleFromChar;
        SerializedProperty addingModules;
        SerializedProperty startDeletingModuleFromChar;
        SerializedProperty deletingModules;
        SerializedProperty customDeleteAfterDuration;
        SerializedProperty deleteAfter;
        SerializedProperty applyModuleOnNewCharacter;
        SerializedProperty applyModulesOnStart;
        SerializedProperty applyModulesOnEnable;

        //advanced settings
        SerializedProperty destroyChildObjectsWithGameObject;
        SerializedProperty repositionOldCharacters;
        SerializedProperty reApplyModulesToOldCharacters;
        SerializedProperty hideOverwrittenVariablesFromInspector;
        SerializedProperty combineMeshInEditor;
        SerializedProperty singleInPrefab;
        SerializedProperty combineMeshDuringRuntime;
        SerializedProperty hideLettersInHierarchyInPlayMode;
        SerializedProperty updateTextOncePerFrame;
        SerializedProperty autoSaveMesh;
        SerializedProperty canBreakOutermostPrefab;
        SerializedProperty debugLogs;

        SerializedProperty meshPostProcess;


        //Debug -- starts
        SerializedProperty wordArray;
        //Debug --- ends

        #region Tooltips
        readonly string addingtoolTip = "During runtime, these modules are called when new characters are added to the text.";
        readonly string deleteingtoolTip = "During runtime, these modules are called when characters are removed from the text.";
        readonly string modulesToolTip = "Modules provide an easy way to manipulate characters.";
        #endregion



        AnimBool showMainSettingsInEditor;
        AnimBool showLayoutSettingsInEditor;
        AnimBool showModuleSettingsInEditor;
        AnimBool showModuleRunSettingsInEditor;
        AnimBool showAdvancedSettingsInEditor;
        AnimBool showAdvancedInspectorSettingsInEditor;
        AnimBool showAdvancedBehaviorSettingsInEditor;
        AnimBool showDebugSettingsInEditor;


        //style
        GUIStyle toggleStyle = null;
        GUIStyle foldOutStyle = null;
        GUIStyle iconButtonStyle = null;
        GUIStyle defaultLabel = null;
        GUIStyle defaultMultilineLabel = null;
        GUIStyle headerLabel = null;

        #region colors
        Color openedFoldoutTitleColor = new Color(136 / 255f, 173 / 255f, 234 / 255f, 1f);
        static readonly Color openedFoldoutTitleColorDarkSkin = new Color(136 / 255f, 173 / 255f, 234 / 255f, 1f);
        static readonly Color openedFoldoutTitleColorLightSkin = new Color(38f / 255f, 88f / 255f, 109f / 255f, 1);

        /// <summary>
        /// settings that are turned off but still visible. Not the toggle button's color
        /// </summary>
        static readonly Color toggledOffColor = new Color(0.75f, 0.75f, 0.75f);

        static readonly Color toggledOnButtonColor = Color.yellow;
        static readonly Color toggledOffButtonColor = Color.white;
        #endregion colors


        Texture documentationIcon;
        Texture addIcon;
        Texture deleteIcon;

        readonly float iconSize = 20;

        public AssetSettings settings;


        readonly string layoutDocumentationURL = "https://ferdowsur.gitbook.io/layout-system/layout-group";








        void OnEnable()
        {
            myTarget = (Modular3DText)target;
            soTarget = new SerializedObject(target);

            FindProperties();
            LoadFoldoutValues();

            documentationIcon = EditorGUIUtility.Load("Assets/Plugins/Tiny Giant Studio/Modular 3D Text/Utility/Editor Icons/Icon_Documentation.png") as Texture;
            addIcon = EditorGUIUtility.Load("Assets/Plugins/Tiny Giant Studio/Modular 3D Text/Utility/Editor Icons/Icon_Plus.png") as Texture;
            deleteIcon = EditorGUIUtility.Load("Assets/Plugins/Tiny Giant Studio/Modular 3D Text/Utility/Editor Icons/Icon_Cross.png") as Texture;

            if (!settings)
                settings = StaticMethods.VerifySettings(settings);
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            GenerateStyle();

            EditorGUI.BeginChangeCheck();

            WarningCheck(); //----------------

            myTarget.Text = EditorGUILayout.TextArea(myTarget.Text, GUILayout.Height(100));
            //EditorGUILayout.PropertyField(text, GUIContent.none, GUILayout.Height(100));

            GUILayout.Space(5);

            MainSettings(); //----------------

            GUILayout.Space(5);

            LayoutSettings(out Alignment anchor, out Vector3 spacing, out float width, out float height); //----------------

            GUILayout.Space(5);

            ModuleSettings(); //----------------

            GUILayout.Space(5);

            AdvancedSettings(); //----------------

            GUILayout.Space(5);

            DebugView(); //----------------

            if (EditorGUI.EndChangeCheck())
            {
                Font font = myTarget.Font;
                string text = myTarget.Text;

                ApplyGridLayoutSettings(anchor, spacing, width, height);

                //In prefab mode font change wasn't updating for some reasons
                if (font != myTarget.Font || soTarget.ApplyModifiedProperties())
                {
                    if (text == myTarget.Text)
                        myTarget.oldText = "";

                    myTarget.updatedAfterStyleUpdateOnPrefabInstances = false;
                    myTarget.UpdateText();
                }

                EditorUtility.SetDirty(myTarget);
            }
        }





        #region Primary Sections
        void MainSettings()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 1;

            GUILayout.BeginVertical(EditorStyles.toolbar);
            showMainSettingsInEditor.target = EditorGUILayout.Foldout(showMainSettingsInEditor.target, "Main Settings", true, foldOutStyle);
            GUILayout.EndVertical();

            if (EditorGUILayout.BeginFadeGroup(showMainSettingsInEditor.faded))
            {
                EditorGUI.indentLevel = 0;

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();

                MText_Editor_Methods.ItalicHorizontalField(font, "Font", "", FieldSize.tiny);

                bool ShowControllableVariables = !myTarget.DoesStyleInheritFromAParent() || !myTarget.hideOverwrittenVariablesFromInspector;
                if (ShowControllableVariables)
                {
                    MText_Editor_Methods.ItalicHorizontalField(fontSize, "Size", "", FieldSize.tiny);
                }
                GUILayout.EndVertical();
                if (ShowControllableVariables)
                {
                    GUILayout.BeginVertical(GUILayout.MaxWidth(50));

                    Texture2D texture = null;

                    if (myTarget.Material)
                        texture = AssetPreview.GetAssetPreview(myTarget.Material);

                    try
                    {
                        if (texture)
                            GUILayout.Box(texture, GUIStyle.none, GUILayout.MaxWidth(40), GUILayout.MaxHeight(40));
                    }
                    catch
                    {

                    }

                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
                if (ShowControllableVariables)
                {
                    MText_Editor_Methods.ItalicHorizontalField(material, "Material", "", FieldSize.tiny);
                }

                CombineMesh();

                EditorGUI.indentLevel = 3;
                DontCombineInEditorEither();

                TextStyles();

                GUILayout.Space(5);
            }
            EditorGUILayout.EndFadeGroup();
            GUILayout.EndVertical();
        }

        void LayoutSettings(out Alignment anchor, out Vector3 spacing, out float width, out float height)
        {
            GridLayoutGroup gridLayout = myTarget.gameObject.GetComponent<GridLayoutGroup>();
            if (!gridLayout)
            {
                anchor = Alignment.MiddleCenter;
                spacing = Vector3.zero;
                width = 0;
                height = 0;
            }
            else
            {
                anchor = gridLayout.Anchor;
                spacing = gridLayout.Spacing;
                width = gridLayout.Width;
                height = gridLayout.Height;
            }

            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 1;
            GUILayout.BeginVertical(EditorStyles.toolbar);
            GUILayout.BeginHorizontal();
            showLayoutSettingsInEditor.target = EditorGUILayout.Foldout(showLayoutSettingsInEditor.target, new GUIContent("Layout", "Layouts are driven by Layout Groups. Although grid layout groups is the default one, it can work with any layout group. Experiment with different ones."), true, foldOutStyle);
            Documentation(layoutDocumentationURL, "Layout Groups");
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            if (EditorGUILayout.BeginFadeGroup(showLayoutSettingsInEditor.faded))
            {
                EditorGUI.indentLevel = 0;
                GUILayout.Space(5);
                MText_Editor_Methods.ItalicHorizontalField(autoLetterSize, "Auto Letter Size", "If turned on, instead of using predetermined size of each letter, their max X size is taken from the size they take in render view", FieldSize.extraLarge);
                EditorGUILayout.PropertyField(_wordSpacing);
                GUILayout.Space(5);

                ChooseLayoutGroups();

                if (myTarget.gameObject.GetComponent<GridLayoutGroup>())
                {
                    //GUILayout.Space(5);
                    //anchor = (Alignment)EditorGUILayout.EnumPopup(myTarget.GetComponent<GridLayoutGroup>().Anchor);

                    //GUILayout.BeginHorizontal();
                    //GUILayout.Label("", GUILayout.MaxWidth(9));
                    //GUILayout.Label("Spacing", GUILayout.MaxWidth(55));
                    //spacing = EditorGUILayout.Vector3Field(GUIContent.none, myTarget.GetComponent<GridLayoutGroup>().Spacing, GUILayout.MinWidth(80));
                    //GUILayout.EndHorizontal();



                    /////Height width
                    //GUILayout.BeginHorizontal();
                    //GUILayout.Label("", GUILayout.MaxWidth(9));
                    //GUILayout.Label("Width", GUILayout.MaxWidth(36));
                    //width = EditorGUILayout.FloatField(GUIContent.none, myTarget.GetComponent<GridLayoutGroup>().Width, GUILayout.MinWidth(60));
                    //GUILayout.Label("Height", GUILayout.MaxWidth(42));
                    //height = EditorGUILayout.FloatField(GUIContent.none, myTarget.GetComponent<GridLayoutGroup>().Height, GUILayout.MinWidth(60));
                    //GUILayout.EndHorizontal();

                    EditorGUI.indentLevel = 0;
                    EditorGUILayout.LabelField("Modify the attached grid layout component to modify the Layout of the text.", defaultMultilineLabel);
                }
                else if (myTarget.gameObject.GetComponent<LayoutGroup>())
                {
                    EditorGUI.indentLevel = 0;
                    EditorGUILayout.HelpBox("Modify the attached layout component to modify the Layout of the text.", MessageType.Info);
                }
                else
                {
                    EditorGUI.indentLevel = 0;
                    EditorGUILayout.HelpBox("No layout group seems to be attached to the text. If this is intentional, ignore this message. Otherwise, please add any layout group to this object. Grid Layout group is the default one.", MessageType.Warning);
                    if (GUILayout.Button("Add grid layout Group"))
                    {
                        AddGridLayout();
                    }
                }
                GUILayout.Space(5);
            }
            EditorGUILayout.EndFadeGroup();
            GUILayout.EndVertical();
        }

        void ModuleSettings()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 0;

            GUILayout.BeginVertical(EditorStyles.toolbar);
            GUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(useModules, GUIContent.none, GUILayout.MaxWidth(25));
            showModuleSettingsInEditor.target = EditorGUILayout.Foldout(showModuleSettingsInEditor.target, new GUIContent("Modules", modulesToolTip), true, foldOutStyle);
            Documentation("https://ferdowsur.gitbook.io/modular-3d-text/modules", "Modules");
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();


            if (EditorGUILayout.BeginFadeGroup(showModuleSettingsInEditor.faded))
            {
                Color contentDefaultColor = GUI.contentColor;

                if (myTarget.combineMeshDuringRuntime)
                    EditorGUILayout.HelpBox("Combine mesh in playmode/build is turned on, modules won't work.", MessageType.Error);

                if (!myTarget.useModules || myTarget.combineMeshDuringRuntime)
                    GUI.contentColor = toggledOffColor;

                GUILayout.Space(5);

                RunModulesSettings();

                EditorGUI.indentLevel = 2;
                GUILayout.Space(5);
                ModuleDrawer.BaseModuleContainerList("Adding", addingtoolTip, myTarget.addingModules, addingModules, soTarget);



                //ModuleContainerList("Adding", addingtoolTip, myTarget.addingModules, addingModules);
                GUILayout.Space(10);
                DeleteAfterDuration();
                ModuleDrawer.BaseModuleContainerList("Deleting", deleteingtoolTip, myTarget.deletingModules, deletingModules, soTarget);
                //ModuleContainerList("Deleting", deleteingtoolTip, myTarget.deletingModules, deletingModules);

                GUI.contentColor = contentDefaultColor;
            }

            EditorGUILayout.EndFadeGroup();
            GUILayout.EndVertical();
        }



        void AdvancedSettings()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 1;

            GUILayout.BeginVertical(EditorStyles.toolbar);
            showAdvancedSettingsInEditor.target = EditorGUILayout.Foldout(showAdvancedSettingsInEditor.target, "Advanced Settings", true, foldOutStyle);
            GUILayout.EndVertical();
            if (EditorGUILayout.BeginFadeGroup(showAdvancedSettingsInEditor.faded))
            {
                EditorGUI.indentLevel = 1;

                EditorGUILayout.Space(5);

                GUILayout.BeginVertical(EditorStyles.helpBox);

                GUILayout.BeginVertical(EditorStyles.toolbar);
                showAdvancedInspectorSettingsInEditor.target = EditorGUILayout.Foldout(showAdvancedInspectorSettingsInEditor.target, "Inspector Settings", true, foldOutStyle);
                GUILayout.EndVertical();

                if (EditorGUILayout.BeginFadeGroup(showAdvancedInspectorSettingsInEditor.faded))
                {
                    EditorGUI.indentLevel = 0;
                    //EditorGUILayout.PropertyField(debugLogs);
                    MText_Editor_Methods.ItalicHorizontalField(debugLogs, "Debug Logs", "", FieldSize.gigantic);
                    MText_Editor_Methods.ItalicHorizontalField(hideOverwrittenVariablesFromInspector, "Hide overwritten values", "Texts under button/list sometimes have styles overwritten. This hides these variables", FieldSize.gigantic);

                    HideLetterInHierarchy();
                    EditorGUILayout.Space(5);
                    PrefabAdvancedSettings(); //mesh save setting
                    CombineMeshSettings();
                }
                EditorGUILayout.EndFadeGroup();

                GUILayout.EndVertical();

                EditorGUILayout.Space(5);

                GUILayout.BeginVertical(EditorStyles.helpBox);

                GUILayout.BeginVertical(EditorStyles.toolbar);
                showAdvancedBehaviorSettingsInEditor.target = EditorGUILayout.Foldout(showAdvancedBehaviorSettingsInEditor.target, "Behavior Settings", true, foldOutStyle);
                GUILayout.EndVertical();

                if (EditorGUILayout.BeginFadeGroup(showAdvancedBehaviorSettingsInEditor.faded))
                {
                    EditorGUI.indentLevel = 0;
                    MText_Editor_Methods.ItalicHorizontalField(meshPostProcess, "UV Remapping", "", FieldSize.normal);
                    EditorGUILayout.Space(5);

                    MText_Editor_Methods.ItalicHorizontalField(repositionOldCharacters, "Reposition old Chars", "If old text = '123' and updated new text = '1234',\nthe '123' will be moved to their correct position when entering the '4'", FieldSize.gigantic);
                    MText_Editor_Methods.ItalicHorizontalField(reApplyModulesToOldCharacters, "Re-apply modules", "If old text = old and updated new text = oldy,\ninstead of applying module to only 'y', it will apply to all chars", FieldSize.gigantic);
                    MText_Editor_Methods.ItalicHorizontalField(updateTextOncePerFrame, "Update once per frame", "If the gameobject is active in hierarchy, uses coroutine to make sure the text is only updated visually once per frame instead of wasting resources if updated multiple times by a script. This is only used if the game object is active in hierarchy and it updates at the end of frame.", FieldSize.gigantic);

                    EditorGUILayout.Space(5);

                    EditorGUILayout.LabelField(new GUIContent("Run module routine on character", "The adding module uses MonoBehavior attached to the char to run the coroutine. This way, if the text is deactivated, the module isn't interrupted."), defaultLabel);
                    EditorGUI.indentLevel = 1;
                    MText_Editor_Methods.ItalicHorizontalField(startAddingModuleFromChar, "Adding module", "If true, the adding module uses MonoBehavior attached to the char to run the coroutine. This way, if the text is deactivated, the module isn't interrupted.", FieldSize.extraLarge);
                    MText_Editor_Methods.ItalicHorizontalField(startDeletingModuleFromChar, "Deleting module", "If true, the deleting module uses MonoBehavior attached to the char to run the coroutine. This way, if the text is deactivated, the module isn't interrupted.", FieldSize.extraLarge);
                    EditorGUILayout.Space(10);
                    EditorGUI.indentLevel = 0;
                    MText_Editor_Methods.ItalicHorizontalField(destroyChildObjectsWithGameObject, "Destroy Letter With this", "If you delete the gameobject, the letters are auto deleted also even if they aren't child object.", FieldSize.gigantic);
                }
                EditorGUILayout.EndFadeGroup();

                GUILayout.EndVertical();
            }

            EditorGUILayout.EndFadeGroup();
            GUILayout.EndVertical();
        }

        void DebugView()
        {
            EditorGUI.indentLevel = 1;

            GUILayout.BeginVertical(EditorStyles.helpBox);
            {
                GUILayout.BeginVertical(EditorStyles.toolbar);
                showDebugSettingsInEditor.target = EditorGUILayout.Foldout(showDebugSettingsInEditor.target, "Debug", true, foldOutStyle);
                GUILayout.EndVertical();
            }
            if (EditorGUILayout.BeginFadeGroup(showDebugSettingsInEditor.faded))
            {
                EditorGUI.indentLevel = 2;

                EditorGUILayout.PropertyField(wordArray);
            }

            EditorGUILayout.EndFadeGroup();
            GUILayout.EndVertical();
            GUILayout.Space(15);
        }

        void WarningCheck()
        {
            EditorGUI.indentLevel = 0;
            if (!myTarget.Font)
                EditorGUILayout.HelpBox("No font selected", MessageType.Error);
            if (!myTarget.Material)
                EditorGUILayout.HelpBox("No material selected", MessageType.Error);
            if (myTarget.DoesStyleInheritFromAParent())
                EditorGUILayout.HelpBox("Some values are overwritten by parent button/list.", MessageType.Info);
        }
        #endregion Primary Sections



        #region Functions for main settings
        /// <summary>
        /// Direction, capitalize etc.
        /// </summary>
        void TextStyles()
        {
            EditorGUI.indentLevel = 0;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Font Style", defaultLabel, GUILayout.MaxWidth(75));

            Color original = GUI.backgroundColor;

            if (myTarget.LowerCase)
                GUI.backgroundColor = toggledOnButtonColor;
            else
                GUI.backgroundColor = toggledOffButtonColor;

            GUIContent smallCase = new GUIContent("ab", "lower case");
            if (LeftButton(smallCase))
            {
                Undo.RecordObject(target, "Update text");
                myTarget.LowerCase = !myTarget.LowerCase;
                myTarget.Capitalize = false;
                myTarget.UpdateText();
                EditorUtility.SetDirty(myTarget);
            }


            if (myTarget.Capitalize)
                GUI.backgroundColor = toggledOnButtonColor;
            else
                GUI.backgroundColor = toggledOffButtonColor;

            GUIContent capitalize = new GUIContent("AB", "UPPER CASE");
            if (RightButton(capitalize))
            {
                Undo.RecordObject(target, "Update text");
                myTarget.Capitalize = !myTarget.Capitalize;
                myTarget.LowerCase = false;
                myTarget.UpdateText();
                EditorUtility.SetDirty(myTarget);
            }

            GUI.backgroundColor = original;
            EditorGUILayout.EndHorizontal();
        }

        void CombineMesh()
        {
            GUIContent combineLabel = new GUIContent("Combine", "Combines each letter into a single mesh.\nNote that there is a maximum amount of verticies each mesh can handle in unity.If the limit is exceeded, rest of the text will be moved to a child. The limit is insanely high and shouldn't be an issue.");
            EditorGUILayout.LabelField(combineLabel, defaultLabel, GUILayout.MaxWidth(72));

            EditorGUI.indentLevel = 1;
            GUILayout.BeginHorizontal();
            MText_Editor_Methods.ItalicHorizontalField(combineMeshInEditor, "in Editor", "Combine into a single mesh in Editor.", FieldSize.small, true);
            MText_Editor_Methods.ItalicHorizontalField(combineMeshDuringRuntime, "Runtime", "Combine into a single mesh runtime. \nPlease note that enabling this might cause problems with some modules.", FieldSize.small, true);

            GUILayout.EndHorizontal();
        }
        #endregion Functions for main settings

        #region Functions for Layout settings
        void ApplyGridLayoutSettings(Alignment anchor, Vector2 spacing, float width, float height)
        {
            GridLayoutGroup gridLayoutGroup = myTarget.GetComponent<GridLayoutGroup>();
            if (gridLayoutGroup == null)
                return;

            if (anchor != gridLayoutGroup.Anchor)
                gridLayoutGroup.Anchor = anchor;

            if (spacing != gridLayoutGroup.Spacing)
                gridLayoutGroup.Spacing = spacing;

            if (width != gridLayoutGroup.Width)
                gridLayoutGroup.Width = width;
            if (height != gridLayoutGroup.Height)
                gridLayoutGroup.Height = height;
        }

        void ChooseLayoutGroups()
        {
            Color defaultColor = GUI.color;

            EditorGUILayout.BeginHorizontal();
            var groups = myTarget.GetListOfAllLayoutGroups();
            for (int i = 0; i < groups.Count; i++)
            {
                if (i == 0) //First layout 
                {
                    if (myTarget.gameObject.GetComponent(groups[i]))
                        GUI.color = toggledOnButtonColor;
                    else
                        GUI.color = toggledOffButtonColor;

                    if (LeftButton(new GUIContent(FormatClassName(groups[i].Name))))
                    {
                        AddLayoutComponent(groups, i);
                    }
                }
                else if (i + 1 == groups.Count) //Last layout
                {
                    if (myTarget.gameObject.GetComponent(groups[i]))
                        GUI.color = toggledOnButtonColor;
                    else
                        GUI.color = toggledOffButtonColor;

                    if (RightButton(new GUIContent(FormatClassName(groups[i].Name))))
                    {
                        AddLayoutComponent(groups, i);
                    }
                }
                else
                {
                    if (myTarget.gameObject.GetComponent(groups[i]))
                        GUI.color = toggledOnButtonColor;
                    else
                        GUI.color = toggledOffButtonColor;

                    if (MidButton(new GUIContent(FormatClassName(groups[i].Name))))
                    {
                        AddLayoutComponent(groups, i);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            GUI.color = defaultColor;
        }

        void AddLayoutComponent(List<System.Type> groups, int i)
        {
            if (myTarget.GetComponent(groups[i]))
                return;

            string title = "Change Layout Group to " + FormatClassName(groups[i].Name);
            string mainDialogue = "This will overwrite all current layout settings";

            bool agreed = EditorUtility.DisplayDialog(title, mainDialogue, "Continue", "Cancel");
            if (agreed)
            {
                if (myTarget.gameObject.GetComponent<LayoutGroup>())
                    Undo.DestroyObjectImmediate(myTarget.gameObject.GetComponent<LayoutGroup>());
                Undo.AddComponent(myTarget.gameObject, groups[i]);
                //DestroyImmediate(myTarget.gameObject.GetComponent<LayoutGroup>());
                //myTarget.gameObject.AddComponent(groups[i]);

                EditorApplication.delayCall += () => UpdateGridLayoutVariables(); //this is because properties aren't having default values for some reason

                EditorApplication.delayCall += () => myTarget.gameObject.GetComponent<Modular3DText>().CleanUpdateText();
            }
        }
        void AddGridLayout()
        {
            myTarget.gameObject.AddComponent<GridLayoutGroup>();
            EditorApplication.delayCall += () => UpdateGridLayoutVariables();
        }

        void UpdateGridLayoutVariables()
        {
            if (myTarget.GetComponent<GridLayoutGroup>())
            {
                myTarget.GetComponent<GridLayoutGroup>().Width = 5;
                myTarget.GetComponent<GridLayoutGroup>().Height = 3;
            }
        }

        string FormatClassName(string name)
        {
            if (name == "GridLayoutGroup")
                return "Grid";
            if (name == "CircularLayoutGroup")
                return "Circular";
            if (name == "LinearLayoutGroup")
                return "Linear";
            return name;
        }
        #endregion

        #region Functions for Module settings
        void RunModulesSettings()
        {
            EditorGUI.indentLevel = 3;
            GUILayout.BeginVertical(EditorStyles.toolbar);
            showModuleRunSettingsInEditor.target = EditorGUILayout.Foldout(showModuleRunSettingsInEditor.target, "Run modules on", true, foldOutStyle);
            GUILayout.EndVertical();

            if (EditorGUILayout.BeginFadeGroup(showModuleRunSettingsInEditor.faded))
            {
                EditorGUI.indentLevel = 4;
                MText_Editor_Methods.ItalicHorizontalField(applyModuleOnNewCharacter, "New character", "Modules are called when new characters are added.", FieldSize.gigantic, true);
                MText_Editor_Methods.ItalicHorizontalField(applyModulesOnStart, "on Start", "On Start(), modules are called on all existing characters. Example: Instantiating a prefab will call the modules when it is active the first time during Unity's Start(). Note that if on enable is active, on start is ignored to avoid updating the text twice in the same frame.", FieldSize.gigantic, true);
                MText_Editor_Methods.ItalicHorizontalField(applyModulesOnEnable, "on Enable", "On OnEnable(), modules are called on all existing characters. Example: Repeatedly enabling-disabling a game object will call modules when they are enabled.", FieldSize.gigantic, true);
            }
            EditorGUILayout.EndFadeGroup();
        }

        void DeleteAfterDuration()
        {
            string toolTip = "When a character is removed, how long it takes the mesh to be deleted.\nIf set to false, when a character is deleted, it is removed instantly or after the highest duration retrievable from modules, if there is any. \nIgnored if modules are disabled.";

            EditorGUI.indentLevel = 0;

            GUILayout.BeginVertical(EditorStyles.helpBox);

            MText_Editor_Methods.ItalicHorizontalField(customDeleteAfterDuration, "Custom delete duration", toolTip, FieldSize.extraLarge);
            if (!myTarget.customDeleteAfterDuration)
            {
                float duration = myTarget.GetDeleteDurationFromEffects();

                if (duration > 0)
                {
                    GUIContent content = new GUIContent("Delete chars after : " + duration + " seconds", toolTip);
                    EditorGUILayout.LabelField(content, defaultLabel);
                }
                else
                {
                    if (myTarget.deletingModules.Count == 0)
                    {
                        EditorGUILayout.LabelField("Letters are instantly removed after removed from text.", defaultMultilineLabel);
                    }
                    else
                    {
                        EditorGUILayout.LabelField(new GUIContent("Letters are instantly removed after removed from text. If this is intentional, ignore this message, otherwise, please specify a custom delete duration."), defaultMultilineLabel);
                    }
                }

            }
            else
            {
                GUILayout.BeginHorizontal();
                MText_Editor_Methods.ItalicHorizontalField(deleteAfter, "Delete After", toolTip, FieldSize.small);
                GUIContent content = new GUIContent(" seconds", toolTip);
                EditorGUILayout.LabelField(content, defaultLabel);
                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();
        }
        #endregion Functions for module settings

        #region Functions for advanced settings        
        void PrefabAdvancedSettings()
        {
            if (PrefabUtility.IsPartOfPrefabInstance(myTarget.gameObject))
            {
                if (PrefabUtility.IsOutermostPrefabInstanceRoot(myTarget.gameObject))
                {
                    MText_Editor_Methods.HorizontalField(canBreakOutermostPrefab, "Break outermost Prefab", "If the text isn't a child object of the prefab, it can break prefab and save the reference.", FieldSize.extraLarge);
                }
            }
            else
                MeshSaveSettings();
            EditorGUI.indentLevel = 1;

            PrefabMeshSaveSettings();
        }
        void MeshSaveSettings()
        {
            if (myTarget.gameObject.GetComponent<MeshFilter>())
            {
                EditorGUI.indentLevel = 0;

                GUILayout.BeginVertical(EditorStyles.helpBox);

                MText_Editor_Methods.ItalicHorizontalField(autoSaveMesh, "Auto save mesh", "");
                //EditorGUILayout.PropertyField(autoSaveMesh);
                GUILayout.BeginHorizontal();

                if (!myTarget.autoSaveMesh)
                {
                    if (GUILayout.Button(new GUIContent("Save")))
                    {
                        if (myTarget.autoSaveMesh && myTarget.meshPaths.Count == 0)
                            myTarget.SaveMeshAsAsset(true);
                        else
                            myTarget.SaveMeshAsAsset(false);
                    }
                }
                if (myTarget.meshPaths.Count != 0
                    && GUILayout.Button(new GUIContent("Save as", "Save a new copy of the mesh in project")))
                {
                    myTarget.SaveMeshAsAsset(true);
                }

                GUILayout.EndHorizontal();
                if (myTarget.meshPaths.Count != 0)
                    if (myTarget.meshPaths[0].Length > 0)
                        EditorGUILayout.LabelField("Mesh path: " + myTarget.meshPaths[0]);

                if (myTarget.autoSaveMesh && myTarget.meshPaths.Count == 0)
                    EditorGUILayout.HelpBox("Auto save is turned on but no save path is detected. Please click 'Save as' and select it.", MessageType.Info);

                GUILayout.EndVertical();
            }
        }
        void PrefabMeshSaveSettings()
        {
            if (myTarget.assetPath != "" && myTarget.assetPath != null && !EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField(myTarget.assetPath, EditorStyles.boldLabel);
                if (GUILayout.Button("Apply to prefab"))
                {
                    myTarget.ReconnectPrefabs();
                }
            }

            if ((myTarget.assetPath != "" && myTarget.assetPath != null && !EditorApplication.isPlaying))
            {
                if (GUILayout.Button("Remove prefab connection"))
                {
                    myTarget.assetPath = "";
                }
            }
            if (PrefabUtility.IsPartOfPrefabInstance(myTarget.gameObject))
            {
                MeshSaveSettings();
            }
        }

        void CombineMeshSettings()
        {
            DontCombineInEditorEither();

            if (myTarget.gameObject.GetComponent<MeshFilter>())
            {
                if (GUILayout.Button(new GUIContent("Optimize mesh", "This causes the geometry and vertices of the combined mesh to be reordered internally in an attempt to improve vertex cache utilisation on the graphics hardware and thus rendering performance. This operation can take a few seconds or more for complex meshes.")))
                {
                    StaticMethods.OptimizeMesh(myTarget.gameObject.GetComponent<MeshFilter>().sharedMesh);
                }
            }
        }

        void DontCombineInEditorEither()
        {
            if (!myTarget.combineMeshInEditor && PrefabUtility.IsPartOfPrefabInstance(myTarget.gameObject))
            {
                GUILayout.BeginHorizontal();

                string tooltip = "Prefabs don't allow child objects that are part of the prefab to be deleted in Editor.\n" +
                    "If you add child objects, then apply, which adds these child objects to the prefab,\n" +
                    "When changing text again, this script can't delete the old gameobjects. Just disables them. Remember to clean them up manually if you enable this.";
                EditorGUILayout.LabelField(new GUIContent("Single in Prefab", tooltip), GUILayout.Width(140));
                EditorGUILayout.PropertyField(singleInPrefab, GUIContent.none);

                //                EditorGUILayout.PropertyField(dontCombineInEditorAnyway, new GUIContent("Single in Prefab too", tooltip), GUILayout.Width(400));
                GUILayout.EndHorizontal();
            }
        }

        void HideLetterInHierarchy()
        {
            EditorGUI.indentLevel = 0;

            GUILayout.BeginHorizontal();
            GUIContent hideLetters = new GUIContent("Hide letters in Hierarchy in playmode", "Hides the game object of letters in the hierarchy. They are still there, accessible by script, just not visible. No impact except for cleaner hierarchy.");
            EditorGUILayout.LabelField(hideLetters, defaultMultilineLabel);
            EditorGUILayout.PropertyField(hideLettersInHierarchyInPlayMode, GUIContent.none, GUILayout.MaxWidth(20));
            //MText_Editor_Methods.HorizontalField(hideLettersInHierarchyInPlayMode, "in PlayMode", "", FieldSize.large);
            GUILayout.EndHorizontal();
        }
        #endregion Functions for advanced settings



        #region Style
        void GenerateStyle()
        {
            if (EditorGUIUtility.isProSkin)
            {
                if (settings)
                    openedFoldoutTitleColor = settings.openedFoldoutTitleColor_darkSkin;
                else
                    openedFoldoutTitleColor = openedFoldoutTitleColorDarkSkin;
            }
            else
            {
                if (settings)
                    openedFoldoutTitleColor = settings.openedFoldoutTitleColor_lightSkin;
                else
                    openedFoldoutTitleColor = openedFoldoutTitleColorLightSkin;
            }

            if (toggleStyle == null)
            {
                toggleStyle = new GUIStyle(GUI.skin.button);
                toggleStyle.margin = new RectOffset(0, 0, toggleStyle.margin.top, toggleStyle.margin.bottom);
                toggleStyle.fontStyle = FontStyle.Bold;
                toggleStyle.active.textColor = Color.yellow;
            }

            if (foldOutStyle == null)
            {
                foldOutStyle = new GUIStyle(EditorStyles.foldout)
                {
                    fontStyle = FontStyle.Bold
                };
                foldOutStyle.onNormal.textColor = openedFoldoutTitleColor;
            }

            if (iconButtonStyle == null)
            {
                iconButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
            }
            EditorStyles.popup.fontSize = 11;
            EditorStyles.popup.fixedHeight = 18;


            if (defaultLabel == null)
            {
                defaultLabel = new GUIStyle(EditorStyles.whiteMiniLabel)
                {
                    fontStyle = FontStyle.Italic,
                    fontSize = 12
                };
                defaultLabel.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 0.75f);
            }
            if (defaultMultilineLabel == null)
            {
                defaultMultilineLabel = new GUIStyle(EditorStyles.wordWrappedLabel)
                {
                    fontSize = 11,
                    fontStyle = FontStyle.Italic,
                    alignment = TextAnchor.MiddleCenter,
                };
                defaultMultilineLabel.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 0.75f);
            }
            if (headerLabel == null)
            {
                headerLabel = new GUIStyle(EditorStyles.wordWrappedLabel)
                {
                    fontSize = 12,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter,
                };
            }
        }

        bool LeftButton(GUIContent content)
        {
            bool clicked = false;
            Rect rect = GUILayoutUtility.GetRect(20, 20);

            GUI.BeginGroup(rect);
            if (GUI.Button(new Rect(0, 0, rect.width + toggleStyle.border.right, rect.height), content, toggleStyle))
                clicked = true;

            GUI.EndGroup();
            return clicked;
        }
        bool MidButton(GUIContent content)
        {
            bool clicked = false;
            Rect rect = GUILayoutUtility.GetRect(20, 20);


            GUI.BeginGroup(rect);
            if (GUI.Button(new Rect(-toggleStyle.border.left, 0, rect.width + toggleStyle.border.left + toggleStyle.border.right, rect.height), content, toggleStyle))
                //if (GUI.Button(new Rect(-toggleStyle.border.left, 0, rect.width + toggleStyle.border.left + toggleStyle.border.right, rect.height), content, toggleStyle))
                clicked = true;
            GUI.EndGroup();
            return clicked;
        }
        bool RightButton(GUIContent content)
        {
            bool clicked = false;
            Rect rect = GUILayoutUtility.GetRect(20, 20);

            GUI.BeginGroup(rect);
            if (GUI.Button(new Rect(-toggleStyle.border.left, 0, rect.width + toggleStyle.border.left, rect.height), content, toggleStyle))
                clicked = true;
            GUI.EndGroup();
            return clicked;
        }
        #endregion

        #region Functions
        void Documentation(string URL, string subject)
        {
            GUIContent doc = new GUIContent(documentationIcon, subject + " documentation\n\nURL: " + URL);
            if (GUILayout.Button(doc, iconButtonStyle, GUILayout.Height(iconSize), GUILayout.Width(iconSize)))
            {
                Application.OpenURL(URL);
            }
        }

        /// <summary>
        /// Called on Enable
        /// </summary>
        void FindProperties()
        {
            //text = soTarget.FindProperty("_text");

            autoSaveMesh = soTarget.FindProperty("autoSaveMesh");

            //main settings
            font = soTarget.FindProperty("_font");
            material = soTarget.FindProperty("_material");
            fontSize = soTarget.FindProperty("_fontSize");

            autoLetterSize = soTarget.FindProperty("_autoLetterSize");
            _wordSpacing = soTarget.FindProperty("_wordSpacing");

            //effects
            useModules = soTarget.FindProperty("useModules");
            startAddingModuleFromChar = soTarget.FindProperty("startAddingModuleFromChar");
            addingModules = soTarget.FindProperty("addingModules");
            startDeletingModuleFromChar = soTarget.FindProperty("startDeletingModuleFromChar");
            deletingModules = soTarget.FindProperty("deletingModules");
            customDeleteAfterDuration = soTarget.FindProperty("customDeleteAfterDuration");
            deleteAfter = soTarget.FindProperty("deleteAfter");
            applyModuleOnNewCharacter = soTarget.FindProperty("applyModuleOnNewCharacter");
            applyModulesOnStart = soTarget.FindProperty("applyModulesOnStart");
            applyModulesOnEnable = soTarget.FindProperty("applyModulesOnEnable");

            //advanced
            destroyChildObjectsWithGameObject = soTarget.FindProperty("destroyChildObjectsWithGameObject");
            repositionOldCharacters = soTarget.FindProperty("repositionOldCharacters");
            reApplyModulesToOldCharacters = soTarget.FindProperty("reApplyModulesToOldCharacters");
            //activateChildObjects = soTarget.FindProperty("activateChildObjects");

            hideOverwrittenVariablesFromInspector = soTarget.FindProperty("hideOverwrittenVariablesFromInspector");
            combineMeshInEditor = soTarget.FindProperty("combineMeshInEditor");
            singleInPrefab = soTarget.FindProperty("singleInPrefab");
            combineMeshDuringRuntime = soTarget.FindProperty("combineMeshDuringRuntime");
            hideLettersInHierarchyInPlayMode = soTarget.FindProperty("hideLettersInHierarchyInPlayMode");
            //hideLettersInHierarchyInEditMode = soTarget.FindProperty("hideLettersInHierarchyInEditMode");
            updateTextOncePerFrame = soTarget.FindProperty("updateTextOncePerFrame");


            canBreakOutermostPrefab = soTarget.FindProperty("canBreakOutermostPrefab");
            //saveObjectInScene = soTarget.FindProperty("saveObjectInScene");
            debugLogs = soTarget.FindProperty("debugLogs");


            wordArray = soTarget.FindProperty("wordArray");

            meshPostProcess = soTarget.FindProperty("meshPostProcess");
        }
        void LoadFoldoutValues()
        {
            showMainSettingsInEditor = new AnimBool(true);
            showMainSettingsInEditor.valueChanged.AddListener(Repaint);

            showLayoutSettingsInEditor = new AnimBool(false);
            showLayoutSettingsInEditor.valueChanged.AddListener(Repaint);

            showModuleSettingsInEditor = new AnimBool(false);
            showModuleSettingsInEditor.valueChanged.AddListener(Repaint);

            showModuleRunSettingsInEditor = new AnimBool(false);
            showModuleRunSettingsInEditor.valueChanged.AddListener(Repaint);

            showAdvancedSettingsInEditor = new AnimBool(false);
            showAdvancedSettingsInEditor.valueChanged.AddListener(Repaint);

            showAdvancedInspectorSettingsInEditor = new AnimBool(false);
            showAdvancedInspectorSettingsInEditor.valueChanged.AddListener(Repaint);

            showAdvancedBehaviorSettingsInEditor = new AnimBool(false);
            showAdvancedBehaviorSettingsInEditor.valueChanged.AddListener(Repaint);

            showDebugSettingsInEditor = new AnimBool(false);
            showDebugSettingsInEditor.valueChanged.AddListener(Repaint);
        }


        #endregion Functions
    }
}