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

    readonly Dictionary<GameObject, Tag<SintPlcMapper, sbyte>> bool_tags = new();
    readonly Dictionary<GameObject, Tag<DintPlcMapper, int>> int_tags = new();
    readonly Dictionary<GameObject, string> opc_tags = new();

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
    public void Connect(string tagName, int dataType, GameObject gameObject)
    {

        //OPC UA
        if (Protocol == Protocols.opc_ua)
        {
            opc_tags.Add(gameObject, tagName);
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

                bool_tags.Add(gameObject, tag);
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

                int_tags.Add(gameObject, tag);
            }
        }
    }

    public async Task<int> Read(GameObject gameObject)
    {
        //OPC UA
        if (Protocol == Protocols.opc_ua)
        {
            return Convert.ToInt32(session.ReadValueAsync(opc_tags[gameObject]).Result.Value);
        }

        //libplctag
        if (int_tags.ContainsKey(gameObject))
        {
            return await int_tags[gameObject].ReadAsync();
        }
        else
        {
            return Convert.ToInt32(await bool_tags[gameObject].ReadAsync());
        }
    }

    public async Task Write(GameObject gameObject, sbyte value)
    {

        //OPC UA
        if (Protocol == Protocols.opc_ua)
        {
            RequestHeader requestHeader = new();

            WriteValueCollection writeValues = new();

            WriteValue writeValue = new()
            {
                NodeId = new NodeId(opc_tags[gameObject]),
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
        bool_tags[gameObject].Value = value;
        await bool_tags[gameObject].WriteAsync();
    }

    public async Task Write(GameObject gameObject, int value)
    {
        int_tags[gameObject].Value = value;
        await int_tags[gameObject].WriteAsync();
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


