<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProcessPostpone.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Operate.ProcessPostpone" %>

<%@ Register Src="../UserControls/Passenger.ascx" TagName="Passenger" TagPrefix="uc" %>
<%@ Register Src="../UserControls/Voyage.ascx" TagName="Voyage" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
 </head>
   <style type="text/css">
        select
        {
            padding-left: 0;
        }
    </style>
<body>
    <div runat="server" id="divError" class="column hd" visible="false">
    </div>
    <form id="form1" runat="server">
    <div class="column">
        <div class="hd form">
            <h3 class="titleBg">
                申请单详情</h3>
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
                        <asp:Label runat="server" ID="lblApplyformId" CssClass="obvious"></asp:Label>
                    </td>
                    <td class="title">
                        申请类型：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblApplyType" CssClass="obvious"></asp:Label>
                    </td>
                    <td class="title">
                        状态：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblStatus" CssClass="obvious"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        订单编号：
                    </td>
                    <td>
                        <a runat="server" id="linkOrderId" class="obvious-a"></a>
                    </td>
                    <td class="title">
                        产品类型：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblProductType" CssClass="obvious"></asp:Label>
                    </td>
                    <td class="title">
                        客票类型：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblTicketType" CssClass="obvious"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        编码：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblPNR" CssClass="obvious"></asp:Label>
                    </td>
                    <td class="title">
                        申请时间：
                    </td>
                    <td>
                        <asp:Label Text="" ID="lblAppliedTime1" runat="server" CssClass="obvious" />
                    </td>
                    <td class="title">
                        处理时间：
                    </td>
                    <td>
                        <asp:Label Text="" ID="lblProcessTime" runat="server" CssClass="obvious" />
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        申请原因：
                    </td>
                    <td>
                        <asp:Label Text="" ID="lblAppliedReason1" runat="server" CssClass="obvious" />
                    </td>
                    <td class="title">
                        改期费：
                    </td>
                    <td colspan="3">
                        <asp:Label Text="" ID="lblFee" runat="server" CssClass="obvious" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <uc:Voyage ID="originalVoyages" runat="server" Tip="原航班信息" />
    <uc:Passenger ID="originalPassengers" runat="server" Tip="原机票信息" />
    <uc:Voyage ID="newVoyages" runat="server" Tip="改期航班信息" />
    <div class="column">
        <h3 class="titleBg">
            改期申请信息</h3>
        <div class="table">
            <table>
                <tr>
                    <th>
                        乘客
                    </th>
                    <th>
                        乘客类型
                    </th>
                    <th>
                        证件号
                    </th>
                </tr>
                <asp:Repeater runat="server" ID="applyInfos">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("Name")%>
                            </td>
                            <td>
                                <%#Eval("Type")%>
                            </td>
                            <td>
                                <%#Eval("Credentials")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <%--政策备注--%>
    <div runat="server" id="divPolicyRemark" visible="false" class="column table">
        <h3 class="titleBg">
            政策备注</h3>
        <div runat="server" id="divPolicyRemarkContent">
        </div>
    </div>
    <div class="column">
        <h3 class="titleBg">
            申请信息</h3>
        <div class="table">
            <table>
                <tr>
                    <th>
                        提交时间
                    </th>
                    <th>
                        原因
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblAppliedTime"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblAppliedReason"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="btns">
        <input type="button" class="btn class1" id="btnAgreeWithoutFeeInfo" value="同意改期"
            onclick="$('#agreeWithoutFeeInfo').click();" />
        <input type="button" class="btn class1" id="btnAgreeWithFeeInfo" runat="server" value="收取改期费"
            onclick="$('#agreeWithFeeInfo').click();" />
        <input type="button" class="btn class1" id="btnDenyInfo" value="拒绝改期" onclick="$('#denyPostponeInfo').click();" />
        <asp:Button class="btn class2" runat="server" ID="btnReleaseLockAndBack" Text="解锁并返回"
            OnClick="btnReleaseLockAndBack_Click" />
        <button class="btn class2" runat="server" id="btnBack">
            返&nbsp;&nbsp;&nbsp;回</button>
    </div>
    <a id="agreeWithoutFeeInfo" style="display: none" data="{type:'pop',id:'divAgreeWithoutFee'}">
    </a>
    <div class="column layer" id="divAgreeWithoutFee" style="display: none; width: 680px;">
        <div class="tips-info tips-info-a">
            <div class="hd">
                <h3>
                    同意改期</h3>
            </div>
            <div class="con">
                <div class="form">
                    <asp:Repeater runat="server" ID="agreeWithoutFeeContent" EnableViewState="False">
                        <HeaderTemplate>
                            <table>
                                <thead>
                                    <tr>
                                        <th>
                                            航空公司
                                        </th>
                                        <th>
                                            航班号
                                        </th>
                                        <th>
                                            航段
                                        </th>
                                        <th>
                                            舱位/折扣
                                        </th>
                                        <th>
                                            航班日期
                                        </th>
                                        <th>
                                            起飞时间
                                        </th>
                                        <th>
                                            降落时间
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("AirlineName") %>
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("AirlineCode") %></span><input type="text" class="text text-s flightNo"
                                            value='<%# Eval("FlightNo") %>' />
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("DepartureName") %>
                                        -
                                        <%# Eval("ArrivalName") %></span>
                                    <input type="hidden" value='<%# Eval("DepartureCode") %>' class="departureCode" />
                                    <input type="hidden" value='<%# Eval("ArrivalCode") %>' class="arrivalCode" />
                                </td>
                                <td>
                                    <%# Eval("Bunk") %>
                                    /
                                    <%# Eval("Discount") %>
                                </td>
                                <td>
                                    <input type="text" class="text text-s flightDate" value='<%# Eval("FlightDate") %>'
                                        onfocus="WdatePicker({isShowClear:false,readOnly:true,minDate:'%y-%M-%d'})" />
                                </td>
                                <td>
                                    <select class="hour takeoffHour">
                                        <option>00</option>
                                        <option>01</option>
                                        <option>02</option>
                                        <option>03</option>
                                        <option>04</option>
                                        <option>05</option>
                                        <option>06</option>
                                        <option>07</option>
                                        <option>08</option>
                                        <option>09</option>
                                        <option>10</option>
                                        <option>11</option>
                                        <option>12</option>
                                        <option>13</option>
                                        <option>14</option>
                                        <option>15</option>
                                        <option>16</option>
                                        <option>17</option>
                                        <option>18</option>
                                        <option>19</option>
                                        <option>20</option>
                                        <option>21</option>
                                        <option>22</option>
                                        <option>23</option>
                                    </select>
                                    :
                                    <select class="minitue takeoffMinitue">
                                        <option>00</option>
                                        <option>05</option>
                                        <option>10</option>
                                        <option>15</option>
                                        <option>20</option>
                                        <option>25</option>
                                        <option>30</option>
                                        <option>35</option>
                                        <option>40</option>
                                        <option>45</option>
                                        <option>50</option>
                                        <option>55</option>
                                    </select>
                                </td>
                                <td>
                                    <select class="hour landingHour">
                                        <option>00</option>
                                        <option>01</option>
                                        <option>02</option>
                                        <option>03</option>
                                        <option>04</option>
                                        <option>05</option>
                                        <option>06</option>
                                        <option>07</option>
                                        <option>08</option>
                                        <option>09</option>
                                        <option>10</option>
                                        <option>11</option>
                                        <option>12</option>
                                        <option>13</option>
                                        <option>14</option>
                                        <option>15</option>
                                        <option>16</option>
                                        <option>17</option>
                                        <option>18</option>
                                        <option>19</option>
                                        <option>20</option>
                                        <option>21</option>
                                        <option>22</option>
                                        <option>23</option>
                                    </select>
                                    :
                                    <select class="minitue landingMinitue">
                                        <option>00</option>
                                        <option>05</option>
                                        <option>10</option>
                                        <option>15</option>
                                        <option>20</option>
                                        <option>25</option>
                                        <option>30</option>
                                        <option>35</option>
                                        <option>40</option>
                                        <option>45</option>
                                        <option>50</option>
                                        <option>55</option>
                                    </select>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody></table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <div class="form" runat="server" id="divNewPNR">
                    <div class="hd">
                        <h2>
                            新编码信息</h2>
                    </div>
                    <span>小编码</span><input type="text" class="text text-s" id="txtPNRCode" />
                    <span>大编码</span><input type="text" class="text text-s" id="txtBPNRCode" />
                </div>
                <div class="btns">
                    <input type="button" class="btn class1" value="确&nbsp;&nbsp;&nbsp;定" onclick="agreePostponeWithoutFee();" />
                    <input type="button" class="btn class2 close" value="返&nbsp;&nbsp;&nbsp;回" />
                </div>
            </div>
        </div>
    </div>
    <a id="agreeWithFeeInfo" style="display: none" data="{type:'pop',id:'divAgreeWithFee'}">
    </a>
    <div class="column layer" id="divAgreeWithFee" style="display: none;">
        <div class="tips-info tips-info-a">
            <div class="hd">
                <h3>
                    收取改期费</h3>
            </div>
            <div class="con">
                <div class="form">
                    <asp:Repeater runat="server" ID="agreeWithFeeContent" EnableViewState="False">
                        <HeaderTemplate>
                            <table>
                                <thead>
                                    <tr>
                                        <th>
                                            航段
                                        </th>
                                        <th>
                                            改期费
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <span>
                                        <%# Eval("DepartureName") %>
                                        -
                                        <%# Eval("ArrivalName") %></span>
                                    <input type="hidden" value='<%# Eval("DepartureCode") %>' class="departureCode" />
                                    <input type="hidden" value='<%# Eval("ArrivalCode") %>' class="arrivalCode" />
                                </td>
                                <td>
                                    <input type='text' class='text fee' />/人
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody></table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <div class="btns">
                    <input type="button" class="btn class1" value="确&nbsp;&nbsp;&nbsp;定" onclick="agreePostponeWithFee();" />
                    <input type="button" class="btn class2 close" value="返&nbsp;&nbsp;&nbsp;回" />
                </div>
            </div>
        </div>
    </div>
    <a id="denyPostponeInfo" style="display: none" data="{type:'pop',id:'divDenyPostpone'}">
    </a>
    <div class="column layer" id="divDenyPostpone" style="display: none">
        <div class="tips-info tips-info-a">
            <div class="hd">
                <h3>
                    拒绝改期</h3>
            </div>
            <div class="con">
                <div>
                    <span class="name">拒绝原因：</span>
                    <textarea rows="5" cols="50" id="txtDenyReason" class="text"></textarea>
                </div>
                <div class="btns">
                    <input type="button" class="btn class1" onclick="denyPostpone();" value="提&nbsp;&nbsp;&nbsp;交" />
                    <input type="button" class="btn class2 close" value="返&nbsp;&nbsp;&nbsp;回" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hidReturnUrl" />
    <asp:HiddenField runat="server" ID="hidRequireNewPNR" />
    </form>
</body>
</html>
<script src="../../Scripts/json2.js" type="text/javascript"></script>
<script src="../../Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/OrderModule/processPostponeApplyform.js?20130109" type="text/javascript"></script>
