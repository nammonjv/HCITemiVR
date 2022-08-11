using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

public class ValueTest : M2MqttUnityClient
{
    public int moving;
    public int bodyyaw;


    [Header("MQTT topics")]
    [Tooltip("Set the topic to subscribe. !!!ATTENTION!!! multi-level wildcard # subscribes to all topics")]
    public string topicSubscribe = "#"; // topic to subscribe. !!! The multi-level wildcard # is used to subscribe to all the topics. Attention i if #, subscribe to all topics. Attention if MQTT is on data plan
    [Tooltip("Set the topic to publish (optional)")]
    public string topicPublish = "Moving"; // topic to publish
    public string topicPublish2 = "BodyYaw";
    private double timelimit = 0.1f;
    private double timenow = 0f;
    private double timer = 0f;
    private int divider;
    private int finalDegree;

    [Tooltip("Set this to true to perform a testing cycle automatically on startup")]
    public bool autoTest = false;


    //using C# Property GET/SET and event listener to reduce Update overhead in the controlled objects
    private string m_msg;

    public string msg
    {
        get
        {
            return m_msg;
        }
        set
        {
            if (m_msg == value) return;
            m_msg = value;
            if (OnMessageArrived != null)
            {
                OnMessageArrived(m_msg);
            }
        }
    }

    public event OnMessageArrivedDelegate OnMessageArrived;
    public delegate void OnMessageArrivedDelegate(string newMsg);

    //using C# Property GET/SET and event listener to expose the connection status
    private bool m_isConnected;

    public bool isConnected
    {
        get
        {
            return m_isConnected;
        }
        set
        {
            if (m_isConnected == value) return;
            m_isConnected = value;
            if (OnConnectionSucceeded != null)
            {
                OnConnectionSucceeded(isConnected);
            }
        }
    }
    public event OnConnectionSucceededDelegate OnConnectionSucceeded;
    public delegate void OnConnectionSucceededDelegate(bool isConnected);

    // a list to store the messages
    private List<string> eventMessages = new List<string>();

    /// <summary>
    /// //////////////////////////
    /// </summary>

    
    public void Publish()
    {
        moving = KATVR.KATDevice_Walk.isMoving;
        bodyyaw = KATVR.KATDevice_Walk.bodyYaw;
        DegreeCalculator();
        timer += Time.deltaTime;
        if (timer - timenow > timelimit)
        {
            Debug.Log("moving get");
            Debug.Log("bodyYaw get");
            client.Publish(topicPublish, System.Text.Encoding.UTF8.GetBytes(moving.ToString()), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            client.Publish(topicPublish2, System.Text.Encoding.UTF8.GetBytes(bodyyaw.ToString()), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log("Test message published");
            timenow = timer;
        }


    }
    public void SetEncrypted(bool isEncrypted)
    {
        this.isEncrypted = isEncrypted;
    }

    protected override void OnConnecting()
    {
        base.OnConnecting();
       
    }

    protected override void OnConnected()
    {
        base.OnConnected();
        isConnected = true;
        
        if (autoTest)
        {
            Debug.Log("Publishing");
            Publish();
            
        }
    }

    protected override void OnConnectionFailed(string errorMessage)
    {
        Debug.Log("CONNECTION FAILED! " + errorMessage);
    }

    protected override void OnDisconnected()
    {
        Debug.Log("Disconnected.");
        isConnected = false;
    }

    protected override void OnConnectionLost()
    {
        Debug.Log("CONNECTION LOST!");
    }

    protected override void SubscribeTopics()
    {
        client.Subscribe(new string[] { topicSubscribe }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    protected override void UnsubscribeTopics()
    {
        client.Unsubscribe(new string[] { topicSubscribe });
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void DecodeMessage(string topic, byte[] message)
    {
        //The message is decoded
        msg = System.Text.Encoding.UTF8.GetString(message);

        Debug.Log("Received: " + msg);
        Debug.Log("from topic: " + m_msg);

        StoreMessage(msg);
        if (topic == topicSubscribe)
        {
            if (autoTest)
            {
                autoTest = false;
                Disconnect();
            }
        }
    }

    private void StoreMessage(string eventMsg)
    {
        if (eventMessages.Count > 50)
        {
            eventMessages.Clear();
        }
        eventMessages.Add(eventMsg);
    }

    protected override void Update()
    {
        base.Update(); // call ProcessMqttEvents()
        Publish();

    }

    private void OnDestroy()
    {
        Disconnect();
    }

    private void OnValidate()
    {
        if (autoTest)
        {
            autoConnect = true;
        }
    }

    private void DegreeCalculator()
    {
        divider = bodyyaw / 12;
        finalDegree = (divider - 1) * 30;
    }
    
    
}
