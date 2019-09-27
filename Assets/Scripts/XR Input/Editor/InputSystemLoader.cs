using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace XRControls.XRInput
{
    [InitializeOnLoad]
    public class InputSystemLoader
    {
        private static List<string> missingInputs = new List<string>();

        #region Properties
        private static SerializedObject _inputManagerSerializedObject;

        static SerializedObject InputManagerSerializedObject 
        {
            get {
                if (_inputManagerSerializedObject == null) {
                    _inputManagerSerializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
                }
                return _inputManagerSerializedObject;
            }
        }

        #endregion

        static InputSystemLoader()
        {
            EditorApplication.update += RunOnStartup;
        }

        private static void RunOnStartup()
        {
            EditorApplication.update -= RunOnStartup;
            CheckInputs();
        }

        private static bool CheckInputs()
        {
            missingInputs.Clear();

            bool ret = CheckAndAddInput("Left " + StaticAliases.alias_Menu, false, StaticAliases.alias_MenuLeftID) & CheckAndAddInput("Right " + StaticAliases.alias_Menu, false, StaticAliases.alias_MenuRightID) &
                        CheckAndAddInput("Left " + StaticAliases.alias_TrackpadPress, false, StaticAliases.alias_TrackpadPressLeftID) & CheckAndAddInput("Right " + StaticAliases.alias_TrackpadPress, false, StaticAliases.alias_TrackpadPressRightID) &
                        CheckAndAddInput("Left " + StaticAliases.alias_TrackpadTouch, false, StaticAliases.alias_TrackpadTouchLeftID) & CheckAndAddInput("Right " + StaticAliases.alias_TrackpadTouch, false, StaticAliases.alias_TrackpadTouchRightID) &
                        CheckAndAddInput("Left " + StaticAliases.alias_TrackpadHorizontal, true, StaticAliases.alias_TrackpadHorizontalLeftID) & CheckAndAddInput("Right " + StaticAliases.alias_TrackpadHorizontal, true, StaticAliases.alias_TrackpadHorizontalRightID) &
                        CheckAndAddInput("Left " + StaticAliases.alias_TrackpadVertical, true, StaticAliases.alias_TrackpadVerticalLeftID) & CheckAndAddInput("Right " + StaticAliases.alias_TrackpadVertical, true, StaticAliases.alias_TrackpadVerticalRightID) &
                        CheckAndAddInput("Left " + StaticAliases.alias_TriggerTouch, false, StaticAliases.alias_TriggerTouchLeftID) & CheckAndAddInput("Right " + StaticAliases.alias_TriggerTouch, false, StaticAliases.alias_TriggerTouchRightID) &
                        CheckAndAddInput("Left " + StaticAliases.alias_TriggerSqueeze, true, StaticAliases.alias_TriggerSqueezeLeftID) & CheckAndAddInput("Right " + StaticAliases.alias_TriggerSqueeze, true, StaticAliases.alias_TriggerSqueezeRightID) &
                        CheckAndAddInput("Left " + StaticAliases.alias_GripSqueeze, true, StaticAliases.alias_GripSqueezeLeftID) & CheckAndAddInput("Right " + StaticAliases.alias_GripSqueeze, true, StaticAliases.alias_GripSqueezeRightID);

            if (ret)
            {
                Debug.Log("XR Inputs OK!");
            }
            else
            {
                System.Text.StringBuilder missingInputsNames = new System.Text.StringBuilder();
                for (int i = 0; i < missingInputs.Count; i++)
                {
                    missingInputsNames.Append("\n- " + missingInputs[i]);
                }

                Debug.Log("Added " + missingInputs.Count + " inputs: " + missingInputsNames);
            }

            return ret;
        }

        private static bool CheckAndAddInput(string axisName, bool isJoystick, int id)
        {
            if (AxisDefined(axisName))
            {
                return true;
            }
            else
            {
                missingInputs.Add(axisName);

                InputAxis newAxis = new InputAxis();

                newAxis.name = axisName;
                newAxis.gravity = isJoystick ? 0f : 1000f;
                newAxis.dead = isJoystick ? 0.19f : 0.001f;
                newAxis.sensitivity = isJoystick ? 1 : 1000f;
                newAxis.type = isJoystick ? AxisType.JoystickAxis : AxisType.KeyOrMouseButton;
                if (isJoystick)
                {
                    newAxis.axis = id;
                }
                else
                {
                    newAxis.positiveButton = "joystick button " + id.ToString();
                }

                AddAxis(newAxis);

                return false;
            }
        }

        private static void AddAxis(InputAxis axis)
        {
            if (AxisDefined(axis.name)) return;

            SerializedProperty axesProperty = InputManagerSerializedObject.FindProperty("m_Axes");

            axesProperty.arraySize++;
            InputManagerSerializedObject.ApplyModifiedProperties();

            SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

            GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
            GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
            GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
            GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
            GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
            GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
            GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
            GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
            GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
            GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
            GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
            GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
            GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
            GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
            GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

            InputManagerSerializedObject.ApplyModifiedProperties();
        }

        private static bool AxisDefined(string axisName)
        {
            // SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
            SerializedProperty axesProperty = InputManagerSerializedObject.FindProperty("m_Axes");

            axesProperty.Next(true);
            axesProperty.Next(true);
            while (axesProperty.Next(false))
            {
                SerializedProperty axis = axesProperty.Copy();
                axis.Next(true);
                if (axis.stringValue == axisName) return true;
            }
            return false;
        }

        private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
        {
            SerializedProperty child = parent.Copy();
            child.Next(true);
            do
            {
                if (child.name == name) return child;
            }
            while (child.Next(false));
            return null;
        }
    }

    public enum AxisType
    {
        KeyOrMouseButton = 0,
        MouseMovement = 1,
        JoystickAxis = 2
    };

    public class InputAxis
    {
        public string name;
        public string descriptiveName;
        public string descriptiveNegativeName;
        public string negativeButton;
        public string positiveButton;
        public string altNegativeButton;
        public string altPositiveButton;

        public float gravity;
        public float dead;
        public float sensitivity;

        public bool snap = false;
        public bool invert = false;

        public AxisType type;

        public int axis;
        public int joyNum;
    }
}