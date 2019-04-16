using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;

namespace SCNS
{
    public partial class ZaduzivanjeUsluga : System.Web.UI.Page
    {
        //Lofg4Net declare log variable
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string SetGray = Constants.SetGray;
        public string SetRed = Constants.SetRed;
        public string SetNothing = Constants.SetNothing;
        public string SetLightGray = Constants.SetLightGray;

        protected void Page_Load(object sender, EventArgs e)
        {
            Utility utility = new Utility();
            bool ConnectionActive = utility.IsAvailableConnection();
            if (!ConnectionActive)
            {
                Response.Redirect("GreskaBaza.aspx", false); // this will tell .NET framework not to stop the execution of the current thread and hence the error will be resolved.
            }
            AvoidCashing();
            ShowDatepicker();

            string encryptedParameters = Request.QueryString["d"];

            if ((encryptedParameters != string.Empty) && (encryptedParameters != null))
            {
                // replace encoded plus sign "%2b" with real plus sign +
                encryptedParameters = encryptedParameters.Replace("%2b", "+");
                string decryptedParameters = AuthenticatedEncryption.AuthenticatedEncryption.Decrypt(encryptedParameters, Constants.CryptKey, Constants.AuthKey);

                HttpRequest req = new HttpRequest("", "http://www.pis.rs", decryptedParameters);

                string data = req.QueryString["idOperater"];

                if ((data != string.Empty) && (data != null))
                {
                    Session["Usluge-idOperater"] = data;
                }
                else
                {
                    Session["Usluge-idOperater"] = "0";
                }

                if (!Page.IsPostBack)
                {
                    if (Session["Usluge-idOperater"] != null)
                    {
                        int idOperater = Convert.ToInt32(Session["Usluge-idOperater"]);
                        if (idOperater != 0)
                        {
                            SetBordersGray();
                            myDiv2.Visible = true;
                            myDiv3.Visible = false;
                            CustomValidatorActionAll(true);
                            GridView2.Visible = false;

                            DisableDDLCashierWithOperator(idOperater, false);

                            log.Info("Aplication successfully start. ");
                        }
                        else {
                            encryptedParametersNullOrEmpty();
                            log.Error("Error. idOperater is: " + encryptedParameters);
                        }
                    }
                }
            }
            else {
                encryptedParametersNullOrEmpty();
                log.Error("Error. encryptedParameters is null or string.empty. encryptedParameters is: " + encryptedParameters);
            }
        }

        protected void encryptedParametersNullOrEmpty()
        {
            SetBordersGray();
            myDiv2.Visible = true;
            myDiv3.Visible = false;
            CustomValidatorActionAll(true);
            GridView2.Visible = false;
        }

        protected void DisableDDLCashierWithOperator(int idOperator, bool enabled)
        {
            cvCashier.Enabled = enabled;
            ddlCashier.Enabled = enabled;
            ddlCashier.SelectedValue = idOperator.ToString();
            ddlCashier.BorderColor = ColorTranslator.FromHtml(SetGray);
        }

