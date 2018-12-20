﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MagiCloud.KGUI
{
    [CustomEditor(typeof(KGUI_Dropdown))]
    [CanEditMultipleObjects]
    public class KGUIDropdownEditor :Editor
    {
        private KGUIButtonTypeEditor buttonType;
        private KGUIButtonAudioEditor buttonAudio;

        private KGUI_Dropdown dropdown;
        public SerializedProperty isTouchExpand;
        public SerializedProperty Names;

        public SerializedProperty ScrollView;

        public SerializedProperty Template;
        public SerializedProperty textName;

        public SerializedProperty gridLayout;
        public SerializedProperty dropdownItem;

        private void OnEnable()
        {
            dropdown = serializedObject.targetObject as KGUI_Dropdown;

            if (buttonType == null)
                buttonType = new KGUIButtonTypeEditor();

            if (buttonAudio == null)
                buttonAudio = new KGUIButtonAudioEditor();

            buttonType.OnInstantiation(serializedObject);
            buttonAudio.OnInstantiation(serializedObject);
            isTouchExpand=serializedObject.FindProperty("isTouchExpand");
            Names = serializedObject.FindProperty("Names");

            ScrollView = serializedObject.FindProperty("scrollView");

            Template = serializedObject.FindProperty("Template");

            textName = serializedObject.FindProperty("textName");
            gridLayout = serializedObject.FindProperty("gridLayout");
            dropdownItem = serializedObject.FindProperty("dropdownItem");

        }

        public override void OnInspectorGUI()
        {

            buttonType.OnInspectorButtonType(dropdown);
            buttonAudio.OnInspectorButtonAudio(dropdown);

            GUILayout.BeginVertical("box",GUILayout.Width(500));

            GUILayout.Space(10);

            EditorGUILayout.LabelField("下拉框属性",MUtilityStyle.LabelStyle);
          
            EditorGUILayout.PropertyField(Names,true,null);
            EditorGUILayout.PropertyField(ScrollView,true,null);
            EditorGUILayout.PropertyField(Template,true,null);
            EditorGUILayout.PropertyField(textName,true,null);

            EditorGUILayout.PropertyField(gridLayout,true,null);
            EditorGUILayout.PropertyField(dropdownItem,true,null);
            EditorGUILayout.PropertyField(isTouchExpand,true,null);
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

    }
}
