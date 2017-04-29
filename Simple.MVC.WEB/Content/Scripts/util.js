$(document).ready(function () {
    $('#RenderBody').hide();
    $('#RenderBody').fadeIn(500);
    //$.pnotify.defaults.styling = "bootstrap3";

    bindBtnModal();
});

function bindBtnModal() {
    $(".btnModal").click(function () {
        $("#myModal").html("");
        $("#myModal").load($(this).attr("data-href"));
        $("#myModal").modal({ keyboard: true });
    });
}

function Autorizar(papel, formId)
{
    if ($('#' + formId).valid()) {
        $("#myModal").html("");

        $.ajax({
            method: "POST",
            url: "/Account/Autorizacao",
            data: { papel: papel, formId: formId }
        })
        .done(function (html) {
            $("#myModal").html(html);
        });
    }
}

$(document).ready(function () {
    
    $(".UFJson").change(function () {
        $(".CidadesJson option").remove();
        $(".CidadesJson").append("<option value=''>Carregando...</option>");
        if ($(this).val() == "") {
            $(".CidadesJson option:first-child").text("Selecione...");
        }
        else {
            $.getJSON('/JSON/Cidades/?uf=' + $(this).val(), function (json) {
                $(json).each(function () {
                    $(".CidadesJson").append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                });
                $(".CidadesJson option:first-child").text("Selecione...");
            });
        }
    });

    $(".TorreJSON").change(function () {
        $(".TorreJSON option").remove();
        $(".TorreJSON").append("<option value=''>Carregando...</option>");
        if ($(this).val() == "") {
            $(".TorreJSON option:first-child").text("Selecione...");
        }
        else {
            $.getJSON('/JSON/Torres/' + $(this).val(), function (json) {
                $(json).each(function () {
                    $(".TorreJSON").append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                });
                $(".TorreJSON option:first-child").text("Selecione...");
            });
        }
    });

    $(".PavimentoJSON").change(function () {
        $(".PavimentoJSON option").remove();
        $(".PavimentoJSON").append("<option value=''>Carregando...</option>");
        if ($(this).val() == "") {
            $(".PavimentoJSON option:first-child").text("Selecione...");
        }
        else {
            $.getJSON('/JSON/Pavimentos/' + $(this).val(), function (json) {
                $(json).each(function () {
                    $(".PavimentoJSON").append("<option value='" + this.Id + "'>" + this.Nome + "</option>");
                });
                $(".PavimentoJSON option:first-child").text("Selecione...");
            });
        }
    });

    // Máscaras
    $(".celular").mask("99999-9999");
    $(".telefone").mask("(99)99999-9999");
    $(".ddd").mask("(99)");
    $('.cep').mask('99999-999');
    $(".cnpj").mask("99.999.999/9999-99");
    $('.cpf').mask('999.999.999-99');
    $('.rg').mask('9.999.999');
    $('.cbo').mask('9999-99');
    $('.pis').mask('999.99999.99-9');
    $('.ric').mask('9999999999-9');
    $('.codigoSindical').mask('999.999.999.99999-9');
    $('.mesAno').datetimepicker({
        locale: 'pt-BR',
        viewMode: 'years',
        format: 'MM/YYYY',
    });
    $('.ano').datetimepicker({
        format: "yyyy",
        viewMode: "years",
    });

    $('.dataHora').datetimepicker({
        locale: 'pt-BR',
        format: 'LT'
    });

    $('.datepicker').datetimepicker({
        locale: 'pt-BR',
        format: 'DD/MM/YYYY',
    });

    $('.dateTime').datetimepicker({
        locale: 'pt-BR',
    });

    $('.diaMes').datetimepicker({
        locale: 'pt-BR',
        format: 'DD/MM',
    });
});
$(function () {
    $('#numonly').on('keydown', '#child', function (e) { -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault() });
})

function utilParseFloat(value)
{
    if (value == "" || value == undefined)
        return 0.0;
    return parseFloat(value.replace(/[^\d\,\-]+/g, '').replace(',', '.'));
}

jQuery(function ($) {
    loadComponents();
    
    $(':password').click(function () { $(this).val(""); });
    $('.icon-phone').popover();
    $('.email').change(function () {
        this.value = this.value.toLowerCase();
    });
    $('i').tooltip({ html: true, delay: 300 });
    $(".btnModal").click(function () {
        $("#myModal").load($(this).attr("data-href"));
        $("#myModal").modal({
            keyboard: true
        });
    });
});

function loadComponents() {
    $(".valor").maskMoney({ symbol: '', showSymbol: false, thousands: '.', decimal: ',', symbolStay: false });

    $('.dataHora').datetimepicker({
        locale: 'pt-BR',
        format: 'LT'
    });

    $('.datepicker').datetimepicker({
        locale: 'pt-BR',
        format: 'DD/MM/YYYY',
    });

    $('.dateTime').datetimepicker({
        locale: 'pt-BR',
    });

    $('.diaMes').datetimepicker({
        locale: 'pt-BR',
        format: 'DD/MM',
    });

    function increment(x) {
        if (isNaN(x)) x = 0;
        return "";
    };
}

function replaceSpecialChars(str) {
    var specialChars = [
        { val: "a", let: "áàãâä" },
        { val: "e", let: "éèêë" },
        { val: "i", let: "íìîï" },
        { val: "o", let: "óòõôö" },
        { val: "u", let: "úùûü" },
        { val: "c", let: "ç" },
        { val: "A", let: "ÁÀÃÂÄ" },
        { val: "E", let: "ÉÈÊË" },
        { val: "I", let: "ÍÌÎÏ" },
        { val: "O", let: "ÓÒÕÔÖ" },
        { val: "U", let: "ÚÙÛÜ" },
        { val: "C", let: "Ç" },
        { val: "", let: "?!()" }
    ];

    var $spaceSymbol = '-';
    var regex;
    var returnString = str;
    for (var i = 0; i < specialChars.length; i++) {
        regex = new RegExp("[" + specialChars[i].let + "]", "g");
        returnString = returnString.replace(regex, specialChars[i].val).toLowerCase();
        regex = null;
    }
    return returnString.replace(/[^a-z0-9 ]/gi, '');
};
// Auto Tab

$(".inputs").keyup(function () {
    if (this.value.length == this.maxLength) {
        var $next = $(this).next('.inputs');
        if ($next.length)
            $(this).next('.inputs').focus();
        else
            $(this).blur();
    }
});
//function ShowMessage(msg) {
//    $(".barraErro").remove();
//    $(".page-content").prepend('<div class="barraErro"><div class="col-lg-9"><div class="alert alert-danger"><a class="close" data-dismiss="alert" href="#" aria-hidden="true">&times;</a><ul class="unstyled"><li>" '+ msg + '"</li></ul></div></div></div>');
//    $(".barraErro").fadeIn(800).removeClass("hide").addClass("alert alert-error");
//    $(".barraErro").delay(2000).fadeOut(1200);
//}
