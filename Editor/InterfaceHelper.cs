using UnityEngine;
using UnityEditor;
using Turnflow.Unity.Custom;
using System.Collections.Generic;
using System.Linq;

namespace TurnFlow.Unity.Editor
{
    [CustomPropertyDrawer(typeof(BarStatFormulaEntry))]
    public class BarStatFormulaEntryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw the foldout label
            position.height = EditorGUIUtility.singleLineHeight;
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                float yOffset = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                // Get available stat names from the root Stats ScriptableObject
                List<string> availableStats = GetAvailableStats(property);
                SerializedProperty statNameProp = property.FindPropertyRelative("statName");

                if (availableStats.Count > 0)
                {
                    // Show dropdown for stat name
                    int currentIndex = availableStats.IndexOf(statNameProp.stringValue);
                    if (currentIndex < 0) currentIndex = 0;

                    int newIndex = EditorGUI.Popup(
                        new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight),
                        "Stat Name",
                        currentIndex,
                        availableStats.ToArray()
                    );

                    if (newIndex >= 0 && newIndex < availableStats.Count)
                    {
                        statNameProp.stringValue = availableStats[newIndex];
                    }
                }
                else
                {
                    // Show warning if no stats defined
                    EditorGUI.LabelField(
                        new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight),
                        "Stat Name",
                        "No stats defined!"
                    );
                }

                yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                // Draw multiplier field
                SerializedProperty multiplierProp = property.FindPropertyRelative("multiplier");
                EditorGUI.PropertyField(
                    new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight),
                    multiplierProp
                );

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            // Foldout + stat name + multiplier
            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3;
        }

        private List<string> GetAvailableStats(SerializedProperty property)
        {
            List<string> statNames = new List<string>();

            // Navigate up to find the root Stats ScriptableObject
            SerializedProperty rootProperty = property.serializedObject.FindProperty("stats");
            
            if (rootProperty != null && rootProperty.isArray)
            {
                for (int i = 0; i < rootProperty.arraySize; i++)
                {
                    SerializedProperty statEntry = rootProperty.GetArrayElementAtIndex(i);
                    SerializedProperty statNameProp = statEntry.FindPropertyRelative("statName");
                    
                    if (statNameProp != null && !string.IsNullOrEmpty(statNameProp.stringValue))
                    {
                        statNames.Add(statNameProp.stringValue);
                    }
                }
            }

            return statNames;
        }
    }


}