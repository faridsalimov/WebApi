using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using WebApiTask1.Dtos;
using WebApiTask1.Entities;

namespace WebApiTask1.Formatters
{
    //public class CsvInputFormatter:TextInputFormatter
    //{
    //    public CsvInputFormatter()
    //    {
    //        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
    //        SupportedEncodings.Add(Encoding.UTF8);
    //        SupportedEncodings.Add(Encoding.Unicode);
    //    }

    //    protected override bool CanReadType(Type type)
    // => type == typeof(CsvInputFormatter);

    //    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
    //    {
    //        var httpContext = context.HttpContext;
    //        var serviceProvider = httpContext.RequestServices;

    //        var logger = serviceProvider.GetRequiredService<ILogger<CsvInputFormatter>>();

    //        using var reader = new StreamReader(httpContext.Request.Body, encoding);
    //        string? nameLine = null;

    //        try
    //        {
    //            await ReadLineAsync("Id FullName SeriaNo Age Score", reader, context, logger);
    //            //await ReadLineAsync("VERSION:", reader, context, logger);

    //            nameLine = await ReadLineAsync("N:", reader, context, logger);

    //            var split = nameLine.Split("\n".ToCharArray());
    //            var student = new Student
    //            {
    //                FullName = split[1],
    //            };

    //            await ReadLineAsync("FN:", reader, context, logger);
    //            await ReadLineAsync("END:VCARD", reader, context, logger);

    //            logger.LogInformation("nameLine = {nameLine}", nameLine);

    //            return await InputFormatterResult.SuccessAsync(student);
    //        }
    //        catch
    //        {
    //            logger.LogError("Read failed: nameLine = {nameLine}", nameLine);
    //            return await InputFormatterResult.FailureAsync();
    //        }

    //    }

    //    private static async Task<string> ReadLineAsync(string v, StreamReader reader, InputFormatterContext context, ILogger<CsvInputFormatter> logger)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}




    public class CsvInputFormatter : TextInputFormatter
    {
        public CsvInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanReadType(Type type)
        {
            // Sadece belirli bir tipi destekle
            return type == typeof(StudentAddDto);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(
            InputFormatterContext context, Encoding effectiveEncoding)
        {
            var httpContext = context.HttpContext;
            var serviceProvider = httpContext.RequestServices;

            var logger = serviceProvider.GetRequiredService<ILogger<CsvInputFormatter>>();

            using var reader = new StreamReader(httpContext.Request.Body, effectiveEncoding);
            string? headerLine = await reader.ReadLineAsync(); // İlk satır genellikle başlık (header) olacaktır
            var headers = headerLine?.Split(',');

            if (headers == null || headers.Length < 4)
            {
                // Başlık hatalı veya eksikse hata fırlat
                context.ModelState.TryAddModelError(context.ModelName, "Invalid or missing header in CSV");
                return await InputFormatterResult.FailureAsync();
            }

            try
            {
                var dataLine = await reader.ReadLineAsync();
                var values = dataLine?.Split(',');

                if (values == null || values.Length < 4)
                {
                    // Veri hatalı veya eksikse hata fırlat
                    context.ModelState.TryAddModelError(context.ModelName, "Invalid or missing data in CSV");
                    return await InputFormatterResult.FailureAsync();
                }

                var obj = new StudentAddDto
                {
                    FullName = values[0],
                    SeriaNo = values[1],
                    Age = int.Parse(values[2]),
                    Score = double.Parse(values[3]),
                };

                return await InputFormatterResult.SuccessAsync(obj);
            }
            catch (Exception ex)
            {
                // Hata oluştuğunda logla ve başka işlemler gerçekleştir
                logger.LogError(ex, "Error reading CSV data");
                context.ModelState.TryAddModelError(context.ModelName, "Error reading CSV data");
                return await InputFormatterResult.FailureAsync();
            }
        }
    }

}