        protected void GetCertificateData(string data)
        {
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters = ParseData(data);

                foreach (var par in parameters)
                {
                    if (par.Key.Equals("idOperater", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Session["Usluge-idOperater"] = par.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Error in function GetCertificateData. " + ex.Message);
            }
        }

        protected Dictionary<string, string> ParseData(string data)
        {
            try
            {
                Dictionary<string, string> parameterList = new Dictionary<string, string>();
                //List<ReturnParameter> parameterList = new List<ReturnParameter>();

                int temp1 = 0;
                int temp2 = 0;

                int start = 0;
                do
                {
                    temp1 = data.IndexOf("|||", start);
                    start = temp1 + 3;
                    temp2 = data.IndexOf("|||", start);

                    if (temp2 > 0)
                    {
                        string paramString = data.Substring(start, temp2 - temp1 - 3);
                        string[] parameter = ParseParameter(paramString);
                        parameterList.Add(parameter[0], parameter[1]);
                    }
                }
                while (temp2 > 0);

                return parameterList;
            }
            catch (Exception ex)
            {
                throw new Exception("Parameters format is not correct. " + ex.Message);
            }
        }

        protected string[] ParseParameter(string param)
        {
            string[] parameter = new string[2];
            int temp1 = param.IndexOf("=");
            parameter[0] = param.Substring(0, temp1);
            parameter[1] = param.Substring(temp1 + 1);

            return parameter;
        }
        /*
        protected void BindGridView()
        {
            try
            {
                Utility utility = new Utility();
                DataTable dt = new DataTable();
                utility.BindGridViewZaduzenja(GridView1, out dt);

                // BIND DATABASE WITH THE GRIDVIEW.
                if (dt.Rows.Count > 0)
                {
                    if (Session["TableSorting"].ToString() != string.Empty)
                    {
                        DataTable dt1 = (DataTable)Session["TableSorting"];
                        GridView1.DataSource = dt1;
                        GridView1.DataBind();
                    }
                    else
                    {
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                        ViewState["dt"] = dt;
                        ViewState["sort"] = "Asc";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while BindGridView. " + ex.Message);
                throw new Exception("Error while BindGridView. " + ex.Message);
            }
        }

        protected void BindSearchingGridView(string SelectedValue)
        {
            try
            {
                Utility utility = new Utility();
                DataTable dt = new DataTable();
                utility.BindSearchingGridViewZaduzenja(GridView1, SelectedValue, out dt);

                // BIND DATABASE WITH THE GRIDVIEW.
                if (dt.Rows.Count > 0)
                {
                    if (Session["TableSorting"].ToString() != string.Empty)
                    {
                        DataTable dt1 = (DataTable)Session["TableSorting"];
                        GridView1.DataSource = dt1;
                        GridView1.DataBind();
                    }
                    else
                    {
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                        ViewState["dt"] = dt;
                        ViewState["sort"] = "Asc";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while BindSearchingGridView. " + ex.Message);
                throw new Exception("Error while BindSearchingGridView. " + ex.Message);
            }
        }
        */
        private void AvoidCashing()
        {
            Response.Cache.SetNoStore();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }

        protected void ShowDatepicker()
        {
            //call function pickdate() every time after PostBack in ASP.Net
            ScriptManager.RegisterStartupScript(this, GetType(), "", "pickdate();", true);
            //Avoid: jQuery DatePicker TextBox selected value Lost after PostBack in ASP.Net
            txtdate.Text = Request.Form[txtdate.UniqueID];
        }

        protected void btnAddCashier_Click(object sender, EventArgs e)
        {
            CustomValidatorActionAll(false);
            SetBordersGray();
            //myDiv1.Visible = true;
            CustomValidatorActionAll(true);
        }

        protected void CustomValidatorActionAll(Boolean action)
        {
            cvTypeOfService.Enabled = action;
            cvCashier.Enabled = action;
            cvprice.Enabled = action;
            cvdate.Enabled = action;
        }

        protected void SetBordersGray()
        {
            txtprice.BorderColor = ColorTranslator.FromHtml(SetGray);
            ddlTypeOfService.BorderColor = ColorTranslator.FromHtml(SetGray);
            ddlCashier.BorderColor = ColorTranslator.FromHtml(SetGray);
            txtdate.BorderColor = ColorTranslator.FromHtml(SetGray);
        }

        protected void txtprice_TextChanged(object sender, EventArgs e)
        {
            CheckIfChannelHasChanged1();
        }

        private void CheckIfChannelHasChanged1()
        {
            try
            {
                string ErrorMessage = string.Empty;
                bool ReturnValidation = Utils.ValidatePrice(txtprice.Text, out ErrorMessage);
                errLabel1.Text = ErrorMessage;
                if (!ReturnValidation)
                {
                    txtprice.BorderColor = ColorTranslator.FromHtml(SetRed);
                    Session["Usluge-event_controle"] = txtprice;
                }
                else
                {
                    txtprice.BorderColor = ColorTranslator.FromHtml(SetGray);
                    Session["Usluge-event_controle"] = txtdate;
                }
                SetFocusOnTextbox();
            }
            catch (Exception)
            {
                errLabel1.Text = string.Empty;
            }

        }

        protected void Cvprice_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                string ErrorMessage = string.Empty;
                args.IsValid = Utils.ValidatePrice(txtprice.Text, out ErrorMessage);
                cvprice.ErrorMessage = ErrorMessage;
                errLabel1.Text = string.Empty;
                if (!args.IsValid)
                {
                    txtprice.BorderColor = ColorTranslator.FromHtml(SetRed);
                }
                else
                {
                    txtprice.BorderColor = ColorTranslator.FromHtml(SetGray);
                }
            }
            catch (Exception)
            {
                cvprice.ErrorMessage = string.Empty;
                args.IsValid = false;
            }
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate("AddCustomValidatorToGroupZaduzenja");

                if (Page.IsValid)
                {
                    int Operater = 11111;

                    string FinalDate = string.Empty;
                    string FormatDateTime = "dd.mm.yyyy";
                    string FormatToString = "yyyy-mm-dd";
                    parceDateTime(txtdate.Text, FormatDateTime, FormatToString, out FinalDate);
                    Utility utility = new Utility();
                    log.Debug("Fields to import: " + ddlTypeOfService.SelectedItem + " " + ddlCashier.SelectedItem + " " + txtprice.Text + " " + FinalDate + " " + Operater);
                    ImportFinishedValuesInDatabase(utility, Convert.ToInt32(ddlTypeOfService.SelectedValue), Convert.ToInt32(ddlCashier.SelectedValue), Convert.ToDecimal(txtprice.Text), FinalDate, Operater);
                    GridView1.DataBind();
                    errLabel1.Text = string.Empty;
                    errLabel2.Text = string.Empty;
                }
                else if (!Page.IsValid)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
                }
            }
            catch (Exception ex)
            {
                log.Error("Button submit error. " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
            }
        }

