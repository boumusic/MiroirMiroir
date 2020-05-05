using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    private AudioManager a;

    private void OnEnable()
    {
        a = (AudioManager)target;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        SourceAmount();
        Initialize();

        EditorGUILayout.Space();

        FoldoutClipSettings();

        AddClipButton();

        serializedObject.ApplyModifiedProperties();
    }

    private void SourceAmount()
    {
        EditorGUILayout.BeginVertical("box");
        SerializedProperty sourceAmount = serializedObject.FindProperty("sourceAmount");
        EditorGUILayout.PropertyField(sourceAmount);
    }

    private void FoldoutClipSettings()
    {
        if(a.clips.Count > 0)
        {
            a.clipsFoldout = EditorGUILayout.Foldout(a.clipsFoldout, "Clips", EditorStyles.foldoutHeader);

            if (a.clipsFoldout)
            {
                DrawClipSettings();
            }
        }
    }

    private void AddClipButton()
    {
        if (GUILayout.Button("Add Clip"))
        {
            Undo.RecordObject(a, "Add Clip");
            a.clips.Add(new AudioClipSettings());
        }
    }

    private void Initialize()
    {
        if(a.sourceAmount != a.sourcePool.Count)
        {
            if (GUILayout.Button("Instantiate Sources"))
            {
                Undo.RecordObject(a, "Instantiate Sources");
                a.Initialize();
            }
        }

        else
        {
            if (GUILayout.Button("Destroy Sources"))
            {
                Undo.RecordObject(a, "Destroy Sources");
                a.DestroySources();
            }
        }


        EditorGUILayout.EndVertical();
    }

    private void DrawClipSettings()
    {
        SerializedProperty clips = serializedObject.FindProperty("clips");
        for (int i = 0; i < clips.arraySize; i++)
        {
            DrawClipSetting(i, clips);
        }
    }

    private void DrawClipSetting(int i, SerializedProperty clips)
    {
        if(i < clips.arraySize)
        {

            SerializedProperty clipSettings = clips.GetArrayElementAtIndex(i);
            SerializedProperty clipAsset = clipSettings.FindPropertyRelative("clip");
            SerializedProperty volume = clipSettings.FindPropertyRelative("volume");
            SerializedProperty pan = clipSettings.FindPropertyRelative("pan");
            SerializedProperty loop = clipSettings.FindPropertyRelative("loop");
            SerializedProperty awk = clipSettings.FindPropertyRelative("playOnAwake");
            SerializedProperty pitch = clipSettings.FindPropertyRelative("randomPitch");
            SerializedProperty variations = clipSettings.FindPropertyRelative("randomVariations");

            EditorGUILayout.BeginVertical("box");

            if(i < a.clips.Count)
            {
                string labelString = a.clips[i].clip == null ? "New Clip" : a.clips[i].clip.name;
                clipSettings.isExpanded = EditorGUILayout.Foldout(clipSettings.isExpanded, labelString, EditorStyles.foldoutHeader);
            }
            if(clipSettings.isExpanded)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(clipAsset);

                if (GUILayout.Button("Remove", GUILayout.MaxWidth(80)))
                {
                    Undo.RecordObject(a, "Remove");
                    a.clips.RemoveAt(i);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(volume);
                EditorGUILayout.PropertyField(pan);
                EditorGUILayout.PropertyField(loop);
                EditorGUILayout.PropertyField(awk);
                EditorGUILayout.PropertyField(pitch);
                EditorGUILayout.PropertyField(variations);
            }            

            EditorGUILayout.EndVertical();
        }
    }
}
