document.getElementById('editButton').addEventListener('click', function () {
    var id = this.getAttribute('data-nota-id');
    document.getElementById('viewMode').style.display = 'none';
    document.getElementById('editMode').style.display = 'block';
});

document.getElementById('cancelButton').addEventListener('click', function () {
    document.getElementById('viewMode').style.display = 'block';
    document.getElementById('editMode').style.display = 'none';
});

document.getElementById('saveButton').addEventListener('click', function () {
    var id = document.getElementById('editButton').getAttribute('data-nota-id');
    var nota = document.getElementById('editTextArea').value;

    // Envia a atualização via AJAX usando PATCH
    fetch('/update/' + id, {
        method: 'PATCH',  // Alteração aqui
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(nota)
    })
        .then(response => {
            if (response.ok) {
                // Recarrega a página após a atualização bem-sucedida
                window.location.reload();
            } else {
                throw new Error('Erro ao atualizar a nota.');
            }
        })
        .catch(error => {
            console.error('Erro ao atualizar a nota:', error);
            alert('Ocorreu um erro ao salvar a nota.');
        });
});
