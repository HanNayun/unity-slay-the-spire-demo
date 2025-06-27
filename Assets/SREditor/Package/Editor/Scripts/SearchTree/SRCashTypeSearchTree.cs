using System.Collections.Generic;
using SerializeReferenceEditor.Editor.Comparers;

namespace SerializeReferenceEditor.Editor.Drawers
{
    public class SRCashTypeSearchTree
    {
        private readonly TypeInfoArrayComparer _typeInfoComparer = new();
        private readonly Dictionary<int, SRTypeTreeFactory> _cashTypes = new();

        public SRTypeTreeFactory GetTypeTreeFactory(TypeInfo[] types)
        {
            SRTypeTreeFactory typeTreeFactory;
            int typesHash = _typeInfoComparer.GetHashCode(types);
            if (_cashTypes.TryGetValue(typesHash, out SRTypeTreeFactory result))
            {
                typeTreeFactory = result;
            }
            else
            {
                typeTreeFactory = new SRTypeTreeFactory(types);
                _cashTypes.Add(typesHash, typeTreeFactory);
            }

            return typeTreeFactory;
        }
    }
}