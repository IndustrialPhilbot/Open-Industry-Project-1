using UnityEngine;
using libplctag;
using libplctag.DataTypes;
using libplctag.NativeImport;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Opc.Ua.Client;
using Opc.Ua;
using Opc.Ua.Configuration;
using MyBox;

public class PLC : MonoBehaviour
{
    [SerializeField] private Protocols Protocol = new();

    [ConditionalField(nameof(Protocol), true, "opc_ua")] [SerializeField] private PlcType PlcType = new();

    [ConditionalField(nameof(Protocol), true, "opc_ua")] [SerializeField] private string Gateway = string.Empty;

    [ConditionalField(nameof(Protocol), true, "opc_ua")] [SerializeField] private string Path = string.Empty;

    [ConditionalField(nameof(Protocol), false, "opc_ua")] [SerializeField] private string EndPoint = "opc.tcp://localhost:62541/discovery";

    public int ScanTime = 100;

    readonly Dictionary<Guid, Tag<SintPlcMapper, sbyte>> bool_tags = new();
    readonly Dictionary<Guid, Tag<DintPlcMapper, int>> int_tags = new();
    readonly Dictionary<Guid, string> opc_tags = new();

    public enum Protocols
    {
        ab_eip,
        modbus_tcp,
        opc_ua
    }

    public Session session;
    private void OpcConnect()
    {
        ApplicationInstance application = new()
        {
            ApplicationType = ApplicationType.Client
        };

        application.LoadApplicationConfiguration(@".\Opc.Ua.SampleClient.Config.xml", false).Wait();
        ApplicationConfiguration m_configuration = application.ApplicationConfiguration;

        EndpointDescription endpointDescription = CoreClientUtils.SelectEndpoint(EndPoint, false);
        EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(m_configuration);
        ConfiguredEndpoint endpoint = new(null, endpointDescription, endpointConfiguration);

        m_configuration.CertificateValidator.AutoAcceptUntrustedCertificates = true;

        session = Session.Create(
          m_configuration,
          endpoint,
          false,
          false,
          "UA Sample Client",
          (uint)m_configuration.ClientConfiguration.DefaultSessionTimeout,
          new UserIdentity(),
          null).Result;
    }

    // 0 = bool
    // 1 = int
    public void Connect(string tagName, int dataType, Guid guid)
    {

        //OPC UA
        if (Protocol == Protocols.opc_ua)
        {
            opc_tags.Add(guid, tagName);
        }
        else
        {
            // libplctag
            try { plctag.ForceExtractLibrary = false; } catch { };

            if (dataType == 0)
            {
                Tag<SintPlcMapper, sbyte> tag = new()
                {
                    Name = tagName,
                    Gateway = Gateway,
                    Path = Path,
                    PlcType = PlcType,
                    Protocol = (Protocol?)Protocol,
                    Timeout = TimeSpan.FromSeconds(5)
                };

                bool_tags.Add(guid, tag);
            }
            else if (dataType == 1)
            {
                Tag<DintPlcMapper, int> tag = new()
                {
                    Name = tagName,
                    Gateway = Gateway,
                    Path = Path,
                    PlcType = PlcType,
                    Protocol = (Protocol?)Protocol,
                    Timeout = TimeSpan.FromSeconds(5)
                };

                int_tags.Add(guid, tag);
            }
        }
    }

    public async Task<int> Read(Guid guid)
    {
        //OPC UA
        if (Protocol == Protocols.opc_ua)
        {
            return Convert.ToInt32(session.ReadValueAsync(opc_tags[guid]).Result.Value);
        }

        //libplctag
        if (int_tags.ContainsKey(guid))
        {
            return await int_tags[guid].ReadAsync();
        }
        else
        {
            return Convert.ToInt32(await bool_tags[guid].ReadAsync());
        }
    }

    public async Task Write(Guid guid, sbyte value)
    {

        //OPC UA
        if (Protocol == Protocols.opc_ua)
        {
            RequestHeader requestHeader = new();

            WriteValueCollection writeValues = new();

            WriteValue writeValue = new()
            {
                NodeId = new NodeId(opc_tags[guid]),
                AttributeId = Attributes.Value,
                Value = new DataValue
                {
                    Value = value
                }
            };

            writeValues.Add(writeValue);

            await session.WriteAsync(requestHeader, writeValues, new System.Threading.CancellationToken());
        }

        //libplctag
        bool_tags[guid].Value = value;
        await bool_tags[guid].WriteAsync();
    }

    public async Task Write(Guid guid, int value)
    {
        int_tags[guid].Value = value;
        await int_tags[guid].WriteAsync();
    }


    private void Awake()
    {
        UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));

        if(Protocol == Protocols.opc_ua)
        {
            OpcConnect();
        }

    }
}


