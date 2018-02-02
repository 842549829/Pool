<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PNRInfo.ascx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.UserControls.PNRInfo" %>
<%@ Register Src="~/OrderModule/UserControls/Voyage.ascx" TagName="Voyage" TagPrefix="uc" %>
<%@ Register Src="~/OrderModule/UserControls/Passenger.ascx" TagName="Passenger" TagPrefix="uc" %>
<div class="column">
    <div class="hover-tips divPNRCodeInfo" id="divPNRCodeInfo" runat="server">
        <h2>编码：<strong class="obvious"><asp:Label runat="server" ID="lblPNRCode" CssClass="pnrCode"></asp:Label></strong>
        <strong class="obvious"><asp:Label runat="server" ID="lblBPNRCode" CssClass="bPNRCode"></asp:Label></strong>
        <strong class="obvious">
            <asp:Label text="" ID="lblReservePNR" runat="server" /></strong>
            <strong class="obvious">
                <asp:Label text="" ID="lblAdultPNR" runat="server" />
            </strong>
        </h2>
    </div>
    <uc:Voyage runat="server" ID="voyage"/>
    <uc:Passenger runat="server" ID="passenger"/>
    <div runat="server" id="divOperation" class="btns">
        <input type="button" class="btn class1" onclick="ApplyRefund(this);" value="申请退/废票"/>
        <input type="button" runat="server" id="btnApplyPostpone" class="btn class1" onclick="ApplyPostpone(this);" value="申请改期"/>
        <input type="button" runat="server" id="btnApplyUpgrade" class="btn class1" onclick="ApplyUpgrade(this);" value="申请升舱" />
    </div>
</div>