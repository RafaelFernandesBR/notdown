using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Dapper;
using System.Data;
using NoteDown.Data.IModels;

namespace NoteDown.Data.Models
{
    public class DataModel : IDataModel
    {
        private MySqlConnection conm { get; set; }
        string TableNots;

        public DataModel()
        {
            //get a file json
            StreamReader r = new StreamReader("conect-sql.json");
            string readFile = r.ReadToEnd();
            conectSql conectData = JsonConvert.DeserializeObject<conectSql>(readFile);

            this.conm = new MySqlConnection("Server=" + conectData.server + ";Database=" + conectData.Database + ";Uid=" + conectData.user + ";Pwd=" + conectData.senha + ";");

            TableNots = "nots";
        }

        public NotsOutput GetOne(string id)
        {
            string query = $"SELECT * FROM {TableNots} WHERE id=@id";

            var parametros = new DynamicParameters();
            parametros.Add("@id", id);

            using (IDbConnection connection = conm)
            {
                try
                {
                    var nota = connection.QueryFirstOrDefault<NotsOutput>(query, parametros);
                    return nota;
                }
                catch (Exception ex)
                {
                    throw new Exception("Ocorreu um erro ao obter os dados da nota.", ex);
                }
            }
        }

        public NotsInput AddNots(NotsInput nota)
        {
            int maxAttempts = 3; // Número máximo de tentativas para gerar um ID único
            int currentAttempt = 1;

            using (IDbConnection connection = conm)
            {
                while (currentAttempt <= maxAttempts)
                {
                    try
                    {
                        nota.Id = GenerateUniqueID();
                        string query = $"INSERT INTO {TableNots} (id, conteudo) VALUES (@Id, @Conteudo)";
                        connection.Execute(query, nota);

                        // Se a inserção for bem-sucedida, retornamos a nota
                        return nota;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException ex) when (ex.Number == 1062)
                    {
                        // Código 1062 indica violação de chave única (colisão de IDs)
                        // Incrementamos a tentativa e tentamos gerar um novo ID
                        currentAttempt++;
                    }
                    catch (Exception ex)
                    {
                        // Outros erros são relançados
                        throw new Exception("Ocorreu um erro ao adicionar a nota ao banco de dados.", ex);
                    }
                }

                // Se atingir o número máximo de tentativas sem sucesso, lançamos uma exceção
                throw new Exception("Não foi possível gerar um ID único após várias tentativas.");
            }
        }

        private string GenerateUniqueID()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 16);
        }

    }
}
