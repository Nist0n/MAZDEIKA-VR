﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.Linq;

#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif

using TinyGiantStudio.Layout;
using TinyGiantStudio.Modules;


namespace TinyGiantStudio.Text
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextUpdater))]
    [AddComponentMenu("Tiny Giant Studio/Modular 3D Text/3D Text", order: 200)]
    [HelpURL("https://ferdowsur.gitbook.io/modular-3d-text/text/modular-3d-text")]
    /// <summary>
    /// The base script to draw 3D texts
    /// </summary>
    public class Modular3DText : MonoBehaviour
    {
        #region Variable Declaration

        #region Main Variables

        //[TextArea] //creates unnecessary space at top in custom inspector
        [SerializeField] private string _text = string.Empty;
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
#if UNITY_EDITOR
                    if (debugLogs)
                        Debug.Log("Old Text is <color=green>" + _text + "</color>" + " new Text is <color=green>" + value + "</color>", gameObject);
#endif
                    if (value != null)
                        _text = value;
                    else
                        _text = string.Empty; //if the new value is null, set text to empty string

                    SetTextDirty();
                }

            }
        }

        /// <summary>
        /// This is used to check which letters need to be recreated/replaced by comparing to new text.
        /// </summary>
        public string oldText;
        string processedText;
        /// <summary>
        /// List of words in the text
        /// </summary>
        public string[] wordArray;



        /// <summary>
        /// Contains a list of all the character gameobject created by Text
        /// </summary>
        public List<GameObject> characterObjectList = new List<GameObject>();
        List<GameObject> objectsPendingModulesApply = new List<GameObject>(); //idk why visual studio suggesting to make it readonly. It won't work as readonly 

#if UNITY_EDITOR
        /// <summary>
        /// EDITOR ONLY!
        /// This holds all the reference for the all characters created to crosscheck if any characters are left over.
        /// This is due to unity editor not being able to delete/create without playmode on
        /// </summary>
        public List<GameObject> _allcharacterObjectList = new List<GameObject>();
