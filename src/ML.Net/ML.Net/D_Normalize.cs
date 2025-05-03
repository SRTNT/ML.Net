using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Net;

internal class D_Normalize
{
    internal static Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Data.ColumnConcatenatingTransformer> NormalizeData(MLContext context, IDataView data)
    {
        // create pipe line
        var pipeline = context.Transforms.Categorical
                           .OneHotHashEncoding("LocationNameEncoded", "LocationName")
                           .Append(context.Transforms.NormalizeMinMax("AreaNormalized", "Area")) // if the names are same, it will replace the column
                           .Append(context.Transforms.NormalizeMinMax("BuildYearNormalized", "BuildYear"))
                           .Append(context.Transforms.NormalizeMinMax("RoomsNormalized", "Rooms"))
                           .Append(context.Transforms.NormalizeMinMax("FloorNormalized", "Floor"))

                           .Append(context.Transforms.NormalizeMinMax("YearNormalized", "Year")) // 
                           .Append(context.Transforms.NormalizeMinMax("MonthNormalized", "Month")) // 
                           .Append(context.Transforms.NormalizeMinMax("DayNormalized", "Day")) // 
                           .Append(context.Transforms.NormalizeMinMax("HourNormalized", "Hour")) // 
                           .Append(context.Transforms.NormalizeMinMax("IsWeekendNormalized", "IsWeekend")) // 

                           // Convert Type Of int/float/bool To Single
                           .Append(context.Transforms.Conversion.ConvertType("ElevatorConverted", "Elevator", outputKind: Microsoft.ML.Data.DataKind.Single))
                           .Append(context.Transforms.Conversion.ConvertType("ParkingConverted", "Parking", outputKind: Microsoft.ML.Data.DataKind.Single))
                           .Append(context.Transforms.Conversion.ConvertType("StorageConverted", "Storage", outputKind: Microsoft.ML.Data.DataKind.Single))

                            // Select Data
                            .Append(context.Transforms.Concatenate("Features", "AreaNormalized",
                                                                               "BuildYearNormalized",
                                                                               "RoomsNormalized",
                                                                               "FloorNormalized",
                                                                               "ElevatorConverted",
                                                                               "ParkingConverted",
                                                                               "StorageConverted",
                                                                               "LocationNameEncoded",
                                                                               "YearNormalized", "MonthNormalized", "DayNormalized", "HourNormalized", "IsWeekendNormalized"));
        return pipeline;
    }
}
