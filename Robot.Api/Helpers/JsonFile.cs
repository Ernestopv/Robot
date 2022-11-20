namespace Robot.Api.Helpers;

public static class JsonFile
{
    public static void AddOrUpdateAppSetting<T>(string key, T value)
    {
        try
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            string json = File.ReadAllText(filePath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json)!;

            var sectionPath = key.Split(":")[0];

            if (!string.IsNullOrEmpty(sectionPath))
            {

                jsonObj[sectionPath] = value;
            }

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(filePath, output);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Error writing app settings");
        }
    }

}