#endif



        //Creation settings--------------------------------------------------------------------------------------
        public bool autoSaveMesh = false;

        //Main Settings------------------------------------------------------------------------------------------
        [SerializeField] private Font _font = null;
        public Font Font
        {
            get { return _font; }
            set
            {
                if (_font != value)
                {
#if UNITY_EDITOR
                    if (debugLogs)
                        Debug.Log("New font being set is <color=green>" + value + "</color>", gameObject);
#endif
                    oldText = "";
                    _font = value;
                    SetTextDirty();
                }
            }
        }


        [SerializeField] private Material _material;
        public Material Material
        {
            get { return _material; }
            set
            {
                if (_material != value)
                {
#if UNITY_EDITOR
                    if (debugLogs)
                        Debug.Log("Old material: " + _material + " & new one is: " + value + " on " + gameObject, gameObject);
#endif
                    oldText = "";
                    _material = value;
                    SetTextDirty();
                }
            }
        }


        [SerializeField] private Vector3 _fontSize = new Vector3(8, 8, 1);
        /// <summary>
        /// Assigning a new font size recreates the entire text. This is to avoid interfering with anything any module or usercreated code is doing.
        /// </summary>
        public Vector3 FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize != value)
                {
                    oldText = "";
                    _fontSize = value;
                    SetTextDirty();
                }
            }
        }









        [SerializeField] private bool _capitalize = false;
        public bool Capitalize
        {
            get { return _capitalize; }
            set
            {
                if (_capitalize != value)
                {
                    _capitalize = value; SetTextDirty();
                }
            }
        }

        [SerializeField] private bool _lowercase = false;
        public bool LowerCase
        {
            get { return _lowercase; }
            set
            {
                if (_lowercase != value)
                {
                    _lowercase = value;
                    SetTextDirty();
                }
            }
        }

        [SerializeField] bool _autoLetterSize = false;
        /// <summary>
        /// If turned on, instead of using the predetermined size of each letter, their size is taken from the size they take in the render view.
        /// </summary>
        public bool AutoLetterSize
        {
            get { return _autoLetterSize; }
            set
            {
                if (_autoLetterSize != value)
                {
                    oldText = string.Empty;
                    _autoLetterSize = value;
                    SetTextDirty();
                }
            }
        }

        [SerializeField] private float _wordSpacing = 1;
        public float WordSpacing
        {
            get { return _wordSpacing; }
            set
            {
                if (_wordSpacing != value)
                {
                    oldText = string.Empty;
                    _wordSpacing = value;
                    SetTextDirty();
                }
            }
        }


        //Spawn effects
        public bool useModules = true;

        /// <summary>
        /// If true, the adding module uses MonoBehavior attached to the character created to run its coroutine. This way, if the text is deactivated, the module isn't interrupted.
        /// </summary>
        public bool startAddingModuleFromChar = false;
        public List<ModuleContainer> addingModules = new List<ModuleContainer>();

        /// <summary>
        /// If true, the deleting module uses MonoBehavior attached to the char to run the coroutine. This way, if the text is deactivated, the module isn't interrupted.
        /// </summary>
        public bool startDeletingModuleFromChar = true;
        public List<ModuleContainer> deletingModules = new List<ModuleContainer>();
        public bool customDeleteAfterDuration = false;
        public float deleteAfter = 1;

        public bool applyModuleOnNewCharacter = true;
        [Tooltip("If turned on, adding modules will be automatically called when a prefab with existing text is instantiated on Start() instead of only when new character is added.")]
        public bool applyModulesOnStart = false;
        [Tooltip("If turned on, adding modules will be automatically called when a prefab with existing text is enabled instead of only when new character is added")]
        public bool applyModulesOnEnable = false;

        bool applyModuleFromStartOrEnable = false;

        //advanced settings-----------------------------------------------------------------------------------------------
        [Tooltip("When text is updated, old characters are moved to their correct position if their position is moved by something like module.")]
        public bool destroyChildObjectsWithGameObject = true;
        public bool repositionOldCharacters = true;
        public bool reApplyModulesToOldCharacters = false;
        //public bool activateChildObjects = true;



        //[Tooltip("Uses unity's Mesh.Combine method.\n" +
        //    "Unity has a limit of verticies one mesh can have which causes the bugs on large texts")]
        public bool combineMeshInEditor = true;
        public bool singleInPrefab = true;
        public bool combineMeshDuringRuntime = false;
        [Tooltip("Don't let letters show up in hierarchy in play mode. They are still there but not visible.")]
        public bool hideLettersInHierarchyInPlayMode = false;
        [Tooltip("If combine mesh is turned off")]
        public bool hideLettersInHierarchyInEditMode = false;

        [Tooltip("Breaks prefab connection while saving prefab location, can replace prefab at that location with a click")]
        public bool canBreakOutermostPrefab = false;
        //bool reconnectingPrefab = false;

        /// <summary>
        /// Where the prefab is saved. 
        /// <br>This is only for some edge cases that can be enabled via advanced settings.</br>
        /// </summary>
        public string assetPath = string.Empty;
        /// <summary>
        /// Where the mesh is saved if mesh save is turned on
        /// </summary>
        public List<string> meshPaths = new List<string>();

        private bool createChilds;
        public bool updateTextOncePerFrame = false;
        private bool runningRoutine = false;
        [SerializeField] private List<GameObject> extraLinesCreatedBecauseOfTooManyVerticies = new List<GameObject>();

        public MeshPostProcess meshPostProcess;


        #endregion Main Variables


        #region remember inspector layout/ Editor Stuff
#if UNITY_EDITOR
        /// <summary>
        /// This is for editor scripts. Don't use them
        /// This is used by textupdater to update the text incase text style was updated in prefab.
        /// This makes sure it doesnt update a tons unnecessarily
        /// </summary>
        [HideInInspector] public bool updatedAfterStyleUpdateOnPrefabInstances = true;

        [Tooltip("This is Editor Only.\nEven if it's turned on by accident in build, it won't print.\nNote: This will spam console logs.")]
        public bool debugLogs = false;
        /// <summary>
        /// Editor only. Do not use it on your script
        /// </summary>
        [HideInInspector] public bool hideOverwrittenVariablesFromInspector = true;
