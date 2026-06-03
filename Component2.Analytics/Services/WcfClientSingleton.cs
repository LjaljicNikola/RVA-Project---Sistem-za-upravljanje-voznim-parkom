using System;
using System.ServiceModel;
using Shared.Contracts.Interfaces;

namespace Component2.Analytics.Services
{
    /// <summary>
    /// Singleton pattern: ensures a single WCF channel factory instance is reused
    /// across the application lifetime, avoiding repeated connection overhead.
    /// The channel itself is recreated on demand if it faults.
    /// </summary>
    public sealed class WcfClientSingleton
    {
        private static readonly Lazy<WcfClientSingleton> _instance =
            new Lazy<WcfClientSingleton>(() => new WcfClientSingleton());

        public static WcfClientSingleton Instance => _instance.Value;

        private readonly NetTcpBinding _binding;
        private readonly EndpointAddress _endpoint;
        private ChannelFactory<IFleetService>? _factory;

        private WcfClientSingleton()
        {
            _binding = new NetTcpBinding
            {
                OpenTimeout = TimeSpan.FromSeconds(5),
                SendTimeout = TimeSpan.FromSeconds(10),
                ReceiveTimeout = TimeSpan.FromSeconds(10)
            };
            _endpoint = new EndpointAddress("net.tcp://localhost:8100/FleetService");
        }

        /// <summary>
        /// Returns an open IFleetService channel. Recreates the factory if faulted.
        /// </summary>
        public IFleetService GetChannel()
        {
            if (_factory == null || _factory.State == CommunicationState.Faulted)
            {
                _factory?.Abort();
                _factory = new ChannelFactory<IFleetService>(_binding, _endpoint);
            }

            return _factory.CreateChannel();
        }

        public void Close()
        {
            if (_factory != null && _factory.State == CommunicationState.Opened)
                _factory.Close();
        }
    }
}
