using Microsoft.ML;
using ML.Net.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Net;

internal class B_HandleMissingValues
{
    public static IDataView Handle(MLContext mLContext, IDataView data)
    {
        var PipeLine = mLContext.Transforms.ReplaceMissingValues(nameof(Advertisement.Area),
                                                                 replacementMode: Microsoft.ML.Transforms.MissingValueReplacingEstimator.ReplacementMode.Mean)
             .Append(mLContext.Transforms.ReplaceMissingValues(nameof(Advertisement.BuildYear),
                                                               replacementMode: Microsoft.ML.Transforms.MissingValueReplacingEstimator.ReplacementMode.Mode))
             .Append(mLContext.Transforms.ReplaceMissingValues(nameof(Advertisement.Rooms),
                                                               replacementMode: Microsoft.ML.Transforms.MissingValueReplacingEstimator.ReplacementMode.Mean))
             .Append(mLContext.Transforms.ReplaceMissingValues(nameof(Advertisement.Floor),
                                                               replacementMode: Microsoft.ML.Transforms.MissingValueReplacingEstimator.ReplacementMode.Mean));

        var transformedData = PipeLine.Fit(data).Transform(data);

        #region Rounded Data
        var roundingPipeline = mLContext.Transforms.CustomMapping(new Action<Advertisement, 
                                                                  AdvertisementRounded>(AdvertisementMapping.MapRounded),
                                                                  contractName: null);

        var roundedData = roundingPipeline.Fit(transformedData)
                                          .Transform(transformedData);

        var combinedPipeline = mLContext.Transforms.CopyColumns("Area", "AreaRounded")
            .Append(mLContext.Transforms.CopyColumns("Rooms", "RoomsRounded"))
            .Append(mLContext.Transforms.CopyColumns("Floor", "FloorRounded"));

        var finalData = combinedPipeline.Fit(roundedData).Transform(roundedData);
        #endregion

        return finalData;
    }
}
