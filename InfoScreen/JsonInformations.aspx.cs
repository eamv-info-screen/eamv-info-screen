using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

public partial class JsonInformations :System.Web.UI.Page {
    public List<InformationEntity> informations = new List<InformationEntity>();
    private int? departmentId;

    protected void Page_Load(object sender, EventArgs e) {
        try {
            departmentId = (int)int.Parse(Request.QueryString["departmentId"]);
        } catch (Exception exc) {
            departmentId = 1;
        }

        if (departmentId != null) {
            DataAccess dataAccess = null;
            try {
                dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
                dataAccess.Open();

                InformationsRepository informationRepository = new InformationsRepository();
                informations = informationRepository.FetchAllActiveInformationsByDepart(dataAccess, (int)departmentId);

                Response.ContentType = "application/json";
                Response.Write(JsonSerializer.ToJSON(informations));
            } catch {

            } finally {
                dataAccess.Close();
            }
        }
    }

}