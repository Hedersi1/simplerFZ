using System.Web.Mvc;

namespace Simple.MVC.WEB.Controllers
{
    [Authorize]
    public class InicioController : Controller
    {
        [Authorize]
        public ActionResult Indice()
        {
            return View();
        }

        public JsonResult DatatableTranslate()
        {
            return Json(new
            {
                emptyTable = "Nenhum registro encontrado.",
                info = "Página _PAGE_ de _PAGES_",
                infoEmpty = "",
                infoFiltered = "_MAX_",
                infoPostFix = "",
                infoThousands = ".",
                lengthMenu = "_MENU_",
                loadingRecords = "carregando",
                processing = "<i class='fa fa-spin fa-spinner'></i> carregando",
                search = "",
                zeroRecords = "Nenhum registro encontrado",
                paginate = new
                {
                    first = "Primeira",
                    previous = "Anterior",
                    next = "Próxima",
                    last = "Última"

                },
                aria = new
                {
                    sortAscending = ": crescente",
                    sortDescending = ": decrescente"

                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}