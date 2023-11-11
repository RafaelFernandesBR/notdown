using NoteDown.Data.Models;

namespace NoteDown.Data.IModels
{
    public interface IDataModel
    {
        NotsOutput GetOne(string id);
        NotsInput AddNots(NotsInput nota);
    }
}
