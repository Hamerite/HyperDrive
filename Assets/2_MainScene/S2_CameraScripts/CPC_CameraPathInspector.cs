using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public enum CPC_EManipulationModes { Free, SelectAndTransform }

public enum CPC_ENewWaypointMode { MainCamera, SceneCamera, LastWaypoint, WaypointIndex, WorldCenter }

[CustomEditor(typeof(CPC_CameraPath))]
public class CPC_CameraPathInspector : Editor {
    protected CPC_CameraPath CPCPathScript;
    protected ReorderableList pointReorderableList;
    protected AnimationCurve allAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    protected SerializedObject serializedObjectTarget;
    protected SerializedProperty selectedCameraProperty;
    protected SerializedProperty lookAtTargetTransformProperty;
    protected SerializedProperty visualPathProperty, visualInactivePathProperty, visualFrustumProperty, visualHandleProperty;
    protected SerializedProperty loopedProperty, afterLoopProperty;
    protected SerializedProperty alwaysShowProperty;
    protected SerializedProperty timer;

    protected GUIContent addPointContent = new GUIContent("Add Point", "Adds a waypoint at the scene view camera's position/rotation");
    protected GUIContent testButtonContent = new GUIContent("Test", "Only available in play mode");
    protected GUIContent pauseButtonContent = new GUIContent("Pause", "Paused Camera at current Position");
    protected GUIContent continueButtonContent = new GUIContent("Continue", "Continues Path at current position");
    protected GUIContent stopButtonContent = new GUIContent("Stop", "Stops the playback");
    protected GUIContent deletePointContent = new GUIContent("X", "Deletes this waypoint");
    protected GUIContent gotoPointContent = new GUIContent("Goto", "Teleports the scene camera to the specified waypoint");
    protected GUIContent relocateContent = new GUIContent("Relocate", "Relocates the specified camera to the current view camera's position/rotation");
    protected GUIContent alwaysShowContent = new GUIContent("Always show", "When true, shows the curve even when the GameObject is not selected - \"Inactive cath color\" will be used as path color instead");
    protected GUIContent chainedContent = new GUIContent("o───o", "Toggles if the handles of the specified waypoint should be chained (mirrored) or not");
    protected GUIContent unchainedContent = new GUIContent("o─x─o", "Toggles if the handles of the specified waypoint should be chained (mirrored) or not");
    protected GUIContent replaceAllPositionContent = new GUIContent("Replace all position lerps", "Replaces curve types (and curves when set to \"Custom\") of all the waypoint position lerp types with the specified values");
    protected GUIContent replaceAllRotationContent = new GUIContent("Replace all rotation lerps", "Replaces curve types (and curves when set to \"Custom\") of all the waypoint rotation lerp types with the specified values");
    protected GUIContent AddTimer = new GUIContent("Add Time", "Adds timer to the camera");

    protected CPC_EManipulationModes cameraTranslateMode, handlePositionMode, cameraRotationMode;
    protected CPC_ENewWaypointMode waypointMode;
    protected CPC_ECurveType allCurveType = CPC_ECurveType.Custom;

    protected bool hasScrollBar, visualFoldout, manipulationFoldout, showRawValues;
    protected int selectedIndex = -1, waypointIndex = 1;
    protected float time, currentTime, previousTime;

    void OnEnable() {
        EditorApplication.update += Update;

        CPCPathScript = (CPC_CameraPath)target;
        if (CPCPathScript == null) return;

        SetupEditorVariables();
        GetVariableProperties();
        SetupReorderableList();
    }

    void OnDisable() { EditorApplication.update -= Update; }

    void Update() {
        if (CPCPathScript == null) return;
        
        currentTime = CPCPathScript.CurrentWaypointIndex + CPCPathScript.CurrentTimeInWaypoint;
        if (Math.Abs(currentTime - previousTime) > 0.0001f) {
            Repaint();
            previousTime = currentTime;
        }
    }

