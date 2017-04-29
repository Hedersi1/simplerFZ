function CustomTable()
{
    this.TableId = '#tbDados',
    this.ActionUrl = '',
    this.Actions = '',
    this.TranslateUrl = '/Inicio/DatatableTranslate',
    this.FilterMessage = 'Digite para filtrar',
    this.Columns = [],
    this.ColumnDefs = [],
    this.UseFilter = false,
    this.Paginate = true,
    this.PaginationInfo = true,
    this.CustomParameters = [],//[{'name' : '', 'element':$()}]
    this.OnCompletedAjaxLoad = function (e, settings, json, xhr) {  },
    this.OnRenderComplete = function () { bindBtnModal(); },
    this.Order = [[0, "desc"]],
    this.DrawCallback = undefined,
    this.Bind = function () {
        var ts = this;
        $(ts.TableId).hide();
        $(ts.TableId).DataTable().destroy();
        $(ts.TableId)
             .on('preXhr.dt', function (e, settings, data)
             {
                 if (data.length == -1) // Quando desliga a paginação
                 {
                     data.length = 999;
                 }
             })
            .on('xhr.dt', ts.OnCompletedAjaxLoad).dataTable({
            "language": { "url": ts.TranslateUrl },
            "serverSide": true,
            "bLengthChange": true,
            "bAutoWidth": false,
            "processing": true,
            "bPaginate": ts.Paginate,
            "drawCallback": ts.DrawCallback,
            "bInfo" : ts.PaginationInfo,
            "columnDefs": ts.ColumnDefs,
            "bFilter": ts.UseFilter,
            "ajax": {
                "url": ts.ActionUrl,
                "data": function (d)
                {
                    for (var i = 0; i < d.columns.length; i++)
                        d.columns[i] = { data: d.columns[i].data }
                    
                    $.each(ts.CustomParameters, function (index, obj)
                    {
                        d[obj.name] = obj.element.val();
                    });
                }
            },
            "columns": ts.Columns,
            "order": ts.Order
        });
        
        $(ts.TableId).on('draw.dt', function () {
            var filter = ts.TableId + '_filter';
            var length = ts.TableId + '_length';
            
            $(length).find('select').addClass('form-control').addClass('input-sm');
            $('.paginate_button').removeClass('paginate_button');
            ts.OnRenderComplete();
            $(ts.TableId).show();
        });

    },
    this.Refresh = function () {
        $(this.TableId).dataTable().fnDraw(false);
    }
    this.Reload = function ()
    {
        $(this.TableId).DataTable().ajax.reload();
    }
}