using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Modules.Core.Common
{
    [Serializable]
    public class AssetReference<TObject> : AssetReferenceT<TObject>, ISerializable, ISerializationCallbackReceiver
        where TObject : Object
    {
        [SerializeField, SuppressMessage("ReSharper", "InconsistentNaming")]
        private string m_AssetName;

        public string AssetName => m_AssetName;

        public AssetReference(string guid) : base(guid)
        {
        }

        public AssetReference(SerializationInfo info, StreamingContext context) : base((string) info.GetValue("_guid", typeof(string)))
        {
        }

        public override bool Equals(object other)
        {
            return other is AssetReference<TObject> reference && reference.AssetGUID == AssetGUID;
        }

        public override int GetHashCode()
        {
            return AssetGUID.GetHashCode();
        }

        public sealed override string ToString()
        {
            return AssetGUID;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_guid", AssetGUID, typeof(string));
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
#if UNITY_EDITOR
            string name = System.IO.Path.GetFileNameWithoutExtension(UnityEditor.AssetDatabase.GUIDToAssetPath(AssetGUID));
            if (!string.IsNullOrWhiteSpace(name) && !name.Equals(m_AssetName))
            {
                m_AssetName = name;
            }
#endif
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
        }

        public static bool operator ==(AssetReference<TObject> a, AssetReference<TObject> b) => a?.AssetGUID == b?.AssetGUID;

        public static bool operator !=(AssetReference<TObject> a, AssetReference<TObject> b) => a?.AssetGUID != b?.AssetGUID;
    }
}