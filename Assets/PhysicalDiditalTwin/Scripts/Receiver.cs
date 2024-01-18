// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Net;
// using System.Net.Sockets;
// using System.Text;
// using System.Threading;

// public class ObjectInfo // match with object_info.py
// {
// 	public string name;
// 	public string weight;
//     public string resistor;
// }


// public class Receiver : MonoBehaviour
// {

//     static UdpClient udp;
//     IPEndPoint remoteEP = null;
//     [SerializeField] VirtualObject[] objects;

//     // Start is called before the first frame update
//     void Start()
//     {
//         int LOCAL_PORT = 50007;
//         udp = new UdpClient(LOCAL_PORT);
//         udp.Client.ReceiveTimeout = 2000;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         IPEndPoint remoteEP = null;
//         byte[] data = udp.Receive(ref remoteEP);
//         string jsonText = Encoding.UTF8.GetString(data);
//         // string jsonText = "{\"name\": \"apple\", \"weight\": 500.0}";
//         ObjectInfo info = JsonUtility.FromJson<ObjectInfo>(jsonText);
//         objects[0].LoadInfo(info);
//     }
// }
