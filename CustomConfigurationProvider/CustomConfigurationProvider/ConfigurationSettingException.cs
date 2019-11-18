using System;
using System.Diagnostics.CodeAnalysis;

namespace CustomConfigurationProviders
{

    [Serializable]
    [ExcludeFromCodeCoverage]
    public sealed class ConfigurationSettingException : Exception
    {
        public ConfigurationSettingException() { }
        public ConfigurationSettingException(string message) : base(message) { }
        public ConfigurationSettingException(string message, Exception inner) : base(message, inner) { }
        private ConfigurationSettingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
