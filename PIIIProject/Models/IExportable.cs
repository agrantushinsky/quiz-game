using System;
using System.Collections.Generic;
using System.Text;

namespace PIIIProject.Models
{
    // IExportable will be actually reading & writing to files compared to what ISavable is.
    public interface IExportable
    {
        void Export(string fileName);
        void Import(string fileName);
    }
}
