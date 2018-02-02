<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyC.ascx.cs" Inherits="ChinaPay.B3B.TransactionWeb.UserControl.CompanyC" %>
<style type="text/css">
    .dropdown-input li
    {
        overflow: hidden;
    }
</style>
<div style="position: relative; display: inline-block; *display: inline;">
    <asp:TextBox CssClass="text" runat="server" Width="140px" ID="txtCompanyName" AutoCompleteType="Disabled"></asp:TextBox>
    <asp:HiddenField ID="hidCompanyId" runat="server" />
    <asp:HiddenField ID="hidCompanyType" runat="server" />
    <asp:HiddenField ID="hidIsShowDisable" runat="server" />
    <div id="divCompany" class="dropdown-input" runat="server" style="display: none;
        z-index: 999; left: 0; top: 26px;">
    </div>
</div>
<script type="text/javascript">
    $(function () {
        $("#<%=hidCompanyId.ClientID %>").addClass("company");
        if ($.trim($("#<%=txtCompanyName.ClientID %>").val()) != "" && $("#<%=hidCompanyId.ClientID %>").val() == "") {
            $("#<%=txtCompanyName.ClientID %>").val("");
        }
        $("#<%=divCompany.ClientID %> ul li").live("click", function () {
            $("#<%=divCompany.ClientID %> ul li").removeClass("obvious-a");
            $(this).addClass("obvious-a");
            $("#<%=hidCompanyId.ClientID %>").val($(this).attr("val"));
            $("#<%=txtCompanyName.ClientID %>").val($(this).html());
            $("#<%=divCompany.ClientID %>").hide();
        });
        var index = 0;
        $("#<%=divCompany.ClientID %>").live("mouseleave", function () {
            $(this).hide();
        });
        $("#<%=txtCompanyName.ClientID %>").live("keyup", function (ev) {
            //SearchItem("<%=txtCompanyName.ClientID %>", "<%=divCompany.ClientID %>", "<%=hidCompanyId.ClientID %>", "<%=txtCompanyName.ClientID %>", "<%=hidCompanyType.ClientID %>", event);
            if ($.trim($("#<%=txtCompanyName.ClientID %>").val()) != "") {
                if (ev.keyCode == 13) {
                    if ($("#<%=divCompany.ClientID %>").css("display") != "none") {
                        $("#<%=hidCompanyId.ClientID %>").val($("#<%=divCompany.ClientID %> ul li").eq(index).attr("val"));
                        $("#<%=txtCompanyName.ClientID %>").val($("#<%=divCompany.ClientID %> ul li").eq(index).html());
                        if (index <= 0) {
                            index = $("#<%=divCompany.ClientID %> ul li").length;
                        }
                    }
                }
                if (ev.keyCode == 38) {
                    if ($("#<%=divCompany.ClientID %>").css("display") != "none") {
                        index = parseInt(index) - 1;
                        $("#<%=divCompany.ClientID %> ul li").removeClass("obvious-a");
                        $("#<%=divCompany.ClientID %> ul li").eq(index).addClass("obvious-a");

                        $("#<%=hidCompanyId.ClientID %>").val($("#<%=divCompany.ClientID %> ul li").eq(index).attr("val"));
                        $("#<%=txtCompanyName.ClientID %>").val($("#<%=divCompany.ClientID %> ul li").eq(index).html());
                        if (index <= 0) {
                            index = $("#<%=divCompany.ClientID %> ul li").length;
                        }
                    }

                } else if (ev.keyCode == 40) {
                    if ($("#<%=divCompany.ClientID %>").css("display") != "none") {
                        index = parseInt(index) + 1;
                        if (index >= $("#<%=divCompany.ClientID %> ul li").length) {
                            index = 0;
                        }
                        $("#<%=hidCompanyId.ClientID %>").val($("#<%=divCompany.ClientID %> ul li").eq(index).attr("val"));
                        $("#<%=txtCompanyName.ClientID %>").val($("#<%=divCompany.ClientID %> ul li").eq(index).html());
                        $("#<%=divCompany.ClientID %> ul li").removeClass("obvious-a");
                        $("#<%=divCompany.ClientID %> ul li").eq(index).addClass("obvious-a");
                    }
                } else if (ev.keyCode == 27) {
                    $("#<%=divCompany.ClientID %>").hide();
                } else {
                    sendPostRequest("/UserControlHandler/CompanyQuery.ashx/QueryCompanyInfo", JSON.stringify({ "companyName": $.trim($("#<%=txtCompanyName.ClientID %>").val()), "companyType": $("#<%=hidCompanyType.ClientID %>").val(), "showDisable": $("#<%=hidIsShowDisable.ClientID %>").val() }), function (e) {
                        $("#<%=divCompany.ClientID %>").html("");
                        var str = "<ul>";
                        $.each(e, function (i, item) {
                            if ($.trim($("#<%=txtCompanyName.ClientID %>").val()) == item.text.split('-')[0]) {
                                $("#<%=hidCompanyId.ClientID %>").val(item.value);
                            }
                            str += "<li val='" + item.value + "' >" + item.text + "</li>";
                        });
                        str += "</ul>";
                        $("#<%=divCompany.ClientID %>").append(str);
                        $(".div").hide();
                        if (e.length == 0) {
                            $("#<%=divCompany.ClientID %>").hide();
                        } else {
                            $("#<%=divCompany.ClientID %>").show();
                        }
                        $("#<%=divCompany.ClientID %> ul li").eq(0).addClass("obvious-a");
                    }, function (e) {
                        alert(e.responseText);
                    });
                    index = 0;
                }
            } else {
                $("#<%=divCompany.ClientID %>").hide();
                $("#<%=hidCompanyId.ClientID %>").val("");
            }
        });
        //        $("#<%=txtCompanyName.ClientID %>").live("blur", function () {
        //            $(".div").hide();
        //        });
        //        $("#<%=txtCompanyName.ClientID %> ul li").blur("keyup", function () {
        //            SetTextvalue($(this),$("#<%=hidCompanyId.ClientID %>"),$("#<%=txtCompanyName.ClientID %>"), $("#<%=txtCompanyName.ClientID %>"));
        //        });
    });
   
</script>
