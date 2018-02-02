<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="FlightQuery.ascx.cs" Inherits="ChinaPay.B3B.TransactionWeb.FlightReserveModule.FlightQuery" %>
<div class="condition">
    <div id="hotCity" class="tipControl" style="display: none;">
        <span>热门城市(支持汉字/拼音/三字码)</span><img alt="关闭" src="/Images/ico_close.gif" width="11" height="11" />
        <div class="hotCityNav"><span class="active">热门</span><span>A-G</span><span>H-L</span><span>M-T</span><span>W-Z</span></div>
        <ul class="cityList"></ul>
        <ul class="cityList"></ul>
        <ul class="cityList"></ul>
        <ul class="cityList"></ul>
        <ul class="cityList"></ul>
    </div>
    <div id="divAirlineSelector" class="airlineSelector tipControl" style="display: none;">
        <span style="color: #F60">请选择航空公司</span><img alt="关闭" src="/Images/ico_close.gif" width="11" height="11" />
        <ul class="clearfix"></ul>
    </div>
    <table>
        <tr>
            <td>
                <input type="radio" id="radOneWay" name="vayageType" value="0" checked="checked"/><label for="radOneWay">单程</label>
                <input type="radio" id="radRoundWay" name="vayageType" value="1" /><label for="radRoundWay">往返</label>
            </td>
            <td>
                <span class="name">航空公司：</span>
                <input type="hidden" id="txtAirlineValue"/>
                <input type="text" class="text text-s" id="txtAirline" value="--全部--" readonly="readonly" onfocus="showAirlines($('#txtAirlineValue'), $('#divAirlineSelector'), $(this));" />
            </td>
            <td>
                <span class="name">出发城市：</span>
                <input type="hidden" id="txtDepartureValue"/>
                <input type="text" id="txtDeparture" class="text text-s" autocomplete="false" onblur="airportControlBlured($('#txtDepartureValue'),$('#hotCity'),$(this));"
                    onkeyup="showCitis($('#txtDepartureValue'),$('#hotCity'),$(this),event);" 
                    onfocus="showCitis($('#txtDepartureValue'),$('#hotCity'),$(this),event);$(this).select();"/>
                <img alt="选择出发城市" title="选择出发城市" src="/Images/btn_inputSlct.gif" class="btn_inputSlct" onclick="showCitiesSelectWindow($('#txtDepartureValue'),$('#hotCity'),$('#txtDeparture'));" width="16" height="16" />
            </td>
            <td>
                <span class="name">到达城市：</span>
                <input type="hidden" id="txtArrivalValue"/>
                <input type="text" id="txtArrival" class="text text-s" autocomplete="false" onblur="airportControlBlured($('#txtArrivalValue'),$('#hotCity'),$(this));"
                    onkeyup="showCitis($('#txtArrivalValue'),$('#hotCity'),$(this),event);" 
                    onfocus="showCitis($('#txtArrivalValue'),$('#hotCity'),$(this),event);$(this).select();"/>
                <img alt="选择到达城市" title="选择到达城市" src="/Images/btn_inputSlct.gif" class="btn_inputSlct" onclick="showCitiesSelectWindow($('#txtArrivalValue'),$('#hotCity'),$('#txtArrival'));" width="16" height="16" />
            </td>
            <td>
                <span class="name">出发日期：</span>
                <input type="text" id="txtGoDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true,minDate:'%y-%M-%d'});"/>
            </td>
            <td>
                <span class="name">返程日期：</span>
                <input type="text" id="txtBackDate" class="text text-s" disabled="disabled"/>
            </td>
            <td>
                <button class="btn class1" id="btnQueryFlight">重新查询</button>
            </td>
        </tr>
    </table>
</div>