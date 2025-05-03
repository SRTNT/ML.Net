// See https://aka.ms/new-console-template for more information
using Microsoft.ML;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Trainers.LightGbm;
using ML.Net;
using ML.Net.Domains;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("--------------------------- Start App ---------------------------");

#region Get Raw Data From SQL
var allData = A_FetchDataFromSQL.FetchAdvertisements();
Console.WriteLine($"--> Get {allData.Count} Raw Data From SQL");
#endregion

#region Upload Data ML

var context = new MLContext();
var data = context.Data.LoadFromEnumerable(allData);

#endregion

#region Handle Missing Data + Rounded Data
data = B_HandleMissingValues.Handle(context, data);
#endregion

#region Normalize
var preprocessingPipeline = D_Normalize.NormalizeData(context, data);
#endregion

#region Create PIPELINE
var trainingPipeline = preprocessingPipeline
    .Append(context.Transforms.CopyColumns("Label", "TotalPrice")) // create label - label is the main result for predict-> must map to TotalPrice
    //.Append(context.Regression.Trainers.FastTree(new FastTreeRegressionTrainer.Options
    //{
    //    NumberOfLeaves = 20,
    //    NumberOfTrees = 100,
    //    LearningRate = 0.1
    //}))
    .Append(context.Regression.Trainers.LightGbm(new LightGbmRegressionTrainer.Options
    {
        NumberOfLeaves = 31,
        LearningRate = 0.3,
        NumberOfThreads = 100,
        MinimumExampleCountPerLeaf = 20
    }))
    .Append(context.Transforms.CopyColumns("Score", "Score"));

var tts = context.Data.TrainTestSplit(data, testFraction: 0.2);
#endregion

#region Train Model + Save Learned Model
var trainedModel = trainingPipeline.Fit(tts.TrainSet);
context.Model.Save(trainedModel, data.Schema, "model.zip");
#endregion

#region Create Predict Engine
// رگرسیون پیوسته
var predEngine = context.Model.CreatePredictionEngine<Advertisement, HousePricePrediction>(trainedModel);
#endregion

#region Predict And Show Result
var result = predEngine.Predict(new Advertisement
{
    Area = 160,
    BuildYear = 139 * 4,
    Rooms = 3,
    Floor = 1,
    Elevator = true,
    Parking = true,
    Storage = true,
    LocationName = "جردن"
});

Console.WriteLine($"Predicted price:{result.Price.ToString("n0")}");


//ارزیابی مدل:
var predictions = trainedModel.Transform(tts.TestSet);
var metrics = context.Regression.Evaluate(predictions);
Console.WriteLine($"R^2: {metrics.RSquared:0.##}");
Console.WriteLine($"Mean Absolute Error(MAE) :{metrics.MeanAbsoluteError.ToString("n0")} Toman");
Console.WriteLine($"Mean Squared Error(MSE) : {metrics.MeanSquaredError.ToString("n0")} Toman ");
Console.WriteLine($"Root Mean Squared Error(RMSE): {metrics.RootMeanSquaredError.ToString("n0")} Toman");
#endregion

Console.WriteLine("--------------------------- Finished App ---------------------------");

Console.ReadKey();