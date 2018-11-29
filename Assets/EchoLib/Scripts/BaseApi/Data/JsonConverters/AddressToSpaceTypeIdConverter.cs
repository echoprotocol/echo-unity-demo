﻿using System;
using CustomTools.Extensions.Core.Array;
using Tools.HexBinDec;


namespace Base.Data.Json
{
    public sealed class AddressToSpaceTypeIdConverter : JsonCustomConverter<SpaceTypeId, string>
    {
        protected override SpaceTypeId Deserialize(string value, Type objectType) => ConvertFrom(value);

        protected override string Serialize(SpaceTypeId value) => ConvertTo(value);

        private static string ConvertTo(SpaceTypeId value)
        {
            var data = new byte[16].Fill((byte)0);
            if (value.SpaceType.Equals(SpaceType.Account))
            {
                data[0] = 0x00;
            }
            if (value.SpaceType.Equals(SpaceType.Contract))
            {
                data[0] = 0x01;
            }
            return data.Concat(BitConverter.GetBytes(value.Id)).ToHexString();
        }

        private static SpaceTypeId ConvertFrom(string value)
        {
            var data = value.FromHex2Data();
            if (data.Length != 20)
            {
                return SpaceTypeId.EMPTY;
            }
            var type = SpaceType.Unknown;
            if (data.First().Equals(0x00))
            {
                type = SpaceType.Account;
            }
            if (data.First().Equals(0x01))
            {
                type = SpaceType.Contract;
            }
            if (type.Equals(SpaceType.Unknown))
            {
                return SpaceTypeId.EMPTY;
            }
            return SpaceTypeId.CreateOne(type, BitConverter.ToUInt32(data, 2));
        }
    }
}