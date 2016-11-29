﻿using pst.encodables.ltp.bth;
using pst.interfaces;

namespace pst.impl.ltp.pc
{
    class PropertyIdFromDataRecordExtractor : IExtractor<DataRecord, PropertyId>
    {
        private readonly IDecoder<int> int32Decoder;

        public PropertyIdFromDataRecordExtractor(IDecoder<int> int32Decoder)
        {
            this.int32Decoder = int32Decoder;
        }

        public PropertyId Extract(DataRecord parameter)
        {
            return new PropertyId(int32Decoder.Decode(parameter.Key));
        }
    }
}
