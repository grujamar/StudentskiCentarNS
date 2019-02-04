﻿<%@ Page EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeFile="ZaduzivanjeUsluga.aspx.cs" Inherits="SCNS.ZaduzivanjeUsluga" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!--#include virtual="~/content/head.inc"-->
    <title>Zaduživanje za određenu uslugu</title>
    <script type="text/javascript">
        function pickdate() {
            $("[id$=txtdate]").datepicker({
                showOn: 'button',
                buttonText: 'Izaberite datum',
                buttonImageOnly: true,                
                buttonImage: "img/calendar.png",
                dayNames: ['Nedelja', 'Ponedeljak', 'Utorak', 'Sreda', 'Četvrtak', 'Petak', 'Subota'],
                dayNamesMin: ['Ned', 'Pon', 'Uto', 'Sre', 'Čet', 'Pet', 'Sub'],
                dateFormat: 'dd.mm.yy',
                monthNames: ['Januar', 'Februar', 'Mart', 'April', 'Maj', 'Jun', 'Jul', 'Avgust', 'Septembar', 'Oktobar', 'Novembar', 'Decembar'],
                monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'Maj', 'Jun', 'Jul', 'Avg', 'Sep', 'Okt', 'Nov', 'Dec'],
                firstDay: 1,
                constrainInput: true,
                changeMonth: true,
                changeYear: true,
                yearRange: '1900:2100',
                showButtonPanel: false,
                closeText: "Zatvori",
                beforeShow: function () { try { FixIE6HideSelects(); } catch (err) { } },
                onClose: function () { try { FixIE6ShowSelects(); } catch (err) { } }
            });
            $(".ui-datepicker-trigger").mouseover(function () {
                $(this).css('cursor', 'pointer');
            });
            $(".ui-datepicker-trigger").css("margin-bottom", "3px");
            $(".ui-datepicker-trigger").css("margin-left", "3px");
        };
        function styleBorder(message, id) {
                document.getElementById(id).style.border = message;
        };
    </script>
    <style>
        .ui-priority-secondary, .ui-widget-content .ui-priority-secondary, .ui-widget-header .ui-priority-secondary {
            font-weight: bold;
            opacity: 1;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!--header start-->
        <header class="py-3" style="background-image: linear-gradient(to right, rgba(220, 220, 220,0.3), rgba(220, 220, 220,0.9))">
            <div class="container">
                <nav class="navbar navbar-expand-md navbar-light px-0">
                <!--logo start-->
                <div class="navbar-container" id="navbar-container">
                    <asp:Image id="logo" runat="server" CssClass="logo-image" imageurl="~/img/logo.jpg"/>
                    <asp:Label id="lblscnsnaziv" runat="server" CssClass="scns-name pl-1 pl-sm-4">                               
                        Studentski centar Novi Sad                                    
                    </asp:Label>         
                </div><!--logo end-->
                <!---->
		        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#main-menu" aria-controls="main-menu" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
			        <!--header navigation start-->
			        <div class="collapse navbar-collapse" id="main-menu">
				        <ul class="navbar-nav ml-auto mt-2 px-lg-5">
					        <li class="nav-item active">
                                <asp:HyperLink ID="HyperLink1" class="nav-link" runat="server" NavigateUrl="~/ZaduzivanjeUsluga.aspx">Zaduživanje usluga <span class="sr-only">(current)</span></asp:HyperLink>
					        </li>
					        <li class="nav-item">
						        <asp:HyperLink ID="HyperLink2" class="nav-link" runat="server" NavigateUrl="~/EksternaPlacanja.aspx">Eksterna Plaćanja </asp:HyperLink>
					        </li>
				        </ul>                        
			        </div><!--header navigation end-->
                </nav>
            </div>
        </header><!--header end-->
        <!--main start-->
        <main>
            <!--lead-section start-->
            <section class="lead-section my-4">
                <div class="container">
                    <asp:Label id="lblstranicanaziv" runat="server" CssClass="page-name">
                        Zaduživanje za određenu uslugu
                    </asp:Label>
                </div>
            </section><!--lead-section end-->
             <!--AJAX ToolkitScriptManager-->
            <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></cc1:ToolkitScriptManager>
            <!--section submitform start-->
            <section class="submit py-4" runat="server" id="myDiv2">
                <div class="container">
                     <asp:UpdatePanel id="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <fieldset>
                                <div class="row" runat="server">
                                    <!--div ddlTypeOfService start-->
                                    <div class="col-12 col-lg-2 mb-1 mb-md-4">
                                        <asp:Label id="spanTypeOfService" runat="server" CssClass="submit-span">*</asp:Label><asp:Label id="lblTypeOfService" runat="server" CssClass="submit-label ml-2">Tip usluge:</asp:Label> 
                                    </div>
                                    <div class="col-12 col-lg-5">
                                        <asp:DropDownList ID="ddlTypeOfService" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="submit-dropdownlist" DataTextField="TipStavkeBlagajnickogIzvestaja" DataValueField="IDTipStavkeBlagajnickogIzvestaja" OnSelectedIndexChanged="ddlTypeOfService_SelectedIndexChanged" TabIndex="1" DataSourceID="dsTipUsluge">
                                        <asp:ListItem Selected="True" Value="0">--Izaberite--</asp:ListItem>
                                        </asp:DropDownList>                   
                                        <asp:SqlDataSource ID="dsTipUsluge" runat="server" ConnectionString="<%$ ConnectionStrings:SCNSPISConnectionString %>" SelectCommand="SELECT IDTipStavkeBlagajnickogIzvestaja, TipStavkeBlagajnickogIzvestaja FROM blVMoguceStavkeZaZaduzivanje"></asp:SqlDataSource>
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                        <asp:CustomValidator runat="server" id="cvTypeOfService" controltovalidate="ddlTypeOfService" errormessage="" OnServerValidate="CvTypeOfService_ServerValidate" CssClass="submit-customValidator" Display="Dynamic" ForeColor="Red" ValidateEmptyText="true"/>
                                    </div><!--div ddlTypeOfService end-->
                                </div>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel id="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <fieldset>
                                <div class="row" runat="server">
                                    <!--div ddlCashier start-->
                                    <div class="col-12 col-lg-2 mb-1 mb-md-4">
                                        <asp:Label id="spanCashier" runat="server" CssClass="submit-span">*</asp:Label><asp:Label id="lblCashier" runat="server" CssClass="submit-label ml-2">Blagajnica:</asp:Label> 
                                    </div>
                                    <div class="col-12 col-lg-5">
                                        <asp:DropDownList ID="ddlCashier" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="submit-dropdownlist" DataTextField="PunoIme" DataValueField="IDOsoba" OnSelectedIndexChanged="ddlCashier_SelectedIndexChanged" DataSourceID="dsBlagajnice" TabIndex="2">
                                        <asp:ListItem Selected="True" Value="0">--Izaberite--</asp:ListItem>
                                        </asp:DropDownList>                      
                                        <asp:SqlDataSource ID="dsBlagajnice" runat="server" ConnectionString="<%$ ConnectionStrings:SCNSPISConnectionString %>" SelectCommand="SELECT IDOsoba, PunoIme FROM blVMoguciBlagajniciZaZaduzivanje"></asp:SqlDataSource>
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                        <asp:CustomValidator runat="server" id="cvCashier" controltovalidate="ddlCashier" errormessage="" OnServerValidate="CvCashier_ServerValidate" CssClass="submit-customValidator" Display="Dynamic" ForeColor="Red" ValidateEmptyText="true"/>
                                    </div><!--div ddlCashier end-->
                                </div>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel id="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <fieldset>
                                <div class="row" runat="server">
                                    <!--div price start-->
                                    <div class="col-12 col-lg-2 mb-1 mb-md-4">
                                        <asp:Label id="spanprice" runat="server" CssClass="submit-span">*</asp:Label><asp:Label id="lblprice" runat="server" CssClass="submit-label ml-2">Iznos:</asp:Label>
                                    </div>
                                    <div class="col-12 col-lg-5">
                                        <asp:TextBox ID="txtprice" runat="server" CssClass="price-textbox" maxlength="8" TabIndex="3" ontextchanged="txtprice_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <asp:Label id="priceexample" runat="server" CssClass="submit-example ml-2">Primer: 3.000 din</asp:Label>
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                        <asp:Label ID="errLabel1" runat="server" CssClass="submit-customValidator"></asp:Label>
                                        <asp:CustomValidator ID="cvprice" runat="server" ErrorMessage="" controltovalidate="txtprice" Display="Dynamic" ForeColor="Red" CssClass="submit-customValidator" ValidateEmptyText="true" OnServerValidate="Cvprice_ServerValidate"></asp:CustomValidator>
                                    </div><!--div price end-->
                                    <!--div date start-->
                                    <div class="col-12 col-lg-2 mb-1 mb-md-4">
                                        <asp:Label id="spandate" runat="server" CssClass="submit-span">*</asp:Label><asp:Label id="lbldate" runat="server" CssClass="submit-label ml-2">Datum:</asp:Label>
                                    </div>
                                    <div class="col-12 col-lg-5">
                                        <asp:TextBox ID="txtdate" runat="server" CssClass="price-textbox" maxlength="10" TabIndex="6" ontextchanged="txtdate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <asp:Label id="dateexample" runat="server" CssClass="submit-example ml-2">Primer: 21.09.2010</asp:Label>
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                        <asp:Label ID="errLabel2" runat="server" CssClass="submit-customValidator"></asp:Label>
                                        <asp:CustomValidator ID="cvdate" runat="server" ErrorMessage="" controltovalidate="txtdate" Display="Dynamic" ForeColor="Red" CssClass="submit-customValidator" ValidateEmptyText="true" OnServerValidate="Cvdate_ServerValidate"></asp:CustomValidator>
                                    </div><!--div date end-->
                                    <!--div button start-->
                                    <div class="col-12 col-md-2">
                                        <article class="py-3">
                                            <asp:Button ID="btnSubmit" runat="server" Text="Upiši zaduživanje" CssClass="btn btn-danger save" OnClick="BtnSubmit_Click" TabIndex="4"/>
                                        </article>
                                    </div><!--div button end-->
                               </div>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </section><!--section submitform end-->
            <!--section search start-->
            <section class="search-section py-1 py-md-2">
                <div class="container">
                    <div class="row">
                        <!--div search start-->
                        <div class="col-12 col-md-4 mb-1">
                        </div>
                        <div class="col-12 col-md-4 mb-1 mb-4 text-center">
                            <asp:Button ID="btnSearch" runat="server" Text=">>>Pretraži zaduživanje za određenu uslugu<<<" CssClass="btn btn-outline-secondary" OnClick="btnSearch_Click" TabIndex="9"/>
                        </div>
                        <div class="col-12 col-md-4 mb-1">
                        </div><!--div search end-->
                    </div>
                    <asp:UpdatePanel id="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <fieldset>
                                <div class="row" runat="server" id="myDiv3">
                                    <div class="col-12 col-md-5 mb-1 my-3 text-center text-md-right">
                                        <asp:DropDownList ID="ddlCashier1" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="submit-dropdownlist" DataTextField="PunoIme" DataValueField="IDOsoba" OnSelectedIndexChanged="ddlCashier1_SelectedIndexChanged" DataSourceID="dsBlagajnice1" TabIndex="1">
                                        <asp:ListItem Selected="True" Value="0">--Izaberite--</asp:ListItem>
                                        </asp:DropDownList>                      
                                        <asp:SqlDataSource ID="dsBlagajnice1" runat="server" ConnectionString="<%$ ConnectionStrings:SCNSPISConnectionString %>" SelectCommand="SELECT IDOsoba, PunoIme FROM blVMoguciBlagajniciZaZaduzivanje"></asp:SqlDataSource>
                                    </div>
                                    <div class="col-12 col-md-7 mb-1 my-3 text-center text-md-left">
                                        <asp:Button ID="btnSearch1" runat="server" Text="Pretraži po blagajnici" CssClass="btn btn-danger" OnClick="btnSearch1_Click"/>
                                        <asp:Button ID="btnBack" runat="server" Text="Nazad" CssClass="btn" OnClick="btnBack_Click"/>
                                    </div>
                                </div>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </section><!--section search end-->
            <!--section GridView start-->
            <section class="section-gridview mb-3 mb-md-5">
                <div class="container container-grid">
                    <asp:UpdatePanel id="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <fieldset>
                                <div class="table-responsive">
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" ShowHeaderWhenEmpty="True" Width="100%" DataKeyNames="IDStavkaBlagajnickogIzvestaja" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowEditing="GridView1_RowEditing" OnRowDeleting="GridView1_RowDeleting" style="margin-top: 0px" RowStyle-CssClass="rowHover">
                                        <Columns>
                                            <asp:BoundField DataField="IDStavkaBlagajnickogIzvestaja" HeaderText="IDStavkaBlagajnickogIzvestaja" SortExpression="IDStavkaBlagajnickogIzvestaja" Visible="false"/>
                                            <asp:BoundField DataField="PunoIme" HeaderText="Blagajnica" SortExpression="PunoIme" readonly="true"/>
                                            <asp:BoundField DataField="TipStavkeBlagajnickogIzvestaja" HeaderText="Tip usluge" SortExpression="TipStavkeBlagajnickogIzvestaja" readonly="true"/>
                                            <asp:BoundField DataField="Datum" HeaderText="Datum" SortExpression="Datum" DataFormatString="{0:yyyy-MM-dd}"/>
                                            <asp:BoundField DataField="Iznos" HeaderText="Iznos" SortExpression="Iznos" />
                                            <asp:BoundField DataField="KadaJeUpisano" HeaderText="Datum upisa" SortExpression="KadaJeUpisano" readonly="true"/>
                                            <asp:BoundField DataField="Storno" HeaderText="Storno" SortExpression="Storno" readonly="true"/>
                                            <asp:TemplateField HeaderText="Poništi">
                                                 <ItemTemplate>
                                                    <asp:LinkButton Text="" runat="server" CssClass="fa fa-trash-o icons" CommandName="Delete"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#333333" BorderColor="#333333" BorderWidth="2px" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <HeaderStyle ForeColor="White" BackColor="#333333" BorderColor="White" BorderWidth="2px" BorderStyle="Solid" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <PagerStyle BackColor="#CCCCCC" BorderColor="#999999" ForeColor="#333333" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <RowStyle BackColor="Silver" BorderColor="Black" BorderWidth="1px" Font-Bold="False" Font-Names="Arial" ForeColor="Black" HorizontalAlign="Center" />
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </section><!--section GridView end-->
        </main><!--main end-->

        <!--#include virtual="~/content/footer.inc"-->
    </form>  
</body>   
</html>
