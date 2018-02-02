<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProcessRevisePrice.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Operate.ProcessRevisePrice" %>

<%@ Reference Control="~/UserControl/OutPutImage.ascx" %>
<%@ Register Src="../UserControls/Voyage.ascx" TagName="Voyage" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/Passenger.ascx" TagName="Passenger" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="/Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/core/jquery.js" type="text/javascript"></script>
</head>
<body>
    <div runat="server" id="divError" class="column hd" visible="false">
    </div>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        修改价格</h3>
    <%--订单头部信息--%>
    <div class="form">
        <table>
            <colgroup>
                <col class="w10" />
                <col class="w20" />
                <col class="w10" />
                <col class="w20" />
                <col class="w10" />
                <col class="w20" />
            </colgroup>
            <tr>
                <td class="title">
                    申请单号：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblApplyformId" CssClass="red"></asp:Label>
                </td>
                <td class="title">
                    申请类型：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblApplyType" CssClass="red"></asp:Label>
                </td>
                <td class="title">
                    状态：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblStatus" CssClass="red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    订单编号：
                </td>
                <td>
                    <a runat="server" id="linkOrderId" class="red"></a>
                </td>
                <td class="title">
                    产品类型：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblProductType" CssClass="red"></asp:Label>
                </td>
                <td class="title">
                    客票类型：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblTicketType" CssClass="red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    编码：
                </td>
                <td colspan="5">
                    <asp:Label runat="server" ID="lblPNR" CssClass="red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <uc2:Passenger ID="passengers" runat="server" Tip="机票信息" />
    <div runat="server" id="divApplyAttachment" class="column table"></div>
    <div id="PriceInputContainer">
        <p style="background-color: #e6e6e6; height: 30px; padding: 0 15px;">
            修改航段价格：<span class="flagType" runat="server" id="flightType">联程</span></p>
        <div class="table">
            <asp:Repeater runat="server" ID="ListVoyage">
                <HeaderTemplate>
                    <table>
                        <colgroup>
                            <col class="w10">
                            <col class="w30">
                            <col class="w30">
                            <col class="w30">
                        </colgroup>
                        <tbody>
                            <tr>
                                <th>
                                    航段顺序
                                </th>
                                <th>
                                    航程
                                </th>
                                <th>
                                    原票面价
                                </th>
                                <th>
                                    新票面价
                                </th>
                                <th>
                                    民航基金
                                </th>
                            </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            第<%#GetIndex((int)Eval("Serial")) %>程
                        </td>
                        <td>
                            <%#Eval("Departure.Name")%>-<%#Eval("Arrival.Name")%>
                            <asp:HiddenField ID="VoyageStart" runat="server" Value='<%#Eval("Departure.Code") %>' />
                            <asp:HiddenField runat="server" ID="VoyageEnd" Value='<%#Eval("Arrival.Code") %>' />
                        </td>
                        <td>
                            <%#Eval("Price.Fare") %>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="ReleasePrice" CssClass="text text-s releasePrice"
                                Text='<%#Eval("Price.Fare") %>' />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="AirPortFee" CssClass="text text-s airportFee" Text='<%#Eval("Price.AirportFee") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td class="total" colspan="3">
                        </td>
                        <td class="total">
                            合计：<span id="totalReleasePrice">3271.00</span>
                        </td>
                        <td class="total">
                            <span id="totalAirPortFee">200.00</span>
                        </td>
                    </tr>
                    </tbody></table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="btns">
        <asp:Button Text="确认价格修改" ID="btnSurePrice" runat="server" CssClass="btn class1"
            OnClientClick="return CheckPrice()" OnClick="SurePrice" />
        <asp:Button class="btn class1" runat="server" ID="btnReleaseLockAndBack" Text="解锁并返回"
            OnClick="btnReleaseLockAndBack_Click" />
        <button class="btn class2" runat="server" id="btnBack">
            返&nbsp;&nbsp;&nbsp;回</button>
    </div>
    <a id="aUploadAttachmentLayer" style="display: none;" data="{type:'pop',id:'divUploadAttachmentLayer'}"></a>
    <div class="form layer3 hidden" id="divUploadAttachmentLayer">
        <h4>上传附件<a href="javascript:void(0);" class="close">关闭</a></h4>
        <div class="con">
            <p class="tips mar">上传退票附件</p>
            <asp:FileUpload  CssClass="text"  runat="server" ID="fileAttachment"/><br />
             <label class="obvious1">附件类型支持jpg\png\bmp三种类型，附件小于600kb</label>
        </div>
        <div class="txt-c mar">
            <asp:Button runat="server" ID="btnConfirm" Text="确定" CssClass="btn class1" onclick="btnConfirm_Click" />
            <input type="button" id="btnCacenlRemind" value="取消" class="btn class2 close" />
        </div>
    </div>
    <div class="form layer2 hidden" id="divLayerImage">
        <h4>原图<a href="javascript:void(0);" class="close">关闭</a></h4>
        <img style="max-height:500px; max-width:500px"/>
    </div>
    <div class="fixed">
    </div>
    </form>
</body>
</html>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/Operate/ProcessRevisePrice.aspx.js" type="text/javascript"></script>
<script src="/Scripts/OrderModule/Operate/UploadApplyAttachment.js" type="text/javascript"></script>