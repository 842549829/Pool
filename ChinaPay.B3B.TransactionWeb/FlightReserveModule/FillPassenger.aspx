<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FillPassenger.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.FlightReserveModule.FillPassenger" %>
<%@ Register Src="/UserControl/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="/UserControl/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<%@ Register Src="~/FlightReserveModule/Flights.ascx" TagPrefix="uc" TagName="Flights" %>
<%@ Register TagPrefix="uc1" TagName="FlightQueryNew" Src="~/FlightReserveModule/FlightQueryNew.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <link href="../Styles/flightQuery.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/ticket.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #passenger
        {
            background: #fff;
            border: 1px solid #ccc;
            margin: 0 0 0 59px;
            padding: 0px 5px 5px;
            position: absolute;
            text-align:left;
            width:150px;
            z-index: 999;
            top: 28px;
            left: 0px;
        }
        #passenger img
        {
            position:absolute;
            right:5px;
            top:5px;
            cursor:pointer;
        }
        #passenger li
        {
            border-bottom:1px solid #ccc;
        }
        #passenger li:hover
        {
            background-color: #eee;
        }
    </style>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
    <div class="wrap">
        <uc:Header runat="server" ID="ucHeader" FlightText="我的机票"></uc:Header>
        <div id="bd">
            <%--快捷查询航班--%>
            <div class="box-a">
                <uc1:FlightQueryNew ID="ucFlightQueryNew" runat="server" />
            </div>
            <%--航段信息--%>
            <div class="column" style="padding-top:25px;z-index:998;">
                <uc:Flights runat="server" ID="ucFlights"></uc:Flights>
                <%--进度条--%>
                <div class="corner">
                    <div class="step">
                        <div class="line-bg">
                        </div>
                        <div style="width: 50%;" class="active-line-bg">
                        </div>
                        <i class="active">●<span>1</span></i> <i class="active" style="left: 48%;">●<span>2</span></i>
                        <i style="left: 174px">●<span>3</span></i>
                        <label style="left: -20px;">
                            选择航班</label>
                        <label style="left: 40%">
                            补全信息</label>
                        <label style="left: 75%">
                            付款完成订单</label>
                    </div>
                </div>
            </div>
            <!-- 机票信息 -->
            <div class="column" style="z-index:997;">
                <div class="hd">
                    <h2 class="ticketInfo">
                        机票信息</h2>
                    <p class="flightPrompt minor">
                        温馨提示:此处价格仅供参考，以订单支付页面价格为准!</p>
                </div>
                <div runat="server" id="divTickets" class="table ticketInfoBox">
                </div>
            </div>
            <!-- 乘机人信息 -->
            <div class="column">
                <div class="hd">
                    <h2 class="passengersInfo">
                        乘机人信息</h2>
                </div>
                <asp:HiddenField runat="server" ID="hidSeatCount" />
                <div class="table passengersInfoBox" id="divPassengers">
                    <table id="memberTable">
                        <tr>
                            <th colspan="7">
                                <span class="fl">乘客类型：<asp:DropDownList runat="server" ID="ddlPassengerType"></asp:DropDownList></span>
                                <span class="fl" id="adultPNRInfo" style="margin-left: 20px; display: none">成人PNR编码：<input
                                    type="text" class="text" id="txtAdultPNRCode" /></span>
                                <%--<span class="fl">乘机人数：<select style="width:50px;">
                                            <option value="1">1</option>
                                            <option value="2">2</option>
                                            <option value="3">3</option>
                                            <option value="4">4</option>
                                            <option value="5">5</option>
                                            <option value="6">6</option>
                                            <option value="7">7</option>
                                            <option value="8">8</option>
                                            <option value="9">9</option>
                                            </select></span>--%>
                            <%--<div class="fr">
                                <a href="javascript:void(0)" class="btnAddCustomer1" title="添加乘机人" id="btnNewPassenger">添加乘机人</a>
                            </div>--%>
                                <span class="fr"><input type="button" class="btn class1" id="btnNewPassenger"  title="添加乘机人" value="添加乘机人"/></span>
                            </th>
                        </tr>
                        <tr class="memberItem">
                            <td>
                                <div style="position:relative;">
                                姓名：<input type="text" class="text passengerName" style="width: 100px"  />
                                    <div id="passenger" class="tipControl" style="display: none;">
                                        <span>乘机人(支持汉字)</span><img alt="关闭" src="/Images/ico_close.gif" width="11"
                                            height="11" />
                                        <ul class="passenger">
                                        </ul>
                                    </div>
                                <span class="birthDay" style="display:none;"><br />
                                  
                                </span>
                                </div>
                            </td>
                            <td>
                                <a href="javascript:void(0)" class="btnSelectCustomer" title="选择常旅客">选择常旅客</a>
                            </td>
                            <td>
                                证件类型：<select class="credentialsType" style="width: 80px"></select>
                                证件号：<input type="text" class="text credentials" />
                            </td>
                            <td style="display: none;" class="birthDay">
                                出生日期：<input type= "text" class="text txtBirthDay" style="width:70px"/>
                            </td>
                            <td>
                                手机号：<input type="text" class="text passengerMobile" style="width: 90px" />
                            </td>
                            <%--<td>
                                <div>
                                    <a href="javascript:void(0)" class="btnAddCustomer" title="添加乘机人" id="btnNewPassenger">添加乘机人</a>
                                </div>
                            </td>--%>
                            <td>
                                <a href="javascript:void(0)" class="btnSeverCustomer1" title="保存常旅客" id="A1">存</a>
                                <a href="javascript:void(0)" style="margin-left:8px;" class="btnRemoverCustomer1" title="删除该旅客" id="A2">删</a>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <!-- 订单联系人信息 -->
            <div class="column" style="z-index: -1; position: relative;">
                <div class="hd">
                    <h2 class="tipsInfo">
                        订单联系人</h2>
                    <p class="flightPrompt minor">
                        温馨提示:该联系人为您注册时所填写的联系人，用于航班变动等通知。
                    </p>
                </div>
                <div class="tipsInfoBox">
                    <div class="col-3">
                        <div class="col">
                            <span>联系人:</span><asp:TextBox runat="server" ID="txtContact" CssClass="text"></asp:TextBox></div>
                        <div class="col">
                            <span>手机号码：</span><asp:TextBox runat="server" ID="txtMobile" CssClass="text"></asp:TextBox></div>
                        <div class="col">
                            <span>邮箱：</span><asp:TextBox runat="server" ID="txtEmail" CssClass="text"></asp:TextBox></div>
                    </div>
                </div>
            </div>
            <div class="btns btn-box">
                <input type="button" id="btnNext" class="btn class1" value="下一步" />
                <input type="button" id="btnWaiting" class="btn class2" disabled="disabled" style="display: none;"
                    value="执行中..." />
            </div>
            
                    <%--加载中图片--%>
            <a id="LoadingPop" data="{type:'pop',id:'Loading'}" style="display:none;"></a>
            <div id="Loading" style="display:none"><img src="/Images/LoadingBar.gif"><span class="close" style="display: none">关闭</span></div>

            <%-- 选择常旅客 --%>
            <a id="lnkCustomers" data="{type:'pop',id:'divCustomers'}" style="display: none">
            </a>
            <div id="divCustomers" class="column layer2" style="display: none; width: 825px;
                background-color: White;">
                <div class="hd">
                    <h4>
                        选择常旅客<a href="javascript:void(0)" class="close">关闭</a></h4>
                </div>
                <div class="box-a">
                    <div class="condition">
                        <table>
                            <colgroup>
                                <col class="w30" />
                                <col class="w35" />
                                <col class="w35" />
                            </colgroup>
                            <tr>
                                <td>
                                    <div class="input">
                                        <span class="name">姓名：</span>
                                        <input type="text" id="txtConditionName" class="text" />
                                    </div>
                                </td>
                                <td>
                                    <div class="input">
                                        <span class="name">证件号：</span>
                                        <input type="text" id="txtConditionCredentials" class="text" />
                                    </div>
                                </td>
                                <td>
                                    <div class="input">
                                        <span class="name">手机：</span>
                                        <input type="text" id="txtConditionMobile" class="text" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="btns" colspan="3">
                                    <input type="button" class="btn class1" value="查&nbsp;&nbsp;&nbsp;&nbsp;询" id="btnQueryCustomers" />
                                    <input type="button" class="btn class2 close" value="返&nbsp;&nbsp;&nbsp;&nbsp;回" id="btnCustomerBack" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="table customerTb" id="divCustomersContent">
                </div>
                <div class="btns" id="divPagination">
                </div>
            </div>
            <uc:Footer runat="server" ID="ucFooter"></uc:Footer>
        </div>
    </div>
    

    </form>
</body>
</html>
<script src="../Scripts/json2.js" type="text/javascript"></script>
<script src="../Scripts/widget/common.js?201301002" type="text/javascript"></script>
<script src="../Scripts/DateExtandJSCount-min.js?2013012102" type="text/javascript"></script>
<script src="../Scripts/FlightModule/fillPassenger.js?20130221" type="text/javascript"></script>
<script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="../Scripts/FlightModule/queryControl.js" type="text/javascript"></script>

