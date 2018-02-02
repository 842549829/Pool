<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PolicySolutionOfHangingManage.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.PolicySolutionOfHangingManage" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/CompanyC.ascx" TagName="Company" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>政策的挂起/解挂</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
    <h3 class="titleBg">
        平台政策的挂起/解挂</h3>
    <div class="box-a">
        <div class="condition">
            <table class="w50">
                <tr>
                    <td> 出票方/产品方： </td>
                    <td>  <uc:Company runat="server" ID="ucCompany" /> </td>
                    <td> <asp:Button ID="btnQuery" runat="server" CssClass="btn class1" Text="查询" OnClick="btnQuery_Click" /> </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="data-list" class="table">
        <asp:GridView ID="grvhang" runat="server" AutoGenerateColumns="false" EnableViewState="False">
            <Columns>
                <asp:BoundField HeaderText="公司类别" DataField="CompanyType" />
                <asp:BoundField HeaderText="公司简称" DataField="AbbreviateName" />
                <asp:BoundField HeaderText="被用户挂起的政策" DataField="SuspendByCompany" />
                <asp:BoundField HeaderText="被本平台挂起的政策" DataField="SuspendByPlatform" />
                <asp:TemplateField HeaderText="操作" ControlStyle-CssClass="w40">
                    <ItemTemplate>
                        <a href='PolicySolutionOfHanging.aspx?flag=hang&website=0&id=<%#Eval("CompanyId") %>'>
                            挂起</a>&nbsp; <a href='PolicySolutionOfHanging.aspx?flag=solution&website=0&id=<%#Eval("CompanyId") %>'>
                                解挂</a>&nbsp; <a href='PolicySoulutionOfHangingLog.aspx?id=<%#Eval("CompanyId") %>'>日志</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="btns">
            <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
        </div>
        <div class="box" id="showempty" visible="false" runat="server">
            没有任何符合条件的查询结果</div>
    </div>
    </form>
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
    <script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
</body>
</html>
