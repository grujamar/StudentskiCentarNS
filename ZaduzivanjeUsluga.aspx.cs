using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                //BindGridFinal(utility);
                //myDiv1.Visible = false;
                myDiv2.Visible = true;
                //myDiv3.Visible = false;
                CustomValidatorAction(true);

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
            //cvAdd.Enabled = action;
        }

        protected void CustomValidatorAction(Boolean action)
        {
            cvTypeOfService.Enabled = action;
            cvCashier.Enabled = action;
            cvprice.Enabled = action;
            //cvAdd.Enabled = !action;
        }

        protected void SetBordersGray()
        {
            txtprice.BorderColor = ColorTranslator.FromHtml(SetGray);
            ddlTypeOfService.BorderColor = ColorTranslator.FromHtml(SetGray);
            ddlCashier.BorderColor = ColorTranslator.FromHtml(SetGray);
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
                    //Session["Usluge-event_controle"] = txtdate;
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
        /*
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                CustomValidatorActionAll(false);

                if (txtCashier.Text != string.Empty)
                {
                    Utility utility = new Utility();
                    if (CheckIfFullNameExist(utility, txtCashier.Text))
                    {
                        errLabel3.Text = "Blagajnica sa tim imenom već postoji. ";
                    }
                    else
                    {
                        ImportValuesInDatabase(utility, txtCashier.Text);
                        ddlCashier.Items.Clear();
                        ddlCashier.Items.Insert(0, new ListItem("--Izaberite--", "0"));
                        ddlCashier.DataBind();
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
                log.Error("AddCashier error. " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
            }
        }
        
        protected bool CheckIfFullNameExist(Utility utility, string Cashier)
        {
            bool returnValue = false;
            try
            {
                List<string> CashierNameList = new List<string>();
                CashierNameList = utility.proveriBlagajnicu();

                foreach (var CashierName in CashierNameList)
                {
                    if (CashierName == Cashier)
                    {
                        returnValue = true;
                    }
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while checking if Cashier name exist. " + ex.Message);
            }
        }

        protected void ImportValuesInDatabase(Utility utility, string Cashier)
        {
            try
            {
                utility.upisiBlagajnicu(Cashier);
                txtCashier.Text = string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while importing value in database. " + ex.Message);
            }
        }
        
        protected void CvAdd_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                string ErrorMessage = string.Empty;
                //args.IsValid = Utils.ValidateOrganizationTxt(txtorganization.Text, out ErrorMessage);
                cvAdd.ErrorMessage = ErrorMessage;
                if (!args.IsValid)
                {
                    txtCashier.BorderColor = ColorTranslator.FromHtml(SetRed);
                }
                else
                {
                    txtCashier.BorderColor = ColorTranslator.FromHtml(SetGray);
                }
            }
            catch (Exception)
            {
                cvAdd.ErrorMessage = string.Empty;
                args.IsValid = false;
            }
        }
        */

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    //int Operater = 11111;


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
    }
}