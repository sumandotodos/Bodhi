using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TableImporter))]
public class TableImporterEditor : Editor
{

    public override void OnInspectorGUI()
    {

        TableImporter importer = (TableImporter)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Import all", GUILayout.Width(200)))
        {
            importer.Import();
        }
    }
}