#endif
        #endregion remember inspector layout





        #endregion Variable Declaration



        #region Unity Stuff

        void Start()
        {
            ////applyModuleFromStartOrEnable to make sure both start and onenable don't call at once
            ///Since on enable is already calling it, no need to update the text twice by calling it in update
            if (!applyModulesOnEnable && applyModulesOnStart && !applyModuleFromStartOrEnable)
            {
                applyModuleFromStartOrEnable = true;
                CleanUpdateText();
            }
        }

        void OnEnable()
        {
            if (applyModulesOnEnable && !applyModuleFromStartOrEnable) //applyModuleFromStartOrEnable to make sure both start and onenable don't call at once
            {
                applyModuleFromStartOrEnable = true;
                CleanUpdateText();
            }
            else if (runningRoutine)
                UpdateText();

            runningRoutine = false;
        }

        void OnDisable()
        {
            if (runningRoutine)
            {
#if UNITY_EDITOR
                if (debugLogs)
                    Debug.Log("Updating text on OnDisable");
#endif
                UpdateText();
            }
        }

        void OnDestroy()
        {
            if (!gameObject.scene.isLoaded && !destroyChildObjectsWithGameObject)
                return;

            for (int i = characterObjectList.Count - 1; i > -1; --i)
            {
                DestroyObject(characterObjectList[i]);
            }
        }

        #endregion Unity Stuff



        /// <summary>
        /// Marks the text as dirty, needs to be cleaned up/Updated
        /// </summary>
        void SetTextDirty()
        {
#if UNITY_EDITOR
            //Editor mode
            if (!Application.isPlaying)
            {
                UpdateText();
                //This is because sometimes when list/button updates text style, it doesn't mark the scene as dirty
                EditorApplication.delayCall += () => SetEditorDirtyToSaveChanges(); //todo: don't set dirty on awake when a prefab is opened but no changes
            }
            //Play mode
            else
            {
                if (gameObject.activeInHierarchy && updateTextOncePerFrame)
                {
                    if (!runningRoutine)
                    {
                        runningRoutine = true;
                        StartCoroutine(UpdateRoutine());
                    }
                }
                else
                {
                    UpdateText();
                }
            }
#else
            if (gameObject.activeInHierarchy && updateTextOncePerFrame)
            {
                if (!runningRoutine)
                {
                    runningRoutine = true;
                    StartCoroutine(UpdateRoutine());
                }
            }
            else
            {
                UpdateText();
            }
#endif
        }


#if UNITY_EDITOR
        void SetEditorDirtyToSaveChanges()
        {
            if (!this)
                return;
            if (!gameObject)
                return;

            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(gameObject);

            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
        }
#endif

        /// <summary>
        /// The purpose of this coroutine is to make sure that texts aren't updated too many times in a single frame. But the downside is, it makes the text update after the end of the frame
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateRoutine()
        {
            yield return new WaitForEndOfFrame();
            UpdateText();
            runningRoutine = false;
        }

        /// <summary>
        /// This updates the complete text from scratch.
        /// <br></br>
        /// <br>Other methods of updating text will try to only update the ones that need to be updated</br>
        /// <br>Example: Adding the 'e' at the end of the existing text "nam" will try to add only 'e' without touching the other text unnecessarily.</br>
        /// This will completely remove the old letters and create the complete text.
        /// </summary>
        [ContextMenu("CleanUpdateText")]
        public void CleanUpdateText()
        {
            oldText = "";
            UpdateText();
        }

        public void UpdateText(string newText)
        {
            Text = newText;
        }
        /// <summary>
        /// Updates the text instantly with the number as text.
        /// </summary>
        /// <param name="number">The Text</param>
        public void UpdateText(float number)
        {
            Text = number.ToString();
        }
        /// <summary>
        /// Updates the text instantly with the number as text.
        /// </summary>
        /// <param name="number">The Text</param>
        public void UpdateText(int number)
        {
            Text = number.ToString();
        }



        public void UpdateText()
        {
#if UNITY_EDITOR
            ///in case of something like build is started the exact frame after update text is called, 
            ///the delayed call calls to update text when the scene doesn't exist(?) and gives a null reference just once. Has mo impact. Just looks ugly
            if (!this)
                return;
#endif
            if (!Font)
                return;



            for (int i = 0; i < extraLinesCreatedBecauseOfTooManyVerticies.Count; i++)
            {
                DestroyObject(extraLinesCreatedBecauseOfTooManyVerticies[i]);
            }
            extraLinesCreatedBecauseOfTooManyVerticies.Clear();


            processedText = ProcessText();
            //This block is for the layout system
            string delimiterChars = "([ \r\n])";
            wordArray = Regex.Split(processedText, delimiterChars);

            //TODO
            //This gives a better result but needs gridlayout group script to be refactored for this
            //string delimiterChars = "[^ \r\n]+|[ \r\n]";
            //wordArray = Regex.Matches(processedText, delimiterChars)
            //        .Cast<Match>()
            //        .Select(m => m.Value)
            //        .ToArray();

            int newCharacterStartsFrom = NewCharacterStartsFrom();
            int startCreatingCharacterFromIndex = 0;
            int startapplyingModulesFromIndex = newCharacterStartsFrom;

            createChilds = ShouldItCreateChild();
            if (createChilds)
            {
                //text had combined mesh before
                if (GetComponent<MeshRenderer>())
                {
                    DestroyMeshRenderAndMeshFilter();
                }
                else
                {
                    startCreatingCharacterFromIndex = newCharacterStartsFrom;
                }
            }

            oldText = processedText;

            DeleteReplacedChars(startCreatingCharacterFromIndex);

            if (!createChilds)
            {
                CreateSingleMesh();
            }
            else
            {
                CreateNewChacracters(startCreatingCharacterFromIndex, startapplyingModulesFromIndex);

                if (GetComponent<LayoutGroup>())
                {
                    if (repositionOldCharacters)
                        GetComponent<LayoutGroup>().UpdateLayout(0);
                    else
                        GetComponent<LayoutGroup>().UpdateLayout(startCreatingCharacterFromIndex);
                }

                ApplyAllPendingModules();

#if UNITY_EDITOR
                if (!Application.isPlaying)
                    EditorApplication.delayCall += () => CheckLeftOversInEditorAndCleanUp();

                RemoveBlink(); //This is used remove a single frame in editor where texts would show up even if the gameobject was disabled
#endif
            }
#if UNITY_EDITOR
            if (!createChilds && autoSaveMesh)
            {
                SaveMeshAsAsset(false);
            }
#endif
        }


        void CreateSingleMesh()
        {
            if (processedText.Length == 0)
            {
                if (GetComponent<MeshFilter>())
                    GetComponent<MeshFilter>().sharedMesh = null;

                return;
            }

            LayoutGroup layoutGroup = GetComponent<LayoutGroup>();

            if (layoutGroup == null)
                return;

            List<MeshLayout> meshLayouts = new List<MeshLayout>();

            for (int i = 0; i < processedText.Length; i++)
            {
                meshLayouts.Add(GetCharacterObject.GetMeshLayout(processedText[i], this, meshPostProcess));
            }

            meshLayouts = layoutGroup.GetPositions(meshLayouts);

            List<Mesh> combinedMeshes = MeshCombiner.CombinedMesh(meshLayouts, transform, GetChildSize());

            ApplyCombinedMesh(combinedMeshes);
        }

        void ApplyCombinedMesh(List<Mesh> combinedMeshes)
        {

#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                if (!GetComponent<MeshFilter>())
                    gameObject.AddComponent<MeshFilter>();
            }
            else
            {
                if (!GetComponent<MeshFilter>())
                    Undo.AddComponent<MeshFilter>(gameObject);
            }
