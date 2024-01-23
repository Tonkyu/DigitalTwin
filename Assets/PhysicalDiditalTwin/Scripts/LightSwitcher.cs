using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _light;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("DiditalTwinObject"))
        {
            var obj = col.gameObject.GetComponent<DiditalTwinObject>();
            if (obj.GetIsConduct())
            {
                _light.SetActive(true);
            }
            else
            {
                _light.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("DiditalTwinObject"))
        {
            _light.SetActive(false);
        }
    }    
}
