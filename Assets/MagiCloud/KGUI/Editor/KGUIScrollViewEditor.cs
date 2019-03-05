﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MagiCloud.KGUI
{
    /// <summary>
    /// 滚动视图
    /// </summary>
    [CustomEditor(typeof(KGUI_ScrollView))]
    [CanEditMultipleObjects]
    public class KGUIScrollViewEditor :Editor
    {
        public SerializedProperty vertical;
        public SerializedProperty horizontal;
        public SerializedProperty panel;
        public SerializedProperty content;
        public SerializedProperty isFollowHand;
        public SerializedProperty initYNum;
        public SerializedProperty initXNum;
        private KGUI_ScrollView scrollView;

        private void OnEnable()
        {
            scrollView = serializedObject.targetObject as KGUI_ScrollView;

            vertical = serializedObject.FindProperty("vertical");
            horizontal = serializedObject.FindProperty("horizontal");
            panel = serializedObject.FindProperty("panel");
            content = serializedObject.FindProperty("content");
            isFollowHand=serializedObject.FindProperty("isFollowHand");
            initYNum=serializedObject.FindProperty("initNum");
            initXNum=serializedObject.FindProperty("initXNum");
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical("box",GUILayout.Width(500));

            EditorGUILayout.PropertyField(vertical,true,null);
            EditorGUILayout.PropertyField(horizontal,true,null);
            EditorGUILayout.PropertyField(panel,true,null);
            EditorGUILayout.PropertyField(content,true,null);
            EditorGUILayout.PropertyField(isFollowHand,true,null);
            EditorGUILayout.PropertyField(initYNum,true,null);
            EditorGUILayout.PropertyField(initXNum,true,null);
            GUILayout.Space(20);

            if (GUILayout.Button("刷新",GUILayout.Width(100),GUILayout.Height(21)))
            {
                scrollView.SetRectData();
            }

            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
