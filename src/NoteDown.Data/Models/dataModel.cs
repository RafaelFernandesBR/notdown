using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using NoteDown.Data.IModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace NoteDown.Data.Models
{
    public class DataModel : IDataModel
    {
        private readonly IDbConnection _connection; private readonly ILogger<DataModel> _logger;
        string TableNots;

        public DataModel(ILogger<DataModel> logger, IDbConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            TableNots = "nots";
        }

        public NotsOutput GetOne(string id)
        {
            string query = $"SELECT * FROM {TableNots} WHERE id=@id";

            var parametros = new DynamicParameters();
            parametros.Add("@id", id);

            using (IDbConnection connection = _connection)
            {
                try
                {
                    var nota = connection.QueryFirstOrDefault<NotsOutput>(query, parametros);
                    return nota;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw new Exception("Ocorreu um erro ao obter os dados da nota.", ex);
                }
            }
        }

        public NotsInput AddNots(NotsInput nota)
        {
            int maxAttempts = 3; // N�mero m�ximo de tentativas para gerar um ID �nico
            int currentAttempt = 1;
            string query = $"INSERT INTO {TableNots} (id, conteudo) VALUES (@Id, @Conteudo)";

            using (IDbConnection connection = _connection)
            {
                while (currentAttempt <= maxAttempts)
                {
                    try
                    {
                        nota.Id = GenerateUniqueID();

                        var parametros = new DynamicParameters();
                        parametros.Add("@Id", nota.Id);
                        parametros.Add("@Conteudo", nota.Conteudo);

                        connection.Execute(query, parametros);

                        // Se a inser��o for bem-sucedida, retornamos a nota
                        return nota;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException ex) when (ex.Number == 1062)
                    {
                        _logger.LogError(ex.Message);
                        // C�digo 1062 indica viola��o de chave �nica (colis�o de IDs)
                        // Incrementamos a tentativa e tentamos gerar um novo ID
                        currentAttempt++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw new Exception("Ocorreu um erro ao adicionar a nota ao banco de dados.", ex);
                    }
                }

                // Se atingir o n�mero m�ximo de tentativas sem sucesso, lan�amos uma exce��o
                throw new Exception("N�o foi poss�vel gerar um ID �nico ap�s v�rias tentativas.");
            }
        }

        public NotsInput UpdateNots(NotsInput nota)
        {
            string query = $"UPDATE {TableNots} SET conteudo = @Conteudo WHERE id = @Id";

            var parametros = new DynamicParameters();
            parametros.Add("@Id", nota.Id);
            parametros.Add("@Conteudo", nota.Conteudo);

            using (IDbConnection connection = _connection)
            {
                try
                {
                    connection.Execute(query, parametros);
                    return nota;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw new Exception("Ocorreu um erro ao atualizar a nota no banco de dados.", ex);
                }
            }
        }

        private string GenerateUniqueID()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 16);
        }

    }
}