    public override void OnInspectorGUI() {
        serializedObjectTarget.Update();
        DrawPlaybackWindow();

        Rect scale = GUILayoutUtility.GetLastRect();
        hasScrollBar = (Screen.width - scale.width <= 12);

        GUILayout.Space(5); GUILayout.Box("", GUILayout.Width(Screen.width - 20), GUILayout.Height(3));
        GUILayout.Space(5); DrawBasicSettings();

        GUILayout.Space(5); 
        GUILayout.Box("", GUILayout.Width(Screen.width - 20), GUILayout.Height(3));
        DrawVisualDropdown();
        GUILayout.Box("", GUILayout.Width(Screen.width - 20), GUILayout.Height(3));
        DrawManipulationDropdown();
        GUILayout.Box("", GUILayout.Width(Screen.width - 20), GUILayout.Height(3));

        GUILayout.Space(10); DrawWaypointList();
        GUILayout.Space(10); DrawRawValues();
        serializedObjectTarget.ApplyModifiedProperties();
    }

    void OnSceneGUI() {
        if (CPCPathScript.GetPoints().Count >= 2) {
            for (int i = 0; i < CPCPathScript.GetPoints().Count; i++) {
                DrawHandles(i);
                Handles.color = Color.white;
            }
        }
    }

    void SelectIndex(int index) {
        selectedIndex = index;
        pointReorderableList.index = index;
        Repaint();
    }

    void SetupEditorVariables() {
        cameraTranslateMode = (CPC_EManipulationModes)PlayerPrefs.GetInt("CPC_cameraTranslateMode", 1);
        cameraRotationMode = (CPC_EManipulationModes)PlayerPrefs.GetInt("CPC_cameraRotationMode", 1);
        handlePositionMode = (CPC_EManipulationModes)PlayerPrefs.GetInt("CPC_handlePositionMode", 0);
        waypointMode = (CPC_ENewWaypointMode)PlayerPrefs.GetInt("CPC_waypointMode", 0);
        time = PlayerPrefs.GetFloat("CPC_time", 10);
    }

    void GetVariableProperties() {
        serializedObjectTarget = new SerializedObject(CPCPathScript);
        selectedCameraProperty = serializedObjectTarget.FindProperty("selectedCamera");
        lookAtTargetTransformProperty = serializedObjectTarget.FindProperty("target");
        visualPathProperty = serializedObjectTarget.FindProperty("visual.pathColor");
        visualInactivePathProperty = serializedObjectTarget.FindProperty("visual.inactivePathColor");
        visualFrustumProperty = serializedObjectTarget.FindProperty("visual.frustrumColor");
        visualHandleProperty = serializedObjectTarget.FindProperty("visual.handleColor");
        loopedProperty = serializedObjectTarget.FindProperty("looped");
        alwaysShowProperty = serializedObjectTarget.FindProperty("alwaysShow");
        afterLoopProperty = serializedObjectTarget.FindProperty("afterLoop");
        timer = serializedObjectTarget.FindProperty("m_Timer");
    }

    void SetupReorderableList() {
        pointReorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("points"), true, true, false, false);
        pointReorderableList.elementHeight *= 2;

        pointReorderableList.drawElementCallback = (rect, index, active, focused) => {
            float startRectY = rect.y;
            if (index > CPCPathScript.GetPoints().Count - 1) return;

            rect.height -= 2;
            float fullWidth = rect.width - 16 * (hasScrollBar ? 1 : 0);
            rect.width = 40;
            fullWidth -= 40;
            rect.height /= 2;
            GUI.Label(rect, "#" + (index + 1));

            rect.y += rect.height - 3;
            rect.x -= 14;
            rect.width += 12;
            if (GUI.Button(rect, CPCPathScript.GetPoints()[index].chained ? chainedContent : unchainedContent)) {
                Undo.RecordObject(CPCPathScript, "Changed chain type");
                CPCPathScript.GetPoints()[index].chained = !CPCPathScript.GetPoints()[index].chained;
            }
            
            // Position
            rect.x += rect.width + 2;
            rect.y = startRectY;
            rect.width = (fullWidth - 22) / 3 - 1;
            EditorGUI.BeginChangeCheck();
            CPC_ECurveType tempP = (CPC_ECurveType)EditorGUI.EnumPopup(rect, CPCPathScript.GetPoints()[index].curveTypePosition);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(CPCPathScript, "Changed enum value");
                CPCPathScript.GetPoints()[index].curveTypePosition = tempP;
            }

