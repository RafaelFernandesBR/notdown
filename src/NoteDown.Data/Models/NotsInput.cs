using System.ComponentModel.DataAnnotations;

namespace NoteDown.Data.Models
{
    public class NotsInput
    {
        [Key]
        public string Id { get; set; }
        public string Conteudo { get; set; }
    }
}
