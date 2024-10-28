
$(document).ready(function () {
    $('#formCadastro').submit(function (e) {
        e.preventDefault();
        $.ajax({
            url: urlPost,
            method: "POST",
            data: {
                "NOME": $(this).find("#Nome").val(),
                "CEP": removeCustomMask($(this).find("#CEP").val(), "00000-000"),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": removeCustomMask($(this).find("#Telefone").val(), "(00) 00000-0000"),
                "Cpf": removeCustomMask($(this).find("#Cpf").val(), "000.000.000-00"),
                "Beneficiarios": beneficiarios
            },
            error:
                function (r) {
                    if (r.status == 400)
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500)
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                },
            success:
                function (r) {
                    ModalDialog("Sucesso!", r)
                    $("#formCadastro")[0].reset();
                }
        });
    })
})

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div>';

    $('body').append(texto);
    $('#' + random).modal('show');

    // Adicionar a funcao de remover o modal do DOM
    $('#' + random).on('hidden.bs.modal', function () {
        $(this).remove();
    });
}




// Rennan que me ajudou em como fazer isso.
let idCliente;
let beneficiarios = [];
let indexBeneficiarioEditando;
function atualizarBeneficiarios() {
    $('#tabelaBeneficarios').empty();
    beneficiarios.forEach((b, index) => {
        $('#tabelaBeneficarios').append(`
                <tr>
                    <td>${b.CPF}</td>
                    <td>${b.Nome}</td>
                    <td>
                        <button type="button" class="btn btn-sm btn-primary" onclick="alterarBeneficiario(${index})">Alterar</button>
                        <button type="button" class="btn btn-sm btn-primary" onclick="removerBeneficiario(${index})">Remover</button>
                    </td>
                </tr>
            `);
    });
}

function atualizarBeneficiarioAlterado() {
    beneficiarios[indexBeneficiarioEditando].CPF = $('#CPFBeneficiario').val();
    beneficiarios[indexBeneficiarioEditando].Nome = $('#NomeBeneficiario').val();
}

$(document).ready(function () {
    $('#formBeneficiario').submit(function (e) {
        e.preventDefault();

        let cpf = $('#CPFBeneficiario').val();
        let nome = $('#NomeBeneficiario').val();

        if (beneficiarios.some(b => b.CPF === cpf)) {
            ModalDialog("Aviso", "O CPF informado ja foi adicionado.");
            return;
        }

        beneficiarios.push({ CPF: cpf, Nome: nome, IdCliente: idCliente });
        atualizarBeneficiarios();

        $('#CPFBeneficiario').val('');
        $('#NomeBeneficiario').val('');
    });
})


window.atualizarBeneficiarioAlterado = function () {
    if ($('#CPFBeneficiario').val() == "") {
        alert("Preencha o campo de CPF")
        return;
    }
    if ($('#NomeBeneficiario').val() == "") {
        alert("Preencha o campo de Nome")
        return;
    }

    beneficiarios[indexBeneficiarioEditando].CPF = $('#CPFBeneficiario').val();
    beneficiarios[indexBeneficiarioEditando].Nome = $('#NomeBeneficiario').val();

    $('#alterarBeneficiario').css("display", "none");
    $('#cancelarAlteracao').css("display", "none");
    $('#incluirBeneficiario').css("display", "");
    atualizarBeneficiarios();
}

window.cancelarAlteracao = function () {
    $('#CPFBeneficiario').val("");
    $('#NomeBeneficiario').val("");
    $('#alterarBeneficiario').css("display", "none");
    $('#cancelarAlteracao').css("display", "none");
    $('#incluirBeneficiario').css("display", "");
}

window.removerBeneficiario = function (index) {
    beneficiarios.splice(index, 1);
    atualizarBeneficiarios();
}

window.alterarBeneficiario = function (index) {
    indexBeneficiarioEditando = index;

    $('#CPFBeneficiario').val(beneficiarios[index].CPF);
    $('#NomeBeneficiario').val(beneficiarios[index].Nome);

    $('#alterarBeneficiario').css("display", "");
    $('#cancelarAlteracao').css("display", "");
    $('#incluirBeneficiario').css("display", "none");
}
