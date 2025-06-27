using System;
using System.Linq;
using SerializeReferenceEditor.Services;

namespace SerializeReferenceEditor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class FormerlySerializedTypeAttribute : Attribute
    {
    #if UNITY_EDITOR
        private string _oldNamespace;
        private string _oldAssemblyName;
        private bool _isInitialized;
        private readonly string _oldTypeFullName;

        public string OldTypeName { get; }

        public string OldNamespace
        {
            get
            {
                EnsureInitialized();
                return _oldNamespace;
            }
        }

        public string OldAssemblyName
        {
            get
            {
                EnsureInitialized();
                return _oldAssemblyName;
            }
        }

        public FormerlySerializedTypeAttribute(string oldTypeFullName)
        {
            if (string.IsNullOrEmpty(oldTypeFullName))
            {
                throw new ArgumentException("Type name cannot be null or empty", nameof(oldTypeFullName));
            }

            _oldTypeFullName = oldTypeFullName;

            string[] assemblyAndType = oldTypeFullName.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string typeWithNamespace;

            if (assemblyAndType.Length > 1)
            {
                _oldAssemblyName = assemblyAndType[0].Trim();
                typeWithNamespace = assemblyAndType[1].Trim();
            }
            else
            {
                typeWithNamespace = assemblyAndType[0].Trim();
            }

            string[] parts = typeWithNamespace.Split('.');
            if (parts.Length <= 1)
            {
                OldTypeName = parts[0];
            }
            else
            {
                OldTypeName = parts[^1];
                _oldNamespace = string.Join(".", parts.Take(parts.Length - 1));
            }

            _isInitialized = !string.IsNullOrEmpty(_oldAssemblyName) && !string.IsNullOrEmpty(_oldNamespace);
        }

        private void EnsureInitialized()
        {
            if (_isInitialized)
            {
                return;
            }

            Type type = SRFormerlyTypeCache.GetTypeForAttribute(this);
            if (type != null)
            {
                _oldAssemblyName ??= type.Assembly.GetName().Name ?? string.Empty;
                _oldNamespace ??= type.Namespace ?? string.Empty;
                _isInitialized = true;
            }
        }
    #else
        public FormerlySerializedTypeAttribute(string oldTypeFullName)
        {
        }
    #endif
    }
}