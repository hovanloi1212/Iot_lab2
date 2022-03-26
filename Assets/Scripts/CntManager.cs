using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;
namespace ChuongGa
{
    public class Status_Data
    {
        public string project_id { get; set; }
        public string project_name { get; set; }
        public string station_id { get; set; }
        public string station_name { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string volt_battery { get; set; }
        public string volt_solar { get; set; }
        public List<data_ss> data_ss { get; set; }
        public string device_status { get; set; }
    }

    public class data_ss
    {
        public string ss_name { get; set; }
        public string ss_unit { get; set; }
        public string ss_value { get; set; }
    }

    public class Config_Data
    {
        public float temperature_max { get; set; }
        public float temperature_min { get; set; }
        public int mode_fan_auto { get; set; }
    }

    public class Control_status
    {
        public string status { get; set; }
        public string device { get; set; }

    }

    public class CntManager : M2MqttUnityClient
    {
        public InputField url_f, topic_f, usr_f, pwd_f, port_f;
        public Button btn;
        public Text btntext;
        // Start is called before the first frame update
        public string topic = "/bkiot/1811068/status";
        public string topic1 = "/bkiot/1811068/led";
        public string topic2 = "/bkiot/1811068/pump";
        public string url = "mqttserver.tk";
        public string usr = "bkiot";
        public string pwd = "12345678";
        public int port = 1883;
        public string msg_received = "";
        private bool isSubscribing = false;
        //
        private List<string> eventMessages = new List<string>();
        public Status_Data _status_data;
        public Status_Data _publish_status_data;
        public Control_status LED;
        public Control_status PUMP;
        public Config_Data _config_data;
        public Control_status _control_LED;
        public Control_status _control_PUMP;
        public void init_data()
        { 
            _publish_status_data = new Status_Data();
            _publish_status_data.data_ss = new List<data_ss>();
            _publish_status_data.project_id = "1811068";
            _publish_status_data.project_name = "hehe";
            _publish_status_data.station_id = "Station 1";
            _publish_status_data.station_name = "meomeo";
            _publish_status_data.longitude = "106.66061516906501"; //10.773364009098115, 106.66061516906501
            _publish_status_data.latitude = "10.773364009098115";
            _publish_status_data.volt_battery = "100%";
            _publish_status_data.volt_solar = "100%";
            _publish_status_data.data_ss.Add(new data_ss()
            {
                ss_name = "temperature",
                ss_unit = "",
                ss_value = "20"
            });
            _publish_status_data.data_ss.Add( new data_ss()
            {
                ss_name = "humidity",
                ss_unit = "",
                ss_value = "70"
            });
            _publish_status_data.device_status = "1";

            LED = new Control_status();
            LED.device = "LED";
            LED.status = "ON";
            PUMP = new Control_status();
            PUMP.device = "PUMP";
            PUMP.status = "OFF";
        }
        protected override void Start()
        {
            init_data();
            GetComponent<ChuongGaManager>().LostConnection();
            GetComponent<ChuongGaManager>().Starting();
        }

        // Update is called once per frame
        //protected override void Update()
        //{
        //    base.Update();
        //}
        public void UpdateBeforeConnect()
        {
            if (url_f.text != "") this.brokerAddress = url_f.text; else this.brokerAddress = url;
            if (usr_f.text != "") this.mqttUserName = usr_f.text; else this.mqttUserName = usr;
            if (pwd_f.text != "") this.mqttPassword = pwd_f.text; else this.mqttPassword = pwd;
            if (port_f.text != "") this.brokerPort = (int)int.Parse(port_f.text); else this.brokerPort = port;
            if (topic_f.text != "") 
            {
                topic = topic_f.text + "/status";
                topic1 = topic_f.text + "/led";
                topic2 = topic_f.text + "/pump";
            }
            Connect();
        }     
        public void PublishTest()
        {
            string msg_config1 = JsonConvert.SerializeObject(LED);
            string msg_config2 = JsonConvert.SerializeObject(PUMP);
            string msg_config = JsonConvert.SerializeObject(_publish_status_data);
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg_config), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            client.Publish(topic1, System.Text.Encoding.UTF8.GetBytes(msg_config1), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            client.Publish(topic2, System.Text.Encoding.UTF8.GetBytes(msg_config2), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("Test Publised");
        }
        public void Publish_LED()
        {
            LED = new Control_status() {device = "LED" };
            LED = GetComponent<ChuongGaManager>().Update_LED_Value(LED);
            string msg_config1 = JsonConvert.SerializeObject(LED);
            client.Publish(topic1, System.Text.Encoding.UTF8.GetBytes(msg_config1), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("Publish:" + msg_config1);
        }
        public void Publish_PUMP()
        {
            PUMP = new Control_status() { device = "PUMP"};
            PUMP = GetComponent<ChuongGaManager>().Update_Pump_Value(PUMP);
            string msg_config2 = JsonConvert.SerializeObject(PUMP);
            client.Publish(topic2, System.Text.Encoding.UTF8.GetBytes(msg_config2), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("Publish:" + msg_config2);
        }
        protected override void SubscribeTopics()
        {
            //PublishTest();
            if (topic != "")
            {
                client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                client.Subscribe(new string[] { topic1 }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                client.Subscribe(new string[] { topic2 }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                isSubscribing = true;
            }
            //Debug.Log("Subscribed");
        }
        protected override void UnsubscribeTopics()
        {
            if (topic != "")
            {
                client.Unsubscribe(new string[] { topic });
                client.Unsubscribe(new string[] { topic1 });
                client.Unsubscribe(new string[] { topic2 });
                isSubscribing = false;
            }
        }
        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("Received: " + msg);
            //StoreMessage(msg);
            if (topic == this.topic)
                ProcessMessageStatus(msg);
            else ProcessMessageControl(msg);
        }
        private void ProcessMessageStatus(string msg)
        {
            _status_data = JsonConvert.DeserializeObject<Status_Data>(msg);
            msg_received = msg;
            GetComponent<ChuongGaManager>().Update_Status(_status_data);
        }
        private void ProcessMessageControl(string msg)
        {
            Control_status _control_data = JsonConvert.DeserializeObject<Control_status>(msg);
            msg_received = msg;
            GetComponent<ChuongGaManager>().Update_control(_control_data);
        }
        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }


        protected override void OnConnecting()
        {
            base.OnConnecting();
            //SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            base.OnConnected();

            //SubscribeTopics();
        }
        protected override void OnConnectionFailed(string errorMessage)
        {
            Debug.Log("CONNECTION FAILED! " + errorMessage);
        }

        protected override void OnDisconnected()
        {
            Debug.Log("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            Debug.Log("CONNECTION LOST!");
        }
        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
            //if (autoTest)
            //{
            //    autoConnect = true;
            //}
        }

        public void UpdateConfig()
        {

        }

        public void UpdateControl()
        {

        }
        public void SubBtn()
        {
            if (isSubscribing == true)
            {
                GetComponent<ChuongGaManager>().LostConnection();
                btntext.text = "Subscribe";
                UnsubscribeTopics();
            }
            else
            {
                SubscribeTopics();
                btntext.text = "Unsubscribe";
            }
        }
    }
}
