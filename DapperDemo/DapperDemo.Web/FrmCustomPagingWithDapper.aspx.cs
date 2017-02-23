using DapperDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DapperDemo.Web
{
    public partial class FrmCustomPagingWithDapper : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //DisplayData();
            }
        }
        private void DisplayData()
        {
            ITableRepository repository = new TableRepository();

            var tables = repository.GetAllWithPaging(0, 5);

            ctlLists.DataSource = tables;
            ctlLists.DataBind();
        }
    }
}