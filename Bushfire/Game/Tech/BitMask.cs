using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Game.Storage
{
    static class BitMask
    {
        private static Dictionary<int, int> bitMaskingList;
      
        public static void Init()
        {
            bitMaskingList = new Dictionary<int, int>();

            bitMaskingList.Add(248, 0);
            bitMaskingList.Add(107, 1);
            bitMaskingList.Add(31, 2);
            bitMaskingList.Add(214, 3);
            bitMaskingList.Add(254, 4);
            bitMaskingList.Add(251, 5);
            bitMaskingList.Add(127, 6);
            bitMaskingList.Add(223, 7);
            bitMaskingList.Add(22, 8);
            bitMaskingList.Add(208, 9);
            bitMaskingList.Add(104, 10);
            bitMaskingList.Add(11, 11);
            bitMaskingList.Add(32, 12);
            bitMaskingList.Add(128, 13);
            bitMaskingList.Add(2, 14);
            bitMaskingList.Add(8, 15);
            bitMaskingList.Add(64, 16);
            bitMaskingList.Add(1, 17);
            bitMaskingList.Add(4, 18);
            bitMaskingList.Add(16, 19);
            bitMaskingList.Add(255, 20);
        }
    
        public static int GetTileIndexFromBitmask(int bitmaskValue)
        {
            if (bitMaskingList.ContainsKey(bitmaskValue))
            {
                return bitMaskingList[bitmaskValue];
            }
            return -1;
        }

        public static int GetBitmaskFromTileIndex(int tileIndex)
        {
            foreach (KeyValuePair<int, int> pair in bitMaskingList)
            {
                if (pair.Value == tileIndex)
                {
                    return pair.Key;
                }
            }
            return -1;
        }
    }
}