        protected void ImportFinishedValuesInDatabase(Utility utility, int TypeOfServiceSelectedValue, int CashierSelectedValue, decimal Price, string Date, int Operater)
        {
            try
            {
                utility.upisiZaduzenje(TypeOfServiceSelectedValue, CashierSelectedValue, Price, Date, Operater);
                ddlTypeOfService.SelectedValue = "0";
                ddlCashier.SelectedValue = "0";
                txtprice.Text = string.Empty;
                txtdate.Text = string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while importing All values in database. " + ex.Message);
            }
        }

        protected void parceDateTime(string dateTime, string FormatDateTime, string FormatToString, out string dateTimeFinal)
        {
            dateTimeFinal = string.Empty;
            DateTime FinalDate1 = DateTime.ParseExact(dateTime, FormatDateTime, CultureInfo.InvariantCulture);
            string FinalDate = FinalDate1.ToString(FormatToString);
            log.Debug("FinalDate to import: " + FinalDate);

            dateTimeFinal = FinalDate;
        }

        public void SetFocusOnTextbox()
        {
            try
            {
                if (Session["Usluge-event_controle"] != null)
                {
                    TextBox controle = (TextBox)Session["Usluge-event_controle"];
                    //controle.Focus();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "foc", "document.getElementById('" + controle.ClientID + "').focus();", true);
                }
            }
            catch (InvalidCastException inEx)
            {
                log.Error("Problem with setting focus on control. Error: " + inEx);
            }
        }

        public void SetFocusOnDropDownLists()
        {
            try
            {
                if (Session["Usluge-event_controle-DropDownList"] != null)
                {
                    DropDownList padajucalista = (DropDownList)Session["Usluge-event_controle-DropDownList"];
                    //padajucalista.Focus();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "foc", "document.getElementById('" + padajucalista.ClientID + "').focus();", true);
                }
            }
            catch (InvalidCastException inEx)
            {
                log.Error("Problem with setting focus on control. Error: " + inEx);
            }
        }

