using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Net;

internal class D_Normalize
{
    internal static IDataView NormalizeData(MLContext mlContext, IDataView data)
    {
        var pipeline = mlContext.Transforms
                           .NormalizeMinMax("Area", "Area")
                           .Append(mlContext.Transforms.NormalizeMinMax("BuildYear", "BuildYear"))
                           .Append(mlContext.Transforms.NormalizeMinMax("Rooms", "Rooms"))
                           .Append(mlContext.Transforms.NormalizeMinMax("Floor", "Floor"));

        return pipeline.Fit(data)
                       .Transform(data);
    }
}