#else
            if (!GetComponent<MeshFilter>())
                gameObject.AddComponent<MeshFilter>();
#endif

#if UNITY_EDITOR
            if (!Application.isPlaying)
                Undo.RecordObject(GetComponent<MeshFilter>(), "Update text");
#endif
            GetComponent<MeshFilter>().mesh = combinedMeshes[0];

            if (combinedMeshes.Count > 1)
            {
                for (int i = 1; i < combinedMeshes.Count; i++)
                {
                    if (combinedMeshes[i] == null)
                        continue;

                    GameObject obj = new GameObject("Extra text " + i, typeof(MeshFilter), typeof(MeshRenderer), typeof(LayoutElement));
                    obj.transform.SetParent(transform, false);

                    //obj.transform.localPosition = Vector3.zero;
                    //obj.transform.rotation = Quaternion.identity;
                    //obj.transform.localScale = Vector3.one;

                    obj.GetComponent<MeshFilter>().sharedMesh = combinedMeshes[i];
                    obj.GetComponent<MeshRenderer>().material = Material;

                    extraLinesCreatedBecauseOfTooManyVerticies.Add(obj);
                }
            }

#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                if (!GetComponent<MeshRenderer>())
                    gameObject.AddComponent<MeshRenderer>();
            }
            else
            {
                if (!GetComponent<MeshRenderer>())
                    Undo.AddComponent<MeshRenderer>(gameObject);

                Undo.RecordObject(GetComponent<MeshRenderer>(), "Text update");
            }
#else
            if (!GetComponent<MeshRenderer>())
                gameObject.AddComponent<MeshRenderer>();
#endif

            GetComponent<MeshRenderer>().material = Material;
        }


#if UNITY_EDITOR
        /// <summary>
        /// This is used remove a single frame in editor where texts would show up even if the gameobject was disabled
        /// Problem: Characters creator for deactivated texts get created, set the text as parent and gets activated.
        ///          The characters get deactivated in hierarchy next frame because the parent is deactivated
        ///          But, Since it takes a single frame, the characters appear in editor for a frame causing a blink affect
        ///          
        /// So, they are manually disabled
        /// </summary>
        void RemoveBlink()
        {
            if (!Application.isPlaying)
            {
                if (!transform.gameObject.activeInHierarchy)
                {
                    for (int i = 0; i < characterObjectList.Count; i++)
                    {
                        GameObject t = characterObjectList[i];
                        t.SetActive(false);
                        EditorApplication.delayCall += () => DeactivateObject(t);
                    }
                }
            }
        }

        void DeactivateObject(GameObject gObj)
        {
            if (gObj)
                gObj.SetActive(true);
        }
