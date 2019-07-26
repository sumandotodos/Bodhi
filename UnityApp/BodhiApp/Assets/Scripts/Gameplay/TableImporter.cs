using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TableImporter : MonoBehaviour
{
    public string BaseDirectory;
    public ContentsManager contentsManager;
    public Transform TablesParent;

    public void ClearTables()
    {
        contentsManager.clear();
        DestroyImmediate(TablesParent.gameObject);
        TablesParent = new GameObject().transform;
        TablesParent.gameObject.name = "Tables";
    }

    public Transform AddEmptyObject(Transform parent, string name)
    {
        GameObject child = new GameObject();
        child.name = name;
        child.transform.parent = parent;
        return child.transform;
    }

    public FGTable AddTable(Transform parent, string name, string contents)
    {
        GameObject newGO = new GameObject();
        newGO.name = name;
        newGO.transform.parent = parent;
        FGTable newTable = newGO.AddComponent<FGTable>();
        newTable.Initialize();
        newTable.importCRSV(contents);
        return newTable;
    }

    public void Import()
    {

        ClearTables();

        var info = new DirectoryInfo(BaseDirectory);
        var fileInfo = info.GetFiles();
        var dirInfo = info.GetDirectories();

        foreach(DirectoryInfo d in dirInfo)
        {
            Category newCat = new Category(d.Name);
            contentsManager.category.Add(newCat);
            Transform currentParent = AddEmptyObject(TablesParent, d.Name);
            var subDirInfo = d.GetDirectories();
            foreach(DirectoryInfo subd in subDirInfo)
            {
                Topic newTopic = new Topic(subd.Name);
                newCat.topics.Add(newTopic);
                Transform currentSubParent = AddEmptyObject(currentParent, subd.Name);
                var files = subd.GetFiles();
                foreach(FileInfo file in files)
                {
                    if (file.FullName.EndsWith(".crsv"))
                    {
                        string contents = System.IO.File.ReadAllText(file.FullName, System.Text.Encoding.UTF8);
                        newTopic.tables.Add(AddTable(currentSubParent, subd.Name+file.Name, contents));
                    }
                }
            }
        }

    }

}
