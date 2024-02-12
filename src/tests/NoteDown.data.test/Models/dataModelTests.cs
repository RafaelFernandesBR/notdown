using Moq;
using Moq.Dapper;
using NoteDown.Data.Models;
using Microsoft.Extensions.Logging;
using System.Data;
using Dapper;
using Xunit;

namespace NoteDown.data.test.Model
{
    public class DataModelTests
    {
        private readonly Mock<ILogger<DataModel>> _loggerMock;
        private readonly Mock<IDbConnection> _connectionMock;

        public DataModelTests()
        {
            _loggerMock = new Mock<ILogger<DataModel>>();
            _connectionMock = new Mock<IDbConnection>();
        }

        [Fact]
        public void GetOne_ShouldReturnNotsOutput()
        {
            // Arrange
            string idDeTeste = "05cdf56cc6524f24";
            var notsOutput = new NotsOutput { id = idDeTeste, conteudo = "Conteudo de Teste", data_criacao = new DateTime(2023, 12, 6, 12, 30, 0) };

            // Configurar o retorno do QueryFirstOrDefaultAsync
            _connectionMock.SetupDapper(x => x.QueryFirstOrDefault<NotsOutput>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .Returns(notsOutput);

            var dataModel = new DataModel(_loggerMock.Object, _connectionMock.Object);

            // Act
            var resultDoMetodoGetOne = dataModel.GetOne(idDeTeste);

            // Assert
            Assert.NotNull(resultDoMetodoGetOne);
            Assert.Equal(notsOutput.id, resultDoMetodoGetOne.id);
            Assert.Equal(notsOutput.conteudo, resultDoMetodoGetOne.conteudo);
            Assert.Equal(notsOutput.data_criacao, resultDoMetodoGetOne.data_criacao);
        }

        [Fact]
        public void AddNots_ShouldReturnInsertedNote()
        {
            // Arrange
            var notaInput = new NotsInput { Conteudo = "Conteudo de Teste" };

            // Configurar o retorno do Execute para simular uma inserção bem-sucedida
            _connectionMock.SetupDapper(C => C.Execute(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
            .Returns(1);

            var dataModel = new DataModel(_loggerMock.Object, _connectionMock.Object);

            // Act
            var result = dataModel.AddNots(notaInput);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.Equal(notaInput.Conteudo, result.Conteudo);
            Assert.True((result.Id?.Length == 16) ? true : false);
        }

        [Fact]
        public void UpdateNots_ShouldReturnInsertedNote()
        {
            // Arrange
            string idDeTeste = "05cdf56cc6524f24";
            var notaInput = new NotsInput { Id = idDeTeste, Conteudo = "Conteudo de Teste" };

            // Configurar o retorno do Execute para simular uma inserção bem-sucedida
            _connectionMock.SetupDapper(C => C.Execute(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
            .Returns(1);

            var dataModel = new DataModel(_loggerMock.Object, _connectionMock.Object);

            // Act
            var result = dataModel.UpdateNots(notaInput);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(notaInput.Id, result.Id);
            Assert.Equal(notaInput.Conteudo, result.Conteudo);
        }

    }
}
