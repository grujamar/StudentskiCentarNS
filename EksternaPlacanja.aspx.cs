using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SCNS
{
    public partial class EksternaPlacanja : System.Web.UI.Page
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
            
            if (!Page.IsPostBack)
            {
                SetBordersGray();
                myDiv1.Visible = false;
                myDiv2.Visible = true;
                myDiv3.Visible = false;
                GridView2.Visible = false;

                log.Info("Aplication successfully start. ");
            }
        }

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

        protected void SetBordersGray()
        {
            txtfacturenumber.BorderColor = ColorTranslator.FromHtml(SetGray);
            txtprice.BorderColor = ColorTranslator.FromHtml(SetGray);
            ddlorganization.BorderColor = ColorTranslator.FromHtml(SetGray);
            ddlTypeOfPayment.BorderColor = ColorTranslator.FromHtml(SetGray);
            txtdate.BorderColor = ColorTranslator.FromHtml(SetGray);
        }
        
        protected void Cvfacturenumber_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                string ErrorMessage = string.Empty;
                args.IsValid = Utils.ValidateFactureNumber(txtfacturenumber.Text, out ErrorMessage);
                cvfacturenumber.ErrorMessage = ErrorMessage;
                errLabel.Text = string.Empty;
                if (!args.IsValid){
                    txtfacturenumber.BorderColor = ColorTranslator.FromHtml(SetRed);
                }else {
                    txtfacturenumber.BorderColor = ColorTranslator.FromHtml(SetGray);
                }
            }
            catch (Exception)
            {
                cvfacturenumber.ErrorMessage = string.Empty;
                args.IsValid = false;
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
                if (!args.IsValid){
                    txtprice.BorderColor = ColorTranslator.FromHtml(SetRed);
                }else{
                    txtprice.BorderColor = ColorTranslator.FromHtml(SetGray);
                }
            }
            catch (Exception)
            {
                cvprice.ErrorMessage = string.Empty;
                args.IsValid = false;
            }
        }

        protected void Cvorganization_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                string ErrorMessage = string.Empty;
                string IDItem = "0";

                args.IsValid = Utils.ValidateOrganisation(ddlorganization.SelectedValue, IDItem, out ErrorMessage);
                cvorganization.ErrorMessage = ErrorMessage;
                errLabel2.Text = string.Empty;
                if (!args.IsValid){
                    ddlorganization.BorderColor = ColorTranslator.FromHtml(SetRed);
                }else {
                    ddlorganization.BorderColor = ColorTranslator.FromHtml(SetGray);
                }
            }
            catch (Exception)
            {
                cvorganization.ErrorMessage = string.Empty;
                args.IsValid = false;
            }
        }

        protected void ddlorganization_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedValue = Convert.ToInt32(ddlorganization.SelectedValue);
            if (SelectedValue != 0){
                ddlorganization.BorderColor = ColorTranslator.FromHtml(SetGray);
                Session["Fakture-event_controle-DropDownList"] = ((DropDownList)sender);
                SetFocusOnDropDownLists();
            }

        }

        protected void CvTypeOfPayment_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                string ErrorMessage = string.Empty;
                string IDItem = "0";

                args.IsValid = Utils.ValidateTypeOdPayment(ddlTypeOfPayment.SelectedValue, IDItem, out ErrorMessage);
                cvTypeOfPayment.ErrorMessage = ErrorMessage;
                if (!args.IsValid)
                {
                    ddlTypeOfPayment.BorderColor = ColorTranslator.FromHtml(SetRed);
                }
                else
                {
                    ddlTypeOfPayment.BorderColor = ColorTranslator.FromHtml(SetGray);
                }
            }
            catch (Exception)
            {
                cvTypeOfPayment.ErrorMessage = string.Empty;
                args.IsValid = false;
            }
        }

        protected void CvAdd_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                string ErrorMessage = string.Empty;
                args.IsValid = Utils.ValidateOrganizationTxt(txtorganization.Text, out ErrorMessage);
                cvAdd.ErrorMessage = ErrorMessage;
                if (!args.IsValid)
                {
                    txtorganization.BorderColor = ColorTranslator.FromHtml(SetRed);
                }
                else
                {
                    txtorganization.BorderColor = ColorTranslator.FromHtml(SetGray);
                }
            }
            catch (Exception)
            {
                cvAdd.ErrorMessage = string.Empty;
                args.IsValid = false;
            }
        }


        protected void ddlTypeOfPayment_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedValue = Convert.ToInt32(ddlTypeOfPayment.SelectedValue);
            if (SelectedValue != 0)
            {
                ddlTypeOfPayment.BorderColor = ColorTranslator.FromHtml(SetGray);
                Session["Fakture-event_controle-DropDownList"] = ((DropDownList)sender);
                SetFocusOnDropDownLists();
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
                    }else {
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

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate("AddCustomValidatorToGroup");

                if (Page.IsValid)
                {
                    int Operater = 11111;

                    string FinalDate = string.Empty;
                    string FormatDateTime = "dd.mm.yyyy";
                    string FormatToString = "yyyy-mm-dd";
                    parceDateTime(txtdate.Text, FormatDateTime, FormatToString, out FinalDate);
                    Utility utility = new Utility();
                    log.Debug("Fields to import: " + ddlTypeOfPayment.SelectedItem + " " + ddlorganization.SelectedItem + " " + txtfacturenumber.Text + " " + txtprice.Text + " " + FinalDate + " " + txtdescription.Text + " " + Operater);
                    ImportFinishedValuesInDatabase(utility, Convert.ToInt32(ddlTypeOfPayment.SelectedValue), Convert.ToInt32(ddlorganization.SelectedValue), txtfacturenumber.Text, Convert.ToDecimal(txtprice.Text), FinalDate, txtdescription.Text, Operater);
                    GridView1.DataBind();
                    errLabel3.Text = string.Empty;
                }
                else if (!Page.IsValid){
                    ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
                }
            }
            catch (Exception ex)
            {          
                log.Error("Button submit error. " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
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

        protected void btnAddOrganization_Click(object sender, EventArgs e)
        {
            CustomValidatorActionAll(false);
            SetBordersGray();
            myDiv1.Visible = true;
            CustomValidatorAction(true);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                CustomValidatorActionAll(false);

                if (txtorganization.Text != string.Empty)
                {
                    Utility utility = new Utility();
                    if (CheckIfFullNameExist(utility, txtorganization.Text))
                    {
                        errLabel3.Text = "Organizacija sa tim imenom već postoji. ";
                    }
                    else
                    {
                        ImportValuesInDatabase(utility, txtorganization.Text);
                        ddlorganization.Items.Clear();
                        ddlorganization.Items.Insert(0, new ListItem("--Izaberite--", "0"));
                        ddlorganization.DataBind();
                        errLabel3.Text = string.Empty;
                    }
                }
                else
                {
                    errLabel3.Text = string.Empty;
                }
                CustomValidatorAction(true);
                SetBordersGray();

            }
            catch (Exception ex)
            {
                log.Error("AddOrganization error. " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
            }
        }

        protected bool CheckIfFullNameExist(Utility utility, string Organization)
        {
            bool returnValue = false;
            try
            {
                List<string> OrganizationNameList = new List<string>();
                OrganizationNameList = utility.proveriOrganizaciju();

                foreach (var organizationname in OrganizationNameList)
                {
                    if (organizationname == Organization)
                    {
                        returnValue = true;
                    }
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while checking if Organization name exist. " + ex.Message);
            }
        }

        protected void ImportValuesInDatabase(Utility utility, string Organization)
        {
            try
            {
                utility.upisiOrganizaciju(Organization);
                txtorganization.Text = string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while importing value in database. " + ex.Message);
            }
        }

        protected void ImportFinishedValuesInDatabase(Utility utility, int TypeOfPaymentSelectedValue, int OrganizationSelectedValue, string FactureNumber, decimal Price, string Date, string Description, int Operater)
        {
            try
            {
                utility.upisiEksternoPlacanje(TypeOfPaymentSelectedValue, OrganizationSelectedValue, FactureNumber, Price, Date, Description, Operater);
                ddlTypeOfPayment.SelectedValue = "0";
                ddlorganization.SelectedValue = "0";
                txtorganization.Text = string.Empty;
                txtfacturenumber.Text = string.Empty;
                txtprice.Text = string.Empty;
                txtdate.Text = string.Empty;
                txtdescription.Text = string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while importing All values in database. " + ex.Message);
            }
        }
        /*
        protected void GridView1_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            try
            {
                Utility utility = new Utility();
                // GRIDVIEW PAGING.
                GridView1.PageIndex = e.NewPageIndex;
                BindGridFinal(utility);
            }
            catch (Exception ex)
            {
                log.Error("PageIndexChanging error. " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
            }
        }
        */
        protected void CustomValidatorAction(Boolean action)
        {
            cvTypeOfPayment.Enabled = action;
            cvorganization.Enabled = action;
            cvfacturenumber.Enabled = action;
            cvprice.Enabled = action;
            cvdate.Enabled = action;
            cvAdd.Enabled = !action;
        }

        protected void CustomValidatorActionAll(Boolean action)
        {
            cvTypeOfPayment.Enabled = action;
            cvorganization.Enabled = action;
            cvfacturenumber.Enabled = action;
            cvprice.Enabled = action;
            cvdate.Enabled = action;
            cvAdd.Enabled = action;
        }
        /*
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                CustomValidatorActionAll(false);
                SetBordersGray();
                Utility utility = new Utility();
                int row = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
                utility.obrisiRed(row);
                log.Debug("Row with ID " + row + " has annulled column. ");
                BindGridFinal(utility);
                CustomValidatorAction(true);
            }
            catch (Exception ex)
            {
                log.Error("RowDeleting error. " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
                CustomValidatorAction(true);
            }
        }
        
        protected void OnUpdate(object sender, EventArgs e)
        {
            try
            {
                CustomValidatorActionAll(false);
                SetBordersGray();
                Utility utility = new Utility();
               
                GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;

                //BoundField column IDEksternoPlacanje set to Visible=false
                int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
                //Get the value of column from the DataKeys using the RowIndex.
                int lblIDEksternoPlacanje = Convert.ToInt32(GridView1.DataKeys[rowIndex].Values[0]);

                //BoundField column IDEksternoPlacanje set to Visible=true
                //int lblIDEksternoPlacanje = Convert.ToInt32((row.Cells[0].Controls[id] as TextBox).Text);

                string txtBrojPlacanja = (row.Cells[3].Controls[0] as TextBox).Text;
                string txtIznos = (row.Cells[4].Controls[0] as TextBox).Text;
                string txtDatumPlacanja = (row.Cells[5].Controls[0] as TextBox).Text;
                string txtDatumPlacanjaKonacno = string.Empty;
                string FormatDateTime = "dd/MM/yyyy HH:mm:ss";
                string FormatToString = "yyyy-MM-dd";
                parceDateTime(txtDatumPlacanja, FormatDateTime, FormatToString, out txtDatumPlacanjaKonacno);
                string txtOpis = (row.Cells[6].Controls[0] as TextBox).Text;
                log.Debug("Row with ID " + lblIDEksternoPlacanje + " was UPDATED. " + txtBrojPlacanja + " " + txtIznos + " " + txtDatumPlacanjaKonacno + " " + txtOpis);
                GridView1.EditIndex = -1;

                utility.editujRed(txtBrojPlacanja, txtIznos, txtDatumPlacanjaKonacno, txtOpis, lblIDEksternoPlacanje);
                //BindGridFinalUpdate(utility, txtBrojPlacanja, txtIznos, txtDatumPlacanjaKonacno, txtOpis);
                CustomValidatorAction(true);
            }
            catch (Exception ex)
            {
                log.Error("RowEditing error. " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
                CustomValidatorAction(true);
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            CustomValidatorActionAll(false);
            SetBordersGray();
            Utility utility = new Utility();
            GridView1.EditIndex = e.NewEditIndex;
            //BindGridFinal(utility);
            CustomValidatorAction(true);
        }

        protected void OnCancel(object sender, EventArgs e)
        {
            CustomValidatorActionAll(false);
            SetBordersGray();
            Utility utility = new Utility();
            GridView1.EditIndex = -1;
            //BindGridFinal(utility);
            CustomValidatorAction(true);
            
        }
        */

        /*ONTEXTCHANGE*/
        protected void txtfacturenumber_TextChanged(object sender, EventArgs e)
        {
            CheckIfChannelHasChanged();
        }

        private void CheckIfChannelHasChanged()
        {
            try
            {
                string ErrorMessage = string.Empty;
                bool ReturnValidation = Utils.ValidateFactureNumber(txtfacturenumber.Text, out ErrorMessage);
                errLabel.Text = ErrorMessage;
                if (!ReturnValidation)
                {
                    txtfacturenumber.BorderColor = ColorTranslator.FromHtml(SetRed);
                    Session["Fakture-event_controle"] = txtfacturenumber;
                }
                else
                {
                    txtfacturenumber.BorderColor = ColorTranslator.FromHtml(SetGray);
                    Session["Fakture-event_controle"] = txtprice;
                }
                SetFocusOnTextbox();
            }
            catch (Exception)
            {
                errLabel.Text = string.Empty;
            }

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
                    Session["Fakture-event_controle"] = txtprice;
                }
                else
                {
                    txtprice.BorderColor = ColorTranslator.FromHtml(SetGray);
                    Session["Fakture-event_controle"] = txtdate;
                }            
                SetFocusOnTextbox();
            }
            catch (Exception)
            {
                errLabel1.Text = string.Empty;
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
                        Session["Fakture-event_controle"] = txtdate;
                    }
                    else
                    {
                        txtdate.BorderColor = ColorTranslator.FromHtml(SetGray);
                        Session["Fakture-event_controle"] = txtdescription;
                    }
                }
                else
                {
                    if (txtdate.Text == string.Empty)
                    {
                        errLabel2.Text = "Datum je obavezno polje. ";
                        txtdate.BorderColor = ColorTranslator.FromHtml(SetRed);
                        Session["Fakture-event_controle"] = txtdate;
                    }
                    else
                    {
                        txtdate.BorderColor = ColorTranslator.FromHtml(SetGray);
                        Session["Fakture-event_controle"] = txtdescription;
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
                Session["Fakture-event_controle"] = txtdate;
                SetFocusOnTextbox();
            }

        }

        public void SetFocusOnTextbox()
        {
            try
            {
                if (Session["Fakture-event_controle"] != null)
                {
                    TextBox controle = (TextBox)Session["Fakture-event_controle"];
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
                if (Session["Fakture-event_controle-DropDownList"] != null)
                {
                    DropDownList padajucalista = (DropDownList)Session["Fakture-event_controle-DropDownList"];
                    //padajucalista.Focus();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "foc", "document.getElementById('" + padajucalista.ClientID + "').focus();", true);
                }
            }
            catch (InvalidCastException inEx)
            {
                log.Error("Problem with setting focus on control. Error: " + inEx);
            }
        }

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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("EksternaPlacanja.aspx", false);
            }
            catch (Exception ex)
            {
                log.Error("btnBack error. " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
            }
        }
        /*
        protected void BindGridFinal(Utility utility) {
            try
            {
                if (txtsearch.Text == string.Empty)
                {
                    BindGridView();
                }
                else
                {
                    BindSearchingGridView(txtsearch.Text);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in BindGridFinal. " + ex.Message);
                throw new Exception("Error in BindGridFinal. " + ex.Message);
            }
        }

        protected void BindGridFinalUpdate(Utility utility, string txtBrojPlacanja, string txtIznos, string txtDatumPlacanja, string txtOpis)
        {
            try
            {
                if (txtsearch.Text == string.Empty)
                {
                    BindGridView();
                }
                else
                {
                    BindSearchingGridViewUpdate(txtsearch.Text, txtBrojPlacanja, txtIznos, txtDatumPlacanja, txtOpis);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in BindGridFinalUpdate. " + ex.Message);
                throw new Exception("Error in BindGridFinalUpdate. " + ex.Message);
            }
        }


        protected void BindGridView()
        {
            try
            {
                Utility utility = new Utility();
                DataTable dt = new DataTable();
                utility.BindGridView(GridView1, out dt);

                // BIND DATABASE WITH THE GRIDVIEW.
                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    ViewState["dtEP"] = dt;
                    ViewState["sortEP"] = "Asc";
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
                utility.BindSearchingGridView(GridView1, SelectedValue, out dt);

                // BIND DATABASE WITH THE GRIDVIEW.
                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    ViewState["dtEP"] = dt;
                    ViewState["sortEP"] = "Asc";
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while BindSearchingGridView. " + ex.Message);
                throw new Exception("Error while BindSearchingGridView. " + ex.Message);
            }
        }

        protected void BindSearchingGridViewUpdate(string SelectedValue, string txtBrojPlacanja, string txtIznos, string txtDatumPlacanja, string txtOpis)
        {
            try
            {
                Utility utility = new Utility();
                DataTable dt = new DataTable();
                utility.BindSearchingGridViewUpdate(GridView1, txtBrojPlacanja, txtIznos, txtDatumPlacanja, txtOpis, out dt);

                // BIND DATABASE WITH THE GRIDVIEW.
                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    ViewState["dtEP"] = dt;
                    ViewState["sortEP"] = "Asc";
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while BindSearchingGridViewUpdate. " + ex.Message);
                throw new Exception("Error while BindSearchingGridViewUpdate. " + ex.Message);
            }
        }

        
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt1 = (DataTable)ViewState["dtEP"];

            if (dt1.Rows.Count > 0)
            {
                if (Convert.ToString(ViewState["sortEP"]) == "Asc")
                {
                    dt1.DefaultView.Sort = e.SortExpression + " Desc";
                    ViewState["sortEP"] = "Desc";
                }
                else
                {
                    dt1.DefaultView.Sort = e.SortExpression + " Asc";
                    ViewState["sortEP"] = "Asc";
                }
                GridView1.DataSource = dt1;
                GridView1.DataBind();
                //Session["TableSortingEP"] = dt1.DefaultView.ToTable();
            }
        }
        */
    }
}