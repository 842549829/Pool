<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSBuyRecord.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SmsModule.SMSBuyRecord" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>短信套餐购买记录</title>
    <link href="/Styles/icon/fontello.css" rel="stylesheet" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        短信套餐购买记录</h3>
    <div class="box-a">
        <div class="condition clearfix" style="line-height: 40px;">
            <span class="input fl">时间：<asp:TextBox runat="server" ID="txtStartTime" onclick="WdatePicker({isShowClear:false,  readOnly:true, maxDate: '2020-10-01' })"
                CssClass="text text-s"></asp:TextBox>至<asp:TextBox runat="server" ID="txtEndTime"
                    onclick="WdatePicker({isShowClear:false,  readOnly:true, maxDate: '2020-10-01' })"
                    CssClass="text text-s "></asp:TextBox></span> <span class="input fl">账号：<asp:TextBox
                        runat="server" ID="txtAccountNo" CssClass="text text-s"></asp:TextBox></span>
            <span class="input fl">联系人名：<asp:TextBox runat="server" ID="txtCompany" CssClass="text text-s"></asp:TextBox></span>
            <span class="input fl">套餐金额：<asp:DropDownList runat="server" ID="ddlPackback">
            </asp:DropDownList>
            </span><span class="input fl">
                <asp:Button runat="server" ID="btnQuery" CssClass="btn class1" Text="查询" OnClick="btnQuery_Click" />
                <input type="button" class="btn class2" onclick="javascript:window.location='SMSPackageManage.aspx';" value="返回套餐列表" />
            </span>
        </div>
    </div>
    <div id="data-list" class="table">
        <asp:GridView ID="grv_Record" runat="server" CssClass="nfo-table info" Width="100%"
            AutoGenerateColumns="false" EnableViewState="false">
            <Columns>
                <asp:BoundField HeaderText="购买账号" DataField="PurchaseAccount" />
                <asp:BoundField HeaderText="联系人名" DataField="CompanyUserName" />
                <asp:BoundField HeaderText="购买金额" DataField="Amount" />
                <asp:BoundField HeaderText="购买数量" DataField="Count" />
                <asp:BoundField HeaderText="购买时间" DataField="PayTime" />
                <asp:BoundField HeaderText="支付渠道" DataField="PayType" />
            </Columns>
        </asp:GridView>
        <br />
        <div class="btns">
            <uc:Pager runat="server" ID="pager" Visible="false">
            </uc:Pager>
        </div>
        <div class="box" id="showempty" visible="false" runat="server">
            没有任何符合条件的查询结果</div>
    </div>
    </form>
</body>
</html>
<script src="/Scripts/Global.js" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
