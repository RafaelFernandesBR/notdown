$(document).ready(function () {
    $('#editar-tab').on('click', function () {
        $('#visualizar-tab').removeClass('active');
        $('#editar-tab').addClass('active');
        $('#visualizar').removeClass('show active');
        $('#editar').addClass('show active');
    });

    $('#visualizar-tab').on('click', function () {
        $('#editar-tab').removeClass('active');
        $('#visualizar-tab').addClass('active');
        $('#editar').removeClass('show active');
        $('#visualizar').addClass('show active');

        // Aqui você pode adicionar lógica para renderizar o markdown em #visualizacao-markdown
        var markdownText = $('#nota').val(); // Obtém o texto do textarea
        var markdownHtml = marked(markdownText); // Usa o marked.js para renderizar o markdown
        $('#visualizacao-markdown').html(markdownHtml); // Insere o HTML renderizado na div de visualização
    });
});