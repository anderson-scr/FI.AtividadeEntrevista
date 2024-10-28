
let idCliente;
let beneficiarios = [];
let indexBeneficiarioEditando;
$(document).ready(function () {
    if (obj) {
        $('#formCadastro #Nome').val(obj.Nome);
        $('#formCadastro #CEP').val(applyMask(obj.CEP, "00000-000"));
        $('#formCadastro #Email').val(obj.Email);
        $('#formCadastro #Sobrenome').val(obj.Sobrenome);
        $('#formCadastro #Nacionalidade').val(obj.Nacionalidade);
        $('#formCadastro #Estado').val(obj.Estado);
        $('#formCadastro #Cidade').val(obj.Cidade);
        $('#formCadastro #Logradouro').val(obj.Logradouro);
        $('#formCadastro #Telefone').val(applyMask(obj.Telefone, "(00) 00000-0000"));
        $('#formCadastro #Cpf').val(applyMask(obj.CPF, "000.000.000-00"));
    }
    beneficiarios = obj.Beneficiarios;
    atualizarBeneficiarios();

    $('#formCadastro').submit(function (e) {
        e.preventDefault();

        $.ajax({
            url: urlPost,
            method: "POST",
            data: {
                "NOME": $(this).find("#Nome").val(),
                "CEP": $(this).find("#CEP").val(),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": $(this).find("#Telefone").val(),
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
                    window.location.href = urlRetorno;
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
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}




function atualizarBeneficiarios() {
    $('#tabelaBeneficarios').empty();
    beneficiarios.forEach((b, index) => {
        $('#tabelaBeneficarios').append(`
                <tr>
                    <td>${applyMask(b.CPF, "000.000.000-00")}</td>
                    <td>${b.Nome}</td>
                    <td>
                        <button type="button" class="btn btn-sm btn-primary" onclick="alterarBeneficiario(${index})">Alterar</button>
                        <button type="button" class="btn btn-sm btn-primary" onclick="removerBeneficiario(${index})">Remover</button>
                    </td>
                </tr>
            `);
    });
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



window.removerBeneficiario = function (index) {
    beneficiarios.splice(index, 1);
    atualizarBeneficiarios();
}


window.cancelarAlteracao = function () {
    $('#CPFBeneficiario').val("");
    $('#NomeBeneficiario').val("");
    $('#alterarBeneficiario').css("display", "none");
    $('#cancelarAlteracao').css("display", "none");
    $('#incluirBeneficiario').css("display", "");
}


window.alterarBeneficiario = function (index) {
    indexBeneficiarioEditando = index;

    $('#CPFBeneficiario').val(beneficiarios[index].CPF);
    $('#NomeBeneficiario').val(beneficiarios[index].Nome);

    $('#alterarBeneficiario').css("display", "");
    $('#cancelarAlteracao').css("display", "");
    $('#incluirBeneficiario').css("display", "none");
}