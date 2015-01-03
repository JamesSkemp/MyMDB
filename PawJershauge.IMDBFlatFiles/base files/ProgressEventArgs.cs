using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PawJershauge.IMDBFlatFiles
{
    public class ProgressEventArgs : EventArgs
    {
        public long StreamPosistion { get; private set; }
        public long StreamLength { get; private set; }

        public ProgressEventArgs(long streamPosistion, long streamLength)
        {
            StreamPosistion = streamPosistion;
            StreamLength = streamLength;
        }
    }
}
