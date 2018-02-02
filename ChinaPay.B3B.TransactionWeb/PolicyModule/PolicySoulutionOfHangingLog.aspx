<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PolicySoulutionOfHangingLog.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.PolicySoulutionOfHangingLog" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>查看日志</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />

<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        挂起/解挂 查看日志</h3>
    <div class="box-a">
        <div class="condition">
            操作时间：
            <asp:TextBox ID="txtStartTime" runat="server" CssClass="text text-s" onclick="WdatePicker({isShowClear:false, readOnly:true, maxDate: '#F{$dp.$D(\'txtEndTime\')||\'2020-10-01\'}' })"></asp:TextBox>
            <span class="fl-l">至</span>
            <asp:TextBox ID="txtEndTime" runat="server" CssClass="text text-s" onclick="WdatePicker({isShowClear:false, readOnly:true, minDate: '#F{$dp.$D(\'txtStartTime\')}', maxDate: '2020-10-01' })"></asp:TextBox>
            <asp:Button ID="btnQuery" runat="server" Text="查询" CssClass="btn class1" OnClick="btnQuery_Click" />
        </div>
    </div>
    <div class="table" id="data-list">
        <asp:GridView ID="grvlog" runat="server" AutoGenerateColumns="false" EnableViewState="False">
            <Columns>
                <asp:BoundField HeaderText="操作时间" DataField="OptionTime" />
                <asp:TemplateField HeaderText="操作人">
                    <ItemTemplate><%#Eval("Option")%></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="操作人类型" DataField="OptionOwnerType" />
                <asp:BoundField HeaderText="挂起/解挂类型" DataField="OptionType" />
                <asp:BoundField HeaderText="IP地址" DataField="IP" />
                <asp:TemplateField HeaderText="操作内容">
                    <ItemTemplate>
                        <span class="OptionContent">
                            <%#Eval("OptionContent")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="原因">
                    <ItemTemplate>
                        <span class="OptionReason">
                            <%#Eval("OptionReason")%></span>
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
</body>
</html>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/PolicyModule/polciy_public.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $(".OptionContent,.OptionReason").tipTip({ limitLength: 10, maxWidth: "300px" });
    })
</script>
