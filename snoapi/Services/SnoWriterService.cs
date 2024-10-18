using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace SNO.API;


public class SnoWriterService<T> where T : class
{
    public SnoWriterService()
    {

    }

    public virtual async Task<bool> WriteData(DbSet<T> dataSet, HttpRequest request)
    {
        int streamLength = (int)request.ContentLength;

        byte[] bytes = new byte[streamLength];
        await request.Body.ReadAsync(bytes);

        string json = Encoding.UTF8.GetString(bytes);

        T e = JsonConvert.DeserializeObject<T>(json);

        await dataSet.AddAsync(e);

        return true;
    }
}
