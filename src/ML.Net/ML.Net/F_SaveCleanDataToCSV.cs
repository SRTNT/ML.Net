using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Net;

internal class F_SaveCleanDataToCSV
{
    public static void Save(MLContext mLContext, IDataView cleanedData, string outputCsvFile)
    {
        using (var fileStream = File.Create(outputCsvFile))
        {
            mLContext.Data.SaveAsText(cleanedData, fileStream, separatorChar: ',', headerRow: true, schema: true);
        }
    }
}
