// This file was generated by a tool; you should avoid making direct changes.
// Consider using 'partial classes' to extend these types
// Input: MsgDefineCommon.proto

#pragma warning disable CS1591, CS0612, CS3021, IDE1006
namespace ProtoMsg
{

    [global::ProtoBuf.ProtoContract()]
    public partial class MsgNetAddress : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public uint u32Port { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public uint u32Ip { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        [global::System.ComponentModel.DefaultValue("")]
        public string domainName { get; set; } = "";

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MsgRegServerInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public uint uType { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public uint uServerId { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        public MsgNetAddress netAddr { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MsgDBStoreInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong u64MinID { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public ulong u64MaxID { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        public ulong u64WarnID { get; set; }

        [global::ProtoBuf.ProtoMember(4)]
        public ulong u64CurMaxUUID { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MsgClientPlatStoreInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong u64PlatformID { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public uint u32Online { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        [global::System.ComponentModel.DefaultValue("")]
        public string strPlatformName { get; set; } = "";

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MsgGSStoreInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public long n64Max { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public long n64Warn { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        public long n64Cur { get; set; }

        [global::ProtoBuf.ProtoMember(4, Name = @"platInfo")]
        public global::System.Collections.Generic.List<MsgClientPlatStoreInfo> platInfoes { get; } = new global::System.Collections.Generic.List<MsgClientPlatStoreInfo>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MsgPlayerBase : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong u64UserId { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public ulong u64UUID { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        [global::System.ComponentModel.DefaultValue("")]
        public string strAccount { get; set; } = "";

        [global::ProtoBuf.ProtoMember(4)]
        [global::System.ComponentModel.DefaultValue("")]
        public string strName { get; set; } = "";

        [global::ProtoBuf.ProtoMember(5)]
        public int n32Level { get; set; }

        [global::ProtoBuf.ProtoMember(6)]
        public int n32Exp { get; set; }

        [global::ProtoBuf.ProtoMember(7)]
        public int n32DayOnlineTime { get; set; }

        [global::ProtoBuf.ProtoMember(8)]
        public int n32TotalOnlineTime { get; set; }

        [global::ProtoBuf.ProtoMember(9)]
        [global::System.ComponentModel.DefaultValue("")]
        public string strRegisterTime { get; set; } = "";

        [global::ProtoBuf.ProtoMember(10)]
        [global::System.ComponentModel.DefaultValue("")]
        public string strLastLogoutTime { get; set; } = "";

        [global::ProtoBuf.ProtoMember(11)]
        [global::System.ComponentModel.DefaultValue("")]
        public string strLastLogoutIp { get; set; } = "";

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MsgSessionInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        [global::System.ComponentModel.DefaultValue("")]
        public string strOfflineSession { get; set; } = "";

        [global::ProtoBuf.ProtoMember(2)]
        public long n64ExpiredTime { get; set; }

    }

}

#pragma warning restore CS1591, CS0612, CS3021, IDE1006