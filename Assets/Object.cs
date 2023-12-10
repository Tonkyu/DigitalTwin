using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Object : MonoBehaviour
{

    [SerializeField] private string name;
    [SerializeField] private TextMeshPro textName;
    [SerializeField] private TextMeshPro textWeight;
    
    // Start is called before the first frame update
    void Start()
    {
        textName.text = "Name: " + name;
        textWeight.text = "Weight: ?";
        Debug.Log(textWeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void LoadInfo(ObjectInfo info)
    {
        Debug.Log("name:\t" + info.name);
        Debug.Log("weight:\t" + info.weight);
        SetWeight(info.weight);
    }

    private void SetWeight(string weight)
    {
    Debug.Log(textWeight);
    	textWeight.text = "Weight: " + weight;
    }
}
