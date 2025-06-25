using System;
using SerializeReferenceEditor;
using UnityEngine;

namespace _Scripts.Models
{
    [Serializable]
    public class AutoTargetEffect
    {
        [field: SerializeReference, SR]
        public TargetMode TargetMode { get; private set; }

        [field: SerializeReference, SR]
        public Effect Effect { get; private set; }
    }
}