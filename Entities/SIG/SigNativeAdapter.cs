using System;
using System.Collections.Generic;
using System.Text;
using static Nrsc5Sharp.Nrsc5Native;

namespace Nrsc5Sharp.Entities.SIG
{
    internal unsafe static class SigNativeAdapter
    {
        public static IReadOnlyList<ISigService> Convert(nrsc5_sig_service_t* services)
        {
            List<ISigService> result = new List<ISigService>();
            while (services != null)
            {
                result.Add(new ServiceImpl(*services));
                services = (nrsc5_sig_service_t*)services->next;
            }
            return result;
        }

        class ServiceImpl : ISigService
        {
            public ServiceImpl(nrsc5_sig_service_t service)
            {
                //Set
                type = service.type;
                number = service.number;
                name = Nrsc5EntityWrappers.StringOrNull(service.name);
                components = new List<ISigComponent>();

                //Wrap components
                nrsc5_sig_component_t* component = (nrsc5_sig_component_t*)service.components;
                while (component != null)
                {
                    switch (component->type)
                    {
                        case 0: components.Add(new AudioComponentImpl(*component)); break;
                        case 1: components.Add(new DataComponentImpl(*component)); break;
                        default: throw new Exception($"Unknown component type {component->type}!");
                    }
                    component = (nrsc5_sig_component_t*)component->next;
                }
            }

            private readonly byte type;
            private readonly ushort number;
            private readonly string name;
            private readonly List<ISigComponent> components;

            public SigServiceType Type => (SigServiceType)type;

            public ushort Number => number;

            public string Name => name;

            public IReadOnlyList<ISigComponent> Components => components;
        }

        class ComponentImpl : ISigComponent
        {
            public ComponentImpl(nrsc5_sig_component_t component)
            {
                id = component.id;
            }

            private readonly byte id;

            public byte Id => id;
        }

        class AudioComponentImpl : ComponentImpl, ISigAudioComponent
        {
            public AudioComponentImpl(nrsc5_sig_component_t component) : base(component)
            {
                port = component.audio.port;
                type = component.audio.type;
                mime = component.audio.mime;
            }

            private readonly ushort port;
            private readonly byte type;
            private readonly uint mime;

            public ushort Port => port;

            public byte Type => type;

            public Nrsc5MimeType Mime => (Nrsc5MimeType)mime;
        }

        class DataComponentImpl : ComponentImpl, ISigDataComponent
        {
            public DataComponentImpl(nrsc5_sig_component_t component) : base(component)
            {
                port = component.data.port;
                type = component.data.type;
                serviceDataType = component.data.service_data_type;
                mime = component.data.mime;
            }

            private readonly ushort port;
            private readonly byte type;
            private readonly ushort serviceDataType;
            private readonly uint mime;

            public ushort Port => port;

            public byte Type => type;

            public Nrsc5MimeType Mime => (Nrsc5MimeType)mime;

            public ushort ServiceDataType => serviceDataType;
        }
    }
}
