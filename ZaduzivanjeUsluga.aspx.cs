﻿using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

            if (!Page.IsPostBack)
            {
                SetBordersGray();
                utility.BindGridViewZaduzenja(GridView1);
                //myDiv1.Visible = false;
                myDiv2.Visible = true;
                myDiv3.Visible = false;
                CustomValidatorActionAll(true);

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
                    utility.BindGridViewZaduzenja(GridView1);
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

        protected void GridView1_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            Utility utility = new Utility();
            // GRIDVIEW PAGING.
            GridView1.PageIndex = e.NewPageIndex;
            utility.BindGridViewZaduzenja(GridView1);
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            SetBordersGray();
            Utility utility = new Utility();
            GridView1.EditIndex = e.NewEditIndex;
            utility.BindGridViewZaduzenja(GridView1);
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
                Utility utility = new Utility();
                BindGridFinal(utility);
            }
            catch (Exception ex)
            {
                log.Error("btnSearch1 error. " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralertSearch", "erroralertSearch();", true);
            }
        }

        protected void BindGridFinal(Utility utility)
        {
            string SelectedValue = ddlCashier1.SelectedItem.Text;
            if (SelectedValue != "--Izaberite--")
            {
                utility.BindSearchingGridViewZaduzenja(GridView1, SelectedValue);
            }
            else
            {
                utility.BindGridViewZaduzenja(GridView1);
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                myDiv2.Visible = false;
                myDiv3.Visible = true;
            }
            catch (Exception ex)
            {
                log.Error("btnSearch error. " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
            }
        }
    }
}