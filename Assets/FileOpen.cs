using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileOpen : MonoBehaviour {

    public CreateFlightRoot _createFlightRoot;

    //this is called as an action for on click button handler
	public void AddFiles()
    {
        var extensions = new[]
        {
            new ExtensionFilter("Flight Plan & Data Files", "txt", "csv"),
            new ExtensionFilter("All Files", "*")
        };
        
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, true);

        List<string> txts = new List<string>();
        List<string> csvs = new List<string>();

        foreach (var path in paths)
        {
            var ext = path.Substring(path.Length - 3);
            if (ext == "txt")
            {
                txts.Add(path);
            }
            else if (ext == "csv")
            {
                csvs.Add(path);
            }
        }

        if (txts.Count != 0)
        {
            _createFlightRoot.UpdatePlan(txts.ToArray());
            Debug.Log("Loaded " + txts.Count + " txt files");
        }
        if (csvs.Count != 0)
        {
            _createFlightRoot.UpdateData(csvs.ToArray());
            Debug.Log("Loaded " + csvs.Count + " csv files");
        }
    }
}
