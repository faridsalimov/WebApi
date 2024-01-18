using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using WebApiTask1.Dtos;

namespace WebApiTask1.Formatters
{
    public class CsvOutputFormatter:TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var sb = new StringBuilder();
            if (context.Object is IEnumerable<StudentDto> list)
            {
                foreach (var item in list)
                {
                    FormatCsv(sb, item);
                }
            }
            else if (context.Object is StudentDto item)
            {
                FormatCsv(sb, item);
            }
            return response.WriteAsync(sb.ToString());
        }

        private void FormatCsv(StringBuilder sb, StudentDto item)
        {
            sb.AppendLine("Id FullName SeriaNo Age Score");
            sb.AppendLine($"{item.Id}, {item.FullName}, {item.SeriaNo}. {item.Age}, {item.Score}");
        }
    }
}