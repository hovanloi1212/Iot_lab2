using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ChuongGa
{
    public class ChuongGaManager : MonoBehaviour
    {
        [SerializeField]
        private Text station;
        [SerializeField]
        private Text project;
        [SerializeField]
        private Text loc;
        [SerializeField]
        private CanvasGroup _canvasLayer1;
        [SerializeField]
        private GameObject LED;
        [SerializeField]
        private GameObject PUMP;
        [SerializeField]
        private Text LEDStatus;
        [SerializeField]
        private Text PumpStatus;
        [SerializeField]
        private Text temperature;
        [SerializeField]
        private Text humidity;
        [SerializeField]
        private Image LedLight;
        [SerializeField]
        private Slider Pump_Silder;
        [SerializeField]
        private GameObject Graph_Temperature;
        [SerializeField]
        private GameObject Graph_Humidity;
        //[SerializeField]
        //private Text min_temperature;
        //[SerializeField]
        //private Text max_temperature;
        //[SerializeField]
        //private Button _btn_config;
        /// <summary>
        /// Layer 2 elements
        /// </summary>
        [SerializeField]
        private CanvasGroup _canvasLayer2;
        //[SerializeField]
        //private InputField _input_min_tempe;
        //[SerializeField]
        //private InputField _input_max_tempe;
        //[SerializeField]
        //private Toggle ModeAuto;

        /// <summary>
        /// General elements
        /// </summary>
        //[SerializeField]
        //private GameObject Btn_Quit;

        private Tween twenFade;

        //private bool device_status = false;

        public void Update_Status(Status_Data _status_data)
        {
            project.text = _status_data.project_id + " - " + _status_data.project_name ;
            station.text = _status_data.station_id + ": " + _status_data.station_name ;
            loc.text     = "lat: " + _status_data.latitude + ", " + "long: "  + _status_data.longitude;
            foreach(data_ss _data in _status_data.data_ss)
            {
                switch (_data.ss_name)
                {
                    case "temperature":
                        temperature.GetComponentInParent<CircleSlider>().b = true;
                        temperature.text = _data.ss_value + "°C";
                        break;
                    case "humidity":
                        humidity.GetComponentInParent<CircleSlider>().b = true;
                        humidity.text = _data.ss_value + "%";
                        break;
                }
            }
            Graph_Temperature.GetComponent<Graph>().b = true;
            Graph_Humidity.GetComponent<Graph>().b = true;
        }
        //public void Update_Status(Status_Data _status_data)
        //{
        //    station_name.text = _status_data.station_name;
        //    foreach(data_ss _data in _status_data.data_ss)
        //    {
        //        switch (_data.ss_name)
        //        {

        //            case "temperature_min":
        //                min_temperature.text = _data.ss_value + "°C";
        //                _input_min_tempe.text = _data.ss_value;
        //                break;

        //            case "temperature_max":
        //                max_temperature.text = _data.ss_value + "°C";
        //                _input_max_tempe.text = _data.ss_value;

        //                break;

        //            case "fan_temperature":
        //                temperature.text = _data.ss_value + "°C";
        //                break;

        //            case "fan_humidity":
        //                humidity.text = _data.ss_value + "%";
        //                break;

        //            case "mode_fan_auto":
        //                if (_data.ss_value == "1") { 
        //                    ModeAuto.isOn = true;
        //                    LampControl.interactable = false;
        //                }
        //                else { 
        //                    ModeAuto.isOn = false;
        //                    LampControl.interactable = true;
        //                }
        //                break;
        //            //case "device_status":
        //            //    Debug.Log("_data.ss_value " + _data.ss_value);
        //            //    if (_data.ss_value == "1")
        //            //        _btn_config.interactable = true;
                       
        //            //    break;
        //        }
                
        //    }
        //    if(_status_data.device_status=="1")
        //        _btn_config.interactable = true;

        //}

        public void Update_control(Control_status _control_data)
        {
            switch(_control_data.device) 
            {
                case "LED":
                    LED.GetComponent<Tgle>().switchBtn.GetComponent<Button>().interactable = true;
                    LedLight.GetComponent<Blink>().blink = true;    
                    if (_control_data.status == "ON")
                    {
                        LED.GetComponent<Tgle>().On_Switch();
                        LedLight.color = LED.GetComponent<Tgle>().on_color;
                        LEDStatus.color = LED.GetComponent<Tgle>().on_color;
                    }
                    else
                    {
                        LED.GetComponent<Tgle>().Off_Switch();
                        LedLight.color = LED.GetComponent<Tgle>().off_color;
                        LEDStatus.color = LED.GetComponent<Tgle>().off_color;
                    }
                    break;
                case "PUMP":
                    PUMP.GetComponent<Tgle>().switchBtn.GetComponent<Button>().interactable = true;
                    if (_control_data.status == "ON")
                    {
                        PUMP.GetComponent<Tgle>().On_Switch();
                        PumpStatus.color = PUMP.GetComponent<Tgle>().on_color;
                        Pump_Silder.GetComponent<SliderScripts>().b = true;
                    }
                    else
                    {
                        PUMP.GetComponent<Tgle>().Off_Switch();
                        PumpStatus.color = PUMP.GetComponent<Tgle>().off_color;
                        Pump_Silder.GetComponent<SliderScripts>().b = false;
                    }
                    break;
            }
        }

        public Control_status Update_LED_Value(Control_status _control_data)
        {
            LED.GetComponent<Tgle>().OnSwitchButtonClicked();
            LED.GetComponent<Tgle>().switchBtn.GetComponent<Button>().interactable = false;
            if (LED.GetComponent<Tgle>().isOn) _control_data.status = "ON";
            else _control_data.status = "OFF";

            return _control_data;
        }
        public Control_status Update_Pump_Value(Control_status _control_data)
        {
            PUMP.GetComponent<Tgle>().OnSwitchButtonClicked();
            PUMP.GetComponent<Tgle>().switchBtn.GetComponent<Button>().interactable = false;
            if (PUMP.GetComponent<Tgle>().isOn) _control_data.status = "ON";
            else _control_data.status = "OFF";
            return _control_data;
        }

        //public Config_Data Update_Config_Value(Config_Data _configdata)
        //{
        //    _configdata.temperature_max = float.Parse(_input_max_tempe.text);
        //    _configdata.temperature_min = float.Parse(_input_min_tempe.text);
        //    _configdata.mode_fan_auto = ModeAuto.isOn ? 1 : 0;
           
        //    return _configdata;
        //}
        //public void Disable_Config_Btn()
        //{
        //    _btn_config.interactable = false;
        //}

        public void Fade(CanvasGroup _canvas, float endValue, float duration, TweenCallback onFinish)
        {
            if (twenFade != null)
            {
                twenFade.Kill(false);
            }
            twenFade = _canvas.DOFade(endValue, duration);
            twenFade.onComplete += onFinish;
        }

        public void FadeIn(CanvasGroup _canvas, float duration)
        {
            Fade(_canvas, 1f, duration, () =>
            {
                _canvas.interactable = true;
                _canvas.blocksRaycasts = true;
            });
        }

        public void FadeOut(CanvasGroup _canvas, float duration)
        {
            Fade(_canvas, 0f, duration, () =>
            {
                _canvas.interactable = false;
                _canvas.blocksRaycasts = false;
            });
        }

        public void Starting() 
        {
            _canvasLayer1.interactable = true;
            _canvasLayer1.blocksRaycasts = true;
            _canvasLayer1.alpha = 1;

            _canvasLayer2.interactable = false;
            _canvasLayer2.blocksRaycasts = false;
            _canvasLayer2.alpha = 0;
        }

        IEnumerator _IESwitchLayer()
        {
            if (_canvasLayer1.interactable == true)
            {
                FadeOut(_canvasLayer1, 0.25f);
                yield return new WaitForSeconds(0.5f);
                FadeIn(_canvasLayer2, 0.25f);
            }
            else
            {
                FadeOut(_canvasLayer2, 0.25f);
                yield return new WaitForSeconds(0.5f);
                FadeIn(_canvasLayer1, 0.25f);
            }
        }

        //IEnumerator _IESwitchlamp()
        //{
        //    if (status_led_off.interactable == true)
        //    {
        //        FadeOut(status_led_off, 0.1f);
        //        yield return new WaitForSeconds(0.15f);
        //        FadeIn(status_led_on, 0.1f);
        //    }
        //    else
        //    {
        //        FadeOut(status_led_on, 0.1f);
        //        yield return new WaitForSeconds(0.15f);
        //        FadeIn(status_led_off, 0.1f);
        //    }
        //}
        public void SwitchLayer()
        {
            StartCoroutine(_IESwitchLayer());
        }

        //public void SwitchLamp()
        //{
        //   StartCoroutine(_IESwitchlamp());

        //}
        public void LostConnection()
        {
            temperature.text = "0°C";
            humidity.text = "0%";
            station.text = "";
            project.text = "";
            loc.text = "";
            Graph_Temperature.GetComponent<Graph>().b = false;
            Graph_Humidity.GetComponent<Graph>().b = false;
            temperature.GetComponentInParent<CircleSlider>().b = false;
            humidity.GetComponentInParent<CircleSlider>().b = false;
            Pump_Silder.GetComponent<SliderScripts>().b = false;
            LedLight.GetComponent<Blink>().blink = false;
            LED.GetComponent<Tgle>().Off_Switch();
            PUMP.GetComponent<Tgle>().Off_Switch();
            LED.GetComponent<Tgle>().switchBtn.GetComponent<Button>().interactable = false;
            PUMP.GetComponent<Tgle>().switchBtn.GetComponent<Button>().interactable = false;
        }
    }
}