#endif

        void CreateNewChacracters(int newCharStartsFrom, int startapplyingModulesFromIndex)
        {
            for (int i = newCharStartsFrom; i < processedText.Length; i++)
            {
                bool applyModuleNow = ApplyModuleToThisCharacterNow(i, startapplyingModulesFromIndex);
                //bool applyModuleNow = false;
                //if (i >= startapplyingModulesFromIndex)
                //    applyModuleNow = true;

                if (i > 0)
                    CreateThisChar(processedText[i - 1], processedText[i], applyModuleNow);
                else
                    CreateThisChar(processedText[i], applyModuleNow);
            }
            applyModuleFromStartOrEnable = false;
        }

        bool ApplyModuleToThisCharacterNow(int currentCharacterIndex, int startApplyingModulesFromIndex)
        {
            if (!useModules) //Completely stops modules
                return false;

            if (currentCharacterIndex < startApplyingModulesFromIndex) //Old character repositioning, don't apply module
                return false;

            if (applyModuleFromStartOrEnable) //Running modules on start/enable
                return true;

            if (applyModuleOnNewCharacter) //Running modules on new character
                return true;

            return false;
        }





        string ProcessText()
        {
            if (Capitalize)
                return Text.ToUpper();
            else if (LowerCase)
                return Text.ToLower();

            return Text;
        }




#if UNITY_EDITOR
        /// <summary>
        /// Once in a while if compile is called multiple times again before first call to compile is finished, 
        /// the delaycalls to create 3D meshes get inturrapted and gets stuck in limbo. 
        /// This is to clean them up
        /// </summary>
        void CheckLeftOversInEditorAndCleanUp()
        {
            for (int i = _allcharacterObjectList.Count - 1; i >= 0; i--)
            {
                if (_allcharacterObjectList[i] == null)
                {
                    _allcharacterObjectList.Remove(_allcharacterObjectList[i]);
                    continue;
                }

                //todo:Shouldn't this be opposite
                if (!characterObjectList.Contains(_allcharacterObjectList[i]))
                {
                    GameObject obj = _allcharacterObjectList[i];
                    EditorApplication.delayCall += () =>
                    {
                        CleanUpDelete(obj);
                    };
                }
            }

            if (!hideLettersInHierarchyInEditMode && !Application.isPlaying)
            {
                try
                {
                    //if (this)
                    {
                        foreach (Transform child in transform)
                        {
                            child.hideFlags = HideFlags.None;
                        }
                    }
                }
                catch
                {

                }
            }
        }
        void CleanUpDelete(GameObject obj)
        {
            try { DestroyImmediate(obj); }
            catch { }
        }
#endif

        void DestroyMeshRenderAndMeshFilter()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                if (GetComponent<MeshRenderer>() != null)
                    Undo.DestroyObjectImmediate(GetComponent<MeshRenderer>());
                if (GetComponent<MeshFilter>() != null)
                    Undo.DestroyObjectImmediate(GetComponent<MeshFilter>());
                //EditorApplication.delayCall += () => DestroyImmediate(GetComponent<MeshRenderer>());
                //EditorApplication.delayCall += () => DestroyImmediate(GetComponent<MeshFilter>());
            }
            else
            {
                if (GetComponent<MeshRenderer>() != null)
                    Destroy(GetComponent<MeshRenderer>());
                if (GetComponent<MeshFilter>() != null)
                    Destroy(GetComponent<MeshFilter>());
            }
#else
            if (GetComponent<MeshRenderer>() != null)
                Destroy(GetComponent<MeshRenderer>());
            if (GetComponent<MeshFilter>() != null)
                Destroy(GetComponent<MeshFilter>());
#endif
        }

        /// <summary>
        /// True = Create child
        /// </summary>
        /// <returns></returns>
        public bool ShouldItCreateChild()
        {
            bool createChilds = false;

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                if (!combineMeshInEditor)
                {
                    if (!this)
                        return true;

                    //not a prefab instance or prefab instance and single in prefab
                    if (!PrefabUtility.IsPartOfPrefabInstance(gameObject) || (PrefabUtility.IsPartOfPrefabInstance(gameObject) && !singleInPrefab))
                    {
                        createChilds = true;
                    }
                    else if (canBreakOutermostPrefab && PrefabBreakable())
                    {
                        RemovePrefabConnection();
                        createChilds = true;
                    }
                }
            }
            else if (!combineMeshDuringRuntime)
            {
                createChilds = true;
            }