        protected void ddlTypeOfService_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedValue = Convert.ToInt32(ddlTypeOfService.SelectedValue);
            if (SelectedValue != 0)
            {
                ddlTypeOfService.BorderColor = ColorTranslator.FromHtml(SetGray);
                Session["Usluge-event_controle-DropDownList"] = ((DropDownList)sender);
                SetFocusOnDropDownLists();
            }
        }

        protected void CvTypeOfService_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                string ErrorMessage = string.Empty;
                string IDItem = "0";

                args.IsValid = Utils.ValidateTypeOdService(ddlTypeOfService.SelectedValue, IDItem, out ErrorMessage);
                cvTypeOfService.ErrorMessage = ErrorMessage;
                if (!args.IsValid)
                {
                    ddlTypeOfService.BorderColor = ColorTranslator.FromHtml(SetRed);
                }
                else
                {
                    ddlTypeOfService.BorderColor = ColorTranslator.FromHtml(SetGray);
                }
            }
            catch (Exception)
            {
                cvTypeOfService.ErrorMessage = string.Empty;
                args.IsValid = false;
            }
        }

        protected void ddlCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedValue = Convert.ToInt32(ddlCashier.SelectedValue);
            if (SelectedValue != 0)
            {
                ddlCashier.BorderColor = ColorTranslator.FromHtml(SetGray);
                Session["Usluge-event_controle-DropDownList"] = ((DropDownList)sender);
                SetFocusOnDropDownLists();
            }
        }

        protected void CvCashier_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                string ErrorMessage = string.Empty;
                string IDItem = "0";

                args.IsValid = Utils.ValidateCashier(ddlCashier.SelectedValue, IDItem, out ErrorMessage);
                cvCashier.ErrorMessage = ErrorMessage;
                if (!args.IsValid)
                {
                    ddlCashier.BorderColor = ColorTranslator.FromHtml(SetRed);
                }
                else
                {
                    ddlCashier.BorderColor = ColorTranslator.FromHtml(SetGray);
                }
            }
            catch (Exception)
            {
                cvCashier.ErrorMessage = string.Empty;
                args.IsValid = false;
            }
        }

        protected void Cvdate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (txtdate.Text != string.Empty)
                {
                    DateTime datum = DateTime.ParseExact(txtdate.Text, "dd.MM.yyyy", null);
                    log.Debug("Datum je: " + datum);
                    string ErrorMessage1 = string.Empty;

                    args.IsValid = Utils.ValidateDate(datum, out ErrorMessage1);
                    cvdate.ErrorMessage = ErrorMessage1;
                    if (!args.IsValid)
                    {
                        txtdate.BorderColor = ColorTranslator.FromHtml(SetRed);
                    }
                    else
                    {
                        txtdate.BorderColor = ColorTranslator.FromHtml(SetGray);
                    }
                }
                else
                {
                    if (txtdate.Text == string.Empty)
                    {
                        cvdate.ErrorMessage = "Datum je obavezno polje. ";
                        txtdate.BorderColor = ColorTranslator.FromHtml(SetRed);
                        args.IsValid = false;
                    }
                    else
                    {
                        txtdate.BorderColor = ColorTranslator.FromHtml(SetGray);
                        args.IsValid = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Greska prilikom validacije cvdate. " + ex.Message);
                txtdate.Text = string.Empty;
                cvdate.ErrorMessage = "Datum je u pogrešnom formatu. ";
                txtdate.BorderColor = ColorTranslator.FromHtml(SetRed);
                args.IsValid = false;
            }
        }

        protected void txtdate_TextChanged(object sender, EventArgs e)
        {
            CheckIfChannelHasChanged2();
        }

        private void CheckIfChannelHasChanged2()
        {
            try
            {
                bool ReturnValidation = false;

                if (txtdate.Text != string.Empty)
                {
                    DateTime datum = DateTime.ParseExact(txtdate.Text, "dd.MM.yyyy", null);
                    log.Debug("Datum je: " + datum);
                    string ErrorMessage1 = string.Empty;

                    ReturnValidation = Utils.ValidateDate(datum, out ErrorMessage1);
                    errLabel2.Text = ErrorMessage1;
                    if (!ReturnValidation)
                    {
                        txtdate.BorderColor = ColorTranslator.FromHtml(SetRed);
                        Session["Usluge-event_controle"] = txtdate;
                    }
                    else
                    {
                        txtdate.BorderColor = ColorTranslator.FromHtml(SetGray);
                        //Session["Usluge-event_controle"] = txtdescription;
                    }
                }
                else
                {
                    if (txtdate.Text == string.Empty)
                    {
                        errLabel2.Text = "Datum je obavezno polje. ";
                        txtdate.BorderColor = ColorTranslator.FromHtml(SetRed);
                        Session["Usluge-event_controle"] = txtdate;
                    }
                    else
                    {
                        txtdate.BorderColor = ColorTranslator.FromHtml(SetGray);
                        //Session["Usluge-event_controle"] = txtdescription;
                    }
                }
                SetFocusOnTextbox();
            }
            catch (Exception ex)
            {
                log.Error("Greska prilikom validacije txtdate. " + ex.Message);
                txtdate.Text = string.Empty;
                errLabel2.Text = "Datum je u pogrešnom formatu. ";
                txtdate.BorderColor = ColorTranslator.FromHtml(SetRed);
                Session["Usluge-event_controle"] = txtdate;
                SetFocusOnTextbox();
            }
        }
        /*
        protected void GridView1_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            Utility utility = new Utility();
            // GRIDVIEW PAGING.
            GridView1.PageIndex = e.NewPageIndex;
            BindGridView();
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt1 = (DataTable)ViewState["dt"];

            if (dt1.Rows.Count > 0)
            {
                if (Convert.ToString(ViewState["sort"]) == "Asc")
                {
                    dt1.DefaultView.Sort = e.SortExpression + " Desc";
                    ViewState["sort"] = "Desc";
                }
                else
                {
                    dt1.DefaultView.Sort = e.SortExpression + " Asc";
                    ViewState["sort"] = "Asc";
                }
                GridView1.DataSource = dt1;
                GridView1.DataBind();
                Session["TableSorting"] = dt1.DefaultView.ToTable();
            }
        }


        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            SetBordersGray();
            Utility utility = new Utility();
            GridView1.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                CustomValidatorActionAll(false);
                SetBordersGray();
                Utility utility = new Utility();
                int row = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
                utility.stornirajRed(row);
                log.Debug("Row with ID " + row + " has annulled column. ");
                BindGridFinal(utility);
                CustomValidatorActionAll(true);
            }
            catch (Exception ex)
            {
                log.Error("RowDeleting error. " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
                CustomValidatorActionAll(true);
            }
        }
        */
        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ZaduzivanjeUsluga.aspx", false);
            }
            catch (Exception ex)
            {
                log.Error("btnBack error. " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
            }
        }

        protected void btnSearch1_Click(object sender, EventArgs e)
        {
            try
            {
                GridView2.DataBind();
            }
            catch (Exception ex)
            {
                log.Error("btnSearch1 error. " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralertSearch", "erroralertSearch();", true);
            }
        }
        /*
        protected void BindGridFinal(Utility utility)
        {
            string SelectedValue = ddlCashier1.SelectedItem.Text;
            if (SelectedValue != "--Izaberite--")
            {
                BindSearchingGridView(SelectedValue);
            }
            else
            {
                BindGridView();
            }
        }
        
        protected void ddlCashier1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedValue = Convert.ToInt32(ddlCashier1.SelectedValue);
            if (SelectedValue != 0)
            {
                ddlTypeOfService.BorderColor = ColorTranslator.FromHtml(SetGray);
                Session["Usluge-event_controle-DropDownList"] = ((DropDownList)sender);
                SetFocusOnDropDownLists();
            }
        }
        */
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                myDiv2.Visible = false;
                myDiv3.Visible = true;
                GridView1.Visible = false;
                GridView2.Visible = true;
            }
            catch (Exception ex)
            {
                log.Error("btnSearch error. " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
            }
        }

    }
}