using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Turnflow.Unity.Custom
{

    public enum BarResetType
    {
        toMinimum,
        toMaximum,
        noUpdate,
    }

    public enum BarMaxCalcType
    {
        constant,
        Formula,
    }

    [Serializable]
    public class StatEntry
    {
        [Tooltip("Define a unique stat name (e.g., 'strength', 'vitality')")]
        public string statName;
    }

    [Serializable]
    public class BarStatFormulaEntry
    {
        public string statName;
        public float multiplier = 1f;
    }

    [Serializable]
    public class BarStatFormulaOption
    {
        public int baseBarValue;
        public List<BarStatFormulaEntry> statFormulas = new List<BarStatFormulaEntry>();
    }

    [Serializable]
    public class BarEntry
    {
        public string statName = "health";
        public int minimumValue = 0;
        public BarResetType resetType = BarResetType.toMaximum;
        
        [HorizontalLine(color: EColor.Gray)]
        public BarMaxCalcType maxCalcType = BarMaxCalcType.constant;

        [AllowNesting]
        [ShowIf("maxCalcType", BarMaxCalcType.constant)]
        public int maximumValue = 100;

        [AllowNesting]
        [ShowIf("maxCalcType", BarMaxCalcType.Formula)]
        public BarStatFormulaOption formulaOption;
        
    }

    [CreateAssetMenu(fileName = "Schema", menuName = "Custom/Schema")]
    public class Stats : ScriptableObject
    {
        
        [SerializeField] 
        private List<StatEntry> stats = new List<StatEntry>();

        [SerializeField] 
        private List<BarEntry> bars = new List<BarEntry>();

    }

}