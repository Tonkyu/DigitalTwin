using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DigitalTwin
{
    public class ObjectInfo // match with object_info.py
    {
        public string name;
        public string weight;
        public string resistor;
    }




    public class DiditalTwinManager : MonoBehaviour
    {
        static SynchronizationContext _mainContext;
        static UdpClient udp;
        IPEndPoint remoteEP = null;
        static DiditalTwinObject[] objects = null;
        static bool isReceiving;

        Thread thread;

        // Start is called before the first frame update
        void Start()
        {
            isReceiving = true;
            Debug.Log($"Main thread ID : " + Thread.CurrentThread.ManagedThreadId);
            _mainContext = SynchronizationContext.Current;


            int LOCAL_PORT = 50007;
            udp = new UdpClient(LOCAL_PORT);
            udp.Client.ReceiveTimeout = 2000;

            //objectsを全て取得
            objects = FindObjectsOfType<DiditalTwinObject>();
            int ID = 0;
            foreach (DiditalTwinObject obj in objects)
            {
                obj.SetID(ID);
                ID++;
                Debug.Log(obj.GetName());
            }

            thread = new Thread(new ThreadStart(ThreadMethod));
            thread.Start();
        }

        private static void ThreadMethod()//Task.Run()と同じ
        {
            while (isReceiving)
            {
                try
                {
                    IPEndPoint remoteEP = null;
                    byte[] data = udp.Receive(ref remoteEP);
                    string jsonText = Encoding.UTF8.GetString(data);
                    Debug.Log(jsonText);
                    Debug.Log($"json thread ID : " + Thread.CurrentThread.ManagedThreadId);
                    // string jsonText = "{\"name\": \"apple\", \"weight\": 500.0}";
                    ObjectInfo info = JsonUtility.FromJson<DigitalTwin.ObjectInfo>(jsonText);
                    string objName = info.name;
                    foreach (DiditalTwinObject obj in objects)
                    {
                        if (obj.GetName() == objName)
                        {
                            _mainContext.Post(_ => obj.LoadInfo(info), null);//mainthreadで実行,Unityではメインスレッド以外で実行できないメソッドがある
                        }
                    }

                }
                catch (System.Exception e)
                {
                    // Debug.Log(e.ToString());
                }
            }
        }

        void OnApplicationQuit()
        {
            isReceiving = false;
            if (thread != null) thread.Abort();
            if (udp != null) udp.Close();
        }
    }
}


