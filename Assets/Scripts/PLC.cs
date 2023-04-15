using UnityEngine;
using libplctag;
using libplctag.DataTypes;
using libplctag.NativeImport;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

public class PLC : MonoBehaviour
{
    [SerializeField] private string Gateway = string.Empty;

    [SerializeField] private string Path = string.Empty;

    [SerializeField] private PlcType PlcType = new();

    [SerializeField] private Protocol Protocol = new();

    public int ScanTime = 0;

    Dictionary<GameObject, Tag<SintPlcMapper, sbyte>> bool_tags = new();

    Dictionary<GameObject, Tag<DintPlcMapper, int>> int_tags = new();

    // 0 = bool
    // 1 = int
    public void Connect(string tagName, int dataType, GameObject gameObject)
    {
        try { plctag.ForceExtractLibrary = false; } catch { };

        if (dataType == 0) 
        {
            Tag<SintPlcMapper, sbyte> tag = new()
            {
                Name = tagName,
                Gateway = Gateway,
                Path = Path,
                PlcType = PlcType,
                Protocol = Protocol,
                Timeout = TimeSpan.FromSeconds(5)
            };

            bool_tags.Add(gameObject,tag);
        }
        else if(dataType == 1)
        {
            Tag<DintPlcMapper, int> tag = new()
            {
                Name = tagName,
                Gateway = Gateway,
                Path = Path,
                PlcType = PlcType,
                Protocol = Protocol,
                Timeout = TimeSpan.FromSeconds(5)
            };

            int_tags.Add(gameObject, tag);
        }
    }

    public async Task<int> Read(GameObject gameObject)
    {
        if (int_tags.ContainsKey(gameObject))
        {

            return await int_tags[gameObject].ReadAsync();
        }
        else
        {
            return Convert.ToInt32(await bool_tags[gameObject].ReadAsync());
        }
    }

    public async Task Write(GameObject gameObject, sbyte value)
    {
        bool_tags[gameObject].Value = value;
        await bool_tags[gameObject].WriteAsync();
    }

    public async Task Write(GameObject gameObject, int value)
    {
        int_tags[gameObject].Value = value;
        await int_tags[gameObject].WriteAsync();
    }


    private void Awake()
    {
        UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
    }
}


