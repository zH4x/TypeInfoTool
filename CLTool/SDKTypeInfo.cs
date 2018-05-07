﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWBF2Tool
{
    /// <summary>
    /// Class to load and contain a generic TypeInfo entry from SWBF2
    /// </summary>
    public class SDKTypeInfo
    {
        public string Name { get; protected set; } = "";
        public IntPtr ThisTypeInfo { get; protected set; } = IntPtr.Zero;
        public BasicTypesEnum Type { get; protected set; } = BasicTypesEnum.kTypeCode_Void;
        public int Alignment { get; protected set; } = 0;
        public int TotalSize { get; protected set; } = 0;
        public int FieldCount { get; protected set; } = 0;
        public int RuntimeId { get; protected set; } = 0;
        public IntPtr Next { get; protected set; } = IntPtr.Zero;
        public int Flags { get; protected set; } = 0;

        public SDKTypeInfo()
        {

        }

        public SDKTypeInfo(IntPtr address, RemoteProcess remoteProcess)
        {
            TypeInfo typeInfo = remoteProcess.Read<TypeInfo>(address);
            TypeInfoData typeInfoData = remoteProcess.Read<TypeInfoData>(typeInfo.m_InfoData);

            Name = remoteProcess.ReadString(typeInfoData.m_Name, 255);
            ThisTypeInfo = address;
            Type = typeInfoData.GetEntryType();
            Flags = typeInfoData.m_Flags;
            Alignment = typeInfoData.m_Alignment;
            TotalSize = typeInfoData.m_TotalSize;
            FieldCount = typeInfoData.m_FieldCount;
            RuntimeId = typeInfo.m_RuntimeId;
            Next = typeInfo.m_Next;
        }

        public string FixTypeName(string name)
        {
            switch (name)
            {
                case "Int8":
                    name = "int8_t";
                    break;
                case "Uint8":
                    name = "uint8_t";
                    break;
                case "Int16":
                    name = "int16_t";
                    break;
                case "Uint16":
                    name = "uint16_t";
                    break;
                case "Int32":
                    name = "int32_t";
                    break;
                case "Uint32":
                    name = "uint32_t";
                    break;
                case "Int64":
                    name = "int64_t";
                    break;
                case "Uint64":
                    name = "uint64_t";
                    break;
                case "Float32":
                    name = "float";
                    break;
                case "Float64":
                    name = "double";
                    break;
                case "Boolean":
                    name = "int16_t";
                    break;
                case "CString":
                    {
                        //if (Type == BasicTypesEnum.kTypeCode_Class)
                        //{
                        //    name = "char*";
                        //}
                        //else
                        //{
                        //    name = "char";
                        //}
                        name = "char*";
                    }
                    break;
                default:
                    {
                        if ((Type == BasicTypesEnum.kTypeCode_Class) || (Type == BasicTypesEnum.kTypeCode_Array))
                        {
                            name = $"fb::{name}*";
                        }
                        if ((Type == BasicTypesEnum.kTypeCode_ValueType) || (Type == BasicTypesEnum.kTypeCode_Enum))
                        {
                            name = $"fb::{name}";
                        }
                    }
                    break;
            }

            return name;
        }

        public string GetCppType()
        {
            var cppType = "";

            switch (Type)
            {
                case BasicTypesEnum.kTypeCode_Void:
                    cppType = "void";
                    break;
                case BasicTypesEnum.kTypeCode_DbObject:
                    cppType = "DbObject";
                    break;
                case BasicTypesEnum.kTypeCode_ValueType:
                    //cppType = "struct";
                    //cppType = Name;
                    cppType = $"fb::{FixTypeName(Name)}";
                    break;
                case BasicTypesEnum.kTypeCode_Class:
                    {
                        //cppType = "class";
                        cppType = $"{FixTypeName(Name)}";
                    }
                    break;
                case BasicTypesEnum.kTypeCode_Array:
                    {
                        //cppType = "Array";
                        Name = Name.Substring(0, Name.Length - 6);
                        //FixTypeName(Name);
                        cppType = $"/* fb::Array<> */{Environment.NewLine}    {FixTypeName(Name)}*";
                        //cppType = $"Array<{FixTypeName(Name)}>";
                    }
                    break;
                case BasicTypesEnum.kTypeCode_FixedArray:
                    cppType = "FixedArray";
                    break;
                case BasicTypesEnum.kTypeCode_String:
                    cppType = "string";
                    break;
                case BasicTypesEnum.kTypeCode_CString:
                    cppType = "char*";
                    break;
                case BasicTypesEnum.kTypeCode_Enum:
                    //cppType = "enum";
                    //cppType = Name;
                    cppType = $"{FixTypeName(Name)}";
                    break;
                case BasicTypesEnum.kTypeCode_FileRef:
                    cppType = "FileRef";
                    break;
                case BasicTypesEnum.kTypeCode_Boolean:
                    cppType = "bool";
                    break;
                case BasicTypesEnum.kTypeCode_Int8:
                    cppType = "int8_t";
                    break;
                case BasicTypesEnum.kTypeCode_Uint8:
                    cppType = "uint8_t";
                    break;
                case BasicTypesEnum.kTypeCode_Int16:
                    cppType = "int16_t";
                    break;
                case BasicTypesEnum.kTypeCode_Uint16:
                    cppType = "uint16_t";
                    break;
                case BasicTypesEnum.kTypeCode_Int32:
                    cppType = "int32_t";
                    break;
                case BasicTypesEnum.kTypeCode_Uint32:
                    cppType = "uint32_t";
                    break;
                case BasicTypesEnum.kTypeCode_Int64:
                    cppType = "int64_t";
                    break;
                case BasicTypesEnum.kTypeCode_Uint64:
                    cppType = "int64_t";
                    break;
                case BasicTypesEnum.kTypeCode_Float32:
                    cppType = "float";
                    break;
                case BasicTypesEnum.kTypeCode_Float64:
                    cppType = "double";
                    break;
                case BasicTypesEnum.kTypeCode_Guid:
                    cppType = "Guid";
                    break;
                case BasicTypesEnum.kTypeCode_SHA1:
                    cppType = "SHA1";
                    break;
                case BasicTypesEnum.kTypeCode_ResourceRef:
                    cppType = "ResourceRef";
                    break;
                case BasicTypesEnum.kTypeCode_BasicTypeCount:
                    cppType = "BasicTypeCount";
                    break;
                default:
                    cppType = "unknown_type";
                    break;
            }

            return cppType;
        }
    }
}