using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Snake
{
    enum InputState
    {
        getFirtKey,
        getSecondKey,
        isWaitingToDefine,
        cancelledDefine,
        none
    }

    public class DefineInput : MonoBehaviour
    {
        public Action finishDefineInput;
        public List<KeyCode[]> definedInputs = new List<KeyCode[]>();
        private List<KeyCode> usedKeys = new List<KeyCode>();
        private bool isDefiningInputs;

        private KeyCode firstKey;
        private KeyCode secondKey;
        private KeyCode[] keyCodes ;
        private float pressTime;

        //Debug
        public Text text;

        InputState inputState = InputState.none;
        

        public void Start()
        {
            isDefiningInputs = true;
            inputState = InputState.getFirtKey;
        }

        private void Update()
        {
            text.text = inputState.ToString();
            if (isDefiningInputs)
            {
                if (inputState == InputState.isWaitingToDefine) 
                { 
                    
                    var aux =  Input.GetKey(firstKey);
                    var aux1 = Input.GetKey(secondKey);

                    if (Input.GetKey(firstKey) || Input.GetKey(secondKey))
                    {
                        //if is pressed for 1 second confirm defined keys
                        if (Time.time - pressTime > 1)
                        {
                            var key1Weight = KeysWeights[firstKey];
                            var key2Weight = KeysWeights[secondKey];

                            //The smaller weight is the left key the 
                            //other will be the right
                            if(key1Weight > key2Weight)
                                keyCodes = new KeyCode[2] { secondKey, firstKey };
                            else
                                keyCodes = new KeyCode[2] { firstKey,secondKey };

                            keyCodes = new KeyCode[2] { firstKey, secondKey };
                            definedInputs.Add(keyCodes);
                            inputState = InputState.getFirtKey;
                            Debug.Log("keys " + definedInputs[definedInputs.Count - 1]);
                        }
                    }
                    //This means that the keys were not hold for the needed time
                    else
                    {
                        //Take out keys from usedkeys list, that way they will be
                        //avaible again;
                        usedKeys.Remove(firstKey);

                        inputState = InputState.getFirtKey;
                        //Todo cosiderer weigth
                        firstKey = KeyCode.None;
                        secondKey = KeyCode.None;

                        pressTime = 0;
                       
                    }
                }
            }
        }


        void OnGUI()
        {
            Event e = Event.current;
            if (inputState != InputState.isWaitingToDefine && e.isKey && e.keyCode != KeyCode.None)
            {
                if(IsAvaibleKey(e.keyCode))
                    AddKey(e.keyCode);
            }
        }

        private void AddKey(KeyCode key)
        {
            Debug.Log("Add key" + key);
            if (inputState == InputState.getFirtKey) 
            {
                firstKey = key;
                usedKeys.Add(key);
                inputState = InputState.getSecondKey;
            }else if (inputState == InputState.getSecondKey)
            {
                secondKey = key;
                usedKeys.Add(key);
                pressTime = Time.time;
                inputState = InputState.isWaitingToDefine;
            }
        }
        KeysWeights

        private bool IsAvaibleKey(KeyCode key)
        {
            var list = usedKeys.Find((obj) => obj == key);
           
            return list != null;
        }

        //public void Update()
        //{
        //    if (isDefiningInputs)
        //    {
        //        Event e = Event.current;
        //        if (e.isKey)
        //        {
        //            Debug.Log("key " + e.keyCode);
        //        }
        //    }
        //}


        //Only here to make more readable
        Dictionary<KeyCode, int> KeysWeights = new Dictionary<KeyCode, int>()
        {
            { KeyCode.Alpha1,1 },
            { KeyCode.Q,2 },
            { KeyCode.A,3 },
            { KeyCode.Z,4 },
            { KeyCode.Alpha2,5 },
            { KeyCode.W,6 },
            { KeyCode.S,7 },
            { KeyCode.X,8 },
            { KeyCode.Alpha3,9 },
            { KeyCode.E,10 },
            { KeyCode.D,11 },
            { KeyCode.C,12 },
            { KeyCode.Alpha4,13 },
            { KeyCode.R,14 },
            { KeyCode.F,15 },
            { KeyCode.V,16 },
            { KeyCode.Alpha5,17 },
            { KeyCode.T,18 },
            { KeyCode.G,19 },
            { KeyCode.B,20 },
            { KeyCode.Alpha6,21 },
            { KeyCode.Y,22 },
            { KeyCode.H,23 },
            { KeyCode.N,24 },
            { KeyCode.Alpha7,25 },
            { KeyCode.U,26 },
            { KeyCode.J,27 },
            { KeyCode.M,28 },
            { KeyCode.Alpha8,29 },
            { KeyCode.I,30 },
            { KeyCode.K,31 },
            { KeyCode.Alpha9,32 },
            { KeyCode.O,33 },
            { KeyCode.L,34 },
            { KeyCode.Alpha0,35 },
            { KeyCode.P,36 }

        };


    }
}
