using System;
namespace JLVidalTranspose.Core
{
    public interface IDefTextConverter
    {
        string Delimiter { get; set; }
        string GetDeclaration();
        string[] GetDefiniedFields();
        string Parse(object model);
    }

    public interface IDefTextConverter<T> : IDefTextConverter
    {
        string Parse(T modelToParse);
    }
}
