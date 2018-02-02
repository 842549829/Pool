<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMMSendRecords.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SmsModule.SMMSendRecords" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>平台查看用户发送的短信</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        平台查看用户发送的短信</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">接收号码：</span>
                            <asp:TextBox runat="server" CssClass="text" ID="txtPhone"></asp:TextBox></div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">操作账号：</span>
                            <asp:TextBox runat="server" CssClass="text" ID="txtAccountNo"></asp:TextBox></div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">发送状态：</span>
                            <asp:DropDownList ID="drpStatus" runat="server">
                                <asp:ListItem>所有</asp:ListItem>
                                <asp:ListItem>已提交</asp:ListItem>
                                <asp:ListItem>发送失败</asp:ListItem>
                                <asp:ListItem>已发送</asp:ListItem>
                                <asp:ListItem>部分发送失败</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div class="input">
                            <span class="name">发送时间：</span>
                            <asp:TextBox runat="server" CssClass="text text-s1" ID="txtSendStartTime" onfocus="WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtSendEndTime\')||\'%y-%M-%d\'}', readOnly:true })"></asp:TextBox>
                            至
                            <asp:TextBox runat="server" CssClass="text text-s1" ID="txtSendEndTime" onfocus="WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtSendStartTime\')}', maxDate: '#F{\'%y-%M-%d\'}', readOnly:true})"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btns" colspan="3">
                        <asp:Button runat="server" CssClass="btn class1" Text="查询" ID="btnSendQuery" OnClick="btnSendQuery_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="data-list" class="table">
        <asp:GridView ID="grv_sendRecord" runat="server" CssClass="nfo-table info" Width="100%"
            AutoGenerateColumns="false" EnableViewState="false">
            <Columns>
                <asp:TemplateField HeaderText="接收号码">
                    <ItemStyle CssClass="w10" />
                    <ItemTemplate>
                        <%#Eval("Mobiles")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="短信内容" DataField="Content" ItemStyle-Width="500" />
                <asp:BoundField HeaderText="发送时间" DataField="SendTime" ItemStyle-CssClass="w10" />
                <asp:BoundField HeaderText="发送状态" DataField="Status" ItemStyle-CssClass="w10" />
                <asp:TemplateField HeaderText="失败号码">
                    <ItemStyle CssClass="w10" />
                    <ItemTemplate>
                        <%#Eval("FiadPhone")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="操作员" DataField="OperatorAccount" ItemStyle-CssClass="w10" />
            </Columns>
        </asp:GridView>
        <br />
        <div class="btns">
            <uc:Pager runat="server" ID="send_pager" Visible="false"></uc:Pager>
        </div>
        <div class="box" id="showempty" visible="false" runat="server">
            没有任何符合条件的查询结果</div>
    </div>
    </form>
</body>
</html>
<script src="/Scripts/Global.js" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
