<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSBindRecord.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SmsModule.SMSBindRecord" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户绑定记录</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        绑定账号记录</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">国付通账号：</span><asp:TextBox runat="server" CssClass="text" ID="txtPoolpay"></asp:TextBox></div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">平台账号：</span><asp:TextBox runat="server" CssClass="text" ID="txtB3B"></asp:TextBox></div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">绑定时间：</span>
                            <asp:TextBox runat="server" CssClass="text text-s1" ID="txtStartTime" onfocus="WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtEndTime\')||\'%y-%M-%d\'}', readOnly:true })"></asp:TextBox>
                            至<asp:TextBox runat="server" CssClass="text text-s1" ID="txtEndTime" onfocus="WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtStartTime\')}', maxDate: '#F{\'%y-%M-%d\'}', readOnly:true})"></asp:TextBox></div>
                    </td>
                </tr>
                <tr>
                    <td class="btns" colspan="3">
                        <asp:Button runat="server" CssClass="btn class1" Text="查询" ID="btnBindQuery" OnClick="btnBindQuery_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="data-list" class="table">
        <asp:GridView ID="grv_Record" runat="server" CssClass="nfo-table info" Width="100%"
            AutoGenerateColumns="false" EnableViewState="false">
            <Columns>
                <asp:BoundField HeaderText="绑定时间" DataField="OperationTime" />
                <asp:BoundField HeaderText="国付通帐号" DataField="AccountNo" />
                <asp:BoundField HeaderText="帐号类型" DataField="AccountType" />
                <asp:TemplateField HeaderText="平台帐号">
                    <ItemTemplate>
                        <a href='/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=<%#Eval("CompanyId") %>'>
                            <%#Eval("CompanyNo")%>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="平台帐号类型" DataField="CompanyType" />
            </Columns>
        </asp:GridView>
        <br />
        <div class="btns">
            <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
        </div>
        <div class="box" id="showempty" visible="false" runat="server">
            没有任何符合条件的查询结果</div>
    </div>
    </form>
</body>
</html>
<script src="/Scripts/Global.js" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