#else
            if (!combineMeshDuringRuntime)
            {
                createChilds = true;
            }
#endif
            return createChilds;
        }


        int NewCharacterStartsFrom()
        {
            int newCharStartsFrom = 0;
            if (oldText == null)//this happens when text is created runtime
            {
                return 0;
            }

            for (int i = 0; i < processedText.Length; i++)
            {
                if (oldText.Length < i)//this can only happen inc edge cases like a clean new download without removing the text scripts in scene that was unopned after deletion of the asset
                    return i;

                //less character than before
                if (i >= oldText.Length)
                {
                    return (newCharStartsFrom);
                }

                //character got replaced
                if (processedText[i] != oldText[i])
                {
                    return (newCharStartsFrom);
                }

                newCharStartsFrom++;
            }
            return newCharStartsFrom;
        }

        void DeleteReplacedChars(int startingFrom)
        {
            int toDeleteCount = characterObjectList.Count - startingFrom;
#if UNITY_EDITOR
            if (debugLogs)
                Debug.Log("To delete : <color=green>" + toDeleteCount + "</color> chars on " + gameObject, this);
#endif
            if (toDeleteCount <= 0)
                return;

            List<GameObject> toDelete = new List<GameObject>();
            for (int i = startingFrom; i < characterObjectList.Count; i++)
            {
                toDelete.Add(characterObjectList[i]);
            }

            foreach (GameObject child in toDelete)
            {
#if UNITY_EDITOR
                if (debugLogs)
                    Debug.Log("Destroy object is being called for : <color=green>" + child + "</color> chars on " + gameObject, this);

#endif
                DestroyObject(child);
                characterObjectList.Remove(child);
            }
        }

        void DestroyObject(GameObject obj)
        {
            if (!obj)
                return;

            if (characterObjectList.Contains(obj))
                characterObjectList.Remove(obj);

#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                if (gameObject.activeInHierarchy)
                    DestroyObjectRuntime(obj);
                else
                    RunTimeDestroyObjectOnDisabledText(obj);
            }
            else
            {
                if (!PrefabUtility.IsPartOfAnyPrefab(obj))
                    Undo.DestroyObjectImmediate(obj); //if this causes error, please contact support and undo comment from the code below
            }
#else
            if (gameObject.activeInHierarchy)
                DestroyObjectRuntime(obj);
            else
                RunTimeDestroyObjectOnDisabledText(obj);
