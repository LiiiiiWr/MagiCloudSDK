// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: SystemSettingInfo.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace MagiCloud.NetWorks
{

  /// <summary>Holder for reflection information generated from SystemSettingInfo.proto</summary>
  public static partial class SystemSettingInfoReflection {

    #region Descriptor
    /// <summary>File descriptor for SystemSettingInfo.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static SystemSettingInfoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChdTeXN0ZW1TZXR0aW5nSW5mby5wcm90bxIITUNTZXJ2ZXIikwEKEVN5c3Rl",
            "bVNldHRpbmdJbmZvEg4KBnZvbHVtZRgBIAEoBRI1CgR0eXBlGAIgASgOMicu",
            "TUNTZXJ2ZXIuU3lzdGVtU2V0dGluZ0luZm8uUGVyZm9ybWFuY2UiNwoLUGVy",
            "Zm9ybWFuY2USCAoETk9ORRAAEgkKBUhJR0hUEAESCgoGTUlERExFEAISBwoD",
            "TE9XEAMiNwoKU2V0dGluZ1JlcRIpCgRpbmZvGAEgASgLMhsuTUNTZXJ2ZXIu",
            "U3lzdGVtU2V0dGluZ0luZm8iGgoKU2V0dGluZ1JlcxIMCgRiYWNrGAEgASgF",
            "YgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::MagiCloud.NetWorks.SystemSettingInfo), global::MagiCloud.NetWorks.SystemSettingInfo.Parser, new[]{ "Volume", "Type" }, null, new[]{ typeof(global::MagiCloud.NetWorks.SystemSettingInfo.Types.Performance) }, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::MagiCloud.NetWorks.SettingReq), global::MagiCloud.NetWorks.SettingReq.Parser, new[]{ "Info" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::MagiCloud.NetWorks.SettingRes), global::MagiCloud.NetWorks.SettingRes.Parser, new[]{ "Back" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///ϵͳ������Ϣ
  /// </summary>
  public sealed partial class SystemSettingInfo : pb::IMessage<SystemSettingInfo> {
    private static readonly pb::MessageParser<SystemSettingInfo> _parser = new pb::MessageParser<SystemSettingInfo>(() => new SystemSettingInfo());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<SystemSettingInfo> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::MagiCloud.NetWorks.SystemSettingInfoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SystemSettingInfo() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SystemSettingInfo(SystemSettingInfo other) : this() {
      volume_ = other.volume_;
      type_ = other.type_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SystemSettingInfo Clone() {
      return new SystemSettingInfo(this);
    }

    /// <summary>Field number for the "volume" field.</summary>
    public const int VolumeFieldNumber = 1;
    private int volume_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Volume {
      get { return volume_; }
      set {
        volume_ = value;
      }
    }

    /// <summary>Field number for the "type" field.</summary>
    public const int TypeFieldNumber = 2;
    private global::MagiCloud.NetWorks.SystemSettingInfo.Types.Performance type_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::MagiCloud.NetWorks.SystemSettingInfo.Types.Performance Type {
      get { return type_; }
      set {
        type_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as SystemSettingInfo);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(SystemSettingInfo other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Volume != other.Volume) return false;
      if (Type != other.Type) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Volume != 0) hash ^= Volume.GetHashCode();
      if (Type != 0) hash ^= Type.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Volume != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Volume);
      }
      if (Type != 0) {
        output.WriteRawTag(16);
        output.WriteEnum((int) Type);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Volume != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Volume);
      }
      if (Type != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Type);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(SystemSettingInfo other) {
      if (other == null) {
        return;
      }
      if (other.Volume != 0) {
        Volume = other.Volume;
      }
      if (other.Type != 0) {
        Type = other.Type;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Volume = input.ReadInt32();
            break;
          }
          case 16: {
            Type = (global::MagiCloud.NetWorks.SystemSettingInfo.Types.Performance) input.ReadEnum();
            break;
          }
        }
      }
    }

    #region Nested types
    /// <summary>Container for nested types declared in the SystemSettingInfo message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      /// <summary>
      ///����
      /// </summary>
      public enum Performance {
        [pbr::OriginalName("NONE")] None = 0,
        [pbr::OriginalName("HIGHT")] Hight = 1,
        [pbr::OriginalName("MIDDLE")] Middle = 2,
        [pbr::OriginalName("LOW")] Low = 3,
      }

    }
    #endregion

  }

  public sealed partial class SettingReq : pb::IMessage<SettingReq> {
    private static readonly pb::MessageParser<SettingReq> _parser = new pb::MessageParser<SettingReq>(() => new SettingReq());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<SettingReq> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::MagiCloud.NetWorks.SystemSettingInfoReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SettingReq() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SettingReq(SettingReq other) : this() {
      info_ = other.info_ != null ? other.info_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SettingReq Clone() {
      return new SettingReq(this);
    }

    /// <summary>Field number for the "info" field.</summary>
    public const int InfoFieldNumber = 1;
    private global::MagiCloud.NetWorks.SystemSettingInfo info_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::MagiCloud.NetWorks.SystemSettingInfo Info {
      get { return info_; }
      set {
        info_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as SettingReq);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(SettingReq other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Info, other.Info)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (info_ != null) hash ^= Info.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (info_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Info);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (info_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Info);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(SettingReq other) {
      if (other == null) {
        return;
      }
      if (other.info_ != null) {
        if (info_ == null) {
          Info = new global::MagiCloud.NetWorks.SystemSettingInfo();
        }
        Info.MergeFrom(other.Info);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (info_ == null) {
              Info = new global::MagiCloud.NetWorks.SystemSettingInfo();
            }
            input.ReadMessage(Info);
            break;
          }
        }
      }
    }

  }

  public sealed partial class SettingRes : pb::IMessage<SettingRes> {
    private static readonly pb::MessageParser<SettingRes> _parser = new pb::MessageParser<SettingRes>(() => new SettingRes());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<SettingRes> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::MagiCloud.NetWorks.SystemSettingInfoReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SettingRes() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SettingRes(SettingRes other) : this() {
      back_ = other.back_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SettingRes Clone() {
      return new SettingRes(this);
    }

    /// <summary>Field number for the "back" field.</summary>
    public const int BackFieldNumber = 1;
    private int back_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Back {
      get { return back_; }
      set {
        back_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as SettingRes);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(SettingRes other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Back != other.Back) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Back != 0) hash ^= Back.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Back != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Back);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Back != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Back);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(SettingRes other) {
      if (other == null) {
        return;
      }
      if (other.Back != 0) {
        Back = other.Back;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Back = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code