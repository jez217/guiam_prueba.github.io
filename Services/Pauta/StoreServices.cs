using Microsoft.AspNetCore.Mvc.Rendering;
using Pautas.Services.Conection;

namespace Pautas.Services.Pauta
{
    public class StoreServices
    {
        ConnectionDb _connService = new ConnectionDb();
        PautaServices _pautaservice = new PautaServices();

        public List<SelectListItem> StoreDropdown()
        {
            List<SelectListItem> oList = new List<SelectListItem>();

            var listStore = _pautaservice.SP_STORE_SELECT();

            foreach (var Store in listStore)
            {
                oList.Add(new SelectListItem
                {
                    Text = Store.STORE_NAME,
                    Value = Store.STORE_CODE.ToString()
                });
            }

            return oList;
        }

    }
}