#endif
        }

        void DestroyObjectRuntime(GameObject obj)
        {
#if UNITY_EDITOR
            if (debugLogs)
                Debug.Log("RunTimeDestroyObjectRoutine is being called for : <color=yellow>" + obj + "</color> chars on " + gameObject, this);
#endif
            obj.transform.SetParent(null);

            if (obj.name != "Space" && useModules)
            {
                float delay = !customDeleteAfterDuration && deletingModules.Count > 0 ? GetDeleteDurationFromEffects() : customDeleteAfterDuration ? deleteAfter : 0;

                obj.GetComponent<MonoBehaviour>().StopAllCoroutines();

                for (int i = 0; i < deletingModules.Count; i++)
                {
                    if (deletingModules[i].module)
                    {
                        if (startDeletingModuleFromChar)
                            obj.GetComponent<MonoBehaviour>().StartCoroutine(deletingModules[i].module.ModuleRoutine(obj, deletingModules[i].variableHolders));
                        else
                            StartCoroutine(deletingModules[i].module.ModuleRoutine(obj, deletingModules[i].variableHolders));
                    }
                }

                Destroy(obj, delay);
            }
            else
            {
                Destroy(obj);
            }
        }

        public float GetDeleteDurationFromEffects()
        {
            float max = 0;
            for (int i = 0; i < deletingModules.Count; i++)
            {
                float duration = 0;

                if (deletingModules[i] == null)
                    continue;

                if (deletingModules[i].module != null)
                {
                    if (deletingModules[i].module.variableHolders == null)
                        continue;

                    if (deletingModules[i].module.variableHolders.Length == 0)
                        continue;

                    if (deletingModules[i].variableHolders.Length == 0)
                        continue;

                    if (deletingModules[i].variableHolders[0] == null)
                        continue;

                    if (deletingModules[i].module.variableHolders[0].variableName == "Delay" || deletingModules[i].module.variableHolders[0].variableName == "Duration")
                    {
                        duration += deletingModules[i].variableHolders[0].floatValue;
                    }

                    if (deletingModules[i].module.variableHolders.Length > 1)
                    {
                        if (deletingModules[i].module.variableHolders[1].variableName == "Delay" || deletingModules[i].module.variableHolders[1].variableName == "Duration")
                        {
                            duration += deletingModules[i].variableHolders[1].floatValue;
                        }
                    }

                }

                if (duration > max)
                    max = duration;
            }
            return max;
        }

        private void RunTimeDestroyObjectOnDisabledText(GameObject obj) => Destroy(obj);




        void CreateThisChar(char previousChar, char currentChar, bool applyModule)
        {
            if (!this)
                return;

            GameObject obj = GetCharacterObject.GetObject(previousChar, currentChar, this, meshPostProcess);
            AddCharacterToList(obj);
            obj.transform.SetParent(transform);
            ApplyStyle(obj);

            if (Application.isPlaying && applyModule)
                objectsPendingModulesApply.Add(obj);
            //ApplyEffects(obj);

            //if (!saveObjectInScene)
            //    obj.hideFlags = HideFlags.DontSave;
#if UNITY_EDITOR
            if (Application.isPlaying)
                Undo.RecordObject(obj, "Update text");
#endif
            obj.SetActive(true);
        }


        void CreateThisChar(char currentChar, bool applyModule)
        {
            if (!this)
                return;

            GameObject obj = GetCharacterObject.GetObject(currentChar, this, meshPostProcess);
            AddCharacterToList(obj);
            obj.transform.SetParent(transform);
            ApplyStyle(obj);

            if (Application.isPlaying && applyModule)
                objectsPendingModulesApply.Add(obj);
            //ApplyEffects(obj);

            //if (!saveObjectInScene)
            //    obj.hideFlags = HideFlags.DontSave;
#if UNITY_EDITOR
            if (Application.isPlaying)
                Undo.RecordObject(obj, "Update text");
#endif
            obj.SetActive(true);
        }


        #region Prepare Character

        void AddCharacterToList(GameObject obj) => characterObjectList.Add(obj);
        #endregion

        void ApplyAllPendingModules()
        {
            for (int i = 0; i < objectsPendingModulesApply.Count; i++)
            {
                ApplyEffects(objectsPendingModulesApply[i]);
            }
            objectsPendingModulesApply.Clear();
        }
        private void ApplyEffects(GameObject obj)
        {
            if (obj == null)
                return;

            if (!gameObject.activeInHierarchy || !useModules)
                return;
            if (obj.name != "space")
            {
                for (int i = 0; i < addingModules.Count; i++)
                {
                    if (addingModules[i].module)
                    {
                        if (startAddingModuleFromChar)
                            obj.GetComponent<MonoBehaviour>().StartCoroutine(addingModules[i].module.ModuleRoutine(obj, addingModules[i].variableHolders));
                        else
                            StartCoroutine(addingModules[i].module.ModuleRoutine(obj, addingModules[i].variableHolders));
                    }
                }
            }
        }
        private void ApplyStyle(GameObject obj)
        {
            if (obj.GetComponent<Letter>())
            {
                if (obj.GetComponent<Letter>().model)
                {
                    obj.GetComponent<Letter>().model.material = Material;
                }
            }
            if (obj.GetComponent<MeshFilter>())
            {
                if (!obj.GetComponent<MeshRenderer>())
                    obj.AddComponent<MeshRenderer>();

                obj.GetComponent<MeshRenderer>().material = Material;
            }



            obj.transform.localScale = GetChildSize();
            obj.transform.localRotation = Quaternion.identity;

#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
                obj.layer = gameObject.layer;
            else
            {
                try
                {
                    EditorApplication.delayCall += () => SetLayer(obj);
                }
                catch
                {

                }
            }
#else
            SetLayer(obj);
#endif
        }

        Vector3 GetChildSize() => new Vector3(FontSize.x, FontSize.y, FontSize.z / 2);


        private void SetLayer(GameObject obj)
        {
            if (!this)
                return;

            if (obj)
                obj.layer = gameObject.layer;
        }


        //TODO
        public void EmptyEffect(List<ModuleContainer> moduleList)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                Undo.RecordObject(this, "Update text");
#endif
            ModuleContainer module = new ModuleContainer();
            moduleList.Add(module);
        }
        public void NewEffect(List<ModuleContainer> moduleList, Modules.Module newModule)
        {
            ModuleContainer module = new ModuleContainer
            {
                module = newModule
            };
            moduleList.Add(module);
        }











        #region Utility