            rect.y += pointReorderableList.elementHeight / 2 - 4;
            rect.x += rect.width + 2;
            EditorGUI.BeginChangeCheck();
            GUI.enabled = CPCPathScript.GetPoints()[index].curveTypePosition == CPC_ECurveType.Custom;
            AnimationCurve tempACP = EditorGUI.CurveField(rect, CPCPathScript.GetPoints()[index].positionCurve);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(CPCPathScript, "Changed curve");
                CPCPathScript.GetPoints()[index].positionCurve = tempACP;
            }
            
            // Rotation
            GUI.enabled = true;
            rect.x += rect.width + 2;
            rect.y = startRectY;
            rect.width = (fullWidth - 22) / 3 - 1;
            EditorGUI.BeginChangeCheck();
            CPC_ECurveType temp = (CPC_ECurveType)EditorGUI.EnumPopup(rect, CPCPathScript.GetPoints()[index].curveTypeRotation);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(CPCPathScript, "Changed enum value");
                CPCPathScript.GetPoints()[index].curveTypeRotation = temp;
            }
            
            rect.y += pointReorderableList.elementHeight / 2 - 4;
            rect.height /= 2;
            rect.x += rect.width + 2;
            EditorGUI.BeginChangeCheck();
            GUI.enabled = CPCPathScript.GetPoints()[index].curveTypeRotation == CPC_ECurveType.Custom;
            AnimationCurve tempAC = EditorGUI.CurveField(rect, CPCPathScript.GetPoints()[index].rotationCurve);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(CPCPathScript, "Changed curve");
                CPCPathScript.GetPoints()[index].rotationCurve = tempAC;
            }
            
            GUI.enabled = true;
            rect.y = startRectY;
            rect.height *= 2;
            rect.x += rect.width + 2;
            rect.width = (fullWidth - 22) / 3;
            rect.height = rect.height / 2 - 1;
            if (GUI.Button(rect, gotoPointContent)) {
                pointReorderableList.index = index;
                selectedIndex = index;
                SceneView.lastActiveSceneView.pivot = CPCPathScript.GetPoints()[pointReorderableList.index].position;
                SceneView.lastActiveSceneView.size = 3;
                SceneView.lastActiveSceneView.Repaint();
            }
            
            rect.y += rect.height + 2;
            if (GUI.Button(rect, AddTimer)) {
                pointReorderableList.index = index;
                selectedIndex = index;
            }
            
            if (GUI.Button(rect, relocateContent)) {
                Undo.RecordObject(CPCPathScript, "Relocated waypoint");
                pointReorderableList.index = index;
                selectedIndex = index;
                CPCPathScript.GetPoints()[pointReorderableList.index].position = SceneView.lastActiveSceneView.camera.transform.position;
                CPCPathScript.GetPoints()[pointReorderableList.index].rotation = SceneView.lastActiveSceneView.camera.transform.rotation;
                SceneView.lastActiveSceneView.Repaint();
            }
            
            rect.height = (rect.height + 1) * 2;
            rect.y = startRectY;
            rect.x += rect.width + 2;
            rect.width = 20;
            if (GUI.Button(rect, deletePointContent)){
                Undo.RecordObject(CPCPathScript, "Deleted a waypoint");
                CPCPathScript.GetPoints().Remove(CPCPathScript.GetPoints()[index]);
                SceneView.RepaintAll();
            }
        };

        pointReorderableList.drawHeaderCallback = rect => {
            float fullWidth = rect.width;
            rect.width = 56;
            GUI.Label(rect, "Sum: " + CPCPathScript.GetPoints().Count);

            rect.x += rect.width;
            rect.width = (fullWidth - 78) / 3;
            GUI.Label(rect, "Position Lerp");

            rect.x += rect.width;
            GUI.Label(rect, "Rotation Lerp");

            rect.x += rect.width * 2;
            GUI.Label(rect, "Del.");
        };

        pointReorderableList.onSelectCallback = l => {
            selectedIndex = l.index;
            SceneView.RepaintAll();
        };
    }

    void DrawPlaybackWindow() {
        GUI.enabled = Application.isPlaying;
        GUILayout.BeginVertical("Box");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(testButtonContent)) { /*CPCPathScript.PlayPath(time);*/ }

        if (!CPCPathScript.Paused) {
            if (Application.isPlaying && !CPCPathScript.Playing) GUI.enabled = false;
            if (GUILayout.Button(pauseButtonContent)) CPCPathScript.SetPaused(true);
        }
        else if (GUILayout.Button(continueButtonContent)) CPCPathScript.SetPaused(false);

        if (GUILayout.Button(stopButtonContent)) /*CPCPathScript.StopPath();*/

        GUI.enabled = true;
        EditorGUI.BeginChangeCheck();
        GUILayout.Label("Time (seconds)");
        time = EditorGUILayout.FloatField("", time, GUILayout.MinWidth(5), GUILayout.MaxWidth(50));
        if (EditorGUI.EndChangeCheck()) {
            time = Mathf.Clamp(time, 0.001f, Mathf.Infinity);
            CPCPathScript.UpdateTimeInSeconds(time);
            PlayerPrefs.SetFloat("CPC_time", time);
        }
        GUILayout.EndHorizontal();

        GUI.enabled = Application.isPlaying;
        EditorGUI.BeginChangeCheck();
        currentTime = EditorGUILayout.Slider(currentTime, 0, CPCPathScript.GetPoints().Count - ((CPCPathScript.GetLooped()) ? 0.01f : 1.01f));
        if (EditorGUI.EndChangeCheck()) {
            CPCPathScript.CurrentWaypointIndex = Mathf.FloorToInt(currentTime);
            CPCPathScript.CurrentTimeInWaypoint = currentTime % 1;
            CPCPathScript.RefreshTransform();
        }
        GUI.enabled = false;
        Rect rr = GUILayoutUtility.GetRect(4, 8);
        float endWidth = rr.width - 60;
        rr.y -= 4;
        rr.width = 4;
        int c = CPCPathScript.GetPoints().Count + ((CPCPathScript.GetLooped()) ? 1 : 0);
        for (int i = 0; i < c; ++i) {
            GUI.Box(rr, "");
            rr.x += endWidth / (c - 1);
        }
        GUILayout.EndVertical();
        GUI.enabled = true;
    }

    void DrawBasicSettings() {
        GUILayout.BeginHorizontal();
        selectedCameraProperty.objectReferenceValue = (Camera)EditorGUILayout.ObjectField(selectedCameraProperty.objectReferenceValue, typeof(Camera), true);
        GUI.enabled = true;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        lookAtTargetTransformProperty.objectReferenceValue = (Transform)EditorGUILayout.ObjectField(lookAtTargetTransformProperty.objectReferenceValue, typeof(Transform), true);
        GUI.enabled = true;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        loopedProperty.boolValue = GUILayout.Toggle(loopedProperty.boolValue, "Looped", GUILayout.Width(Screen.width / 3f));
        GUI.enabled = loopedProperty.boolValue;
        GUILayout.Label("After loop:", GUILayout.Width(Screen.width / 4f));
        afterLoopProperty.enumValueIndex = Convert.ToInt32(EditorGUILayout.EnumPopup((CPC_EAfterLoop)afterLoopProperty.intValue));
        GUI.enabled = true;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Camera Timer: ", GUILayout.Width(Screen.width / 4f));
        timer.floatValue = EditorGUILayout.FloatField(timer.floatValue);
        GUI.enabled = true;
        GUILayout.EndHorizontal();
    }

    void DrawVisualDropdown() {
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginHorizontal();
        visualFoldout = EditorGUILayout.Foldout(visualFoldout, "Visual");
        alwaysShowProperty.boolValue = GUILayout.Toggle(alwaysShowProperty.boolValue, alwaysShowContent);
        GUILayout.EndHorizontal();
        
        if (visualFoldout) {
            GUILayout.BeginVertical("Box");
            visualPathProperty.colorValue = EditorGUILayout.ColorField("Path color", visualPathProperty.colorValue);
            visualInactivePathProperty.colorValue = EditorGUILayout.ColorField("Inactive path color", visualInactivePathProperty.colorValue);
            visualFrustumProperty.colorValue = EditorGUILayout.ColorField("Frustum color", visualFrustumProperty.colorValue);
            visualHandleProperty.colorValue = EditorGUILayout.ColorField("Handle color", visualHandleProperty.colorValue);
            
            if (GUILayout.Button("Default colors")) {
                Undo.RecordObject(CPCPathScript, "Reset to default color values");
                CPCPathScript.visual = new CPC_Visual();
            }
            GUILayout.EndVertical();
        }
        if (EditorGUI.EndChangeCheck()) SceneView.RepaintAll();
    }

    void DrawManipulationDropdown() {
        manipulationFoldout = EditorGUILayout.Foldout(manipulationFoldout, "Transform manipulation modes");
        EditorGUI.BeginChangeCheck();
        
        if (manipulationFoldout) {
            GUILayout.BeginVertical("Box");
            cameraTranslateMode = (CPC_EManipulationModes)EditorGUILayout.EnumPopup("Waypoint Translation", cameraTranslateMode);
            cameraRotationMode = (CPC_EManipulationModes)EditorGUILayout.EnumPopup("Waypoint Rotation", cameraRotationMode);
            handlePositionMode = (CPC_EManipulationModes)EditorGUILayout.EnumPopup("Handle Translation", handlePositionMode);
            GUILayout.EndVertical();
        }
        
        if (EditorGUI.EndChangeCheck()) {
            PlayerPrefs.SetInt("CPC_cameraTranslateMode", (int)cameraTranslateMode);
            PlayerPrefs.SetInt("CPC_cameraRotationMode", (int)cameraRotationMode);
            PlayerPrefs.SetInt("CPC_handlePositionMode", (int)handlePositionMode);
            SceneView.RepaintAll();
        }
    }

    void DrawWaypointList() {
        GUILayout.Label("Replace all lerp types");
        GUILayout.BeginVertical("Box");
        GUILayout.BeginHorizontal();
        allCurveType = (CPC_ECurveType)EditorGUILayout.EnumPopup(allCurveType, GUILayout.Width(Screen.width / 3f));
        if (GUILayout.Button(replaceAllPositionContent)) {
            Undo.RecordObject(CPCPathScript, "Applied new position");
            foreach (var index in CPCPathScript.GetPoints()) {
                index.curveTypePosition = allCurveType;
                if (allCurveType == CPC_ECurveType.Custom) index.positionCurve.keys = allAnimationCurve.keys;
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUI.enabled = allCurveType == CPC_ECurveType.Custom;
        allAnimationCurve = EditorGUILayout.CurveField(allAnimationCurve, GUILayout.Width(Screen.width / 3f));
        GUI.enabled = true;
        if (GUILayout.Button(replaceAllRotationContent)) {
            Undo.RecordObject(CPCPathScript, "Applied new rotation");
            foreach (var index in CPCPathScript.GetPoints()) {
                index.curveTypeRotation = allCurveType;
                if (allCurveType == CPC_ECurveType.Custom) index.rotationCurve = allAnimationCurve;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width / 2f - 20);
        GUILayout.Label("↓");
        GUILayout.EndHorizontal();

        serializedObject.Update();
        pointReorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        Rect r = GUILayoutUtility.GetRect(Screen.width - 16, 18);
        r.height = 18;
        r.y -= 10;
        GUILayout.Space(-30);
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(addPointContent)) {
            Undo.RecordObject(CPCPathScript, "Added camera path point");
            switch (waypointMode) {
                case CPC_ENewWaypointMode.MainCamera: CPCPathScript.GetPoints().Add(new CPC_Point(Camera.main.transform.position, Camera.main.transform.rotation)); break;
                case CPC_ENewWaypointMode.SceneCamera: CPCPathScript.GetPoints().Add(new CPC_Point(SceneView.lastActiveSceneView.camera.transform.position, SceneView.lastActiveSceneView.camera.transform.rotation)); break;
                case CPC_ENewWaypointMode.LastWaypoint:
                    if (CPCPathScript.GetPoints().Count > 0) {
                        CPCPathScript.GetPoints().Add(new CPC_Point(CPCPathScript.GetPoints()[CPCPathScript.GetPoints().Count - 1].position, CPCPathScript.GetPoints()[CPCPathScript.GetPoints().Count - 1].rotation) {
                            handlenext = CPCPathScript.GetPoints()[CPCPathScript.GetPoints().Count - 1].handlenext,
                            handleprev = CPCPathScript.GetPoints()[CPCPathScript.GetPoints().Count - 1].handleprev
                        });
                    }
                    else { CPCPathScript.GetPoints().Add(new CPC_Point(Vector3.zero, Quaternion.identity)); }
                    break;
                case CPC_ENewWaypointMode.WaypointIndex:
                    if (CPCPathScript.GetPoints().Count > waypointIndex - 1 && waypointIndex > 0) {
                        CPCPathScript.GetPoints().Add(new CPC_Point(CPCPathScript.GetPoints()[waypointIndex - 1].position, CPCPathScript.GetPoints()[waypointIndex - 1].rotation) {
                            handlenext = CPCPathScript.GetPoints()[waypointIndex - 1].handlenext,
                            handleprev = CPCPathScript.GetPoints()[waypointIndex - 1].handleprev
                        });
                    }
                    else { CPCPathScript.GetPoints().Add(new CPC_Point(Vector3.zero, Quaternion.identity)); }
                    break;
                case CPC_ENewWaypointMode.WorldCenter: CPCPathScript.GetPoints().Add(new CPC_Point(Vector3.zero, Quaternion.identity)); break;
                default: throw new ArgumentOutOfRangeException();
            }
            selectedIndex = CPCPathScript.GetPoints().Count - 1;
            SceneView.RepaintAll();
        }

        GUILayout.Label("at", GUILayout.Width(20));
        EditorGUI.BeginChangeCheck();
        waypointMode = (CPC_ENewWaypointMode)EditorGUILayout.EnumPopup(waypointMode, waypointMode == CPC_ENewWaypointMode.WaypointIndex ? GUILayout.Width(Screen.width / 4) : GUILayout.Width(Screen.width / 2));
        
        if (waypointMode == CPC_ENewWaypointMode.WaypointIndex) waypointIndex = EditorGUILayout.IntField(waypointIndex, GUILayout.Width(Screen.width / 4));
        if (EditorGUI.EndChangeCheck()) PlayerPrefs.SetInt("CPC_waypointMode", (int)waypointMode); 
        GUILayout.EndHorizontal();
    }

    void DrawHandles(int i) {
        DrawHandleLines(i);
        Handles.color = CPCPathScript.visual.handleColor;
        DrawNextHandle(i);
        DrawPrevHandle(i);
        DrawWaypointHandles(i);
        DrawSelectionHandles(i);
    }

    void DrawHandleLines(int i) {
        Handles.color = CPCPathScript.visual.handleColor;
        if (i < CPCPathScript.GetPoints().Count - 1 || CPCPathScript.GetLooped() == true)  Handles.DrawLine(CPCPathScript.GetPoints()[i].position, CPCPathScript.GetPoints()[i].position + CPCPathScript.GetPoints()[i].handlenext);
        if (i > 0 || CPCPathScript.GetLooped() == true) Handles.DrawLine(CPCPathScript.GetPoints()[i].position, CPCPathScript.GetPoints()[i].position + CPCPathScript.GetPoints()[i].handleprev);
        Handles.color = Color.white;
    }

    void DrawNextHandle(int i) {
        if (i < CPCPathScript.GetPoints().Count - 1 || loopedProperty.boolValue) {
            EditorGUI.BeginChangeCheck();
            Vector3 posNext = Vector3.zero;
            float size = HandleUtility.GetHandleSize(CPCPathScript.GetPoints()[i].position + CPCPathScript.GetPoints()[i].handlenext) * 0.1f;
            
            if (handlePositionMode == CPC_EManipulationModes.Free) posNext = Handles.FreeMoveHandle(CPCPathScript.GetPoints()[i].position + CPCPathScript.GetPoints()[i].handlenext, Quaternion.identity, size, Vector3.zero, Handles.SphereHandleCap);
            else {
                if (selectedIndex == i) {
                    Handles.SphereHandleCap(0, CPCPathScript.GetPoints()[i].position + CPCPathScript.GetPoints()[i].handlenext, Quaternion.identity, size, EventType.Repaint);
                    posNext = Handles.PositionHandle(CPCPathScript.GetPoints()[i].position + CPCPathScript.GetPoints()[i].handlenext, Quaternion.identity);
                }
                else if (Event.current.button != 1 && Handles.Button(CPCPathScript.GetPoints()[i].position + CPCPathScript.GetPoints()[i].handlenext, Quaternion.identity, size, size, Handles.CubeHandleCap)) SelectIndex(i);
            }
            
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(target, "Changed Handle Position");
                CPCPathScript.GetPoints()[i].handlenext = posNext - CPCPathScript.GetPoints()[i].position;
                if (CPCPathScript.GetPoints()[i].chained) CPCPathScript.GetPoints()[i].handleprev = CPCPathScript.GetPoints()[i].handlenext * -1;
            }
        }

    }

    void DrawPrevHandle(int i) {
        if (i > 0 || loopedProperty.boolValue) {
            EditorGUI.BeginChangeCheck();
            Vector3 posPrev = Vector3.zero;
            float size = HandleUtility.GetHandleSize(CPCPathScript.GetPoints()[i].position + CPCPathScript.GetPoints()[i].handleprev) * 0.1f;
            if (handlePositionMode == CPC_EManipulationModes.Free) posPrev = Handles.FreeMoveHandle(CPCPathScript.GetPoints()[i].position + CPCPathScript.GetPoints()[i].handleprev, Quaternion.identity, 0.1f * HandleUtility.GetHandleSize(CPCPathScript.GetPoints()[i].position + CPCPathScript.GetPoints()[i].handleprev), Vector3.zero, Handles.SphereHandleCap);
            else {
                if (selectedIndex == i) {
                    Handles.SphereHandleCap(0, CPCPathScript.GetPoints()[i].position + CPCPathScript.GetPoints()[i].handleprev, Quaternion.identity, 0.1f * HandleUtility.GetHandleSize(CPCPathScript.GetPoints()[i].position + CPCPathScript.GetPoints()[i].handlenext), EventType.Repaint);
                    posPrev = Handles.PositionHandle(CPCPathScript.GetPoints()[i].position + CPCPathScript.GetPoints()[i].handleprev, Quaternion.identity);
                }
                else if (Event.current.button != 1 && Handles.Button(CPCPathScript.GetPoints()[i].position + CPCPathScript.GetPoints()[i].handleprev, Quaternion.identity, size, size, Handles.CubeHandleCap)) SelectIndex(i);
            }

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(target, "Changed Handle Position");
                CPCPathScript.GetPoints()[i].handleprev = posPrev - CPCPathScript.GetPoints()[i].position;

                if (CPCPathScript.GetPoints()[i].chained) CPCPathScript.GetPoints()[i].handlenext = CPCPathScript.GetPoints()[i].handleprev * -1;
            }
        }
    }

    void DrawWaypointHandles(int i) {
        if (Tools.current == Tool.Move) {
            EditorGUI.BeginChangeCheck();
            Vector3 pos = Vector3.zero;

            if (cameraTranslateMode == CPC_EManipulationModes.SelectAndTransform && i == selectedIndex) pos = Handles.PositionHandle(CPCPathScript.GetPoints()[i].position, (Tools.pivotRotation == PivotRotation.Local) ? CPCPathScript.GetPoints()[i].rotation : Quaternion.identity);
            else pos = Handles.FreeMoveHandle(CPCPathScript.GetPoints()[i].position, (Tools.pivotRotation == PivotRotation.Local) ? CPCPathScript.GetPoints()[i].rotation : Quaternion.identity, HandleUtility.GetHandleSize(CPCPathScript.GetPoints()[i].position) * 0.2f, Vector3.zero, Handles.RectangleHandleCap);

            if (EditorGUI.EndChangeCheck()){
                Undo.RecordObject(target, "Moved Waypoint");
                CPCPathScript.GetPoints()[i].position = pos;
            }
        }
        else if (Tools.current == Tool.Rotate) {
            EditorGUI.BeginChangeCheck();
            Quaternion rot = Quaternion.identity;

            if (cameraRotationMode == CPC_EManipulationModes.SelectAndTransform && i == selectedIndex) rot = Handles.RotationHandle(CPCPathScript.GetPoints()[i].rotation, CPCPathScript.GetPoints()[i].position);
            else rot = Handles.FreeRotateHandle(CPCPathScript.GetPoints()[i].rotation, CPCPathScript.GetPoints()[i].position, HandleUtility.GetHandleSize(CPCPathScript.GetPoints()[i].position) * 0.2f);

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(target, "Rotated Waypoint");
                CPCPathScript.GetPoints()[i].rotation = rot;
            }
        }
    }

    void DrawSelectionHandles(int i) {
        if (Event.current.button != 1 && selectedIndex != i) {
            if (cameraTranslateMode == CPC_EManipulationModes.SelectAndTransform && Tools.current == Tool.Move || cameraRotationMode == CPC_EManipulationModes.SelectAndTransform && Tools.current == Tool.Rotate) {
                float size = HandleUtility.GetHandleSize(CPCPathScript.GetPoints()[i].position) * 0.2f;
                if (Handles.Button(CPCPathScript.GetPoints()[i].position, Quaternion.identity, size, size, Handles.CubeHandleCap)) SelectIndex(i);
            }
        }
    }

    void DrawRawValues(){
        if (GUILayout.Button(showRawValues ? "Hide raw values" : "Show raw values")) showRawValues = !showRawValues;

        if (showRawValues) {
            foreach (var i in CPCPathScript.GetPoints()) {
                EditorGUI.BeginChangeCheck();
                GUILayout.BeginVertical("Box");

                Quaternion rot = Quaternion.Euler(EditorGUILayout.Vector3Field("Waypoint Rotation", i.rotation.eulerAngles));
                Vector3 pos = EditorGUILayout.Vector3Field("Waypoint Position", i.position);
                Vector3 posp = EditorGUILayout.Vector3Field("Previous Handle Offset", i.handleprev);
                Vector3 posn = EditorGUILayout.Vector3Field("Next Handle Offset", i.handlenext);
                GUILayout.EndVertical();

                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(CPCPathScript, "Changed waypoint transform");
                    i.position = pos;
                    i.rotation = rot;
                    i.handleprev = posp;
                    i.handlenext = posn;
                    SceneView.RepaintAll();
                }
            }
        }
    }
}