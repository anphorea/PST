﻿using pst.interfaces.ltp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pst.utilities;
using pst.interfaces;
using pst.encodables.ltp.hn;

namespace pst.impl.ltp
{
    class HeapItemsExtractor : IHeapItemsExtractor
    {
        private readonly IDecoder<HNHDR> headerDecoder;

        private readonly IDecoder<HNPAGEMAP> pageMapDecoder;

        private readonly IDecoder<int> int32Decoder;

        public HeapItemsExtractor(IDecoder<HNHDR> headerDecoder, IDecoder<HNPAGEMAP> pageMapDecoder, IDecoder<int> int32Decoder)
        {
            this.headerDecoder = headerDecoder;
            this.pageMapDecoder = pageMapDecoder;
            this.int32Decoder = int32Decoder;
        }

        public BinaryData[] Extract(BinaryData encodedHeapOnNode, int blockIndex)
        {
            if (blockIndex < 0)
                throw new Exception("Invalid block index");

            if (blockIndex > 0)
                throw new Exception("Only single block is current supported");

            using (var parser = BinaryDataParser.OfValue(encodedHeapOnNode))
            {
                var header =
                    parser.TakeAndSkip(12, headerDecoder);

                var pageMap =
                    parser.TakeAtWithoutChangingStreamPosition(
                        header.PageMapOffset,
                        encodedHeapOnNode.Length - header.PageMapOffset,
                        pageMapDecoder);

                var data = new List<BinaryData>();

                var offsets = GetOffsetsFromPageMap(pageMap);

                for(int i = 0; i < offsets.Length - 1; i++)
                {
                    data.Add(parser.TakeAt(offsets[i], offsets[i + 1] - offsets[i]));
                }

                return data.ToArray();
            }
        }

        private int[] GetOffsetsFromPageMap(HNPAGEMAP pageMap)
        {
            var offsets = new List<int>();

            using (var parser = BinaryDataParser.OfValue(pageMap.AllocationTable))
            {
                for(int i = 0; i <= pageMap.AllocationCount; i++)
                {
                    offsets.Add(parser.TakeAndSkip(i * 2, int32Decoder));
                }
            }

            return offsets.ToArray();
        }
    }
}
