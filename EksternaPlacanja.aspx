﻿<%@ Page EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeFile="EksternaPlacanja.aspx.cs" Inherits="SCNS.EksternaPlacanja" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!--#include virtual="~/content/head.inc"-->
    <title>Unos i pregled eksternih plaćanja</title>
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
					        <li class="nav-item">
						        <!--<a class="nav-link" href="ZaduzivanjeUsluga.aspx">Zaduživanje usluga </a>-->
                                <asp:HyperLink ID="HyperLink1" class="nav-link" runat="server" NavigateUrl="~/ZaduzivanjeUsluga.aspx">Zaduživanje usluga</asp:HyperLink>
					        </li>
					        <li class="nav-item active">
						        <!--<a class="nav-link" href="EksternaPlacanja.aspx">Eksterna Plaćanja <span class="sr-only">(current)</span></a>-->
                                <asp:HyperLink ID="HyperLink2" class="nav-link" runat="server" NavigateUrl="~/EksternaPlacanja.aspx">Eksterna Plaćanja <span class="sr-only">(current)</span></asp:HyperLink>
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
                        Unos i pregled eksternih plaćanja
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
                                    <!--div ddlTypeOfPayment start-->
                                    <div class="col-12 col-lg-2 mb-1 mb-md-4">
                                        <asp:Label id="spanTypeOfPayment" runat="server" CssClass="submit-span">*</asp:Label><asp:Label id="lblTypeOfPayment" runat="server" CssClass="submit-label ml-2">Tip plaćanja:</asp:Label> 
                                    </div>
                                    <div class="col-12 col-lg-5">
                                        <asp:DropDownList ID="ddlTypeOfPayment" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="submit-dropdownlist" DataTextField="TipPlacanja" DataValueField="IDTipPlacanja" OnSelectedIndexChanged="ddlTypeOfPayment_SelectedIndexChanged" DataSourceID="dsTipPlacanja" TabIndex="1">
                                        <asp:ListItem Selected="True" Value="0">--Izaberite--</asp:ListItem>
                                        </asp:DropDownList>                   
                                        <asp:SqlDataSource ID="dsTipPlacanja" runat="server" ConnectionString="<%$ ConnectionStrings:SCNSPISConnectionString %>" SelectCommand="SELECT [IDTipPlacanja], [TipPlacanja] FROM [pisTipPlacanja]"></asp:SqlDataSource>
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                        <asp:CustomValidator runat="server" id="cvTypeOfPayment" controltovalidate="ddlTypeOfPayment" errormessage="" OnServerValidate="CvTypeOfPayment_ServerValidate" CssClass="submit-customValidator" Display="Dynamic" ForeColor="Red" ValidateEmptyText="true" ValidationGroup="AddCustomValidatorToGroup"/>
                                    </div><!--div ddlTypeOfPayment end-->
                                </div>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel id="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <fieldset>
                                <div class="row" runat="server">
                                    <!--div ddlorganization start-->
                                    <div class="col-12 col-lg-2 mb-1 mb-md-4">
                                        <asp:Label id="spanorganization" runat="server" CssClass="submit-span">*</asp:Label><asp:Label id="lblorganization" runat="server" CssClass="submit-label ml-2">Organizacija:</asp:Label> 
                                    </div>
                                    <div class="col-12 col-lg-5">
                                        <asp:DropDownList ID="ddlorganization" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="submit-dropdownlist" DataTextField="NazivOrganizacije" DataValueField="IDOrganizacija" OnSelectedIndexChanged="ddlorganization_SelectedIndexChanged" DataSourceID="dsOrganization" TabIndex="2">
                                        <asp:ListItem Selected="True" Value="0">--Izaberite--</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="dsOrganization" runat="server" ConnectionString="<%$ ConnectionStrings:SCNSPISConnectionString %>" SelectCommand="SELECT [NazivOrganizacije], [IDOrganizacija] FROM [blOrganizacija]"></asp:SqlDataSource>                      
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                        <asp:CustomValidator runat="server" id="cvorganization" controltovalidate="ddlorganization" errormessage="" OnServerValidate="Cvorganization_ServerValidate" CssClass="submit-customValidator" Display="Dynamic" ForeColor="Red" ValidateEmptyText="true" ValidationGroup="AddCustomValidatorToGroup"/>
                                    </div><!--div ddlorganization end-->
                                </div>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel id="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <fieldset>
                                <div class="row" runat="server">
                                    <!--div addOrganization start-->
                                    <div class="col-12 col-lg-2 mb-1 mb-md-4">                                    
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-md-4">
                                        <asp:Button ID="btnAddOrganization" runat="server" Text="Dodaj organizaciju" CssClass="btn btn-success" OnClick="btnAddOrganization_Click" TabIndex="2"/>
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                    </div><!--div addOrganization end-->
                                </div>
                                <div class="row" runat="server" id="myDiv1">
                                    <!--div addOrganization TextBox start-->
                                    <div class="col-12 col-lg-2 mb-1 mb-md-4">                                    
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-md-4">
                                        <asp:TextBox ID="txtorganization" runat="server" CssClass="submit-dropdownlist" maxlength="30"></asp:TextBox>
                                        <asp:Button ID="btnAdd" runat="server" Text=" + Dodaj" CssClass="btn btn-secondary" OnClick="btnAdd_Click"/>
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                        <asp:Label ID="errLabel3" runat="server" CssClass="submit-customValidator"></asp:Label>
                                        <asp:CustomValidator ID="cvAdd" runat="server" ErrorMessage="" controltovalidate="txtorganization" Display="Dynamic" ForeColor="Red" CssClass="submit-customValidator" ValidateEmptyText="true" OnServerValidate="CvAdd_ServerValidate" ValidationGroup="AddCustomValidatorToGroup"></asp:CustomValidator>
                                    </div><!--div addOrganization TextBox end-->
                                </div>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel id="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <fieldset>
                                <div class="row" runat="server">
                                    <!--div facture number start-->
                                    <div class="col-12 col-lg-2 mb-1 mb-md-4">
                                        <asp:Label id="spanfacturenumber" runat="server" CssClass="submit-span">*</asp:Label><asp:Label id="lblfacturenumber" runat="server" CssClass="submit-label ml-2">Broj fakture:</asp:Label>
                                    </div>
                                    <div class="col-12 col-lg-5">
                                        <asp:TextBox ID="txtfacturenumber" runat="server" CssClass="submit-textbox" maxlength="20" TabIndex="4" ontextchanged="txtfacturenumber_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <asp:Label id="facturenumberexample" runat="server" CssClass="submit-example ml-2"></asp:Label>
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                        <asp:Label ID="errLabel" runat="server" CssClass="submit-customValidator"></asp:Label>
                                        <asp:CustomValidator ID="cvfacturenumber" runat="server" ErrorMessage="" controltovalidate="txtfacturenumber" Display="Dynamic" ForeColor="Red" CssClass="submit-customValidator" ValidateEmptyText="true" OnServerValidate="Cvfacturenumber_ServerValidate" ValidationGroup="AddCustomValidatorToGroup"></asp:CustomValidator>
                                    </div><!--div facture number end-->
                                    <!--div price start-->
                                    <div class="col-12 col-lg-2 mb-1 mb-md-4">
                                        <asp:Label id="spanprice" runat="server" CssClass="submit-span">*</asp:Label><asp:Label id="lblprice" runat="server" CssClass="submit-label ml-2">Iznos:</asp:Label>
                                    </div>
                                    <div class="col-12 col-lg-5">
                                        <asp:TextBox ID="txtprice" runat="server" CssClass="price-textbox" maxlength="8" TabIndex="5" ontextchanged="txtprice_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <asp:Label id="priceexample" runat="server" CssClass="submit-example ml-2">Primer: 3.000 din</asp:Label>
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                        <asp:Label ID="errLabel1" runat="server" CssClass="submit-customValidator"></asp:Label>
                                        <asp:CustomValidator ID="cvprice" runat="server" ErrorMessage="" controltovalidate="txtprice" Display="Dynamic" ForeColor="Red" CssClass="submit-customValidator" ValidateEmptyText="true" OnServerValidate="Cvprice_ServerValidate" ValidationGroup="AddCustomValidatorToGroup"></asp:CustomValidator>
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
                                        <asp:CustomValidator ID="cvdate" runat="server" ErrorMessage="" controltovalidate="txtdate" Display="Dynamic" ForeColor="Red" CssClass="submit-customValidator" ValidateEmptyText="true" OnServerValidate="Cvdate_ServerValidate" ValidationGroup="AddCustomValidatorToGroup"></asp:CustomValidator>
                                    </div><!--div date end-->
                                    <!--div description start-->
                                    <div class="col-12 col-lg-2 mb-1 mb-md-4">
                                        <asp:Label id="spandescription" runat="server" CssClass="submit-span-none">&nbsp;</asp:Label><asp:Label id="lbldescription" runat="server" CssClass="submit-label ml-2">Opis/Napomena:</asp:Label>
                                    </div>
                                    <div class="col-12 col-lg-10">
                                        <asp:TextBox ID="txtdescription" runat="server" CssClass="price-textbox" maxlength="250" TabIndex="7" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                    </div><!--div description end-->                                
                                    <!--div button start-->
                                    <div class="col-12 col-md-2">
                                        <article class="py-3">
                                            <asp:Button ID="btnSubmit" runat="server" Text="Upiši eksterno plaćanje" CssClass="btn btn-danger save" OnClick="BtnSubmit_Click" TabIndex="8"/>
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
                            <asp:Button ID="btnSearch" runat="server" Text=">>>Pretraži eksterno plaćanje<<<" CssClass="btn btn-outline-secondary" OnClick="btnSearch_Click" TabIndex="9"/>
                        </div>
                        <div class="col-12 col-md-4 mb-1">
                        </div><!--div search end-->
                    </div>
                    <asp:UpdatePanel id="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <fieldset>
                                <div class="row" runat="server" id="myDiv3">
                                    <div class="col-12 col-md-5 mb-1 my-3 text-center text-md-right">
                                        <asp:TextBox ID="txtsearch" runat="server" CssClass="submit-textbox" maxlength="20"></asp:TextBox>
                                    </div>
                                    <div class="col-12 col-md-7 mb-1 my-3 text-center text-md-left">
                                        <asp:Button ID="btnSearch1" runat="server" Text="Pretraži po broju plaćanja" CssClass="btn btn-danger" OnClick="btnSearch1_Click"/>
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
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" ShowHeaderWhenEmpty="True" Width="100%" DataKeyNames="IDEksternoPlacanje" style="margin-top: 0px" RowStyle-CssClass="rowHover" DataSourceID="dsGridViewEP">
                                        <Columns>
                                            <asp:BoundField DataField="IDEksternoPlacanje" HeaderText="IDEksternoPlacanje" SortExpression="IDEksternoPlacanje" Visible="false"/>
                                            <asp:BoundField DataField="TipPlacanja" HeaderText="Tip plaćanja" SortExpression="TipPlacanja" readonly="true"/>
                                            <asp:BoundField DataField="NazivOrganizacije" HeaderText="Naziv organizacije" SortExpression="NazivOrganizacije" readonly="true"/>
                                            <asp:BoundField DataField="BrojPlacanja" HeaderText="Broj plaćanja" SortExpression="BrojPlacanja" />
                                            <asp:BoundField DataField="Iznos" HeaderText="Iznos" SortExpression="Iznos"/>
                                            <asp:BoundField DataField="DatumPlacanja" HeaderText="Datum plaćanja" SortExpression="DatumPlacanja" DataFormatString="{0:dd-MM-yyyy}"/>
                                            <asp:BoundField DataField="Opis" HeaderText="Opis" SortExpression="Opis"/>
                                            <asp:BoundField DataField="Operater" HeaderText="Operater" SortExpression="Operater" readonly="true"/>
                                            <asp:BoundField DataField="Ponisteno" HeaderText="Poništeno" SortExpression="Ponisteno" readonly="true" DataFormatString="{0:dd-MM-yyyy HH:mm:ss}"/>
                                            <asp:CommandField CancelText="Odustani" EditText="Promeni" ShowEditButton="True" UpdateText="Upiši" ControlStyle-CssClass="link-style-gridview"/>
                                            <asp:CommandField ShowDeleteButton="true" DeleteText="Poništi" ControlStyle-CssClass="link-style-gridview"/>
                                        </Columns>
                                        <FooterStyle BackColor="#333333" BorderColor="#333333" BorderWidth="2px" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <HeaderStyle ForeColor="White" BackColor="#333333" BorderColor="White" BorderWidth="2px" BorderStyle="Solid" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <PagerStyle BackColor="#CCCCCC" BorderColor="#999999" ForeColor="#333333" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <RowStyle BackColor="Silver" BorderColor="Black" BorderWidth="1px" Font-Bold="False" Font-Names="Arial" ForeColor="Black" HorizontalAlign="Center" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="dsGridViewEP" runat="server" ConnectionString="<%$ ConnectionStrings:SCNSPISConnectionString %>" SelectCommand="SELECT TOP (100) PERCENT blEksternoPlacanje.IDEksternoPlacanje, blEksternoPlacanje.DatumPlacanja, blEksternoPlacanje.BrojPlacanja, blEksternoPlacanje.Opis, blEksternoPlacanje.Iznos, blEksternoPlacanje.Operater, blOrganizacija.NazivOrganizacije, pisTipPlacanja.TipPlacanja, blEksternoPlacanje.Ponisteno FROM blEksternoPlacanje INNER JOIN blOrganizacija ON blEksternoPlacanje.IDOrganizacija = blOrganizacija.IDOrganizacija INNER JOIN pisTipPlacanja ON blEksternoPlacanje.IDTipPlacanja = pisTipPlacanja.IDTipPlacanja ORDER BY blEksternoPlacanje.IDEksternoPlacanje DESC" OldValuesParameterFormatString="original_{0}"
                                        UpdateCommand="UPDATE blEksternoPlacanje SET [BrojPlacanja] = @BrojPlacanja,[Iznos] = @Iznos,[DatumPlacanja] = @DatumPlacanja,[Opis] = @Opis WHERE [IDEksternoPlacanje] = @original_IDEksternoPlacanje"
                                        DeleteCommand="UPDATE blEksternoPlacanje set [Ponisteno]=getDate() WHERE [IDEksternoPlacanje] = @original_IDEksternoPlacanje">
                                        <UpdateParameters>
                                            <asp:Parameter Name="BrojPlacanja" Type="String" />
                                            <asp:Parameter Name="Iznos" Type="Decimal" />
                                            <asp:Parameter Name="DatumPlacanja" Type="DateTime" />
                                            <asp:Parameter Name="Opis" Type="String" />
                                            <asp:Parameter Name="original_IDEksternoPlacanje" Type="Int32" />
                                        </UpdateParameters>
                                        <DeleteParameters>
                                            <asp:Parameter Name="original_IDEksternoPlacanje" Type="Int32" />
                                        </DeleteParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <div class="table-responsive">
                                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" ShowHeaderWhenEmpty="True" Width="100%" DataKeyNames="IDEksternoPlacanje" style="margin-top: 0px" RowStyle-CssClass="rowHover" DataSourceID="dsGridViewEPFilter">
                                        <Columns>
                                            <asp:BoundField DataField="IDEksternoPlacanje" HeaderText="IDEksternoPlacanje" SortExpression="IDEksternoPlacanje" Visible="false"/>
                                            <asp:BoundField DataField="TipPlacanja" HeaderText="Tip plaćanja" SortExpression="TipPlacanja" readonly="true"/>
                                            <asp:BoundField DataField="NazivOrganizacije" HeaderText="Naziv organizacije" SortExpression="NazivOrganizacije" readonly="true"/>
                                            <asp:BoundField DataField="BrojPlacanja" HeaderText="Broj plaćanja" SortExpression="BrojPlacanja" />
                                            <asp:BoundField DataField="Iznos" HeaderText="Iznos" SortExpression="Iznos" />
                                            <asp:BoundField DataField="DatumPlacanja" HeaderText="Datum plaćanja" SortExpression="DatumPlacanja" DataFormatString="{0:dd-MM-yyyy}"/>
                                            <asp:BoundField DataField="Opis" HeaderText="Opis" SortExpression="Opis" />
                                            <asp:BoundField DataField="Operater" HeaderText="Operater" SortExpression="Operater" readonly="true"/>
                                            <asp:BoundField DataField="Ponisteno" HeaderText="Poništeno" SortExpression="Ponisteno" readonly="true" DataFormatString="{0:dd-MM-yyyy HH:mm:ss}"/>
                                            <asp:CommandField CancelText="Odustani" EditText="Promeni" ShowEditButton="True" UpdateText="Upiši" ControlStyle-CssClass="link-style-gridview"/>
                                            <asp:CommandField ShowDeleteButton="true" DeleteText="Poništi" ControlStyle-CssClass="link-style-gridview"/>
                                        </Columns>
                                        <FooterStyle BackColor="#333333" BorderColor="#333333" BorderWidth="2px" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <HeaderStyle ForeColor="White" BackColor="#333333" BorderColor="White" BorderWidth="2px" BorderStyle="Solid" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <PagerStyle BackColor="#CCCCCC" BorderColor="#999999" ForeColor="#333333" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <RowStyle BackColor="Silver" BorderColor="Black" BorderWidth="1px" Font-Bold="False" Font-Names="Arial" ForeColor="Black" HorizontalAlign="Center" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="dsGridViewEPFilter" runat="server" ConnectionString="<%$ ConnectionStrings:SCNSPISConnectionString %>" SelectCommand="SELECT TOP (100) PERCENT blEksternoPlacanje.IDEksternoPlacanje, blEksternoPlacanje.DatumPlacanja, blEksternoPlacanje.BrojPlacanja, blEksternoPlacanje.Opis, blEksternoPlacanje.Iznos, blEksternoPlacanje.Operater, blOrganizacija.NazivOrganizacije, pisTipPlacanja.TipPlacanja, blEksternoPlacanje.Ponisteno FROM blEksternoPlacanje INNER JOIN blOrganizacija ON blEksternoPlacanje.IDOrganizacija = blOrganizacija.IDOrganizacija INNER JOIN pisTipPlacanja ON blEksternoPlacanje.IDTipPlacanja = pisTipPlacanja.IDTipPlacanja ORDER BY blEksternoPlacanje.IDEksternoPlacanje DESC"
                                        FilterExpression="BrojPlacanja LIKE '%{0}%'" OldValuesParameterFormatString="original_{0}"
                                        UpdateCommand="UPDATE blEksternoPlacanje SET [BrojPlacanja] = @BrojPlacanja,[Iznos] = @Iznos,[DatumPlacanja] = @DatumPlacanja,[Opis] = @Opis WHERE [IDEksternoPlacanje] = @original_IDEksternoPlacanje"
                                        DeleteCommand="UPDATE blEksternoPlacanje set [Ponisteno]=getDate() WHERE [IDEksternoPlacanje] = @original_IDEksternoPlacanje">
                                        <UpdateParameters>
                                            <asp:Parameter Name="BrojPlacanja" Type="String" />
                                            <asp:Parameter Name="Iznos" Type="Decimal" />
                                            <asp:Parameter Name="DatumPlacanja" Type="DateTime" />
                                            <asp:Parameter Name="Opis" Type="String" />
                                            <asp:Parameter Name="original_IDEksternoPlacanje" Type="Int32" />
                                        </UpdateParameters>
                                        <DeleteParameters>
                                            <asp:Parameter Name="original_IDEksternoPlacanje" Type="Int32" />
                                        </DeleteParameters>
                                        <FilterParameters>
                                            <asp:ControlParameter Name="BrojPlacanja" ControlID="txtsearch" PropertyName="Text" />
                                        </FilterParameters>
                                    </asp:SqlDataSource>
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
