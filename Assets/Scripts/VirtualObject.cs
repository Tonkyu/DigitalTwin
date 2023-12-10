using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VirtualObject : MonoBehaviour
{

    [SerializeField] private string _name = "";
    [SerializeField] private TextMeshPro _textName = default;
    [SerializeField] private TextMeshPro _textWeight = default;

    // Start is called before the first frame update
    void Start()
    {
        _textName.text = "Name: " + _name;
        _textWeight.text = "Weight: ?";
    }

    public void LoadInfo(ObjectInfo info)
    {
        Debug.Log("name:\t" + info.name);
        Debug.Log("weight:\t" + info.weight);
        SetWeight(info.weight);
    }

    public void SetWeight(string weight)
    {
        _textWeight.text = "Weight: " + weight;
    }
}
