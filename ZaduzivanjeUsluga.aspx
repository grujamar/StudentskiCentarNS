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
        <!--#include virtual="~/content/header.inc"-->
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
                    <asp:UpdatePanel id="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <fieldset>
                                <div class="row" runat="server">
                                    <!--div ddlType start-->
                                    <div class="col-12 col-lg-2 mb-1 mb-md-4">
                                        <asp:Label id="lblType" runat="server" CssClass="submit-span">*</asp:Label><asp:Label id="Label2" runat="server" CssClass="submit-label ml-2">Tip usluge:</asp:Label> 
                                    </div>
                                    <div class="col-12 col-lg-5">
                                        <asp:DropDownList ID="ddlType" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="submit-dropdownlist" DataTextField="TipUsluge" DataValueField="IDTipUsluge" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" TabIndex="1" DataSourceID="dsTipUslugeNovo">
                                        <asp:ListItem Selected="True" Value="0">--Izaberite--</asp:ListItem>
                                        </asp:DropDownList>                   
                                        <asp:SqlDataSource ID="dsTipUslugeNovo" runat="server" ConnectionString="<%$ ConnectionStrings:SCNSConnectionString %>" SelectCommand="SELECT [IDTipUsluge], [TipUsluge] FROM [blTipUsluge]"></asp:SqlDataSource>
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                        <asp:CustomValidator runat="server" id="cvType" controltovalidate="ddlType" errormessage="" OnServerValidate="cvType_ServerValidate" CssClass="submit-customValidator" Display="Dynamic" ForeColor="Red" ValidateEmptyText="true" ValidationGroup="AddCustomValidatorToGroupZaduzenja"/>
                                    </div><!--div ddlType end-->
                                </div>
                                <div class="row" runat="server">
                                    <!--div ddlTypeOfService start-->
                                    <div class="col-12 col-lg-2 mb-1 mb-md-4">
                                        <asp:Label id="spanTypeOfService" runat="server" CssClass="submit-span">*</asp:Label><asp:Label id="lblTypeOfService" runat="server" CssClass="submit-label ml-2">Tip stavke:</asp:Label> 
                                    </div>
                                    <div class="col-12 col-lg-5">
                                        <asp:DropDownList ID="ddlTypeOfService" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="submit-dropdownlist" DataTextField="TipStavkeBlagajnickogIzvestaja" DataValueField="IDTipStavkeBlagajnickogIzvestaja" OnSelectedIndexChanged="ddlTypeOfService_SelectedIndexChanged" TabIndex="2" DataSourceID="dsTipUsluge">
                                        <asp:ListItem Selected="True" Value="0">--Izaberite--</asp:ListItem>
                                        </asp:DropDownList>                   
                                        <asp:SqlDataSource ID="dsTipUsluge" runat="server" ConnectionString="<%$ ConnectionStrings:SCNSConnectionString %>" SelectCommand="SELECT [IDTipStavkeBlagajnickogIzvestaja], [TipStavkeBlagajnickogIzvestaja], [IDTipUsluge] FROM [blVMoguceStavkeZaZaduzivanje] WHERE ([IDTipUsluge] = @IDTipUsluge)">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddlType" Name="IDTipUsluge" PropertyName="SelectedValue" Type="Int32" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                        <asp:CustomValidator runat="server" id="cvTypeOfService" controltovalidate="ddlTypeOfService" errormessage="" OnServerValidate="CvTypeOfService_ServerValidate" CssClass="submit-customValidator" Display="Dynamic" ForeColor="Red" ValidateEmptyText="true" ValidationGroup="AddCustomValidatorToGroupZaduzenja"/>
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
                                        <asp:Label id="spanCashier" runat="server" CssClass="submit-span">*</asp:Label><asp:Label id="lblCashier" runat="server" CssClass="submit-label ml-2">Osoba:</asp:Label> 
                                    </div>
                                    <div class="col-12 col-lg-5">
                                        <asp:DropDownList ID="ddlCashier" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="submit-dropdownlist" DataTextField="PunoIme" DataValueField="IDOsoba" OnSelectedIndexChanged="ddlCashier_SelectedIndexChanged" DataSourceID="dsBlagajnice" TabIndex="3">
                                        <asp:ListItem Selected="True" Value="0">--Izaberite--</asp:ListItem>
                                        </asp:DropDownList>                      
                                        <asp:SqlDataSource ID="dsBlagajnice" runat="server" ConnectionString="<%$ ConnectionStrings:SCNSConnectionString %>" SelectCommand="SELECT IDOsoba, PunoIme FROM blVMoguciBlagajniciZaZaduzivanje"></asp:SqlDataSource>
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                        <asp:CustomValidator runat="server" id="cvCashier" controltovalidate="ddlCashier" errormessage="" OnServerValidate="CvCashier_ServerValidate" CssClass="submit-customValidator" Display="Dynamic" ForeColor="Red" ValidateEmptyText="true" ValidationGroup="AddCustomValidatorToGroupZaduzenja"/>
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
                                        <asp:TextBox ID="txtprice" runat="server" CssClass="price-textbox" maxlength="8" TabIndex="4" ontextchanged="txtprice_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <asp:Label id="priceexample" runat="server" CssClass="submit-example ml-2">Primer: 3.000 din</asp:Label>
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                        <asp:Label ID="errLabel1" runat="server" CssClass="submit-customValidator"></asp:Label>
                                        <asp:CustomValidator ID="cvprice" runat="server" ErrorMessage="" controltovalidate="txtprice" Display="Dynamic" ForeColor="Red" CssClass="submit-customValidator" ValidateEmptyText="true" OnServerValidate="Cvprice_ServerValidate" ValidationGroup="AddCustomValidatorToGroupZaduzenja"></asp:CustomValidator>
                                    </div><!--div price end-->
                                    <!--div date start-->
                                    <div class="col-12 col-lg-2 mb-1 mb-md-4">
                                        <asp:Label id="spandate" runat="server" CssClass="submit-span">*</asp:Label><asp:Label id="lbldate" runat="server" CssClass="submit-label ml-2">Datum:</asp:Label>
                                    </div>
                                    <div class="col-12 col-lg-5">
                                        <asp:TextBox ID="txtdate" runat="server" CssClass="price-textbox" maxlength="10" TabIndex="5" ontextchanged="txtdate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <asp:Label id="dateexample" runat="server" CssClass="submit-example ml-2">Primer: 21.09.2010</asp:Label>
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                        <asp:Label ID="errLabel2" runat="server" CssClass="submit-customValidator"></asp:Label>
                                        <asp:CustomValidator ID="cvdate" runat="server" ErrorMessage="" controltovalidate="txtdate" Display="Dynamic" ForeColor="Red" CssClass="submit-customValidator" ValidateEmptyText="true" OnServerValidate="Cvdate_ServerValidate" ValidationGroup="AddCustomValidatorToGroupZaduzenja"></asp:CustomValidator>
                                    </div><!--div date end-->
                                    <!--div button start-->
                                    <div class="col-12 col-md-2">
                                        <article class="py-3">
                                            <asp:Button ID="btnSubmit" runat="server" Text="Upiši zaduživanje" CssClass="btn btn-danger save" OnClick="BtnSubmit_Click" TabIndex="6"/>
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
                                        <asp:TextBox ID="txtsearch" runat="server" CssClass="submit-textbox" maxlength="20"></asp:TextBox>
                                    </div>
                                    <div class="col-12 col-md-7 mb-1 my-3 text-center text-md-left">
                                        <asp:Button ID="btnSearch1" runat="server" Text="Pretraži po osobi" CssClass="btn btn-danger" OnClick="btnSearch1_Click"/>
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
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" ShowHeaderWhenEmpty="True" Width="100%" DataKeyNames="IDStavkaBlagajnickogIzvestaja" style="margin-top: 0px" RowStyle-CssClass="rowHover" DataSourceID="dsGridView">
                                        <Columns>
                                            <asp:BoundField DataField="IDStavkaBlagajnickogIzvestaja" HeaderText="IDStavkaBlagajnickogIzvestaja" SortExpression="IDStavkaBlagajnickogIzvestaja" Visible="false"/>
                                            <asp:BoundField DataField="TipStavkeBlagajnickogIzvestaja" HeaderText="Tip stavke" SortExpression="TipStavkeBlagajnickogIzvestaja" readonly="true"/>
                                            <asp:BoundField DataField="Datum" HeaderText="Datum" SortExpression="Datum" DataFormatString="{0:dd.MM.yyyy}"/>
                                            <asp:BoundField DataField="Iznos" HeaderText="Iznos" SortExpression="Iznos" />
                                            <asp:BoundField DataField="KadaJeUpisano" HeaderText="Datum upisa" SortExpression="KadaJeUpisano" DataFormatString="{0:dd.MM.yyyy HH:mm:ss}" readonly="true"/>
                                            <asp:BoundField DataField="Storno" HeaderText="Storno" SortExpression="Storno" DataFormatString="{0:dd.MM.yyyy HH:mm:ss}" readonly="true"/>
                                            <asp:CommandField ShowDeleteButton="true" DeleteText="Poništi" ControlStyle-CssClass="link-style-gridview"/>
                                        </Columns>
                                        <FooterStyle BackColor="#333333" BorderColor="#333333" BorderWidth="2px" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <HeaderStyle ForeColor="White" BackColor="#333333" BorderColor="White" BorderWidth="2px" BorderStyle="Solid" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <PagerStyle BackColor="#CCCCCC" BorderColor="#999999" ForeColor="#333333" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <RowStyle BackColor="Silver" BorderColor="Black" BorderWidth="1px" Font-Bold="False" Font-Names="Arial" ForeColor="Black" HorizontalAlign="Center" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="dsGridView" runat="server" ConnectionString="<%$ ConnectionStrings:SCNSConnectionString %>" SelectCommand="SELECT TOP (100) PERCENT IDStavkaBlagajnickogIzvestaja, TipStavkeBlagajnickogIzvestaja, Datum, Iznos, KadaJeUpisano, Storno FROM blVPregledUpisanihZaduzenja WHERE (PunoIme = @punoime) ORDER BY KadaJeUpisano DESC" DeleteCommand="blSpPonistavanjeZaduzenjaBlagajnika" DeleteCommandType="StoredProcedure">
                                        <DeleteParameters>
                                            <asp:Parameter Name="IDStavkaBlagajnickogIzvestaja" Type="Int32" />
                                        </DeleteParameters>
                                        <SelectParameters>
                                            <asp:SessionParameter Name="punoime" SessionField="Usluga-PunoIme" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <div class="table-responsive">
                                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" ShowHeaderWhenEmpty="True" Width="100%" DataKeyNames="IDStavkaBlagajnickogIzvestaja" style="margin-top: 0px" RowStyle-CssClass="rowHover" DataSourceID="dsGridView2">
                                        <Columns>
                                            <asp:BoundField DataField="IDStavkaBlagajnickogIzvestaja" HeaderText="IDStavkaBlagajnickogIzvestaja" SortExpression="IDStavkaBlagajnickogIzvestaja" Visible="false"/>
                                            <asp:BoundField DataField="PunoIme" HeaderText="Osoba" SortExpression="PunoIme" readonly="true"/>
                                            <asp:BoundField DataField="TipStavkeBlagajnickogIzvestaja" HeaderText="Tip usluge" SortExpression="TipStavkeBlagajnickogIzvestaja" readonly="true"/>
                                            <asp:BoundField DataField="Datum" HeaderText="Datum" SortExpression="Datum" DataFormatString="{0:dd-MM-yyyy}"/>
                                            <asp:BoundField DataField="Iznos" HeaderText="Iznos" SortExpression="Iznos" />
                                            <asp:BoundField DataField="KadaJeUpisano" HeaderText="Datum upisa" SortExpression="KadaJeUpisano" DataFormatString="{0:dd-MM-yyyy HH:mm:ss}" readonly="true"/>
                                            <asp:BoundField DataField="Storno" HeaderText="Storno" SortExpression="Storno" DataFormatString="{0:dd-MM-yyyy HH:mm:ss}" readonly="true"/>
                                            <asp:CommandField ShowDeleteButton="true" DeleteText="Poništi" ControlStyle-CssClass="link-style-gridview"/>
                                        </Columns>
                                        <FooterStyle BackColor="#333333" BorderColor="#333333" BorderWidth="2px" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <HeaderStyle ForeColor="White" BackColor="#333333" BorderColor="White" BorderWidth="2px" BorderStyle="Solid" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <PagerStyle BackColor="#CCCCCC" BorderColor="#999999" ForeColor="#333333" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <RowStyle BackColor="Silver" BorderColor="Black" BorderWidth="1px" Font-Bold="False" Font-Names="Arial" ForeColor="Black" HorizontalAlign="Center" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="dsGridView2" runat="server" ConnectionString="<%$ ConnectionStrings:SCNSConnectionString %>" SelectCommand="SELECT TOP (100) PERCENT IDStavkaBlagajnickogIzvestaja, PunoIme, TipStavkeBlagajnickogIzvestaja, Datum, Iznos, KadaJeUpisano, Storno FROM dbo.blVPregledUpisanihZaduzenja ORDER BY KadaJeUpisano DESC" 
                                        FilterExpression="PunoIme LIKE '%{0}%'" DeleteCommand="blSpPonistavanjeZaduzenjaBlagajnika" DeleteCommandType="StoredProcedure">
                                        <DeleteParameters>
                                            <asp:Parameter Name="IDStavkaBlagajnickogIzvestaja" Type="Int32" />
                                        </DeleteParameters>
                                        <FilterParameters>
                                            <asp:ControlParameter Name="PunoIme" ControlID="txtsearch" PropertyName="Text" />
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
