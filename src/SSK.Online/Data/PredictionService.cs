using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using PKDSS.CoreLibrary.Model;
using System.Data;
using PKDSS.CoreLibrary;
using System.Text.RegularExpressions;
using SSK.Online.Helpers;
using Newtonsoft.Json.Linq;
using SSK.Online.Model;

namespace SSK.Online.Data
{
    public class PredictionService
    {
        static FertilizerCalculator calc;
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            StreamReader sr = new StreamReader(strFilePath);
            string[] headers = sr.ReadLine().Split(',');
            DataTable dt = new DataTable();
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public async Task<(Dictionary<string, double> hasil, Dictionary<string, float> input)> InferenceWithApi(string DataInputFile=null)
        {
            //var modelOut = new ModelOutput();
            var ApiRef = StorageInfo.GetAbsolutePath("Files") + "/Service_SSK.csv";
            var dt = ConvertCSVtoDataTable(ApiRef);
            var sampleFile = StorageInfo.GetAbsolutePath("Files")+$"/Measurement_Perc_7.Spectrum";
            var DataInput = !string.IsNullOrEmpty(DataInputFile) ? PrepTool.PreprocessDataInput(DataInputFile) : PrepTool.PreprocessDataInput(File.ReadAllText(sampleFile));
            Dictionary<string, double> hasil = new Dictionary<string, double>();
            foreach (DataRow dr in dt.Rows)
            {
                var nilai = await Predict(dr["input"].ToString(), DataInput, dr["key"].ToString(), dr["url"].ToString());
                hasil.Add(dr["input"].ToString(), nilai < 0 ? 0 : nilai);
            }
            return (hasil,DataInput);
        }
        public RecommendationOutput GetFertilizerRecommendation(string komoditas,ModelOutput unsur)
        {
            var output = new RecommendationOutput();
            if (calc==null)
                calc = new FertilizerCalculator();
            // textbox rekomendasi
            //var calc = new FertilizerCalculator(DataRekomendasi);
            switch (komoditas)
            {
                case "Padi":
                    output.Urea = (calc.GetFertilizerDoze(Convert.ToDouble(unsur.KJELDAHL_N), komoditas, "Urea") * AppConstants.FactorUrea );
                    output.SP36 = (calc.GetFertilizerDoze(Convert.ToDouble(unsur.HCl25_P2O5), komoditas, "SP36") * AppConstants.FactorSP36 );
                    output.KCL  = (calc.GetFertilizerDoze(Convert.ToDouble(unsur.HCl25_K2O), komoditas, "KCL") *    AppConstants.FactorKCL  );
                    break;

                case "Jagung":
                    output.Urea = (calc.GetFertilizerDoze(Convert.ToDouble(unsur.KJELDAHL_N), komoditas, "Urea") * AppConstants.FactorUrea);
                    output.SP36= (calc.GetFertilizerDoze(Convert.ToDouble(unsur.Bray1_P2O5), komoditas, "SP36") * AppConstants.FactorSP36  );
                    output.KCL = (calc.GetFertilizerDoze(Convert.ToDouble(unsur.HCl25_K2O), komoditas, "KCL") *    AppConstants.FactorKCL  );
                    break;

                case "Kedelai":
                    output.Urea = (calc.GetFertilizerDoze(Convert.ToDouble(unsur.KJELDAHL_N), komoditas, "Urea") * AppConstants.FactorUrea);
                    output.SP36= (calc.GetFertilizerDoze(Convert.ToDouble(unsur.Bray1_P2O5), komoditas, "SP36") * AppConstants.FactorSP36 );
                    output.KCL = (calc.GetFertilizerDoze(Convert.ToDouble(unsur.K), komoditas, "KCL") *            AppConstants.FactorKCL  );
                    break;
            }

            // textbox rekomendasi npk 15:15:15
            var x = calc.GetNPKDoze(P2O5: unsur.HCl25_P2O5,
                K2O: unsur.HCl25_K2O, Jenis: komoditas);

            output.NPK15 = x.NPK;
            if (komoditas == "Kedelai")
            {
                output.Urea15 = -1;
            }
            else
            {
                output.Urea15= x.Urea;
            }
            return output;
        }
        async Task<double> Predict(string InputName,Dictionary<string,float> DataInput, string ApiKey, string ApiUrl)
        {
            using (var client = new HttpClient())
            { 
                var FinalInput = new Dictionary<string, string>(){
                                            {
                                                InputName, "1"
                                            },
                                            {
                                                "2501.982414", "1"
                                            },
                                            {
                                                "2488.121682", "1"
                                            },
                                            {
                                                "2474.413679", "1"
                                            },
                                            {
                                                "2460.855893", "1"
                                            },
                                            {
                                                "2447.445869", "1"
                                            },
                                            {
                                                "2434.181205", "1"
                                            },
                                            {
                                                "2421.059549", "1"
                                            },
                                            {
                                                "2408.078601", "1"
                                            },
                                            {
                                                "2395.23611", "1"
                                            },
                                            {
                                                "2382.529873", "1"
                                            },
                                            {
                                                "2369.957732", "1"
                                            },
                                            {
                                                "2357.517576", "1"
                                            },
                                            {
                                                "2345.207338", "1"
                                            },
                                            {
                                                "2333.024993", "1"
                                            },
                                            {
                                                "2320.968557", "1"
                                            },
                                            {
                                                "2309.03609", "1"
                                            },
                                            {
                                                "2297.225689", "1"
                                            },
                                            {
                                                "2285.535489", "1"
                                            },
                                            {
                                                "2273.963667", "1"
                                            },
                                            {
                                                "2262.508432", "1"
                                            },
                                            {
                                                "2251.168031", "1"
                                            },
                                            {
                                                "2239.940747", "1"
                                            },
                                            {
                                                "2228.824895", "1"
                                            },
                                            {
                                                "2217.818825", "1"
                                            },
                                            {
                                                "2206.920917", "1"
                                            },
                                            {
                                                "2196.129586", "1"
                                            },
                                            {
                                                "2185.443276", "1"
                                            },
                                            {
                                                "2174.860461", "1"
                                            },
                                            {
                                                "2164.379645", "1"
                                            },
                                            {
                                                "2153.999359", "1"
                                            },
                                            {
                                                "2143.718166", "1"
                                            },
                                            {
                                                "2133.534652", "1"
                                            },
                                            {
                                                "2123.447432", "1"
                                            },
                                            {
                                                "2113.455147", "1"
                                            },
                                            {
                                                "2103.556462", "1"
                                            },
                                            {
                                                "2093.750069", "1"
                                            },
                                            {
                                                "2084.034683", "1"
                                            },
                                            {
                                                "2074.409043", "1"
                                            },
                                            {
                                                "2064.871912", "1"
                                            },
                                            {
                                                "2055.422073", "1"
                                            },
                                            {
                                                "2046.058334", "1"
                                            },
                                            {
                                                "2036.779523", "1"
                                            },
                                            {
                                                "2027.584491", "1"
                                            },
                                            {
                                                "2018.472107", "1"
                                            },
                                            {
                                                "2009.441263", "1"
                                            },
                                            {
                                                "2000.490869", "1"
                                            },
                                            {
                                                "1991.619854", "1"
                                            },
                                            {
                                                "1982.827168", "1"
                                            },
                                            {
                                                "1974.111777", "1"
                                            },
                                            {
                                                "1965.472667", "1"
                                            },
                                            {
                                                "1956.90884", "1"
                                            },
                                            {
                                                "1948.419317", "1"
                                            },
                                            {
                                                "1940.003134", "1"
                                            },
                                            {
                                                "1931.659347", "1"
                                            },
                                            {
                                                "1923.387023", "1"
                                            },
                                            {
                                                "1915.18525", "1"
                                            },
                                            {
                                                "1907.053129", "1"
                                            },
                                            {
                                                "1898.989776", "1"
                                            },
                                            {
                                                "1890.994322", "1"
                                            },
                                            {
                                                "1883.065913", "1"
                                            },
                                            {
                                                "1875.20371", "1"
                                            },
                                            {
                                                "1867.406887", "1"
                                            },
                                            {
                                                "1859.674631", "1"
                                            },
                                            {
                                                "1852.006145", "1"
                                            },
                                            {
                                                "1844.400641", "1"
                                            },
                                            {
                                                "1836.857348", "1"
                                            },
                                            {
                                                "1829.375506", "1"
                                            },
                                            {
                                                "1821.954366", "1"
                                            },
                                            {
                                                "1814.593192", "1"
                                            },
                                            {
                                                "1807.291262", "1"
                                            },
                                            {
                                                "1800.047862", "1"
                                            },
                                            {
                                                "1792.862291", "1"
                                            },
                                            {
                                                "1785.73386", "1"
                                            },
                                            {
                                                "1778.66189", "1"
                                            },
                                            {
                                                "1771.645713", "1"
                                            },
                                            {
                                                "1764.684671", "1"
                                            },
                                            {
                                                "1757.778116", "1"
                                            },
                                            {
                                                "1750.925412", "1"
                                            },
                                            {
                                                "1744.125931", "1"
                                            },
                                            {
                                                "1737.379056", "1"
                                            },
                                            {
                                                "1730.684178", "1"
                                            },
                                            {
                                                "1724.040698", "1"
                                            },
                                            {
                                                "1717.448027", "1"
                                            },
                                            {
                                                "1710.905585", "1"
                                            },
                                            {
                                                "1704.412798", "1"
                                            },
                                            {
                                                "1697.969105", "1"
                                            },
                                            {
                                                "1691.573951", "1"
                                            },
                                            {
                                                "1685.226788", "1"
                                            },
                                            {
                                                "1678.927079", "1"
                                            },
                                            {
                                                "1672.674295", "1"
                                            },
                                            {
                                                "1666.467911", "1"
                                            },
                                            {
                                                "1660.307414", "1"
                                            },
                                            {
                                                "1654.192297", "1"
                                            },
                                            {
                                                "1648.12206", "1"
                                            },
                                            {
                                                "1642.096211", "1"
                                            },
                                            {
                                                "1636.114265", "1"
                                            },
                                            {
                                                "1630.175743", "1"
                                            },
                                            {
                                                "1624.280176", "1"
                                            },
                                            {
                                                "1618.427097", "1"
                                            },
                                            {
                                                "1612.61605", "1"
                                            },
                                            {
                                                "1606.846583", "1"
                                            },
                                            {
                                                "1601.118252", "1"
                                            },
                                            {
                                                "1595.430619", "1"
                                            },
                                            {
                                                "1589.78325", "1"
                                            },
                                            {
                                                "1584.175721", "1"
                                            },
                                            {
                                                "1578.607611", "1"
                                            },
                                            {
                                                "1573.078506", "1"
                                            },
                                            {
                                                "1567.587997", "1"
                                            },
                                            {
                                                "1562.135681", "1"
                                            },
                                            {
                                                "1556.721163", "1"
                                            },
                                            {
                                                "1551.344049", "1"
                                            },
                                            {
                                                "1546.003954", "1"
                                            },
                                            {
                                                "1540.700496", "1"
                                            },
                                            {
                                                "1535.433301", "1"
                                            },
                                            {
                                                "1530.201996", "1"
                                            },
                                            {
                                                "1525.006217", "1"
                                            },
                                            {
                                                "1519.845603", "1"
                                            },
                                            {
                                                "1514.719799", "1"
                                            },
                                            {
                                                "1509.628452", "1"
                                            },
                                            {
                                                "1504.571218", "1"
                                            },
                                            {
                                                "1499.547754", "1"
                                            },
                                            {
                                                "1494.557722", "1"
                                            },
                                            {
                                                "1489.600791", "1"
                                            },
                                            {
                                                "1484.676633", "1"
                                            },
                                            {
                                                "1479.784922", "1"
                                            },
                                            {
                                                "1474.92534", "1"
                                            },
                                            {
                                                "1470.097571", "1"
                                            },
                                            {
                                                "1465.301304", "1"
                                            },
                                            {
                                                "1460.536231", "1"
                                            },
                                            {
                                                "1455.802049", "1"
                                            },
                                            {
                                                "1451.098459", "1"
                                            },
                                            {
                                                "1446.425165", "1"
                                            },
                                            {
                                                "1441.781875", "1"
                                            },
                                            {
                                                "1437.168301", "1"
                                            },
                                            {
                                                "1432.584159", "1"
                                            },
                                            {
                                                "1428.029169", "1"
                                            },
                                            {
                                                "1423.503052", "1"
                                            },
                                            {
                                                "1419.005535", "1"
                                            },
                                            {
                                                "1414.536349", "1"
                                            },
                                            {
                                                "1410.095225", "1"
                                            },
                                            {
                                                "1405.681902", "1"
                                            },
                                            {
                                                "1401.296118", "1"
                                            },
                                            {
                                                "1396.937616", "1"
                                            },
                                            {
                                                "1392.606143", "1"
                                            },
                                            {
                                                "1388.301449", "1"
                                            },
                                            {
                                                "1384.023285", "1"
                                            },
                                            {
                                                "1379.771406", "1"
                                            },
                                            {
                                                "1375.545573", "1"
                                            },
                                            {
                                                "1371.345545", "1"
                                            },
                                            {
                                                "1367.171087", "1"
                                            },
                                            {
                                                "1363.021967", "1"
                                            },
                                            {
                                                "1358.897955", "1"
                                            },
                                            {
                                                "1354.798823", "1"
                                            },
                                            {
                                                "1350.724346", "1"
                                            }
            };
                int counter = 0;
                var dataInputVal = DataInput.Values.ToList();
                var keys = FinalInput.Keys.ToList();
                foreach (var key in keys)
                {
                    if (counter > 0)
                    {
                       
                            FinalInput[key] = dataInputVal[counter-1].ToString();
                        
                    }
                    counter++;
                }
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>>() {
                        {
                            "input1",
                            new List<Dictionary<string, string>>(){
                            
                            FinalInput

                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
                client.BaseAddress = new Uri(ApiUrl);

                // WARNING: The 'await' statement below can result in a deadlock
                // if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false)
                // so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)

                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    dynamic rootData = JObject.Parse(result);
                    dynamic data1 = rootData.Results.output1[0]["Scored Labels"];
                    var hasil = Convert.ToDouble(data1);
                    Console.WriteLine("Result: {0}", result);
                    return hasil;
                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
                return -1;
            }
        }
    }
}
