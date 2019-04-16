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
			<!--header navigation start-->
			<div class="collapse navbar-collapse" id="main-menu">
                <div class="navbar-nav ml-auto mt-2 px-lg-5">
                    <span id="lbl_Timer" style="font-size:21px;">Stranica ažurirana: <span id="date" style="font-size:21px;"></span>
                        <script>
                            var d = new Date();
                            var months = ["January","February","March","April","May","June","July","August","September","October","November","December"];
                            document.getElementById("date").innerHTML = d.getDate() + " " + months[d.getMonth()] + " " + d.getFullYear() + " u " + d.getHours() + ":" + d.getMinutes() +  ":" + d.getSeconds();
                        </script>
                    </span>
                </div>                    
			</div><!--header navigation end-->
        </nav>
    </div>
</header><!--header end-->
