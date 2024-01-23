using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DigitalTwin;
using System.Threading;


public class DiditalTwinObject : MonoBehaviour
{

    [SerializeField] private string _name = "";

    public string GetName()
    {
        return _name;
    }
    [SerializeField] private int _ID = 0;

    public int GetID()
    {
        return _ID;
    }
    public void SetID(int id)
    {
        _ID = id;
    }

    [SerializeField] private float _resistor = 0.0f;

    public float GetResistor()
    {
        return _resistor;
    }

    public void SetResistor(float res)
    {
        _resistor = res;
    }
    public bool GetIsConduct()
    {
        bool _isConduct = false;
        if (_resistor < 2000.0f)
        {
            _isConduct = true;
        }
        else
        {
            _isConduct = false;
        }
        return _isConduct;
    }

    [SerializeField] private float _weight = 0.0f;

    public float GetWeight()
    {
        return _weight;
    }

    [SerializeField] TextMeshPro _textName = default;
    [SerializeField] TextMeshPro _textWeight = default;
    [SerializeField] TextMeshPro _textResistor = default;

    private ParticleSystem effect = default;

    private Rigidbody _rigidbody = default;

    // Start is called before the first frame update
    void Start()
    {
        _textName.text = "Name: " + _name;
        _textWeight.text = "Weight: ?";
        _textResistor.text = "Resistor: ?";

        //シーン中からeffect/particle systemを取得
        effect = GameObject.Find("Effect").gameObject.GetComponentInChildren<ParticleSystem>();

        //rigidbodyを取得
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdatedEffect();
        }
    }

    public void LoadInfo(ObjectInfo info)
    {
        Debug.Log($"LoadINfo thread ID : " + Thread.CurrentThread.ManagedThreadId);

        Debug.Log("name:\t" + info.name);
        Debug.Log("weight:\t" + info.weight);
        Debug.Log("resistor:\t" + info.resistor);
        SetWeight(info.weight);
        SetResistor(info.resistor);
        UpdatedEffect();
    }

    private void SetWeight(string weight)
    {
        //weightを数値にする
        float.TryParse(weight, out _weight);
        _textWeight.text = "Weight: " + weight;
        //rigidbodyのmassを変更
        _rigidbody.mass = _weight/1000.0f;
    }

    private void SetResistor(string resistor)
    {
        //resistorを数値にする
        float.TryParse(resistor, out _resistor);
        _textResistor.text = "Resistor: " + resistor;
    }

    private void UpdatedEffect()
    {
        Debug.Log("UpdatedEffect");
        // effect.transform.position = transform.position + new Vector3(0, 0.03f, 0);
        //ローカル座標系で3cm上
        effect.transform.position = transform.position + transform.rotation * new Vector3(0, 0.0f, 0.06f);
        effect.Play();
    }
}
