using System;
using System.Collections.Generic;
using System.Text;

namespace PIIIProject.Models
{
    // ISavable is used for export a class as a string,
    // and setting up a class from a string.
    public interface ISavable
    {
        // The Separator defines how the data will separated.
        char GetSeparator();

        string Save();
        void Load(string data);
    }
}
