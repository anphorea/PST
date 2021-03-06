﻿using pst.interfaces;
using pst.utilities;
using pst.encodables.ndb;
using pst.encodables.ndb.btree;

namespace pst.impl.decoders.ndb
{
    class INBTEntryDecoder : IDecoder<INBTEntry>
    {
        private readonly IDecoder<NID> nidDecoder;

        private readonly IDecoder<BREF> brefDecoder;

        public INBTEntryDecoder(IDecoder<NID> nidDecoder, IDecoder<BREF> brefDecoder)
        {
            this.nidDecoder = nidDecoder;
            this.brefDecoder = brefDecoder;
        }

        public INBTEntry Decode(BinaryData encodedData)
        {
            var parser = BinaryDataParser.OfValue(encodedData);

            return
                INBTEntry.OfValue(
                    parser.TakeAndSkip(4, nidDecoder),
                    parser.TakeAtWithoutChangingStreamPosition(8, 16, brefDecoder));
        }
    }
}
