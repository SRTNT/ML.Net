// See https://aka.ms/new-console-template for more information
using Microsoft.ML;
using Microsoft.ML.Trainers.FastTree;
using ML.Net;
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

#region Show Upload Data
foreach (var row in data.Preview().RowView)
{
    foreach (var item in row.Values)
    {
        Console.Write($"{item.Key}:{item.Value} ");
    }
    Console.WriteLine();
}
#endregion

#endregion

#region Handle Missing Data + Rounded Data
data = B_HandleMissingValues.Handle(context, data);
#endregion

#region Normalize
data = D_Normalize.NormalizeData(context, data);
#endregion

#region Encode Categorical Values
{
    var pipeline = context.Transforms.Categorical.OneHotHashEncoding("LocationName", "LocationName");
    data= pipeline.Fit(data).Transform(data);
}
#endregion

#region Save Clean Data
F_SaveCleanDataToCSV.Save(context, data, "CleanData.csv");
#endregion

Console.WriteLine("--------------------------- Finished App ---------------------------");

Console.ReadKey();