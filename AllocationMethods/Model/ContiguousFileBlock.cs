using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllocationMethods.Model
{
    public class ContiguousFileBlock : FileBlock
    {
        #region Constructors

        public ContiguousFileBlock(int blockAddress)
        {
            BlockAddress = blockAddress;
            IsInUse = false;
            IsOccupied = false;
            Type = AllocationType.Contiguous;
        }

        #endregion
    }
}
