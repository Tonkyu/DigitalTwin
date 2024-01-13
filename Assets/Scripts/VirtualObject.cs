using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VirtualObject : MonoBehaviour
{

    [SerializeField] private string _name = "";
    [SerializeField] private TextMeshPro _textName = default;
    [SerializeField] private TextMeshPro _textWeight = default;
    [SerializeField] private TextMeshPro _textResistor = default;

    // Start is called before the first frame update
    void Start()
    {
        _textName.text = "Name: " + _name;
        _textWeight.text = "Weight: ?";
        _textResistor.text = "Resistor: ?";
    }

    public void LoadInfo(ObjectInfo info)
    {
        Debug.Log("name:\t" + info.name);
        Debug.Log("weight:\t" + info.weight);
        Debug.Log("resistor:\t" + info.resistor);
        SetWeight(info.weight);
    }

    private void SetWeight(string weight)
    {
        _textWeight.text = "Weight: " + weight;
    }

    private void SetResistor(string resistor)
    {
        _textResistor.text = "Resistor: " + resistor;
    }
}
