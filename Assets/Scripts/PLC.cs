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

    [SerializeField] private int scanTime = 100;
    public int ScanTime { get { return scanTime; } private set { scanTime = value; } }

    readonly Dictionary<Guid, (GameObject,Tag<SintPlcMapper, sbyte>)> bool_tags = new();
    readonly Dictionary<Guid, (GameObject,Tag<DintPlcMapper, int>)> int_tags = new();
    readonly Dictionary<Guid, (GameObject,Tag<RealPlcMapper, float>)> float_tags = new();
    readonly Dictionary<Guid, (GameObject,string)> opc_tags = new();

    public enum Protocols
    {
        ab_eip,
        modbus_tcp,
        opc_ua
    }

    public enum DataType
    {
        Bool,
        Int,
        Float
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
    public void Connect(Guid guid, DataType dataType, string tagName, GameObject gameObject)
    {

        //OPC UA
        if (Protocol == Protocols.opc_ua)
        {
            if (!opc_tags.ContainsKey(guid))
            {
                opc_tags.Add(guid, (gameObject, tagName));
            }
        }
        else
        {
            // libplctag
            try { plctag.ForceExtractLibrary = false; } catch { };

            if (dataType == DataType.Bool)
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

                bool_tags.Add(guid, (gameObject,tag));
            }
            else if (dataType == DataType.Int)
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

                int_tags.Add(guid, (gameObject, tag));
            }
            else if (dataType == DataType.Float)
            {
                Tag<RealPlcMapper, float> tag = new()
                {
                    Name = tagName,
                    Gateway = Gateway,
                    Path = Path,
                    PlcType = PlcType,
                    Protocol = (Protocol?)Protocol,
                    Timeout = TimeSpan.FromSeconds(5)
                };

                float_tags.Add(guid, (gameObject, tag));
            }
        }
    }
    public async Task<bool> ReadBool(Guid guid)
    {
        try
        {
            if (Protocol == Protocols.opc_ua)
            {
                return Convert.ToBoolean(session.ReadValueAsync(opc_tags[guid].Item2).Result.Value);
            }
            else
            {
                return Convert.ToBoolean(await bool_tags[guid].Item2.ReadAsync());
            }
        }
        catch
        {
            Debug.Log("Failure to read: " + opc_tags[guid].Item2 + " in GameObject: " + opc_tags[guid].Item1.name);
            return false;
        }
    }

    public async Task<int> ReadInt(Guid guid)
    {
        try
        {
            if (Protocol == Protocols.opc_ua)
            {
                return Convert.ToInt32(session.ReadValueAsync(opc_tags[guid].Item2).Result.Value);
            }
            else
            {
                return Convert.ToInt32(await bool_tags[guid].Item2.ReadAsync());
            }
        }
        catch
        {
            Debug.Log("Failure to read: " + opc_tags[guid].Item2 + " in GameObject: " + opc_tags[guid].Item1.name);
            return 0;
        }
    }

    public async Task<float> ReadFloat(Guid guid)
    {
        try
        {
            if (Protocol == Protocols.opc_ua)
            {
                return (float)session.ReadValueAsync(opc_tags[guid].Item2).Result.Value;
            }
            else
            {
                return (float)(await float_tags[guid].Item2.ReadAsync());
            }
        }
        catch
        {
            Debug.Log("Failure to read: " + opc_tags[guid].Item2 + " in GameObject: " + opc_tags[guid].Item1.name);
            return 0;
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
                NodeId = new NodeId(opc_tags[guid].Item2),
                AttributeId = Attributes.Value,
                Value = new DataValue
                {
                    Value = Convert.ToBoolean(value)
                }
            };

            writeValues.Add(writeValue);

            var writeResult = await session.WriteAsync(requestHeader, writeValues, new System.Threading.CancellationToken());
            //Debug.Log(writeResult.Results[0].ToString());
        }
        else
        {
            //libplctag
            bool_tags[guid].Item2.Value = value;
            await bool_tags[guid].Item2.WriteAsync();
        }
    }

    public async Task Write(Guid guid, int value)
    {
        //OPC UA
        if (Protocol == Protocols.opc_ua)
        {
            RequestHeader requestHeader = new();

            WriteValueCollection writeValues = new();

            WriteValue writeValue = new()
            {
                NodeId = new NodeId(opc_tags[guid].Item2),
                AttributeId = Attributes.Value,
                Value = new DataValue
                {
                    Value = Convert.ToInt16(value)
                }
            };

            writeValues.Add(writeValue);

            var writeResult = await session.WriteAsync(requestHeader, writeValues, new System.Threading.CancellationToken());
            //Debug.Log(writeResult.Results[0].ToString());
        }
        else
        {
            int_tags[guid].Item2.Value = value;
            await int_tags[guid].Item2.WriteAsync();
        }
    }

    public async Task Write(Guid guid, float value)
    {
        //OPC UA
        if (Protocol == Protocols.opc_ua)
        {
            RequestHeader requestHeader = new();

            WriteValueCollection writeValues = new();

            WriteValue writeValue = new()
            {
                NodeId = new NodeId(opc_tags[guid].Item2),
                AttributeId = Attributes.Value,
                Value = new DataValue
                {
                    Value = value
                }
            };

            writeValues.Add(writeValue);

            var writeResult = await session.WriteAsync(requestHeader, writeValues, new System.Threading.CancellationToken());
            //Debug.Log(writeResult.Results[0].ToString());
        }
        else
        {
            float_tags[guid].Item2.Value = value;
            await float_tags[guid].Item2.WriteAsync();
        }
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