#if UNITY_EDITOR
        /// <summary>
        /// Editor only. 
        /// Checks if the processed text is the same as new text.
        /// Used in editor
        /// </summary>
        /// <returns></returns>
        public bool ProcessedTextIsCorrect()
        {
            return ProcessText() == processedText;
        }

        public bool PrefabBreakable()
        {
            if (!EditorApplication.isPlaying)
            {
                if (PrefabUtility.IsPartOfPrefabInstance(gameObject))
                {
                    if (!PrefabUtility.IsOutermostPrefabInstanceRoot(gameObject))
                        return false;
                    if (PrefabUtility.IsPartOfVariantPrefab(gameObject))
                        return false;
                }
                return true;
            }
            else
            {
                return true;
            }
        }
        public void ReconnectPrefabs()
        {
            //reconnectingPrefab = true;
            PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, assetPath, InteractionMode.AutomatedAction);
        }
        public void SaveMeshAsAsset(bool saveAsDifferent)
        {
            if (!EditorApplication.isPlaying)
            {
                bool canceledAction = false;

                //gets save path from explorer
                if (!HasSavePath() || saveAsDifferent)
                {
                    canceledAction = GetPaths();
                }
                if (!canceledAction)
                    SaveAsset();
            }
        }
        private void RemovePrefabConnection()
        {
            assetPath = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromSource(gameObject));
            PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }

        private void SaveAsset()
        {
            if (GetComponent<MeshFilter>())
            {
                if (meshPaths.Count == 0)
                    return;

                //not trying to overwrite with same mesh
                if (AssetDatabase.LoadAssetAtPath(meshPaths[0], typeof(Mesh)) == GetComponent<MeshFilter>().sharedMesh)
                {
                    //Debug.Log("<color=green>The current mesh is already the asset at selected location. No need to overwrite.</color>");
                }
                else
                {
                    if (!GetComponent<MeshFilter>().sharedMesh)
                        return;

                    AssetDatabase.CreateAsset(GetComponent<MeshFilter>().sharedMesh, meshPaths[0]);
                    AssetDatabase.SaveAssets();
                }
            }

            for (int i = 0; i < characterObjectList.Count; i++)
            {
                if (characterObjectList[i])
                {
                    if (!characterObjectList[i].GetComponent<MeshFilter>())
                        break;

                    //not trying to overwrite with same mesh
                    if (AssetDatabase.LoadAssetAtPath(meshPaths[i], typeof(Mesh)) == characterObjectList[i].GetComponent<MeshFilter>().sharedMesh)
                    {
                        //Debug.Log("<color=green>The current mesh is already the asset at selected location. No need to overwrite.</color>");
                    }
                    else
                    {
                        AssetDatabase.CreateAsset(characterObjectList[i].GetComponent<MeshFilter>().sharedMesh, meshPaths[i + 1]); //path i+1 because 0 is taken by main object
                        AssetDatabase.SaveAssets();
                    }
                }
            }
        }

        private bool HasSavePath()
        {
            if (meshPaths.Count > 0)
            {
                if (string.IsNullOrEmpty(meshPaths[0]))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private bool GetPaths()
        {
            meshPaths.Clear();
            for (int i = 0; i < characterObjectList.Count + 1; i++)
            {
                string meshNumber;
                if (i == 0) meshNumber = string.Empty;
                else meshNumber = "mesh " + i;

                string path = EditorUtility.SaveFilePanel("Save Separate Mesh" + i + " Asset", "Assets/", name + meshNumber, "asset");

                if (string.IsNullOrEmpty(path))
                    return true;
                else
                    path = FileUtil.GetProjectRelativePath(path);

                meshPaths.Add(path);
            }
            return false;
        }
#endif
        #endregion Utility



        #region Text in Button/List
        /// <summary>
        /// Used by editor to write the values overwritten info message in the info box and hide property in inspector 
        /// </summary>
        /// <returns></returns>
        public bool DoesStyleInheritFromAParent()
        {
            if (transform.parent)
            {
                Button button = transform.parent.gameObject.GetComponent<Button>();
                if (button)
                {
                    if (button.Text == this)
                    {
                        if (button.ApplyNormalStyle().Item1 || button.ApplyNormalStyle().Item2)
                        //if (button.useStyles)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        #endregion Text in Button/List



#if UNITY_EDITOR
        ///For inspector only
        public List<Type> GetListOfAllLayoutGroups()
        {
            List<Type> groups = new List<Type>();

            foreach (Type t in FindDerivedTypes(Assembly.GetExecutingAssembly(), typeof(LayoutGroup)))
            {
                if (!t.IsAbstract)
                {
                    groups.Add(t);
                }
            }

            return groups;
        }

        public IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
        {
            return assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t));
        }
#endif
    }
}