using AutoMapper;

namespace Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class NullStringConverter : ITypeConverter<string, string>
    {
        public string Convert(string source, string destination, ResolutionContext context)
        {
            return source ?? string.Empty;
        }
    }
}
