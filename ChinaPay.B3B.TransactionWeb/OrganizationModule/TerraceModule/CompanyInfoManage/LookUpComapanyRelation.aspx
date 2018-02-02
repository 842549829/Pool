<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LookUpComapanyRelation.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage.LookUpComapanyRelation" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagPrefix="uc" TagName="Pagerl" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>查看公关联组关系</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form runat="server">
    <div id="data-list">
        <ul class="navType1" id="sel" runat="server">
            <li runat="server" id="liSuperiors"><a href="javascript:;" runat="server" id="superiors">
            </a></li>
            <li><a href="javascript:;" class="navType1Selected" id="Spreading" runat="server">推广用户</a></li>
            <li><a href="javascript:" id="Purchases" runat="server">下级公司</a></li>
            <li><a href="javascript:;" id="Subordinate" runat="server">内部机构</a></li>
            <li><a href="javascript:;" id="Employee" runat="server">员工列表</a></li>
        </ul>
        <div class="divInfo">
            <%--上级用户或推广用户--%>
        <h3 class="titleBg">关联关系</h3>
            <div runat="server" id="divSuperior" class="table superiors">
                <table>
                    <tr>
                        <th>
                            公司类别
                        </th>
                        <th>
                            公司简称
                        </th>
                        <th>
                            所在地
                        </th>
                        <th>
                            联系人
                        </th>
                        <th>
                            联系电话
                        </th>
                        <th>
                            用户名
                        </th>
                        <th>
                            开户时间
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCompanyType" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCompanyShortName" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblLoaction" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblContact" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblContactPhone" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblLoginAccount" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblOpentOnAccountTime" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <%--推广公司用户--%>
            <div class="table Spreading Purchases Subordinate" runat="server" id="divInfos">
                <div class="column clearfix">
                    <div class="fl">
                        用户名：<asp:TextBox ID="txtUserName" runat="server" CssClass="text"></asp:TextBox>
                        开户时间:
                        <asp:TextBox runat="server" ID="txtStartDate" class="text text-s" onfocus="WdatePicker({isShowClear:true,readOnly:true,maxDate:'#F{$dp.$D(\'txtEndDate\')}'})"></asp:TextBox>-
                        <asp:TextBox runat="server" ID="txtEndDate" class="text text-s" onfocus="WdatePicker({isShowClear:true,readOnly:true,maxDate:'%y-%M-%d',minDate:'#F{$dp.$D(\'txtStartDate\')}'})"></asp:TextBox>
                        公司简称:
                        <asp:TextBox ID="txtAbberviateName" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnSerach" runat="server" CssClass="btn class1 fr" Text="查询" OnClick="btnSerach_Click" />
                </div>
                <asp:Repeater runat="server" ID="reperList" OnPreRender="AddEmptyTemplate">
                    <HeaderTemplate>
                        <table>
                            <tr>
                                <th>
                                    公司类别
                                </th>
                                <th>
                                    公司简称
                                </th>
                                <th>
                                    所在地
                                </th>
                                <th>
                                    联系人
                                </th>
                                <th>
                                    联系电话
                                </th>
                                <th>
                                    用户名
                                </th>
                                <th>
                                    开户时间
                                </th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("Type")%>
                            </td>
                            <td>
                                <%#Eval("AbbreviateName")%>
                            </td>
                            <td>
                                <%#Eval("City")%>
                            </td>
                            <td>
                                <%#Eval("Contact")%>
                            </td>
                            <td>
                                <%#Eval("ContactCellphone")%>
                            </td>
                            <td>
                                <%#Eval("Admin")%>
                            </td>
                            <td>
                                <%#Eval("RegisterTime")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <div class="btns">
                    <uc:Pagerl runat="server" ID="PagerExtend" />
                </div>
            </div>
            <%--员工列表 --%>
            <div class="table Employee" runat="server">
                <asp:Repeater runat="server" ID="rptEmployees" OnPreRender="AddEmptyTemplate">
                    <HeaderTemplate>
                        <table>
                            <tr>
                                <th>
                                    姓名
                                </th>
                                <th>
                                    性别
                                </th>
                                <th>
                                    用户名
                                </th>
                                <th>
                                    E_Mail
                                </th>
                                <th>
                                    手机
                                </th>
                                <th>
                                    状态
                                </th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("Name")%>
                            </td>
                            <td>
                                <%#Eval("Gender")%>
                            </td>
                            <td>
                                <%#Eval("UserName")%>
                            </td>
                            <td>
                                <%#Eval("Email")%>
                            </td>
                            <td>
                                <%#Eval("Cellphone")%>
                            </td>
                            <td>
                                <%#(bool)Eval("Enabled")?"启用":"禁用"%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <div class="btns">
                    <uc:Pagerl runat="server" ID="empoyeePager" />
                </div>
            </div>
        </div>
        <div class="btns">
            <input type="button" class="btn class2" value="返回" runat="server" id="btnGoBack" />
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hfdType" Value="Spreading" />
    </form>
</body>
</html>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/Global.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("div.divInfo>div").each(function (index) { $(this).hide(); });
        $("#sel>li>a").each(function () {
            var self = $(this);
            self.removeClass("navType1Selected");
            if (self.attr("id") == $("#hfdType").val()) {
                self.addClass("navType1Selected");
                var id = "." + self.attr("id");
                $("div.divInfo>div" + id).show();
            }
        }).click(function () {
            var self = $(this);
            var value = self.attr("id");
            $("#hfdType").val(value);
            if (value != "superiors" && value != "Employee") {
                $("#btnSerach").click();
            }
            $("#sel > li > a").removeClass("navType1Selected");
            self.addClass("navType1Selected");
            var id = "." + self.attr("id");
            $("div.divInfo>div").hide();
            $("div.divInfo>div" + id).show();
        });
    });
</script>
