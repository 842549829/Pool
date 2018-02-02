<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateProduct.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage.UpdateProduct" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Styles/core.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../Styles/form.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../Styles/icon/main.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <div class="form">
        <h2>基础修改信息：</h2>
        <form id="form1" runat="server">
        <table>
            <colgroup>
                <col class="w15" />
                <col class="w35" />
                <col class="w15" />
                <col class="w35" />
            </colgroup>
            <tr>
                <td class="title">
                    用户名
                </td>
                <td>
                    <asp:Label ID="lblAccountNo" runat="server" ></asp:Label>
                </td>
                <td class="title">
                    公司类型 
                </td>
                <td>
                    <asp:Label ID="lblCompanyType" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    姓名
                </td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="text"></asp:TextBox>
                        <span id="lblUserName" runat="server" class="tips-txt"></span>
                </td>
                <td class="title">
                    昵称 
                </td>
                <td>
                    <asp:TextBox ID="txtPetName" runat="server" CssClass="text"></asp:TextBox>
                    <span id="lblPetName" runat="server" class="tips-txt"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    所在地
                </td>
                <td>
                    <a style="display: inline;" class="areaSelect" href="javascript:void(0);">
                        <span class="provinceName"  runat="server" id="lblBindLocation">请选择地区</span><i class="iconfont">&#405;</i>
                    </a> 
                    <span id="lblLocation"></span>
                </td>
                <td class="title">
                    地址
                </td>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="text"></asp:TextBox>
                        <span class="tips-txt" id="lblAddress" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    邮政编码
                </td>
                <td>
                    <asp:TextBox ID="txtPostCode" runat="server" CssClass="text"></asp:TextBox>
                    <span></span>
                </td>
                <td class="title">
                    传真</td>
                <td>
                    <asp:TextBox ID="txtFaxes" runat="server" CssClass="text"></asp:TextBox>
                    <span></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    联系人
                </td>
                <td>
                    <asp:TextBox ID="txtLinkman" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt" id="lblLinkman" runat="server"></span>
                </td>
                <td class="title">
                    联系人手机
                </td>
                <td>
                    <asp:TextBox ID="txtLinkManPhone" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt" id="lblLinkManPhone" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    E_Mail
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="text"></asp:TextBox>
                    <span></span>
                </td>
               <%-- <td class="title">
                    MSN
                </td>
                <td>
                    <asp:TextBox ID="txtMSN" runat="server" CssClass="text"></asp:TextBox>
                    <span></span>
                </td>--%>
            </tr>
            <tr>
                <td class="title">
                    QQ
                </td>
                <td>
                    <asp:TextBox ID="txtQQ" runat="server" CssClass="text"></asp:TextBox>
                    <span></span>
                </td>
                <td class="title">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr class="btns">
                <td colspan="4">
                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn class1" onclick="btnSave_Click" />
                    <input class="btn class2" value="返回" type="button" onclick="window.location.href='./CompanyList.aspx?Search=Back';" />
                </td>
            </tr>
        </table>

        <asp:HiddenField ID="hidAddress" runat="server" />
        </form>
    </div>
    <script src="../../../Scripts/json2.js" type="text/javascript"></script>
    <script src="../../../Scripts/widget/form-ui.js" type="text/javascript"></script>
    <script src="../../../Scripts/widget/common.js" type="text/javascript"></script>
    <script src="../../../Scripts/OrganizationModule/Address.js" type="text/javascript"></script>
    <script src="../../../Scripts/OrganizationModule/RoleModule/FixityInformation.js" type="text/javascript"></script>
    <script src="../../../Scripts/OrganizationModule/TerraceModule/UpdateCompanyInfo.js" type="text/javascript"></script>
  <%--  <script type="text/javascript">
        $(function () {
            $("#btnSave").click(function () {
                document.getElementById("hidAddress").value = $(".areaData").val().length < 1 ? $("#hidAddress").val() : $(".areaData").val();
            });
        });
    </script>--%>
</body>
</html>
