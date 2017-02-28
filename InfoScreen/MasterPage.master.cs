using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage :System.Web.UI.MasterPage {

    protected String today;

    protected void Page_Load(object sender,EventArgs e) {
        CultureInfo ci = new CultureInfo("da-DK");
        Thread.CurrentThread.CurrentCulture = ci;

        FormaтAndSetDate();
    }

    protected void UpdateDate(Object sender, EventArgs e) {
        FormaтAndSetDate();
    }

    protected void FormaтAndSetDate() {
        DateTime date = DateTime.Now.Date;

        today = date.ToString("dddd 'den' d'.' MMMM");
        today = char.ToUpper(today[0]) + today.Substring(1);

        DataBind();
    }

}
