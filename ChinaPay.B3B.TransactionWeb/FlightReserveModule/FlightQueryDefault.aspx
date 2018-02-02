<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlightQueryDefault.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.FlightReserveModule.FlightQueryDefault" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript">
        var flightUrl = "/OrderModule/Purchase/Default.aspx";
        if (parent != null)
        {
            parent.location.href = flightUrl;
        }
        else location.href = flightUrl;
    </script>
    <title></title>
    <link href="/Styles/core.css?20121205" rel="stylesheet" type="text/css" />
    <link href="/Styles/form.css?20121118" rel="stylesheet" type="text/css" />
    <link href="/Styles/public.css?20121118" rel="stylesheet" type="text/css" />
    <link href="/Styles/skin.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/page.css?20121118" rel="stylesheet" type="text/css" />
    <link href="/Styles/flightQuery.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div runat="server" id="divQueryFlight" class="tab index-tab">
        <div class="s-idtabs tab-menu">
            <a href="#tabs_yd" class="btn class1 active">航班查询/预订<i class="arrow"></i></a> <a
                href="#tabs_cj">PNR创建订单<i class="arrow"></i></a>
        </div>
        <div class="tab-con" id="middle" style="width: auto;">
            <div id="tabs_yd" class="tab-item col-2 scheduled" style="display: block; border: none;">
                <ul>
                    <li style="text-align: center">
                        <input type="radio" value="0" name="vayageType" checked="checked" id="radOneWay" /><label
                            for="radOneWay">单程</label>
                        <input type="radio" value="1" name="vayageType" id="radRoundWay" /><label for="radRoundWay">返程</label>
                    </li>
                    <li><span>航空公司：</span>
                        <input type="hidden" id="txtAirlineValue" />
                        <input type="text" class="text" id="txtAirline" value="--全部--" tabindex="4" readonly="readonly"
                            onfocus="showAirlines($('#txtAirlineValue'), $('#divAirlineSelector'), $(this));" />
                    </li>
                    <li><span>出发城市：</span>
                        <input type="hidden" id="txtDepartureValue" runat="server" />
                        <input type="text" id="txtDeparture" runat="server" class="text" tabindex="1" autocomplete="false"
                            onblur="airportControlBlured($('#txtDepartureValue'),$('#hotCity'),$(this));"
                            onkeyup="showCitis($('#txtDepartureValue'),$('#hotCity'),$(this),event);" onfocus="showCitis($('#txtDepartureValue'),$('#hotCity'),$(this),event);$(this).select();" />
                        <img alt="选择出发城市" title="选择出发城市" src="/Images/btn_inputSlct.gif" class="btn_inputSlct"
                            onclick="showCitiesSelectWindow($('#txtDepartureValue'),$('#hotCity'),$('#txtDeparture'));"
                            width="16" height="16" />
                    </li>
                    <li><span>到达城市：</span>
                        <input type="hidden" id="txtArrivalValue" runat="server" />
                        <input type="text" id="txtArrival" runat="server" class="text" tabindex="2" autocomplete="false"
                            onblur="airportControlBlured($('#txtArrivalValue'),$('#hotCity'),$(this));" onkeyup="showCitis($('#txtArrivalValue'),$('#hotCity'),$(this),event);"
                            onfocus="showCitis($('#txtArrivalValue'),$('#hotCity'),$(this),event);$(this).select();" />
                        <img alt="选择到达城市" title="选择到达城市" src="/Images/btn_inputSlct.gif" class="btn_inputSlct"
                            onclick="showCitiesSelectWindow($('#txtArrivalValue'),$('#hotCity'),$('#txtArrival'));"
                            width="16" height="16" />
                    </li>
                    <li><span>出发日期：</span>
                        <input type="text" id="txtGoDate" runat="server" class="text" tabindex="3" onfocus="WdatePicker({isShowClear:false,readOnly:true,minDate:'%y-%M-%d',doubleCalendar:true});" /></li>
                    <li><span>回程日期：</span>
                        <input type="text" id="txtBackDate" tabindex="5" class="text" disabled="disabled"
                            onfocus="WdatePicker({isShowClear:false,readOnly:true,minDate:'#F{$dp.$D(\'txtGoDate\')}',doubleCalendar:true})" /></li>
                </ul>
                <div class="btns">
                    <button class="btn class1" id="btnQueryFlight">
                        查询航班</button>
                </div>
            </div>
            <div id="tabs_cj" class="tab-item">
                <table class="table-bor">
                    <colgroup>
                        <col class="w50" />
                        <col class="w50" />
                    </colgroup>
                    <tr>
                        <th>
                            PNR记录编号创建订单
                        </th>
                        <td>
                            注意事项：
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="check">
                                <asp:RadioButton runat="server" ID="radAdultPNR" Checked="True" Text="成人编码" GroupName="radPNRType" />
                                <asp:RadioButton runat="server" ID="radChildrenPNR" Text="儿童编码" GroupName="radPNRType" />
                            </div>
                        </td>
                        <td rowspan="3">
                            <div>
                                1、重要更新提示：国航(CA)、海航(HU)、联航(KN)编码需要加OSI项<br />
                                指令：OSI 航空公司代码 CTCT您的手机号码/Pn<br />
                                例如：OSI CA CTCT138*****888/P1<br />
                                2、支持成人单程、往返程及团队PNR编码导入<br />
                                3、该PNR姓名组正确<br />
                                4、航段组正确、舱位状态正确<br />
                                5、每个乘客均有真实的SSR FOID 项输入<br />
                                6、该PNR不能包含票价组项，如FC/FN/FP<br />
                                7、儿童编码导入时，需要备注成人编码<br />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divAdultPNRContent">
                                <span>PNR编码：</span><asp:TextBox runat="server" id="txtPNRCode" class="text" /></div>
                            <div id="divChildrenPNRContent" style="display: none">
                                <span>儿童PNR编码：</span><asp:TextBox runat="server" ID="txtChildrenPNRCode" class="text" />
                                <br />
                                <br />
                                <span>成人PNR编码：</span><asp:TextBox runat="server" id="txtAdultPNRCode" class="text" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="btns">
                            <asp:Button runat="server" ID="btnPNRCodeImport" CssClass="btn class1" Text="提交PNR编码"
                                OnClick="btnPNRCodeImport_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div runat="server" id="divSpecialProduct" class="newList">
    </div>
    <div id="divAirlineSelector" class="airlineSelector tipControl" style="display: none;">
        <span style="color: #F60">请选择航空公司</span><img alt="关闭" src="/Images/ico_close.gif"
            width="11" height="11" />
        <ul class="clearfix">
        </ul>
    </div>
    <div id="hotCity" class="tipControl" style="display: none;">
        <span>热门城市(支持汉字/拼音/三字码)</span><img alt="关闭" src="/Images/ico_close.gif" width="11"
            height="11" />
        <div class="hotCityNav">
            <span class="active">热门</span><span>A-G</span><span>H-L</span><span>M-T</span><span>W-Z</span></div>
        <ul class="cityList">
        </ul>
        <ul class="cityList">
        </ul>
        <ul class="cityList">
        </ul>
        <ul class="cityList">
        </ul>
        <ul class="cityList">
        </ul>
    </div>
    </form>
</body>
</html>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="../Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/d.tabs.js" type="text/javascript"></script>
<script src="/Scripts/widget/form-ui.js" type="text/javascript"></script>
<script src="/Scripts/default.js" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/Global.js" type="text/javascript"></script>
<script src="/Scripts/FlightModule/queryControl.js" type="text/javascript"></script>
<script src="/Scripts/FlightModule/pnrImport.js" type="text/javascript"></script>
