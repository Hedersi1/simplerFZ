$(document).ready(function () {
    function limpa_formulário_cep() {
        // Limpa valores do formulário de cep.
        $("#Logradouro").val("");
        $("#Bairro").val("");
        $("#UF").val("");
        $("#IdCidade").val("");
        $("#Numero").val("");
        $("#Complemento").val("");
        //$("#IBGE").val("");
    }

    //Quando o campo cep perde o foco.
    $("#CEP").blur(function () {

        //Nova variável "cep" somente com dígitos.
        var cep = $(this).val().replace(/\D/g, '');

        //Verifica se campo cep possui valor informado.
        if (cep != "") {

            //Expressão regular para validar o CEP.
            var validacep = /^[0-9]{8}$/;

            //Valida o formato do CEP.
            if (validacep.test(cep)) {

                //Preenche os campos com "..." enquanto consulta webservice.
                $("#Logradouro").val("...");
                $("#Bairro").val("...");
                $("#UF").val("...");
                $("#IdCidade").val("...");
                $("#Numero").val("...");
                $("#Complemento").val("...");
                //$("#IBGE").val("...");

                //Consulta o webservice viacep.com.br/
                $.getJSON("//viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {
                    if (!("erro" in dados)) {
                        //Atualiza os campos com os valores da consulta.
                        $("#Logradouro").val(dados.logradouro);
                        $("#Bairro").val(dados.bairro);
                        $("#UF").val(dados.uf).change();
                        $("#IdCidade").val(dados.localidade);
                        $("#Numero").val(dados.numero);
                        $("#Complemento").val(dados.complemento);
                        //$("#IBGE").val(dados.ibge);
                    } //end if.
                    else {
                        //CEP pesquisado não foi encontrado.
                        limpa_formulário_cep();
                        alert("CEP não encontrado.");
                    }
                });
            } //end if.
            else {
                //cep é inválido.
                limpa_formulário_cep();
                alert("Formato de CEP inválido.");
            }
        } //end if.
        else {
            //cep sem valor, limpa formulário.
            limpa_formulário_cep();
        }
    });
});