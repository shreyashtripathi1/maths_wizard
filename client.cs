using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

public class SimpleUDPClient : MonoBehaviour
{
    // Server configuration
    [SerializeField] private string serverIP = "192.168.128.227";
    [SerializeField] private int serverPort = 700;

    // Control flags
    [SerializeField] private bool sendOnStart = true;
    [SerializeField] private bool sendPeriodically = false;
    [SerializeField] private float sendInterval = 5.0f;

    private UdpClient client;
    private IPEndPoint serverEndPoint;
    private Thread receiveThread;
    private bool threadRunning = false;
    private float timeCounter = 0;

    void Start()
    {
        InitializeClient();

        // Optionally send a message right at start
        if (sendOnStart)
        {
            SendHelloWorld();
        }
    }

    void Update()
    {
        // Optionally send messages periodically
        if (sendPeriodically)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= sendInterval)
            {
                SendHelloWorld();
                timeCounter = 0;
            }
        }
    }

    void InitializeClient()
    {
        try
        {
            // Create UDP client
            client = new UdpClient();
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);

            // Start listening thread
            threadRunning = true;
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();

            Debug.Log($"UDP client initialized, ready to send to {serverIP}:{serverPort}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error initializing UDP client: {e.Message}");
        }
    }

    public void SendHelloWorld()
    {
        try
        {
            string message = "Hello World";
            byte[] data = Encoding.ASCII.GetBytes(message);

            client.Send(data, data.Length, serverEndPoint);
            Debug.Log($"Sent 'Hello World' to {serverIP}:{serverPort}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error sending message: {e.Message}");
        }
    }

    private void ReceiveData()
    {
        IPEndPoint receiveEndPoint = new IPEndPoint(IPAddress.Any, 0);

        Debug.Log("Receiver thread started");

        while (threadRunning)
        {
            try
            {
                byte[] data = client.Receive(ref receiveEndPoint);
                string responseMessage = Encoding.ASCII.GetString(data);

                // Log the response (main thread safe)
                MainThreadLogger($"Received response from {receiveEndPoint}: {responseMessage}");
            }
            catch (Exception e)
            {
                if (threadRunning)
                {
                    MainThreadLogger($"Error in receiver thread: {e.Message}");
                }
                break;
            }
        }

        Debug.Log("Receiver thread stopped");
    }

    // Helper method for logging from thread
    private void MainThreadLogger(string message)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => {
            Debug.Log(message);
        });
    }

    void OnDestroy()
    {
        // Clean up
        threadRunning = false;

        if (client != null)
        {
            client.Close();
            client = null;
        }

        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Join(500);
        }
    }
}

// Simplified version of the dispatcher for thread safety
public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher instance;
    private System.Collections.Generic.Queue<Action> actionQueue = new System.Collections.Generic.Queue<Action>();

    public static UnityMainThreadDispatcher Instance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("UnityMainThreadDispatcher");
            instance = go.AddComponent<UnityMainThreadDispatcher>();
            DontDestroyOnLoad(go);
        }
        return instance;
    }

    public void Enqueue(Action action)
    {
        lock (actionQueue)
        {
            actionQueue.Enqueue(action);
        }
    }

    private void Update()
    {
        lock (actionQueue)
        {
            while (actionQueue.Count > 0)
            {
                actionQueue.Dequeue().Invoke();
            }
        }
    }

}