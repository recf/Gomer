using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomer.Cli.Exceptions
{
    public class TooManyPileFilesException : Exception
    {
        public TooManyPileFilesException(IList<string> files)
        {
            Files = files;
        }

        public IList<string> Files { get; private set; }
    }
}
