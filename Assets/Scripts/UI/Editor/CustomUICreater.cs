using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomUICreater
{
    /// <summary>
    /// Create Model Renderer by right click in Hierarchy
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/UI/Model Renderer", false, 501)]
    private static void CreateUIModelRenderer(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Model Renderer");
        go.AddComponent<UIModelRenderer>();

        GameObjectUtility.SetParentAndAlign(go, (GameObject)menuCommand.context);

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);

        Selection.activeObject = go;
    }

    [MenuItem("GameObject/UI/Selectable Graphic", false, 500)]
    private static void CreateUISelectableGraphic(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Selectable Graphic");
        go.AddComponent<CustomSelectableGraphic>();

        GameObjectUtility.SetParentAndAlign(go, (GameObject)menuCommand.context);

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);

        Selection.activeObject = go;
    }
}
